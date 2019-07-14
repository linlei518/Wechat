var canControl = true;

window.MaterialEditor = function(config){
	this.dom = $(config.dom);
	this.parent = config.parent;
	this.inputer = {
		title: this.dom.find('.jsTitle'),
		type: this.dom.find('.jsType'),
		img: this.dom.find('.jsImage'),
		summary: this.dom.find('.jsSummary'),
		intro: this.dom.find('.jsIntro'),
		link:{
			dom:this.dom.find('.link'),
			content:this.dom.find('.jsLink')
		},
		app:{
			dom:this.dom.find('.app'),
			appDom: this.dom.find('.appShow'),
			title:this.dom.find('.app .appShow .info h2'),
			content:this.dom.find('.app .appShow .info p').eq(0),
			//link:this.dom.find('.app .appShow .info p').eq(1),
			img:this.dom.find('.app .appShow .img img')
		},
		article:{
			dom:this.dom.find('.article'),
			content:this.dom.find('.jsText'),
			origin:this.dom.find('.jsOrigin'),
			template:{
				dom:this.dom.find('.template'),
				title:this.dom.find('.template .info h2'),
				img:this.dom.find('.template .img img')
			}
		}
	},
	
	this.nTab = {
		article:this.dom.find('.nTabBtn').eq(0),
		link:this.dom.find('.nTabBtn').eq(1),
		app:this.dom.find('.nTabBtn').eq(2)
	};
	
	this.defaultPic = '../images/blank.gif';
	this.inputerDom = [];
	this.editor = $('#ueditor_0').contents().find('body');// $('.ke-edit-iframe').contents().find('.ke-content');
	this.imgUploader = this.dom.find('.ajaxFile');
	this.outPut = {};
	this.isStart = false;
	
	this.bindEvents();
}

MaterialEditor.prototype = {
	bindEvents: function(){
		var _this = this;
		this.readInputerDom(this.inputer);
		
		for(var i in this.inputerDom){
			
			var parents = this.inputerDom[i][1];
			var dom = this.inputerDom[i][0];
			dom.attr('parents',parents.join('|'));
			
			if(dom.get(0).tagName.toLowerCase() == 'select'){
				dom.get(0).onchange = function(){
					if(_this.isStart){
						var parents = $(this).attr('parents').split('|');
						var parentData = _this.getParentData(parents);
						parentData[parents[parents.length-1]] = this.value;
						_this.outPutData();
					}
				}
			}else if(dom.get(0).tagName.toLowerCase() == 'input' || dom.get(0).tagName.toLowerCase() == 'textarea'){
				dom.bind('input propertychange',function(){
					if(_this.isStart){
						var parents = $(this).attr('parents').split('|');
						var parentData = _this.getParentData(parents);
						var value = this.value.replace(/\n\r/gi,"<br>").replace(/\r/gi,"<br>").replace(/\n/gi,"<br>");
						parentData[parents[parents.length-1]] = value;
						_this.outPutData();
					}
				});
			}
			if(parents[0]=='img'){
				dom.get(0).onchange = function(){
					if(_this.isStart){
						var parents = $(this).attr('parents').split('|');
						var parentData = _this.getParentData(parents);
						parentData[parents[parents.length-1]] = this.value;
						_this.outPutData();
					}
				}
			}
			if (parents[0] == 'article' && parents[1] == 'content') {
			    dom.get(0).onchange = function () {
					if(_this.isStart){
						var parents = $(this).attr('parents').split('|');
						var parentData = _this.getParentData(parents);
						parentData[parents[parents.length-1]] = $(this).val();
						_this.outPutData();
					}
			    }
			}
		}
	},
	
	getParentData: function(parents){
		var tempData = this.outPut;
		for (var i = 0; i < parents.length - 1; i++) {
			var nowData = tempData[parents[i]];
			if (typeof nowData == 'undefined') {
				tempData[parents[i]] = {};
				nowData = tempData[parents[i]];
			}
			tempData = nowData;
		}
		return tempData;
	},
	
	readInputerDom: function(obj,parents){
		parents = parents || [];
		for(var i in obj){
			var tempParents = parents.concat(i);
			if(obj[i].bind){
				this.inputerDom.push([obj[i],tempParents]);
			}else{
				this.readInputerDom(obj[i],tempParents);
			}
		}
	},
	outPutData: function(){
		this.parent.outPutData(this.outPut);
	},
	inputData: function(data){
		this.outPut = data;
		this.readData();
		this.isStart = true;
	},
	
	readData: function(){
		for(var i in this.inputerDom){
			
			var parents = this.inputerDom[i][1];
			var data = this.getDomData(parents);
			var dom = this.inputerDom[i][0];
			
			if(dom.get(0).tagName.toLowerCase() == 'select'){
				
				if(!!data){
					for(var j =0; j< dom.get(0).options.length; j++){
						if(dom.get(0).options[j].text == data || dom.get(0).options[j].value == data){
							dom.get(0).options[j].selected = true;
							break;
						}
					}
				}else{
					dom.get(0).options[0].selected = true;
				}
				setupSelect();
			}else if(parents[0]=='article' && parents[1]=='content'){
				
			    //this.editor = $('.ke-edit-iframe').contents().find('.ke-content');
			    this.editor = $('#ueditor_0').contents().find('body');
				this.editor.html(data || '');
				dom.val(data || '');
				
			}else if(dom.get(0).tagName.toLowerCase() == 'img'){
				dom.attr('src',data || this.defaultPic);
			}else if(dom.get(0).tagName.toLowerCase() == 'input' || dom.get(0).tagName.toLowerCase() == 'textarea'){
				var value = data || '';
				value = value.replace(/<br>/gi,"\n").replace(/<br[ ]*\/>/gi,"\n");
				dom.val(value);
			}else if(parents[parents.length-1].toLowerCase().indexOf('dom')!=-1){
				dom.hide();
			}else{
				dom.html(data || '');
			}
		}
		
		for(var i in this.nTab){
			this.nTab[i].removeClass('current');
		}
		
		if(this.outPut.pushType){
			this.inputer[this.outPut.pushType].dom.show();
			this.nTab[this.outPut.pushType].addClass('current');
		}else{
			this.inputer.article.dom.show();
			this.nTab.article.addClass('current');
		}
		if(this.outPut.pushType == 'article' && this.outPut.article && this.outPut.article.template){
			this.inputer.article.template.dom.show();
		}else{
			this.inputer.article.template.dom.hide();
		}
		if(this.outPut.pushType == 'app' && this.outPut.app){
			this.inputer.app.appDom.show();
		}else{
			this.inputer.app.appDom.hide();
		}
	},
	
	getDomData: function(parents){
		var tempData = this.outPut;
		for(var i = 0; i<parents.length; i++){
			tempData = tempData[parents[i]];
			if(typeof tempData == 'undefined'){
				tempData = null;
				break;
			}
		}
		return tempData;
	}
}



