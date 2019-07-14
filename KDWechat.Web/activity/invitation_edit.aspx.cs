using System;
using KDWechat.Common;
using KDWechat.DAL;
using KDWechat.Web.UI;

namespace KDWechat.Web.activity
{
    public partial class invitation_edit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                WriteReturnPage(hfReturnUrl, "invitation_list");

                if (id > 0)
                {
                    CheckUserAuthority("invitation_list", RoleActionType.Edit, "invitation_list.aspx");
                    bindData();
                }
                else
                {
                    
                }
            }
        }


        private void bindData()
        {
            var model = DapperConnection.minebea.GetModel<t_invitation>(new { id = id });
            if (model != null)
            {
                txtcompany_name.Text = model.company_name;
                txtname.Text = model.name;
                txtdpt_name.Text = model.dpt_name;
                txtemail.Text = model.email;
                txtphone.Text = model.phone;
                txtpost.Text = model.post;
                txtcreate_date.Text = model.create_date.ToString();
            }
            else
            {
                JscriptMsg("内容已不存在！", hfReturnUrl.Value);
                return;
            }
        }



       
        private class Imgs_Url
        {
            public string img_url { get; set; }
        }
    }
}