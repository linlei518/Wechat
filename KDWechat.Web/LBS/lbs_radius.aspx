<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="lbs_radius.aspx.cs" Inherits="KDWechat.Web.lbs_radius" %>

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
            <h1>修改搜索半径</h1>
        </div>
           </header>
        <section id="bombboxMain">
        <div class="listPanel_01 bottomLine">
            <dl>
                <dt>搜索半径（米）：</dt>
                <dd>
                    <asp:TextBox ID="txtRadius" MaxLength="8" runat="server" class="txt digits required"></asp:TextBox>

                </dd>
            </dl>
        </div>
        <div class="btnPanel_01">
            <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn1" OnClientClick="return checkRad()" Text="确定" OnClick="btnSubmit_Click"></asp:Button>
            <asp:LinkButton ID="btnCancel" Name="btnCancel" runat="server" CssClass="btn btn2">取消</asp:LinkButton>
        </div>
      </section>
        <script src="../scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
        <script src="../scripts/controls.js" type="text/javascript"></script>
        <script src="../Scripts/jquery.validate/jquery.validate.js" type="text/javascript"></script>
        <script src="../Scripts/jquery.validate/messages_cn.js" type="text/javascript"></script>
        <script>
            function checkRad()
            {
                var rad = $("#txtRadius").val();
                if (!Number(rad))
                {
                    alert("请输入整数！");
                    return false;
                }
                if (rad > 5000000)
                {
                    alert("范围不能大于5,000,000");
                    return false;
                }
                return true;
            }
            $("#btnCancel").click(function () {
                parent.bombbox.closeBox();
            });
            var offsetSize = {//这玩意定义弹出框的高宽
                width: 520,
                height: 300
            }
        </script>
    </form>
</body>
</html>

