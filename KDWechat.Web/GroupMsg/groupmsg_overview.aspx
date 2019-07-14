<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="groupmsg_overview.aspx.cs" Inherits="KDWechat.Web.GroupMsg.groupmsg_overview" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../scripts/html5.js"></script>
    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/jquery.form.js"></script>
    <script src="../scripts/selectAddress.js"></script>
    <!--三级联动选择地址的JS-->
    <script src="../scripts/controls.js"></script>
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
                    <h1>选择群发预览接收人</h1>
                </div>
            </header>
            <section id="bombboxMain">
                <div class="listPanel_01 bottomLine">
                    <dl>
                        <dt>微信昵称：</dt>
                        <dd>
                            <asp:TextBox ID="txtNickName" runat="server"></asp:TextBox>
                            <asp:Button ID="btnSearch" runat="server" CssClass="btn btn3" OnClick="btnSearch_Click" Text="查找" />
                        </dd>
                    </dl>
                    <dl runat="server" id="dlResult" style="display:none">
                        <dt>搜索结果：</dt>
                        <dd>
                            <asp:Repeater ID="Repeater1" runat="server">
                                <HeaderTemplate>
                                    <ul>
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <li class="userBaseInfo" id="user<%#Eval("id") %>">
                                        <div class="check">

                                        </div>
                                        <div class="img">
                                            <span>
                                                <img src="<%#Eval("headimgurl") %>" onerror="this.src='../images/logo_01.png'" alt="">
                                            </span>
                                            &nbsp;&nbsp;
                                        </div>
                                        <div class="info">
                                            <h2><em><%#Eval("nick_name") %></em></h2>
                                            <p>性别 ：<%#((KDWechat.Common.WeChatSex)(int.Parse(Eval("sex").ToString()))).ToString() %></p>
                                        </div>
                                        <div class="btns">
                                            <a href="javascript:void(0)" onclick="SelectFans('<%#Eval("open_id") %>','<%#Eval("nick_name") %>')" class="btn btn5">选择</a>
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
    </form>
    <script>
        $("#btnCancel").click(function () {
            parent.bombbox.closeBox();
        });
        function SelectFans(opid,nickname)
        {
            parent.setOpid(opid,nickname);
            parent.bombbox.closeBox();
        }

    </script>
</body>
</html>
