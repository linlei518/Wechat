<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="user_detail.aspx.cs" Inherits="KDWechat.Web.fans.user_detail" %>

<%@ Register Src="../UserControl/TopControl.ascx" TagName="TopControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/MenuList.ascx" TagName="MenuList" TagPrefix="uc2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8">
    <title><%=pageTitle %></title>
    <script src="../scripts/html5.js" type="text/javascript"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet">
    <!--[if lt IE 9 ]><link href="styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <style type="text/css">
        .checkbox {
            margin: auto;
        }

        .checkBoxList label {
            padding: inherit;
            vertical-align: bottom;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <uc1:TopControl ID="TopControl1" runat="server" />
        <uc2:MenuList ID="MenuList1" runat="server" />
        <section id="main">
            <%=NavigationName %>
            <div class="titlePanel_01">
                <div class="btns">
                    <%--<asp:LinkButton ID="lbtnExport" Visible="false" runat="server" CssClass="btn btn5" OnClick="lbtnExport_Click">导出数据</asp:LinkButton>--%>
                    <a href="javascript:history.back(1)" class="btn btn5"><i class="black back"></i>返回</a>
                </div>
                <h1>用户详细资料</h1>
            </div>

            <div class="userDetailModule_01">

                <div class="userBaseInfo" id="user0">
                    <div class="img">
                        <span>
                            <img src="images/pic_03.jpg" alt="" id="img_head" runat="server">
                        </span>
                    </div>
                    <div class="group">
                        <input type="hidden" class="hide">
                        <span>所在分组 ：</span><em class="selected"><asp:Literal ID="lblGroupName" runat="server"></asp:Literal></em>
                        <input type="button" class="btn btnSelect" title="修改分组" id="btnModifyGroup" runat="server" onclick="userTypeController.open(this)">
                    </div>
                    <div class="info">
                        <h2><em>
                            <asp:Literal ID="lblNickName" runat="server"></asp:Literal></em>
                            <asp:Literal ID="lblMemberName" runat="server"></asp:Literal></h2>
                        <p>信息状态 ：<asp:Literal ID="lblReplyStatus" runat="server"></asp:Literal></p>
                    </div>
                    <div class="tags">
                        <input type="hidden" class="hide">
                        <span>用户标签 ：</span>
                        <asp:Literal ID="lblFansTags" runat="server"></asp:Literal>
                        <input type="button" class="btn btnAdd" title="修改标签" id="btnModifyTag" runat="server" onclick="userTagController.open(this)">
                    </div>
                    <div class="btns" id="divShowMsg" runat="server">
                        <a href="<%=showMsg %>" class="btn btn5">查看消息</a>
                        <asp:HiddenField ID="hfopenid" runat="server" />
                        <span>
                            <asp:Literal ID="lblLastMsgTime" runat="server"></asp:Literal></span>
                    </div>
                </div>

                <div class="infoPanel_01">
                    <dl class="colspan">
                        <dt>关注来源：</dt>
                        <dd><span>
                            <asp:Literal ID="lblsource" runat="server"></asp:Literal></span></dd>
                    </dl>
                    <dl class="colspan">
                        <dt>已关注微信号：</dt>
                        <dd>
                            <asp:Literal ID="lblwechats" runat="server"></asp:Literal></dd>
                    </dl>
                </div>
                <div class="infoPanel_01">
                    <div class="title">
                        <h2>微信信息</h2>
                    </div>
                    <dl>
                        <dt>性别：</dt>
                        <dd>
                            <asp:Literal ID="lblSex" runat="server"></asp:Literal></dd>
                        <dt>国家：</dt>
                        <dd>
                            <asp:Literal ID="lblcounty" runat="server"></asp:Literal></dd>
                    </dl>
                    <dl>
                        <dt>地区：</dt>
                        <dd>
                            <asp:Literal ID="lblcity" runat="server"></asp:Literal></dd>
                        <dt>语言：</dt>
                        <dd>
                            <asp:Literal ID="lbllanuage" runat="server"></asp:Literal></dd>
                    </dl>
                </div>

                <div class="btnPanel_01">
                    <asp:Button ID="btnsynchro" runat="server" OnClientClick="dialogue.dlLoading();" CssClass="btn btn5" Text="同步微信信息" OnClick="btnsynchro_Click" />
                    <asp:HiddenField ID="hfReturnUrl" runat="server"></asp:HiddenField>
                    <asp:Button ID="btnCancel" runat="server" CssClass="btn btn2" Text="取消" OnClick="btnCancel_Click"></asp:Button>
                </div>


          
                    
                </div>
        

        </section>


        <script src="../scripts/jquery-1.10.2.min.js"></script>
        <script src="../scripts/controls.js"></script>
        <script src="../Scripts/DatePicker/WdatePicker.js"></script>
        <script src="../Scripts/jquery.validate/jquery.validate.js" type="text/javascript"></script>
        <script src="../Scripts/jquery.validate/messages_cn.js" type="text/javascript"></script>
        <script src="../Scripts/function.js"></script>
        <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
        <script>
            nav.change('<%=m_id%>');//此处填写被激活的导航项的ID
        </script>
        <script src="../scripts/userList.js"></script>
        <script src="../scripts/selectAddress.js"></script>
        <!--三级联动选择地址的JS-->
        <script>
            <%=selectCity%>
        </script>
        <script src="../scripts/addKeywords.js"></script>
        <script src="../Scripts/fans.js"></script>
        <script type="text/javascript" src="http://api.map.baidu.com/api?v=2.0&ak=DD610e50dbdc31173edc1d5c92be4076"></script>
        <script>
            var keywords = new AddKeywords({
                inputer: {
                    text: $('#keywordsInput .txt'),
                    btn: $('#keywordsInput .btn'),
                    show: $('#keywordsList'),
                    save: $('#keywordsList .hide')
                },
                hideDom: $('#keywordsList').parent()
            });

        </script>
        <script>
            $(function () {
                $("textarea[maxlength]").bind('input propertychange', function () {
                    var maxLength = $(this).attr('maxlength');
                    if ($(this).val().length > maxLength) {
                        $(this).val($(this).val().substring(0, maxLength));
                    }
                });
            })
            userTagController.readTag(<%=tagList%>);//这个数组是所有的标签名字
            userTagController.tagSubmit = function (tags,oldTag,lengthOverFlag) {
                var addInfo = '';
                if (lengthOverFlag) addInfo = '部分用户标签超出10条，已经截取为10条';
                if (addInfo=="") {
                    var tipString = '';
                    for (var i in tags) {
                        var tempTags ="";
                        for (var j in tags[i].tags) {
                            tempTags+="{\"id\":\"" + tags[i].tags[j].id + "\",\"name\":\"" + tags[i].tags[j].name + "\"},";
                        
                        }
                        tipString += tempTags;
                    
                    }
                    upadateAttribute('<%=id%>', 2, tipString,'<%=lblNickName.Text%>','');
                }else{
                    showTip.show(addInfo, false);
                }     
           
            }

            userTypeController.readType(<%=groupList%>);//这个数组是所有的分组名字
            userTypeController.typeSubmit = function (ids, checked, value, oldTag) {
                upadateAttribute('<%=id%>', 1, value,'<%=lblNickName.Text%>',checked);
            }
        </script>
        <script>
            var map = new BMap.Map("allmap"); <%--新建地图--%>
            var overLay = new BMap.Marker();  <%--新建一个覆盖图--%>
            map.enableScrollWheelZoom();   <%--适用滚轮缩放--%>
            map.addControl(new BMap.NavigationControl()); <%--添加缩放控件--%>
        
            function loca2() {
                <%=lng==0?"<!--":""%>
                var myGeo = new BMap.Geocoder();  <%--新建一个地址翻译类--%>

                var point =  new BMap.Point(<%=lng%>,<%=lat%>);
                map.centerAndZoom(point, 15);   <%--将地图中心转向坐标处--%>
                overLay.setPosition(point);  <%--给覆盖物赋坐标--%>
                map.addOverlay(overLay);   <%--在地图中添加覆盖物--%>
                <%=lng==0?"--!>":""%>
            };
   
        </script>
        <script type="text/javascript">
            $(function () {

                var is_wuye="<%=is_wuye%>";
                if (is_wuye=="1") {
                    $("#dl_wuye").show();
                    $("#dl_yigou").show();
                }else{
                    $("#dl_wuye").hide();
                    $("#dl_yigou").hide();
                }
                $("#rboYes").click(function(){
                    $("#dl_wuye").show();
                    $("#dl_yigou").show();
                });

                $("#rboNo").click(function(){
                    $("#dl_wuye").hide();
                    $("#dl_yigou").hide();
                })

                $("#btnSubmit").click(function(){
                    var mobile =/(^18\d{9}$)|(^13\d{9}$)|(^15\d{9}$)|(^17\d{9}$)|(^14\d{9}$)/;
                    var tel = /^\d{3,4}-?\d{7,9}$/;
                    var zip = /^[0-9]{6}$/;  
                    var qq = /^\d$/;
                    var mail=/^[a-z0-9]+([._\\-]*[a-z0-9])*@([a-z0-9]+[-a-z0-9]*[a-z0-9]+.){1,63}[a-z0-9]+$/;
                    var ids = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;  
                    if ($("#txtMobile").val()!="" && !mobile.test($("#txtMobile").val())) {
                        showTip.show("请输入11位的手机号码",true);
                        $("#txtMobile").focus();
                        return false;
                    }else if ($("#txtZipCode").val()!="" && !zip.test($("#txtZipCode").val())) {
                        showTip.show("请输入正确的邮编",true);
                        $("#txtZipCode").focus();
                        return false;
                    }else  if ($("#txtMail").val()!="" && !mail.test($("#txtMail").val())) {
                        showTip.show("请输入正确的邮箱",true);
                        $("#txtMail").focus();
                        return false;
                    }else  if ($("#txtPhone").val()!="" && !tel.test($("#txtPhone").val())) {
                        showTip.show("请输入正确的电话号码,不带分机",true);
                        $("#txtPhone").focus();
                        return false;
                    }else  if ($("#txtQQ").val()!="" && !qq.test($("#txtQQ").val())) {
                        showTip.show("请输入正确的QQ号码",true);
                        $("#txtQQ").focus();
                        return false;
                    }
                    dialogue.dlLoading();//显示Loading

                })

                

                $("#loca").click();
            })
        </script>
    </form>
</body>
</html>
