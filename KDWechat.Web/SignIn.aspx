<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignIn.aspx.cs" Inherits="KDWechat.Web.SignIn" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>美蓓亚签到</title>
    <script src="Scripts/jquery-1.6.4.min.js"></script>
       <link type="text/css" href="/styles/global.css" rel="stylesheet">
    <link type="text/css" href="/styles/style.css" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server" action="">
    <div>
    
    </div>
    </form>
</body>
</html>
<script src="/scripts/controls.js"></script><!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
        <script src="/scripts/Bombbox.js"></script>
        <script src="/scripts/statisticsContrast.js"></script>

<script type="text/javascript">
    $(function () {
        var is_show = "<%=is_show%>";
        if (is_show == "0") {
            if (confirm("您确定要签到该邀请函？")) {
                $("#form1").submit();
            }
        }
    });

</script>