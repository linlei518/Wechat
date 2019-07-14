using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;
using KDWechat.DAL;
using LitJson;

namespace KDWechat.Web.material
{
    public partial class multi_news_add : KDWechat.Web.UI.BasePage
    {
        protected string obj = string.Empty;
        protected string groupStr = string.Empty;

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

                if (isnotop)
                {
                    this.TopControl1.Visible = false;
                    this.MenuList1.Visible = false;
                }
                if (is_public == 0) { CheckWXid(); }
                WriteReturnPage(hfReturlUrl, "multi-news_list.aspx?m_id=" + m_id + "&is_pub=" + is_pub);
                bindGroup();
                if (id > 0)
                {
                    CheckUserAuthority((is_public == 1 ? "material_multi_news_public" : "material_multi_news"), RoleActionType.Edit, hfReturlUrl.Value);
                    bindDisplay(); 
                    
                }
                else
                {
                    CheckUserAuthority((is_public == 1 ? "material_multi_news_public" : "material_multi_news"), RoleActionType.Add, hfReturlUrl.Value);
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
                    groupStr += "<option value='" + m.id + "' " + (m.status == 0 ? " disabled=\"disabled\"" : "") + ">" + m.title + "</option>";
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
                    return;
                }
                hf_old_img1.Value = model.cover_img;
                obj += "[{";
                obj += "id: '" + model.id + "',";
                obj += "title: '" + model.title.Trim() + "',";
                obj += "type: '" + model.group_id + "',";
                obj += "img: '" + model.cover_img.Trim() + "',";
                obj += "summary: '" + model.summary.Replace("\r", "").Replace("\n", "").Replace("'", "’").Trim() + "',";
                obj += "intro: '" + model.author.Trim() + "',";
                if (string.IsNullOrEmpty(model.push_type))
                {
                    model.push_type = "article";
                }
                obj += "pushType:'" + model.push_type.Trim() + "',";
                switch (model.push_type)
                {
                    case "article":
                        string template_name = "";

                        if (model.template_id > 0)
                        {
                            DAL.t_wx_templates template = BLL.Chats.wx_templates.GetModel(model.template_id);
                            if (template != null)
                            {
                                template_name = ",template:{id:'" + model.template_id + "',title:'" + template.title.Trim() + "',img:'" + template.img_url.Trim() + "'}";

                            }
                        }
                        obj += "article:{content:'" + model.contents.Replace("\r", "").Replace("\n", "").Replace("'", "’").Trim() + "',origin:'" + model.source_url.Trim() + "'" + template_name.Trim().Trim() + "},";
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


                obj += "}";

                string[] imgs = Utils.GetHtmlImageSrcList(model.contents);
                if (imgs.Length > 0)
                {
                    hf_content_img.Value += string.Join(",", imgs);
                }

                //取出子级列表
                List<KDWechat.DAL.t_wx_news_materials> list = KDWechat.BLL.Chats.wx_news_materials.GetChildList(id);
                if (list != null)
                {
                    obj += ",";
                    int i = 1;
                    foreach (KDWechat.DAL.t_wx_news_materials m in list)
                    {
                        if (i>9)
                        {
                            break;
                        }
                        hf_old_img2.Value += m.cover_img + ",";

                        obj += "{";
                        obj += "id: '" + m.id + "',";
                        obj += "title: '" + m.title.Trim() + "',";
                        obj += "type: '" + m.group_id + "',";
                        obj += "img: '" + m.cover_img + "',";
                        obj += "summary: '" + m.summary.Replace("\r", "").Replace("\n", "").Replace("'", "’").Trim() + "',";
                        obj += "intro: '" + m.author.Trim() + "',";
                        obj += "pushType:'" + m.push_type.Trim() + "',";
                        switch (m.push_type)
                        {
                            case "article":
                                string template_name = "";

                                if (m.template_id > 0)
                                {
                                    DAL.t_wx_templates template = BLL.Chats.wx_templates.GetModel(m.template_id);
                                    if (template != null)
                                    {
                                        template_name = ",template:{id:'" + m.template_id + "',title:'" + template.title.Trim() + "',img:'" + template.img_url.Trim() + "'}";

                                    }
                                }
                                obj += "article:{content:'" + m.contents.Replace("\r", "").Replace("\n", "").Replace("'", "’") + "',origin:'" + m.source_url.Trim() + "'" + template_name + "},";
                                obj += "link:{content:''}";
                                break;
                            case "link":
                                obj += "article:{content:'',origin:''},";
                                obj += "link:{content:'" + m.link_url.Trim() + "'}";
                                break;
                            case "app":
                                obj += "app:{id:'" + m.app_id + "',title:'" + m.app_type_name.Trim() + "',content:'" + m.app_name.Trim() + "',img: '" + m.app_type_img.Trim() + "',link:'" + m.app_link.Trim() + "'}";
                                break;
                        }

                        obj += "}";
                        if (i < list.Count)
                        {
                            obj += ",";
                        }
                        i++;

                        string[] imgs2 = Utils.GetHtmlImageSrcList(m.contents);
                        if (imgs2.Length > 0)
                        {
                            hf_content_img.Value += string.Join(",", imgs2);
                        }
                    }
                }
                obj += "]";
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

            try
            {
                string json_list = hfResult.Value;
                if (!string.IsNullOrEmpty(json_list))
                {
                    json_list = json_list.TrimEnd('~');
                    string[] list = json_list.Split(new string[] { "~!@#$" }, StringSplitOptions.RemoveEmptyEntries);
                    KDWechat.DAL.t_wx_news_materials model = new KDWechat.DAL.t_wx_news_materials();
                    model.multi_html ="";
                    model.content_html="";
                    List<KDWechat.DAL.t_wx_news_materials> child = new List<DAL.t_wx_news_materials>();
                    int first = 1;
                    for (int i = 0; i < list.Length; i++)
                    {
                        if (list[i].Trim().Length > 0)
                        {
                            JsonData jsonData2 = JsonMapper.ToObject(list[i].Trim().TrimEnd(',').TrimStart(',') + "}}");
                            if (jsonData2 != null)
                            {
                                if (first == 1)
                                {
                                    #region 取出父级图文

                                    model.title = GetJsonText(jsonData2, "title");
                                    model.wx_id = (is_public == 1 ? 0 : wx_id);
                                    model.wx_og_id = (is_public == 1 ? "" : wx_og_id);
                                    model.author = GetJsonText(jsonData2, "intro");
                                    model.channel_id = (int)ResponseNewsType.多图文;

                                    model.cover_img = GetJsonText(jsonData2, "img");
                                    model.create_time = DateTime.Now;
                                    model.group_id = Common.Utils.StrToInt(GetJsonText(jsonData2, "type"), 0);
                                    model.is_public = is_public;
                                    model.link_url = "";
                                    model.par_id = 0;
                                    model.source_url = "";
                                    model.status = 1;
                                    model.summary = Common.Utils.UnicodeToStr(GetJsonText(jsonData2, "summary")).Replace("\r", "").Replace("\n", "").Replace("'", "’");
                                    model.template_id = 0;
                                    model.u_id = u_id;
                                    model.id = id;

                                    model.contents = "";
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

                                    #endregion

                                    //model.content_html += " <div class=\"mainInfo\"><div class=\"img\"> <span> <img src=\""+model.cover_img+"\" alt=\"\"> </span> </div><div class=\"title\"><h1><a href=\"multi-news_add.aspx?m_id="+m_id+"&id=$id$&is_pub="+is_pub+"\">"+model.title+"</a></h1> </div> </div>";
                                    //if (id>0)
                                    //{
                                    //    model.content_html = model.content_html.Replace("$id$", id.ToString());
                                    //}
                                }
                                else
                                {

                                    #region 添加子级图文
                                    KDWechat.DAL.t_wx_news_materials m = new KDWechat.DAL.t_wx_news_materials()
                                                           {
                                                               title = GetJsonText(jsonData2, "title"),
                                                               wx_id = (is_public == 1 ? 0 : wx_id),
                                                               wx_og_id = (is_public == 1 ? "" : wx_og_id),
                                                               author = GetJsonText(jsonData2, "intro"),
                                                               channel_id = (int)ResponseNewsType.多图文,
                                                               contents = "",
                                                               cover_img = GetJsonText(jsonData2, "img"),
                                                               create_time = DateTime.Now,
                                                               group_id = Common.Utils.StrToInt(GetJsonText(jsonData2, "type"), 0),
                                                               is_public = is_public,
                                                               link_url = "",
                                                               par_id = id,
                                                               source_url = "",
                                                               status = 1,
                                                               summary = Common.Utils.UnicodeToStr(GetJsonText(jsonData2, "summary")).Replace("\r", "").Replace("\n", "<br>").Replace("'", "’"),
                                                               template_id = 0,
                                                               u_id = u_id,
                                                               id = Common.Utils.StrToInt(GetJsonText(jsonData2, "id"), 0)
                                                           };
                                    #region 新增的字段

                                    m.push_type = GetJsonText(jsonData2, "pushType");

                                    string article = GetJsonText(jsonData2, "article");
                                    string link = GetJsonText(jsonData2, "link");
                                    switch (m.push_type)
                                    {
                                        case "article":
                                            if (!string.IsNullOrEmpty(article))
                                            {
                                                JsonData json_article = jsonData2["article"];// JsonMapper.ToObject(article);
                                                if (json_article != null)
                                                {
                                                    m.contents = GetJsonText(json_article, "content", false).Replace("\r", "").Replace("\n", "").Replace("'", "’");
                                                    m.source_url = GetJsonText(json_article, "origin");
                                                    string template = GetJsonText(json_article, "template");

                                                    if (!string.IsNullOrEmpty(template))
                                                    {
                                                        JsonData json_template = json_article["template"]; //JsonMapper.ToObject(template);
                                                        if (json_template != null)
                                                        {
                                                            m.template_id = Common.Utils.StrToInt(GetJsonText(json_template, "id"), 0);
                                                        }
                                                    }

                                                }
                                            }

                                            if (!string.IsNullOrEmpty(link))
                                            {
                                                JsonData json_link = jsonData2["link"];// JsonMapper.ToObject(link);
                                                if (json_link != null)
                                                {
                                                    m.link_url = GetJsonText(json_link, "content");
                                                }
                                            }
                                            break;
                                        case "link":

                                            if (!string.IsNullOrEmpty(article))
                                            {
                                                JsonData json_article = jsonData2["article"];// JsonMapper.ToObject(article);
                                                if (json_article != null)
                                                {
                                                    m.contents = GetJsonText(json_article, "content", false).Replace("\r", "").Replace("\n", "").Replace("'", "’");
                                                    m.source_url = GetJsonText(json_article, "origin");
                                                }
                                            }

                                            if (!string.IsNullOrEmpty(link))
                                            {
                                                JsonData json_link = jsonData2["link"]; //JsonMapper.ToObject(link);
                                                if (json_link != null)
                                                {
                                                    m.link_url = GetJsonText(json_link, "content");
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
                                                    m.app_id = Common.Utils.StrToInt(GetJsonText(json_app, "id"), 0);
                                                    m.app_link = GetJsonText(json_app, "link");
                                                    m.app_type_name = GetJsonText(json_app, "title");
                                                    m.app_name = GetJsonText(json_app, "content");
                                                    m.app_type_img = GetJsonText(json_app, "img");
                                                    m.app_table_name = "";
                                                }
                                            }
                                            break;
                                    }
                                    #endregion

                                    child.Add(m);
                                    #endregion
                                    if (first > 1 && first < 4)
                                    {
                                        model.content_html += " <div class=\"info\">";
                                        model.content_html += "<div class=\"img\"> <span><img src=\"" + m.cover_img + "\" alt=\"\">  </span></div>";
                                        model.content_html += "<div class=\"title\"> <h1>" + m.title + "</h1> </div> </div>";

                                        model.multi_html += "<div class=\"infoField\"><div class=\"img\"> <span><img class=\"cover\" src=\"" + m.cover_img + "\" > </span> </div><div class=\"title\"><h1>" + m.title + "</h1></div> </div>";
                                    }

                                }
                                first++;

                            }
                        }
                    }

                    Dictionary<string, List<string>> content_imgs = new Dictionary<string, List<string>>();
                    model.img_list = "";
                    if (id > 0)
                    {

                        #region 获取需要复制的图片，第一条图文
                        Dictionary<int, List<string>> imgs = new Dictionary<int, List<string>>();
                        int j = 2;
                        if (model.cover_img.ToLower().Contains("http") == false && model.cover_img.ToLower().Contains("https") == false && model.cover_img.ToLower().Contains("file:") == false)
                        {
                            string new_img_path1 = "/upload/material/images/" + DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.Ticks + ".jpg";
                            string og_img1 = model.cover_img;
                            if (model.cover_img.ToLower().Contains("/upload/material/images/") == false && File.Exists(Common.Utils.GetMapPath(model.cover_img)))
                            {
                                model.cover_img = new_img_path1;
                                imgs.Add(1, new List<string> { og_img1, new_img_path1, hf_old_img1.Value });

                            }
                            model.img_list += model.cover_img + ",";
                        }

                        string[] imgs_1 = Utils.GetHtmlImageSrcList(model.contents);
                        for (int i = 0; i < imgs_1.Length; i++)
                        {
                            if (imgs_1[i].ToLower().Contains("http") == false && imgs_1[i].ToLower().Contains("https") == false && imgs_1[i].ToLower().Contains("file:") == false)
                            {
                                if (imgs_1[i].ToLower().Contains("/upload/material/images/") == false && File.Exists(Common.Utils.GetMapPath(imgs_1[i].ToLower())))
                                {
                                    string content_img_path = "/upload/material/images/" + DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.Ticks + i + "1_1_1_1" + ".jpg";
                                    model.contents = model.contents.Replace(imgs_1[i], content_img_path);
                                    content_imgs.Add(i + "_1_1_1_1", new List<string> { imgs_1[i], content_img_path });
                                    model.img_list += content_img_path + ",";
                                }
                                else
                                {
                                    model.img_list += imgs_1[i] + ",";

                                }

                                if (hf_content_img.Value.Trim().Length > 0)
                                {
                                    hf_content_img.Value = hf_content_img.Value.Replace(imgs_1[i], "");
                                }
                            }
                        }



                        foreach (t_wx_news_materials item in child)
                        {
                            #region 封面
                            if (item.cover_img.ToLower().Contains("http") == false && item.cover_img.ToLower().Contains("https") == false && item.cover_img.ToLower().Contains("file:") == false)
                            {
                                string new_img_path = "/upload/material/images/" + DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.Ticks + j + ".jpg";

                                if (item.cover_img.ToLower().Contains("/upload/material/images/") == false && File.Exists(Common.Utils.GetMapPath(item.cover_img)))
                                {
                                    imgs.Add(j, new List<string> { item.cover_img, new_img_path });
                                    item.cover_img = new_img_path;
                                }

                                if (hf_old_img2.Value.Trim().Length > 0)
                                {
                                    hf_old_img2.Value = hf_old_img2.Value.Replace(item.cover_img, "");
                                }
                                model.img_list += item.cover_img + ",";
                            }
                            #endregion

                            #region 编辑器
                            string[] c_imgs = Utils.GetHtmlImageSrcList(item.contents);
                            for (int i = 0; i < c_imgs.Length; i++)
                            {
                                if (c_imgs[i].ToLower().Contains("http") == false && c_imgs[i].ToLower().Contains("https") == false && c_imgs[i].ToLower().Contains("file:") == false)
                                {

                                    if (c_imgs[i].ToLower().Contains("/upload/material/images/") == false && File.Exists(Common.Utils.GetMapPath(c_imgs[i].ToLower())))
                                    {
                                        string content_img_path = "/upload/material/images/" + DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.Ticks + i + j + ".jpg";
                                        item.contents = item.contents.Replace(c_imgs[i], content_img_path);
                                        content_imgs.Add(i + "_" + j, new List<string> { c_imgs[i], content_img_path });
                                        model.img_list += content_img_path + ",";
                                    }
                                    else
                                    {
                                        model.img_list += c_imgs[i] + ",";
                                    }
                                    if (hf_content_img.Value.Trim().Length > 0)
                                    {
                                        hf_content_img.Value = hf_content_img.Value.Replace(c_imgs[i], "");
                                    }
                                }


                            }


                            #endregion


                            #region buyao de
                            /* if (j == 2)
                        {
                            if (item.cover_img != hf_old_img2.Value)
                            {
                                imgs.Add(j, new List<string> { item.cover_img, new_img_path,hf_old_img2.Value });
                                item.cover_img = new_img_path;
                            }
                        }

                        if (j == 3)
                        {
                            if (item.cover_img != hf_old_img3.Value)
                            {
                                imgs.Add(j, new List<string> { item.cover_img, new_img_path,hf_old_img3.Value });
                                item.cover_img = new_img_path;
                            }
                        }

                        if (j == 4)
                        {
                            if (item.cover_img != hf_old_img4.Value)
                            {
                                imgs.Add(j, new List<string> { item.cover_img, new_img_path ,hf_old_img4.Value});
                                item.cover_img = new_img_path;
                            }
                        }

                        if (j == 5)
                        {
                            if (item.cover_img != hf_old_img5.Value)
                            {
                                imgs.Add(j, new List<string> { item.cover_img, new_img_path,hf_old_img5.Value });
                                item.cover_img = new_img_path;
                            }
                        }

                        if (j == 6)
                        {
                            if (item.cover_img != hf_old_img6.Value)
                            {
                                imgs.Add(j, new List<string> { item.cover_img, new_img_path,hf_old_img6.Value });
                                item.cover_img = new_img_path;
                            }
                        }

                        if (j == 7)
                        {
                            if (item.cover_img != hf_old_img7.Value)
                            {
                                imgs.Add(j, new List<string> { item.cover_img, new_img_path,hf_old_img7.Value });
                                item.cover_img = new_img_path;
                            }
                        }

                        if (j == 8)
                        {
                            if (item.cover_img != hf_old_img8.Value)
                            {
                                imgs.Add(j, new List<string> { item.cover_img, new_img_path ,hf_old_img8.Value});
                                item.cover_img = new_img_path;
                            }
                        }

                        if (j == 9)
                        {
                            if (item.cover_img != hf_old_img9.Value)
                            {
                                imgs.Add(j, new List<string> { item.cover_img, new_img_path ,hf_old_img9.Value});
                                item.cover_img = new_img_path;
                            }
                        }

                        if (j == 10)
                        {
                            if (item.cover_img != hf_old_img10.Value)
                            {
                                imgs.Add(j, new List<string> { item.cover_img, new_img_path,hf_old_img10.Value });
                                item.cover_img = new_img_path;
                            }
                        }
                        * */

                            #endregion


                            j++;
                        }

                        #endregion

                        bool is_result = KDWechat.BLL.Chats.wx_news_materials.UpdateMulti(model, child);
                        if (is_result)
                        {
                            #region 生成二维码
                            CreateNewsQrCode(model.id);
                            #endregion

                            #region 图片处理
                            try
                            {
                                #region copy封面图片

                                if (!Directory.Exists(Common.Utils.GetMapPath("/upload/material/images/" + DateTime.Now.ToString("yyyyMM") + "/")))
                                {
                                    Directory.CreateDirectory(Common.Utils.GetMapPath("/upload/material/images/" + DateTime.Now.ToString("yyyyMM") + "/"));
                                }

                                foreach (int key in imgs.Keys)
                                {
                                    string og_img = imgs[key][0];
                                    string new_img = imgs[key][1];
                                    if (File.Exists(Common.Utils.GetMapPath(og_img)) && new_img.ToLower().Contains("http")==false)
                                    {
                                        File.Copy(Common.Utils.GetMapPath(og_img), Common.Utils.GetMapPath(new_img), true);
                                    }
                                    //if (key == 1)
                                    //{
                                    //    try
                                    //    {
                                    //        if (File.Exists(Common.Utils.GetMapPath(imgs[key][2])))
                                    //        {
                                    //            File.Delete(Common.Utils.GetMapPath(imgs[key][2]));
                                    //        }
                                    //    }
                                    //    catch (Exception)
                                    //    {
                                    //    }
                                    //}

                                }


                                //string[] del = hf_old_img2.Value.TrimEnd(',').TrimStart(',').Split(new char[] { ',' });
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

                                #region copy编辑器图片
                                foreach (string k in content_imgs.Keys)
                                {
                                    string old = content_imgs[k][0];
                                    string _new = content_imgs[k][1];
                                    try
                                    {
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

                                //string[] del_list = hf_content_img.Value.TrimEnd(',').TrimStart(',').Split(new char[] { ',' });
                                //for (int i = 0; i < del_list.Length; i++)
                                //{
                                //    try
                                //    {
                                //        if (File.Exists(Common.Utils.GetMapPath(del_list[i])))
                                //        {
                                //            File.Delete(Common.Utils.GetMapPath(del_list[i]));

                                //        }
                                //    }
                                //    catch (Exception)
                                //    {
                                //    }
                                //}
                                #endregion
                            }
                            catch (Exception)
                            {
                            } 
                            #endregion

                            AddLog("修改多图文：" + model.title, LogType.修改);
                            JsHelper.AlertAndRedirect("保存成功", hfReturlUrl.Value);
                        }
                        else
                        {
                            JsHelper.Alert(Page, "保存失败", "true");
                        }
                    }
                    else
                    {


                        #region 获取需要复制的封面图片和编辑器图片
                        string new_img_path1 = "/upload/material/images/" + DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.Ticks + ".jpg";
                        string og_img1 = model.cover_img;
                        Dictionary<int, List<string>> imgs = new Dictionary<int, List<string>>();
                        if (model.cover_img.ToLower().Contains("http") == false && model.cover_img.ToLower().Contains("https") == false && model.cover_img.ToLower().Contains("file:") == false)
                        {

                            model.cover_img = new_img_path1;
                            imgs.Add(1, new List<string> { og_img1, new_img_path1 });
                            model.img_list += model.cover_img + ",";
                        }



                        int j = 2;

                        foreach (t_wx_news_materials item in child)
                        {
                            if (item.cover_img.ToLower().Contains("http") == false && item.cover_img.ToLower().Contains("https") == false && item.cover_img.ToLower().Contains("file:") == false)
                            {
                                string new_img_path = "/upload/material/images/" + DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.Ticks + j + ".jpg";
                                imgs.Add(j, new List<string> { item.cover_img, new_img_path });
                                item.cover_img = new_img_path;
                                model.img_list += item.cover_img + ",";
                            }




                            string[] c_imgs = Utils.GetHtmlImageSrcList(item.contents);
                            for (int i = 0; i < c_imgs.Length; i++)
                            {
                                if (c_imgs[i].ToLower().Contains("http") == false && c_imgs[i].ToLower().Contains("https") == false && c_imgs[i].ToLower().Contains("file:") == false)
                                {
                                    if (c_imgs[i].ToLower().Contains("/upload/material/images/") == false && File.Exists(Common.Utils.GetMapPath(c_imgs[i].ToLower())))
                                    {
                                        string content_img_path = "/upload/material/images/" + DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.Ticks + i + j + ".jpg";
                                        item.contents = item.contents.Replace(c_imgs[i], content_img_path);
                                        content_imgs.Add(i + "_" + j, new List<string> { c_imgs[i], content_img_path });
                                        model.img_list += content_img_path + ",";
                                    }
                                    else
                                    {
                                        model.img_list += c_imgs[i] + ",";
                                    }
                                }

                            }


                            j++;
                        }
                        #endregion

                        int num = KDWechat.BLL.Chats.wx_news_materials.AddMulti(model, child);
                        if (num > 0)
                        {
                            #region 生成二维码
                            CreateNewsQrCode(num);
                            #endregion

                            #region 图片处理
                            try
                            {

                                #region copy封面图片
                                if (!Directory.Exists(Common.Utils.GetMapPath("/upload/material/images/" + DateTime.Now.ToString("yyyyMM") + "/")))
                                {
                                    Directory.CreateDirectory(Common.Utils.GetMapPath("/upload/material/images/" + DateTime.Now.ToString("yyyyMM") + "/"));
                                }

                                foreach (int key in imgs.Keys)
                                {
                                    try
                                    {
                                        string og_img = imgs[key][0];
                                        string new_img = imgs[key][1];
                                        if (File.Exists(Common.Utils.GetMapPath(og_img)) && new_img.ToLower().Contains("http")==false)
                                        {
                                            File.Copy(Common.Utils.GetMapPath(og_img), Common.Utils.GetMapPath(new_img), true);
                                        }
                                    }
                                    catch (Exception)
                                    {
                                    }

                                }

                                #endregion

                                #region copy编辑器图片
                                foreach (string k in content_imgs.Keys)
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
                            }
                            catch (Exception)
                            {
                            } 
                            #endregion

                            AddLog("添加多图文：" + model.title, LogType.添加);
                            JsHelper.AlertAndRedirect("保存成功", hfReturlUrl.Value);
                        }
                        else
                        {
                            JsHelper.Alert(Page, "保存失败", "true");
                        }
                    }

                }
                else
                {
                    JsHelper.AlertAndRedirect("保存失败,未获取到数据", hfReturlUrl.Value, "fail");
                }
            }
            catch (Exception ex)
            {
                InsertErrorLog(ex);
                JsHelper.AlertAndRedirect("保存失败,请稍后再试", hfReturlUrl.Value,"fail");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(hfReturlUrl.Value);
        }

        /// <summary>
        /// 插入错误日志
        /// </summary>
        /// <param name="ex"></param>
        private void InsertErrorLog(Exception ex)
        {
            StringBuilder str = new StringBuilder();
            DateTime date = DateTime.Now;
            str.Append("\r\n" + date.ToString("yyyy.MM.dd HH:mm:ss"));
            str.Append("\r\n.客户信息：");

            string ip = "";
            if (Request.ServerVariables.Get("HTTP_X_FORWARDED_FOR") != null)
            {
                ip = Request.ServerVariables.Get("HTTP_X_FORWARDED_FOR").ToString().Trim();
            }
            else
            {
                ip = Request.ServerVariables.Get("Remote_Addr").ToString().Trim();
            }
            str.Append("\r\n\tIp:" + ip);
            str.Append("\r\n\t浏览器:" + Request.Browser.Browser.ToString());
            str.Append("\r\n\t浏览器版本:" + Request.Browser.MajorVersion.ToString());
            str.Append("\r\n\t操作系统:" + Request.Browser.Platform.ToString());
            str.Append("\r\n.错误信息：");
            str.Append("\r\n\t页面：" + Request.Url.ToString());
            str.Append("\r\n\t错误信息：" + ex.Message);
            str.Append("\r\n\t错误源：" + ex.Source);
            str.Append("\r\n\t异常方法：" + ex.TargetSite);
            str.Append("\r\n\t堆栈信息：" + ex.StackTrace);
            str.Append("\r\n--------------------------------------------------------------------------------------------------");
            //保存错误日志 
            KDWechat.DAL.t_wx_error_logs logs = new DAL.t_wx_error_logs();
            logs.user_id = 0;
            logs.login_name = "";
            logs.content = str.ToString();
            logs.add_time = date;
            BLL.Logs.wx_error_log.CreateWxErrorLog(logs);
        }
    }
}