using KDWechat.Common;
using KDWechat.DAL;
using Newtonsoft.Json;
using BaiDuMapAPI.HttpUtility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq.Expressions;

namespace KDWechat.Web.Statistics
{
    public partial class reply_bydate : Web.UI.BasePage
    {
        protected string barChartData="";                          //条形图数据
        protected string replyMsgData="";                //一周回复过的用户数据
        protected string reciveMsgData="";             //一周收到的留言用户数据
        protected DateTime startTime;
        protected DateTime endTime;
        protected List<DAL.t_wx_wechats> wxList = null;
        protected List<DAL.t_wx_fans_chats> chatList = null;
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
            wxList = Companycn.Core.EntityFramework.EFHelper.GetList<creater_wxEntities, t_wx_wechats, int>(wx_list_where, x => x.id, int.MaxValue, 1); BindReplyBar();
            otherHeght = retainHeight + wxList.Count * 61;
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            InitData();
        }


        private void BindReplyBar()
        {

            StringBuilder strJsonName = new StringBuilder();

            foreach (var wechat in wxList)
            {
                strJsonName.Append("'" + wechat.wx_pb_name + "'" + ",");
            }

            barChartData += strJsonName.ToString().TrimEnd(',');
            chatList = Companycn.Core.EntityFramework.EFHelper.GetList<DAL.creater_wxEntities, t_wx_fans_chats, int>(x => x.create_time >= startTime && x.create_time <= endTime, x => x.id, int.MaxValue, 1);

            if (wxList.Count > 0)
            {

                foreach (var wechat in wxList)
                {
                    reciveMsgData += chatList.Where(x => x.wx_id == wechat.id && x.from_type == (int)Common.FromUserType.用户).GroupBy(x => x.open_id).Count().ToString() + ",";
                    replyMsgData += chatList.Where(x => x.wx_id == wechat.id && x.from_type == (int)Common.FromUserType.公众号).GroupBy(x => x.open_id).Count().ToString() + ",";
                }
            }
        }



        protected string GetShowName()
        {
            return BLL.Chats.Statistics_Dashboard.GetDashboardCompareString(wxList);
        }




    }
}