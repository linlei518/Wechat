<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Select_QY_Group.aspx.cs" Inherits="KDWechat.Web.QY.Select_QY_Group" %>
<%@ Register TagName="TopControl" Src="~/UserControl/TopControl.ascx" TagPrefix="uc" %>

<%@ Register TagName="MenuList" Src="~/UserControl/MenuList.ascx" TagPrefix="uc" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../scripts/html5.js"></script>
    <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->

    <link type="text/css" href="../styles/global.css" rel="stylesheet" />
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->

</head>
<body>
    <uc:TopControl ID="TopControl1" runat="server" />
    <uc:MenuList ID="MenuList1" runat="server" />

    <form id="form1" runat="server">
            <section id="main">
                <div class="tablePanel_01">
                    <asp:Repeater ID="DataRepeater" runat="server" OnItemCommand="DataRepeater_ItemCommand">

                        <HeaderTemplate>
                            <table cellpadding="0" cellspacing="0" class="table">
                                <thead>
                                    <tr>
                                        <th class="name">企业号分组名</th>
                                        <th class="contro2" style="width: 60px; padding: 0 10px">操作</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td class="name"><%#Eval("name") %></td>
                                <td class="contro2">
                                    <asp:LinkButton ID="LinkButton2" CommandArgument='<%#Eval("id") %>' CommandName="set" runat="server" CssClass="btn btn6">选择</asp:LinkButton>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <%# DataRepeater.Items.Count == 0 ? "<tr><td style=\"text-align:center;\" colspan=\"8\">暂无数据</td></tr>" : ""%>
                                </tbody>
		                </table>
                        </FooterTemplate>
                    </asp:Repeater>

                    <asp:HiddenField ID="hfReturlUrl" runat="server" />




                    <div class="btnPanel_01">
                        <asp:LinkButton ID="btnCancel" Name="btnCancel" runat="server" CssClass="btn btn2">取消</asp:LinkButton>
                    </div>
                </div>
            </section>
        <script src="../scripts/jquery-1.10.2.min.js"></script>
        <script src="../scripts/controls.js"></script>
        <script src="../Scripts/function.js"></script>

    </form>
    <script>
        $("#btnCancel").click(function () {
            parent.bombbox.closeBox();
        });

    </script>
</body>
</html>
