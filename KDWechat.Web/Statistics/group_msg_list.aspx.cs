using KDWechat.BLL.Chats;
using KDWechat.Common;
using KDWechat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.Statistics
{
    public partial class group_msg_list : Web.UI.BasePage
    {
        protected Common.Config.wechatconfig wechatConfig = new BLL.Config.wechat_config().loadConfig();
        protected new int wx_id = RequestHelper.GetQueryInt("wx_id", 0);
        protected string sub_sdate { get { return RequestHelper.GetQueryString("sdate"); } }
        protected string sub_edate { get { return RequestHelper.GetQueryString("edate"); } }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckUserAuthority("sys_dashboard");
                SetRefferUrl();
                InitData();
            }
        }

        private void SetRefferUrl()
        {
            try
            {
                hfReturlUrl.Value = Request.UrlReferrer.ToString();
            }
            catch (Exception)
            {
                hfReturlUrl.Value = "group_msg_list.aspx?m_id="+m_id.ToString();
            }
        }


        private void InitData()
        {

            int totalCount;
            List<t_wx_group_msgs> lis2 = null;
            if (wx_id > 0)
            {
                var startDate = DateTime.Now.Date.AddDays(-1).AddMonths(-1);
                var endDate = DateTime.Now.Date.AddDays(-1);
                if (!string.IsNullOrWhiteSpace(sub_sdate))
                    startDate = Utils.StrToDateTime(DESEncrypt.Decrypt(sub_sdate,"sdate"), startDate);
                if (!string.IsNullOrWhiteSpace(sub_edate))
                    endDate = Utils.StrToDateTime(DESEncrypt.Decrypt(sub_edate, "edate"), endDate);

                lis2 = Companycn.Core.EntityFramework.EFHelper.GetList<creater_wxEntities, t_wx_group_msgs, int>(x => x.wx_id == wx_id && (x.msg_type == (int)msg_type.单图文 || x.msg_type == (int)msg_type.多图文) && x.is_send == (int)is_sendMode.是 && x.send_time > startDate && x.send_time < endDate, x => x.id, int.MaxValue, 1, out totalCount, true);
            }

            DataRepeater.DataSource = lis2;
            DataRepeater.DataBind();
        }

        protected void DataRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }


        public string GetMsgCount(object objID)
        {
            var id = Utils.StrToInt(objID.ToString(), 0);
            if (id > 0)
            {
                var count = Companycn.Core.EntityFramework.EFHelper.GetCount<creater_wxEntities, t_wx_news_materials>(x => x.id == id || x.par_id == id);
                return count.ToString();
            }
            return "0";
        }
    }
}