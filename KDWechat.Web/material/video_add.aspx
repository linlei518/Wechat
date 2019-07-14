<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="video_add.aspx.cs" Inherits="KDWechat.Web.material.video_add" ValidateRequest="false" %>


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
        <section <%=isnotop==true?"id=\"bombboxMain\" style='top:5px;'":"id=\"main\"" %>>
            <%=NavigationName %>
            <div class="titlePanel_01">
                <div class="btns">
                    <asp:LinkButton ID="lbtnBack" runat="server" OnClick="btnCancel_Click" CssClass="btn btn5"><i class="black back"></i>返回</asp:LinkButton>
                </div>
                <h1><%=id==0?"新建":"编辑" %>视频</h1>
            </div>

            <div class="listPanel_01 bottomLine">
                <dl>
                    <dt>视频标题：</dt>
                    <dd>
                        <asp:TextBox ID="txtTitle" runat="server" class="txt " MaxLength="50"></asp:TextBox><i>*</i><em>不超过50个字</em></dd>
                </dl>

                <dl class="newgraphic">
                    <dt>素材分组：</dt>
                    <dd>
                        <asp:DropDownList ID="ddlGroup" runat="server" CssClass="select  " AppendDataBoundItems="true">
                            <asp:ListItem Value="0" Selected="True">默认分组</asp:ListItem>
                        </asp:DropDownList>

                    </dd>
                </dl>



                <dl>
                    <dt>上传封面：</dt>
                    <dd>
                        <asp:HiddenField ID="hf_old_img" runat="server" />
                        <asp:TextBox ID="txtFile" runat="server" class="txt " Style="display: none"></asp:TextBox>
                        <%--调用编辑器的上传--%>
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
                        <input type="button" id="btnUploadImg" class="btn btn6" value=" 上传 ">
                          <input type="button"  onclick="javascript:bombbox.openBox('/select_pic.aspx?channel_id=<%=(int)KDWechat.Common.media_type.素材图片库 %>&is_pub=<%=is_pub%> ');"  class="btn btn6" value="从图片库选择" />
                        <br />
                        <i>*</i>图片建议尺寸：900px * 500px，大小：不超过1M,格式：bmp, png, jpeg, jpg, gif
                    </dd>
                </dl>
                <dl>
                    <dt>视频地址：</dt>
                    <dd>

                        <asp:TextBox ID="txtFile_Weishi" runat="server" class="txt "></asp:TextBox><i>*</i>
                        <label id="lblweixin" class="error"></label>
                        <p>&nbsp;</p>
                        <p class="frm_tips">暂时只支持<a href="http://www.weishi.com/" target="_blank">微视</a>，如：http://www.weishi.com/t/2000548113094025?pgv_ref=weishi.channel.fall.img</p>
                    </dd>
                    <%-- <dd><input type="button" id="btnLoalUpload" class="btn btn6" value="本地上传">&nbsp;<input type="button" id="btnNetWork" class="btn btn6" value="微视"> <label id="lblvideo_type" class="error"></label></dd>--%>
                </dl>
             
                <dl id="dlNetWork" style="display: none">
                    <dt></dt>

                </dl>
                <dl style="display:none">
                    <dt>简介说明：</dt>
                    <dd>
                        <asp:TextBox ID="txtContents" runat="server" TextMode="MultiLine" CssClass="textarea"></asp:TextBox>
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
        <script src="../Scripts/jquery.form.js" type="text/javascript"></script>
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
                    uploadJson: '../handles/upload_ajax.ashx?action=EditorFile&IsWater=1&folder=<%=folder%>&upload_type=<%=(int)KDWechat.Common.UploadType.视频%>',
                    fileManagerJson: '../handles/upload_ajax.ashx?action=ManagerFile&folder=<%=folder%>&upload_type=<%=(int)KDWechat.Common.UploadType.视频%>',
                    allowFileManager: true
                });

                var editor2 = K.editor({
                    uploadJson: '../handles/upload_ajax.ashx?action=EditorFile&IsWater=1&folder=<%=folder%>&upload_type=<%=(int)KDWechat.Common.media_type.素材图片库%>&is_public=<%=is_public%>',
                    fileManagerJson: '../handles/upload_ajax.ashx?action=ManagerFile&folder=<%=folder%>&upload_type=<%=(int)KDWechat.Common.media_type.素材图片库%>',
                    allowFileManager: true
                });
               
                K('#btnUploadImg').click(function () {
                    editor2.loadPlugin('image', function () {
                        editor2.plugin.imageDialog({
                            showRemote: false,
                            imageUrl: K('#txtFile').val(),
                            clickFn: function (url, title, width, height, border, align) {
                                K('#txtFile').val(url);
                                K('#img_show').attr("src", url);
                                K('.tip').html("");
                                editor2.hideDialog();

                            }
                        });
                    });

                });

                <%--调用编辑器的上传事件结束--%>






            });
        </script>
        <script type="text/javascript">
            function ckName(value) {
                $.ajax({
                    type: "POST",
                    async: false,
                    url: "/handles/wechat_ajax.ashx?action=check_exists_name&channel_id=3",
                    data: { tb: "<%=KDWechat.Common.DESEncrypt.Encrypt("t_wx_media_materials")%>", prefix: "<%=KDWechat.Common.DESEncrypt.Encrypt("kd_wechats")%>", new_name: value, old_name: "<%=hftitle.Value%>" },
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
                $("#txtTitle").blur(function () {
                    if (!$(this).val()) {
                        showTip.show("请输入视频标题", true);

                    } else {
                        if (!ckName($(this).val())) {
                            showTip.show("视频标题已存在", true);
                        }

                    }
                })



                $("#btnSubmit").click(function () {
                    if (!$("#txtTitle").val()) {
                        showTip.show("请输入视频标题", true);
                        //$("#txtTitle").focus();
                        return false;
                    } else if (!ckName($("#txtTitle").val())) {
                        showTip.show("视频标题已存在", true);
                        $("#txtTitle").focus();
                        return false;
                    } else if ($("#txtFile").val() == "") {
                        showTip.show("请上传封面图片", true);
                        return false;
                    } else if (!$("#txtFile_Weishi").val()) {
                        showTip.show("请填写微视地址", true);
                        $("#txtFile_Weishi").focus();
                        return false;
                    } else if ($("#txtFile_Weishi").val().toLocaleLowerCase().indexOf("http://www.weishi.com") < 0) {
                        showTip.show("请填写正确的微视地址", true);
                        $("#txtFile_Weishi").focus();
                        return false;
                    }
                    dialogue.dlLoading();//显示Loading
                    form.submit();
                })


 

                $("#btnLoalUpload").click(function () {
                    $("#dlNetWork").hide();
                    $("#dlLoalUpload").show();
                    $("#hf_video_type").val("1");
                })

                $("#btnNetWork").click(function () {
                    $("#dlNetWork").show();
                    $("#dlLoalUpload").hide();
                    $("#hf_video_type").val("2");
                })
            });
            nav.change('<%=m_id%>');
        </script>



    </form>
</body>
</html>
