<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="key_rule_list.aspx.cs" Inherits="KDWechat.Web.keyworld.key_rule_list" %>


<%@ Register Src="../UserControl/TopControl.ascx" TagName="TopControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/MenuList.ascx" TagName="MenuList" TagPrefix="uc2" %>

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
</head>

<body>
    <form id="form1" runat="server">
        <uc1:TopControl ID="TopControl1" runat="server" />
        <uc2:MenuList ID="MenuList1" runat="server" />
        <section id="main">
               <%=NavigationName %>
            <div class="titlePanel_01">
                <div class="btns">
                    <%=isAdd==true?"<a href=\"key_rule_add.aspx?m_id="+m_id+"\" class=\"btn btn3\"><i class=\"add\"></i>新建规则</a>":"" %>
                </div>
                <h1>关键词自动回复列表</h1>
            </div>
            <div class="searchPanel_01">
                <div class="searchField">
                    <input type="text" class="txt searchTxt" placeholder="搜索关键词..." runat="server" id="txtKey">
                    <asp:Button ID="btnSearch" runat="server" Text="" CssClass="btn searchBtn" ToolTip="点击搜索" OnClick="btnSearch_Click"></asp:Button>
                </div>
                <div class="filterField">
                    <asp:DropDownList ID="ddlGroup" AutoPostBack="true" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged" CssClass="select" runat="server">
                        <asp:ListItem Value="-1">所有状态</asp:ListItem>
                        <asp:ListItem Value="1">启用</asp:ListItem>
                        <asp:ListItem Value="0">禁用</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlReplyType" AutoPostBack="true" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged" CssClass="select" runat="server">
                        <asp:ListItem Value="-1">所有回复类型</asp:ListItem>
                        <asp:ListItem Value="1">文本</asp:ListItem>
                        <asp:ListItem Value="3">图片</asp:ListItem>
                        <asp:ListItem Value="4">语音</asp:ListItem>
                        <asp:ListItem Value="5">视频</asp:ListItem>
                        <asp:ListItem Value="2">单图文</asp:ListItem>
                        <asp:ListItem Value="6">多图文</asp:ListItem>
                        <asp:ListItem Value="9">模块</asp:ListItem>
                        <asp:ListItem Value="10">多客服</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="tablePanel_01">
                <table cellpadding="0" cellspacing="0" class="table">
                    <thead>
                        <tr>
                            <th class="name">规则名称</th>
                            <th class="info info1" style=" width:25%">关键词</th>
                            <th class="info info1" style=" width:60px">回复类型</th>
                            <th class="info info1" style=" width:40px">状态</th>
                            <th class="control" style="width:180px">操作</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="repList" runat="server" OnItemCommand="repList_ItemCommand">
                            <ItemTemplate>
                                <tr>

                                    <td class="name">
                                        <asp:HiddenField ID="hidId" Value='<%#Eval("id")%>' runat="server" />
                                        <a href="key_rule_add.aspx?id=<%# Eval("id") %>&m_id=<%=m_id %>">
                                            <asp:Literal ID="lblTitle" runat="server" Text='<%# Eval("rule_name") %>'></asp:Literal></a>
                                    </td>
                                    <td class="info info1" style=" width:25%"><%# GetKeyList(Eval("id")) %></td>
                                    <td class="info info1" style=" width:60px"><%# GetStatus(Eval("reply_type")) %></td>
                                    <td class="info info1" style=" width:40px"><%# Eval("status").ToString()=="1"?"启用":"禁用" %></td>

                                    <td class="control" style=" width:180px">

                                          <%# isEdit==true?"<a href=\"key_rule_add.aspx?m_id="+m_id+"&id="+Eval("id")+"\" class=\"btn btn6\"   >编辑</a>":"" %>

                                        <asp:LinkButton ID="lbtnStatus" CssClass="btn btn6" CommandArgument='<%# Eval("id") %>' Visible='<%# isEdit %>'  OnClientClick="dialogue.dlLoading();" CommandName="status" runat="server" Text='<%# Eval("status").ToString()=="0"?"启用":"禁用" %>'></asp:LinkButton>
                                        <asp:LinkButton ID="lbtnDelete" CssClass="btn btn6" CommandArgument='<%# Eval("id") %>'  Visible='<%# isDelete %>' CommandName="del" OnClientClick="return confirm('您确认要删除吗?');" runat="server" Text='删除'></asp:LinkButton>
                                    </td>
                                </tr>

                            </ItemTemplate>
                            <FooterTemplate>
                                <%#repList.Items.Count == 0 ? "<tr><td style=\"text-align:center;\" colspan=\"5\">暂无数据</td></tr>" : ""%>
                            </FooterTemplate>
                        </asp:Repeater>


                    </tbody>
                </table>
                <%-- 需要引用function.js--%>
                <div class="pageNum" id="div_page" runat="server">
                </div>
            </div>

        </section>
        <script src="../Scripts/jquery-1.10.2.min.js"></script>
        <script src="../Scripts/function.js"></script>
        <script src="../scripts/controls.js"></script>
        <script>
            nav.change('<%=m_id%>'); 
        </script>
    </form>
</body>
</html>
