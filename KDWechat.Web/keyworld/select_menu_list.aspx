<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="select_menu_list.aspx.cs" Inherits="KDWechat.Web.keyworld.select_menu_list" %>

<!doctype html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=channel_name %></title>
    <script src="../scripts/html5.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet">
    <!--[if lt IE 9 ]><link href="/styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="/styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="/styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>

<body class="bombbox">
 <form id="form1" runat="server">
   <%--     <header id="bombboxTitle">
	<div class="titlePanel_01">
        <div class="btns">
            <asp:Literal ID="lblPublic" runat="server"></asp:Literal>
		</div>
		<h1><%=channel_name %></h1>
	</div>
</header>--%>
 <section id="bombboxMain">
	<div class="tablePanel_01 materialList selectTable">

         <dl>
             <dt><%=channel_name %>：</dt>
             <dd>
                 <input type="text" name="mname" id="mname" runat="server" class="txt required" maxlength="225" />

             </dd>
         </dl>
     </div>
</section>
 </form>
    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../scripts/controls.js"></script>
    <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
    <script src="../Scripts/function.js"></script>
 
</body>
</html>
