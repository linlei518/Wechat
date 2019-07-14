<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WXList.aspx.cs" Inherits="KDWechat.Web.main.WXList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <title><%=pageTitle %></title>
    <script src="../scripts/html5.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet" />
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>
<body>
    <form id="form1" runat="server">


        <header id="bombboxTitle">
            <div class="titlePanel_01">
                <div class="btns">
<%--                    <a href="javascript:rediec();" class="btn btn3">新建微信公众号</a>--%>
                </div>
                <h1><%=parentName %> 管理的微信账号</h1>
            </div>
        </header>
        <section id="bombboxMain">
            <div class="tablePanel_01">
                <table cellpadding="0" cellspacing="0" class="table">
                    <thead>
                        <tr>
                            <th class="name">名称</th>
                            <th class="info info1">类型</th>
                            <th class="time">创建时间</th>
<%--                            <th class="control">操作</th>--%>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">
                            <ItemTemplate>
                                <tr>
                                    <td class="name">
                                        <a href='#'><%#Eval("wx_pb_name") %></a>
                                    </td>
                                    <td class="info info1"><%#(KDWechat.Common.WeChatServiceType)(int.Parse(Eval("type_id").ToString())) %></td>
                                    <td class="time"><%#Eval("create_time") %></td>
<%--                                    <td class="control">
                                        <a href='NewWXAccount.aspx?id=<%#Eval("id") %>' class="btn btn6">编辑</a>
                                        <asp:Button ID="btnDelete" CssClass="btn btn6" CommandName="del" CommandArgument='<%#Eval("id") %>' OnClientClick="return confirm('你确定要删除这条记录？')" runat="server" Text="删除" />
                                    </td>--%>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                <%# Repeater1.Items.Count == 0 ? "<tr><td style=\"text-align:center;\" colspan=\"8\">暂无数据</td></tr>" : ""%>
                            </FooterTemplate>
                        </asp:Repeater>
                        <asp:HiddenField ID="hfReturlUrl" runat="server" />
                    </tbody>
                </table>
                <div class="pageNum" id="div_page" runat="server">
                </div>
            </div>
        </section>

        <script src="../scripts/jquery-1.10.2.min.js"></script>
        <script src="../scripts/controls.js"></script>
        <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
        <script>
            nav.change('<%=m_id%>'); 
        </script>
    </form>
</body>
</html>
