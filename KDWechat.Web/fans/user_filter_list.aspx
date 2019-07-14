<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="user_filter_list.aspx.cs" Inherits="KDWechat.Web.fans.user_filter_list" %>

<%@ Register Src="../UserControl/TopControl.ascx" TagName="TopControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/MenuList.ascx" TagName="MenuList" TagPrefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8"/>
    <title><%=pageTitle %></title>
    <script src="../scripts/html5.js"></script>
    <script lang="javascript" type="text/javascript" src="../Scripts/DatePicker/WdatePicker.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet"/>
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>
<body>
    <uc1:TopControl ID="TopControl1" runat="server" />
    <uc2:MenuList ID="MenuList1" runat="server" />

    <form id="form1" runat="server">
        
    <section id="main">
        <%=NavigationName %>
	    <div class="titlePanel_01">
		    <div class="btns">
                 <a href="user_list.aspx?m_id=<%=m_id %>" class="btn btn5"><i class="black back"></i>返回</a>
		        <a href="javascript:bombbox.openBox('filter_export.aspx')" class="btn btn3">导出粉丝</a>
		    </div>
		    <h1>&nbsp;</h1>
	    </div>
        <div class="filterPanel_01">
		    <dl class="selectedList">
			    <dt>筛选条件：</dt>
			    <dd class="btns">
				    <a href="javascript:bombbox.openBox('fans_filter.aspx?s=1')" class="btn filterCancel">重新筛选</a>
			    </dd>
			    <dd>
                    <%=selectString %>
                    <asp:HiddenField ID="hfJson" runat="server" />
                    <asp:Button ID="btnSubbmit" style="width:1px;height:1px; background-color:transparent; border-color:transparent; color:transparent;" OnClick="btnSubbmit_Click" runat="server"/>			    
			    </dd>
		    </dl>
	    </div>
	    <div class="userListPanel_01">
		    <div class="list">
		        <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemCreated">
                    <HeaderTemplate>
                        <ul>
                    </HeaderTemplate>

                    <ItemTemplate>
                        <li class="userBaseInfo" id="user<%#Eval("id") %>">
					        <div class="check"><label><input type="checkbox" class="checkbox" name="listSelector" value="user<%#Eval("id") %>"></label></div>
					        <div class="img">
						        <span>
							        <img src="<%#Eval("headimgurl") %>" onerror="this.src='../images/logo_01.png'" alt="">
						        </span>
					        &nbsp;&nbsp;</div>
					        <div class="group">
						        <input type="hidden" class="hide">
						        <span>所在分组 ：</span>
                                 <em class="selected"><asp:Label ID="GroupLabel" runat="server"></asp:Label></em>
					        </div>
					        <div class="info">
						        <h2><em><%#Eval("nick_name") %></em><%#Eval("unionid")==null?"":"注册会员 ：" %> <%# KDWechat.BLL.Users.wx_fans.GetMemberName(Eval("unionid")==null?"":Eval("unionid").ToString())??"" %></h2>
						        <p>信息状态 ：<%#KDWechat.BLL.Users.wx_fans.GetFansChatStatus(Eval("last_interact_time"),Eval("reply_state")) %></p>
					        </div>
					        <div class="tags">
						        <input type="hidden" class="hide">
						        <span>用户标签 ：</span>
                                 <asp:Repeater ID="tag_repeater" runat="server">
                                     <ItemTemplate>
                                         <span class="tag"><%#Eval("title") %></span>
                                     </ItemTemplate>
                                 </asp:Repeater>
					        </div>
					        <div class="btns">
						        <span><%# Eval("reply_state").ToString()=="0"?"":"最后互动 ："+ Eval("last_interact_time") %></span>
						        <a href="user_msg_list.aspx?openId=<%#Eval("open_id") %>&m_id=<%=m_id %>" class="btn btn5">查看消息</a>
						        <a href="user_detail.aspx?id=<%#Eval("id") %>&m_id=<%=m_id %>" class="btn btn5">查看资料</a>
					        </div>
				        </li>
                    </ItemTemplate>

                    <FooterTemplate>
                        <%# Repeater1.Items.Count == 0 ? "<div style='background-color:white;border-top:10px solid #C7CED3'><div style=\"text-align:center;\" colspan=\"8\">暂无数据</div></div>" : ""%>
                        </ul>
                    </FooterTemplate>
                </asp:Repeater>	
                <div class="pageNum" id="div_page" runat="server">
			                  
		        </div>
		    </div>	
	    </div>

    </section>
        
        <script src="../scripts/jquery-1.10.2.min.js"></script>
        <script src="../scripts/jquery.ba-resize.min.js"></script>
        <script src="../scripts/controls.js"></script>
        <script src="../scripts/Bombbox.js"></script>
        <script src="../scripts/userList.js"></script>
        <script src="../Scripts/function.js"></script>

        <script>

            function CheckData(data)
            {
                $("#hfJson").val(data);
                $("#btnSubbmit").click();
            }


            nav.change('<%=m_id%>'); 

        </script>
    </form>
</body>
</html>
