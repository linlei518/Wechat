using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;
using KDWechat.DAL;

namespace KDWechat.Web.keyworld
{
    public partial class select_pic : KDWechat.Web.UI.BasePage
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
                        style_name = "<th class=\"img\" >图片预览</th>";
                        page_name = "pic_add.aspx?m_id=44";
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
            material_search1.page_link = "select_pic.aspx?channel_id=" + channel_id;
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
        /// 绑定数据
        /// </summary>
        private void BindList()
        {
            StringBuilder Query = new StringBuilder();

            Query.Append(string.Format("select id,title,file_url as cover_img,group_id,create_time,remark as summary,hq_music_url,video_type from t_wx_media_materials where wx_id={0} and wx_og_id='{1}' and is_public={2} and channel_id={3} ", (is_public == 1 ? 0 : wx_id), (is_public == 1 ? "" : wx_og_id), is_public, channel_id));



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
            string pageUrl = string.Format("select_pic.aspx?key={0}&group_id={1}&beginDate={2}&is_pub={3}&page=__id__&endDate={4}&channel_id={5}", key, group_id, beginDate, is_pub, endDate, channel_id);
            div_page.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);

            if (is_public == 0)
            {
                if (CheckUserAuthorityBool("material_pic", RoleActionType.Add))
                {
                    lblPublic.Text = "<a href=\"../material/" + page_name + "\"  target=\"_blank\"   class=\"btn btn1\"><i class=\"add\"></i>新建" + channel_name + "</a> ";
                }
                
                
            }
            else
            {
                lblPublic.Text = "<a href=\"" + string.Format("select_pic.aspx?is_pub={0}&channel_id={1}", "0.0.0", channel_id) + "\" class=\"btn btn3\">返回</a>";
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
                int id = KDWechat.Common.Utils.StrToInt(e.CommandArgument.ToString(), 0);
                List<int> use_list = KDWechat.BLL.Chats.wx_media_materials.GetUseCount(id);
                if (use_list.Sum() > 0)
                {
                    string error = "该图片被";
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
                    JsHelper.Alert(this.Page, error,"true");
                    return;
                }
                else
                {
                    Literal lblTitle = e.Item.FindControl("lblTitle") as Literal;
                    KDWechat.BLL.Chats.wx_media_materials.Delete(id);
                    AddLog("删除图片素材：" + lblTitle.Text, LogType.删除);
                    JsHelper.Alert(this.Page, "删除成功");
                    Response.Redirect("select_pic.aspx?key=" + HttpUtility.UrlEncode(key) + "&group_id=" + group_id + "&page=" + page + "&is_pub=" + is_pub + "&beginDate=" + beginDate + "&endDate=" + endDate + "&channel_id=" + channel_id);
                }
            }
        }

        /*
        WebRequest webRequest = null;
        HttpWebResponse webResponse = null;
        Stream stream = null;
        FileStream fileStream = null;
     
        public string GetPcSize(object pic_url)
        {
            string str = "";
            if (pic_url.ToString().Contains("http"))
            {
                try
                {
                    webRequest = HttpWebRequest.Create(pic_url.ToString());   //打开图片地址
                    webResponse = (HttpWebResponse)webRequest.GetResponse();
                    stream = webResponse.GetResponseStream();

                    if (stream != null)
                    {
                        System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                        if (img != null)
                        {
                            str = img.Width + "*" + img.Height;
                        }

                    }
                }
                catch (Exception)
                {
                }
            }
            else
            {
                try
                {
                    if (File.Exists(Server.MapPath(pic_url.ToString()))) //系统本地图片
                    {

                        fileStream = new FileStream(Server.MapPath(pic_url.ToString()), FileMode.OpenOrCreate);
                        if (fileStream != null)
                        {
                            System.Drawing.Image img = System.Drawing.Image.FromStream(fileStream);
                            fileStream.Dispose();
                            fileStream.Close();
                            if (img != null)
                            {
                                str = img.Width + "*" + img.Height;
                            }
                        }
                        else
                        {
                            //str = "读取流失败";
                        }

                    }
                    else
                    {
                       // str = "不存在";
                    }
                }
                catch (Exception ex)
                {
                    str = "";
                }
            }




            return str;
        }

        */
    }
}