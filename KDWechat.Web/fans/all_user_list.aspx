<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="all_all_user_list.aspx.cs" Inherits="KDWechat.Web.fans.all_user_list" %>

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
		    <h1>用户列表</h1>
	    </div>
	
	    <div class="filterPanel_01">
            <%=selectedString %>
            <dl runat="server" id="dlWechat">
			    <dt>所属微信：</dt>
			    <dd>
                    <asp:Repeater ID="repWeChat" runat="server">
                        <ItemTemplate>
                            <a href="all_user_list.aspx?nn=<%=nn %>&sex=<%=sex %>&mt=<%=memberType %>&key=<%=keyword %>&m_id=53&wc=<%#Eval("id") %>&mc=<%=msgContain %>" class="btn filterSelect"><%#Eval("wx_pb_name") %></a>
                        </ItemTemplate>
                    </asp:Repeater>  
			    </dd>
		    </dl>
<%--            <dl runat="server" id="dlMemberType">
			    <dt>会员属性：</dt>
			    <dd>
				    <a href="all_user_list.aspx?nn=<%=nn %>&sex=<%=sex %>&key=<%=keyword %>&wc=<%=wxID %>&m_id=53&mc=<%=msgContain %>&mt=<%=(int)KDWechat.Common.MemberType.注册用户 %>" class="btn filterSelect">注册会员</a>
				    <a href="all_user_list.aspx?nn=<%=nn %>&sex=<%=sex %>&key=<%=keyword %>&wc=<%=wxID %>&m_id=53&mc=<%=msgContain %>&mt=<%=(int)KDWechat.Common.MemberType.非注册用户 %>" class="btn filterSelect">非注册会员</a>
			    </dd>
		    </dl>
		    <dl runat="server" id="dlMsgContain">
			    <dt>信息包含：</dt>
			    <dd>
				    <a href="all_user_list.aspx?nn=<%=nn %>&sex=<%=sex %>&mt=<%=memberType %>&wc=<%=wxID %>&key=<%=keyword %>&m_id=53&mc=<%=(int)KDWechat.Common.MsgContainType.手机 %>" class="btn filterSelect">手机</a>
				    <a href="all_user_list.aspx?nn=<%=nn %>&sex=<%=sex %>&mt=<%=memberType %>&wc=<%=wxID %>&key=<%=keyword %>&m_id=53&mc=<%=(int)KDWechat.Common.MsgContainType.姓名 %>" class="btn filterSelect">姓名</a>
				    <a href="all_user_list.aspx?nn=<%=nn %>&sex=<%=sex %>&mt=<%=memberType %>&wc=<%=wxID %>&key=<%=keyword %>&m_id=53&mc=<%=(int)KDWechat.Common.MsgContainType.身份证 %>" class="btn filterSelect">身份证</a>
			    </dd>
		    </dl>--%>
            <dl runat="server" id="dlSex">
			    <dt>性别：</dt>
			    <dd>
				    <a href="all_user_list.aspx?nn=<%=nn %>&mt=<%=memberType %>&wc=<%=wxID %>&key=<%=keyword %>&m_id=53&mc=<%=msgContain %>&sex=<%=(int)KDWechat.Common.WeChatSex.男 %>" class="btn filterSelect">男</a>
				    <a href="all_user_list.aspx?nn=<%=nn %>&mt=<%=memberType %>&wc=<%=wxID %>&key=<%=keyword %>&m_id=53&mc=<%=msgContain %>&sex=<%=(int)KDWechat.Common.WeChatSex.女 %>" class="btn filterSelect">女</a>
				    <a href="all_user_list.aspx?nn=<%=nn %>&mt=<%=memberType %>&wc=<%=wxID %>&key=<%=keyword %>&m_id=53&mc=<%=msgContain %>&sex=<%=(int)KDWechat.Common.WeChatSex.未知 %>" class="btn filterSelect">未知</a>
			    </dd>
		    </dl>
            <dl runat="server" id="dlFansName">
			    <dt>用户昵称：</dt>
			    <dd>
                	<div class="searchPanel_01" style=" padding:0; margin:0;">
                    <div class="searchField"  style=" float:none">
                        <asp:TextBox ID="txtUserName" CssClass="txt" Width="190" MaxLength="20" placeholder="用户昵称" runat="server"></asp:TextBox>
                        <asp:Button ID="btnSearch" runat="server" Text="" CssClass="btn searchBtn" ToolTip="点击搜索" OnClientClick="return searchUname()" ></asp:Button>
		            </div>
                    </div>

