<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="menu_edit.aspx.cs" Inherits="KDWechat.Web.keyworld.menu_edit" %>

<%@ Register Src="../UserControl/TopControl.ascx" TagName="TopControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/MenuList.ascx" TagName="MenuList" TagPrefix="uc2" %>

<!doctype html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=pageTitle %></title>
    <script src="../scripts/html5.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet">
    <link type="text/css" href="../styles/style.css" rel="stylesheet">
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
            <div class="titlePanel_01" style=" ">
                <h1><%=id==0?"新建":"编辑" %>自定义菜单</h1>
            </div>

            <div class="listPanel_01">
                <dl>
                    <dt>选择父菜单：</dt>
                    <dd>
                        <asp:DropDownList ID="ddlMParent" runat="server" CssClass="select required">
                        </asp:DropDownList>

                    </dd>

                </dl>

                <dl>
                    <dt>菜单名称：</dt>
                    <dd>
                        <input type="text" name="mname" id="mname" runat="server" class="txt required" maxlength="16" />

                    </dd>
                </dl>

            </div>

            <div class="selectMessageModule_01">
                <div class="messageNTabPanel_01">
                    <ul>
                        <li class=".link1"><a href="javascript:void(0)" title="文本" class="current"><i class="message1"></i>文本消息</a></li>
                        <li><a href="javascript:bombbox.openBox('select_pic.aspx?channel_id=1');" title="图片"><i class="message2"></i>图片消息</a></li>
                        <li><a href="javascript:bombbox.openBox('select_material_list.aspx?channel_id=4');" title="单图文"><i class="message3"></i>单图文消息</a></li>
                        <li><a href="javascript:bombbox.openBox('select_material_list.aspx?channel_id=5');" title="多图文"><i class="message4"></i>多图文消息</a></li>
                        <li><a href="javascript:bombbox.openBox('select_material_list.aspx?channel_id=2');" title="语音"><i class="message5"></i>语音消息</a></li>
                        <li><a href="javascript:bombbox.openBox('select_material_list.aspx?channel_id=3');" title="视频"><i class="message6"></i>视频消息</a></li>
                         <li><a href="javascript:void(0)" onclick="selectList(6)" title="外部链接"><i class="message7"></i>外部链接</a></li>
                         <li><a href="javascript:void(0)" onclick="selectList(7)" title="授权模块"><i class="message8"></i>授权链接</a></li>
                         <li><a href="javascript:bombbox.openBox('select_module.aspx?channel_id=8');"   title="模块"><i class="message9"></i>模块</a></li>
                    </ul>
                </div>
                <div class="children">
                    <!--文本消息-->
                    <div class="texchild" id="div_text">
                        <textarea name="txtContents" runat="server" style="width: 99.9%; height: 338px; visibility: hidden;" id="txtContents" class="textarea"></textarea>
                        <div class="infor">
                            <span>&nbsp;你还可以输入<label><%=strLength %></label>字</span>
                              <p  >
                        <label id="lblError" style="padding-left:0px;" class="error"></label>
                    </p>
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
                    <!--  外链消息-->
                    <div class="listPanel_01" id="div_link" style="display: none">
                        <dl>
                            <dt>外链地址：</dt>
                            <dd>
                                <input type="text" name="txtlike" style="width:500px" id="txtlike" runat="server" class="txt" maxlength="225" placeholder="http://" onfocus='$("#lblError").html("");' />
                                <br /><span style="color:red">例如:http://www.baidu.com/</span>
                            </dd>
                        </dl>
                    </div>
                    <!--  授权消息-->
                    <div class="listPanel_01" id="div_author" style="display: none">
                        <dl>
                            <dt>授权地址：</dt>
                            <dd>
                                <input type="text" name="txtauthor" style="width:500px" id="txtauthor" runat="server" class="txt" maxlength="225" placeholder="http://" onfocus='$("#lblError").html("");'/>
                                 <br /><span style="color:red">例如:http://www.baidu.com/</span>
                            </dd>
                        </dl>
                    </div>
                    <!--  模块消息-->
                   <div class="simulationPanel_01" id="div_module" style="display: none"> </div>

                </div>

            </div>
          
            <div class="btnPanel_01">
                <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn1" Text="确定" OnClick="btnSubmit_Click"></asp:Button>
                <input id="btnReset" type="reset" value="取消" class="btn btn2" onclick="location.href = 'menu_list.aspx?m_id=<%=m_id%>'" />
                <%--<asp:LinkButton ID="btnCancel" runat="server" CssClass="btn btn2" OnClick="btnCancel_Click">取消</asp:LinkButton>--%>
            </div>
        </section>
           <asp:HiddenField ID="hftitle" runat="server" />
         <asp:HiddenField ID="hfparentid" runat="server" />
        <asp:HiddenField ID="hfhas" runat="server" />
        <%--保存消息类型--%>
        <asp:HiddenField ID="hftype" runat="server" Value="0" />
        <%--保存素材id--%>
        <asp:HiddenField ID="hfid" runat="server" Value="0" />
        <%--记录日志的标题--%>
        <asp:HiddenField ID="hflogtitle" runat="server" Value="" />
        <%--返回地址 ，在父类文件里有封装了记录地址的方法--%>
        <asp:HiddenField ID="hfReturlUrl" runat="server" />

        <%--  <asp:HiddenField ID="hfReturlUrl" runat="server" />--%>
        <script src="../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
        <script src="../scripts/controls.js" type="text/javascript"></script>
        <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
        <script src="../scripts/Bombbox.js" type="text/javascript"></script>
        <!--弹出框JS 调用方法：1.开启弹出框：bombbox.openBox('链接地址，可以带参')，2.关闭弹出框：bombbox.closeBox();注意：此方法无需在弹出框里面的页面引用-->
        <script src="../scripts/swfobject.js"></script>
        <script src="../scripts/audio.js"></script>
        <script src="../scripts/video.js"></script>

        <script src="../Scripts/jquery.validate/jquery.validate.js" type="text/javascript"></script>
        <script src="../Scripts/jquery.validate/jquery.metadata.js" type="text/javascript"></script>
        <script src="../Scripts/jquery.validate/messages_cn.js" type="text/javascript"></script>
        <script src="../editor/kindeditor-min.js" type="text/javascript"></script>
        <script src="../editor/lang/zh_CN.js" type="text/javascript"></script>
        <%--选择素材的js--%>
        <script src="../Scripts/selectMenu.js" type="text/javascript"></script>

        <script type="text/javascript">
            //选中菜单
            nav.change('<%=m_id%>');

            $(function () {
                $.validator.addMethod("checkUserExist", function (value, element) {

                    $.ajax({
                        type: "POST",
                        async: false,
                        url: "/handles/wechat_ajax.ashx?action=check_exists_name&tb=t_wx_diy_menus&parent_id="+$("#ddlMParent").val()+"&old_par=<%=hfparentid.Value%>&prefix=kd_wechats&old_name=<%=hftitle.Value%>&new_name=" + value,
                        success: function (response) {
                            $("#hfhas").val(response);
                        }
                    });
                    if ($("#hfhas").val() == "0") {
                        return true;
                    } else {
                        return false;
                    }

                }, "<font color='#E47068'>菜单名称已存在</font>");

                $("#form1").validate({
                    rules: {
                        mname:{
                            required: true,
                            checkUserExist: true
                        },
                        txtlike: {
                            url: true
                        },
                        txtauthor: {
                            url: true
                        }
                    },
                    messages: {

                        mname:  { required: "请输入菜单名称", checkUserExist: "菜单名称已存在" },
                        txtlike: {
                            url: "</br>请输入以http://开头的地址"

                        },
                        txtauthor: {
                            url: "</br>请输入以http://开头的地址"

                        }
                    }, submitHandler: function (form) {

                        if (btnSubmitClick()) {
                            form.submit();
                        } 

                      
                    }
                });
            });


            KindEditor.ready(function (K) {
                var editor = K.create('textarea[name="txtContents"]', {
                    resizeType: 1,
                    pasteType: 1, //纯文本粘贴
                    allowPreviewEmoticons: false,
                    allowImageUpload: false,
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

                //$(".tableStyle_02 span").hover(function () {
                //    $(this).find(".text").show();
                //}, function () {
                //    $(this).find(".text").hide();
                //});
                $(".message1").click(function () {
                    firstTextClick();
                });

               

                <%=loadjs%>

            });

          
        </script>
    </form>
</body>
</html>
