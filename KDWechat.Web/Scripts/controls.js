

var selectAreaList = [];

window.SelectArea = function(selectDom){
	if(!selectDom)return false;
	this.selectDom = $(selectDom);
	this.selectedIndex = this.selectDom.get(0).selectedIndex;
	
	this.selectOptions = [];
	var length = this.selectDom.get(0).options.length;
	for(var i =0; i<length; i++){
		this.selectOptions.push(this.selectDom.get(0).options[i]);
	}
	
	this.dom = $('<div class="selectAera"></div>');
	this.selectedField = $('<div class="selected"></div>');
	this.optionsField = $('<ul class="options"></ul>');
	this.optionsHTML = '<li></li>';
	this.selectedClass = 'selectedOption';
	this.optionItem = [];
	
	this.isOpen = false;
	
	this.width = 0;
	
	this.disabled = !!this.selectDom.get(0).disabled;
	this.disabledClass = 'disabled';
	
	this.setup();
}

SelectArea.prototype = {
	setup: function(){
		selectAreaList.push(this);
		this.dom.insertAfter(this.selectDom).append(this.selectedField).append(this.optionsField);
		this.selectDom.get(0).selectArea = this;
		this.readDom();
		this.bindEvents();
	},
	readDom: function(){
		var _this = this;
		
		if(this.disabled){
			this.dom.addClass(this.disabledClass);
		}else{
			this.dom.removeClass(this.disabledClass);
		}
		this.dom.removeAttr('style');
		this.width = this.dom.width();
		
		this.selectedField.html(this.selectOptions[this.selectedIndex] && this.selectOptions[this.selectedIndex].text || '');
		if(this.selectOptions[this.selectedIndex] && this.selectOptions[this.selectedIndex].disabled){
			this.selectedField.addClass(this.disabledClass);
		}
		this.checkWidth();
		
		for(var i in this.optionItem){
			this.optionItem[i].unbind().remove();
		}
		
		for(var i in this.selectOptions){
			var tempOption = $(this.optionsHTML);
			tempOption.html(this.selectOptions[i].text);
			tempOption.appendTo(this.optionsField);
			tempOption.attr('num',i);
			if(i == this.selectedIndex){
				tempOption.addClass(this.selectedClass);
			}
			if(this.selectOptions[i].disabled){
				tempOption.attr('disabled', true).addClass(this.disabledClass);
			}
			this.optionItem.push(tempOption);
			tempOption.bind('click',function(e){
				if(_this.disabled)return false;
				if($(this).attr('disabled'))return false;
				if(e.stopPropagation) {
					e.stopPropagation();  
				} else {  
					e.cancelBubble = true;
				} 
				_this.selectChange($(this).attr('num'));
			});
		}
	},
	bindEvents:function(){
		var _this = this;
		this.selectedField.bind('click',function(e){
			if(_this.disabled)return false;
			if(e.stopPropagation) {
				e.stopPropagation();  
			} else {  
				e.cancelBubble = true;
			} 
			if(!this.isOpen){
				_this.openField();
			}else{
				_this.closeField();
			}
		});
	},
	checkWidth: function(){
		if(!this.width)this.width = this.dom.width();
		var hiddenParents = [];
		this.selectedField.parents(":hidden").each(function(){
			if($(this).css('display')=='none'){
				hiddenParents.push({
					node:$(this),
					style:$(this).attr('style')||''
				});
				$(this).show();
			}
		});
		var tempSelectedField = this.selectedField.clone();
		this.dom.append(tempSelectedField);
		tempSelectedField.css('position','fixed').css('left','0').css('top','0');
		var tempWidth = tempSelectedField.get(0).offsetWidth;
		tempSelectedField.remove();
		for(var i in hiddenParents){
			hiddenParents[i].node.attr('style',hiddenParents[i].style);
		}
		
		if(tempWidth>this.width){
			this.dom.css('width',tempWidth);
		}else{
			this.dom.removeAttr('style');
		}
	},
	selectChange: function(num){
		num = parseInt(num);
		this.selectedField.html(this.optionItem[num].html());
		if(this.optionItem[num].attr('disabled')){
			this.selectedField.addClass(this.disabledClass);
		}else{
			this.selectedField.removeClass(this.disabledClass);
		}
		for(var i in this.optionItem){
			this.optionItem[i].removeClass('selectedOption');
		}
		this.selectedIndex = num;
		this.optionItem[num].addClass('selectedOption');
		this.checkWidth();
		this.selectDom.get(0).options[num].selected = true;
		this.closeField();
		try{
			this.selectDom.get(0).onchange();
		}catch(e){}
	},
	openField: function(){
		for(var i in selectAreaList){
			selectAreaList[i].closeField();
		}
		this.isOpen = true;
		this.dom.css('z-index',20);
		this.optionsField.stop().css('opacity',0).show().fadeTo(500,1);
	},
	closeField: function(){
		this.isOpen = false;
		this.dom.css('z-index','');
		this.optionsField.stop().fadeOut(500);
	},
	
	disableAll: function(){
		this.disabled = true;
		this.dom.addClass(this.disabledClass);
		this.selectDom.get(0).disabled = true;
	},
	
	enableAll: function(){
		this.disabled = false;
		this.dom.removeClass(this.disabledClass);
		this.selectDom.get(0).disabled = false;
	},
	distory: function(){
		for(var i in this.optionItem){
			this.optionItem[i].unbind().remove();
		}
		this.selectedField.unbind().remove();
		this.dom.remove();
		
		this.selectDom.get(0).selectArea = null;
		for(var i in selectAreaList){
			if(selectAreaList[i] == this){
				selectAreaList.splice(i,1);
				break;
			}
		}
		for(var i in this){
			delete this[i];
		}
	}
}

