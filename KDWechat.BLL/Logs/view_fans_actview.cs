using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KDWechat.BLL.Logs
{
    /// <summary>
    /// 用户参与活动记录
    /// </summary>
    public class view_fans_actview
    { 
        
        /// <summary>
        /// 根据openId获取用户的浏览行为记录
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public static List<KDWechat.DAL.view_fans_actview> GetListbyOpenId(string openid)
        {
            List<KDWechat.DAL.view_fans_actview> list = null;
            using (KDWechat.DAL.creater_wxEntities db = new DAL.creater_wxEntities())
            {
                list = (from x in db.view_fans_actview where x.open_id == openid orderby x.join_time descending select x).ToList();
            }
            return list;
        }
    }
}
