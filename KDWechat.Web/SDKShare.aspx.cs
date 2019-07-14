using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web
{
    public partial class SDKShare : System.Web.UI.Page
    {
        public string timestamp = "";
        public string nonceStr = "";
        public string signature = "";
        public string appId = "wxe8c2d892dea7ec14";
        protected void Page_Load(object sender, EventArgs e)
        {
            timestamp = ((int)((DateTime.Now - DateTime.Parse("1970-1-1")).TotalSeconds)).ToString();
            nonceStr = "528417";
            var accessToken = BLL.Chats.wx_wechats.GetAccessToken(24);
            var jsapi_ticket = Common.WeChatJsApi.GetJsApiTicket(accessToken);
            signature = Common.WeChatJsApi.GetSignature(nonceStr, jsapi_ticket, timestamp, Request.Url.ToString());
        }
    }
}