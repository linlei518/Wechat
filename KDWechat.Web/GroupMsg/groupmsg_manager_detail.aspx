<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="groupmsg_manager_detail.aspx.cs" Inherits="KDWechat.Web.GroupMsg.groupmsg_manager_detail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../scripts/html5.js"></script>
    <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->

    <link type="text/css" href="../styles/global.css" rel="stylesheet" />
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->

</head>
<body class="bombbox">
    <form id="form1" runat="server">
        <div>
            <header id="bombboxTitle">
                <div class="titlePanel_01">
                    <h1>选择群发管理员</h1>
                </div>
            </header>
            <section id="bombboxMain">
                <div class="listPanel_01 bottomLine">
                    <dl>
                        <dt>微信昵称：</dt>
                        <dd>
                            <asp:TextBox ID="txtNickName" runat="server"></asp:TextBox>
                            <asp:Button ID="btnSearch" runat="server" CssClass="btn btn3"  Text="查找" />
                        </dd>
                    </dl>
                    <dl runat="server" id="dlResult">
                        <dt>搜索结果：</dt>
                        <dd>
                            <asp:Repeater ID="Repeater1" OnItemCommand="Repeater1_ItemCommand" runat="server">
                                <HeaderTemplate>
                                    <ul>
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <li class="userBaseInfo">
                                        <div class="check">

                                        </div>
                                        <div class="img">
                                            <span>
                                                <img src="<%#Eval("avatar").ToString().Replace("/0","/64") %>" onerror="this.src='../images/logo_01.png'" alt="">
                                            </span>
                                            &nbsp;&nbsp;
                                        </div>
                                        <div class="info">
                                            <h2><em><%#Eval("name") %></em></h2>
                                            <p>用户名（唯一） ：<%#Eval("userid") %></p>
                                            <p>微信号 ：<%#Eval("weixinid") %></p>
                                        </div>
                                        <div class="btns">
                                            <asp:Button ID="btnCheck" CommandName="sel" CommandArgument='<%#Eval("userid") %>' runat="server" Text="选择" CssClass="btn btn5" />
                                            <%--<a href="javascript:void(0)" onclick="SelectFans('<%#Eval("userid") %>','<%#Eval("name") %>')" class="btn btn5">选择</a>--%>
                                        </div>
                                    </li>
                                </ItemTemplate>

                                <FooterTemplate>
                                    <%# Repeater1.Items.Count == 0 ? "<div style='background-color:white;border-top:10px solid #C7CED3'><div style=\"text-align:center;\" colspan=\"8\">暂无数据</div></div>" : ""%>
                                    </ul>
                                </FooterTemplate>
                            </asp:Repeater>
                        </dd>
                    </dl>
                </div>
        <input id="excelfileName" name="excelfileName" type="hidden" value="" />

        <div class="btnPanel_01">
            <asp:LinkButton ID="btnCancel" Name="btnCancel" runat="server" CssClass="btn btn2">取消</asp:LinkButton>
        </div>

        </section>
        </div>
        <script src="../scripts/jquery-1.10.2.min.js"></script>
        <script src="../scripts/controls.js"></script>
        <script src="../Scripts/function.js"></script>

    </form>
    <script>
        $("#btnCancel").click(function () {
            parent.bombbox.closeBox();
        });

    </script>
</body>
</html>



<%--<!DOCTYPE html>
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
            <h1><%=id==0?"新建":"编辑" %>群发管理员</h1>
        </div>
           </header>

        <section id="bombboxMain">
            <div class="listPanel_01 bottomLine">
            <dl>
                <dt>用户名（唯一）</dt>
                <dd>
                    <asp:HiddenField ID="hfhas" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hftitle" runat="server"></asp:HiddenField>
                    <asp:TextBox ID="txtUsername" MaxLength="30" runat="server" class="txt"></asp:TextBox><br /><i>*</i><em>不超过30个字</em>

                </dd>
            </dl>
            <dl>
                <dt>用户昵称</dt>
                <dd>
                    <asp:TextBox ID="txtNickname" MaxLength="20" runat="server" class="txt"></asp:TextBox><br />

                </dd>
            </dl>            
            <dl>
                <dt>微信号</dt>
                <dd>
                    <asp:TextBox ID="txtWeChatNo" MaxLength="50" runat="server" class="txt"></asp:TextBox><br />
                </dd>
            </dl>
            <dl>
                <dt>手机号</dt>
                <dd>
                    <asp:TextBox ID="txtMobile" MaxLength="11" runat="server" class="txt"></asp:TextBox><br />
                </dd>
            </dl>
            <dl>
                <dt>邮箱地址</dt>
                <dd>
                    <asp:TextBox ID="txtEmail" MaxLength="50" runat="server" class="txt"></asp:TextBox><br />
                </dd>
            </dl>
          

            <asp:HiddenField ID="hfReturlUrl" runat="server" />
            </div>
            <div class="btnPanel_01">
            <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn1" Text="确定" OnClick="btnSubmit_Click"></asp:Button>
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

            $(function () {

                $("#form1").validate({
                    submitHandler: function (form) {
                         var re = new RegExp("^[a-z,A-Z,0-9]+$");//判断是否为英文字母
                         if ($("#txtUsername").val() == "") {
                             showTip.show('请输入用户名', true);
                             return false;
                         }
                         else if (!re.test($("#txtUsername").val())) {
                             showTip.show('用户名只能为英文及数字', true);
                             return false;
                         }
                         if ($("#txtNickname").val() == "") {
                             showTip.show('请输入昵称', true);
                             return false;
                         }

                         if ($("#txtWeChatNo").val() == "" && $("#txtMobile").val() == "" && $("#txtEmail").val() == "") {
                             showTip.show('请至少输入微信号，手机号或emali中的一项', true);
                             return false;
                         }
                         var mobile = /^(13[0-9]{9})|(15[0-9]{9})|(18[0-9]{9})|(14[0-9]{9})|(170[0-9]{8})$/;
                         if ($("#txtMobile").val() != "" && !mobile.test($("#txtMobile").val())) {
                             showTip.show('请输入正确的手机号码',true);
                             return false;
                         }
                         var email = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
                         if ($("#txtEmail").val()!="" && !email.test($("#txtEmail").val())) {
                             showTip.show("请输入正确邮箱格式", true);
                             $("#txtEmail").focus();
                             return false;
                         }

                        form.submit();
                    }
                });
            });
            $("#btnCancel").click(function () {
                location.href="groupmsg_manager_list.aspx?m_id=<%=m_id%>";
            });
            var offsetSize = {//这玩意定义弹出框的高宽
                width: 840,
                height: 500
            }

            nav.change('<%=m_id%>');
        </script>

</body>
</html>--%>
