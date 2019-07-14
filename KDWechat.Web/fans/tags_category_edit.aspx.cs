using KDWechat.Common;
using KDWechat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.fans
{
    public partial class tags_category : Web.UI.BasePage
    {
        protected string errorMsg = "";
        protected int parent_id { get { return RequestHelper.GetQueryInt("pid", 0); } }
        protected int id
        {
            get { return RequestHelper.GetQueryInt("id", 0); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (id > 0)
                {
                    //CheckUserAuthority("tag_list", RoleActionType.Edit);
                    var model = Companycn.Core.EntityFramework.EFHelper.GetModel<creater_wxEntities, t_tags_category>(x => x.id == id);//KDWechat.BLL.Users.wx_group_tags.GetModel(id);
                    if (model != null)
                    {
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
                    //CheckUserAuthority("tag_list", RoleActionType.Add);
                }
            }

        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //CheckUserAuthority("tag_list");

            var model = new t_tags_category()
            {
                title = Utils.DropHTML(txtTitle.Text.Trim()),
                wx_id = 0,
                wx_og_id = wx_og_id,
                creat_time = DateTime.Now,
                parent_id = parent_id
            };
            if (id > 0)
            {
                bool is_exists = true;
                if (hftitle.Value.Trim().Length > 0)
                {
                    if (hftitle.Value.Trim() != model.title.Trim())
                    {
                        is_exists = Companycn.Core.EntityFramework.EFHelper.GetModel<creater_wxEntities, t_tags_category>(x => x.title == model.title) == null;
                    }

                }
                if (is_exists)
                {
                    bool isc = Companycn.Core.EntityFramework.EFHelper.UpdateModel<creater_wxEntities, t_tags_category>(x => x.id == id, x => new t_tags_category { title = model.title })>0;
                    if (isc)
                    {
                        AddLog("修改标签类型：" + model.title, LogType.修改);
                        JsHelper.RegisterScriptBlock(this, "backParentPage('success','标签类型修改成功')");
                    }
                    else
                    {
                        JsHelper.RegisterScriptBlock(this, "backParentPage('fail','标签类型修改失败，该标签类型已不存在')");

                    }
                }
                else
                {
                    JsHelper.RegisterScriptBlock(this, "backParentPage('fail','类型名称已存在')");
                }
            }
            else
            {
                bool isc = Companycn.Core.EntityFramework.EFHelper.GetModel<creater_wxEntities, t_tags_category>(x => x.title == model.title) == null;
                if (isc)
                {
                    model = Companycn.Core.EntityFramework.EFHelper.AddModel<creater_wxEntities, t_tags_category>(model);
                    if (model != null)
                    {
                        AddLog("添加标签类型：" + model.title, LogType.添加);
                        JsHelper.RegisterScriptBlock(this, "backParentPage('success','保存成功')");
                    }
                    else
                    {
                        JsHelper.RegisterScriptBlock(this, "backParentPage('fail','保存失败')");
                    }
                }
                else
                {
                    JsHelper.RegisterScriptBlock(this, "backParentPage('fail','标签类型已存在')");

                }
            }

        }
    }
}