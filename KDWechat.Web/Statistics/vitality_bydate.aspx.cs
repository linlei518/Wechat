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
    public partial class vitality_bydate : Web.UI.BasePage
    {
        protected DateTime startTime;
        protected DateTime endTime;
        protected List<DAL.t_wx_wechats> wxList = null;
        protected List<BLL.Chats.CountStatistics<int>> suscribtList = null;
        protected string barChartData = "";                     //条形图数据
        protected string vitalityChartData = "";          //活跃度条形图数据
        protected string IDs = RequestHelper.GetQueryString("Ids");
        protected int retainHeight = 160;                          //图表保留高度
        protected int vitalityHeight = 0;                        //活跃度图表高度


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
            vitalityHeight = retainHeight + wxList.Count * 33;
            suscribtList = Companycn.Core.EntityFramework.EFHelper.GetGroupBy<creater_wxEntities, t_wx_fans, int, CountStatistics<int>>(x => x.subscribe_time >= startTime && x.subscribe_time <= endTime, x => x.wx_id, x => new CountStatistics<int> { count = x.Count(), key = x.Key });
            BindVitality();
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            InitData();
        }


        //绑定活跃度 -Damos add at 2014-4-1 17:40
        private void BindVitality()
        {
            var startDate = startTime.Date;
            var endDate = endTime.Date;
            //var fansHisViewList = Companycn.Core.EntityFramework.EFHelper.GetGroupBy<creater_wxEntities, t_wx_fans_hisview, int, CountStatistics<int>>(x => x.view_time > startDate && x.view_time < endDate, x => x.wx_id, x => new CountStatistics<int> { count = x.Count(), key = x.Key });
            //var fansChatList = Companycn.Core.EntityFramework.EFHelper.GetGroupBy<creater_wxEntities, t_wx_fans_chats, int, CountStatistics<int>>(x => x.create_time > startDate && x.create_time < endDate, x => x.wx_id, x => new CountStatistics<int> { count = x.Count(), key = x.Key });
            //var fansLocationList = Companycn.Core.EntityFramework.EFHelper.GetGroupBy<creater_wxEntities, t_wx_fans_hislocation, int, CountStatistics<int>>(x => x.create_time > startDate && x.create_time < endDate, x => x.wx_id, x => new CountStatistics<int> { count = x.Count(), key = x.Key });
            //var fansMenuClickList = Companycn.Core.EntityFramework.EFHelper.GetGroupBy<creater_wxEntities, t_st_diymenu_click, string, CountStatistics<string>>(x => x.add_time > startDate && x.add_time < endDate, x => x.wx_og_id, x => new CountStatistics<string> { count = x.Count(), key = x.Key });

            StringBuilder strJsonName = new StringBuilder();

            wxList.ForEach(x =>
            {
                strJsonName.Append("'" + x.wx_pb_name + "'" + ",");
                var total = 0;
                var fansHisViewList = Companycn.Core.EntityFramework.EFHelper.GetGroupBy<creater_wxEntities, t_wx_fans_hisview, string, string>(y => y.view_time > startDate && y.view_time < endDate && y.wx_id == x.id, y => y.open_id, y => y.Key);
                var fansChatList = Companycn.Core.EntityFramework.EFHelper.GetGroupBy<creater_wxEntities, t_wx_fans_chats, string, string>(y => y.create_time > startDate && y.create_time < endDate && y.wx_id == x.id, y => y.open_id, y => y.Key);
                var fansLocationList = Companycn.Core.EntityFramework.EFHelper.GetGroupBy<creater_wxEntities, t_wx_fans_hislocation, string, string>(y => y.create_time > startDate && y.create_time < endDate && y.wx_id == x.id, y => y.open_id, y => y.Key);
                var fansMenuClickList = Companycn.Core.EntityFramework.EFHelper.GetGroupBy<creater_wxEntities, t_st_diymenu_click, string, string>(y => y.add_time > startDate && y.add_time < endDate && y.wx_og_id == x.wx_og_id, y => x.wx_og_id, y => y.Key);

                var totalList = new List<string>();
                totalList.AddRange(fansHisViewList);
                totalList.AddRange(fansChatList);
                totalList.AddRange(fansLocationList);
                totalList.AddRange(fansMenuClickList);
                totalList = totalList.Distinct().ToList();
                total = totalList.Count();

                //total += (fansHisViewList.Where(y => y.key == x.id).FirstOrDefault() ?? new CountStatistics<int> { count = 0 }).count;
                //total += (fansChatList.Where(y => y.key == x.id).FirstOrDefault() ?? new CountStatistics<int> { count = 0 }).count;
                //total += (fansLocationList.Where(y => y.key == x.id).FirstOrDefault() ?? new CountStatistics<int> { count = 0 }).count;
                //total += (fansMenuClickList.Where(y => y.key == x.wx_og_id).FirstOrDefault() ?? new CountStatistics<string> { count = 0 }).count;

                //var fansCount = (suscribtList.Where(y => y.key == x.id).FirstOrDefault() ?? new CountStatistics<int> { count = 1 }).count;
                var fansCount = Companycn.Core.EntityFramework.EFHelper.GetCount<creater_wxEntities, t_wx_fans>(y => y.wx_id == x.id);//(suscribtList.Where(y => y.key == x.id).FirstOrDefault() ?? new CountStatistics<int> { count = 1 }).count;
                fansCount = fansCount == 0 ? 1 : fansCount;
                vitalityChartData += string.Format("{0:F}", ((total * 1.0) / (fansCount) * 100)) + ",";
            });
            barChartData += strJsonName.ToString().TrimEnd(',');
        }

        protected string GetShowName()
        {
            return BLL.Chats.Statistics_Dashboard.GetDashboardCompareString(wxList);
        }







    }
}