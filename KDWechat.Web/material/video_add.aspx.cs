using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;

namespace KDWechat.Web.material
{
    public partial class video_add : KDWechat.Web.UI.BasePage
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
                //
                if (is_public == 0)
                {
                    CheckWXid();
                }

                if (isnotop)
                {
                    this.TopControl1.Visible = false;
                    this.MenuList1.Visible = false;
                }
                WriteReturnPage(hfReturlUrl, "video_list.aspx?m_id=" + m_id + "&is_pub=" + is_pub);
                bindGroup();
                if (id > 0)
                {
                    CheckUserAuthority((is_public == 1 ? "material_video_public" : "material_video"), RoleActionType.Edit, hfReturlUrl.Value);
                    bindDisplay();
                }
                else
                {
                    CheckUserAuthority((is_public == 1 ? "material_video_public" : "material_video"), RoleActionType.Add, hfReturlUrl.Value);
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
                if (is_public==0 && model.wx_id != wx_id)
                {
                    //Response.Redirect("multi_news_add.aspx?is_pub=" + is_pub+"&m_id="+m_id);
                    JsHelper.AlertAndRedirect("访问地址错误", hfReturlUrl.Value,"fail");
                }
                if (model.channel_id == (int)media_type.素材视频)
                {
                    hftitle.Value = model.title;
                    txtTitle.Text = model.title;
                    txtContents.Text = model.remark;
                    ddlGroup.SelectedValue = model.group_id.ToString();
                   
                    txtFile.Text = model.hq_music_url.Trim();
                    img_show.Src = model.hq_music_url;
                    hf_old_img.Value = model.hq_music_url;
                    txtFile_Weishi.Text = model.file_url;
                    //}
                }
                else
                {
                    switch (model.channel_id)
                    {
                        case (int)media_type.素材图片库:
                            Server.Transfer("pic_add.aspx?id=" + id + "&m_id=44");
                            // Response.Redirect("pic_add.aspx?id=" + id);
                            break;
                        case (int)media_type.素材语音:
                            Response.Redirect("voice_add.aspx?id=" + id + "&m_id=45");
                            break;
                        case (int)media_type.图文模板图片库:
                            Response.Redirect("/setting/template_pic_add.aspx?id=" + id + "&m_id=92");
                            break;
                    }
                }
            }
            else
            {
                JsHelper.AlertAndRedirect("该视频不存在", hfReturlUrl.Value, "fail");
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

            

            if (id > 0)
            {
                bool is_exists = false;
                if (hftitle.Value != Utils.DropHTML(txtTitle.Text.Trim()))
                {
                    is_exists = KDWechat.BLL.Chats.wx_media_materials.Exists(Utils.DropHTML(txtTitle.Text.Trim()), (int)media_type.素材视频, wx_id);
                }
                if (!is_exists)
                {
                    bool is_new_file = false;
                    string new_img_path = txtFile.Text.Trim();
                    if (txtFile.Text.Trim().ToLower().Contains("http")==false)
                    {
                        if (hf_old_img.Value != txtFile.Text.Trim())
                        {
                            is_new_file = true;
                            new_img_path = "/upload/material/images/" + DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.Ticks + ".jpg";
                        }
                    }
                  

                    //判断是否重新上传了图片
                    model = KDWechat.BLL.Chats.wx_media_materials.Update(
                        id,
                        Utils.DropHTML(txtTitle.Text.Trim()),
                        KDWechat.Common.Utils.Filter(txtContents.Text.Trim()),
                        Utils.StrToInt(ddlGroup.SelectedValue, 0),
                       Utils.DropHTML(txtFile_Weishi.Text.Trim()),
                        "",
                        "",
                        2,
                        "",
                        new_img_path,
                        1,
                        is_public, false);
                    if (model != null)
                    {
                        if (is_new_file)
                        {
                            //检查上传的物理路径是否存在，不存在则创建
                            if (!Directory.Exists(Server.MapPath("/upload/material/images/" + DateTime.Now.ToString("yyyyMM") + "/")))
                            {
                                Directory.CreateDirectory(Server.MapPath("/upload/material/images/" + DateTime.Now.ToString("yyyyMM") + "/"));
                            }
                            if (File.Exists(Server.MapPath(txtFile.Text.Trim())))
                            {
                                File.Copy(Server.MapPath(txtFile.Text.Trim()), Server.MapPath(new_img_path), true);
                            }
                            try
                            {
                                File.Delete(Server.MapPath(hf_old_img.Value.Trim()));
                            }
                            catch (Exception)
                            {
                            }

                        }
                        AddLog("修改视频素材：" + model.title, LogType.修改);
                        JsHelper.AlertAndRedirect("保存成功", hfReturlUrl.Value);
                    }
                    else
                    {
                        JsHelper.Alert(Page, "保存失败，该视频不存在", "true");
                    }
                }
                else
                {
                    JsHelper.AlertAndRedirect("视频标题已存在", hfReturlUrl.Value, "fail");
                }
            }
            else
            {

                bool is_new_file = true;
                string new_img_path = "/upload/material/images/"+DateTime.Now.ToString("yyyyMM")+"/"+DateTime.Now.Ticks+".jpg";
                if (txtFile.Text.Trim().ToLower().Contains("http") == true)
                {
                    is_new_file = false;
                    new_img_path = txtFile.Text.Trim();
                }
                if (!KDWechat.BLL.Chats.wx_media_materials.Exists(Utils.DropHTML(txtTitle.Text.Trim()), (int)media_type.素材视频, wx_id))
                {
                    model = KDWechat.BLL.Chats.wx_media_materials.Add(
                      (is_public == 1 ? 0 : wx_id),

                        u_id,
                        Utils.DropHTML(txtTitle.Text.Trim()),
                        KDWechat.Common.Utils.Filter(txtContents.Text.Trim()),
                        Utils.StrToInt(ddlGroup.SelectedValue, 0),
                         Utils.DropHTML(txtFile_Weishi.Text.Trim()),
                        "",
                       "",
                        media_type.素材视频,
                        2,
                        "",
                        new_img_path,
                        DateTime.Now.AddDays(-7),
                        1,
                        is_public);
                    if (model != null)
                    {
                        if (is_new_file)
                        {
                            //检查上传的物理路径是否存在，不存在则创建
                            if (!Directory.Exists(Server.MapPath("/upload/material/images/" + DateTime.Now.ToString("yyyyMM") + "/")))
                            {
                                Directory.CreateDirectory(Server.MapPath("/upload/material/images/" + DateTime.Now.ToString("yyyyMM") + "/"));
                            }
                            if (File.Exists(Server.MapPath(txtFile.Text.Trim())))
                            {
                                File.Copy(Server.MapPath(txtFile.Text.Trim()), Server.MapPath(new_img_path), true);
                            }
                        }
                       
                        AddLog("添加视频素材：" + model.title, LogType.添加);
                        JsHelper.AlertAndRedirect("保存成功", hfReturlUrl.Value);
                    }
                    else
                    {
                        JsHelper.Alert(Page, "保存失败","true");
                    }
                }
                else
                {
                    JsHelper.AlertAndRedirect("视频标题已存在", hfReturlUrl.Value, "fail");
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(hfReturlUrl.Value);
        }
    }
}