window.StatisticsContrast = function(){
	this.accounts = [];
	this.maxLength = 10;
	this.manageLink = '';	//管理分组地址
	this.domHTML = [
		'<div class="statisticsContrast">',
			'<div class="title">',
				'<div class="control">',
					'<input type="button" class="btn close" type="关闭">',
				'</div>',
				'<h2>数据对比<span>最多对比3组</span></h2>',
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
		'<li><label><input type="checkbox" class="checkbox" name="account">',
		'</label></li>'
	];
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
	this.setup();
}
StatisticsContrast.prototype = {
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
	readAccount: function(accounts){
		this.accounts = accounts || this.accounts;
		for(var i in this.labels){
			this.labels[i].remove();
		}
		this.labels = [];
		for(var i in this.accounts){
			var tempLabel = $(this.labelHTML[0]+this.accounts[i]['name']+this.labelHTML[1]);
			tempLabel.attr('labelName',this.accounts[i]['name']).attr('value',this.accounts[i]['id']);
			this.listField.append(tempLabel);
			this.labels.push(tempLabel);
		}
	},
	open: function(btn,checked){
		if(!btn)return false;
		this.checked = checked || false;
		if(typeof this.checked == 'string'){
			this.checked = [this.checked];
		}
		this.btn = $(btn);
		this.isShow = true;
		this.checkPos();
		this.readChecked();
		this.dom.appendTo(this.area).stop().css('opacity',0).show().fadeTo(500,1);
		this.bindEvents();
	},
	submit: function(){
		var submitValue = [];
		for(var i in this.labels){
			if(this.labels[i].find('input').get(0).checked){
				submitValue.push({
					id:this.labels[i].attr('value'),
					name:this.labels[i].attr('labelName')
				});
			}
		}
		if(submitValue.length>3){
			showTip.show('对比条目不能超过3条',true);
		}else{
			this.accountSubmit(submitValue);
			this.close();
		}
		
	},
	close: function(){
		this.unbindEvents();
		this.isShow = false;
		this.dom.remove();
	},
	
	accountSubmit: function(){
		
	},
	
	
	readChecked: function(){
		var _this = this;
		for(var i in this.labels){
			this.labels[i].find('input').get(0).checked = false;
		}
		if(this.checked){
			for(var i in this.labels){
				for(var j in this.checked){
					if(this.labels[i].attr('value') == this.checked[j]){
						this.labels[i].find('input').get(0).checked = true;
						break;
					}
				}
			}
		}
	},
	checkPos: function(){
		var pos = {
			left:this.btn.offset().left-this.area.offset().left+this.btn.get(0).offsetWidth-this.size.width,
			top:this.btn.offset().top-this.area.offset().top+this.btn.get(0).offsetHeight+this.area.get(0).scrollTop - 4
		}
		//if(pos.top+this.size.height+10 > this.area.height()){
		//	pos.top = this.btn.offset().top-this.area.offset().top+this.area.get(0).scrollTop-this.size.height + 3;
		//}
		this.dom.css('top',pos.top).css('left',pos.left);
	}
}
var statisticsContrast = new StatisticsContrast();
