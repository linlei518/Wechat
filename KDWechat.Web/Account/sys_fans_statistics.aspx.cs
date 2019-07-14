using KDWechat.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
/////////////////////////
///time:2014-9-11 by:yzl/
///////////////////////
namespace KDWechat.Web.Account
{
    public partial class sys_fans_statistics : Web.UI.BasePage
    {
        protected string strIds;    //选中对比的公众号ID
        protected string strTolIds; //当前用户的所拥有的所有公众号ID
        protected int allCount;    //总人数
        protected string strWxlist; //微信列表
        protected string strDefwx;  //默认微信Id           
        protected string strTitles; //对比标题
        protected string pieChartData;//饼状图数据

        protected string chartDateRange;
        protected string chartXInterval;

        protected string chartDateRange2; //关注用户统计参数
        protected string JsonData;
        protected string chartXInterval2;

        protected string unchartDateRange2; //取消关注用户统计参数
        protected string unJsonData;
        protected string unchartXInterval2;

        protected string MsgchartDateRange2; //发送消息统计参数
        protected string MsgJsonData;
        protected string MsgchartXInterval2;

        protected string RecchartDateRange2; //接收消息统计参数
        protected string RecJsonData;
        protected string RecchartXInterval2;

        protected string sub_Sdate;  //关注用户统计开始时间
        protected string sub_Edate;  //关注用户统计结束时间

        protected string un_Sdate;  //取消关注用户统计开始时间
        protected string un_Edate;  //取消关注用户统计结束时间

        protected string Msg_Sdate;  //发送消息统计开始时间
        protected string Msg_Edate;  //发送消息统计结束时间

        protected string Rec_Sdate;  //接收消息统计开始时间
        protected string Rec_Edate;  //接收消息统计结束时间

            
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                strIds = RequestHelper.GetQueryString("ids");

