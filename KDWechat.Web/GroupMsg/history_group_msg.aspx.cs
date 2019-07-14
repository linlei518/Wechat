using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;
using KDWechat.DAL;
using System.Text;

namespace KDWechat.Web.GroupMsg
{
    public partial class history_group_msg : System.Web.UI.Page
    {

        protected string enWx_id { get { return Request["fd"]; } }
        protected string ouputContent = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitData();
            }
        }




        private void InitData()
        {
            if (!string.IsNullOrWhiteSpace(enWx_id))
            {
                var wx_id = Utils.StrToInt(DESEncrypt.Decrypt(enWx_id), 0);
                if (wx_id != 0)
                {
                    var groupMsgList = Companycn.Core.EntityFramework.EFHelper.GetList<creater_wxEntities, t_wx_group_msgs, int>(x => x.wx_id == wx_id && (x.msg_type == (int)msg_type.单图文 || x.msg_type == (int)msg_type.多图文) && x.is_send == (int)is_sendMode.是, x => x.id, int.MaxValue, 1, true);
                    StringBuilder sb =new StringBuilder();
                    var wechatConfig = new BLL.Config.wechat_config().loadConfig();
                    groupMsgList.ForEach(x => {
                        if (x.msg_type == (int)msg_type.单图文)
                        {
                            var material = Companycn.Core.EntityFramework.EFHelper.GetModel<creater_wxEntities, t_wx_news_materials>(y => y.id == x.source_id);
                            if (material != null)
                            {
                                sb.Append("<div class=\"simulatorPanel_02\">");
                                sb.Append(string.Format("<a href=\"{0}/wxpage/news_template.aspx?id={1}&wx_og_id={2}\">",wechatConfig.domain,material.id,x.wx_og_id));
                                sb.Append("<div class=\"title\">");
                                sb.Append(string.Format("<h1>{0}</h1>",material.title));
                                sb.Append(string.Format("<h2>{0}</h2>",(x.send_time??DateTime.Now).ToString("MM月dd日 HH:mm")));
                                sb.Append("</div>");
                                sb.Append("<div class=\"img\">");
                                sb.Append("<img src=\"img/blank.gif\" class=\"placeholder\" alt=\"\">");
                                sb.Append(string.Format("<span style=\"background-image: url({0})\"></span>",material.cover_img));
                                sb.Append("</div>");
                                sb.Append("<div class=\"text\">");
                                sb.Append(string.Format("<p>{0}</p>",material.summary));
                                sb.Append("</div>");
                                sb.Append("<div class=\"links\">");
                                sb.Append("<p>阅读全文</p>");
                                sb.Append("</div>");
                                sb.Append("</a>");
                                sb.Append("</div>");
                            }
                        }
                        else if (x.msg_type == (int)msg_type.多图文)
                        {
                            var materialList = Companycn.Core.EntityFramework.EFHelper.GetList<creater_wxEntities, t_wx_news_materials,int>(y => y.id == x.source_id||y.par_id==x.source_id,y=>y.id,int.MaxValue,1);
                            if(materialList.Count>0)
                            {
                                
                                sb.Append("<div class=\"simulatorPanel_01\">");
                                var top = materialList.Where(y=>y.id==x.source_id).FirstOrDefault();
                                if(top!=null)
                                {
                                    sb.Append(string.Format("<h2 class=\"multiH2\">{0}</h2>",(x.send_time??DateTime.Now).ToString("MM月dd日 HH:mm")));
                                    sb.Append(string.Format("<a href=\"{0}/wxpage/news_template.aspx?id={1}&wx_og_id={2}\" class=\"main\">",wechatConfig.domain,top.id,x.wx_og_id));
                                    sb.Append("<div class=\"title\">");
                                    sb.Append(string.Format("<h1>{0}</h1>",top.title));
                                    sb.Append("</div>");
                                    sb.Append("<div class=\"img\">");
                                    sb.Append("<img src=\"img/blank.gif\" class=\"placeholder\" alt=\"\">");
                                    sb.Append(string.Format("<span style=\"background-image: url({0})\"></span>",top.cover_img));
                                    sb.Append("</div>");
                                    sb.Append("</a>");
                                }
                                var childList= materialList.Where(y=>y.par_id==x.source_id).ToList();
                                childList.ForEach(y=>{
                                    sb.Append(string.Format("<a href=\"{0}/wxpage/news_template.aspx?id={1}&wx_og_id={2}\">", wechatConfig.domain, y.id, x.wx_og_id));
                                    sb.Append("<div class=\"title\">");
                                    sb.Append(string.Format("<h1>{0}</h1>",y.title));
                                    sb.Append("</div>");
                                    sb.Append("<div class=\"img\">");
                                    sb.Append(string.Format("<span style=\"background-image: url({0})\"></span>",y.cover_img));
                                    sb.Append("</div>");
                                    sb.Append("</a>");
                                });
                                sb.Append("</div>");
                            }  
                        }
                    });
                    
                    ouputContent = sb.ToString();
                }
            }
        }






    }
}