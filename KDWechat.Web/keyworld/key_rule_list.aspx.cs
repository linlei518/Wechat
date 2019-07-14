using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;

namespace KDWechat.Web.keyworld
{
    public partial class key_rule_list : KDWechat.Web.UI.BasePage
    {
        #region 自定义属性
        /// <summary>
        /// 状态
        /// </summary>
        protected int status
        {
            get { return RequestHelper.GetQueryInt("status", -1); }
        }
        /// <summary>
        /// 关键字
        /// </summary>
        protected string key
        {
            get { return RequestHelper.GetQueryString("key"); }
        }

        /// <summary>
        /// 回复类型
        /// </summary>
        protected int reply_type
        {
            get { return RequestHelper.GetQueryInt("reply_type", -1); }
        }
        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //判断权限
                CheckUserAuthority("key_reply_rule");

                CheckWXid();


                BindList();
            }
        }

        public string GetStatus(object reply_type)
        {
            string key = "";
            switch (Common.Utils.ObjToInt(reply_type,0))
            {
                case (int)msg_type.文本:
                    key = "文本";
                    break;
                case (int)msg_type.图片:
                    key = "图片";
                    break;
                case (int)msg_type.语音:
                    key = "语音";
                    break;
                case (int)msg_type.视频:
                    key = "视频";
                    break;

                case (int)msg_type.单图文:
                    key = "单图文";
                    break;
                case (int)msg_type.多图文:
                    key = "多图文";
                    break;
                case (int)msg_type.模块:
                    key = "模块";
                    break;
                case (int)msg_type.多客服:
                    key = "多客服";
                    break;
            }
            return key;
        }


        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindList()
        {
            StringBuilder Query = new StringBuilder();
            Query.Append(string.Format("select id,rule_name,status,reply_type from t_wx_rules where wx_id={0} and wx_og_id='{1}'", wx_id, wx_og_id));
            if (!string.IsNullOrEmpty(key))
            {
                Query.Append(" and  id in(select r_id from t_wx_rules_keywords where key_words like '%" + key + "%')");
            }
            
            if (status > -1)
            {
                Query.Append(" and status=" + status);
            }
            if (reply_type>-1)
            {
                Query.Append(" and reply_type=" + reply_type);
            }

            repList.DataSource = GetPageList(DbDataBaseEnum.KD_WECHATS, Query.ToString(), pageSize, page, "*", "id desc", ref totalCount);
            repList.DataBind();
            string pageUrl = string.Format("key_rule_list.aspx?key={0}&status={1}&m_id={2}&page=__id__&reply_type={3}", HttpUtility.UrlEncode(key), status, m_id, reply_type);
            div_page.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
            if (totalCount < pageSize)
            {
                div_page.Visible = false;
            }
            ddlGroup.SelectedValue = status.ToString();
            txtKey.Value = key;
            ddlReplyType.SelectedValue = reply_type.ToString();
        }
        /// <summary>
        /// 获取关键词列表
        /// </summary>
        /// <param name="group_id"></param>
        /// <returns></returns>
        public string GetKeyList(object r_id)
        {
            return KDWechat.BLL.Chats.wx_rules_keywords.GetKeywordLists(Common.Utils.ObjToInt(r_id, 0));
           
        }



        protected void repList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Literal lblTitle = e.Item.FindControl("lblTitle") as Literal;
            if (e.CommandName == "status")
            {
                int id = KDWechat.Common.Utils.StrToInt(e.CommandArgument.ToString(), 0);
                int status = 0;
                if (((LinkButton)e.CommandSource).Text == "启用")
                {
                    status = 1;
                }
                KDWechat.BLL.Chats.wx_rules.UpdateStatus(id, status);
                AddLog("更改规则关键词状态：" + lblTitle.Text + " 为" + (status == 1 ? "启用" : "禁用"), LogType.修改);
                JsHelper.AlertAndRedirect((status == 1 ? "已启用" : "已禁用"),"key_rule_list.aspx?key=" + HttpUtility.UrlEncode(txtKey.Value.Trim()) + "&status=" + ddlGroup.SelectedValue + "&page=" + page + "&m_id=" + m_id  );
               // Response.Redirect("key_rule_list.aspx?key=" + HttpUtility.UrlEncode(txtKey.Value.Trim()) + "&status=" + ddlGroup.SelectedValue + "&page=" + page + "&m_id=" + m_id + "&success=" + HttpUtility.UrlEncode( (status == 1 ? "已启用" : "已禁用")));
               
            }
            else if (e.CommandName == "del")
            {
                int id = KDWechat.Common.Utils.StrToInt(e.CommandArgument.ToString(), 0);
                KDWechat.BLL.Chats.wx_rules.Delete(id);
                AddLog("删除规则关键词：" + lblTitle.Text, LogType.删除);
               JsHelper.AlertAndRedirect("删除成功","key_rule_list.aspx?key=" + HttpUtility.UrlEncode(txtKey.Value.Trim()) + "&status=" + ddlGroup.SelectedValue + "&page=" + page + "&m_id=" + m_id  );
                
            }
            
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string pageUrl = string.Format("key_rule_list.aspx?key={0}&status={1}&m_id={2}&reply_type={3}", HttpUtility.UrlEncode(txtKey.Value.Trim()), ddlGroup.SelectedValue, m_id, ddlReplyType.SelectedValue);
           
            Response.Redirect(pageUrl);
        }

        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pageUrl = string.Format("key_rule_list.aspx?key={0}&status={1}&m_id={2}&reply_type={3}", HttpUtility.UrlEncode(txtKey.Value.Trim()), ddlGroup.SelectedValue, m_id, ddlReplyType.SelectedValue);

            Response.Redirect(pageUrl);
        }
    }
}