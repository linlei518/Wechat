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

namespace KDWechat.Web.setting
{
    public partial class template_pic_list : KDWechat.Web.UI.BasePage
    {
        #region 页面属性
        /// <summary>


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



        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            // pageSize = 5;
            if (!IsPostBack)
            {
                //判断权限
                CheckUserAuthority("template_material");

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
            Query.Append(string.Format("select id,title,file_url,group_id,status,create_time from t_wx_media_materials where wx_id={0} and wx_og_id='{1}' and is_public=0 and channel_id={2} ", wx_id, wx_og_id, (int)media_type.图文模板图片库));
            if (!string.IsNullOrEmpty(key))
            {
                Query.Append(" and title like '%" + key + "%'");
            }


            if (material_search1.beginDate.Length > 0 && material_search1.endDate.Length > 0)
            {
                Query.Append(" and convert(varchar(10),create_time,120)>= '" + material_search1.beginDate + "' and convert(varchar(10),create_time,120)<='" + material_search1.endDate + "'");
            }
            else if (material_search1.beginDate.Length > 0)
            {
                Query.Append(" and create_time between '" + material_search1.beginDate + "'  and getdate()");
            }
            else if (material_search1.endDate.Length > 0)
            {
                Query.Append(" and convert(varchar(10),create_time) <='" + material_search1.endDate + "' ");
            }
            repList.DataSource = GetPageList(DbDataBaseEnum.KD_WECHATS, Query.ToString(), pageSize, page, "*", "id desc", ref totalCount);
            repList.DataBind();
            string pageUrl = string.Format("template_pic_list.aspx?key={0}&page=__id__&m_id={1}", HttpUtility.UrlEncode(key), m_id);
            div_page.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
            if (totalCount < pageSize)
            {
                div_page.Visible = false;
            }
        }



        protected void repList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Literal lblTitle = e.Item.FindControl("lblTitle") as Literal;
            if (e.CommandName == "del")
            {
                int id = KDWechat.Common.Utils.StrToInt(e.CommandArgument.ToString(), 0);

                KDWechat.BLL.Chats.wx_media_materials.Delete(id);
                AddLog("删除模板图片素材：" + lblTitle.Text, LogType.删除);
                JsHelper.AlertAndRedirect("删除成功", "template_pic_list.aspx?key=" + HttpUtility.UrlEncode(key) + "&page=" + page + "&m_id=" + m_id);


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

                    }
                }
                catch (Exception)
                {
                }
            }




            return str;
        }
         * */

    }
}