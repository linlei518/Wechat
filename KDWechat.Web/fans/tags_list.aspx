<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="tags_list.aspx.cs" Inherits="KDWechat.Web.fans.tags_list" %>

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
	<div class="titlePanel_01">
		<div class="btns">
					<div class="searchField">
			<input type="text" class="txt searchTxt" placeholder="搜索名称..." runat="server" id="txtKey">
            <asp:Button ID="btnSearch" runat="server" Text="" CssClass="btn searchBtn" ToolTip="点击搜索" OnClick="btnSearch_Click"></asp:Button>
		</div><%="<a href=\"javascript:bombbox.openBox('tags_edit.aspx?m_id="+m_id+"');\" class=\"btn btn3\"><i class=\"add\"></i>新建标签</a>" %>
		</div>
		<h1>&nbsp;</h1>
	</div>  
	
	 
	<div class="tablePanel_01">
		<table cellpadding="0" cellspacing="0" class="table">
			<thead>
				<tr>
					<th class="check file"></th>
					<th class="name">标签名称</th>
                    <th class="info info1" style="width:80px;">所属分类</th>
                    <th class="time" style=" width:115px">创建时间</th>
					<th class="control" style=" width:140px">操作</th>
				</tr>
			</thead>
			<tbody>
                <asp:Repeater ID="repList" runat="server" OnItemDataBound="repList_ItemDataBound" OnItemCommand="repList_ItemCommand">
                    <ItemTemplate>
                        	<tr  >
					<td class="check file">
                          <asp:CheckBox ID="chkId" CssClass="checkall" runat="server"   />
                        <asp:HiddenField ID="hidId" Value='<%#Eval("id")%>' runat="server" />
					</td>
					<td class="name">
				        <asp:Literal ID="lblTitle" runat="server" Text='<%# Eval("title") %>'></asp:Literal>
					</td>
                    <td class="info info1"><%#GetCategoryName(Eval("parent_id")) %></td>
					<td class="time"  style=" width:115px"><%# Eval("create_time","{0:yyyy/MM/dd HH:mm}") %></td>
					<td class="control"  style=" width:140px">
						 <a runat="server" id="renameA" class="btn btn6">重命名</a>
                         <asp:LinkButton ID="lbtnDelete" CssClass="btn btn6" CommandArgument='<%# Eval("id") %>' CommandName="del" OnClientClick="return confirm('您确认要删除吗?');" runat="server" Text='删除'></asp:LinkButton>
					</td>
				</tr>

                    </ItemTemplate>
                    <FooterTemplate>
                         <%#repList.Items.Count == 0 ? "<tr><td style=\"text-align:center;\" colspan=\"5\">暂无数据</td></tr>" : ""%>
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
        <script src="../scripts/Bombbox.js"></script>
        <!--弹出框JS 调用方法：1.开启弹出框：bombbox.openBox('链接地址，可以带参')，2.关闭弹出框：bombbox.closeBox();注意：此方法无需在弹出框里面的页面引用-->
        <script>nav.change('<%=m_id%>');</script>


    </form>
</body>
</html>


