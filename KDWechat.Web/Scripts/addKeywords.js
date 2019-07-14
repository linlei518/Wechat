window.AddKeywords = function(config){
	if(!config) return false;
	this.inputer = config.inputer
	this.hideDom = config.hideDom || false;
	this.disabled = config.disabled || false;
	this.canSort = config.canSort || false;
	this.hasId = config.hasId || false;
	this.disableClass = 'disabled'
	this.data = [];
	this.editList = [];
	
	
	this.editListAreaHtml = '<ul class="editList"></ul>';
	this.editListHTML = [
		'<li>',
			'<div class="show">',
				'<span class="text"></span>',
				'<input type="button" class="btn up" title="向上">',
				'<input type="button" class="btn down" title="向下">',
				'<input type="button" class="btn edit" title="编辑">',
				'<input type="button" class="btn delete" title="删除">',
			'</div>',
			'<div class="editor">',
				'<input type="text" class="txt">',
				'<input type="button" class="btn btn1 ok" value="确定">',
				'<input type="button" class="btn btn2 cancel" value="取消">',
			'</div>',
		'</li>'
	].join('');
	this.editListSelector = {
		txt:'.show .text',
		upBtn:'.show .up',
		downBtn:'.show .down',
		editBtn:'.show .edit',
		deleteBtn:'.show .delete',
		inputTxt:'.editor .txt',
		inputBtn:'.editor .ok',
		cancelBtn:'.editor .cancel',
		btns:'.btn'
	}
	this.editClass = 'editing';
	//this.editListHTML = '<a href="#" class="btn cancelBubble" title="点击取消"></a>';
	this.setup();
}
AddKeywords.prototype = {
	setup: function(){
		this.editListArea = $(this.editListAreaHtml);
		this.editListArea.appendTo(this.inputer.show);
		this.readInputer();
		this.bindEvents();
		if(this.disabled){
			this.disableAll();
		}
	},
	addList: function(data,num){
		var text = data;
		if(this.hasId){
			text = data.split('^')[0];
		}
		var tempList = $(this.editListHTML);
		var _this = this;
		
		tempList.attr('nData',data);
		tempList.attr('num',num);
		tempList.find(this.editListSelector.txt).html(text);
		
		if(!this.canSort){
			tempList.find(this.editListSelector.upBtn).hide();
			tempList.find(this.editListSelector.downBtn).hide();
		}
		
		this.editListArea.append(tempList);
		
		tempList.find(this.editListSelector.upBtn).click(function(e){
			if(_this.disabled)return false;
			e.preventDefault();
			_this.upList(tempList);
		});
		
		tempList.find(this.editListSelector.downBtn).click(function(e){
			if(_this.disabled)return false;
			e.preventDefault();
			_this.downList(tempList);
		});
		
		tempList.find(this.editListSelector.editBtn).click(function(e){
			if(_this.disabled)return false;
			e.preventDefault();
			_this.editingList(tempList);
		});
		
		tempList.find(this.editListSelector.deleteBtn).click(function(e){
			if(_this.disabled)return false;
			e.preventDefault();
			_this.removeList(tempList);
		});
		
		tempList.find(this.editListSelector.inputBtn).click(function(e){
			if(_this.disabled)return false;
			e.preventDefault();
			_this.editListOk(tempList);
		});
		
		tempList.find(this.editListSelector.cancelBtn).click(function(e){
			if(_this.disabled)return false;
			e.preventDefault();
			_this.editListCancel(tempList);
		});
		
		this.editList.push(tempList);
	},
	
	upList: function(obj){
		var editNum = parseInt(obj.attr('num'),10);
		if(editNum<1){
			showTip.show('已经到顶了!',true);
			return false;
		}
		var tempData0 = this.data[editNum-1];
		this.data[editNum-1] = this.data[editNum];
		this.data[editNum] = tempData0;
		this.readData();
	},
	
	downList: function(obj){
		var editNum = parseInt(obj.attr('num'),10);
		if(editNum>this.data.length-2){
			showTip.show('已经到底了!',true);
			return false;
		}
		var tempData0 = this.data[editNum+1];
		this.data[editNum+1] = this.data[editNum];
		this.data[editNum] = tempData0;
		this.readData();
	},
	
	editingList: function(obj){
		for(var i in this.editList){
			this.editList[i].removeClass(this.editClass);
		}
		var text = obj.attr('nData');
		if(this.hasId){
			text = text.split('^')[0];
		}
		obj.addClass(this.editClass).find(this.editListSelector.inputTxt).val(text);
		console.log(this.editListSelector.inputTxt);
	},
	editListOk: function(obj){
		var editNum = parseInt(obj.attr('num'),10);
		var data = obj.find(this.editListSelector.inputTxt).val();
		
		var flag = this.checkResult(data,editNum);
		
		if(flag){
			if(this.hasId){
				var id = obj.attr('nData').split('^')[1];
				data += '^'+id;
			}
			this.data[editNum] = data;
			this.readData();
		}else{
			return flag;
		}
	},
	
	editListCancel: function(obj){
		obj.removeClass(this.editClass);
	},
	
	removeList: function(obj){
		var _this = this;
		dialogue.dlAlert({
			content:'确定要删除该项吗?',
			btns:[{
				text:'确定',
				fn:function(){
					var editNum = parseInt(obj.attr('num'),10);
					_this.data.splice(editNum,1);
					_this.readData();
					
					dialogue.closeAll();
				}
			},{
				text:'取消',
				fn:function(){
					dialogue.closeAll();
				}
			}]
		});
	},
	readInputer: function(){
		if(this.inputer.save.val()){
			this.data = this.inputer.save.val().split('|');
		}
		this.readData();
	},
	readData: function(){
		for(var i in this.editList){
			this.editList[i].find(this.editListSelector.btns).unbind()
			this.editList[i].remove();
		}
		for(var i=0; i<this.data.length; i++){
			this.addList(this.data[i],i);
		}
		var tempData = this.data.join('|');
		this.inputer.save.val(tempData);
		if(this.hideDom){
			if(!this.data.length){
				this.hideDom.hide();
			}else{
				this.hideDom.show();
			}
		}
	},
	
	disableAll: function(){
		this.disabled = true;
		this.inputer.btn.get(0).disabled = true;
		this.inputer.text.get(0).disabled = true;
		for(var i in this.editList){
			this.editList[i].addClass(this.disableClass);
		}
	},
	
	enableAll: function(){
		this.disabled = false;
		this.inputer.btn.get(0).disabled = false;
		this.inputer.text.get(0).disabled = false;
		for(var i in this.editList){
			this.editList[i].removeClass(this.disableClass);
		}
	},
	
	bindEvents: function(){
		var _this = this;
		this.inputer.btn.click(function(){
			var val = _this.inputer.text.val().replace(/ /g,'');
			var flag = _this.checkResult(val);
			
			if(flag){
				val += (_this.hasId)?'^':'';
				_this.data.push(val);
				_this.readData();
				_this.inputer.text.val('');
			}else{
				return flag;
			}
			
		});
	},
	
	checkResult: function(val,num){
		num = num || -1
		var flag = true;
		if(!val){
			showTip.show('内容不能为空',true);
			flag = false;
		}else if(val.indexOf('|')!=-1 || val.indexOf('^')!=-1 ){
			showTip.show('内容格式不正确',true);
			flag = false;
		}else{
			for(var i in this.data){
				if(num!=i){
					var tempData = this.data[i];
					if(this.hasId){
						tempData = tempData.split('^')[0];
					}
					if(tempData == val){
						showTip.show('内容不能重复',true);
						flag = false;
						break;
					}
				}
			}
		}
		return flag;
	}
}