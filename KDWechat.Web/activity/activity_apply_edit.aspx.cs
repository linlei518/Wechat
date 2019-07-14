using System;
using KDWechat.Common;
using KDWechat.DAL;
using KDWechat.Web.UI;

namespace KDWechat.Web.activity
{
    public partial class activity_apply_edit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                WriteReturnPage(hfReturnUrl, "activity_apply_list");

                if (id > 0)
                {
                    CheckUserAuthority("activity_apply_list", RoleActionType.Edit, "activity_apply_list.aspx");
                    bindData();
                }
                else
                {
                    
                }
            }
        }


        private void bindData()
        {
            var model = DapperConnection.minebea.GetModel<t_draw_list>(new { id = id });
            if (model != null)
            {
                txtnick_name.Text = model.nick_name;
                txtname.Text = model.user_name;
                txtphone.Text = model.phone;
                head_img_url.ImageUrl = model.head_img_url;
                txtphone.Text = model.phone;
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