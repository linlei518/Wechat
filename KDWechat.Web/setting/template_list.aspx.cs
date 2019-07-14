using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;

namespace KDWechat.Web.setting
{
    public partial class template_list : KDWechat.Web.UI.BasePage
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
                //判断权限
                CheckUserAuthority("template_list");

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
            string pageUrl = string.Format("template_list.aspx?key={0}&type={1}&page=__id__&m_id={2}&beginDate={3}&endDate={4}", HttpUtility.UrlEncode(key), type, m_id,beginDate,endDate);
            div_page.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
            if (totalCount < pageSize)
            {
                div_page.Visible = false;
            }
        }




        protected void repList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string pageUrl = string.Format("template_list.aspx?key={0}&type={1}&m_id={2}&beginDate={3}&endDate={4}", txtKey.Value.Trim(), ddlType.SelectedValue, m_id,txtbegin_date.Text.Trim(),txtend_date.Text.Trim());
            Literal lblTitle = e.Item.FindControl("lblTitle") as Literal;
            
            if (e.CommandName == "edit")
            {
                Response.Redirect("template_add.aspx?id=" + e.CommandArgument + "&m_id=" + m_id);
            }
            else if (e.CommandName == "is_default")
            {
                int id = Common.Utils.StrToInt((e.Item.FindControl("hidid") as HiddenField).Value, 0);
                int template_id = KDWechat.Common.Utils.StrToInt(e.CommandArgument.ToString(), 0);
                KDWechat.BLL.Chats.wx_templates.SetDefaultWithWX(id, template_id,wx_id);
                AddLog("将“"+lblTitle.Text+"”设为默认的图文模板");
                JsHelper.AlertAndRedirect("设置成功", pageUrl+"&al=1");
            }
            else if (e.CommandName == "del")
            {
                int id = KDWechat.Common.Utils.StrToInt(e.CommandArgument.ToString(), 0);
                if (KDWechat.BLL.Chats.wx_templates.Delete(id))
                {
                    try
                    {
                        string img = (e.Item.FindControl("hf_img") as HiddenField).Value;
                        if (File.Exists(Server.MapPath(img)))
                        {
                            File.Delete(Server.MapPath(img));
                        }
                    }
                    catch (Exception)
                    {
                    }
                    AddLog("删除自定义图文模板：" + lblTitle.Text, LogType.删除);
                    JsHelper.AlertAndRedirect("删除成功", pageUrl);
                }
                else
                {
                    JsHelper.AlertAndRedirect("删除失败", pageUrl,"fail");
                }
               

            }

        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pageUrl = string.Format("template_list.aspx?key={0}&type={1}&m_id={2}&beginDate={3}&endDate={4}", txtKey.Value.Trim(), ddlType.SelectedValue, m_id, txtbegin_date.Text.Trim(), txtend_date.Text.Trim());
            Response.Redirect(pageUrl);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string pageUrl = string.Format("template_list.aspx?key={0}&type={1}&m_id={2}&beginDate={3}&endDate={4}", txtKey.Value.Trim(), ddlType.SelectedValue, m_id, txtbegin_date.Text.Trim(), txtend_date.Text.Trim());
            Response.Redirect(pageUrl);
        }
    }
}