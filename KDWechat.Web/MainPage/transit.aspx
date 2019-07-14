<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="transit.aspx.cs" Inherits="KDWechat.Web.Main.transit" %>

<%@ Import Namespace="KDWechat.Common" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>页面跳转中</title>
    <style>
        body {
            TEXT-ALIGN: center;
            background-color: #ECF4FD;
        }

        #center {
            margin: 15% auto;
            vertical-align: middle;
            border: 1px solid #B8BBC9;
            width: 560px;
            height: 200px;
            line-height: 30px;
        }

            #center img {
                padding-top: 50px;
                width: 100px;
                height: 100px;
            }
    </style>
</head>
<body>

    <div id="center">
        <img src="ajax_loader.gif" />
        <div style="padding-left: 20px;">正在登录跳转中...</div>
    </div>
    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script>
        GetUrlParameters = function (name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return "";
        }
        $(function () {
            $.ajax({
                type: "GET",
                timeout: 60000,
                url: "transit.aspx?type=load&returnUrl=" + GetUrlParameters("url"),
                cache: false,
                success: function (data) {
                    //alert(data);
                   location.href = data;
                },
                error: function () {
                    alert("登陆失败，请重试");
                    //location.reload();
                }
            });
        })
    </script>


</body>
</html>
