using KDWechat.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web
{
    public partial class FansStatistics : Web.UI.BasePage
    {
        protected string pieChartData = "";

        protected string chartDateRange = ""; //这些属性解释请见前台
        protected string chartName = "";
        protected string chartSubName = "";
        protected string chartUnit = "";
        protected string chartSerisName1 = "";
        protected string chartSerisName2 = "";
        protected string chartSeris1 = "";
        protected string chartSeris2 = "";
        protected string chartYName = "";
        protected string chartXInterval = "";

        protected string chartDateRange2 = ""; //这些属性解释请见前台
        protected string JsonData = "";
        protected string chartXInterval2 = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
                InitData();//初始化数据
        }

        private void InitData()
        {
            InitChart();//初始化图表
        }


        private void InitChart()
        {
            #region area chart
            var dateList = Utils.GetDateListByStartAndEnd(DateTime.Now.AddDays(-RequestHelper.GetQueryInt("day",1)), DateTime.Now);//获取一个时间段内的所有日期 -Damos

            chartXInterval = (dateList.Count / 10).ToString(); //日期过多时，显示间隔为日期数/10 -Damos
            chartDateRange = Utils.GetDateXAxisByDateList(dateList);//获取X轴的值（短日期,分割） -Damos
           
            chartName = RequestHelper.GetQueryInt("day",1).ToString()+"天关注人数统计";  //标题 -Damos
            chartSubName = "趋势图-关注人数"; //副标题 -Damos
            chartUnit = "人"; //toolTip里面的单位 -Damos
            chartYName = "人数（人）";//y轴名称

            string[] weekList;
            weekList = BLL.Users.wx_fans.GetFansCountByDateList(dateList,wx_id);
            chartSeris1 = Utils.GetArrayStr(weekList, ",");
            chartSerisName1 = "订阅人数";

            weekList = BLL.Users.wx_fans.GetUnFansCountByDateList(dateList,wx_id);
            chartSeris2 = Utils.GetArrayStr(weekList, ",");
            chartSerisName2 = "退订人数";
            #endregion

            #region pie chart
            var wxList = KDWechat.BLL.Chats.wx_wechats.GetListByUid(u_id);
            foreach (var wechat in wxList)
            {
                int cout = BLL.Users.wx_fans.GetTotalCountByWxID(wechat.id);
                pieChartData += "['"+wechat.wx_pb_name+"',"+cout.ToString()+"],";
            }

            if(pieChartData.Length>0)
                pieChartData=pieChartData.Substring(0,pieChartData.Length-1);
            #endregion

            #region muti pb chart
            chartDateRange2 = chartDateRange;
            chartXInterval2 = chartXInterval;
            foreach (var wechat in wxList)
            {
                weekList = BLL.Users.wx_fans.GetFansCountByDateList(dateList,wechat.id);
                JsonData+=string.Format("{{name:\"{0}\",data:[{1}]}},",wechat.wx_pb_name,Utils.GetArrayStr(weekList,","));
            }
            JsonData = JsonData.Length > 0 ? JsonData.Substring(0, JsonData.Length - 1) : JsonData;
            #endregion
        }

    }
}