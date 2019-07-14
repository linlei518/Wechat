// JavaScript Document
window.RegistrationEdit = function(url){
	this.editBoxUrl = url;
	this.table = new Table($('.registrationEditTable').get(0));
	this.setup();
	this.trCode = [
		'<tr>',
			'<td class="name qTitle">',
				'<input type="hidden" value="" name="txtTheTitle"><input type="hidden" value="0" name="hidTopicId">',
				'<span></span>',
			'</td>',
			'<td class="info info1 qType1">',
				'<input type="hidden" value="" name="txtAlterType">',
				'<span></span>',
			'</td>',
			'<td class="info info1 qOption">',
				'<input type="hidden" value="" name="txtOptions">',
			'</td>',
			'<td class="info info1 qType2">',
				'<label><input type="checkbox" name="checkBitian">必填</label>',
			'</td>',
			'<td class="info info1 qShow">',
				'<label><input type="checkbox" name="checkQiyong">启用</label>',
			'</td>',
			'<td class="info info1">',
				'<input type="button" class="btn btnUp" title="上移" value="">',
				'<input type="button" class="btn btnDown" title="下移" value="">',
			'</td>',
			'<td class="control2">',
				'<input type="button" class="btn btn5 edit" value="编辑">',
				'<input type="button" class="btn btn5 delete" value="删除">',
			'</td>',
		'</tr>'
	].join(' ');
}
RegistrationEdit.prototype = {
	setup: function(){
		this.bindEvents();
	},
	bindEvents: function(){
		var _this = this;
		this.table.trs.each(function(){
			var obj = this;
			$(this).find('.edit').click(function(){
				_this.edit(obj.num);
			});
			$(this).find('.delete').click(function(){
				_this.deleteTr(obj.num);
			});
		});
	},
	unbindEvents: function(){
		this.table.tableDom.find('.edit').unbind();
		this.table.tableDom.find('.delete').unbind();
	},
	edit: function(num){
		var data = {};
		if(typeof num != 'undefined'){
			var editGroup = this.table.trs.eq(num);
			var title = editGroup.find('.qTitle input').eq(0).val();
			var type1Value = editGroup.find('.qType1 input').eq(0).val();
			var option = editGroup.find('.qOption input').eq(0).val();
			var type2Value = (editGroup.find('.qType2 input').eq(0).prop('checked'))?0:1;
			var isShow = (editGroup.find('.qShow input').eq(0).prop('checked'))?0:1;
			data = {
				qTitle:title,
				qType1:{value:type1Value},
				qOption:option,
				qType2:{value:type2Value},
				qShow:{value:isShow}
			}
		}
		num = (typeof num != 'undefined')?num:this.table.trs.length;
		bombbox.openBox(this.editBoxUrl,function(){
			bombbox.iframeItem.get(0).contentWindow.setData(data,num);
		});
	},
	setData: function(data,num){
		var editGroup = null;
		num = (typeof num!='undefined')?num:this.table.trs.length;
		if(num >= this.table.trs.length){
			editGroup = $(this.trCode);
		}else{
			editGroup = this.table.trs.eq(num);
		}
		editGroup.find('.qTitle input').eq(0).val(data.qTitle);
		editGroup.find('.qTitle span').eq(0).html(data.qTitle);
		
		editGroup.find('.qType1 input').eq(0).val(data.qType1.value);
		editGroup.find('.qType1 span').eq(0).html(data.qType1.name);
		
		editGroup.find('.qOption input').eq(0).val(data.qOption);
		editGroup.find('.qOption p').remove();
		if(data.qOption){
			var qOptions = data.qOption.split('|');
			for(var i=0; i<qOptions.length && i<6; i++){
				var tempText = qOptions[i].split('^')[0];
				if(i==5 && qOptions.length>5){
					tempText = '...';
				}
				editGroup.find('.qOption').append('<p>'+tempText+'</p>');
			}
		}else{
			editGroup.find('.qOption').append('<p>/</p>');
		}
		if(data.qType2.value+''=='0'){
			editGroup.find('.qType2 input').get(0).checked = true;
		}else{
			editGroup.find('.qType2 input').get(0).checked = false;
		}
		if(data.qShow.value+''=='0'){
			editGroup.find('.qShow input').get(0).checked = true;
		}else{
			editGroup.find('.qShow input').get(0).checked = false;
		}
		
		if(num >= this.table.trs.length){
			this.table.addTr(num,editGroup);
			this.unbindEvents();
			this.checkTrs();
			this.bindEvents();
		}
	},
	checkTrs: function(){
		if(!this.table.trs.length){
			this.table.tableDom.hide();
		}else{
			this.table.tableDom.show();
		}
	},
	deleteTr:function(num){
		var _this = this;
		dialogue.dlAlert({
			content:'是否要删除此被填项？',
			btns:[{
				text:'确认',
				fn:function(){
					_this.unbindEvents();
					_this.table.deleteTr(num);
					_this.checkTrs();
					_this.bindEvents();
					dialogue.closeAll();
				}
			},{
				text:'取消',
				fn:function(){
					dialogue.closeAll();
				}
			}]
		});
	}
}