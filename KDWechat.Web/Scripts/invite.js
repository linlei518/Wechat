
$(function () {

    //$(".listNTab a").each(function (i) {
    //    $(this).click(function () {


    //        $(".listNTab a").prop("class", "btn nTabBtn");
    //        $(this).prop("class", "btn nTabBtn current");

    //        $(".listPanel_01").attr("style", "display:none");
    //        $(".listPanel_01").eq(i).attr("style", "display:block");
    //        if (i == 8) {
    //            $("#loca").click();
    //        }

    //    })
    //});




    $("#btnSubmit").click(function () {
        var isc = true;

        var type = $("#hftype").val();
        switch (type) {
            case "1":
            case "2":
                $("#hfContents1").val("");
                if (isc) {
                    var testStr = "";
                    if ($("#rboStatus1_0").prop("checked") == false && $("#rboStatus1_1").prop("checked") == false) {
                        showTip.show("请选择状态", true);
                        isc = false;
                    }
                    else if ($("#txtContents1").val() == "" && $("#rboStatus1_1").prop("checked") == false) {
                        showTip.show("请输入内容", true);

                        $("#txtContents1").focus();
                        isc = false;
                    } else if ($("#rboIsMultiPage1_0").prop("checked") == true) {
                        $("#divContents1 textarea").each(function () {
                            if ($(this).val() == "") {
                                isc = false;
                                showTip.show("请输入内容", true);
                                $(this).focus();
                                return;
                            }
                            testStr += $(this).val() + "!@#$%";

                        })
                        $("#hfContents1").val(testStr);
                    }
                }
                break;
            case "3":
                //取出上传的项目图片
                var list_img = "";
                var type_list = $("#hf_type_name").val();

                $("#img_list img").each(function () {
                    var temp = $(this).attr("lang");
                    list_img += temp + "!@#$";
                    var type = temp.split('|')[0];
                    type_list = type_list.replace(type, "");
                })
                $("#hfImg7").val(list_img);

                if ( $("#hfImg7").val() == "" && $("#rboStatus7_1").prop("checked") == false) {
                    showTip.show("请上传活动相册图片", true);
                    isc = false;
                } 

                var list_temp = type_list.split(',');
                for (var i = 0; i < list_temp.length; i++) {
                    if ($.trim(list_temp[i]) != "") {
                        showTip.show("请上传" + list_temp[i] + "的图片", true);
                        isc = false;
                        break;
                    }
                }
                break;
            case "4":
                if (!$("#txtName1").val()) {
                    showTip.show("请输入嘉宾名字", true);
                    $("#txtName1").focus();
                    isc = false;
                } else if (!ckName($("#txtName1").val())) {
                    showTip.show("嘉宾名字已存在", true);
                    $("#txtName1").focus();
                    return false;
                } else if ($("#rboStatus1_0").prop("checked") == false && $("#rboStatus1_1").prop("checked") == false) {
                    showTip.show("请选择状态", true);
                    isc = false;
                } else if (!$("#txtFile").val()) {
                    showTip.show("请上传嘉宾图片", true);
                    $("#txtFile").focus();
                    isc = false;
                }
                break;
            case "5":
                if ($("#rboStatus5_0").prop("checked") == false && $("#rboStatus5_1").prop("checked") == false) {
                    showTip.show("请选择状态", true);
                    isc = false;
                } else if ($("#txtLink5").val() == "" && $("#rboStatus5_1").prop("checked") == false) {
                    showTip.show("请填写视频地址", true);
                    $("#txtLink5").focus();
                    isc = false;
                }
                
                break;
            case "6":
            case "7":
                if ($("#rboStatus5_0").prop("checked") == false && $("#rboStatus5_1").prop("checked") == false) {
                    showTip.show("请选择状态", true);
                    isc = false;
                } else if ( $("#hfApp5").val() == "" && $("#rboStatus5_1").prop("checked") == false) {
                    showTip.show("请选择一个应用", true);
                    isc = false;
                }
                $("#hfAppHtml5").val($("#div_app_yuyue").html());
                break;
            case "8":
                if ($("#rboStatus1_0").prop("checked") == false && $("#rboStatus1_1").prop("checked") == false) {
                    showTip.show("请选择状态", true);
                    isc = false;
                } else if ($("#AddressText").val() == "" && $("#rboStatus1_1").prop("checked") == false) {
                    showTip.show("请输入详细地址", true);
                    $("#AddressText").focus();
                    isc = false;
                } else if ($("#lat").val() == "" && $("#lng").val() == "" && $("#rboStatus1_1").prop("checked") == false) {
                    showTip.show("请选择一个地理坐标", true);
                    isc = false;
                }
                break;

        }
 

        if (isc == false) {
            return false;

        }
        dialogue.dlLoading();
    });


})


