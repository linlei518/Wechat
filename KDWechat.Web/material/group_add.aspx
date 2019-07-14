<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="group_add.aspx.cs" Inherits="KDWechat.Web.material.group_add" %>

<!doctype html>
<html>
<head>
    <meta charset="UTF-8">
    <title>添加分组</title>
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

                <h1><%=id==0?"新建":"编辑" %>素材分组</h1>
            </div>
        </header>
        <section id="bombboxMain">
            <br />
            <div class="listPanel_01 bottomLine longTitle">
                <dl>
                    <dt style="width:100px;">分组名称：</dt>
                    <dd style="margin-left: 100px;">
                        <asp:HiddenField ID="hfhas" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hftitle" runat="server"></asp:HiddenField>
                        <asp:TextBox ID="txtTitle" runat="server" class="txt" MaxLength="50"></asp:TextBox><br/><i>*</i><em>不超过50个字</em></dd>
                </dl>
                <br />
            </div>

            <div class="btnPanel_01">
                <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn1" Text="保存" OnClick="btnSubmit_Click"></asp:Button>
                <input type="button" class="btn btn2" value="取消" onClick="javascript: closeBox();" />
            </div>

        </section>
        <script src="../scripts/jquery-1.6.4.min.js" type="text/javascript"></script>
        <script src="../Scripts/controls.js"></script>
        <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
        <script src="../Scripts/jquery.validate/jquery.validate.js" type="text/javascript"></script>
        <script src="../Scripts/jquery.validate/messages_cn.js" type="text/javascript"></script>
        <script src="../Scripts/function.js"></script>
        <script type="text/javascript">
            var offsetSize = {//这玩意定义弹出框的高宽
                width: 600,
                height: 340
            }
            function ckName(value) {

                $.ajax({
                    type: "POST",
                    async: false,
                    url: "/handles/wechat_ajax.ashx?action=check_group&channel_id=3&old_name=<%=hftitle.Value%>&new_name=" + value,
                    success: function (response) {
                        $("#hfhas").val(response);
                    }
                });
                if ($("#hfhas").val() == "0") {
                    return true;
                } else {
                    return false;
                }

            }
            $(function () {


                $("#btnSubmit").click(function () {
                    if (!$("#txtTitle").val()) {
                        showTip.show("请输入分组名称", true);
                        $("#txtTitle").focus();
                        return false;
                    } else if (!ckName($("#txtTitle").val())) {
                        showTip.show("分组名称已存在", true);
                        $("#txtTitle").focus();
                        return false;
                    }
                    dialogue.dlLoading();//显示Loading
                
                })

            });

        </script>



    </form>
</body>
</html>
