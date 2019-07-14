<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Binding_List.aspx.cs" Inherits="KDWechat.Web.Statistics.Binding_List" %>

<%@ Register Src="../UserControl/TopControl.ascx" TagName="TopControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/MenuList.ascx" TagName="Sys_menulist" TagPrefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
        <uc1:TopControl ID="TopControl1" runat="server" />
        <uc2:Sys_menulist ID="MenuList1" runat="server" />
        <section id="main">
            <%=NavigationName %>
                <div class="tablePanel_01">
                    <table cellpadding="0" cellspacing="0" class="table">
                        <thead>
                            <tr>
                                <th class="name">公众号</th>
                                <th class="info info1" style=" width:55px">目前接入</th>
                                <th class="info info1" style=" width:80px">本周新增会员</th>
                                <th class="info info1" style=" width:80px">本周流失会员</th>
                                <th class="time" style=" width:115px">最后互动时间</th>
                                <th class="contro2" style=" width:60px; padding:0 10px">互动详情</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="Repeater1" OnItemDataBound="Repeater1_ItemDataBound" runat="server" >
                                <ItemTemplate>
                                    <tr style='<%# GetDisplay(Eval("id")) %>'>
                                        <td class="name">
                                           <a href="javascript:bombbox.openBox('binding_detail.aspx?id=<%#Eval("id") %>');"> <%#Eval("wx_pb_name") %></a>
                                        </td>
                                        <td class="info info1"><%#GetStatus(Eval("id")) %></td>
                                        <td class="info info1"><%#GetSCount(Eval("id")) %></td>
                                        <td class="info info1"><%#GetUSCount(Eval("id")) %></td>
                                        <td class="time" style=" width:115px"><%#GetLastTime( Eval("id")) %></td>
                                        <td class="contro2" style=" width:110px; padding:0 10px">
                                            <a class="btn btn6" href="javascript:bombbox.openBox('binding_detail.aspx?id=<%#Eval("id") %>');">查看详情</a>    
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <%# Repeater1.Items.Count == 0 ? "<tr><td style=\"text-align:center;\" colspan=\"4\">暂无数据</td></tr>" : ""%>
                                </FooterTemplate>
                            </asp:Repeater>
                            <asp:HiddenField ID="hfReturlUrl" runat="server" />
                        </tbody>
                    </table>
<%--                    <div class="pageNum" id="div_page" runat="server">
                    </div>--%>
                </div>
        </section>
        <script src="../scripts/jquery-1.10.2.min.js"></script>
        <script src="../scripts/controls.js"></script>
         <script src="../scripts/Bombbox.js"></script>
        <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
        <script>
            nav.change('<%=m_id%>'); 
        </script>
        <script src="../Scripts/HighChart/js/themes/KDTheme.js"></script>
    </form>
</body>
</html>