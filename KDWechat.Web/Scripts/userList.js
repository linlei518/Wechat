window.UserTagController = function(){
	this.userTags = [];
	this.maxLength = 10;
	this.manageLink = 'http://kdwechat.companycn.net/fans/user_list.aspx?m_id=17';	//管理标签地址
	
	this.defaultTitle = '修改标签';
	this.domHTML = [
		'<div class="userTags">',
			'<div class="title">',
				'<div class="control">',
					'<input type="button" class="btn close" type="关闭">',
				'</div>',
				'<h2><em>修改标签</em><a href="'+this.manageLink+'" target="_blank" class="link">管理标签</a></h2>',
			'</div>',
			'<div class="list">',
				'<ul>',
				'</ul>',
			'</div>',
			'<div class="btns">',
				'<span class="tip">最多可选'+this.maxLength+'个标签</span>',
				'<input type="button" class="btn btn1" value="确定">',
				'<input type="button" class="btn btn2" value="取消">',
			'</div>',
		'</div>'
	].join('\n');
	this.listSelector = '.list ul';
	this.labelHTML = [
		'<li><label><input type="checkbox" class="checkbox">',
		'</label></li>'
	];
	this.checkedSelector = '.tag';
	this.btnOpenClass = 'btnOpen';
	this.globalClass = 'global';
	this.size = {
		width:0,
		height:0
	};
	this.type = 0;
	this.closeBtnSelector = '.close, .btns .btn2';
	this.submitBtnSelector = '.btns .btn1';
	this.titleSelector = '.title h2 em';
	this.btn = null;
	this.toParent = false;
	this.isShow = false;
	this.area = $('#main');
	this.labels = [];
	this.ids = [];
	this.oldTag = [];
	this.tagHTML = '<span class="tag"></span>'
	this.setup();
}
UserTagController.prototype = {
	setup: function(){
		var _this = this;
		this.dom = $(this.domHTML);
		this.listField = this.dom.find(this.listSelector);
		this.getSize();
		$(window).resize(function(){
			if(_this.isShow)
				_this.checkPos();
		});
	},
	bindEvents: function(){
		var _this = this;
		this.dom.find(this.closeBtnSelector).bind('click',function(){
			_this.close();
		});
		this.dom.find(this.submitBtnSelector).bind('click',function(){
			_this.submit();
		});
		this.dom.bind('click',function(e){
			if(e.stopPropagation){
				e.stopPropagation();
			}else{
				e.cancelBubble = true;
			} 
		})
	},
	unbindEvents: function(){
		this.dom.find(this.closeBtnSelector).unbind('click');
		this.dom.find(this.submitBtnSelector).unbind('click');
		this.dom.unbind('click')
	},
	
	getSize: function(){
		this.dom.appendTo(this.area).css('opacity',0).show();
		this.size = {
			width:this.dom.get(0).offsetWidth,
			height:this.dom.get(0).offsetHeight
		}
		this.dom.remove().removeAttr('style');
	},
	readTag: function(tags){
		this.userTags = tags || this.userTags;
		for(var i in this.labels){
			this.labels[i].remove();
		}
		this.labels = [];
		for(var i in this.userTags){
			var tempLabel = $(this.labelHTML[0]+this.userTags[i]['name']+this.labelHTML[1]);
			tempLabel.attr('labelName',this.userTags[i]['name']).attr('value',this.userTags[i]['id']);
			if(this.userTags[i].global){
				tempLabel.addClass(this.globalClass).attr('title','公共标签');
			}
			this.listField.append(tempLabel);
			this.labels.push(tempLabel);
		}
	},
	open: function(btn,toParent,ids,type,title){
		if(!btn)return false;
		if(this.btn){
			this.btn.removeClass(this.btnOpenClass);
		}
		this.btn = $(btn);
		this.type = type || 0;
		this.title = title || this.defaultTitle;
		this.btn.addClass(this.btnOpenClass);
		this.isShow = true;
		this.toParent = (typeof toParent == 'undefined')?true:!!toParent;
		if(this.toParent){
			this.ids = [this.btn.parents('.userBaseInfo').eq(0).attr('id')];
		}else{
			this.ids = ids;
		}
		this.checkPos();
		this.readChecked();
		this.dom.find(this.titleSelector).html(this.title);
		this.dom.appendTo(this.area).stop().css('opacity',0).show().fadeTo(500,1);
		this.unbindEvents();
		this.bindEvents();
	},
	submit: function(){
		
		var tempChecked = [];
		
		for(var i in this.labels){
			if(this.labels[i].find('input').get(0).checked){
				tempChecked.push({
					id:this.labels[i].attr('value'),
					name:this.labels[i].attr('labelName')
				});
			}
		}
		
		var lengthOverFlag = false;
		this.tags = [];
		for(var i in this.oldTag){
			var tempTags = this._getTags['fn'+this.type].call(this,tempChecked,this.oldTag[i].tags);
			if(tempTags.lengthOver){
				lengthOverFlag = true;
			}
			this.tags.push({
				id:this.oldTag[i].id,
				tags:tempTags.tags
			})
		}
		
		if(tempChecked.length > this.maxLength || lengthOverFlag && this.oldTag.length<=1){
			
			showTip.show('标签不能选择超过'+this.maxLength+'个',true);
		}else{
			this.showResult(this.tags);
			this.tagSubmit(this.tags,this.oldTag,lengthOverFlag);
			this.close();
		}
	},
	_getTags:{
		fn0:function(checked,tags0){
			return {
				tags:checked.concat(),
				lengthOver:false
			};
		},
		fn1:function(checked,tags0){
			var lengthOverFlag = false;
			var tempTags = tags0.concat();
			for(var k in checked){
				var flag = false;
				for(var j in tempTags){
					if(checked[k].id == tempTags[j].id){
						flag = true;
						break;
					}
				}
				if(!flag){
					tempTags.push(checked[k]);
				}
			}
			if(tempTags.length>this.maxLength){
				lengthOverFlag = true;
				tempTags = tempTags.splice(0,this.maxLength);
			}
			return {
				tags:tempTags,
				lengthOver:lengthOverFlag
			}
		},
		fn2:function(checked,tags0){
			var tempTags = [];
			for(var j in tags0){
				var flag = false;
				for(var k in checked){
					if(checked[k].id == tags0[j].id){
						flag = true;
						break;
					}
				}
				if(!flag){
					tempTags.push(tags0[j]);
				}
			}
			return {
				tags:tempTags,
				lengthOver:false
			}
		}
	},
	
	close: function(){
		this.unbindEvents();
		this.btn.removeClass(this.btnOpenClass);
		this.isShow = false;
		this.dom.remove();
	},
	
	tagSubmit: function(){
		
	},
	
	showResult: function(tags){
		for(var i in tags){
			var tagField = $('#'+tags[i].id).find('.tags');
			if(!tagField.length) continue;
			tagField.find(this.checkedSelector).remove();
			
			if(tags[i].tags.length){
				for(var j in tags[i].tags){
					var tempTag = $(this.tagHTML);
					tempTag.html(tags[i].tags[j].name);
					tempTag.attr('name',tags[i].tags[j].id);
					tempTag.insertBefore(tagField.find('.btnAdd'));
				}
				tagField.find('.btnAdd').val('');
			}else{
				tagField.find('.btnAdd').val('添加标签');
			}
		}
		
	},
	
	
	readChecked: function(){
		var _this = this;
		for(var i in this.labels){
			this.labels[i].find('input').get(0).checked = false;
		}
		this.oldTag = [];
		for(var i in this.ids){
			var tagField = $('#'+this.ids[i]).find('.tags');
			if(!tagField.length) continue;
			var tempOldTag = [];
			
			tagField.find(this.checkedSelector).each(function(){
				for(var j in _this.userTags){
					if(_this.userTags[j].id == $(this).attr('name')){
						tempOldTag.push(_this.userTags[j]);
					}
				}
			});
			this.oldTag.push({
				id:this.ids[i],
				tags:tempOldTag	
			});
		}
		
		if(this.toParent){
			this.btn.parent().find(this.checkedSelector).each(function(){
				for(var i in _this.labels){
					if(_this.labels[i].attr('value') == $(this).attr('name')){
						_this.labels[i].find('input').get(0).checked = true;
					}
				}
			});
		}
	},
	checkPos: function(){
		var pos = {
			left:this.btn.offset().left-this.area.offset().left,
			top:this.btn.offset().top-this.area.offset().top+this.btn.get(0).offsetHeight+this.area.get(0).scrollTop - 4
		}
		if(this.toParent){
			var tempLeft1 = this.btn.parent().offset().left-this.area.offset().left - parseFloat(this.btn.parent().css('padding-left'));
			var tempLeft2 = pos.left + this.btn.get(0).offsetWidth - this.size.width;
			pos.left = Math.max(tempLeft1,tempLeft2);
		}
		if(pos.top+this.size.height+10 > this.area.height()){
			pos.top = this.btn.offset().top-this.area.offset().top+this.area.get(0).scrollTop-this.size.height + 3;
		}
		this.dom.css('top',pos.top).css('left',pos.left);
	}
}
var userTagController = new UserTagController();



