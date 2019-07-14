using KDWechat.Common;
using KDWechat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.GroupMsg
{
    public partial class sys_group_msg_list : Web.UI.BasePage
    {
        protected int pagesize = 10;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckUserAuthority("sys_group_list");
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
                hfReturlUrl.Value = "group_message_list.aspx?m_id=10";
            }
        }


        private void InitData()
        {

            int totalCount;
            List<wechat_groupmsg_view> lis2 = null;
            lis2 = BLL.Chats.wx_group_msgs.GetGroupMsgList(page,pagesize, out totalCount);
            string pageUrl = "group_messsage_list.aspx?m_id=10&page=__id__";
            div_page.InnerHtml = Utils.OutPageList(pagesize, page, totalCount, pageUrl, 8);

            DataRepeater.DataSource = lis2;
            DataRepeater.DataBind();
        }
    }
}