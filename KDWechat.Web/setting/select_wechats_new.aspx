<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="select_wechats_new.aspx.cs" Inherits="KDWechat.Web.setting.select_wechats_new" %>

<!doctype html>
<html>
<head>
    <meta charset="UTF-8">
    <title>模板分配公众号</title>
    <script src="../scripts/html5.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet">
    <!--[if lt IE 9 ]><link href="/styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="/styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="/styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>

<body class="bombbox">
    <header id="bombboxTitle">
        <div class="titlePanel_01">
            <div class="btns">
                <a href="javascript:window.parent.bombbox.prev()" class="btn btn5"><i class="black back"></i>返回已分配列表</a>
            </div>
            <h1>分配新公众号</h1>
        </div>
    </header>
    <form id="form1" runat="server">


        <section id="bombboxMain">
            <div class="searchPanel_01">
                <div class="searchField">
                    <input type="text" class="txt searchTxt" placeholder="搜索名称..." runat="server" id="txtKey">
                    <asp:Button ID="btnSearch" runat="server" Text="" CssClass="btn searchBtn" ToolTip="点击搜索" OnClick="btnSearch_Click"></asp:Button>
                </div>
                <div class="filterField">
                    <asp:DropDownList ID="ddlType" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" CssClass="select" runat="server">
                        <asp:ListItem Value="0">所以类型</asp:ListItem>
                        <asp:ListItem Value="1">普通订阅号</asp:ListItem>
                        <asp:ListItem Value="2">认证后订阅号</asp:ListItem>
                        <asp:ListItem Value="3">普通服务号</asp:ListItem>
                        <asp:ListItem Value="4">认证后服务号</asp:ListItem>
                    </asp:DropDownList>

                </div>
            </div>
            <div class="tablePanel_01 materialList selectTable">
                <table cellpadding="0" cellspacing="0" class="table">
                    <thead>
                        <tr>
                            <th class="name">名称</th>
                            <th class="info info1">类型</th>
                            <th class="selectControl">操作</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="repList" runat="server" OnItemCommand="repList_ItemCommand">
                            <ItemTemplate>
                                <tr>
                                    <td class="name"><%# Eval("wx_pb_name") %></td>

                                    <td class="info info1"><%# (KDWechat.Common.WeChatServiceType)(int.Parse(Eval("type_id").ToString())) %></td>
                                    <td class="selectControl">
                                        <asp:HiddenField ID="hfWx_ogid"  Value='<%# Eval("wx_og_id") %>' runat="server" />
                                        <asp:Button ID="Button1" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="select" Text="选择" CssClass="btn btn5" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>

                    </tbody>
                </table>
                <%-- 需要引用function.js--%>
                <div class="pageNum" id="div_page" runat="server">
                </div>
            </div>
        </section>
    </form>
    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../scripts/controls.js"></script>
    <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
    <script src="../Scripts/function.js"></script>

</body>
</html>

