using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;
using KDWechat.DAL;
using LitJson;
using QuickMark;

namespace KDWechat.Web.material
{
    public partial class news_add : KDWechat.Web.UI.BasePage
    {
        protected string obj = string.Empty;

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

                WriteReturnPage(hfReturlUrl, "news_list.aspx?m_id=" + m_id + "&is_pub=" + is_pub);
                bindGroup();
                if (id > 0)
                {
                    CheckUserAuthority((is_public == 1 ? "material_news_public" : "material_news"), RoleActionType.Edit, hfReturlUrl.Value);
                    bindDisplay();
                }
                else
                {
                    CheckUserAuthority((is_public == 1 ? "material_news_public" : "material_news"), RoleActionType.Add, hfReturlUrl.Value);
                }

            }
        }

        /// <summary>
        /// 绑定分组
        /// </summary>
        private void bindGroup()
        {

            List<t_wx_group_tags> list = KDWechat.BLL.Users.wx_group_tags.GetListByChannelId((int)channel_idType.素材分组, (is_public == 1 ? 0 : wx_id), (is_public == 1 ? "" : wx_og_id), is_public, -1);
            if (list != null)
            {
                foreach (t_wx_group_tags m in list)
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
            KDWechat.DAL.t_wx_news_materials model = KDWechat.BLL.Chats.wx_news_materials.GetModel(id);
            if (model != null)
            {
                if (is_public == 0 && model.wx_id != wx_id)
                {
                    //Response.Redirect("multi_news_add.aspx?is_pub=" + is_pub+"&m_id="+m_id);
                    JsHelper.AlertAndRedirect("访问地址错误", hfReturlUrl.Value);
                }
                hftitle.Value = model.title;
                hf_old_img.Value = model.cover_img;
                obj += "[{";
                obj += "title: '" + model.title.Trim() + "',";
                obj += "type: '" + model.group_id + "',";
                obj += "img: '" + model.cover_img.Trim() + "',";
                obj += "summary: '" + model.summary.Replace("\r", "").Replace("\n", "").Replace("'", "’").Trim() + "',";
                obj += "intro: '" + model.author.Trim() + "',";

                obj += "pushType:'" + model.push_type.Trim() + "',";
                switch (model.push_type)
                {
                    case "article":
                        string template_name = "";
                         
                        if (model.template_id>0)
                        {
                            DAL.t_wx_templates template = BLL.Chats.wx_templates.GetModel(model.template_id);
                            if (template!=null)
                            {
                                template_name = ",template:{id:'" + model.template_id + "',title:'" + template.title.Trim() + "',img:'" + template.img_url.Trim() + "'}";

                            }
                        }
                        obj += "article:{content:'" + model.contents.Replace("\r", "").Replace("\n", "").Replace("'", "’").Trim() + "',origin:'" + model.source_url.Trim() + "'" + template_name + "},";
                        obj += "link:{content:''}";
                        break;
                    case "link":
                        obj += "article:{content:'',origin:''},";
                        obj += "link:{content:'" + model.link_url.Trim() + "'}";
                        break;
                    case "app":
                        obj += "app:{id:'" + model.app_id + "',title:'" + model.app_type_name.Trim() + "',content:'" + model.app_name.Trim() + "',img: '" + model.app_type_img.Trim() + "',link:'" + model.app_link.Trim() + "'}";
                        break;
                }
                
                obj += "}]";

                string[] imgs = Utils.GetHtmlImageSrcList(model.contents);
                if (imgs.Length>0)
                {
                    hf_content_img.Value=string.Join(",", imgs);
                }
            }
            else
            {
                JsHelper.AlertAndRedirect("该图文消息不存在", hfReturlUrl.Value);
            }
        }

        private string GetJsonText(JsonData jsonObj, string key, bool is_check = true)
        {
            

            string str = "";
            try
            {
                if (is_check)
                {
                    str = jsonObj[key] == null ? "" : Common.Utils.DropHTML(jsonObj[key].ToString());
                }
                else
                {
                    str = jsonObj[key] == null ? "" : Common.Utils.Filter(jsonObj[key].ToString());
                }

            }
            catch (Exception)
            {
            }
            if (str == null)
            {
                str = "";
            }
            return str;
        }



        //提交
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (is_public == 0)
            {
                CheckWXid();
            }

            string json_list = hfResult.Value;
            if (!string.IsNullOrEmpty(json_list))
            {
                json_list = json_list.TrimEnd(']').TrimStart('[');
                JsonData jsonData2 = JsonMapper.ToObject(json_list);
                if (jsonData2 != null)
                {
                    KDWechat.DAL.t_wx_news_materials model = null;
                    if (id > 0)
                    {
                        model = KDWechat.BLL.Chats.wx_news_materials.GetModel(id);
                    }
                    else
                    {
                        model = new t_wx_news_materials();
                    }
                    model.title = GetJsonText(jsonData2, "title");
                    model.wx_id = (is_public == 1 ? 0 : wx_id);
                    model.wx_og_id = (is_public == 1 ? "" : wx_og_id);
                    model.author = GetJsonText(jsonData2, "intro");
                    model.channel_id = (int)ResponseNewsType.单图文;
                    string img = GetJsonText(jsonData2, "img");
                    model.cover_img = img;
                    model.create_time = DateTime.Now;
                    model.group_id = Common.Utils.StrToInt(GetJsonText(jsonData2, "type"), 0);
                    model.is_public = is_public;
                    
                    model.par_id = 0;
                    model.template_id = 0;
                    model.status = 1;
                    model.summary = GetJsonText(jsonData2, "summary").Replace("\r", "").Replace("\n", "").Replace("'", "’");
                    model.contents = "";
                    model.u_id = u_id;
                    #region 新增的字段

                    model.push_type = GetJsonText(jsonData2, "pushType");

                    string article = GetJsonText(jsonData2, "article");
                    string link = GetJsonText(jsonData2, "link");
                    switch (model.push_type)
                    {
                        case "article":
                            if (!string.IsNullOrEmpty(article))
                            {
                                JsonData json_article = jsonData2["article"];// JsonMapper.ToObject(article);
                                if (json_article != null)
                                {
                                    model.contents = GetJsonText(json_article, "content", false).Replace("\r", "").Replace("\n", "").Replace("'", "’");
                                    model.source_url = GetJsonText(json_article, "origin");
                                    string template = GetJsonText(json_article, "template");

                                    if (!string.IsNullOrEmpty(template))
                                    {
                                        JsonData json_template = json_article["template"]; //JsonMapper.ToObject(template);
                                        if (json_template != null)
                                        {
                                            model.template_id = Common.Utils.StrToInt(GetJsonText(json_template, "id"), 0);
                                        }
                                    }

                                }
                            }

                            if (!string.IsNullOrEmpty(link))
                            {
                                JsonData json_link = jsonData2["link"];// JsonMapper.ToObject(link);
                                if (json_link != null)
                                {
                                    model.link_url = GetJsonText(json_link, "content");
                                }
                            }
                            break;
                        case "link":

                            if (!string.IsNullOrEmpty(article))
                            {
                                JsonData json_article = jsonData2["article"];// JsonMapper.ToObject(article);
                                if (json_article != null)
                                {
                                    model.contents = GetJsonText(json_article, "content", false).Replace("\r", "").Replace("\n", "").Replace("'", "’");
                                    model.source_url = GetJsonText(json_article, "origin");
                                }
                            }

                            if (!string.IsNullOrEmpty(link))
                            {
                                JsonData json_link = jsonData2["link"]; //JsonMapper.ToObject(link);
                                if (json_link != null)
                                {
                                    model.link_url = GetJsonText(json_link, "content");
                                }
                            }
                            break;
                        case "app":
                            string app = GetJsonText(jsonData2, "app");
                            if (!string.IsNullOrEmpty(app))
                            {
                                JsonData json_app = jsonData2["app"];// JsonMapper.ToObject(app);
                                if (json_app != null)
                                {
                                    model.app_id = Common.Utils.StrToInt(GetJsonText(json_app, "id"), 0);
                                    model.app_link = GetJsonText(json_app, "link");
                                    model.app_type_name = GetJsonText(json_app, "title");
                                    model.app_name = GetJsonText(json_app, "content");
                                    model.app_type_img = GetJsonText(json_app, "img");
                                    model.app_table_name = "";
                                }
                            }
                            break;
                    } 
                    #endregion

                    model.multi_html = "";
                    model.content_html="";
                    model.img_list = "";

                    #region 获取编辑器里的图片

                    Dictionary<int, List<string>> content_imgs = new Dictionary<int, List<string>>();
                    List<string> del_list = new List<string>();
                    string[] imgs = Utils.GetHtmlImageSrcList(model.contents);
                    for (int i = 0; i < imgs.Length; i++)
                    {
                        if (imgs[i].ToLower().Contains("http") == false && imgs[i].ToLower().Contains("https") == false && imgs[i].ToLower().Contains("file:") == false)
                        {
                            if (imgs[i].ToLower().Contains("/upload/material/images/") == false && File.Exists(Common.Utils.GetMapPath(imgs[i].ToLower())))
                            {
                                string content_img_path = "/upload/material/images/" + DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.Ticks + i + ".jpg";
                                model.contents = model.contents.Replace(imgs[i], content_img_path);
                                content_imgs.Add(i, new List<string> { imgs[i], content_img_path });
                                model.img_list += content_img_path + ",";
                            }

                            if (hf_content_img.Value.Trim().Length > 0)
                            {
                                hf_content_img.Value = hf_content_img.Value.Replace(imgs[i], "");
                            }
                        }
                    } 
                    #endregion

                    if (id > 0)
                    {
                        bool is_exists = false;
                        if (hftitle.Value != model.title)
                        {
                            is_exists = KDWechat.BLL.Chats.wx_news_materials.Exists(model.title, (int)ResponseNewsType.单图文, wx_id);
                        }
                        if (!is_exists)
                        {
                            #region 获取封面图
                            bool is_new_file = false;
                            string new_img_path = hf_old_img.Value;
                            if (model.cover_img.ToLower().Contains("http") == false && model.cover_img.ToLower().Contains("https") == false && model.cover_img.ToLower().Contains("file:") == false)
                            {
                                
                                if (model.cover_img.ToLower().Contains("/upload/material/images/") == false && File.Exists(Common.Utils.GetMapPath(model.cover_img)))
                                {
                                    is_new_file = true;
                                    new_img_path = "/upload/material/images/" + DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.Ticks + ".jpg";
                                    model.cover_img = new_img_path;
                                }
                            }
                            #endregion

                            model = KDWechat.BLL.Chats.wx_news_materials.Update( model);
                            if (model != null)
                            {
                                #region copy封面图
                                if (is_new_file)
                                {
                                    //检查上传的物理路径是否存在，不存在则创建
                                    if (!Directory.Exists(Common.Utils.GetMapPath("/upload/material/images/" + DateTime.Now.ToString("yyyyMM") + "/")))
                                    {
                                        Directory.CreateDirectory(Common.Utils.GetMapPath("/upload/material/images/" + DateTime.Now.ToString("yyyyMM") + "/"));
                                    }
                                    if (File.Exists(Common.Utils.GetMapPath(img)) && new_img_path.ToLower().Contains("http")==false)
                                    {
                                        File.Copy(Common.Utils.GetMapPath(img), Common.Utils.GetMapPath(new_img_path), true);

                                    }
                                    //try
                                    //{
                                    //    File.Delete(Common.Utils.GetMapPath(hf_old_img.Value.Trim()));
                                    //}
                                    //catch (Exception)
                                    //{
                                    //}

                                } 
                                #endregion

                                #region copy编辑器图片
                                foreach (int k in content_imgs.Keys)
                                {
                                    try
                                    {
                                        string old = content_imgs[k][0];
                                        string _new = content_imgs[k][1];
                                        if (File.Exists(Common.Utils.GetMapPath(old)) && _new.ToLower().Contains("http") == false)
                                        {
                                            File.Copy(Common.Utils.GetMapPath(old), Common.Utils.GetMapPath(_new), true);
                                        }
                                    }
                                    catch (Exception)
                                    {
                                    }
                                } 
                                #endregion

                                #region 删除多余的编辑器图片

                                //string[] del = hf_content_img.Value.TrimEnd(',').TrimStart(',').Split(new char[] { ',' });
                                //for (int i = 0; i < del.Length; i++)
                                //{
                                //    try
                                //    {
                                //        if (File.Exists(Common.Utils.GetMapPath(del[i])))
                                //        {
                                //            File.Delete(Common.Utils.GetMapPath(del[i]));

                                //        }
                                //    }
                                //    catch (Exception)
                                //    {
                                //    }
                                //} 
                                #endregion
                                #region 生成二维码
                                CreateNewsQrCode(model.id);
                                #endregion
                                AddLog("修改单图文：" + model.title, LogType.修改);
                                JsHelper.AlertAndRedirect("保存成功", hfReturlUrl.Value);
                            }
                            else
                            {
                                JsHelper.Alert("保存失败，该图文消息不存在");
                            }
                        }
                        else
                        {
                            JsHelper.AlertAndRedirect("图文标题已存在", hfReturlUrl.Value, "fail");
                        }
                    }
                    else
                    {
                        if (!KDWechat.BLL.Chats.wx_news_materials.Exists(model.title, (int)ResponseNewsType.单图文, wx_id))
                        {

                            string new_img_path = "/upload/material/images/" + DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.Ticks + ".jpg";
                            bool is_new_file = false;
                            if (model.cover_img.ToLower().Contains("http") == false && model.cover_img.ToLower().Contains("https") == false && model.cover_img.ToLower().Contains("file:") == false)
                            {
                                is_new_file = true;
                                model.cover_img = new_img_path;
                            }
                           

                            model = KDWechat.BLL.Chats.wx_news_materials.Add(model);
                            if (model != null)
                            {
                                #region copy封面图
                                if (is_new_file)
                                {
                                    //检查上传的物理路径是否存在，不存在则创建
                                    if (!Directory.Exists(Common.Utils.GetMapPath("/upload/material/images/" + DateTime.Now.ToString("yyyyMM") + "/")))
                                    {
                                        Directory.CreateDirectory(Common.Utils.GetMapPath("/upload/material/images/" + DateTime.Now.ToString("yyyyMM") + "/"));
                                    }
                                    if (File.Exists(Common.Utils.GetMapPath(img)) && new_img_path.ToLower().Contains("http")==false)
                                    {
                                        File.Copy(Common.Utils.GetMapPath(img), Common.Utils.GetMapPath(new_img_path), true);
                                    } 
                                }
                               
                                #endregion

                                #region copy编辑器图片
                                foreach (int k in content_imgs.Keys)
                                {
                                    try
                                    {
                                        string old = content_imgs[k][0];
                                        string _new = content_imgs[k][1];
                                        if (File.Exists(Common.Utils.GetMapPath(old)) && _new.ToLower().Contains("http") == false)
                                        {
                                            File.Copy(Common.Utils.GetMapPath(old), Common.Utils.GetMapPath(_new), true);
                                        }
                                    }
                                    catch (Exception)
                                    {
                                    }
                                } 
                                #endregion

                                #region 生成二维码
                                CreateNewsQrCode(model.id);
                                #endregion

                                AddLog("添加单图文：" + model.title, LogType.添加);
                                JsHelper.AlertAndRedirect("保存成功", hfReturlUrl.Value);
                            }
                            else
                            {
                                JsHelper.Alert("保存失败");
                            }
                        }
                        else
                        {
                            JsHelper.AlertAndRedirect("图文标题已存在", hfReturlUrl.Value, "fail");
                        }
                    }
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(hfReturlUrl.Value);
        }
    }
}