<%--                    <asp:TextBox ID="txtUserName" CssClass="txt" Width="180" MaxLength="20" placeholder="用户昵称" runat="server"></asp:TextBox>
                    <a href="javascript:searchUname()" style="margin-left:2px;" class="btn btn6">确定</a>--%>
			    </dd>
		    </dl>
		    <dl runat="server" id="dlHuDong">
			    <dt>关注时间：</dt>
			    <dd>
                    <input type="text" class="txt date" runat="server" id="txt_date_show" placeholder="按关注时间筛选" onfocus="selectStartDate();"/>
                    <%--这两个文本框是必须要的 ,一个是开始日期，一个是结束日期--%>
                    <asp:Button ID="Button1" class="btn btn6" runat="server" Text="确定"  style="margin-left:2px; width:1px; height:1px; border:none; color:transparent;background-color:transparent"  OnClick="Button1_Click" />
                    <asp:TextBox ID="txtbegin_date" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;"></asp:TextBox>
                    <asp:TextBox ID="txtend_date" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;" onfocus="selectEndDate();"></asp:TextBox>

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
                          <div class="check"></div>
					        <div class="img">
						        <span>
							        <img src="<%#Eval("headimgurl") %>" onerror="this.src='/images/logo_01.png'"  alt="">
						        </span>
					        &nbsp;&nbsp;</div>
					        <div class="group">
						        <input type="hidden" class="hide">
						        <span>所在分组 ：</span>
                                 <em class="selected"><%# Eval("group_id").ToString()=="0"?"默认分组": getGroupName(Eval("group_id").ToString()) %></em>
					        </div>
					        <div class="info">
						        <h2><em><%#Eval("nick_name") %></em><%#Eval("unionid")==null?"":"注册会员 ：" %> <%# KDWechat.BLL.Users.wx_fans.GetMemberName(Eval("unionid")==null?"":Eval("unionid").ToString())??"" %></h2>
						        <p>信息状态 ：<%#KDWechat.BLL.Users.wx_fans.GetFansChatStatus(Eval("open_id").ToString()) %></p>
                                 <p>所属微信号：<%#GetWxName(Eval("wx_id").ToString()) %></p>
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
						        <span><%# KDWechat.BLL.Users.wx_fans.GetFansChatStatus(Eval("open_id").ToString())==KDWechat.Common.FansChatStatus.暂无?"":"最后互动 ："+ KDWechat.BLL.Users.wx_fans.GetFansChatLastTime(Eval("open_id").ToString()) %></span>
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
            userTagController.readTag([<%=tagJson%>]);//这个数组是所有的标签名字

            userTypeController.readType([<%=groupJson%>]);//这个数组是所有的分组名字

            function searchUname() {
                location.href = "all_user_list.aspx?sex=<%=sex %>&mt=<%=memberType %>&key=<%=keyword %>&m_id=53&mc=<%=msgContain %>&nn=" + document.getElementById("txtUserName").value;
                return false;
            }

            
            function searchInter()
            {
                location.href =
                    'all_user_list.aspx?sex=<%=sex %>&key=' + document.getElementById("txt_date_show").value + '&m_id=53&mc=<%=msgContain%>&mt=<%=memberType %>&nn=<%=nn%>'
            }

            nav.change('<%=m_id%>'); 
            function selectStartDate() {
                var txtbegin_date = $dp.$('txtbegin_date');
                var txtend_date = $dp.$('txtend_date');
                var txt_date_show = $dp.$('txt_date_show');

                WdatePicker(
                {
                    position: { left: -198, top: 10 },
                    el: 'txtbegin_date',
                    onpicked: function (dp) {
                        txt_date_show.value = dp.cal.getNewDateStr();
                        txtend_date.value = "";
                        txtend_date.focus();
                    },
                    onclearing: function () {
                        txt_date_show.value = "";
                        txtend_date.value = "";
                        txtbegin_date.value = "";
                    },
                    doubleCalendar: true,
                    isShowClear: true,
                    readOnly: true,
                    dateFmt: 'yyyy-MM-dd',
                    maxDate: '%y-%M-%d'

                });
            }

            function selectEndDate() {
                var txt_date_show = $dp.$('txt_date_show');
                WdatePicker({
                    position: { left: -120, top: 10 },
                    doubleCalendar: true,
                    isShowClear: true,
                    readOnly: true,
                    dateFmt: 'yyyy-MM-dd',
                    minDate: '#F{$dp.$D(\'txtbegin_date\',{d:0});}',
                    maxDate: '%y-%M-%d',
                    onpicked: function (dp) {
                        if (txt_date_show.value.length > 0) {
                            txt_date_show.value += " — " + dp.cal.getNewDateStr();
                            $("#Button1").click();
                        } else {
                            txt_date_show.value = dp.cal.getNewDateStr();
                        }

                    }
                });
            }
        </script>
    </form>
</body>
</html>

