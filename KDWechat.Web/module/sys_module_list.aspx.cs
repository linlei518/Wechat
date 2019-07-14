using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.DAL;
using KDWechat.BLL.Chats;
using System.Linq.Expressions;
using KDWechat.Common;

namespace KDWechat.Web.module
{
    public partial class sys_module_list : Web.UI.BasePage
    {
        protected int pagesize = 10;
        protected int tag { get { return RequestHelper.GetQueryInt("tag", 0); } }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckUserAuthority("sys_module");
                InitData();
            }
        }

        private void InitData()
        {
            Expression<Func<t_modules, bool>> where = x => x.status == (int)Status.正常;
            var list = modules.GetList(where, int.MaxValue, 1,out totalCount);
            repChooseList.DataSource = list;
            repChooseList.DataBind();



            where = x => x.is_sys == (int)ModuleMode.系统模块;
            if (tag != 0)
                where = where.And(x => x.type == tag);
            list = modules.GetList(where, pagesize, page,out totalCount);
            string pageUrl = "sys_module_list.aspx?m_id=" + m_id.ToString() + "&page=__id__";
            div_page.InnerHtml = Utils.OutPageList(pagesize, page, totalCount, pageUrl, 8);
            repAllSysModule.DataSource = list;
            repAllSysModule.DataBind();


        }

        protected void repChooseList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                int id = int.Parse(e.CommandArgument.ToString());
                var module =modules.RemoveOrAddModule(id);
                if (null!=module)
                {
                    BLL.Chats.module_menu.DeletebyMID(id);
                    module_wx_switch.RemoveByMid(id);
                    AddLog(string.Format("禁用模块"+module.title, id.ToString()), LogType.修改);
                    JsHelper.AlertAndRedirect("禁用成功",Request.Url.ToString());
                }
                else
                    JsHelper.Alert("禁用失败");
            }
        }

        protected void repAllSysModule_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Add")
            {
                int id = int.Parse(e.CommandArgument.ToString());
                var module = modules.RemoveOrAddModule(id);
                if (null != module)
                {
                    if (module.status == (int)Status.禁用)
                    {
                        BLL.Chats.module_menu.DeletebyMID(id);
                        module_wx_switch.RemoveByMid(id);
                    }
                    //else
                    //{
                        //module_wx_switch.ChangeStatusToOk(id);
                    //}
                    string option = module.status == (int)Status.正常 ? "启用" : "禁用";
                    AddLog(string.Format(option + "模块" + module.title, id.ToString()), LogType.修改);
                    JsHelper.AlertAndRedirect(option+ "成功", Request.Url.ToString());
                }
                else
                    JsHelper.Alert("操作失败");
            }

        }
    }
}