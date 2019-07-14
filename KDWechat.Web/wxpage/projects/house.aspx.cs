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
    public partial class house : KDWechat.Web.UI.TemplateHelp
    {
        protected void Page_Load(object sender, EventArgs e)
        {



            DAL.t_projects model = BLL.Chats.projects.GetModel(proj, 1);
            if (model != null)
            {
                //返回模板路劲、模版列表字段以及模版id
                string path = "/templates/projects/item2/house.html";
                switch (type)
                {
                    case "item2":
                        path = "/templates/projects/item2/house.html";
                        break;
                }
                if (path != "")
                {
                    string _html = ReadFile(Server.MapPath(path), System.Text.Encoding.UTF8);
                    List<DAL.t_project_contents> list_contents = null;
                    _html = _html.Replace("$page_title$", model.title);

                    #region 高级模板
                    list_contents = BLL.Chats.projects.GetList(proj);
                    DataTable dt_room_type = null;
                    if (list_contents != null)
                    {
                        int type_id = RequestHelper.GetQueryInt("type_id", 0);
                        if (type_id > 0)
                        {
                            dt_room_type = BLL.Module.md_view360type.GetListByIds(type_id.ToString(), (int)model.edit_wx_id);
                        }
                        else
                        {
                            string ids = "";
                            t_project_contents contents = list_contents.Where(x => x.channel_id == (int)ProjectContentType.户型图).FirstOrDefault();
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
                                dt_room_type = BLL.Module.md_view360type.GetListByIds(ids.TrimEnd(',').TrimStart(','), (int)model.edit_wx_id);
                            }
                        }

                    }
                    #endregion






                    //解析模板
                    Response.Write(StartParsing(_html, list_contents, dt_room_type));

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