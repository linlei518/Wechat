using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using KDWechat.Common;
using KDWechat.Common.Config;
using QuickMark;
using Companycn.Core.DbHelper;
using KDWechat.DAL;

namespace KDWechat.Web.UI
{
    public class BasePage : System.Web.UI.Page
    {

        #region 页面属性
        /// <summary>
        /// 是否可以编辑
        /// </summary>
        protected bool isEdit;
        /// <summary>Elux
        /// 是否可以添加
        /// </summary>
        protected bool isAdd;
        /// <summary>
        /// 是否可以删除
        /// </summary>
        protected bool isDelete;
        /// <summary>
        /// 是否可以导入
        /// </summary>
        protected bool isImport;
        /// <summary>
        /// 是否可以导出
        /// </summary>
        protected bool isExport;
        /// <summary>
        /// 是否可以发布
        /// </summary>
        protected bool isRelease;
        /// <summary>
        /// 是否可以回复
        /// </summary>
        protected bool isReply;

        /// <summary>
        /// 页面菜单的id，每个页面跳转的时候必须要传这个id
        /// </summary>
        protected int m_id
        {
            get { return RequestHelper.GetQueryInt("m_id", 0); }
        }
        /// <summary>
        /// 每页大小
        /// </summary>
        protected int pageSize = 15;
        /// <summary>
        /// 当前页
        /// </summary>
        protected int page
        {
            get { return RequestHelper.GetQueryInt("page", 1); }
        }

        /// <summary>
        /// 当前页
        /// </summary>
        protected int rq
        {
            get { return RequestHelper.GetQueryInt("rq", 0); }
        }


        /// <summary>
        /// 检索状态
        /// </summary>
        protected int status => RequestHelper.GetQueryInt("status", -1);

        /// <summary>
        /// 检索的关键词
        /// </summary>
        protected string key => RequestHelper.GetQueryString("key");

        /// <summary>
        /// 对象id
        /// </summary>
        protected int id => RequestHelper.GetQueryInt("id", 0);
        /// <summary>
        /// 数据总条数
        /// </summary>
        protected int totalCount;
        /// <summary>
        /// 页面标题
        /// </summary>
        protected string pageTitle
        {
            get
            {
                string title = "";
                //if (wx_name.Length > 0)
                //{
                //    title = wx_name + " | 凯德微信公共平台";
                //}
                //else
                //{
                //    title = "凯德微信公共平台";
                //}
                title = "微信公众平台";
                return title;
            }
        }
        /// <summary>
        /// 上传的文件夹
        /// </summary>
        public string folder
        {
            get
            {
                string _temp = "";
                if (wx_id > 0)
                {
                    _temp = wx_og_id;
                    string is_pub = RequestHelper.GetQueryString("is_pub");
                    if (is_pub == "1.1.1")
                    {
                        _temp = "public";
                    }
                }
                else
                {
                    string is_pub = RequestHelper.GetQueryString("is_pub");
                    if (is_pub == "1.1.1")
                    {
                        _temp = "public";
                    }
                    else
                    {
                        _temp = u_name;
                    }

                }

                return _temp;
            }
        }

        /// <summary>
        /// 站点配置
        /// </summary>
        public siteconfig siteConfig;
        /// <summary>
        /// 微信配置
        /// </summary>
        public wechatconfig wchatConfig;
        /// <summary>
        /// 用户当前的权限对象
        /// </summary>
        public DAL.t_sys_users_power power;

        /// <summary>
        /// 地区号能管理的子系统
        /// </summary>
        public DAL.t_sys_user_manage_child_sys user_manage_child_sys;

        /// <summary>
        /// 微邀请的ModuleID
        /// </summary>
        public int invite_module_id { get { return Common.Utils.StrToInt(System.Configuration.ConfigurationManager.AppSettings["invite_module_id"], 0); } }
        #endregion

        #region 头部和菜单

        /// <summary>
        /// 头部菜单字符串
        /// </summary>
        public string TopString
        {
            get
            {
                string lblTopMenu = " <nav class=\"navField\"><ul>";
                string lblWeiXin = "";
                string lblMenu = "";
                string lblUserName = u_name;
                #region 选了公众号之后,判断所能管理的微信号
                int parentId = -1;
                if (m_id > 1900)
                {
                    parentId = 1;
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
                            if (wx_id <= 0)
                            {

                                // new BasePage().AddLog("TopControl first,wx_id=" + wx_id, LogType.添加);
                                Response.Redirect("/loginout.html");

                            }
                            parentId = 1; //选择了公众号之后的菜单
                        }
                        else if (model.class_list.Contains(",50,"))
                        {
                            parentId = 50;  //总部顶级ID
                        }
                        else if (model.class_list.Contains(",58,"))
                        {
                            parentId = 58; //地区帐号登录后未选择微信号的时候
                        }
                    }
                }
                if (parentId == 1)
                {
                    if (wx_id > 0)
                    {
                        StringBuilder query2 = new StringBuilder();
                        query2.Append("select id,wx_pb_name  from t_wx_wechats where  status=1");
                        switch (u_type)
                        {
                            case 1:
                                query2.Append(" and ( uid in( select id from  t_sys_users where flag=2 union select id from  t_sys_users where flag=3  ) or uid=" + u_id + ")");
                                break;

                            case 2:
                                query2.Append(" and uid=" + u_id);
                                break;

                            case 3:
                                query2.Append(" and id in(select distinct wx_id from  t_sys_users_power where u_id=" + u_id + ")");
                                break;
                        }
                        query2.Append(" order by id desc");

                        DataTable dt = KDWechat.DBUtility.DbHelperSQL.Query(query2.ToString()).Tables[0];
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            lblWeiXin += "<h1><a  >" + wx_name + "<i class=\"moreInfo\"></i></a></h1><ul>";
                            foreach (DataRow r in dt.Rows)
                            {
                                lblWeiXin += "<li><a href=\"/select_wechat.aspx?id=" + r["id"] + "\">" + r["wx_pb_name"] + "</a></li>";
                            }
                            lblWeiXin += "</ul>";
                        }

                    }
                }

                #endregion

                #region 地区帐号和子帐号都要有“公众号管理”
                //if (u_type > 1 && u_type < 4)
                //{


                    
                //        //地区帐号和子帐号都要有“公众号管理”
                //  lblMenu = "<li><a href=\"/Account/region_wxlist.aspx?m_id=59\">公众号管理</a></li>";
            
