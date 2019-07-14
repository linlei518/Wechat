<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="retrans_server_list.aspx.cs" Inherits="KDWechat.Web.keyworld.retrans_server_list" %>

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
<body onload="setV()">
    <form id="form1" runat="server">
        <uc1:TopControl ID="TopControl1" runat="server" />
        <uc2:Sys_menulist ID="MenuList1" runat="server" />
        <section id="main">
            <%= NavigationName %>
            <div class="titlePanel_01">
                <div class="btns" runat="server" id="div_add">
                    <a href="javascript:bombbox.openBox('retrans_server_detail.aspx');" class="btn btn3"><i class="add"></i>新建第三方服务</a>
                </div>
            </div>

            <!--搜索开始-->
            <div class="searchPanel_01">
                <div class="searchField">
                    <input type="text" class="txt searchTxt" placeholder="搜索名称..." runat="server" id="txtKey" maxlength="100" />
                    <asp:Button ID="btnSearch" runat="server" Text="" CssClass="btn searchBtn" ToolTip="点击搜索" OnClick="btnSearch_Click"></asp:Button>
                </div>
                <div class="filterField">
                    <input type="text" class="txt date" runat="server" id="txt_date_show" placeholder="按时间筛选" onfocus="selectStartDate();" />
                    <%--这两个文本框是必须要的 ,一个是开始日期，一个是结束日期--%>
                    <asp:TextBox ID="txtbegin_date" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;"></asp:TextBox>
                    <asp:TextBox ID="txtend_date" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;" onfocus="selectEndDate();"></asp:TextBox>
                </div>
            </div>

            <!--搜索结束-->
            <div class="tablePanel_01">
                <table cellpadding="0" cellspacing="0" class="table">
                    <thead>
                        <tr>
                            <th class="name">名称</th>
                            <th class="info info1" style="width:80px;" >连续转发次数</th>
                            <th class="time"  style="width:115px">创建时间</th>
                            <th class="control" style="width:30px">操作</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="Repeater1" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td class="name">
                                        <a href='retrans_server_detail.aspx?id=<%#Eval("id") %>&m_id=<%=m_id %>'><%#Eval("title") %></a>
                                    </td>
                                    <td class="info info1" ><%#Eval("image_retrans_times") %></td>
                                    <td class="time"><%#Eval("create_time") %></td>
                                    <td class="control">
                                        <a href="javascript:bombbox.openBox('retrans_server_detail.aspx?id=<%#Eval("id") %>');" class="btn btn6">编辑</a>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                <%# Repeater1.Items.Count == 0 ? "<tr><td style=\"text-align:center;\" colspan=\"3\">暂无数据</td></tr>" : ""%>
                            </FooterTemplate>
                        </asp:Repeater>
                        <asp:HiddenField ID="hfReturlUrl" runat="server" />
                    </tbody>
                </table>
                <div class="pageNum" id="div_page" runat="server">
                </div>
            </div>
        </section>
        <script src="../Scripts/jquery-1.10.2.min.js"></script>
        <script src="../Scripts/function.js"></script>
        <script src="../scripts/controls.js"></script>
        <script src="../scripts/Bombbox.js"></script>

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
                        parent.location.replace("retrans_server_list.aspx?key=<%=key%>&beginDate=&endDate=&m_id=<%=m_id%>");
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
                            location.replace("retrans_server_list.aspx?key=<%=key%>&beginDate=" + $("#txtbegin_date").val() + "&endDate=" + $("#txtend_date").val() + "&m_id=<%=m_id%>");
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

