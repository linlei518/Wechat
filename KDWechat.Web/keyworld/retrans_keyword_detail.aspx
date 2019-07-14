<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="retrans_keyword_detail.aspx.cs" Inherits="KDWechat.Web.keyworld.retrans_keyword_detail" %>
<%@ Register Src="../UserControl/TopControl.ascx" TagName="TopControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/MenuList.ascx" TagName="MenuList" TagPrefix="uc2" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=pageTitle %></title>
    <script src="../scripts/html5.js"></script>
    <script src="../Scripts/DatePicker/WdatePicker.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet">
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>
<body class="bombbox">
    <form id="form1" runat="server">
       <header id="bombboxTitle">
        <div class="titlePanel_01">
            <h1><%=id==0?"新建":"编辑" %>第三方关键词</h1>
        </div>
           </header>
        <section id="bombboxMain">
        <div class="listPanel_01 bottomLine">
            <dl>
                <dt>关键词名称</dt>
                <dd>
                    <asp:HiddenField ID="hfhas" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hftitle" runat="server"></asp:HiddenField>
                    <asp:TextBox ID="txtTitle" MaxLength="20" runat="server" class="txt"></asp:TextBox><br /><i>*</i><em>不超过20个字,此关键词转发后将不再触发本平台关键词</em>

                </dd>
            </dl>
            <dl>
                <dt>转发至</dt>
                <dd>
                    <asp:DropDownList ID="ddlServer" CssClass="select" AppendDataBoundItems="true" runat="server">
                        <asp:ListItem Value="-1">请选择</asp:ListItem>
                    </asp:DropDownList><br /><i>*</i><em></em>

                </dd>
            </dl>
            <dl>
                <dt>连续转发次数</dt>
                <dd>
                    <asp:TextBox ID="txtRetransTimes" MaxLength="2" runat="server" class="txt"></asp:TextBox><br /><em>在触发关键词后需要连续转发的次数</em>
                </dd>
            </dl>    
        </div>
        <div class="btnPanel_01">
            <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn1" Text="确定" OnClick="btnSubmit_Click"></asp:Button>
            <asp:LinkButton ID="btnCancel" Name="btnCancel" runat="server" CssClass="btn btn2">取消</asp:LinkButton>
        </div>
      </section>
        <script src="../scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
        <script src="../scripts/controls.js" type="text/javascript"></script>
        <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
        <script src="../Scripts/jquery.validate/jquery.validate.js" type="text/javascript"></script>
        <script src="../Scripts/jquery.validate/messages_cn.js" type="text/javascript"></script>
        <script src="../Scripts/function.js" type="text/javascript"></script>
        <script type="text/javascript">
            $(function () {

                $("#form1").validate({
                     submitHandler: function (form) {
                         if ($("#txtTitle").val() == "")
                         {
                             showTip.show('请输入关键词', true);
                             return false;
                         }
                         if ($("#ddlServer").val() == "-1")
                         {
                             showTip.show('请选择需要转发到的第三方服务器', true);
                             return false;
                         }
                         var regTimes = /^\+?[1-9][0-9]*$/

                         if ($("#txtRetransTimes").val() != "" && !regTimes.test($("#txtRetransTimes").val())) {
                             showTip.show('请输入正确的转发次数', true);
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
                width: 840,
                height: 500
            }
        </script>
    </form>
</body>
</html>

