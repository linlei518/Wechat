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
    public partial class piclist : KDWechat.Web.UI.TemplateHelp
    {
        protected void Page_Load(object sender, EventArgs e)
        {



            DAL.t_projects model = BLL.Chats.projects.GetModel(proj, 1);
            if (model != null)
            {
                //返回模板路劲、模版列表字段以及模版id
                string path = "/templates/projects/item2/piclist.html";
                switch (type)
                {
                    case "item2":
                        path = "/templates/projects/item2/piclist.html";
                        break;
                }
                if (path != "")
                {
                    string _html = ReadFile(Server.MapPath(path), System.Text.Encoding.UTF8);
                    List<DAL.t_project_contents> list_contents = null;
                    _html = _html.Replace("$page_title$", model.title);

                    #region 高级模板
                    list_contents = BLL.Chats.projects.GetList(proj);
                    List<string> pic_type_list = BLL.Chats.projects.GetProjectImageType(proj);



                    string currnt_name = tname;
                    string prev_name = "";
                    string next_name = "";

                    if (pic_type_list != null)
                    {
                        if (pic_type_list.Count > 0)
                        {
                            if (tname == "")
                            {
                                currnt_name = pic_type_list[0];
                                if (pic_type_list.Count > 1)
                                {
                                    next_name = pic_type_list[0];
                                }
                            }
                            else
                            {
                                for (int i = 0; i < pic_type_list.Count; i++)
                                {
                                    if (tname == pic_type_list[i])
                                    {
                                        if (i > 0)
                                        {
                                            prev_name = pic_type_list[i - 1];
                                        }
                                        if (i + 1 < pic_type_list.Count)
                                        {
                                            next_name = pic_type_list[i + 1];
                                        }
                                        break;
                                    }
                                }
                            }

                        }
                        _html = _html.Replace("$currnt_name$", currnt_name);
                        if (prev_name != "")
                        {
                            _html = _html.Replace("$prevUrl$", "piclist.aspx?proj=" + proj + "&tname=" + prev_name + "&page=1&openId=" + openId + "&wx_og_id=" + wx_og_id + "&wx_id=" + wx_id + "");
                        }
                        else
                        {
                            _html = _html.Replace("$prevUrl$", "");
                        }

                        if (next_name != "")
                        {
                            _html = _html.Replace("$nextUrl$", "piclist.aspx?proj=" + proj + "&tname=" + next_name + "&page=1&openId=" + openId + "&wx_og_id=" + wx_og_id + "&wx_id=" + wx_id + "");
                        }
                        else
                        {
                            _html = _html.Replace("$nextUrl$", "");
                        }

                        _html = _html.Replace("$pageUrl$", "pageitem.aspx?proj=" + proj + "&tname=" + currnt_name + "&page=1&openId=" + openId + "&wx_og_id=" + wx_og_id + "&wx_id=" + wx_id + "");
                    }
                    else
                    {
                        _html = _html.Replace("$nextUrl$", "");
                        _html = _html.Replace("$prevUrl$", "");
                        _html = _html.Replace("$pageUrl$", "");
                    }
                    #endregion




                    //解析模板
                    Response.Write(StartParsing(_html, list_contents, null, pic_type_list));

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