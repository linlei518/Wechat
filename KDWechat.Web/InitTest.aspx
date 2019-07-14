<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InitTest.aspx.cs" Inherits="KDWechat.Web.InitTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link type="text/css" href="styles/global.css" rel="stylesheet"/>
    <link type="text/css" href="styles/style.css" rel="stylesheet"/>
    <script src="Scripts/jquery.validate/jquery.validate.js"></script>
    <script src="Scripts/jquery.validate/jquery.metadata.js"></script>
    <script src="Scripts/jquery.validate/messages_cn.js"></script>

</head>
<body>
        <form id="form2" runat="server">
    <div>
        <header id="bombboxTitle">
            <div class="titlePanel_01">
                <h1>导入多图文素材</h1>
            </div>
        </header>
        <section id="bombboxMain">
            <div class="listPanel_01 bottomLine">
                <dl>
                    <dt>用户名：</dt>
                    <dd>
                        <asp:TextBox CssClass="txt required clearAll" ID="TextBox1" runat="server"></asp:TextBox>
                    </dd>
                </dl>       
                <dl>
                    <dt>密  码：</dt>
                    <dd>
                        <asp:TextBox CssClass="txt required clearAll" ID="TextBox2" TextMode="Password" runat="server"></asp:TextBox>                    
                    </dd>
                </dl>
                <div>
                    说明：请输入您的微信公众号及密码（与<a href="http://mp.weixin.qq.com" target="_blank">mp.weixin.qq.com</a>相同）；
                </div>                        
            </div>
            <div class="btnPanel_01">
                <asp:Button ID="btnSubmit" CssClass="btn btn1" runat="server" Text="确定" OnClick="Button1_Click" />
                <asp:LinkButton ID="btnCancel" Name="btnCancel" runat="server" CssClass="btn btn2">取消</asp:LinkButton>
            </div>
        </section>
    </div>
    </form>
    <script src="../scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../scripts/controls.js" type="text/javascript"></script>
    <script>
        $("#btnCancel").click(function () {
            parent.bombbox.closeBox();
        });

        $("#btnSubmit").click(function () {
            if (!$("#TextBox1").val()) {
                showTip.show("请输入微信公众号", true);
                $("#TextBox1").focus();
                return false;
            } else if (!$("#TextBox2").val()) {
                showTip.show("请输入密码", true);
                $("#TextBox2").focus();
                return false;
            }
            dialogue.dlLoading();
        })
        setTimeout(function () {
            var list = $(".clearAll");
            for (i = 0; i < list.length; i++) $
                (list[i]).val("");
        }, 100);

    </script>

</body>
</html>
