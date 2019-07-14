
$(function () {

    $(".listNTab a").each(function (i) {
        $(this).click(function () {


            $(".listNTab a").prop("class", "btn nTabBtn");
            $(this).prop("class", "btn nTabBtn current");

            $(".listPanel_01").attr("style", "display:none");
            $(".listPanel_01").eq(i).attr("style", "display:block");
            if (i == 8) {
                $("#loca").click();
            }

        })
    });

    $("#btnSubmit").click(function () {
        var isc = true;
        if ($("#hfImg0").val() == "" && $("#hfshowtype").val()!="6") {
            showTip.show("请上传首页背景图片", true);
            pushTap(0);
            isc = false;
        }

        if (isc) {
            if ($("#txtLink1").val() == "" && $("#rboStatus1_1").prop("checked") == false) {
                showTip.show("请输入微官网地址", true);
                pushTap(1);
                $("#txtLink1").focus();
                isc = false;
            } else if ($("#txtLink1").val() != "" && $("#txtLink1").val().indexOf('http://') != 0) {
                showTip.show("请输入正确的微官网地址", true);
                pushTap(1);
                $("#txtLink1").focus();
                isc = false;
            }
        }

        if (isc) {
            if ($("#txtLink2").val() == "" && $("#txtContents2").val() == "" && $("#rboStatus2_1").prop("checked") == false) {
                showTip.show("项目介绍内容或外链地址必须选择一个", true);
                pushTap(2);
                $("#txtContents2").focus();
                isc = false;
            } else if ($("#txtLink2").val() != "" && $("#txtLink2").val().indexOf('http://') != 0) {
                showTip.show("请输入正确的外链地址", true);
                pushTap(2);
                $("#txtLink2").focus();
                isc = false;
            }
        }


        if (isc) {
            if ($("#txtLink4").val() == "" && $("#txtContents4").val() == "" && $("#rboStatus4_1").prop("checked") == false) {
                showTip.show("交通详细内容或外链地址必须选择一个", true);
                pushTap(3);
                $("#txtLink4").focus();
                isc = false;
            } else if ($("#txtLink4").val() != "" && $("#txtLink4").val().indexOf('http://') != 0) {
                showTip.show("请输入正确的外链地址", true);
                pushTap(3);
                $("#txtLink4").focus();
                isc = false;
            }
        }




        if (isc) {
           

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

                if ($("#txtLink7").val() == "" && $("#hfImg7").val() == "" && $("#rboStatus7_1").prop("checked") == false) {
                    showTip.show("项目图片或外链地址必须选择一个", true);
                    pushTap(4);
                    $("#txtLink7").focus();
                    isc = false;
                } else if ($("#txtLink7").val() != "" && $("#txtLink7").val().indexOf('http://') != 0) {
                    showTip.show("请输入正确的外链地址", true);
                    pushTap(4);
                    $("#txtLink7").focus();
                    isc = false;
                }


                var list_temp = type_list.split(',');
                for (var i = 0; i < list_temp.length; i++) {
                    if ($.trim(list_temp[i]) != "") {
                        showTip.show("请上传" + list_temp[i] + "的图片", true);
                        pushTap(4);
                        isc = false;
                        break;
                    }
                }
 
        }
        if (isc) {
            if ($("#txtLink8").val() == "" && $("#hfApp8").val() == "" && $("#rboStatus8_1").prop("checked") == false) {
                showTip.show("360全景应用或外链地址必须选择一个", true);
                pushTap(5);
                $("#txtLink8").focus();
                isc = false;
            } else if ($("#txtLink8").val() != "" && $("#txtLink8").val().indexOf('http://') != 0) {
                showTip.show("请输入正确的外链地址", true);
                pushTap(5);
                $("#txtLink8").focus();
                isc = false;
            }
            $("#hfAppHtml8").val($("#div_app_360").html());
        }
        if (isc) {
            if ($("#txtLink9").val() == "" && $("#hfApp9").val() == "" && $("#rboStatus9_1").prop("checked") == false) {
                showTip.show("上传户型图或外链地址必须选择一个", true);
                pushTap(6);
                $("#txtLink9").focus();
                isc = false;
            } else if ($("#txtLink9").val() != "" && $("#txtLink9").val().indexOf('http://') != 0) {
                showTip.show("请输入正确的外链地址", true);
                pushTap(6);
                $("#txtLink9").focus();
                isc = false;
            }
            $("#hfAppHtml9").val($("#div_app_3602").html());
        }
        if (isc) {
            var tel = /((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)/;
            var mobile = /^(13[0-9]{9})|(15[0-9]{9})|(18[0-9]{9})|(14[0-9]{9})|(170[0-9]{8})$/;
            if ($("#txtphone").val() == "" && $("#rboStatus6_1").prop("checked") == false) {
                showTip.show("请输入电话号码", true);
                pushTap(7);
                $("#txtphone").focus();
                isc = false;
            } else if ($("#txtphone").val() != "" && (tel.test($("#txtphone").val()) == false && mobile.test($("#txtphone").val()) == false)) {
                showTip.show("请输入正确的电话号码", true);
                pushTap(7);
                $("#txtphone").focus();
                isc = false;
            }
        }

        if (isc) {
            if ($("#lat").val() == "" && $("#lng").val() == "" && $("#rboStatus3_1").prop("checked") == false) {
                showTip.show("请选择一个地理坐标", true);
                pushTap(8);
                isc = false;
            }
        }

        if (isc) {
            if ($("#txtLink5").val() == "" && $("#hfApp5").val() == "" && $("#rboStatus5_1").prop("checked") == false) {
                showTip.show("预约看房应用或外链地址必须选择一个", true);
                pushTap(9);
                $("#txtLink5").focus();
                isc = false;
            } else if ($("#txtLink5").val() != "" && $("#txtLink5").val().indexOf('http://') != 0) {
                showTip.show("请输入正确的外链地址", true);
                pushTap(9);
                $("#txtLink5").focus();
                isc = false;
            }
            $("#hfAppHtml5").val($("#div_app_yuyue").html());
        }

        if (isc == false) {
            return false;

        }
        dialogue.dlLoading();
    });


})

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
            if (hfshow=="1") {
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

//删除图片分类
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
        if (new_value.length>0) {
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
        showTip.show("请输入图片分类名称", true);
        $("#txttypename").focus();
    } else if ($("#hf_type_name").val().indexOf(typename) > -1) {
        showTip.show("分类名称已存在", true);
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
        html += "<tr> <td style=\"border-bottom: 1px solid #E6E7EC; width: 50%; font-size: 14px; color: #000000\">分类名称：" + typename + "</td>";
        html += "<td style=\"border-bottom: 1px solid #E6E7EC; text-align: right\">";
        html += "<input type=\"button\" class=\"btn btn5\" onclick=\"javascript:bombbox.openBox('upload_img.aspx?hf=" + hf + "&ul=" + ul + "&tname=" + escape(typename) + "');\" value=\"上传图片\" />&nbsp;";
        html += "<input type=\"button\" class=\"btn btn5\" value=\"删除分类\" onclick=\"delimghtml(this,'" + typename + "')\" />";
        html += " </td></tr>";
        html += " <tr> <td colspan=\"2\"> <ul class=\"ul_img\" id=\"" + ul + "\"> </ul></td></tr></table> </div>";
        $("#img_list").append(html);
        $("#txttypename").val("");
    }
}


//选择结果
function selectResult(channel_id, material_id, _path_link, _title, _suumary, _app_link, is_close) {


    if (channel_id == 8) {  //全景



        var html = "<div class=\"app\" style=\"display: block\"><dl class=\"appShow\"><dd style=\"margin-left: 0px;\"><div class=\"img\"><span><img src=\"" + _path_link + "\" alt=\"\" parents=\"app|img\"></span> </div><div class=\"info\"><h2 parents=\"app|title\">" + _title + "</h2><p parents=\"app|content\">" + _suumary + " </p><p>&nbsp;</p><p onclick=\"delApp(this," + material_id + ",'hfApp8')\" parents=\"app|content\" style=\"cursor:pointer; color:#2282CE\" >删除</p></div></dd> </dl> </div>";
        var app_html = $("#div_app_360").html();
        var app_id = $("#hfApp8").val();
        var app_link = $("#hfAppLink8").val();


        $("#div_app_360").show();
        if ($.trim(app_id) == "") {
            $("#hfApp8").val(material_id);
            $("#hfAppLink8").val(_app_link);
            $("#div_app_360").html(html);
        } else {
            $("#hfApp8").val(app_id + "," + material_id);
            $("#hfAppLink8").val(app_link + "," + _app_link);
            $("#div_app_360").html(app_html + html);
        }



    } else if (channel_id == 9) {  //户型



        var html = "<div class=\"app\" style=\"display: block\"><dl class=\"appShow\"><dd style=\"margin-left: 0px;\"><div class=\"img\"><span><img src=\"" + _path_link + "\" alt=\"\" parents=\"app|img\"></span> </div><div class=\"info\"><h2 parents=\"app|title\">" + _title + "</h2><p parents=\"app|content\">" + _suumary + " </p><p>&nbsp;</p><p onclick=\"delApp(this," + material_id + ",'hfApp9')\" parents=\"app|content\" style=\"cursor:pointer; color:#2282CE\" >删除</p></div></dd> </dl> </div>";
        var app_html = $("#div_app_3602").html();
        var app_id = $("#hfApp9").val();
        var app_link = $("#hfAppLink9").val();


        $("#div_app_3602").show();
        if ($.trim(app_id) == "") {
            $("#hfApp9").val(material_id);
            $("#hfAppLink9").val(_app_link);
            $("#div_app_3602").html(html);
        } else {
            $("#hfApp9").val(app_id + "," + material_id);
            $("#hfAppLink9").val(app_link + "," + _app_link);
            $("#div_app_3602").html(app_html + html);
        }



    } else if (channel_id == 5) {  //预约
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
