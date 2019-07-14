<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="multi-news_list.aspx.cs" Inherits="KDWechat.Web.material.multi_news_list" %>


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
                    <%=is_public==1?"":"<a href=\"javascript:bombbox.openBox('../InitTest.aspx');\" class=\"btn btn3\">从微信平台导入图文消息</a>" %>
                     <%=isAdd==true?"<a href='multi-news_add.aspx?m_id="+m_id+"&is_pub="+is_pub+"' class='btn btn3'><i class='add'></i>新建多图文消息</a>":"" %> 
                    <asp:Button ID="btnCreateQRCode" runat="server" Text="生成图文二维码" OnClientClick="dialogue.dlLoading();"  style="display:none" CssClass="btn btn3"  OnClick="btnCreateQRCode_Click" />
                    
                </div>
                <h1>多图文消息列表</h1>
            </div>

            <uc3:material_search ID="material_search1" page_link="multi-news_list.aspx" isshow_group="1" runat="server" />

            <div class="graphicMaterialListPanel_01">
                <div class="graphicMaterialList">
                    <ul>
                        <asp:Repeater ID="repList" runat="server" OnItemCommand="repList_ItemCommand">
                            <ItemTemplate>
                                <li class="group">
                                    <div class="mainInfo">
                                        <div class="img">
                                            <span>
                                                <img src="<%# Eval("cover_img") %>" alt="">
                                                <asp:HiddenField ID="hf_img" runat="server" Value='<%# Eval("img_list") %>'/>
                                            </span>
                                        </div>
                                        <div class="title">
                                            <h1><a href="multi-news_add.aspx?m_id=<%=m_id %>&id=<%# Eval("id") %>&is_pub=<%=is_pub %>"><asp:Literal ID="lblTitle" runat="server" Text='<%# Eval("title") %>'></asp:Literal></a></h1>
                                        </div>
                                    </div>
                                    <%# Eval("content_html") %>
                                    
                                    <div class="mask"></div>
                                    <div class="control">
                                          <input type="button" class="btn preview" onClick="dialogue.dlShowPic('/upload/qr_codes/news_<%# Eval("id") %>.png')" value="二维码预览">
                                         <%#isEdit==true?"<a href='multi-news_add.aspx?id="+Eval("id")+"&m_id="+m_id+"&is_pub="+is_pub+"' class='btn edit' title=\"编辑\" type=\"button\"></a>":"" %> 

                                         <asp:LinkButton ID="lbtnDelete" CssClass="btn delete" CommandArgument='<%# Eval("id") %>' Visible='<%# isDelete %>' CommandName="del" OnClientClick="return confirm('您确认要删除吗?');" runat="server" ToolTip='删除'></asp:LinkButton>
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
        <script src="../Scripts/Bombbox.js"></script>

        <script>

            nav.change('<%=m_id%>');

        </script>
    </form>
</body>
</html>

