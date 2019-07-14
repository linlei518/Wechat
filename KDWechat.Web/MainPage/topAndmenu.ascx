<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="topAndmenu.ascx.cs" Inherits="KDWechat.Web.main.topAndmenu" %>
<header id="header">
    <div class="logoField">
        <span class="logo">
            <img src="/images/logo_01.png" width="40" height="40" alt="">凯德OTO管理平台</span>
    </div>
    <div class="userField">
        <p>您好，<a><%=u_name %><i class="moreInfo"></i></a></p>
        <ul>

            <asp:Literal ID="lblMenu" runat="server"></asp:Literal>
            <li><a href="change_password.aspx?m_id=4">修改密码</a></li>
            <li><a href="/wxlogin/loginout.aspx">退出</a></li>
        </ul>
    </div>
    <%-- <nav class="navField">
            <ul>
                <asp:Literal ID="lblTopMenu" runat="server"></asp:Literal>
            </ul>
        </nav>--%>
</header>
<nav id="mainNav">
    <ul>
        <li id="1"><a id="1" onclick="dialogue.dlLoading();" href="index.aspx?m_id=1"><i class="navLog"></i>平台入口</a></li>
        <asp:Literal ID="lblMenu2" runat="server"></asp:Literal>
        <li id="4"><a id="4" onclick="dialogue.dlLoading();" href="change_password.aspx?m_id=4"><i class="navPassword"></i>修改密码</a></li>
    </ul>
</nav>
