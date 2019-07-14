<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="select_multi-news.aspx.cs" Inherits="KDWechat.Web.keyworld.select_multi_news" %>


<%@ Register Src="../UserControl/material_search.ascx" TagName="material_search" TagPrefix="uc3" %>

<!doctype html>
<html>
<head>
    <meta charset="UTF-8">
    <title>选择多图文</title>
    <script src="../scripts/html5.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet">
    <!--[if lt IE 9 ]><link href="/styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="/styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="/styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>
<body class="bombbox">
    <form id="form1" runat="server">
        <header id="bombboxTitle">
            <div class="titlePanel_01">
                <div class="btns">
                    <asp:Literal ID="lblPublic" runat="server"></asp:Literal>
                </div>
                <h1>选择多图文消息</h1>
            </div>
        </header>
        <section id="bombboxMain">
            <uc3:material_search ID="material_search1" runat="server" />

            <div class="graphicMaterialListPanel_01">
                <div class="graphicMaterialList">
                    <ul>
                        <asp:Repeater ID="repList" runat="server" OnItemCommand="repList_ItemCommand">
                            <ItemTemplate>

                                <li  class="group">
                                    <div class="mainInfo">
                                        <div class="img">
                                            <span>
                                                <img src="<%# Eval("cover_img") %>" alt="">
                                            </span>
                                        </div>
                                        <div class="title">
                                            <h1>
                                                <asp:Literal ID="lblTitle" runat="server" Text='<%# Eval("title") %>'></asp:Literal></h1>
                                        </div>
                                    </div>
                                    <%# Eval("content_html") %>

                                    <div class="mask"></div>
                                    <div class="control">
                                        <input type="button" class="btn preview" onClick="dialogue.dlShowPic('/upload/qr_codes/news_<%# Eval("id") %>.png')" value="二维码预览">
                                        <input type="button" class="btn choose" value="选择" onclick='selectThis(this)'>
                                        <input type="hidden" class="title" value="<%#Eval("title") %>" />
                                        <input type="hidden" class="cover_img" value="<%#Eval("cover_img") %>" />
                                        <input type="hidden" class="id" value="<%#Eval("id") %>" />
                                        <input type="hidden" class="summary" value="<%# KDWechat.Common.Utils.DropHTML( Eval("summary").ToString(),40) %>" />
                                        <input type="hidden" class="child_list" value='<%# Eval("multi_html") %>' />
                                       <%-- <%# (is_public==1?"":"<a href='/material/multi-news_add.aspx?id="+Eval("id")+"&tef=1895623541' class='btn edit' title='编辑' type='button'></a>") %>
                                        <asp:LinkButton ID="lbtnDelete" CssClass="btn delete" CommandArgument='<%# Eval("id") %>' CommandName="del" OnClientClick="return confirm('您确认要删除吗?');" runat="server" ToolTip='删除'></asp:LinkButton>--%>
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
        <script src="../scripts/jquery-1.10.2.min.js"></script>
        <script src="../scripts/controls.js"></script>
        <script src="../Scripts/function.js"></script>
        <script>
            function selectThis(obj_this)
            {
                var channel_id=<%=channel_id%>;
                parent.selectResult(channel_id, $(obj_this).parent().find(".id").val(),$(obj_this).parent().find(".cover_img").val() , $(obj_this).parent().find(".title").val(), "", $(obj_this).parent().find(".summary").val(), 1,  $(obj_this).parent().find(".child_list").val(),"",0);
            }

        </script>
    </form>
</body>
</html>
