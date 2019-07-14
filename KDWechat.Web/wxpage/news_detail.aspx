<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="news_detail.aspx.cs" Inherits="KDWechat.Web.wxpage.news_detail" %>

<%@ Register Src="top.ascx" TagName="top" TagPrefix="uc1" %>
<%@ Register Src="bottom.ascx" TagName="bottom" TagPrefix="uc2" %>
<!doctype html>
<html>
<head runat="server" >
    <meta charset="UTF-8">
    <title>图文标题</title>
    <meta name="viewport" id="viewport" content="width=device-width, user-scalable=no,initial-scale=1,maximum-scale=1">
    <meta name="screen-orientation" content="portrait/landscape">
    <meta content="yes" name="apple-mobile-web-app-capable">
    <meta content="black" name="apple-mobile-web-app-status-bar-style">
    <meta content="telephone=no" name="format-detection">
    <link href="css/global.css" rel="stylesheet" type="text/css">
</head>
<body>

    <uc1:top ID="top1" runat="server" />
    <section id="main">
	<div class="articlePanel_01">
		<div class="titleField">
			<h1><asp:Literal ID="litTitle" runat="server"></asp:Literal></h1>
			<h2><asp:Literal ID="litTime" runat="server"></asp:Literal></h2>
		</div>
		<div class="textField">
			 <asp:Literal ID="litContent" runat="server"></asp:Literal>
		</div>
	</div>
</section>
    <script src="js/share.js"></script>
<script>
function showShareField(){
	var header = document.getElementById('header');
	if(header.className.indexOf('shareShow')==-1){
		header.className += ' shareShow';
	}
}
function hideShareField(){
	var header = document.getElementById('header');
	if(header.className.indexOf('shareShow')!=-1){
		header.className = header.className.replace(/ shareShow/g,'').replace(/shareShow/g,'');
	}
}



//下面是分享
var pathItems = window.location.pathname.split('/');
var pathName = '';
for (var i = 0; i < pathItems.length; i++) {
	if (pathItems[i].indexOf('.html') == -1 && pathItems[i].indexOf('.aspx') == -1) {
		pathName += pathItems[i] + '/';
	}
}

var host = 'http://' + window.location.host + pathName;
var weixinShare = new WeixinShare({
	title: document.title,
	content: '<%=ShareContent%>',
	url: window.location.href,
	imgUrl: host + '<%=top1.wx_head_pic%>',
	showControl: false,
	noWeixin: function() {
		document.querySelector('.shareToFriend').style.display = 'none';
		document.querySelector('.shareToFriendZone').style.display = 'none';
	}/*,
	okCallback: function(success){
		if(success){
			alert('分享成功');
		}else{
			alert('分享失败');
		}
	}*/
});

</script>

</body>
</html>
