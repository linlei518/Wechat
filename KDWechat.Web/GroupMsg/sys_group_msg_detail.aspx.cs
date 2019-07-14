using KDWechat.BLL.Chats;
using KDWechat.Common;
using KDWechat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.GroupMsg
{
    public partial class sys_group_msg_detail : Web.UI.BasePage
    {
        protected int id
        {
            get
            {
                return RequestHelper.GetQueryInt("id", 0);
            }
        }
        protected string loadjs = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckUserAuthority("sys_group_list");
                InitData();
            }
        }

        private void InitData()
        {
            if (id != 0)
            {
                var model = wx_group_msgs.GetGroupMsgByID(id);//取群发信息

                if (model != null)
                {
                    string[] openIDs = model.openIDs.Split(',');
                    //var fanList = BLL.Users.wx_fans.GetFansListByOpenIDs(openIDs);
                    //DataRepeater.Visible = true;
                    //DataRepeater.DataSource = fanList;
                    //DataRepeater.DataBind();
                    txtTitle.Value = model.title;
                    switch (model.msg_type)
                    {
                        case (int)msg_type.文本:
                            txtContents.Value = model.contents;
                            break;
                        case (int)msg_type.图片:
                        case (int)msg_type.语音:
                        case (int)msg_type.视频:
                            int _channel_id = 1;
                            if (model.msg_type == (int)msg_type.语音)
                                _channel_id = 2;
                            else if (model.msg_type == (int)msg_type.视频)
                                _channel_id = 3;
                            t_wx_media_materials m = KDWechat.BLL.Chats.wx_media_materials.GetMediaMaterialByID((int)model.source_id);
                            if (m != null)
                            {
                                loadjs = "selectResult(" + _channel_id + ", " + model.source_id + ",'" + m.file_url + "','" + m.title + "','" + m.create_time.ToString("yyyy-MM-dd") + "','" + (Common.Utils.DropHTML(m.remark, 40)) + "',0,'','" + (_channel_id == 3 ? m.hq_music_url : "") + "'," + (_channel_id == 3 ? (int)m.video_type : 0) + ");";

                            }
                            break;

                        case (int)msg_type.单图文:
                            t_wx_news_materials m2 = KDWechat.BLL.Chats.wx_news_materials.GetModel((int)model.source_id);
                            if (m2 != null)
                            {
                                loadjs = "selectResult(4, " + model.source_id + ",'" + m2.cover_img + "','" + m2.title + "','" + m2.create_time.ToString("yyyy-MM-dd") + "','" + (Common.Utils.DropHTML(m2.summary, 40)) + "',0,'','',0);";
                            }
                            break;
                        case (int)msg_type.多图文:
                            t_wx_news_materials multi = KDWechat.BLL.Chats.wx_news_materials.GetModel((int)model.source_id);
                            if (multi != null)
                            {
                                //取出子级图文
                                string child_str = string.Empty;
                                List<t_wx_news_materials> list = KDWechat.BLL.Chats.wx_news_materials.GetChildList(multi.id);
                                if (list != null)
                                {
                                    foreach (t_wx_news_materials item in list)
                                    {
                                        child_str += "<div class=\"infoField\"><div class=\"img\"> <span><img src=\"" + item.cover_img + "\" > </span> </div><div class=\"title\"><h1>" + item.title + "</h1></div> </div>";
                                    }

                                }
                                loadjs = "selectResult(5, " + model.source_id + ",'" + multi.cover_img + "','" + multi.title + "','" + multi.create_time.ToString("yyyy-MM-dd") + "','" + (Common.Utils.DropHTML(multi.summary, 40)) + "',0,'" + child_str + "','',0);";
                            }
                            break;
                    }
                }
            }
        }

    }
}