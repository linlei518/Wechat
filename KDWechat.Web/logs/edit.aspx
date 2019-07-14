<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="edit.aspx.cs" Inherits="KDWechat.Web.logs.edit" %>
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
<body>
    <form id="form1" runat="server">
            <div class="titlePanel_01">
		        <h1>查看操作日志详情</h1>
	        </div>
	
	        <div class="listPanel_01 bottomLine">
                 <dl>
			        <dt>操作用户</dt>
			        <dd><area ID="txtUName" runat="server" class="txt" ReadOnly="True"></area></dd>
		        </dl>
                 <dl>
			        <dt>操作内容</dt>
			        <dd><area ID="txtContents" runat="server" class="txt" ReadOnly="True"></area></dd>
		        </dl>
                 <dl>
			        <dt>操作IP</dt>
			        <dd><area  ID="txtIP" runat="server" class="txt" ReadOnly="True"></area></dd>
		        </dl>
                 <dl>
			        <dt>操作时间</dt>
			        <dd><area ID="txtTime" runat="server" class="txt" ReadOnly="True"></area></dd>
		        </dl>
            </div>
            <div class="btnPanel_01">                
                <asp:LinkButton ID="btnSubmit" Name="btnSubmit" runat="server" CssClass="btn btn1">关闭</asp:LinkButton>
	        </div>
        <script src="../scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
        <script src="../Scripts/jquery.form.js" type="text/javascript"></script>
        <script src="../scripts/controls.js" type="text/javascript"></script>
        <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
        <script src="../Scripts/jquery.validate/jquery.validate.js" type="text/javascript"></script>
        <script src="../Scripts/jquery.validate/messages_cn.js" type="text/javascript"></script>
        <script src="../editor/kindeditor-min.js" type="text/javascript"></script>
        <script src="../editor/lang/zh_CN.js" type="text/javascript"></script>
        <script src="../Scripts/function.js" type="text/javascript"></script>
        <script type="text/javascript">
            $("#btnSubmit").click(function () {
                parent.bombbox.closeBox();
            });
        </script>
    </form>
</body>
</html>
