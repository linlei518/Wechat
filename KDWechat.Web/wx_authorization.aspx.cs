using KDWechat.Common;
using KDWechat.DAL;
using LitJson;
using Senparc.Weixin;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP.AdvancedAPIs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web
{
    public partial class wx_authorization : System.Web.UI.Page
    {

        protected string open_id = RequestHelper.GetQueryString("openID");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(open_id) || open_id.Trim().Length == 0)//没取到opid，失败
            {
                var wechat = Companycn.Core.EntityFramework.EFHelper.GetModel<creater_wxEntities, t_wx_wechats>(x => x.id == 24);
                if (wechat != null)
                {
                    if (HttpContext.Current.Request.QueryString["code"] == null)
                    {
                        string wchatconfig = new KDWechat.BLL.Config.wechat_config().loadConfig().domain;
                        string url = wchatconfig + "/wx_authorization.aspx?token=this_is_token";
                        Response.Redirect("https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + wechat.app_id + "&redirect_uri=" + HttpUtility.UrlEncode(url) + "&response_type=code&scope=snsapi_userinfo&state=frank#wechat_redirect");
                    }
                    else
                    {
                        BaseCallbackTest(HttpContext.Current.Request.QueryString["code"], HttpContext.Current.Request.QueryString["state"], "/wx_authorization.aspx", wechat);
                    }
                }
                else//非法进入
                {

                }
            }
            else
                Response.Write(open_id);
                //Response.Redirect("http://gatsby.companycn.net/ht.html?openID=" + open_id + "&token="+Session["OAuthAccessToken"].ToString());
                //Response.Redirect("http://gatsby.companycn.net/testform.aspx?openID=" + open_id + "&token=" + Session["OAuthAccessToken"].ToString());
        }


        #region 授权
        public void OAuthToMy(string code, string state, string page_name, t_wx_wechats wechat)
        {
            try
            {               
                var url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type={3}",
                                    wechat.app_id.Trim(), wechat.app_secret.Trim(), code, "authorization_code");
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)WebRequest.Create(url);

                //提供向 URI 标识的资源发送数据和从 URI 标识的资源接收数据的公共方法
                System.Net.WebClient Client = new System.Net.WebClient();
                //存储 Internet 资源的凭据
                System.Net.CredentialCache myCache = new System.Net.CredentialCache();

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
                }
                else
                {
                    JsonData jsonData = JsonMapper.ToObject(retString);
                    if (jsonData != null)
                    {
                        if (jsonData["openid"] != null)
                        {
                            if (page_name.Contains("?"))
                            {
                                Response.Redirect(page_name + "&openId=" + jsonData["openid"]);
                            }
                            else
                            {
                                Response.Redirect(page_name + "?openId=" + jsonData["openid"]);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                
            }


        }

        public void BaseCallbackTest(string code, string state, string page_name, t_wx_wechats wechat)
        {

            if (string.IsNullOrEmpty(code))
            {
                // Response.Write("您拒绝了授权！");
                return;
            }

            //if (state != "frank")
            //{
            //    //这里的state其实是会暴露给客户端的，验证能力很弱，这里只是演示一下
            //    //实际上可以存任何想传递的数据，比如用户ID，并且需要结合例如下面的Session["OAuthAccessToken"]进行验证
            //    Response.Write("验证失败！请从正规途径进入！");
            //    return;
            //}

            //通过，用code换取access_token

            try
            {
                var result = OAuth.GetAccessToken(wechat.app_id.Trim(), wechat.app_secret.Trim(), code);

                //Response.Write("aa：" + result.openid+"bbb"+result.access_token);
                if (result.errcode != ReturnCode.请求成功)
                {
                    HttpContext.Current.Response.Write("错误：" + result.errmsg);
                    return;
                }
                //下面2个数据也可以自己封装成一个类，储存在数据库中（建议结合缓存）
                //如果可以确保安全，可以将access_token存入用户的cookie中，每一个人的access_token是不一样的
                HttpContext.Current.Session["OAuthAccessTokenStartTime"] = DateTime.Now;
                HttpContext.Current.Session["OAuthAccessToken"] = (result ?? new OAuthAccessTokenResult { access_token="this_is_fake"}).access_token;

                //因为这里还不确定用户是否关注本微信，所以只能试探性地获取一下
                //Senparc.Weixin.MP.AdvancedAPIs.OAuthUserInfo userInfo = null;
                string _openId = "";

                _openId = result.openid;
                Utils.WriteCookie("OpenID", _openId);//!!此处写入COOKID！！——————————————
                //HttpCookie OpenID = new HttpCookie("OpenID");
                //OpenID.Value = _openId;
                //OpenID.Path = "/";
                //HttpContext.Current.Response.Cookies.Add(OpenID);
                if (page_name.Contains("?"))
                {
                    Response.Redirect(page_name + "&openID=" + _openId);
                }
                else
                {
                    Response.Redirect(page_name + "?openID=" + _openId);
                }



            }
            catch (ErrorJsonResultException ex)
            {
                //未关注，只能授权，无法得到详细信息
                //这里的 ex.JsonResult 可能为："{\"errcode\":40003,\"errmsg\":\"invalid openid\"}"


            }
            finally
            {
            }
        }
        #endregion

    }
}