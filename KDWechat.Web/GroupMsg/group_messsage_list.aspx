<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="group_messsage_list.aspx.cs" Inherits="KDWechat.Web.GroupMsg.group_messsage_list" %>

<%@ Register TagName="TopControl" Src="~/UserControl/TopControl.ascx" TagPrefix="uc" %>

<%@ Register TagName="MenuList" Src="~/UserControl/MenuList.ascx" TagPrefix="uc" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8"/>
    <title><%=pageTitle %></title>
    <script src="../scripts/html5.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet"/>
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>
<body>
    <uc:TopControl ID="TopControl1" runat="server" />
    <uc:MenuList ID="MenuList1" runat="server" />
    <form id="form2" runat="server">
        <section id="main">
                    <%=NavigationName %>
	        <div class="titlePanel_01">
		        <div class="btns">
			        <a href="SendGroupMessage.aspx?m_id=40" class="btn btn3"><i class="add"></i>新建群发消息</a>
   			        <a href="javascript:copyString.copy('<%= historyLink %>')" class="btn btn3"><i class="add"></i>复制历史群发连接</a>
		        </div>
                <h1>&nbsp;</h1>
	        </div>
	
	
	        <div class="tablePanel_01">
                <asp:Repeater ID="DataRepeater" runat="server" OnItemCommand="DataRepeater_ItemCommand" >
                    
                    <HeaderTemplate>
                        <table cellpadding="0" cellspacing="0" class="table">
			                <thead>
				                <tr>
					                <th class="name">群发描述</th>
					                <th class="info info1" style=" width:60px">信息类型</th>
					                <th class="info info1" style=" width:60px">接收人数</th>
					                <th class="info info1" style=" width:60px">发送状态</th>
                                      <th class="info info1" style=" width:50px">是否定时</th>
                                      <th class="time" style=" width:115px">发送时间</th>
<%--                                      <th class="time">创建时间</th>--%>
					                <th class="contro2" style=" width:286px; padding:0 10px">操作</th>
				                </tr>
			                </thead>
			                <tbody>
                    </HeaderTemplate>
                    <ItemTemplate> 
                         <tr>
                            <td class="name"><%#Eval("title") %></td>
                            <td class="info info1" style=" width:60px"><%#((KDWechat.Common.msg_type)int.Parse(Eval("msg_type").ToString())).ToString() %></td>
                            <td class="info info1" style=" width:60px"><%#Eval("send_num")%></td>
                            <td class="info info1" style=" width:60px"><%#Eval("is_check").ToString()=="1"?(Eval("is_send").ToString()=="1"?"发送成功":(Eval("is_timer").ToString()=="1"?"未发送":"发送失败")):"等待审核" %></td>
                            <td class="info info1" style=" width:50px"><%#((KDWechat.Common.is_timerMode)int.Parse(Eval("is_timer").ToString())).ToString() %></td>
    					    <td class="info info1" style=" width:115px"><%#Eval("send_time")??Eval("create_time") %></td>
<%--                            <td class="info info1"><%#Eval("create_time") %></td>--%>
					        <td class="contro2" >
                                <a href="sendGroupMessage.aspx?m_id=40&id=<%#Eval("id") %>" style='<%#Eval("is_send").ToString()=="1"?"display:none":""%>' class="btn btn6"><%#Eval("is_send").ToString()=="1"?"查看":"编辑" %></a>
                                <asp:LinkButton ID="LinkButton1" OnClientClick="return confirm('您确定要删除？')" CommandArgument='<%#Eval("id") %>' CommandName="del" runat="server" CssClass="btn btn6">删除</asp:LinkButton>
                                <a href="javascript:bombbox.openBox('groupmsg_send_confirm.aspx?id=<%#Eval("id") %>')" style='<%#Eval("is_check").ToString()=="1"?"display:none":""%>' class="btn btn6" >审核发送</a>
                                <asp:LinkButton ID="LinkButton2" CommandArgument='<%#Eval("id") %>' CommandName="sendCheck" runat="server" CssClass="btn btn6">重发验证码</asp:LinkButton>
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

    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/function.js"></script>

    <script src="../scripts/controls.js"></script>
    <script src="../scripts/Bombbox.js"></script>
            </form>
    <script>
        nav.change('<%=m_id%>'); 
    </script>
</body>
</html>
