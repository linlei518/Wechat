<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="news_list.aspx.cs" Inherits="KDWechat.Web.material.news_list" %>


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
                    <%=isAdd==true?"<a href=\"news_add.aspx?m_id="+m_id+"&is_pub="+is_pub+"\" class=\"btn btn3\"><i class=\"add\"></i>新建单图文消息</a>":"" %>
                   
                </div>
                <h1>单图文消息列表</h1>
            </div>

            <uc3:material_search ID="material_search1" page_link="news_list.aspx" isshow_group="1" runat="server" />
            <div class="graphicMaterialListPanel_01">
                <div class="graphicMaterialList">
                    <ul>
                        <asp:Repeater ID="repList" runat="server" OnItemCommand="repList_ItemCommand">
                            <ItemTemplate>

                                <li  class="sigle">
                                    <div class="title">
                                        <h1>
                                            <a href="news_add.aspx?m_id=<%=m_id %>&id=<%# Eval("id") %>&is_pub=<%=is_pub %>"><asp:Literal ID="lblTitle" runat="server" Text='<%# Eval("title") %>'></asp:Literal></a> </h1>
                                    </div>
                                    <div class="img">
                                        <span>
                                            <img src="<%# Eval("cover_img") %>" alt="">
                                            <asp:HiddenField ID="hf_img" Value='<%#Eval("cover_img")%>' runat="server" />
                                            <asp:HiddenField ID="hf_img_list" Value='<%#Eval("img_list")%>' runat="server" />
                                        </span>
                                    </div>
                                    <div class="content">
                                      <%# Eval("summary") %>
                                    </div>
                                    <div class="mask"></div>
                                    <div class="control">
                                        <input type="button" class="btn preview" onClick="dialogue.dlShowPic('/upload/qr_codes/news_<%# Eval("id") %>.png')" value="二维码预览">
                                         <input type="button" class="btn preview" onClick="copyString.copy('<%#GetLink(Eval("id"),Eval("push_type"),Eval("link_url"),Eval("app_link")) %>')" value="复制图文链接">
                                            
                                        <%# isEdit==true?"<a href=\"news_add.aspx?m_id="+m_id+"&id="+Eval("id")+"&is_pub="+is_pub+"\" class=\"btn edit\" title=\"编辑\" type=\"button\"></a>":"" %>
                                        
                                        <asp:LinkButton ID="lbtnDelete" CssClass="btn delete" Visible='<%# isDelete %>' CommandArgument='<%# Eval("id") %>' CommandName="del" OnClientClick="return confirm('您确认要删除吗?');" runat="server" ToolTip='删除'></asp:LinkButton>

                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                    <%=repList.Items.Count == 0 ? "<li><div style=\"text-align:center; border:none;width:100%\"  >暂无数据</div></li>" : ""%>

                </div>
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

