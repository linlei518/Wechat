using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web
{
    public partial class ScanTest : System.Web.UI.Page
    {
        protected string wechatConfig = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var stramp = ((int)((DateTime.Now - DateTime.Parse("1970-1-1")).TotalSeconds)).ToString();
                var nonceStr = "528417";
                var accessToken = BLL.Chats.wx_wechats.GetAccessToken(24);
                var jsapi_ticket = BLL.Chats.wx_wechats.GetJsTicket("wxe8c2d892dea7ec14");
                var signature = Common.WeChatJsApi.GetSignature(nonceStr, jsapi_ticket, stramp, Request.Url.ToString());
                wechatConfig = "wx.config({debug: true, appId: 'wxe8c2d892dea7ec14',  timestamp: '" + stramp + "', nonceStr: '" + nonceStr + "',  signature: '" + signature + "', jsApiList: ['onMenuShareTimeline','onMenuShareAppMessage','onMenuShareQQ','onMenuShareWeibo','startRecord','stopRecord','onVoiceRecordEnd','playVoice','pauseVoice','stopVoice','onVoicePlayEnd','uploadVoice','downloadVoice','chooseImage','previewImage','uploadImage','downloadImage','translateVoice','getNetworkType','openLocation','getLocation','hideOptionMenu','showOptionMenu','hideMenuItems','showMenuItems','hideAllNonBaseMenuItem','showAllNonBaseMenuItem','closeWindow','scanQRCode','chooseWXPay','openProductSpecificView','addCard','chooseCard','openCard']});";

            }
        }
    }
}