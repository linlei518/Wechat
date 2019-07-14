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

namespace KDWechat.Web.main
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
                if (u_type == 1 || u_type == 4)
                {
                    WriteReturnPage(hfReturlUrl, "ManageAccount.aspx?m_id=96");
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




        private void InitData()
        {
            radStatusOk.Checked = true;
            if (id != 0)
            {

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

                if (user.flag == (int)UserFlag.总部账号)
                {

                }
                if (user.is_manage_user_role == (int)Status.正常)
                    rboManageUserOk.Checked = true;
                else
                    rboManageUserFlase.Checked = true;

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
                        case "Reply":
                            strHtml.Append("<label><input id=\"" + parent_action_name + "_" + actions[i] + "\" type=\"checkbox\" " + (ckeck_role(action_type_list, parent_action_name + "_" + actions[i]) == true ? "checked=\"checked\"" : "") + "  class=\"checkbox\">回复</label>");
                            break;
                        case "Aduit":
                            strHtml.Append("<label><input id=\"" + parent_action_name + "_" + actions[i] + "\" type=\"checkbox\" " + (ckeck_role(action_type_list, parent_action_name + "_" + actions[i]) == true ? "checked=\"checked\"" : "") + "  class=\"checkbox\">审核</label>");
                            break;
                        case "Manage":
                            strHtml.Append("<label><input id=\"" + parent_action_name + "_" + actions[i] + "\" type=\"checkbox\" " + (ckeck_role(action_type_list, parent_action_name + "_" + actions[i]) == true ? "checked=\"checked\"" : "") + "  class=\"checkbox\">代管</label>");
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


            #region 总部帐号,次权限精确到各个操作，已不用
            /*
            StringBuilder strHtml = new StringBuilder();

            List<DAL.t_sys_navigation> list = BLL.Users.sys_navigation.GetListByParentId(50, (int)UserFlag.总部账号);

            DAL.t_sys_users_power power = new t_sys_users_power
            {
                action_type = "none",
                nav_name = "none",
                u_id = u_id,
                wx_id = wx_id
            };
            if (uid > 0)
            {
                power = BLL.Users.sys_users_power.GetPowerRole(0, uid,50);
            }
            strHtml.Append("<li>");
            strHtml.Append("<h3>");
            strHtml.Append("<input type=\"button\" class=\"btn btnOpen\">"); //所有

            if (power.action_type != "none")
            {
                strHtml.Append("<label><input id=\"all\" type=\"checkbox\" class=\"checkbox\" name=\"root\" checked=\"checked\" >所有系统</label>");
            }
            else
            {
                strHtml.Append("<label><input id=\"all\" type=\"checkbox\" class=\"checkbox\" name=\"root\">所有系统</label>");
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
             * */
            #endregion

            #region 总部账户，精确到详细操作

            List<DAL.t_sys_navigation> child_sys_list_admin = Companycn.Core.EntityFramework.EFHelper.GetList<DAL.creater_wxEntities, t_sys_navigation, int?>(x => x.parent_id == 0 && (x.type_id == 0 || x.type_id == 2) && x.is_lock == 0, x => x.sort_id, int.MaxValue, 1);




            StringBuilder strHtml = new StringBuilder();



            #region 循环加入其他子系统
            if (child_sys_list_admin != null)
            {
                string temp = "";
                foreach (var item in child_sys_list_admin)
                {
                    temp += item.name + ",";
                    #region 构造子系统



                    // strHtml.AppendLine(" <div id=\"" + item.name + "\"><div class=\"title\"> <h2>" + item.title + "</h2> </div>");
                    strHtml.AppendLine(" <div class=\"jurisdictionSet\"><ul>");

                    #region 子系统的菜单
                    strHtml.AppendLine("<li>");
                    strHtml.AppendLine("<h3>");
                    strHtml.AppendLine("<input type=\"button\" class=\"btn btnOpen\">"); //公众号
                    DAL.t_sys_users_power power = null;
                    if (uid > 0)
                    {
                        power = Companycn.Core.EntityFramework.EFHelper.GetModel<DAL.creater_wxEntities, t_sys_users_power>(x => x.nav_type_id == item.id && x.u_id == uid && x.nav_type_id == item.id);
                    }
                    if (power == null)
                    {
                        power = new t_sys_users_power();
                        power.action_type = "none";
                        power.nav_name = "none";
                        power.u_id = uid;
                        power.wx_id = wx_id;
                    }

                    if (power.action_type != "none")
                    {
                        strHtml.AppendLine("<label><input id=\"" + item.id + "\" type=\"checkbox\" class=\"checkbox\" name=\"" + item.name + "\" checked=\"checked\" >" + item.title + "</label>");
                    }
                    else
                    {
                        strHtml.AppendLine("<label><input id=\"" + item.id + "\" type=\"checkbox\" class=\"checkbox\" name=\"" + item.name + "\" >" + item.title + "</label>");
                    }
                    strHtml.AppendLine("</h3>");




                    List<DAL.t_sys_navigation> list = BLL.Users.sys_navigation.GetListByParentId(item.id);              //一级菜单
                    int len = list.Count;
                    if (len > 0)
                    {
                        strHtml.AppendLine("<ul>");
                        for (int j = 0; j < len; j++)
                        {
                            #region 一级菜单
                            string nav_name = list[j].name;//当前权限

                            strHtml.AppendLine("<li>");
                            strHtml.AppendLine("<h3>");
                            strHtml.AppendLine("<input type=\"button\" class=\"btn btnOpen\">");
                            if (ckeck_role(power.action_type, nav_name))
                            {
                                strHtml.AppendLine("<label><input id=\"" + list[j].name + "\" type=\"checkbox\" class=\"checkbox\" checked=\"checked\">" + list[j].title + "</label>");
                            }
                            else
                            {
                                strHtml.AppendLine("<label><input id=\"" + list[j].name + "\" type=\"checkbox\" class=\"checkbox\">" + list[j].title + "</label>");
                            }
                            strHtml.AppendLine("</h3>");
                            #endregion

                            strHtml.AppendLine("<ul>");

                            int parId = list[j].id;
                            List<DAL.t_sys_navigation> listchild = BLL.Users.sys_navigation.GetListByParentId(parId);  //二级菜单
                            int lench = listchild.Count;
                            if (lench > 0)
                            {

                                #region 二级菜单
                                for (int k = 0; k < lench; k++)
                                {

                                    bool is_three = false;
                                    //查找3级菜单
                                    List<DAL.t_sys_navigation> listthree = BLL.Users.sys_navigation.GetListByParentId(listchild[k].id);  //
                                    if (listthree != null && listthree.Count > 0)
                                    {
                                        is_three = true;
                                    }
                                    string nav_name_child = listchild[k].name;
                                    string action_type = listchild[k].action_type;
                                    strHtml.AppendLine("<li>");
                                    strHtml.AppendLine("<h3>");
                                    strHtml.AppendLine("<input type=\"button\" class=\"btn btnOpen\">");
                                    //if (nav_name == "wechat_system" || nav_name == "wechat_systempub")
                                    //{
                                    //    strHtml.AppendLine("<label><input id=\"" + list[j].name + "\" type=\"checkbox\" class=\"checkbox\" checked=\"checked\" disabled>" + list[j].title + "</label>");
                                    //}
                                    //else 
                                    if (ckeck_role(power.action_type, nav_name_child))
                                    {
                                        strHtml.AppendLine("<label><input id=\"" + listchild[k].name + "\" type=\"checkbox\" class=\"checkbox\" checked=\"checked\">" + listchild[k].title + "</label>");
                                    }
                                    else
                                    {
                                        strHtml.AppendLine("<label><input id=\"" + listchild[k].name + "\" type=\"checkbox\" class=\"checkbox\">" + listchild[k].title + "</label>");
                                    }


                                    strHtml.AppendLine("</h3>");

                                    #region 添加三级菜单和操作
                                    if (!is_three)
                                    {
                                        #region 没3级菜单,添加操作
                                        strHtml.AppendLine(GetActionHtml(power.action_type, action_type, nav_name_child));

                                        #endregion
                                    }
                                    else
                                    {
                                        strHtml.AppendLine("<ul>");
                                        #region 循环3级菜单
                                        foreach (DAL.t_sys_navigation three in listthree)
                                        {
                                            strHtml.AppendLine("<li>");
                                            strHtml.AppendLine("<h3>");
                                            strHtml.AppendLine("<input type=\"button\" class=\"btn btnOpen\">");
                                            if (ckeck_role(power.action_type, three.name))
                                            {
                                                strHtml.AppendLine("<label><input id=\"" + three.name + "\" type=\"checkbox\" class=\"checkbox\" checked=\"checked\">" + three.title + "</label>");
                                            }
                                            else
                                            {
                                                strHtml.AppendLine("<label><input id=\"" + three.name + "\" type=\"checkbox\" class=\"checkbox\">" + three.title + "</label>");
                                            }


                                            strHtml.AppendLine("</h3>");
                                            #region 添加操作
                                            strHtml.AppendLine(GetActionHtml(power.action_type, three.action_type, three.name));

                                            #endregion
                                            strHtml.AppendLine("</li>");

                                        }
                                        #endregion
                                        strHtml.AppendLine("</ul>");


                                    }
                                    #endregion

                                    strHtml.AppendLine("</li>");




                                }
                                #endregion
                            }
                            else
                            {
                                #region 添加操作
                                strHtml.AppendLine(GetActionHtml(power.action_type, list[j].action_type, list[j].name, false));

                                #endregion
                            }

                            strHtml.AppendLine("</ul>");
                            strHtml.AppendLine("</li>");
                        }
                        strHtml.AppendLine("</ul>");
                    }

                    strHtml.AppendLine("</li>");
                    #endregion

                    strHtml.AppendLine("   </ul> </div>   ");



                    #endregion
                }
                hfchild_sys_name.Value = temp.TrimEnd(',');
            }
            #endregion


            litRole.Text = strHtml.ToString();
            #endregion

            #region 地区帐号(子系统的超级管理员)


            string nav_type_ids = "";
            if (uid > 0)  //有用户再去查库
            {
                DAL.t_sys_user_manage_child_sys manage_child_sys = Companycn.Core.EntityFramework.EFHelper.GetModel<DAL.creater_wxEntities, t_sys_user_manage_child_sys>(x => x.u_id == uid);
                if (manage_child_sys != null)
                {
                    nav_type_ids = manage_child_sys.nav_type_ids;
                }
            }
            List<DAL.t_sys_navigation> child_sys_list = Companycn.Core.EntityFramework.EFHelper.GetList<DAL.creater_wxEntities, t_sys_navigation, int?>(x => x.parent_id == 0 && (x.type_id == 1 || x.type_id == 2) && x.is_lock == 0, x => x.sort_id, int.MaxValue, 1);




            StringBuilder role_html = new StringBuilder();
            role_html.Append("<li>");
            role_html.Append("<h3>");
            role_html.Append("<input type=\"button\" class=\"btn btnOpen\">"); //所有

            if (!string.IsNullOrEmpty(nav_type_ids))
            {
                role_html.Append("<label><input id=\"all_child_sys\" type=\"checkbox\" class=\"checkbox\" name=\"root_child_sys\" checked=\"checked\" >所有系统</label>");
            }
            else
            {
                role_html.Append("<label><input id=\"all_child_sys\" type=\"checkbox\" class=\"checkbox\" name=\"root_child_sys\">所有系统</label>");
            }
            role_html.Append("</h3>");
            role_html.Append("<ul>");


            #region 循环加入其他子系统
            if (child_sys_list != null)
            {
                foreach (var item in child_sys_list)
                {

                    role_html.Append("<li>");
                    role_html.Append("<h3>");
                    if (nav_type_ids.Contains("," + item.id + ","))
                    {
                        role_html.Append("<label><input id=\"" + item.name + "\" type=\"checkbox\"  value=\"" + item.id + "\" class=\"checkbox\" checked=\"checked\">" + item.title + "</label>");
                    }
                    else
                    {
                        role_html.Append("<label><input id=\"" + item.name + "\" type=\"checkbox\"  value=\"" + item.id + "\" class=\"checkbox\">" + item.title + "</label>");
                    }
                    role_html.Append("</h3>");
                    role_html.Append("</li>");
                }
            }
            #endregion

            role_html.Append("</ul>");
            role_html.Append("</li>");

            lblChildSysRole.Text = role_html.ToString();
            #endregion
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
            int is_manage_user_role = rboManageUserOk.Checked ? (int)Status.正常 : (int)Status.禁用;
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
                is_manage_user_role=is_manage_user_role,
                salt = salt,
                user_pwd = password,
                status = status,
                parent_id = 0
            };

            user = BLL.Users.sys_users.InsertUser(user);
            if (user.id == 0)
                JsHelper.Alert(Page, "系统账号添加失败，请重试", "true");
            else
            {
                #region =======================添加权限
                if (user.flag == (int)UserFlag.总部账号)
                {
                    //DAL.t_sys_users_power power = new t_sys_users_power();
                    //power.u_id = user.id;
                    //power.action_type = (hfactions.Value.Trim() == "" ? "" : ",") + hfactions.Value;
                    //power.wx_id = 0;
                    //power.nav_type_id = 50;
                    //power.nav_name = Utils.DropHTML(user.user_name);
                    //BLL.Users.sys_users_power.InsertPower(power);

                    //具体权限
                    string child_sys_id = ",";
                    string[] child_list = hfactions.Value.Split(new string[] { "~!@#" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string child in child_list)
                    {
                        DAL.t_sys_users_power power = new t_sys_users_power();
                        string[] c_list = child.Split('|');
                        power.u_id = user.id;
                        power.action_type = (c_list[1].Trim() == "" ? "" : ",") + c_list[1];
                        power.wx_id = 0;
                        power.nav_name = user.user_name;
                        power.nav_type_id = Common.Utils.StrToInt(c_list[0], 0);
                        BLL.Users.sys_users_power.InsertPower(power);
                        child_sys_id += power.nav_type_id + ",";
                    }

                    //能管理的系统
                    DAL.t_sys_user_manage_child_sys manage_child_sys = new t_sys_user_manage_child_sys();
                    manage_child_sys.u_id = user.id;
                    manage_child_sys.nav_type_ids = child_sys_id;
                    Companycn.Core.EntityFramework.EFHelper.AddModel<DAL.creater_wxEntities, t_sys_user_manage_child_sys>(manage_child_sys);

                }
                else if (user.flag == (int)UserFlag.地区账号)
                {
                    DAL.t_sys_user_manage_child_sys power = new t_sys_user_manage_child_sys();
                    power.u_id = user.id;
                    power.nav_type_ids = (hfChildSysRole.Value.Trim() == "" ? "" : ",") + hfChildSysRole.Value;
                    Companycn.Core.EntityFramework.EFHelper.AddModel<DAL.creater_wxEntities, t_sys_user_manage_child_sys>(power);

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
            int is_manage_user_role = rboManageUserOk.Checked ? (int)Status.正常 : (int)Status.禁用;
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
            user.is_manage_user_role=is_manage_user_role;

            if (!BLL.Users.sys_users.UpdateUsers(user))
                JsHelper.Alert(Page, "系统账号修改失败，请重试", "true");
            else
            {
                #region =======================添加权限
                if (user.flag == (int)UserFlag.总部账号)
                {
                    string child_sys_id = ",";
                    //具体权限
                    if (BLL.Users.sys_users_power.DeletePowerByUID(id))
                    {
                        string[] child_list = hfactions.Value.Split(new string[] { "~!@#" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string child in child_list)
                        {
                            DAL.t_sys_users_power power = new t_sys_users_power();
                            string[] c_list = child.Split('|');
                            power.u_id = id;
                            power.action_type = (c_list[1].Trim() == "" ? "" : ",") + c_list[1];
                            power.wx_id = 0;
                            power.nav_name = user.user_name;
                            power.nav_type_id = Common.Utils.StrToInt(c_list[0], 0);
                            BLL.Users.sys_users_power.InsertPower(power);
                            child_sys_id += power.nav_type_id + ",";
                        }

                    }

                    //能管理的系统
                    DAL.t_sys_user_manage_child_sys manage_child_sys = Companycn.Core.EntityFramework.EFHelper.GetModel<DAL.creater_wxEntities, t_sys_user_manage_child_sys>(x => x.u_id == id);
                    if (manage_child_sys != null)
                    {
                        manage_child_sys.nav_type_ids = child_sys_id;
                        Companycn.Core.EntityFramework.EFHelper.UpdateModel<DAL.creater_wxEntities, t_sys_user_manage_child_sys>(manage_child_sys);
                  
                    }
                    else
                    {
                        manage_child_sys = new t_sys_user_manage_child_sys();
                        manage_child_sys.u_id = user.id;
                        manage_child_sys.nav_type_ids = child_sys_id;
                        Companycn.Core.EntityFramework.EFHelper.AddModel<DAL.creater_wxEntities, t_sys_user_manage_child_sys>(manage_child_sys);
                    }

                }
                else if (user.flag == (int)UserFlag.地区账号)
                {
                    DAL.t_sys_user_manage_child_sys power = Companycn.Core.EntityFramework.EFHelper.GetModel<DAL.creater_wxEntities, t_sys_user_manage_child_sys>(x => x.u_id == id);
                    if (power != null)
                    {
                        power.nav_type_ids = (hfChildSysRole.Value.Trim() == "" ? "" : ",") + hfChildSysRole.Value;
                        Companycn.Core.EntityFramework.EFHelper.UpdateModel<DAL.creater_wxEntities, t_sys_user_manage_child_sys>(power);
                    }
                    else
                    {
                        power = new t_sys_user_manage_child_sys();
                        power.u_id = id;
                        power.nav_type_ids = (hfChildSysRole.Value.Trim() == "" ? "" : ",") + hfChildSysRole.Value;
                        Companycn.Core.EntityFramework.EFHelper.AddModel<DAL.creater_wxEntities, t_sys_user_manage_child_sys>(power);
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