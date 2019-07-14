// JavaScript Document
window.QuestionnaireEdit = function(url){
	this.editBoxUrl = url;
	this.table = new Table($('.questionnaireEditTable').get(0));
	this.setup();
	this.trCode = [
		'<tr>',
			'<td class="name qTitle">',
				'<input type="hidden" value="" name="txtTheTitle"><input type="hidden" value="0" name="hidTopicId">',
				'<span></span>',
			'</td>',
			'<td class="info info1 qOption">',
				'<input type="hidden" value="" name="txtOptions">',
			'</td>',
			'<td class="info info1 qType1">',
				'<input type="hidden" value="" name="radRadioChoice">',
				'<span></span>',
			'</td>',
			'<td class="info info1 qType2">',
				'<input type="hidden" value="" name="radIsRequired">',
				'<span></span>',
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
QuestionnaireEdit.prototype = {
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
			var option = editGroup.find('.qOption input').eq(0).val();
			var type1Value = editGroup.find('.qType1 input').eq(0).val();
			var type2Value = editGroup.find('.qType2 input').eq(0).val();
			data = {
				qTitle:title,
				qOption:option,
				qType1:{value:type1Value},
				qType2:{value:type2Value}
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
		
		editGroup.find('.qOption input').eq(0).val(data.qOption);
		editGroup.find('.qOption p').remove();
		var qOptions = data.qOption.split('|');
		for(var i in qOptions){
			var tempText = qOptions[i].split('^')[0];
			editGroup.find('.qOption').append('<p>'+tempText+'</p>');
		}
		
		editGroup.find('.qType1 input').eq(0).val(data.qType1.value);
		editGroup.find('.qType1 span').eq(0).html(data.qType1.name);
		
		
		editGroup.find('.qType2 input').eq(0).val(data.qType2.value);
		editGroup.find('.qType2 span').eq(0).html(data.qType2.name);
		
		if(num >= this.table.trs.length){
			this.table.addTr(num,editGroup.html());
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
		if(!confirm('是否要删除此问题？')) return false;
		this.unbindEvents();
		this.table.deleteTr(num);
		this.checkTrs();
		this.bindEvents();
	}
}