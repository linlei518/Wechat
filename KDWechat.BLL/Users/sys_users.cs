using KDWechat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KDWechat.Common;
using System.Security.Cryptography;
using System.Linq.Expressions;

using System.Data.SqlClient;
using System.Data;
using LinqKit;
using KDWechat.DBUtility;

namespace KDWechat.BLL.Users
{
    public class sys_users
    {
        #region 外部方法
        /// <summary>
        /// 检查用户名是否可用
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>用户名是否可用</returns>
        public static bool CheckUserName(string userName)
        {
            bool isChecked = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isChecked = (from x in db.t_sys_users where x.user_name == userName select x).FirstOrDefault() == null;
            }
            return isChecked;
        }

        /// <summary>
        /// 根据用户ID获取其管理的微信号数量
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static int GetWxCountByUid(int uid)
        {
            int cou = 0;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                var query = (from x in db.t_wx_wechats where x.uid == uid select x.id);
                var wx_ids = query.ToArray().Distinct();
                cou = wx_ids.Count();
            }
            return cou;
        }

        /// <summary>
        /// 禁用用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DisableUser(int id)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                var user = db.t_sys_users.Where(x => x.id == id).FirstOrDefault();
                if (null != user)
                {
                    user.status = (int)Status.禁用;
                    isFinish = db.SaveChanges() > 0;
                }
            }
            return isFinish;
        }

        public static bool SetUserStatus(int id, Status status)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                var user = db.t_sys_users.Where(x => x.id == id).FirstOrDefault();
                if (null != user)
                {
                    user.status = (int)status;
                    isFinish = db.SaveChanges() > 0;
                }
            }
            return isFinish;
        }

        /// <summary>
        /// 提取一个用户的所有信息
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>该ID对应的用户</returns>
        public static t_sys_users GetUserByID(int id)
        {
            t_sys_users user = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                user = (from x in db.t_sys_users where x.id == id select x).FirstOrDefault();
            }
            return user;
        }
        /// <summary>
        /// 按照添加时间倒序提取用户列表
        /// </summary>
        /// <param name="pagesize">每一页容量</param>
        /// <param name="pageindex">当前页码</param>
        /// <returns>用户列表</returns>
        public static List<t_sys_users> GetUserListByIndexAndSize(int pagesize, int pageindex)
        {
            List<t_sys_users> userList = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                userList = (from x in db.t_sys_users orderby x.id descending select x).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            }
            return userList;
        }

        public static List<t_sys_users> GetUserListByIndexAndSize(int pagesize, int pageindex, Expression<Func<t_sys_users, bool>> where)
        {
            List<t_sys_users> userList = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                userList = db.t_sys_users.Where(where).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                //userList = (from x in db.t_sys_users orderby x.id descending select x)
            }
            return userList;
        }

        /// <summary>
        /// 取子账号列表
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public static List<t_sys_users> GetUserListByParentID(int parentID, int pagesize, int pageindex)
        {
            List<t_sys_users> userList = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                userList = (from x in db.t_sys_users where x.parent_id == parentID orderby x.id descending select x).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            }
            return userList;
        }

        /// <summary>
        /// 取子账号列表
        /// </summary>
        /// <param name="parentID">负责人id</param>
        /// <param name="is_not_lock">是否不包含禁用的</param>
        /// <returns></returns>
        public static List<t_sys_users> GetUserListByParentID(int parentID, bool is_not_lock=false)
        {
            List<t_sys_users> userList = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                if (is_not_lock)
                {
                    userList = (from x in db.t_sys_users where x.parent_id == parentID && x.status!=0 orderby x.id descending select x).ToList();
                }
                else
                {
                    userList = (from x in db.t_sys_users where x.parent_id == parentID orderby x.id descending select x).ToList();
                }
               
            }
            return userList;
        }
        /// <summary>
        /// 根据用户ID取微信列表
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public static List<t_wx_wechats> GetWechatsListByUID(int uid, int pagesize, int pageindex, out int count)
        {
            List<t_wx_wechats> userList = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                var query = (from x in db.t_wx_wechats where x.uid == uid orderby x.id descending select x);
                userList = query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                count = query.Count();
            }
            return userList;
        }

        /// <summary>
        /// 用户管理页面列表
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public static List<sysUser_WeChat_View> GetUserWeChatCountList(int pagesize, int pageindex)
        {

            List<sysUser_WeChat_View> userList = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {

                userList = (from x in db.sysUser_WeChat_View orderby x.create_time descending select x).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            }
            return userList;

        }


        public static List<sysUser_WeChat_View> GetUserWeChatCountList(int pagesize, int pageindex, Expression<Func<sysUser_WeChat_View, bool>> where, out int count)
        {

            List<sysUser_WeChat_View> userList = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                var query = db.sysUser_WeChat_View.Where(where.Expand());
                userList = query.OrderByDescending(x => x.id).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                count = query.Count();
            }
            return userList;

        }


        /// <summary>
        /// 建立用户
        /// </summary>
        /// <param name="parentID">上级ID</param>
        /// <param name="flag">用户类型</param>
        /// <param name="typeID">所属机构ID</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="ip">用户IP</param>
        /// <returns>添加后的用户</returns>
        public static t_sys_users CreateUser(int parentID, UserFlag flag, int typeID, string userName, string password, string ip)
        {
            t_sys_users user = null;
            if (CheckUserName(userName))
            {
                string salt = CreateSalt();
                string pwd = CreatePassword(password, salt);
                user = new t_sys_users()
                {
                    parent_id = parentID,
                    flag = (int)flag,
                    type_id = typeID,
                    user_name = userName,
                    user_pwd = pwd,
                    login_ip = ip,
                    login_time = DateTime.Now,
                    create_time = DateTime.Now,
                    create_ip = ip,
                    salt = salt,
                    status = (int)Status.正常
                };
            }
            return InsertUser(user);
        }

        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">用户密码</param>
        /// <returns>登陆成功则返回对应用户，否则返回空</returns>
        public static t_sys_users UserLogin(string username, string password)
        {
            t_sys_users user = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                user = (from x in db.t_sys_users where x.user_name == username select x).FirstOrDefault();
                if (null == user || CreatePassword(password, user.salt) != user.user_pwd)
                    user = null;
                else
                {
                    user.login_ip = Utils.GetUserIp();
                    user.login_time = DateTime.Now;
                    db.SaveChanges();
                }
            }
            return user;
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="state">用户状态</param>
        /// <param name="typeID">用户类型</param>
        /// <param name="password">用户密码</param>
        /// <param name="ip">登陆IP</param>
        /// <returns>修改后的用户</returns>
        public static bool UpdateUser(int id, int typeID = 0, string password = null, string ip = null, int area = 0, string realName = null, string nickName = null, string deptName = null, string tel = null, string email = null, string mobile = null, Status state = 0)
        {
            var user = GetUserByID(id);
            if (null != user)
            {
                if (password != null)
                    user.user_pwd = CreatePassword(password, user.salt);
                user.status = DataHelper.GetRealValue((int)state, user.status);
                user.type_id = DataHelper.GetRealValue(typeID, user.type_id);
                user.login_ip = ip ?? user.login_ip;
                user.area = DataHelper.GetRealValue(area, user.area);
                user.real_name = realName ?? user.real_name;
                user.nick_name = nickName ?? user.nick_name;
                user.dept_name = deptName ?? user.dept_name;
                user.tel = tel ?? user.tel;
                user.email = email ?? user.email;
                user.mobile = mobile ?? user.mobile;
            }
            return UpdateUsers(user);
        }
        public static t_sys_users InsertUser(t_sys_users userToInsert)
        {
            t_sys_users user = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                user = db.t_sys_users.Where(x => x.user_name == userToInsert.user_name).FirstOrDefault();
            }
            if (user != null)
                return null;
            else
                return EFHelper.AddUser<t_sys_users>(userToInsert);
        }

        /// <summary>
        /// 删除一个子帐号及其相关所有信息(可能还会增加删除表格)
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static bool DelAccountChild(int uid)
        {
            #region 处理事务
            List<CommandInfo> sqllist = new List<CommandInfo>();

            //删除用户表（1）
            StringBuilder strSql1 = new StringBuilder();
            strSql1.Append("delete from t_sys_users ");
            strSql1.Append(" where id=@id ");
            SqlParameter[] parameters1 = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters1[0].Value = uid;
            CommandInfo cmd = new CommandInfo(strSql1.ToString(), parameters1);
            sqllist.Add(cmd);

            //删除用户权限表（2）
            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append("delete from t_sys_users_power ");
            strSql2.Append(" where u_id=@u_id ");
            SqlParameter[] parameters2 = {
					new SqlParameter("@u_id", SqlDbType.Int,4)};
            parameters2[0].Value = uid;
            cmd = new CommandInfo(strSql2.ToString(), parameters2);
            sqllist.Add(cmd);

            //删除用户站内信表（3）
            StringBuilder strSql3 = new StringBuilder();
            strSql3.Append("delete from t_sys_letter_receiver ");
            strSql3.Append(" where u_id=@u_id ");
            SqlParameter[] parameters3 = {
					new SqlParameter("@u_id", SqlDbType.Int,4)};
            parameters3[0].Value = uid;
            cmd = new CommandInfo(strSql3.ToString(), parameters3);
            sqllist.Add(cmd);

            int rowsAffected = KDWechat.DBUtility.DbHelperSQL.ExecuteSqlTran(sqllist);
            #endregion 结束处理事务

            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 删除一个地区帐号及其相关所有信息(可能还会增加删除表格)
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static bool DelAccountRegion(int uid)
        {
            #region 处理事务
            List<CommandInfo> sqllist = new List<CommandInfo>();

            //删除用户表（1）
            StringBuilder strSql1 = new StringBuilder();
            strSql1.Append("delete from t_sys_users ");
            strSql1.Append(" where id=@id ");
            SqlParameter[] parameters1 = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters1[0].Value = uid;
            CommandInfo cmd = new CommandInfo(strSql1.ToString(), parameters1);
            sqllist.Add(cmd);

            //删除用户权限表（2）
            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append("delete from t_sys_users_power ");
            strSql2.Append(" where u_id=@u_id ");
            SqlParameter[] parameters2 = {
					new SqlParameter("@u_id", SqlDbType.Int,4)};
            parameters2[0].Value = uid;
            cmd = new CommandInfo(strSql2.ToString(), parameters2);
            sqllist.Add(cmd);

            //删除用户站内信表（3）
            StringBuilder strSql3 = new StringBuilder();
            strSql3.Append("delete from t_sys_letter_receiver ");
            strSql3.Append(" where u_id=@u_id ");
            SqlParameter[] parameters3 = {
					new SqlParameter("@u_id", SqlDbType.Int,4)};
            parameters3[0].Value = uid;
            cmd = new CommandInfo(strSql3.ToString(), parameters3);
            sqllist.Add(cmd);

            //删除地区账号的子帐号（4）
            StringBuilder strSql4 = new StringBuilder();
            strSql4.Append("delete from t_sys_users ");
            strSql4.Append(" where parent_id=@u_id ");
            SqlParameter[] parameters4 = {
					new SqlParameter("@u_id", SqlDbType.Int,4)};
            parameters4[0].Value = uid;
            cmd = new CommandInfo(strSql4.ToString(), parameters4);
            sqllist.Add(cmd);

            //---------------------------------------------------------------------

            List<CommandInfo> sqllist_wx = new List<CommandInfo>();
            //删除微信公众号表（1）
            StringBuilder strSql_wx1 = new StringBuilder();
            strSql_wx1.Append("delete from t_wx_wechats ");
            strSql_wx1.Append(" where u_id=@u_id ");
            SqlParameter[] parameters_wx1 = {
					new SqlParameter("@u_id", SqlDbType.Int,4)};
            parameters_wx1[0].Value = uid;
            CommandInfo cmd_wx = new CommandInfo(strSql_wx1.ToString(), parameters_wx1);
            sqllist_wx.Add(cmd);

            //删除规则表（2）
            StringBuilder strSql_wx2 = new StringBuilder();
            strSql_wx2.Append("delete from t_wx_rules ");
            strSql_wx2.Append(" where wx_id in (select id from t_wx_wechats where u_id=@u_id) ");
            SqlParameter[] parameters_wx2 = {
					new SqlParameter("@u_id", SqlDbType.Int,4)};
            parameters_wx2[0].Value = uid;
            cmd = new CommandInfo(strSql_wx2.ToString(), parameters_wx2);
            sqllist_wx.Add(cmd);

            //删除规则关键词表（3）
            StringBuilder strSql_wx3 = new StringBuilder();
            strSql_wx3.Append("delete from t_wx_rules_keywords ");
            strSql_wx3.Append(" where  wx_id in (select id from t_wx_wechats where u_id=@u_id) ");
            SqlParameter[] parameters_wx3 = {
					new SqlParameter("@u_id", SqlDbType.Int,4)};
            parameters_wx3[0].Value = uid;
            cmd = new CommandInfo(strSql_wx3.ToString(), parameters_wx3);
            sqllist_wx.Add(cmd);

            //删除关键词回复主表（4）
            StringBuilder strSql_wx4 = new StringBuilder();
            strSql_wx4.Append("delete from t_wx_rule_reply ");
            strSql_wx4.Append(" where wx_id in (select id from t_wx_wechats where u_id=@u_id) ");
            SqlParameter[] parameters_wx4 = {
					new SqlParameter("@u_id", SqlDbType.Int,4)};
            parameters_wx4[0].Value = uid;
            cmd = new CommandInfo(strSql_wx4.ToString(), parameters_wx4);
            sqllist_wx.Add(cmd);

            //删除公众号图文素材表（5）
            StringBuilder strSql_wx5 = new StringBuilder();
            strSql_wx5.Append("delete from t_wx_news_materials ");
            strSql_wx5.Append(" where  wx_id in (select id from t_wx_wechats where u_id=@u_id) ");
            SqlParameter[] parameters_wx5 = {
					new SqlParameter("@u_id", SqlDbType.Int,4)};
            parameters_wx5[0].Value = uid;
            cmd = new CommandInfo(strSql_wx5.ToString(), parameters_wx5);
            sqllist_wx.Add(cmd);

            //删除公众号媒体素材表（6）
            StringBuilder strSql_wx6 = new StringBuilder();
            strSql_wx6.Append("delete from t_wx_media_materials ");
            strSql_wx6.Append(" where wx_id in (select id from t_wx_wechats where u_id=@u_id) ");
            SqlParameter[] parameters_wx6 = {
					new SqlParameter("@u_id", SqlDbType.Int,4)};
            parameters_wx6[0].Value = uid;
            cmd = new CommandInfo(strSql_wx6.ToString(), parameters_wx6);
            sqllist_wx.Add(cmd);

            //删除公众号基本回复表（7）
            StringBuilder strSql_wx7 = new StringBuilder();
            strSql_wx7.Append("delete from t_wx_basic_reply ");
            strSql_wx7.Append(" where wx_id in (select id from t_wx_wechats where u_id=@u_id) ");
            SqlParameter[] parameters_wx7 = {
					new SqlParameter("@u_id", SqlDbType.Int,4)};
            parameters_wx7[0].Value = uid;
            cmd = new CommandInfo(strSql_wx7.ToString(), parameters_wx7);
            sqllist_wx.Add(cmd);

            //删除公众号LBS表（8）
            StringBuilder strSql_wx8 = new StringBuilder();
            strSql_wx8.Append("delete from t_wx_lbs ");
            strSql_wx8.Append(" where wx_id in (select id from t_wx_wechats where u_id=@u_id) ");
            SqlParameter[] parameters_wx8 = {
					new SqlParameter("@u_id", SqlDbType.Int,4)};
            parameters_wx8[0].Value = uid;
            cmd = new CommandInfo(strSql_wx8.ToString(), parameters_wx8);
            sqllist_wx.Add(cmd);

            //删除公众号群发信息表（9）
            StringBuilder strSql_wx9 = new StringBuilder();
            strSql_wx9.Append("delete from t_wx_group_msgs ");
            strSql_wx9.Append(" where wx_id in (select id from t_wx_wechats where u_id=@u_id) ");
            SqlParameter[] parameters_wx9 = {
					new SqlParameter("@u_id", SqlDbType.Int,4)};
            parameters_wx9[0].Value = uid;
            cmd = new CommandInfo(strSql_wx9.ToString(), parameters_wx9);
            sqllist_wx.Add(cmd);

            //删除公众号自定义菜单表（10）
            StringBuilder strSql_wx10 = new StringBuilder();
            strSql_wx10.Append("delete from t_wx_diy_menus ");
            strSql_wx10.Append(" where wx_id in (select id from t_wx_wechats where u_id=@u_id) ");
            SqlParameter[] parameters_wx10 = {
					new SqlParameter("@u_id", SqlDbType.Int,4)};
            parameters_wx10[0].Value = uid;
            cmd = new CommandInfo(strSql_wx10.ToString(), parameters_wx10);
            sqllist_wx.Add(cmd);

            //删除公众号自定义菜单表（11）
            StringBuilder strSql_wx11 = new StringBuilder();
            strSql_wx11.Append("delete from t_wx_diy_menus ");
            strSql_wx11.Append(" where wx_id in (select id from t_wx_wechats where u_id=@u_id) ");
            SqlParameter[] parameters_wx11 = {
					new SqlParameter("@u_id", SqlDbType.Int,4)};
            parameters_wx11[0].Value = uid;
            cmd = new CommandInfo(strSql_wx11.ToString(), parameters_wx11);
            sqllist_wx.Add(cmd);

            //删除公众号模块表（12）
            StringBuilder strSql_wx12 = new StringBuilder();
            strSql_wx12.Append("delete from t_module_wechat ");
            strSql_wx12.Append(" where wx_id in (select id from t_wx_wechats where u_id=@u_id) ");
            SqlParameter[] parameters_wx12 = {
					new SqlParameter("@u_id", SqlDbType.Int,4)};
            parameters_wx12[0].Value = uid;
            cmd = new CommandInfo(strSql_wx12.ToString(), parameters_wx12);
            sqllist_wx.Add(cmd);

            int rowsAffected = KDWechat.DBUtility.DbHelperSQL.ExecuteSqlTran(sqllist, sqllist_wx);
            #endregion 结束处理事务

            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 内部方法
        public static bool UpdateUsers(t_sys_users userToUpdate)
        {
            return EFHelper.UpdateUser<t_sys_users>(userToUpdate);
        }
        public static string CreatePassword(string pwd, string salt)
        {
            return DESEncrypt.Encrypt(pwd, salt);
        }
        public static string GetPasswordBySalt(string pwd, string salt)
        {
            return DESEncrypt.Decrypt(pwd, salt);
        }
        public static string CreateSalt()
        {
            string cl = DateTime.Now.ToString();
            string pwd = "";
            MD5 md5 = MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            for (int i = 0; i < s.Length; i++)
            {
                pwd = pwd + s[i].ToString("X");
            }
            return pwd.Substring(0, 9);
        }
        #endregion

        public static int[] GetAdminArray(Expression<Func<t_sys_users, bool>> where, Expression<Func<t_sys_users, int>> selectedValue)
        {
            int[] array = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                array = db.t_sys_users.Where(where.Expand()).Select(selectedValue.Expand()).ToArray();
            }
            return array;
        }
    }
}
