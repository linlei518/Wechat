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
    public class md_GreetingCards_views
    {
        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static t_md_GreetingCard_views GetModel(int id)
        {
            t_md_GreetingCard_views model = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                model = db.t_md_GreetingCard_views.Where(x => x.id == id).FirstOrDefault();
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
                isFinish = db.t_md_GreetingCard_views.Where(x => x.id == id).Delete() > 0;
            }
            return isFinish;
        }


        /// <summary>
        /// 添加一条信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_md_GreetingCard_views Add(t_md_GreetingCard_views model)
        {

            model = EFHelper.AddModule<t_md_GreetingCard_views>(model);
            return model;//返回添加后的信息
        }

        //简单的增删改查可在后台
        //model = EFHelper.AddModel<kd_moduleEntities, t_md_GreetingCard_views>(model);

        //int i;
        //List<t_md_GreetingCard_views> list = EFHelper.GetList<kd_moduleEntities, t_md_GreetingCard_views, int>(x => x.id == 1, x => x.id, int.MaxValue, 1, out i, true);

        /// <summary>
        /// 修改一条信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_md_GreetingCard_views Update(t_md_GreetingCard_views model)
        {
            return EFHelper.UpdateModule<t_md_GreetingCard_views>(model);
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_GreetingCard_views> GetList(string from_openId, string view_openId, int pagesize, int pageindex, out int count)
        {
            List<t_md_GreetingCard_views> list = null;
            List<t_md_GreetingCard_views> listForCount = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {

                list = (from x in db.t_md_GreetingCard_views where x.From_openId == from_openId && x.View_openId == view_openId orderby x.id select x).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                listForCount = (from x in db.t_md_GreetingCard_views where x.From_openId == from_openId && x.View_openId == view_openId orderby x.id select x).ToList();



                count = listForCount.Count();

            }
            return list;
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_GreetingCard_views> GetList(int cardID, string from_openId, string view_openId, int pagesize, int pageindex, out int count)
        {
            List<t_md_GreetingCard_views> list = null;
            List<t_md_GreetingCard_views> listForCount = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {

                list = (from x in db.t_md_GreetingCard_views where x.cardID == cardID && x.From_openId == from_openId && x.View_openId == view_openId orderby x.id select x).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                listForCount = (from x in db.t_md_GreetingCard_views where x.cardID == cardID && x.From_openId == from_openId && x.View_openId == view_openId orderby x.id select x).ToList();



                count = listForCount.Count();

            }
            return list;
        }
      
      

        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_GreetingCard_views> GetList(int cardID,string from_openId, string view_openId)
        {
            List<t_md_GreetingCard_views> list = null;

            using (kd_moduleEntities db = new kd_moduleEntities())
            {

                list = (from x in db.t_md_GreetingCard_views where x.cardID == cardID && x.From_openId == from_openId && x.View_openId == view_openId orderby x.id descending, x.id descending select x).ToList();

            }
            return list;
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_GreetingCard_views> GetList(int cardID, string from_openId)
        {
            List<t_md_GreetingCard_views> list = null;

            using (kd_moduleEntities db = new kd_moduleEntities())
            {

                list = (from x in db.t_md_GreetingCard_views where x.cardID == cardID && x.From_openId == from_openId orderby x.id descending, x.id descending select x).ToList();

            }
            return list;
        }
        /// <summary>
        /// 根据贺卡ID获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_GreetingCard_views> GetList(int cardID)
        {
            List<t_md_GreetingCard_views> list = null;

            using (kd_moduleEntities db = new kd_moduleEntities())
            {

                list = (from x in db.t_md_GreetingCard_views where x.cardID == cardID  orderby x.id descending, x.id descending select x).ToList();

            }
            return list;
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static DataTable GetList()
        {
            return DbHelperSQLModule.Query("select distinct View_openId from t_md_GreetingCard_views").Tables[0];
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_GreetingCard_views> GetListView()
        {
            List<t_md_GreetingCard_views> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                var query = (from x in db.t_md_GreetingCard_views select x);
                list = query.ToList();

            }
            return list;
        }



        public static List<t_md_GreetingCard_views> GetMaxID()
        {
            List<t_md_GreetingCard_views> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {

                var query = (from x in db.t_md_GreetingCard_views select x);
                list = query.ToList();
            }
            return list;
        }


    }

}


