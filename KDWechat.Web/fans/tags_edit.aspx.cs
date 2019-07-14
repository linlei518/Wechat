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
    public partial class tags_edit : KDWechat.Web.UI.BasePage
    {
        protected string errorMsg = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                ddlGroup.DataSource = Companycn.Core.EntityFramework.EFHelper.GetList<creater_wxEntities, t_tags_category, int>(x => true, x => x.id, int.MaxValue, 1, true);
                ddlGroup.DataTextField = "title";
                ddlGroup.DataValueField = "id";
                ddlGroup.DataBind();
                if (id > 0)
                {
                    CheckUserAuthority("tag_list", RoleActionType.Edit);
                    KDWechat.DAL.t_wx_group_tags model = KDWechat.BLL.Users.wx_group_tags.GetModel(id);
                    if (model != null)
                    {
                        //txtContents.Value = model.contents;
                        txtTitle.Text = model.title;
                        hftitle.Value = model.title;
                    }
                    else
                    {
                        Response.Redirect("tags_edit.aspx");
                    }
                }
                else
                {
                    CheckUserAuthority("tag_list", RoleActionType.Add);
                }
            }

        }

        protected int id
        {
            get { return RequestHelper.GetQueryInt("id", 0); }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            CheckUserAuthority("tag_list");

            KDWechat.DAL.t_wx_group_tags model = new DAL.t_wx_group_tags()
            {
                title = Utils.DropHTML(txtTitle.Text.Trim()),
                contents = string.Empty,
                channel_id = (int)channel_idType.关注用户标签,
                is_public = 0,
                status = 1,
                wx_id = 0,
                wx_og_id = wx_og_id,
                create_time = DateTime.Now,
                parent_id = Utils.StrToInt(ddlGroup.SelectedValue,-1)
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
                        AddLog("修改粉丝标签：" + model.title, LogType.修改);
                        JsHelper.RegisterScriptBlock(this, "backParentPage('success','标签修改成功')");
                    }
                    else
                    {
                        JsHelper.RegisterScriptBlock(this, "backParentPage('fail','标签修改失败，该粉丝标签已不存在')");
                       
                    }
                }
                else
                {
                    JsHelper.RegisterScriptBlock(this, "backParentPage('fail','标签名称已存在')");
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
                        AddLog("添加粉丝标签：" + model.title, LogType.添加);
                       // JsHelper.RegisterScriptBlock(this, "alert('保存成功');closeBox('reload_page');");
                        JsHelper.RegisterScriptBlock(this, "backParentPage('success','保存成功')");

                        //errorMsg = string.Format("  showTip.show(\"标签名称已存在\", true);");
                        
                    }
                    else
                    {
                        JsHelper.RegisterScriptBlock(this, "backParentPage('fail','保存失败')");
                    }
                }
                else
                {
                    JsHelper.RegisterScriptBlock(this, "backParentPage('fail','标签名称已存在')");
                    
                }
            }

        }

    }
}