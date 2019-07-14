<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="auto_reply.aspx.cs" Inherits="KDWechat.Web.keyworld.auto_reply" ValidateRequest="false" %>


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

        <div class="selectMessageModule_01">
            <div class="messageNTabPanel_01">
                <ul>
                    <li><a href="javascript:void(0)" title="文本" class="current"><i class="message1"></i>文本消息</a></li>
                    <li><a href="javascript:bombbox.openBox('select_pic.aspx?channel_id=1');" title="图片"><i class="message2"></i>图片消息</a></li>
                    <li><a href="javascript:bombbox.openBox('select_news.aspx?channel_id=4');" title="单图文"><i class="message3"></i>单图文消息</a></li>
                    <li><a href="javascript:bombbox.openBox('select_multi-news.aspx?channel_id=5');" title="多图文"><i class="message4"></i>多图文消息</a></li>
                    <li><a href="javascript:bombbox.openBox('select_material_list.aspx?channel_id=2');" title="语音"><i class="message5"></i>语音消息</a></li>
                    <li><a href="javascript:bombbox.openBox('select_material_list.aspx?channel_id=3');" title="视频"><i class="message6"></i>视频消息</a></li>
            <%--        <li><a href="javascript:bombbox.openBox('select_module.aspx?channel_id=8');" title="应用"><i class="message9"></i>应用</a></li>
                    <li><a href="javascript:void(0)" title="多客服"><i class="message11"></i>多客服</a></li>--%>
                </ul>
            </div>
            <div class="children">
                <!--文本消息-->


                <div class="articleEditor noBorder" id="div_text">
                    <div class="text">
                        <textarea class="textarea" id="txtContents" runat="server" maxlength="600"></textarea>
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
                <div class="simulationPanel_01" id="div_video" style="display: none"></div>
                <!--  模块消息-->
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
          
                <%--保存消息类型--%>
                <asp:HiddenField ID="hftype" runat="server" Value="0" />
                <%--保存素材id--%>
                <asp:HiddenField ID="hfid" runat="server" Value="0" />
                <%--记录日志的标题--%>
                <asp:HiddenField ID="hflogtitle" runat="server" Value="" />
                <%--返回地址 ，在父类文件里有封装了记录地址的方法--%>
                <asp:HiddenField ID="hfReturlUrl" runat="server" />
                 <asp:HiddenField ID="_f" runat="server" />
                <asp:HiddenField ID="c_" runat="server" />
                <asp:HiddenField ID="k_" runat="server" />
                <asp:HiddenField ID="_u" runat="server" />
                <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn1" Text="保存" OnClick="btnSubmit_Click"></asp:Button>
                <asp:Button ID="btnClare" runat="server" CssClass="btn btn2" Enabled="false" Text="清除内容" OnClick="btnClare_Click"></asp:Button>
           
        </div>
    </section>

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

    <script type="text/javascript">


        $('input[name="timing"]').change(function () {
            if (this.value == '1' && this.checked == true) {
                $('#setTiming').show();
            } else {
                $('#setTiming').hide();
            }
        });
        $('#setTiming').hide();
        $(function () {
            nav.change('<%=m_id%>');
                $(".message1").click(function () {
                    firstTextClick();
                });

                $("#btnSubmit").click(function () {
                    return btnSubmitClick();
                });
                $(".message11").click(function () {
                    multiCustomerClick(7);
                });

                <%=loadjs%>

            });




    </script>

     </form>
</body>
</html>