window.UserTypeController = function(){
	this.userTypes = [];
	this.maxLength = 10;
	this.manageLink = 'http://kdwechat.companycn.net/fans/group_list.aspx?m_id=78';	//管理分组地址
	this.domHTML = [
		'<div class="userTypes">',
			'<div class="title">',
				'<div class="control">',
					'<input type="button" class="btn close" type="关闭">',
				'</div>',
				'<h2>修改分组<a href="'+this.manageLink+'" target="_blank" class="link">管理分组</a></h2>',
			'</div>',
			'<div class="list">',
				'<ul>',
				'</ul>',
			'</div>',
			'<div class="btns">',
				'<input type="button" class="btn btn1" value="确定">',
				'<input type="button" class="btn btn2" value="取消">',
			'</div>',
		'</div>'
	].join('\n');
	this.listSelector = '.list ul';
	this.labelHTML = [
		'<li><label><input type="radio" class="radio" name="userType">',
		'</label></li>'
	];
	this.checkedSelector = '.selected';
	this.btnOpenClass = 'btnOpen';
	this.size = {
		width:0,
		height:0
	};
	this.closeBtnSelector = '.close, .btns .btn2';
	this.submitBtnSelector = '.btns .btn1';
	this.btn = null;
	this.toParent = false;
	this.isShow = false;
	this.area = $('#main');
	this.labels = [];
	this.ids = [];
	this.oldType = [];
	this.typeHTML = '<span class="tag"></span>'
	this.setup();
}
UserTypeController.prototype = {
	setup: function(){
		var _this = this;
		this.dom = $(this.domHTML);
		this.listField = this.dom.find(this.listSelector);
		this.getSize();
		$(window).resize(function(){
			if(_this.isShow)
				_this.checkPos();
		});
	},
	bindEvents: function(){
		var _this = this;
		this.dom.find(this.closeBtnSelector).bind('click',function(){
			_this.close();
		});
		this.dom.find(this.submitBtnSelector).bind('click',function(){
			_this.submit();
		});
		this.dom.bind('click',function(e){
			if(e.stopPropagation){
				e.stopPropagation();
			}else{
				e.cancelBubble = true;
			} 
		})
	},
	unbindEvents: function(){
		this.dom.find(this.closeBtnSelector).unbind('click');
		this.dom.find(this.submitBtnSelector).unbind('click');
		this.dom.unbind('click')
	},
	
	getSize: function(){
		this.dom.appendTo(this.area).css('opacity',0).show();
		this.size = {
			width:this.dom.get(0).offsetWidth,
			height:this.dom.get(0).offsetHeight
		}
		this.dom.remove().removeAttr('style');
	},
	readType: function(types){
		this.userTypes = types || this.userTypes;
		for(var i in this.labels){
			this.labels[i].remove();
		}
		this.labels = [];
		for(var i in this.userTypes){
			var tempLabel = $(this.labelHTML[0]+this.userTypes[i]['name']+this.labelHTML[1]);
			tempLabel.attr('labelName',this.userTypes[i]['name']).attr('value',this.userTypes[i]['id']);
			this.listField.append(tempLabel);
			this.labels.push(tempLabel);
		}
	},
	open: function(btn,toParent,ids){
		if(!btn)return false;
		if(this.btn){
			this.btn.removeClass(this.btnOpenClass);
		}
		this.btn = $(btn);
		this.btn.addClass(this.btnOpenClass);
		this.isShow = true;
		this.toParent = (typeof toParent == 'undefined')?true:!!toParent;
		if(this.toParent){
			this.ids = [this.btn.parents('.userBaseInfo').eq(0).attr('id')];
		}else{
			this.ids = ids;
		}
		this.checkPos();
		this.readChecked();
		this.dom.appendTo(this.area).stop().css('opacity',0).show().fadeTo(500,1);
		this.bindEvents();
	},
	submit: function(){
		var tempChecked = '';
		var tempCheckedValue = ''
		for(var i in this.labels){
			if(this.labels[i].find('input').get(0).checked){
				tempChecked = this.labels[i].attr('labelName');
				tempCheckedValue = this.labels[i].attr('value');
				break;
			}
		}
		if(!tempChecked){
			showTip.show('未选择分组',true);
		}else{
			this.showResult(tempChecked);
			var oldType = this.oldType;
			this.typeSubmit(this.ids,tempChecked,tempCheckedValue,oldType);
			this.close();
		}
	},
	close: function(){
		this.unbindEvents();
		this.btn.removeClass(this.btnOpenClass);
		this.isShow = false;
		this.dom.remove();
	},
	
	typeSubmit: function(){
		
	},
	
	showResult: function(types){
		this.oldType = [];
		for(var i in this.ids){
			var typeField = $('#'+this.ids[i]).find('.group');
			if(!typeField.length) continue;
			var tempOldType = typeField.find(this.checkedSelector).html();
			this.oldType.push(tempOldType);
			typeField.find(this.checkedSelector).html(types);
		}
		
	},
	
	backResult: function(ids,oldType){
		for(var i in ids){
			var typeField = $('#'+ids[i]).find('.group');
			if(!typeField.length) continue;
			typeField.find(this.checkedSelector).html(oldType[i]);
		}
	},
	
	
	readChecked: function(){
		var _this = this;
		for(var i in this.labels){
			this.labels[i].find('input').get(0).checked = false;
		}
		if(this.toParent){
			var nowSelected = '';
			if(this.btn.parent().find(this.checkedSelector).find('span').length){
				nowSelected = this.btn.parent().find(this.checkedSelector).find('span').html();
			}else{
				nowSelected = this.btn.parent().find(this.checkedSelector).html();
			}
			
			for(var i in this.labels){
				
				if(this.labels[i].attr('labelName') == nowSelected){
					this.labels[i].find('input').get(0).checked = true;
					break;
				}
			}
		}
	},
	checkPos: function(){
		var pos = {
			left:this.btn.offset().left-this.area.offset().left,
			top:this.btn.offset().top-this.area.offset().top+this.btn.get(0).offsetHeight+this.area.get(0).scrollTop - 4
		}
		if(this.toParent){
			pos.left = pos.left + this.btn.get(0).offsetWidth - this.size.width;
		}
		if(pos.top+this.size.height+10 > this.area.height()){
			pos.top = this.btn.offset().top-this.area.offset().top+this.area.get(0).scrollTop-this.size.height + 3;
		}
		this.dom.css('top',pos.top).css('left',pos.left);
	}
}
var userTypeController = new UserTypeController();




var listSelector = $('input[name="listSelector"]');
listSelector.change(function(){
	var parent = $(this).parents('.userBaseInfo').eq(0);
	if(this.checked){
		parent.addClass('selected');
	}else{
		parent.removeClass('selected');
	}
});


function listOpen(btn,controller,type){
	var type = type || 0;
	var titles = ['批量修改标签','批量增加标签','批量删除标签']
	var ids = [];
	listSelector.each(function(){
		if(this.checked){
			ids.push(this.value);
		}
	});
	if(ids.length){
		controller.open(btn,false,ids,type,titles[type]);
	}else{
		showTip.show('未选择用户',true);
	}
}








