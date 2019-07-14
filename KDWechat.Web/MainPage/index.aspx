<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="KDWechat.Web.main.index" %>
<%@ Register src="topAndmenu.ascx" tagname="topAndmenu" tagprefix="uc2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="UTF-8" />
    <title>凯德OTO管理后台</title>
    <link type="text/css" href="/styles/global.css" rel="stylesheet" />
    <script src="/scripts/html5.js"></script>
    <!--[if lt IE 9 ]><link href="/styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="/styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="/styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <style>
        .navtext {
            background-color: #8588D2;
            text-align: center;
            vertical-align: middle;
            line-height: 170px;
            color: #ffffff;
            font-size: 18px;
        }

            .navtext a {
                color: #ffffff;
            }

        .weixinInfoPanel_01 .textField {
            width: 180px;
            height: 180px;
            padding: 0 30px 0 0;
            float: left;
        }

            .weixinInfoPanel_01 .textField span {
                display: block;
                width: 100%;
                height: 100%;
                overflow: hidden;
            }
    </style>
</head>
<body>
   <uc2:topAndmenu ID="topAndmenu1" runat="server" />
    <form id="form1" runat="server">
        <section id="main">
            <div class="titlePanel_01">
                <h1>欢迎您，<%=u_name %></h1>
            </div>
            <div class="weixinInfoPanel_01">
                <asp:Literal ID="lblTopMenu2" runat="server"></asp:Literal>

            </div>
        </section>
        <script src="/Scripts/jquery-1.10.2.min.js"></script>
        <script src="/Scripts/controls.js"></script>
        <script src="/scripts/main.js"></script>
        <script>
            nav.change('<%=m_id%>'); 
        </script>
    </form>
</body>
</html>
