using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.DAL;
using KDWechat.Common;
using KDWechat.Web.UI;
using System.Text;

namespace KDWechat.Web
{
    public partial class NewAccount : BasePage
    {


        protected int id
        {
            get { return RequestHelper.GetQueryInt("id", 0); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                WriteReturnPage(hfReturlUrl, "ManageAccount.aspx?m_id=96");
                InitData();

            }
        }




        private void InitData()
        {
            radStatusOk.Checked = true;
            if (id != 0)
            {
                CheckUserAuthority("sys_account", RoleActionType.Edit, hfReturlUrl.Value);
                var user = BLL.Users.sys_users.GetUserByID(id);

                ddlAreaType.SelectedValue = user.area.ToString();
                txtDepartment.Text = user.dept_name;
                txtEmail.Text = user.email;
                ddlUserFlag.SelectedValue = user.flag.ToString();
                txtMobil.Text = user.mobile;
                txtRealName.Text = user.real_name;
                txtTel.Text = user.tel;
                ddlBusssinessType.SelectedValue = user.type_id.ToString();
                txtAccountName.Text = user.user_name;
                txtAccountName.Enabled = false;
                txtPassword.Attributes.Add("value", DESEncrypt.Decrypt(user.user_pwd, user.salt));
                if (user.status == (int)Status.正常)
                    radStatusOk.Checked = true;
                else
                    radStatusFalse.Checked = true;

                if (user.flag==(int)UserFlag.总部账号)
                {

                }

            }
            else
            {
                CheckUserAuthority("sys_account", RoleActionType.Add, hfReturlUrl.Value);
            }

            BindRoel();
        }

        #region 权限加载

        private bool ckeck_role(string action_type_list, string action_type)
        {
            bool isc = false;

            if (string.IsNullOrEmpty(action_type_list) == false && string.IsNullOrEmpty(action_type) == false)
            {
                string[] lists = action_type_list.TrimEnd(',').TrimStart(',').Split(new char[] { ',' });
                string[] list_two = action_type.TrimEnd(',').TrimStart(',').Split(new char[] { ',' });

                for (int i = 0; i < lists.Length; i++)
                {
                    if (isc)
                    {
                        break;
                    }
                    for (int j = 0; j < list_two.Length; j++)
                    {
                        if (lists[i].Trim() == list_two[j].Trim())
                        {
                            isc = true;
                            break;
                        }
                    }
                }
            }


            return isc;
        }

        /// <summary>
        ///  获取动作
        /// </summary>
        /// <param name="action_type_list">用户的动作列表</param>
        /// <param name="action_type">当前菜单的所有动作</param>
        /// <param name="parent_action_name">当前菜单的动作</param>
        /// <returns></returns>
        private string GetActionHtml(string action_type_list, string action_type, string parent_action_name, bool isc_add = true)
        {
            StringBuilder strHtml = new StringBuilder();
            #region  添加操作

            if (!string.IsNullOrEmpty(action_type))
            {
                if (isc_add)
                {
                    strHtml.Append("<ul>");
                }

                string[] actions = action_type.Split(new char[] { ',' });

                for (int i = 0; i < actions.Length; i++)
                {
                    strHtml.Append("<li>");
                    strHtml.Append("<h3>");

                    switch (actions[i])
                    {
                        case "View":
                            strHtml.Append("<label><input id=\"" + parent_action_name + "_" + actions[i] + "\" type=\"checkbox\" " + (ckeck_role(action_type_list, parent_action_name + "_" + actions[i]) == true ? "checked=\"checked\"" : "") + " class=\"checkbox\">查看</label>");
                            break;
                        case "Add":
                            strHtml.Append("<label><input id=\"" + parent_action_name + "_" + actions[i] + "\" type=\"checkbox\" " + (ckeck_role(action_type_list, parent_action_name + "_" + actions[i]) == true ? "checked=\"checked\"" : "") + "  class=\"checkbox\">添加</label>");
                            break;
                        case "Edit":
                            strHtml.Append("<label><input id=\"" + parent_action_name + "_" + actions[i] + "\" type=\"checkbox\" " + (ckeck_role(action_type_list, parent_action_name + "_" + actions[i]) == true ? "checked=\"checked\"" : "") + "  class=\"checkbox\">修改</label>");
                            break;
                        case "Delete":
                            strHtml.Append("<label><input id=\"" + parent_action_name + "_" + actions[i] + "\" type=\"checkbox\" " + (ckeck_role(action_type_list, parent_action_name + "_" + actions[i]) == true ? "checked=\"checked\"" : "") + "  class=\"checkbox\">删除</label>");
                            break;
                        case "Import":
                            strHtml.Append("<label><input id=\"" + parent_action_name + "_" + actions[i] + "\" type=\"checkbox\" " + (ckeck_role(action_type_list, parent_action_name + "_" + actions[i]) == true ? "checked=\"checked\"" : "") + "  class=\"checkbox\">导入</label>");
                            break;
                        case "Export":
                            strHtml.Append("<label><input id=\"" + parent_action_name + "_" + actions[i] + "\" type=\"checkbox\" " + (ckeck_role(action_type_list, parent_action_name + "_" + actions[i]) == true ? "checked=\"checked\"" : "") + "  class=\"checkbox\">导出</label>");
                            break;
                        case "Release":
                            strHtml.Append("<label><input id=\"" + parent_action_name + "_" + actions[i] + "\" type=\"checkbox\" " + (ckeck_role(action_type_list, parent_action_name + "_" + actions[i]) == true ? "checked=\"checked\"" : "") + "  class=\"checkbox\">发布</label>");
                            break;
                    }
                    strHtml.Append("</h3>");
                    strHtml.Append("</li>");
                }
                if (isc_add)
                {
                    strHtml.Append("</ul>");
                }
            }
            #endregion

            return strHtml.ToString();
        }


