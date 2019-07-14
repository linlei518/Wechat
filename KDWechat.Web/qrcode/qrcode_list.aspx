<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeBehind="qrcode_list.aspx.cs" Inherits="KDWechat.Web.qrcode.qrcode_list" %>

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
            <%= (wx_type==1||wx_type==3)?"<h1>您的公众号未认证，无法使用此功能！</h1>":""%>

            <div class="titlePanel_01">
                <div class="btns">
                    <a href="javascript:bombbox.openBox('qrcode_edit.aspx');" runat="server" visible="<%#isAdd %>" class="btn btn3"><i class="add"></i>新建拓客二维码</a>
                </div>
                <h1>拓客二维码列表</h1>
            </div>

            <div class="picListPanel_01">
                <div class="picList">
                    <ul>
                        <asp:Repeater ID="repList" runat="server" OnItemCommand="repList_ItemCommand">
                            <ItemTemplate>
                                <li>
                                    <div class="img">
                                        <a href='<%#"https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" +Eval("ticket") %>' target="_blank">
                                            <img src="<%#"https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" +Eval("ticket") %>" alt=""></a>
                                    </div>
                                    <div class="info">
                                        <h2><asp:Literal ID="lblTitle" runat="server" Text='<%# Eval("q_name") %>'></asp:Literal></h2>
                                    </div>
                                    <div class="control">
                                        <a href='<%#"https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" +Eval("ticket") %>' target="_blank" class="btn view" title="查看" type="button"></a>
                                        <a href="javascript:bombbox.openBox('qrcode_edit.aspx?id=<%# Eval("id") %>')" class="btn edit" title="编辑" type="button"></a>
                                        <asp:LinkButton ID="lbtnDelete" CssClass="btn download"  type="button" CommandArgument='<%#Eval("id")+"|"+"https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" +Eval("ticket") %>' CommandName="sss" runat="server" ToolTip='下载'></asp:LinkButton>
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
                <%=repList.Items.Count == 0 ? "<li><div style=\"text-align:center; border:none;width:100%\"  >暂无数据</div></li>" : ""%>
                <div class="pageNum" id="div_page" runat="server">
                </div>
            </div>

        </section>
        <script src="../Scripts/jquery-1.10.2.min.js"></script>
        <script src="../scripts/jquery.ba-resize.min.js"></script>
        <script src="../Scripts/function.js"></script>
        <script src="../scripts/controls.js"></script>
        <script src="../scripts/Bombbox.js"></script>
        <script>

            nav.change('<%=m_id%>');

        </script>
    </form>
</body>
</html>
