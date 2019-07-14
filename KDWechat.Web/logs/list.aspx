<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="list.aspx.cs" Inherits="KDWechat.Web.logs.list" %>

<%@ Register Src="../UserControl/TopControl.ascx" TagName="TopControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/MenuList.ascx" TagName="Sys_menulist" TagPrefix="uc2" %>
<!DOCTYPE html>
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
        <uc2:Sys_menulist ID="MenuList1" runat="server" />
        <section id="main">
               <%=NavigationName %>
           
            <!--搜索开始-->
            <div class="searchPanel_01">
                <div class="searchField">
                    <input type="text" class="txt searchTxt" placeholder="搜索操作内容..." runat="server" id="txtKey" maxlength="100">
                    <asp:Button ID="btnSearch" runat="server" Text="" CssClass="btn searchBtn" ToolTip="点击搜索" OnClick="btnSearch_Click"></asp:Button>
                </div>
                <div class="filterField">
                    <asp:DropDownList ID="ddlWechat" AutoPostBack="true" OnSelectedIndexChanged="ddlWechat_SelectedIndexChanged" CssClass="select" DataValueField="id" DataTextField="title" AppendDataBoundItems="true" runat="server">
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlType" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" CssClass="select" DataValueField="id" DataTextField="title" AppendDataBoundItems="true" runat="server">
                        <asp:ListItem Value="-1">选择操作类型</asp:ListItem>
                        <asp:ListItem Value="1">添加</asp:ListItem>
                        <asp:ListItem Value="2">删除</asp:ListItem>
                        <asp:ListItem Value="3">修改</asp:ListItem>
                    </asp:DropDownList>
                    <input type="text" class="txt date" runat="server" id="txt_date_show" placeholder="按操作时间筛选" onfocus="selectStartDate();">
                    <%--这两个文本框是必须要的 ,一个是开始日期，一个是结束日期--%>
                    <asp:TextBox ID="txtbegin_date" runat="server" Style="width: 1px; border: none; background-color:  transparent; color:  transparent;"></asp:TextBox>
                    <asp:TextBox ID="txtend_date" runat="server" Style="width: 1px; border: none; background-color:  transparent; color: transparent;" onfocus="selectEndDate();"></asp:TextBox>
                </div>
            </div>

            <!--搜索结束-->
            <div class="tablePanel_01">
                <table cellpadding="0" cellspacing="0" class="table">
                    <thead>
                        <tr>

                            <th class="info info1">操作人</th>
                            <th class="info info1">公众号</th>
                            <th class="name">操作内容</th>
                            <th class="info info1">类型</th>
                            <th class="info info1">操作IP</th>
                            <th class="info info1">操作时间</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="repList" runat="server">
                            <ItemTemplate>
                                <tr>
                                   
                                    <td class="info info1">
                                        <asp:HiddenField runat="server" ID="hid_UID" Value='<%#Eval("u_id")%>' />
                                        <%#Eval("login_name")%></td>
                                    <td class="info info1"><%#GetWxnameById(Eval("wx_id")) %></td>
                                    <td class="name">
                                        <%#Eval("contents")%>
                                    </td>
                                    <td class="info info1"><%#GetType(Eval("type"))%></td>
                                    <td class="info info1"><%# Eval("ip")%></td>
                                    <td class="info info1"><%# Eval("create_time")%></td>
                                </tr>

                            </ItemTemplate>
                            <FooterTemplate>
                                <%#repList.Items.Count == 0 ? "<tr><td style=\"text-align:center;\" colspan=\"6\">暂无记录</td></tr>" : ""%>
                            </FooterTemplate>
                        </asp:Repeater>


                    </tbody>
                </table>
                <%-- 需要引用function.js--%>
                <div class="pageNum" id="div_page" runat="server">
                </div>
            </div>

        </section>
        <script src="../Scripts/jquery-1.10.2.min.js"></script>
        <script src="../Scripts/function.js"></script>
        <script src="../scripts/controls.js"></script>
        <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
        <script type="text/javascript">
            //选中菜单
            nav.change('<%=m_id%>');
            $(".text").css({ width: 80 });

        </script>
          <!--日期js-->
            <script src="../Scripts/DatePicker/WdatePicker.js"></script>
            <script type="text/javascript">
                function selectStartDate() {
                    var txtbegin_date = $dp.$('txtbegin_date');
                    var txtend_date = $dp.$('txtend_date');
                    var txt_date_show = $dp.$('txt_date_show');

                    WdatePicker(
                    {
                        position: { left: -198, top: 10 },
                        el: 'txtbegin_date',
                        onpicked: function (dp) {
                            txt_date_show.value = dp.cal.getNewDateStr();
                            txtend_date.value = "";
                            txtend_date.focus();
                        },
                        onclearing: function () {
                            txt_date_show.value = "";
                            txtend_date.value = "";
                            txtbegin_date.value = "";
                            parent.location.replace("list.aspx?key=<%=key%>&beginDate=&endDate=&m_id=34&wxid=<%=wxid%>&type=<%=type%>");
                        },
                        doubleCalendar: true,
                        isShowClear: true,
                        readOnly: true,
                        dateFmt: 'yyyy-MM-dd',
                        maxDate: '%y-%M-%d'

                    });
                }

                function selectEndDate() {
                    var txt_date_show = $dp.$('txt_date_show');
                    WdatePicker({
                        position: { left: -120, top: 10 },
                        doubleCalendar: true,
                        isShowClear: true,
                        readOnly: true,
                        dateFmt: 'yyyy-MM-dd',
                        minDate: '#F{$dp.$D(\'txtbegin_date\',{d:0});}',
                        maxDate: '%y-%M-%d}',
                        onpicked: function (dp) {
                            if (txt_date_show.value.length > 0) {
                                txt_date_show.value += " — " + dp.cal.getNewDateStr();
                                location.replace("list.aspx?key=" + $("#txtKey").val() + "&beginDate=" + $("#txtbegin_date").val() + "&endDate=" + $("#txtend_date").val() + "&m_id=<%=m_id%>&wxid=" + $("#ddlWechat").val() + "&type=" + $("#ddlType").val() + "");
                            } else {
                                txt_date_show.value = dp.cal.getNewDateStr();
                            }

                        }
                    });
                }
            </script>
    </form>
</body>
</html>

