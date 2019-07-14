using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;

namespace KDWechat.Web.main
{
    public partial class index : UI.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                List<DAL.t_sys_navigation> list_child_sys = null;

                if (u_type == (int)UserFlag.超级管理员)
                {
                    list_child_sys = Companycn.Core.EntityFramework.EFHelper.GetList<DAL.creater_wxEntities, DAL.t_sys_navigation, int?>(x => x.parent_id == 0 && x.is_lock==0 && (x.type_id == 0 || x.type_id==2), x => x.sort_id, int.MaxValue, 1);
                }
                else
                {
                    if (user_manage_child_sys == null)
                    {
                        load_user_manage_child_sys();
                    }
                    if (user_manage_child_sys != null)
                    {
                        string[] temp_list = user_manage_child_sys.nav_type_ids.TrimStart(',').TrimEnd(',').Split(new char[] { ',' });
                        List<int> nav_ids = new List<int>();
                        for (int i = 0; i < temp_list.Length; i++)
                        {
                            int _id = Common.Utils.StrToInt(temp_list[i], 0);
                            nav_ids.Add(_id);
                        }

                        //List<DAL.t_sys_navigation> list_child_sys = Common.CacheHelper.Get("list_child_sys_" + user_manage_child_sys.nav_type_ids.TrimStart(',').TrimEnd(',').Replace(",", "_") + "_" + u_id) as List<DAL.t_sys_navigation>;
                        //if (list_child_sys == null)
                        //{
                        list_child_sys = Companycn.Core.EntityFramework.EFHelper.GetList<DAL.creater_wxEntities, DAL.t_sys_navigation, int?>(x => x.parent_id == 0 && nav_ids.Contains(x.id), x => x.sort_id, int.MaxValue, 1);
                        //    Common.CacheHelper.Insert("list_child_sys_" + user_manage_child_sys.nav_type_ids.TrimStart(',').TrimEnd(',').Replace(",", "_") + "_" + u_id, list_child_sys, 10);
                        //}


                    }
                }

                if (list_child_sys != null)
                {
                    //循环子系统
                    string temp = "";
                    var admin = GetAdminInfo();
                    if (admin == null)
                    {
                        Response.Redirect("/KDlogin/login.aspx");
                        return;
                    }
                    foreach (var item in list_child_sys)
                    {
                        string loginStr = u_id + "|" + u_type + "|" + u_name + "|" + Guid.NewGuid();
                        string url = item.sub_title;
                        if (url.Contains("?"))
                        {
                            url += "&t=" + Common.DESEncrypt.Encrypt(loginStr, "kd_sys_login");
                        }
                        else
                        {
                            url += "?t=" + Common.DESEncrypt.Encrypt(loginStr, "kd_sys_login");
                        }

                        if (u_type == 3)
                        {
                            DAL.t_sys_users_power power = BLL.Users.sys_users_power.GetPowerRole(-1, u_id, item.id);
                            if (power != null)
                            {
                                temp += "   <div class=\"textField\"><span class=\"navtext\"><a target=\"_blank\" href=\"" + item.link_url + "\" >" + item.title + "</a><img src='" + url + "' style='width: 1px; height:1px; border: none; background-color: transparent; color: transparent;' /></span></div>";
                               // temp += "<script href=\"" + url + "\"></script>";
                                // lblTopMenu.Text += "<li><a  target=\"_blank\" href=\"" + item.link_url + "\" >" + item.title + "</a></li>";
                            }
                        }
                        else
                        {
                            temp += "   <div class=\"textField\"><span class=\"navtext\"><a  target=\"_blank\" href=\"" + item.link_url + "\" >" + item.title + "</a><img src='" + url + "' style='width: 1px; height:1px;border: none; background-color: transparent; color: transparent;' /></span></div>";
                           // temp += "<script href=\"" + url + "\"></script>";

                            //lblTopMenu.Text += "<li><a target=\"_blank\" href=\"" + item.link_url + "\" >" + item.title + "</a></li>";
                        }

                    }
                    lblTopMenu2.Text = temp;
                }




            }
        }
    }
}