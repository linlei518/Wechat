using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KDWechat.DAL;
using EntityFramework.Extensions;
using KDWechat.Common;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP;
using KDWechat.DBUtility;
using System.Data.SqlClient;
using System.Data;
using System.Linq.Expressions;
using LinqKit;
using Senparc.Weixin.Exceptions;


namespace KDWechat.BLL.Chats
{
    public class wx_wechats
    {

        #region 公开访问方法


        #region 创建公众号
        /// <summary>
        /// 创建一个公众号
        /// </summary>
        /// <param name="uid">添加者ID</param>
        /// <param name="name">公众号名称</param>
        /// <param name="ogID">公众号原始ID</param>
        /// <param name="weChatNo">微信号</param>
        /// <param name="imgUrl">头像地址</param>
        /// <param name="weChatType">公众号类型</param>
        /// <param name="appID">APPID</param>
        /// <param name="appSecret">APPSECRET</param>
        /// <returns>被创建后的公众号</returns>
        public static t_wx_wechats CreateWeChat(int uid, string name, string ogID, string weChatNo, string imgUrl, WeChatServiceType weChatType, string appID, string appSecret, string token, string guid,string qy_user_id,string qy_user_name,string city,string qrcode_img)
        {
            var checkWeChat = Companycn.Core.EntityFramework.EFHelper.GetModel<creater_wxEntities, t_wx_wechats>(x => x.wx_og_id == ogID);
            if (checkWeChat != null)
                return null;
            t_wx_wechats wechat = new t_wx_wechats()
            {
                wx_pb_name = name,
                uid = uid,
                wx_name = weChatNo,
                wx_og_id = ogID,
                header_pic = imgUrl,
                type_id = (int)weChatType,
                create_time = DateTime.Now,
                modify_time = DateTime.Now,
                app_id = appID,
                app_secret = appSecret,
                token = token,
                wx_guid = guid.Split(new char[] { '=' })[1],
                api_url = guid,
                status=1,
                qy_manager_name=qy_user_id,
                qy_manager_nick =qy_user_name,
                city=city,
                is_open_wx_reg=0,
                mall_source=1000,
                qrcode_img = qrcode_img
            };
            wechat = Companycn.Core.EntityFramework.EFHelper.AddModel<creater_wxEntities, t_wx_wechats>(wechat);
            //wechat = InsertWeChats(wechat);
            //creater_wxEntities db = new creater_wxEntities();
            //db.Add<t_wx_wechats>(wechat);
            //db.Dispose();
            return wechat;
        }
        #endregion

        #region 更新公众号
        /// <summary>
        /// 更新公众号数据
        /// </summary>
        /// <param name="id">公众号ID</param>
        /// <param name="uid">添加者ID</param>
        /// <param name="name">公众号名称</param>
        /// <param name="weChatNo">微信号</param>
        /// <param name="imgUrl">头像地址</param>
        /// <param name="weChatType">公众号类型</param>
        /// <param name="appID">appid</param>
        /// <param name="appSecret">appsecret</param>
        /// <returns></returns>
        public static t_wx_wechats UpdateWeChat(int id, string name = "", string weChatNo = "", string imgUrl = "", WeChatServiceType weChatType = 0, string appID = "", string appSecret = "")
        {

            t_wx_wechats wechat = GetWeChatByID(id);
            if (null != wechat)
            {
                wechat.wx_pb_name = DataHelper.GetRealValue(name, wechat.wx_pb_name);
                wechat.wx_name = DataHelper.GetRealValue(weChatNo, wechat.wx_name);
                wechat.header_pic = DataHelper.GetRealValue(imgUrl, wechat.header_pic);
                wechat.type_id = DataHelper.GetRealValue((int)weChatType, wechat.type_id);
                wechat.modify_time = DateTime.Now;
                wechat.app_id = DataHelper.GetRealValue(appID, wechat.app_id);
                wechat.app_secret = DataHelper.GetRealValue(appSecret, wechat.app_secret);
            }
            UpdateWeChat(wechat);

            return wechat;
        }
        #endregion