//显示内容
disContents = function (num,status,contents)
{
    if (status==1) {
        $("#spMultiPage" + num).show();
        var html = "";
        for (var i = 0; i < contents.length; i++) {
             html += "<div class=\"divclass\"><textarea name=\"txtContents"+num+"\"   maxlength=\"200\" class=\"textarea\">"+contents[i]+"</textarea> <br /> <input type=\"button\" class=\"btn btn6\" onclick=\"delContent(this)\" value=\"删除\" /> </div>";
        }
        $("#divContents" + num).html(html);
    } else {
        $("#spMultiPage" + num).hide();
        $("#divContents" + num).html("");
    }
}

//添加内容
addContent = function (num)
{
    var div = $("#divContents" + num);
    var html = "<div class=\"divclass\"><textarea name=\"txtContents1\"   maxlength=\"200\" class=\"textarea\"></textarea> <br /> <input type=\"button\" class=\"btn btn6\" onclick=\"delContent(this)\" value=\"删除\" /> </div>";
    div.append(html);
}

//删除内容
delContent = function (this_obj) {
    if (confirm("您确定要删除吗?")) {
        $(this_obj).parent().remove();
    }
   
}

//显示内容操作
showAddContent = function (num, is_multi)
{
    $("#hfIsMultiPage" + num).val(is_multi);
    if (is_multi==0) {
        $("#spMultiPage" + num).hide();
        $("#divContents" + num).hide();
    } else {
        $("#spMultiPage" + num).show();
        $("#divContents" + num).show();
    }
}


function loca2() {

    var myGeo = new BMap.Geocoder();  //<%--新建一个地址翻译类--%>
    var dizhi = $("#AddressText").val(); //<%--组成地址--%>
    myGeo.getPoint(dizhi, function (point) {  //<%--地址翻译成坐标--%>
        if (point) {
            map.centerAndZoom(point, 15);   //<%--将地图中心转向坐标处--%>
            $("#lat").val(point.lat); //<%--控件赋值--%>
            $("#lng").val(point.lng); //<%--控件赋值--%>
            overLay.setPosition(point);  //<%--给覆盖物赋坐标--%>
            map.addOverlay(overLay);   //<%--在地图中添加覆盖物--%>
        }
    }, "北京");
};

function pushTap(num) {
    $(".listNTab a").eq(num).click();
}

//显示图片
function showimg(hfimg, ulimg) {
    var width = 160;
    var height = 265;
    if (hfimg == "hfImg7") {
        width = 180;
        height = 150;
    }
    var hfshow = $("#hfshow").val();
    var list = $("#" + hfimg).val().split(",");
    var li = "";
    for (var i = 0; i < list.length; i++) {
        if ($.trim(list[i]) != "") {
            if (hfshow == "1") {
                li += "<li><img src='" + list[i] + "' width='" + width + "' height='" + height + "'><p></p></li>";
            } else {
                li += "<li><img src='" + list[i] + "' width='" + width + "' height='" + height + "'><p><a href='javascript:' onclick=\"delImg('" + list[i] + "','" + hfimg + "','" + ulimg + "')\">删除</a></p></li>";
            }

        }
    }
    $("#" + ulimg).html(li);
}

//删除图片
function delImg(delvalue, hfimg, ulimg) {
    var width = 160;
    var height = 265;
    if (hfimg == "hfImg7") {
        width = 180;
        height = 150;
    }
    var list = $("#" + hfimg).val().split(",");
    var li = "";
    var list_str = "";
    for (var i = 0; i < list.length; i++) {
        if ($.trim(list[i]) != "" && $.trim(list[i]) != delvalue) {
            li += "<li><img src='" + list[i] + "' width='" + width + "' height='" + height + "'><p><a href='javascript:' onclick=\"delImg('" + list[i] + "','" + hfimg + "','" + ulimg + "')\">删除</a></p></li>";
            if (list_str == "") {
                list_str = list[i];
            } else {
                list_str = list_str + "," + list[i];
            }
        }
    }
    $("#" + hfimg).val(list_str);
    $("#" + ulimg).html(li);
}

function selectApp(project_id, channel_id, hfappid) {
    javascript: bombbox.openBox('select_app.aspx?id=2&project_id=' + project_id + '&channel_id=' + channel_id + '&ids=' + $("#" + hfappid).val());
}

