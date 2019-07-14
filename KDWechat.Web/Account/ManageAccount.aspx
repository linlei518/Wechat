<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageAccount.aspx.cs" Inherits="KDWechat.Web.Account.ManageAccount" %>

<%@ Register TagName="TopControl" Src="~/UserControl/TopControl.ascx" TagPrefix="uc" %>

<%@ Register Src="../UserControl/MenuList.ascx" TagName="Sys_menulist" TagPrefix="uc2" %>


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
    <uc2:Sys_menulist ID="MenuList1" runat="server"   />
    <form id="form1" runat="server">
        <section id="main">
                    <%=NavigationName %>
	        <div class="titlePanel_01">
		        <div class="btns">
			        <%=isAdd?"<a href=\"NewSysAccount.aspx?m_id="+m_id+"\" class=\"btn btn3\">新建账号</a>":""%>
                    <asp:Button ID="Button1" runat="server" Visible='<%#isEdit %>' CssClass="btn btn5" Text="禁用选中" OnClientClick="return checkRad()" OnClick="Button1_Click" />
		        </div>
		        <h1>系统账号列表</h1>
	        </div>
	
	        <div class="filterPanel_01">
                <%=selectedString %>
		        <dl runat="server" id="dlBussinessType">
			        <dt>业务类型：</dt>
			        <dd>
				        <a href="ManageAccount.aspx?uf=<%=userFlag %>&m_id=<%=m_id %>&bt=<%=(int)KDWechat.Common.BussinessType.CLC %>&st=<%=status %>&at=<%=areaType %>" class="btn filterSelect">CLC</a>
				        <a href="ManageAccount.aspx?uf=<%=userFlag %>&m_id=<%=m_id %>&bt=<%=(int)KDWechat.Common.BussinessType.CMA %>&st=<%=status %>&at=<%=areaType %>" class="btn filterSelect">CMA</a>
				        <a href="ManageAccount.aspx?uf=<%=userFlag %>&m_id=<%=m_id %>&bt=<%=(int)KDWechat.Common.BussinessType.AScott %>&st=<%=status %>&at=<%=areaType %>" class="btn filterSelect">Ascott</a>
			        </dd>
		        </dl>
		        <dl id="dlUserFlag" runat="server">
			        <dt>账号类型：</dt>
			        <dd>
				        <a href="ManageAccount.aspx?uf=<%=(int)KDWechat.Common.UserFlag.总部账号 %>&m_id=<%=m_id %>&bt=<%=bussinessType %>&st=<%=status %>&at=<%=areaType %>" class="btn filterSelect">总部账号</a>
				        <a href="ManageAccount.aspx?uf=<%=(int)KDWechat.Common.UserFlag.地区账号 %>&m_id=<%=m_id %>&bt=<%=bussinessType %>&st=<%=status %>&at=<%=areaType %>" class="btn filterSelect">地区账号</a>
			        </dd>
		        </dl>
		        <dl id="dlAreaType" runat="server">
			        <dt>地区：</dt>
			        <dd>
				        <a href="ManageAccount.aspx?uf=<%=userFlag %>&bt=<%=bussinessType %>&st=<%=status %>&at=0&m_id=<%=m_id %>" class="btn filterSelect">华东</a>
				        <a href="ManageAccount.aspx?uf=<%=userFlag %>&bt=<%=bussinessType %>&st=<%=status %>&at=1&m_id=<%=m_id %>" class="btn filterSelect">华北</a>
				        <a href="ManageAccount.aspx?uf=<%=userFlag %>&bt=<%=bussinessType %>&st=<%=status %>&at=2&m_id=<%=m_id %>" class="btn filterSelect">华南</a>
				        <a href="ManageAccount.aspx?uf=<%=userFlag %>&bt=<%=bussinessType %>&st=<%=status %>&at=3&m_id=<%=m_id %>" class="btn filterSelect">西南</a>
				        <a href="ManageAccount.aspx?uf=<%=userFlag %>&bt=<%=bussinessType %>&st=<%=status %>&at=4&m_id=<%=m_id %>" class="btn filterSelect">凯德城镇开发</a>
			        </dd>
		        </dl>
		        <dl id="dlStatus" runat="server">
			        <dt>状态：</dt>
			        <dd>
				        <a href="ManageAccount.aspx?uf=<%=userFlag %>&bt=<%=bussinessType %>&st=<%=(int)KDWechat.Common.Status.正常 %>&at=<%=areaType %>&m_id=<%=m_id %>" class="btn filterSelect">正常</a>
				        <a href="ManageAccount.aspx?uf=<%=userFlag %>&bt=<%=bussinessType %>&st=<%=(int)KDWechat.Common.Status.禁用 %>&at=<%=areaType %>&m_id=<%=m_id %>" class="btn filterSelect">禁用</a>
			        </dd>
		        </dl>
	        </div>
	
	        <div class="tablePanel_01">
                <asp:Repeater ID="DataRepeater"  runat="server"  OnItemCommand="DataRepeater_ItemCommand" >
                    <HeaderTemplate>
                        <table cellpadding="0" cellspacing="0" class="table" >
			                <thead>
				                <tr>
					                <th class="check" style=" width:25px"></th>
					                <th class="name">用户名</th>
                                    <th class="info info1" style=" width:120px">姓名</th>
					                <th class="info info1" style=" width:80px">账号角色</th>
					                <th class="info info1" style=" width:10%" >类型</th>
					                <th class="info info1" style=" width:10%"  >地区</th>
					                <th class="info info1" style=" width:40px">状态</th>
					                <th class="info info1" style=" width:80px">负责微信号</th>
					                <th class="info info1" style=" width:80px">所属子帐号</th>
                                      <th class="info info1" style=" width:135px">最后操作时间</th>
					                <th class="control" style=" width:130px">操作</th>
				                </tr>
			                </thead>
			                <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                         <tr>
					        <td class="check" style=" width:25px">
					            <asp:CheckBox ID="chkId" class="checkbox" runat="server" Value='<%# Eval("id") %>' />
                                <asp:HiddenField ID="hidId" runat="server" Value='<%# Eval("id") %>' />
					        </td>
					        <td class="name">
						        <a href="NewSysAccount.aspx?id=<%#Eval("id") %>&m_id=<%=m_id %>"><%#Eval("user_name") %></a>
					        </td>
					        <td class="info info1" style=" width:120px"><%#Eval("real_name") %></td>
					        <td class="info info1" style=" width:80px"><%#((KDWechat.Common.UserFlag)int.Parse(Eval("flag").ToString())).ToString() %></td>
					        <td class="info info1" style=" width:10%" ><%#((KDWechat.Common.BussinessType)int.Parse(Eval("type_id").ToString())).ToString() %></td>
					        <td class="info info1" style=" width:10%" ><%#((KDWechat.Common.AreaType)int.Parse(Eval("area").ToString())).ToString() %></td>
					        <td class="info info1" style=" width:40px"><%#((KDWechat.Common.Status)int.Parse(Eval("status").ToString())).ToString() %></td>
					        <td class="info info1" style=" width:80px">
						        <a href='javascript:bombbox.openBox("WXList.aspx?uid=<%#Eval("id") %>");'><%# Eval("flag").ToString()=="1"?"无": (GetWxCountByUid(Eval("id").ToString())==null?"无":("查看("+GetWxCountByUid(Eval("id").ToString()))+")") %></a>
					        </td>
					        <td class="info info1" style=" width:80px">
						        <a href='javascript:bombbox.openBox("ChildrenAccount.aspx?id=<%#Eval("id") %>");'><%#Eval("flag").ToString()=="1"?"无": ( Eval("uscou")==null?"无":"查看("+Eval("uscou")+")") %></a>
					        </td>
                             <td class="info info1" style=" width:135px">
						        <%#GetLastOpreation(Eval("id")) %>
					        </td>
					        <td class="control"  style=" width:130px">
                                <%#isEdit?"<a href='NewSysAccount.aspx?id="+Eval("id")+"&m_id="+m_id+"' class=\"btn btn6\" >编辑</a>":""%>
                                <asp:Button runat="server" CssClass="btn btn6" Visible='<%#isEdit %>' CommandName="Disable" CommandArgument='<%#Eval("id") %>' Text='<%#Eval("status").ToString()=="0"?"启用":"禁用" %>' />
                                <%--	<a href="#" class="btn btn6">发信</a>--%>
					        </td>
				        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                                <%# DataRepeater.Items.Count == 0 ? "<tr><td style=\"text-align:center;\" colspan=\"10\">暂无数据</td></tr>" : ""%>
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
        function checkRad()
        {
            var list = $(".table [type='checkbox']");
            for (var i = 0; i < list.length; i++)
                if (list[i].checked) {
                    if(confirm("您确定要禁用这些账号？"))
                        return true;
                } 
           showTip.show("请选择需要禁用的条目", true);
            return false;
        }
        nav.change('<%=m_id%>'); 
    </script>

</body>
</html>
