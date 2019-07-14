using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web
{
    public partial class JsTest : System.Web.UI.Page
    {
        protected string wechatConfig="";
        protected string appid = "wxe8c2d892dea7ec14";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var stramp = ((int)((DateTime.Now -DateTime.Parse("1970-1-1").AddHours(-8)).TotalSeconds)).ToString();
                var nonceStr = "528417";
                var jsapi_ticket = BLL.Chats.wx_wechats.GetJsTicket(appid);//Common.WeChatJsApi.GetJsApiTicket(accessToken);
                var signature = Common.WeChatJsApi.GetSignature(nonceStr, jsapi_ticket, stramp, Request.Url.ToString());
                wechatConfig = "wx.config({debug: true, appId: '" + appid + "',  timestamp: '" + stramp + "', nonceStr: '" + nonceStr + "',  signature: '" + signature + "', jsApiList: ['onMenuShareTimeline','onMenuShareAppMessage','onMenuShareQQ','onMenuShareWeibo']});";

            }
        }
    }
}