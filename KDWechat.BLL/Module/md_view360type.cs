using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KDWechat.DAL;
using EntityFramework.Extensions;
using System.Data;
using Companycn.Core.DbHelper;


namespace KDWechat.BLL.Module
{
    public class md_view360type
    {
        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static t_md_360viewtype GetModel(int id)
        {
            t_md_360viewtype model = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                model = db.t_md_360viewtype.Where(x => x.id == id).FirstOrDefault();
            }
            return model;
        }

        /// <summary>
        /// 获取房型列表
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="wx_id"></param>
        /// <returns></returns>
        public static DataTable GetListByIds(string ids, int wx_id)
        {
            return DbHelperSQLModule.Query("select *,(select wx_og_id from kd_wechats.dbo.t_wx_wechats where id=" + wx_id + ") as wx_og_id,(select COUNT(*) from t_md_360view where p_id=t_md_360viewtype.id) as fullview_count,(select top 1 id from t_md_360view  where p_id=t_md_360viewtype.id order by id asc) as full_view_id   from t_md_360viewtype where id in(" + ids + ") order by id asc").Tables[0];
        }

        /// <summary>
        /// 获取房型列表
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="wx_id"></param>
        /// <returns></returns>
        public static DataTable GetListByProjectId(int project_id,string wx_og_id="")
        {
            return DbHelperSQLModule.Query("select *,(select COUNT(*) from t_md_360view where p_id=t_md_360viewtype.id) as fullview_count,(select top 1 id from t_md_360view  where p_id=t_md_360viewtype.id order by id asc) as full_view_id   from t_md_360viewtype where pid=" + project_id + " " + (wx_og_id == "" ? "" : " and wx_og_id='"+wx_og_id+"'") + " order by id desc").Tables[0];
        }

        /// <summary>
        /// 获取全景列表
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="wx_id"></param>
        /// <returns></returns>
        public static DataTable GetFullViewListByIds(string ids, int wx_id)
        {
            return DbHelperSQLModule.Query("select id,title,showimg,(select name from t_md_360viewtype where id=p_id) as room_type_name ,(select wx_og_id from kd_wechats.dbo.t_wx_wechats where id=" + wx_id + ") as wx_og_id  from  t_md_360view where p_id in(" + ids + ") order by id asc").Tables[0];
        }
      

        /// <summary>
        /// 根据所属菜单ID提取1条数据
        /// </summary>
        /// <param name="id">M_id</param>
        /// <returns></returns>
        public static t_md_360viewtype GetModelByM_id(int m_id)
        {
            t_md_360viewtype model = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                model = db.t_md_360viewtype.Where(x => x.id == m_id).FirstOrDefault();
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
                isFinish = db.t_md_360viewtype.Where(x => x.id == id).Delete() > 0;
            }
            return isFinish;
        }

        /// <summary>
        /// 添加一条信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_md_360viewtype Add(t_md_360viewtype model)
        {

            model = EFHelper.AddModule<t_md_360viewtype>(model);
            return model;//返回添加后的信息
        }

        /// <summary>
        /// 修改一条信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_md_360viewtype Update(t_md_360viewtype model)
        {
            return EFHelper.UpdateModule<t_md_360viewtype>(model);
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_360viewtype> GetListForPro(int p_id, string wx_og_id)
        {
            List<t_md_360viewtype> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                var query = (from x in db.t_md_360viewtype where x.pid==p_id &&  x.wx_og_id == wx_og_id orderby x.sort_id select x);
                list = query.ToList();

            }
            return list;
        }
     
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_360viewtype> GetList(int p_id,  string wx_og_id)
        {
            List<t_md_360viewtype> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                var query = (from x in db.t_md_360viewtype where  x.pid == p_id &&  x.wx_og_id == wx_og_id select x);
                list = query.ToList();

            }
            return list;
        }

        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_360viewtype> GetListForName(int p_id, string name, string wx_og_id)
        {
            List<t_md_360viewtype> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                var query = (from x in db.t_md_360viewtype where x.pid == p_id && x.name==name && x.wx_og_id == wx_og_id select x);
                list = query.ToList();

            }
            return list;
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_360viewtype> GetList( string wx_og_id, int pagesize, int pageindex, out int count)
        {
            List<t_md_360viewtype> list = null;
            List<t_md_360viewtype> listForCount = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                list = (from x in db.t_md_360viewtype where  x.wx_og_id == wx_og_id orderby x.id select x).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                listForCount = (from x in db.t_md_360viewtype where  x.wx_og_id == wx_og_id orderby x.id select x).ToList();
                count = listForCount.Count();

            }
            return list;
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_360viewtype> GetListForPid(int p_id,  string wx_og_id, int pagesize, int pageindex, out int count)
        {
            List<t_md_360viewtype> list = null;
            List<t_md_360viewtype> listForCount = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                list = (from x in db.t_md_360viewtype where x.pid == p_id  && x.wx_og_id == wx_og_id orderby x.id select x).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                listForCount = (from x in db.t_md_360viewtype where x.pid == p_id  && x.wx_og_id == wx_og_id orderby x.id select x).ToList();
                count = listForCount.Count();

            }
            return list;
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_360viewtype> GetListForList(int id, string wx_og_id)
        {
            List<t_md_360viewtype> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                var query = (from x in db.t_md_360viewtype where x.id == id  && x.wx_og_id == wx_og_id select x);
                list = query.ToList();

            }
            return list;
        }
        public static List<t_md_360viewtype> GetMaxID()
        {
            List<t_md_360viewtype> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {

                var query = (from x in db.t_md_360viewtype select x);
                list = query.ToList();
            }
            return list;
        }

    }
}
