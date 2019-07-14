using KDWechat.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.DAL;

namespace KDWechat.Web.main
{
    public partial class ChildrenAccount_Edit : KDWechat.Web.UI.BasePage
    {

        public string wx_style = "";

        protected int id
        {
            get { return RequestHelper.GetQueryInt("id", 0); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           

            if (!IsPostBack)
            {
                if (u_type == 2)
                {
                    base.WriteReturnPage(hfReturlUrl, "regoin_account.aspx?m_id=3");

                    InitData();
                    BindChatRoel();
                }
                else
                {
                    if (Request.UrlReferrer!=null)
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

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void InitData()
        {
            if (id != 0)
            {
                var user = BLL.Users.sys_users.GetUserByID(id);
                txtEmail.Text = user.email;
                txtMobil.Text = user.mobile;
                txtRealName.Text = user.real_name;
                txtTel.Text = user.tel;
                txtAccountName.Text = user.user_name;
                txtPassword.Attributes.Add("value", DESEncrypt.Decrypt(user.user_pwd, user.salt));
                txtAccountName.Enabled = false;
                hftitle.Value = user.user_name;
                rbl_only_op_self.SelectedValue = user.only_op_self.ToString();
            }
        }

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
                            strHtml.Append("<label><input id=\"" + parent_action_name + "_" + actions[i] + "\" type=\"checkbox\" " + (ckeck_role(action_type_list, parent_action_name + "_" + actions[i]) == true ? "checked=\"checked\"" : "") + "  class=\"checkbox\">管理</label>");
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
        protected void BindChatRoel()
        {
            int uid = this.id;//被设置用户



            string nav_type_ids = "";

            DAL.t_sys_user_manage_child_sys manage_child_sys = Companycn.Core.EntityFramework.EFHelper.GetModel<DAL.creater_wxEntities, t_sys_user_manage_child_sys>(x => x.u_id == u_id);
            if (manage_child_sys != null)
            {
                nav_type_ids = manage_child_sys.nav_type_ids;
            }


            #region 微信的公众号

            if (nav_type_ids.Contains(",1,"))
            {
                #region 地区主帐号有管理微信权限
                StringBuilder strHtml = new StringBuilder();

                List<DAL.t_wx_wechats> models = BLL.Chats.wx_wechats.GetListByUid(u_id);

                int lenth = models.Count;
                if (lenth > 0)
                {
                    for (int i = 0; i < lenth; i++)
                    {
                        int wxid = +models[i].id;//被设置公众号

                        #region 公众号
                        strHtml.AppendLine("<li>");
                        strHtml.AppendLine("<h3>");
                        strHtml.AppendLine("<input type=\"button\" class=\"btn btnOpen\">"); //公众号
                        DAL.t_sys_users_power power = null;
                        if (uid > 0)
                        {
                            power = BLL.Users.sys_users_power.GetPowerRole(wxid, uid,1);
                        }
                        if (power == null)
                        {
                            power = new t_sys_users_power();
                            power.action_type = "none";
                            power.nav_name = "none";
                            power.u_id = u_id;
                            power.wx_id = wx_id;
                        }

                        if (power.action_type != "none")
                        {
                            strHtml.AppendLine("<label><input id=\"" + models[i].id + "\" type=\"checkbox\" class=\"checkbox\" name=\"root\" checked=\"checked\" >" + models[i].wx_pb_name + "</label>");
                        }
                        else
                        {
                            strHtml.AppendLine("<label><input id=\"" + models[i].id + "\" type=\"checkbox\" class=\"checkbox\" name=\"root\">" + models[i].wx_pb_name + "</label>");
                        }
                        strHtml.AppendLine("</h3>");
                        #endregion



                        List<DAL.t_sys_navigation> list = BLL.Users.sys_navigation.GetListByParentId(1, (int)UserFlag.子账号);              //一级菜单
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
                                if (nav_name == "wechat_system" || nav_name == "wechat_systempub")
                                {
                                    strHtml.AppendLine("<label><input id=\"" + list[j].name + "\" type=\"checkbox\" class=\"checkbox\" checked=\"checked\" disabled>" + list[j].title + "</label>");
                                }
                                else if (ckeck_role(power.action_type, nav_name))
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
                                List<DAL.t_sys_navigation> listchild = BLL.Users.sys_navigation.GetListByParentId(parId, (int)UserFlag.子账号);  //二级菜单
                                int lench = listchild.Count;
                                if (lench > 0)
                                {

                                    for (int k = 0; k < lench; k++)
                                    {

                                        bool is_three = false;
                                        //查找3级菜单
                                        List<DAL.t_sys_navigation> listthree = BLL.Users.sys_navigation.GetListByParentId(listchild[k].id, (int)UserFlag.子账号);  //二级菜单
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
                    }
                }

                litRole.Text = strHtml.ToString();
                #endregion
            }
            else
            {
                wx_style = "style='display:none'";
            }


            #endregion

            #region 子系统
            if (!string.IsNullOrEmpty(nav_type_ids))
            {
                string[] temp_list = nav_type_ids.TrimStart(',').TrimEnd(',').Split(new char[] { ',' });
                List<int> nav_ids = new List<int>();
                for (int i = 0; i < temp_list.Length; i++)
                {
                    int _id = Common.Utils.StrToInt(temp_list[i], 0);
                    if (_id > 1)
                    {
                        nav_ids.Add(_id);
                    }

                }

                StringBuilder strHtml = new StringBuilder();
                List<DAL.t_sys_navigation> list_child_sys = Companycn.Core.EntityFramework.EFHelper.GetList<DAL.creater_wxEntities, t_sys_navigation, int?>(x => x.parent_id == 0 && nav_ids.Contains(x.id), x => x.sort_id, int.MaxValue, 1);
                //循环子系统
                string temp = "";
                foreach (var item in list_child_sys)
                {
                    temp += item.name + ",";

                    #region 构造子系统



                    strHtml.AppendLine(" <div id=\"" + item.name + "\"><div class=\"title\"> <h2>" + item.title + "</h2> </div>");
                    strHtml.AppendLine(" <div class=\"jurisdictionSet\"><ul>");

                    #region 子系统的菜单
                    strHtml.AppendLine("<li>");
                    strHtml.AppendLine("<h3>");
                    strHtml.AppendLine("<input type=\"button\" class=\"btn btnOpen\">"); //公众号
                    DAL.t_sys_users_power power = null;
                    if (uid > 0)
                    {
                        power = Companycn.Core.EntityFramework.EFHelper.GetModel<DAL.creater_wxEntities, t_sys_users_power>(x => x.nav_type_id == item.id && x.u_id == uid);
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
                            List<DAL.t_sys_navigation> listchild = BLL.Users.sys_navigation.GetListByParentId(parId, (int)UserFlag.子账号);  //二级菜单
                            int lench = listchild.Count;
                            if (lench > 0)
                            {

                                #region 二级菜单
                                for (int k = 0; k < lench; k++)
                                {

                                    bool is_three = false;
                                    //查找3级菜单
                                    List<DAL.t_sys_navigation> listthree = BLL.Users.sys_navigation.GetListByParentId(listchild[k].id, (int)UserFlag.子账号);  //
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

                    strHtml.AppendLine("   </ul> </div>  </div>");



                    #endregion
                }
                hfchild_sys_name.Value = temp.TrimEnd(',');

                lblChildSysList.Text = strHtml.ToString();
            }



            #endregion

        }
        /// <summary>
        /// 判断用户是否有某个公众号的权限
        /// </summary>
        /// <param name="wxid"></param>
        /// <returns></returns>
        protected bool GetWXrole(string wxid)
        {
            int _wxid = Common.Utils.StrToInt(wxid, 0);
            return BLL.Users.sys_users_power.GetPowerRoleByWXID(_wxid, this.id);
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool is_add = false;
            t_sys_users user = BLL.Users.sys_users.GetUserByID(id);
            if (user == null)
            {
                is_add = true;
                user = new t_sys_users();
                user.create_time = DateTime.Now;
                user.login_ip = Utils.GetUserIp();
                user.login_time = DateTime.Now;
            }

            string salt = Utils.CreateSalt();
            string password = Utils.CreatePassword(Common.Utils.DropHTML(txtPassword.Text.Trim()), salt);
            user.area = GetAdminInfo().area;

            user.create_ip = Utils.GetUserIp();
            user.dept_name = "";
            user.email = Common.Utils.DropHTML(txtEmail.Text.Trim());
            user.flag = (int)Common.UserFlag.子账号;

            user.mobile = Common.Utils.DropHTML(txtMobil.Text.Trim());
            user.real_name = Common.Utils.DropHTML(txtRealName.Text.Trim());
            user.tel = Common.Utils.DropHTML(txtTel.Text.Trim());
            user.type_id = 0;
            user.user_name = Common.Utils.DropHTML(txtAccountName.Text.Trim());
            user.salt = salt;
            user.user_pwd = password;
            user.status = 1;
            user.parent_id = u_id;
            user.only_op_self = Common.Utils.StrToInt(rbl_only_op_self.SelectedValue, 0);
            if (is_add)
            {
                user = BLL.Users.sys_users.InsertUser(user);
                if (user == null || user.id == 0)
                {
                    JsHelper.AlertAndRedirect("子账号添加失败", hfReturlUrl.Value, "fail");
                    return;
                }
                else
                {
                    //添加权限
                    #region 微信权限

                    string[] p_list = hfactions.Value.Split(new string[] { "~!@#" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string child in p_list)
                    {
                        DAL.t_sys_users_power power = new t_sys_users_power();
                        string[] c_list = child.Split('|');
                        power.u_id = user.id;
                        power.action_type = (c_list[1].Trim() == "" ? "" : ",") + c_list[1];
                        power.wx_id = Common.Utils.StrToInt(c_list[0], 0);
                        power.nav_name = user.user_name;
                        power.nav_type_id = 1;
                        BLL.Users.sys_users_power.InsertPower(power);
                    }
                    #endregion

                    #region 子系统权限

                    string[] child_list = hfchild_sys_action.Value.Split(new string[] { "~!@#" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string child in child_list)
                    {
                        DAL.t_sys_users_power power = new t_sys_users_power();
                        string[] c_list = child.Split('|');
                        power.u_id = user.id;
                        power.action_type = (c_list[1].Trim() == "" ? "" : ",") + c_list[1];
                        power.wx_id = -1;
                        power.nav_name = user.user_name;
                        power.nav_type_id = Common.Utils.StrToInt(c_list[0], 0);
                        BLL.Users.sys_users_power.InsertPower(power);
                    }
                    #endregion

                    new KDWechat.Web.UI.BasePage().AddLog("创建子帐号,账号ID：" + user.id + ";账号名称：" + user.user_name + "", LogType.添加);
                    JsHelper.AlertAndRedirect("保存成功", hfReturlUrl.Value);
                }
            }
            else
            {
                bool res = BLL.Users.sys_users.UpdateUsers(user);
                if (!res)
                {
                    JsHelper.AlertAndRedirect("子账号修改失败", hfReturlUrl.Value, "fail");
                    return;
                }
                else
                {
                    //添加权限

                    BLL.Users.sys_users_power.DeletePowerByUID(user.id);
                    #region 微信权限
                    string[] p_list = hfactions.Value.Split(new string[] { "~!@#" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string child in p_list)
                    {
                        DAL.t_sys_users_power power = new t_sys_users_power();
                        string[] c_list = child.Split('|');
                        power.u_id = user.id;
                        power.action_type = (c_list[1].Trim() == "" ? "" : ",") + c_list[1];
                        power.wx_id = Common.Utils.StrToInt(c_list[0], 0);
                        power.nav_name = user.user_name;
                        power.nav_type_id = 1;
                        BLL.Users.sys_users_power.InsertPower(power);
                    }
                    #endregion

                    #region 子系统权限

                    string[] child_list = hfchild_sys_action.Value.Split(new string[] { "~!@#" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string child in child_list)
                    {
                        DAL.t_sys_users_power power = new t_sys_users_power();
                        string[] c_list = child.Split('|');
                        power.u_id = user.id;
                        power.action_type = (c_list[1].Trim() == "" ? "" : ",") + c_list[1];
                        power.wx_id = -1;
                        power.nav_name = user.user_name;
                        power.nav_type_id = Common.Utils.StrToInt(c_list[0], 0);
                        BLL.Users.sys_users_power.InsertPower(power);
                    }
                    #endregion

                    new KDWechat.Web.UI.BasePage().AddLog("修改子帐号,账号ID：" + user.id + ";账号名称：" + user.user_name + "", LogType.修改);

                    JsHelper.AlertAndRedirect("保存成功", hfReturlUrl.Value);

                }
            }
        }
    }
}