<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="error.aspx.cs" Inherits="KDWechat.Web.error" %>

<%@ Register TagName="TopControl" Src="~/UserControl/TopControl.ascx" TagPrefix="uc" %>

<%@ Register TagName="MenuList" Src="~/UserControl/MenuList.ascx" TagPrefix="uc" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>访问出错了！</title>
    <script src="scripts/html5.js"></script>
    <link type="text/css" href="styles/global.css" rel="stylesheet" />
    <link type="text/css" href="styles/style.css" rel="stylesheet" />
    <!--[if lt IE 9 ]><link href="styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>
<body>
    <uc:TopControl ID="TopControl1" runat="server" />
    <uc:MenuList ID="MenuList1" runat="server" />
    <section id="main">
        <div>
            <img src="images/no_role.png" />
        </div>    
             
    </section>
    <script src="scripts/jquery-1.10.2.min.js"></script>
    <script src="Scripts/controls.js"></script>
    <script src="scripts/main.js"></script>


</body>
</html>