function setupSelect(){
	var tempSelectAreaList = selectAreaList.slice();
	for(var i in tempSelectAreaList){
		tempSelectAreaList[i].distory();
	}
	var selectItems = $('select.select');
	selectItems.each(function(i){
		var tempSelect = new SelectArea(this);
	});
	$('body').unbind().bind('click',function(){
		for(var i in selectAreaList){
			selectAreaList[i].closeField();
		}
	});
}
setupSelect();

function resetSelect(){
	for(var i in selectAreaList){
		selectAreaList[i].readDom();
	}
}

function addSelect(dom){
	var tempSelect = new SelectArea(dom);
}








var fileAreaList = [];

window.FileArea = function(fileDom){
	if(!fileDom)return false;
	this.fileDom = $(fileDom);
	this.fileText = this.fileDom.attr('title') || '浏览...';
	this.dom = $('<div class="fileAera"></div>');
	this.textField = $('<input type="text" class="txt" />');
	this.btnField = $('<input type="button" class="btn btn6" value="'+this.fileText+'" />');
	
	
	this.disabled = !!this.fileDom.get(0).disabled;
	this.disabledClass = 'disabled';
	
	this.setup();
}
FileArea.prototype = {
	setup: function(){
		fileAreaList.push(this);
		this.dom.insertAfter(this.fileDom).append(this.textField).append(this.btnField);
		this.fileDom.get(0).FileArea = this;
		this.textField.val(this.fileDom.val());
		
		if(this.disabled){
			this.dom.addClass(this.disabledClass);
			this.textField.addClass(this.disabledClass).get(0).disabled = true;
			this.btnField.addClass(this.disabledClass).get(0).disabled = true;
		}else{
			this.dom.removeClass('disabled');
			this.textField.removeClass(this.disabledClass).get(0).disabled = false;
			this.btnField.removeClass(this.disabledClass).get(0).disabled = false;
		}
		
		this.bindEvents();
	},
	bindEvents: function(){
		var _this = this;
		this.fileDom.change(function(){
			_this.textField.val($(this).val());
		});
		this.textField.click(function(){
			if(_this.disabled) return false;
			_this.fileDom.click();
		});
		this.btnField.click(function(){
			if(_this.disabled) return false;
			_this.fileDom.click();
		});
	},
	distory: function(){
		this.fileDom.unbind();
		this.textField.unbind().remove();
		this.btnField.unbind().remove();
		
		this.dom.remove();
		this.fileDom.get(0).FileArea = null;
		for(var i in fileAreaList){
			if(fileAreaList[i] == this){
				fileAreaList.splice(i,1);
				break;
			}
		}
		for(var i in this){
			delete this[i];
		}
	}
}

function setupFile(){
	var tempFileAreaList = fileAreaList.slice();
	for(var i in tempFileAreaList){
		tempFileAreaList[i].distory();
	}
	var fileItems = $('input.file');
	fileItems.each(function(i){
		var tempFile = new FileArea(this);
	});
}
setupFile();





