using KDWechat.Common;
using KDWechat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.keyworld
{
    public partial class retrans_keyword_list : Web.UI.BasePage
    {
        /// <summary>
        /// 关键字
        /// </summary>
        protected string key
        {
            get { return RequestHelper.GetQueryString("key"); }
        }
        /// <summary>
        /// 起始时间
        /// </summary>
        public string beginDate
        {
            get { return RequestHelper.GetQueryString("beginDate"); }
        }
        /// <summary>
        ///结束时间
        /// </summary>
        public string endDate
        {
            get { return RequestHelper.GetQueryString("endDate"); }
        }
        public int sv { get { return RequestHelper.GetQueryInt("sv", -1); } }


        protected void Page_Load(object sender, EventArgs e)
        {
            //权限相关，发布开启
            //CheckUserAuthority("draw_winner_all");

            var serverList = Companycn.Core.EntityFramework.EFHelper.GetList<creater_wxEntities, t_retrans_server, int>(x => x.wx_id == wx_id, x => x.id, int.MaxValue, 1, true);
            ddlServer.DataSource = serverList;
            ddlServer.DataTextField = "title";
            ddlServer.DataValueField = "id";
            ddlServer.DataBind();
            if (!IsPostBack)
            {
                BindData();
                ddlServer.SelectedValue = sv.ToString();
                //ddlWechats.SelectedValue = wxID.ToString();

            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string pageUrl = string.Format("retrans_keyword_list.aspx?key={0}&beginDate={1}&endDate={2}&m_id={3}&sv={4}", HttpUtility.UrlEncode(Common.Utils.Filter(txtKey.Value.Trim())), Common.Utils.Filter(txtbegin_date.Text.Trim()), Common.Utils.Filter(txtend_date.Text.Trim()), m_id,sv);

            Response.Redirect(pageUrl);
        }
        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindData()
        {
            Expression<Func<retrans_keyword_server_view, bool>> where = x => x.wx_id == wx_id;

            if (beginDate.Length > 0 && endDate.Length > 0)
            {
                DateTime start = Utils.StrToDateTime(beginDate, DateTime.Now).Date;
                DateTime end = Utils.StrToDateTime(endDate, DateTime.Now).Date.AddDays(1);
                where = where.And(x => x.create_time > start && x.create_time < end);
            }
            else if (beginDate.Length > 0)
            {
                DateTime start = Utils.StrToDateTime(beginDate, DateTime.Now).Date;
                where = where.And(x => x.create_time > start);
            }
            else if (endDate.Length > 0)
            {
                DateTime end = Utils.StrToDateTime(endDate, DateTime.Now).Date.AddDays(1);
                where = where.And(x => x.create_time < end);
            }
            if (!string.IsNullOrEmpty(key))
            {
                where = where.And(x => x.keyword.Contains(key));
            }
            if (sv != -1)
            {
                where = where.And(x => x.retrans_id == sv);
            }
            Repeater1.DataSource = Companycn.Core.EntityFramework.EFHelper.GetList<creater_wxEntities, retrans_keyword_server_view, int>(where, x => x.id, pageSize, page, out totalCount, true);
            Repeater1.DataBind();
            string pageUrl = string.Format("retrans_keyword_list.aspx?key={0}&beginDate={1}&endDate={2}&m_id={3}&sv={4}&page=__id__", HttpUtility.UrlEncode(Common.Utils.Filter(txtKey.Value.Trim())), Common.Utils.Filter(txtbegin_date.Text.Trim()), Common.Utils.Filter(txtend_date.Text.Trim()), m_id, sv);
            div_page.InnerHtml = Utils.OutPageList(pageSize, page, totalCount, pageUrl, 8);

            //绑定控件值
            txtKey.Value = key;
            txtbegin_date.Text = beginDate;
            txtend_date.Text = endDate;

            if (beginDate.Trim() != "" && endDate.Trim() != "")
            {
                txt_date_show.Value = beginDate + " — " + endDate;
            }
            else if (beginDate.Trim() != "")
            {
                txt_date_show.Value = beginDate;
            }
            else if (endDate.Trim() != "")
            {
                txt_date_show.Value = endDate;
            }


        }

        protected void ddlServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pageUrl = string.Format("retrans_keyword_list.aspx?key={0}&beginDate={1}&endDate={2}&m_id={3}&page=__id__&sv={4}", HttpUtility.UrlEncode(Common.Utils.Filter(txtKey.Value.Trim())), Common.Utils.Filter(txtbegin_date.Text.Trim()), Common.Utils.Filter(txtend_date.Text.Trim()), m_id, ddlServer.SelectedValue);
            Response.Redirect(pageUrl);
        }
    }
}