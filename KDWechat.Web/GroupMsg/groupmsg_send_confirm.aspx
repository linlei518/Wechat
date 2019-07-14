<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="groupmsg_send_confirm.aspx.cs" Inherits="KDWechat.Web.GroupMsg.groupmsg_send_confirm" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=pageTitle %></title>
    <script src="../scripts/html5.js"></script>
    <link href="../editor/themes/default/default.css" rel="stylesheet" type="text/css" />
    <link type="text/css" href="../styles/global.css" rel="stylesheet">
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>
<body class="bombbox">
        <form id="form1" runat="server">
        <header id="bombboxTitle">
        <div class="titlePanel_01">
            <h1>审核群发</h1>
        </div>
           </header>

        <section id="bombboxMain">
            <div class="listPanel_01 bottomLine">
            <dl>
                <dt>验证码</dt>
                <dd>
                    <asp:HiddenField ID="hfhas" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hftitle" runat="server"></asp:HiddenField>
                    <asp:TextBox ID="txtConfirmCode" MaxLength="6" runat="server" class="txt"></asp:TextBox><br /><i>*</i><em>不超过6个字</em>
                    
                </dd>
            </dl>
          
            <%--前连接地址--%>
            <asp:HiddenField ID="hfReturlUrl" runat="server" />
            </div>
            <div class="btnPanel_01">
            <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn1" Text="确定" OnClientClick="SetDisabled()" OnClick="btnSubmit_Click"></asp:Button>
                <span id="showTip"></span>
            <a ID="btnCancel" Name="btnCancel" class="btn btn2">取消</a>
            </div>

        </section>
       

        <script src="../scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
        <script src="../scripts/controls.js" type="text/javascript"></script>
        <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
        <script src="../Scripts/jquery.validate/jquery.validate.js" type="text/javascript"></script>
        <script src="../Scripts/jquery.validate/messages_cn.js" type="text/javascript"></script>
        <script src="../Scripts/function.js" type="text/javascript"></script>
        <script src="../Scripts/Bombbox.js"></script>
    </form>
        <script type="text/javascript">

            function SetDisabled()
            {
                $("#btnSubmit").attr("style", "display:none");
                $("#showTip").html("发送中......");
            }

            $(function () {

                $("#form1").validate({
                     submitHandler: function (form) {
                         if ($("#txtConfirmCode").val() == "")
                         {
                             showTip.show('请输入验证码！', true);
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
                width: 600,
                height: 300
            }

            nav.change('<%=m_id%>');
        </script>

</body>
</html>
