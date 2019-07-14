window.ShowTip = function(){
	this.domHtml = [
		'<div class="showTip">',
			'<h1></h1>',
			'<p></p>',
			'<a class="btn close" href="javascript:void(0)"></a>',
		'</div>'
	].join('');
	this.selector = {
		title:'h1',
		content:'p',
		closeBtn:'.close'
	};
	this.colors = {
		fail:'#c00',
		success:'#44b549'
	}
	this.setup();
}

ShowTip.prototype = {
	setup: function(){
		this.dom = $(this.domHtml);
		this.title = this.dom.find(this.selector.title);
		this.content = this.dom.find(this.selector.content);
		this.closeBtn = this.dom.find(this.selector.closeBtn);
		this.closeTimeout = null;
		this.checkUrl();
	},
	checkUrl: function(){
		var url = window.location.href;
		if(url.indexOf('#')!=-1||url.indexOf('?')!=-1||url.indexOf('&')!=-1){
			var text = '';
			var flag = false;
			if(url.indexOf('fail=')!=-1){
				text = url.split('fail=')[1];
				flag = true;
			}else if(url.indexOf('success=')!=-1){
				text = url.split('success=')[1];
			}
			if(text){
				text = decodeURI(text);
				this.show(text,flag);
			}
		}
	},
	show: function(info,fail){
		if(!info)return;
		var title = '',content = '',control = false,delay = 3000;
		
		if(typeof info == 'string'){
			title = info;
		}else{
			title = info.title;
			content = info.content || '';
			control = info.control || false;
			delay = (typeof info.delay != 'undefined')?info.delay:3000;
		}
		fail = fail || false;
		
		var _this = this;
		this.closeBtn.hide();
		if(control){
			this.closeBtn.css('display','block').click(function(){
				_this.hide();
			});
		}
		
		if(!content){
			this.content.hide();
		}else{
			this.content.show();
		}
		
		if(fail){
			this.dom.css('background-color',this.colors.fail);
		}else{
			this.dom.css('background-color',this.colors.success);
		}
		this.content.html(content);
		this.title.html(title);
		
		this.dom.appendTo('body').stop().show().css('opacity',0).fadeTo(500,1);
		
		clearTimeout(this.closeTimeout);
		if(delay){
			this.closeTimeout = setTimeout(function(){
				_this.hide();
			},delay);
		}
		
	},
	hide: function(){
		
		this.closeBtn.unbind().hide();
		this.dom.stop().fadeOut(500,function(){
			$(this).remove();
		});
	}
}

var showTip = new ShowTip();



window.Bombbox = function(){
	this.windowSize = {
		width:document.documentElement.clientWidth || document.body.clientWidth,
		height:document.documentElement.clientHeight || document.body.clientHeight
	}
	this.config = {
		loadingIcon:'../images/icon_loading_01.gif'
	};
	
	this.html = [
		'<div style="position:fixed; left:0; right:0; top:100%; height:100%; z-index:20;">',
			'<div class="iframe" style="position:absolute; left:0; top:0; right:0; bottom:0; background:url('+this.config.loadingIcon+') 50% 50% no-repeat #fff; z-index:21;">',
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
			this.bombbox.appendTo('body').css('top','100%').animate({top:0},this.showSpeed);
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
	},
	unbindEvents: function(){
		this.iframeItem.unbind();
	},
	
	loadOk: function(){
		var speed = this.showSpeed;
		this.iframeItem.hide().fadeIn(speed);
	},
	closeBox: function(){
		var _this = this;
		
		this.disabled = true;
		this.unbindEvents();
		this.bombbox.stop().animate({top:'100%'},300,function(){
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