<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="history_group_msg.aspx.cs" Inherits="KDWechat.Web.GroupMsg.history_group_msg" %>

<!doctype html>
<html>
<head>
    <meta charset="UTF-8">
    <title>查看历史消息</title>
    <link href="css/preview.css" rel="stylesheet" type="text/css">
</head>
<body>
    <div id="container">
        <section id="main" style="bottom: 0;">
            <%=ouputContent %>
        </section>
        <script>
            var links = document.getElementsByTagName('a');
            var container = document.getElementById('container');
            for (var i = 0; i < links.length; i++) {
                links[i].addEventListener('click', linkClickHandler, false);
            }
            function linkClickHandler(e) {
                e.preventDefault();
                container.classList.add('getURL');
                var url = this.href;
                setTimeout(function () {
                    window.location.href = url;
                }, 300);
            }
            function openurl(obj) {
                location.href = "www.baidu.com";
            }

</script>
    </div>
</body>
</html>