                //    if (u_type == 2)
                //    {   //地区帐号加个“子帐号管理”
                //        lblMenu += "<li><a href=\"/Account/regoin_account.aspx?m_id=60\">子帐号管理</a></li>";
                //    }
                //}
                //else
                //{
                //    lblMenu = "<li><a href=\"/Account/region_wxlist.aspx?m_id=97\">公众号管理</a></li>";
                //}
                #endregion

                #region 加载出当前用户能管理的系统顶部菜单
                //判断该帐号是否有微信公众号管理权限
                if (user_manage_child_sys == null)
                {
                    load_user_manage_child_sys();
                }

                if (user_manage_child_sys.nav_type_ids.Contains(",1,"))
                {
                    //地区帐号和子帐号都要有“公众号管理”
                    lblMenu = "<li><a href=\"/Account/region_wxlist.aspx?m_id=59\">公众号管理</a></li>";
                }
                else if (user_manage_child_sys.nav_type_ids.Contains(",50,"))
                {
                    //地区帐号和子帐号都要有“公众号管理”
                    lblMenu = "<li><a href=\"/Account/region_wxlist.aspx?m_id=97\">公众号管理</a></li>";
                }

                string[] temp_list = user_manage_child_sys.nav_type_ids.TrimStart(',').TrimEnd(',').Split(new char[] { ',' });
                List<int> nav_ids = new List<int>();
                for (int i = 0; i < temp_list.Length; i++)
                {
                    int _id = Common.Utils.StrToInt(temp_list[i], 0);

                    nav_ids.Add(_id);


                }


                List<DAL.t_sys_navigation> list_child_sys = Common.CacheHelper.Get("list_child_sys_" + user_manage_child_sys.nav_type_ids.TrimStart(',').TrimEnd(',').Replace(",", "_") + "_" + u_id) as List<DAL.t_sys_navigation>;
                if (list_child_sys == null)
                {
                    list_child_sys = Companycn.Core.EntityFramework.EFHelper.GetList<DAL.creater_wxEntities, DAL.t_sys_navigation, int?>(x => x.parent_id == 0 && nav_ids.Contains(x.id), x => x.sort_id, int.MaxValue, 1);
                    Common.CacheHelper.Insert("list_child_sys_" + user_manage_child_sys.nav_type_ids.TrimStart(',').TrimEnd(',').Replace(",", "_") + "_" + u_id, list_child_sys, 5);
                }
                //循环子系统

                foreach (var item in list_child_sys)
                {
                    string class_name = "";
                    if (item.id == 1 || item.id == 50 || item.id==58)
                    {
                        class_name = "class=\"current\"";
                    }


                    if (u_type == 3)
                    {
                        DAL.t_sys_users_power power = BLL.Users.sys_users_power.GetPowerRole(-1, u_id, item.id);
                        if (power != null)
                        {
                            lblTopMenu += "<li><a  " + item.target_type + " " + class_name + " href=\"" + item.link_url + "\" >" + item.title + "</a></li>";
                        }
                    }
                    else
                    {
                        lblTopMenu += "<li><a " + item.target_type + "  " + class_name + " href=\"" + item.link_url + "\" >" + item.title + "</a></li>";
                    }

                }
                #endregion

                lblTopMenu += "</ul></nav>";

                #region 加载消息提醒的数量

                int count = 0;
                string lblNoReplyCount = "";
                #region 加载48小时内未回复的数量
                if (wx_id > 0)
                {
                    bool isc = false;
                    if (u_type == 1 || u_type == 4)
                    {
                        if (parentId == 1)
                        {
                            isc = true;
                        }

                    }
                    else
                    {
                        var powe = BLL.Users.sys_users_power.GetModel(x => x.u_id == u_id && x.nav_name == "message_list");
                        if (powe != null || u_type == 2)
                        {
                            isc = true;
                        }
                    }
                    if (isc)
                    {
                        int no_reply_count = KDWechat.BLL.Logs.wx_fans_chats.GetNoReplyCount(wx_id);
                        lblNoReplyCount = "<li><a href=\"/fans/message_list.aspx?key=&beginDate=&endDate=&m_id=80&replyStatus=0\">未回复粉丝：<em>" + no_reply_count + "</em>个</a></li>";
                        count += no_reply_count;
                    }



                }

                #endregion

                 string lit_letCount = "";
                 #region 加载站内信提醒的数量

                //int msgCount = BLL.Users.sys_letter.GetUnread(u_id);
                //int _mid = 10;
                //if (u_type == 1 || u_type == 4)
                //{
                //    if (wx_id == 0)
                //    {
                //        _mid = 51;
                //    }
                //}
                //else
                //{
                //    if (wx_id == 0)
                //    {
                //        _mid = 58;
                //    }
                //}
                //lit_letCount = "<li><a href='../Account/letter_list_rec.aspx?m_id=" + _mid + "'>站内信：<em>" + msgCount + "</em>条未读</a></li>";
                //count += msgCount;

               #endregion

                #region 修改密码
                #endregion


                #region 加载未读的站内信

                #endregion
                if (count > 0)
                {
                    lblUserName += "<sup>" + count + "</sup>";
                }
                #endregion

                StringBuilder str = new StringBuilder();
                str.AppendLine("<header id=\"header\">");
                str.AppendLine("<div class=\"logoField\">");
                str.AppendLine("<span class=\"logo\"><img src=\"/images/logo_01.png\" width=\"40\" height=\"40\" alt=\"\">凯德微信管理平台</span>");
                str.AppendLine("</div>");
                str.AppendLine("<div class=\"userField\">");
                str.AppendLine("<p>您好，<a  >" + lblUserName + "<i class=\"moreInfo\"></i></a></p>");
                str.AppendLine("<ul>");
               // str.AppendLine(lit_letCount);
                str.AppendLine(lblNoReplyCount);
                str.AppendLine(lblMenu);
                string _m_id = "61";
                if (u_type == 1 || u_type == 4) { _m_id = "57"; }
                str.AppendLine("<li><a href=\"/Account/change_password.aspx?m_id=" + _m_id + "\">修改密码</a></li>");
                str.AppendLine("<li><a href=\"/kdlogin/loginout.aspx\">退出</a></li>");
                str.AppendLine("</ul>");
                str.AppendLine("</div>");
                str.AppendLine(lblTopMenu);
                str.AppendLine("<div class=\"titleField\">");
                str.AppendLine(lblWeiXin);
                str.AppendLine("</div>");
                str.AppendLine("</header>");

