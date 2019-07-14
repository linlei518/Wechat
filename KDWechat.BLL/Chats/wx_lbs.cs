using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KDWechat.DAL;

namespace KDWechat.BLL.Chats
{
    public class wx_lbs
    {
        /// <summary>
        /// 向本地数据库插入一条位置信息
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static t_wx_lbs InsertLbs(t_wx_lbs location)
        {
            return EFHelper.AddWeChat<t_wx_lbs>(location);
        }

        public static List<t_wx_lbs> GetList(int wxID,int pagesize, int pageindex,out int count)
        {
            List<t_wx_lbs> list = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                var query = (from x in db.t_wx_lbs where x.wx_id==wxID orderby x.ID descending select x);
                list = query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                count = query.Count();
            }
            return list;
        }

        public static bool DeleteLbsByID(int id)
        {
            bool isComplete = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                var lbsToDelete = db.t_wx_lbs.Where(x => x.ID == id).FirstOrDefault();
                if (null != lbsToDelete)
                {
                    bool deleteLbs = BaiDuMapAPI.BaiDuLBS.DeletePoint(lbsToDelete.baidu_id.ToString());
                    db.t_wx_lbs.Remove(lbsToDelete);
                    isComplete = db.SaveChanges() > 0 && deleteLbs;
                }
            }
            return isComplete;

        }


        public static t_wx_lbs GetLBSByID(int id)
        {
            t_wx_lbs lbs = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                lbs = (from x in db.t_wx_lbs where x.ID == id select x).FirstOrDefault();
            }
            return lbs;
        }


        /// <summary>
        /// 更新本地数据库中的位置信息
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static t_wx_lbs UpdateLbs(t_wx_lbs location)
        {
            return EFHelper.UpdateWeChat<t_wx_lbs>(location);
        }
    }
}
