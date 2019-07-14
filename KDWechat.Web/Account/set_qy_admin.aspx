<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="set_qy_admin.aspx.cs" Inherits="KDWechat.Web.Account.set_qy_admin" %>

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
                    <h1>选择签到管理员</h1>
                </div>
            </header>
            <section id="bombboxMain">
                 <div class="searchPanel_01">
                 <div class="searchField">
                    <input type="text" class="txt searchTxt" placeholder="微信昵称..." runat="server" id="txtKeywords">
                    <asp:Button ID="btnSearch" runat="server" Text="" CssClass="btn searchBtn" ToolTip="点击搜索" OnClick="btnSearch_Click" OnClientClick="return  check_keys()" ></asp:Button>
                </div>
                     </div>
                <div class="listPanel_01 bottomLine">
                    <dl runat="server" id="dlResult">
                        <dt>请选择：</dt>
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
                                                <img src="<%#Eval("headimgurl").ToString().Replace("/0","/64") %>" onerror="this.src='../images/logo_01.png'" alt="">
                                            </span>
                                            &nbsp;&nbsp;
                                        </div>
                                        <div class="info">
                                            <h2><em><%#Eval("nick_name") %></em></h2>
                                         <%--   <p>用户名（唯一） ：<%#Eval("userid") %></p>
                                            <p>微信号 ：<%#Eval("weixinid") %></p>--%>
                                        </div>
                                        <div class="btns">
                                            <asp:Button ID="btnCheck" CommandName="sel" CommandArgument='<%#Eval("open_id") %>' runat="server" Text="选择" CssClass="btn btn5" />
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

        function check_keys() {
            if ($("#txtKeywords").val() == "") {
                alert("请输入微信昵称！");
                return false;
            }
            return true;
        }

    </script>
</body>
</html>

