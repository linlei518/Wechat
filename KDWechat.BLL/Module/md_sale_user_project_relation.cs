using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqKit;
using KDWechat.DAL;
using System.Linq.Expressions;
using EntityFramework.Extensions;
namespace KDWechat.BLL.Module
{
    public class md_sale_user_project_relation
    {
        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static t_md_sale_user_project_relation GetModel(int id)
        {
            t_md_sale_user_project_relation model = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                model = db.t_md_sale_user_project_relation.Where(x => x.id == id).FirstOrDefault();
            }
            return model;
        }
        /// <summary>
        /// 添加一条信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_md_sale_user_project_relation Add(t_md_sale_user_project_relation model)
        {
            model = EFHelper.AddModule<t_md_sale_user_project_relation>(model);
            return model;//返回添加后的信息
        }
        /// <summary>
        /// 项目和享受是否存在关联
        /// </summary>
        /// <param name="p_id">项目Id</param>
        /// <param name="s_id">销售Id</param>
        /// <returns></returns>
        public static bool HasRelation(int p_id, int s_id)
        {
            bool res = false;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                res = db.t_md_sale_user_project_relation.Where(x => x.project_id == p_id && x.seller_id == s_id).FirstOrDefault() != null;
            }
            return res;
        }
        /// <summary>
        /// 删除项目和销售的关联
        /// </summary>
        /// <param name="id">需要删除的项目ID</param>
        /// <returns>删除是否成功</returns>
        public static bool DeleteByPID(int p_id)
        {
            bool isFinish = false;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                isFinish = db.t_md_sale_user_project_relation.Where(x => x.project_id == p_id).Delete() > 0;
            }
            return isFinish;
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
        public static List<t_md_sale_user_project_relation> GetList<T>(Expression<Func<t_md_sale_user_project_relation, bool>> where, Expression<Func<t_md_sale_user_project_relation, T>> ordreBy, int pagesize, int pageindex, out int totalCount, bool orderByDesc = false)
        {
            List<t_md_sale_user_project_relation> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                var query = db.t_md_sale_user_project_relation.Where(where.Expand());
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
        public static T2[] GetArray<T1,T2>(Expression<Func<t_md_sale_user_project_relation, bool>> where, Expression<Func<t_md_sale_user_project_relation, T1>> ordreBy,Expression<Func<t_md_sale_user_project_relation, T2>> select, int pagesize, int pageindex, out int totalCount, bool orderByDesc = false)
        {
            T2[] list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                var query = db.t_md_sale_user_project_relation.Where(where.Expand());
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
        public static int GetCount(Expression<Func<t_md_sale_user_project_relation, bool>> where)
        {
            int count = 0;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                count = db.t_md_sale_user_project_relation.Where(where.Expand()).Count();
            }
            return count;
        }
    }
}
