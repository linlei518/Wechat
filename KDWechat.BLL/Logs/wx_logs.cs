using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using KDWechat.DBUtility;
using KDWechat.DAL;
using EntityFramework.Extensions;
using System.Linq.Expressions;
using LinqKit;

namespace KDWechat.BLL.Logs
{
    public class wx_logs
    {
        public static KDWechat.DAL.t_wx_logs Add(KDWechat.DAL.t_wx_logs model)
        {
            model = KDWechat.DAL.EFHelper.AddLog<KDWechat.DAL.t_wx_logs>(model);
            return model;
        }

        public static t_wx_logs GetModel<T>(Expression<Func<t_wx_logs,bool>> where,Expression<Func<t_wx_logs,T>> orderBy,bool orderByDesc = false)
        {
            t_wx_logs logToRegurn = null;
            using(creater_wxEntities db = new creater_wxEntities())
            {
                if (!orderByDesc)
                    logToRegurn = db.t_wx_logs.Where(where.Expand()).OrderBy(orderBy).FirstOrDefault();
                else
                    logToRegurn = db.t_wx_logs.Where(where.Expand()).OrderByDescending(orderBy).FirstOrDefault();
            }
            return logToRegurn;
        }


        public static t_wx_logs GetWxLogsByID(int id)
        {
            t_wx_logs tag = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                tag = (from x in db.t_wx_logs where x.id == id select x).FirstOrDefault();
            }
            return tag;
        }


        public static t_wx_logs CreateWxLog(t_wx_logs tag)
        {
            return EFHelper.AddLog<t_wx_logs>(tag);
        }

        public static bool DeleteWxLogByID(int id)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isFinish = db.t_wx_logs.Where(x => x.id == id).Delete() > 0;
            }
            return isFinish;
        }

        public static bool UpdateWxLog(t_wx_logs tag)
        {
            return EFHelper.UpdateLog<t_wx_logs>(tag);
        }

    }
}

