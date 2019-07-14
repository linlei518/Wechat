<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="module_wx_user_role.aspx.cs" Inherits="KDWechat.Web.module.module_wx_user_role" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link type="text/css" href="../styles/global.css" rel="stylesheet" />
    <link type="text/css" href="../styles/style.css" rel="stylesheet" />
       <script src="../scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
        <script src="../scripts/controls.js" type="text/javascript"></script>
        <script src="../Scripts/function.js"></script>
        <script>
            $(function () {
                var is_parent = parent.$(".listNTab").html();
                if ($.trim(is_parent) == "") {

                    location.href = "/index.aspx?m_id=24";
                }

            })

            $("#btnCancel").click(function () {
                parent.bombbox.closeBox();
            });
            var offsetSize = {//这玩意定义弹出框的高宽
                width: 650,
                height: 450
            }

            function checkAll(btn) {
                var list = $("#chbAllowWechats [type='checkbox']");
                if (!btn.checked) {
                    for (var i = 0; i < list.length; i++)
                        list[i].checked = false;
                }
                else {
                    for (var i = 0; i < list.length; i++)
                        list[i].checked = true;
                }
            }
        </script>
</head>
<body class="bombbox">
    <form id="form1" runat="server">
        <div>
            <header id="bombboxTitle">
                <div class="titlePanel_01">
                    <h1>权限分配</h1>
                </div>
            </header>
            <section id="bombboxMain">
                <div class="listPanel_01 bottomLine">
                    <dl>
                        <dt>子账号：</dt>
                        <dd>
                            <input id="checkall" value="全部" type="checkbox" onclick="checkAll(this)" />
                            全部
                        <asp:CheckBoxList RepeatColumns="3" ID="chbAllowWechats" RepeatDirection="Horizontal" runat="server"></asp:CheckBoxList>
                        </dd>
                    </dl>
                </div>
                <div class="btnPanel_01">
                    <asp:HiddenField ID="hfname" runat="server" />
                    <asp:Button ID="btnSubmit" CssClass="btn btn1" OnClientClick="dialogue.dlLoading();" runat="server" Text="确定" OnClick="btnSubmit_Click" />
                    <input type="button" class="btn btn2" value="取消" onClick="javascript: closeBox();" />
                </div>

            </section>
        </div>

     

    </form>
</body>
</html>
