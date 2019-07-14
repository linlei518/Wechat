using KDWechat.Common;
using KDWechat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.Account
{
    public partial class create_new_children : Web.UI.BasePage
    {
        protected int id
        {
            get { return RequestHelper.GetQueryInt("id", 0); }
        }

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
                hfReturlUrl.Value = "ManageAccount.aspx";
            }
        }


        private void InitData()
        {
            radStatusOk.Checked = true;
            if (id != 0)
            {
                var user = BLL.Users.sys_users.GetUserByID(id);

                ddlAreaType.SelectedValue = user.area.ToString();
                txtDepartment.Text = user.dept_name;
                txtEmail.Text = user.email;
                txtMobil.Text = user.mobile;
                txtRealName.Text = user.real_name;
                txtTel.Text = user.tel;
                ddlBusssinessType.SelectedValue = user.type_id.ToString();
                txtAccountName.Text = user.user_name;
                txtAccountName.Enabled = false;
                if (user.status == (int)Status.正常)
                    radStatusOk.Checked = true;
                else
                    radStatusFalse.Checked = true;
            }
        }

        protected void SubmitButtom_Click(object sender, EventArgs e)
        {
            if (id != 0)
                UpdateUser();
            else
                InsertUser();
        }

        private void InsertUser()
        {
            string salt = Utils.CreateSalt();
            string password = Utils.CreatePassword(txtPassword.Text.Trim(), salt);
            int status = radStatusOk.Checked ? (int)Status.正常 : (int)Status.禁用;
            t_sys_users user = new t_sys_users()
            {
                area = int.Parse(ddlAreaType.SelectedValue),
                create_time = DateTime.Now,
                create_ip = Utils.GetUserIp(),
                dept_name = txtDepartment.Text,
                email = txtEmail.Text,
                flag = (int)UserFlag.子账号,
                login_ip = Utils.GetUserIp(),
                login_time = DateTime.Now,
                mobile = txtMobil.Text,
                real_name = txtRealName.Text,
                tel = txtTel.Text,
                type_id = int.Parse(ddlBusssinessType.SelectedValue),
                user_name = txtAccountName.Text,
                salt = salt,
                user_pwd = password,
                status = status,
                parent_id = wx_id
            };

            user = BLL.Users.sys_users.InsertUser(user);
            if (user.id == 0)
                JsHelper.Alert("子账号添加失败，请重试");
            else
                JsHelper.AlertAndRedirect("子账号添加成功", hfReturlUrl.Value);

        }

        private void UpdateUser()
        {
            string salt = Utils.CreateSalt();
            string password = Utils.CreatePassword(txtPassword.Text.Trim(), salt);
            int status = radStatusOk.Checked ? (int)Status.正常 : (int)Status.禁用;
            t_sys_users user = BLL.Users.sys_users.GetUserByID(id);
            user.area = int.Parse(ddlAreaType.SelectedValue);
            user.create_time = DateTime.Now;
            user.create_ip = Utils.GetUserIp();
            user.dept_name = txtDepartment.Text;
            user.email = txtEmail.Text;
            user.flag = (int)UserFlag.子账号;
            user.login_ip = Utils.GetUserIp();
            user.login_time = DateTime.Now;
            user.mobile = txtMobil.Text;
            user.real_name = txtRealName.Text;
            user.tel = txtTel.Text;
            user.type_id = int.Parse(ddlBusssinessType.SelectedValue);
            user.user_name = txtAccountName.Text;
            user.salt = salt;
            user.user_pwd = password;
            user.status = status;
            user.parent_id = wx_id;

            if (!BLL.Users.sys_users.UpdateUsers(user))
                JsHelper.Alert("子账号修改失败，请重试");
            else
                JsHelper.AlertAndRedirect("子账号修改成功", hfReturlUrl.Value);
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(hfReturlUrl.Value);
        }
    }

}
