var audioPlayingClass = 'audioPlaying';

window.Audio = function(){
	this.mainId = 'musicContainer';
	this.domHtml = '<div style="position:absolute;left:0;right:0;width:0;height:0;overflow:hidden"><div id="'+this.mainId+'"></div></div>';
	this.mainFlash = '/flash/musicPlayer.swf';
	this.flashvars = {
		playCb:'audioPlayCb',
		cb:'audioCb'
	};
	this.params = {"allowFullScreen": false, "wmode": "transparent"};
	this.attributes = {};
	this.playingClass = audioPlayingClass;
	
	this.audioArr = [];
	this.audioNum = 0;
	
	this.setup();
}

Audio.prototype = {
	setup:function(){
		this.dom = $(this.domHtml);
		this.dom.appendTo('body');
		swfobject.embedSWF(this.mainFlash,this.mainId, "1", "1", "9.0.0",this.mainFlash, this.flashvars, this.params, this.attributes);
	},
	play:function(url,dom){
		var newAudio = {
			url:url,
			num:this.audioNum,
			dom:dom
		}
		this.audioArr.push(newAudio);
		this.audioNum++;
		var swf = swfobject.getObjectById(this.mainId);
		try{
			swf.musicCall('play',newAudio.url,newAudio.num);
		}catch(e){
		}
	},
	stop:function(){
		var swf = swfobject.getObjectById(this.mainId);
		try{
			swf.musicCall('stop');
		}catch(e){
		}
	},
	playCb: function(num){
		for(var i in this.audioArr){
			if(this.audioArr[i].num == num){
				this.audioArr[i].dom.addClass(this.playingClass);
			}
			break;
		}
	},
	cb: function(type,num){
		var tempNum = 0;
		for(var i in this.audioArr){
			if(this.audioArr[i].num == num){
				this.audioArr[i].dom.removeClass(this.playingClass);
				tempNum = i;
			}
			break;
		}
		this.audioArr.splice(tempNum,1);
		if(type=='error'){
			alert('音频载入错误');
		}
	}
}

var audio = new Audio();

function audioPlayCb(num){
	audio.playCb(num);
}
function audioCb(type,num){
	audio.cb(type,num);
}

function audioControl(dom,url){
	if($(dom).hasClass(audioPlayingClass)){
		audio.stop();
	}else{
		audio.play(url,$(dom));
	}
}


