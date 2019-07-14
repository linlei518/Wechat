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
    public partial class select_materialList : KDWechat.Web.UI.BasePage
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
        protected string beginDate
        {
            get { return RequestHelper.GetQueryString("beginDate"); }
        }


        /// <summary>
        ///结束时间
        /// </summary>
        protected string endDate
        {
            get { return RequestHelper.GetQueryString("endDate"); }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {


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
            Query.Append(string.Format("select id,title,cover_img,group_id,status,push_Type,summary,contents,link_url,source_url,author,create_time,app_id,app_type_name,app_name,app_type_img,app_link,template_id ,(case when template_id>0 then (select title from t_wx_templates where id=template_id) else '' end ) as template_name, (case when template_id>0 then (select img_url from t_wx_templates where id=template_id) else '' end ) as template_img from t_wx_news_materials where wx_id={0}  and is_public={1} and channel_id=1 ", (is_public == 1 ? 0 : wx_id), is_public));
            if (only_op_self == 1)
            {
                Query.Append(" and u_id=" + u_id);
            }

            if (group_id > -1)
            {
                Query.Append(" and group_id=" + group_id);
            }

            if (beginDate.Length > 0 && endDate.Length > 0)
            {
                Query.Append(" and create_time between '" + beginDate + "' and '" + endDate + "'");
            }
            else if (beginDate.Length > 0)
            {
                Query.Append(" and create_time between '" + beginDate + "'  and getdate()");
            }
            else if (endDate.Length > 0)
            {
                Query.Append(" and convert(varchar(10),create_time) <='" + endDate + "' ");
            }

            if (!string.IsNullOrEmpty(key))
            {
                Query.Append(" and title like '%" + key + "%'");
            }
            repList.DataSource = GetPageList(DbDataBaseEnum.KD_WECHATS, Query.ToString(), pageSize, page, "*", "id desc", ref totalCount);
            repList.DataBind();
            string pageUrl = string.Format("select_materialList.aspx?key={0}&group_id={1}&beginDate={2}&is_pub={3}&page=__id__&endDate={4}", key, group_id, beginDate, is_pub, endDate);
            div_page.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
            if (totalCount < pageSize)
            {
                div_page.Visible = false;
            }
        }
        /// <summary>
        /// 获取分组名称
        /// </summary>
        /// <param name="group_id"></param>
        /// <returns></returns>
        public string GetGroupName(object group_id)
        {
            string name = KDWechat.BLL.Users.wx_group_tags.GetGroupName(KDWechat.Common.Utils.ObjToInt(group_id, 0));

            return name;
        }
        protected void repList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                Literal lblTitle = e.Item.FindControl("lblTitle") as Literal;
                int id = KDWechat.Common.Utils.StrToInt(e.CommandArgument.ToString(), 0);
                List<int> use_list = KDWechat.BLL.Chats.wx_news_materials.GetUseCount(id);
                if (use_list.Sum() > 0)
                {
                    string error = "该图文消息被";
                    if (use_list[0] > 0)
                    {
                        error += "“被添加自动回复”、";
                    }
                    if (use_list[1] > 0)
                    {
                        error += "“消息自动回复”、";
                    }
                    if (use_list[2] > 0)
                    {
                        error += "“关键词自动回复”、";
                    }
                    if (use_list[3] > 0)
                    {
                        error += "“自定义菜单”、";
                    }
                    error = error.Substring(0, error.Length - 1) + "使用，您不能删除。";
                    JsHelper.Alert(this.Page, error, "true");
                    return;
                }
                else
                {
                    KDWechat.BLL.Chats.wx_news_materials.Delete(id);
                    AddLog("删除单图文素材：" + lblTitle.Text, LogType.删除);
                    JsHelper.Alert(this.Page, "删除成功");
                    Response.Redirect("select_materialList.aspx?key=" + HttpUtility.UrlEncode(key) + "&group_id=" + group_id + "&page=" + page + "&is_pub=" + is_pub + "&beginDate=" + beginDate + "&endDate=" + endDate);
                }
            }
        }




    }
}