        #region 删除公众号
        /// <summary>
        /// 删除一个公众号
        /// </summary>
        /// <param name="id">需要删除的公众号ID</param>
        /// <returns>删除是否成功</returns>
        public static bool DeleteWeChatsByID(int id)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isFinish = db.t_wx_wechats.Where(x => x.id == id).Delete() > 0;
            }
            return isFinish;
        }
        #endregion

        /// <summary>
        /// 删除一个公众号及其相关所有信息(可能还会增加删除表格)
        /// </summary>
        /// <param name="id">需要删除的公众号ID</param>
        /// <returns>删除是否成功</returns>
        public static bool DeleteWeChatsAll(int id)
        {
            #region 处理事务
            List<CommandInfo> sqllist = new List<CommandInfo>();

            //删除微信公众号表（1）
            StringBuilder strSql1 = new StringBuilder();
            strSql1.Append("delete from t_wx_wechats ");
            strSql1.Append(" where id=@id ");
            SqlParameter[] parameters1 = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters1[0].Value = id;
            CommandInfo cmd = new CommandInfo(strSql1.ToString(), parameters1);
            sqllist.Add(cmd);

            //删除规则表（2）
            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append("delete from t_wx_rules ");
            strSql2.Append(" where wx_id=@wx_id ");
            SqlParameter[] parameters2 = {
					new SqlParameter("@wx_id", SqlDbType.Int,4)};
            parameters2[0].Value = id;
            cmd = new CommandInfo(strSql2.ToString(), parameters2);
            sqllist.Add(cmd);

            //删除规则关键词表（3）
            StringBuilder strSql3 = new StringBuilder();
            strSql3.Append("delete from t_wx_rules_keywords ");
            strSql3.Append(" where wx_id=@wx_id ");
            SqlParameter[] parameters3 = {
					new SqlParameter("@wx_id", SqlDbType.Int,4)};
            parameters3[0].Value = id;
            cmd = new CommandInfo(strSql3.ToString(), parameters3);
            sqllist.Add(cmd);

            //删除关键词回复主表（4）
            StringBuilder strSql4 = new StringBuilder();
            strSql4.Append("delete from t_wx_rule_reply ");
            strSql4.Append(" where wx_id=@wx_id ");
            SqlParameter[] parameters4 = {
					new SqlParameter("@wx_id", SqlDbType.Int,4)};
            parameters4[0].Value = id;
            cmd = new CommandInfo(strSql4.ToString(), parameters4);
            sqllist.Add(cmd);

            //删除公众号图文素材表（5）
            StringBuilder strSql5 = new StringBuilder();
            strSql5.Append("delete from t_wx_news_materials ");
            strSql5.Append(" where wx_id=@wx_id ");
            SqlParameter[] parameters5 = {
					new SqlParameter("@wx_id", SqlDbType.Int,4)};
            parameters5[0].Value = id;
            cmd = new CommandInfo(strSql5.ToString(), parameters5);
            sqllist.Add(cmd);

            //删除公众号媒体素材表（6）
            StringBuilder strSql6 = new StringBuilder();
            strSql6.Append("delete from t_wx_media_materials ");
            strSql6.Append(" where wx_id=@wx_id ");
            SqlParameter[] parameters6 = {
					new SqlParameter("@wx_id", SqlDbType.Int,4)};
            parameters6[0].Value = id;
            cmd = new CommandInfo(strSql6.ToString(), parameters6);
            sqllist.Add(cmd);

            //删除公众号基本回复表（7）
            StringBuilder strSql7 = new StringBuilder();
            strSql7.Append("delete from t_wx_basic_reply ");
            strSql7.Append(" where wx_id=@wx_id ");
            SqlParameter[] parameters7 = {
					new SqlParameter("@wx_id", SqlDbType.Int,4)};
            parameters7[0].Value = id;
            cmd = new CommandInfo(strSql7.ToString(), parameters7);
            sqllist.Add(cmd);

            //删除公众号LBS表（8）
            StringBuilder strSql8 = new StringBuilder();
            strSql8.Append("delete from t_wx_lbs ");
            strSql8.Append(" where wx_id=@wx_id ");
            SqlParameter[] parameters8 = {
					new SqlParameter("@wx_id", SqlDbType.Int,4)};
            parameters8[0].Value = id;
            cmd = new CommandInfo(strSql8.ToString(), parameters8);
            sqllist.Add(cmd);

            //删除公众号群发信息表（9）
            StringBuilder strSql9 = new StringBuilder();
            strSql9.Append("delete from t_wx_group_msgs ");
            strSql9.Append(" where wx_id=@wx_id ");
            SqlParameter[] parameters9 = {
					new SqlParameter("@wx_id", SqlDbType.Int,4)};
            parameters9[0].Value = id;
            cmd = new CommandInfo(strSql9.ToString(), parameters9);
            sqllist.Add(cmd);

            //删除公众号自定义菜单表（10）
            StringBuilder strSql10 = new StringBuilder();
            strSql10.Append("delete from t_wx_diy_menus ");
            strSql10.Append(" where wx_id=@wx_id ");
            SqlParameter[] parameters10 = {
					new SqlParameter("@wx_id", SqlDbType.Int,4)};
            parameters10[0].Value = id;
            cmd = new CommandInfo(strSql10.ToString(), parameters10);
            sqllist.Add(cmd);

            //删除公众号自定义菜单表（11）
            StringBuilder strSql11 = new StringBuilder();
            strSql11.Append("delete from t_wx_diy_menus ");
            strSql11.Append(" where wx_id=@wx_id ");
            SqlParameter[] parameters11 = {
					new SqlParameter("@wx_id", SqlDbType.Int,4)};
            parameters11[0].Value = id;
            cmd = new CommandInfo(strSql11.ToString(), parameters11);
            sqllist.Add(cmd);

            //删除公众号模块表（12）
            StringBuilder strSql12 = new StringBuilder();
            strSql12.Append("delete from t_module_wechat ");
            strSql12.Append(" where wx_id=@wx_id ");
            SqlParameter[] parameters12 = {
					new SqlParameter("@wx_id", SqlDbType.Int,4)};
            parameters12[0].Value = id;
            cmd = new CommandInfo(strSql12.ToString(), parameters12);
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
        /// 提取1条公众号
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>公众号信息</returns>
        public static t_wx_wechats GetWeChatByID(int id)
        {
            t_wx_wechats wechat = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                wechat = (from x in db.t_wx_wechats where x.id == id select x).FirstOrDefault();
            }
            return wechat;
        }
        /// <summary>
        /// 根据原始ID取wechat
        /// </summary>
        /// <param name="ogid"></param>
        /// <returns></returns>
        public static t_wx_wechats GetWeChatByogID(string ogid)
        {
            t_wx_wechats wechat = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                wechat = (from x in db.t_wx_wechats where x.wx_og_id == ogid select x).FirstOrDefault();
            }
            return wechat;
        }

        /// <summary>
        /// 根据时间倒叙取公众号列表
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public static List<t_wx_wechats> GetListBySizeAndIndex(int pagesize, int pageindex)
        {
            List<t_wx_wechats> chatList = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                chatList = (from x in db.t_wx_wechats orderby x.id descending select x).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            }
            return chatList;
        }
        /// <summary>
        /// 根据时间倒叙取所有公众号列表
        /// </summary>
        /// <returns></returns>
        public static List<t_wx_wechats> GetList()
        {
            List<t_wx_wechats> chatList = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                chatList = (from x in db.t_wx_wechats orderby x.id descending select x).ToList();
            }
            return chatList;
        }

        /// <summary>
        /// 根据时间倒叙取所有未禁用公众号列表
        /// </summary>
        /// <returns></returns>
        public static List<t_wx_wechats> GetUseList()
        {
            List<t_wx_wechats> chatList = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                chatList = (from x in db.t_wx_wechats where x.status==(int)Status.正常 orderby x.id descending select x).ToList();
            }
            return chatList;
        }

        public static List<t_wx_wechats> GetList<T>(Expression<Func<t_wx_wechats, bool>> where, int pagesize, int pageindex, out int count, Expression<Func<t_wx_wechats, T>> orderBy, bool isDesc = false)
        {
            List<t_wx_wechats> chatList = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                var query = db.t_wx_wechats.Where(where.Expand());
                count = query.Count();
                if (isDesc)
                    chatList = query.OrderByDescending(orderBy.Expand()).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                else
                    chatList = query.OrderBy(orderBy.Expand()).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            }
            return chatList;
        }
        /// <summary>
        /// 根据ID列表取所有公众号列表
        /// </summary>
        /// <returns></returns>
        public static List<t_wx_wechats> GetList(int[] idList)
        {
            List<t_wx_wechats> chatList = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                chatList = (from x in db.t_wx_wechats where idList.Contains(x.id) orderby x.id descending select x).ToList();
            }
            return chatList;
        }
        /// <summary>
        /// 根据创建者取微信公众号列表
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public static List<t_wx_wechats> GetListByUid(int uid, int pagesize, int pageindex, out int count)
        {
            List<t_wx_wechats> chatList = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                chatList = (from x in db.t_wx_wechats where x.uid == uid orderby x.id descending select x).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                count = db.t_wx_wechats.Count();
            }
            return chatList;
        }
        /// <summary>
        /// 获取子帐号公众号列表（权限验证）
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public static List<t_wx_wechats> GetListByChildUid(int uid, int pagesize, int pageindex, out int count)
        {
            List<t_wx_wechats> chatList = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                creater_wxEntities userDb = new creater_wxEntities();
                var powerList = userDb.t_sys_users_power.Where(x => x.u_id == uid).Select(x => x.wx_id).ToArray();
                powerList.Distinct();

                var query = db.t_wx_wechats.Where(x => powerList.Contains(x.id));
                count = query.Count();
                chatList = query.OrderByDescending(x => x.id).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                userDb.Dispose();
            }
            return chatList;
        }

        /// <summary>
        /// 根据创建者取微信公众号列表
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public static List<t_wx_wechats> GetListByUid(int uid)
        {
            List<t_wx_wechats> chatList = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                chatList = (from x in db.t_wx_wechats where x.uid == uid orderby x.id descending select x).ToList();
            }
            return chatList;
        }
        /// <summary>
        /// 获取子帐号的公众号列表（权限验证）
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public static List<t_wx_wechats> GetListByChildUid(int uid)
        {
            List<t_wx_wechats> chatList = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                creater_wxEntities userDb = new creater_wxEntities();
                var powerList = userDb.t_sys_users_power.Where(x => x.u_id == uid).Select(x => x.wx_id).ToArray();
                powerList.Distinct();
                chatList = db.t_wx_wechats.Where(x => powerList.Contains(x.id)).OrderByDescending(x => x.id).ToList();
                userDb.Dispose();
            }
            return chatList;
        }
        //更新一条服务号数据
        public static bool UpdateWeChat(t_wx_wechats weChatsToInsert)
        {
            return Companycn.Core.EntityFramework.EFHelper.UpdateModel<creater_wxEntities,t_wx_wechats>(weChatsToInsert);
        }

        /// <summary>
        /// 通过原始ID取TOKEN
        /// </summary>
        /// <param name="originID"></param>
        /// <returns></returns>
        public static string GetTokenByOriginID(string originID)
        {
            string token = "";
            using (creater_wxEntities db = new creater_wxEntities())
            {
                var wechat = db.t_wx_wechats.Where(x => x.wx_og_id == originID).FirstOrDefault();
                if (null != wechat)
                    token = wechat.token;
            }
            return token;
        }



      

        #endregion

        #region 内部操作方法

        //插入一条服务号数据
        static t_wx_wechats InsertWeChats(t_wx_wechats weChatsToInsert)
        {
            return EFHelper.AddWeChat<t_wx_wechats>(weChatsToInsert);
        }


        #endregion



        public static string GetTokenByGuID(string guid)
        {
            string token = "";
            using (creater_wxEntities db = new creater_wxEntities())
            {
                var wechat = db.t_wx_wechats.Where(x => x.wx_guid == guid).FirstOrDefault();
                if (null != wechat)
                    token = wechat.token;
            }
            return token;
        }

        //获取int array
        public static int[] GetWeChatArray(System.Linq.Expressions.Expression<Func<t_wx_wechats, bool>> wechat_where, System.Linq.Expressions.Expression<Func<t_wx_wechats, int>> wechat_selectedValue)
        {
            int[] array = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                array = db.t_wx_wechats.Where(wechat_where.Expand()).Select(wechat_selectedValue.Expand()).ToArray();
            }
            return array;
        }

        public static string GetNameList(int wx_id, string unionid)
        {
            string str = "";
            string sql = "select wx_pb_name from t_wx_wechats where id=" + wx_id;
            if (unionid != null&&!string.IsNullOrWhiteSpace(unionid))
            {
                sql = " select wx_pb_name from t_wx_wechats where id in(select wx_id from t_wx_fans where unionid='" + unionid + "') ";
            }


            DataTable dt = KDWechat.DBUtility.DbHelperSQL.Query(sql).Tables[0];
            if (dt != null)
            {
                foreach (DataRow r in dt.Rows)
                {
                    str += "<span>" + r["wx_pb_name"] + "</span>";
                }
            }

            return str;
        }
        #region 更新/获取sccess_token add by danny

        /// <summary>
        /// 请勿直接调用，这是定时更新所有微信号access_token用的
        /// </summary>
        public static void Update()
        {
           

            DataTable dt = GetUpdateAccessTokenList();
            if (dt!=null)
            {                                                                                                                                              
                Dictionary<int, string> list = new Dictionary<int, string>();
                foreach (DataRow r in dt.Rows)
                {
                    int wx_id = Common.Utils.StrToInt(r["id"].ToString(), 0);
                    if (wx_id>0)
                    {
                        string app_id = r["app_id"].ToString();
                        string app_secret = r["app_secret"].ToString();
                        string access_token = AccessTokenContainer.TryGetToken(app_id, app_secret,true);
                        if (!string.IsNullOrEmpty(access_token))
                        {
                            list.Add(wx_id, access_token);
                        }
                    }
                }
                UpdateAccessTokenList(list);
            }
        }

        /// <summary>
        /// 单个更新微信号的access_token
        /// </summary>
        /// <param name="wx_id"></param>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static int UpdateAccessToken(int wx_id, string access_token)
        {
            int result = 0;
            string sql = "update t_wx_wechats set access_token=@access_token,update_time=getdate() where id=@wx_id";
            SqlParameter[] par = new SqlParameter[] { 
                new SqlParameter("@access_token",SqlDbType.NVarChar,550),
                new SqlParameter("@wx_id",SqlDbType.Int,4)
            };
            par[0].Value = access_token;
            par[1].Value = wx_id;
            result = KDWechat.DBUtility.DbHelperSQL.ExecuteSql(sql, par);
            return result;
        }
        /// <summary>
        /// 获取需要更新access_token的列表
        ///  </summary>
        /// <returns></returns>
        private static DataTable GetUpdateAccessTokenList()
        {
            string sql = "select id,app_id,app_secret,update_time from t_wx_wechats ";
            return KDWechat.DBUtility.DbHelperSQL.Query(sql).Tables[0];
        }



        /// <summary>
        /// 批量更新微信号的access_token
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static int UpdateAccessTokenList(Dictionary<int, string> list)
        {
            StringBuilder sql = new StringBuilder();
            int result = 0;
            if (list != null)
            {
                foreach (int key in list.Keys)
                {
                    sql.Append("update t_wx_wechats set access_token='" + list[key] + "',update_time=getdate() where id=" + key + ";\n");
                }
            }
            result = KDWechat.DBUtility.DbHelperSQL.ExecuteSql(sql.ToString());
            return result;
        }

        /// <summary>
        /// 获取微信号的access_token
        /// </summary>
        /// <param name="wx_id"></param>
        /// <param name="app_id"></param>
        /// <param name="app_secret"></param>
        /// <returns></returns>
        public static string GetAccessToken(int wx_id, t_wx_wechats wx_wechat=null)
        {
            string access_token = "";
            DAL.t_wx_wechats model = wx_wechat;
            if (model==null)
            {
                model = GetWeChatByID(wx_id);
            } 
            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.access_token))
                {
                    access_token = model.access_token;
                    try
                    {
                        var info = User.Info(access_token, "123");
                    }
                    catch(ErrorJsonResultException ex)
                    {
                        string errcode = ex.JsonResult.errcode.ToString();
                        if (errcode == "验证失败" || errcode.Contains("access_token"))
                        {
                            try
                            {
                                access_token = AccessTokenContainer.TryGetToken(model.app_id, model.app_secret, true);
                                UpdateAccessToken(wx_id, access_token);
                            }
                            catch (ErrorJsonResultException)
                            {

                                access_token = "Error:微信接口服务器较忙，请稍后再试。";
                            }
                        }
                    }
                }
                else
                {
                    try
                    {
                        access_token = AccessTokenContainer.TryGetToken(model.app_id, model.app_secret,true);
                        if (!string.IsNullOrEmpty(access_token))
                        {
                            UpdateAccessToken(wx_id, access_token);
                        }
                        else
                        {
                            access_token = "Error:微信接口服务器较忙，请稍后再试。";
                        }
                    }
                    catch (ErrorJsonResultException ex)
                    {
                         
                    }
                }
            }
            else
            {
                access_token = "Error:暂无此微信号";
            }
            return access_token;
        }


        /// <summary>
        /// 获取微信号的access_token
        /// </summary>
        /// <param name="wx_id"></param>
        /// <param name="accessToken"></param>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        public static string GetAccessToken(int wx_id, string accessToken, string appId, string appSecret)
        {
            string new_access_token = accessToken;
            if (!string.IsNullOrEmpty(accessToken))
            {
                
                try
                {
                    var info = User.Info(accessToken, "123");
                }
                catch (ErrorJsonResultException ex)
                {
                    string errcode = ex.JsonResult.errcode.ToString();
                    if (errcode == "验证失败" || errcode.Contains("access_token"))
                    {
                        try
                        {
                            new_access_token = AccessTokenContainer.TryGetToken(appId, appSecret, true);
                            UpdateAccessToken(wx_id, accessToken);
                        }
                        catch (ErrorJsonResultException)
                        {

                            new_access_token = "Error:微信接口服务器较忙，请稍后再试。";
                        }
                    }
                }
            }
            else
            {
                try
                {
                    new_access_token = AccessTokenContainer.TryGetToken(appId, appSecret, true);
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        UpdateAccessToken(wx_id, accessToken);
                    }
                    else
                    {
                        new_access_token = "Error:微信接口服务器较忙，请稍后再试。";
                    }
                }
                catch (ErrorJsonResultException ex)
                {

                }
            }

            return new_access_token;
        }
        #endregion

        #region 获取jsapi_ticket
        public static string GetJsTicket(string appid)
        {
            var JsTicket = Companycn.Core.EntityFramework.EFHelper.GetModel<creater_wxEntities, t_wx_jsapi_ticket>(x => x.appid == appid);
            if (JsTicket == null)
            {
                var wechat = Companycn.Core.EntityFramework.EFHelper.GetModel<creater_wxEntities, t_wx_wechats>(x => x.app_id == appid);
                if (wechat == null)
                    return "Error:该APPID并未在本平台托管";
                var accessToken = GetAccessToken(wechat.id, wechat);
                if (!accessToken.Contains("Error"))
                {
                    var ticket = Common.WeChatJsApi.GetJsApiTicket(accessToken);
                    if(ticket!=null)
                    {
                        Companycn.Core.EntityFramework.EFHelper.AddModel<creater_wxEntities,t_wx_jsapi_ticket>(new t_wx_jsapi_ticket{appid=appid,expire_time=DateTime.Now.AddHours(1),jsapi_ticket=ticket});
                    }
                    return ticket ?? "Error:微信接口请求失败";
                }
                else
                    return "Error:该APPID对应的APPsecret已失效";
            }
            else
            {
                var isExpire = DateTime.Now > JsTicket.expire_time;
                if (!isExpire)
                {
                    return JsTicket.jsapi_ticket;
                }
                else
                {
                    var wechat = Companycn.Core.EntityFramework.EFHelper.GetModel<creater_wxEntities, t_wx_wechats>(x => x.app_id == appid);
                    if (wechat != null)
                    {
                        var accessToken = GetAccessToken(wechat.id, wechat);
                        if (!accessToken.Contains("Error"))
                            return UpdateJsTicket(accessToken, appid);
                        else
                            return "Error:该APPID对应的APPsecret已失效";
                    }
                    else
                        return "Error:该APPID并未在本平台托管";
                }
            }
        }

        static string UpdateJsTicket(string accessToken, string appid)
        {
            var ticket = Common.WeChatJsApi.GetJsApiTicket(accessToken);
            if (ticket != null)
            {
                Companycn.Core.EntityFramework.EFHelper.UpdateModel<creater_wxEntities, t_wx_jsapi_ticket>(x => x.appid == appid, x => new t_wx_jsapi_ticket { jsapi_ticket = ticket, expire_time = DateTime.Now.AddHours(1) });
                return ticket;
            }
            return "Error:调用微信接口失败";
        }

        

        #endregion


        public static int CheckUserExists(string old_name, string new_name, string db_table, int parent_id, int channel_id, int cate_id, int wx_id, string db_name = "kd_wechats",int vid=0)
        {
            int result = 0;
            bool is_check = false;
            if (old_name.Trim().Length > 0)
            {
                if (old_name.Trim() != new_name.Trim()) //有修改名称
                {
                    is_check = true;
                }
            }
            else
            {
                is_check = true;

            }
            if (is_check)
            {
                //
               
                string sql = "";
                if (db_table == "t_wx_rules")
                {
                    sql = "select COUNT(1) from t_wx_rules where rule_name='" + new_name + "' and wx_id=" + wx_id;
                }
                else if (db_table == "t_wx_diy_menus")
                {

                    sql = "select COUNT(1) from t_wx_diy_menus where menu_name='" + new_name + "' and parent_id=" + parent_id + " and wx_id=" + wx_id;
                }
                else if (db_table == "t_wx_templates")
                {

                    sql = "select COUNT(1) from t_wx_templates where id in(select template_id from t_wx_templates_wechats where wx_id=" + wx_id + ") and title='" + new_name + "' and channel_id=" + channel_id + " and cate_id=" + cate_id;
                }
                else if (db_table == "t_v_invite_info")
                {

                    sql = "select COUNT(1) from t_v_invite_info where activity_name='" + new_name + "' and wx_id=" + wx_id;
                }
                else if (db_table == "t_v_invite_images")
                {

                    sql = "select COUNT(1) from t_v_invite_images where img_name='" + new_name + "' and v_id="+vid;
                }
                else
                {
                    sql = "select COUNT(1) from " + db_table + " where title='" + new_name + "' and channel_id=" + channel_id + " and wx_id=" + wx_id;
                }
                if (db_name == "kd_module")
                {
                    result = Convert.ToInt32(KDWechat.DBUtility.DbHelperSQL.GetSingle(sql));
                }
                else
                {
                    result = Convert.ToInt32(KDWechat.DBUtility.DbHelperSQL.GetSingle(sql));
                }

               
                

            }
            return result;
        }
    }
}
