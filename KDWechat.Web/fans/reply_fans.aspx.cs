using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;
using KDWechat.DAL;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.CommonAPIs;

namespace KDWechat.Web.fans
{
    public partial class reply_fans :KDWechat.Web.UI.BasePage
    {
        /// <summary>
        /// 粉丝用户的openid
        /// </summary>
        protected string openId { get { return Common.RequestHelper.GetQueryString("openId"); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                string code = Common.Utils.Number(4, true);
                char[] list = code.ToCharArray();
                _f.Value = list[0].ToString();
                _u.Value = list[1].ToString();
                c_.Value = list[2].ToString();
                k_.Value = list[3].ToString();
                Common.Utils.WriteCookie("rf", code);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!CheckUserAuthorityBool("message_list",RoleActionType.Reply))
            {
                JsHelper.RegisterScriptBlock(this, "backParentPage('fail','您没回复权限');");
                return;
            }

            KDWechat.DAL.t_wx_fans model = KDWechat.BLL.Users.wx_fans.GetFansByID(openId);
            if (model != null)
            {
                 string code = _f.Value + _u.Value + c_.Value + k_.Value;
                 string code2 = Common.Utils.GetCookie("rf");
                if (code == code2)
                {
                    t_wx_wechats wx_wechat = BLL.Chats.wx_wechats.GetWeChatByID(model.wx_id);
                    if (wx_wechat != null)
                    {

                        string accessToken = BLL.Chats.wx_wechats.GetAccessToken(wx_wechat.id, wx_wechat);// AccessTokenContainer.TryGetToken(wx_wechat.app_id, wx_wechat.app_secret);
                        if (!accessToken.Contains("Error:"))
                        {
                            //推送文本信息
                            try
                            {
                                Common.Utils.WriteCookie("rf", "");
                                var resultText = Custom.SendText(accessToken, openId, txtContent.Value);
                                if (resultText.errcode == 0)
                                {
                                    KDWechat.BLL.Logs.wx_fans_chats.CreateFansChat(new t_wx_fans_chats()
                                    {
                                        contents = txtContent.Value,
                                        create_time = DateTime.Now,
                                        from_type = (int)FromUserType.公众号,
                                        is_sys_auto_reply = 0,
                                        media_id = "",
                                        msg_type = (int)msg_type.文本,
                                        open_id = openId,
                                        wx_id = model.wx_id,
                                        wx_og_id = model.wx_og_id
                                    });
                                    AddLog("对粉丝“" + model.nick_name + "”回复了：" + txtContent.Value, LogType.添加);
                                    BLL.Users.wx_fans.SetReplyStateAndTime(openId, FansReplyState.已回复);
                                    JsHelper.RegisterScriptBlock(this, "backParentPage('success','回复成功');");
                                }
                                else
                                {
                                    //JsHelper.RegisterScriptBlock(this, "backParentPage('success','回复失败,错误信息：" + resultText.errmsg + "');");
                                    JsHelper.Alert(Page, "回复失败,错误信息：" + resultText.errmsg, "true");
                                }
                            }
                            catch (ErrorJsonResultException ex)
                            {
                                Common.Utils.WriteCookie("rf", "");
                                string errcode = ex.JsonResult.errcode.ToString();

                                //JsHelper.RegisterScriptBlock(this, "backParentPage('success','回复失败,错误信息：" + ex.Message + "');");
                                JsHelper.Alert(Page, "微信服务器繁忙，请稍后再试。", "true");
                            }
                        }
                        else
                        {
                            Common.Utils.WriteCookie("rf", "");
                            JsHelper.RegisterScriptBlock(this, "backParentPage('success','" + accessToken.Replace("Error:", "") + "');");
                        }
                    }

                }
            }
        }
    }
}