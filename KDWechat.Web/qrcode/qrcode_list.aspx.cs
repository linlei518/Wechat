using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.DAL;
using KDWechat.BLL.Chats;
using KDWechat.Common;
using System.IO;

namespace KDWechat.Web.qrcode
{
    public partial class qrcode_list : Web.UI.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckUserAuthority("qrcode_list");
            if (!IsPostBack)
                InitData();
        }

        private void InitData()
        {
            var qrcode_list = wx_qrcode.GetList<int>((x => x.wx_id==wx_id&&x.q_type==(int)QrCodeType.拓客用), x => x.id, pageSize, page, out totalCount, true);
            repList.DataSource = qrcode_list;
            repList.DataBind();

            string pageUrl = string.Format("qrcode_list.aspx?page=__id__&m_id={0}", m_id);
            div_page.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }

        protected void repList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "sss")
            {
                string[] list = e.CommandArgument.ToString().Split('|');
                string id = list[0];
                string fileName = "~/upload/" + wx_id + "_" + id + ".png";
                if (!File.Exists(fileName))//动态添加文件夹
                {
                    string url = list[1];
                    System.Net.WebClient wc = new System.Net.WebClient();
                    wc.DownloadFile(url,Server.MapPath(fileName));
                    wc.Dispose();
                }
                Response.AppendHeader("Content-Disposition", "attachment;filename="+DateTime.Now.Ticks+".jpg");
                Response.ContentType = "image/jpeg";
                Response.WriteFile(Server.MapPath(fileName));
                Response.End();
            }
        }
    }
}