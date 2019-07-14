<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="template_add.aspx.cs" ValidateRequest="false" Inherits="KDWechat.Web.setting.template_add" %>


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
            <div class="titlePanel_01">
                <div class="btns">
                    <asp:LinkButton ID="lbtnBack" runat="server" OnClick="btnCancel_Click" CssClass="btn btn5"><i class="black back"></i>返回</asp:LinkButton>
                </div>
                <h1><%=id==0?"新建":"编辑" %>图文模板</h1>
            </div>

            <div class="listPanel_01 bottomLine">
                <dl>
                    <dt>模板名称：</dt>
                    <dd>
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="txt required" MaxLength="30"></asp:TextBox><i>*</i><em>不超过30个字</em></dd>
                </dl>
                <dl>
                    <dt>模板图片：</dt>
                    <dd>
                        <%--原文件路劲，针对修改的时候--%>
                        <asp:HiddenField ID="hf_old_file" runat="server"></asp:HiddenField>
                        <%--调用编辑器的上传--%>

                        <asp:TextBox ID="txtFile" runat="server" CssClass=" txt " Style="display: none"></asp:TextBox>

                        <div class="simulationPanel_00">
                            <div class="infoField mainInfo ">
                                <div class="img">
                                    <span>
                                        <img src="../images/blank.gif" runat="server" id="img_show" width="268" height="150">
                                    </span>
                                    <div class="tip"><%=id==0?"上传图片":"" %></div>
                                </div>
                            </div>
                        </div>
                        <input type="button" id="btnUpload" class="btn btn6" value="上传">
                        <input type="button"  onclick="javascript:bombbox.openBox('/select_pic.aspx?channel_id=<%=(int)KDWechat.Common.media_type.图文模板图片库 %>');"  class="btn btn6" value="从图片库选择" />
                        <%--自己上传调用方法，Upload_File方法在function.js里需引用，目前file控件样式有问题，这里暂不用--%>
                        
                        <br />
                        <i>*</i><em>图片建议尺寸：228像素 * 348像素，大小：不超过2M,格式：bmp, png, jpeg, jpg, gif</em>
			        </dd>
                </dl>
                <dl class="hidden">
                    <dt>设为默认：</dt>
                    <dd>
                        <asp:RadioButtonList ID="rblDefault" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                            <asp:ListItem Value="1">是</asp:ListItem>
                            <asp:ListItem Value="0" Selected="True">否</asp:ListItem>
                        </asp:RadioButtonList>
                    </dd>
                </dl>

                <dl>
                    <dt>简介说明：</dt>
                    <dd>
                        <textarea id="txtRemark" runat="server"  class="textarea" maxlength="490"></textarea>
                        <br />
                        (说明：模板的简介说明，内部查看。)
                        
                    </dd>
                </dl>
                <dl>
                    <dt>模板内容：</dt>
                    <dd>
                        <asp:TextBox ID="txtContents" runat="server" TextMode="MultiLine" Height="250" CssClass="textarea"></asp:TextBox><br />
                        <i>*</i>(说明：模板内容，微信调用，支持html。) 
                          <br />
                        模板占位符说明：<br />
                        <p>1、$title$ ：表示图文的标题</p>
                        <p>2、$year$、$month$、$day$ ：表示图文的创建日期</p>
                        <p>3、$remark$ ：表示图文的简介</p>
                        <p>4、$contents$ ：表示图文的详细内容</p>
                        <p>5、$news_img$ ：表示图文的图片地址</p>
                        <p>6、$news_author$ ：表示图文的作者</p>
                        <p>7、$read_number$ ：表示图文的阅读数</p>
                        <p>8、$good_number$ ：表示图文的点赞数</p>
                        <p>9、$good_event$ ：表示图文的点赞事件，请写在onclick事件里</p>
                        <p>10、$original_link$ ：表示图文的原文链接地址</p>
                        <p>11、$wechat_head_img$ ：表示微信头像地址</p>
                        <p>12、$wechat_name$ ：表示微信名称</p>
                    </dd>
                </dl>
            </div>

            <div class="btnPanel_01">
                <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn1" Text="保存" OnClick="btnSubmit_Click"></asp:Button>
                <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn btn2" OnClick="btnCancel_Click">取消</asp:LinkButton>
            </div>

        </section>
        <asp:HiddenField ID="hfReturlUrl" runat="server" />
        <asp:HiddenField ID="hftitle" runat="server" />
        <asp:HiddenField ID="hfhas" runat="server" />
        <script src="../scripts/jquery-1.6.4.min.js" type="text/javascript"></script>
        <script src="../scripts/controls.js" type="text/javascript"></script>
         <script src="../Scripts/Bombbox.js"></script>
        <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
        <script src="../Scripts/jquery.validate/jquery.validate.js" type="text/javascript"></script>
        <script src="../Scripts/jquery.validate/messages_cn.js" type="text/javascript"></script>
        <link href="../editor/themes/default/default.css" rel="stylesheet" />
        <script src="../editor/kindeditor-min.js" type="text/javascript"></script>
        <script src="../editor/lang/zh_CN.js" type="text/javascript"></script>
        <script src="../Scripts/function.js" type="text/javascript"></script>

        <script type="text/javascript">
            KindEditor.ready(function (K) {

                <%--调用编辑器的上传加载开始,folder参数表示当前微信号的文件夹--%>
                var editor = K.editor({
                    uploadJson: '../handles/upload_ajax.ashx?action=EditorFile&IsWater=1&folder=<%=folder%>&upload_type=<%=(int)KDWechat.Common.media_type.图文模板图片库%>&write=1',
                    fileManagerJson: '../handles/upload_ajax.ashx?action=ManagerFile&folder=<%=folder%>&upload_type=<%=(int)KDWechat.Common.media_type.图文模板图片库%>',
                    allowFileManager: true
                });
                K('#btnUpload').click(function () {
                    editor.loadPlugin('image', function () {
                        editor.plugin.imageDialog({
                            showRemote: false,
                            imageUrl: K('#txtFile').val(),
                            clickFn: function (url, title, width, height, border, align) {
                                K('#txtFile').val(url);
                                K('#img_show').attr("src", url);
                                K('.tip').html("");
                                editor.hideDialog();

                            }
                        });
                    });

                });
                <%--调用编辑器的上传事件结束--%>


            });
            function ckName(value) {

                $.ajax({
                    type: "POST",
                    async: false,
                    url: "/handles/wechat_ajax.ashx?action=check_exists_name&channel_id=1&cate_id=-1",
                    data: { tb: "<%=KDWechat.Common.DESEncrypt.Encrypt("t_wx_templates")%>", prefix: "<%=KDWechat.Common.DESEncrypt.Encrypt("kd_wechats")%>", new_name: value, old_name: "<%=hftitle.Value%>" },
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

                $("textarea[maxlength]").bind('input propertychange', function () {
                    var maxLength = $(this).attr('maxlength');
                    if ($(this).val().length > maxLength) {
                        $(this).val($(this).val().substring(0, maxLength));
                    }
                });

                $("#txtTitle").blur(function () {
                    if (!$(this).val()) {
                        showTip.show("请输入模板名称", true);

                    } else {
                        if (!ckName($(this).val())) {
                            showTip.show("模板名称已存在", true);
                        }

                    }
                })

                $("#btnSubmit").click(function () {
                    if (!$("#txtTitle").val()) {
                        showTip.show("请输入模板名称", true);
                        //$("#txtTitle").focus();
                        return false;
                    } else if (!ckName($("#txtTitle").val())) {
                        showTip.show("模板名称已存在", true);
                        $("#txtTitle").focus();
                        return false;
                    } else if ($("#txtFile").val() == "") {
                        showTip.show("请上传模板图片", true);
                        return false;
                    } else if ($("#txtContents").val() == "") {
                        showTip.show("请输入模板内容", true);
                        $("#txtContents").focus();
                        return false;
                    }
                    dialogue.dlLoading();//显示Loading
                    form.submit();
                })

            });
            nav.change('<%=m_id%>');
        </script>

    </form>
</body>
</html>

