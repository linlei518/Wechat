using KDWechat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KDWechat.BLL.Users
{
    public class wx_tags_relation
    {
        public static List<wx_tags_relation_View> GetRelationViewByFanID(int fanID)
        {
            List<wx_tags_relation_View> list = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                list = db.wx_tags_relation_View.Where(x => x.fans_id == fanID).ToList();
            }
            return list;
        }
    }
}
