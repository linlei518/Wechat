using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;
using KDWechat.DAL;

namespace KDWechat.Web.keyworld
{
    public partial class select_multi_news : KDWechat.Web.UI.BasePage
    {
        #region 页面属性
        protected string media_style_name = string.Empty;

        protected string style_name = string.Empty;

        protected string page_name = string.Empty;

        protected string channel_name = string.Empty;

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

        /// <summary>
        /// 图片 = 1, 语音 = 2, 视频 = 3, 单图文 = 4 ，多图文=5 
        /// </summary>
        protected int channel_id
        {
            get { return RequestHelper.GetQueryInt("channel_id", 1); }
        }

        protected string _channel_name
        {
            get
            {
                string _name = "";

                switch (channel_id)
                {
                    case 1:
                        _name = "图片";
                        page_name = "pic_add.aspx?r=0.0.0&m_id=44";
                        break;
                    case 2:
                        _name = "语音";

                        page_name = "voice_add.aspx?r=0.0.0&m_id=45";
                        media_style_name = "<th class=\"info info1\" >预览</th>";
                        break;
                    case 3:
                        _name = "视频";
                        page_name = "video_add.aspx?r=0.0.0&m_id=46";

                        media_style_name = "<th class=\"info info1\" >预览</th>";
                        break;
                    case 4:
                        _name = "单图文消息";
                        page_name = "news_add.aspx?r=0.0.0&m_id=47";
                        break;
                    case 5:
                        _name = "多图文消息";
                        page_name = "multi-news_add.aspx?r=0.0.0&m_id=48";
                        break;
                    default:
                        JsHelper.RegisterScriptBlock(this, "closeBox();parent.location.replace(parent.location);");
                        break;
                }
                return _name;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            //this.pageSize = 5;
            material_search1.page_link = "select_multi-news.aspx?channel_id=" + channel_id;
            material_search1.isshow_group = "1";
            if (!IsPostBack)
            {
                //判断权限

                //.......
                channel_name = _channel_name;


                BindList();
            }
        }

        /// <summary>
        /// 获取子级图文
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public string GetChildList(object parent_id, int channel_id)
        {
            string result = "";

            List<t_wx_news_materials> list = KDWechat.BLL.Chats.wx_news_materials.GetChildList(Common.Utils.ObjToInt(parent_id, 0));
            if (list != null)
            {
                foreach (t_wx_news_materials item in list)
                {
                    if (channel_id == 1)
                    {
                        result += "<div class=\"infoField\"><div class=\"img\"> <span><img class=\"cover\" src=\"" + item.cover_img + "\" > </span> </div><div class=\"title\"><h1>" + item.title + "</h1></div> </div>";
                    }
                    else
                    {
                        result += " <div class=\"info\">";
                        result += "<div class=\"img\"> <span><img src=\"" + item.cover_img + "\" alt=\"\">  </span></div>";
                        result += "<div class=\"title\"> <h1>" + item.title + "</h1> </div> </div>";
                    }

                }

            }

            return result;
        }



        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindList()
        {
            StringBuilder Query = new StringBuilder();

            Query.Append(string.Format("select id,title,cover_img,summary,group_id,status,create_time,multi_html,content_html from t_wx_news_materials where wx_id={0} and wx_og_id='{1}' and is_public={2} and channel_id={3} and par_id=0 ", (is_public == 1 ? 0 : wx_id), (is_public == 1 ? "" : wx_og_id), is_public, (channel_id == 4 ? 1 : channel_id == 5 ? 2 : 0)));



            if (!string.IsNullOrEmpty(key))
            {
                Query.Append(" and title like '%" + key + "%'");
            }
            if (group_id > -1)
            {
                Query.Append(" and group_id=" + group_id);
            }

            if (beginDate.Length > 0 && endDate.Length > 0)
            {
                Query.Append(" and convert(varchar(10),create_time,120)>= '" + material_search1.beginDate + "' and convert(varchar(10),create_time,120)<='" + material_search1.endDate + "'");
            }
            else if (beginDate.Length > 0)
            {
                Query.Append(" and create_time between '" + beginDate + "'  and getdate()");
            }
            else if (endDate.Length > 0)
            {
                Query.Append(" and convert(varchar(10),create_time) <='" + endDate + "' ");
            }

            repList.DataSource = GetPageList(DbDataBaseEnum.KD_WECHATS, Query.ToString(), pageSize, page, "*", "id desc", ref totalCount);
            repList.DataBind();
            string pageUrl = string.Format("select_multi-news.aspx?key={0}&group_id={1}&beginDate={2}&is_pub={3}&page=__id__&endDate={4}&channel_id={5}", key, group_id, beginDate, is_pub, endDate, channel_id);
            div_page.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);

            if (is_public == 0)
            {
                if (CheckUserAuthorityBool("material_multi_news", RoleActionType.Add))
                {
                    lblPublic.Text = "<a href=\"../material/" + page_name + "\"   target=\"_blank\"   class=\"btn btn1\"><i class=\"add\"></i>新建" + channel_name + "</a> ";
                }
                else
                {
                }
            }
            else
            {
                lblPublic.Text = "<a href=\"" + string.Format("select_multi-news.aspx?is_pub={0}&channel_id={1}", "0.0.0", channel_id) + "\" class=\"btn btn3\">返回</a>";
            }
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
                    JsHelper.Alert(Page, error, "true");
                    return;
                }
                else
                {
                    KDWechat.BLL.Chats.wx_news_materials.Delete(id, true);
                    AddLog("删除多图文素材：" + lblTitle.Text, LogType.删除);
                    JsHelper.Alert(this.Page, "删除成功");
                    Response.Redirect("select_multi-news.aspx?key=" + HttpUtility.UrlEncode(key) + "&group_id=" + group_id + "&page=" + page + "&is_pub=" + is_pub + "&beginDate=" + beginDate + "&endDate=" + endDate + "&channel_id=" + channel_id);
                }
            }
        }


    }
}