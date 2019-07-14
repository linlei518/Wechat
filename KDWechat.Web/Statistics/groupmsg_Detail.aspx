<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="groupmsg_Detail.aspx.cs" Inherits="KDWechat.Web.Statistics.groupmsg_Detail" %>

<%@ Register Src="../UserControl/TopControl.ascx" TagName="TopControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/MenuList.ascx" TagName="Sys_menulist" TagPrefix="uc2" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="UTF-8" />
    <title>群发图文统计</title>
    <script src="../scripts/html5.js"></script>
    <script src="../scripts/function.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet" />
    <script src="../Scripts/DatePicker/WdatePicker.js"></script>
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>
<body>
    <form id="form1" runat="server">
        <uc1:TopControl ID="TopControl1" runat="server" />
        <uc2:Sys_menulist ID="MenuList1" runat="server" />
        <section id="main">
            <div class="tablePanel_01" id="div_detail" style="display: " runat="server">

                <%--                    <asp:TextBox runat="server" ID="txtDate" CssClass="date txt" onfocus="WdatePicker({dateFmt:'yyyy年MM月dd日'})" ></asp:TextBox>
                    <asp:Button runat="server" ID="txtSearch" OnClick="txtSearch_Click" CssClass="btn btn3" Text="查询当日" /><br />
                    appid:<asp:TextBox ID="txtAppid" runat="server"></asp:TextBox>appsecret:<asp:TextBox ID="txtAppSecret" runat="server"></asp:TextBox><br />--%>
                <asp:Repeater ID="Repeater1" runat="server">
                    <ItemTemplate>
                        图文名称：
                        <asp:Label ID="labTitle" runat="server" Text=""></asp:Label>
                        图文id：<%#Eval("msgid") %>
                        <br />
                        <asp:Repeater ID="Repeater2" runat="server">
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" class="table">
                                    <thead>
                                        <tr>
                                            <th class="name">统计时间</th>
                                            <th class="info info1" style="width: 40px">目标用户数</th>
                                            <th class="info info1" style="width: 40px">阅读人数</th>
                                            <th class="info info1" style="width: 40px">阅读次数</th>
                                            <th class="info info1" style="width: 40px">原文阅读人数</th>
                                            <th class="info info1" style="width: 40px;">原文阅读次数</th>
                                            <th class="info info1" style="width: 40px">分享人数</th>
                                            <th class="info info1" style="width: 40px">分享次数</th>
                                            <th class="info info1" style="width: 40px">添加到收藏的人数</th>
                                            <th class="info info1" style="width: 40px">添加到收藏的次数</th>

                                        </tr>
                                    </thead>
                                    <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td class="name"><%#Eval("stat_date") %></td>
                                    <td class="info info1"><%#Eval("target_user") %></td>
                                    <td class="info info1"><%#Eval("int_page_read_user") %></td>
                                    <td class="info info1"><%#Eval("int_page_read_count") %></td>
                                    <td class="info info1"><%#Eval("ori_page_read_user") %></td>
                                    <td class="info info1"><%#Eval("ori_page_read_count") %></td>
                                    <td class="info info1"><%#Eval("share_user") %></td>
                                    <td class="info info1"><%#Eval("share_count") %></td>
                                    <td class="info info1"><%#Eval("add_to_fav_user") %></td>
                                    <td class="info info1"><%#Eval("add_to_fav_count") %></td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                <tr>
                                    <td colspan="2"></td>
                                    <td class="info info1">
                                        <asp:Label Text="" ID="read_people" runat="server" /></td>
                                    <td class="info info1">
                                        <asp:Label Text="" ID="read_count" runat="server" /></td>
                                    <td class="info info1">
                                        <asp:Label Text="" ID="old_read_people" runat="server" /></td>
                                    <td class="info info1">
                                        <asp:Label Text="" ID="old_read_count" runat="server" /></td>
                                    <td class="info info1">
                                        <asp:Label Text="" ID="share_people" runat="server" /></td>
                                    <td class="info info1">
                                        <asp:Label Text="" ID="share_count" runat="server" /></td>
                                    <td class="info info1">
                                        <asp:Label Text="" ID="collect_people" runat="server" /></td>
                                    <td class="info info1">
                                        <asp:Label Text="" ID="collect_count" runat="server" /></td>
                                </tr>
                                </tbody>
                                    
                                    </table>
                            </FooterTemplate>
                        </asp:Repeater>

                    </ItemTemplate>
                </asp:Repeater>
                <asp:HiddenField ID="hfReturlUrl" runat="server" />

            </div> 
            <div class="tablePanel_01" id="div_total" style="display: " runat="server">

                <table cellpadding="0" cellspacing="0" class="table">
                    <thead>
                        <tr>
                            <th class="info info1" style="width: 40px">阅读人数</th>
                            <th class="info info1" style="width: 40px">阅读次数</th>
                            <th class="info info1" style="width: 40px">原文阅读人数</th>
                            <th class="info info1" style="width: 40px;">原文阅读次数</th>
                            <th class="info info1" style="width: 40px">分享人数</th>
                            <th class="info info1" style="width: 40px">分享次数</th>
                            <th class="info info1" style="width: 40px">添加到收藏的人数</th>
                            <th class="info info1" style="width: 40px">添加到收藏的次数</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td class="info info1"><%=read_people_total %></td>
                            <td class="info info1"><%=read_count_total %></td>
                            <td class="info info1"><%=old_read_people_total  %></td>
                            <td class="info info1"><%=old_read_count_total  %></td>
                            <td class="info info1"><%=share_people_total  %></td>
                            <td class="info info1"><%=share_count_total  %></td>
                            <td class="info info1"><%=collect_people_total  %></td>
                            <td class="info info1"><%=collect_count_total  %></td>
                        </tr>
                    </tbody>
                </table>

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
