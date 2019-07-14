
function subAll() {//这里是一个示范，通过获取textarea有没有字符超出，来确定能不能发布，这里不能判定没有输入字符的情况！！
    var flag = false;
    if ($("#txtContents").val() != "") {
        flag = $('.articleEditor').get(0).editor.canSubmit();
    }
    return flag;
}

//提交
function btnSubmitClick() {
    var link1 = $("#txtlike").val();
    var link2 = $("#txtauthor").val();
    var type = $("#hftype").val();
    if (type == "0" && subAll()==false) {
        showTip.show("文字必须为1到600个字", true);
        $("#txtContents").focus();
        return false;
    } else if (type == "1" && parseInt($("#hfid").val()) <= 0) {
        showTip.show("请选择一条图片素材", true);
        return false;
    } else if (type == "4" && parseInt($("#hfid").val()) <= 0) {
        showTip.show("请选择一条单图文素材", true);
        return false;
    } else if (type == "5" && parseInt($("#hfid").val()) <= 0) {
        showTip.show("请选择一条多图文素材", true);
        return false;
    } else if (type == "2" && parseInt($("#hfid").val()) <= 0) {
        showTip.show("请选择一条语音素材", true);
        return false;
    } else if (type == "3" && parseInt($("#hfid").val()) <= 0) {
        showTip.show("请选择一条视频素材", true);
        return false;
    } else if (type == "6" && $("#txtlike").val() == "") {
        showTip.show("请输入外链地址", true);
        $("#txtlike").focus();
        return false;
    } else if (type == "6" && $("#txtlike").val() != "" && link1.indexOf("http://") < 0 && link1.indexOf("https://") < 0) {
        showTip.show("外链地址必须是以http://开头", true)
        $("#txtlike").focus();
        return false;
    } else if (type == "7" && $("#txtauthor").val() == "") {
        showTip.show("请输入授权地址", true);
        $("#txtauthor").focus();
        return false;
    } else if (type == "7" && $("#txtauthor").val() != "" && link2.indexOf("http://") < 0 && link2.indexOf("https://") < 0) {
        showTip.show("授权地址必须是以http://开头", true)
        $("#txtauthor").focus();
        return false;
    } else if (type == "8" && parseInt($("#hfid").val()) <= 0) {
        showTip.show("请选择一个应用", true);
        return false;
    } 
    return true;
}

//文本点击
function firstTextClick() {
    clareShow();
 
    $("#txtContents").focus();
    $("#div_text").show();
    $("#hftype").val("0");
    $("#hfid").val("0");
    
    $(".messageNTabPanel_01").find("li").find("a").eq(0).attr("class", "current");
}

//多客服点击
function multiCustomerClick(index) {
    clareShow();
    $("#hftype").val("10");
    $("#hfid").val("10");
    $("#div_multiCustomer").show();
    $(".messageNTabPanel_01").find("li").find("a").eq(index).attr("class", "current");
}

//选择结果
function selectResult(channel_id, material_id, _path_link, _title, _create_time, _suumary, is_close, _multi_list, video_img, video_type) {
    $("#hflogtitle").val(_title);
    clareShow();
    $("#hftype").val(channel_id);
    $("#hfid").val(material_id);
    if (channel_id == 1) { //图片
        $("#div_pic").show();
        $("#div_pic").html(" <div class=\"infoField\"><div class=\"img\"><span><img src=\"" + _path_link + "\"   ></span></div></div>")
        $(".messageNTabPanel_01").find("li").find("a").eq(1).attr("class", "current");
    } else if (channel_id == 2) { //语音
        $("#div_voice").show();
        $("#div_voice").html("<div class=\"infoField\"><div class=\"title\"><h1>" + _title + "</h2></div><div class=\"sound\"><a class=\"audio\" href=\"#\" onClick=\"audioControl(this,'" + _path_link + "')\" title=\"点击试听\"></a></div></div>");
        $(".messageNTabPanel_01").find("li").find("a").eq(4).attr("class", "current");
    } else if (channel_id == 3) { //视频
        $("#div_video").show();
        var str = "<div class=\"infoField\"><div class=\"title\"><h1>" + _title + "</h2></div><div class=\"img\"><span>";
        if (video_type == 1) {
            str += "<a class=\"video\" href=\"#\" onClick=\"video.play('" + _path_link + "')\" title=\"点击播放\"></a>";
        } else {
            str += "<a class=\"video\" href=\"" + _path_link + "\"  target=\"_blank\" title=\"点击播放\"></a>";
        }
        str += "<img src=\"" + video_img + "\" alt=\"\"></span></div></div>";
        $("#div_video").html(str);
        $(".messageNTabPanel_01").find("li").find("a").eq(5).attr("class", "current");
    } else if (channel_id == 4) { //单图文
        $("#div_news").show();
        $("#div_news").html("<div class=\"infoField\"><div class=\"title\"><h1>" + _title + "</h2></div><div class=\"img\"><span><img src=\"" + _path_link + "\" alt=\"\"></span></div><div class=\"content\">" + _suumary + "</div></div>");
        $(".messageNTabPanel_01").find("li").find("a").eq(2).attr("class", "current");

    } else if (channel_id == 5) { //多图文
        $("#div_multi_news").show();
        $("#div_multi_news").html("<div class=\"infoField mainInfo act\"><div class=\"img\"><span><img src=\"" + _path_link + "\" alt=\"\"></span></div><div class=\"title\"><h1>" + _title + "</h1></div></div>" + _multi_list);
        $(".messageNTabPanel_01").find("li").find("a").eq(3).attr("class", "current");

    } else if (channel_id == 8) {  //模块
        $("#div_module").show();
        $("#div_module").html("<div class=\"infoField\"><div class=\"title\"><h1>" + _title + "</h2></div><div class=\"img\"><span><img src=\"" + _path_link + "\" alt=\"\"></span></div><div class=\"content\">" + _suumary + "</div></div>");
        $(".messageNTabPanel_01").find("li").find("a").eq(8).attr("class", "current");
    }
    if (is_close == 1) {
        bombbox.closeBox();
    }

}

function selectList(channel_Id) {
    clareShow();  
    $("#hftype").val(channel_Id);
    if (channel_Id == 6)
    {
        
        $("#div_link").show();
        $(".messageNTabPanel_01").find("li").find("a").eq(6).attr("class", "current");
    }
    else if (channel_Id == 7)
    {
        $("#div_author").show();
        $(".messageNTabPanel_01").find("li").find("a").eq(7).attr("class", "current");
    }
    else if (channel_Id == 8)
    {
        $("#div_module").show();
        $(".messageNTabPanel_01").find("li").find("a").eq(8).attr("class", "current");
    }
}
//清除数据
function clareShow() {
    $("#divtxt").hide();
    $("#div_text").hide();
    $("#div_pic").hide();
    $("#div_news").hide();
    $("#div_voice").hide();
    $("#div_video").hide();
    $("#div_multi_news").hide();
    $("#div_link").hide();
    $("#div_author").hide();
    $("#div_module").hide();
    $("#div_multiCustomer").hide();

   // $("#divtxt").html("");
    $("#div_pic").html("");
    $("#div_news").html("");
    $("#div_voice").html("");
    $("#div_video").html("");
    $("#div_multi_news").html("");
    $("#txtlike").val();
    $("#txtauthor").val();
    $("#div_module").html("");
    $("#hftype").val("-1");
    $("#hfid").val("0");
    $(".messageNTabPanel_01").find("li").find("a").each(function () {
        $(this).removeAttr("class");
    })
}
