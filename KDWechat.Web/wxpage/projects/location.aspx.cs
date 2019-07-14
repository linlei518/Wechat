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
    public partial class location : KDWechat.Web.UI.TemplateHelp
    {
        protected void Page_Load(object sender, EventArgs e)
        {



            DAL.t_projects model = BLL.Chats.projects.GetModel(proj, 1);
            if (model != null)
            {
                //返回模板路劲、模版列表字段以及模版id
                string path = "/templates/projects/item2/station.html";
                switch (type)
                {
                    case "item2":
                        path = "/templates/projects/item2/station.html";
                        break;
                }
                if (path != "")
                {
                    string _html = ReadFile(Server.MapPath(path), System.Text.Encoding.UTF8);
                    List<DAL.t_project_contents> list_contents = null;
                    _html = _html.Replace("$page_title$", model.title);

                    #region 高级模板
                    list_contents = BLL.Chats.projects.GetList(proj);
                    if (list_contents != null)
                    {
                        t_project_contents contents = list_contents.Where(x => x.channel_id == (int)ProjectContentType.地理位置).FirstOrDefault();
                        if (contents != null)
                        {
                            _html = _html.Replace("$address$", contents.address);
                            _html = _html.Replace("$traffic_line$", contents.traffic);

                            _html = _html.Replace("$lng$", contents.lng);
                            _html = _html.Replace("$lat$", contents.lat);
                        }
                    }
                    #endregion





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