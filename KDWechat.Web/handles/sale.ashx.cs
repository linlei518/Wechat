using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using KDWechat.DAL;
using KDWechat.BLL.Module;
using KDWechat.BLL.Users;
using KDWechat.Common;
using KDWechat.Web.sales.seller;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.AdvancedAPIs;

namespace KDWechat.Web.handles
{
    /// <summary>
    /// sale 的摘要说明
    /// </summary>
    public class sale : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";//返回类型JSON
            context.Response.Expires = -1;//相对过期时间
            context.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);//绝对过期时间

            string method = RequestHelper.GetQueryString("method");
            string output=JsonConvert.SerializeObject(new {result=0,msg="请求错误"});

            string result = null;

            switch (method)
            {
                case "getfans":
                    result = getFans();//获取粉丝
                    break;
                case "getmsg":
                    result = getMessage();//获取最新消息
                    break;
                case "sendmsg":
                    result = sendMsg();
                    break;
            }
            context.Response.Write(result??output);
        }

        private string sendMsg()
        {
            string output = null;
            int sellerID = RequestHelper.GetQueryInt("seller_id");
            int fansID = RequestHelper.GetQueryInt("fans_id");
            string project = HttpUtility.HtmlDecode(RequestHelper.GetFormString("project"));
            string message = RequestHelper.GetFormString("msg", true);

            KDWechat.DAL.t_wx_fans fan = KDWechat.BLL.Users.wx_fans.GetFansByID(fansID);
            if (fan != null)
            {
                var seller = md_sale_users.GetModel(sellerID);
                if (seller != null)
                {
                    t_wx_wechats wx_wechat = BLL.Chats.wx_wechats.GetWeChatByID(fan.wx_id);
                    if (wx_wechat != null)
                    {
                        string accessToken = BLL.Chats.wx_wechats.GetAccessToken(wx_wechat.id,wx_wechat);//  AccessTokenContainer.TryGetToken(wx_wechat.app_id, wx_wechat.app_secret);
                        if (!accessToken.Contains("Error:"))
                        {
                            var pro = md_sale_project.GetModel(x => x.p_name == project && x.wx_id == wx_wechat.id);
                            if (pro != null)
                            {
                                //推送文本信息
                                if (wx_fans.CheckUser(fan.open_id) != FansState.客服聊天状态)
                                {
                                    message += "\r\n此条为销售客服回复，如需退出销售聊天，请发送“退出聊天”。";
                                    wx_fans.SetState(fan.open_id, FansState.客服聊天状态);
                                    var relation = md_sale_users_fans_relation.GetModel(x => x.fans_open_id == fan.open_id && x.project_id == pro.id && x.user_id == sellerID);
                                    relation.status = (int)Status.正常;
                                    md_sale_users_fans_relation.UpdateModel(relation);
                                }
                                try
                                {
                                    var resultText = Custom.SendText(accessToken, fan.open_id, seller.realname + ":" + message);
                                    if (resultText.errcode == 0)
                                    {
                                        BLL.Module.md_sale_chats.AddModel(
                                            new t_md_sale_chats
                                            {
                                                creat_time = DateTime.Now,
                                                seller_id = seller.id,
                                                status = (int)Status.正常,
                                                is_read = (int)SaleChatIsReadType.已读,
                                                message = message,
                                                user_openid = fan.open_id,
                                                type = (int)SaleChatSendType.用户接收,
                                                wx_og_id = wx_wechat.wx_og_id,
                                                wx_id = wx_wechat.id,
                                                project_id = pro.id
                                            }
                                        );
                                        output = JsonConvert.SerializeObject(new { result = 1, msg = "发送成功" });
                                    }
                                }
                                catch (Senparc.Weixin.Exceptions.ErrorJsonResultException ex)  
                                {
                                    
                                }
                            }
                        }
                    }
                }
            }
            return output;
        }

        //获取最新消息
        private string getMessage()
        {
            string output = null;
            int sellerID = RequestHelper.GetQueryInt("seller_id");
            var count =10;
            var list = new List<Sale_Chat_Fans>();
            var fans = md_sale_chats.GetGroupBy<string>((x => x.seller_id == sellerID&&x.is_read==(int)SaleChatIsReadType.未读), (x => x.user_openid));//去所有OPENID
            if (fans.Length > 0)
            {
                foreach (var opid in fans)
                {
                    var fan = wx_fans.GetFansByID(opid);//取粉丝
                    if (fan != null)
                    {
                        Sale_Chat_Fans chatFan = new Sale_Chat_Fans()//拼接粉丝信息
                        {
                            id = fan.id.ToString(),
                            name = fan.nick_name,
                            pic = fan.headimgurl,
                            messages = new List<Sale_Message>()
                        };
                        var history_list = md_sale_chats.GetViewList<DateTime>((x => x.seller_id == sellerID && x.is_read == (int)SaleChatIsReadType.未读 && x.user_openid == opid), x => x.creat_time, int.MaxValue, 1, out count, true);//取3条历史记录
                        foreach (var x in history_list)
                        {
                            chatFan.messages.Add(new Sale_Message()
                                {
                                    project = x.p_name,
                                    content = Utils.ChangeToWeChatEmotion(x.message),
                                    time = x.creat_time.ToString("yyyy-MM-dd HH:mm:ss")
                                });
                        }
                        int[] ids = (from x in history_list select x.id).ToArray();
                        md_sale_chats.SetIsRead(ids);
                        list.Add(chatFan);
                    }//fan != null
                }//foreach
                output = JsonConvert.SerializeObject(list, Formatting.Indented);

                //var msg_list = md_sale_chats.GetViewList<DateTime>((x => x.seller_id == sellerID&&x.is_read==(int)SaleChatIsReadType.未读), x => x.creat_time, int.MaxValue, 1, out count, true);
            }
            else
                output = "[]";

            return output;
        }

        //获取粉丝信息
        private string getFans()
        {
            string output = null;
            int fansID = RequestHelper.GetQueryInt("fan_id", -1);
            if (fansID != -1)
            {
                var fans = wx_fans.GetFansByID(fansID);
                if (fans != null)
                {
                    var proID = md_sale_chats.GetModel<DateTime>((x => x.user_openid == fans.open_id), (x => x.creat_time), true);
                    if (proID != null)
                    {
                        var pro = md_sale_project.GetModel(proID.project_id);
                        string state;
                        if (proID.is_read == (int)SaleChatIsReadType.已读)
                            state = "已回复";
                        else
                        {
                            state = "未回复";
                            if (proID.creat_time - DateTime.Now > TimeSpan.FromHours(48))
                            {
                                state += "已过期";
                            }
                        }
                        if (null != pro)
                        {
                            output = JsonConvert.SerializeObject(new { result = 1, msg = "成功", data = new { id = fans.id, nick_name = fans.nick_name, head_url = fans.headimgurl, pro_name = pro.p_name,state = state } });
                        }
                    }
                }
            }
            return output;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}