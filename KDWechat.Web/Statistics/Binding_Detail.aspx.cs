using KDWechat.BLL.Chats;
using KDWechat.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.Statistics
{
    public partial class Binding_Detail : Web.UI.BasePage
    {
        protected int id { get { return RequestHelper.GetQueryInt("id", 0); } }
        protected void Page_Load(object sender, EventArgs e)
        {

            CheckUserAuthority("binding_list");
        }

        protected string GetLastTime(int tp)
        {
            if (id != 0)
            {
                List<BLL.Chats.TimeStatistics<int>> chatTimeList = null;
                TimeStatistics<int> last = null;
                switch (tp)
                {
                    case 1:
                        chatTimeList = CacheHelper.Get<List<BLL.Chats.TimeStatistics<int>>>("s_chatTimeList");
                        last = chatTimeList.Where(x => x.key == id).FirstOrDefault();
                        if (last != null)
                            return last.last_time.ToString();
                        break;
                    case 2:
                        chatTimeList = CacheHelper.Get<List<BLL.Chats.TimeStatistics<int>>>("s_viewTimeList");
                        last = chatTimeList.Where(x => x.key == id).FirstOrDefault();
                        if (last != null)
                            return last.last_time.ToString();
                        break;
                    case 3:
                        chatTimeList = CacheHelper.Get<List<BLL.Chats.TimeStatistics<int>>>("s_locationTimeList");
                        last = chatTimeList.Where(x => x.key == id).FirstOrDefault();
                        if (last != null)
                            return last.last_time.ToString();
                        break;
                    case 4:
                        chatTimeList = CacheHelper.Get<List<BLL.Chats.TimeStatistics<int>>>("s_fansTimeList");
                        last = chatTimeList.Where(x => x.key == id).FirstOrDefault();
                        if (last != null)
                            return last.last_time.ToString();
                        break;
                }
            }
            return "无";
        }
    }
}