window.Simulation = function(config){
	this.dom = $(config.dom);
	this.num = config.num;
	this.parent = config.parent;
	
	this.outPuter = {
		title: this.dom.find('.title h1'),
		img: this.dom.find('.img img'),
		summary: this.dom.find('.content')
	};
	this.controllerField = this.dom.find('.btns');
	this.controller = {
		edit:this.controllerField.find('.editBtn'),
		del:this.controllerField.find('.deleteBtn'),
		up:this.controllerField.find('.upBtn'),
		down:this.controllerField.find('.downBtn')
	};
	this.tip = this.dom.find('.tip');
	this.title0 = '请输入标题';
	this.src0 = '../images/blank.gif';
	this.bindEvents();
}
Simulation.prototype = {
	bindEvents:function(){
		var _this = this;
		this.controller.edit.click(function(){
			if(!canControl) return false;
			_this.parent.edit(_this.num);
		});
		this.controller.del.click(function(){
			if(!canControl) return false;
			_this.parent.del(_this.num);
		});
		this.controller.up.click(function(){
			if(!canControl) return false;
			_this.parent.up(_this.num);
		});
		this.controller.down.click(function(){
			if(!canControl) return false;
			_this.parent.down(_this.num);
		});
	},
	show:function(data){
		for(var i in this.outPuter){
			if(this.outPuter[i].length){
				if(this.outPuter[i].get(0).tagName.toLowerCase() == 'img'){
					var flag = !!data[i];
					var tempSrc = data[i] || this.src0;
					this.outPuter[i].attr('src',tempSrc);
					if(flag){
						this.tip.hide();
					}else{
						this.tip.show();
					}
				}else if(i == 'summary'){
					var tempSummary = data[i] || '';
					var contentTexts = tempSummary.split('<br>');
					this.outPuter[i].html('');
					for(var j in contentTexts){
						this.outPuter[i].append($('<p>'+contentTexts[j]+'</p>'));
					}
				}else if(i=='title'){
					var tempTitle = data[i] || this.title0;
					this.outPuter[i].html(tempTitle);
				}
			}
		}
	},
	distory: function(){
		for(var i in this.controller){
			this.controller[i].unbind();
		}
		this.dom.remove();
		for(var i in this){
			delete this[i];
		}
	}
}


