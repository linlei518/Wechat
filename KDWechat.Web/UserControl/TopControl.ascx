<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopControl.ascx.cs" Inherits="KDWechat.Web.UserControl.TopControl" %>
 

<header id="header">
	<div class="logoField">
		<span class="logo">微信管理平台</span>
	</div>
	<div class="userField">
		<p>您好，<a  ><asp:Literal ID="lblUserName" runat="server"></asp:Literal><i class="moreInfo"></i></a></p>
		<ul>
			<%--<asp:Literal ID="lit_letCount" runat="server"></asp:Literal>--%>
             <asp:Literal ID="lblNoReplyCount" runat="server"></asp:Literal>
            <asp:Literal ID="lblMenu" runat="server"></asp:Literal>
            <li><a href="/Account/change_password.aspx?m_id=<%if (u_type == 1 || u_type == 4) { Response.Write("57"); } else { Response.Write("61"); } %>">修改密码</a></li>
			<li><a href="/wxlogin/loginout.aspx">退出</a></li>
		</ul>
	</div>
    <nav class="navField">
		<ul>
			 
             <asp:Literal ID="lblTopMenu" runat="server" Visible="False"></asp:Literal>
		</ul>
	</nav>
   
	<div class="titleField">
		 <asp:Literal ID="lblWeiXin" runat="server"></asp:Literal>
	</div>
</header>