<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeBehind="qrcode_edit.aspx.cs" Inherits="KDWechat.Web.qrcode.qrcode_edit" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=pageTitle %></title>
    <script src="../scripts/html5.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet">
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>
<body class="bombbox">
    <form id="form1" runat="server">
       <header id="bombboxTitle">
        <div class="titlePanel_01">
            <h1><%=id==0?"新建":"编辑" %>二维码</h1>
        </div>
           </header>
        <section id="bombboxMain">
            <%= (wx_type==1||wx_type==3)?"<h1>您的公众号未认证，无法使用此功能！</h1>":""%>
        <div class="listPanel_01 bottomLine">
            <dl>
                <dt>二维码名称</dt>
                <dd>
                    <asp:HiddenField ID="hfhas" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hftitle" runat="server"></asp:HiddenField>
                    <asp:TextBox ID="txtTitle" MaxLength="15" runat="server" class="txt"></asp:TextBox>

                </dd>
            </dl>
        </div>
        <div class="btnPanel_01">
            <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn1" Text="确定" OnClick="btnSubmit_Click"></asp:Button>
            <asp:LinkButton ID="btnCancel" Name="btnCancel" runat="server" CssClass="btn btn2">取消</asp:LinkButton>
        </div>
      </section>
        <script src="../scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
        <script src="../Scripts/jquery.form.js" type="text/javascript"></script>
        <script src="../scripts/controls.js" type="text/javascript"></script>
        <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
        <script src="../Scripts/jquery.validate/jquery.validate.js" type="text/javascript"></script>
        <script src="../Scripts/jquery.validate/messages_cn.js" type="text/javascript"></script>
        <script src="../Scripts/function.js" type="text/javascript"></script>


        <script type="text/javascript">
            $(function () {
                
                $("#form1").validate({
                    submitHandler: function (form) {
                        var tit = $("#txtTitle");
                        if (tit.val() == "")
                        {
                            showTip.show('请输入二维码名称', true);
                            tit.focus();
                            return false;
                        }

                        form.submit();
                    }
                });
            });
            $("#btnCancel").click(function () {
                parent.bombbox.closeBox();
            });
            var offsetSize = {//这玩意定义弹出框的高宽
                width: 620,
                height: 300
            }
        </script>
    </form>
</body>
</html>

