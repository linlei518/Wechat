//视频窗口
var is360 = false;
try {
	window.external.twExtSendMessage2();
	is360 = true
} catch (g) {}

var ShowVideoCallBack = function(){};

window.ShowVideo = function(config){
	if(!config) return false;
	this.video = [];
	this.videoInnerHTML = '<div></div>';
	this.btnInnerHTML = '<span style="padding:0 12px; cursor:pointer;"></span>';
	var title,url,width,height,autoPlay,control,videoInner,btnInner;
	if(!config.video){
		title = config.title || '';
		if(typeof config.url == 'string'){
			url = [config.url];
		}else{
			url = config.url;
		}
		width = config.width || 1280;
		height = config.height || 720;
		autoPlay = config.autoPlay !== undefined ? config.autoPlay : true;
		videoInner = $(this.videoInnerHTML);
		btnInner = $(this.btnInnerHTML);
		this.video.push({
			title:title,
			url:url,
			width:width,
			height:height,
			autoPlay:autoPlay,
			control:control,
			videoInner:videoInner,
			btnInner:btnInner
		});
	}else{
		for(var i in config.video){
			title = config.video[i].title || '';
			if(typeof config.video[i].url == 'string'){
				url = [config.video[i].url];
			}else{
				url = config.video[i].url;
			}
			width = config.video[i].width || 1280;
			height = config.video[i].height || 720;
			autoPlay = config.video[i].autoPlay !== undefined ? config.video[i].autoPlay : true;
			videoInner = $(this.videoInnerHTML);
			btnInner = $(this.btnInnerHTML);
			this.video.push({
				title:title,
				url:url,
				width:width,
				height:height,
				autoPlay:autoPlay,
				control:control,
				videoInner:videoInner,
				btnInner:btnInner
			});
		}
	}
	this.bgColor = config.bgColor || config.backgroundColor || config.background || '#000';
	this.cb = config.callback || function(){};
	this.closePic = '/images/btn_close_01.gif';
	this.windowSize = config.windowSize || {
		width:document.documentElement.clientWidth,
		height:document.documentElement.clientHeight
	}
	this.padding = 60;
	this.videoContent = $('<section id="showVideo" style="position:fixed; left:0; right:0; top:0; bottom:0; background:'+this.bgColor+'; overflow:hidden; z-index:30; display:none;"></section>');
	this.videoItemCode = '<div id="showVideoItem" style="width:100%; height:100%; display:block;"></div>';
	this.closeBtn = $('<div style="width:45px; height:45px; display:block; cursor:pointer; position:absolute; z-index:2; left:20px; top:20px; background:url('+this.closePic+') 0 0 no-repeat;"></div>');
	this.btnField = $('<div style=" height:40px; line-height:40px; font-size:24px; font-weight:bold; color:#aaa; position:absolute; z-index:2; right:60px; top:18px;"></div>');
	this.videoBg = $('<div style="position:absolute; left:0; right:0; top:0; bottom:0; background:'+this.bgColor+'; cursor:pointer; z-index:0;"></div>');
	this.setup();
}