window.MaterialAddModule = function(data){
	var _this = this;
	
	this.data = data || [];
	
	this.dom = $('.graphicMaterialModule_01');
	this.simulations = [];
	
	this.onEdited = 0;
	
	this.simulationType = 1;
	this.simulationField = this.dom.find('.simulationPanel_01');
	if(!this.simulationField.length){
		this.simulationType = 2;
		this.simulationField = this.dom.find('.simulationPanel_02');
	}
	if(this.simulationType==1){
		this.data = [this.data[0] || {pushType:'article'}];
	}
	this.simulationField.find('.infoField').each(function(i){
		var tempSimulations = new Simulation({
			dom:this,
			num:parseInt(i),
			parent:_this
		});
		_this.simulations.push(tempSimulations);
	});
	this.materialEditor = new MaterialEditor({
		dom:this.dom.find('.listPanel_02'),
		parent:this
	});
	this.maxLength = 11;
	this.onEditedClass = 'act';
	
	this.addBtn = this.simulationField.find('.addField .addBtn')
	
	this.newSimulationHtml = [
		'<div class="infoField">',
			'<div class="img">',
				'<span>',
					'<img src="../images/blank.gif" alt="">',
				'</span>',
				'<div class="tip">缩略图</div>',
			'</div>',
			'<div class="title">',
				'<h1>请输入标题</h1>',
			'</div>',
			'<div class="btns">',
				'<input type="button" class="btn editBtn" title="编辑">',
				'<input type="button" class="btn deleteBtn" title="删除">',
				'<input type="button" class="btn upBtn" title="向上">',
				'<input type="button" class="btn downBtn" title="向下">',
			'</div>',
		'</div>'
	].join('');
	this.warning = {
		noDel1: '无法删除，多条图文至少需要2条消息！',
		noDel2:'无法删除主要信息！',
		del:'是否要删除该条信息？',
		noUp:'已经到顶了！',
		noDown:'已经到底了！',
		noAdd:'不能再添加信息了！',
		subError:{
			content1:'',
			content2:'第%d篇图文',
			title:'标题不能为空',
			summary:'摘要格式错误',
			img:'封面图片不能为空',
			article:{
				content:'必须请输入正文'
			},
			link:{
				content:'外链未输入或格式错误'
			},
			app:{
				link:'必须选择一条应用'
			},
			intro:'简介格式错误',
			noLength:'请输入第二条图文的数据'
		}
	}
	this.setup();
}
MaterialAddModule.prototype = {
	setup: function(){
		if(this.data){
			
			this.readData();
		}
		var dataLength = this.data.length;
		for (var i = 0; i < this.simulations.length - dataLength; i++) {
			this.data.push({pushType:'article'});
		}
		this.bindEvents();
		this.edit(this.onEdited);
	},
	
	bindEvents: function(){
		var _this = this;
		this.addBtn.click(function(){
			if(_this.data.length == _this.maxLength){
				showTip.show(_this.warning.noAdd,true);
				return false;
			}
			_this.add({pushType:'article'});
		});
	},
	
	readData : function(){
		for(var i=0; i<this.data.length; i++){
			if(this.simulations.length<i+1){
				this.add(this.data[i],false);
			}else{
				this.simulations[i].show(this.data[i]);
				this.simulations[i].num = i;
			}
		}
	},
	add: function(data,add){
		add = (typeof add == 'undefined')?true:!!add;
		var tempSimulationDom = $(this.newSimulationHtml);
		tempSimulationDom.insertAfter(this.simulations[this.simulations.length-1].dom);
		var tempSimulations = new Simulation({
			dom:tempSimulationDom,
			num:this.simulations.length,
			parent:this
		});
		this.simulations.push(tempSimulations);
		
		if(add){
			this.data.push(data);
		}
		tempSimulations.show(data);
	},
	outPutData: function(data){
		this.data[this.onEdited] = data;
		this.simulations[this.onEdited].show(this.data[this.onEdited]);
	},
	
	edit: function(num){
		//以后这里要判断num范围;
		this.onEdited = num;
		for(var i in this.simulations){
			this.simulations[i].dom.removeClass(this.onEditedClass);
		}
		this.simulations[this.onEdited].dom.addClass(this.onEditedClass);
		this.materialEditor.inputData(this.data[this.onEdited]||{});
		this.materialEditor.dom.css('margin-top',this.simulations[this.onEdited].dom.offset().top + $('#main').get(0).scrollTop -190);//待优化
	},
	del: function(num){
		var _this = this;
		if(this.simulations.length<3){
			showTip.show(this.warning.noDel1,true);
			return false;
		}
		if(num == 0){
			showTip.show(this.warning.noDel2,true);
			return false;
		}
		dialogue.dlAlert({
			content:'确定要删除该项吗?',
			btns:[{
				text:'确定',
				fn:function(){
					
					_this.simulations[num].distory();
					_this.simulations.splice(num,1);
					_this.data.splice(num,1);
					_this.readData();
					if(_this.onEdited == num){
						_this.edit(0);
					}
					
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
	up: function(num){
		if(num<1){
			showTip.show(this.warning.noUp,true);
			return false;
		}
		var tempData0 = this.data[num-1];
		this.data[num-1] = this.data[num];
		this.data[num] = tempData0;
		this.readData();
		this.edit(this.onEdited);
	},
	down: function(num){
		if(num>this.data.length-2){
			showTip.show(this.warning.noDown,true);
			return false;
		}
		var tempData0 = this.data[num+1];
		this.data[num+1] = this.data[num];
		this.data[num] = tempData0;
		this.readData();
		this.edit(this.onEdited);
	},
	change: function(data){
	    data = data || {};
		if(this.data[this.onEdited].id){
			data.id = this.data[this.onEdited].id
		}else if(data.id){
			delete data['id'];
		}
		this.data[this.onEdited] = data;
		this.readData();
		this.edit(this.onEdited);
	},
	selectTemplate: function(templateData){
		if(!this.data[this.onEdited].article){
			this.data[this.onEdited].article = {};
		}
		this.data[this.onEdited].article.template = templateData;
		this.readData();
		this.edit(this.onEdited);
	},
	selectApp: function(appData){
		this.data[this.onEdited].app = appData;
		this.readData();
		this.edit(this.onEdited);
	},
	selectType: function(pushType){
		this.data[this.onEdited].pushType = pushType;
		this.readData();
		this.edit(this.onEdited);
	},
	checkError: function(){
		var flag = true;
		var alamText = '';
		var checkProject = {
			article:['title','img','article_content'],
			link:['title','img',{
				text:'link_content',
				fn:function(content){
					var flag = !!content;
					if(content && content.indexOf('http://')!=0){
						flag = false;
					}
					return flag;
				}
			}],
			app:['title','img','app_link']
		};
		var errorNum = 0;
		var errorInfo = checkProject[0];
		for (var i = 0; i < this.data.length; i++) {
			for(var j in checkProject[this.data[i].pushType]){
				var checkProjectData = checkProject[this.data[i].pushType][j];
				if(typeof checkProjectData == 'string'){
					checkProjectData = {text:[checkProjectData]};
				}
				
				var key = checkProjectData.text;
				if(!(key instanceof Array)){
					key = [key];
				}
				var or = checkProjectData.or || false;
				var flagGroup = [];
				
				var fn = checkProjectData.fn || function(content){
					return !!content;
				}
				
				var tempFlag = false;
				
				for(var k in key){
					var keyGroup = key[k];
					if(keyGroup.indexOf('_')!=-1){
						keyGroup = keyGroup.split('_');
					}else{
						keyGroup = [keyGroup];
					}
					var tempData = this.data[i];
					for(var m in keyGroup){
						tempData = tempData[keyGroup[m]];
						if(typeof tempData == 'undefined'){
							break;
						}
					}
					if(!tempData || !fn($.trim(tempData))){
						if(!or){
							flag = false;
							break;
						}
					}else{
						tempFlag = true;
					}
				}
				if(!tempFlag){
					flag = false;
				}
				
				var warningKey = checkProjectData.warning || key[0];
				if(warningKey.indexOf('_')!=-1){
					warningKey = warningKey.split('_');
				}else{
					warningKey = [warningKey];
				}
				var warning = this.warning.subError;
				for(var n in warningKey){
					warning = warning[warningKey[n]];
				}
				var inputer = this.materialEditor.inputer;
				for(var l in warningKey){
					inputer = inputer[warningKey[l]];
				}
				
				
				if (!flag){
					alamText = this.warning.subError['content'+this.simulationType].replace('%d',i+1) + warning;
					flag = false;
					errorNum = i;
					errorInfo = inputer;
					break;
				}
			}
			if(!flag)break;
		}
		if(flag && this.data.length < 2 && this.simulationType == 2){
			alamText = this.warning.subError.noLength;
			errorNum = 1;
			flag = false;
		}
		
		if(!flag){
			showTip.show(alamText,true);
			this.edit(errorNum);
			if(errorInfo){
				errorInfo.get(0).focus();
			}
		}
		return flag;
	}
}

