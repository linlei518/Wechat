<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="menu_edit_name.aspx.cs" Inherits="KDWechat.Web.keyworld.menu_edit_name" %>

<!doctype html>
<html>
<head>
    <meta charset="UTF-8">
    <title>新建或编辑菜单名称</title>
    <script src="../scripts/html5.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet">
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>

<body class="bombbox">
    <form runat="server" id="form1">
        <header id="bombboxTitle">
            <div class="titlePanel_01">
                <h1><%--<%=id>0?"编辑":"新建" %>菜单名称--%>输入提示框</h1>
            </div>
        </header>
        <section id="bombboxMain">
            <div class="inputPanel_01">


                <div class="inputField">
                    <p style="text-align: left; padding: 8px;"><%=(hfparentid.Value=="0"?"菜单名称名字不多于8个汉字或16个字母:":"菜单名称名字不多于8个汉字或16个字母:") %></p>
                    <input type="text" class="txt" placeholder="请输入菜单名称" id="txtName" runat="server">
                    <asp:HiddenField ID="hftitle" runat="server" />
                    <asp:HiddenField ID="hfparentid" runat="server" />
                    <asp:HiddenField ID="hfhas" runat="server" />
                </div>
                <div class="btnField">
                    <input type="button" id="btnSave" class="btn btn1" value="保存">
                    <input type="button" class="btn btn2" value="取消" onclick="window.parent.bombbox.closeBox();">
                </div>


            </div>
        </section>

        <script src="../scripts/jquery-1.10.2.min.js"></script>
        <script src="../scripts/controls.js"></script>
        <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
        <!--如果页面table标签内内容更新 请在更新好后调用一次setupTable方法-->

        <script>
            $(function () {
                $("#btnSave").click(function () {
                    dialogue.dlLoading();//显示Loading
                    var isc = true;
                    if (!$("#txtName").val()) {
                        isc = false;
                        dialogue.closeAll();//隐藏Loading
                        //dialogue.dlAlert('请输入菜单名称');
                        showTip.show("请输入菜单名称", true);
                        // window.parent.dialogue.dlAlert("请输入菜单名称");
                    }
                    if (isc) {

                        $.ajax({
                            type: "POST",
                            url: "/handles/wechat_ajax.ashx?action=diy_meun&type=<%=(id>0?"edit":"add")%>&parent_id=<%=hfparentid.Value%>&id=<%=id%>",
                            timeout: 60000,
                            dataType: 'json',
                            data: { menu_name: $("#txtName").val(), old_name: "<%=hftitle.Value%>" },
                            beforeSend: function () {

                            },
                            success: function (data) {
                                dialogue.closeAll();//隐藏Loading
                                if (data.status == 1) {
                                    window.parent.showTip.show("保存成功", false);
                                    window.parent.loadList();
                                    window.parent.bombbox.closeBox();
                                } else {
                                    showTip.show(data.msg, true);
                                }


                            },
                            error: function (data, status, e) {
                                dialogue.closeAll();//隐藏Loading
                                showTip.show("保存失败", true);
                            }
                        });

                    }



            })
        })

            function ck_name() {
                $.ajax({
                    type: "POST",
                    async: false,
                    url: "/handles/wechat_ajax.ashx?action=check_exists_name&parent_id=<%=hfparentid.Value%>",
                data: { tb: "<%=KDWechat.Common.DESEncrypt.Encrypt("t_wx_diy_menus")%>", prefix: "<%=KDWechat.Common.DESEncrypt.Encrypt("kd_wechats")%>", new_name: $("#txtName").val(), old_name: "<%=hftitle.Value%>" },
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

        </script>
        <script>
            var offsetSize = {
                width: 490,
                height: 260
            }
        </script>
    </form>
</body>
</html>
