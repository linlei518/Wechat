<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="activity_apply_list.aspx.cs" Inherits="KDWechat.Web.activity.activity_apply_list" %>


<%@ Import Namespace="KDWechat.Common" %>

<!DOCTYPE html>
<%@ Register Src="../UserControl/TopControl.ascx" TagName="TopControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/MenuList.ascx" TagName="MenuList" TagPrefix="uc2" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title><%=pageTitle %></title>
     <script src="../scripts/html5.js"></script>
      <link type="text/css" href="../styles/global.css" rel="stylesheet">
    <link type="text/css" href="../styles/style.css" rel="stylesheet">
</head>



     




<body class="mainbody">
    <form id="form2" runat="server">
           <uc1:TopControl ID="TopControl1" runat="server" />
        <uc2:MenuList ID="MenuList1" runat="server" />
        <!--导航栏-->
         <section id="main">
                    <%=NavigationName %>
                <div class="titlePanel_01">
                    <div class="btns" runat="server" id="div_add">
                          <asp:LinkButton ID="btn_checked" runat="server"  CssClass="btn btn3" OnClick="btn_checked_Click"><i class="add"></i>批量领取</asp:LinkButton>
                       <asp:LinkButton ID="btnExport" runat="server"  CssClass="btn btn3" OnClick="btnExport_Click"><i class="add"></i>批量导出</asp:LinkButton>
                    </div>
                    <h1>活动报名管理列表</h1>
                </div>
                  <div class="searchPanel_01">
                <div class="searchField">
                    <input type="text" class="txt searchTxt" placeholder="手机号码..." runat="server" id="txtKeywords">
                    <asp:Button ID="btnSearch" runat="server" Text="" CssClass="btn searchBtn" ToolTip="点击搜索" OnClick="btnSearch_Click"></asp:Button>
                </div>
                <div class="filterField">
                    <label><input type="checkbox" class="checkbox" name="listSelectAll" onChange="checkAll(this)"/>全选</label>
                    <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="True" AppendDataBoundItems="true" OnSelectedIndexChanged="btnSearch_Click"  CssClass="select">
                                  <asp:ListItem Value="-1" Selected="True">是否中奖</asp:ListItem>
                                    <asp:ListItem Value="0" >是</asp:ListItem>
                                    <asp:ListItem Value="1" >否</asp:ListItem>
                                    
                                </asp:DropDownList>
                     <input type="text" class="txt date" runat="server" id="txt_date_show" placeholder="按时间筛选" onfocus="selectStartDate();">
                <%--这两个文本框是必须要的 ,一个是开始日期，一个是结束日期--%>
                <asp:TextBox ID="txtbegin_date" runat="server" Style="width: 1px; border: none; background-color:transparent ; color: transparent;"></asp:TextBox>
                <asp:TextBox ID="txtend_date" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;" onfocus="selectEndDate();"></asp:TextBox>
                </div>
            </div>
                <div class="tablePanel_01">
                    <table cellpadding="0" cellspacing="0" class="table">
                        <thead>
                            <tr>
                            <th width="5%">选择</th>
                            <th align="left"width="10%">用户昵称</th>
                            <th align="left"width="10%">姓名</th>
                            <th align="left"width="10%">手机号码</th>
                            <th align="left"width="5%">头像</th>
                            <th align="left" width="5%">报名时间</th>
                            <th align="left" width="5%">奖品名称</th>
                            <th align="left"width="5%">中奖码</th>
                            <th align="left" width="5%">是否领取</th>
                            <th width="10%">操作</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptList" runat="server" OnItemCommand="rptList_ItemCommand">
                                <ItemTemplate>
                                    <tr>
                       <td align="center">
                           
                            <asp:CheckBox ID="chkId" CssClass="checkall" runat="server" name="listSelector" Style="vertical-align: middle;" /><%--Visible='<%#Utils.ObjToInt(Eval("prize"),-1)>=0 %>'--%> 
                            <asp:HiddenField ID="hidId" Value='<%#Eval("id")%>' runat="server" />
                            <asp:HiddenField ID="HiddenField1" Value='<%#Eval("prize_number")%>' runat="server" />
                        </td>
                        <td>
                            <asp:Literal ID="lblTitle" runat="server" Text='<%#isEdit?" <a href=\"activity_apply_edit.aspx?action="+Enums.RoleActionType.Edit+"&id="+Eval("id")+"&m_id="+m_id+"\">"+Eval("nick_name")+"</a>":Eval("company_name") %>'></asp:Literal></td>
                        <td ><%#Eval("user_name")%></td>
                        <td ><%#Eval("phone")%></td>
                         <td> <img width="80px" height="80px" src="<%#Eval("head_img_url") %>" /></td>
                        <td ><%#string.Format("{0:g}",Eval("create_date"))%></td>
                         <td ><%#Eval("prize_name")%></td>
                        <td align="center"><%#Eval("prize_number")%></td>
                         <td ><%#Utils.ObjToInt(Eval("status"),0)==0?"否":"是"%></td>
                        <td align="center">
                            <%# (isEdit?" <a  class=\"btn btn6\" href=\"activity_apply_edit.aspx?action="+Enums.RoleActionType.Edit+"&id="+Eval("id")+"&m_id="+m_id+"\">查看</a>":"") %>
                             <asp:LinkButton CssClass="btn btn6" ID="lbtnDel" runat="server" Visible='<%#true %>' OnClientClick="return confirm('您确定要删除吗？删除后报名信息将无法恢复！');" ToolTip='删除'  CommandArgument='<%# Eval("id") %>' CommandName="del">删除</asp:LinkButton>
                             <asp:LinkButton CssClass="btn btn6" ID="LinkButton1" runat="server" Visible='<%#!string.IsNullOrEmpty(Utils.ObjectToStr(Eval("prize_name"))) %>' OnClientClick="return confirm('您确定要领取吗？领取后抽奖报名信息将无法撤销！');" ToolTip='领取'   CommandArgument='<%# Eval("id") %>' CommandName="checked">领取</asp:LinkButton>

                        </td>
                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <%# rptList.Items.Count == 0 ? "<tr><td style=\"text-align:center;\" colspan=\"9\">暂无数据</td></tr>" : ""%>
                                </FooterTemplate>
                            </asp:Repeater>
                            <asp:HiddenField ID="hfReturlUrl" runat="server" />
                        </tbody>
                    </table>
                    <div class="pageNum" id="div_page" runat="server">
                    </div>
                </div>
        </section>
    </form>

   <script src="../scripts/jquery-1.10.2.min.js"></script>
        <script src="../scripts/controls.js"></script><!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
        <script src="../scripts/Bombbox.js"></script>
        <script src="../scripts/statisticsContrast.js"></script>
        <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
        <script>
           

            nav.change('<%=m_id%>'); 
        </script>
</body>

<script>
        //创建窗口
         function showdialog(id) {
            var objNum = arguments.length;
            var d = top.dialog({
                title: '积分商品组织机构',
                url: '/product/product_dpt.aspx?id=' + id,
                width: 600,
                height: 440,
                onclose: function () {
                }
            }).showModal();

         }


         //全选取消按钮函数
         function checkAll(chkobj) {
             if ($(chkobj).prop("checked") == true) {
                
                 $(".checkall input:enabled").prop("checked", true);
             } else {
                
                 $(".checkall input:enabled").prop("checked", false);
             }
         }

    </script>
    
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
                    var urll = "activity_apply_list.aspx?key=" + $("#txtKeywords").val() + "&beginDate=&endDate=&m_id=<%=m_id%>";
                    parent.location.href = urll;
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
                        location.replace("activity_apply_list.aspx?key=" + $("#txtKeywords").val() + "&beginDate=" + $("#txtbegin_date").val() + "&endDate=" + $("#txtend_date").val() + "&m_id=<%=m_id%>");
                    } else {
                        txt_date_show.value = dp.cal.getNewDateStr();
                    }

                }
            });
        }
    </script>
