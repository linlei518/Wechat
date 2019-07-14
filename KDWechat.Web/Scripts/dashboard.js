var widList = new Array();

function getList(area) {
    var nameI = "";
    switch (area) {
        case 0:
            nameI = "粉丝人数";
            break;
        case 1:
            nameI = "新增人数";
            break;
        case 2:
            nameI = "流失人数";
            break;
    
    }
    $(".selectedList").removeClass("selected");
    $("#dou").append("<a href=\"javascript:RemoveTag(1)\" class=\"btn cancelBubble\" title=\"点击取消\">" + nameI + "</a>")
    $("#dlArea").addClass("selected");

    $("#hidType").val(area);
 
    var wxId = $("#hidWX").val();
    var type = $("#hidType").val();
    var sub_sdate = $("#txtbegin_date").val();
    var sub_edate = $("#txtend_date").val();
    var sub_sdateD = $("#txtbegin_dateD").val();
    var sub_edateD = $("#txtend_dateD").val();
    var mid = $("#hidMid").val();

    if (wxId != "") {
        location.replace("dashboard.aspx?wxId=" + wxId + "&type=" + type + "&sub_sdate=" + sub_sdate + "&sub_edate=" + sub_edate + "&sub_sdateD=" + sub_sdateD + "&sub_edateD=" + sub_edateD + "&m_id="+mid);
    }
    else {
        alert('请选择公众号');
    }

}


function getListArea(area) {
    var nameI = "";
    switch (area) {
        case 0:
            nameI = "粉丝人数";
            break;
        case 1:
            nameI = "新增人数";
            break;
        case 2:
            nameI = "流失人数";
            break;

    }
    $(".selectedList").removeClass("selected");
    $("#dou").append("<a href=\"javascript:RemoveTag(1)\" class=\"btn cancelBubble\" title=\"点击取消\">" + nameI + "</a>")
    $("#dlArea").addClass("selected");

    $("#hidType").val(area);

   
}


function RemoveTag(tag) {
    $("#dou").html("");
    $("#dlArea").removeClass("selected");
    $(".selectedList").addClass("selected");
    $.ajax({
        type: "POST",
        url: "sys_fans_statistics_list.aspx?t=1&area=-1",
        data: "",
        success: function (msg) {
            $("#tbd").html(msg);
        }
    });
    return;
}

function RemoveInput(input) {
    widList.pop(input.wid);
    $(input).remove();
}

function AddChange(labelA, wsid) {
    var idString = "#aLabel" + wsid;
    aLabel = $(idString);
    aLabel.html("已添加");
    if (widList.length > 9) {
        alert("最多只能添加10条！");
        return false;
    }
    for (var i = 0; i < widList.length; i++) {
        if (widList[i] == wsid) {
            alert("该公众号已加入对比，请勿重复操作！");
            return;
        }
    }
    widList.push(wsid);
    $("#ddShow").append('<a href="javascript:void(0)" onclick="RemoveInput(this)" class="btn cancelBubble"  wid="' + wsid + '"  title="点击取消">' + labelA + '</a>');
}

function ContrastAll() {
    if (widList.length == 0) {
        alert("请先选择微信号加入对比！");
        return false;
    }
    if (widList.length < 2) {
        alert("请至少选择两个微信号加入对比！");
        return false;
    }
    var ids = "";
    for (var i = 0; i < widList.length; i++) {
        ids += widList[i] + ",";
    }
    location.href = "sys_fans_statistics.aspx?Ids=" + ids + "&m_id=87";
}