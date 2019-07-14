<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="select_wechats.aspx.cs" Inherits="KDWechat.Web.setting.select_wechats" %>

<!doctype html>
<html>
<head>
    <meta charset="UTF-8">
    <title>模板分配公众号</title>
    <script src="../scripts/html5.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet">
    <!--[if lt IE 9 ]><link href="/styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="/styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="/styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>

<body class="bombbox">
    <header id="bombboxTitle">
	<div class="titlePanel_01">
        <div class="btns">
			<a  href="javascript:window.parent.bombbox.openBox('select_wechats_new.aspx?template_id=<%=template_id %>')" class="btn btn3">分配新公众号</a>
		</div>
		<h1>已分配的公众号</h1>
	</div>
</header>
    <form id="form1" runat="server">


    <section id="bombboxMain">
	<div class="tablePanel_01 materialList selectTable">
		<table cellpadding="0" cellspacing="0" class="table">
			<thead>
				<tr>
					<th class="name">名称</th>
					<th class="info info1">类型</th>
					<th class="selectControl">操作</th>
				</tr>
			</thead>
			<tbody>
                <asp:Repeater ID="repList" runat="server" OnItemCommand="repList_ItemCommand">
                    <ItemTemplate>
                        	<tr>
					            <td class="name"><%# Eval("wx_pb_name") %></td>
					           
					            <td class="info info1"><%# (KDWechat.Common.WeChatServiceType)(int.Parse(Eval("type_id").ToString())) %></td>
					            <td class="selectControl">
                                    <asp:Button ID="Button1" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="remove" Text="移除" CssClass="btn btn5" />
					            </td>
				            </tr>
                    </ItemTemplate>
                </asp:Repeater>
				 
			</tbody>
		</table>
		  <%-- 需要引用function.js--%>
		<div class="pageNum" id="div_page" runat="server">
			 
		</div>
	</div>
</section>
    </form>
    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../scripts/controls.js"></script>
    <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
    <script src="../Scripts/function.js"></script>
 
</body>
</html>
