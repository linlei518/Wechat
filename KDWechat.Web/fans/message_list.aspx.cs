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

namespace KDWechat.Web.fans
{
    public partial class message_list : KDWechat.Web.UI.BasePage
    {
        protected string showDetail = string.Empty;
        

        /// 分组数据
        /// </summary>
        protected string groupList = string.Empty;
        /// <summary>
        /// 标签数据
        /// </summary>
        protected string tagList = string.Empty;
        /// <summary>
        /// 回复状态
        /// </summary>
        protected int replyStatus
        {
            get { return RequestHelper.GetQueryInt("replyStatus",-1); }
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
        public string beginDate
        {
            get { return RequestHelper.GetQueryString("beginDate"); }
        }


        /// <summary>
        ///结束时间
        /// </summary>
        public string endDate
        {
            get { return RequestHelper.GetQueryString("endDate"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckUserAuthority("message_list");
                
                CheckWXid();
                ShowPage();
            }
        }

        private void ShowPage()
        {

            StringBuilder Query = new StringBuilder();
            //Query.Append(string.Format("select *,(select headimgurl from t_wx_fans  where open_id=t_wx_fans_chats.open_id) as img_head,(select nick_name from t_wx_fans  where open_id=t_wx_fans_chats.open_id) as nick_name from t_wx_fans_chats where from_type=1 and wx_id=" + wx_id + " and open_id  in(select open_id from t_wx_fans where status=1 )"));

            Query.Append("select k.*,j.headimgurl as img_head,j.nick_name,j.last_interact_time,j.reply_state from (select max(id) as id,open_id from t_wx_fans_chats where from_type=1 group by open_id) s left join t_wx_fans_chats k on s.id=k.id left join t_wx_fans j on j.open_id=k.open_id where from_type=1 and k.wx_id=" + wx_id + " and k.open_id in(select open_id from t_wx_fans where status=1 ) and k.contents!='关注成功！'");
            if (replyStatus > -1)
            {
                switch (replyStatus)
                {
                    case 0:  //未回复
                        Query.Clear();
                        Query.Append("select k.*,j.headimgurl as img_head,j.nick_name,j.last_interact_time,j.reply_state from (select max(id) as id,open_id from t_wx_fans_chats group by open_id) s left join t_wx_fans_chats k on s.id=k.id left join t_wx_fans j on j.open_id=k.open_id where from_type=1 and k.wx_id=" + wx_id + " and k.open_id in(select open_id from t_wx_fans where status=1  and DATEDIFF(hh, last_interact_time, getdate())<48) and k.contents!='关注成功！'");
                        break;
                    case 1:  //已回复
                        Query.Clear();
                        Query.Append("select k.*,j.headimgurl as img_head,j.nick_name,j.last_interact_time,j.reply_state from (select max(id) as id,open_id from t_wx_fans_chats group by open_id) s left join t_wx_fans_chats k on s.id=k.id left join t_wx_fans j on j.open_id=k.open_id where from_type=2 and k.wx_id=" + wx_id + " and k.open_id in(select open_id from t_wx_fans where status=1 )  and k.contents!='关注成功！'");
                        break;
                    case 2:  //已过期
                        Query.Clear();
                        Query.Append("select k.*,j.headimgurl as img_head,j.nick_name,j.last_interact_time,j.reply_state from (select max(id) as id,open_id from t_wx_fans_chats group by open_id) s left join t_wx_fans_chats k on s.id=k.id left join t_wx_fans j on j.open_id=k.open_id where from_type=1 and k.wx_id=" + wx_id + " and k.open_id in(select open_id from t_wx_fans where status=1  and DATEDIFF(hh, last_interact_time, getdate())>48) and k.contents!='关注成功！'");
                        break;
                }

            }
            txtKey.Value = key;
            txtbegin_date.Text = beginDate;
            txtend_date.Text = endDate;

            if (beginDate.Trim() != "" && endDate.Trim() != "")
            {
                Query.Append(" and convert(varchar(10),k.create_time,120) >= '" + beginDate + "' and convert(varchar(10),k.create_time,120)<='" + endDate + "'");
                txt_date_show.Value = beginDate + " — " + endDate;
            }
            else if (beginDate.Trim() != "")
            {
                Query.Append(" and k.create_time between '" + beginDate + "'  and getdate()");
                txt_date_show.Value = beginDate;
            }
            else if (endDate.Trim() != "")
            {
                Query.Append(" and convert(varchar(10),k.create_time) <='" + endDate + "' ");
                txt_date_show.Value = endDate;
            }
            if (key.Trim().Length>0)
            {
                Query.Append(" and k.contents like '%" + key + "%'");
            }
            ddlReplyStatus.SelectedValue = replyStatus.ToString();


            #region 绑定聊天列表

            repList.DataSource = GetPageList(DbDataBaseEnum.KD_LOGS, Query.ToString(), pageSize, page, "*", "create_time desc", ref totalCount);
            repList.DataBind();
            string pageUrl = string.Format("message_list.aspx?page=__id__&m_id={0}&key={1}&beginDate={2}&endDate={3}", m_id, key, beginDate,endDate);
            div_page.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
            if (totalCount<pageSize)
            {
                div_page.Visible = false;
            }
            #endregion


        }

     

        protected string GetContents(object msgtype, object contents, object media_id)
        {
            string str = "";
            switch (Utils.ObjToInt(msgtype, 0))
            {
                case (int)msg_type.文本:
                    str = "<div class=\"text\"><p>" + Utils.ChangeToEditorEmotion(contents.ToString()) + "</p></div>";
                    break;
                case (int)msg_type.图片:


                    var src = "/upload/wx_img/"+ media_id+".jpg";

                    str = "<div class=\"text\"><p><a target=\"_blank\" href='" + src + "'><img src='" + src  + "' width='200' height='210' /></a></p></div>";
                    break;
                case (int)msg_type.语音:
                    if (contents.ToString().Trim().Length > 0)
                    {
                        str = "<div class=\"text\"><p><i class=\"sound\"></i>" + contents + "</p></div>";
                    }
                    else
                    {
                        str = "<div class=\"text\"><p><a href=\"down_file.aspx?media_id=" + media_id + "\" target='_blank'><i class=\"sound\"></i>语音消息下载</a></p></div>";
                    }

                    break;
                case (int)msg_type.视频:
                    str = "<div class=\"text\"><p><a href=\"down_file.aspx?media_id=" + media_id + "\" target='_blank'><i class=\"video\"></i>视频消息下载</a></p></div>";
                    break;
            }

            return str;
        }

        protected string GetReplyStatus(object create_time, object openId)
        {
            string str = "";
            TimeSpan ts = DateTime.Now - Common.Utils.StrToDateTime(create_time.ToString(),DateTime.Now.AddDays(-7));
            int hours = ts.Hours;
            hours += ts.Days * 24;
            if (hours < 48)
            {
                str = "<a href=\"javascript:bombbox.openBox('reply_fans.aspx?openId=" + openId + "');\" class=\"link\"><i class=\"navMessage\"></i>回复</a>";
            }
            
            return str;
                 
        }

        protected string GetState(object last_time,object reply_state)
        {
            var state = BLL.Users.wx_fans.GetFansChatStatus(last_time, reply_state);

            string space = "&nbsp; &nbsp;";
            var stringtoReturn = "暂无";
            switch (state)
            {
                case FansChatsTypeNew.未回复:
                    stringtoReturn = space + "<em>" + SaleChatsMode.未回复.ToString() + "</em>";
                    break;
                case FansChatsTypeNew.已过期:
                    stringtoReturn = space + SaleChatsMode.已过期.ToString();
                    break;
                case FansChatsTypeNew.已回复:
                    stringtoReturn = space + "<span>" + SaleChatsMode.已回复.ToString() + "</span>";
                    break;
                case FansChatsTypeNew.未回复已过期:
                    stringtoReturn = space + "<em>" + SaleChatsMode.未回复已过期.ToString() + "</em>";
                    break;
                default:
                    stringtoReturn = space+stringtoReturn;
                    break;
            }
            return stringtoReturn;
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string pageUrl = string.Format("message_list.aspx?key={0}&beginDate={1}&endDate={2}&m_id={3}&replyStatus={4}", HttpUtility.UrlEncode(Common.Utils.Filter(txtKey.Value.Trim())), Common.Utils.Filter(txtbegin_date.Text.Trim()), Common.Utils.Filter(txtend_date.Text.Trim()), m_id, ddlReplyStatus.SelectedValue);
             
            Response.Redirect(pageUrl);
        }

        protected void ddlReplyStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pageUrl = string.Format("message_list.aspx?key={0}&beginDate={1}&endDate={2}&m_id={3}&replyStatus={4}", HttpUtility.UrlEncode(Common.Utils.Filter(txtKey.Value.Trim())), Common.Utils.Filter(txtbegin_date.Text.Trim()), Common.Utils.Filter(txtend_date.Text.Trim()), m_id,ddlReplyStatus.SelectedValue);

            Response.Redirect(pageUrl);
        }


        private bool RemoteFileExists(string fileUrl)
        {
            bool result = false;//下载结果
            WebResponse response = null;
            try
            {
                WebRequest req = WebRequest.Create(fileUrl);

                response = req.GetResponse();

                result = true;

            }
            catch (Exception ex)
            {
                result = false;
            }
            finally
            {
                response?.Close();
            }
            return result;
        }





    }
}