<%@ Page EnableEventValidation="false" ValidateRequest="false" Language="C#" AutoEventWireup="true" CodeBehind="SendGroupMessage.aspx.cs" Inherits="KDWechat.Web.GroupMsg.SendGroupMessage" %>


<%@ Register Src="../UserControl/TopControl.ascx" TagName="TopControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/MenuList.ascx" TagName="MenuList" TagPrefix="uc2" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=pageTitle %></title>
    <script src="../scripts/html5.js"></script>
    <script lang="javascript" type="text/javascript" src="../Scripts/DatePicker/WdatePicker.js"></script>
    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/ajaxfileupload.js"></script>
    <script src="../Scripts/jquery.form.js"></script>
    <script>

        function GetList(listString,count)
        {   
            bombbox.closeBox();
            $("#hf_openIDs").val(listString);
            $("#labOpCount").html("发送人数："+count+"人。");
            dialogue.closeAll();

        }


        
    </script>
    <link type="text/css" href="../styles/style.css" rel="stylesheet" />
    <link type="text/css" href="../styles/global.css" rel="stylesheet" />
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>
<body>
    <form id="form1" runat="server">
        <uc1:TopControl ID="TopControl1" runat="server" />
        <uc2:MenuList ID="MenuList1" runat="server" />
        <section id="main">
            <%=NavigationName %>
            <div class="titlePanel_01">
                <div class="btns">
                    <a href="group_messsage_list.aspx?m_id=71" class="btn btn5"><i class="black back"></i>返回</a>
                </div>
            </div>
            <div class="listPanel_01">
                <dl>
                    <dt>进入历史消息：</dt>
                    <dd>
                        <label class="radioArea">
                            <input type="radio" class="radio" value="1" runat="server" id="radSendAll" name="sendAll" />是</label>
                        <label class="radioArea">
                            <input type="radio" class="radio" value="0" id="radSendPart" runat="server" name="sendAll" />否</label>
                        <i>*</i><em>进入历史消息的消息只能群发给所有用户</em>
                    </dd>
                </dl>
                <dl id="dlGroupFilter" runat="server">
                    <dt>群发对象：</dt>
                    <dd>
                        <input onclick="bombbox.openBox('group_msg_filter.aspx');" type="button" class="btn btn6" value="设置群发对象" /><label id="labOpCount" style="margin-left:20px;"><%=showNo %></label>
                    </dd>
                </dl>
                <dl>
                    <dt>群发描述：</dt>
                    <dd>
                        <input class="txt" id="txtTitle" style="" maxlength="30" runat="server" type="text" /><i>*</i><em>不超过30个字</em>
                    </dd>
                </dl>
            </div>
            <div class="selectMessageModule_01">
                <div class="messageNTabPanel_01">
                    <ul>
                        <li><a href="javascript:void(0)" title="文本" class="current"><i class="message1"></i>文本消息</a></li>
                        <li><a href="javascript:bombbox.openBox('../keyworld/select_pic.aspx?channel_id=1');" title="图片"><i class="message2"></i>图片消息</a></li>
                        <li><a href="javascript:bombbox.openBox('../keyworld/select_news.aspx?channel_id=4');" title="单图文"><i class="message3"></i>单图文消息</a></li>
                        <li><a href="javascript:bombbox.openBox('../keyworld/select_multi-news.aspx?channel_id=5');" title="多图文"><i class="message4"></i>多图文消息</a></li>
                        <li><a href="javascript:bombbox.openBox('../keyworld/select_material_list.aspx?channel_id=2');" title="语音"><i class="message5"></i>语音消息</a></li>
                        <%--				        <li><a href="javascript:bombbox.openBox('../keyworld/select_material_list.aspx?channel_id=3');" title="视频"><i class="message6"></i>视频消息</a></li>--%>
                    </ul>
                </div>
                <div class="children">
                    <!--文本消息-->
                    <div class="texchild" id="div_text">
                        <textarea name="txtContents" runat="server" style="width: 99.9%; height: 338px; visibility: hidden;" id="txtContents" class="textarea"></textarea>
                        <div class="infor">
                        	<i>*</i><em>您还可以输入<%=strLength %>个字</em>
                        </div>
                    </div>
                    <!--图片消息-->
                    <div class="simulationPanel_00" id="div_pic" style="display: none"></div>
                    <!--单图文消息-->
                    <div class="simulationPanel_01" id="div_news" style="display: none"></div>
                    <!--多图文消息-->
                    <div class="simulationPanel_02" id="div_multi_news" style="display: none"></div>
                    <!-- 音频消息-->
                    <div class="simulationPanel_03" id="div_voice" style="display: none"></div>
                    <!--  视频消息-->
                    <%--			        <div class="simulationPanel_01" id="div_video" style="display:none"> </div>--%>
                </div>

            </div>
            <div class="listPanel_01 bottomLine">

                <dl>
                    <dt>启用定时发送：</dt>
                    <dd>
                        <label class="radioArea">
                            <input type="radio" class="radio" value="1" runat="server" id="radTimer" name="timing" />是</label>
                        <label class="radioArea">
                            <input type="radio" class="radio" value="0" id="radNotTimer" runat="server" name="timing" />否</label>
                        <input class="txt date" id="setTiming" placeholder="发送时间" runat="server" type="text" onclick="WdatePicker({ dateFmt: 'yyyy/MM/dd HH:mm:ss' })" />
                        <label id="timeLabel" class="error"></label>
                    </dd>
                </dl>

            </div>