var tableList = [];
window.Table = function(tableDom){
	if(!tableDom) return false;
	this.tableDom = $(tableDom);
	if(this.tableDom.get(0).table&&this.tableDom.get(0).table!=null){
		this.tableDom.get(0).table.distory();
	}
	this.oddClass = 'odd';
	this.warning = {
		cantUp:'不能再向上了！',
		cantDown:'不能再向下了！'
	};
	this.pageNumDomHtml = [
		'<div class="pageNum">',
			'<div class="info">共有<em></em>页，<em></em>条信息</div>',
			'<a href="#" class="prev">‹</a>',
			'<a href="#" class="next">›</a>',
		'</div>'
	].join('');
	
	this.pageNumHtml = '<a href="#"></a>';
	this.pageNumShowHtml = '<span></span>';
	this.pageNums = [];
	this.stateGroup = {};
	this.setup();
}
Table.prototype = {
	setup: function(){
		this.getState();
		tableList.push(this);
		this.tableDom.get(0).table = this;
		this.getTrs();
		this.setOdd();
		this.setPageNum();
		this.bindEvents();
	},
	
	getState: function(){
		this.state = this.tableDom.attr('title') || this.tableDom.attr('state') || '';
		if(!this.state) return false;
		this.tableDom.attr('state',this.state);
		this.tableDom.attr('title','');
		
		var tempGroup = [];
		
		if(this.state.indexOf(',')!=-1){
			tempGroup = this.state.split(',');
		}else{
			tempGroup = [this.state];
		}
		var tempState = {};
		for(var i in tempGroup){
			var tempData = tempGroup[i].split(':');
			tempState[tempData[0]] = parseInt(tempData[1]);
		}
		this.stateGroup = tempState;
	},
	
	
	setPageNum: function(){
		
		if(!this.stateGroup.showLength) return false;
		var pages = Math.ceil(this.trs.length/this.stateGroup.showLength);
		if(pages<=1) return false;
		
		var nowPage = Math.floor((this.stateGroup.from || 0)/this.stateGroup.showLength);
		
		this.pageNumDom = $(this.pageNumDomHtml);
		this.pageNumDom.find('em').eq(0).html(pages);
		this.pageNumDom.find('em').eq(1).html(this.trs.length);
		this.pageNumDom.insertAfter(this.tableDom);
		
		if(pages<7){
			for(var i=0; i<pages; i++){
				var tempPageNum = $(this.pageNumHtml);
				tempPageNum.html(i+1).insertBefore(this.pageNumDom.find('.next'));
				tempPageNum.attr('num',i);
				this.pageNums.push(tempPageNum);
				if(i == nowPage){
					tempPageNum.addClass('current');
				}
			}
		}else{
			this.pageNumShow = $(this.pageNumShowHtml);
			this.pageNumShow.html((nowPage+1)+'/'+pages).insertBefore(this.pageNumDom.find('.next'));
		}
	},
	
	bindEvents: function(){
		var _this = this;
		this.trs.each(function(){
			var obj = this;
			$(this).find('.btnUp').click(function(){
				_this.up(obj.num);
			});
			$(this).find('.btnDown').click(function(){
				_this.down(obj.num);
			});
		});
		this.tableDom.find('.sorts').click(function(){
			_this.sortting(this);
		});
		
		if(this.pageNumDom){
			this.pageNumDom.find('.next').click(function(e){
				e.preventDefault();
				var num = _this.stateGroup.from + _this.stateGroup.showLength;
				if(num>=_this.trs.length) return false;
				_this.getPage(Math.ceil(num/_this.stateGroup.showLength));
			});
			this.pageNumDom.find('.prev').click(function(e){
				e.preventDefault();
				var num = _this.stateGroup.from - _this.stateGroup.showLength;
				if(num<0) num = 0;
				_this.getPage(Math.ceil(num/_this.stateGroup.showLength));
			});
			for(var i in this.pageNums){
				this.pageNums[i].click(function(e){
					e.preventDefault();
					var num = parseInt($(this).attr('num'));
					_this.getPage(num);
				});
			}
		}
	},
	unbindEvents: function(){
		this.tableDom.find('.btnUp').unbind();
		this.tableDom.find('.btnDown').unbind();
		this.tableDom.find('.sorts').unbind();
		if(this.pageNumDom){
			this.pageNumDom.find('.next').unbind();
			this.pageNumDom.find('.prev').unbind();
			for(var i in this.pageNums){
				this.pageNums[i].unbind();
			}
		}
	},
	sortting: function(th){
		var index = $(th).parent().children().index(th);
		var isUp = $(th).hasClass('sortUp');
		$(th).parent().children().removeClass('sortUp').removeClass('sortDown');
		if(isUp){
			$(th).removeClass('sortUp').addClass('sortDown');
		}else{
			$(th).removeClass('sortDown').addClass('sortUp');
		}
		var trGroup = [];
		this.trs.each(function(){
			var content = $(this).children().eq(index).html();
			trGroup.push({
				content:content,
				dom:this
			});
		});
		trGroup.sort(function(a,b){
			var flag = 1;
			var content1 = a.content;
			var content2 = b.content;
			if(content1.indexOf('-')<=0 && content2.indexOf('-')<=0){
				content1 = parseFloat(content1);
				content2 = parseFloat(content2);
			}
			if(content1>content2){
				flag = -1;
			}
			if(isUp){
				flag = flag*-1;
			}
			return flag;
		});
		for(var i in trGroup){
			$(trGroup[i].dom).appendTo($(trGroup[i].dom).parent());
		}
		this.getTrs();
		this.setOdd();
		if(this.pageNumDom){
			this.getPage(0);
		}
	},
	
	setOdd: function(){
		this.trs.removeClass(this.oddClass);
		for(var i=0; i<this.trs.length; i+=1){
			var from = this.stateGroup.from || 0;
			var showLength = this.stateGroup.showLength || 0;
			if(showLength && (i-from>=showLength || i<from)){
				this.trs.eq(i).hide();
			}else{
				this.trs.eq(i).show();
			}
			if((i-from)%2 == 0){
				this.trs.eq(i).addClass(this.oddClass);
			}
		}
	},
	up: function(num){
		if(num == 0){
			showTip.show(this.warning.cantUp,true);
		}else{
			this.trs.eq(num).insertBefore(this.trs.eq(num-1));
			this.getTrs();
			this.setOdd();
		}
	},
	down: function(num){
		if(num == this.trs.length-1){
			showTip.show(this.warning.cantDown,true);
		}else{
			this.trs.eq(num).insertAfter(this.trs.eq(num+1));
			this.getTrs();
			this.setOdd();
		}
	},
	
	getPage: function(page){
		if(!this.stateGroup.showLength)return false;
		var showLength = this.stateGroup.showLength;
		this.stateGroup.from = page*showLength;
		for(var i in this.pageNums){
			if(parseInt(this.pageNums[i].attr('num'),10) == page){
				this.pageNums[i].addClass('current');
			}else{
				this.pageNums[i].removeClass('current');
			}
		}
		if(this.pageNumShow){
			this.pageNumShow.html((page+1)+'/'+Math.ceil(this.trs.length/this.stateGroup.showLength));
		}
		this.getTrs();
		this.setOdd();
	},
	
	getTrs: function(){
		this.trs = this.tableDom.find('tr').filter(function(){return $(this).find('th').length==0;});
		this.trs.each(function(i){
			this.num = i;
		})
	},
	deleteTr: function(num){
		this.unbindEvents();
		this.trs.eq(num).remove();
		this.getTrs();
		this.setOdd();
		this.bindEvents();
	},
	addTr: function(num,code){
		this.unbindEvents();
		var newTr = $('<tr></tr>');
		if(typeof code == 'string'){
			newTr.html(code);
		}else{
			$(code).children().each(function(){
				$(this).appendTo(newTr);
			});
		}
		if(num>=this.trs.length){
			newTr.insertAfter(this.tableDom.find('tr').eq(this.tableDom.find('tr').length-1));
		}else{
			newTr.insertBefore(this.trs.eq(num));
		}
		this.getTrs();
		this.setOdd();
		this.bindEvents();
	},
	distory: function(){
		this.unbindEvents();
		if(this.pageNumDom){
			this.pageNumDom.remove();
		}
		for(var i in tableList){
			if(tableList[i] == this){
				tableList.splice(i,1);
				break;
			}
		}
		this.tableDom.get(0).table = null;
		
		for(var i in this){
			delete this[i];
		}
	}
	
}
function setupTable(){
	for(var i in tableList){
		tableList[i].distory();
	}
	var tables = $('.table');
	tables.each(function(){
		var tempTable = new Table(this);
	})
}
setupTable();



