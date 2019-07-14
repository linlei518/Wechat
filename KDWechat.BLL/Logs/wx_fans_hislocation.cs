using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KDWechat.DAL;
using EntityFramework.Extensions;

namespace KDWechat.BLL.Logs
{
    public class wx_fans_hislocation
    {
        public static t_wx_fans_hislocation GetFansHislocationsByID(int id)
        {
            t_wx_fans_hislocation tag = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                tag = (from x in db.t_wx_fans_hislocation where x.id == id select x).FirstOrDefault();
            }
            return tag;
        }


        public static t_wx_fans_hislocation GetFansHislocationsByOpenID(string open_id)
        {
            t_wx_fans_hislocation tag = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                tag = (from x in db.t_wx_fans_hislocation where x.open_id == open_id select x).OrderByDescending(x=>x.id).Take(1).FirstOrDefault();
            }
            return tag;
        }

        public static t_wx_fans_hislocation CreateFansHislocation(t_wx_fans_hislocation tag)
        {
            return EFHelper.AddLog<t_wx_fans_hislocation>(tag);
        }

        public static bool DeleteFansHislocationByID(int id)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isFinish = db.t_wx_fans_hislocation.Where(x => x.id == id).Delete() > 0;
            }
            return isFinish;
        }

        public static bool UpdateFansHislocation(t_wx_fans_hislocation tag)
        {
            return EFHelper.UpdateLog<t_wx_fans_hislocation>(tag);
        }


    }
}
