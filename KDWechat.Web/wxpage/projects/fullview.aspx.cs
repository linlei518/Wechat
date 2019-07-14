using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;
using KDWechat.Common.Config;
using KDWechat.DAL;
namespace KDWechat.Web.wxpage.projects
{
    public partial class fullview : KDWechat.Web.UI.TemplateHelp
    {
        protected void Page_Load(object sender, EventArgs e)
        {



            DAL.t_projects model = BLL.Chats.projects.GetModel(proj, 1);
            if (model != null)
            {
                //返回模板路劲、模版列表字段以及模版id
                string path = "/templates/projects/item2/fullview.html";
                switch (type)
                {
                    case "item2":
                        path = "/templates/projects/item2/fullview.html";
                        break;
                }
                if (path != "")
                {
                    string _html = ReadFile(Server.MapPath(path), System.Text.Encoding.UTF8);
                    List<DAL.t_project_contents> list_contents = null;
                    _html = _html.Replace("$page_title$", model.title);

                    #region 高级模板
                    list_contents = BLL.Chats.projects.GetList(proj);
                    DataTable dt_full_view = null;
                    if (list_contents != null)
                    {
                        string ids = "";
                        t_project_contents contents = list_contents.Where(x => x.channel_id == (int)ProjectContentType.项目全景).FirstOrDefault();
                        if (contents != null && contents.app_id != null)
                        {
                            string[] temp = contents.app_id.Split(new char[] { ',' });
                            foreach (string s in temp)
                            {
                                if (s.Trim() != "")
                                {
                                    ids += s + ",";
                                }
                            }
                        }
                        if (ids.Length > 0)
                        {
                            dt_full_view = BLL.Module.md_view360type.GetFullViewListByIds(ids.TrimEnd(',').TrimStart(','), (int)model.edit_wx_id);
                        }
                    }
                    #endregion




                    //解析模板
                    Response.Write(StartParsing(_html, list_contents, dt_full_view));

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