window.Nav = function(navCurrent){
	this.navField = $('#mainNav');
	this.navs = $('#mainNav li');
	this.navCurrent = '';
	this.navShowGroup = [];
	this.navHideGroup = [];
	this.currentId = '';
	try{
		this.navCurrent = navCurrent
	}catch(e){};
	this.setup();
}

Nav.prototype = {
	setup: function(){
		this.bindEvents();
		this.changeCurrentNav();
	},
	bindEvents: function(){
		var _this = this;
		this.navField.find('ul').show().css('height','auto');
		this.navs.each(function(){
			if($(this).children('ul').length){
				$(this).children('ul').find('ul').hide();
				$(this).attr('jqHeight',$(this).children('ul').height());
				$(this).attr('isShow','hide');
				$(this).children('ul').find('ul').show();
				$(this).children('a').click(function(e){
					if(e.stopPropagation) {
						e.stopPropagation();  
					} else {  
						e.cancelBubble = true;
					}
					_this.openNav($(this).parent().attr('id'));
				});
			}
		});
		this.navField.find('ul').removeAttr('style');
	},
	openNav: function(id){
		var _this = this;
		this.navShowGroup = [];
		this.navHideGroup = [];
		
		var flag = true;
		this.navs.each(function(){
			if(this.id == id && $(this).attr('isShow') == 'show' && this.className.indexOf('current')==-1){
				$(this).find('li').each(function(){
					if($(this).attr('isShow') == 'show'){
						_this.navHideGroup.push(this);
					}
				});
				_this.navHideGroup.push(this);
				flag = false;
			}
		});
		
		if(flag){
			this.navs.each(function(){
				if($(this).find('li').length){
					var flag1 = false;
					$(this).find('li').each(function(){
						if(this.id == id) flag1 = true;
					});
				}
				if(this.id == id) flag1 = true;
				
				var flag2 = $(this).attr('isShow') == 'show';
				
				if(flag1 && !flag2 && !!$(this).attr('isShow')){
					_this.navShowGroup.push(this);
				}
				if(!flag1 && flag2 && this.className.indexOf('current')==-1){
					_this.navHideGroup.push(this);
				}
			});
		}
		
		for(var i in this.navShowGroup){
			$(this.navShowGroup[i]).attr('isShow','show');
			if($(this.navShowGroup[i]).children('ul').css('display')=='none'){
				$(this.navShowGroup[i]).children('ul').show();
			}
			$(this.navShowGroup[i]).children('ul').stop().animate({height:$(this.navShowGroup[i]).attr('jqHeight')},500,function(){
				$(this).css('height','auto');
			});
		}
		
		for(var i in this.navHideGroup){
			$(this.navHideGroup[i]).attr('isShow','hide');
			$(this.navHideGroup[i]).children('ul').stop().animate({height:0},500,function(){
				$(this).hide();
			});
		}
	},
	
	changeCurrentNav: function(){
		if(!this.navCurrent) return false;
		change(this.navCurrent);
	},
	change: function(id){
		this.navCurrent = id;
		var _this = this;
		
		this.navs.each(function(){
			if($(this).find('li').length){
				var flag = false;
				$(this).find('li').each(function(){
					if(this.id == _this.navCurrent) flag = true;
				});
			}
			if(this.id == _this.navCurrent) flag = true;
			if(flag){
				$(this).addClass('current');
				$(this).attr('isShow','show');
			}
		});
	}
}
var nav = new Nav();

