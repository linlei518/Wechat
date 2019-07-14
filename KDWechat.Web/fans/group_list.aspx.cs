using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;
using System.Text;
using KDWechat.DAL;

namespace KDWechat.Web.fans
{
    public partial class group_list : KDWechat.Web.UI.BasePage
    {
        #region 页面属性


        protected string key
        {
            get { return RequestHelper.GetQueryString("key"); }
        }

        /// <summary>
        /// 状态
        /// </summary>
        protected int status
        {
            get { return RequestHelper.GetQueryInt("status", -1); }
        }
        /// <summary>
        /// 区分id
        /// </summary>
        public int channel_id
        {
            get { return RequestHelper.GetQueryInt("channel_id", (int)channel_idType.关注用户分组); }
        }

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //判断权限
                CheckUserAuthority("wechat_user_group");

                CheckWXid();
                BindList();
            }
        }





        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindList()
        {
            StringBuilder Query = new StringBuilder();
            Query.Append(string.Format("select ID,TITLE,STATUS,create_time from t_wx_group_tags where  wx_id={0} and wx_og_id='{1}'  and channel_id={2} ",   wx_id, wx_og_id, channel_id));
            if (status > -1)
            {
                Query.Append(" and status=" + status);
            }

            if (key.Trim().Length > 0)
            {
                Query.Append(" and TITLE like '%" + key + "%'");
            }
            txtKey.Value = key;

            repList.DataSource = GetPageList(DbDataBaseEnum.KD_USERS, Query.ToString(), pageSize, page, "*", "id desc", ref totalCount);
            repList.DataBind();
            string pageUrl = string.Format("group_list.aspx?page=__id__&key={0}&m_id={1}",  HttpUtility.UrlEncode(key), m_id.ToString());
            div_page.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }




        protected void repList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Literal lblTitle = e.Item.FindControl("lblTitle") as Literal;
            string link = "group_list.aspx?page=" + page + "&key=" + HttpUtility.UrlEncode(key) + "&m_id=" + m_id;
           
             if (e.CommandName == "del")
            {
                int id = KDWechat.Common.Utils.StrToInt(e.CommandArgument.ToString(), 0);
                string errorMsg = "";
                KDWechat.BLL.Users.wx_group_tags.Delete(id, channel_idType.关注用户分组, ref errorMsg);
                if (errorMsg.Length > 0)
                {
                    JsHelper.AlertAndRedirect(errorMsg, link, "fail");
                }
                else
                {
                    AddLog("删除粉丝分组：" + lblTitle.Text, LogType.删除);
                    JsHelper.AlertAndRedirect("删除成功", link);
                }
            }

        }

   

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string pageUrl = string.Format("group_list.aspx?key={0}&m_id={1}", HttpUtility.UrlEncode(txtKey.Value.Trim()),  m_id.ToString());
            Response.Redirect(pageUrl);
        }
    }
}