<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="select_module.aspx.cs" Inherits="KDWechat.Web.keyworld.select_module" %>

<!doctype html>
<html>
<head>
    <meta charset="UTF-8">
    <title>选择功能应用列表</title>
    <script src="../scripts/html5.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet">
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="/styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>

<body class="bombbox">
    <form id="form1" runat="server">
        <header id="bombboxTitle">
            <div class="titlePanel_01">
                <div class="btns">
                    <asp:Literal ID="lblPublic" runat="server"></asp:Literal>
                </div>
                <h1>选择应用</h1>
            </div>
        </header>
        <section id="bombboxMain">
            <div class="searchPanel_01">
                <div class="searchField">
                    <input type="text" class="txt searchTxt" placeholder="搜索标题..." runat="server" id="txtKey">
                    <asp:Button ID="btnSearch" runat="server" Text="" CssClass="btn searchBtn" ToolTip="点击搜索" OnClick="btnSearch_Click"></asp:Button>
                </div>
                <div class="filterField">
                    <asp:DropDownList ID="ddlGroup" AutoPostBack="true" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged" CssClass="select" DataValueField="id" DataTextField="title" AppendDataBoundItems="true" runat="server">
                        <asp:ListItem Value="0">所有应用</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <div class="tablePanel_01 materialList selectTable">
                <table cellpadding="0" cellspacing="0" class="table">
                    <thead>
                        <tr>
                            <th class="name">标题</th>
                            <th class="info info1">所属应用</th>
                            <th class="info info1">简介</th>
                            <th class="selectControl">操作</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="repList" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td class="name"><%# Eval("app_name") %></td>


                                    <td class="info info1"><%# Eval("module_name") %></td>
                                    <td class="info info1"><%# KDWechat.Common.Utils.DropHTML( Eval("app_remark").ToString(),140) %></td>
                                    <td class="selectControl">

                                        <input type="button" value="选择" class="btn btn5" onclick='selectThis(this)'>
                                        <input type="hidden" class="title" value="<%# "【"+Eval("module_name")+"】"+ Eval("app_name") %>" />
                                        <input type="hidden" class="cover_img" value="<%#Eval("app_img_url") %>" />
                                        <input type="hidden" class="summary" value="<%# KDWechat.Common.Utils.DropHTML( Eval("app_remark").ToString(),140) %>" />
                                        <input type="hidden" class="id" value="<%#Eval("id") %>" />

                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>

                    </tbody>
                </table>
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
        function selectThis(obj_this)
        {
            var channel_id=<%=channel_id%>;
            parent.selectResult(channel_id, $(obj_this).parent().find(".id").val(),$(obj_this).parent().find(".cover_img").val() , $(obj_this).parent().find(".title").val(), "", $(obj_this).parent().find(".summary").val(), 1, "","",0);
           
        }

    </script>
</body>
</html>
