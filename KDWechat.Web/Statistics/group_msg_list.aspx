<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="group_msg_list.aspx.cs" Inherits="KDWechat.Web.Statistics.group_msg_list" %>


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
                    
                    <a href="<%=(string.IsNullOrWhiteSpace(sub_sdate)?"dashboard.aspx?IDs="+Request["Ids"]+"&m_id=":"group_message_bydate.aspx?IDs="+Request["Ids"]+"&m_id=")+m_id.ToString() %>" class="btn btn5"><i class="black back"></i>返回</a>
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
                                      <th class="info info1" style=" width:60px">单图文条数</th>
                                      <th class="info info1" style=" width:60px">接收人数</th>
					                <th class="info info1" style=" width:60px">发送状态</th>
                                      <th class="info info1" style=" width:50px">是否定时</th>
                                      <th class="time" style=" width:115px">发送时间</th>
					                <th class="contro2" style=" width:76px; padding:0 10px">操作</th>
				                </tr>
			                </thead>
			                <tbody>
                    </HeaderTemplate>
                    <ItemTemplate> 
                         <tr>
                            <td class="name"><%#Eval("title") %></td>
                            <td class="info info1" style=" width:60px"><%#((KDWechat.Common.msg_type)int.Parse(Eval("msg_type").ToString())).ToString() %></td>
                            <td class="info info1" style=" width:60px"><%#GetMsgCount(Eval("source_id")) %></td>
                            <td class="info info1" style=" width:60px"><%#Eval("send_num") %></td>
                            <td class="info info1" style=" width:60px"><%#Eval("is_check").ToString()=="1"?(Eval("is_send").ToString()=="1"?"发送成功":(Eval("is_timer").ToString()=="1"?"未发送":"发送失败")):"等待审核" %></td>
                            <td class="info info1" style=" width:50px"><%#((KDWechat.Common.is_timerMode)int.Parse(Eval("is_timer").ToString())).ToString() %></td>
    					    <td class="info info1" style=" width:115px"><%#Eval("send_time")??Eval("create_time") %></td>
					        <td class="contro2" >
                                <a href="groupmsg_Detail.aspx?m_id=<%#m_id %>&msgID=<%#Eval("id") %>&wx_id=<%#wx_id %>" class="btn btn6">查看统计</a>
                            </td>
				        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                                <%# DataRepeater.Items.Count == 0 ? "<tr><td style=\"text-align:center;\" colspan=\"7\">暂无数据</td></tr>" : ""%>
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
