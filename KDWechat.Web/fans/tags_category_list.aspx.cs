using KDWechat.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.DAL;

namespace KDWechat.Web.fans
{
    public partial class tags_category_list : KDWechat.Web.UI.BasePage
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
            get { return RequestHelper.GetQueryInt("channel_id", (int)channel_idType.关注用户标签); }
        }

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //CheckUserAuthority("tag_list");
                BindList();
            }
        }



        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string pageUrl = string.Format("tags_list.aspx?key={0}&m_id={1}", HttpUtility.UrlEncode(txtKey.Value.Trim()), m_id.ToString());
            Response.Redirect(pageUrl);
        }


        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindList()
        {
            Expression<Func<t_tags_category, bool>> where = x => true;

            if (key.Trim().Length > 0)
            {
                where = where.And(x => x.title == key.Trim());
            }
            txtKey.Value = key;

            repList.DataSource = Companycn.Core.EntityFramework.EFHelper.GetList<creater_wxEntities, t_tags_category, int>(where, x => x.id, pageSize, page,out totalCount, true);//GetPageList(DbDataBaseEnum.KD_USERS, Query.ToString(), pageSize, page, "*", "id desc", ref totalCount);
            repList.DataBind();
            string pageUrl = string.Format("tags_category_list.aspx?page=__id__&key={0}&m_id={1}", HttpUtility.UrlEncode(key), m_id);
            div_page.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }




        protected void repList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Literal lblTitle = e.Item.FindControl("lblTitle") as Literal;
            string link = "tags_category_list.aspx?page=" + page + "&key=" + HttpUtility.UrlEncode(key) + "&m_id=" + m_id;
           if (e.CommandName == "del")
            {
                int id = KDWechat.Common.Utils.StrToInt(e.CommandArgument.ToString(), -1);
                var count = Companycn.Core.EntityFramework.EFHelper.GetCount<creater_wxEntities, t_tags_category>(x => x.parent_id == id);
                if (count > 0)
                {
                    JsHelper.AlertAndRedirect("此分类非空，请删除分类下所有标签后再试！", link,"fail");
                }
                else
                {
                    Companycn.Core.EntityFramework.EFHelper.DeleteModel<creater_wxEntities, t_tags_category>(x => x.id == id);
                    AddLog("删除标签分类：" + lblTitle.Text, LogType.删除);
                    JsHelper.AlertAndRedirect("删除成功", link);
                }
            }

        }
 

    }
}