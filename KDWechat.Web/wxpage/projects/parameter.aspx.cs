using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.DAL;

namespace KDWechat.Web.wxpage.projects
{
    public partial class parameter : KDWechat.Web.UI.TemplateHelp
    {
        protected void Page_Load(object sender, EventArgs e)
        {



         model = BLL.Chats.projects.GetModel(proj, 1);
            if (model != null)
            {
                //返回模板路劲、模版列表字段以及模版id
                DAL.t_wx_templates template = template = BLL.Chats.wx_templates.GetModel((int)model.template_id);
                if (template != null)
                {
                    template_path = template.file_path;
                }
                if (template_path != "")
                {
                   
                    string _html = ReadFile(Server.MapPath(template_path + "parameter.html"), System.Text.Encoding.UTF8);
                    _html = _html.Replace("$page_title$", model.title);

                    list_contents = BLL.Chats.projects.GetList(proj);
                     
 

                    //解析模板
                    Response.Write(StartParsing(_html));

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