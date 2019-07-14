using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KDWechat.DAL;
using LinqKit;
using EntityFramework.Extensions;
using KDWechat.Common;
using System.Linq.Expressions;

namespace KDWechat.BLL.Chats
{
    public class wx_qrcode
    {

        /// <summary>
        /// 添加model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool AddModel(t_wx_qrcode model)
        {
            bool isOk =false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                db.t_wx_qrcode.Add(model);
                isOk = db.SaveChanges() > 0;
            }
            return isOk;
        }

        public static t_wx_qrcode UpdateModel(t_wx_qrcode model)
        {
            return EFHelper.UpdateWeChat<t_wx_qrcode>(model);
        }

        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static t_wx_qrcode GetModel<T>(Expression<Func<t_wx_qrcode, bool>> where,Expression<Func<t_wx_qrcode, T>> ordreBy,bool desc=false)
        {
            t_wx_qrcode model = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                if(!desc)
                    model = db.t_wx_qrcode.Where(where.Expand()).OrderBy(ordreBy.Expand()).FirstOrDefault();
                else
                    model = db.t_wx_qrcode.Where(where.Expand()).OrderByDescending(ordreBy.Expand()).FirstOrDefault();
            }
            return model;
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="ordreBy"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalCount"></param>
        /// <param name="orderByDesc"></param>
        /// <returns></returns>
        public static List<t_wx_qrcode> GetList<T>(Expression<Func<t_wx_qrcode, bool>> where, Expression<Func<t_wx_qrcode, T>> ordreBy, int pagesize, int pageindex, out int totalCount, bool orderByDesc = false)
        {
            List<t_wx_qrcode> list = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                var query = db.t_wx_qrcode.Where(where.Expand());
                totalCount = query.Count();
                if (!orderByDesc)
                    list = query.OrderBy(ordreBy.Expand()).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                else
                    list = query.OrderByDescending(ordreBy.Expand()).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            }
            return list;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="T1">order by 类型</typeparam>
        /// <typeparam name="T2">select 类型</typeparam>
        /// <param name="where"></param>
        /// <param name="ordreBy"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalCount"></param>
        /// <param name="orderByDesc"></param>
        /// <returns></returns>
        public static T2[] GetArray<T1, T2>(Expression<Func<t_wx_qrcode, bool>> where, Expression<Func<t_wx_qrcode, T1>> ordreBy, Expression<Func<t_wx_qrcode, T2>> select, int pagesize, int pageindex, out int totalCount, bool orderByDesc = false)
        {
            T2[] list = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                var query = db.t_wx_qrcode.Where(where.Expand());
                totalCount = query.Count();
                if (!orderByDesc)
                    list = query.OrderBy(ordreBy.Expand()).Select(select).Skip((pageindex - 1) * pagesize).Take(pagesize).ToArray();
                else
                    list = query.OrderByDescending(ordreBy.Expand()).Select(select).Skip((pageindex - 1) * pagesize).Take(pagesize).ToArray();
            }
            return list;
        }


        /// <summary>
        /// 获取数量
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static int GetCount(Expression<Func<t_wx_qrcode, bool>> where)
        {
            int count = 0;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                count = db.t_wx_qrcode.Where(where.Expand()).Count();
            }
            return count;
        }

        /// <summary>
        /// 获取数量
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static List<t_wx_qrcode> GetList(Expression<Func<t_wx_qrcode, bool>> where)
        {
            List<t_wx_qrcode> list = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                list = db.t_wx_qrcode.Where(where.Expand()).ToList();
            }
            return list;
        }
    }
}
