<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="show_description.aspx.cs" Inherits="KDWechat.Web.module.show_description" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
        <link type="text/css" href="../styles/global.css" rel="stylesheet"/>
    <link type="text/css" href="../styles/style.css" rel="stylesheet"/>

</head>
<body style="background-color:#e8e8e8;">
    <form id="form1" runat="server">
    <div>
        <section id="bombboxMain">
            <div style="padding:30px; word-break:break-all;">
                最后操作事件：<br />
                <%=description %>
            </div>
        </section>
    </div>
        <script>
            var offsetSize = {//这玩意定义弹出框的高宽
                width: 520,
                height: 300
            }

        </script>
    </form>
</body>
</html>
