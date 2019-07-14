<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="audio_detail.aspx.cs" Inherits="KDWechat.Web.wxpage.audio_detail" %>


<%@ Register Src="top.ascx" TagName="top" TagPrefix="uc1" %>
<%@ Register Src="bottom.ascx" TagName="bottom" TagPrefix="uc2" %>
<!doctype html>
<html>
<head id="Head1" runat="server">
    <meta charset="UTF-8">
    <title>音频标题</title>
    <meta name="viewport" id="viewport" content="width=device-width, user-scalable=no,initial-scale=1,maximum-scale=1">
    <meta name="screen-orientation" content="portrait/landscape">
    <meta content="yes" name="apple-mobile-web-app-capable">
    <meta content="black" name="apple-mobile-web-app-status-bar-style">
    <meta content="telephone=no" name="format-detection">
    <link href="css/global.css" rel="stylesheet" type="text/css">
</head>
<body>

    <uc1:top ID="top1" Visible="false" runat="server" />
    <section id="main">
	<div class="audioPanel_01">
		<div class="titleField">
			<h1><asp:Literal ID="lblTitle" Visible="false" runat="server"></asp:Literal></h1>
		</div>
		<div class="audioField">
			<audio preload="auto" id="audio">
				<asp:Literal ID="lblAudio" runat="server"></asp:Literal>
			</audio>
			<button onClick="audioPlay()" id="audioBtn" class="btn"><i class="stopIcon"><svg style="enable-background:new 0 0 24 32" xmlns="http://www.w3.org/2000/svg" xml:space="preserve" height="32px" width="24px" version="1.1" y="0px" x="0px" xmlns:xlink="http://www.w3.org/1999/xlink" viewBox="0 0 24 32"><polygon points="0 0 24 16 0 32" fill="#010101"/></svg></i><i class="playingIcon"><svg version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" width="64px" height="64px" viewBox="0 0 64 64" enable-background="new 0 0 64 64" xml:space="preserve"><path d="M9.928,60.75h17.729V3.25H9.928V60.75z"/><path d="M36.343,3.25v57.5h17.729V3.25H36.343z"/></svg></i></button>
		</div>
	</div>
</section>
    <uc2:bottom ID="bottom1" runat="server" />
    <script>
        var audioIsPlaying = false;
        function audioPlay() {
            var audio = document.getElementById('audio');
            var audioBtn = document.getElementById('audioBtn');
            if (audioIsPlaying) {
                audioIsPlaying = false;
                audio.pause();
                audioBtn.className = audioBtn.className.replace(/ playing/g, '').replace(/playing/g, '');
            } else {
                audioIsPlaying = true;
                audio.play();
                audioBtn.className = audioBtn.className + ' playing';
            }
        }
    </script>
</body>
</html>
