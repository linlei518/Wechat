using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;
using KDWechat.DAL;
using System.Text;
using System.Data;

namespace KDWechat.Web.keyworld
{
    public partial class auto_reply : KDWechat.Web.UI.BasePage
    {
        protected string loadjs = string.Empty;
        protected int channel_id
        {
            get
            {
                int _channel_id = 0;
                string type = RequestHelper.GetQueryString("t");
                switch (type)
                {
                    case "1.1.1":
                        _channel_id = (int)AutoReply.关注时;
                        break;
                    case "0.0.0":
                        _channel_id = (int)AutoReply.无匹配时;
                        break;
                }
                return _channel_id;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (channel_id == 1 || channel_id == 2)
                {

                    string code = Common.Utils.Number(4, true);
                    char[] list = code.ToCharArray();
                    _f.Value = list[0].ToString();
                    _u.Value = list[1].ToString();
                    c_.Value = list[2].ToString();
                    k_.Value = list[3].ToString();
                    Common.Utils.WriteCookie("zdhf" + channel_id, code);


                    if (channel_id == 0)
                    {
                        JsHelper.AlertAndParentUrl("请勿非法访问", "../Account/region_wxlist.aspx?m_id=59");
                        return;
                    }
                    if (channel_id == 1)
                    {
                        CheckUserAuthority("auto_reply_follow");
                    }
                    else if (channel_id == 2)
                    {
                        CheckUserAuthority("auto_reply_nokey");
                    }
                    displayBind();
                }
            }
        }




        private void displayBind()
        {
            t_wx_basic_reply model = KDWechat.BLL.Chats.wx_basic_reply.GetModel(wx_id, wx_og_id, channel_id);
            btnClare.CssClass = "btn btn2";
            btnClare.Visible = isDelete;
            if (model != null)
            {

                btnSubmit.Visible = isEdit;


                btnClare.Enabled = true;
                btnClare.CssClass = "btn btn3";
                switch (model.reply_type)
                {
                    case (int)msg_type.文本:
                        txtContents.Value = model.contents;
                        break;
                    case (int)msg_type.图片:
                    case (int)msg_type.语音:
                    case (int)msg_type.视频:
                        int _channel_id = 1;
                        if (model.reply_type == (int)msg_type.语音)
                            _channel_id = 2;
                        else if (model.reply_type == (int)msg_type.视频)
                            _channel_id = 3;
                        t_wx_media_materials m = KDWechat.BLL.Chats.wx_media_materials.GetMediaMaterialByID((int)model.source_id);
                        if (m != null)
                        {
                            loadjs = "selectResult(" + _channel_id + ", " + model.source_id + ",'" + m.file_url + "','" + m.title + "','" + m.create_time.ToString("yyyy-MM-dd") + "','" + (Common.Utils.DropHTML(m.remark, 140).Replace("\n", "").Trim()) + "',0,'','" + (_channel_id == 3 ? m.hq_music_url : "") + "'," + (_channel_id == 3 ? (int)m.video_type : 0) + ");";

                        }
                        break;

                    case (int)msg_type.单图文:
                        t_wx_news_materials m2 = KDWechat.BLL.Chats.wx_news_materials.GetModel((int)model.source_id);
                        if (m2 != null)
                        {
                            loadjs = "selectResult(4, " + model.source_id + ",'" + m2.cover_img + "','" + m2.title + "','" + m2.create_time.ToString("yyyy-MM-dd") + "','" + m2.summary.Replace("\n", "").Trim() + "',0,'','',0);";
                        }
                        break;
                    case (int)msg_type.多图文:
                        t_wx_news_materials multi = KDWechat.BLL.Chats.wx_news_materials.GetModel((int)model.source_id);
                        if (multi != null)
                        {
                            //取出子级图文
                            //string child_str = string.Empty;
                            //List<t_wx_news_materials> list = KDWechat.BLL.Chats.wx_news_materials.GetChildList(multi.id);
                            //if (list != null)
                            //{
                            //    foreach (t_wx_news_materials item in list)
                            //    {
                            //        child_str += "<div class=\"infoField\"><div class=\"img\"> <span><img src=\"" + item.cover_img + "\" > </span> </div><div class=\"title\"><h1>" + item.title + "</h1></div> </div>";
                            //    }

                            //}
                            loadjs = "selectResult(5, " + model.source_id + ",'" + multi.cover_img + "','" + multi.title + "','" + multi.create_time.ToString("yyyy-MM-dd") + "','" + multi.summary.Replace("\n", "").Trim() + "',0,'" + multi.multi_html.Replace("\n", "").Trim() + "','',0);";
                        }
                        break;
                    case (int)msg_type.模块:
                        DataTable dt = BLL.Chats.module_wechat.GetListByQuery("select id,module_id,wx_id,app_name,app_img_url,app_remark,(select title from t_modules where id=module_id) as module_name  from t_module_wechat where id=" + (int)model.source_id);
                        if (dt != null)
                        {
                            loadjs = "selectResult(8, " + model.source_id + ",'" + dt.Rows[0]["app_img_url"] + "','" + "【" + dt.Rows[0]["module_name"] + "】" + dt.Rows[0]["app_name"] + "','','" + (Common.Utils.DropHTML(dt.Rows[0]["app_remark"].ToString(), 140).Replace("\n", "").Trim()) + "',0,'','',0);";
                        }
                        break;
                    case (int)msg_type.多客服:
                        loadjs = "multiCustomerClick(7);";//to do
                        break;

                }


            }
            else
            {
                btnSubmit.Visible = isAdd;
            }
        }

