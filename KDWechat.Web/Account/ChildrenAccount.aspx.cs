using KDWechat.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.Account
{
    public partial class ChildrenAccount : Web.UI.BasePage
    {
        protected int id
        {
            get { return RequestHelper.GetQueryInt("id", 0); }
        }
        protected int pagesize { get { return 10; } }
        protected string parentName = "";

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
                hfReturlUrl.Value = "ManageAccount.aspx?m_id=51";
            }
        }

        private void InitData()
        {
            if (id != 0)
            {
                var parent = BLL.Users.sys_users.GetUserByID(id);
                parentName = parent.user_name;
                var list = BLL.Users.sys_users.GetUserListByParentID(id, pagesize, page);
                Repeater1.DataSource = list;
                Repeater1.DataBind();
            }
        }

        protected void DataRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Disable")
            {

                int id = KDWechat.Common.Utils.StrToInt(e.CommandArgument.ToString(), 0);
                Status status = Status.禁用;
                if (((Button)e.CommandSource).Text == "启用")
                {
                    status = Status.正常;
                }
                BLL.Users.sys_users.SetUserStatus(id, status);
                AddLog(string.Format("{0}公众账号:{1}", status == Status.正常 ? "启用" : "停用", u_name), LogType.修改);
                Response.Redirect(Request.Url.ToString());
            }
        }
    }
}