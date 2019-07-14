using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;
using KDWechat.Common.Config;
using KDWechat.DAL;

namespace KDWechat.Web.wxpage.projects
{
    public partial class detail : KDWechat.Web.UI.TemplateHelp
    {
        protected void Page_Load(object sender, EventArgs e)
        {



            DAL.t_projects model = BLL.Chats.projects.GetModel(proj, 1);
            if (model != null)
            {
                //返回模板路劲、模版列表字段以及模版id
                string path = "";
                DAL.t_wx_templates template = template = BLL.Chats.wx_templates.GetModel((int)model.template_id);
                if (template != null)
                {

                    path = template.file_path;
                }
                if (path != "")
                {
                    string _html = ReadFile(Server.MapPath(path), System.Text.Encoding.UTF8);
                    List<DAL.t_project_contents> list_contents = null;
                    _html = _html.Replace("$page_title$", model.title);
                    list_contents = BLL.Chats.projects.GetList(proj);
                    string remark = "";
                    #region 简洁模板
                    if (template.title == ProjectDetaileTemplate.项目简洁模版.ToString() || template.title == ProjectDetaileTemplate.项目滑动模板.ToString())
                    {
                        string img = model.img_url;
                        if (!string.IsNullOrEmpty(img))
                        {
                            string[] l = img.Split(new char[] { '|' });
                            img = l[0];

                        }
                        _html = _html.Replace("$project_img$", img);



                        if (list_contents != null)
                        {
                            #region 项目介绍
                            t_project_contents m = list_contents.Where(x => x.channel_id == (int)ProjectContentType.项目介绍).FirstOrDefault();
                            if (m != null)
                            {
                                if (m.status == 0)
                                {
                                    _html = _html.Replace("$xmjs_class$", "style='display:none'");
                                }
                                else
                                {

                                    _html = _html.Replace("$jiaotong_class$", "");
                                }
                                if (string.IsNullOrEmpty(m.contents))
                                {
                                    _html = _html.Replace("$project_contents$", "");
                                }
                                else
                                {
                                    _html = _html.Replace("$project_contents$", m.contents.Replace("\r\n", "<br>"));
                                }

                                _html = _html.Replace("$xmjs_display_name$", m.display_name);
                                remark = Common.Utils.DropHTML(m.contents, 60);
                            }
                            else
                            {

                                _html = _html.Replace("$jiaotong_class$", "");
                                _html = _html.Replace("$project_contents$", "");
                                _html = _html.Replace("$xmjs_display_name$", "项目介绍");
                            }

                            #endregion

                            #region 交通配套
                            m = list_contents.Where(x => x.channel_id == (int)ProjectContentType.交通配套).FirstOrDefault();
                            if (m != null)
                            {

                                if (m.status == 0)
                                {
                                    _html = _html.Replace("$jiaotong_class$", "style='display:none'");

                                }
                                else
                                {
                                    _html = _html.Replace("$jiaotong_class$", "");
                                }
                                _html = _html.Replace("$jtpt_display_name$", m.display_name);
                                _html = _html.Replace("$project_jiaotong$", m.contents);
                            }
                            else
                            {
                                _html = _html.Replace("$jtpt_display_name$", "交通配套");
                                _html = _html.Replace("$jiaotong_class$", "style='display:none'");
                                _html = _html.Replace("$project_jiaotong$", "");
                            }
                            #endregion

                            #region 地理位置
                            m = list_contents.Where(x => x.channel_id == (int)ProjectContentType.地理位置).FirstOrDefault();
                            if (m != null)
                            {
                                if (m.status == 0)
                                {
                                    _html = _html.Replace("$map_class$", "style='display:none'");
                                    _html = _html.Replace("$project_address$", "");
                                    _html = _html.Replace("$lng$", "");
                                    _html = _html.Replace("$lat$", "");
                                }
                                else
                                {
                                    _html = _html.Replace("$map_class$", "");
                                    _html = _html.Replace("$project_address$", m.address);
                                    _html = _html.Replace("$lng$", m.lng);
                                    _html = _html.Replace("$lat$", m.lat);
                                }
                                _html = _html.Replace("$map_display_name$", m.display_name);

                            }
                            else
                            {
                                _html = _html.Replace("$map_display_name$", "地理位置");
                                _html = _html.Replace("$map_class$", "style='display:none'");
                                _html = _html.Replace("$project_address$", "");
                                _html = _html.Replace("$lng$", "");
                                _html = _html.Replace("$lat$", "");
                            }
                            #endregion

                            #region 项目图片
                            m = list_contents.Where(x => x.channel_id == (int)ProjectContentType.项目图片).FirstOrDefault();
                            if (m != null)
                            {
                                if (m.status == 0)
                                {
                                    _html = _html.Replace("$img_class$", "style='display:none'");
                                }
                                else
                                {
                                    _html = _html.Replace("$img_class$", "");
                                    #region 取出图片分类

                                    List<string> type_list = BLL.Chats.projects.GetProjectImageType(proj);
                                    string types = "";
                                    if (type_list != null)
                                    {
                                        int i = 0;
                                        foreach (string item in type_list)
                                        {
                                            string class_name = "";
                                            if (tname != "" && tname == item)
                                            {
                                                class_name = "class='on'";
                                            }
                                            else if (tname == "" && i == 0)
                                            {
                                                class_name = "class='on'";
                                            }

                                            types += " <a " + class_name + " href=\"detail.aspx?proj=" + proj + "&tname=" + item + "&openId=" + openId + "&wx_og_id=" + wx_og_id + "&wx_id=" + wx_id + "#img\">" + item + "</a>";
                                            i++;
                                        }
                                    }
                                    _html = _html.Replace("$ProjectImageType$", types);
                                    #endregion
                                }
                                _html = _html.Replace("$xmtp_display_name$", m.display_name);
                            }
                            else
                            {
                                _html = _html.Replace("$xmtp_display_name$", "项目图片");
                                _html = _html.Replace("$img_class$", "style='display:none'");
                                _html = _html.Replace("$ProjectImageType$", "");
                            }
                            #endregion


                            #region 微官网
                            m = list_contents.Where(x => x.channel_id == (int)ProjectContentType.微官网).FirstOrDefault();
                            if (m != null)
                            {

                                if (m.status == 0)
                                {
                                    _html = _html.Replace("$website_class$", "style='display:none'");
                                }
                                else
                                {
                                    _html = _html.Replace("$website_class$", "");
                                }
                                _html = _html.Replace("$website_display_name$", m.display_name);
                                _html = _html.Replace("$website_linnk$", m.link_url);
                            }
                            else
                            {
                                _html = _html.Replace("$website_display_name$", "微官网");
                                _html = _html.Replace("$website_class$", "style='display:none'");
                                _html = _html.Replace("$website_linnk$", "#");
                            }
                            #endregion

                            #region 联系热线
                            m = list_contents.Where(x => x.channel_id == (int)ProjectContentType.联系热线).FirstOrDefault();
                            if (m != null)
                            {

                                if (m.status == 0)
                                {
                                    _html = _html.Replace("$tel_class$", "style='display:none'");
                                }
                                else
                                {
                                    _html = _html.Replace("$tel_class$", "");
                                }
                                _html = _html.Replace("$tel_display_name$", m.display_name);
                                _html = _html.Replace("$phone$", m.phone);
                            }
                            else
                            {
                                _html = _html.Replace("$tel_display_name$", "联系热线");
                                _html = _html.Replace("$tel_class$", "style='display:none'");
                                _html = _html.Replace("$phone$", "");
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        t_project_contents m = list_contents.Where(x => x.channel_id == (int)ProjectContentType.项目介绍).FirstOrDefault();
                        if (m != null)
                        {
                            remark = Common.Utils.DropHTML(m.contents, 60);
                        }
                    }

                    #endregion
                    _html = _html.Replace("$remark$", remark);
                    //解析模板
                    Response.Write(StartParsing(_html, list_contents));

                }
                else
                {
                    Response.Write("找不到模板");
                }


            }
            else
            {
                Response.Write("项目不存在或已关闭");
            }


        }
    }
}