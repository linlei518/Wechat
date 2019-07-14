using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using KDWechat.Common;
using Companycn.Core.DbHelper;
using System;
using KDWechat.Web.UI;

namespace KDWechat.Web.UserControl
{
    public partial class TopControl : Web.UI.BaseControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblUserName.Text = u_name;

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
                                return;
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
                            lblWeiXin.Text += "<h1><a  >" + wx_name + "<i class=\"moreInfo\"></i></a></h1><ul>";
                            foreach (DataRow r in dt.Rows)
                            {
                                lblWeiXin.Text += "<li><a href=\"/select_wechat.aspx?id=" + r["id"] + "\">" + r["wx_pb_name"] + "</a></li>";
                            }
                            lblWeiXin.Text += "</ul>";
                        }

                    }
                }

                #endregion

                #region 地区帐号和子帐号都要有“公众号管理”
                //if (u_type > 1 && u_type < 4)
                //{
                //    //判断该帐号是否有微信公众号管理权限
                  

                //       // lblTopMenu.Text += "<li><a href=\"/Account/region_wxlist.aspx?m_id=59\" class=\"current\">微信</a></li>";
                //        //地区帐号和子帐号都要有“公众号管理”
                //        lblMenu.Text = "<li><a href=\"/Account/region_wxlist.aspx?m_id=59\">公众号管理</a></li>";
 
                //    if (u_type == 2)
                //    {   //地区帐号加个“子帐号管理”
                //        lblMenu.Text += "<li><a href=\"/Account/regoin_account.aspx?m_id=60\">子帐号管理</a></li>";
                //    }
                //}
                //else
                //{
                //    lblMenu.Text = "<li><a href=\"/Account/region_wxlist.aspx?m_id=97\">公众号管理</a></li>";
                //}
                #endregion


                #region 加载出当前用户能管理的系统顶部菜单
                //判断该帐号是否有微信公众号管理权限
                if (user_manage_child_sys == null)
                {
                    load_user_manage_child_sys();
                }

                //if (user_manage_child_sys.nav_type_ids.Contains(",1,"))
                //{
                    //地区帐号和子帐号都要有“公众号管理”
                    lblMenu.Text = "<li><a href=\"/Account/region_wxlist.aspx?m_id=59\">公众号管理</a></li>";
                //}
                //else if (user_manage_child_sys.nav_type_ids.Contains(",50,"))
                //{
                //    //地区帐号和子帐号都要有“公众号管理”
                //    lblMenu.Text = "<li><a href=\"/Account/region_wxlist.aspx?m_id=97\">公众号管理</a></li>";
                //}

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
                    if (item.id == 1 || item.id == 50 || item.id == 58)
                    {
                        class_name = "class=\"current\"";
                    }


                    if (u_type == 3)
                    {
                        DAL.t_sys_users_power power = BLL.Users.sys_users_power.GetPowerRole(-1, u_id, item.id);
                        if (power != null)
                        {
                            lblTopMenu.Text += "<li><a  " + item.target_type + " " + class_name + " href=\"" + item.link_url + "\" >" + item.title + "</a></li>";
                        }
                    }
                    else
                    {
                        lblTopMenu.Text += "<li><a " + item.target_type + "  " + class_name + " href=\"" + item.link_url + "\" >" + item.title + "</a></li>";
                    }

                }
                #endregion


                #region 加载消息提醒的数量

                int count = 0;

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
                        lblNoReplyCount.Text = "<li><a href=\"/fans/message_list.aspx?key=&beginDate=&endDate=&m_id=80&replyStatus=0\">未回复粉丝：<em>" + no_reply_count + "</em>个</a></li>";
                        count += no_reply_count;
                    }



                }

                #endregion

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
                //lit_letCount.Text = "<li><a href='../Account/letter_list_rec.aspx?m_id=" + _mid + "'>站内信：<em>" + msgCount + "</em>条未读</a></li>";
                //count += msgCount;

                #endregion

                #region 修改密码
                #endregion


                #region 加载未读的站内信

                #endregion
                if (count > 0)
                {
                    lblUserName.Text += "<sup>" + count + "</sup>";
                }
                #endregion


            }
        }
    }
}