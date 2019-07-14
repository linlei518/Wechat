using KDWechat.BLL.Chats;
using KDWechat.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.Account
{
    public partial class sys_fans_wx_statistics : Web.UI.BasePage
    {
        #region 公众号相关属性
        protected new string wx_name = "";
        protected string wx_pb_name = "";
        protected string wxType = "";
        protected string wx_apiurl = "";
        protected string wx_token = "";
        protected string head_url = "";
        protected new int wx_id { get { return RequestHelper.GetQueryInt("id", 0); } }
        #endregion

        #region 图表相关属性
        protected string chartDateRange = ""; //这些属性解释请见前台
        protected string chartName = "";
        protected string chartSubName = "";
        protected string chartUnit = "";
        protected string chartSerisName1 = "";
        protected string chartSerisName2 = "";
        protected string chartSeris1 = "";
        protected string chartSeris2 = "";
        protected string chartYName = "";
        #endregion

        protected int currentTag { get { return RequestHelper.GetQueryInt("tag", 0); } }//目前选中的标签


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //CheckUserAuthority("wechat_system");  //权限，最后添加
                InitData();
            }
        }

        private void InitData()
        {
            if (wx_id > 0)
            {
                fansTag.HRef = "sys_fans_wx_statistics.aspx?tag=0&m_id=" + m_id + "&id=" + wx_id;
                msgTag.HRef = "sys_fans_wx_statistics.aspx?tag=1&m_id=" + m_id + "&id=" + wx_id;
                //判断当前选中的栏目，进行CSS样式更新
                if (currentTag == 0)
                {
                    fansTag.Attributes.Add("class","btn nTabBtn current");
                }
                else if (currentTag == 1)
                {
                    msgTag.Attributes.Add("class", "btn nTabBtn current");
                }
                else if (currentTag == 2)
                {
                    //columnTag.CssClass = "btn nTabBtn current";   //暂时隐藏，第三项
                }

                //获取wechat进行数据绑定
                var wechat = wx_wechats.GetWeChatByID(wx_id);
                if (null != wechat)
                {
                    wx_pb_name = wechat.wx_pb_name;
                    wxType = ((Common.WeChatServiceType)wechat.type_id).ToString();
                    wx_apiurl = wechat.api_url;
                    wx_token = wechat.token;
                    head_url = wechat.header_pic;
                    wx_name = wechat.wx_name;
                    InitChart();
                }
            }
                //JsHelper.AlertAndRedirect("登陆超时，请重新登陆！", "../KDLogin/login.aspx");
        }

        //图表的初始化
        private void InitChart()
        {
            if (wx_id > 0)
            {
                //取最近一周日期，绑定X轴
                string[] weekList = new string[7];
                for (int i = 0; i < 7; i++)
                {
                    weekList[6 - i] = "'" + DateTime.Now.AddDays(-i).ToString("MM-dd") + "'";
                }
                chartDateRange = Utils.GetArrayStr(weekList, ",");

                //判断当前选中的栏目
                if (currentTag == 0)
                {
                    //图表属性初始化
                    chartName = "一周关注人数统计";
                    chartSubName = "趋势图-关注人数";
                    chartUnit = "人";
                    chartYName = "人数（人）";

                    //绑定第一条线的数据
                    weekList = BLL.Users.wx_fans.GetFansCountWeekly(wx_id);
                    chartSeris1 = Utils.GetArrayStr(weekList, ",");
                    chartSerisName1 = "订阅人数";

                    //绑定第二条线的数据
                    weekList = BLL.Users.wx_fans.GetFansCountWeekly(wx_id, Status.禁用);
                    chartSeris2 = Utils.GetArrayStr(weekList, ",");
                    chartSerisName2 = "退订人数";

                }
                else if (currentTag == 1)
                {
                    //同上
                    chartName = "一周消息统计";
                    chartSubName = "趋势图-消息数";
                    chartUnit = "条";
                    chartYName = "条数（条）";


                    //顺序问题：第二块阴影会遮住第一块，所以优先设置数据量大的数据
                    weekList = BLL.Logs.wx_fans_chats.GetChatCountWeekly(wx_id, FromUserType.用户);
                    chartSeris1 = Utils.GetArrayStr(weekList, ",");
                    chartSerisName1 = "接受条数";

                    weekList = BLL.Logs.wx_fans_chats.GetChatCountWeekly(wx_id, FromUserType.公众号);
                    chartSeris2 = Utils.GetArrayStr(weekList, ",");
                    chartSerisName2 = "发送条数";
                }
                else//第三项栏目，暂时隐藏
                {

                }

            }
        }
    }
}