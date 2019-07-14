using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KDWechat.DAL;
using EntityFramework.Extensions;
using KDWechat.Common;
using System.Linq.Expressions;
using LinqKit;
namespace KDWechat.BLL.Module
{
    public class md_sale_users
    {
        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static t_md_sale_users GetModel(int id)
        {
            t_md_sale_users model = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                model = db.t_md_sale_users.Where(x => x.id == id).FirstOrDefault();
            }
            return model;
        }
        /// <summary>
        /// 添加一条信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_md_sale_users Add(t_md_sale_users model)
        {

            model = EFHelper.AddModule<t_md_sale_users>(model);
            return model;//返回添加后的信息
        }

        /// <summary>
        /// 修改一条信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_md_sale_users Update(t_md_sale_users model)
        {
            return EFHelper.UpdateModule<t_md_sale_users>(model);
        }
        /// <summary>
        /// 根据时间倒叙取销售列表
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public static List<t_md_sale_users> GetListBySizeAndIndex(int wx_id,int pagesize, int pageindex,out int count)
        {
            List<t_md_sale_users> chatList = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                var query=(from x in db.t_md_sale_users where x.wx_id==wx_id orderby x.id descending select x);
                chatList = query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                count = query.Count();
            }
            return chatList;
        }
        /// <summary>
        /// 修改销售状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static bool SetUserStatus(int id, Status status)
        {
            bool isFinish = false;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                var user = db.t_md_sale_users.Where(x => x.id == id).FirstOrDefault();
                if (null != user)
                {
                    user.status = (int)status;
                    isFinish = db.SaveChanges() > 0;
                }
            }
            return isFinish;
        }

        /// <summary>
        /// 修改销售状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static bool SetUserStatus(int id, SaleUserStatus status)
        {
            bool isFinish = false;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                var user = db.t_md_sale_users.Where(x => x.id == id).FirstOrDefault();
                if (null != user)
                {
                    user.status = (int)status;
                    isFinish = db.SaveChanges() > 0;
                }
            }
            return isFinish;
        }

        /// <summary>
        /// 获取数量
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static int GetCount(Expression<Func<t_md_sale_users, bool>> where)
        {
            int count = 0;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                count = db.t_md_sale_users.Where(where.Expand()).Count();
            }
            return count;
        }

        public static T[] GetArray<T>(Expression<Func<t_md_sale_users,bool>> where,Expression<Func<t_md_sale_users,T>> select)
        {
            T[] array = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                array = db.t_md_sale_users.Where(where.Expand()).Select(select).ToArray();
            }
            return array;
        }
        /// <summary>
        /// 检查销售账号是否可用
        /// </summary>
        /// <param name="userName">账号</param>
        /// <returns>账号名是否可用</returns>
        public static bool CheckUserName(string userName)
        {
            bool isChecked = false;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                isChecked = (from x in db.t_md_sale_users where x.username == userName select x).FirstOrDefault() == null;
            }
            return isChecked;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static t_md_sale_users Login(string userName, string password)
        {
            t_md_sale_users user = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                user = (from x in db.t_md_sale_users where x.username == userName && x.status != (int)Status.禁用 select x).FirstOrDefault();
                if (null == user || DESEncrypt.Encrypt(password, user.salt) != user.password)
                    user = null;
            }
            return user;

        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="T">orderby的字段类型</typeparam>
        /// <param name="where">where条件，可拼接</param>
        /// <param name="orderBy">orderby的字段</param>
        /// <param name="count">where条件下的总数</param>
        /// <param name="pagesize">页大小</param>
        /// <param name="pageindex">页号</param>
        /// <param name="ordrebyDesc">是否倒序</param>
        /// <returns></returns>
        public static List<t_md_sale_users> GetList<T>(Expression<Func<t_md_sale_users, bool>> where, Expression<Func<t_md_sale_users, T>> orderBy, out int count, int pagesize, int pageindex, bool ordrebyDesc = false)
        {
            List<t_md_sale_users> chatList = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                var query = db.t_md_sale_users.Where(where.Expand());
                count = query.Count();
                if (!ordrebyDesc)
                    chatList = query.OrderBy(orderBy).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                else
                    chatList = query.OrderByDescending(orderBy).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            }
            return chatList;
        }
    }
}
