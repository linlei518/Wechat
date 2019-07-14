<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="regoin_account.aspx.cs" Inherits="KDWechat.Web.Account.regoin_account" %>

<%@ Register TagName="TopControl" Src="~/UserControl/TopControl.ascx" TagPrefix="uc1" %>

<%@ Register Src="../UserControl/MenuList.ascx" TagName="Sys_menulist" TagPrefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="UTF-8" />
    <title><%=pageTitle %></title>
    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../scripts/html5.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet" />
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>
<body class="bombbox">
    <form runat="server" id="form1">
        <uc1:TopControl ID="TopControl1" runat="server" />
        <uc2:Sys_menulist ID="MenuList1" runat="server" />
        <section id="main">
                   <%=NavigationName %>
                <div class="titlePanel_01">
                     <div class="btns">
                        <a href="ChildrenAccount_Edit.aspx?m_id=<%= m_id %>" class="btn btn3"><i class="add"></i>新建子帐号</a>
                    </div>
                    <h1><%--<%=parentName %> 下的子帐号--%></h1>
                </div>
            
           

                <div class="tablePanel_01">
                    <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="DataRepeater_ItemCommand">
                        <HeaderTemplate>
                            <table cellpadding="0" cellspacing="0" class="table">
                                <thead>
                                    <tr>
                                        <th class="name">账号名称</th>
                                        <th class="info info1" style=" width:15%">负责人</th>
                                        <th class="info info1" style=" width:60px">状态</th>
                                        <th class="time" style=" width:135px">最后登录时间</th>
                                        <th class="control" style=" width:200px">操作</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>

                        <ItemTemplate>
                            <tr>
                                <td class="name"><%#Eval("user_name") %></td>
                                <td class="info info1"  style=" width:15%"><%#Eval("real_name") %></td>
                                <td class="info info1"  style=" width:60px"><%#((KDWechat.Common.Status)int.Parse(Eval("status").ToString())).ToString() %></td>
                                <td class="time"  style=" width:135px"><%#Eval("login_time") %></td>
                                <td class="control" style=" width:210px">
                                   <%#"<a href='ChildrenAccount_Edit.aspx?id="+Eval("id")+"&m_id="+m_id+"' class=\"btn btn6\" >编辑</a>"%>
                                <asp:Button ID="btnDis" runat="server"  CssClass="btn btn6" CommandName="Disable" CommandArgument='<%#Eval("id")+","+Eval("user_name") %>' OnClientClick='<%#GetConfirmString(Eval("status")) %>' Text='<%#Eval("status").ToString()=="0"?"启用":"禁用" %>' />
                                <asp:Button ID="btnDel" runat="server"  CssClass="btn btn6" CommandName="del" CommandArgument='<%#Eval("id")+","+Eval("user_name") %>'  OnClientClick="return confirm('您确定要删除这条记录？')"  Text="删除" />
                                </td>
                            </tr>
                        </ItemTemplate>

                        <FooterTemplate>
                            <%# Repeater1.Items.Count == 0 ? "<tr><td style=\"text-align:center;\" colspan=\"8\">暂无数据</td></tr>" : ""%>
                            </tbody>
		            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>

                <asp:HiddenField ID="hfReturlUrl" runat="server" />

            </section>
        <script src="../scripts/controls.js"></script>
        <script>
            nav.change('<%=m_id%>'); 
        </script>
    </form>
</body>
</html>
