<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="qr_code_show.aspx.cs" Inherits="KDWechat.Web.product.qr_code_show" %>




<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title><%=pageTitle %></title>
      <script src="../scripts/html5.js"></script>

</head>

<body class="mainbody">
    <form id="form1" runat="server">
        <!--导航栏-->
      
        <div class="line10"></div>
        <!--/导航栏-->

      

        <div class="tab-content listPanel_01">
          <img width="200px" height="200px" src="<%=code %>"/>
              
        </div>
        <!--/内容-->
       
    </form>
</body>
</html>