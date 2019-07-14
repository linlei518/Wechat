using KDWechat.BLL.Chats;
using KDWechat.Common;
using KDWechat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.module
{
    public partial class module_list : Web.UI.BasePage
    {
        protected int pagesize = 10;
        protected int tag { get { return RequestHelper.GetQueryInt("tag", 0); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckUserAuthority("wechat_appmanage");
                InitData();
            }
        }


        public string RoleStr(object id)
        {
            string str = "";

            if (u_type==2)
            {
                str = " <input type=\"button\"   onclick=\"bombbox.openBox('module_wx_user_role.aspx?module_id=" + id + "');\" class=\"btn btnCancel\" value='管理权限'>";
            }
            return str;
        }

        private void InitData()
        {
            Expression<Func<t_module_wx_switch, bool>> where = x => x.wx_id == wx_id && x.status == (int)Status.正常;
            var list = module_wx_switch.GetList(where, int.MaxValue, 1, out totalCount);
            var openArray = (from x in list select x.module_id).ToArray();



            Expression<Func<t_modules, bool>> where2 = x => x.status == (int)Status.正常;
            if (tag > 0)
                where2 = where2.And(x => x.type == tag);
            var list2 = modules.GetList(where2, pagesize, page, out totalCount);
            //从尚未添加的模块中去除未授权的模块
            foreach (var selectX in list2)
            {
                var allowArray = selectX.allow_wechats.Split(',');
                if (!openArray.Contains(selectX.id))
                {
                    if (!((allowArray.Length == 1 && allowArray[0] == "0") || allowArray.Contains(wx_id.ToString())))
                        selectX.id = 0; //去除没有权限的模块
                }
            }
            list2 = (from x in list2 where x.id != 0 select x).ToList();



            //设置分页
            string pageUrl = "module_list.aspx?m_id=" + m_id.ToString() + "&page=__id__";
            div_page.InnerHtml = Utils.OutPageList(pagesize, page, totalCount, pageUrl, 8);

            //绑定repeater
            repAllSysModule.DataSource = list2;
            repAllSysModule.DataBind();


        }


        protected Status GetStatusByModuleID(string moduleid)
        {
            int module_id = Utils.StrToInt(moduleid, 0);
            var moduleSwitch = module_wx_switch.GetModelByWxIDAndModuleID(wx_id, module_id);
            if (moduleSwitch != null)
                return (Status)moduleSwitch.status;
            return Status.禁用;
        }

        protected string GetConfirmByModuleID(string moduleid)
        {
            Status status = Status.禁用;
            int module_id = Utils.StrToInt(moduleid, 0);
            var moduleSwitch = module_wx_switch.GetModelByWxIDAndModuleID(wx_id, module_id);
            if (moduleSwitch != null)
                status = (Status)moduleSwitch.status;
            return "return confirm('您确认要"+(status==Status.正常?"禁用":"启用")+"此应用？')";
        }

        protected void repAllSysModule_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Add")
            {
                int module_id = int.Parse(e.CommandArgument.ToString());
                var module_switch = module_wx_switch.GetModelByWxIDAndModuleID(wx_id, module_id);
                var module = modules.GetModel(module_id);

                if (module_switch != null)
                {
                    module_switch = module_wx_switch.RemoveOrAddModule(module_switch.id);


                    #region 添加（删除）模块菜单
                    if (module_switch.status == 1)
                    {
                        DAL.t_modules model = BLL.Chats.modules.GetModel(module_switch.module_id);

                        if (model.id == 3)  //在线销售
                        {
                            DAL.t_module_menu menu = new t_module_menu();
                            menu.wx_id = wx_id;
                            menu.module_id = model.id;
                            menu.title = "置业顾问";
                            menu.link_url = model.manage_url;
                            menu.create_time = DateTime.Now;
                            menu.sort = 0;
                            menu.parent_id = 0;
                            int p_id = BLL.Chats.module_menu.Add(menu).id;
                            if (p_id > 0)
                            {
                                //项目菜单
                                DAL.t_module_menu menuP = new t_module_menu();
                                menuP.wx_id = wx_id;
                                menuP.module_id = model.id;
                                menuP.title = "管理项目";
                                menuP.link_url = "/sales/project_list.aspx";
                                menuP.create_time = DateTime.Now;
                                menuP.sort = 1;
                                menuP.parent_id = p_id;
                                BLL.Chats.module_menu.Add(menuP);

                                //销售菜单
                                DAL.t_module_menu menuS = new t_module_menu();
                                menuS.wx_id = wx_id;
                                menuS.module_id = model.id;
                                menuS.title = "管理销售";
                                menuS.link_url = "/sales/sales_list.aspx";
                                menuS.create_time = DateTime.Now;
                                menuS.sort = 2;
                                menuS.parent_id = p_id;
                                BLL.Chats.module_menu.Add(menuS);
                            }
                        }
                       
                        else
                        {
                            if (model.id!=25)
                            {
                                DAL.t_module_menu menu = new t_module_menu();
                                menu.wx_id = wx_id;
                                menu.module_id = model.id;
                                menu.title = model.title;
                                menu.link_url = model.manage_url;
                                menu.create_time = DateTime.Now;
                                menu.sort = 0;
                                menu.parent_id = 0;
                                BLL.Chats.module_menu.Add(menu);
                            }
                        }
                    }
                    else
                    {
                        BLL.Chats.module_menu.DeletebyMID(module_id, wx_id);
                    }
                    #endregion


                    if (null != module_switch)
                    {
                        AddLog((module_switch.status == (int)Status.正常 ? "启用" : "禁用") + "模块" + module.title, LogType.修改);
                        JsHelper.AlertAndRedirect((module_switch.status == (int)Status.正常 ? "启用" : "禁用") + "成功", Request.Url.ToString());
                    }
                    else
                        JsHelper.Alert("操作失败");
                }
                else
                {
                    module_switch = new t_module_wx_switch()
                    {
                        create_time = DateTime.Now,
                        status = (int)Status.正常,
                        module_id = module_id,
                        wx_id = wx_id,
                        wx_og_id = wx_og_id
                    };
                    module_switch = module_wx_switch.Add(module_switch);

                    #region 添加模块菜单表



                    DAL.t_modules model = BLL.Chats.modules.GetModel(module_switch.module_id);

                    if (model.id == 3)  //在线销售
                    {
                        DAL.t_module_menu menu = new t_module_menu();
                        menu.wx_id = wx_id;
                        menu.module_id = model.id;
                        menu.title = "置业顾问";
                        menu.link_url = model.manage_url;
                        menu.create_time = DateTime.Now;
                        menu.sort = 0;
                        menu.parent_id = 0;

                        t_module_wechat relation = new t_module_wechat()
                        {
                            status = (int)Status.正常,
                            create_time = DateTime.Now,
                            module_id = 3,
                            u_id = u_id,
                            wx_id = wx_id,
                            wx_og_id = wx_og_id,
                            channel_id=0,
                            app_id=3,
                            app_img_url=model.img_url,
                            app_link_url="",
                            app_name="置业顾问",
                            app_parent_id=1,
                            app_remark = "置业顾问",
                            app_table = "置业顾问"
                        };

                        module_wechat.Add(relation);


                        int p_id = BLL.Chats.module_menu.Add(menu).id;
                        if (p_id > 0)
                        {

                            //项目菜单
                            DAL.t_module_menu menuP = new t_module_menu();
                            menuP.wx_id = wx_id;
                            menuP.module_id = model.id;
                            menuP.title = "管理项目";
                            menuP.link_url = "/sales/project_list.aspx";
                            menuP.create_time = DateTime.Now;
                            menuP.sort = 1;
                            menuP.parent_id = p_id;
                            BLL.Chats.module_menu.Add(menuP);

                            //销售菜单
                            DAL.t_module_menu menuS = new t_module_menu();
                            menuS.wx_id = wx_id;
                            menuS.module_id = model.id;
                            menuS.title = "管理销售";
                            menuS.link_url = "/sales/sales_list.aspx";
                            menuS.create_time = DateTime.Now;
                            menuS.sort = 2;
                            menuS.parent_id = p_id;
                            BLL.Chats.module_menu.Add(menuS);
                        }
                    }
                    else if (model.id==25)
                    {
                        t_module_wechat relation = new t_module_wechat()
                        {
                            status = (int)Status.正常,
                            create_time = DateTime.Now,
                            module_id =25,
                            u_id = u_id,
                            wx_id = wx_id,
                            wx_og_id = wx_og_id,
                            channel_id = 0,
                            app_id = 25,
                            app_img_url = model.img_url,
                            app_link_url = "",
                            app_name = "上传小票",
                            app_parent_id = 1,
                            app_remark = "上传小票",
                            app_table = "上传小票"
                        };

                        module_wechat.Add(relation);
                    }else
                    {
                        if (model.id!=25)
                        {
                            DAL.t_module_menu menu = new t_module_menu();
                            menu.wx_id = wx_id;
                            menu.module_id = model.id;
                            menu.title = model.title;
                            menu.link_url = model.manage_url;
                            menu.create_time = DateTime.Now;
                            menu.sort = 0;
                            menu.parent_id = 0;
                            BLL.Chats.module_menu.Add(menu);
                        }
                       
                    }
                }
                    #endregion
                JsHelper.AlertAndRedirect("应用启用成功", Request.Url.ToString());

            }

        }
    }
}