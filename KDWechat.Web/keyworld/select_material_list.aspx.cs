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
    public partial class select_material_list : KDWechat.Web.UI.BasePage
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
            get { return RequestHelper.GetQueryInt("channel_id", 0); }
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
                        style_name = "<th style=\"width:180px\" class=\"img\" >图片预览</th>";
                        page_name = "pic_add.aspx?r=0.0.0&m_id=44&tef=1895623541";
                        break;
                    case 2:
                        _name = "语音";

                        page_name = "voice_add.aspx?r=0.0.0&m_id=45&tef=1895623541";
                        media_style_name = "<th style=\"width:40px\"  class=\"info info1\" >预览</th>";
                        break;
                    case 3:
                        _name = "视频";
                        page_name = "video_add.aspx?r=0.0.0&m_id=46&tef=1895623541";

                        media_style_name = "<th style=\"width:40px\"  class=\"info info1\" >预览</th>";
                        break;
                    case 4:
                        _name = "单图文消息";
                        style_name = "<th style=\"width:180px\"  class=\"img\" >图片预览</th>";
                        page_name = "news_add.aspx?r=0.0.0&m_id=47&tef=1895623541";
                        break;
                    case 5:
                        _name = "多图文消息";
                        style_name = "<th style=\"width:180px\"  class=\"img\" >图片预览</th>";
                        page_name = "multi-news_add.aspx?r=0.0.0&m_id=48&tef=1895623541";
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
            material_search1.page_link = "select_material_list.aspx?channel_id=" + channel_id;
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
        public string GetChildList(object parent_id)
        {
            string result = "";
            if (channel_id==5)
            {
                List<t_wx_news_materials> list = KDWechat.BLL.Chats.wx_news_materials.GetChildList(Common.Utils.ObjToInt(parent_id,0));
                if (list != null)
                {
                    foreach (t_wx_news_materials item in list)
                    {
                        result += "<div class=\"infoField\"><div class=\"img\"> <span><img class=\"cover\" src=\"" + item.cover_img + "\" > </span> </div><div class=\"title\"><h1>" + item.title + "</h1></div> </div>";
                    }

                }
            }
            return result;
        }

        public string GetMediaShow(object fileUrl, object video_type)
        {
            string str = "";
            if (channel_id==2)
            {
                str = "<td class=\"info info1\"><a class=\"audio\" href=\"#\" onClick=\"window.parent.audioControl(this,'"+fileUrl+"')\"  title=\"点击播放\">播放</a></td>";
            }
            else if (channel_id==3)
            {
                if (video_type.ToString()=="2")
                {
                    str = "<td class=\"info info1\"><a class=\"video\" href=\""+fileUrl+"\"  target=\"_blank\"  title=\"点击播放\">播放</a></td>";
                }
                else
                {
                    str = "<td class=\"info info1\"><a class=\"video\" href=\"#\" onClick=\"window.parent.video.play('" + fileUrl + "')\"  title=\"点击播放\">播放</a></td>";
                }
                
            }
            return str;
        }

        public string GetImageShow(object img)
        {
            string str = "";
            if (channel_id==1 || channel_id==4 || channel_id==5)
            {
                str = "<td class=\"img\" ><span><img class=\"cover\" src=\"" + img + "\"   style=\"width:161px; height:90px;\"></span></td>";
            }
            return str;
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindList()
        {
            StringBuilder Query = new StringBuilder();
            if (channel_id == 4 || channel_id == 5)
            {
                Query.Append(string.Format("select id,title,cover_img,group_id,create_time,summary, '' as hq_music_url, '' as video_type from t_wx_news_materials where wx_id={0} and wx_og_id='{1}' and is_public={2} and channel_id={3} and  par_id=0 ", (is_public == 1 ? 0 : wx_id), (is_public == 1 ? "" : wx_og_id), is_public, (channel_id == 4 ? 1 : channel_id == 5 ? 2 : 0)));
            }
            else  if (channel_id == 1 || channel_id == 2 || channel_id == 3)
            {
                Query.Append(string.Format("select id,title,file_url as cover_img,group_id,create_time,remark as summary,hq_music_url,video_type from t_wx_media_materials where wx_id={0} and wx_og_id='{1}' and is_public={2} and channel_id={3} ", (is_public == 1 ? 0 : wx_id), (is_public == 1 ? "" : wx_og_id), is_public, channel_id));

            }
           
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
            string pageUrl = string.Format("select_material_list.aspx?key={0}&group_id={1}&beginDate={2}&is_pub={3}&page=__id__&endDate={4}&channel_id={5}", key, group_id, beginDate, is_pub, endDate,channel_id);
            div_page.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);

            if (is_public==0)
            {
                //onclick=\"parent.bombbox.closeBox();\" target=\"_blank\"
                string code = "";
                if (channel_id==2)
                {
                    code = "material_voice";
                }
                else if (channel_id == 3)
                {
                    code = "material_video";
                }
                if (CheckUserAuthorityBool(code, RoleActionType.Add))
                {
                    lblPublic.Text = "<a href=\"../material/" + page_name + "\" target=\"_blank\"  class=\"btn btn1\"><i class=\"add\"></i>新建" + channel_name + "</a> ";
                }
               
            }
            else
            {
                lblPublic.Text = "<a href=\"" + string.Format("select_material_list.aspx?is_pub={0}&channel_id={1}", "0.0.0", channel_id) + "\" class=\"btn btn3\">返回</a>";
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

      


    }
}