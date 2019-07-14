using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KDWechat.DAL;
using EntityFramework.Extensions;

namespace KDWechat.BLL.Module
{
    public class md_view
    {
        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static t_md_360view GetModel(int id)
        {
            t_md_360view model = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                model = db.t_md_360view.Where(x => x.id == id).FirstOrDefault();
            }
            return model;
        }

        /// <summary>
        /// 根据所属菜单ID提取1条数据
        /// </summary>
        /// <param name="id">M_id</param>
        /// <returns></returns>
        public static t_md_360view GetModelByM_id(int m_id)
        {
            t_md_360view model = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                model = db.t_md_360view.Where(x => x.id == m_id).FirstOrDefault();
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
                isFinish = db.t_md_360view.Where(x => x.id == id).Delete() > 0;
            }
            return isFinish;
        }
        /// <summary>
        /// 删除一条信息
        /// </summary>
        /// <param name="id">信息ID</param>
        /// <returns>是否删除成功</returns>
        public static bool DeleteP_ID(int id)
        {
            bool isFinish = false;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                isFinish = db.t_md_360view.Where(x => x.p_id == id).Delete() > 0;
            }
            return isFinish;
        }

        /// <summary>
        /// 添加一条信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_md_360view Add(t_md_360view model)
        {

            model = EFHelper.AddModule<t_md_360view>(model);
            return model;//返回添加后的信息
        }

        /// <summary>
        /// 修改一条信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_md_360view Update(t_md_360view model)
        {
            return EFHelper.UpdateModule<t_md_360view>(model);
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_360view> GetList(string wx_og_id, int pagesize, int pageindex,out int count)
        {
            List<t_md_360view> list = null;
            List<t_md_360view> listForCount = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                list = (from x in db.t_md_360view where   x.wx_og_id == wx_og_id orderby x.id select x).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                listForCount = (from x in db.t_md_360view where  x.wx_og_id == wx_og_id orderby x.id select x).ToList();
                count = listForCount.Count();

            }
            return list;
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_360view>  GetList(int pid, string wx_og_id, int pagesize, int pageindex, out int count)
        {
            List<t_md_360view> list = null;
            List<t_md_360view> listForCount = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                if (pid == -1)
                {
                    list = (from x in db.t_md_360view where    x.wx_og_id == wx_og_id orderby x.id select x).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                    listForCount = (from x in db.t_md_360view where    x.wx_og_id == wx_og_id orderby x.id select x).ToList();
                }
                if (pid != 0)
                {
                    list = (from x in db.t_md_360view where x.p_id == pid && x.wx_og_id == wx_og_id orderby x.id select x).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                    listForCount = (from x in db.t_md_360view where x.p_id == pid  && x.wx_og_id == wx_og_id orderby x.id select x).ToList();
                }
                if (pid == 0)
                {
                    count = 0;
                }
                else
                {
                    count = listForCount.Count();
                }

            }
            return list;
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_360view> GetListView()
        {
            List<t_md_360view> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                var query = (from x in db.t_md_360view  select x);
                list = query.ToList();

            }
            return list;
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_360view> GetListForPro(int p_id)
        {
            List<t_md_360view> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                var query = (from x in db.t_md_360view where x.p_id==p_id  orderby x.sort_id select x);
                list = query.ToList();

            }
            return list;
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_360view> GetListView(string wx_og_id)
        {
            List<t_md_360view> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                var query = (from x in db.t_md_360view where x.wx_og_id==wx_og_id select x);
                list = query.ToList();

            }
            return list;
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_360view> GetListForName(int pid,string Name,string wx_og_id)
        {
            List<t_md_360view> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                var query = (from x in db.t_md_360view where x.p_id==pid && x.title==Name && x.wx_og_id == wx_og_id select x);
                list = query.ToList();

            }
            return list;
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_360view> GetListFort_id(int t_id)
        {
            List<t_md_360view> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                var query = (from x in db.t_md_360view where x.p_id == t_id orderby x.sort_id select x);
                list = query.ToList();

            }
            return list;
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_360view> GetListView(string wx_og_id,int pagesize,int pageindex,out int count)
        {
            List<t_md_360view> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                var query = (from x in db.t_md_360view where x.wx_og_id == wx_og_id orderby x.id descending select x);
                count = query.Count();
                list = query.Skip((pageindex-1)*pagesize).Take(pagesize).ToList();

            }
            return list;
        }
        public static List<t_md_360view> GetMaxID()
        {
            List<t_md_360view> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {

                var query = (from x in db.t_md_360view select x);
                list = query.ToList();
            }
            return list;
        }
    

    }
}
