using KDWechat.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.BLL.Users;
using System.Linq.Expressions;
using KDWechat.DAL;
using KDWechat.BLL.Chats;

namespace KDWechat.Web.Account
{
    public partial class sys_user_statics : Web.UI.BasePage
    {
        public string selectedString { get; set; }
        protected int area { get { return RequestHelper.GetQueryInt("area", -1); } }

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


        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.HttpMethod == "POST" && Request.QueryString["t"] == "1")
            {
                CheckAjaxData();
            }

            if (!IsPostBack)
                InitData();
        }

        private void CheckAjaxData()
        {
            Expression<Func<t_sys_users, bool>> sys_where = x => x.flag == (int)UserFlag.地区账号;
            if(area!=-1)
                sys_where = sys_where.And(x => x.area == area);
            Expression<Func<t_sys_users, int>> sys_selectedValue = x => x.id;
            int[] adminArray = sys_users.GetAdminArray(sys_where, sys_selectedValue);
            var list = wx_wechats.GetList<int>(x => adminArray.Contains(x.uid), int.MaxValue, 1, out totalCount, x => x.id);
            string output = "";
            foreach (var x in list)
            {
                output += "<tr><td class=\"name\">"+x.wx_pb_name+"</td><td class=\"info info1\">"+((Common.WeChatServiceType)x.type_id).ToString()+"</td><td class=\"time\">"+x.create_time+"</td><td class=\"control\"><a href=\"sys_fans_wx_statistics.aspx?m_id="+m_id+"&id="+x.id+"\" class=\"btn btn6\" >查看详细</a><a href=\"javascript:AddChange('"+x.wx_pb_name+"',"+x.id+");\" id=\"aLabel"+x.id+"\" class=\"btn btn6\" >加入对比</a></td></tr>";
            }
            if (list.Count == 0)
                output = "<tr><td style=\"text-align:center;\" colspan=\"8\">暂无数据</td></tr>";
            Response.Write(output);
            Response.End();

        }

        private void InitData()
        {
            string chooseFormat = "{0}";
            bool showChoose = false;
            string itemString = "";
            if (area != -1)
            {
                itemString += "<a href=\"sys_fans_statistics_list.aspx?m_id="+m_id+"\" class=\"btn cancelBubble\" title=\"点击取消\">标签：" + ((AreaType)area).ToString() + "</a>";
                dlArea.Attributes.Add("class", "selected");
                showChoose = true;
            }
            if (showChoose)
                chooseFormat = "<dl class=\"selectedList\"><dt>已选择：</dt><dd class=\"btns\"> <a href=\"sys_fans_statistics_list.aspx?m_id=" + m_id + "\" class=\"btn filterCancel\">全部撤销</a></dd><dd>{0}</dd></dl>";
            selectedString = string.Format(chooseFormat, itemString);


            Expression<Func<t_sys_users, bool>> sys_where = x => x.flag == (int)UserFlag.地区账号;
            Expression<Func<t_sys_users, int>> sys_selectedValue = x => x.id;
            int[] adminArray = sys_users.GetAdminArray(sys_where, sys_selectedValue);
            var list = wx_wechats.GetList<int>(x => adminArray.Contains(x.uid), int.MaxValue, 1, out totalCount, x => x.id);
            repWxList.DataSource = list;
            repWxList.DataBind();


            InitChart();
        }





        //图表的初始化
        private void InitChart()
        {
            //取最近一周日期，绑定X轴
            string[] weekList = new string[5] { "华东", "华北", "华南", "西南", "凯德城镇开发" };

            chartDateRange = "\"" + Utils.GetArrayStr(weekList, "\",\"") + "\"";

            for (int i = 0; i < 5; i++)
            {
                Expression<Func<t_sys_users,bool>> sys_where=x=>x.area==i&&x.flag==(int)UserFlag.地区账号;
                Expression<Func<t_sys_users, int>> sys_selectedValue = x => x.id;
                int[] adminArray = sys_users.GetAdminArray(sys_where,sys_selectedValue);
                Expression<Func<t_wx_wechats, bool>> wechat_where = x => adminArray.Contains(x.uid);
                Expression<Func<t_wx_wechats, int>> wechat_selectedValue = x => x.id;
                int[] wechatArray = wx_wechats.GetWeChatArray(wechat_where,wechat_selectedValue);
                weekList[i] = wx_fans.GetCount(x => wechatArray.Contains(x.wx_id)).ToString();
            }


                //图表属性初始化
            chartName = "关注人数统计";
            chartSubName = "趋势图-关注人数";
            chartUnit = "人";
            chartYName = "人数（人）";

            //绑定第一条线的数据
            chartSeris1 = Utils.GetArrayStr(weekList, ",");
            chartSerisName1 = "订阅人数";



            //绑定第二条线的数据             暂时没有第二条线
            //weekList = BLL.Users.wx_fans.GetFansCountWeekly(wx_id, Status.禁用);
            //chartSeris2 = Utils.GetArrayStr(weekList, ",");
            //chartSerisName2 = "退订人数";
        }

        protected void repWxList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }

    }
}