                return str.ToString();
            }
        }

        /// <summary>
        /// 左侧菜单字符串
        /// </summary>
        public string MenuString
        {
            get
            {
                string lblstr = "";
                if (u_type > 0)
                {
                    if (m_id > 0)
                    {
                        int parentId = -1;
                        if (m_id > 1900)
                        {
                            parentId = 1;
                            power = BLL.Users.sys_users_power.GetPowerRole(wx_id, u_id, (wx_id > 0 ? 1 : 50));
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
                                    if (wx_id <= 0)
                                    {
                                        //new BasePage().AddLog("MenuList first,wx_id=" + wx_id, LogType.添加);
                                        Response.Redirect("/loginout.html");

                                    }
                                    power = BLL.Users.sys_users_power.GetPowerRole(wx_id, u_id, 1);
                                    parentId = 1; //选择了公众号之后的菜单
                                }
                                else if (model.class_list.Contains(",50,"))
                                {
                                    parentId = 50;  //总部顶级ID
                                    power = BLL.Users.sys_users_power.GetPowerRole(0, u_id, 50);
                                }
                                else if (model.class_list.Contains(",58,"))
                                {
                                    parentId = 58; //地区帐号登录后未选择微信号的时候
                                    //  power = BLL.Users.sys_users_power.GetPowerRole(0, u_id);
                                    power = new t_sys_users_power();
                                    power.action_type = "none";
                                    power.nav_name = "none";
                                    power.u_id = u_id;
                                    power.wx_id = wx_id;
                                }


                            }
                        }


                        lblstr = BindData(parentId);
                    }

                }
                else
                {
                    // new BasePage().AddLog("MenuList two,wx_id=" + wx_id, LogType.添加);
                    Response.Redirect("/loginout.html");
                }

                StringBuilder str = new StringBuilder();
                str.AppendLine("<nav id=\"mainNav\">");
                str.AppendLine("<ul>");
                str.AppendLine(lblstr);
                str.AppendLine("</ul>");
                str.AppendLine("</nav>");
                return str.ToString();
            }
        }

        #region 菜单
        private string GetTarget(string url, int clid_count)
        {
            string str = "";
            if (clid_count == 0 && url.ToLower().Contains("help.aspx"))
            {
                str = "target='_blank'";
            }
            return str;
        }

        /// <summary>
        /// 获取地区帐号能管理哪些子系统的权限
        /// </summary>
        protected void load_user_manage_child_sys()
        {

            if (user_manage_child_sys == null)
            {
                if (u_type == (int)UserFlag.地区账号 || u_type == (int)UserFlag.总部账号)
                {
                    user_manage_child_sys = Companycn.Core.EntityFramework.EFHelper.GetModel<DAL.creater_wxEntities, DAL.t_sys_user_manage_child_sys>(x => x.u_id == u_id);
                    if (user_manage_child_sys == null)
                    {
                        user_manage_child_sys = new t_sys_user_manage_child_sys();
                        user_manage_child_sys.nav_type_ids = "";
                    }
                }
                else if (u_type == (int)UserFlag.子账号)
                {
                    var userinfo = Companycn.Core.EntityFramework.EFHelper.GetModel<DAL.creater_wxEntities, DAL.t_sys_users>(x => x.id == u_id);
                    if (userinfo != null)
                    {
                        user_manage_child_sys = Companycn.Core.EntityFramework.EFHelper.GetModel<DAL.creater_wxEntities, DAL.t_sys_user_manage_child_sys>(x => x.u_id == userinfo.parent_id);
                        if (user_manage_child_sys == null)
                        {
                            user_manage_child_sys = new t_sys_user_manage_child_sys();
                            user_manage_child_sys.nav_type_ids = "";
                        }
                    }
                }
                else
                {
                    //超级管理员
                    List<DAL.t_sys_navigation> list_child_sys = Companycn.Core.EntityFramework.EFHelper.GetList<DAL.creater_wxEntities, DAL.t_sys_navigation, int?>(x => x.parent_id == 0 && x.is_lock == 0 && (x.type_id == 0 || x.type_id == 2), x => x.sort_id, int.MaxValue, 1);

                    user_manage_child_sys = new t_sys_user_manage_child_sys();
                    user_manage_child_sys.nav_type_ids = ",";
                    foreach (var item in list_child_sys)
                    {
                        user_manage_child_sys.nav_type_ids += item.id + ",";
                    }
                }

            }
        }

        /// <summary>
        /// 绑定菜单
        /// </summary>
        /// <param name="par_id"></param>
        protected string BindData(int par_id)
        {
            StringBuilder strHtml = new StringBuilder();


            List<t_sys_navigation> lists = Common.CacheHelper.Get("t_sys_navigation_list_" + par_id + "_" + u_id + "_" + wx_id) as List<t_sys_navigation>;// 
            if (lists == null)
            {
                lists = BLL.Users.sys_navigation.GetListByParentId(par_id);
                Common.CacheHelper.Insert("t_sys_navigation_list_" + par_id + "_" + u_id + "_" + wx_id, lists, 120);
            }
            int rowCount = lists.Count;
            if (rowCount > 0)
            {
                foreach (t_sys_navigation nav in lists)
                {
                    #region 判断子账号，不显示“子帐号管理”,“新建子帐号”和“日志管理”菜单
                    if (u_type == (int)UserFlag.子账号 && par_id == 58)
                    {
                        if (nav.id == 60 || nav.id == 68 || nav.id == 34)
                        {
                            continue;
                        }
                    }
                    #endregion

                    if (par_id == 1)
                    {
                        #region 选择公众号之后的菜单
                        //地区帐号，既是公众号创建者，所以不需要判断权限
                        if (u_type == (int)UserFlag.地区账号 || u_type == (int)UserFlag.超级管理员 || u_type == (int)UserFlag.总部账号)
                        {
                            List<t_sys_navigation> lists_child = Common.CacheHelper.Get("lists_child_" + nav.id + "_" + u_id + "_" + wx_id) as List<t_sys_navigation>;// 
                            if (lists_child == null)
                            {
                                lists_child = BLL.Users.sys_navigation.GetListByParentId(nav.id);
                                Common.CacheHelper.Insert("lists_child_" + nav.id + "_" + u_id + "_" + wx_id, lists_child, 120);
                            }
                            strHtml.Append("<li id=" + nav.id + ">");//class=\"current\"
                            strHtml.Append("<a  " + GetTarget(nav.link_url, lists_child.Count) + " id=" + nav.id + " " + (lists_child.Count == 0 ? nav.link_url.ToLower().Contains("help.aspx") == true ? "" : " onclick='dialogue.dlLoading();'" : "") + " href=\"" + (lists_child.Count > 0 ? "javascript:void(0)" : GetUrl(nav.link_url, nav.id)) + "\" >" + (lists_child.Count > 0 ? "<i class=\"hasChild\"></i><i class=\"" + (nav.remark != "" ? nav.remark : "navBase") + "\"></i>" : "<i class=\"" + (nav.remark != "" ? nav.remark : "navBase") + "\"></i>") + "" + nav.title + "</a>");
                            strHtml.Append(BindChild(lists_child, nav.id));
                            strHtml.Append("</li>");
                        }
                        //子帐号，需要判断权限
                        else if (power.action_type.Contains("," + nav.name + ","))  //nav.id==32公众号基本页面，权限必须有
                        {
                            List<t_sys_navigation> lists_child = Common.CacheHelper.Get("lists_child_" + nav.id + "_" + u_id + "_" + wx_id) as List<t_sys_navigation>;// 
                            if (lists_child == null)
                            {
                                lists_child = BLL.Users.sys_navigation.GetListByParentId(nav.id);
                                Common.CacheHelper.Insert("lists_child_" + nav.id + "_" + u_id + "_" + wx_id, lists_child, 120);
                            }
                            strHtml.Append("<li id=" + nav.id + ">");//class=\"current\"
                            strHtml.Append("<a " + GetTarget(nav.link_url, lists_child.Count) + " id=" + nav.id + " " + (lists_child.Count == 0 ? nav.link_url.ToLower().Contains("help.aspx") == true ? "" : " onclick='dialogue.dlLoading();'" : "") + " href=\"" + (lists_child.Count > 0 ? "javascript:void(0)" : GetUrl(nav.link_url, nav.id)) + "\">" + (lists_child.Count > 0 ? "<i class=\"hasChild\"></i><i class=\"" + (nav.remark != "" ? nav.remark : "navBase") + "\"></i>" : "<i class=\"" + (nav.remark != "" ? nav.remark : "navBase") + "\"></i>") + "" + nav.title + "</a>");
                            strHtml.Append(BindChild(lists_child, nav.id));
                            strHtml.Append("</li>");
                        }
                        #endregion
                    }
                    else if (par_id == 50)
                    {
                        #region 系统超级管理员的菜单
                        if (u_type == (int)UserFlag.超级管理员)
                        {
                            List<t_sys_navigation> lists_child = Common.CacheHelper.Get("lists_child_" + nav.id + "_" + u_id + "_" + wx_id) as List<t_sys_navigation>;// 
                            if (lists_child == null)
                            {
                                lists_child = BLL.Users.sys_navigation.GetListByParentId(nav.id);
                                Common.CacheHelper.Insert("lists_child_" + nav.id + "_" + u_id + "_" + wx_id, lists_child, 120);
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
                                List<t_sys_navigation> lists_child = Common.CacheHelper.Get("lists_child_" + nav.id + "_" + u_id + "_" + wx_id) as List<t_sys_navigation>;// 
                                if (lists_child == null)
                                {
                                    lists_child = BLL.Users.sys_navigation.GetListByParentId(nav.id);
                                    Common.CacheHelper.Insert("lists_child_" + nav.id + "_" + u_id + "_" + wx_id, lists_child, 120);
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
                        List<t_sys_navigation> lists_child = Common.CacheHelper.Get("lists_child_" + nav.id + "_" + u_id + "_" + wx_id) as List<t_sys_navigation>;// 
                        if (lists_child == null)
                        {
                            lists_child = BLL.Users.sys_navigation.GetListByParentId(nav.id);
                            Common.CacheHelper.Insert("lists_child_" + nav.id + "_" + u_id + "_" + wx_id, lists_child, 120);
                        }

                        //判断该帐号是否有微信公众号管理权限
                        if (user_manage_child_sys == null)
                        {
                            load_user_manage_child_sys();
                        }
                        if (!user_manage_child_sys.nav_type_ids.Contains(",1,"))
                        {
                            //没有微信公众号的权限,移除公众号管理的菜单
                            if (nav.id == 59)
                            {
                                continue;
                            }
                        }

                        strHtml.Append("<li id=" + nav.id + ">");//class=\"current\"
                        strHtml.Append("<a id=" + nav.id + "  " + (lists_child.Count == 0 ? " onclick='dialogue.dlLoading();'" : "") + " href=\"" + (lists_child.Count > 0 ? "javascript:void(0)" : GetUrl(nav.link_url, nav.id)) + "\">" + (lists_child.Count > 0 ? "<i class=\"hasChild\"></i><i class=\"" + (nav.remark != "" ? nav.remark : "navBase") + "\"></i>" : "<i class=\"" + (nav.remark != "" ? nav.remark : "navBase") + "\"></i>") + "" + nav.title + "</a>");
                        strHtml.Append(BindChild(lists_child, nav.id));
                        strHtml.Append("</li>");
                        #endregion
                    }


                }

            }
            return strHtml.ToString();
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
                            List<t_sys_navigation> lists_three = Common.CacheHelper.Get("lists_three_" + nav.id + "_" + u_id + "_" + wx_id) as List<t_sys_navigation>;// 
                            if (lists_three == null)
                            {
                                lists_three = BLL.Users.sys_navigation.GetListByParentId(nav.id);
                                Common.CacheHelper.Insert("lists_three_" + nav.id + "_" + u_id + "_" + wx_id, lists_three, 120);
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
                if (par_id == 19)
                {
                    List<t_module_menu> listm = BLL.Chats.module_menu.GetListWxId(wx_id, 0, u_type, u_id);// Common.CacheHelper.Get("t_module_menu_list" + "_" + u_id + "_" + wx_id) as List<t_module_menu>;
                    //if (listm==null)
                    //{
                    //    listm = BLL.Chats.module_menu.GetListWxId(wx_id, 0);
                    //    Common.CacheHelper.Insert("t_module_menu_list" + "_"+u_id+"_"+wx_id, listm, 120);
                    //}
                    foreach (t_module_menu menu in listm)
                    {
                        string strmid = par_id.ToString() + menu.id.ToString();
                        int mid = Utils.StrToInt(strmid, 0);

                        List<t_module_menu> listm_child = BLL.Chats.module_menu.GetListWxId(wx_id, Utils.ObjToInt(menu.id, -1));// Common.CacheHelper.Get("t_module_menu_child" + "_" + u_id + "_" + wx_id + "_" + wx_id + "_" + menu.id) as List<t_module_menu>;// 
                        //if (listm_child==null)
                        //{
                        //    listm_child=BLL.Chats.module_menu.GetListWxId(wx_id, Utils.ObjToInt(menu.id, -1));
                        //    Common.CacheHelper.Insert("t_module_menu_child" + "_"+u_id+"_"+wx_id + "_" + wx_id + "_" + menu.id, listm_child, 120);
                        //}

                        strHtml.Append("<li id=" + mid + ">");
                        strHtml.Append("<a id=" + mid + "  " + (listm_child.Count == 0 ? " onclick='dialogue.dlLoading();'" : "") + " href=\"" + (listm_child.Count > 0 ? "javascript:void(0)" : GetUrl(menu.link_url, mid)) + "\">" + (listm_child.Count > 0 ? "<i class=\"hasChild\"></i>" : "") + "" + menu.title + "</a>");
                        strHtml.Append(BindThirdChildModule(listm_child, Utils.ObjToInt(menu.id, -1)));
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
        protected string BindThirdChildModule(List<t_module_menu> listm, int par_id)
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


        #endregion

        #endregion

        public BasePage()
        {
            siteConfig = new BLL.Config.siteconfig().loadConfig();
            wchatConfig = new BLL.Config.wechat_config().loadConfig();
            this.Load += BasePage_Load;

        }



        void BasePage_Load(object sender, EventArgs e)
        {
            //判断管理员是否登录
            if (!IsAdminLogin())
            {
                if (rq > 0)
                {
                    return;
                }
                string url = siteConfig.webpath + "loginout.html";
                //AddLog("BasePage Load时 wx_id="+wx_id, LogType.添加);
                Response.Write("<script>window.parent.location='" + url + "';</script>");
                Response.End();
                //Response.Redirect(url);

            }

        }

        #region 模块权限
        /// <summary>
        /// 检查模块权限
        /// </summary>
        public void CheckModuleAuthority(int module_id)
        {
            if (module_id > 0)
            {
                if (u_type == (int)UserFlag.子账号)
                {
                    var m_switch = BLL.Chats.module_wx_switch.GetModel(wx_id, u_id, module_id);
                    if (m_switch != null)
                        return;
                }
                else
                {
                    var m_switch = BLL.Chats.module_wx_switch.GetModel(x => x.module_id == module_id && x.status == (int)Status.正常 && x.wx_og_id == wx_og_id);
                    if (m_switch != null)
                        return;
                }

            }

            string url = siteConfig.webpath + "error.aspx?m_id=" + m_id;
            Response.Redirect(url);

        }


        #endregion

        #region 登录用户信息

        /// <summary>
        /// 检测选择的微信公众号是否失效
        /// </summary>
        public void CheckWXid()
        {
            if (wx_id == 0)
            {
                //跳到选择公众帐号页面
                string url = siteConfig.webpath + "loginout.html";
                //AddLog("BasePage CheckWXid wx_id=0时,wx_id=" + wx_id, LogType.添加);
                Response.Write("<script>window.parent.location='" + url + "';</script>");
                Response.End();
                // Response.Redirect(url);
            }
            else if (wx_id < 0)
            {
                string url = siteConfig.webpath + "loginout.html";
                // AddLog("BasePage CheckWXid wx_id<0时,wx_id=" + wx_id, LogType.添加);
                Response.Write("<script>window.parent.location='" + url + "';</script>");
                Response.End();
                // Response.Redirect(url);
            }
        }

        /// <summary>
        /// 取得管理员信息，session丢失的时候将会读数据库，请勿直接使用，以免影响性能(如是取id、用户名、用户类型，请直接调用u_id、u_name、u_type，这样不用读数据库效率会很高)
        /// </summary>
        public KDWechat.DAL.t_sys_users GetAdminInfo()
        {
            if (IsAdminLogin())
            {
                KDWechat.DAL.t_sys_users model = Session[KDKeys.SESSION_ADMIN_INFO] as KDWechat.DAL.t_sys_users;
                if (model != null)
                {
                    return model;
                }
                else
                {
                    string adminname = Utils.GetCookie(KDKeys.COOKIE_USER_NAME);
                    string adminpwd = Utils.GetCookie(KDKeys.COOKIE_USER_PWD);
                    if (adminname != "" && adminpwd != "")
                    {
                        model = KDWechat.BLL.Users.sys_users.UserLogin(adminname, adminpwd);
                        if (model != null && model.status != (int)Status.禁用)
                        {
                            Session[KDKeys.SESSION_ADMIN_INFO] = model;
                            return model;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 判断管理员是否登录
        /// </summary>
        /// <returns></returns>
        private bool IsAdminLogin()
        {
            string url = siteConfig.webpath + "loginout.html";
            //如果Session为Null
            if (Session[KDKeys.SESSION_ADMIN_INFO] != null)
            {
                return true;
            }
            else
            {
                //检查Cookies
                string adminname = Utils.GetCookie(KDKeys.COOKIE_USER_NAME);
                string adminpwd = Utils.GetCookie(KDKeys.COOKIE_USER_PWD);
                if (adminname != "" && adminpwd != "")
                {


                    KDWechat.DAL.t_sys_users model = KDWechat.BLL.Users.sys_users.UserLogin(adminname, adminpwd);
                    if (model != null && model.status != (int)Status.禁用)
                    {
                        Session[KDKeys.SESSION_ADMIN_INFO] = model;
                        //Utils.WriteCookie(KDKeys.COOKIE_USER_NAME, adminname);
                        //Utils.WriteCookie(KDKeys.COOKIE_USER_PWD, adminpwd);
                        //Session[KDKeys.SESSION_ADMIN_INFO] = model;
                        return true;
                    }
                    else
                    {
                        Response.Write("<script>window.parent.location='" + url + "';</script>");
                        Response.End();
                        return false;
                    }
                }
                else
                {
                    Response.Write("<script>window.parent.location='" + url + "';</script>");
                    Response.End();
                    return false;
                }
            }

        }
        /// <summary>
        /// 登录用户的id(小于0的时候表示登录超时)
        /// </summary>
        public int u_id
        {
            get
            {
                KDWechat.DAL.t_sys_users model = GetAdminInfo();
                int _id = 0;
                if (model != null)
                {
                    _id = model.id;
                }
                else
                {
                    string temp = Utils.GetCookie(KDKeys.COOKIE_USER_ID);
                    temp = temp.Replace(DateTime.Now.ToString("yyyyMMdd"), "");
                    _id = Utils.StrToInt(temp, 0);
                }
                return _id;
            }
        }

        /// <summary>
        /// 登录用户的用户名(为空时候表示登录超时)
        /// </summary>
        public string u_name
        {
            get
            {
                KDWechat.DAL.t_sys_users model = GetAdminInfo();
                string _name = "";
                if (model != null)
                {
                    _name = model.user_name;
                }
                else
                {
                    _name = Utils.GetCookie(KDKeys.COOKIE_USER_NAME);
                }
                return _name;
            }
        }

        /// <summary>
        /// 登录用户的类型(1-总部，2-负责人，3-子账号，4,超级管理员)
        /// </summary>
        public int u_type
        {
            get
            {
                KDWechat.DAL.t_sys_users model = GetAdminInfo();
                int _flag = 0;
                if (model != null)
                {
                    _flag = model.flag;
                }
                else
                {
                    _flag = Utils.StrToInt(Utils.GetCookie(KDKeys.COOKIE_USER_FlAG), 0);
                }
                return _flag;
            }
        }

        /// <summary>
        /// 当前的微信公众号id(等于-1的时候表示登录超时,等于0的时候表示没有选择任何公众号)
        /// </summary>
        public int wx_id
        {
            get
            {
                int _wx_id = -1;
                if (u_id > 0)
                {
                    _wx_id = Utils.StrToInt(Utils.GetCookie(KDKeys.COOKIE_WECHATS_ID), 0);
                }

                return _wx_id;
            }
        }

        /// <summary>
        /// 当前的微信公众号原始ID(为空时候表示登录超时或选择的公众号已过期)
        /// </summary>
        public string wx_og_id
        {
            get
            {
                string _wx_og_id = "";
                if (u_id > 0)
                {
                    _wx_og_id = Utils.GetCookie(KDKeys.COOKIE_WECHATS_WX_OG_ID);
                }

                return _wx_og_id;
            }
        }
        /// <summary>
        /// 当前页面分页cook名称
        /// </summary>
        public string page_size_cook { get { return this.GetType().Name; } }

        /// <summary>
        /// 当前的微信公众号显示的名称(为空时候表示登录超时或选择的公众号已过期)
        /// </summary>
        public string wx_name
        {
            get
            {
                string _wx_og_id = "";
                if (u_id > 0)
                {
                    _wx_og_id = Common.Utils.UrlDecode(Utils.GetCookie(KDKeys.COOKIE_WECHATS_NAME));
                }

                return _wx_og_id;
            }
        }
        /// <summary>
        /// 当前的微信公众号的头像(为空时候表示登录超时或选择的公众号已过期)
        /// </summary>
        public string wx_head_pic
        {
            get
            {

                string _wx_head_pic = "";
                if (u_id > 0)
                {
                    _wx_head_pic = Common.Utils.UrlDecode(Utils.GetCookie(KDKeys.COOKIE_WECHATS_HEADIMG));
                }

                return _wx_head_pic;
            }
        }

        public int wx_type
        {
            get
            {

                int _wx_type = 0;
                if (u_id > 0)
                {
                    _wx_type = Utils.StrToInt(Utils.GetCookie(KDKeys.COOKIE_WECHATS_TYPE), 0);
                }

                return _wx_type;
            }
        }

        /// <summary>
        ///  只能操作自己
        /// </summary>
        public int only_op_self
        {
            get
            {
                KDWechat.DAL.t_sys_users model = GetAdminInfo();
                int _only_op_self = 0;
                if (model != null)
                {
                    _only_op_self = model.only_op_self;
                }

                return _only_op_self;
            }
        }


        protected void setRole(string action_list, string page_code, int status = -1)
        {
            if (status == 1)
            {
                isEdit = true;
                isAdd = true;
                isDelete = true;
                isImport = true;
                isExport = true;
                isRelease = true;
                isReply = true;
            }
            else if (status == 0)
            {
                isEdit = false;
                isAdd = false;
                isDelete = false;
                isImport = false;
                isExport = false;
                isRelease = false;
                isReply = false;
            }
            else
            {
                isEdit = action_list.Contains("," + page_code + "_" + RoleActionType.Edit.ToString() + ",");
                isAdd = action_list.Contains("," + page_code + "_" + RoleActionType.Add.ToString() + ",");
                isDelete = action_list.Contains("," + page_code + "_" + RoleActionType.Delete.ToString() + ",");
                isImport = action_list.Contains("," + page_code + "_" + RoleActionType.Import.ToString() + ",");
                isExport = action_list.Contains("," + page_code + "_" + RoleActionType.Export.ToString() + ",");
                isRelease = action_list.Contains("," + page_code + "_" + RoleActionType.Release.ToString() + ",");
                isReply = action_list.Contains("," + page_code + "_" + RoleActionType.Reply.ToString() + ",");
            }

        }


        /// <summary>
        /// 检测用户当前页面的权限(主要用于检测某个按钮是否有权限，不会跳转页面)
        /// </summary>
        /// <param name="page_code">当前页面调用别名</param>
        /// <param name="action_type">页面动作(枚举)</param>
        public bool CheckUserAuthorityBool(string page_code, RoleActionType action_type = RoleActionType.View)
        {


            bool res = false;
            if (u_type == (int)UserFlag.超级管理员)
            {
                res = true;
                isEdit = true;
                isAdd = true;
                isDelete = true;
                isImport = true;
                isExport = true;
                isRelease = true;
                isReply = true;
            }
            else  
            {
                DAL.t_sys_navigation navigation = Common.CacheHelper.Get("t_sys_navigation_" + page_code) as DAL.t_sys_navigation;
                if (navigation == null)
                {
                    navigation = BLL.Users.sys_navigation.GetNavigationByName(page_code);
                    if (navigation != null)
                    {
                        Common.CacheHelper.Insert("t_sys_navigation_" + page_code, navigation, 120);
                    }
                }
                if (navigation != null)
                {

                    int parentId = -1;
                    if (navigation.class_list.Contains(",1,"))
                    {
                        parentId = 1; //选择了公众号之后的菜单
                        if (power == null)
                        {
                            power = BLL.Users.sys_users_power.GetPowerRole(wx_id, u_id,1);
                        }
                    }
                    else if (navigation.class_list.Contains(",50,"))
                    {
                        parentId = 50;  //总部顶级ID
                        if (power == null)
                        {
                            power = BLL.Users.sys_users_power.GetPowerRole(0, u_id,50);
                        }
                    }
                    else if (navigation.class_list.Contains(",58,"))
                    {
                        parentId = 58; //地区帐号登录后未选择微信号的时候
                        if (power == null)
                        {
                            power = new t_sys_users_power();
                            power.action_type = "none";
                            power.nav_name = "none";
                            power.u_id = u_id;
                            power.wx_id = wx_id;
                        }
                    }
                    switch (u_type)
                    {

                        case (int)UserFlag.总部账号:
                            if (parentId == 1) //总部帐号，进入某个公众号管理菜单
                            {
                                setRole(power.action_type, page_code, 1);
                                res = true;
                            }
                            else
                            {
                                res = power.action_type.Contains("," + page_code + "_" + action_type.ToString() + ",");
                                if (!res && action_type == RoleActionType.View)
                                {
                                    res = power.action_type.Contains("," + page_code + ",");
                                }
                                setRole(power.action_type, page_code);

                            }

                            break;
                        case (int)UserFlag.地区账号:  //负责人
                            if (parentId != 50)
                            {
                                //地区帐号进入管理微信菜单，判断该帐号是否能管理微信
                                //var child_sys = Companycn.Core.EntityFramework.EFHelper.GetModel<DAL.creater_wxEntities, DAL.t_sys_user_manage_child_sys>(x => x.u_id == u_id);

                                //string[] list_ids = navigation.class_list.TrimEnd(',').TrimStart(',').Split(new char[] { ',' });
                                //string sys_id = list_ids[0];

                                //if (child_sys.nav_type_ids.Contains(",1,"))
                                //{
                                //    setRole("", page_code, 1);
                                //    res = true;
                                //}
                                //else
                                //{
                                setRole("", page_code, 1);
                                //    res = false;
                                //}
                                res = true;
                            }
                            else
                            {
                            // 地区帐号进入总部菜单的情况，不给权限
                                setRole(power.action_type, page_code, 0);
                                res = false;
                            }
                            break;
                        case (int)UserFlag.子账号:  //子账号

                            //if ( page_code == "region_wechat" || page_code == "region_password")
                            //{
                            //    setRole(power.action_type, page_code, 1);
                            //    res = true;
                            //}
                            //else    if (page_code == "wechat_system" || page_code == "wechat_systempub")
                            //{
                            //    setRole(power.action_type, page_code, 1);
                            //    res = true;
                            //}
                            //else
                            //{
                                setRole(power.action_type, page_code,1);
                                //res = power.action_type.Contains("," + page_code + "_" + action_type.ToString() + ",");
                                //if (!res && action_type == RoleActionType.View)
                                //{
                                //    res = power.action_type.Contains("," + page_code + ",");
                                //}
                                res = true;
                            //}
                            break;
                        case (int)UserFlag.超级管理员:  //系统超级管理员
                            setRole(power.action_type, page_code, 1);
                            res = true;

                            break;
                    }
                }
               
 
            }
             

            return res;
        }


        /// <summary>
        /// 检测用户当前页面的权限
        /// </summary>
        /// <param name="page_code">当前页面调用别名</param>
        /// <param name="action_type">页面动作(枚举)</param>
        /// <param name="RedirectUrl">没有权限时跳转页面，如自己指定跳转页面，默认是指向error.aspx</param>
        public void CheckUserAuthority(string page_code, RoleActionType action_type = RoleActionType.View, string RedirectUrl = "")
        {
            bool res = CheckUserAuthorityBool(page_code, action_type);
            if (!res)
            {
                int mid = 1;
                if ((u_type == 1 && wx_id == 0) || u_type == 4)
                {
                    mid = 50;
                }
                else if ((u_type == 2 || u_type == 3) && wx_id == 0)
                {
                    mid = 58;
                }
                if (RedirectUrl == "")
                {
                    string url = siteConfig.webpath + "error.aspx?m_id=" + mid;
                    Response.Redirect(url);
                }
                else
                {
                    Response.Redirect(RedirectUrl + "#fail=您没有权限访问哦");
                }
                return;
            }

        }


        #endregion

        #region 分页
        /// <summary>
        /// 存储过程分页
        /// </summary>
        /// <param name="dbType">数据库（1、操作日志库 2、用户信息库 3、微信架构库）</param>
        /// <param name="QueryStr">查询数据</param>
        /// <param name="PageSize">分页大小</param>
        /// <param name="PageCurrent">当前页</param>
        /// <param name="FdShow">显示的字段</param>
        /// <param name="FdOrder">排序字段</param>
        /// <param name="rowCount">数据总条数</param>
        /// <returns></returns>
        public DataTable GetPageList(DbDataBaseEnum dbType, string QueryStr, int PageSize, int PageCurrent, string FdShow, string FdOrder, ref int rowCount)
        {
            return KDWechat.BLL.PageHelper.GetPageList(dbType, QueryStr, PageSize, PageCurrent, FdShow, FdOrder, ref rowCount);
        }


        #endregion

        /// <summary>
        /// 添加操作日志
        /// </summary>
        /// <param name="contents"></param>
        protected void AddLog(string contents)
        {
            KDWechat.DAL.t_wx_logs model = new DAL.t_wx_logs()
            {
                wx_id = wx_id,
                contents = contents,
                create_time = DateTime.Now,
                ip = Utils.GetUserIp(),
                login_name = u_name,
                u_id = u_id,
                wx_og_id = wx_og_id
            };
            KDWechat.BLL.Logs.wx_logs.Add(model);
        }
        /// <summary>
        /// 添加操作日志
        /// </summary>
        /// <param name="contents"></param>
        public void AddLog(string contents, LogType type)
        {
            try
            {
                KDWechat.DAL.t_wx_logs model = new DAL.t_wx_logs()
                   {
                       wx_id = wx_id,
                       contents = contents,
                       create_time = DateTime.Now,
                       ip = Utils.GetUserIp(),
                       login_name = u_name,
                       u_id = u_id,
                       wx_og_id = wx_og_id,
                       type = (int)type
                   };
                KDWechat.BLL.Logs.wx_logs.Add(model);
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 记录上一次过来的页面，用户返回
        /// </summary>
        /// <param name="hfReturlUrl"></param>
        /// <param name="defaultPage"></param>
        protected void WriteReturnPage(HiddenField hfReturlUrl, string defaultPage)
        {
            try
            {
                if (RequestHelper.GetQueryString("r") == "1.1.1")
                {
                    hfReturlUrl.Value = defaultPage;
                }
                else
                {
                    hfReturlUrl.Value = Request.UrlReferrer.ToString();
                }
            }
            catch (Exception)
            {
                hfReturlUrl.Value = defaultPage;
            }
        }

        /// <summary>
        /// 页面导航面包屑
        /// </summary>
        protected string NavigationName
        {
            get
            {
                string name = "";

                DataTable dt = KDWechat.BLL.Users.sys_navigation.GetNavigationName(m_id);
                if (dt != null)
                {
                    DataRow[] list = dt.Select("id not in(1,50,58,90)");
                    name += "<div class=\"breadcrumbPanel_01\"><h1>";
                    int i = 1;
                    foreach (DataRow r in list)
                    {

                        if (r["id"].ToString() != m_id.ToString())
                        {
                            name += "<span>" + r["title"] + "</span>";
                        }
                        else
                        {
                            name += "<em>" + r["title"] + "</em>";
                        }
                        if (i < list.Length)
                        {
                            name += "<i class=\"breadcrumbArrow\"></i>";
                        }

                        i++;

                    }
                    name += "</h1></div>";
                }
                return name;
            }
        }

        /// <summary>
        /// 生成图文二维码
        /// </summary>
        /// <param name="news_id"></param>
        /// <returns></returns>
        public string CreateNewsQrCode(int news_id)
        {
            string img_url = "/upload/qr_codes/news_" + news_id + ".png";
            if (!File.Exists(Server.MapPath(img_url)))
            {
                QuickMark.CreateTwoCode ctc = new QuickMark.CreateTwoCode();
                var bitmap = ctc.CreateCode(wchatConfig.domain + "/wxpage/news_list.aspx?id=" + news_id, CreateTwoCode.CodeType.Byte, CreateTwoCode.Correct.M, 5, 10);
                if (bitmap != null)
                {
                    //检查上传的物理路径是否存在，不存在则创建
                    if (!Directory.Exists(Server.MapPath("/upload/qr_codes/")))
                    {
                        Directory.CreateDirectory(Server.MapPath("/upload/qr_codes/"));
                    }
                    bitmap.Save(Server.MapPath("/upload/qr_codes/news_" + news_id + ".png"), ImageFormat.Png);
                }
            }
            return img_url;
        }

        /// <summary>
        /// 生成图文二维码
        /// </summary>
        /// <param name="obj_id">自己的id</param>
        /// <param name="link_url">前台页面，绝对路劲，不能带http</param>
        /// <param name="img_name">生成的二维码图片的名称，不用担心图片重名的问题</param>
        /// <param name="is_outlink">是否外链</param>
        /// <param name="is_cover">是否覆盖之前</param>
        /// <returns></returns>
        public string CreateQrCode(int obj_id, string link_url, string img_name, bool is_outlink = false, bool is_cover = false)
        {
            string img_url = "/upload/qr_codes/" + img_name + "_" + obj_id + ".png";
            if (is_cover)
            {
                try
                {
                    File.Delete(Server.MapPath(img_url));
                }
                catch (Exception)
                {
                }
            }
            if (!File.Exists(Server.MapPath(img_url)))
            {
                QuickMark.CreateTwoCode ctc = new QuickMark.CreateTwoCode();
                string link = wchatConfig.domain + link_url;
                if (is_outlink)
                {
                    link = link_url;
                }
                var bitmap = ctc.CreateCode(link, CreateTwoCode.CodeType.Byte, CreateTwoCode.Correct.M, 5, 10);
                if (bitmap != null)
                {
                    //检查上传的物理路径是否存在，不存在则创建
                    if (!Directory.Exists(Server.MapPath("/upload/qr_codes/")))
                    {
                        Directory.CreateDirectory(Server.MapPath("/upload/qr_codes/"));
                    }
                    bitmap.Save(Server.MapPath(img_url), ImageFormat.Png);
                }
            }
            return img_url;
        }



        #region JS提示============================================

        /// <summary>
        /// 添加编辑删除提示
        /// </summary>
        /// <param name="msgtitle">提示文字</param>
        /// <param name="url">返回地址</param>
        protected void JscriptMsg(string msgtitle, string url, bool is_parent = true)
        {
            string msbox = "parent.jsprint(\"" + msgtitle + "\", \"" + url + "\")";
            if (!is_parent)
                msbox = "jsprint_child(\"" + msgtitle + "\", \"" + url + "\")";
            ClientScript.RegisterClientScriptBlock(Page.GetType(), "JsPrint", msbox, true);
        }
        /// <summary>
        /// 带回传函数的添加编辑删除提示
        /// </summary>
        /// <param name="msgtitle">提示文字</param>
        /// <param name="url">返回地址</param>
        /// <param name="callback">JS回调函数</param>
        protected void JscriptMsg(string msgtitle, string url, string callback, bool is_parent = true)
        {
            string msbox = "parent.jsprint(\"" + msgtitle + "\", \"" + url + "\",0, " + callback + ")";
            if (!is_parent)
                msbox = "jsprint_child(\"" + msgtitle + "\", \"" + url + "\", " + callback + ")";
            ClientScript.RegisterClientScriptBlock(Page.GetType(), "JsPrint", msbox, true);
        }





        #endregion
    }
}
