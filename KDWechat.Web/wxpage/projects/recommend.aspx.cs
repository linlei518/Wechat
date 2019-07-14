using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;

namespace KDWechat.Web.wxpage.projects
{
    public partial class recommend : KDWechat.Web.UI.TemplateHelp
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
                    string _html = ReadFile(Server.MapPath(template_path + "tuijian.html"), System.Text.Encoding.UTF8);
                    _html = _html.Replace("$page_title$", model.title);
                    _html = _html.Replace("_project_name_", model.title);

                    #region 高级模板
                    list_contents = BLL.Chats.projects.GetList(proj);
                     if (template.remark == ProjectDetaileTemplate.住宅房产详细模板.ToString())
                    {
                        _html = _html.Replace("_index_utl_", "detail.aspx?proj=" + proj + "&openId=" + openId + "&wx_id=" + wx_id + "&wx_og_id=" + wx_og_id);
                    }

                    #endregion


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