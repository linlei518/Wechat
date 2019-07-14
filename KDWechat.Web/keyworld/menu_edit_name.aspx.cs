using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;

namespace KDWechat.Web.keyworld
{
    public partial class menu_edit_name :Web.UI.BasePage
    {
        protected int id { get { return RequestHelper.GetQueryInt("id"); } }

        protected int parent_id { get { return RequestHelper.GetQueryInt("parent_id"); } }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckUserAuthority("wechat_diymenu");
                if (id>0) //编辑
                {
                    if (!isEdit)
                    {
                        JsHelper.ResponseScript(this.Page, "window.parent.bombbox.closeBox();window.parent.dialogue.dlAlert('您没有编辑权限');");
                        return;
                    }

                    DAL.t_wx_diy_menus modeldiy = new DAL.t_wx_diy_menus();
                    modeldiy = BLL.Chats.wx_diy_menus.GetModel(id);
                    if (modeldiy != null)
                    {
                        if (modeldiy.wx_id != wx_id)
                        {
                            JsHelper.ResponseScript(this.Page, "window.parent.bombbox.closeBox();window.parent.dialogue.dlAlert('访问地址错误');");
                            return;
                        }
                        hftitle.Value = modeldiy.menu_name;
                        hfparentid.Value = modeldiy.parent_id.ToString();
                        txtName.Value = modeldiy.menu_name;
                        if (modeldiy.parent_id==0)
                        {
                            txtName.Attributes.Add("maxlength", "8");
                        }
                        else
                        {
                            txtName.Attributes.Add("maxlength", "16");
                        }
                    }
                }
                else
                {
                    if (!isAdd)
                    {
                        JsHelper.ResponseScript(this.Page, "window.parent.bombbox.closeBox();window.parent.dialogue.dlAlert('您没有添加权限');");
                        return;
                    }
                    txtName.Focus();
                    hfparentid.Value = parent_id.ToString();
                    if (parent_id == 0)
                    {
                        txtName.Attributes.Add("maxlength", "8");
                    }
                    else
                    {
                        txtName.Attributes.Add("maxlength", "16");
                    }

                    //获取菜单的数量
                    int count = KDWechat.BLL.Chats.wx_diy_menus.GetCountByWxIdAndParentId(wx_id, parent_id);
                    if (parent_id==0 && count>=3)
                    {
                        JsHelper.ResponseScript(this.Page, "window.parent.bombbox.closeBox();window.parent.dialogue.dlAlert('主菜单不能超过3条');");
                    }
                    else if (parent_id>0 && count>=5)
                    {
                        JsHelper.ResponseScript(this.Page, "window.parent.bombbox.closeBox();window.parent.dialogue.dlAlert('子菜单不能超过5条');");
                    }
                }
               
               
            }
        }
    }
}