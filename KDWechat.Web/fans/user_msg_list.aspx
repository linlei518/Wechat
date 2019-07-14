<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="user_msg_list.aspx.cs" EnableViewState="false" Inherits="KDWechat.Web.fans.user_msg_list" %>


<%@ Register Src="../UserControl/TopControl.ascx" TagName="TopControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/MenuList.ascx" TagName="MenuList" TagPrefix="uc2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="UTF-8">
    <title><%=pageTitle %></title>
    <script src="../scripts/html5.js" type="text/javascript"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet">
    <!--[if lt IE 9 ]><link href="styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    
</head>
<body>
    <form id="form1" runat="server">
        <uc1:TopControl ID="TopControl1" runat="server" />
        <uc2:MenuList ID="MenuList1" runat="server" />
        <section id="main">

	<div class="titlePanel_01">
         <div class="btns">
                    <a href="<%=hfReturnUrl.Value %>" class="btn btn5"><i class="black back"></i>返回</a>
                </div>
		<h1>用户消息记录</h1>
	</div>
	
	<div class="userMessageModule_01">
		
		<div class="userBaseInfo" id="user0">
			<div class="img">
				<span>
					<img src="images/pic_03.jpg" alt="" id="img_head" runat="server">
				</span>
			</div>
			<div class="group">
				<input type="hidden" class="hide">
				<span>所在分组 ：</span><em class="selected"><asp:Literal ID="lblGroupName" runat="server"></asp:Literal></em>
				<input type="button" class="btn btnSelect" title="修改分组" onClick="userTypeController.open(this)">
			</div>
			<div class="info">
				<h2><em><asp:Literal ID="lblNickName" runat="server"></asp:Literal></em> <asp:Literal ID="lblMemberName" runat="server"></asp:Literal></h2>
				<p>信息状态 ：<asp:Literal ID="lblReplyStatus" runat="server"></asp:Literal></p>
			</div>
			<div class="tags">
				<input type="hidden" class="hide">
				<span>用户标签 ：</span>
                <asp:Literal ID="lblFansTags" runat="server"></asp:Literal>
				<input type="button" class="btn btnAdd" title="修改标签" onClick="userTagController.open(this)">
			</div>
            	<div class="btns2">
				<asp:Literal ID="lblLastMsgTime" runat="server"></asp:Literal> 
				<!--<em>已超过回复时限</em>--><!--超时就显示这个-->
				
			</div>
			<div class="btns">
				<a href="<%=showDetail %>" class="btn btn5">查看资料</a>
			</div>
		</div>
		 <div class="userMessageListPanel_01">
			<div class="list">
				<ul>
                    <asp:Repeater ID="repList" runat="server">
                        <ItemTemplate>

                            <li class="userMessage <%# Eval("from_type").ToString()=="2"?"repeat":"" %>">
                                <%# GetInfoAndImg(Eval("open_id"),Eval("from_type"),Eval("create_time")) %>
                                <%# GetContents(Eval("msg_type"),Eval("contents"),Eval("media_id")) %>
					        </li>
                        </ItemTemplate>
                    </asp:Repeater>
					 
				</ul>
			</div>
		
			<div class="pageNum" id="div_page" runat="server">
			</div>
		</div>
	</div>
	  <asp:HiddenField ID="hfReturnUrl"  runat="server"></asp:HiddenField>
	
</section>
    </form>

    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/function.js"></script>
    <script src="../scripts/Bombbox.js"></script>
    <script src="../scripts/controls.js"></script>
        <script src="../Scripts/fans.js"></script>
    <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
    <script>
        nav.change('<%=m_id%>');//此处填写被激活的导航项的ID
        </script>
    <script src="../scripts/userList.js"></script>
    
   <script>
       userTagController.readTag(<%=tagList%>);//这个数组是所有的标签名字
       userTagController.tagSubmit = function (tags, oldTag, lengthOverFlag) {
           var addInfo = '';
           if (lengthOverFlag) addInfo = '部分用户标签超出10条，已经截取为10条';
           if (addInfo == "") {
               var tipString = '';
               for (var i in tags) {
                   var tempTags = "";
                   for (var j in tags[i].tags) {
                       tempTags += "{\"id\":\"" + tags[i].tags[j].id + "\",\"name\":\"" + tags[i].tags[j].name + "\"},";

                   }
                   tipString += tempTags;

               }
               upadateAttribute('<%=id%>', 2, tipString, '<%=lblNickName.Text%>', '');
            } else {
                alert(addInfo);
            }

     }

        userTypeController.readType(<%=groupList%>);//这个数组是所有的分组名字
       userTypeController.typeSubmit = function (ids, checked, value, oldTag) {
           upadateAttribute('<%=id%>', 1, value, '<%=lblNickName.Text%>', checked);
        }
</script>
     

</body>
</html>