                if (!string.IsNullOrEmpty(strIds))
                {
                    if (!string.IsNullOrEmpty(RequestHelper.GetQueryString("sub_sdate")) && !string.IsNullOrEmpty(RequestHelper.GetQueryString("sub_edate")))
                    {
                        sub_Sdate = RequestHelper.GetQueryString("sub_sdate");
                        sub_Edate = RequestHelper.GetQueryString("sub_edate");
                        txt_date_show.Value = sub_Sdate + " — " + sub_Edate;
                    }
                    else if (!string.IsNullOrEmpty(RequestHelper.GetQueryString("sub_sdate")))
                    {
                        sub_Sdate = RequestHelper.GetQueryString("sub_sdate");
                        sub_Edate = DateTime.Now.ToString();
                        txt_date_show.Value = sub_Sdate;
                    }
                    else if (!string.IsNullOrEmpty(RequestHelper.GetQueryString("sub_edate")))
                    {
                        sub_Sdate = DateTime.Now.AddDays(-6).ToString();
                        sub_Edate = RequestHelper.GetQueryString("sub_edate");
                        txt_date_show.Value = sub_Edate;
                    }
                    else
                    {
                        sub_Sdate = DateTime.Now.AddDays(-6).ToString();
                        sub_Edate = DateTime.Now.ToString();
                    }
                    txtbegin_date.Text = sub_Sdate;
                    txtend_date.Text = sub_Edate;
                    //---------------------------------
                    if (!string.IsNullOrEmpty(RequestHelper.GetQueryString("un_sdate")) && !string.IsNullOrEmpty(RequestHelper.GetQueryString("un_edate")))
                    {
                        un_Sdate = RequestHelper.GetQueryString("un_sdate");
                        un_Edate = RequestHelper.GetQueryString("un_edate");
                        txt_undate_show.Value = un_Sdate + " — " + un_Edate;
                    }
                    else if (!string.IsNullOrEmpty(RequestHelper.GetQueryString("un_sdate")))
                    {
                        un_Sdate = RequestHelper.GetQueryString("un_sdate");
                        un_Edate = DateTime.Now.ToString();
                        txt_undate_show.Value = un_Sdate;
                    }
                    else if (!string.IsNullOrEmpty(RequestHelper.GetQueryString("un_edate")))
                    {
                        un_Sdate = DateTime.Now.AddDays(-6).ToString();
                        un_Edate = RequestHelper.GetQueryString("un_edate");
                        txt_undate_show.Value = un_Edate;
                    }
                    else
                    {
                        un_Sdate = DateTime.Now.AddDays(-6).ToString();
                        un_Edate = DateTime.Now.ToString();
                    }
                    txtunbegin_date.Text = un_Sdate;
                    txtunend_date.Text = un_Edate;
                    //-------------------------------------
                    if (!string.IsNullOrEmpty(RequestHelper.GetQueryString("Msg_sdate")) && !string.IsNullOrEmpty(RequestHelper.GetQueryString("Msg_edate")))
                    {
                        Msg_Sdate = RequestHelper.GetQueryString("Msg_sdate");
                        Msg_Edate = RequestHelper.GetQueryString("Msg_edate");
                        txt_Msgdate_show.Value = Msg_Sdate + " — " + Msg_Edate;
                    }
                    else if (!string.IsNullOrEmpty(RequestHelper.GetQueryString("Msg_sdate")))
                    {
                        Msg_Sdate = RequestHelper.GetQueryString("Msg_sdate");
                        Msg_Edate = DateTime.Now.ToString();
                        txt_Msgdate_show.Value = Msg_Sdate;
                    }
                    else if (!string.IsNullOrEmpty(RequestHelper.GetQueryString("Msg_edate")))
                    {
                        Msg_Sdate = DateTime.Now.AddDays(-6).ToString();
                        Msg_Edate = RequestHelper.GetQueryString("Msg_edate");
                        txt_Msgdate_show.Value = Msg_Edate;
                    }
                    else
                    {
                        Msg_Sdate = DateTime.Now.AddDays(-6).ToString();
                        Msg_Edate = DateTime.Now.ToString();
                    }
                    txtMsgbegin_date.Text = Msg_Sdate;
                    txtMsgend_date.Text = Msg_Edate;
                    //------------------------------------
                    if (!string.IsNullOrEmpty(RequestHelper.GetQueryString("Rec_sdate")) && !string.IsNullOrEmpty(RequestHelper.GetQueryString("Rec_edate")))
                    {
                        Rec_Sdate = RequestHelper.GetQueryString("Rec_sdate");
                        Rec_Edate = RequestHelper.GetQueryString("Rec_edate");
                        txt_Recdate_show.Value = Rec_Sdate + " — " + Rec_Edate;

                    }
                    else if (!string.IsNullOrEmpty(RequestHelper.GetQueryString("Rec_sdate")))
                    {
                        Rec_Sdate = RequestHelper.GetQueryString("Rec_sdate");
                        Rec_Edate = DateTime.Now.ToString();
                        txt_Recdate_show.Value = Rec_Sdate;
                    }
                    else if (!string.IsNullOrEmpty(RequestHelper.GetQueryString("Rec_edate")))
                    {
                        Rec_Sdate = DateTime.Now.AddDays(-6).ToString();
                        Rec_Edate = RequestHelper.GetQueryString("Rec_edate");
                        txt_Recdate_show.Value = Rec_Edate;
                    }
                    else
                    {
                        Rec_Sdate = DateTime.Now.AddDays(-6).ToString();
                        Rec_Edate = DateTime.Now.ToString();
                    }
                    txtRecbegin_date.Text = Rec_Sdate;
                    txtRecend_date.Text = Rec_Edate;
                    //-------------------------------------
                    BindData();
                }
            }
        }
        /// <summary>
        /// 绑定数据
        /// </summary>
        protected void BindData()
        {
            strIds = HttpUtility.UrlDecode(strIds);
            strIds = strIds.Remove(strIds.Length - 1, 1);

            string[] idlist = strIds.Split(',');
            foreach (string def in idlist)
            {
                strDefwx += "'" + def + "'" + ",";
            }
            strDefwx = strDefwx.Remove(strDefwx.Length - 1, 1);

            var listAll = BLL.Chats.wx_wechats.GetList();
            BindWxlist(listAll);
           

        }
        /// <summary>
        /// 绑定对比公众号
        /// </summary>
        protected void BindWxlist(List<DAL.t_wx_wechats> list)
        {
            int len = list.Count;

            StringBuilder strJson = new StringBuilder();

            strJson.Append("[");


            for (int i = 0; i < len; i++)
            {
                strJson.Append("{ id: '" + list[i].id + "', name: '" + list[i].wx_pb_name + "' }, ");
                strTolIds += list[i].id.ToString() + ",";
            }

            if (strJson.Length > 0)
            {
                strJson.Remove(strJson.Length - 1, 1);
                strJson.Append("]");
            }

            strWxlist = strJson.ToString();


            BindCharts(strTolIds, strIds);
        }
        /// <summary>
        /// 绑定对比分析数据
        /// </summary>
        /// <param name="ids"></param>
        protected void BindCharts(string tids, string ids)
        {
            #region 关注用户总数对(饼状图)
            StringBuilder strHtml = new StringBuilder();

            //总数
            tids = tids.Remove(tids.Length - 1, 1);

            string[] tIds = tids.Split(',');
            int tlenth = tIds.Length;
            int[] tidsItems = new int[tlenth];
            for (int j = 0; j < tlenth; j++)
            {
                tidsItems[j] = Utils.StrToInt(tIds[j], 0);
            }
            allCount = BLL.Users.wx_fans.GetTotalCountByNoSel(tidsItems);



            string[] idlist = ids.Split(',');
            int len = idlist.Length;
            int[] idItems = new int[len];
            for (int i = 0; i < len; i++)
            {
                idItems[i] = Common.Utils.StrToInt(idlist[i], 0);
            }


            var wxList = KDWechat.BLL.Chats.wx_wechats.GetList(idItems);
            foreach (var wechat in wxList)
            {
                int cout = BLL.Users.wx_fans.GetTotalCountByWxID(wechat.id);
                pieChartData += "['" + wechat.wx_pb_name + "'," + cout.ToString() + "],";
                strTitles += wechat.wx_pb_name + "、";

                strHtml.Append("<tr>");
                strHtml.Append("<td class=\"name\">" + wechat.wx_pb_name + "</td>");
                strHtml.Append("<td>" + cout.ToString() + "人</td>");
                strHtml.Append("</tr>");
            }

            lit_table.Text = strHtml.ToString();

            //绑定标题
            if (!string.IsNullOrEmpty(strTitles))
            {
                strTitles = strTitles.Remove(strTitles.Length - 1, 1);

                strTitles += "&nbsp;数据对比";

            }


            if (pieChartData.Length > 0)
            {
                pieChartData = pieChartData.Remove(pieChartData.Length - 1, 1);
            }

            #endregion


            #region 绑定订阅人数area图

            var dateList = Utils.GetDateListByStartAndEnd(Utils.StrToDateTime(sub_Sdate), Utils.StrToDateTime(sub_Edate));//获取一个时间段内的所有日期 -Damos
            chartXInterval = (dateList.Count / 10).ToString(); //日期过多时，显示间隔为日期数/10 -Damos
            chartDateRange = Utils.GetDateXAxisByDateList(dateList);//获取X轴的值（短日期,分割） -Damos

            string[] weekList;
            chartDateRange2 = chartDateRange;
            chartXInterval2 = chartXInterval;
            foreach (var wechat in wxList)
            {
                weekList = BLL.Users.wx_fans.GetFansCountByDateList(dateList, wechat.id);
                JsonData += string.Format("{{name:\"{0}\",data:[{1}]}},", wechat.wx_pb_name, Utils.GetArrayStr(weekList, ","));


            }

            JsonData = JsonData.Length > 0 ? JsonData.Substring(0, JsonData.Length - 1) : JsonData;

            #endregion

            #region 绑定取消订阅人数area图

            dateList = Utils.GetDateListByStartAndEnd(Utils.StrToDateTime(un_Sdate), Utils.StrToDateTime(un_Edate));//获取一个时间段内的所有日期 -Damos
            chartXInterval = (dateList.Count / 10).ToString(); //日期过多时，显示间隔为日期数/10 -Damos
            chartDateRange = Utils.GetDateXAxisByDateList(dateList);//获取X轴的值（短日期,分割） -Damos

            unchartDateRange2 = chartDateRange;
            unchartXInterval2 = chartXInterval;
            foreach (var wechat in wxList)
            {
                weekList = BLL.Users.wx_fans.GetUnFansCountByDateList(dateList, wechat.id);
                unJsonData += string.Format("{{name:\"{0}\",data:[{1}]}},", wechat.wx_pb_name, Utils.GetArrayStr(weekList, ","));

            }

            unJsonData = unJsonData.Length > 0 ? unJsonData.Substring(0, unJsonData.Length - 1) : unJsonData;

            #endregion

            #region 绑定发送消息数据area图

            dateList = Utils.GetDateListByStartAndEnd(Utils.StrToDateTime(Msg_Sdate), Utils.StrToDateTime(Msg_Edate));//获取一个时间段内的所有日期 -Damos
            chartXInterval = (dateList.Count / 10).ToString(); //日期过多时，显示间隔为日期数/10 -Damos
            chartDateRange = Utils.GetDateXAxisByDateList(dateList);//获取X轴的值（短日期,分割） -Damos

            MsgchartDateRange2 = chartDateRange;
            MsgchartXInterval2 = chartXInterval;
            foreach (var wechat in wxList)
            {
                weekList = BLL.Logs.wx_fans_chats.GetChatCountByDateList(dateList, wechat.id, FromUserType.公众号);
                MsgJsonData += string.Format("{{name:\"{0}\",data:[{1}]}},", wechat.wx_pb_name, Utils.GetArrayStr(weekList, ","));

            }

            MsgJsonData = MsgJsonData.Length > 0 ? MsgJsonData.Substring(0, MsgJsonData.Length - 1) : MsgJsonData;

            #endregion

            #region 绑定接收消息数据area图

            dateList = Utils.GetDateListByStartAndEnd(Utils.StrToDateTime(Rec_Sdate), Utils.StrToDateTime(Rec_Edate));//获取一个时间段内的所有日期 -Damos
            chartXInterval = (dateList.Count / 10).ToString(); //日期过多时，显示间隔为日期数/10 -Damos
            chartDateRange = Utils.GetDateXAxisByDateList(dateList);//获取X轴的值（短日期,分割） -Damos

            RecchartDateRange2 = chartDateRange;
            RecchartXInterval2 = chartXInterval;

            foreach (var wechat in wxList)
            {
                weekList = BLL.Logs.wx_fans_chats.GetChatCountByDateList(dateList, wechat.id, FromUserType.用户);
                RecJsonData += string.Format("{{name:\"{0}\",data:[{1}]}},", wechat.wx_pb_name, Utils.GetArrayStr(weekList, ","));

            }

            RecJsonData = RecJsonData.Length > 0 ? RecJsonData.Substring(0, RecJsonData.Length - 1) : RecJsonData;

            #endregion
        }
    }
}