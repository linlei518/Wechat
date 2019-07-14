<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="key_rule_add.aspx.cs"  ValidateRequest="false" Inherits="KDWechat.Web.keyworld.key_rule_add" %>


<%@ Register Src="../UserControl/TopControl.ascx" TagName="TopControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/MenuList.ascx" TagName="MenuList" TagPrefix="uc2" %>

<!doctype html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=pageTitle %></title>
    <script src="../scripts/html5.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet">
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
                    <a href="<%=hfReturlUrl.Value %>" class="btn btn5"><i class="black back"></i>返回</a>
                </div>
                <h1><%=id==0?"新建":"编辑" %>规则</h1>
            </div>

            <div class="listPanel_01 ">
                <dl>
                    <dt>规则名称：</dt>
                    <dd>
                        <input type="text" class="txt" id="txtrule_name" runat="server" maxlength="50"><i>*</i><em>不超过50个字</em>
                      
                    </dd>
                </dl>
                <dl>
                    <dt>关键词：</dt>
                    <dd id="keywordsInput">
                        <input type="text" id="txtkey" class="txt">
                        <input type="button" class="btn btn3" value="添加"> 
                    </dd>
                </dl>
                <dl class="colspan">
                    <dt>已添加关键词：</dt>
                    <dd id="keywordsList">
                        <input type="hidden" class="hide" id="hfkey" runat="server">
                    </dd>
                </dl>
                <dl>
                    <dt>状　态：</dt>
                    <dd>
                        <label class="radioArea">
                            <asp:RadioButton ID="rboStatusOk" GroupName="rbo" runat="server" Checked="true" Text="启用"></asp:RadioButton></label>
                        <label class="radioArea">
                            <asp:RadioButton ID="rboStatusNo" GroupName="rbo" runat="server" Text="禁用"></asp:RadioButton></label>
                    </dd>
                </dl>
            </div>
            <div class="selectMessageModule_01 bottomLine ">
                <div class="messageNTabPanel_01">
                    <ul>
                        <li><a href="javascript:void(0)" title="文本" class="current"><i class="message1"></i>文本消息</a></li>
                        <li><a href="javascript:bombbox.openBox('select_pic.aspx?channel_id=1');" title="图片"><i class="message2"></i>图片消息</a></li>
                        <li><a href="javascript:bombbox.openBox('select_news.aspx?channel_id=4');" title="单图文"><i class="message3"></i>单图文消息</a></li>
                        <li><a href="javascript:bombbox.openBox('select_multi-news.aspx?channel_id=5');" title="多图文"><i class="message4"></i>多图文消息</a></li>
                        <li><a href="javascript:bombbox.openBox('select_material_list.aspx?channel_id=2');" title="语音"><i class="message5"></i>语音消息</a></li>
                        <li><a href="javascript:bombbox.openBox('select_material_list.aspx?channel_id=3');" title="视频"><i class="message6"></i>视频消息</a></li>
                    <%--    <li><a href="javascript:bombbox.openBox('select_module.aspx?channel_id=8');" title="应用"><i class="message9"></i>应用</a></li>
                        <li><a href="javascript:void(0)" title="多客服"><i class="message11"></i>多客服</a></li>--%>
                    </ul>
                </div>
                <div class="children">
                 
                    <div class="articleEditor noBorder" id="div_text">
                        <div class="text">
                            <textarea class="textarea" id="txtContents" runat="server" maxlength="600"></textarea>
                        </div>
                    </div>
                    <div class="simulationPanel_00" id="div_pic" style="display: none"></div>
                    <div class="simulationPanel_01" id="div_news" style="display: none"></div>
                    <div class="simulationPanel_02" id="div_multi_news" style="display: none"></div>
                    <div class="simulationPanel_03" id="div_voice" style="display: none"></div>
                    <div class="simulationPanel_01" id="div_video" style="display: none"></div>
                    <div class="simulationPanel_01" id="div_module" style="display: none"></div>

                    <div class="listPanel_01" id="div_multiCustomer" style="display: none">
                        <dl>
                            <dt>多客服：</dt>
                            <dd>
                                <span style="color: red">回复对应内容将会激活多客服</span>
                            </dd>
                        </dl>
                    </div>
                </div>
            </div>

            <div class="btnPanel_01">
                <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn1" Text="保存" OnClick="btnSubmit_Click"></asp:Button>
                <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn btn2" OnClick="btnCancel_Click">取消</asp:LinkButton>
            </div>

        </section>
        <asp:HiddenField ID="hfReturlUrl" runat="server" />
        <%--保存消息类型--%>
        <asp:HiddenField ID="hftype" runat="server" Value="0" />
        <%--保存素材id--%>
        <asp:HiddenField ID="hfid" runat="server" Value="0" />
        <%--记录日志的标题--%>
        <asp:HiddenField ID="hflogtitle" runat="server" Value="" />
        <asp:HiddenField ID="hftitle" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hfhas" runat="server"></asp:HiddenField>
        <script src="../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
        <script src="../scripts/controls.js" type="text/javascript"></script>
        <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
        <script src="../scripts/Bombbox.js" type="text/javascript"></script>
        <!--弹出框JS 调用方法：1.开启弹出框：bombbox.openBox('链接地址，可以带参')，2.关闭弹出框：bombbox.closeBox();注意：此方法无需在弹出框里面的页面引用-->
        <script src="../scripts/swfobject.js"></script>
        <script src="../scripts/audio.js"></script>
        <script src="../scripts/video.js"></script>

 
        <%--选择素材的js--%>
        <script src="../Scripts/selectMaterial.js" type="text/javascript"></script>

        <script src="../scripts/addKeywords.js"></script>
        <script>

            $('input[name="timing"]').change(function () {
                if (this.value == '1' && this.checked == true) {
                    $('#setTiming').show();
                } else {
                    $('#setTiming').hide();
                }
            });
            $('#setTiming').hide();

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
        

        <script type="text/javascript">

            function ckName(value) {
                $.ajax({
                    type: "POST",
                    async: false,
                    url: "/handles/wechat_ajax.ashx?action=check_exists_name",
                    data: { tb: "<%=KDWechat.Common.DESEncrypt.Encrypt("t_wx_rules")%>", prefix: "<%=KDWechat.Common.DESEncrypt.Encrypt("kd_wechats")%>", new_name: value, old_name: "<%=hftitle.Value%>" },
                    success: function (response) {
                        $("#hfhas").val(response);
                    }
                });
                if ($("#hfhas").val() == "0") {
                    return true;
                } else {
                    return false;
                }
            }

            $(function () {

                $("#txtrule_name").blur(function () {
                    if (!$(this).val()) {
                        showTip.show("请输入规则名",true);
                       
                    } else {
                        if (!ckName($(this).val())) {
                            showTip.show("规则名已存在", true);
                        }
                    }
                })
                $(".message1").click(function () {
                    firstTextClick();
                });
                $(".message11").click(function () {
                    multiCustomerClick(7);
                });


                <%=loadjs%>

                $("#btnSubmit").click(function () {
                   
                    if (!$("#txtrule_name").val()) {
                        showTip.show("请输入规则名", true);
                        $("#txtrule_name").focus();
                        return false;
                    } else if (!ckName($("#txtrule_name").val())) {
                        showTip.show("规则名已存在", true);
                        $("#txtrule_name").focus();
                        return false;
                    } else if (!$("#hfkey").val()) {
                        showTip.show("请添加关键词", true);
                        $("#txtkey").focus();
                        return false;
                    } else {
                        var type = $("#hftype").val();
                        if (type == "0" && subAll() == false) {
                            showTip.show("文字必须为1到600个字", true);
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
                        } else if (type == "8" && parseInt($("#hfid").val()) <= 0) {
                            showTip.show("请选择一个应用", true);
                            return false;
                        }
                        dialogue.dlLoading();//显示Loading
                    }
                })
            })
            nav.change('<%=m_id%>');
        </script>

    </form>
</body>
</html>

