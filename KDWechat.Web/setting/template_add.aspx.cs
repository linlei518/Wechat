using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;

namespace KDWechat.Web.setting
{
    public partial class template_add : KDWechat.Web.UI.BasePage
    {
        protected int id
        {
            get { return RequestHelper.GetQueryInt("id", 0); }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                CheckWXid();

                WriteReturnPage(hfReturlUrl, "template_list.aspx?m_id=" + m_id);
                if (id > 0)
                {
                    CheckUserAuthority("template_list", RoleActionType.Edit, hfReturlUrl.Value);
                    bindDisplay();
                }
                else
                {
                    CheckUserAuthority("template_list", RoleActionType.Add, hfReturlUrl.Value);
                }

            }
        }


        private void bindDisplay()
        {
            KDWechat.DAL.t_wx_templates model = KDWechat.BLL.Chats.wx_templates.GetModel(id);
            if (model != null)
            {
                if (model.channel_id == 2)
                {
                    JsHelper.AlertAndRedirect("访问地址错误", hfReturlUrl.Value);
                    return;
                }
                if (model.cate_id > -1)
                {
                    JsHelper.AlertAndRedirect("访问地址错误", hfReturlUrl.Value);
                    return;
                }

                KDWechat.DAL.t_wx_templates_wechats model_wechat = KDWechat.BLL.Chats.wx_templates.GetTemplateWechatModel(id, wx_id);
                if (model_wechat == null)
                {
                    JsHelper.AlertAndRedirect("访问地址错误", hfReturlUrl.Value);
                    return;
                }
                img_show.Src = model.img_url;
                txtFile.Text = model.img_url;
                hf_old_file.Value = model.img_url;
                txtTitle.Text = model.title;
                txtContents.Text = model.contents;
                rblDefault.SelectedValue = model.is_default.ToString();
                txtRemark.Value = model.remark;
                hftitle.Value = model.title;
            }
            else
            {
                JsHelper.AlertAndRedirect("该模板已不存在", hfReturlUrl.Value);
            }
        }
        //提交
        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            CheckWXid();

            KDWechat.DAL.t_wx_templates model = null;
            if (id > 0)
            {
                model = KDWechat.BLL.Chats.wx_templates.GetModel(id);
                if (model == null)
                {
                    JsHelper.AlertAndRedirect("模板修改失败，该模板已不存在", hfReturlUrl.Value);
                    return;
                }
                model.img_url = Utils.DropHTML(txtFile.Text.Trim());
                model.title = Utils.DropHTML(txtTitle.Text.Trim());
                model.remark = Utils.DropHTML(txtRemark.Value.Trim());
                model.contents = txtContents.Text.Trim();// Utils.Filter(txtContents.Text.Trim());
                //  model.is_default = Common.Utils.StrToInt(rblDefault.SelectedValue, 0);
                //判断是否重新上传了图片

                bool is_exists = false;
                if (hftitle.Value != Utils.DropHTML(txtTitle.Text.Trim()))
                {
                    is_exists = KDWechat.BLL.Chats.wx_templates.Exists(model.title, (int)TemplateType.微信, (int)TemplateCategoryType.自定义图文模版, wx_id);
                }
                if (!is_exists)
                {
                    bool is_new_file = false;
                    string new_img_path = txtFile.Text.Trim();
                    if (txtFile.Text.Trim().ToLower().Contains("http") == false)
                    {
                        if (hf_old_file.Value != txtFile.Text.Trim())
                        {
                            is_new_file = true;
                            new_img_path = "/upload/newstemplate/images/" + DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.Ticks + ".jpg";
                            model.img_url = new_img_path;
                        }
                    }
                   
                    bool isc = KDWechat.BLL.Chats.wx_templates.Update(model);
                    if (isc)
                    {
                        if (is_new_file)
                        {
                            //检查上传的物理路径是否存在，不存在则创建
                            if (!Directory.Exists(Server.MapPath("/upload/newstemplate/images/" + DateTime.Now.ToString("yyyyMM") + "/")))
                            {
                                Directory.CreateDirectory(Server.MapPath("/upload/newstemplate/images/" + DateTime.Now.ToString("yyyyMM") + "/"));
                            }
                            if (File.Exists(Server.MapPath(txtFile.Text.Trim())))
                            {
                                File.Copy(Server.MapPath(txtFile.Text.Trim()), Server.MapPath(new_img_path), true);
                            }
                            try
                            {
                                File.Delete(Server.MapPath(hf_old_file.Value.Trim()));
                            }
                            catch (Exception)
                            {
                            }

                        }
                        AddLog("修改图文模板：" + model.title, LogType.修改);
                        JsHelper.AlertAndRedirect("保存成功", hfReturlUrl.Value);
                    }
                    else
                    {
                        JsHelper.Alert(Page, "保存失败，该模板已不存在", "true");
                    }
                }
                else
                {
                    JsHelper.AlertAndRedirect("模板名称已存在", hfReturlUrl.Value, "fail");
                }
            }
            else
            {
                bool is_new_file = true;
                string new_img_path = "/upload/newstemplate/images/" + DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.Ticks + ".jpg";
                if (txtFile.Text.Trim().ToLower().Contains("http") == true)
                {
                    is_new_file = false;
                    new_img_path = txtFile.Text.Trim();
                }
                model = new DAL.t_wx_templates()
                {
                    is_default = 0,
                    cate_id = -1,
                    channel_id = 1,
                    contents = txtContents.Text.Trim(),//Utils.Filter(txtContents.Text.Trim()),
                    create_time = DateTime.Now,
                    file_path = "",
                    img_url = new_img_path,
                    remark = Utils.DropHTML(txtRemark.Value.Trim()),
                    sort_id = 99,
                    status = 1,
                    title = Utils.DropHTML(txtTitle.Text.Trim())
                };
                DAL.t_wx_templates_wechats model_wechat = new DAL.t_wx_templates_wechats()
                {
                    is_default = 0,
                    template_id = 0,
                    wx_id = wx_id,
                    wx_og_id = wx_og_id,
                    channel_id = 1
                };
                if (!KDWechat.BLL.Chats.wx_templates.Exists(model.title, (int)TemplateType.微信, (int)TemplateCategoryType.自定义图文模版, wx_id))
                {
                    int num = KDWechat.BLL.Chats.wx_templates.Add(model, model_wechat);
                    if (num > 0)
                    {
                        if (is_new_file)
                        {
                            //检查上传的物理路径是否存在，不存在则创建
                            if (!Directory.Exists(Server.MapPath("/upload/newstemplate/images/" + DateTime.Now.ToString("yyyyMM") + "/")))
                            {
                                Directory.CreateDirectory(Server.MapPath("/upload/newstemplate/images/" + DateTime.Now.ToString("yyyyMM") + "/"));
                            }
                            if (File.Exists(Server.MapPath(txtFile.Text.Trim())))
                            {
                                File.Copy(Server.MapPath(txtFile.Text.Trim()), Server.MapPath(new_img_path), true);
                            }
                        }
                        AddLog("添加图文模板：" + model.title, LogType.添加);
                        JsHelper.AlertAndRedirect("保存成功", hfReturlUrl.Value);
                    }
                    else
                    {
                        JsHelper.Alert(Page, "模板添加失败", "true");
                    }
                }
                else
                {
                    JsHelper.AlertAndRedirect("模板名称已存在", hfReturlUrl.Value, "fail");
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(hfReturlUrl.Value);
        }
    }
}