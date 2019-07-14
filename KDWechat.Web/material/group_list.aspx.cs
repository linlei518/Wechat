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
    public partial class group_list : KDWechat.Web.UI.BasePage
    {
        #region 页面属性
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
            get { return RequestHelper.GetQueryInt("channel_id", (int)channel_idType.素材分组); }
        }

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //判断权限
                CheckUserAuthority((is_public == 1 ? "material_group_public" : "material_group"));

                if (is_public == 0)
                {
                    CheckWXid();
                }
                
                BindList();
            }
        }

  

       

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindList()
        {
            StringBuilder Query = new StringBuilder();
            Query.Append(string.Format("select ID,TITLE,STATUS,create_time from t_wx_group_tags where  wx_id={0} and wx_og_id='{1}' and is_public={2} and channel_id={3} ", (is_public == 1 ? 0 : wx_id), (is_public == 1 ? "" : wx_og_id), is_public, channel_id));
            if (only_op_self==1)
            {
                Query.Append(" and u_id=" + u_id);
            }


            if (status > -1)
            {
                Query.Append(" and status=" + status);
            }

            if (key.Trim().Length>0)
            {
                Query.Append(" and TITLE like '%"+key+"%'" );
            }
            //ddlStatus.SelectedValue = status.ToString();
            txtKey.Value = key;

            repList.DataSource = GetPageList(DbDataBaseEnum.KD_USERS, Query.ToString(), pageSize, page, "*", "id desc", ref totalCount);
            repList.DataBind();
            string pageUrl = string.Format("group_list.aspx?is_pub={0}&channel_id={1}&page=__id__&key={2}&m_id={3}",  is_pub, channel_id, HttpUtility.UrlEncode(key), m_id.ToString());
            div_page.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
            if (totalCount < pageSize)
            {
                div_page.Visible = false;
            }
        }
       



        protected void repList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Literal lblTitle = e.Item.FindControl("lblTitle") as Literal;
            string link = "group_list.aspx?status=" + this.status + "&page=" + page + "&is_pub=" + is_pub + "&channel_id=" + channel_id + "&key=" + HttpUtility.UrlEncode(key) + "&m_id=" + m_id;
            if (e.CommandName == "status")
            {
                //int id = KDWechat.Common.Utils.StrToInt(e.CommandArgument.ToString(), 0);
                //int status = 0;
                //if (((LinkButton)e.CommandSource).Text == "启用")
                //{
                //    status = 1;
                //}
                //KDWechat.BLL.Users.wx_group_tags.UpdateStatus(id, status);
                //AddLog("更改素材分组状态：" + lblTitle.Text + " 为" + (status == 1 ? "启用" : "禁用"), LogType.修改);
                //Response.Redirect(link);
            }
            else if (e.CommandName == "del")
            {
                int id = KDWechat.Common.Utils.StrToInt(e.CommandArgument.ToString(), 0);
                string errorMsg = "";
                KDWechat.BLL.Users.wx_group_tags.Delete(id, channel_idType.素材分组, ref errorMsg);
                if (errorMsg.Length>0)
                {
                    JsHelper.AlertAndRedirect(errorMsg, link, "fail");
                }
                else
                {
                    AddLog("删除素材分组：" + lblTitle.Text, LogType.删除);
                    JsHelper.AlertAndRedirect("删除成功",link);
                }
            }
           
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pageUrl = string.Format("group_list.aspx?key={0}&is_pub={1}&m_id={2}", HttpUtility.UrlEncode(txtKey.Value.Trim()), is_pub, m_id.ToString());
            Response.Redirect(pageUrl);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string pageUrl = string.Format("group_list.aspx?key={0}&is_pub={1}&m_id={2}", HttpUtility.UrlEncode(txtKey.Value.Trim()),  is_pub, m_id.ToString());
            Response.Redirect(pageUrl);
        }
    }
}