//删除应用
function delApp(this_obj, app_id, hfappid) {
    if (confirm("您确定要删除吗?")) {
        $(this_obj).parent().parent().parent().parent().remove();
        var app_ids = $("#" + hfappid).val();
        var list = app_ids.split(",");
        var new_id = "";
        for (var i = 0; i < list.length; i++) {
            if ($.trim(list[i]) != "") {
                if (list[i] != app_id) {
                    new_id += list[i] + ",";
                }
            }

        }
        $("#" + hfappid).val(new_id);
    }

}

//删除图片相册
function delimghtml(this_obj, dele_text) {
    if (confirm("您确定要删除吗?")) {
        $(this_obj).parent().parent().parent().parent().parent().remove();
        var list = $("#hf_type_name").val().split(',')
        var new_value = "";
        for (var i = 0; i < list.length; i++) {
            if ($.trim(list[i]) != "" && list[i] != dele_text) {
                new_value += list[i] + ",";
            }
        }
        if (new_value.length > 0) {
            new_value = new_value.substr(0, new_value.length - 1);
        }
        $("#hf_type_name").val(new_value);
    }
}

function uploadresult(hf, ul, img, name, type_name) {

    var html = $("#" + ul).html();

    var li = "<li><img src='" + img + "' lang='" + type_name + "|" + name + "|" + img + "' width='180' height='150'><p>" + name + "&nbsp;&nbsp;<a href='javascript:' onclick=\"removeImg(this)\">删除</a></p></li>";
    $("#" + ul).html(html + li);
    bombbox.closeBox();

}

function removeImg(this_obj) {
    if (confirm("您确定要删除吗?")) {
        $(this_obj).parent().parent().remove();
    }
}

function btnadd_click() {
    var typename = $("#txttypename").val();
    if (typename == "") {
        showTip.show("请输入相册名称", true);
        $("#txttypename").focus();
    } else if ($("#hf_type_name").val().indexOf(typename) > -1) {
        showTip.show("相册名称已存在", true);
        $("#txttypename").focus();
    } else {
        var type_name_list = $("#hf_type_name").val();
        if (type_name_list == "") {
            $("#hf_type_name").val(typename);
        } else {
            $("#hf_type_name").val(type_name_list + "," + typename);
        }

        var imgtypecount = parseInt($("#hfimgcont").val()) + 1;
        $("#hfimgcont").val(imgtypecount);
        var hf = "hf_" + imgtypecount;
        var ul = "ul_" + imgtypecount;
        var html = "<div class=\"div_img\">";
        html += "<table cellspacing=\"0\" cellpadding=\"0\" width=\"100%\">";
        html += "<tr> <td style=\"border-bottom: 1px solid #E6E7EC; width: 50%; font-size: 14px; color: #000000\">相册名称：" + typename + "</td>";
        html += "<td style=\"border-bottom: 1px solid #E6E7EC; text-align: right\">";
        html += "<input type=\"button\" class=\"btn btn5\" onclick=\"javascript:bombbox.openBox('upload_img.aspx?hf=" + hf + "&ul=" + ul + "&tname=" + escape(typename) + "');\" value=\"上传图片\" />&nbsp;";
        html += "<input type=\"button\" class=\"btn btn5\" value=\"删除相册\" onclick=\"delimghtml(this,'" + typename + "')\" />";
        html += " </td></tr>";
        html += " <tr> <td colspan=\"2\"> <ul class=\"ul_img\" id=\"" + ul + "\"> </ul></td></tr></table> </div>";
        $("#img_list").append(html);
        $("#txttypename").val("");
    }
}


//选择结果
function selectResult(channel_id, material_id, _path_link, _title, _suumary, _app_link, is_close) {


    if (channel_id == 6 || channel_id == 7) {  
        $("#hfApp5").val(material_id);
        $("#hfAppLink5").val(_app_link);
        $("#hfAppName5").val(_title + "," + _suumary);
        $("#hfAppImg5").val(_path_link);
        $("#div_app_yuyue").show();
        $("#div_app_yuyue").html("<div class=\"app\" style=\"display: block\"><dl class=\"appShow\"><dd style=\"margin-left: 0px;\"><div class=\"img\"><span><img src=\"" + _path_link + "\" alt=\"\" parents=\"app|img\"></span> </div><div class=\"info\"><h2 parents=\"app|title\">" + _title + "</h2><p parents=\"app|content\">" + _suumary + " </p></div></dd> </dl> </div>");

    }
    if (is_close == 1) {
        bombbox.closeBox();
    }

}
