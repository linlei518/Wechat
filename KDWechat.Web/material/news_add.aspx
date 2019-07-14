﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="news_add.aspx.cs" Inherits="KDWechat.Web.material.news_add" ValidateRequest="false" %>

<%@ Register Src="../UserControl/TopControl.ascx" TagName="TopControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/MenuList.ascx" TagName="MenuList" TagPrefix="uc2" %>
<!doctype html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=pageTitle %></title>
    <script src="../scripts/html5.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet">
    <!--[if lt IE 9 ]><link href="/styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="/styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="/styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>

<body>
    <form id="form1" runat="server">
        <uc1:TopControl ID="TopControl1" runat="server" />
        <uc2:MenuList ID="MenuList1" runat="server" />
        <section <%=isnotop==true?"id=\"bombboxMain\" style='top:5px;'":"id=\"main\"" %>>
            <%=NavigationName %>
            <div class="titlePanel_01">
                <div class="btns">
                    <asp:LinkButton ID="lbtnBack" runat="server" OnClick="btnCancel_Click" CssClass="btn btn5"><i class="black back"></i>返回</asp:LinkButton>
                </div>
                <h1><%=id>0?"编辑":"新建" %>单图文消息</h1>
            </div>

            <div class="graphicMaterialModule_01">
                <div class="simulationPanel_01">
                    <div class="infoField">
                        <div class="title">
                            <h1>请输入标题</h1>
                        </div>
                        <div class="img">
                            <span>
                                <img src="../images/blank.gif" alt="">
                            </span>
                            <div class="tip">封面图片</div>
                        </div>
                        <div class="content">
                        </div>
                    </div>
                </div>
                <div class="listPanel_02">
                    <div class="btns" <%=isnotop==true?"style='display:none'":"" %>>
                     <%-- <%=is_public==0?"<a href=\"javascript:bombbox.openBox('select_materialList.aspx?is_pub=1.1.1');\" class=\"btn btn3\">从公共素材库内复制一条</a>":"" %> --%> <a href="javascript:bombbox.openBox('select_materialList.aspx?is_pub=<%=is_pub %>');" class="btn btn3">从素材库内复制一条</a>
                    </div>
                    <dl>
                        <dt><em>标题</em><i>*</i>（不能超过64个字）</dt>
                        <dd>
                            <input type="text" value="" class="txt jsTitle" maxlength="64"></dd>
                    </dl>
                    <dl>
                        <dt><em>所属分组</em><i>*</i></dt>
                        <dd>
                            <asp:DropDownList ID="ddlGroup" AppendDataBoundItems="true" CssClass="select jsType" runat="server">
                                <asp:ListItem Value="0">默认分组</asp:ListItem>
                            </asp:DropDownList>

                        </dd>
                    </dl>
                    <dl>
                        <dt><em>封面</em><i>*</i>(大图片建议尺寸：900像素 * 500像素,大小：不超过1M,格式：bmp, png, jpeg, jpg, gif)</dt>
                        <dd>
                            <input type="hidden" class="jsImage">
                            <asp:HiddenField ID="hf_old_img" runat="server" />
                            <input type="button" class="btn6 ajaxFile" id="btnUpload" value="上传">
                             <input type="button"  onclick="javascript:bombbox.openBox('/select_pic.aspx?channel_id=<%=(int)KDWechat.Common.media_type.素材图片库 %>&is_pub=<%=is_pub%>&type=news');"  class="btn btn6" value="从图片库选择" />
                        </dd>
                    </dl>
                    <dl>
                        <dt><em>作者</em> (选填)</dt>
                        <dd>
                            <input type="text" value="" class="txt jsIntro" maxlength="10"></dd>
                    </dl>
                    <dl>
                        <dt><em>摘要</em> (不能超过120个字)</dt>
                        <dd>
                            <textarea class="textarea jsSummary" maxlength="120"></textarea></dd>
                    </dl>

                    <div class="listNTab">
                        <a href="javascript:materialAddModule.selectType('article');" class="btn nTabBtn">推送文章</a>
                        <a href="javascript:materialAddModule.selectType('link');" class="btn nTabBtn">关联外链</a>
                        <%--<a href="javascript:materialAddModule.selectType('app');bombbox.openBox('select_appList.aspx');" class="btn nTabBtn">关联应用</a>--%>
                    </div>
                    <div class="article">
                        <dl>
                            <dt><em>正文</em><i>*</i>(必填,不能超过80000个字符)&nbsp;&nbsp;<em><%--<a href="/upload/关注PSD.psd" style="color:red;">点击下载关注图片模板</a>--%></em>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a target="_blank" href="http://wxedit.yead.net/">第三方微信编辑器</a></dt>
                            <dd>
                                 <asp:HiddenField ID="hf_content_img" runat="server" />
                          <textarea id="txtContents" name="txtContents" class="jsText" style="position:relative;z-index:8;"></textarea>
                            </dd>
                        </dl>
                        <dl class="template">
                            <dt><em>当前模板</em></dt>
                            <dd>
                                <div class="img">
                                    <span>
                                        <img src="" alt=""></span>
                                </div>
                                <div class="info">
                                    <h2></h2>
                                </div>
                            </dd>
                        </dl>
                        <dl>
                            <dd>
                                <input type="button" class="btn btn5 setTemplate" value="选择正文模板" onclick="javascript: bombbox.openBox('select_templateList.aspx');"></dd>
                        </dl>
                        <dl>
                            <!--Damos update-->
                            <dt><em>原文链接</em>(<span style="color:red">必须是以http://开头的链接格式</span>,在图文详情页面中会生成“阅读原文”链接)</dt>
                            <dd>
                                <input type="text" value="" class="txt jsOrigin"></dd>
                        </dl>
                    </div>
                    <div class="link">
                        <dl>
                            <dt><em>外链地址</em><i>*</i>(必填，必须是以http://开头的链接格式)<br>
                                (设置后，点击图文消息，不会进入图文详情，而进入外链所设地址。)</dt>
                            <dd>
                                <input type="text" value="" class="txt jsLink"></dd>
                        </dl>
                    </div>
                    <div class="app">
                        <dl class="appShow">
                            <dt><em>关联应用</em><i>*</i></dt>
                            <dd>
                                <div class="img">
                                    <span>
                                        <img src="" alt=""></span>
                                </div>
                                <div class="info">
                                    <h2></h2>
                                    <p></p>
                                </div>
                            </dd>
                        </dl>
                        <dl>
                            <dd>
                                <input type="button" class="btn btn5 setTemplate" value="选择应用" onclick="javascript: bombbox.openBox('select_appList.aspx');"></dd>
                        </dl>
                    </div>
                </div>

            </div>

            <div class="btnPanel_02">
                <asp:HiddenField ID="hfReturlUrl" runat="server" />
                <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn1" Text="保存" OnClientClick="return submitAll();" OnClick="btnSubmit_Click"></asp:Button>
                <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn btn2" OnClick="btnCancel_Click">取消</asp:LinkButton>
            </div>

        </section>
        <asp:HiddenField ID="hfResult" runat="server" />
        <asp:HiddenField ID="hftitle" runat="server" />
        <asp:HiddenField ID="hfhas" runat="server" />
    </form>

    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../scripts/controls.js"></script>
    <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
    <!--如果页面table标签内内容更新 请在更新好后调用一次setupTable方法-->
    <script src="../scripts/Bombbox.js"></script>
    <!--弹出框JS 调用方法：1.开启弹出框：bombbox.openBox('链接地址，可以带参')，2.关闭弹出框：bombbox.closeBox();注意：此方法无需在弹出框里面的页面引用-->
    <script src="../scripts/materialEditor.js"></script>
    <script src="../editor/kindeditor-min.js" type="text/javascript"></script>
    <script src="../editor/lang/zh_CN.js" type="text/javascript"></script>
    <link href="../editor/themes/default/default.css" rel="stylesheet" />
    <script src="../Ueditor/ueditor.config.js"></script>
    <script src="../Ueditor/ueditor.all.js"></script>
    <script src="../Ueditor/zh-cn.js"></script>
     <script  >  
        // $(window).bind('beforeunload', function() {return  '请确保您的内容已保存，避免数据丢失!';} );
    </script>
    <script type="text/javascript">

        /*region实例化ue编辑器*/
        var ue = new UE.getEditor('txtContents');
        UE.getEditor('txtContents').addListener('contentChange', function (editor) {
            //获取编辑器中的内容（html 代码）
            var img = UE.getEditor('txtContents').getContent();
            if (img != "") {                 
                $('.jsText').val(img);
                $('.jsText').get(1).onchange();
                //console.log($('.jsText').val());
            }
        });
        ue.ready(function () {
            //上传图片需要传入参数列表
            ue.execCommand('serverparam', {
                'IsWater':1,
                'folder':'<%=folder%>',
                'upload_type':<%=(int)KDWechat.Common.media_type.素材图片库%>,
                'wx_id':<%=wx_id%>,
                'u_id':<%=u_id%>,
                'is_public':<%=is_public%>
                });             
        });

        //单传图片回调函数，避免添加等待图片的地址
        function uploadSingleImgCallback() { 
            var img = UE.getEditor('txtContents').getContent();
            if (img != "") {                 
                $('.jsText').val(img);
                $('.jsText').get(1).onchange();
                //console.log($('.jsText').val());
            }
        }
        /*regionEnd实例化ue编辑器*/

        KindEditor.ready(function (K) {
            <%--初始化文本框的编辑器--%>

             <%--调用编辑器的上传加载开始,folder参数表示当前微信号的文件夹--%>
            var editor_upload = K.editor({
                uploadJson: '../handles/upload_ajax.ashx?action=EditorFile&IsWater=1&folder=<%=folder%>&upload_type=<%=(int)KDWechat.Common.media_type.素材图片库%>&wx_id=<%=wx_id%>&u_id=<%=u_id%>&is_public=<%=is_public%>',
                fileManagerJson: '../handles/upload_ajax.ashx?action=ManagerFile&folder=<%=folder%>&upload_type=<%=(int)KDWechat.Common.media_type.素材图片库%>',
                allowFileManager: true
            });
           
            K('#btnUpload').click(function () {
                editor_upload.loadPlugin('image', function () {
                    editor_upload.plugin.imageDialog({
                        showRemote: false,
                        imageUrl: K('.jsImage').val(),
                        clickFn: function (url, title, width, height, border, align) {
                            $('.jsImage').val(url);
                            $('.jsImage').get(0).onchange();
                            editor_upload.hideDialog();


                        }
                    });
                });

            });                 <%--调用编辑器的上传事件结束--%>

        });        


        $(function () {
            $("textarea[maxlength]").bind('input propertychange', function () {
                var maxLength = $(this).attr('maxlength');
                if ($(this).val().length > maxLength) {
                    $(this).val($(this).val().substring(0, maxLength));
                }
            });
        })
        

        function getimgBynews(url)
        {
            $('.jsImage').val(url);
            $('.jsImage').get(0).onchange();
            bombbox.closeBox();
        }

             
    </script>


    <script>



        var materialAddModule = new MaterialAddModule(<%=obj%>);//注意！这里是单图文编辑器，如果输入多图文的数据则会自动转化为单图文，取第一条，如果不传任何参数，则为新建数据

        function ck_title() {
            if ($(".jsTitle").val()=="") {
                return false;
                 
            }
            $.ajax({
                type: "POST",
                async: false,
                url: "/handles/wechat_ajax.ashx?action=check_exists_name&channel_id=1",
                data: { tb: "<%=KDWechat.Common.DESEncrypt.Encrypt("t_wx_news_materials")%>", prefix: "<%=KDWechat.Common.DESEncrypt.Encrypt("kd_wechats")%>", new_name: $(".jsTitle").val(), old_name: "<%=hftitle.Value%>" },
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

        function submitAll() {                      
            //点确定时的JS
            if (materialAddModule.checkError()) {
                if (ck_title() == false) {
                    showTip.show("图文标题已存在", true);
                    $(".jsTitle").focus();
                    return false;
                }
                console.log(materialAddModule.data);
                var result = JSON.stringify(materialAddModule.data);
                $("#hfResult").val(result);
                $(window).unbind('beforeunload');
                dialogue.dlLoading();//显示Loading
            } else {
                return false;

            }
        }
        nav.change('<%=m_id%>');

        $("#mainNav a").each(function(){
        
        })
</script>
</body>
</html>