        /// <summary>
        /// 绑定公众号及其模块
        /// </summary>
        protected void BindRoel()
        {
            int uid = this.id;//被设置用户

            StringBuilder strHtml = new StringBuilder();

            List<DAL.t_sys_navigation> list = BLL.Users.sys_navigation.GetListByParentId(50, (int)UserFlag.总部账号);

            DAL.t_sys_users_power power = BLL.Users.sys_users_power.GetPowerRole(0, uid);

            strHtml.Append("<li>");
            strHtml.Append("<h3>");
            strHtml.Append("<input type=\"button\" class=\"btn btnOpen\">"); //所有

            if (power.action_type != "none")
            {
                strHtml.Append("<label><input id=\"all\" type=\"checkbox\" class=\"checkbox\" name=\"root\" checked=\"checked\" >所有权限</label>");
            }
            else
            {
                strHtml.Append("<label><input id=\"all\" type=\"checkbox\" class=\"checkbox\" name=\"root\">所有权限</label>");
            }
            strHtml.Append("</h3>");
            if (list != null && list.Count > 0)
            {
                strHtml.Append("<ul>");
                for (int j = 0; j < list.Count; j++)
                {

                    #region 一级菜单
                    string nav_name = list[j].name;//当前权限

                    strHtml.Append("<li>");
                    strHtml.Append("<h3>");
                    strHtml.Append("<input type=\"button\" class=\"btn btnOpen\">");
                    if (ckeck_role(power.action_type, nav_name))
                    {
                        strHtml.Append("<label><input id=\"" + list[j].name + "\" type=\"checkbox\" class=\"checkbox\" checked=\"checked\">" + list[j].title + "</label>");
                    }
                    else
                    {
                        strHtml.Append("<label><input id=\"" + list[j].name + "\" type=\"checkbox\" class=\"checkbox\">" + list[j].title + "</label>");
                    }
                    strHtml.Append("</h3>");
                    #endregion

                    strHtml.Append("<ul>");

                    int parId = list[j].id;
                    List<DAL.t_sys_navigation> listchild = BLL.Users.sys_navigation.GetListByParentId(parId, (int)UserFlag.总部账号);  //二级菜单
                    int lench = listchild.Count;
                    if (lench > 0)
                    {

                        for (int k = 0; k < lench; k++)
                        {

                            bool is_three = false;
                            //查找3级菜单
                            List<DAL.t_sys_navigation> listthree = BLL.Users.sys_navigation.GetListByParentId(listchild[k].id, (int)UserFlag.总部账号);  //二级菜单
                            if (listthree != null && listthree.Count > 0)
                            {
                                is_three = true;
                            }
                            string nav_name_child = listchild[k].name;
                            string action_type = listchild[k].action_type;
                            strHtml.Append("<li>");
                            strHtml.Append("<h3>");
                            strHtml.Append("<input type=\"button\" class=\"btn btnOpen\">");

                            if (ckeck_role(power.action_type, nav_name_child))
                            {
                                strHtml.Append("<label><input id=\"" + listchild[k].name + "\" type=\"checkbox\" class=\"checkbox\" checked=\"checked\">" + listchild[k].title + "</label>");
                            }
                            else
                            {
                                strHtml.Append("<label><input id=\"" + listchild[k].name + "\" type=\"checkbox\" class=\"checkbox\">" + listchild[k].title + "</label>");
                            }


                            strHtml.Append("</h3>");

                            #region 添加三级菜单和操作
                            if (!is_three)
                            {
                                #region 没3级菜单,添加操作
                                strHtml.Append(GetActionHtml(power.action_type, action_type, nav_name_child));

                                #endregion
                            }
                            else
                            {
                                strHtml.Append("<ul>");
                                #region 循环3级菜单
                                foreach (DAL.t_sys_navigation three in listthree)
                                {
                                    strHtml.Append("<li>");
                                    strHtml.Append("<h3>");
                                    strHtml.Append("<input type=\"button\" class=\"btn btnOpen\">");
                                    if (ckeck_role(power.action_type, three.name))
                                    {
                                        strHtml.Append("<label><input id=\"" + three.name + "\" type=\"checkbox\" class=\"checkbox\" checked=\"checked\">" + three.title + "</label>");
                                    }
                                    else
                                    {
                                        strHtml.Append("<label><input id=\"" + three.name + "\" type=\"checkbox\" class=\"checkbox\">" + three.title + "</label>");
                                    }


                                    strHtml.Append("</h3>");
                                    #region 添加操作
                                    strHtml.Append(GetActionHtml(power.action_type, three.action_type, three.name));

                                    #endregion
                                    strHtml.Append("</li>");

                                }
                                #endregion
                                strHtml.Append("</ul>");


                            }
                            #endregion

                            strHtml.Append("</li>");




                        }
                    }
                    else
                    {
                        #region 添加操作
                        strHtml.Append(GetActionHtml(power.action_type, list[j].action_type, list[j].name, false));

                        #endregion
                    }

                    strHtml.Append("</ul>");
                    strHtml.Append("</li>");
                }



                strHtml.Append("</ul>");
            }
            strHtml.Append("</li>");
            litRole.Text = strHtml.ToString();
        }
        #endregion



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
                dept_name = Utils.DropHTML(txtDepartment.Text),
                email = Utils.DropHTML(txtEmail.Text),
                flag = int.Parse(ddlUserFlag.SelectedValue),
                login_ip = Utils.GetUserIp(),
                login_time = DateTime.Now,
                mobile = Utils.DropHTML(txtMobil.Text),
                real_name = Utils.DropHTML(txtRealName.Text),
                tel = Utils.DropHTML(txtTel.Text),
                type_id = int.Parse(ddlBusssinessType.SelectedValue),
                user_name = Utils.DropHTML(txtAccountName.Text),
                salt = salt,
                user_pwd = password,
                status = status,
                parent_id = 0
            };

