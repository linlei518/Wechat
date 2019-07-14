<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sys_group_msg_list.aspx.cs" Inherits="KDWechat.Web.GroupMsg.sys_group_msg_list" %>

<%@ Register TagName="TopControl" Src="~/UserControl/TopControl.ascx" TagPrefix="uc" %>

<%@ Register TagName="MenuList" Src="~/UserControl/MenuList.ascx" TagPrefix="uc" %>
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
    <uc:TopControl ID="TopControl1" runat="server" />
    <uc:MenuList ID="MenuList1" runat="server" />
    <form id="form2" runat="server">
        <section id="main">
                    <%=NavigationName %>
	        <div class="titlePanel_01">
		        <div class="btns">
<%--			        <a href="SendGroupMessage.aspx?m_id=40" class="btn btn3">新建群发</a>--%>
		        </div>
		        <h1>群发信息列表</h1>
	        </div>
	
	
	        <div class="tablePanel_01">
                <asp:Repeater ID="DataRepeater" runat="server" >
                    
                    <HeaderTemplate>
                        <table cellpadding="0" cellspacing="0" class="table">
			                <thead>
				                <tr>
                                      <th class="name" style=" width:15%">微信号</th>
					                <th class="info info1">标题</th>
					                <th class="time" style=" width:115px">创建时间</th>
					                <th class="time" style=" width:115px">发送时间</th>
					                <th class="info info1" style=" width:60px">信息类型</th>
					                <th class="info info1" style=" width:60px">接收人数</th>
					                <th class="info info1" style=" width:50px">提交状态</th>
					                <th class="control" style=" width:70px">操作</th>
				                </tr>
			                </thead>
			                <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                         <tr>
                            <td class="name" style=" width:15%"><%#Eval("wx_name") %></td>
                            <td class="info info1"><%#Eval("title") %></td>
                            <td class="info info1" style=" width:115px"><%#Eval("create_time") %></td>
					       <td class="info info1"  style=" width:115px"><%#Eval("send_time")??"未定时" %></td>
					        <td class="info info1" style=" width:60px"><%#((KDWechat.Common.msg_type)int.Parse(Eval("msg_type").ToString())).ToString() %></td>
					        <td class="info info1"  style=" width:60px"><%#Eval("send_num") %></td>
					        <td class="info info1"  style=" width:50px"><%#((KDWechat.Common.is_sendMode)int.Parse(Eval("is_send").ToString())).ToString() %></td>
					        <td class="control"  style=" width:70px">
                                <a href='sys_group_msg_detail.aspx?m_id=<%=m_id %>&id=<%#Eval("id") %>' class="btn btn6" >查看</a>
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

    </form>
    <script src="../scripts/controls.js"></script>
    <script src="../scripts/Bombbox.js"></script>
    <script>
        nav.change('<%=m_id%>'); 
    </script>
</body>
</html>
