using KDWechat.BLL.Chats;
using KDWechat.BLL.Logs;
using KDWechat.BLL.Users;
using KDWechat.Common;
using KDWechat.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace KDWechat.Web.Statistics
{
    public partial class users_trend : Web.UI.BasePage
    {
        protected int count1 = 0;//统计总数1
        protected int count2 = 0;//统计总数2
        public string strWxlist = "";//拼接微信号JSON的string
        protected string detailTable = "";
        #region chart porperty
        protected string chartDateRange = ""; //这些属性解释请见前台JS
        protected string chartUnit = "";
        protected string chartSerisName1 = "";
        protected string chartSeris1 = "";
        protected string chartYName = "";
        protected string chartXInterval = "";
        protected string chartSerisName2 = "";
        protected string chartSeris2 = "";
        protected string jsonData2 = "";
        protected string jsonSex = "";
        protected string jsonLang = "";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                InitData();//初始化数据
        }

        private void InitData()
        {
            DateTime startTime =Utils.StrToDateTime(txtbegin_date.Text, DateTime.Now.Date.AddDays(-7));
            DateTime endTime = Utils.StrToDateTime(txtend_date.Text, DateTime.Now);
            List<BLL.Chats.FansStatistics> fansList = wx_fans.GetFansListByWxID(wx_id,startTime,endTime);

            var dateList = Utils.GetDateListByStartAndEnd(startTime,endTime );//获取一个时间段内的所有日期 -Damos
            BindTotalCount(fansList);//根据tag绑定总数
            var detail = BindChart(dateList, fansList);//绑定图表信息

            BindTable(detail);

            
            
            
            
            BindWxlist();//绑定左上角可选微信号JSON
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            InitData();
        }

        #region 绑定数据表
        private void BindTable(fans_Statistics_Trend_Detail detail)
        {
            var fansCount = count1;
            StringBuilder sb = new StringBuilder();
            for (int i = detail.unsubscribNo.Length-1; i >=0; i--)
            {
                int growthNo=detail.subscribeNo[i] - detail.unsubscribNo[i];
                sb.Append("<tr>");
                sb.Append("<td class=\"name\">").Append(detail.dateList[i].ToString("yyyy-MM-dd")).Append("</td>");
                sb.Append("<td class=\"info info2\">").Append(detail.subscribeNo[i]).Append("</td>");
                sb.Append("<td class=\"info info1\">").Append(detail.unsubscribNo[i]).Append("</td>");
                sb.Append("<td class=\"info info1\">").Append(growthNo).Append("</td>");
                sb.Append("<td class=\"info info1\">").Append(fansCount).Append("</td>");
                fansCount -= growthNo;
                sb.Append("</tr>");
            }
            detailTable = sb.ToString();
        }
        #endregion

        #region 绑定总数
        void BindTotalCount(List<BLL.Chats.FansStatistics> fansList)
        {
            count1 = wx_fans.GetCount(x => x.status == (int)Status.正常&&x.wx_id==wx_id);//fansList.Count(x => x.status == (int)Status.正常);
            //Expression<Func<BLL.Chats.FansStatistics, bool>> where = x => x.status == (int)Status.正常;
           //BindStringStatisticsData(where, fansList); //绑定关注用户数据
        }
        #endregion

        #region 绑定图表
        private fans_Statistics_Trend_Detail BindChart(List<DateTime> dateList, List<BLL.Chats.FansStatistics> fansList)
        {
            fans_Statistics_Trend_Detail detail =new fans_Statistics_Trend_Detail();
            detail.dateList = dateList;

            int[] weekList;
            chartXInterval = (dateList.Count / 10).ToString(); //日期过多时，显示间隔为日期数/10 -Damos
            chartDateRange = Utils.GetDateXAxisByDateList(dateList);//获取X轴的值（短日期,分割） -Damos

            chartUnit = "人"; //toolTip里面的单位 -Damos
            chartYName = "人数（人）";//y轴名称

            weekList = wx_fans.GetTrendByDateList(dateList, fansList, Status.正常);
            chartSeris1 = JsonConvert.SerializeObject(weekList);
            chartSerisName1 = "新关注人数";
            detail.subscribeNo = weekList;

            weekList = wx_fans.GetTrendByDateList(dateList, fansList, Status.禁用);
            chartSeris2 = JsonConvert.SerializeObject(weekList);
            chartSerisName2 = "取消关注人数";
            detail.unsubscribNo = weekList;

            return detail;
        }
        #endregion

        #region 右上角的微信对比绑定
        protected void BindWxlist()
        {
         
            List<t_wx_wechats> listAll;
            if (u_type== (int)UserFlag.子账号)
                listAll = BLL.Chats.wx_wechats.GetListByChildUid(u_id, int.MaxValue, 1, out totalCount);
            else
                listAll = BLL.Chats.wx_wechats.GetListByUid(u_id, int.MaxValue, 1, out totalCount);
            StringBuilder strJson = new StringBuilder();
            strJson.Append("[");
            foreach (DAL.t_wx_wechats wechat in listAll)
            {
                strJson.Append("{ id: '" + wechat.id + "', name: '" + wechat.wx_pb_name + "' }, ");
            }

            if (strJson.Length > 0)
            {
                strJson.Remove(strJson.Length - 1, 1);
                if(strJson.Length>0)
                strJson.Append("]");
            }

            strWxlist = strJson.ToString();
        }
        #endregion

    }

    #region 统计整体
    public class fans_Statistics_Trend_Detail
    {
        public List<DateTime> dateList { get; set; }
        public int[] subscribeNo { get; set; }
        public int[] unsubscribNo { get; set; }
    }
    #endregion


}