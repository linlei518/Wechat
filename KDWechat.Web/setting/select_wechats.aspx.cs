using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;

namespace KDWechat.Web.setting
{
    public partial class select_wechats :Web.UI.BasePage
    {
        protected int type { get { return RequestHelper.GetQueryInt("type", 0); } }

        protected string key { get { return RequestHelper.GetQueryString("key"); } }
        protected int template_id { get { return RequestHelper.GetQueryInt("template_id", 0); } }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckUserAuthority("template");
                //ddlType.SelectedValue = type.ToString();
                //txtKey.Value = key;
                int rowCount = 0;
                //string where = "";
                //if (type > 0)
                //{
                //    where += " and type_id=" + type;
                //}
                //if (key.Trim() != "")
                //{
                //    where += " and (wx_pb_name like '%" + key + "%' or wx_name like '%" + key + "%')";
                //}
                repList.DataSource = GetPageList(DbDataBaseEnum.KD_WECHATS, "select *,(select wx_pb_name from t_wx_wechats where id=wx_id) as wx_pb_name,(select type_id from t_wx_wechats where id=wx_id) as type_id from t_wx_templates_wechats where template_id="+template_id, pageSize, page, "*", "id asc", ref rowCount);
                repList.DataBind();

                string pageUrl = string.Format("select_wechats.aspx?template_id={0}&page=__id__", template_id);
                div_page.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
                if (rowCount<pageSize)
                {
                    div_page.Visible = false;
                }

            }
        }

       
        protected void repList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName=="remove")
            {
                BLL.Chats.wx_templates.DeleteTemplateWechat(Convert.ToInt32(e.CommandArgument));
            }
            Response.Redirect("select_wechats.aspx?template_id=" + template_id);
        }
    }
}