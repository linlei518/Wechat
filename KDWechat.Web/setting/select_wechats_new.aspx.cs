using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;
using KDWechat.DAL;

namespace KDWechat.Web.setting
{
    public partial class select_wechats_new : Web.UI.BasePage
    {
        protected int type { get { return RequestHelper.GetQueryInt("type", 0); } }

        protected string key { get { return RequestHelper.GetQueryString("key"); } }

        protected int template_id { get { return RequestHelper.GetQueryInt("template_id", 0); } }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckUserAuthority("template");
                ddlType.SelectedValue = type.ToString();
                txtKey.Value = key;
                int rowCount = 0;
                string where = "";
                if (type>0)
                {
                    where += " and type_id="+type;
                }
                if (key.Trim()!="")
                {
                    where += " and (wx_pb_name like '%"+key+"%' or wx_name like '%"+key+"%')";
                }
                repList.DataSource = GetPageList(DbDataBaseEnum.KD_WECHATS, "select  id,wx_pb_name,type_id,wx_og_id from t_wx_wechats where id not in(select wx_id from t_wx_templates_wechats where template_id=" + template_id + ") "+where, pageSize, page, "*", "id asc", ref rowCount);
                repList.DataBind();

                string pageUrl = string.Format("select_wechats_new.aspx?template_id={0}&page=__id__&type={1}&key={2}", template_id,type,key);
                div_page.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
                if (rowCount < pageSize)
                {
                    div_page.Visible = false;
                }

            }
        }


        protected void repList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "select")
            {
                HiddenField hfWx_ogid=e.Item.FindControl("hfWx_ogid") as HiddenField;
                t_wx_templates_wechats model = new t_wx_templates_wechats() { 
                    is_default=0,
                    template_id=template_id,
                    wx_id = Convert.ToInt32(e.CommandArgument),
                    wx_og_id = hfWx_ogid.Value
                };
                BLL.Chats.wx_templates.AddTemplatesWechats(new List<t_wx_templates_wechats>() { model},template_id);
            }
            Response.Redirect(string.Format("select_wechats_new.aspx?template_id={0}&page=__id__&type={1}&key={2}", template_id,type,key));
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("select_wechats_new.aspx?template_id={0}&page=__id__&type={1}&key={2}", template_id, ddlType.SelectedValue, txtKey.Value.Trim()));
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("select_wechats_new.aspx?template_id={0}&page=__id__&type={1}&key={2}", template_id,  ddlType.SelectedValue, txtKey.Value.Trim()));
        }
    }
}