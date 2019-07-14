
function upadateAttribute(fans_id, chaneel_id, id_list,nick_name,title)
{
    $.ajax({
        type: "POST",
        url: "/handles/wechat_ajax.ashx?action=fans_tag_group&" + "fans_id=" + fans_id + "&chaneel_id=" + chaneel_id + "&id_list=" + escape(id_list) + "&name=" + nick_name + "&title=" + title,
        timeout: 60000,
        beforeSend: function () {
            
        },
        success: function (msg) {
            alert(msg);
        }, error: function (data, status, e) {
        alert("更新失败：" + e);
    }
    });
}

function UpdateData()
{
    
}