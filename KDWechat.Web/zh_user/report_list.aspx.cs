using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using ICSharpCode.SharpZipLib.Zip;
using KDWechat.Common;
using KDWechat.DAL;
using KDWechat.Web.UI;
using QuickMark;

namespace KDWechat.Web.zh_user
{
    public partial class report_list : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckUserAuthority("report_list");
                BindList();
            }
        }


        private void BindList()
        {
            txtKeywords.Value = key;
            var where = " where 1=1  ";

            if (!string.IsNullOrEmpty(key))
            {
                where += "  and tzu.user_name like  '%'+@name+'%'  ";
                txtKeywords.Value = key;
            }
          
            if (status > -1)
            {
                where += "  and tzr.status=@status  ";
                ddlStatus.SelectedValue = status.ToString();
            }
            
            var list = DapperConnection.minebea.GetListBySql<view_zh_report>(" select tzr.*,tzu.user_name,tzu.user_code,tzu.user_tel from t_zh_report tzr left join t_zh_user tzu on tzr.user_id=tzu.id " + where, " tzr.id desc ", pageSize, page, out totalCount, new { name = key, status = status });
           
            this.rptList.DataSource = list;
            this.rptList.DataBind();

          
            string pageUrl = $"report_list.aspx?status={status}&key={key}&page=__id__&m_id=" +m_id;
         
            div_page.InnerHtml = Utils.OutPageList(pageSize, page, totalCount, pageUrl, 8);


            EnumHelper.BindEnumDDLSource(typeof(Enums.YesOrNo), ddlStatus);//发布状态

        }


        public class view_zh_report: t_zh_report
        {
            public string user_name { get; set; }
            public string user_code { get; set; }
            public string user_tel { get; set; }
        }


        public string get_imgs(string imgs_url)
        {
            var str = "";
            foreach (var item in imgs_url.Split('|'))
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    str += "<img src='"+RequestHelper.GetHost()+":8011"+ "/zh_wechat/" + item+ "' width='40px' height='40px' onclick='dialogue.dlShowPic(\""+ RequestHelper.GetHost() + ":8011" + "/zh_wechat/" + item + "\")' /> ";
                }
            }
            return str;
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect($"report_list.aspx?status={ddlStatus.SelectedValue}&key={txtKeywords.Value.Trim()}" + "&m_id=" + m_id);
        }

        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            var url = $"report_list.aspx?status={status}&key={key}" + "&m_id=" + m_id;
            var id = Utils.StrToInt(e.CommandArgument.ToString(), 0);
            switch (e.CommandName)
            {
                case "del":
                 
                    break;
                case "do":
                    if (DapperConnection.minebea.UpdateModel<t_zh_report>(new { status = 1 }, new { id = id }))
                    {
                        AddLog(string.Format("处理举报信息：{0}", ((e.Item.FindControl("lblTitle") as Literal).Text)), LogType.修改);
                        JsHelper.AlertAndRedirect("处理举报信息成功！", "report_list.aspx?key=" + txtKeywords.Value + "&status=" + status + "&m_id=" + m_id); return;
                    }
                    JsHelper.AlertAndRedirect("处理举报信息失败！", "report_list.aspx?key=" + txtKeywords.Value + "&status=" + status + "&m_id=" + m_id); return;
                case "unpush":
                   
                    break;
            }
            Response.Redirect(url);
        }


     




      


    }
}