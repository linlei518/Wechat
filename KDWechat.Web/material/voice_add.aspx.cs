using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;

namespace KDWechat.Web.material
{
    public partial class voice_add : KDWechat.Web.UI.BasePage
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
        protected bool isnotop
        {
            get
            {
                string tef = RequestHelper.GetQueryString("tef");
                if (tef == "1895623541")
                {
                    return true;

                }
                else
                {
                    return false;
                }
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
                if (isnotop)
                {
                    this.TopControl1.Visible = false;
                    this.MenuList1.Visible = false;
                }
                WriteReturnPage(hfReturlUrl, "voice_list.aspx?m_id=" + m_id + "&is_pub=" + is_pub);
                bindGroup();
                if (id > 0)
                {
                    CheckUserAuthority((is_public == 1 ? "material_voice_public" : "material_voice"), RoleActionType.Edit, hfReturlUrl.Value);
                    bindDisplay();
                }
                else
                {
                    CheckUserAuthority((is_public == 1 ? "material_voice_public" : "material_voice"), RoleActionType.Add, hfReturlUrl.Value);
                }

            }
        }
        /// <summary>
        /// 绑定分组
        /// </summary>
        private void bindGroup()
        {
            List<KDWechat.DAL.t_wx_group_tags> list = KDWechat.BLL.Users.wx_group_tags.GetListByChannelId((int)channel_idType.素材分组, (is_public == 1 ? 0 : wx_id), (is_public == 1 ? "" : wx_og_id), is_public, -1);
            if (list != null)
            {
                foreach (KDWechat.DAL.t_wx_group_tags m in list)
                {
                    ListItem item = new ListItem(m.title, m.id.ToString());
                    if (m.status == 0)
                    {
                        item.Attributes.Add("disabled", "disabled");
                    }
                    ddlGroup.Items.Add(item);
                }
            }
        }

        private void bindDisplay()
        {
            KDWechat.DAL.t_wx_media_materials model = KDWechat.BLL.Chats.wx_media_materials.GetMediaMaterialByID(id);
            if (model != null)
            {
                if (is_public == 0 && model.wx_id != wx_id)
                {
                    //Response.Redirect("multi_news_add.aspx?is_pub=" + is_pub+"&m_id="+m_id);
                    JsHelper.AlertAndRedirect("访问地址错误", hfReturlUrl.Value, "fail");
                }
                if (model.channel_id == (int)media_type.素材语音)
                {
                    txtTitle.Text = model.title;
                    hftitle.Value = model.title;
                    txtContents.Text = model.remark;
                    ddlGroup.SelectedValue = model.group_id.ToString();
                    hf_old_file.Value = model.file_url;
                    txtFile.Text = model.file_url;
                    hf_name.Value = model.file_url;
                    hf_size.Value = model.file_size;
                    hf_type.Value = model.file_ext;
                }
                else
                {
                    switch (model.channel_id)
                    {
                        case (int)media_type.素材视频:
                            Response.Redirect("video_add.aspx?id=" + id + "&m_id=46");
                            break;
                        case (int)media_type.素材图片库:
                            Response.Redirect("pic_add.aspx?id=" + id + "&m_id=44");
                            break;
                        case (int)media_type.图文模板图片库:
                            Response.Redirect("/setting/template_pic_add.aspx?id=" + id + "&m_id=92");
                            break;
                    }
                }

            }
            else
            {
                JsHelper.AlertAndRedirect("该语音不存在", hfReturlUrl.Value, "fail");
            }
        }
        //提交
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (is_public == 0)
            {
                CheckWXid();
            }
            KDWechat.DAL.t_wx_media_materials model = null;
            string file_url = Utils.DropHTML(hf_name.Value.Trim());
            string ext = file_url.Substring(file_url.LastIndexOf('.') + 1);
            if (!Utils.IsVoice(ext))
            {
                JsHelper.Alert(Page,"请上传mp3,amr类型的语音文件","true");
                return;
            }

            if (id > 0)
            { bool is_exists = false;
                if (hftitle.Value != Utils.DropHTML(txtTitle.Text.Trim()))
                {
                    is_exists = KDWechat.BLL.Chats.wx_media_materials.Exists(Utils.DropHTML(txtTitle.Text.Trim()), (int)media_type.素材语音, wx_id);
                }
                if (!is_exists)
                {
                    //判断是否重新上传了图片
                    model = KDWechat.BLL.Chats.wx_media_materials.Update(
                        id,
                        Utils.DropHTML(txtTitle.Text.Trim()),
                        KDWechat.Common.Utils.Filter(txtContents.Text.Trim()),
                        Utils.StrToInt(ddlGroup.SelectedValue, 0),
                        file_url,
                        ext,
                        hf_size.Value,
                        -1,
                        "",
                        "",
                        1,
                        is_public, (hf_old_file.Value.Trim() == txtFile.Text.Trim() ? false : true));
                    if (model != null)
                    {
                        AddLog("修改语音素材：" + model.title, LogType.修改);
                        JsHelper.AlertAndRedirect("保存成功", hfReturlUrl.Value);
                    }
                    else
                    {
                        JsHelper.Alert(Page,"保存失败，该语音已不存在", "true");
                    }
                }
                else
                {
                    JsHelper.AlertAndRedirect("语音标题已存在", hfReturlUrl.Value, "fail");
                }
            }
            else
            {
                if (!KDWechat.BLL.Chats.wx_media_materials.Exists(Utils.DropHTML(txtTitle.Text.Trim()), (int)media_type.素材语音, wx_id))
                {

                    model = KDWechat.BLL.Chats.wx_media_materials.Add(
                      (is_public == 1 ? 0 : wx_id),

                        u_id,
                        Utils.DropHTML(txtTitle.Text.Trim()),
                        KDWechat.Common.Utils.Filter(txtContents.Text.Trim()),
                        Utils.StrToInt(ddlGroup.SelectedValue, 0),
                        file_url,
                        ext,
                        hf_size.Value,
                        media_type.素材语音,
                        -1,
                        "",
                        "",
                        DateTime.Now.AddDays(-7),
                        1,
                        is_public);
                    if (model != null)
                    {
                        AddLog("添加语音素材：" + model.title, LogType.添加);
                        JsHelper.AlertAndRedirect("保存成功", hfReturlUrl.Value);
                    }
                    else
                    {
                        JsHelper.Alert(Page, "保存失败", "true");
                    }
                }
                else
                {
                    JsHelper.AlertAndRedirect("语音标题已存在", hfReturlUrl.Value, "fail");
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(hfReturlUrl.Value);
        }
    }
}