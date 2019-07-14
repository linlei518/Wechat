<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="material_search.ascx.cs" Inherits="KDWechat.Web.UserControl.material_search" %>
<%--日期js--%>
<script src="../Scripts/DatePicker/WdatePicker.js"></script>
<script type="text/javascript">
    function selectStartDate() {
        var txtbegin_date = $dp.$('material_search1_txtbegin_date');
        var txtend_date = $dp.$('material_search1_txtend_date');
        var txt_date_show = $dp.$('material_search1_txt_date_show');

        WdatePicker(
        {
            position: { left: -212, top: 10 },
            el: 'material_search1_txtbegin_date',
            onpicked: function (dp) {
                txt_date_show.value = dp.cal.getNewDateStr();
                txtend_date.value = "";
                txtend_date.focus();
            },
            onclearing: function () {
                txt_date_show.value = "";
                txtend_date.value = "";
                txtbegin_date.value = "";
                var urll = "<%=(page_link.Contains("?")?page_link+"&":page_link+"?")%>key=" + $("#material_search1_txtKey").val() + "&beginDate=&endDate=&group_id=" + $("#material_search1_ddlGroup").val() + "&is_pub=<%=is_pub%>&m_id=<%=m_id%>";
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
        var txt_date_show = $dp.$('material_search1_txt_date_show');
        WdatePicker({
            position: { left: -120, top: 10 },
            doubleCalendar: true,
            isShowClear: true,
            readOnly: true,
            dateFmt: 'yyyy-MM-dd',
            minDate: '#F{$dp.$D(\'material_search1_txtbegin_date\',{d:0});}',
            maxDate: '%y-%M-%d}',
            onpicked: function (dp) {
                if (txt_date_show.value.length > 0) {
                    txt_date_show.value += " — " + dp.cal.getNewDateStr();
                    location.replace("<%=(page_link.Contains("?")?page_link+"&":page_link+"?")%>key=" + $("#material_search1_txtKey").val() + "&beginDate=" + $("#material_search1_txtbegin_date").val() + "&endDate=" + $("#material_search1_txtend_date").val() + "&group_id=" + $("#material_search1_ddlGroup").val() + "&is_pub=<%=is_pub%>&m_id=<%=m_id%>");
                } else {
                    txt_date_show.value = dp.cal.getNewDateStr();
                }

            }
        });
    }
</script>
<div class="searchPanel_01">
    <div class="searchField">
        <input type="text" class="txt searchTxt" placeholder="搜索标题..." runat="server" id="txtKey">
        <asp:Button ID="btnSearch" runat="server" Text="" CssClass="btn searchBtn" ToolTip="点击搜索" OnClick="btnSearch_Click"></asp:Button>
    </div>
    <div class="filterField">
        <asp:DropDownList ID="ddlGroup" AutoPostBack="true" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged" CssClass="select" DataValueField="id" DataTextField="title" AppendDataBoundItems="true" runat="server">
            <asp:ListItem Value="-1">所有分组</asp:ListItem>
            <asp:ListItem Value="0">默认分组</asp:ListItem>
        </asp:DropDownList>

        <input type="text" class="txt date" runat="server" id="txt_date_show" placeholder="按时间筛选" onfocus="selectStartDate();">
        <%--这两个文本框是必须要的 ,一个是开始日期，一个是结束日期--%>
        <asp:TextBox ID="txtbegin_date" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;"></asp:TextBox>
        <asp:TextBox ID="txtend_date" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;" onfocus="selectEndDate();"></asp:TextBox>
    </div>
</div>
