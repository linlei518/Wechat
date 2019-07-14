<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="group_msg_filter.aspx.cs" Inherits="KDWechat.Web.GroupMsg.group_msg_filter" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../scripts/html5.js"></script>
    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/jquery.form.js"></script>
    <script src="../scripts/selectAddress.js"></script>
    <!--三级联动选择地址的JS-->
    <script src="../scripts/controls.js"></script>
    <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->

    <script>
        var opidList = "";

        //获取文件扩展名
        function getExtName(file_name) {
            var result = /\.[^\.]+/.exec(file_name);
            return result;
        }
        //手动组合，放弃controls.js
        function fbtClic() {
            $("#filExcelList").click();
        }

        //ajax异步提交form用于计算筛选条件中包含的人数
        function ajaxFormSubmit() {
            $("#messageShower").html("");
            var ajax_option = {
                url: "group_msg_filter.aspx?ut=1",//默认是form action
                async: false,
                success: function (data) {
                    var shower = $("#messageShower");
                    var list = data.split("|");
                    shower.html(list[0]);
                    opidList = list[1];
                }
            };
            $('#form1').ajaxSubmit(ajax_option);
            return false;
        }

        //ajax 异步上传excel并获取其中条数信息
        function ajaxFileUpload(e) {
            $("#fileText").val($("#filExcelList").val());
            var elementIds = ["flag"];
            var myDate = new Date();//新建日期对象
            var time = myDate.getTime();//获取毫秒数（唯一）
            var extName = getExtName($("#filExcelList").val());
            if (extName != ".xls" && extName != ".xlsx") {
                showTip.show("请上传以.xls或.xlsx结尾的文件", true);
                return false;
            }
            time += extName//为毫秒数添加文件扩展名
            $("#excelfileName").val(time);//存储文件名（用于form提交之后提取openid列表）
            var ajax_option = {
                url: 'group_msg_filter.aspx?s=' + time,
                success: function (data) {
                    var list = data.split("|");
                    if (list[0] == 1) {
                        var shower = $("#messageShower");
                        var upCount = list[1];
                        var totalCount = list[2];
                        showTip.show("共上传了 " + upCount + " 个用户，其中重复 " + (upCount - totalCount).toString() + " 个，有效 " + totalCount + " 个。", false);
                        opidList = list[3];
                    }
                    else {

                        showTip.show("请上传正确格式的文件", true);
                    }
                }
            };
            $('#form1').ajaxSubmit(ajax_option);

        }

        function isIE() { //ie?
            if (!!window.ActiveXObject || "ActiveXObject" in window) {
                console.log('isie');
                return true;
            }
            else
                return false;
        }

        function checkForm() {
            dialogue.dlLoading();
            var filterType = $("#ddlSendGroup").find("option:selected").val();
            if (filterType == "listGroup2" && $("#excelfileName").val() == "") {
                showTip.show("请上传文件", true);
                dialogue.closeAll();
                return false;
            }
        }


    </script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet" />
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->

</head>
<body class="bombbox">
    <form id="form1" runat="server">
        <div>
            <header id="bombboxTitle">
                <div class="titlePanel_01">
                    <h1>选择群发接收人</h1>
                </div>
            </header>
            <section id="bombboxMain">
                <div class="listPanel_01 bottomLine">
                    <dl>
                        <dt>对象来源：</dt>
                        <dd>
                            <select class="select" runat="server" id="ddlSendGroup" onchange="$('.listGroup').hide();$('.'+this.value).show();$('#messageShower').html('');">
                                <option selected="selected" value="listGroup1">系统内部筛选</option>
                                <option value="listGroup2">上传会员名单EXCEL</option>
                            </select>
                        </dd>
                    </dl>
                    <dl class="listGroup listGroup1">
                        <dt>分组标签筛选：</dt>
                        <dd>
                            <asp:DropDownList ID="ddlGroup" AppendDataBoundItems="true" CssClass="select jsType" runat="server">
                                <asp:ListItem Value="-2">选择分组</asp:ListItem>
                                <asp:ListItem Value="-1">全部分组</asp:ListItem>
                                <asp:ListItem Value="0">默认分组</asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlTag" CssClass="select" AppendDataBoundItems="true" runat="server">
                                <asp:ListItem Value="-2">选择标签</asp:ListItem>
                                <asp:ListItem Value="-1">全部标签</asp:ListItem>
                                <asp:ListItem Value="0">无标签</asp:ListItem>
                            </asp:DropDownList>

                        </dd>
                    </dl>
                    <dl class="listGroup listGroup1">
                        <dt>地区筛选：</dt>
                        <dd>
                            <select class="select" name="Province">
                            </select><select class="select" name="City">
                            </select><select class="select" name="Area">
                            </select>
                        </dd>
                    </dl>
                    <dl class="listGroup listGroup1">
                        <dt>性别筛选：</dt>
                        <dd>
                            <label>
                                <input runat="server" id="radSexAll" type="radio" class="radio" name="sex" />全部</label>
                            <label class="radioArea">
                                <input type="radio" runat="server" id="radSexMale" class="radio" name="sex" />男</label>
                            <label class="radioArea">
                                <input type="radio" runat="server" id="radSexFemale" class="radio" name="sex" />女</label>
                            <label>
                                <input type="radio" class="radio" name="sex" runat="server" id="radSexUnknow" />未知</label>
                            <%--                            <asp:Button ID="btnSearchCount" runat="server" CssClass="btn btn5" style="margin-left: 20px;" Text="查询人数" OnClientClick="return ajaxFormSubmit()" />--%>
                        </dd>
                    </dl>
                    <dl class="listGroup listGroup2">
                        <dt>上传名单：</dt>
                        <dd>
                            <div class="fileAera" style=" width:450px">
                             <a style=" width:460px; display:block; height:33px; overflow:hidden; position:relative"><asp:FileUpload ID="filExcelList" runat="server" style="filter:alpha(opacity=0); opacity:0;  font-size:100px; width:400px; position:absolute; z-index:2" onchange="ajaxFileUpload(this);"  /> 
                             <div  style="  position:absolute; z-index:1">  <input id="fileText" disabled="disabled" class="txt" type="text" style=" display:inline-block; vertical-align:middle" />
                                <input id="fileLiuLan" class="btn btn5"  value="浏览..." type="button" style=" display:inline-block; vertical-align:middle"/></div> </a>
                                <a href="../upload/excel/openIDs.xls">下载模板</a><label style="padding-left: 4px" id="messageShower"></label>
                           
                            </div>
                        </dd>
                    </dl>

                </div>
                <input id="excelfileName" name="excelfileName" type="hidden" value="" />

                <div class="btnPanel_01">
                    <asp:Button ID="Button1" runat="server" OnClientClick="return checkForm()" CssClass="btn btn1" Text="确定" OnClick="Button1_Click" />
                    <asp:LinkButton ID="btnCancel" Name="btnCancel" runat="server" CssClass="btn btn2">取消</asp:LinkButton>
                </div>

            </section>
        </div>
    </form>
    <script>
        $("#btnCancel").click(function () {
            parent.bombbox.closeBox();
        });
        new PCAS("Province", "City", "Area");

        var offsetSize = {//这玩意定义弹出框的高宽
            width: 600,
            height: 400
        }
    </script>
</body>
</html>
