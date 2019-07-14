using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqKit;
using EntityFramework.Extensions;
using KDWechat.DAL;
using System.Linq.Expressions;

namespace KDWechat.BLL.Module
{
    public class md_sale_users_project_view
    {

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalCount"></param>
        /// <param name="orderByDesc"></param>
        /// <returns></returns>
        public static List<t_md_sale_users_project_view> GetList<T>(Expression<Func<t_md_sale_users_project_view, bool>> where, Expression<Func<t_md_sale_users_project_view, T>> orderBy, int pagesize, int pageindex, out int totalCount, bool orderByDesc = false)
        {
            List<t_md_sale_users_project_view> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                var query = db.t_md_sale_users_project_view.Where(where.Expand());
                totalCount = query.Count();
                if (!orderByDesc)
                    query = query.OrderBy(orderBy.Expand());
                else
                    query = query.OrderByDescending(orderBy.Expand());
                list = query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            }
            return list;

        }
    }
}
