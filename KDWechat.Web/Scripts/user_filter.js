var filterLIst = new Array();
var counter = 0;

function RemoveInput(input) {
    for (var i = 0; i < filterLIst.length; i++)
    {
        if (filterLIst[i].id == $(input).attr("wid"))
        {
            filterLIst.splice(i, 1);
            $(input).remove();
            setFilterSize();
            return;
        }
    }
}

function InitSSa(idss, shss, valuesss, namess)
{
    var idA = idss.split(',');
    var shA = shss.split(',');
    var valuesA = valuesss.split(',');
    var nameA = namess.split(',');
    for(var i=0;i<idA.length;i++,counter++)
    {
        filterLIst.push({ id: idA[i], sh: shA[i], values: valuesA[i], name: nameA[i] });
    }
}

$(function () {
    
    $(".timeBtn").click(function () {
        var se = $(this);
        var titles = se.parent().parent().children().html();
        for (var i = 0; i < filterLIst.length; i++) {
            if (filterLIst[i].sh == se.attr("sh") && filterLIst[i].name == titles +  se.attr("pro")) {//这里用了name做唯一认证
                alert("请不要重复添加相同条件！")
                return false;
            }
        }
        var times = $("#" + se.attr("pro")).val();
        if (!times || times == "")
        {
            alert("请选择时间");
            return false;
        }
        var shi = { id: counter, sh: se.attr("sh"), values: times, name: titles + se.attr("pro") };
        filterLIst.push(shi);
        $("#ddShow").append('<a href="javascript:void(0)" onclick="RemoveInput(this)" class="btn cancelBubble"  wid="' + counter + '"  title="点击取消">' +titles + times + '</a>');
        setFilterSize();
        counter++;
    });

    $(".areaBtn").click(function () {
        var se = $(this);
        var sh = se.attr("sh");
        var areaTxt;

        if (sh == "Area")
        {
            if (!$("#Province").val() || $("#Province").val() == "") {
                alert("请先选择位置");
                return false;
            }
            areaTxt = $("#Province").val()+"-"+$("#City").val()+"-"+$("#Area").val();
        }
        else if (sh = "WeChatArea")
        {
            if (!$("#WeChatProvince").val() || $("#WeChatProvince").val() == "") {
                alert("请先选择位置"); return false;
            }
            areaTxt = $("#WeChatProvince").val()+"-"+$("#WeChatCity").val()+"-"+$("#WeChatArea").val();
        }
        var titles = se.parent().parent().children().html();

        for (var i = 0; i < filterLIst.length; i++) {
            if (filterLIst[i].sh == se.attr("sh") && filterLIst[i].values == areaTxt) {//这里用了name做唯一认证
                alert("请不要重复添加相同条件！")
                return false;
            }
        }
        var shi = { id: counter, sh: se.attr("sh"), values: areaTxt, name: titles + areaTxt };
        filterLIst.push(shi);
        $("#ddShow").append('<a href="javascript:void(0)" onclick="RemoveInput(this)" class="btn cancelBubble"  wid="' + counter + '"  title="点击取消">' + titles + areaTxt + '</a>');
        setFilterSize();
        counter++;
    });
    

    $(".filterSelect").click(function () {
        var se = $(this);
        for (var i = 0; i < filterLIst.length; i++) {
            if (filterLIst[i].sh == se.attr("sh") && filterLIst[i].values == se.attr("pro")) {
                alert("请不要重复添加相同条件！")
                return false;
            }
        }
        var titles = se.parent().parent().children().html();
        var shi = {id:counter,sh:se.attr("sh"),values:se.attr("pro"),name: titles + se.html()};
        filterLIst.push(shi);
        $("#ddShow").append('<a href="javascript:void(0)" onclick="RemoveInput(this)" class="btn cancelBubble"  wid="' + counter + '"  title="点击取消">' + titles+ se.html() + '</a>');
        setFilterSize();
        counter++;
    });

    $("#Button1").click(function () {
        var data = JSON.stringify(filterLIst);
        parent.CheckData(data);
        //alert(data);
        return false;
    });

});


function setFilterSize() {//每次增加或删除一个筛选条件，必须调用此方法一次
    var placeholder = $('.filterShowPanel_01 .prev');
    var filterList = $('.filterShowPanel_01 .selectedList');
    filterList.css('top', placeholder.offset().top).css('width', placeholder.width());
    placeholder.css('height', filterList.get(0).offsetHeight);
}
window.onresize = function () {
    setFilterSize();
}