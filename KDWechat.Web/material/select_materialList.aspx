<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="select_materialList.aspx.cs" Inherits="KDWechat.Web.material.select_materialList" %>

<%@ Register Src="../UserControl/material_search.ascx" TagName="material_search" TagPrefix="uc3" %>
<!doctype html>
<html>
<head>
    <meta charset="UTF-8">
    <title>选择图文消息</title>
    <script src="../scripts/html5.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet">
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>

<body class="bombbox">
    <header id="bombboxTitle">
        <div class="titlePanel_01">
            <h1>选择<%=is_public==1?"公共素材":"" %>图文消息</h1>
        </div>
    </header>
    <form id="form1" runat="server">


        <section id="bombboxMain">
            <uc3:material_search ID="material_search1" page_link="select_materialList.aspx" isshow_group="1" runat="server" />
            <div class="graphicMaterialListPanel_01">
                <div class="graphicMaterialList">
                    <ul>
                        <asp:Repeater ID="repList" runat="server" OnItemCommand="repList_ItemCommand">
                            <ItemTemplate>

                                <li class="sigle">
                                    <div class="title">
                                        <h1>
                                            <asp:Literal ID="lblTitle" runat="server" Text='<%# Eval("title") %>'></asp:Literal></h1>
                                    </div>
                                    <div class="img">
                                        <span>
                                            <img src="<%# Eval("cover_img") %>" alt="">
                                        </span>
                                    </div>
                                    <div class="content">
                                        <%# Eval("summary") %>
                                    </div>
                                    <div class="mask"></div>
                                    <div class="control">
                                        <input type="button" class="btn preview" onClick="dialogue.dlShowPic('/upload/qr_codes/news_<%# Eval("id") %>.png')" value="二维码预览">
                                        <input type="button" class="btn choose" value="选择" onclick='selectThis(this)'>
                                        <input type="hidden" class="title" value="<%#Eval("title") %>" />
                                        <input type="hidden" class="cover_img" value="<%#Eval("cover_img") %>" />
                                        <input type="hidden" class="summary" value="<%#Eval("summary") %>" />
                                        <input type="hidden" class="author" value="<%#Eval("author") %>" />
                                        <input type="hidden" class="contents" value='<%#Eval("contents") %>' />
                                        <input type="hidden" class="link_url" value="<%#Eval("link_url") %>" />
                                        <input type="hidden" class="origin" value="<%#Eval("source_url") %>" />
                                        <input type="hidden" class="pushType" value="<%#Eval("push_Type") %>" />
                                        <input type="hidden" class="template_id" value="<%#Eval("template_id") %>" />
                                        <input type="hidden" class="template_name" value="<%#Eval("template_name") %>" />
                                        <input type="hidden" class="template_img" value="<%#Eval("template_img") %>" />
                                        <input type="hidden" class="app_id" value="<%#Eval("app_id") %>" />
                                        <input type="hidden" class="app_title" value="<%#Eval("app_type_name") %>" />
                                        <input type="hidden" class="app_content" value="<%#Eval("app_name") %>" />
                                        <input type="hidden" class="app_img" value="<%#Eval("app_type_img") %>" />
                                        <input type="hidden" class="app_link" value="<%#Eval("app_link") %>" />
                                       <%-- <%# (is_public==1?"":"<a href='news_add.aspx?id="+Eval("id")+"&tef=1895623541' class='btn edit' title='编辑' type='button'></a>") %>--%>
                                       <%-- <asp:LinkButton ID="lbtnDelete" CssClass="btn delete" CommandArgument='<%# Eval("id") %>' CommandName="del" OnClientClick="return confirm('您确认要删除吗?');" runat="server" ToolTip='删除'></asp:LinkButton>--%>

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
    </form>
    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../scripts/controls.js"></script>
    <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
    <script src="../Scripts/function.js"></script>
    <script>
        function selectThis(obj_this) {
            var pushType = $(obj_this).parent().find(".pushType").val();
            if (pushType == "article") {
                var template_id = $(obj_this).parent().find(".template_id").val();
                var template_name = $(obj_this).parent().find(".template_name").val();
                var template_img = $(obj_this).parent().find(".template_img").val();
                if (template_id != "0" && template_img != "" && template_name != "") {
                    window.parent.materialAddModule.change({ title: '' + $(obj_this).parent().find(".title").val() + '', type: '0', img: '' + $(obj_this).parent().find(".cover_img").val() + '', summary: '' + $(obj_this).parent().find(".summary").val() + '', intro: '' + $(obj_this).parent().find(".author").val() + '', pushType: 'article', article: { content: '' + $(obj_this).parent().find(".contents").val() + '', origin: '' + $(obj_this).parent().find(".origin").val() + '', template: { id: '' + template_id + '', title: '' + template_name + '', img: '' + template_img + '' } }, link: { content: '' + $(obj_this).parent().find(".link_url").val() + '' } });
                } else {
                    window.parent.materialAddModule.change({ title: '' + $(obj_this).parent().find(".title").val() + '', type: '0', img: '' + $(obj_this).parent().find(".cover_img").val() + '', summary: '' + $(obj_this).parent().find(".summary").val() + '', intro: '' + $(obj_this).parent().find(".author").val() + '', pushType: 'article', article: { content: '' + $(obj_this).parent().find(".contents").val() + '', origin: '' + $(obj_this).parent().find(".origin").val() + '' }, link: { content: '' + $(obj_this).parent().find(".link_url").val() + '' } });
                }

            } else if (pushType == "link") {

                window.parent.materialAddModule.change({ title: '' + $(obj_this).parent().find(".title").val() + '', type: '0', img: '' + $(obj_this).parent().find(".cover_img").val() + '', summary: '' + $(obj_this).parent().find(".summary").val() + '', intro: '' + $(obj_this).parent().find(".author").val() + '', pushType: 'link', article: { content: '' + $(obj_this).parent().find(".contents").val() + '', origin: '' + $(obj_this).parent().find(".origin").val() + '' }, link: { content: '' + $(obj_this).parent().find(".link_url").val() + '' } });
            } else if (pushType == "app") {
                {
                    window.parent.materialAddModule.change({ title: '' + $(obj_this).parent().find(".title").val() + '', type: '0', img: '' + $(obj_this).parent().find(".cover_img").val() + '', summary: '' + $(obj_this).parent().find(".summary").val() + '', intro: '' + $(obj_this).parent().find(".author").val() + '', pushType: 'app', app: { id: '' + $(obj_this).parent().find(".app_id").val() + '', title: '' + $(obj_this).parent().find(".app_title").val() + '', content: '' + $(obj_this).parent().find(".app_content").val() + '', img: '' + $(obj_this).parent().find(".app_img").val() + '', link: '' + $(obj_this).parent().find(".app_link").val() + '' } });
                }

              
            }
            window.parent.bombbox.closeBox();

        }

    </script>
</body>
</html>
