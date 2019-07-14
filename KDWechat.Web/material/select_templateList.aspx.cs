using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;

namespace KDWechat.Web.material
{
    public partial class select_templateList : KDWechat.Web.UI.BasePage
    {
        #region 页面属性


        /// <summary>
        /// 关键字
        /// </summary>
        protected string key
        {
            get { return RequestHelper.GetQueryString("key"); }
        }

        /// <summary>
        /// 状态
        /// </summary>
        protected int type
        {
            get { return RequestHelper.GetQueryInt("type", -2); }
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

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                BindList();
            }
        }



        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindList()
        {
            ddlType.SelectedValue = type.ToString();
            txtKey.Value = key;
            txtbegin_date.Text = beginDate;
            txtend_date.Text = endDate;
            StringBuilder Query = new StringBuilder();
            Query.Append(string.Format("select * from ( select *,(select title from t_wx_templates where id=template_id) as title,(select create_time from t_wx_templates where id=template_id) as create_time,(select img_url from t_wx_templates where id=template_id) as img_url,(select status from t_wx_templates where id=template_id) as status,(select cate_id from t_wx_templates where id=template_id) as cate_id,(select remark from t_wx_templates where id=template_id) as remark from t_wx_templates_wechats where wx_id={0} ) as M  where status=1  ", wx_id));
            if (!string.IsNullOrEmpty(key))
            {
                Query.Append(" and title like '%" + key + "%'");
            }

            if (type > -2)
            {
                Query.Append(" and cate_id=" + type);
            }

            if (beginDate.Length > 0 && endDate.Length > 0)
            {
                txt_date_show.Value = beginDate + " — " + endDate;
                Query.Append(" and convert(varchar(10),create_time,120)>= '" + beginDate + "' and convert(varchar(10),create_time,120)<='" + endDate + "'");
            }
            else if (beginDate.Length > 0)
            {
                txt_date_show.Value = beginDate;
                Query.Append(" and create_time between '" + beginDate + "'  and getdate()");
            }
            else if (endDate.Length > 0)
            {
                txt_date_show.Value = endDate;
                Query.Append(" and convert(varchar(10),create_time) <='" + endDate + "' ");
            }

            repList.DataSource = GetPageList(DbDataBaseEnum.KD_WECHATS, Query.ToString(), pageSize, page, "*", "id desc", ref totalCount);
            repList.DataBind();
            //string pageUrl = string.Format("select_templateList.aspx?key={0}&type={1}&page=__id__&m_id={2}&beginDate={3}&endDate={4}", HttpUtility.UrlEncode(key), type, m_id, beginDate, endDate);
            //div_page.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
            //if (totalCount < pageSize)
            //{
            //    div_page.Visible = false;
            //}
        }



 

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pageUrl = string.Format("select_templateList.aspx?key={0}&type={1}&m_id={2}&beginDate={3}&endDate={4}", txtKey.Value.Trim(), ddlType.SelectedValue, m_id, txtbegin_date.Text.Trim(), txtend_date.Text.Trim());
            Response.Redirect(pageUrl);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string pageUrl = string.Format("select_templateList.aspx?key={0}&type={1}&m_id={2}&beginDate={3}&endDate={4}", txtKey.Value.Trim(), ddlType.SelectedValue, m_id, txtbegin_date.Text.Trim(), txtend_date.Text.Trim());
            Response.Redirect(pageUrl);
        }
    }
}