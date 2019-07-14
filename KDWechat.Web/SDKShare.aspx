<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SDKShare.aspx.cs" Inherits="KDWechat.Web.SDKShare" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>微信JSSDK分享</title>
      <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
     <script src="Scripts/SDKShare.js"></script>
    <script>
        var weixinShare = new WeixinShare({
            title: '分享测试',
            content: '测试内容',
            linkUrl: window.location.href,
            imgUrl: 'http://wx.companycn.net/images/login_bg2.png',
            appId: '<%=appId%>',
            debug: true,
            timestamp: '<%=timestamp%>',
            nonceStr: '<%=nonceStr%>',
            signature:'<%=signature%>',
            shareAppMessageSuccessCallback: function () {
                alert("分享给朋友成功");
            },
            shareTimelineSuccessCallback: function () {
                alert("分享朋友圈成功");
            },
            shareQQSuccessCallback: function () {
                alert("分享给QQ好友成功");
            },
            shareWeiboSuccessCallback: function () {
                alert("分享到微博成功");
            }

        });

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
