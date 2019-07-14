<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JsTest.aspx.cs" Inherits="KDWechat.Web.JsTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js" type="text/javascript"></script>
    <title></title>
</head>
<body>
<form id="form1" runat="server">
<div>
<input type="button" onclick="record()" value="录音" />
<input type="button" onclick="stopRecord()" value="停止录音" />
<input type="button" onclick="playAudio()" value="播放录音" />
<input type="button" onclick="translateVoice()" value="翻译录音" />
<input type="button" onclick="map()" value="获取地理位置" />
<input type="button" onclick="scan()" value="扫一扫" />

<img src="https://ss0.baidu.com/94o3dSag_xI4khGko9WTAnF6hhy/super/whfpf%3D425%2C260%2C50/sign=c092209b29f5e0feee4dda413a5d009a/eaf81a4c510fd9f9e7a77aeb212dd42a2934a4fb.jpg" alt="" onClick="previewImg(this)">
<img src="https://ss2.baidu.com/-vo3dSag_xI4khGko9WTAnF6hhy/super/whfpf%3D425%2C260%2C50/sign=4deacbf1a94bd1130498e4723c92903d/48540923dd54564e1727cbbbb7de9c82d0584ff5.jpg" alt="" onClick="previewImg(this)">
<img src="https://ss1.baidu.com/-4o3dSag_xI4khGko9WTAnF6hhy/super/whfpf%3D425%2C260%2C50/sign=ada8a683586034a829b7ebc1ad2e7d66/d62a6059252dd42a1cb92e29073b5bb5c8eab8fb.jpg" alt="" onClick="previewImg(this)">
<img src="https://ss0.baidu.com/94o3dSag_xI4khGko9WTAnF6hhy/super/whfpf%3D425%2C260%2C50/sign=c092209b29f5e0feee4dda413a5d009a/eaf81a4c510fd9f9e7a77aeb212dd42a2934a4fb.jpg" alt="" onClick="previewImg(this)">

<script>
<%=wechatConfig%>
var shareTitle = "璐璐璐璐璐";
var link = "http://www.baidu.com";
var imgUrl = "http://www.baidu.com/img/bdlogo.png";
var shareContent = "这个是内容啊啊露露露露露露";
var localId ="";
function record()
{
	wx.startRecord();
}

function stopRecord()
{
	wx.stopRecord({
		success: function (res) {
			localId = res.localId;
		}
	});
}

function playAudio()
{
	wx.playVoice({
		localId: localId // 需要播放的音频的本地ID，由stopRecord接口获得
	});
}

function translateVoice() {
	wx.translateVoice({
		localId: localId, // 需要识别的音频的本地Id，由录音相关接口获得
		isShowProgressTips: 1, // 默认为1，显示进度提示
		success: function (res) {
			alert(res.translateResult); // 语音识别的结果
		}
	});
}
function previewImg(img){
	var url = img.src;
	var totalImgs = Array.prototype.slice.call(document.querySelectorAll('img'));
	var totalUrls = [];
	for(var i in totalImgs){
		totalUrls.push(totalImgs[i].src);
	}
	wx.previewImage({
		current: url, // 当前显示的图片链接
		urls: totalUrls // 需要预览的图片链接列表
	});
}

function map(){
	wx.getLocation({
		success: function (res) {
			var latitude = res.latitude; // 纬度，浮点数，范围为90 ~ -90
			var longitude = res.longitude; // 经度，浮点数，范围为180 ~ -180。
			wx.openLocation({
				latitude: latitude, // 纬度，浮点数，范围为90 ~ -90
				longitude: longitude, // 经度，浮点数，范围为180 ~ -180。
				name: '测试地址', // 位置名
				address: '测试地址说明', // 地址详情说明
				scale: 14, // 地图缩放级别,整形值,范围从1~28。默认为最大
				infoUrl: 'http://www.baidu.com' // 在查看位置界面底部显示的超链接,可点击跳转
			});
		}
	});
}

function scan(){
	wx.scanQRCode({
		needResult: 1, // 默认为0，扫描结果由微信处理，1则直接返回扫描结果，
		scanType: ["qrCode","barCode"], // 可以指定扫二维码还是一维码，默认二者都有
		success: function (res) {
			var result = res.resultStr; // 当needResult 为 1 时，扫码返回的结果
			alert(result);
		}
	});
}


wx.ready(function () {

	wx.onMenuShareTimeline({
		title: shareTitle,
		link: link,
		imgUrl: imgUrl,
		success: function () {
		// 用户确认分享后执行的回调函数
		},
		cancel: function () {
		// 用户取消分享后执行的回调函数
		}
	});
	wx.onMenuShareAppMessage({
		title: shareTitle, // 分享标题
		desc: shareContent, // 分享描述
		link: link, // 分享链接
		imgUrl: imgUrl, // 分享图标
		type: link, // 分享类型,music、video或link，不填默认为link
		dataUrl: '', // 如果type是music或video，则要提供数据链接，默认为空
		success: function () {
		// 用户确认分享后执行的回调函数
		},
		cancel: function () {
		// 用户取消分享后执行的回调函数
		}
	});
	wx.onMenuShareQQ({
		title: shareTitle, // 分享标题
		desc: shareContent, // 分享描述
		link: link, // 分享链接
		imgUrl: imgUrl, // 分享图标
		success: function () {
		// 用户确认分享后执行的回调函数
		},
		cancel: function () {
		// 用户取消分享后执行的回调函数
		}
	});
	wx.onMenuShareWeibo({
		title: shareTitle, // 分享标题
		desc: shareContent, // 分享描述
		link: link, // 分享链接
		imgUrl: imgUrl, // 分享图标
		success: function () {
		// 用户确认分享后执行的回调函数
		},
		cancel: function () {
		// 用户取消分享后执行的回调函数
		}
	});
});

</script>
</div>
</form>
</body>
</html>
