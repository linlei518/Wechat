using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;

namespace KDWechat.Web.setting
{
    public partial class template_manager : Web.UI.BasePage
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
               
                if (RequestHelper.GetQueryString("action")=="set_default")
                {
                    wchatConfig.is_use_default_template = RequestHelper.GetQueryString("status");
                    new BLL.Config.wechat_config().saveConifg(wchatConfig);
                    Response.Write("1");
                }
                else
                {
                    CheckUserAuthority("template");
                    BindList();
                }
            
            }
        }



        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindList()
        {
            txtKey.Value = key;
            txtbegin_date.Text = beginDate;
            txtend_date.Text = endDate;
            StringBuilder Query = new StringBuilder();
            Query.Append("select * from t_wx_templates where channel_id=1 and cate_id=0 ");
            if (!string.IsNullOrEmpty(key))
            {
                Query.Append(" and title like '%" + key + "%'");
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
            string pageUrl = string.Format("template_manager.aspx?page=__id__&m_id={0}&beginDate={1}&endDate={2}", m_id,beginDate,endDate);
            if (totalCount > pageSize)
            {
                div_page.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
            }
            else
            {
                div_page.Visible = false;
            }

          

            if (wchatConfig.is_use_default_template=="1")
            {
                rboStatus_0.Checked = true;
            }
            else if (wchatConfig.is_use_default_template == "0")
            {
                rboStatus_1.Checked = true;
            }

        }

        protected void repList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

            if (e.CommandName == "is_default")
            {
                if ((e.Item.FindControl("hfstatus") as HiddenField).Value == "0")
                {
                    JsHelper.Alert(Page,"该模板已被禁用，不能设为默认","true");
                    return;
                }
                KDWechat.BLL.Chats.wx_templates.SetDefaultWithSystem(Convert.ToInt32(e.CommandArgument));
                JsHelper.AlertAndRedirect("设置成功", "template_manager.aspx?m_id=" + m_id + "&beginDate=" + txtbegin_date.Text.Trim() + "&endDate=" + txtend_date.Text.Trim());
            }
            else if (e.CommandName == "setStatus")
            {
                Button btn = e.CommandSource as Button;
                if (btn.Text == "禁用" && (e.Item.FindControl("hfisdefault") as HiddenField).Value == "1")
                {
                    JsHelper.Alert(Page, "该模板为当前默认模板，不能禁用", "true");
                    return;
                }
                KDWechat.BLL.Chats.wx_templates.SetStatus(Convert.ToInt32(e.CommandArgument), (btn.Text == "启用" ? 1 : 0));
                JsHelper.AlertAndRedirect(btn.Text + "成功", "template_manager.aspx?m_id=" + m_id + "&beginDate=" + txtbegin_date.Text.Trim() + "&endDate=" + txtend_date.Text.Trim());
            }

          
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string pageUrl = string.Format("template_manager.aspx?key={0}&m_id={1}&beginDate={2}&endDate={3}", txtKey.Value.Trim(), m_id,txtbegin_date.Text.Trim(),txtend_date.Text.Trim());
            Response.Redirect(pageUrl);
        }
    }
}