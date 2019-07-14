<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="change_password.aspx.cs" Inherits="KDWechat.Web.Account.change_password" %>

<%@ Register Src="../UserControl/TopControl.ascx" TagName="TopControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/MenuList.ascx" TagName="Sys_menulist" TagPrefix="uc2" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8"/>
    <title><%=pageTitle %></title>
    <script src="../scripts/html5.js"></script>
    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/jquery.validate/jquery.validate.js"></script>
    <script src="../Scripts/jquery.validate/jquery.metadata.js"></script>
    <script src="../Scripts/jquery.validate/messages_cn.js"></script>
    <script type="text/javascript">
        //$(document).ready(function () {
//            $("#form1").validate({
//                rules: {
//                    txtOldPassword: "required",
//                    txtPassword: {
//                        required: true,
//                        minlength: 5
//                    },
//                    txtConfirmPasswrod: {
//                        required: true,
//                        minlength: 5,
//                        equalTo: "#txtPassword"
//                    }
//                }
//            }
//         );
//        });

$(function () {
	$("#btnSubmit").click(function () {
	if ($("#txtOldPassword").val() == "") {
		showTip.show("请输入原密码", true);
		$("#txtOldPassword").focus();
		return false;
	} else if ($("#txtPassword").val() == "") {
	    showTip.show("请输入新密码", true);
		$("#txtPassword").focus();
		return false;
		} else if ($("#txtPassword").val().length<5) {
		showTip.show("长度最少5位", true);
		$("#txtPassword").focus();
		return false;
		} else if ($("#txtPassword").val() != $("#txtConfirmPasswrod").val()) {
		    showTip.show("确认密码输入错误，请重新输入", true);
		$("#txtConfirmPasswrod").focus();
		return false;
		}
		
	})
})
               
    </script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet"/>
    <!--[if lt IE 9 ]><link href="styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>
<body>
    <uc1:TopControl ID="TopControl1" runat="server" />
    <uc2:Sys_menulist ID="MenuList1" runat="server" />
    <form id="form1" runat="server">
        <section id="main">
	        <div class="titlePanel_01">
		        <h1>修改密码</h1>
	        </div>
            <div class="listPanel_01 bottomLine">
		        <dl>
			        <dt>账号名称：</dt>
			        <dd><asp:TextBox CssClass="txt required" ID="txtAccountName" Enabled="false" runat="server"></asp:TextBox></dd>
		        </dl>
                <dl>
			        <dt>原 密 码：</dt>
			        <dd><asp:TextBox ID="txtOldPassword" CssClass="txt required" TextMode="Password" runat="server" MaxLength="15" ></asp:TextBox><i>*</i></dd>
		        </dl>
		        <dl>
			        <dt>新 密 码：</dt>
			        <dd><asp:TextBox ID="txtPassword" CssClass="txt required" TextMode="Password" runat="server" MaxLength="15" ></asp:TextBox><i>*</i><em>5-15位以内</em></dd>
		        </dl>
                <dl>
			        <dt>确认密码：</dt>
			        <dd><asp:TextBox ID="txtConfirmPasswrod" CssClass="txt required" TextMode="Password" runat="server" MaxLength="15" ></asp:TextBox><i>*</i></dd>
		        </dl>
	        </div>
            <%--前连接地址--%>
            <asp:HiddenField ID="hfReturlUrl" runat="server" />
	        <div class="btnPanel_01">
                <asp:Button ID="btnSubmit" CssClass="btn1 btn" runat="server" Text="保存" OnClick="SubmitButtom_Click" />
                <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn btn2"  OnClick="CancelButton_Click">取消</asp:LinkButton>
	        </div>
        </section>
    </form>
    <script src="../scripts/controls.js"></script>
    <script>
        nav.change('<%=m_id%>'); 
    </script>
</body>
</html>
