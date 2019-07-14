/*
分页跳转指定页
pageUrl：跳转地址
pageCount：总页数
*/
function goLink(pageUrl, pageCount) {

    var page = $("#txtpage").val();
    if (page == "" || page == "0") {
        alert("请输入页数");
        $("#txtpage").focus();

    } else {
        page = parseInt(page);
        if (page > pageCount) {
            alert("您输入的页数不存在");
            $("#txtpage").focus();

        } else {
            var linkUrl = pageUrl.replace("__id__", page);
            location.href = linkUrl;
        }
    }
}

//子页面关闭
function closeBox(reload_page)
{
    parent.bombbox.closeBox();
    if (arguments.length==1) {
        window.parent.location.replace(window.parent.location.href);
    }
}

//关闭子窗口，并刷新提示父窗口,type=(success=成功 ，fail=失败) msg=提示内容
function backParentPage(type, msg) {
    parent.bombbox.closeBox();
    var link_url = parent.location.href;
    var list = link_url.split(type);
    if (list.length>1) {
        parent.location.replace(list[0] + type + "=" + msg + "");
        parent.location.reload();
    } else {
        parent.location.replace(list[0] + "#" + type + "=" + msg + "");
        parent.location.reload();
    }
    
    

}

function getimg(hf_img_id,show_img_id,img_url)
{
    $("#" + hf_img_id).val(img_url);
    $("#" + show_img_id).attr("src", img_url);
    $(".tip").html("");
    bombbox.closeBox();
}


/*
jquery.form文件上传(自定义文件夹，并且不更改文件名称,以下的控件都是input 控件，如需其他控件请更改赋值的地方)
upload_file：需要上传的控件
return_repath：上传成功后返回完整文件路劲给需要的控件 
return_type：上传成功后返回文件后缀给需要的控件
return_size：上传成功后返回文件大小给需要的控件
return_org_name：上传成功后返回文件原名称给需要的控件
folder：设置上传的文件夹
is_rename：是否对文件重命名
is_cover：是否覆盖已存在的文件(1、覆盖 0不覆盖)
upload_type：上传类型（1、图片 2、音频 3 、视频  4、其他）
*/
function Upload_File(upload_file, return_repath, return_type, return_size, return_org_name, folder, is_rename, is_cover, upload_type) {
    var sendUrl = "../handles/upload_ajax.ashx?action=PathFile&ReFilePath=" + return_repath + "&UpFilePath=" + upload_file + "&folder=" + folder + "&is_cover=" + is_cover + "&is_rename=" + is_rename + "&upload_type=" + upload_type;

    //开始提交
    $("#form1").ajaxSubmit({
        beforeSubmit: function (formData, jqForm, options) {
           
        },
        success: function (data, textStatus) {
            if (data.status == 1) {
                $("#" + return_repath).val(data.path);
                $("#" + return_type).val(data.ext);
                $("#" + return_size).val(data.size);
                $("#" + return_org_name).val(data.name);

            } else {
                alert(data.msg);
            }
            
        },
        error: function (data, status, e) {
            alert("上传失败：" + e);
             
        },
        url: sendUrl,
        type: "post",
        dataType: "json",
        timeout: 600000
    });
};

