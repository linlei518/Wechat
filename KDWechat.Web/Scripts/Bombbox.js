window.Bombbox = function(){
	this.windowSize = {
		width:document.documentElement.clientWidth || document.body.clientWidth,
		height:document.documentElement.clientHeight || document.body.clientHeight
	}
	this.config = {
		borderWidth:5,
		startWidth:200,
		startHeight:200,
		loadingIcon:'/images/icon_loading_01.gif',
		closeBtn:'/images/btn_close_01.gif',
		defaultSize:{
			width:this.windowSize.width-150,
			height:this.windowSize.height-100
		}
	};
	
	this.html = [
		'<div style="position:fixed; left:0; right:0; top:0; bottom:0; z-index:20;">',
			'<div class="mask" style="position:absolute; left:0; right:0; top:0; bottom:0; background:url(/images/bg_mask_01.png); cursor:pointer; z-index:20;"></div>',
			'<div class="iframe" style="position:absolute; left:50%; top:50%; width:'+this.config.startWidth+'px; height:'+this.config.startHeight+'px; margin:-'+(this.config.startHeight/2+this.config.borderWidth)+'px 0 0 -'+(this.config.startWidth/2+this.config.borderWidth)+'px; background:url('+this.config.loadingIcon+') 50% 50% no-repeat #fff; border:'+this.config.borderWidth+'px solid #ccc; z-index:21;">',
				'<div class="close" style="width:45px; height:45px; position:absolute; background:url('+this.config.closeBtn+') 50% 50% no-repeat; right:-65px; top:-'+this.config.borderWidth+'px; cursor:pointer;"></div>',
			'</div>',
		'</div>'
	].join('');
	this.iframeHtml = '<iframe frameborder="0" scrolling="no" style=" display:none; width:100%; height:100%;"></iframe>';
	this.oldSize = {
		width: this.config.startWidth,
		height: this.config.startHeight,
	}
	this.history = [];
	this.showSpeed = 300;
	this.isOpen = false;
	this.useDefaultSize = true;
	this.disabled = false;
}
Bombbox.prototype = {
	openBox: function(url,callback){
		var _this = this;
		if(!url || typeof url != 'string' || this.disabled)return false;
		callback = callback || function(){};
		this.history.push(url);
		if(!this.isOpen){
			this.isOpen = true;
			this.iframeItem = $(this.iframeHtml);
			this.bombbox = $(this.html);
			this.bombbox.appendTo('body').hide().fadeIn(300);
		}else{
			this.iframeItem.unbind().remove();
			this.iframeItem = $(this.iframeHtml);
		}
		this.iframeItem.appendTo(this.bombbox.find('.iframe'));
		this.bombbox.find('.iframe').css('background','url('+this.config.loadingIcon+') 50% 50% no-repeat #fff');
		this.bindEvents();
		
		this.iframeItem.bind('load',function(){
			_this.loadOk();
			callback();
		});
		this.iframeItem.attr('src',url);
	},
	
	bindEvents: function(){
		var _this = this;
		this.bombbox.find('.close').bind('click',function(){
			_this.closeBox();
		});
		this.bombbox.find('.mask').bind('click',function(){
			_this.closeBox();
		});
		
		$(window).resize(function(){
			_this.resizeBox();
		});
	},
	
	resizeBox: function(){
		this.windowSize = {
			width:document.documentElement.clientWidth || document.body.clientWidth,
			height:document.documentElement.clientHeight || document.body.clientHeight
		}
		this.config.defaultSize = {
			width:this.windowSize.width-120,
			height:this.windowSize.height-100
		}
		if(this.useDefaultSize && this.isOpen){
			this.oldSize.width = this.config.defaultSize.width;
			this.oldSize.height = this.config.defaultSize.height;
			this.bombbox.find('.iframe').css('margin-top',-this.config.defaultSize.height/2-this.config.borderWidth).css('margin-left',-this.config.defaultSize.width/2-this.config.borderWidth-25).css('width',this.config.defaultSize.width).css('height',this.config.defaultSize.height);
		}
	},
	unbindEvents: function(){
		this.iframeItem.unbind();
		this.bombbox.find('.close').unbind();
		this.bombbox.find('.mask').unbind();
	},
	
	loadOk: function(){
		var speed = this.showSpeed;
		var size = this.config.defaultSize;
		this.useDefaultSize = true;
		try{
			size = this.iframeItem.get(0).contentWindow.offsetSize || this.config.defaultSize;
			if(this.iframeItem.get(0).contentWindow.offsetSize){
				this.useDefaultSize = false
			}
		}catch(e){}
		
		var _this = this;
		
		if(this.oldSize.width != size.width || this.oldSize.height != size.height){
			this.oldSize.width = size.width;
			this.oldSize.height = size.height;
			this.bombbox.find('.iframe').css('background','#fff').animate({
				marginTop: -size.height/2-this.config.borderWidth,
				marginLeft: -size.width/2-this.config.borderWidth-25,
				width:size.width,
				height:size.height
			},speed,function(){
				_this.iframeItem.hide();
				try{
					this.iframeItem.get(0).contentWindow.setupSelect();
				}catch(e){}
				_this.iframeItem.fadeIn(speed);
			});
		}
		
	},
	closeBox: function(){
		var _this = this;
		
		this.oldSize = {
			width: this.config.startWidth,
			height: this.config.startHeight,
		}
		this.disabled = true;
		this.unbindEvents();
		this.bombbox.stop().fadeOut(300,function(){
			_this.disabled = false;
			_this.iframeItem.remove();
			_this.bombbox.remove();
			_this.history = [];
			_this.isOpen = false;
		});
		try{
			audio.stop();
		}catch(e){}
	},
	prev: function(){
		if(this.history.length<2){
			this.closeBox();
			return;
		}
		var url = this.history.splice(this.history.length-2,2)[0];
		this.openBox(url);
	}
}
var bombbox = new Bombbox();