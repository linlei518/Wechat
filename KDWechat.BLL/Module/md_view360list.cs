using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KDWechat.DAL;
using EntityFramework.Extensions;

namespace KDWechat.BLL.Module
{
    public class md_view360list
    {
        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static t_md_360viewList GetModel(int id)
        {
            t_md_360viewList model = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                model = db.t_md_360viewList.Where(x => x.id == id).FirstOrDefault();
            }
            return model;
        }

        /// <summary>
        /// 根据所属菜单ID提取1条数据
        /// </summary>
        /// <param name="id">M_id</param>
        /// <returns></returns>
        public static t_md_360viewList GetModelByM_id(int m_id)
        {
            t_md_360viewList model = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                model = db.t_md_360viewList.Where(x => x.id == m_id).FirstOrDefault();
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
                isFinish = db.t_md_360viewList.Where(x => x.id == id).Delete() > 0;
            }
            return isFinish;
        }

        /// <summary>
        /// 添加一条信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_md_360viewList Add(t_md_360viewList model)
        {

            model = EFHelper.AddModule<t_md_360viewList>(model);
            return model;//返回添加后的信息
        }

        /// <summary>
        /// 修改一条信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_md_360viewList Update(t_md_360viewList model)
        {
            return EFHelper.UpdateModule<t_md_360viewList>(model);
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_360viewList> GetList(string wx_og_id)
        {
            List<t_md_360viewList> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                var query = (from x in db.t_md_360viewList where  x.wx_og_id == wx_og_id select x);
                list = query.ToList();

            }
            return list;
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_360viewList> GetListForBuid(int pid,string wx_og_id)
        {
            List<t_md_360viewList> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                var query = (from x in db.t_md_360viewList where x.pid == pid && x.wx_og_id==wx_og_id orderby x.sort_id select x);
                list = query.ToList();

            }
            return list;
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_360viewList> GetListForName(int pid,string name, string wx_og_id)
        {
            List<t_md_360viewList> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                var query = (from x in db.t_md_360viewList where x.pid == pid && x.name==name && x.wx_og_id == wx_og_id orderby x.sort_id select x);
                list = query.ToList();

            }
            return list;
        }
        public static List<t_md_360viewList> GetMaxID()
        {
            List<t_md_360viewList> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {

                var query = (from x in db.t_md_360viewList select x);
                list = query.ToList();
            }
            return list;
        }


    }
}