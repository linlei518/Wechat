<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="letter_list_rec.aspx.cs" Inherits="KDWechat.Web.Account.letter_list_rec" %>

<%@ Register Src="../UserControl/TopControl.ascx" TagName="TopControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/MenuList.ascx" TagName="Sys_menulist" TagPrefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="UTF-8" />
    <title><%=pageTitle %></title>
    <script src="../scripts/html5.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet" />
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>
<body>
    <form id="form1" runat="server">
        <uc1:TopControl ID="TopControl1" runat="server" />
        <uc2:Sys_menulist ID="MenuList1" runat="server" />
        <section id="main">
            <%=NavigationName %>
            <div class="titlePanel_01">
                <h1>站内信</h1>
            </div>

            <div class="pmListModule_01">

                <div class="control">
                    <label>
                        
                        <input type="checkbox" class="checkbox" name="listSelectAll" onchange="checkAll(this);">全选</label>
                        <asp:Button ID="btnDelete" class="btn btn6" runat="server" Visible='<%#isDelete %>' Text="批量删除" OnClick="btnDelete_Click" />
                </div>
                <div class="tablePanel_01 materialList selectTable">
                    <table cellpadding="0" cellspacing="0" class="table">
                        <thead>
                            <tr>
                                <th class="check"></th>
                                <th class="name">标题</th>
                                <th class="info info1">状态</th>
                                <th class="time">时间</th>
                                <th class="selectControl">操作</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="repItem" runat="server" OnItemCommand="repItem_ItemCommand">
                                <ItemTemplate>
                                    <tr <%# Eval("status").ToString()=="1"?" class=\"readed\"":"" %>>
                                        <td class="check">
                                            <asp:CheckBox ID="chkId" CssClass="checkall" runat="server" /><asp:HiddenField ID="hidId" Value='<%#Eval("lrId")%>' runat="server" />
                                        </td>
                                        <td class="name"><a href='letter_detail.aspx?id=<%#Eval("id") %>&lr_id=<%#Eval("lrId") %>&m_id=<%=m_id %>'>
                                            <asp:Literal ID="lblTitle" runat="server" Text='<%#Eval("title") %>'></asp:Literal></a></td>
                                        <td class="info info1"><%# Eval("status").ToString()=="0"?"未读":"已读" %></td>
                                        <td class="time"><%#Eval("create_time") %></td>
                                        <td class="selectControl">
                                            <%-- <a class="btn btn6" href='letter_detail.aspx?id=<%#Eval("id") %>&lr_id=&<%#Eval("lrId") %>m_id=<%=m_id %>'>查看</a>--%>
                                            <asp:Button ID="btnDelete" Visible='<%#isDelete %>' CssClass="btn btn6" CommandName="del" CommandArgument='<%#Eval("lrId") %>' OnClientClick="return confirm('你确定要删除这条记录？')" runat="server" Text="删除" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <%# repItem.Items.Count == 0 ? "<tr><td style=\"text-align:center;\" colspan=\"5\">暂无数据</td></tr>" : ""%>
                                </FooterTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>


                <div class="pageNum" id="div_page" runat="server">
                </div>
            </div>

            <asp:HiddenField ID="hfReturlUrl" runat="server" />

        </section>

        <script src="../scripts/jquery-1.10.2.min.js"></script>
        <script src="../scripts/jquery.ba-resize.min.js"></script>

        <script src="../scripts/controls.js"></script>
        <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
        <script>
            nav.change('<%=m_id%>');

            //全选取消按钮函数
            function checkAll(chkobj) {
                if ($(chkobj).prop("checked") == true) {

                    $(".checkall input:enabled").prop("checked", true);
                } else {

                    $(".checkall input:enabled").prop("checked", false);
                }
            }

            $(function () {
                $("#btnDelete").click(function () {
                    if ($(".checkall input:checked").size() < 1) {
                        showTip.show("对不起，请选中您要删除的记录", true);
                       // alert("对不起，请选中您要操作的记录");
                        return false;
                    }
                    if (!confirm("删除记录后不可恢复，您确定吗？")) {
                        return false;
                    }
                })
            })
        </script>
    </form>
</body>
</html>
