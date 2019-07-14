using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KDWechat.DAL;
using EntityFramework.Extensions;
namespace KDWechat.BLL.Users
{
    /// <summary>
    /// 会员与粉丝的关系表
    /// </summary>
    public class member_fans
    {
        public static t_member_fans GetModel(string unionid, string fans_guid)
        {
            t_member_fans tag = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                tag = (from x in db.t_member_fans where x.unionid == unionid && x.fans_guid==fans_guid select x).FirstOrDefault();
            }
            return tag;
        }
      
        public static t_member_fans Add(t_member_fans tag)
        {
            t_member_fans model = GetModel(tag.unionid,tag.fans_guid);
            if (model==null)
            {
                model=EFHelper.AddUser<t_member_fans>(tag);
            }
            return model;
        }

        public static bool Delete(int id)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isFinish = db.t_member_fans.Where(x => x.id == id).Delete() > 0;
            }
            return isFinish;
        }

        public static bool Update(t_member_fans tag)
        {
            return EFHelper.UpdateUser<t_member_fans>(tag);
        }
    }
}
