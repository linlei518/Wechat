<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="member_bind_result.aspx.cs" Inherits="KDWechat.Web.fans.member_bind_result" %>

<!doctype html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=Request.QueryString["msg"] %></title>
    <meta name="viewport" id="viewport" content="width=device-width, user-scalable=no,initial-scale=1,maximum-scale=1">
    <meta name="screen-orientation" content="portrait/landscape">
    <meta content="yes" name="apple-mobile-web-app-capable">
    <meta content="black" name="apple-mobile-web-app-status-bar-style">
    <meta content="telephone=no" name="format-detection">
    <link href="../check_in/styles/global.css" rel="stylesheet" type="text/css">
    <link href="../check_in/styles/zise.css" rel="stylesheet" type="text/css">

</head>
<body class="bodystyle">
    <section id="contenter">
        <div class="closeTitle">
            <h1><%=Request.QueryString["msg"] %></h1>
        </div>
    </section>
    <div class="closebtn"><a href="javascript:void(0)" onclick="closePage()"></a></div>
    <script type="text/javascript">
    function closePage() {
        WeixinJSBridge.call("closeWindow");
    }
    </script>
</body>