ShowVideo.prototype = {
	setup:function(){
		ShowVideoCallBack = this.hide;
		
		this.isShow = -1;
		for(var k in this.video){
			var tempMaxWidth = this.windowSize.width - this.padding*2;
			var tempMaxHeight = this.windowSize.height - this.padding*2;
			if(this.video[k].width > tempMaxWidth){
				this.video[k].height = this.video[k].height*tempMaxWidth/this.video[k].width;
				this.video[k].width = tempMaxWidth;
			}
			if(this.video[k].height > tempMaxHeight){
				this.video[k].width = this.video[k].width*tempMaxHeight/this.video[k].height;
				this.video[k].height = tempMaxHeight;
			}
			
			this.video[k].videoInner.attr('style','position:absolute; z-index:1; display:block; left:50%; top:50%; width:'+this.video[k].width+'px; height:'+this.video[k].height+'px; margin:-'+parseInt(this.video[k].height/2)+'px 0 0 -'+parseInt(this.video[k].width/2)+'px;');
			this.video[k].btnInner.html(this.video[k].title).attr('num',k).appendTo(this.btnField);
		}
	},
	bindEvents: function(){
		var _this = this;
		for(var k in this.video){
			this.video[k].btnInner.click(function(){
				_this.changeVideo($(this).attr('num'));
			});
		}
		this.closeBtn.bind('click',function(){
			_this.hide();
		});
		this.videoBg.bind('click',function(){
			_this.hide();
		});
	},
	unBindEvents: function(){
		var _this = this;
		for(var k in this.video){
			this.video[k].btnInner.unbind();
		}
		this.closeBtn.unbind();
		this.videoBg.unbind();
	},
	createObject: function(num){
		if(num == -1) return false;
		var videoFlashvars = {
			videoPatch:'../'+this.video[num].url,
			skinPatch:'/flash/SkinOverPlayStopSeekFullVol.swf',
			isAutoPlay:(this.video[num].autoPlay)?1:0,
			callBack:'ShowVideoCallBack'
		};
		var videoParams = {"allowFullScreen": false, "wmode": "transparent"};
		var videoAttributes = {};
		var swfPlayerAddress = "/flash/player.swf";
		if(is360){
			swfPlayerAddress = "/flash/player.swf?url="+parseInt(100000*Math.random());
		}
		swfobject.embedSWF(swfPlayerAddress, "showVideoItem", this.video[num].width.toString(), this.video[num].height.toString(), "9.0.0", swfPlayerAddress, videoFlashvars, videoParams, videoAttributes);
	},
	removeObject: function(){
		swfobject.removeSWF("showVideoItem");
	},
	changeVideo:function(num){
		if(num == this.isShow) return false;
		if(this.isShow>-1){
			this.removeObject();
		}
		this.isShow = num;
		for(var k in this.video){
			if(k!=num){
				this.video[k].videoInner.remove();
				this.video[k].btnInner.css('color','');
			}else{
				this.video[k].videoInner.append($(this.videoItemCode)).appendTo(this.videoContent);
				this.video[k].btnInner.css('color','#ddd');
				this.createObject(this.isShow);
			}
		}
	},
	show:function(){
		if(this.isShow>-1) return false;
		var _this = this;
		this.videoContent.append(this.closeBtn).append(this.videoBg).append(this.btnField).appendTo('body');
		this.changeVideo(0);
		this.videoContent.fadeIn(1000,function(){
			if(!_this.noAutoPlay){
				try{
					var videoItem = swfobject.getObjectById('showVideoItem');
					videoItem.moviePlayFunction();
				}catch(e){
				}
			}
			_this.bindEvents();
		});
	},
	hide:function(){
		if(!this.isShow==-1) return false;
		var _this = this;
		this.unBindEvents();
		this.videoContent.stop().fadeOut(1000,function(){
			_this.changeVideo(-1);
			_this.cb();
		});
	},
	distory:function(){
		this.unBindEvents();
		this.removeObject();
		this.videoContent.remove();
		for(var i in this){
			delete this[i];
		}
	}
}
//视频窗口结束

window.Video = function(){
	var _this = this;
	this.defaultConfig = {
		title:'',
		width:1280,
		height:720,
		callback:function(){
			_this.cb(_this);
		}
	}
	this.isPlaying = false;
}
Video.prototype = {
	play: function(url,config){
		if(!url)return false;
		if(this.isPlaying)return false;
		this.isPlaying = true;
		config = config || this.defaultConfig;
		this.videoContent = new ShowVideo({
			video:[{
				title:config.title || this.defaultConfig.title,
				url:url,
				width:parseFloat(config.width || this.defaultConfig.width),
				height:parseFloat(config.height || this.defaultConfig.height)
			}],
			callback:this.defaultConfig.callback
		});
		this.videoContent.show();
		try{
			audio.stop();
		}catch(e){}
	},
	cb: function(obj){
		this.isPlaying = false;
		obj.videoContent.distory();
	}
}

var video = new Video();
