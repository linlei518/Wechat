using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.Account
{
    public partial class change_password : Web.UI.BasePage
    {
        protected string errorMsg = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (u_type<4)
            //{
            //    CheckUserAuthority((u_type == 1 ? "head_password" : "region_password"));
            //}
           

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
                hfReturlUrl.Value = "../kdlogin/login.aspx";
            }
        }


        private void InitData()
        {
            txtAccountName.Text = BLL.Users.sys_users.GetUserByID(u_id).user_name;
        }
        protected void SubmitButtom_Click(object sender, EventArgs e)
        {
            bool isComplete = false;
            var user = BLL.Users.sys_users.UserLogin(txtAccountName.Text, txtOldPassword.Text);
            if (null != user && txtPassword.Text == txtConfirmPasswrod.Text)
            {
                string secPassword = Common.Utils.CreatePassword(txtConfirmPasswrod.Text, user.salt);
                user.user_pwd = secPassword;
                isComplete = BLL.Users.sys_users.UpdateUsers(user);
            }
            else
            {
                //errorMsg="location.href=\"change_password.aspx?m_id=" + m_id.ToString()+"&success=子账号修改成功";
                Common.JsHelper.AlertAndRedirect("请输入正确的原密码", "change_password.aspx?m_id=" + m_id.ToString(),"fail");
                return;
            }
            if (isComplete)
                //errorMsg="location.href=\"change_password.aspx?m_id=" + m_id.ToString()+"&success=密码修改成功,请重新登陆！";
                Common.JsHelper.AlertAndRedirect("密码修改成功,请重新登陆", "../kdlogin/loginout.aspx");
            else
               Common.JsHelper.AlertAndRedirect("密码修改失败，请重试", "change_password.aspx?m_id=" + m_id.ToString(),"fail");
        }
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(hfReturlUrl.Value);
        }
    }
}