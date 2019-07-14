window.SalesDialogue = function (config) {
	this.data = config.data || [];
	this.headPic = config.headPic || "demo/demo_header_01.jpg";
	this.auto = config.auto || 5000;
	
	this.mainArea = $('#main');
	this.userListArea = $('.userListPanel_01');
	this.messageArea = $('.dialoguePanel_01');
	this.userListField = this.userListArea.find('.listField ul');
	this.messageField = this.messageArea.find('.userDialogue .list ul');
	this.inputField = this.messageArea.find('.inputField .text .expanding .textarea');
	this.inputShower = this.messageArea.find('.inputField .text .expanding span');
	this.nameField = this.messageArea.find('.titleField .title h1');
	this.noUserClass = 'noUser';
	this.cantReplyClass = 'cantReply';
	this.showTalkClass = 'showMessage';
	this.userDomGroup = [];
	this.currentId = null;
	this.currentProject = null;
	this.autoTimeout = null;
	this.setup();
}
SalesDialogue.prototype = {
	structure: {
		userList:{
			dom:[
				'<li>',
					'<div class="img">',
						'<span><img src="" alt=""></span>',
					'</div>',
					'<div class="info">',
						'<h2></h2>',
						'<p></p>',
					'</div>',
					'<div class="time">',
						'<p></p>',
					'</div>',
					'<sup></sup>',
				'</li>',
			].join(''),
			selector:{
				pic:'.img img',
				name:'.info h2',
				content:'.info p',
				time:'.time p',
				count:'sup'
			},
			classes:{
				overTime:'old'
			}
		},
		message:{
			dom:[	
				'<li>',
					'<div class="img"><span><img src="demo/demo_header_09.jpg" alt=""></span></div>',
					'<div class="info">',
						'<span class="time">09-26 10:42</span><em>丸子没樱桃</em>',
					'</div>',
					'<div class="content">',
						'<div class="project">咨询：凯德龙之梦</div>',
						'<div class="text">',
							'<p>你好，凯德龙之梦最近有没有优惠啊？</p>',
						'</div>',
					'</div>',
				'</li>'
			].join(''),
			selector:{
				pic:'.img img',
				name:'.info em',
				time:'.info .time',
				project:'.content .project',
				text:'.content .text'
			},
			classes:{
				reply:'reply'
			},
			content:{
				project:'咨询：',
				replyName:'回复道：'
			}
		},
		errorTip:{
			dom:[
				'<li class="tip">',
					'<p>',
					'</p>',
				'</li>'
			].join(''),
			selector:{
				tip:'p'
			},
			content:{
				ioError:'消息发送失败！！'
			}
		}
		
	},
	warning: {
		noUser:'未选定联系人',
		noMessage:'发送消息不能为空',
		overTime:'会话已过期，不能回复'
	},
	setup: function(){
		this.bindEvents();
		this.readData();
		this.getDataInterval();
	},
	
	bindEvents: function(){
		var _this = this;
		this.inputField.bind('keyup',function(){
			_this.showInput();
		});
	},
	showInput: function(){
		var content = this.inputField.val().split('\n').join('<br>');
		this.inputShower.html(content);
		//this.scrollToBottom();
	},
	
	getData: function(data){
		if(!data) return false;
		if(!(data instanceof Array)){
			data = [data];
		}
		var currentTalkingChange = null;
		for(var i in data){
			var flag = false;
			for(var j in this.data){
				if(data[i].id == this.currentId){
					currentTalkingChange = data[i];
				}
				if(data[i].id == this.data[j].id){
					Array.prototype.push.apply(this.data[j].messages, data[i].messages);
					flag = true;
					break;
				}
			}
			if(!flag){
				this.data.unshift(data[i]);
			}
		}
		this.readData();
		if(currentTalkingChange){
			this.readMessage(currentTalkingChange);
		}
	},
	readData: function(){
		if(!this.data.length){
			this.userListField.addClass(this.noUserClass);
			return false;
		}
		this.readDataTime();
		this.removeUserDomGroup();
		this.createUserDomGroup();
		this.checkOverTime();
	},
	removeUserDomGroup: function(){
		for(var i in this.userDomGroup){
			this.userDomGroup[i].unbind().remove();
		}
		this.userDomGroup = [];
	},
	createUserDomGroup: function(){
		var _this = this;
		for(var i in this.data){
			var tempDom = $(this.structure.userList.dom);
			tempDom.find(this.structure.userList.selector.pic).attr('src',this.data[i].pic);
			tempDom.find(this.structure.userList.selector.name).html(this.data[i].name);
			tempDom.find(this.structure.userList.selector.time).html(this.getShowTime(this.data[i].time));
			tempDom.find(this.structure.userList.selector.content).html(this.data[i].content);
			if(this.data[i].count){
				tempDom.find(this.structure.userList.selector.count).css('display','').html(this.data[i].count);
			}else{
				tempDom.find(this.structure.userList.selector.count).hide();
			}
			tempDom.attr('currentId',this.data[i].id);
			this.userDomGroup.push(tempDom);
			tempDom.appendTo(this.userListField);
			tempDom.bind('click',function(){
				_this.changeTalk($(this).attr('currentId'));
			});
		}
	},
	checkOverTime: function(){
		var nowDate = new Date();
		var nowTime = nowDate.getTime();
		for(var i in this.data){
			if(nowTime -this.data[i].date.getTime()>172800000){
				this.data[i].overTime = true;
			}else{
				this.data[i].overTime = false;
			}
			for(var j in this.userDomGroup){
				if(this.userDomGroup[j].attr('currentId')==this.data[i].id){
					if(this.data[i].overTime){
						this.userDomGroup[j].addClass(this.structure.userList.classes.overTime);
					}else{
						this.userDomGroup[j].removeClass(this.structure.userList.classes.overTime);
					}
				}
			}
			if(this.data[i].id == this.currentId){
				if(this.data[i].overTime){
					this.messageArea.addClass(this.cantReplyClass);
				}else{
					this.messageArea.removeClass(this.cantReplyClass);
				}
			}
		}
	},
	readDataTime: function(){
		for(var i in this.data){
			var latestTime = 0;
			var latestMessage = 0;
			for(var j in this.data[i].messages){
				this.data[i].messages[j].date = this.readTime(this.data[i].messages[j].time);
				var time = this.data[i].messages[j].date.getTime();
				if(latestTime<time){
					latestTime = time;
					latestMessage = j;
				}
			}
			this.data[i].date = this.data[i].messages[latestMessage].date;
			this.data[i].time = this.data[i].messages[latestMessage].time;
			this.data[i].content = this.data[i].messages[latestMessage].content.replace(/<[\/]?[\w\s\(\)="':;-]+[\/]?>/gi,' ');
			this.data[i].messages.sort(function(a,b){return a.date.getTime()>b.date.getTime()?1:-1});
			
			var count = 0;
			for(var k in this.data[i].messages){
				count++;
				if(!!this.data[i].messages[k].reply){
					count = 0;
				}
			}
			this.data[i].count = count;
		}
	},
	readTime: function(time){
		if(!time)return false;
		time = time.split(' ');
		var data = time[0].split('-'), clock = time[1].split(':');
		var year = parseInt(data[0], 10);
		var month = parseInt(data[1], 10)-1;
		var day = parseInt(data[2], 10);
		var hour = parseInt(clock[0],10);
		var minute = parseInt(clock[1],10);
		var second = parseInt(clock[2],10);
		var date = new Date(year, month, day, hour, minute, second);
		return date;
	},
	
	getShowTime: function(time){
		if(!time)return false;
		var tempTime0 = time.split('-');
		tempTime0.shift();
		time = tempTime0.join('-');
		var tempTime1 = time.split(':');
		tempTime1.pop();
		time = tempTime1.join(':');
		return time;
	},
	
	sortUserAsTime: function(num){
			num = num || 1;
			this.data.sort(function(a,b){return a.date.getTime()<b.date.getTime()?num:-num});
			this.readData();
	},
	
	sortUserAsCount: function(num){
			num = num || 1;
			this.data.sort(function(a,b){return a.count<b.count?num:-num});
			this.readData();
	},
	
	changeTalk: function(currentId){
		if(currentId == this.currentId){
			this.showTalk();
			return false;
		}
		for(var i in this.data){
			if(this.data[i].id == this.currentId){
				this.data[i].value = this.inputField.val();
			}
		}
		this.currentId = currentId;
		this.readMessage();
		this.showTalk();
	},
	
	hideTalk: function(){
		this.mainArea.removeClass(this.showTalkClass);
	},
	showTalk: function(){
		this.mainArea.addClass(this.showTalkClass);
	},
	
	readMessage: function(dataItem){
		var isAdd = !(typeof dataItem == 'undefined');
		var current = null;
		for(var i in this.data){
			if(this.currentId == this.data[i].id){
				current = this.data[i];
				break;
			}
		}
		if( current.overTime){
			this.messageArea.addClass(this.cantReplyClass);
		}else{
			this.messageArea.removeClass(this.cantReplyClass);
		}
		if(!isAdd){
			this.messageField.html('');
			dataItem = current;
		}
		this.nameField.html(dataItem.name);
		for(var j in dataItem.messages){
			var tempDom = $(this.structure.message.dom);
			if(!!dataItem.messages[j].reply){
				tempDom.addClass(this.structure.message.classes.reply);
				tempDom.find(this.structure.message.selector.pic).attr('src',this.headPic);
				tempDom.find(this.structure.message.selector.name).html(this.structure.message.content.replyName);
			}else{
				tempDom.find(this.structure.message.selector.pic).attr('src',dataItem.pic);
				tempDom.find(this.structure.message.selector.name).html(dataItem.name);
				tempDom.find(this.structure.message.selector.project).html(this.structure.message.content.project+dataItem.messages[j].project);
			}
			tempDom.find(this.structure.message.selector.time).html(this.getShowTime(dataItem.messages[j].time));
			tempDom.find(this.structure.message.selector.text).html(dataItem.messages[j].content);
			tempDom.appendTo(this.messageField);
			
			if(!!dataItem.messages[j].ioError){
				this.addErrorTip();
			}
		}
		this.currentProject = dataItem.messages[dataItem.messages.length-1].project;
		this.inputField.val(dataItem.value || '');
		this.showInput();
		this.scrollToBottom();
		
	},
	
	addErrorTip: function(){
		var tempTip = $(this.structure.errorTip.dom);
		tempTip.find(this.structure.errorTip.selector.tip).html(this.structure.errorTip.content.ioError);
		tempTip.appendTo(this.messageField);
		this.scrollToBottom();
	},
	
	submitReply: function(ajaxPush){
		if(!ajaxPush)return false;
		if(!this.currentId){
			showTip.show(this.warning.noUser,true);
			this.inputField.val('');
			this.showInput();
			return false;
		}
		if(!this.inputField.val()){
			showTip.show(this.warning.noMessage,true);
			return false;
		}
		if(this.messageArea.is('.'+this.cantReplyClass)){
			showTip.show(this.warning.overTime,true);
			return false;
		}
		var _this = this;
		var time = '';
		var nowTime = this.getTxtTime();
		var content = this.inputField.val().replace(/\n/gi,'<br>');
		
		var message = {
			project:this.currentProject,
			time:nowTime,
			content:content,
			reply:true
		};
		
		var tempData = {
			id:this.currentId,
			messages:[message]
		}
		this.getData(tempData);
		this.inputField.val('');
		this.showInput();
		ajaxPush(this.currentId,message,function(){
			message.ioError = true;
			_this.addErrorTip();
		});
	},
	
	getTxtTime:function(date){
		date = date || new Date();
		var year = date.getYear();
		if(year<2000) year+= 1900;
		var nowDate = [year,date.getMonth()+1,date.getDate()];
		var nowClock = [date.getHours(),date.getMinutes(),date.getSeconds()];
		for(var i in nowDate){
			if(nowDate[i]<10){
				nowDate[i] = '0' + nowDate[i];
			}else{
				nowDate[i] = '' + nowDate[i];
			}
		}
		for(var i in nowClock){
			if(nowClock[i]<10){
				nowClock[i] = '0' + nowClock[i];
			}else{
				nowClock[i] = '' + nowClock[i];
			}
		}
		var time = nowDate.join('-')+' '+nowClock.join(':');
		return time;
	},
	scrollToBottom: function(){
		this.messageField.get(0).scrollTop = this.messageField.get(0).scrollHeight;
	},
	getDataInterval: function(){
		var _this = this;
		clearTimeout(this.autoTimeout);
		this.autoTimeout = setTimeout(function(){
			_this.autoGetData(function(data){
				if(data){
					_this.getData(data);
				}
				_this.getDataInterval();
			});
		},this.auto);
	},
	autoGetData: function(fn){
		fn();
	}
}