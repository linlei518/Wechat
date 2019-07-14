using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;

namespace KDWechat.Web
{
    public partial class select_pic : KDWechat.Web.UI.BasePage
    {
        #region 页面属性

        protected string type { get { return RequestHelper.GetQueryString("type"); } }

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


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            //this.pageSize = 5;
            material_search1.page_link = "select_pic.aspx?channel_id=" + channel_id;
            material_search1.isshow_group = "1";
            if (!IsPostBack)
            {

                BindList();
            }
        }





        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindList()
        {
            StringBuilder Query = new StringBuilder();

            if (channel_id==(int)media_type.项目模块)
            {
                Query.Append(string.Format("select id,title,file_url as cover_img,group_id,create_time,remark as summary,hq_music_url,video_type from t_wx_media_materials where  channel_id={0} ", channel_id));
            }
            else
            {
                if (is_public == 1)
                {
                    Query.Append(string.Format("select id,title,file_url as cover_img,group_id,create_time,remark as summary,hq_music_url,video_type from t_wx_media_materials where  is_public=1 and channel_id={0} ", channel_id));
                }
                else
                {
                    if (channel_id==(int)media_type.公众号头像)
                    {
                        if (u_type==1 || u_type==4)
                        {
                            Query.Append(string.Format("select id,title,file_url as cover_img,group_id,create_time,remark as summary,hq_music_url,video_type from t_wx_media_materials where wx_id={0}   and channel_id={1} ", 0, channel_id));
                        }
                        else
                        {
                            Query.Append(string.Format("select id,title,file_url as cover_img,group_id,create_time,remark as summary,hq_music_url,video_type from t_wx_media_materials where wx_id={0}   and channel_id={1} ", wx_id, channel_id));
                        }
                    }
                    else
                    {
                        Query.Append(string.Format("select id,title,file_url as cover_img,group_id,create_time,remark as summary,hq_music_url,video_type from t_wx_media_materials where wx_id={0}   and channel_id={1} ", wx_id, channel_id));
                    }
                    
                }
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
            string pageUrl = string.Format("select_pic.aspx?key={0}&group_id={1}&beginDate={2}&is_pub={3}&page=__id__&endDate={4}&channel_id={5}&type={6}&hf={7}&ul={8}&img={9}", key, group_id, beginDate, is_pub, endDate, channel_id, type, KDWechat.Common.RequestHelper.GetQueryString("hf"), KDWechat.Common.RequestHelper.GetQueryString("ul"), RequestHelper.GetQueryString("img"));
            div_page.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);


            if (totalCount < pageSize)
            {
                div_page.Visible = false;
            }
        }


        protected void repList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                int id = KDWechat.Common.Utils.StrToInt(e.CommandArgument.ToString(), 0);

                Literal lblTitle = e.Item.FindControl("lblTitle") as Literal;
               bool isc=  KDWechat.BLL.Chats.wx_media_materials.Delete(id);
               if (isc)
               {
                   AddLog("删除" + ((media_type)channel_id).ToString() + "的图片：" + lblTitle.Text, LogType.删除);
                   try
                   {
                       //删除本地图片
                       string file = (e.Item.FindControl("hffile") as HiddenField).Value;
                       if (File.Exists(Server.MapPath(file)))
                       {
                           File.Delete(Server.MapPath(file));
                       }
                   }
                   catch (Exception)
                   {
                   }
                   JsHelper.AlertAndRedirect("删除成功", "select_pic.aspx?key=" + HttpUtility.UrlEncode(key) + "&group_id=" + group_id + "&page=" + page + "&is_pub=" + is_pub + "&beginDate=" + beginDate + "&endDate=" + endDate + "&channel_id=" + channel_id + "&type=" + type + "&hf=" + KDWechat.Common.RequestHelper.GetQueryString("hf") + "&ul=" + KDWechat.Common.RequestHelper.GetQueryString("ul"));
               }
               else
               {
                   JsHelper.AlertAndRedirect("删除失败", "select_pic.aspx?key=" + HttpUtility.UrlEncode(key) + "&group_id=" + group_id + "&page=" + page + "&is_pub=" + is_pub + "&beginDate=" + beginDate + "&endDate=" + endDate + "&channel_id=" + channel_id + "&type=" + type + "&hf=" + KDWechat.Common.RequestHelper.GetQueryString("hf") + "&ul=" + KDWechat.Common.RequestHelper.GetQueryString("ul"));
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