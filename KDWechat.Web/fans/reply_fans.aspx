<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="reply_fans.aspx.cs" Inherits="KDWechat.Web.fans.reply_fans" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>回复粉丝</title>
    <script src="../scripts/html5.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet">
    <!--[if lt IE 9 ]><link href="styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>
<body class="bombbox">
    <header id="bombboxTitle">
        <div class="titlePanel_01">
            <h1>快速回复</h1>
        </div>
    </header>
    <form id="form1" runat="server">
        <section id="bombboxMain">
            <%= (wx_type==1||wx_type==3)?"<h1>您的公众号未认证，无法使用此功能！</h1>":""%>
            <div class="listPanel_01">
                <dl>
                    <dt>回复内容：</dt>
                    <dd>
                        <textarea name="txtContent" runat="server" maxlength="140" id="txtContent" class="textarea" style="width: 550px;"></textarea>
                        <br />
                        <i>*</i><em>不超过140个字</em>
                    </dd>
                </dl>
            </div>
            <div class="btnPanel_01 longTitle">
                <asp:Button ID="btnSubmit" CssClass="btn btn1" runat="server" Text="发送" OnClick="btnSubmit_Click"></asp:Button>
                <input type="button" class="btn btn2" value="取消" onclick="javascript: closeBox();">
            </div>
        </section>
        <asp:HiddenField ID="_f" runat="server" />
        <asp:HiddenField ID="c_" runat="server" />
        <asp:HiddenField ID="k_" runat="server" />
        <asp:HiddenField ID="_u" runat="server" />
        <script src="../scripts/jquery-1.10.2.min.js"></script>
        <script src="../scripts/controls.js"></script>
        <script src="../Scripts/function.js"></script>
        <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
        <!--如果页面table标签内内容更新 请在更新好后调用一次setupTable方法-->
        <script>
            var offsetSize = {//这玩意定义弹出框的高宽
                width: 800,
                height: 410
            }
            $(function () {
                $("textarea[maxlength]").bind('input propertychange', function () {
                    var maxLength = $(this).attr('maxlength');
                    if ($(this).val().length > maxLength) {
                        $(this).val($(this).val().substring(0, maxLength));
                    }
                });
                $("#btnSubmit").click(function () {
                    if ($("#txtContent").val() == "") {
                        showTip.show("请输入回复内容", true);
                        $("#txtContent").focus();
                        return false;
                    }
                    dialogue.dlLoading();//显示Loading
                })
            })

        </script>
    </form>
</body>
</html>
