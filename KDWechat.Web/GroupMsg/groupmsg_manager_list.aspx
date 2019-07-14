<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="groupmsg_manager_list.aspx.cs" Inherits="KDWechat.Web.GroupMsg.groupmsg_manager_list" %>
<%@ Register Src="../UserControl/TopControl.ascx" TagName="TopControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/MenuList.ascx" TagName="MenuList" TagPrefix="uc2" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8"/>
    <title><%=pageTitle %></title>
    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/function.js"></script>
    <script src="../scripts/html5.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet"/>
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>
<body>
    <uc1:TopControl ID="TopControl1" runat="server" />
    <uc2:MenuList ID="MenuList1" runat="server" />
    <form id="form2" runat="server">
        <section id="main">
            <%=NavigationName %>
	        <div class="titlePanel_01">
		        <div class="btns">
			        <a href="javascript:bombbox.openBox('groupmsg_manager_detail.aspx?m_id=<%=m_id %>')" class="btn btn3"><i class="add"></i>新建群发管理员</a>
		        </div>
                <h1>&nbsp;</h1>
	        </div>
	
	
	        <div class="tablePanel_01">
                <asp:Repeater ID="DataRepeater" runat="server" OnItemCommand="DataRepeater_ItemCommand" >
                    
                    <HeaderTemplate>
                        <table cellpadding="0" cellspacing="0" class="table">
			                <thead>
				                <tr>
					                <th class="name">昵称</th>
					                <th class="info info1" style=" width:120px">用户名（唯一）</th>
					                <th class="info info1" style=" width:60px">手机</th>
					                <th class="info info1" style=" width:60px">微信号</th>
                                      <th class="info info1" style=" width:80px">邮箱地址</th>
                                      <th class="time" style=" width:115px">创建时间</th>
					                <th class="contro2" style=" width:56px; padding:0 10px">操作</th>
				                </tr>
			                </thead>
			                <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                         <tr>
                            <td class="name"><%#Eval("nick_name") %></td>
                            <td class="info info1"><%#Eval("user_name") %></td>
                            <td class="info info1"><%#Eval("mobile") %></td>
                            <td class="info info1"><%#Eval("wechat_no") %></td>
                            <td class="info info1"><%#Eval("email") %></td>
    					    <td class="info info1"><%#Eval("create_time") %></td>
					        <td class="contro2">
<%--                                <a href="javascript:bombbox.openBox('groupmsg_manager_detail.aspx?m_id=<%=m_id %>&id=<%#Eval("id") %>')" class="btn btn6">编辑</a>--%>
                                <asp:LinkButton ID="LinkButton1" OnClientClick="return confirm('您确定要删除？')" CommandArgument='<%#Eval("user_name") %>' CommandName="del" runat="server" CssClass="btn btn6">删除</asp:LinkButton>
<%--                                <asp:LinkButton ID="LinkButton2" CommandName="send" CommandArgument='<%#Eval("user_name") %>' CssClass="btn btn6" runat="server">发送邀请</asp:LinkButton>--%>
                            </td>
				        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                                <%# DataRepeater.Items.Count == 0 ? "<tr><td style=\"text-align:center;\" colspan=\"8\">暂无数据</td></tr>" : ""%>
                                </tbody>
		                </table>
                    </FooterTemplate>
                </asp:Repeater>
		    
                <asp:HiddenField ID="hfReturlUrl" runat="server" />

			    
		        <div class="pageNum" id="div_page" runat="server">
			 
		        </div>
	        </div>
        </section>


    <script src="../scripts/controls.js"></script>
    <script src="../scripts/Bombbox.js"></script>
    <script>
        nav.change('<%=m_id%>'); 
    </script>
            </form>
</body>
</html>
