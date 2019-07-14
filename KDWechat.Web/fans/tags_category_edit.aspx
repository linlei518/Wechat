<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="tags_category_list.aspx.cs" Inherits="KDWechat.Web.fans.tags_category" %>

<%@ Register Src="../UserControl/TopControl.ascx" TagName="TopControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/MenuList.ascx" TagName="MenuList" TagPrefix="uc2" %>

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
                <h1><%=id==0?"新建":"编辑" %>标签分类</h1>
            </div>
        </header>
        <section id="bombboxMain">
            <br />
            <br />
            <div class="listPanel_01 bottomLine">
                <dl>
                    <dt>分类名称：</dt>
                    <dd>
                        <asp:HiddenField ID="hfhas" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hftitle" runat="server"></asp:HiddenField>
                        <asp:TextBox ID="txtTitle" runat="server" class="txt required" MaxLength="50"></asp:TextBox><br /><i>*</i><em>不超过50个字</em></dd>
                </dl>
                <br />
                <%--<dl>
			        <dt>标签说明</dt>
			        <dd>
                        <textarea id="txtContents" runat="server" name="content"   class="textarea"></textarea>
                        <label id="lblContent" class="error" style="display: inline;"></label>
                       </dd>
		        </dl>--%>
            </div>
            <div class="btnPanel_01">
                <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn1" Text="保存" OnClick="btnSubmit_Click"></asp:Button>
                <asp:LinkButton ID="btnCancel" Name="btnCancel" runat="server" CssClass="btn btn2">取消</asp:LinkButton>
            </div>
        </section>
        <script src="../scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
        <script src="../Scripts/jquery.form.js" type="text/javascript"></script>
        <script src="../scripts/controls.js" type="text/javascript"></script>
        <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->

        <script src="../Scripts/function.js" type="text/javascript"></script>
        <script type="text/javascript">
          
            $(function () {
                $("#btnSubmit").click(function () {

                    if ($("#txtTitle").val() == "") {
                        showTip.show("请输入标签类型名称", true);
                        $("#txtTitle").focus();
                        return false;
                    }
                    dialogue.dlLoading();//显示Loading
                })
               


            })
            var offsetSize = {//这玩意定义弹出框的高宽
                width: 620,
                height: 300
            }
            $("#btnCancel").click(function() {
                parent.bombbox.closeBox();
            });
         
        </script>
    </form>
</body>
</html>
