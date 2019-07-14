<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="letter_detail.aspx.cs" Inherits="KDWechat.Web.Account.letter_detail" %>

<%@ Register TagName="TopControl" Src="~/UserControl/TopControl.ascx" TagPrefix="uc" %>

<%@ Register Src="~/UserControl/MenuList.ascx" TagName="Sys_menulist" TagPrefix="uc2" %>

<!doctype html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="UTF-8" />
    <title><%=pageTitle %></title>
    <script src="../scripts/html5.js"></script>
    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet" />
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>
<body>
    <uc:TopControl ID="TopControl1" runat="server" />
    <uc2:Sys_menulist ID="MenuList1" runat="server" />
    <form id="form1" runat="server">
        <section id="main">
           <%=NavigationName %>
            <div class="titlePanel_01">
                <div class="btns">
                    <a href='<%=ReturlUrl %>' class="btn btn5"><i class="back black"></i>返回列表</a>
                </div>
                <h1>站内信内容</h1>
            </div>

            <div class="pmDetailPanel_01">
                <div class="titleField">
                    <h1><%=title %></h1>
                    <h2><%=time %></h2>
                </div>

                <div class="textField">
                  <%=contents %>
                </div>


            </div>
        </section>
    </form>
    <script src="../scripts/controls.js"></script>

    <script>
        nav.change('<%=mid%>'); 
    </script>
</body>
</html>
