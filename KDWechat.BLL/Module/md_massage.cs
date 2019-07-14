using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KDWechat.DAL;
using EntityFramework.Extensions;
using KDWechat.Common;

using System.Data.SqlClient;
using System.Data;

namespace KDWechat.BLL.Module
{
    public class md_massage
    {
        /// <summary>
        /// 添加一条信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_md_massage Add(t_md_massage model)
        {

            model = EFHelper.AddModule<t_md_massage>(model);
            return model;//返回添加后的信息
        }
        public static t_md_massage Update(t_md_massage model)
        {
            return EFHelper.UpdateModule<t_md_massage>(model);
        }
        public static t_md_massage GetModel(int id)
        {
            t_md_massage model = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                model = db.t_md_massage.Where(x => x.id == id).FirstOrDefault();
            }
            return model;
        }
        public static List<t_md_massage> GetList(int wx_id, string wx_og_id)
        {
            List<t_md_massage> list = null;

            using (kd_moduleEntities db = new kd_moduleEntities())
            {

                list = (from x in db.t_md_massage where x.wx_id == wx_id && x.wx_og_id == wx_og_id orderby x.id select x).ToList();

            }
            return list;
        }
        public static List<t_md_massage> GetList(int p_id, int wx_id, string wx_og_id)
        {
            List<t_md_massage> list = null;

            using (kd_moduleEntities db = new kd_moduleEntities())
            {

                list = (from x in db.t_md_massage where x.p_id == p_id && x.wx_id == wx_id && x.wx_og_id == wx_og_id orderby x.createtime descending, x.id descending select x).ToList();

            }
            return list;
        }
    }
}
