<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="template_pic_list.aspx.cs" Inherits="KDWechat.Web.setting.template_pic_list" %>


<%@ Register Src="../UserControl/TopControl.ascx" TagName="TopControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/MenuList.ascx" TagName="MenuList" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/material_search.ascx" TagName="material_search" TagPrefix="uc3" %>
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
                     <%=isAdd==true?"<a href=\"template_pic_add.aspx?m_id="+m_id+"\" class=\"btn btn3\"><i class=\"add\"></i>新建图片</a>":"" %>
                </div>
                <h1>模板图片素材列表</h1>
            </div>

            <uc3:material_search ID="material_search1" page_link="template_pic_list.aspx" isshow_group="0" runat="server" />
            <div class="picListPanel_01">
                <div class="picList">
                    <ul>
                        <asp:Repeater ID="repList" runat="server" OnItemCommand="repList_ItemCommand">
                            <ItemTemplate>
                                <li>
                                    <div class="img">
                                        <a href='<%# Eval("file_url") %>' target="_blank">
                                            <img src="<%# Eval("file_url") %>" alt=""></a>
                                    </div>
                                    <div class="info">
                                        <p><%--<%# GetPcSize(Eval("file_url")) %>--%></p>
                                        <h2><asp:Literal ID="lblTitle" runat="server" Text='<%# Eval("title") %>'></asp:Literal></h2>
                                    </div>
                                    <div class="control">
                                        <a href="javascript:copyString.copy('<%# wchatConfig.domain+Eval("file_url") %>')" class="btn choose">复制图片链接</a>
                                        <a href='<%# Eval("file_url") %>' target="_blank" class="btn view" title="查看" type="button"></a>

                                        <%#isEdit==true?"<a href=\"template_pic_add.aspx?id="+Eval("id")+"&m_id="+m_id+"\" class=\"btn edit\" title=\"编辑\" type=\"button\"></a>":"" %>

                                        <asp:LinkButton ID="lbtnDelete" CssClass="btn delete"   type="button" CommandArgument='<%# Eval("id") %>' Visible='<%# isDelete %>' CommandName="del" OnClientClick="return confirm('您确认要删除吗?');" runat="server" ToolTip='删除'></asp:LinkButton>

                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
                <div class="pageNum" id="div_page" runat="server">
                </div>
            </div>

        </section>
        <script src="../Scripts/jquery-1.10.2.min.js"></script>
        <script src="../scripts/jquery.ba-resize.min.js"></script>
        <script src="../Scripts/function.js"></script>
        <script src="../scripts/controls.js"></script>
          <script src="../Scripts/ZeroClipboard.js"></script>
        <script>
            nav.change('<%=m_id%>');

        </script>
    </form>
</body>
</html>
