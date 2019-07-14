using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;

namespace KDWechat.Web.keyworld
{
    public partial class select_module : KDWechat.Web.UI.BasePage
    {
        #region 页面属性



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
        /// 图片 = 1, 语音 = 2, 视频 = 3, 单图文 = 4 ，多图文=5 ，模块=8
        /// </summary>
        protected int channel_id
        {
            get { return RequestHelper.GetQueryInt("channel_id", 8); }
        }


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                BindModule();
                BindList();
            }
        }



        //获取模块列表
        protected void BindModule()
        {
            StringBuilder Query = new StringBuilder();
            //Query.Append(string.Format("select tmw.ID,tm.title from t_module_wechat tmw,t_modules tm where tmw.module_id=tm.id and tmw.wx_id={0}",wx_id));
            Query.Append(string.Format("select tm.id,tm.title from t_module_wx_switch ts,t_modules tm where ts.module_id=tm.id and tm.is_push=0 and ts.status={0} and ts.wx_id={1} order by id", 1, wx_id));
            DataTable dt = BLL.Chats.module_wechat.GetListByQuery(Query.ToString());

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ListItem itemP = new ListItem();

                    itemP.Text = dt.Rows[i]["title"].ToString();
                    itemP.Value = dt.Rows[i]["id"].ToString();
                    ddlGroup.Items.Add(itemP);
                }
                
            }


        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindList()
        {
            StringBuilder Query = new StringBuilder();
            Query.Append("select id,module_id,wx_id,app_name,app_img_url,channel_id,app_remark,(select title from t_modules where id=module_id) as module_name  from t_module_wechat where status=1 and channel_id=0 and wx_id=" + wx_id);

            if (string.IsNullOrEmpty(key) && group_id == 0)
            {
                Query.Append("   or ID=1 ");
            }
            else
            {
                if (!string.IsNullOrEmpty(key))
                {
                    Query.Append(" and app_name like '%" + key + "%'");
                }
                if (group_id > 0)
                {
                    Query.Append(" and module_id=" + group_id);
                }
            }

            ddlGroup.SelectedValue = group_id.ToString();
            txtKey.Value = key.ToString();



            repList.DataSource = GetPageList(DbDataBaseEnum.KD_WECHATS, Query.ToString(), pageSize, page, "*", "id asc", ref totalCount);
            repList.DataBind();
            string pageUrl = string.Format("select_module.aspx?key={0}&group_id={1}&page=__id__", key, group_id);
            div_page.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
            if (totalCount<pageSize)
            {
                div_page.Visible = false;
            }
            
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect("select_module.aspx?key="+txtKey.Value.Trim()+"&group_id="+ddlGroup.SelectedValue);
        }

        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect("select_module.aspx?key=" + txtKey.Value.Trim() + "&group_id=" + ddlGroup.SelectedValue);
        }




    }
}