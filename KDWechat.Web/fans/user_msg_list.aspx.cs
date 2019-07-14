using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;

namespace KDWechat.Web.fans
{
    public partial class user_msg_list : KDWechat.Web.UI.BasePage
    {
        protected string showDetail = string.Empty;
        /// <summary>
        /// 粉丝用户的openid
        /// </summary>
        protected string openId { get { return Common.RequestHelper.GetQueryString("openId"); } }

        /// 分组数据
        /// </summary>
        protected string groupList = string.Empty;
        /// <summary>
        /// 标签数据
        /// </summary>
        protected string tagList = string.Empty;
        /// <summary>
        /// 粉丝用户的id
        /// </summary>
        protected int id = 0;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckUserAuthority((u_type == 1 ? "wechat_fans_user_list_hq" : "wechat_fans_user_list"));

                WriteReturnPage(hfReturnUrl, "user_list.aspx?m_id=" + m_id);

                KDWechat.DAL.t_wx_fans model = KDWechat.BLL.Users.wx_fans.GetFansByID(openId);
                if (model != null)
                {
                    ShowPage(model);

                }
                else
                {
                    Response.Redirect(hfReturnUrl.Value);
                }
            }
        }

        private void ShowPage(DAL.t_wx_fans model)
        {
            

            #region 绑定基础数据
            showDetail = "user_detail.aspx?id=" + model.id + "&m_id=18";
            id = model.id;

            string member_name = KDWechat.BLL.Users.wx_fans.GetMemberName(model.unionid);
            if (!string.IsNullOrEmpty(member_name))
            {
                lblMemberName.Text += " 注册会员 ：" + member_name;
            }

            img_head.Src = model.headimgurl;
            lblNickName.Text = model.nick_name;
           
            #endregion

            #region 加载已选的分组和标签
            if (model.group_id > 0)
            {
                lblGroupName.Text = KDWechat.BLL.Users.wx_group_tags.GetGroupName(model.group_id);
            }
            else
            {
                lblGroupName.Text = "默认分组";
            }

            List<string> list_tags = KDWechat.BLL.Users.wx_fans_tags.GetTagListByFansid(model.guid);
            if (list_tags != null)
            {
                foreach (string tag in list_tags)
                {
                    lblFansTags.Text += "<span class=\"tag\">" + tag + "</span>";
                }
            }

            #endregion

            #region 计算粉丝的聊天状态
            DateTime lastTime = model.last_interact_time ?? DateTime.Now.AddDays(-7);
            FansChatsTypeNew fcs = KDWechat.BLL.Users.wx_fans.GetFansChatStatus(model.last_interact_time, model.reply_state);
            lblReplyStatus.Text = fcs.ToString();
            switch (fcs)
            {
                case FansChatsTypeNew.暂无:
                    break;
                case FansChatsTypeNew.未回复:
                case FansChatsTypeNew.已回复:
                    lblLastMsgTime.Text = "<span>最后互动 ：" + lastTime.ToString("yyyy.MM/dd HH:mm") + " </span>";
                    lblLastMsgTime.Text += "<a href=\"javascript:bombbox.openBox('reply_fans.aspx?openId=" + openId + "');\" class=\"link\"><i class=\"navMessage\"></i>回复</a>";
                    break;
                case FansChatsTypeNew.未回复已过期:
                case FansChatsTypeNew.已过期:
                    lblLastMsgTime.Text = "<em>已超过回复时限</em>";
                    break;
                default:
                    break;
            }
          
            #endregion

            #region 加载所有标签和分组
            List<KDWechat.DAL.t_wx_group_tags> list_group = KDWechat.BLL.Users.wx_group_tags.GetListByChannelId((int)channel_idType.关注用户分组, model.wx_id);
            groupList += "[{id:'0',name:'默认分组'}";
            if (list_group != null)
            {
                if (list_group.Count > 0)
                {
                    groupList += ",";
                    int i = 1;
                    foreach (var item in list_group)
                    {
                        groupList += "{id:'" + item.id + "',name:'" + item.title + "'}";
                        if (i < list_group.Count)
                        {
                            groupList += ",";
                        }
                        i++;
                    }

                }

            }
            groupList += "]";

            List<KDWechat.DAL.t_wx_group_tags> list_tag = KDWechat.BLL.Users.wx_group_tags.GetListByChannelId((int)channel_idType.关注用户标签, model.wx_id);
            if (list_tag != null)
            {
                if (list_tag.Count > 0)
                {
                    tagList += "[";
                    int i = 1;
                    foreach (var item in list_tag)
                    {
                        tagList += "{id:'" + item.id + "',name:'" + item.title + "'}";
                        if (i < list_tag.Count)
                        {
                            tagList += ",";
                        }
                        i++;
                    }
                    tagList += "]";
                }

            }
            #endregion

            #region 绑定聊天列表
            StringBuilder Query = new StringBuilder();
            Query.Append(string.Format("select * from t_wx_fans_chats where open_id='{0}'", openId));
             
            repList.DataSource = GetPageList(DbDataBaseEnum.KD_LOGS, Query.ToString(), pageSize, page, "*", "create_time desc", ref totalCount);
            repList.DataBind();
            string pageUrl = string.Format("user_msg_list.aspx?openId={0}&page=__id__&m_id={1}", openId,  m_id);
            div_page.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
            #endregion


        }

        protected string GetInfoAndImg(object open_id, object from_type, object create_time)
        {
            string str = "";
            if (from_type.ToString()=="2") //系统
            {
                str = "  <div class=\"img\"> <span> <img src=\"" + wx_head_pic + "\"> </span> </div><div class=\"info\"> <div class=\"time\">" + create_time.ToString() + "</div> <div class=\"name\">回复</div></div>";
            }
            else
            {
                
                str = "  <div class=\"img\"> <span> <img src=\""+img_head.Src+"\"> </span> </div><div class=\"info\"> <div class=\"time\">" + create_time.ToString() + "</div> <div class=\"name\">"+lblNickName.Text+"</div></div>";
            }

            return str;
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
                    var src = "/upload/wx_img/" + media_id + ".jpg";
                    str = "<div class=\"text\"><p><a target=\"_blank\" href='" + src + "'><img src='" + src +  "' width='200' height='210' /></a></p></div>";
                    break;
                case (int)msg_type.语音:
                    if (contents.ToString().Trim().Length>0)
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
    }
}