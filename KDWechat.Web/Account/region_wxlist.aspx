<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="region_wxlist.aspx.cs" Inherits="KDWechat.Web.Account.region_wxlist" %>

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
                    <%--<div class="btns" runat="server" id="div_add">
                        <%=u_type==2? "<a href=\"NewWXAccount.aspx?m_id="+m_id+"\" class=\"btn btn3\"><i class=\"add\"></i>新建微信公众号</a>":""%>
                        <input class="btn btn5" value="查看相关统计数据" type="button" onClick="statisticsContrast.open(this, [])">
                    </div>--%>
                    <h1> 微信公众号列表</h1>
                </div>
                  <div class="searchPanel_01">
                <div class="searchField">
                    <input type="text" class="txt searchTxt" placeholder="搜索公众号名称..." runat="server" id="txtKey">
                    <asp:Button ID="btnSearch" runat="server" Text="" CssClass="btn searchBtn" ToolTip="点击搜索" OnClick="btnSearch_Click"></asp:Button>
                </div>
                <div class="filterField">

                    <input type="text" class="txt date" runat="server" id="txt_date_show" placeholder="按创建时间筛选" onfocus="selectStartDate();">
                    <%--这两个文本框是必须要的 ,一个是开始日期，一个是结束日期--%>
                    <asp:TextBox ID="txtbegin_date" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;"></asp:TextBox>
                    <asp:TextBox ID="txtend_date" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;" onfocus="selectEndDate();"></asp:TextBox>

                </div>
            </div>
                <div class="tablePanel_01">
                    <table cellpadding="0" cellspacing="0" class="table">
                        <thead>
                            <tr>
                                <th class="name">名称</th>
                                <th class="info info1" style=" width:80px">类型</th>
                                <th class="time" style=" width:90px">创建者</th>
                                <th class="time" style=" width:80px">企业号管理员</th>
                                <th class="time" style=" width:120px">创建时间</th>
                                <th class="time" style=" width:120px">最后操作时间</th>
                                <th class="control" style=" width:200px; text-align:right">操作</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound" OnItemCommand="Repeater1_ItemCommand">
                                <ItemTemplate>
                                    <tr>
                                        <td class="name">
                                            <a href='<%# Eval("status").ToString()=="1"?"NewWxAccount.aspx?id="+Eval("id")+"&m_id="+m_id+"":"javascript:void();" %>'>
                                                <asp:Literal ID="lblTitle" runat="server" Text='<%#Eval("wx_pb_name") %>'></asp:Literal></a>
                                        </td>
                                        <td class="info info1"><%#(KDWechat.Common.WeChatServiceType)(int.Parse(Eval("type_id").ToString())) %></td>
                                         <td class="time" ><%#Eval("u_name") %></td>
                                         <td class="time" ><%#Eval("qy_manager_nick") %></td>
                                        <td class="time" ><%#Eval("create_time") %></td>
                                        <td class="time"><%#GetLastOpreation(Eval("id")) %></td>
                                        <td class="control">
                                              <%# GetManageLink(Eval("status"),Eval("id")) %>
                                            <asp:Button ID="aEdit" CssClass="btn btn6" CommandName="edit" CommandArgument='<%#Eval("id") %>' runat="server" Text="编辑" />
                                            <asp:Button ID="Button1" CssClass="btn btn6" CommandName="status" CommandArgument='<%#Eval("id") %>' runat="server"  Text='<%# Eval("status").ToString()=="1"?"关闭":"开启" %>' />
                                            <asp:Button ID="btnDelete" CssClass="btn btn6" Visible="false" CommandName="del" CommandArgument='<%#Eval("id")+","+Eval("wx_pb_name") %>' OnClientClick="return confirm('您确认要删除吗？')" runat="server" Text="删除" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <%# Repeater1.Items.Count == 0 ? "<tr><td style=\"text-align:center;\" colspan=\"8\">暂无数据</td></tr>" : ""%>
                                </FooterTemplate>
                            </asp:Repeater>
                            <asp:HiddenField ID="hfReturlUrl" runat="server" />
                        </tbody>
                    </table>
                    <div class="pageNum" id="div_page" runat="server">
                    </div>
                </div>
        </section>
        <script src="../scripts/jquery-1.10.2.min.js"></script>
        <script src="../scripts/controls.js"></script><!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
        <script src="../scripts/Bombbox.js"></script>
        <script src="../scripts/statisticsContrast.js"></script>
        <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
        <script>
            statisticsContrast.readAccount(<%=strWxlist%>);//这个数组是所有的标签名字
            statisticsContrast.accountSubmit = function (submitValue) {
                var ids = '';
                var names = '';
                for (var i in submitValue) {
                    ids += submitValue[i].id + ',';
                    //names += submitValue[i].name + ',';
                }

                if (ids == '') {
                    alert('至少选中一项');
                }
                else
                {
                    var url = 'fans_statistics.aspx?Ids=' + encodeURI(ids)+'&m_id=<%=m_id%>';
                    location.href = url;
                }
               // alert('选中的是：' + names + ' 他们的ID分别是：' + ids);
            }

            nav.change('<%=m_id%>'); 
        </script>
         <script src="../Scripts/DatePicker/WdatePicker.js"></script>
    <script type="text/javascript">
        function selectStartDate() {
            var txtbegin_date = $dp.$('txtbegin_date');
            var txtend_date = $dp.$('txtend_date');
            var txt_date_show = $dp.$('txt_date_show');

            WdatePicker(
            {
                position: { left: -212, top: 10 },
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
                    var urll = "region_wxlist.aspx?key=" + $("#txtKey").val() + "&beginDate=&endDate=&m_id=<%=m_id%>";
                    location.href = urll;
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
                        location.replace("region_wxlist.aspx?key=" + $("#txtKey").val() + "&beginDate=" + $("#txtbegin_date").val() + "&endDate=" + $("#txtend_date").val() + "&m_id=<%=m_id%>");
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
