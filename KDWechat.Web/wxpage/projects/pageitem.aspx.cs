using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Data;
using System.Text.RegularExpressions;
using KDWechat.Common;
namespace KDWechat.Web.wxpage.projects
{
    public partial class pageitem : Web.UI.TemplateHelp
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Expires = -1;
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            Response.CacheControl = "no-cache";
            int page = Common.RequestHelper.GetQueryInt("page", 1);
            int pagesize = 10;
            string url = "";


            int rowcount = 0;
            DataTable dt = GetPageList(DbDataBaseEnum.KD_WECHATS, "select  * from t_project_imgage where project_id=" + proj + " and category_name='" + tname + "' ", pagesize, page, "*", "id asc", ref rowcount);

            if (rowcount > (pagesize * page))
            {
                url = "pageitem.aspx?page=" + (page + 1) + "&proj=" + proj + "&tname=" + tname + "&openId=" + openId + "&wx_og_id=" + wx_og_id + "&wx_id=" + wx_id + "";
            }



            #region xml字符串

            System.Text.StringBuilder values = new System.Text.StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<listItems>\r\n");
            values.AppendLine("<config url = \"" + url + "\"  node =\"li\"></config>");

            if (dt != null)
            {

                foreach (DataRow r in dt.Rows)
                {
                    values.AppendLine("<item>");

                    values.AppendLine("<a href=\"picdetail.aspx?proj=" + proj + "&tname=" + tname + "&openId=" + openId + "&wx_og_id=" + wx_og_id + "&wx_id=" + wx_id + "#pic=" + r["id"] + "\">");
                    values.AppendLine("<span><img src=\"" + r["img_url"] + "\" alt=\"\"/></span>");
                    values.AppendLine("</a>");
                    //values.AppendLine("<![CDATA[");

                    //values.AppendLine("]]>");
                    values.AppendLine("</item>");

                }
            }
            values.AppendLine("</listItems>");

            Response.Write(values.ToString());
            #endregion
        }

        /// <summary>
        /// 存储过程分页
        /// </summary>
        /// <param name="dbType">数据库（1、操作日志库 2、用户信息库 3、微信架构库）</param>
        /// <param name="QueryStr">查询数据</param>
        /// <param name="PageSize">分页大小</param>
        /// <param name="PageCurrent">当前页</param>
        /// <param name="FdShow">显示的字段</param>
        /// <param name="FdOrder">排序字段</param>
        /// <param name="rowCount">数据总条数</param>
        /// <returns></returns>
        public DataTable GetPageList(DbDataBaseEnum dbType, string QueryStr, int PageSize, int PageCurrent, string FdShow, string FdOrder, ref int rowCount)
        {
            return KDWechat.BLL.PageHelper.GetPageList(dbType, QueryStr, PageSize, PageCurrent, FdShow, FdOrder, ref rowCount);
        }
    }
}