window.NTab = function(nTabDom){
	if(!nTabDom) return false;
	this.nTabDom = $(nTabDom);
	this.nTabs = this.nTabDom.find('a');
	this.nTabTipHtml = '<em></em>';
	this.nTabTipTextHtml = '<span></span>';
	this.setup();
}
NTab.prototype = {
	setup: function(){
		var _this = this;
		this.nTabs.each(function(){
			var title = this.title;
			var tempTip = $(_this.nTabTipHtml);
			var tempText = $(_this.nTabTipTextHtml);
			tempText.html(title).appendTo(tempTip);
			tempTip.appendTo($(this));
			this.title = '';
		});
	}
}
var nTab = new NTab('.messageNTabPanel_01 ul');


window.Menu = function(dom,list){
	if(!dom)return false;
	if(!list)return false;
	this.dom = $(dom);
	if(!dom.length) return false;
	this.list = $(list);
	this.timeout = null;
	this.delay = 200;
	this.speed = 500;
	this.state = 0;
	this.isShow = false;
	this.setup();
}
Menu.prototype = {
	setup:function(){
		this.bindEvents();
	},
	bindEvents: function(){
		var _this = this;
		this.dom.hover(function(){
			if(_this.state!=1){
				_this.state = 1;
				clearTimeout(_this.timeout);
				if(!_this.isShow){
					_this.timeout = setTimeout(function(){
						_this.show();
					},_this.delay);
				}
			}
		},function(){
			if(_this.state!=2){
				_this.state = 2;
				clearTimeout(_this.timeout);
				if(_this.isShow){
					_this.timeout = setTimeout(function(){
						_this.hide();
					},_this.delay);
				}
			}
		});
		this.list.hover(function(){
			if(_this.state!=1){
				_this.state = 1;
				clearTimeout(_this.timeout);
				if(!_this.isShow){
					_this.timeout = setTimeout(function(){
						_this.show();
					},_this.delay);
				}
			}
		},function(){
			if(_this.state!=2){
				_this.state = 2;
				clearTimeout(_this.timeout);
				if(_this.isShow){
					_this.timeout = setTimeout(function(){
						_this.hide();
					},_this.delay);
				}
			}
		});
	},
	show: function(){
		this.state = 0;
		this.isShow = true;
		this.list.stop().css('opacity',0).show().fadeTo(this.speed,1);
	},
	hide: function(){
		this.state = 0;
		this.isShow = false;
		this.list.stop().fadeOut(this.speed);
	}
}
var headTitle = new Menu('#header .titleField h1 a','#header .titleField ul');
var userMenu = new Menu('#header .userField p','#header .userField ul');
var userMenu = new Menu('#salesHeader .userField p','#salesHeader .userField ul');


