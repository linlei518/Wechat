using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KDWechat.DAL;
using EntityFramework.Extensions;

namespace KDWechat.BLL.Module
{
    public class md_reserve_manage
    {
        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static t_md_reserve_manage GetModel(int id)
        {
            t_md_reserve_manage model = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                model = db.t_md_reserve_manage.Where(x => x.id == id).FirstOrDefault();
            }
            return model;
        }
        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static t_md_reserve_manage GetModel(string reserveName,string wx_og_id)
        {
            t_md_reserve_manage model = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                model = db.t_md_reserve_manage.Where(x => x.reservename == reserveName && x.wx_og_id == wx_og_id).FirstOrDefault();
            }
            return model;
        }
       
        /// <summary>
        /// 根据所属菜单ID提取1条数据
        /// </summary>
        /// <param name="id">M_id</param>
        /// <returns></returns>
        public static t_md_reserve_manage GetModelByM_id(int m_id)
        {
            t_md_reserve_manage model = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                model = db.t_md_reserve_manage.Where(x => x.id == m_id).FirstOrDefault();
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
                isFinish = db.t_md_reserve_manage.Where(x => x.id == id).Delete() > 0;
            }
            return isFinish;
        }


        /// <summary>
        /// 添加一条信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_md_reserve_manage Add(t_md_reserve_manage model)
        {

            model = EFHelper.AddModule<t_md_reserve_manage>(model);
            return model;//返回添加后的信息
        }

        /// <summary>
        /// 修改一条信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_md_reserve_manage Update(t_md_reserve_manage model)
        {
            return EFHelper.UpdateModule<t_md_reserve_manage>(model);
        }

        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_reserve_manage> GetList(string wx_og_id, int pagesize, int pageindex, out int count)
        {
            List<t_md_reserve_manage> list = null;
            List<t_md_reserve_manage> listForCount = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {

                list = (from x in db.t_md_reserve_manage where x.wx_og_id == wx_og_id orderby x.id descending select x).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                listForCount = (from x in db.t_md_reserve_manage orderby x.id select x).ToList();



                count = listForCount.Count();

            }
            return list;
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_reserve_manage> GetListView()
        {
            List<t_md_reserve_manage> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                var query = (from x in db.t_md_reserve_manage select x);
                list = query.ToList();

            }
            return list;
        }



        public static List<t_md_reserve_manage> GetMaxID()
        {
            List<t_md_reserve_manage> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {

                var query = (from x in db.t_md_reserve_manage select x);
                list = query.ToList();
            }
            return list;
        }


    }
}
