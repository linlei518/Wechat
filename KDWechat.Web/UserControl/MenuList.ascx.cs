using KDWechat.Common;
using KDWechat.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Web.UI;

namespace KDWechat.Web.UserControl
{
    public partial class MenuList : BaseControl
    {

        DAL.t_sys_users_power power = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                if (u_type > 0)
                {
                    if (m_id>0)
                    {
                        int parentId = -1;
                        if (m_id>1900)
                        {
                            parentId = 1;
                            power = BLL.Users.sys_users_power.GetPowerRole(wx_id, u_id,(wx_id>0?1:50));
                        }
                        else
                        {
                            DAL.t_sys_navigation model = Common.CacheHelper.Get("t_sys_navigation_" + m_id) as DAL.t_sys_navigation;
                            if (model == null)
                            {
                                model = BLL.Users.sys_navigation.GetNavigationByID(m_id);
                                Common.CacheHelper.Insert("t_sys_navigation_" + m_id, model);
                            }
                            if (model != null)
                            {

                                if (model.class_list.Contains(",1,"))
                                {
                                    if (wx_id<=0)
                                    {
                                        //new BasePage().AddLog("MenuList first,wx_id=" + wx_id, LogType.添加);
                                        Response.Redirect("/loginout.html");
                                        
                                        return;
                                    }
                                    power = BLL.Users.sys_users_power.GetPowerRole(wx_id, u_id,1);
                                    parentId = 1; //选择了公众号之后的菜单
                                }
                                else if (model.class_list.Contains(",50,"))
                                {
                                    parentId = 50;  //总部顶级ID
                                    power = BLL.Users.sys_users_power.GetPowerRole(0, u_id,50);
                                }
                                else if (model.class_list.Contains(",58,"))
                                {
                                    parentId = 58; //地区帐号登录后未选择微信号的时候
                                    //power = BLL.Users.sys_users_power.GetPowerRole(0, u_id);
                                    power = new t_sys_users_power();
                                    power.action_type = "none";
                                    power.nav_name = "none";
                                    power.u_id = u_id;
                                    power.wx_id = wx_id;
                                }


                            }
                        }
                       
                      
                        BindData(parentId); 
                    }
                      
                }
                else
                {
                   // new BasePage().AddLog("MenuList two,wx_id=" + wx_id, LogType.添加);
                    Response.Redirect("/loginout.html");
                }
            }
        }

        private string GetTarget(string url,int clid_count )
        {
            string str = "";
            if (clid_count==0 && url.ToLower().Contains("help.aspx"))
            {
                str = "target='_blank'";
            }
            return str;
        }
       

        /// <summary>
        /// 绑定菜单
        /// </summary>
        /// <param name="par_id"></param>
        protected void BindData(int par_id)
        {
            StringBuilder strHtml = new StringBuilder();


            List<t_sys_navigation> lists = Common.CacheHelper.Get("t_sys_navigation_list_"+par_id+"_"+u_id+"_"+wx_id) as List<t_sys_navigation>;// 
            if (lists==null)
            {
                lists = BLL.Users.sys_navigation.GetListByParentId(par_id);
                Common.CacheHelper.Insert("t_sys_navigation_list_" + par_id + "_"+u_id+"_"+wx_id, lists, 120);
            }
            int rowCount = lists.Count;
            if (rowCount > 0)
            {
                foreach (t_sys_navigation nav in lists)
                {
                    #region 判断子账号，不显示“子帐号管理”,“新建子帐号”和“日志管理”菜单
                    if (u_type == (int)UserFlag.子账号 && par_id == 58)
                    {
                        if (nav.id == 60   || nav.id == 68 || nav.id == 34)
                        {
                            continue;
                        }
                    } 
                    #endregion

                    if (par_id==1)
                    {
                        #region 选择公众号之后的菜单
                        //地区帐号，既是公众号创建者，所以不需要判断权限
                        if (u_type == (int)UserFlag.地区账号 || u_type == (int)UserFlag.超级管理员 || u_type == (int)UserFlag.总部账号)
                        {
                            List<t_sys_navigation> lists_child = Common.CacheHelper.Get("lists_child_" + nav.id + "_"+u_id+"_"+wx_id) as List<t_sys_navigation>;// 
                            if (lists_child==null)
                            {
                                lists_child = BLL.Users.sys_navigation.GetListByParentId(nav.id);
                                Common.CacheHelper.Insert("lists_child_" + nav.id + "_"+u_id+"_"+wx_id, lists_child, 120);
                            }
                            strHtml.Append("<li id=" + nav.id + ">");//class=\"current\"
                            strHtml.Append("<a  " + GetTarget(nav.link_url, lists_child.Count) + " id=" + nav.id + " " + (lists_child.Count == 0 ? nav.link_url.ToLower().Contains("help.aspx") == true ? "" : " onclick='dialogue.dlLoading();'" : "") + " href=\"" + (lists_child.Count > 0 ? "javascript:void(0)" : GetUrl(nav.link_url, nav.id)) + "\" >" + (lists_child.Count > 0 ? "<i class=\"hasChild\"></i><i class=\"" + (nav.remark != "" ? nav.remark : "navBase") + "\"></i>" : "<i class=\"" + (nav.remark != "" ? nav.remark : "navBase") + "\"></i>") + "" + nav.title + "</a>");
                            strHtml.Append(BindChild(lists_child, nav.id));
                            strHtml.Append("</li>");
                        }
                            //子帐号，需要判断权限
                        else if (power.action_type.Contains(","+nav.name+","))  //nav.id==32公众号基本页面，权限必须有
                        {
                            List<t_sys_navigation> lists_child = Common.CacheHelper.Get("lists_child_" + nav.id + "_"+u_id+"_"+wx_id) as List<t_sys_navigation>;// 
                            if (lists_child == null)
                            {
                                lists_child = BLL.Users.sys_navigation.GetListByParentId(nav.id);
                                Common.CacheHelper.Insert("lists_child_" + nav.id + "_"+u_id+"_"+wx_id, lists_child, 120);
                            }
                            strHtml.Append("<li id=" + nav.id + ">");//class=\"current\"
                            strHtml.Append("<a " + GetTarget(nav.link_url, lists_child.Count) + " id=" + nav.id + " " + (lists_child.Count == 0 ? nav.link_url.ToLower().Contains("help.aspx") == true ? "" : " onclick='dialogue.dlLoading();'" : "") + " href=\"" + (lists_child.Count > 0 ? "javascript:void(0)" : GetUrl(nav.link_url, nav.id)) + "\">" + (lists_child.Count > 0 ? "<i class=\"hasChild\"></i><i class=\"" + (nav.remark != "" ? nav.remark : "navBase") + "\"></i>" : "<i class=\"" + (nav.remark != "" ? nav.remark : "navBase") + "\"></i>") + "" + nav.title + "</a>");
                            strHtml.Append(BindChild(lists_child, nav.id));
                            strHtml.Append("</li>");
                        } 
                        #endregion
                    }
                    else if (par_id==50)
                    {
                        #region 系统超级管理员的菜单
                        if (u_type == (int)UserFlag.超级管理员  )
                        {
                            List<t_sys_navigation> lists_child = Common.CacheHelper.Get("lists_child_" + nav.id + "_"+u_id+"_"+wx_id) as List<t_sys_navigation>;// 
                            if (lists_child == null)
                            {
                                lists_child = BLL.Users.sys_navigation.GetListByParentId(nav.id);
                                Common.CacheHelper.Insert("lists_child_" + nav.id + "_"+u_id+"_"+wx_id, lists_child, 120);
                            }
                            strHtml.Append("<li id=" + nav.id + ">");//class=\"current\"
                            strHtml.Append("<a id=" + nav.id + " " + (lists_child.Count == 0 ? " onclick='dialogue.dlLoading();'" : "") + " href=\"" + (lists_child.Count > 0 ? "javascript:void(0)" : GetUrl(nav.link_url, nav.id)) + "\">" + (lists_child.Count > 0 ? "<i class=\"hasChild\"></i><i class=\"" + (nav.remark != "" ? nav.remark : "navBase") + "\"></i>" : "<i class=\"" + (nav.remark != "" ? nav.remark : "navBase") + "\"></i>") + "" + nav.title + "</a>");
                            strHtml.Append(BindChild(lists_child, nav.id));
                            strHtml.Append("</li>");
                        }
                        #endregion

                        #region 总部帐号的菜单
                        if (u_type == (int)UserFlag.总部账号)
                        {
                            if (power.action_type.Contains("," + nav.name + ","))
                            {
                                List<t_sys_navigation> lists_child = Common.CacheHelper.Get("lists_child_" + nav.id + "_"+u_id+"_"+wx_id) as List<t_sys_navigation>;// 
                                if (lists_child == null)
                                {
                                    lists_child = BLL.Users.sys_navigation.GetListByParentId(nav.id);
                                    Common.CacheHelper.Insert("lists_child_" + nav.id + "_"+u_id+"_"+wx_id, lists_child, 120);
                                }
                                strHtml.Append("<li id=" + nav.id + ">");//class=\"current\"
                                strHtml.Append("<a id=" + nav.id + " " + (lists_child.Count == 0 ? " onclick='dialogue.dlLoading();'" : "") + " href=\"" + (lists_child.Count > 0 ? "javascript:void(0)" : GetUrl(nav.link_url, nav.id)) + "\">" + (lists_child.Count > 0 ? "<i class=\"hasChild\"></i><i class=\"" + (nav.remark != "" ? nav.remark : "navBase") + "\"></i>" : "<i class=\"" + (nav.remark != "" ? nav.remark : "navBase") + "\"></i>") + "" + nav.title + "</a>");
                                strHtml.Append(BindChild(lists_child, nav.id));
                                strHtml.Append("</li>");
                            }
                        }
                        #endregion

                    }
                    else
                    {
                        #region 地区帐号和子帐号在未选择公众号之前的菜单
                        List<t_sys_navigation> lists_child = Common.CacheHelper.Get("lists_child_" + nav.id + "_"+u_id+"_"+wx_id) as List<t_sys_navigation>;// 
                        if (lists_child == null)
                        {
                            lists_child = BLL.Users.sys_navigation.GetListByParentId(nav.id);
                            Common.CacheHelper.Insert("lists_child_" + nav.id + "_"+u_id+"_"+wx_id, lists_child, 120);
                        }

                        //判断该帐号是否有微信公众号管理权限
                        if (user_manage_child_sys == null)
                        {
                            load_user_manage_child_sys();
                        }
                        //if (!user_manage_child_sys.nav_type_ids.Contains(",1,"))
                        //{
                        //    //没有微信公众号的权限,移除公众号管理的菜单
                        //    if (nav.id==59)
                        //    {
                        //        continue;
                        //    }
                        //}

                        strHtml.Append("<li id=" + nav.id + ">");//class=\"current\"
                        strHtml.Append("<a id=" + nav.id + "  " + (lists_child.Count == 0 ? " onclick='dialogue.dlLoading();'" : "") + " href=\"" + (lists_child.Count > 0 ? "javascript:void(0)" : GetUrl(nav.link_url, nav.id)) + "\">" + (lists_child.Count > 0 ? "<i class=\"hasChild\"></i><i class=\"" + (nav.remark != "" ? nav.remark : "navBase") + "\"></i>" : "<i class=\"" + (nav.remark != "" ? nav.remark : "navBase") + "\"></i>") + "" + nav.title + "</a>");
                        strHtml.Append(BindChild(lists_child, nav.id));
                        strHtml.Append("</li>"); 
                        #endregion
                    }
                    
                  
                }
                lit_nav.Text = strHtml.ToString();
            }
        }
        /// <summary>
        /// 绑定二级子菜单
        /// </summary>
        /// <param name="par_id"></param>
        protected string BindChild(List<t_sys_navigation> lists, int par_id)
        {
            StringBuilder strHtml = new StringBuilder();


            
            int rowCount = lists.Count;
            if (rowCount > 0)
            {
                strHtml.Append("<ul>");

                foreach (t_sys_navigation nav in lists)
                {
                    if ((nav.channel_id == 3 && u_type == (int)UserFlag.子账号) || (nav.channel_id == 1 && u_type == (int)UserFlag.总部账号))
                    {
                        if (power.action_type.Contains("," + nav.name + ",")) //权限判断到二级菜单；nav.id==32公众号基本页面，权限必须有
                        {
                            List<t_sys_navigation> lists_three = Common.CacheHelper.Get("lists_three_" + nav.id + "_"+u_id+"_"+wx_id) as List<t_sys_navigation>;// 
                            if (lists_three==null)
                            {
                                lists_three = BLL.Users.sys_navigation.GetListByParentId(nav.id);
                                Common.CacheHelper.Insert("lists_three_" + nav.id + "_"+u_id+"_"+wx_id, lists_three, 120);
                            }
                            strHtml.Append("<li id=" + nav.id + ">");
                            strHtml.Append("<a id=" + nav.id + "  " + (lists_three.Count == 0 ? " onclick='dialogue.dlLoading();'" : "") + " href=\"" + (lists_three.Count > 0 ? "javascript:void(0)" : GetUrl(nav.link_url, nav.id)) + "\">" + (lists_three.Count > 0 ? "<i class=\"hasChild\"></i>" : "") + "" + nav.title + "</a>");
                            strHtml.Append(BindThirdChild(lists_three));
                            strHtml.Append("</li>");
                        }
                    }
                    else
                    {
                        List<t_sys_navigation> lists_three = Common.CacheHelper.Get("lists_three_" + nav.id + "_" + u_id + "_" + wx_id) as List<t_sys_navigation>;// 
                        if (lists_three == null)
                        {
                            lists_three = BLL.Users.sys_navigation.GetListByParentId(nav.id);
                            Common.CacheHelper.Insert("lists_three_" + nav.id + "_" + u_id + "_" + wx_id, lists_three, 120);
                        }
                        strHtml.Append("<li id=" + nav.id + ">");
                        strHtml.Append("<a id=" + nav.id + " " + (lists_three.Count == 0 ? " onclick='dialogue.dlLoading();'" : "") + " href=\"" + (lists_three.Count > 0 ? "javascript:void(0)" : GetUrl(nav.link_url, nav.id)) + "\">" + (lists_three.Count > 0 ? "<i class=\"hasChild\"></i>" : "") + "" + nav.title + "</a>");
                        strHtml.Append(BindThirdChild(lists_three));
                        strHtml.Append("</li>");
                    }
                }

                #region 加载功能模块
                if(par_id==19)
                {
                    List<t_module_menu> listm = BLL.Chats.module_menu.GetListWxId(wx_id, 0,u_type,u_id);// Common.CacheHelper.Get("t_module_menu_list" + "_" + u_id + "_" + wx_id) as List<t_module_menu>;
                    //if (listm==null)
                    //{
                    //    listm = BLL.Chats.module_menu.GetListWxId(wx_id, 0);
                    //    Common.CacheHelper.Insert("t_module_menu_list" + "_"+u_id+"_"+wx_id, listm, 120);
                    //}
                    foreach(t_module_menu menu in listm)
                    {
                        string strmid = par_id.ToString() + menu.id.ToString();
                        int mid = Utils.StrToInt(strmid,0);

                        List<t_module_menu> listm_child = BLL.Chats.module_menu.GetListWxId(wx_id, Utils.ObjToInt(menu.id, -1));// Common.CacheHelper.Get("t_module_menu_child" + "_" + u_id + "_" + wx_id + "_" + wx_id + "_" + menu.id) as List<t_module_menu>;// 
                        //if (listm_child==null)
                        //{
                        //    listm_child=BLL.Chats.module_menu.GetListWxId(wx_id, Utils.ObjToInt(menu.id, -1));
                        //    Common.CacheHelper.Insert("t_module_menu_child" + "_"+u_id+"_"+wx_id + "_" + wx_id + "_" + menu.id, listm_child, 120);
                        //}

                        strHtml.Append("<li id=" + mid + ">");
                        strHtml.Append("<a id=" + mid + "  " + (listm_child.Count == 0 ? " onclick='dialogue.dlLoading();'" : "") + " href=\"" + (listm_child.Count > 0 ? "javascript:void(0)" : GetUrl(menu.link_url, mid)) + "\">" + (listm_child.Count > 0 ? "<i class=\"hasChild\"></i>" : "") + "" + menu.title + "</a>");
                        strHtml.Append(BindThirdChildModule(listm_child,Utils.ObjToInt(menu.id, -1)));
                        strHtml.Append("</li>");
                    }

                }
                #endregion 加载功能模块

                strHtml.Append("</ul>");

                return strHtml.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 绑定三级子菜单
        /// </summary>
        /// <param name="par_id"></param>
        protected string BindThirdChild(List<t_sys_navigation> lists)
        {
            StringBuilder strHtml = new StringBuilder();


            
            int rowCount = lists.Count;
            if (rowCount > 0)
            {
                strHtml.Append("<ul>");

                foreach (t_sys_navigation nav in lists)
                {
                    if ((nav.channel_id == 3 && u_type == (int)UserFlag.子账号) || (nav.channel_id == 1 && u_type == (int)UserFlag.总部账号))
                    {
                        if (power.action_type.Contains("," + nav.name + ",")) //权限判断
                        {
                            strHtml.Append("<li id=" + nav.id + ">");
                            strHtml.Append("<a id=" + nav.id + "  onclick='dialogue.dlLoading();' href=\"" + GetUrl(nav.link_url, nav.id) + "\"><i class=\"offNav\"></i>" + nav.title + "</a>");
                            strHtml.Append("</li>");
                        }
                    }
                    else
                    {
                        strHtml.Append("<li id=" + nav.id + ">");
                        strHtml.Append("<a id=" + nav.id + "  onclick='dialogue.dlLoading();' href=\"" + GetUrl(nav.link_url, nav.id) + "\"><i class=\"offNav\"></i>" + nav.title + "</a>");
                        strHtml.Append("</li>");
                    }
                }

                strHtml.Append("</ul>");

                return strHtml.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 绑定三级模块子菜单
        /// </summary>
        /// <param name="par_id"></param>
        protected string BindThirdChildModule(List<t_module_menu> listm,int par_id)
        {
            StringBuilder strHtml = new StringBuilder();

            
            int rowCount = listm.Count;
            if (rowCount > 0)
            {
                strHtml.Append("<ul>");

                foreach (t_module_menu nav in listm)
                {

                    string strmid = par_id.ToString() + nav.id.ToString();
                    int mid = Utils.StrToInt(strmid, 0);

                    strHtml.Append("<li id=" + mid + ">");
                    strHtml.Append("<a id=" + mid + "  onclick='dialogue.dlLoading();' href=\"" + GetUrl(nav.link_url, mid) + "\"><i class=\"offNav\"></i>" + nav.title + "</a>");
                    strHtml.Append("</li>");
                }

                strHtml.Append("</ul>");

                return strHtml.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        //菜单加ID
        protected string GetUrl(string linkurl, int id)
        {
            if (linkurl.Contains("?"))
            {
                linkurl += "&m_id=" + id;
            }
            else
            {
                linkurl += "?m_id=" + id;
            }

            return linkurl;
        }



       

       
    }
}