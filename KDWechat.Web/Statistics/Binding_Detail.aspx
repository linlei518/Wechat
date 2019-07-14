<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Binding_Detail.aspx.cs" Inherits="KDWechat.Web.Statistics.Binding_Detail" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../scripts/html5.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet" />
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->

</head>
<body class="bombbox">
    <header id="bombboxTitle">
        <div class="titlePanel_01">
            <h1>互动详情</h1>
        </div>
    </header>
    <form id="form1" runat="server">
        <section id="bombboxMain">

            <div class="tablePanel_01">
                <table cellpadding="0" cellspacing="0" class="table">
                    <thead>
                        <tr>
                            <th class="time">最后一条消息</th>
                            <th class="time">最后一次浏览页面</th>
                            <th class="time">最后一次上报地理位置</th>
<%--                            <th class="time">最后一次关注</th>--%>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td class="time"><%=GetLastTime(1) %></td>
                            <td class="time"><%=GetLastTime(2) %></td>
                            <td class="time"><%=GetLastTime(3) %></td>
<%--                            <td class="time"><%=GetLastTime(4) %></td>--%>
                        </tr>

                    </tbody>
                </table>
            </div>
        </section>

        <script src="../scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
        <script src="../scripts/jquery.ba-resize.min.js"></script>
        <script src="../scripts/controls.js" type="text/javascript"></script>
        <script src="../Scripts/function.js"></script>
    </form>
    <script type="text/javascript">

    </script>
    <script src="../Scripts/HighChart/js/themes/KDTheme.js"></script>
</body>
</html>
