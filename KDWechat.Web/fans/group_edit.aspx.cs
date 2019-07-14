using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;
using KDWechat.DAL;
using KDWechat.BLL.Users;

namespace KDWechat.Web.fans
{
    public partial class group_edit : KDWechat.Web.UI.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                if (id > 0)
                {
                    CheckUserAuthority("wechat_user_group", RoleActionType.Edit);
                    KDWechat.DAL.t_wx_group_tags model = KDWechat.BLL.Users.wx_group_tags.GetModel(id);
                    if (model != null)
                    {
                        txtTitle.Text = model.title;
                        hftitle.Value = model.title;
                    }
                    else
                    {
                        Response.Redirect("group_edit.aspx");
                    }
                }
                else
                    CheckUserAuthority("wechat_user_group",RoleActionType.Add);
            }

        }

        protected int id
        {
            get { return RequestHelper.GetQueryInt("id", 0); }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            CheckWXid();

            KDWechat.DAL.t_wx_group_tags model = new DAL.t_wx_group_tags()
            {
                title = Utils.DropHTML(txtTitle.Text.Trim()),
                contents = "",
                channel_id = (int)channel_idType.关注用户分组,
                is_public = 0,
                status = 1,
                wx_id = wx_id,
                wx_og_id = wx_og_id,
                create_time = DateTime.Now
            };
            if (id > 0)
            {
                bool is_exists = true;
                if (hftitle.Value.Trim().Length > 0)
                {
                    if (hftitle.Value.Trim() != model.title.Trim())
                    {
                        is_exists = KDWechat.BLL.Users.wx_group_tags.CheckTagOrGroup(model.title, new KDWechat.Web.UI.BasePage().wx_id, model.channel_id);
                    }

                }
                if (is_exists)
                {
                    bool isc = KDWechat.BLL.Users.wx_group_tags.Update(id, model);
                    if (isc)
                    {
                        AddLog("修改粉丝分组：" + model.title, LogType.修改);
                        JsHelper.RegisterScriptBlock(this, "backParentPage('success','保存成功');");
                    }
                    else
                    {
                        JsHelper.Alert(Page,"保存失败，该分组已不存在","true");
                    }
                }
                else
                {
                    JsHelper.Alert(Page, "分组名称已存在", "true");
                }
            }
            else
            {
                bool isc = KDWechat.BLL.Users.wx_group_tags.CheckTagOrGroup(model.title, new KDWechat.Web.UI.BasePage().wx_id, model.channel_id);
                if (isc)
                {
                    model = KDWechat.BLL.Users.wx_group_tags.Add(model);
                    if (model != null)
                    {
                        AddLog("添加粉丝分组：" + model.title, LogType.添加);
                        JsHelper.RegisterScriptBlock(this, "backParentPage('success','保存成功');");
                    }
                    else
                    {
                        JsHelper.Alert(Page, "保存失败", "true");
                    }
                }
                else
                {
                    JsHelper.Alert(Page, "分组名称已存在", "true");
                }
            }

        }

    }
}