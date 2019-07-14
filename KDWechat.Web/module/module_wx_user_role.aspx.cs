using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.BLL;
using KDWechat.Common;
using KDWechat.DAL;

namespace KDWechat.Web.module
{
    public partial class module_wx_user_role : Web.UI.BasePage
    {
        protected int module_id { get { return RequestHelper.GetQueryInt("module_id", -1); } }
        protected void Page_Load(object sender, EventArgs e)
        {
         
            if (!IsPostBack)
            {
                InitCheckBox();
                InitData();
            }
        }

        private void InitCheckBox()
        {
            var wechatList = BLL.Users.sys_users.GetUserListByParentID(u_id,true);

            chbAllowWechats.DataSource = wechatList;

            chbAllowWechats.DataTextField = "user_name";
            chbAllowWechats.DataValueField = "id";

            chbAllowWechats.DataBind();
        }

        private void InitData()
        {
            if (module_id > 0)
            {
                DAL.t_modules model = BLL.Chats.modules.GetModel(module_id);
                if (model != null)
                {
                    hfname.Value = model.title;
                    List<string> list = BLL.Chats.modules.GetModuleWeChatUser(module_id,wx_id);
                    if (list == null)
                    {
                        list = new List<string>();
                    }

                    foreach (ListItem x in chbAllowWechats.Items)
                    {
                        if (list.Contains(x.Value))
                        {
                            x.Selected = true;
                        }
                    }
                }
                else
                {
                    JsHelper.RegisterScriptBlock(this, " backParentPage('fail', '应用不存在');");
                }


            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            List<DAL.t_module_wx_user_role> list = new List<DAL.t_module_wx_user_role>();
            List<int> remove = new List<int>();
            string name = "";
            foreach (ListItem item in chbAllowWechats.Items)
            {
                if (item.Selected)
                {
                    list.Add(new t_module_wx_user_role()
                    {
                        user_id = Common.Utils.StrToInt(item.Value, 0),
                        wx_id = wx_id,
                        module_id = module_id,
                        role =""

                    });
                    name += item.Text + ",";
                }
                else
                {
                    remove.Add(Common.Utils.StrToInt(item.Value, 0));
                }

            }
            int num = BLL.Chats.modules.AddModuleWeChatUser(list, module_id,wx_id);
            if (num > 0)
            {

                AddLog("为应用【" + hfname.Value + "】分配了以下用户：" + name.TrimEnd(','), LogType.修改);
                JsHelper.RegisterScriptBlock(this, " parent.showMsg(true, '权限分配成功');");
            }
            else
                JsHelper.RegisterScriptBlock(this, " parent.showMsg(false, '权限分配失败');");
        }
    }
}