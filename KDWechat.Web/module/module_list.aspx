<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="module_list.aspx.cs" Inherits="KDWechat.Web.module.module_list" %>

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
		        <h1></h1>
	        </div>
            <div class="appListPanel_01">
<%--		        <div class="titleField">
			        <h1>应用模块</h1>
		        </div>--%>
		        <div class="listNTab">
			        <a href="module_list.aspx?m_id=<%=m_id %>&tag=0" class="btn nTabBtn <%=tag==0?"current":"" %>">全部应用</a>
			        <a href="module_list.aspx?m_id=<%=m_id %>&tag=1" class="btn nTabBtn <%=tag==1?"current":"" %>">资讯应用</a>
			        <a href="module_list.aspx?m_id=<%=m_id %>&tag=2" class="btn nTabBtn <%=tag==2?"current":"" %>">活动应用</a>
			        <a href="module_list.aspx?m_id=<%=m_id %>&tag=3" class="btn nTabBtn <%=tag==3?"current":"" %>">业务应用</a>
			        <a href="module_list.aspx?m_id=<%=m_id %>&tag=4" class="btn nTabBtn <%=tag==4?"current":"" %>">其他</a>
		        </div>
		        <div class="listField">
			        <ul>
                        <asp:Repeater ID="repAllSysModule" runat="server" OnItemCommand="repAllSysModule_ItemCommand">
                            <ItemTemplate>
                                <li class="<%#GetStatusByModuleID(Eval("id").ToString())==KDWechat.Common.Status.正常?"selected":"" %>">
					                <div class="img">
						                <span><img src="<%#Eval("img_url") %>" alt=""></span>
					                </div>
					                <div class="info">
						                <div class="title">
							                <h2><%#Eval("title") %><%#Eval("is_push").ToString()=="1"?"（可用于图文）":"（用于菜单及关键词）" %></h2>
						                </div>
						                <div class="text">
							                <p><%#KDWechat.Common.Utils.CutString(Eval("description"),60) %></p>
						                </div>
						                <div class="btns">
                                            <%# RoleStr(Eval("id")) %>
                                            <asp:Button class="btn btnCancel" OnClientClick='<%#GetConfirmByModuleID(Eval("id").ToString()) %>' Visible='<%# isEdit %>' CommandArgument='<%#Eval("id") %>' CommandName="Add" runat="server" Text='<%#GetStatusByModuleID(Eval("id").ToString())==KDWechat.Common.Status.正常?"已启用":"已禁用"%>' />
						                </div>
					                </div>
				                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                       
			        </ul> <%=repAllSysModule.Items.Count==0?"<div style='height:100px; width:300px; margin:0 auto'>暂无应用模块</div>":"" %>
		        </div>
                <div class="pageNum" id="div_page" runat="server">
			 
		        </div>
	        </div>
	
        </section>

        <script src="../scripts/Bombbox.js"></script> 
        <script src="../scripts/jquery-1.10.2.min.js"></script>
        <script src="../scripts/selectAddress.js"></script><!--三级联动选择地址的JS-->
        <script src="../scripts/controls.js"></script><!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
         <script>
             nav.change('<%=m_id%>'); 
             function showMsg(type, msg)
             {
                 bombbox.closeBox();
                 dialogue.closeAll();
                 if (type) {
                     showTip.show(msg);
                 } else {
                     showTip.show(msg,true);
                 }
             }
        </script>
    </form>
</body>
</html>
