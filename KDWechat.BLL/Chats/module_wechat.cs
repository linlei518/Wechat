using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityFramework.Extensions;
using KDWechat.DAL;
using System.Linq.Expressions;
using System.Data;
using KDWechat.DBUtility;
using KDWechat.Common;
using LinqKit;


namespace KDWechat.BLL.Chats
{
    public class module_wechat
    {
        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static t_module_wechat GetModel(int id)
        {
            t_module_wechat material = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                material = db.t_module_wechat.Where(x => x.ID == id).FirstOrDefault();
            }
            return material;
        }

        /// <summary>
        /// 提取module_wechat
        /// </summary>
        /// <param name="wxID"></param>
        /// <param name="moduleID"></param>
        /// <returns></returns>
        public static t_module_wechat GetModelByWxIDAndModuleID(int wxID, int moduleID)
        {
            t_module_wechat material = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                material = db.t_module_wechat.Where(x => x.wx_id == wxID && x.module_id == moduleID).FirstOrDefault();
            }
            return material;
        }

        /// <summary>
        /// 删除一条消息
        /// </summary>
        /// <param name="id">消息ID</param>
        /// <returns>是否删除成功</returns>
        public static t_module_wechat Delete(int id)
        {
            t_module_wechat isFinish = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isFinish = db.t_module_wechat.Where(x => x.ID == id).FirstOrDefault();

                if (isFinish != null)
                {
                    isFinish.status = (int)Status.禁用;
                    db.SaveChanges();
                }
            }
            return isFinish;
        }

        /// <summary>
        /// 移除一个模块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static t_module_wechat Remove(int id)
        {
            t_module_wechat isFinish = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isFinish = db.t_module_wechat.Where(x => x.ID == id).FirstOrDefault();
                isFinish.status = (int)Status.禁用;
                db.SaveChanges();
            }
            return isFinish;
        }

        /// <summary>
        /// 添加一条消息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_module_wechat Add(t_module_wechat model)
        {

            model = EFHelper.AddWeChat<t_module_wechat>(model);
            return model;//返回添加后的消息
        }

        /// <summary>
        /// 修改一条消息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_module_wechat Update(t_module_wechat model)
        {
            if (model != null)
            {
                EFHelper.UpdateWeChat<t_module_wechat>(model);
            }
            return model;
        }

        //提取列表
        public static List<t_module_wechat> GetList(Expression<Func<t_module_wechat, bool>> where, int pagesize, int pageindex, out int totalCount)
        {
            List<t_module_wechat> list = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                var query = db.t_module_wechat.Where(where.Expand());
                totalCount = query.Count();
                list = query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            }
            return list;

        }

        public static DataTable GetListByQuery(string query)
        {

            DataSet ds = KDWechat.DBUtility.DbHelperSQL.Query(query);
            DataTable dt = ds.Tables[0] as DataTable;
            return dt;
        }

        /// <summary>
        /// 添加禁用模块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static t_module_wechat RemoveOrAddModule(int id)
        {
            t_module_wechat module = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                module = (from x in db.t_module_wechat where x.ID == id select x).FirstOrDefault();
                if (module != null)
                {
                    if (module.status == (int)Common.Status.禁用)
                        module.status = (int)Common.Status.正常;
                    else
                        module.status = (int)Common.Status.禁用;
                    db.SaveChanges();
                }
            }
            return module;
        }

        #region 360全景（添加，修改，删除）
        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="app_id">app_id</param>
        /// <param name="app_parent_id">app_parent_id</param>
        /// <param name="app_table">app_table</param>
        /// <returns></returns>
        public static t_module_wechat GetModelForViewBuid(int wx_id, int app_id, int app_parent_id, string app_table)
        {
            t_module_wechat material = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                material = db.t_module_wechat.Where(x => x.wx_id == wx_id && x.app_id == app_id && x.app_parent_id == app_parent_id && x.app_table == app_table).FirstOrDefault();
            }
            return material;
        }
        /// <summary>
        /// 删除一条信息
        /// </summary>
        /// <param name="app_id">app_id</param>
        /// <param name="app_parent_id">app_parent_id</param>
        /// <param name="app_table">app_table</param>
        /// <returns>是否删除成功</returns>
        public static bool DeleteForBuid(int app_id, int app_parent_id, string app_table)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isFinish = db.t_module_wechat.Where(x => x.app_id == app_id && x.app_parent_id == app_parent_id && x.app_table == app_table).Delete() > 0;
            }
            return isFinish;
        }

        #endregion


        public static DataTable GetAllList(int wx_id, int parent_id, int level)
        {
            string sql = "select app_id,app_parent_id,app_table,app_name,app_img_url,app_link_url, '' as app_all_name,1 as 'level',create_time from t_module_wechat where  wx_id=" + wx_id + " and status=1 and channel_id=1 and  module_id=2";
            sql += " and app_parent_id=" + parent_id;
            if (level == 1)
            {
                sql += "   and app_table='t_projects' ";
            }

            else if (level == 2)
            {
                sql += " and app_table='t_md_360viewtype'";

            }
            else if (level == 3)
            {
                sql += " and app_table='t_md_360view'";

            }


            DataTable oldData = KDWechat.DBUtility.DbHelperSQL.Query(sql).Tables[0];


            return oldData;
        }
        public static DataTable GetAllListForView360(int wx_id)
        {
            string sql = "select id as  app_id,'0' as app_parent_id,'' as  app_table,title as app_name, '' as app_img_url, '' as app_link_url, title as app_all_name,1 as 'level',create_time  from t_projects_new where id in(select app_parent_id from t_module_wechat where app_table='t_md_360viewtype' and wx_id=" + wx_id + ")";

            DataTable oldData = KDWechat.DBUtility.DbHelperSQL.Query(sql).Tables[0];


            return oldData;
        }

        public static DataTable Get360List(int wx_id, int project_id)
        {

            DataTable oldData = KDWechat.DBUtility.DbHelperSQL.Query("select app_id,app_parent_id,app_table,app_name,app_img_url,app_link_url, '' as app_all_name,1 as 'level',create_time from t_module_wechat where  wx_id=" + wx_id + " and status=1 and channel_id=1 and  module_id=2 and app_id in(select id from  kd_module.dbo.t_md_360viewtype where pid=" + project_id + " union select " + project_id + " union select app_id from  t_module_wechat  where app_parent_id in(select app_id from  t_module_wechat  where app_parent_id=" + project_id + "))").Tables[0];


            return oldData;
        }

        public static DataTable Get360List(int wx_id, int project_id, int level)
        {

            string sql = "select app_id,app_parent_id,app_table,app_name,app_img_url,app_link_url,'' as app_all_name,1 as 'level',create_time from t_module_wechat where  wx_id=" + wx_id + " and status=1 and channel_id=1 and  module_id=2";
            if (level == 1)
            {
                sql += " and app_id=" + project_id + " and app_table='t_projects' ";
            }
            else
            {
                sql += " and app_parent_id=" + project_id + " ";
                if (level == 2)
                {
                    sql += " and app_table='t_md_360viewtype'";

                }
                else if (level == 3)
                {
                    sql += " and app_table='t_md_360view'";

                }
            }
            DataTable oldData = KDWechat.DBUtility.DbHelperSQL.Query(sql).Tables[0];


            return oldData;
        }

    }
}
