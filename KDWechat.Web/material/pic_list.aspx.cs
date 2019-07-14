﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;

namespace KDWechat.Web.material
{
    public partial class pic_list : KDWechat.Web.UI.BasePage
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
        /// 状态
        /// </summary>
        protected int status
        {
            get { return RequestHelper.GetQueryInt("status", -1); }
        }


      
        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
           // pageSize = 5;
            if (!IsPostBack)
            {
                //判断权限
                CheckUserAuthority((is_public == 1 ? "material_pic_public" : "material_pic"));
                //.......
 
                BindList();
            }
        }

      

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindList()
        {
            StringBuilder Query = new StringBuilder();
            if (is_public==1)
            {
                Query.Append(string.Format("select id,title,file_url,group_id,status,create_time from t_wx_media_materials where   is_public=1 and channel_id={0} ",  (int)media_type.素材图片库));
            }
            else
            {
                Query.Append(string.Format("select id,title,file_url,group_id,status,create_time from t_wx_media_materials where wx_id={0} and channel_id={1} ", wx_id, (int)media_type.素材图片库));
            }

            if (only_op_self == 1)
            {
                Query.Append(" and u_id=" + u_id);
            }
            
            if (group_id > -1)
            {
                Query.Append(" and group_id=" + group_id);
            }
            if (status > -1)
            {
                Query.Append(" and status=" + status);
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

            if (!string.IsNullOrEmpty(key))
            {
                Query.Append(" and title like '%" + key + "%'");
            }

            repList.DataSource = GetPageList(DbDataBaseEnum.KD_WECHATS, Query.ToString(), pageSize, page, "*", "id desc", ref totalCount);
            repList.DataBind();
            string pageUrl = string.Format("pic_list.aspx?key={0}&group_id={1}&status={2}&is_pub={3}&page=__id__&m_id={4}", HttpUtility.UrlEncode(key), group_id, status, is_pub,m_id);
            div_page.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
            if (totalCount < pageSize)
            {
                div_page.Visible = false;
            }
        }
        



        protected void repList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Literal lblTitle = e.Item.FindControl("lblTitle") as Literal;
            if (e.CommandName == "status")
            {
                int id = KDWechat.Common.Utils.StrToInt(e.CommandArgument.ToString(), 0);
                int status = 0;
                if (((LinkButton)e.CommandSource).Text == "启用")
                {
                    status = 1;
                }
                KDWechat.BLL.Chats.wx_media_materials.UpdateStatus(id, status);
                AddLog("更改图片素材状态：" + lblTitle.Text + " 为" + (status == 1 ? "启用" : "禁用"), LogType.修改);
                Response.Redirect("pic_list.aspx?key=" + HttpUtility.UrlEncode(key) + "&status=" + this.status + "&group_id=" + group_id + "&page=" + page + "&is_pub=" + is_pub + "&m_id=" + m_id);
            }
            else if (e.CommandName == "del")
            {
                int id = KDWechat.Common.Utils.StrToInt(e.CommandArgument.ToString(), 0);
                List<int> use_list = KDWechat.BLL.Chats.wx_media_materials.GetUseCount(id);
                if (use_list.Sum()>0)
                {
                    string error = "该图片被";
                    if (use_list[0]>0)
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
                    error = error.Substring(0, error.Length - 1)+"使用，您不能删除。";
                    JsHelper.Alert(Page, error,"true");
                    return;
                }
                else
                {
                    if (KDWechat.BLL.Chats.wx_media_materials.Delete(id)) {
                        try
                        {
                            string img = (e.Item.FindControl("hf_img") as HiddenField).Value;
                            if (File.Exists(Server.MapPath(img)))
                            {
                                File.Delete(Server.MapPath(img));
                            }
                        }
                        catch (Exception)
                        {
                        }
                        AddLog("删除素材图片库：" + lblTitle.Text, LogType.删除);
                        JsHelper.AlertAndRedirect("删除成功", "pic_list.aspx?key=" + HttpUtility.UrlEncode(key) + "&status=" + this.status + "&group_id=" + group_id + "&page=" + page + "&is_pub=" + is_pub + "&m_id=" + m_id);
                    }
                    else
                    {
                        JsHelper.AlertAndRedirect("删除失败", "pic_list.aspx?key=" + HttpUtility.UrlEncode(key) + "&status=" + this.status + "&group_id=" + group_id + "&page=" + page + "&is_pub=" + is_pub + "&m_id=" + m_id);
                    }
                    

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

                    }
                }
                catch (Exception)
                {
                }
            }




            return str;
        }
         */

    }
}