using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;
using KDWechat.BLL.Chats;
using KDWechat.DAL;

namespace KDWechat.Web.module
{
    public partial class module_power : Web.UI.BasePage
    {
        protected int module_id { get { return RequestHelper.GetQueryInt("id", -1); } }
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckUserAuthority("sys_module");
            if (!IsPostBack)
            {
                InitCheckBox();
                InitData();
            }
        }

        private void InitCheckBox()
        {
            var wechatList = wx_wechats.GetList();

            //wechatList.Add(new t_wx_wechats() { id = 0, wx_pb_name = "全部" });
            //wechatList.Reverse();

            chbAllowWechats.DataSource = wechatList;

            chbAllowWechats.DataTextField = "wx_pb_name";
            chbAllowWechats.DataValueField = "id";

            chbAllowWechats.DataBind();
        }

        private void InitData()
        {
            if (module_id > 0)
            {
                var model = modules.GetModel(module_id);
                string[] wechatIds = model.allow_wechats.Split(',');

                if (wechatIds.Length > 1)
                    wechatIds = (from x in wechatIds where x != "0" select x).ToArray();

                foreach (ListItem x in chbAllowWechats.Items)
                {
                    if (wechatIds.Contains(x.Value))
                    {
                        x.Selected = true;
                    }
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string wechatIDs = "0,";
            foreach (ListItem item in chbAllowWechats.Items)
            {
                if (item.Selected)
                {
                    wechatIDs += item.Value + ",";
                    //module_wx_switch.ChangeStatusToOk(module_id, int.Parse(item.Value));
                }
                else
                {
                    module_wx_switch.RemoveByMid(module_id, int.Parse(item.Value));
                    module_menu.DeletebyMID(module_id, int.Parse(item.Value));
                }
            }
            wechatIDs = wechatIDs.TrimEnd(',');
            if (module_id > 0)
            {
                var module = modules.GetModel(module_id);
                module.allow_wechats = wechatIDs;
                modules.Update(module); 
                AddLog("设置了模块" + module.title + "的权限。", LogType.修改);

                JsHelper.RegisterScriptBlock(this, " backParentPage('success', '权限设置成功');");
              
            }
            else
                JsHelper.Alert(Page,"请不要乱入","true");
        }
    }
}