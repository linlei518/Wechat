<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="bottom.ascx.cs" Inherits="KDWechat.Web.wxpage.bottom" %>
    <script src="../scripts/share.js"></script>
<script>
    function showShareField() {
        var header = document.getElementById('header');
        if (header.className.indexOf('shareShow') == -1) {
            header.className += ' shareShow';
        }
    }
    function hideShareField() {
        var header = document.getElementById('header');
        if (header.className.indexOf('shareShow') != -1) {
            header.className = header.className.replace(/ shareShow/g, '').replace(/shareShow/g, '');
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
        content: "<%=ShareContent%>",
	url: window.location.href,
	imgUrl: host + '<%=top1.wx_head_pic%>',
	showControl: false,
	noWeixin: function () {
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