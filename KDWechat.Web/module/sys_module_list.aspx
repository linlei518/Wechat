<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sys_module_list.aspx.cs" Inherits="KDWechat.Web.module.sys_module_list" %>

<%@ Register TagName="TopControl" Src="~/UserControl/TopControl.ascx" TagPrefix="uc" %>

<%@ Register Src="~/UserControl/MenuList.ascx" TagName="Sys_menulist" TagPrefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8"/>
    <title><%=pageTitle %></title>
    <script src="../scripts/html5.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet"/>
    <link type="text/css" href="../styles/style.css" rel="stylesheet"/>
    <!--[if lt IE 9 ]><link href="styles/ie8Fix.css" rel="stylesheet" type="text/css"/><![endif]-->
    <!--[if lt IE 8 ]><link href="styles/ie7Fix.css" rel="stylesheet" type="text/css"/><![endif]-->
    <!--[if lt IE 7 ]><link href="styles/ie6Fix.css" rel="stylesheet" type="text/css"/><![endif]-->
</head>
<body>
    <uc:TopControl runat="server" />
    <uc2:Sys_menulist ID="MenuList1" runat="server" />

    <form id="form1" runat="server">
        <section id="main">
                    <%=NavigationName %>
	        <div class="titlePanel_01">
		        <h1>应用管理</h1>
	        </div>
		    <div class="appListPanel_01">
		        <div class="titlePanel_01">
			        <h1>已启用的应用模块</h1>
		        </div>
		        <div class="listField">
			        <ul>
                        <asp:Repeater ID="repChooseList" runat="server" OnItemCommand="repChooseList_ItemCommand">
                            <ItemTemplate>
                                <li class="selected">
					                <div class="img">
						                <span><img src="<%#Eval("img_url") %>" alt=""></span>
					                </div>
					                <div class="info">
						                <div class="title">
							                <h2><%#Eval("title") %></h2>
						                </div>
						                <div class="text">
							                <p><%# KDWechat.Common.Utils.CutString(Eval("description"),60) %></p>
						                </div>
						                <div class="btns">
                                            <input class="btn btnCancel" value="管理" onclick="bombbox.openBox('module_power.aspx?id=<%#Eval("id") %>')" type="button">
                                            <asp:Button  class="btn btnCancel"  CommandArgument='<%#Eval("id") %>' CommandName="Remove" runat="server" Text="停用" />
						                </div>
					                </div>
				                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                        <%=repChooseList.Items.Count==0?"<dd style='height:100px; width:300px; margin:auto'>暂无已应用模块</dd>":"" %>
			        </ul>
		        </div>
	        </div>

            <div class="appListPanel_01">
		        <div class="titlePanel_01">
			        <h1>本地应用模块</h1>
		        </div>
		        <div class="listNTab">
			        <a href="sys_module_list.aspx?m_id=<%=m_id %>&tag=0" class="btn nTabBtn <%=tag==0?"current":"" %>">全部应用</a>
			        <a href="sys_module_list.aspx?m_id=<%=m_id %>&tag=1" class="btn nTabBtn <%=tag==1?"current":"" %>">资讯应用</a>
			        <a href="sys_module_list.aspx?m_id=<%=m_id %>&tag=2" class="btn nTabBtn <%=tag==2?"current":"" %>">活动应用</a>
			        <a href="sys_module_list.aspx?m_id=<%=m_id %>&tag=3" class="btn nTabBtn <%=tag==3?"current":"" %>">业务应用</a>
			        <a href="sys_module_list.aspx?m_id=<%=m_id %>&tag=4" class="btn nTabBtn <%=tag==4?"current":"" %>">其他</a>
		        </div>
		        <div class="listField">
			        <ul>
                        <asp:Repeater ID="repAllSysModule" runat="server" OnItemCommand="repAllSysModule_ItemCommand">
                            <ItemTemplate>
                                <li class="<%#Eval("status").ToString()=="0"?"":"selected" %>">
					                <div class="img">
						                <span><img src="<%#Eval("img_url") %>" alt=""></span>
					                </div>
					                <div class="info">
						                <div class="title">
							                <h2><%#Eval("title") %></h2>
						                </div>
						                <div class="text">
							                <p><%#KDWechat.Common.Utils.CutString(Eval("description"),60) %></p>
						                </div>
						                <div class="btns">
                                            <asp:Button class="btn btnCancel" CommandArgument='<%#Eval("id") %>' CommandName="Add"  runat="server" Text='<%#Eval("status").ToString()=="0"?"已禁用":"已启用" %>' />
						                </div>
					                </div>
				                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                        <%=repAllSysModule.Items.Count==0?"<dd style='height:100px; width:300px; margin:auto'>暂无已应用模块</dd>":"" %>
			        </ul>
		        </div>
                <div class="pageNum" id="div_page" runat="server">
			 
		        </div>
	        </div>

            <%--第三方暂时隐藏--%>
            <%--    	<div class="appListPanel_01">
		                    <div class="titleField">
			                    <h1>第三方应用模块</h1>
		                    </div>
		                    <div class="listField">
			                    <ul>
				                    <li>
					                    <div class="img">
						                    <span><img src="demo/demo_app_01.jpg" alt=""></span>
					                    </div>
					                    <div class="info">
						                    <div class="title">
							                    <h2>互动调查</h2>
						                    </div>
						                    <div class="text">
							                    <p>这里是一段该模块的简单描述，不宜过长，30-60个字就差不多了。这里是一段该模块的简单描述，不宜过长</p>
						                    </div>
						                    <div class="btns">
							                    <input type="button" class="btn btnCancel" value="启用">
						                    </div>
					                    </div>
				                    </li>
			                    </ul>
		                    </div>
	                    </div>
                    --%>

	
        </section>

        <script src="../scripts/jquery-1.10.2.min.js"></script>
        <script src="../scripts/controls.js"></script><!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
        <script src="../scripts/Bombbox.js"></script>
        <script>
            nav.change('<%=m_id%>'); 
        </script>
    </form>
</body>
</html>
