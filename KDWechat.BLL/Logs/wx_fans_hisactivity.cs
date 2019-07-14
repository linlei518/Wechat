using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KDWechat.DAL;
using EntityFramework.Extensions;

namespace KDWechat.BLL.Logs
{
    public class wx_fans_hisactivity
    {
        public static t_wx_fans_hisactivity GetFansHisactivityByID(int id)
        {
            t_wx_fans_hisactivity tag = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                tag = (from x in db.t_wx_fans_hisactivity where x.id == id select x).FirstOrDefault();
            }
            return tag;
        }


        public static t_wx_fans_hisactivity CreateFansHisactivity(t_wx_fans_hisactivity tag)
        {
            return EFHelper.AddLog<t_wx_fans_hisactivity>(tag);
        }

        public static bool DeleteFansHisactivityByID(int id)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isFinish = db.t_wx_fans_hisactivity.Where(x => x.id == id).Delete() > 0;
            }
            return isFinish;
        }

        public static bool UpdateFansHisactivity(t_wx_fans_hisactivity tag)
        {
            return EFHelper.UpdateLog<t_wx_fans_hisactivity>(tag);
        }

    }
}
