using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;

namespace KDWechat.Web.setting
{
    public partial class template_pic_add : KDWechat.Web.UI.BasePage
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


                WriteReturnPage(hfReturlUrl, "template_pic_add.aspx?m_id=" + m_id);

                if (id > 0)
                {
                    CheckUserAuthority("template_material", RoleActionType.Edit, hfReturlUrl.Value);
                    bindDisplay();
                }
                else
                {
                    CheckUserAuthority("template_material", RoleActionType.Add, hfReturlUrl.Value);
                }

            }
        }


        private void bindDisplay()
        {
            KDWechat.DAL.t_wx_media_materials model = KDWechat.BLL.Chats.wx_media_materials.GetMediaMaterialByID(id);
            if (model != null)
            {
                if (model.wx_id != wx_id)
                {
                    //Response.Redirect("multi_news_add.aspx?is_pub=" + is_pub+"&m_id="+m_id);
                    JsHelper.AlertAndRedirect("访问地址错误", hfReturlUrl.Value);
                }
                if (model.channel_id == (int)media_type.图文模板图片库)
                {
                    txtTitle.Text = model.title;
                    hftitle.Value = model.title;
                    txtContents.Text = model.remark;
                    hf_old_file.Value = model.file_url;
                    txtFile.Text = model.file_url;
                    hf_size.Value = model.file_size;
                    hf_type.Value = model.file_ext;
                    img_show.Src = model.file_url;
                }
                else
                {
                    switch (model.channel_id)
                    {
                        case (int)media_type.素材视频:
                            Server.Transfer("/material/video_add.aspx?id=" + id + "&m_id=46");
                            // Response.Redirect("pic_add.aspx?id=" + id);
                            break;
                        case (int)media_type.素材语音:
                            Response.Redirect("/material/voice_add.aspx?id=" + id + "&m_id=45");
                            break;
                        case (int)media_type.素材图片库:
                            Response.Redirect("/material/pic_add.aspx?id=" + id + "&m_id=44");
                            break;
                    }
                }

            }
            else
            {
                JsHelper.AlertAndRedirect("该图片不存在", hfReturlUrl.Value);
            }
        }
        //提交
        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            CheckWXid();

            KDWechat.DAL.t_wx_media_materials model = null;
            if (id > 0)
            {
                bool is_exists = false;
                if (hftitle.Value!=Utils.DropHTML(txtTitle.Text.Trim()))
                {
                    is_exists = KDWechat.BLL.Chats.wx_media_materials.Exists(Utils.DropHTML(txtTitle.Text.Trim()), (int)media_type.图文模板图片库, wx_id);
                }
                if (!is_exists)
                {
                    //判断是否重新上传了图片
                    model = KDWechat.BLL.Chats.wx_media_materials.Update(
                        id,
                        Utils.DropHTML(txtTitle.Text.Trim()),
                        KDWechat.Common.Utils.Filter(txtContents.Text.Trim()),
                       0,
                        Utils.DropHTML(txtFile.Text.Trim()),
                        hf_type.Value,
                        hf_size.Value,
                        -1,
                        "",
                        "",
                        1,
                        0, (hf_old_file.Value.Trim() == txtFile.Text.Trim() ? false : true));
                    if (model != null)
                    {
                        AddLog("修改模板图片素材：" + model.title, LogType.修改);
                        JsHelper.AlertAndRedirect("保存成功", hfReturlUrl.Value);
                    }
                    else
                    {
                        JsHelper.Alert(Page,"保存失败，该图片已不存在","true");
                    }
                }
                else
                {
                    JsHelper.Alert(Page, "图片标题已存在", "true");
                }
              
            }
            else
            {
                if (!KDWechat.BLL.Chats.wx_media_materials.Exists(Utils.DropHTML(txtTitle.Text.Trim()),(int)media_type.图文模板图片库,wx_id))
                {

                    model = KDWechat.BLL.Chats.wx_media_materials.Add(
                       wx_id,
                        u_id,
                        Utils.DropHTML(txtTitle.Text.Trim()),
                        KDWechat.Common.Utils.Filter(txtContents.Text.Trim()),
                        0,
                        Utils.DropHTML(txtFile.Text.Trim()),
                        hf_type.Value,
                        hf_size.Value,
                        media_type.图文模板图片库,
                        -1,
                        "",
                        "",
                        DateTime.Now.AddDays(-7),
                        1,
                        0);
                    if (model != null)
                    {
                        AddLog("添加模板图片素材：" + model.title, LogType.添加);
                        JsHelper.AlertAndRedirect("保存成功", hfReturlUrl.Value);
                    }
                    else
                    {
                        JsHelper.Alert(Page, "保存失败", "true");
                    }
                }
                else
                {
                    JsHelper.Alert(Page, "图片标题已存在", "true");
                }

            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(hfReturlUrl.Value);
        }
    }
}