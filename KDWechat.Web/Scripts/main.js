//选择标签

var checkBiaoqian=$(".ckbqBtn");
checkBiaoqian.each(function(e) {
var checkBQok=$(this).parent().find(".checkBQok")	;
var allinputs=$(this).next(".spanContent").find("input");
var hiddiv=$(this).next(".spanContent");
var biaoqian=$(this).parents(".con").find(".biaoqian");


$(this).click(function(){
	hiddiv.show();
})
    
 checkBQok.click(function(){ 
	var allinputVal="";
	hiddiv.hide();
	for(var i in allinputs){
		var eachinput=allinputs[i];
		var num=0;
			if(eachinput.type=='checkbox'&&eachinput.checked==true){
				 var eachinputval=$(eachinput).val();
				 allinputVal+=' '+eachinputval+' |';
			 }			
		}
	biaoqian.text(allinputVal);

	})
	


});