            user = BLL.Users.sys_users.InsertUser(user);
            if (user.id == 0)
                JsHelper.Alert(Page,"系统账号添加失败，请重试","true");
            else
            {
                #region =======================添加权限
                if (user.flag == (int)UserFlag.总部账号)
                {
                    DAL.t_sys_users_power power = new t_sys_users_power();
                    power.u_id = user.id;
                    power.action_type = (hfactions.Value.Trim() == "" ? "" : ",") + hfactions.Value;
                    power.wx_id = 0;
                    power.nav_name = Utils.DropHTML(user.user_name);
                    BLL.Users.sys_users_power.InsertPower(power);

                }
                #endregion================================
                AddLog("添加了用户：" + user.user_name, LogType.添加);
                JsHelper.AlertAndRedirect("系统账号添加成功", hfReturlUrl.Value);
            }
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
            user.dept_name = Utils.DropHTML(txtDepartment.Text);
            user.email = Utils.DropHTML(txtEmail.Text);
            user.flag = int.Parse(ddlUserFlag.SelectedValue);
            user.login_ip = Utils.GetUserIp();
            user.login_time = DateTime.Now;
            user.mobile = Utils.DropHTML(txtMobil.Text);
            user.real_name = Utils.DropHTML(txtRealName.Text);
            user.tel = Utils.DropHTML(txtTel.Text);
            user.type_id = int.Parse(ddlBusssinessType.SelectedValue);
            user.user_name = Utils.DropHTML(txtAccountName.Text);
            user.salt = salt;
            user.user_pwd = password;
            user.status = status;
            user.parent_id = 0;

            if (!BLL.Users.sys_users.UpdateUsers(user))
                JsHelper.Alert(Page, "系统账号修改失败，请重试", "true");
            else
            {
                #region =======================添加权限
                if (user.flag == (int)UserFlag.总部账号)
                {
                    if (BLL.Users.sys_users_power.DeletePowerByUID(id))
                    {
                        DAL.t_sys_users_power power = new t_sys_users_power();
                        power.u_id = user.id;
                        power.action_type = (hfactions.Value.Trim() == "" ? "" : ",") + hfactions.Value;
                        power.wx_id = 0;
                        power.nav_name = Utils.DropHTML(user.user_name);
                        BLL.Users.sys_users_power.InsertPower(power);

                    }
                }
                #endregion================================

                AddLog("修改了用户：" + user.user_name, LogType.修改);
                JsHelper.AlertAndRedirect("保存成功", hfReturlUrl.Value);
            }
        }


        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(hfReturlUrl.Value);
        }


    }
}