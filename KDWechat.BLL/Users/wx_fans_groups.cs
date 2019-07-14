using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using KDWechat.DAL;
using EntityFramework.Extensions;

namespace KDWechat.BLL.Users
{
    public class wx_fans_groups
    {
        public static t_wx_fans_groups GetFansGroupsByID(int id)
        {
            t_wx_fans_groups tag = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                tag = (from x in db.t_wx_fans_groups where x.id == id select x).FirstOrDefault();
            }
            return tag;
        }





        public static t_wx_fans_groups CreateFansGroup(t_wx_fans_groups tag)
        {
            return EFHelper.AddUser<t_wx_fans_groups>(tag);
        }

        public static bool DeleteFansGroupByID(int id)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isFinish = db.t_wx_fans_groups.Where(x => x.id == id).Delete() > 0;
            }
            return isFinish;
        }

        public static bool UpdateFansGroup(t_wx_fans_groups tag)
        {
            return EFHelper.UpdateUser<t_wx_fans_groups>(tag);
        }

    }
}
