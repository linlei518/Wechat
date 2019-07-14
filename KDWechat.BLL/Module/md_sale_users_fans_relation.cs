using KDWechat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LinqKit;
using EntityFramework.Extensions;

namespace KDWechat.BLL.Module
{
    public class md_sale_users_fans_relation
    {
        /// <summary>
        /// 添加一条
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_md_sale_users_fans_relation AddModel(t_md_sale_users_fans_relation model)
        {
            return EFHelper.AddModule<t_md_sale_users_fans_relation>(model);
        }

        /// <summary>
        /// 修改一条
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_md_sale_users_fans_relation UpdateModel(t_md_sale_users_fans_relation model)
        {
            return EFHelper.UpdateModule<t_md_sale_users_fans_relation>(model);
        }

        /// <summary>
        /// 获取一条
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static t_md_sale_users_fans_relation GetModel(Expression<Func<t_md_sale_users_fans_relation, bool>> where)
        {
            t_md_sale_users_fans_relation model = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                model = db.t_md_sale_users_fans_relation.Where(where.Expand()).OrderByDescending(x=>x.id).FirstOrDefault();
            }
            return model;
        }

        /// <summary>
        /// 删除联系
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static bool DeleteRelation(Expression<Func<t_md_sale_users_fans_relation, bool>> where)
        {
            bool isOk = false;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                isOk = db.t_md_sale_users_fans_relation.Where(where.Expand()).Delete()>0;
            }
            return isOk;
        }
    }
}
