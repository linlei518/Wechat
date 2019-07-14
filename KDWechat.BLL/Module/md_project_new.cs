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
    public  class md_project_new
    {
        /// <summary>
        /// 根据微信id获取项目列表（字段只包含：id和title）
        /// </summary>
        /// <param name="wx_id"></param>
        /// <returns></returns>
        public static DataTable GetListByWxid(int wx_id = 0)
        {
            return DbHelperSQLWechats.Query("select * from kd_wechats.dbo.t_projects_new where status=1").Tables[0];
        }

        public static DataTable GetListByWxid(string id)
        {
            return DbHelperSQLWechats.Query("select * from kd_wechats.dbo.t_projects_new where status=1 and id in (" + id + ")").Tables[0];
        }

        public static t_projects_new GetModel(int id, int status = -1)
        {
            t_projects_new model = null;
            using (kd_wechatsEntities db = new kd_wechatsEntities())
            {
                if (status > -1)
                {
                    model = (from x in db.t_projects_new where x.id == id && x.status == status select x).FirstOrDefault();
                }
                else
                {
                    model = (from x in db.t_projects_new where x.id == id select x).FirstOrDefault();
                }

            }
            return model;
        }

        /// <summary>
        /// 用于预约管理根据title进行查找
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static t_projects_new GetModel(string title)
        {
            t_projects_new model = null;
            using (kd_wechatsEntities db = new kd_wechatsEntities())
            {
                model = (from x in db.t_projects_new where x.title == title select x).FirstOrDefault();
            }
            return model;
        }
    }
}
