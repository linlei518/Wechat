<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="KDWechat.Web.KDlogin.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <title>微信平台-登录</title>
    <script src="../scripts/html5.js"></script>
    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/controls.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet" />
    <link type="text/css" href="../styles/style.css" rel="stylesheet" />
    <script>
        function getQueryStringByName(name) {
            var result = location.search.match(new RegExp("[\?\&]" + name + "=([^\&]+)", "i"));
            if (result == null || result.length < 1) {
                return "";
            }
            return result[1];
        }

        function ajaxCheck() {
            $(".sibtn").val("登录中。。。");
            $.ajax({
                type: "POST",
                url: "login.aspx?returnUrl=" + getQueryStringByName("returnUrl"),
                data: "txtUsername=" + $("#txtUsername").val() + "&txtPassword=" + $("#txtPassword").val() + "&txtIdentifyCode=" + $("#txtIdentifyCode").val(),
                success: function (msg) {
                    var lis = msg.split(",");
                    if (lis[0] == "0") {
                        alert(lis[1]);
                        $(".sibtn").val("登　　录");
                        if (lis[1] == "用户名，密码错误或账号已被禁用。")
                            $("#identifyCode").click();
                    }
                    else if (lis[0] == "1")
                        location.href = lis[1];
                    else 
                    {
                        alert("当前页面已过期，请重新登录");
                        location.reload();
                    }
                },
                error: function () {
                    alert("登陆失败，请重试");
                    location.reload();
                }
            });
            return false;
        }

        
    </script>
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>
<body class="bodyStyle_01">
    <form runat="server">
        <div class="mgLoginPannel">
        
        <%
            string loginout_url = "";
            string t = KDWechat.Common.RequestHelper.GetQueryString("t");
            if (t == "loginout")
            {
                loginout_url += "<img src='" + System.Configuration.ConfigurationManager.AppSettings["mall_url"] + "' style='width: 1px; height:1px;border: none; background-color: transparent; color: transparent;' />";
            }
            Response.Write(loginout_url);
         %>
            <div class="loginArea">
                <div class="bg">
                    <div class="loginOut">
                        <dl>
                            <dt>用户名/邮箱：</dt>
                            <dd> 
                                <input id="txtUsername" type="text" class="txt" value="" runat="server" /></dd>
                        </dl>
                        <dl>
                            <dt>密码：</dt>
                            <dd>
                                <input id="txtPassword" type="password" class="txt" value="" runat="server" /></dd>
                        </dl>
                        <dl>
                            <dt>验证码：</dt>
                            <dd>
                                <input id="txtIdentifyCode" type="text" class="txt txt2" maxlength="4" value="" runat="server" />
                                <div class="code">
                                    <img id="identifyCode" src="../handles/verify_code.ashx" onclick="this.src=this.src+'?'" width="56" height="23" /></div>
                            </dd>
                        </dl>
                        <div class="signinbtn">
                            <input class="sibtn" onclick="return ajaxCheck()" value="登　　录" type="submit" />
                            <label class="error"></label>
                        </div>

                    </div>
                    <div class="copyright">版权所有：Copyright © 2017.  </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
