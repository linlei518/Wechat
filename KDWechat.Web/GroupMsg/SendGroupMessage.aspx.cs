using KDWechat.Common;
using KDWechat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.BLL.Users;
using System.Linq.Expressions;
using KDWechat.BLL.Chats;
using System.IO;

namespace KDWechat.Web.GroupMsg
{
    public partial class SendGroupMessage : Web.UI.BasePage
    {
        protected int id { get {  return RequestHelper.GetQueryInt("id", 0); } }
        protected string loadjs = string.Empty;//为运行时添加的JS
        protected string strLength = "600";//限定的文字数量
        protected int canSendCount = 0;//本月已发送的数量
        protected string showNo = "";
        protected bool showTime = false;
        protected Common.Config.wechatconfig wechatConfig = new BLL.Config.wechat_config().loadConfig();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod == "POST" && Request.QueryString["pre"] == "ss")
            {
                Preview();
            }
            if (!IsPostBack)
            {
                CheckUserAuthority("wechat_massage");
                displayBind();
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


                    if ((model.is_send_all ?? 0) == 1)
                    {
                        radSendAll.Checked = true;
                        radSendPart.Checked = false;
                        dlGroupFilter.Attributes.Add("style", "display:none");
                    }
                    else
                    {
                        showNo = "发送人数：" + model.send_num;
                        hf_openIDs.Value = model.openIDs;
                        radSendAll.Checked = false;
                        radSendPart.Checked = true;
                    }

                    if (model.is_timer == (int)is_timerMode.是)//判断是否是定时发送
                    {
                        radNotTimer.Checked = false;
                        radTimer.Checked = true;
                        setTiming.Value = model.send_time.ToString();
                        showTime = true;
                     }
                    else
                    {
                        radTimer.Checked = false;
                        radNotTimer.Checked = true;
                    }
                    if (model != null)
                    {
                        hftype.Value = getSortIDBymsgType((msg_type)model.msg_type);
                        txtTitle.Value = model.title;

                        if (model.is_send == (int)is_sendMode.是)
                        {
                            btnSubmit.Visible = false;//已发送的数据，屏蔽发送按钮
                        }
                        switch (model.msg_type)
                        {
                            case (int)msg_type.文本:
                                txtContents.Value = model.contents;
                                strLength = (600 - model.contents.Trim().Length).ToString();
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
                                    loadjs = "selectResult(" + _channel_id + ", " + model.source_id + ",'" + m.file_url + "','" + m.title + "','" + m.create_time.ToString("yyyy-MM-dd") + "','" + (Common.Utils.DropHTML(m.remark, 140).Replace("\n", "").Trim()) + "',0,'','" + (_channel_id == 3 ? m.hq_music_url : "") + "'," + (_channel_id == 3 ? (int)m.video_type : 0) + ");";

                                }
                                break;

                            case (int)msg_type.单图文:
                                t_wx_news_materials m2 = KDWechat.BLL.Chats.wx_news_materials.GetModel((int)model.source_id);
                                if (m2 != null)
                                {
                                    loadjs = "selectResult(4, " + model.source_id + ",'" + m2.cover_img + "','" + m2.title + "','" + m2.create_time.ToString("yyyy-MM-dd") + "','" + (Common.Utils.DropHTML(m2.summary, 140).Replace("\n", "").Trim()) + "',0,'','',0);";
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
                                    loadjs = "selectResult(5, " + model.source_id + ",'" + multi.cover_img + "','" + multi.title + "','" + multi.create_time.ToString("yyyy-MM-dd") + "','" + (Common.Utils.DropHTML(multi.summary, 140).Replace("\n", "").Trim()) + "',0,'" + multi.multi_html.Replace("\r", "").Replace("\n", "").Trim() + "','',0);";
                                }
                                break;
                        }
                    }
                }
            }
        }

        private void displayBind()
        {
            var managerCount = Companycn.Core.EntityFramework.EFHelper.GetCount<creater_wxEntities, t_qy_manager>(x => x.wx_id == wx_id);
            if (managerCount <= 0)
            {
                JsHelper.AlertAndParentUrl("请先设置群发管理员，否则无法进行群发审核。", "groupmsg_manager_list.aspx?m_id=71", "fail");
                return;
            }
            radNotTimer.Checked = true;
            canSendCount = KDWechat.BLL.Chats.wx_group_msgs.GetSendNo(wx_id);//获取已发送条数

        }

        //提交
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string log_title = "发送了群发信息。";
            bool isSend = false;
            string alertMsg = "发送失败";//成功后的提示

            string[] groupIDS = hf_openIDs.Value.Split(',');


            if (id != 0)//保存或定时的信息
            {
                var msg = wx_group_msgs.GetGroupMsgByID(id);
                if (msg != null)
                {
                    msg.u_id = u_id;
                    msg.openIDs = Utils.GetArrayStr(groupIDS, ",");
                    msg.title = txtTitle.Value;
                    if (radSendAll.Checked)
                        msg.send_num = Companycn.Core.EntityFramework.EFHelper.GetCount<creater_wxEntities, t_wx_fans>(x => x.wx_id == wx_id&&x.status==(int)Status.正常);
                    else
                        msg.send_num = groupIDS.Count();
                    msg.is_send = (int)is_sendMode.否;
                    log_title += ((msg_type)msg.msg_type).ToString() + "类型，素材id为" + msg.source_id + " , 群发ID为：" + msg.id;
                    msg.is_send_all = radSendAll.Checked ? 1 : 0;
                    if (radTimer.Checked)
                    {
                        msg.send_time = Utils.StrToDateTime(setTiming.Value);
                        wx_group_msgs.UpdateGroupMsg(msg);
                        log_title = "修改了群发：" + msg.title;
                        isSend = true;
                        alertMsg = "保存成功，审核信息已发送，请通知管理员在预订群发时间前进行审核。";
                    }
                    else
                    {
                        msg.send_time = null;
                        msg.is_timer = (int)is_timerMode.否;
                        wx_group_msgs.UpdateGroupMsg(msg);
                        isSend = true;
                        alertMsg = "保存成功，审核信息已发送，请通知管理员进行审核。";

                    }
                }
            }
            else
            {
                msg_type reply_type = msg_type.文本;
                string contents = "";
                int source_id = Common.Utils.StrToInt(hfid.Value, 0);
                t_wx_group_msgs groupMsg = null;
                switch (hftype.Value)
                {
                    case "0":
                        reply_type = msg_type.文本;
                        contents = txtContents.Value.Trim().Replace("<br>","").Replace("<br />","").Replace("<br/>","");
                        log_title += "文本类型，内容为：" + contents;
                        break;
                    case "1":
                        reply_type = msg_type.图片;
                        log_title += "图片类型，素材id为" + hfid.Value + " , 素材标题为：" + hflogtitle.Value;
                        break;
                    case "2":
                        reply_type = msg_type.语音;
                        log_title += "语音类型，素材id为" + hfid.Value + " , 素材标题为：" + hflogtitle.Value;
                        break;
                    case "3":
                        reply_type = msg_type.视频;
                        log_title += "视频类型，素材id为" + hfid.Value + " , 素材标题为：" + hflogtitle.Value;
                        break;
                    case "4":
                        reply_type = msg_type.单图文;
                        log_title += "单图文类型，素材id为" + hfid.Value + " , 素材标题为：" + hflogtitle.Value;
                        break;
                    case "5":
                        reply_type = msg_type.多图文;
                        log_title += "多图文类型，素材id为" + hfid.Value + " , 素材标题为：" + hflogtitle.Value;
                        break;
                }
                groupMsg = new t_wx_group_msgs()//初始化群发信息
                {
                    u_id = u_id,
                    openIDs = Utils.GetArrayStr(groupIDS, ","),
                    msg_type = (int)reply_type,
                    wx_id = wx_id,
                    wx_og_id = wx_og_id,
                    title = txtTitle.Value,
                    send_num = groupIDS.Count(),
                    create_time = DateTime.Now,
                    is_timer=(int)is_timerMode.否,
                    is_send = (int)is_sendMode.否,
                    send_time=null,
                    is_send_all = radSendAll.Checked ? 1 : 0
                };
                if (radSendAll.Checked)
                    groupMsg.send_num = Companycn.Core.EntityFramework.EFHelper.GetCount<creater_wxEntities, t_wx_fans>(x => x.wx_id == wx_id && x.status == (int)Status.正常);

                if (reply_type == msg_type.文本)
                {
                    groupMsg.contents = contents;
                }
                else
                {
                    groupMsg.source_id = source_id;
                }
                if (radTimer.Checked)
                {
                    groupMsg.is_timer = (int)is_timerMode.是;//延时发送时属性变更
                    groupMsg.send_time = DateTime.Parse(setTiming.Value);

                    groupMsg = BLL.Chats.wx_group_msgs.InsertGroupMsg(groupMsg);//保存
                    isSend = groupMsg.id != 0;
                    alertMsg = "保存成功，审核信息已发送，请通知管理员在预订群发时间前进行审核。";
                }
                else
                {
                    

                    groupMsg = BLL.Chats.wx_group_msgs.InsertGroupMsg(groupMsg);//保存
                    isSend = groupMsg.id != 0;
                    alertMsg = "保存成功，审核信息已发送，请通知管理员进行审核。";
                }
                var code = Companycn.Core.EntityFramework.EFHelper.AddModel<creater_wxEntities, t_wx_group_msg_key>(new t_wx_group_msg_key()
                {
                    accessKey = Utils.Number(6),
                    create_time = DateTime.Now,
                    status = (int)Status.正常,
                    group_msg_id = groupMsg.id,
                    wx_id = wx_id,
                    wx_og_id = wx_og_id
                });
                var managerList = Companycn.Core.EntityFramework.EFHelper.GetList<creater_wxEntities, t_qy_manager, int>(x => x.wx_id == wx_id, x => x.id, int.MaxValue, 1);
                string uList = "";
                if (null != managerList)
                    managerList.ForEach(x => uList += x.user_name + "|");
                if (uList.Length > 0)
                {
                    uList = uList.TrimEnd('|');
                    try
                    {
                        if (code != null)
                        {
                            var accessToken = Senparc.Weixin.QY.CommonAPIs.CommonApi.GetToken(wechatConfig.qy_app_id, wechatConfig.qy_app_secret);
                            Senparc.Weixin.QY.AdvancedAPIs.Mass.SendText(accessToken.access_token, uList, "", "", wechatConfig.qy_agent_id, string.Format("有一条群发消息需要您的验证，消息名称为‘{0}’，验证码为‘{1}’", groupMsg.title, code.accessKey));
                        }
                    }
                    catch
                    {
                        JsHelper.AlertAndParentUrl("保存成功，验证码未正确发送，请手动尝试", "group_messsage_list.aspx?m_id=71");
                    }
                }
            }
            if (isSend)
            {
                AddLog(log_title, id == 0 ? LogType.添加 : LogType.修改);
                JsHelper.AlertAndParentUrl(alertMsg, "group_messsage_list.aspx?m_id=71");

            }
            else
            {
                //if (hftype.Value == "4" || hftype.Value == "5")
                //    JsHelper.AlertAndParentUrl("您发送的图文中包含外链、模块或从微信平台导入的图文消息，根据微信平台群发规则，本条无法被发送", "group_messsage_list.aspx?m_id=71", "fail");
                //else
                JsHelper.AlertAndParentUrl("保存失败，请刷新后再试", "group_messsage_list.aspx?m_id=71","fail");
            }

        }

        string getSortIDBymsgType(msg_type type)
        {
            string output = "";
            switch (type)
            {
                case msg_type.文本:
                    output="0";
                    break;
                case msg_type.图片:
                    output = "1";
                    break;
                case msg_type.语音:
                    output = "2";
                    break;
                case msg_type.视频:
                    output = "3";
                    break;
                case msg_type.单图文:
                    output = "4";
                    break;
                case msg_type.多图文:
                    output = "5";
                    break;
            }
            return output;
        }

        private void Preview()
        {
           msg_type reply_type = msg_type.文本;
            var log_title = "预览了群发，";
            var hfid = Request.Form["hfid"];
            var txtContents = Request.Form["txtContents"];
            var hflogtitle = Request.Form["hflogtitle"];
            var overViewOpid = Request.Form["overViewOpid"];
            switch (Request.Form["hftype"])
            {
                case "0":
                    reply_type = msg_type.文本;
                    log_title += "文本类型，内容为：" + txtContents;
                    break;
                case "1":
                    reply_type = msg_type.图片;
                    log_title += "图片类型，素材id为" + hfid + " , 素材标题为：" + hflogtitle;
                    break;
                case "2":
                    reply_type = msg_type.语音;
                    log_title += "语音类型，素材id为" + hfid + " , 素材标题为：" + hflogtitle;
                    break;
                case "3":
                    reply_type = msg_type.视频;
                    log_title += "视频类型，素材id为" + hfid + " , 素材标题为：" + hflogtitle;
                    break;
                case "4":
                    reply_type = msg_type.单图文;
                    log_title += "单图文类型，素材id为" + hfid + " , 素材标题为：" + hflogtitle;
                    break;
                case "5":
                    reply_type = msg_type.多图文;
                    log_title += "多图文类型，素材id为" + hfid + " , 素材标题为：" + hflogtitle;
                    break;
            }


            var sentMsg = BLL.Chats.wx_group_msgs.OverView(reply_type, overViewOpid, wx_id, Common.Utils.StrToInt(hfid, 0), txtContents.Trim().Replace("<br>", "").Replace("<br />", "").Replace("<br/>", ""));

            if (sentMsg.Item1)
            {
                AddLog(log_title, LogType.添加);
                Response.Write("1|"+sentMsg.Item2);
            }
            else
            {
                Response.Write("0|" + sentMsg.Item2);
            }
            Response.End();
        }


    }
}