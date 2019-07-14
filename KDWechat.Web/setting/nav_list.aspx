<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="nav_list.aspx.cs" Inherits="KDWechat.Web.setting.nav_list" %>

<%@ Register Src="../UserControl/TopControl.ascx" TagName="TopControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/MenuList.ascx" TagName="Sys_menulist" TagPrefix="uc2" %>
<!doctype html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=pageTitle %></title>
    <script src="../scripts/html5.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet">
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <style>
        .folder-open {
            display: inline-block;
            margin-right: 2px;
            width: 20px;
            height: 20px;
            background: url(/images/skin_icons.png) -40px -196px no-repeat;
            vertical-align: middle;
            text-indent: -999em;
            *text-indent: 0;
        }

        .folder-line {
            display: inline-block;
            margin-right: 2px;
            width: 20px;
            height: 20px;
            background: url(/images/skin_icons.png) -80px -196px no-repeat;
            vertical-align: middle;
            text-indent: -999em;
            *text-indent: 0;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <uc1:TopControl ID="TopControl1" runat="server" />
        <uc2:Sys_menulist ID="MenuList1" runat="server" />
        <section id="main">
            <div class="titlePanel_01">
                <div class="btns">
                    <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn3" OnClick="btnSave_Click">保存排序</asp:LinkButton>&nbsp;&nbsp;<a href="nav_edit.aspx?action=Add&m_id=<%=m_id %>" class="btn btn3">新建导航菜单</a>
                </div>
                <h1>导航菜单设置</h1>
            </div>
            <div class="tablePanel_01">
                <table cellpadding="0" cellspacing="0" class="table">
                    <thead>
                        <tr>
                            <th class="check file">选择</th>
                            <th class="info info1">ID</th>
                            <th class="info info1">调用ID</th>
                            <th class="info info1">导航标题</th>
                            <th class="info info1">菜单类型</th>
                            <th class="info info1">显示</th>
                            <th class="info info1">系统菜单</th>
                            <th class="info info1">排序</th>
                            <th class="control">操作</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="repList" runat="server" OnItemDataBound="rptList_ItemDataBound" OnItemCommand="repList_ItemCommand">
                            <ItemTemplate>
                                <tr>
                                    <td class="check file">
                                        <asp:HiddenField ID="hidId" Value='<%#Eval("id")%>' runat="server" />
                                        <asp:HiddenField ID="hidLayer" Value='<%#Eval("class_layer") %>' runat="server" />
                                    </td>
                                    <td><%#Eval("id") %></td>
                                    <td class="info info1" style="white-space: nowrap; word-break: break-all; overflow: hidden;"><%#Eval("name")%></td>
                                    <td class="info info1" style="white-space: nowrap; word-break: break-all; overflow: hidden; text-align: left">
                                        <asp:Literal ID="LitFirst" runat="server"></asp:Literal>
                                        <a href="nav_edit.aspx?action=Edit&id=<%#Eval("id")%>&channel_id=<%#Eval("channel_id") %>&m_id=<%=m_id %>"><%#Eval("title")%></a>
                                        <%#Eval("link_url").ToString() == "" ? "" : "(链接：" + Eval("link_url") + ")"%>
                                    </td>
                                    <td class="info info1"><%# GetMenuType(Eval("type_id"))%></td>
                                    <td class="info info1"><%#Convert.ToInt32(Eval("is_lock")) == 0 ? "是" : "否"%></td>
                                    <td class="info info1"><%#Convert.ToInt32(Eval("is_sys")) == 1 ? "是" : "否"%></td>
                                    <td class="info info1">
                                        <asp:TextBox ID="txtSortId" runat="server" Text='<%#Eval("sort_id")%>' CssClass="sort" onkeydown="return checkNumber(event);" Width="60px" /></td>
                                    <td class="control" style="white-space: nowrap; word-break: break-all; overflow: hidden; text-align:left">
                                        <a href="nav_edit.aspx?action=Add&id=<%#Eval("id")%>&channel_id=<%#Eval("channel_id") %>&m_id=<%=m_id %>" class="btn btn6">添加子级</a>
                                        <a href="nav_edit.aspx?action=Edit&id=<%#Eval("id")%>&channel_id=<%#Eval("channel_id") %>&m_id=<%=m_id %>" class="btn btn6">修改</a>
                                        <asp:LinkButton ID="lbtnDelete" CssClass="btn btn6" CommandArgument='<%# Eval("id")+","+Eval("title") %>' CommandName="del" OnClientClick="return confirm('本操作会删除本导航及下属子导航，是否继续？');" runat="server" Text='删除' Visible='<%#bool.Parse((Convert.ToInt32(Eval("is_sys"))==0 ).ToString())%>'></asp:LinkButton>
                                    </td>
                                </tr>

                            </ItemTemplate>
                            <FooterTemplate>
                                <%#repList.Items.Count == 0 ? "<tr><td style=\"text-align:center;\" colspan=\"7\">暂无数据</td></tr>" : ""%>
                            </FooterTemplate>
                        </asp:Repeater>


                    </tbody>
                </table>
                <%-- 需要引用function.js--%>
            </div>

        </section>
        <script src="../Scripts/jquery-1.10.2.min.js"></script>
        <script src="../Scripts/function.js"></script>
         <script src="../scripts/controls.js" type="text/javascript"></script>
         <script type="text/javascript">

             //选中菜单
             nav.change('<%=m_id%>'); 
             </script>
    </form>
</body>
</html>
