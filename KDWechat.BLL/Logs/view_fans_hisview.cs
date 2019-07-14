using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KDWechat.BLL.Logs
{
    /// <summary>
    /// 用户的浏览行为记录
    /// </summary>
    public class view_fans_hisview
    {
        /// <summary>
        /// 根据openId获取用户的浏览行为记录
        /// </summary>
        /// <param name="openid">1、浏览数 2、点赞数 3、分享数</param>
        /// <returns></returns>
        public static List<KDWechat.DAL.view_fans_hisview> GetListbyOpenId(string openid,int type_id=1)
        {
            List<KDWechat.DAL.view_fans_hisview> list = null;
            using (KDWechat.DAL.creater_wxEntities db=new DAL.creater_wxEntities())
            {
                list=(from x in db.view_fans_hisview where x.open_id==openid && x.type_id==type_id orderby x.view_time descending select x ).ToList();
            }
            return list;
        }
    }
}
