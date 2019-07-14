using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;

namespace KDWechat.Web.material
{
    public partial class group_add : KDWechat.Web.UI.BasePage
    {
        protected int id
        {
            get { return RequestHelper.GetQueryInt("id", 0); }
        }
        /// <summary>
        /// 是否公共素材
        /// </summary>
        protected int is_public
        {
            get
            {
                int _is_pub = 0;
                if (is_pub == "1.1.1")
                {
                    _is_pub = 1;
                }
                return _is_pub;
            }
        }

        /// <summary>
        /// 公共素材标记文本
        /// </summary>
        protected string is_pub
        {
            get { return RequestHelper.GetQueryString("is_pub"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                if (is_public == 0)
                {
                    CheckWXid();
                }
                if (id > 0)
                {
                    CheckUserAuthority((is_public == 1 ? "material_group_public" : "material_group"),RoleActionType.Edit);
                    bindDisplay();
                }
                else
                {
                    CheckUserAuthority((is_public == 1 ? "material_group_public" : "material_group"), RoleActionType.Add);
                }

            }
        }

        private void bindDisplay()
        {
            KDWechat.DAL.t_wx_group_tags model = KDWechat.BLL.Users.wx_group_tags.GetModel(id);
            if (model != null)
            {
                if (model.wx_id!=wx_id)
                {
                    Response.Redirect("group_add.aspx?is_pub=" + is_pub);
                    return;
                }
                txtTitle.Text = model.title;
                hftitle.Value = model.title;
            }
            else
            {
                JsHelper.AlertAndRedirect("该分组不存在", "group_add.aspx?is_pub=" + is_pub);
            }
        }
        //提交
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (is_public == 0)
            {
                CheckWXid();
            }
            KDWechat.DAL.t_wx_group_tags model = new DAL.t_wx_group_tags()
            {
                title = Utils.DropHTML(txtTitle.Text.Trim()),
                contents = "",
                channel_id = (int)channel_idType.素材分组,
                is_public = is_public,
                status = 1,
                wx_id = (is_public == 1 ? 0 : wx_id),
                wx_og_id = (is_public == 1 ? "" : wx_og_id),
                create_time = DateTime.Now,
                u_id=u_id
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
                        AddLog("修改素材分组：" + model.title, LogType.修改);
                        JsHelper.RegisterScriptBlock(this, "backParentPage('success','保存成功');");
                    }
                    else
                    {
                        JsHelper.Alert(Page, "保存失败，该素材不存在", "true");
                    }
                }
                else
                {
                    JsHelper.RegisterScriptBlock(this, "backParentPage('fail','分组名称已存在');");
                    
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
                        AddLog("添加素材分组：" + model.title, LogType.添加);
                        JsHelper.RegisterScriptBlock(this, "backParentPage('success','保存成功');");
                    }
                    else
                    {
                        JsHelper.Alert(Page,"保存失败","true");
                    }
                }
                else
                {
                    JsHelper.RegisterScriptBlock(this, "backParentPage('fail','分组名称已存在');");
                }

            }
        }


    }
}