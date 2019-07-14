using KDWechat.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web
{
    public partial class LBSList : Web.UI.BasePage
    {
        protected int uid { get { return Common.RequestHelper.GetQueryInt("uid", 0); } }
        protected int pagesize = 10;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckUserAuthority("wechat_lbsset");
                SetRefferUrl();//设置上一页
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
                hfReturlUrl.Value = "LbsList.aspx?m_id="+m_id;
            }
        }


        private void InitData()
        {
            var list = BLL.Chats.wx_lbs.GetList(wx_id,pagesize, page,out totalCount);

            string url = "LbsList.aspx?page=__id__&m_id="+m_id;
            div_page.InnerHtml = Utils.OutPageList(pagesize, page, totalCount, url, 8);

            Repeater1.DataSource = list;
            Repeater1.DataBind();

        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                int id = int.Parse(e.CommandArgument.ToString());
                if (BLL.Chats.wx_lbs.DeleteLbsByID(id))//删除LBS信息（方法内包含了云存储平台的删除）
                {
                    AddLog(string.Format("删除{0}号LBS信息", id.ToString()),LogType.删除);
                    JsHelper.AlertAndRedirect("LBS删除成功", "LBSList.aspx?m_id="+m_id);
                }
                else
                    JsHelper.Alert("LBS删除失败");
            }
        }

    }
}