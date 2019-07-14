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
    public class md_news_type
    {
        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static  t_md_news_type GetModel(int id)
        {
            t_md_news_type model = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                model = db.t_md_news_type.Where(x => x.id == id).FirstOrDefault();
            }
            return model;
        }
        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static t_md_news_type GetModel(string name,int wx_id,string wx_og_id)
        {
            t_md_news_type model = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                model = db.t_md_news_type.Where(x => x.name == name && x.wx_id== wx_id && x.wx_og_id== x.wx_og_id).FirstOrDefault();
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
                isFinish = db.t_md_news_type.Where(x => x.id == id).Delete() > 0;
            }
            return isFinish;
        }


        /// <summary>
        /// 添加一条信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_md_news_type Add(t_md_news_type model)
        {

            model = EFHelper.AddModule<t_md_news_type>(model);
            return model;//返回添加后的信息
        }

        //简单的增删改查可在后台
        //model = EFHelper.AddModel<kd_moduleEntities, t_md_news_type>(model);

        //int i;
        //List<t_md_news_type> list = EFHelper.GetList<kd_moduleEntities, t_md_news_type, int>(x => x.id == 1, x => x.id, int.MaxValue, 1, out i, true);

        /// <summary>
        /// 修改一条信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_md_news_type Update(t_md_news_type model)
        {
            return EFHelper.UpdateModule<t_md_news_type>(model);
        }
        /// <summary>
        /// 修改IsBook
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int  Update_2(string str,int wx_id,string wx_og_id)
        {
            return DbHelperSQLModule.ExecuteSql("update t_md_news_type set isBook=2 where id in ("+str+") and wx_id="+wx_id+" and wx_og_id='"+wx_og_id+"'");
        }
        public static int Update_1(string str, int wx_id, string wx_og_id)
        {
            return DbHelperSQLModule.ExecuteSql("update t_md_news_type set isBook=1 where id not in (" + str + ") and wx_id=" + wx_id + " and wx_og_id='" + wx_og_id + "'");
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_news_type> GetList(int wx_id, string wx_og_id)
        {
            List<t_md_news_type> list = null;

            using (kd_moduleEntities db = new kd_moduleEntities())
            {

                list = (from x in db.t_md_news_type where  x.wx_id == wx_id && x.wx_og_id == wx_og_id orderby x.id select x).ToList();

            }
            return list;
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_news_type> GetList(int wx_id, string wx_og_id,int isbook)
        {
            List<t_md_news_type> list = null;

            using (kd_moduleEntities db = new kd_moduleEntities())
            {

                list = (from x in db.t_md_news_type where x.wx_id == wx_id && x.wx_og_id == wx_og_id  && isbook==2 orderby x.id select x).ToList();

            }
            return list;
        }
        public static DataTable GetlistForBook(int wx_id, string wx_og_id, int isbook)
        {
            return DbHelperSQLModule.Query("select * from t_md_news_type where isbook="+isbook+" and wx_id="+wx_id+" and wx_og_id='"+wx_og_id+"'").Tables[0];
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_news_type> GetList(string name,int wx_id, string wx_og_id)
        {
            List<t_md_news_type> list = null;

            using (kd_moduleEntities db = new kd_moduleEntities())
            {

                list = (from x in db.t_md_news_type where x.name==name && x.wx_id == wx_id && x.wx_og_id == wx_og_id orderby x.id select x).ToList();

            }
            return list;
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_news_type> GetListView()
        {
            List<t_md_news_type> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                var query = (from x in db.t_md_news_type select x);
                list = query.ToList();

            }
            return list;
        }



        public static List<t_md_news_type> GetMaxID()
        {
            List<t_md_news_type> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {

                var query = (from x in db.t_md_news_type select x);
                list = query.ToList();
            }
            return list;
        }


    }

}

