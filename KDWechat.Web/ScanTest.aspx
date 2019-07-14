<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScanTest.aspx.cs" Inherits="KDWechat.Web.ScanTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <input type="button" onclick="scan()" value="扫一扫" />
        <span id="sp"></span>
    </div>
    </form>
    <script>
        <%=wechatConfig%>
        function scan() {
            //alert("ss");
            wx.scanQRCode({
                needResult: 1, // 默认为0，扫描结果由微信处理，1则直接返回扫描结果，
                scanType: ["qrCode", "barCode"], // 可以指定扫二维码还是一维码，默认二者都有
                success: function (res) {
                    var result = res.resultStr; // 当needResult 为 1 时，扫码返回的结果
                    alert(result);
                    //document.getElementById("sp").innerHTML = result;
                }
            });
        }
    </script>
</body>
</html>
