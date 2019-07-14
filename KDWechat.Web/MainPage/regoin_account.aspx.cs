using KDWechat.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.main
{
    public partial class regoin_account : Web.UI.BasePage
    {
        protected int id
        {
            get { return u_id; }
        }
        protected int pagesize { get { return 10; } }
        protected string parentName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
           　

            if (!IsPostBack)
            {
                if (u_type==2)
                {
                    SetRefferUrl();
                    InitData();
                }
                else
                {
                    if (Request.UrlReferrer != null)
                    {
                        Response.Redirect(Request.UrlReferrer.ToString() + "#fail=您没有权限访问哦");
                    }
                    else
                    {
                        Response.Redirect(siteConfig.webpath + "error.aspx?m_id=" + m_id);
                    }
                }
              　
              
            }
        }

        private void SetRefferUrl()
        {
            //try
            //{
            //    hfReturlUrl.Value = Request.UrlReferrer.ToString();
            //}
            //catch (Exception)
            //{
            hfReturlUrl.Value = "regoin_account.aspx?m_id=" + m_id;
            //}
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
            string[] estr = e.CommandArgument.ToString().Split(',');
            int id = int.Parse(estr[0]);
            string name = estr[1];

            if (e.CommandName == "Disable")
            {

                //int id = KDWechat.Common.Utils.StrToInt(e.CommandArgument.ToString(), 0);
                Status status = Status.禁用;
                if (((Button)e.CommandSource).Text == "启用")
                {
                    status = Status.正常;
                }
                BLL.Users.sys_users.SetUserStatus(id, status);
                AddLog(string.Format("{0}公众账号:{1}", status == Status.正常 ? "启用" : "停用", name), LogType.修改);
                Response.Redirect(Request.Url.ToString());
            }
            else if (e.CommandName == "del")  //删除一条数据（执行事务,删除该子帐号的所有信息，慎重使用）
            {
               

                if (BLL.Users.sys_users.DelAccountChild(id))
                {

                    AddLog(string.Format("删除了子帐号:{0}", name), LogType.删除);
                    JsHelper.AlertAndRedirect("子帐号删除成功", hfReturlUrl.Value);
                }
                else
                    JsHelper.Alert("子帐号删除失败");
            }
        }

        protected string GetConfirmString(object status) 
        {
            string statusString = "启用";
            if (status.ToString() == "1")
                statusString = "禁用";
            return "return confirm('您确定要" + statusString + "这个账号？')";
        }

    }
}