window.SalesDialogue = function (config) {
	this.data = config.data || [];
	this.headPic = config.headPic || "images/pic_header_01.jpg";
	this.auto = config.auto || 5000;
	
	this.userListArea = $('.userListPanel_02');
	this.messageArea = $('.dialoguePanel_01');
	this.userListField = this.userListArea.find('.listField ul');
	this.messageField = this.messageArea.find('.userDialogue .list ul');
	this.messageList = this.messageArea.find('.userDialogue');
	this.inputField = this.messageArea.find('.inputField .text .textarea');
	this.noUserClass = 'noUser';
	this.cantReplyClass = 'cantReply';
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
						'<p>2014-09-15 10:02:07</p>',
						'<sup>3</sup>',
					'</div>',
				'</li>'
			].join(''),
			selector:{
				pic:'.img img',
				name:'.info h2',
				time:'.info p',
				count:'.info sup'
			},
			classes:{
				current:'current',
				overTime:'old'
			}
		},
		message:{
			dom:[
				'<li>',
					'<div class="img">',
						'<span><img src="" alt=""></span>',
					'</div>',
					'<div class="content">',
						'<div class="info">',
							'<span class="time"></span>',
							'<em></em>',
							'<span class="project"></span>',
						'</div>',
						'<div class="text">',
						'</div>',
						'<i class="arrow"></i>',
					'</div>',
				'</li>'
			].join(''),
			selector:{
				pic:'.img img',
				name:'.content .info em',
				time:'.content .info .time',
				project:'.content .info .project',
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
		this.readData();
		this.getDataInterval();
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
			tempDom.find(this.structure.userList.selector.time).html(this.data[i].time);
			if(this.data[i].count){
				tempDom.find(this.structure.userList.selector.count).css('display','').html(this.data[i].count);
			}else{
				tempDom.find(this.structure.userList.selector.count).hide();
			}
			tempDom.attr('currentId',this.data[i].id);
			if(this.data[i].id == this.currentId){
				tempDom.addClass(this.structure.userList.classes.current);
			}
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
		if(currentId == this.currentId)return false;
		for(var i in this.data){
			if(this.data[i].id == this.currentId){
				this.data[i].value = this.inputField.val();
			}
		}
		this.currentId = currentId;
		for(var i in this.data){
			if(this.data[i].id == this.currentId){
				this.inputField.val(this.data[i].value || '');
			}
		}
		for(var i in this.userDomGroup){
			if(this.userDomGroup[i].attr('currentId') == this.currentId){
				this.userDomGroup[i].addClass(this.structure.userList.classes.current);
			}else{
				this.userDomGroup[i].removeClass(this.structure.userList.classes.current);
			}
		}
		this.readMessage();
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
			tempDom.find(this.structure.message.selector.time).html(dataItem.messages[j].time);
			tempDom.find(this.structure.message.selector.text).html(dataItem.messages[j].content);
			tempDom.appendTo(this.messageField);
			
			if(!!dataItem.messages[j].ioError){
				this.addErrorTip();
			}
		}
		this.currentProject = dataItem.messages[dataItem.messages.length-1].project;
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
			alert(this.warning.noUser);
			this.inputField.val('')
			return false;
		}
		if(!this.inputField.val()){
			alert(this.warning.noMessage);
			return false;
		}
		if(this.messageArea.is('.'+this.cantReplyClass)){
			alert(this.warning.overTime);
			return false;
		}
		var _this = this;
		var time = '';
		var nowTime = this.getTxtTime();
		
		var message = {
			project:this.currentProject,
			time:nowTime,
			content:this.inputField.val(),
			reply:true
		};
		
		var tempData = {
			id:this.currentId,
			messages:[message]
		}
		this.getData(tempData);
		this.inputField.val('');
		ajaxPush(this.currentId,message,function(){
			message.ioError = true;
			_this.addErrorTip();
		});
	},
	
	getTxtTime:function(date){
		date = date || new Date();
		var year = date.getYear();
		if(year<2000){
			year += 1900;
		}
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
		this.messageList.get(0).scrollTop = this.messageList.get(0).scrollHeight;
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