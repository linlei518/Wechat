using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;
using KDWechat.DAL;
using System.IO;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.Exceptions;
using LitJson;
using System.Net;
using System.Text;
using KDWechat.Common.Config;
using KDWechat.BLL.Entity.JsonResult;

namespace KDWechat.Web.fans
{
    public partial class member_bind : System.Web.UI.Page
    {
        protected int wx_id = 0;
        protected wechatconfig wechatconfig = new BLL.Config.wechat_config().loadConfig();
        protected string wx_id_without_des = RequestHelper.GetQueryString("wx_id");
        protected string openID = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(wx_id_without_des))
                wx_id = Utils.StrToInt(DESEncrypt.Decrypt(wx_id_without_des), -1);
            openID = Utils.GetCookie("openID" + wx_id);
            if (wx_id > 0)
            {
                if (string.IsNullOrWhiteSpace(openID))
                {
                    #region 授权
                    DAL.t_wx_wechats wechat = KDWechat.BLL.Chats.wx_wechats.GetWeChatByID(wx_id);
                    if (wechat != null)
                    {
                        if (HttpContext.Current.Request.QueryString["code"] == null)
                        {
                            string url = wechatconfig.domain + "/fans/member_bind.aspx?wx_id=" + wx_id_without_des;
                            Response.Redirect("https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + wechat.app_id + "&redirect_uri=" + HttpUtility.UrlEncode(url) + "&response_type=code&scope=snsapi_base&state=frank#wechat_redirect");
                        }
                        else
                            BaseCallbackTest(Request.QueryString["code"], Request.QueryString["state"], "member_bind.aspx?wx_id=" + wx_id_without_des, wechat);
                    }
                    #endregion
                }
                else
                {
                    var relation = Companycn.Core.EntityFramework.EFHelper.GetModel<wechatEntities, t_member_fans_relation>(x => x.openid == openID);
                    if (relation != null)
                    {
                        Response.Write("<script>alert('您绑定过会员，请勿重复操作！');location.href='member_bind_result.aspx?msg=您已绑定过会员，请勿重复操作';</script>");
                        Response.End();
                    }
                }
            }
            else
            {
                Response.Write("<script>alert('请勿非法进入！');</script>");
                Response.End();
            }
        }

        #region 授权
        public void OAuthToMy(string code, string state, string page_name, DAL.t_wx_wechats wechat)
        {
            try
            {
                var url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type={3}",
                                    wechat.app_id.Trim(), wechat.app_secret.Trim(), code, "authorization_code");
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

                if (retString.Contains("errcode"))
                {
                    Response.Write("<script>alert('当前微信号无法获取您的凭证')</script>");
                }
                else
                {
                    JsonData jsonData = JsonMapper.ToObject(retString);
                    if (jsonData != null && jsonData["openid"] != null)
                    {
                        if (page_name.Contains("?"))
                            Response.Redirect(page_name + "&openId=" + jsonData["openid"]);
                        else
                            Response.Redirect(page_name + "?openId=" + jsonData["openid"]);
                    }
                }
            }
            catch (Exception)
            {
            }


        }

        public void BaseCallbackTest(string code, string state, string page_name, DAL.t_wx_wechats wechat)
        {
            if (string.IsNullOrEmpty(code))            
                return;            
            try
            {
                var result = OAuth.GetAccessToken(wechat.app_id.Trim(), wechat.app_secret.Trim(), code);
                if (result.errcode != ReturnCode.请求成功)
                {
                    HttpContext.Current.Response.Write("错误：" + result.errmsg);
                    return;
                }
                Utils.WriteCookie("openID" + wechat.id, result.openid);
                if (page_name.Contains("?"))
                    Response.Redirect(page_name);                
                else                
                    Response.Redirect(page_name);                
            }
            catch (ErrorJsonResultException ex)
            {
            }
        }
        #endregion

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string location = "member_bind_result.aspx";
            var ip = Utils.GetUserIp();
            var apiUrl = System.Configuration.ConfigurationManager.AppSettings["KDMemberUrl"];
            var url = apiUrl + "&Mt=loginM&uTel=" + txtPhone.Value + "&uPwd=" + txtPassword.Value + "&uIP=" + ip + "";
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            string result = wc.DownloadString(url);
            var ss = JsonApi.GetJsonResult<JsonDataResult<t_member>>(result);
            if (ss is JsonDataResult<t_member>)
            {
                var newMember = (JsonDataResult<t_member>)ss;
                int member_id = newMember.data.id;
                string phones = newMember.data.phone;
                if (member_id > 0)
                {
                    var relation = Companycn.Core.EntityFramework.EFHelper.GetModel<wechatEntities, t_member_fans_relation>(x => x.openid == openID);
                    if (relation != null)
                    {
                        Response.Write("<script>alert('您绑定过会员，请勿重复操作！');location.href='" + location + "?msg=您已绑定过会员，请勿重复操作';</script>");
                        Response.End();
                    }
                    else
                    {
                        var fans = Companycn.Core.EntityFramework.EFHelper.GetModel<wechatEntities, t_wx_fans>(x => x.open_id == openID);
                        if (fans != null)
                        {
                            t_member_fans_relation mods = new t_member_fans_relation
                            {
                                fans_id = fans.id,
                                member_id = member_id,
                                openid = openID,
                                wx_id = fans.wx_id,
                                wx_og_id = fans.wx_og_id
                            };
                            Companycn.Core.EntityFramework.EFHelper.AddModel<wechatEntities, t_member_fans_relation>(mods);
                            Response.Write("<script>alert('绑定成功！');location.href='" + location + "?msg=绑定成功';</script>");
                        }
                        else
                        {
                            Response.Write("<script>alert('尚未关注本微信号，请重新关注！');location.href='" + location + "?msg=请重新关注';</script>");
                            Response.End();
                        }
                    }                    
                }

            }
            else
            {
                Response.Write("<script>alert('用户名密码错误！');</script>");
            }
            wc.Dispose();
           
               
        }

        protected void btnRegist_Click(object sender, EventArgs e)
        {
            Response.Redirect("member_regist.aspx?wx_id=" + wx_id_without_des);
        }
    }
}