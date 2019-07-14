using KDWechat.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.BLL.Chats;

namespace KDWechat.Web.Account
{
    public partial class WXList : Web.UI.BasePage
    {
        protected int uid { get { return RequestHelper.GetQueryInt("uid",0); } }
        protected int pagesize { get { return 10; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
                hfReturlUrl.Value = "WXList.aspx";
            }
        }


        private void InitData()
        {
            int totalCount;
            if (uid != 0)
            {
                var list = BLL.Users.sys_users.GetWechatsListByUID(uid,pagesize,page,out totalCount);
                Repeater1.DataSource = list;
            }
            else
            {
                var list = BLL.Chats.wx_wechats.GetListByUid(u_id, pagesize, page,out totalCount);
                Repeater1.DataSource = list;
            }
            string url = "WXList.aspx?uid=" + uid.ToString() + "&page=__id__";
            div_page.InnerHtml = Utils.OutPageList(pagesize, page, totalCount, url, 8);
            Repeater1.DataBind();
            
        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                int id = int.Parse(e.CommandArgument.ToString());
                if (BLL.Chats.wx_wechats.DeleteWeChatsByID(id))
                {
                    AddLog(string.Format("删除{0}号公众号成功", id.ToString()),LogType.删除);
                    JsHelper.AlertAndRedirect("删除成功", hfReturlUrl.Value);
                }
                else
                    JsHelper.Alert("删除失败");
            }
            else if (e.CommandName == "manage")
            {
                int id = int.Parse(e.CommandArgument.ToString());
                var wechat = wx_wechats.GetWeChatByID(id);
                Utils.WriteCookie(KDKeys.COOKIE_WECHATS_ID, wechat.id.ToString());
                Utils.WriteCookie(KDKeys.COOKIE_WECHATS_WX_OG_ID, wechat.wx_og_id);
                Utils.WriteCookie(KDKeys.COOKIE_WECHATS_NAME, wechat.wx_name);
                Response.Redirect("~/Index.aspx");
            }
        }
    }
}