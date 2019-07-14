<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="message_list.aspx.cs"  Inherits="KDWechat.Web.fans.message_list" %>


<%@ Register Src="../UserControl/TopControl.ascx" TagName="TopControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/MenuList.ascx" TagName="MenuList" TagPrefix="uc2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="UTF-8">
    <title><%=pageTitle %></title>
    <script src="../scripts/html5.js" type="text/javascript"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet">
    <!--[if lt IE 9 ]><link href="styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->

</head>
<body>
    <form id="form1" runat="server">
        <uc1:TopControl ID="TopControl1" runat="server" />
        <uc2:MenuList ID="MenuList1" runat="server" />

        <section id="main">
               <%=NavigationName %>
	 
	    <div class="searchPanel_01">
            <div class="searchField">
                <input type="text" class="txt searchTxt" placeholder="搜索内容..." runat="server" id="txtKey">
                <asp:Button ID="btnSearch" runat="server" Text="" CssClass="btn searchBtn" ToolTip="点击搜索" OnClick="btnSearch_Click"></asp:Button>
            </div>
            <div class="filterField">
                     <asp:DropDownList ID="ddlReplyStatus" AutoPostBack="true" OnSelectedIndexChanged="ddlReplyStatus_SelectedIndexChanged" CssClass="select"  runat="server">
                            <asp:ListItem Value="-1">所有状态</asp:ListItem>
                            <asp:ListItem Value="0">未回复</asp:ListItem>
                          <asp:ListItem Value="1">已回复</asp:ListItem>
                         <asp:ListItem Value="2">已过期</asp:ListItem>
                    </asp:DropDownList>
                <input type="text" class="txt date" runat="server" id="txt_date_show" placeholder="按时间筛选" onfocus="selectStartDate();">
                <%--这两个文本框是必须要的 ,一个是开始日期，一个是结束日期--%>
                <asp:TextBox ID="txtbegin_date" runat="server" Style="width: 1px; border: none; background-color:transparent ; color: transparent;"></asp:TextBox>
                <asp:TextBox ID="txtend_date" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;" onfocus="selectEndDate();"></asp:TextBox>
            </div>
        </div>
	<div class="userMessageModule_01">
		
		 
		 <div class="userMessageListPanel_01">
			<div class="list">
				<ul>
                    <asp:Repeater ID="repList" runat="server">
                        <ItemTemplate>

                            <li class="userMessage">
                               <div class="img"> 
                                   <span> 
                                       <a href="user_msg_list.aspx?openId=<%# Eval("open_id") %>&m_id=<%=m_id %>"><img src='<%# Eval("img_head") %>' onerror="this.src='../images/logo_01.png'" /> </a>

                                   </span> 

                               </div>
                                <div class="info">
                                     <div class="time"><%# Eval("create_time") %><span><%#GetState(Eval("last_interact_time"),Eval("reply_state")) %></span></div> 
                                     <div class="name"><a href="user_msg_list.aspx?openId=<%# Eval("open_id") %>&m_id=<%=m_id %>"><%# Eval("nick_name") %></a></div>

                                </div>
                                <div class="btns">
							        <a href="user_msg_list.aspx?openId=<%# Eval("open_id") %>&m_id=<%=m_id %>" class="link"><i class="navTime"></i>查看消息记录</a>
                                    <%# isEdit ? GetReplyStatus(Eval("last_interact_time"),Eval("open_id")):"" %>
						        </div>
                                <%# isExport ? GetContents(Eval("msg_type"),Eval("contents"),Eval("media_id")):"" %>
					        </li>
                        </ItemTemplate>
                         <FooterTemplate>
                         <%#repList.Items.Count == 0 ? "<li><div style=\"text-align:center;\"  >暂无数据</div></li>" : ""%>
                    </FooterTemplate>
                    </asp:Repeater>
					 
				</ul>
			</div>
		
			<div class="pageNum" id="div_page" runat="server">
			</div>
		</div>
	</div>
	
</section>
    </form>

    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../scripts/Bombbox.js"></script>
    <script src="../Scripts/function.js"></script>
    <script src="../scripts/controls.js"></script>
    <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
    <script>
        nav.change('<%=m_id%>');//此处填写被激活的导航项的ID
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
                    var urll = "message_list.aspx?m_id=80&key=" + $("#txtKey").val() + "&beginDate=&endDate=&replyStatus=<%=replyStatus%>&m_id=<%=m_id%>";
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
                        location.replace("message_list.aspx?key=" + $("#txtKey").val() + "&beginDate=" + $("#txtbegin_date").val() + "&endDate=" + $("#txtend_date").val() + "&m_id=<%=m_id%>&replyStatus=" + $("#ddlReplyStatus").val() + "");
                    } else {
                        txt_date_show.value = dp.cal.getNewDateStr();
                    }

                }
            });
        }
    </script>


</body>
</html>
