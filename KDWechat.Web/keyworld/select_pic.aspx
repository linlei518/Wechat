<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="select_pic.aspx.cs" Inherits="KDWechat.Web.keyworld.select_pic" %>

<%@ Register Src="../UserControl/material_search.ascx" TagName="material_search" TagPrefix="uc3" %>

<!doctype html>
<html>
<head>
    <meta charset="UTF-8">
    <title>选择图片</title>
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
                <h1>选择图片</h1>
            </div>
        </header>
        <section id="bombboxMain">

            <uc3:material_search ID="material_search1" runat="server" />
            <div class="picListPanel_01">
                <div class="picList">
                    <ul>
                    <asp:Repeater ID="repList" runat="server" OnItemCommand="repList_ItemCommand">
                    <ItemTemplate>
                        <li>
                            <div class="img">
                                <a href='<%# Eval("cover_img") %>' target="_blank">
                                    <img src='<%# Eval("cover_img") %>' alt=""></a>
                            </div>
                            <div class="info">
                                <p><%--<%# GetPcSize(Eval("cover_img")) %>--%></p>
                                <h2>
                                    <asp:Literal ID="lblTitle" runat="server" Text='<%# Eval("title") %>'></asp:Literal></h2>
                            </div>
                            <div class="control">
                                <input type="button" class="btn choose" value="选择" onclick='selectThis(this)'>
                                   <input type="hidden" class="title" value="<%#Eval("title") %>" />
                                    <input type="hidden" class="cover_img" value="<%#Eval("cover_img") %>" />
                                    <input type="hidden" class="id" value="<%#Eval("id") %>" />
                                <a href='<%# Eval("cover_img") %>' target="_blank" class="btn view" title="查看" type="button"></a>
                              <%--  <%# (is_public==1?"":"<a href='/material/pic_add.aspx?id="+Eval("id")+"&tef=1895623541' class='btn edit' title='编辑' type='button'></a>") %>
                                
                                <asp:LinkButton ID="LinkButton1" runat="server" Visible='<%# is_public==1?false:true %>' OnClientClick="return confirm('您确定要删除吗?');" CommandArgument='<%# Eval("id") %>' CommandName="del" CssClass="btn delete" ToolTip="删除"  type="button" ></asp:LinkButton>--%>
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
                parent.selectResult(channel_id, $(obj_this).parent().find(".id").val(),$(obj_this).parent().find(".cover_img").val() , $(obj_this).parent().find(".title").val(), "","", 1,"","",0);
           
        }

        </script>
     
    </form>
</body>
</html>
