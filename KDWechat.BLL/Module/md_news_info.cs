using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KDWechat.DAL;
using EntityFramework.Extensions;
using KDWechat.Common;
using Companycn.Core.DbHelper;
using System.Data.SqlClient;
using System.Data;

namespace KDWechat.BLL.Module
{
    public  class md_news_info
    {/// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static t_md_news_info GetModel(int id)
        {
            t_md_news_info model = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                model = db.t_md_news_info.Where(x => x.id == id).FirstOrDefault();
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
                isFinish = db.t_md_news_info.Where(x => x.id == id).Delete() > 0;
            }
            return isFinish;
        }


        /// <summary>
        /// 添加一条信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_md_news_info Add(t_md_news_info model)
        {

            model = EFHelper.AddModule<t_md_news_info>(model);
            return model;//返回添加后的信息
        }

        //简单的增删改查可在后台
        //model = EFHelper.AddModel<kd_moduleEntities, t_md_news_info>(model);

        //int i;
        //List<t_md_news_info> list = EFHelper.GetList<kd_moduleEntities, t_md_news_info, int>(x => x.id == 1, x => x.id, int.MaxValue, 1, out i, true);

        /// <summary>
        /// 修改一条信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_md_news_info Update(t_md_news_info model)
        {
            return EFHelper.UpdateModule<t_md_news_info>(model);
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_news_info> GetList(int wx_id, string wx_og_id, int pagesize, int pageindex, out int count)
        {
            List<t_md_news_info> list = null;
            List<t_md_news_info> listForCount = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {

                list = (from x in db.t_md_news_info where x.wx_id == wx_id && x.wx_og_id == wx_og_id orderby x.id select x).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                listForCount = (from x in db.t_md_news_info where  x.wx_id == wx_id && x.wx_og_id == wx_og_id orderby x.id select x).ToList();



                count = listForCount.Count();

            }
            return list;
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_news_info> GetList(int typeid,int wx_id, string wx_og_id, int pagesize, int pageindex, out int count)
        {
            List<t_md_news_info> list = null;
            List<t_md_news_info> listForCount = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {

                list = (from x in db.t_md_news_info where x.type_id==typeid && x.wx_id == wx_id && x.wx_og_id == wx_og_id orderby x.id select x).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                listForCount = (from x in db.t_md_news_info where x.type_id == typeid && x.wx_id == wx_id && x.wx_og_id == wx_og_id orderby x.id select x).ToList();



                count = listForCount.Count();

            }
            return list;
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_news_info> GetList(int wx_id, string wx_og_id)
        {
            List<t_md_news_info> list = null;

            using (kd_moduleEntities db = new kd_moduleEntities())
            {

                list = (from x in db.t_md_news_info where x.wx_id == wx_id && x.wx_og_id == wx_og_id orderby x.id select x).ToList();

            }
            return list;
        }
        public static DataTable GetlistForType(int wx_id, string wx_og_id)
        {
            return DbHelperSQLModule.Query(" select a.* from t_md_news_info as a inner join t_md_news_users as b on a.type_id=b.id where b.isBook=2 order by Istop desc,id desc  ").Tables[0];
        }
        
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_news_info> GetList(int typeid,int wx_id, string wx_og_id)
        {
            List<t_md_news_info> list = null;

            using (kd_moduleEntities db = new kd_moduleEntities())
            {

                list = (from x in db.t_md_news_info where x.type_id==typeid && x.wx_id == wx_id && x.wx_og_id == wx_og_id orderby x.Istop descending, x.id descending select x).ToList();

            }
            return list;
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_news_info> GetListForTop(int Istop,int type_id, int wx_id, string wx_og_id)
        {
            List<t_md_news_info> list = null;

            using (kd_moduleEntities db = new kd_moduleEntities())
            {

                list = (from x in db.t_md_news_info where x.Istop == Istop && x.type_id==type_id && x.wx_id == wx_id && x.wx_og_id == wx_og_id orderby x.id descending select x).ToList();

            }
            return list;
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_news_info> GetListView()
        {
            List<t_md_news_info> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                var query = (from x in db.t_md_news_info select x);
                list = query.ToList();

            }
            return list;
        }



        public static List<t_md_news_info> GetMaxID()
        {
            List<t_md_news_info> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {

                var query = (from x in db.t_md_news_info select x);
                list = query.ToList();
            }
            return list;
        }


    }

}