window.CheckAll = function(){
	this.check = function(obj,name){
		var selectItem = $('input[type="checkbox"][name="'+name+'"]');
		var flag = true;
		if(!obj.checked){
			flag = false;
		}
		selectItem.each(function(){
			if(this.checked!=flag){
				this.checked = flag;
				var _this = this;
				var tempFn = this.onchange || function(){
					$._data(_this, "events") && $._data(_this, "events")["change"][0].handler && $._data(_this, "events")["change"][0].handler.call(_this);
				};
				tempFn();
			}
		});
	}
}
var checkAll = new CheckAll();



//未完
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
		var flag2 = false;
		if(url.indexOf('#')!=-1){
			flag2 = true;
		}
		if(flag2||url.indexOf('?')!=-1||url.indexOf('&')!=-1){
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
		if(flag2){
			window.location.hash = '';
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


window.Dialogue = function(){
	this.dom = $('<div class="dialogue"><i></i></div>');
	this.element = $('<div class="dialogueField"></div>');
	this.title = $('<h1></h1>');
	this.content = $('<p></p>');
	this.loadField = $('<div class="loading"></div>');
	this.btnField = $('<div class="btns"></div>');
	this.btnHtml = '<button class="btn"></button>';
	this.picClass = 'pics';
	this.autoSizeClass = 'auto';
	this.pic = $('<img src="" alt="" />');
	this.btnClass = {
		btn1:'btn1',
		btn2:'btn2'
	}
	this.btns = [];
	this.isShow = false;
	this._setup();
}
Dialogue.prototype = {
	dlShowPic: function(config){
		var _this = this;
		config = this._readConfig(config,'扫描二维码，在微信中预览');
		this._readBoxSize();
		if(!config.btns.length){
			config.btns = [{
				text:'关闭',
				fn:function(){
					_this.closeAll();
				}
			}]
		}
		this.closeAll(function(_this){
			_this.isShow = true;
			_this.dom.appendTo('body');
			_this.element.addClass(_this.picClass).appendTo(_this.dom);
			_this.title.html(config.title).appendTo(_this.element);
			_this.pic.attr('src',config.content).appendTo(_this.element);
			_this._addBtn(config.btns);
			_this.dom.stop().css('z-index','21').fadeTo(300,1);
		});
	},
	
	dlAlert: function(config,boxSize){
		var _this = this;
		config = this._readConfig(config);
		this._readBoxSize(boxSize);
		if(!config.btns.length){
			config.btns = [{
				text:'确认',
				fn:function(){
					_this.closeAll();
				}
			}]
		}
		this.closeAll(function(_this){
			_this.isShow = true;
			_this.dom.appendTo('body');
			_this.element.appendTo(_this.dom);
			_this.title.html(config.title).appendTo(_this.element);
			if(config.content){
				_this.content.html(config.content).appendTo(_this.element);
			}
			_this._addBtn(config.btns);
			_this.dom.stop().css('z-index','21').fadeTo(300,1);
		});
	},
	
	dlWarning: function(config,boxSize){
		var _this = this;
		config = this._readConfig(config);
		this._readBoxSize(boxSize);
		if(!config.btns.length){
			config.btns = [{
				text:'重试',
				fn:function(){
					_this.closeAll();
				}
			}]
		}
		this.closeAll(function(_this){
			_this.isShow = true;
			_this.dom.appendTo('body');
			_this.element.appendTo(_this.dom);
			_this.title.html(config.title).appendTo(_this.element);
			if(config.content){
				_this.content.html(config.content).appendTo(_this.element);
			}
			_this._addBtn(config.btns);
			_this.dom.stop().css('z-index','21').fadeTo(300,1);
		});
	},
	
	dlLoading: function(){
		var _this = this;
		this._readBoxSize();
		//config = this._readConfig(config);
		this.closeAll(function(_this){
			_this.isShow = true;
			_this.dom.appendTo('body');
			//_this.title.html(config.title).appendTo(_this.element);
			_this.loadField.appendTo(_this.dom);
			_this.dom.stop().css('z-index','21').fadeTo(300,1);
		});
	},
	
	closeAll: function(fn){
		
		var _this = this;
		fn = fn || function(){};
		var callBack = function(){
			_this.element.removeClass(_this.picClass).remove();
			_this.title.remove();
			_this.content.remove();
			_this.loadField.remove();
			for(var i in _this.btns){
				_this.btns[i].unbind().remove();
			}
			_this.btns = [];
			_this.btnField.remove();
			_this.dom.remove();
			_this.isShow = false;
			fn(_this);
		}
		if(this.isShow){
			this.dom.stop().fadeTo(300,0,function(){
				callBack();
			});
		}else{
			callBack();
		}
	},
	
	_setup: function(){
	},
	
	_readConfig: function(config,title){
		title = title || '温馨提示';
		var configReturn = {};
		if(typeof config == 'string'){
			config = {title:title,content:config};
		}
		configReturn.title = config.title || title;
		configReturn.content = config.content || '';
		configReturn.btns = config.btns || [];
		return configReturn;
	},
	
	_readBoxSize: function(boxSize){
		boxSize = (typeof(boxSize) == 'undefined')?'default':boxSize;
		if(boxSize == 'auto'){
			this.element.addClass(this.autoSizeClass);
		}else{
			this.element.removeClass(this.autoSizeClass);
		}
		this.element.css('width','').css('height','');
		if(!!boxSize.width){
			this.element.addClass('auto');
			this.element.css('width',boxSize.width);
		}
		if(!!boxSize.height){
			this.element.addClass('auto');
			this.element.css('height',boxSize.height);
		}

	},
	
	_addBtn: function(btns){
		if(!!btns.length){
			this.btnField.appendTo(this.element);
		}
		for(var i in btns){
			var tempBtn = $(this.btnHtml);
			if(i == 0){
				tempBtn.addClass(this.btnClass.btn1);
			}else{
				tempBtn.addClass(this.btnClass.btn2);
			}
			var tempFn = btns[i].fn;
			tempBtn.html(btns[i].text).appendTo(this.btnField).bind('click',tempFn);
			this.btns.push(tempBtn);
		}
	}
}
var dialogue = null;
if(window.top != window.self && window.top.dialogue){
	dialogue = window.top.dialogue;
}else{
	dialogue = new Dialogue();
}



window.ArticleEditor = function(editorDom){
	if(!editorDom)return false;
	this.editorDom = $(editorDom);
	this.editorText = this.editorDom.find('.textarea');
	if(!this.editorText)return false;
	this.controlHtml = [
		'<div class="control">',
			'<div class="count"><span>您还可以输入</span><em></em>个字</div>',
			'<div class="btns">',
				'<a href="#" class="btn emotBtn" title="插入表情">',
					'<i class="message10"></i>',
				'</a>',
			'</div>',
			'<div class="emotList">',
				'<ul>',
				'</ul>',
			'</div>',
		'</div>'
	].join('');
	this.controlSelector = {
		countArea: '.count',
		count: '.count em',
		tip: '.count span',
		emotBtn: '.btns .emotBtn',
		emotArea: '.emotList',
		emotList: '.emotList ul'
	};
	this.emoticonHtml = [
		'<li>',
			'<i class="emoticon"></i>',
		'</li>'
	].join('');
	this.emoticons = [];
	this.emoticonCodes = "/::)_/::~_/::B_/::|_/:8-)_/::<_/::$_/::X_/::Z_/::'(_/::-|_/::@_/::P_/::D_/::O_/::(_/::+_/:--b_/::Q_/::T_/:,@P_/:,@-D_/::d_/:,@o_/::g_/:|-)_/::!_/::L_/::>_/::,@_/:,@f_/::-S_/:?_/:,@x_/:,@@_/::8_/:,@!_/:!!!_/:xx_/:bye_/:wipe_/:dig_/:handclap_/:&-(_/:B-)_/:<@_/:@>_/::-O_/:>-|_/:P-(_/::'|_/:X-)_/::*_/:@x_/:8*_/:pd_/:<W>_/:beer_/:basketb_/:oo_/:coffee_/:eat_/:pig_/:rose_/:fade_/:showlove_/:heart_/:break_/:cake_/:li_/:bome_/:kn_/:footb_/:ladybug_/:shit_/:moon_/:sun_/:gift_/:hug_/:strong_/:weak_/:share_/:v_/:@)_/:jj_/:@@_/:bad_/:lvu_/:no_/:ok_/:love_/:<L>_/:jump_/:shake_/:<O>_/:circle_/:kotow_/:turn_/:skip_/:oY_/:#-0_/:hiphot_/:kiss_/:<&_/:&>".split('_');
	this.iconDistance = 24;
	this.tip = {
		normal:'您还可以输入',
		warning:'您已超出',
		warningClass:'warning'
	};
	this.isShow = false;
	this.setup();
}

ArticleEditor.prototype = {
	setup: function(){
		this.editorDom.get(0).editor = this;
		this.control = $(this.controlHtml);
		this.control.appendTo(this.editorDom);
		for(var i in this.emoticonCodes){
			var tempEmoticon = $(this.emoticonHtml);
			tempEmoticon.appendTo(this.control.find(this.controlSelector.emotList));
			tempEmoticon.attr('code',this.emoticonCodes[i]);
			tempEmoticon.find('.emoticon').css('background-position',-this.iconDistance*i+'px 0');
			this.emoticons.push(tempEmoticon);
		}
		
		this.canCount = !!this.editorText.attr('maxlength');
		if(this.canCount){
			this.totalCount = parseInt(this.editorText.attr('maxlength'),10);
			this.editorText.attr('max',this.totalCount);
			this.editorText.attr('maxlength','');
		}
		this.bindEvents();
		this.readCount();
	},
	bindEvents: function(){
		var _this = this;
		for(var i in this.emoticons){
			this.emoticons[i].click(function(e){
				_this.addCode($(this).attr('code'));
				_this.isShow = false;
				_this.control.find(_this.controlSelector.emotArea).hide();
				if(e.stopPropagation){
					e.stopPropagation();
				} else {
					e.cancelBubble = true;
				}
			});
		}
		
		this.control.find(this.controlSelector.emotBtn).click(function(e){
			if(_this.isShow){
				_this.isShow = false;
				_this.control.find(_this.controlSelector.emotArea).hide();
			}else{
				_this.isShow = true;
				_this.control.find(_this.controlSelector.emotArea).show();
			}
			if(e.stopPropagation){
				e.stopPropagation();
			} else {
				e.cancelBubble = true;
			}
			e.preventDefault();
		});
		
		this.editorText.bind('input propertychange',function(e){
			_this.readCount();
		});
		
		$('#main').bind('click',function(){
			if(_this.isShow){
				_this.isShow = false;
				_this.control.find(_this.controlSelector.emotArea).hide();
			}
		});
		
	},
	readCount: function(num){
		num = num || 0;
		var flag = true;
		if(this.canCount){
			var textCount = this.editorText.val().length;
			var count = this.totalCount - textCount - num;
			if(count<0){
				flag = false;
			}
			this.showCount(count);
		}else{
			this.control.find(this.controlSelector.countArea).hide();
		}
		return flag;
	},
	showCount: function(count){
		if(count>1){
			this.control.find(this.controlSelector.count).removeClass(this.tip.warningClass).html(count);
			this.control.find(this.controlSelector.tip).html(this.tip.normal);
		}else{
			this.control.find(this.controlSelector.count).addClass(this.tip.warningClass).html(Math.abs(count));
			if(count>=0){
				this.control.find(this.controlSelector.tip).html(this.tip.normal);
			}else{
				this.control.find(this.controlSelector.tip).html(this.tip.warning);
			}
		}
	},
	addCode: function(code) {
		//IE support
		var editorText = this.editorText.get(0);
		if (document.selection) {
			editorText.focus();
			var sel = document.selection.createRange();
			sel.text = code;
			sel.select();
		}
		//MOZILLA/NETSCAPE support 
		else if (editorText.selectionStart || editorText.selectionStart == '0') {
			var startPos = editorText.selectionStart;
			var endPos = editorText.selectionEnd;
			// save scrollTop before insert www.keleyi.com
			var restoreTop = editorText.scrollTop;
			editorText.value = editorText.value.substring(0, startPos) + code + editorText.value.substring(endPos, editorText.value.length);
			if (restoreTop > 0) {
				editorText.scrollTop = restoreTop;
			}
			editorText.focus();
			editorText.selectionStart = startPos + code.length;
			editorText.selectionEnd = startPos + code.length;
		} else {
			editorText.value += code;
			editorText.focus();
		}
		this.readCount();
	},
	canSubmit: function(){
		var flag = this.readCount();
		return flag;
	}
}

$('.articleEditor').each(function(){
	var tempEditor = new ArticleEditor(this);
});


window.CopyString = function(){
	this.dom = $('<div class="copyForm"></div>');
	this.element = $('<div class="copyFormField"></div>');
	this.title = $('<h1>按下Ctrl+C复制以下文字</h1>');
	this.content = $('<textarea></textarea>');
	this.isShow = false;
	this.setup();
}
CopyString.prototype = {
	setup: function(){
	},
	bindEvents: function(){
		var _this = this;
		
		this.dom.bind('click',function(){
			_this.hide();
		});
		
		$(document).bind('keydown',function(e){
			var currKey=e.keyCode||e.which||e.charCode;
			var keyName = String.fromCharCode(currKey);
			if ((e.ctrlKey||e.metaKey) && keyName.toLowerCase() == 'c'){
				_this.hide();
			} 
		})
	},
	unbindEvents: function(){
		this.dom.unbind('click');
		$(document).unbind('keydown');
	},
	
	copy: function(string){
		if(!string)return false;
		this.element.append(this.title).append(this.content);
		this.content.html(string);
		this.dom.appendTo('body').append(this.element).css('opacity',0).stop().show().fadeTo(200,1);
		this.content.get(0).focus()
		this.content.get(0).select();
		this.bindEvents();
	},
	
	hide: function(){
		var _this = this;
		this.unbindEvents();
		this.dom.stop().fadeOut(200,function(){
			_this.dom.remove();
		});
	}
}
var copyString = null;
if(window.top != window.self && window.top.copyString){
	copyString = window.top.copyString;
}else{
	copyString = new CopyString();
}
