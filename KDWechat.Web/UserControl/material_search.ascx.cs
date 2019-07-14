using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;
using KDWechat.Web.UI;

namespace KDWechat.Web.UserControl
{
    public partial class material_search : BaseControl
    { 
        
        /// <summary>
        /// 是否公共素材
        /// </summary>
        protected int is_public
        {
            get
            {
                int _is_pub = 0;
                if (is_pub == "1.1.1")
                {
                    _is_pub = 1;
                }
                return _is_pub;
            }
        }

        /// <summary>
        /// 分组id
        /// </summary>
        protected int group_id
        {
            get { return RequestHelper.GetQueryInt("group_id", -1); }
        }
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

        /// <summary>
        /// 公共素材标记文本
        /// </summary>
        protected string is_pub
        {
            get
            {
                string temp = RequestHelper.GetQueryString("is_pub");
                temp = temp == "" ? "0.0.0" : temp;
                return temp;
            }
        }
        /// <summary>
        /// 设置页面地址
        /// </summary>
        public string page_link
        { get; set; }

        /// <summary>
        /// 设置页面地址
        /// </summary>
        public string isshow_group
        { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SettingSearch();
            }
        }

        private void SettingSearch()
        {
            ddlGroup.Visible = false;
            if (isshow_group=="1")
            {
                ddlGroup.Visible = true;
                if (is_public == 1)
                {
                    ddlGroup.DataSource = KDWechat.BLL.Users.wx_group_tags.GetListByChannelId((int)channel_idType.素材分组, 0, "", is_public, -1);
                }
                else
                {
                    ddlGroup.DataSource = KDWechat.BLL.Users.wx_group_tags.GetListByChannelId((int)channel_idType.素材分组, wx_id, wx_og_id, is_public, -1);
                }

                ddlGroup.DataBind();

                ddlGroup.SelectedValue = group_id.ToString();
               
            }
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string pageUrl = string.Format("key={0}&group_id={1}&beginDate={2}&is_pub={3}&endDate={4}&m_id={5}", HttpUtility.UrlEncode(Common.Utils.Filter(txtKey.Value.Trim())), ddlGroup.SelectedValue, Common.Utils.Filter(txtbegin_date.Text.Trim()), is_pub, Common.Utils.Filter(txtend_date.Text.Trim()), m_id);
            if (page_link.Contains("?"))
            {
                pageUrl = page_link + "&" + pageUrl;
            }
            else
            {
                pageUrl = page_link + "?" + pageUrl;
            }
            Response.Redirect(pageUrl);
        }

        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pageUrl = string.Format("key={0}&group_id={1}&beginDate={2}&is_pub={3}&endDate={4}&m_id={5}", HttpUtility.UrlEncode(Common.Utils.Filter(txtKey.Value.Trim())), ddlGroup.SelectedValue, Common.Utils.Filter(txtbegin_date.Text.Trim()), is_pub, Common.Utils.Filter(txtend_date.Text.Trim()), m_id);
            if (page_link.Contains("?"))
            {
                pageUrl = page_link + "&" + pageUrl;
            }
            else
            {
                pageUrl = page_link + "?" + pageUrl;
            }
            Response.Redirect(pageUrl);
        }
    }
}