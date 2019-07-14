<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LBSList.aspx.cs" Inherits="KDWechat.Web.LBSList" %>

<%@ Register TagName="TopControl" Src="~/UserControl/TopControl.ascx" TagPrefix="uc" %>

<%@ Register TagName="MenuList" Src="~/UserControl/MenuList.ascx" TagPrefix="uc" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8"/>
    <title><%=pageTitle %></title>
    <script src="../scripts/html5.js"></script>
    <script src="../scripts/jquery-1.10.2.min.js"></script>

    <link type="text/css" href="../styles/global.css" rel="stylesheet"/>
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>
<body>
    <uc:TopControl ID="TopControl1" runat="server" />
    <uc:MenuList ID="MenuList1" runat="server" />
    <form id="form1" runat="server">
        <section id="main">
                    <%=NavigationName %>
	        <div class="titlePanel_01">
		        <div class="btns">
   			        <a href="javascript:bombbox.openBox('lbs_radius.aspx')" class="btn btn3">修改搜索半径</a>
			        <a href="LBS.aspx?m_id=70" class="btn btn3"><i class="add"></i>新建位置服务</a>
		        </div>
		        <h1>LBS信息列表</h1>
	        </div>
	
	
	        <div class="tablePanel_01">
                <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand" >
                    
                    <HeaderTemplate>
                        <table cellpadding="0" cellspacing="0" class="table">
			                <thead>
				                <tr>
					                <th class="name">名称</th>
					                <th class="info info1">地址</th>
                                      <th class="info info1">是否封面</th>
					                <th class="time">创建时间</th>
					                <th class="control">操作</th>
				                </tr>
			                </thead>
			                <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
					        <td class="name">
						        <a href='Lbs.aspx?m_id=70&id=<%#Eval("ID") %>'><%#Eval("Title") %></a>
					        </td>
					        <td class="info info1"><%#Eval("Address") %></td>
  					        <td class="info info1"><%#Eval("is_top").ToString()=="0"?"否":"是" %></td>
					        <td class="time"><%#Eval("CreateTime") %></td>
					        <td class="control">
                                <a href='Lbs.aspx?id=<%#Eval("ID") %>&m_id=70'  class="btn btn6">编辑</a>
						        <asp:Button ID="btnDelete" CssClass="btn btn6" CommandName="del" CommandArgument='<%#Eval("id") %>' OnClientClick="return confirm('你确定要删除这条记录？')" runat="server" Text="删除" />
					        </td>
				        </tr>   
                    </ItemTemplate>
                    <FooterTemplate>
                                <%# Repeater1.Items.Count == 0 ? "<tr><td style=\"text-align:center;\" colspan=\"8\">暂无数据</td></tr>" : ""%>
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
