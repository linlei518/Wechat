<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChildrenAccount.aspx.cs" Inherits="KDWechat.Web.main.ChildrenAccount" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <title><%=pageTitle %></title>
    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../scripts/html5.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet" />
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>
<body>
    <form runat="server" id="form1">

            <header id="bombboxTitle">
                <div class="titlePanel_01">
                    <h1><%=parentName %> 下的子帐号</h1>
                </div>
            </header>
            <section id="bombboxMain">

                <div class="tablePanel_01">
                    <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="DataRepeater_ItemCommand">
                        <HeaderTemplate>
                            <table cellpadding="0" cellspacing="0" class="table">
                                <thead>
                                    <tr>
                                        <th class="name">账号名称</th>
                                        <th class="info info1">负责人</th>
                                        <th class="info info1">状态</th>
                                        <th class="time">最后登录时间</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>

                        <ItemTemplate>
                            <tr>
                                <td class="name"><%#Eval("user_name") %></td>
                                <td class="info info1"><%#Eval("real_name") %></td>
                                <td class="info info1"><%#((KDWechat.Common.Status)int.Parse(Eval("status").ToString())).ToString() %></td>
                                <td class="time"><%#Eval("login_time") %></td>

                            </tr>
                        </ItemTemplate>

                        <FooterTemplate>
                            <%# Repeater1.Items.Count == 0 ? "<tr><td style=\"text-align:center;\" colspan=\"4\">暂无数据</td></tr>" : ""%>
                            </tbody>
		            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>

                <asp:HiddenField ID="hfReturlUrl" runat="server" />

            </section>
        <script src="../scripts/controls.js"></script>
        <script>
            function rediec(id) {
                parent.bombbox.closeBox();
                parent.location.href = "ChildrenAccount_Edit.aspx?id=" + id + "&m_id=<%=m_id %>";
                return false;
            }
            nav.change('<%=m_id%>');
        </script>
    </form>
</body>
</html>
