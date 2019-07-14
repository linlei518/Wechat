window.WeixinShare = function(config){
	if(!config) config = {};
	
	this.imgConfigUrl = config.imgUrl || document.getElementsByTagName('img')[0].src || '';
	this.linkURL   =  config.url || window.location.href;		//分享的网址链接
	this.title  = config.title || window.document.title;		//分享的标题
	this.desc = config.content || title;
	this.showControl = config.showControl || false;
	
	this.readyToShare = false;
	this.readyBridge = false;
	this.canShare = (typeof config.canShare !== 'undefined')?config.canShare:true;
	this.isWeixin = navigator.userAgent.toLowerCase().match(/MicroMessenger/i)=="micromessenger";
	this.noWeixin = config.noWeixin || function(){};
	this.shareCallback = config.callback || function(){};
	this.shareOkCallback = config.okCallback || function(success){};
	
	this.setup();
}

WeixinShare.prototype = {
	setup: function(){
		if(this.isWeixin){
			this.bindLoadEvents();
		}else{
			this.noWeixin();
			this.imgUrl = this.imgConfigUrl;
			this.linkURL = this.checkUrl(this.linkURL) ;	
		}
	},
	bindLoadEvents: function(){
		var _this = this;
		if(document.addEventListener){
			document.addEventListener('WeixinJSBridgeReady', onBridgeReady, false);
		} else if(document.attachEvent){
			document.attachEvent('WeixinJSBridgeReady'   , onBridgeReady);
			document.attachEvent('onWeixinJSBridgeReady' , onBridgeReady);
		}
		if(this.imgConfigUrl!=''){
			var tempImg = new Image();
			
			tempImg.onload = function(){
				tempImg.onload = null;
				tempImg.onerror = null;
				_this.readyToShare = true;
				if(_this.readyBridge){
					_this.shareInitOk();
				}
			}
			tempImg.onerror = function(){
				tempImg.onload = null;
				tempImg.onerror = null;
				_this.readyToShare = true;
				if(_this.readyBridge){
					_this.shareInitOk();
				}
			}
			
			tempImg.src = this.imgConfigUrl;
		}
		function onBridgeReady(){
			_this.readyBridge = true;
			if(_this.readyToShare){
				_this.shareInitOk();
			}
		}
		
	},
	shareInitOk: function(){
		this.imgUrl = this.imgConfigUrl;	//分享的图片链接
		this.linkURL = this.checkUrl(this.linkURL) ;		//分享的网址链接
		var _this = this;
		
		
		
		if(this.imgUrl.toLowerCase()=='none'){
			this.imgUrl = '';
		}else{
			this.imgUrl = this.checkUrl(this.imgUrl);
		}
		
		// 发送给好友; 
		WeixinJSBridge.on('menu:share:appmessage', function(argv){
			shareToWeixin('sendAppMessage',argv);
		});
		// 分享到朋友圈;
		WeixinJSBridge.on('menu:share:timeline', function(argv){
			shareToWeixin('shareTimeline',argv);
		});
	
		// 分享到微博;
		WeixinJSBridge.on('menu:share:weibo', function(argv){
			shareToWeixin('shareWeibo',argv);
		});
	
		// 隐藏右上角的选项菜单入口;
		if(!_this.showControl){
			WeixinJSBridge.call('hideToolbar');
		}
	},
	
	shareToWeixin: function(goal,argv){
		if(!this.canShare) return false;
		this.shareCallback();
		var shareContent = {
			img_url:this.imgUrl,
			link:this.linkURL,
			desc:this.desc,
			title:this.title
		};
		if(goal == 'shareWeibo'){
			shareContent = {
				img_url:this.imgUrl,
				content:(this.title==this.desc)?this.title:this.title +' '+ this.desc,
				url:this.linkURL
			}
		}
		WeixinJSBridge.invoke(goal,shareContent,function(res){
			this.checkShare(res.err_msg);
			WeixinJSBridge.log(res.err_msg);
		});
	},
	
	openUrl: function(url){
		var tempLink = document.createElement('a');
		tempLink.href = url;
		tempLink.target = '_blank';
		tempLink.click();
	},
	
	shareToWeibo: function(){
		var content = (this.title==this.desc)?this.title:this.title +' '+ this.desc;
		var shareLink = 'http://service.weibo.com/share/mobile.php?title='+content+'&url='+this.linkURL+'&source=bookmark&pic='+this.imgUrl+'&ralateUid=';
		this.openUrl(shareLink);
	},
	shareToTWeibo: function(){
		if(this.isWeixin){
			shareToWeixin('shareWeibo');
		}else{
			var content = (this.title==this.desc)?this.title:this.title +' '+ this.desc;
			var shareLink = 'http://share.v.t.qq.com/index.php?c=share&a=index&title='+content+'&url='+this.linkURL+'&pic='+this.imgUrl;
			this.openUrl(shareLink);
		}
	},
	shareToRenren: function(){
		var content = (this.title==this.desc)?this.title:this.title +' '+ this.desc;
		var shareLink = 'http://widget.renren.com/dialog/share?resourceUrl='+this.linkURL+'&srcUrl='+this.linkURL+'&title='+content+'&pic='+this.imgUrl+'&description=';
		this.openUrl(shareLink);
	},
	shareToQzone: function(){
		var content = (this.title==this.desc)?this.title:this.title +' '+ this.desc;
		var shareLink = 'http://sns.qzone.qq.com/cgi-bin/qzshare/cgi_qzshare_onekey?url='+this.linkURL+'&title='+content+'&pics='+this.imgUrl+'&summary=';
		this.openUrl(shareLink);
	},
	
	checkUrl: function(linkU){
		if(!linkU)return false;
		var tempUrl = '';
		var urlStrs = window.location.href.replace('http://','').split('/');
		var url = urlStrs[0];
		if(linkU.indexOf('http://')!=-1){
			tempUrl = linkU;
		}else if(linkU.indexOf(url)!=-1){
			tempUrl = 'http://'+linkU;
		}else{
			if(linkU.indexOf('../')!=-1){
				var k = 0
				while(linkU.indexOf('../')!=-1){
					linkU = linkU.replace('../','');
					k++;
				}
				tempUrl = 'http://'+url+'/';
				for(var i = 1; i<urlStrs.length-k-1; i++){
					tempUrl+= urlStrs[i]+'/';
				}
				tempUrl+=linkU;
			}else if(linkU.indexOf('./')!=-1 || linkU[0]!='/'){
				while(linkU.indexOf('./')!=-1){
					linkU = linkU.replace('./','');
				}
				tempUrl = 'http://'+url+'/';
				for(var i = 1; i<urlStrs.length-1; i++){
					tempUrl+= urlStrs[i]+'/';
				}
				tempUrl+=linkU;
			}else{
				tempUrl = 'http://'+url+'/'+linkU;
			}
		}
		return tempUrl;
	},
	
	checkShare: function(msg){
		msg = msg.toLowerCase();
		if(msg.indexOf('confirm')!=-1 || msg.indexOf('ok')!=-1 ){
			this.shareOkCallback(true);
		}else{
			this.shareOkCallback(false);
		}
	},
	
	changeShareUrl: function(url){
		this.linkURL = this.checkUrl(url);
	},
	stopShare: function(){
		this.canShare = false;
	},
	beginShare: function(){
		this.canShare = true;
	}
}