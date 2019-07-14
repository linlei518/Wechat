using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KDWechat.DAL;
using EntityFramework.Extensions;
namespace KDWechat.BLL.Module
{
    public class md_view360_buid
    {
        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static t_md_360buid GetModel(int id)
        {
            t_md_360buid model = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                model = db.t_md_360buid.Where(x => x.id == id).FirstOrDefault();
            }
            return model;
        }
        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static t_md_360buid GetModel(string name)
        {
            t_md_360buid model = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                model = db.t_md_360buid.Where(x => x.name == name).FirstOrDefault();
            }
            return model;
        }
        /// <summary>
        /// 根据所属菜单ID提取1条数据
        /// </summary>
        /// <param name="id">M_id</param>
        /// <returns></returns>
        public static t_md_360buid GetModelByM_id(int m_id)
        {
            t_md_360buid model = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                model = db.t_md_360buid.Where(x => x.id == m_id).FirstOrDefault();
            }
            return model;
        }

        /// <summary>
        /// 删除一条信息
        /// </summary>
        /// <param name="id">信息ID</param>
        /// <returns>是否删除成功</returns>
        public static bool Delete(int id)
        {
            bool isFinish = false;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                isFinish = db.t_md_360buid.Where(x => x.id == id).Delete() > 0;
            }
            return isFinish;
        }

        /// <summary>
        /// 添加一条信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_md_360buid Add(t_md_360buid model)
        {

            model = EFHelper.AddModule<t_md_360buid>(model);
            return model;//返回添加后的信息
        }

        /// <summary>
        /// 修改一条信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_md_360buid Update(t_md_360buid model)
        {
            return EFHelper.UpdateModule<t_md_360buid>(model);
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_360buid> GetList(string wx_og_id)
        {
            List<t_md_360buid> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                var query = (from x in db.t_md_360buid where  x.wx_og_id == wx_og_id select x);
                list = query.ToList();

            }
            return list;
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_360buid> GetListForName(string name,string wx_og_id)
        {
            List<t_md_360buid> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                var query = (from x in db.t_md_360buid where x.name==name && x.wx_og_id == wx_og_id select x);
                list = query.ToList();

            }
            return list;
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_360buid> GetList(string wx_og_id, int pagesize, int pageindex, out int count)
        {
            List<t_md_360buid> list = null;
            List<t_md_360buid> listForCount = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                list = (from x in db.t_md_360buid where x.wx_og_id == wx_og_id orderby x.id descending select x).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                listForCount = (from x in db.t_md_360buid where x.wx_og_id == wx_og_id orderby x.id descending select x).ToList();
                count = listForCount.Count();

            }
            return list;
        }
        public static List<t_md_360buid> GetMaxID()
        {
            List<t_md_360buid> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {

                var query = (from x in db.t_md_360buid select x);
                list = query.ToList();
            }
            return list;
        }


    }
}