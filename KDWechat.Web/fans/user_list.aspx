<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="user_list.aspx.cs" Inherits="KDWechat.Web.fans.user_list" %>
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
    <style type="text/css">
    	li.userBaseInfo .btns {top: 46px;}
		li.userBaseInfo .info, li.userBaseInfo .tags {margin-right: 390px;}
    </style>
</head>
<body>
    <uc1:TopControl ID="TopControl1" runat="server" />
    <uc2:MenuList ID="MenuList1" runat="server" />

    <form id="form1" runat="server">
        
    <section id="main">
        <%=NavigationName %>
	    <div class="titlePanel_01">
		    <div class="btns">
                 <%--<a href="javascript:bombbox.openBox('fans_filter.aspx');" class="btn btn5">高级筛选</a>--%>
			    <%--  <a href="javascript:bombbox.openBox('tags_edit.aspx');" class="btn btn3"><i class="add"></i>新建标签</a>
                 <a href="javascript:DownLoadUsers();" class="btn btn5">导出数据</a>--%>
                 <%=isEdit?"<a href=\"javascript:GetAllUser()\" class=\"btn btn3\">导入所有粉丝</a>":""%>
                <%--<a href="javascript:void(0)" class="btn btn5" onclick="copyString.copy('<%=base.wchatConfig.domain+"/fans/member_bind.aspx?wx_id="+KDWechat.Common.DESEncrypt.Encrypt(wx_id) %>')">复制会员绑定链接</a>--%>
		    </div>
		    <h1>&nbsp;</h1>
	    </div>
	    <div class="filterPanel_01">
            
            <%=selectedString %>
            
		    <dl runat="server" id="dlGroup">
			    <dt>分组：</dt>
			    <dd>
                    <asp:Repeater ID="repGroup" runat="server">
                        <ItemTemplate>
                            <a href="user_list.aspx?ff=<%=fansFrom %>&ptag=<%=publicTag %>&state=<%=state%>&sex=<%=sex %>&nn=<%=nn %>&mt=<%=memberType %>&key=<%=keyword %>&m_id=18&gp=<%#Eval("id") %>&tag=<%=tagID %>&mc=<%=msgContain %>" class="btn filterSelect"><%#Eval("title") %></a>
                        </ItemTemplate>
                    </asp:Repeater>  
			    </dd>
		    </dl>
            <dl runat="server" id="dlPTag" visible="false">
                <dt>公用标签：</dt>
			    <dd>
                    <asp:Repeater ID="repPublicTag" runat="server">
                        <ItemTemplate>
                            <a href="user_list.aspx?ff=<%=fansFrom %>&ptag=<%#Eval("id") %>&state=<%=state%>&sex=<%=sex %>&nn=<%=nn %>&mt=<%=memberType %>&key=<%=keyword %>&m_id=18&gp=<%=gpID %>&tag=<%=tagID%>&mc=<%=msgContain %>" class="btn filterSelect"><%#Eval("title") %></a>
                        </ItemTemplate>
                    </asp:Repeater>
			    </dd>
            </dl>
		    <dl runat="server" id="dlTag">
			    <dt>标签：</dt>
			    <dd>
                    <asp:Repeater ID="repTag" runat="server">
                        <ItemTemplate>
                            <a href="user_list.aspx?ff=<%=fansFrom %>&ptag=<%=publicTag %>&state=<%=state%>&sex=<%=sex %>&nn=<%=nn %>&mt=<%=memberType %>&key=<%=keyword %>&m_id=18&gp=<%=gpID %>&tag=<%#Eval("id") %>&mc=<%=msgContain %>" class="btn filterSelect"><%#Eval("title") %></a>
                        </ItemTemplate>
                    </asp:Repeater>

			    </dd>
		    </dl>
            <dl runat="server" id="dlMemberType" visible="false">
			    <dt>会员属性：</dt>
			    <dd>
				    <a href="user_list.aspx?ff=<%=fansFrom %>&ptag=<%=publicTag %>&state=<%=state%>&sex=<%=sex %>&nn=<%=nn %>&key=<%=keyword %>&m_id=18&tag=<%=tagID %>&gp=<%=gpID %>&mc=<%=msgContain %>&mt=<%=(int)KDWechat.Common.MemberType.注册用户 %>" class="btn filterSelect">注册会员</a>
				    <a href="user_list.aspx?ff=<%=fansFrom %>&ptag=<%=publicTag %>&state=<%=state%>&sex=<%=sex %>&nn=<%=nn %>&key=<%=keyword %>&m_id=18&tag=<%=tagID %>&gp=<%=gpID %>&mc=<%=msgContain %>&mt=<%=(int)KDWechat.Common.MemberType.非注册用户 %>" class="btn filterSelect">非注册会员</a>
			    </dd>
		    </dl>

		    <dl runat="server" id="dlState">
			    <dt>回复状态：</dt>
			    <dd>
				    <a href="user_list.aspx?ff=<%=fansFrom %>&ptag=<%=publicTag %>&nn=<%=nn %>&mt=<%=memberType %>&key=<%=keyword %>&m_id=18&gp=<%=gpID %>&tag=<%=tagID %>&mc=<%=msgContain%>&sex=<%=sex %>&state=<%=(int)KDWechat.Common.FansChatsTypeNew.已回复 %>" class="btn filterSelect">已回复</a>
				    <a href="user_list.aspx?ff=<%=fansFrom %>&ptag=<%=publicTag %>&nn=<%=nn %>&mt=<%=memberType %>&key=<%=keyword %>&m_id=18&gp=<%=gpID %>&tag=<%=tagID %>&mc=<%=msgContain%>&sex=<%=sex %>&state=<%=(int)KDWechat.Common.FansChatsTypeNew.未回复 %>" class="btn filterSelect">未回复</a>
				    <a href="user_list.aspx?ff=<%=fansFrom %>&ptag=<%=publicTag %>&nn=<%=nn %>&mt=<%=memberType %>&key=<%=keyword %>&m_id=18&gp=<%=gpID %>&tag=<%=tagID %>&mc=<%=msgContain%>&sex=<%=sex %>&state=<%=(int)KDWechat.Common.FansChatsTypeNew.已过期 %>" class="btn filterSelect">已过期</a>
                     <a href="user_list.aspx?ff=<%=fansFrom %>&ptag=<%=publicTag %>&nn=<%=nn %>&mt=<%=memberType %>&key=<%=keyword %>&m_id=18&gp=<%=gpID %>&tag=<%=tagID %>&mc=<%=msgContain%>&sex=<%=sex %>&state=<%=(int)KDWechat.Common.FansChatsTypeNew.未回复已过期 %>" class="btn filterSelect">未回复已过期</a>
                     <a href="user_list.aspx?ff=<%=fansFrom %>&ptag=<%=publicTag %>&nn=<%=nn %>&mt=<%=memberType %>&key=<%=keyword %>&m_id=18&gp=<%=gpID %>&tag=<%=tagID %>&mc=<%=msgContain%>&sex=<%=sex %>&state=<%=(int)KDWechat.Common.FansChatsTypeNew.暂无 %>" class="btn filterSelect">暂无</a>
			    </dd>
		    </dl>
            <dl runat="server" id="dlSex">
			    <dt>性别：</dt>
			    <dd>
				    <a href="user_list.aspx?ff=<%=fansFrom %>&ptag=<%=publicTag %>&state=<%=state%>&nn=<%=nn %>&mt=<%=memberType %>&key=<%=keyword %>&m_id=18&gp=<%=gpID %>&tag=<%=tagID %>&mc=<%=msgContain%>&sex=<%=(int)KDWechat.Common.WeChatSex.男 %>" class="btn filterSelect">男</a>
				    <a href="user_list.aspx?ff=<%=fansFrom %>&ptag=<%=publicTag %>&state=<%=state%>&nn=<%=nn %>&mt=<%=memberType %>&key=<%=keyword %>&m_id=18&gp=<%=gpID %>&tag=<%=tagID %>&mc=<%=msgContain%>&sex=<%=(int)KDWechat.Common.WeChatSex.女 %>" class="btn filterSelect">女</a>
				    <a href="user_list.aspx?ff=<%=fansFrom %>&ptag=<%=publicTag %>&state=<%=state%>&nn=<%=nn %>&mt=<%=memberType %>&key=<%=keyword %>&m_id=18&gp=<%=gpID %>&tag=<%=tagID %>&mc=<%=msgContain %>&sex=<%=(int)KDWechat.Common.WeChatSex.未知 %>" class="btn filterSelect">未知</a>
			    </dd>
		    </dl>
		    <dl runat="server" id="dlMsgContain" visible="false">
			    <dt>信息包含：</dt>
			    <dd>
				    <a href="user_list.aspx?ff=<%=fansFrom %>&ptag=<%=publicTag %>&state=<%=state%>&sex=<%=sex %>&nn=<%=nn %>&mt=<%=memberType %>&key=<%=keyword %>&m_id=18&gp=<%=gpID %>&tag=<%=tagID %>&mc=<%=(int)KDWechat.Common.MsgContainType.手机 %>" class="btn filterSelect">手机</a>
				    <a href="user_list.aspx?ff=<%=fansFrom %>&ptag=<%=publicTag %>&state=<%=state%>&sex=<%=sex %>&nn=<%=nn %>&mt=<%=memberType %>&key=<%=keyword %>&m_id=18&gp=<%=gpID %>&tag=<%=tagID %>&mc=<%=(int)KDWechat.Common.MsgContainType.姓名 %>" class="btn filterSelect">姓名</a>
				    <a href="user_list.aspx?ff=<%=fansFrom %>&ptag=<%=publicTag %>&state=<%=state%>&sex=<%=sex %>&nn=<%=nn %>&mt=<%=memberType %>&key=<%=keyword %>&m_id=18&gp=<%=gpID %>&tag=<%=tagID %>&mc=<%=(int)KDWechat.Common.MsgContainType.身份证 %>" class="btn filterSelect">身份证</a>
			    </dd>
		    </dl>
            <dl runat="server" id="dlFansFrom">
			    <dt>用户来源：</dt>
			    <dd>
                    <a href="user_list.aspx?ff=0&ptag=<%=publicTag %>&state=<%=state%>&sex=<%=sex %>&nn=<%=nn %>&mt=<%=memberType %>&key=<%=keyword %>&m_id=18&gp=<%=gpID %>&tag=<%=tagID %>&mc=<%=msgContain %>" class="btn filterSelect">搜索关注</a>
                    <asp:Repeater ID="repFansFrom" runat="server">
                        <ItemTemplate>
				            <a href="user_list.aspx?ff=<%#Eval("souce_id") %>&ptag=<%=publicTag %>&state=<%=state%>&sex=<%=sex %>&nn=<%=nn %>&mt=<%=memberType %>&key=<%=keyword %>&m_id=18&gp=<%=gpID %>&tag=<%=tagID %>&mc=<%=msgContain %>" class="btn filterSelect"><%#Eval("q_name") %></a>
                        </ItemTemplate>
                    </asp:Repeater>
			    </dd>
		    </dl>
            <dl runat="server" id="dlFansName">
			    <dt>用户昵称：</dt>
			    <dd>
                   <div class="searchPanel_01" style=" padding:0; margin:0;">
                   		 <div class="searchField" style=" float:none">
                        <asp:TextBox ID="txtUserName" CssClass="txt" Width="190" MaxLength="200" placeholder="多个用户名请使用“|”分割。" runat="server"></asp:TextBox>
                        <asp:Button ID="btnSearch" runat="server" Text="" CssClass="btn searchBtn" ToolTip="点击搜索" OnClientClick="return searchUname()" ></asp:Button>
                      
                        </div>
		            </div>
			    </dd>
		    </dl>
		    <dl runat="server" id="dlHuDong">
			    <dt>关注时间：</dt>
			    <dd>
                    <input type="text" class="txt date" runat="server" id="txt_date_show" placeholder="按关注时间筛选" onfocus="selectStartDate();"/>
                    <%--这两个文本框是必须要的 ,一个是开始日期，一个是结束日期--%>
                    <asp:TextBox ID="txtbegin_date" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;"></asp:TextBox>
                    <asp:TextBox ID="txtend_date" runat="server" Style="width: 1px; border: none; background-color:transparent ; color: transparent;" onfocus="selectEndDate();"></asp:TextBox>

			    </dd>
		    </dl>
       		<div class="btns2">
    			<input type="button" class="btn btn6" value="展开">
	    	</div>
	    </div>
	
	    <div class="userListPanel_01">
		
		    <div class="control" style="margin-top:10px;">
			<label><input type="checkbox" class="checkbox" name="listSelectAll" onChange="checkAll.check(this,'listSelector')"/>全选</label>
			<input type="button" class="btn btn6" value="批量修改标签" onClick="listOpen(this, userTagController, 0)"/>
			<input type="button" class="btn btn6" value="批量增加标签" onClick="listOpen(this, userTagController, 1)"/>
			<input type="button" class="btn btn6" value="批量删除标签" onClick="listOpen(this, userTagController, 2)"/>
			<input type="button" class="btn btn6" value="批量分组" onClick="listOpen(this, userTypeController)"/>
		    </div>

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
						        <input type="button" class="btn btnSelect" title="修改分组" onclick="userTypeController.open(this)">
					        </div>
					        <div class="info">
						        <h2><em><%#Eval("nick_name") %><%# wx_type%2==1?"用户ID："+Eval("id"):""%></em><%#(Eval("unionid")==null||Eval("unionid").ToString()=="")?"":"注册会员 ：" %> <%# KDWechat.BLL.Users.wx_fans.GetMemberName(Eval("unionid")==null?"":Eval("unionid").ToString())??"" %></h2>
						        <p>信息状态 ：<%#KDWechat.BLL.Users.wx_fans.GetFansChatStatus(Eval("last_interact_time"),Eval("reply_state")) %></p>
					        </div>
					        <div class="tags">
						        <input type="hidden" class="hide">
						        <span>用户标签 ：</span>
                                 <asp:Repeater ID="tag_repeater" runat="server">
                                     <ItemTemplate>
                                         <span class="tag" name="<%#Eval("tagID") %>"><%#Eval("title") %></span>
                                     </ItemTemplate>
                                 </asp:Repeater>
						        <input type="button" class="btn btnAdd" runat="server" id="input_add_Tag" title="添加标签" onclick="userTagController.open(this)"/>
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
            var filterShowNum = 4; //默认显示几条筛选
            var filterHidden = true;

            function filterHide() {
                $('.filterPanel_01 dl').each(function (i) {
                    if (i > filterShowNum) {
                        $(this).hide();
                    } else if (this.className.indexOf('selected') == -1) {
                        $(this).show();
                    }
                })
            }
            filterHide();

            function filterShow() {
                $('.filterPanel_01 dl').not('.selected').show();
            }

            $('.filterPanel_01 .btns2 .btn').click(function () {
                if (filterHidden) {
                    $(this).val('收起')
                    filterHidden = false;
                    filterShow();
                } else {
                    $(this).val('展开');
                    filterHidden = true;
                    filterHide();
                }
            });


            userTagController.readTag([<%=tagJson%>]);//这个数组是所有的标签名字
            userTagController.tagSubmit = function (tags, oldTag, lengthOverFlag) {
                var addInfo = '';
                if (lengthOverFlag) addInfo = '部分用户标签超出10条，已经截取为10条';

                var tipString = [];
                for (var i in tags) {
                    var tempTags = [];
                    for (var j in tags[i].tags) {
                        tempTags.push(tags[i].tags[j].id);
                    }
                    tipString.push('{"id":' + tags[i].id + ',"data":[' + tempTags.join(',') + ']}');
                }

                $.ajax({
                    type: "POST",
                    url: "user_list.aspx?a=2",
                    data: "data=["+tipString+"]",
                    success: function (msg) {
                        var lis = msg.split("|");
                        if (lis[0] == "0") {
                            userTagController.showResult(oldTag);
                            //userTagController.backResult(ids, oldTag);
                            showTip.show(lis[1], true);
                        }
                        else {
                            showTip.show(lis[1], false);
                        }
                    }
                });

            }

            userTypeController.readType([<%=groupJson%>]);//这个数组是所有的分组名字
            userTypeController.typeSubmit = function (ids, checked, value, oldTag) {
                var idString = ids.join(',');
                $.ajax({
                    type: "POST",
                    url: "user_list.aspx?b=2",
                    data: "uids=" + idString + "&groupID=" + value,
                    success: function (msg) {
                        var lis = msg.split("|");
                        if (lis[0] == "0") {
                            userTypeController.backResult(ids, oldTag);
                        }
                        showTip.show(lis[1], false);
                    }
                });
            }

            function CheckData(data) {
                $.ajax({
                    type: "POST",
                    url: "user_list.aspx?as=2",
                    data: "data="+data,
                    success: function (msg) {
                        var lis = msg.split("|");
                        if (lis[0] == "0") {
                            location.href = lis[1];
                        }
                    }
                });
                //$("#hfJson").val(data);
                //$("#btnSubbmit").click();
            }


            function searchUname()
            { 
                location.href = "user_list.aspx?ff=<%=fansFrom %>&ptag=<%=publicTag %>&state=<%=state%>&sex=<%=sex %>&mt=<%=memberType %>&key=<%=keyword %>&m_id=18&gp=<%=gpID %>&tag=<%=tagID %>&mc=<%=msgContain %>&nn=" + document.getElementById("txtUserName").value;
                return false;
            }

            function GetAllUser()
            {
                if (confirm("该操作将与微信后台进行粉丝同步，可能会消耗较长时间，您确定要执行此操作？")) {
                    //document.getElementById('loading_div').style.display = '';
                    dialogue.dlLoading();
                    $.ajax({
                        type: "POST",
                        timeout: 600000,
                        url: "user_list.aspx?sss=12345",
                        data: "",
                        success: function (msg) {
                            lis = msg.split("|");
                            if (lis[0] == "1") {
                                alert("关注总人数为：" + lis[1] + "人,本次同步了：" + lis[2] + "人");
                                location.reload();
                            }
                            else {
                                alert("由于微信接口频率限制，本次导入尚未完全完成，请再次点击导入。");
                                location.reload();
                            }
                        },
                        complete : function(XMLHttpRequest,status){ 
                            if (status == 'timeout') {
                                ajaxTimeoutTest.abort();
                                alert("由于微信接口频率限制，本次导入尚未完全完成，请再次点击导入。");
                                location.reload();
                            }
                            else if (status == 'error')
                            {
                                alert("由于微信接口频率限制，本次导入尚未完全完成，请再次点击导入。");
                                location.reload();
                            }
                        },
                    });
                }
            }

            function DownLoadUsers()
            {
                if (confirm("该操作将导出当前筛选条件下的所有用户，可能会消耗较长时间，您确定要执行此操作？")) {
                    //document.getElementById('loading_div').style.display = '';
                    dialogue.dlLoading();
                    if (location.href.indexOf("?") == -1)
                        location.href = location.href + "?ad=1";
                    else
                        location.href = location.href + "&ad=1";
                    dialogue.closeAll();
                }
            }


            function searchInter()
            {
                location.href =
                    'user_list.aspx?ff=<%=fansFrom %>&ptag=<%=publicTag %>&state=<%=state%>&sex=<%=sex %>&key=' + document.getElementById("txt_date_show").value + '&nn=<%=nn %>&m_id=18&gp=<%=gpID %>&tag=<%=tagID %>&mc=<%=msgContain%>&mt=<%=memberType %>'
                return false;
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
                            location.href =
                    'user_list.aspx?ff=<%=fansFrom %>&ptag=<%=publicTag %>&state=<%=state%>&sex=<%=sex %>&key=' + document.getElementById("txt_date_show").value + '&nn=<%=nn %>&m_id=18&gp=<%=gpID %>&tag=<%=tagID %>&mc=<%=msgContain%>&mt=<%=memberType %>'

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
