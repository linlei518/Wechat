using System;
using KDWechat.Common;
using KDWechat.DAL;
using KDWechat.Web.UI;

namespace KDWechat.Web.activity
{
    public partial class contact_edit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                WriteReturnPage(hfReturnUrl, "contact_list");

                if (id > 0)
                {
                    CheckUserAuthority("contact_list", RoleActionType.Edit, "contact_list.aspx");
                    bindData();
                }
                else
                {
                    
                }
            }
        }


        private void bindData()
        {
            var model = DapperConnection.minebea.GetModel<t_contact>(new { id = id });
            if (model != null)
            {
                txtcompany_name.Text = model.company_name;
                txtname.Text = model.user_name;
                txtdpt_name.Text = model.dpt_name;
                txtemail.Text = model.email;
                txtphone.Text = model.phone;
                txtadress.Text = model.address;
                txtcontent.Text = model.content;
                txtproduct_menu.Text = model.product_menu;
                txtproduct_name.Text = model.product_name;
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