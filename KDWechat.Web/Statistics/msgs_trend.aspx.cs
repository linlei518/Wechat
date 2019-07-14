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
    public partial class msg_trend : Web.UI.BasePage
    {

        protected int count1 = 0;//统计总数1
        protected int count2 = 0;//统计总数2
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
            DateTime startTime = Utils.StrToDateTime(txtbegin_date.Text, DateTime.Now.AddDays(-7));
            DateTime endTime = Utils.StrToDateTime(txtend_date.Text, DateTime.Now);
            List<BLL.Chats.FansStatistics> fansList = wx_fans.GetFansListByWxID(wx_id, startTime, endTime);

            var dateList = Utils.GetDateListByStartAndEnd(startTime, endTime);//获取一个时间段内的所有日期 -Damos
            BindTotalCount();//根据tag绑定总数
            BindChart(dateList, fansList);//绑定图表信息

            BindTable(dateList);

        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            InitData();
        }

        #region 绑定数据表
        private void BindTable(List<DateTime> dateList)
        {
            int[] userList = BLL.Logs.wx_fans_chats.GetTrendGroupCountByDateList<string>(dateList, wx_id, FromUserType.用户,x=>x.open_id);
            int[] msgCountList = BLL.Logs.wx_fans_chats.GetTrendGroupCountByDateList<int>(dateList, wx_id, FromUserType.用户, x => x.id);

            StringBuilder sb = new StringBuilder();
            for (int i = userList.Length - 1; i >= 0; i--)
            {
                double perNo=0;
                if(userList[i]!=0)
                    perNo = (msgCountList[i]*1.0)/userList[i];
                sb.Append("<tr>");
                sb.Append("<td class=\"name\">").Append(dateList[i].ToShortDateString()).Append("</td>");
                sb.Append("<td class=\"info info2\">").Append(userList[i]).Append("</td>");
                sb.Append("<td class=\"info info1\">").Append(msgCountList[i]).Append("</td>");
                sb.Append("<td class=\"info info1\">").Append(perNo.ToString("0.00")).Append("</td>");
                sb.Append("</tr>");
            }
            detailTable = sb.ToString();
        }
        #endregion

        #region 绑定总数
        void BindTotalCount()
        {
            count1 = wx_fans_chats.GetCount(x => x.wx_id == wx_id && x.from_type == (int)FromUserType.用户);
        }
        #endregion

        #region 绑定图表
        private void BindChart(List<DateTime> dateList, List<BLL.Chats.FansStatistics> fansList)
        {
            int[] weekList;
            chartXInterval = (dateList.Count / 10).ToString(); //日期过多时，显示间隔为日期数/10 -Damos
            chartDateRange = Utils.GetDateXAxisByDateList(dateList);//获取X轴的值（短日期,分割） -Damos

            chartUnit = "条"; //toolTip里面的单位 -Damos
            chartYName = "条数（条）";//y轴名称

            weekList = BLL.Logs.wx_fans_chats.GetTrendCountByDateList(dateList, wx_id, FromUserType.用户);
            chartSeris1 = JsonConvert.SerializeObject(weekList);
            chartSerisName1 = "消息接收条数";


            weekList = BLL.Logs.wx_fans_chats.GetTrendCountByDateList(dateList, wx_id, FromUserType.公众号);
            chartSeris2 = JsonConvert.SerializeObject(weekList);
            chartSerisName2 = "消息发送条数";
        }
        #endregion


    }


}