<!--            <div>
                <label id="lblError" class="error"></label>
            </div>-->
            <div class="btnPanel_01">
                <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn1" Text="发送" OnClick="btnSubmit_Click"></asp:Button>
                <asp:Button ID="btnOverView" runat="server" CssClass="btn btn1" OnClientClick="return checkOpid()" Text="预览"></asp:Button>
                <input type="button" value="取消" onclick="location.href='group_messsage_list.aspx?m_id=<%=m_id%>'" class="btn btn2" />
                <div class="text">
                    <p>本月已通过系统发送<em><%=canSendCount %></em>次</p>
                </div>
            </div>

        </section>
        <asp:HiddenField ID="hf_openIDs" runat="server" Value="0" />
        <asp:HiddenField ID="overViewOpid" runat="server" Value="0" />



        <%--保存消息类型--%>
        <asp:HiddenField ID="hftype" runat="server" Value="0" />
        <%--保存素材id--%>
        <asp:HiddenField ID="hfid" runat="server" Value="0" />
        <%--记录日志的标题--%>
        <asp:HiddenField ID="hflogtitle" runat="server" Value="" />
        <%--返回地址 ，在父类文件里有封装了记录地址的方法--%>
        <asp:HiddenField ID="hfReturlUrl" runat="server" />
        <%--AJAX POST 的excel的文件名--%>
        <input id="excelfileName" name="excelfileName" type="hidden" value="" />
                    
        <script src="../scripts/selectAddress.js"></script>
        <!--三级联动选择地址的JS-->
        <script src="../scripts/controls.js"></script>
        <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
        <script src="../scripts/Bombbox.js"></script>
        <!--弹出框JS 调用方法：1.开启弹出框：bombbox.openBox('链接地址，可以带参')，2.关闭弹出框：bombbox.closeBox();注意：此方法无需在弹出框里面的页面引用-->
        <script src="../scripts/swfobject.js"></script>
        <script src="../scripts/audio.js"></script>
        <script src="../scripts/video.js"></script>
        <script src="../editor/kindeditor-min.js" type="text/javascript"></script>
        <script src="../editor/lang/zh_CN.js" type="text/javascript"></script>

        <%--选择素材的js--%>
        <script src="../Scripts/selectMaterial.js" type="text/javascript"></script>

        <script type="text/javascript">
            function sendPreView()
            {
                $.ajax({
                    type: "POST",
                    url: "sendGroupMessage.aspx?pre=ss",
                    data: 
                        {
                            hftype:$("#hftype").val(),
                            txtContents:$("#txtContents").val(),
                            hfid:$("#hfid").val(),
                            hflogtitle:$("#hflogtitle").val(),
                            overViewOpid:$("#overViewOpid").val()
                        },
                    success: function (data) {
                        var status = data.split("|")[0];
                        var msg = data.split("|")[1];
                        showTip.show(msg, status=="0");
                    }
                });
            }


            function setOpid(opid,nickName)
            {
                $("#overViewOpid").val(opid);
                $("#btnOverView").val("发送预览给："+nickName);
            }
            function checkOpid()
            {
                var opid =$("#overViewOpid").val();
                if(opid=="0")
                {
                    bombbox.openBox("groupmsg_overview.aspx");
                }
                else
                {
                    $("#btnOverView").attr("disabled","disabled");
                    $("#btnOverView").removeClass("btn1");
                    $("#btnOverView").addClass("btn2");
                    sendPreView();
                }
                return false;
            }
            KindEditor.ready(function (K) {
                var editor = K.create('textarea[name="txtContents"]', {
                    resizeType: 1,
                    pasteType: 1, //纯文本粘贴
                    allowPreviewEmoticons: false,
                    allowImageUpload: false,
                    newlineTag:"br",
                    items: ['emoticons'],
                    afterBlur: function () { this.sync(); },
                    afterCreate: function () {
                        $(".ke-container").attr("style", "border-style:none;");
                        $(".ke-toolbar").attr("style", "border-top:1px solid #CCC;")
                    },
                    afterChange: function () {

                        //限制字数
                        var limitNum = 600;  //设定限制字数
                        var tempLimit = limitNum;
                        var img = $('.ke-edit-iframe').contents().find('.ke-content img');
                        tempLimit -= img.length * 2;
                        if (this.count('text') > limitNum) {
                            //超过字数限制自动截取
                            var strValue = editor.text();

                            strValue = strValue.substring(0, tempLimit);
                            editor.text(strValue);

                        }
                        $(".infor").find("span").find("label").html((tempLimit - parseInt(this.count('text'))));

                    }
                });
            });
            $(function () {

                $(".message1").click(function () {
                    firstTextClick();
                });

                $('#btnSubmit').click(function (){
                    if ($("input:radio[name='timing']:checked").val() == "1")
                    {
                        if ($("#setTiming").val() == "") {
                            showTip.show("请输入发送时间", true);
                            return false;
                        }
                    }
                    var txtArea = $("#txtContents");
                    if($("#txtTitle").val()=="")
                    {
                        showTip.show("请输入描述", true);
                        return false;
                    }
                    if ($("#hftype").val()=="0"&& txtArea.val() == "")
                    {
                        showTip.show("请输入内容", true);
                        return false;
                    }
                    if($("input:radio[name='sendAll']:checked").val() == "0")
                    {
                        if($("#hf_openIDs").val()=="0")
                        {
                            showTip.show("请先选择接收人", true);
                            return false;
                        }
                        else if($("#hf_openIDs").val()=="")
                        {
                            showTip.show("接收人数为0，请重新选择", true);
                            return false;
                        }
                        else if($("#hf_openIDs").val()=="1")
                        {
                            showTip.show("接收人数至少为2人", true);
                            return false;
                        }
                    }
                });

                <%=loadjs%>

            });
            nav.change('<%=m_id%>'); 

            <%=showTime?"":"$('#setTiming').hide();"%>
              
            $('input[name="sendAll"]').change(function () {
                if (this.value == '1' && this.checked == true) {
                    $('#dlGroupFilter').hide();
                } else {
                    $('#dlGroupFilter').show();
                }
            });
            

            $('input[name="timing"]').change(function () {
                if (this.value == '1' && this.checked == true) {
                    $('#setTiming').show();
                } else {
                    $('#setTiming').hide();
                    timeLabel.html("");
                }
            });


        </script>
    </form>
</body>
</html>
