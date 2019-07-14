using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;
using KDWechat.Common.Config;
using KDWechat.DAL;

namespace KDWechat.Web.wxpage
{
    public partial class news_list : System.Web.UI.Page
    {
        protected int id { get { return RequestHelper.GetQueryInt("id", 0); } }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //取出微信


                //取出图文
                DAL.t_wx_news_materials model = BLL.Chats.wx_news_materials.GetModel(id);
                if (model != null)
                {

                    StringBuilder list = new StringBuilder();
  
                    //取出内容
                    string _html = ReadFile(Server.MapPath("/templates/wechat/list_simulator/simulator.html"), System.Text.Encoding.UTF8);
                    

                    if (_html.Trim().Length > 0)
                    {
                           BLL.Config.wechat_config bll = new BLL.Config.wechat_config();
                        wechatconfig wchatConfig = bll.loadConfig();
                        if (model.channel_id == (int)ResponseNewsType.单图文)
                        {

                            list.Append("<div class=\"simulatorPanel_02\">");
                            list.Append("<a href=\"" + GetLink(model.id, model.push_type, model.link_url, model.app_link, wchatConfig, model.wx_og_id) + "\"><div class=\"title\"><h1>" + model.title + "</h1><h2>" + model.create_time.ToString("MM月dd日") + "</h2></div>");
                            list.Append("<div class=\"img\"><img src=\"/templates/wechat/list_simulator/img/blank.gif\" class=\"placeholder\" alt=\"\"><span style=\"background-image:url(" + (model.cover_img.ToLower().Contains("http") == true ? "" : wchatConfig.domain) + model.cover_img + ")\"></span></div>");
                            list.Append("<div class=\"text\"><p>"+model.summary+"</p></div><div class=\"links\"><p>阅读全文</p></div></a></div>");
                        }
                        else
                        {
                            list.Append("<div class=\"simulatorPanel_01\">");

                            //第一个
                            list.Append("<a href=\"" + GetLink(model.id, model.push_type, model.link_url, model.app_link, wchatConfig,model.wx_og_id) + "\"  class=\"main\"><div class=\"title\"><h1>" + model.title + "</h1></div>");
                            list.Append("<div class=\"img\"><img src=\"/templates/wechat/list_simulator/img/blank.gif\" class=\"placeholder\" alt=\"\"><span style=\"background-image:url(" + (model.cover_img.ToLower().Contains("http") == true ? "" : wchatConfig.domain) + model.cover_img + ")\"></span></div>");
                            list.Append("</a>");

                            //取出子级列表
                            List<KDWechat.DAL.t_wx_news_materials> list_child = KDWechat.BLL.Chats.wx_news_materials.GetChildList(id);
                            if (list_child!=null)
                            {
                                foreach (t_wx_news_materials news in list_child)
                                {
                                    list.Append("<a href=\"" + GetLink(news.id, news.push_type, news.link_url, news.app_link, wchatConfig,news.wx_og_id) + "\"  ><div class=\"title\"><h1>" + news.title + "</h1></div>");
                                    list.Append("<div class=\"img\"><span style=\"background-image:url(" + (model.cover_img.ToLower().Contains("http") == true ? "" : wchatConfig.domain) + news.cover_img + ")\"></span></div>");
                                    list.Append("</a>");
                                }
                            }

                           
                            list.Append("</div>");
                        }

                        _html = _html.Replace("$news_list$", list.ToString());
                        Response.Write(_html);
                    }


                }
                else
                {
                    Response.Write("找不到图文!");
                }

            }
        }

        public string GetLink(object id, object push_type, object link_url, object app_link, wechatconfig wchatConfig,string wx_og_id)
        {
            string url = "javascript:alert('请先在后台设置推送类型');";
            if (push_type!=null)
            {
                switch (push_type.ToString())
                {
                    case "article":
                        url = wchatConfig.domain + "/wxpage/news_template.aspx?id=" + id + "&wx_og_id=" + wx_og_id;
                        break;

                    case "link":
                        url = link_url.ToString().Trim().Replace("$openId$", "xxxxxxxx").Replace("$wx_og_id$", wx_og_id);
                        break;

                    case "app":
                        url = app_link.ToString().Trim().Replace("$openId$", "xxxxxxxx");
                        if (!url.ToLower().Contains("wx_og_id"))
                        {
                            if (url.Trim().Contains("?"))
                            {
                                url += "&wx_og_id=" + wx_og_id;
                            }
                            else
                            {
                                url += "?wx_og_id=" + wx_og_id;
                            }
                        }
                        if (!url.ToLower().Contains("openId"))
                        {

                            url += "&openId=xxxxxxxx";
                            
                        }
                        
                        break;
                }
            }
            

            return url;
        }

        /// <summary>
        /// 读文件
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <returns></returns>
        protected string ReadFile(string Path, Encoding code)
        {
            string s = "";
            if (!System.IO.File.Exists(Path))
                s = "不存在相应的目录:" + Path;
            else
            {
                StreamReader f2 = new StreamReader(Path, code);
                s = f2.ReadToEnd();
                f2.Close();
                f2.Dispose();
            }
            return s;
        }

    
    }
}