using KDWechat.BLL.Chats;
using KDWechat.Common;
using KDWechat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.Statistics
{
    public partial class users_bydate : Web.UI.BasePage
    {
        protected string userChartData = "";                       //累计用户数据
        protected string subChartData = "";                    //一周关注用户数据
        protected string unsubChartData = "";              //一周取消关注用户数据
        protected string userIncreaseData = "";              //一周用户净增长数据
        protected string barChartData = "";                          //条形图数据
        protected DateTime startTime;
        protected DateTime endTime;
        protected List<DAL.t_wx_wechats> wxList = null;
        protected List<BLL.Chats.CountStatistics<int>> suscribtList = null;
        protected List<BLL.Chats.CountStatistics<int>> unSuscribtList = null;
        protected string IDs = RequestHelper.GetQueryString("Ids");
        protected int retainHeight = 160;                          //图表保留高度
        protected int otherHeght = 0;                          //其他图文图表高度


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckUserAuthority("sys_dashboard");
                InitData();//初始化数据
            }
        }

        private void InitData()
        {
            startTime = Utils.StrToDateTime(txtbegin_date.Text, DateTime.Now.AddDays(-7));
            endTime = Utils.StrToDateTime(txtend_date.Text, DateTime.Now);
            Expression<Func<t_wx_wechats, bool>> wx_list_where = x => x.status == (int)Status.正常;
            if (!string.IsNullOrWhiteSpace(IDs))
            {
                var idArray = IDs.Split(',');
                if (idArray.Length != 0)
                {
                    var wx_ids = new int[idArray.Length];
                    for (int i = 0; i < wx_ids.Length; i++)
                    {
                        wx_ids[i] = Utils.StrToInt(idArray[i], 0);
                    }
                    wx_list_where = wx_list_where.And(x => wx_ids.Contains(x.id));
                }
            }
            wxList = Companycn.Core.EntityFramework.EFHelper.GetList<creater_wxEntities, t_wx_wechats, int>(wx_list_where, x => x.id, int.MaxValue, 1);
            otherHeght = retainHeight + wxList.Count * 61;
            suscribtList = Companycn.Core.EntityFramework.EFHelper.GetGroupBy<creater_wxEntities, t_wx_fans, int, CountStatistics<int>>(x => x.subscribe_time >= startTime&&x.subscribe_time<=endTime, x => x.wx_id, x => new CountStatistics<int> { count = x.Count(), key = x.Key });
            unSuscribtList = Companycn.Core.EntityFramework.EFHelper.GetGroupBy<creater_wxEntities, t_wx_fans, int, CountStatistics<int>>(x => x.remove_time >= startTime && x.remove_time <= endTime, x => x.wx_id, x => new CountStatistics<int> { count = x.Count(), key = x.Key });
            BindUsersBar();
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            InitData();
        }


        private void BindUsersBar()
        {
            if (wxList.Count > 0)
            {

                StringBuilder strJsonName = new StringBuilder();
                StringBuilder strJsonUser = new StringBuilder();
                StringBuilder strJsonSub = new StringBuilder();
                StringBuilder strJsonNoSub = new StringBuilder();

                foreach (var wechat in wxList)
                {
                    strJsonName.Append("'" + wechat.wx_pb_name + "'" + ",");

                    int Fcount = BLL.Users.wx_fans.GetTotalCountByWxID(wechat.id);
                    var Scount = GetSCount(wechat.id);//内存中取数据--Damos
                    var Uscount = GetUSCount(wechat.id);//内存中取数据--Damos
                    strJsonUser.Append(Fcount.ToString() + ",");
                    strJsonSub.Append((Scount) + ",");
                    strJsonNoSub.Append((Uscount) + ",");
                    userIncreaseData += (int.Parse(Scount) - int.Parse(Uscount)) + ",";
                }

                barChartData += strJsonName.ToString().TrimEnd(',');
                userChartData += strJsonUser.ToString().TrimEnd(',');
                subChartData += strJsonSub.ToString().TrimEnd(',');
                unsubChartData += strJsonNoSub.ToString().TrimEnd(',');

            }
        }


        protected string GetSCount(object objId)
        {
            var strToReturn = "0";
            var id = Utils.ObjToInt(objId, 0);
            if (id != 0)
            {
                var last_time = suscribtList.Where(x => x.key == id).FirstOrDefault();
                if (last_time != null)
                    strToReturn = last_time.count.ToString();
            }
            return strToReturn;
        }

        protected string GetUSCount(object objId)
        {
            var strToReturn = "0";
            var id = Utils.ObjToInt(objId, 0);
            if (id != 0)
            {
                var last_time = unSuscribtList.Where(x => x.key == id).FirstOrDefault();
                if (last_time != null)
                    strToReturn = last_time.count.ToString();
            }
            return strToReturn;

        }

        protected string GetShowName()
        {
            return BLL.Chats.Statistics_Dashboard.GetDashboardCompareString(wxList);
        }








    }
}