        //提交
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (channel_id == 1 || channel_id == 2)
            {
                string code = _f.Value + _u.Value + c_.Value + k_.Value;
                string code2 = Common.Utils.GetCookie("zdhf" + channel_id);
                if (code == code2)
                {
                    string log_title = "设置" + (channel_id == 1 ? "被添加" : "消息") + "自动回复信息为";
                    t_wx_basic_reply model = KDWechat.BLL.Chats.wx_basic_reply.GetModel(wx_id, wx_og_id, channel_id);
                    bool isAdd = false;
                    if (model == null)
                    {
                        model = new t_wx_basic_reply();
                        isAdd = true;
                    }
                    model.contents = "";
                    switch (hftype.Value)
                    {
                        case "0":
                            model.reply_type = (int)msg_type.文本;
                            model.contents = Common.Utils.DropHTMLOnly(txtContents.Value.Trim());
                            log_title += "文本类型，内容为：" + model.contents;
                            break;
                        case "1":
                            model.reply_type = (int)msg_type.图片;
                            log_title += "图片类型，素材id为" + hfid.Value + " , 素材标题为：" + hflogtitle.Value;
                            break;
                        case "2":
                            model.reply_type = (int)msg_type.语音;
                            log_title += "语音类型，素材id为" + hfid.Value + " , 素材标题为：" + hflogtitle.Value;
                            break;
                        case "3":
                            model.reply_type = (int)msg_type.视频;
                            log_title += "视频类型，素材id为" + hfid.Value + " , 素材标题为：" + hflogtitle.Value;
                            break;
                        case "4":
                            model.reply_type = (int)msg_type.单图文;
                            log_title += "单图文类型，素材id为" + hfid.Value + " , 素材标题为：" + hflogtitle.Value;
                            break;
                        case "5":
                            model.reply_type = (int)msg_type.多图文;
                            log_title += "多图文类型，素材id为" + hfid.Value + " , 素材标题为：" + hflogtitle.Value;
                            break;
                        case "8":
                            model.reply_type = (int)msg_type.模块;
                            log_title += "模块类型，素材id为" + hfid.Value + " , 素材标题为：" + hflogtitle.Value;
                            break;
                        case "10":
                            model.reply_type = (int)msg_type.多客服;
                            log_title += "多客服类型";
                            break;
                    }
                    model.source_id = Common.Utils.StrToInt(hfid.Value, 0);
                    if (!isAdd)
                    {
                        Common.Utils.WriteCookie("zdhf" + channel_id, "");
                        model = KDWechat.BLL.Chats.wx_basic_reply.Update(model);
                        if (model != null)
                        {
                            AddLog(log_title, LogType.修改);
                            JsHelper.AlertAndParentUrl("保存成功", "auto_reply.aspx?m_id=" + m_id + "&t=" + RequestHelper.GetQueryString("t"));
                        }
                        else
                        {
                            JsHelper.AlertAndParentUrl("保存失败，请稍后再试", "auto_reply.aspx?m_id=" + m_id + "&t=" + RequestHelper.GetQueryString("t"));
                        }
                    }
                    else
                    {
                        Common.Utils.WriteCookie("zdhf" + channel_id, "");
                        model.channel_id = channel_id;
                        model.status = 1;
                        model.u_id = u_id;
                        model.wx_id = wx_id;
                        model.wx_og_id = wx_og_id;
                        model = KDWechat.BLL.Chats.wx_basic_reply.Add(model);
                        if (model != null)
                        {
                            AddLog(log_title, LogType.添加);
                            JsHelper.AlertAndParentUrl("保存成功", "auto_reply.aspx?m_id=" + m_id + "&t=" + RequestHelper.GetQueryString("t"));
                        }
                        else
                        {
                            JsHelper.AlertAndParentUrl("保存失败，请稍后再试", "auto_reply.aspx?m_id=" + m_id + "&t=" + RequestHelper.GetQueryString("t"));
                        }

                    }
                }
            }


        }
        //清除内容
        protected void btnClare_Click(object sender, EventArgs e)
        {
            if (channel_id == 1 || channel_id == 2)
            {
                string code = _f.Value + _u.Value + c_.Value + k_.Value;
                string code2 = Common.Utils.GetCookie("zdhf" + channel_id);
                if (code == code2)
                {
                    Common.Utils.WriteCookie("zdhf" + channel_id, "");
                    bool result = KDWechat.BLL.Chats.wx_basic_reply.Delete(wx_id, wx_og_id, channel_id);
                    if (result)
                    {
                        AddLog("删除" + (channel_id == 1 ? "关注" : "无匹配") + "时自动回复信息", LogType.删除);
                        JsHelper.AlertAndRedirect("删除成功", "auto_reply.aspx?m_id=" + m_id + "&t=" + RequestHelper.GetQueryString("t"));
                    }
                    else
                    {
                        JsHelper.AlertAndRedirect("删除失败，请稍后再试", "auto_reply.aspx?m_id=" + m_id + "&t=" + RequestHelper.GetQueryString("t"), "fail");
                    }
                }
            }

        }

    }
}