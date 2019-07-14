<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pic_add.aspx.cs" Inherits="KDWechat.Web.material.pic_add" %>


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
                <h1><%=id==0?"新建":"编辑" %>图片</h1>
            </div>

            <div class="listPanel_01 bottomLine">
                <dl>
                    <dt>图片标题：</dt>
                    <dd>
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="txt " MaxLength="50"></asp:TextBox><i>*</i><em>不超过50个字</em></dd>
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
                    <dt>上传图片：</dt>
                    <dd>
                        <%--原文件路劲，针对修改的时候--%>
                        <asp:HiddenField ID="hf_old_file" runat="server"></asp:HiddenField>
                        <%--文件后缀--%>
                        <asp:HiddenField ID="hf_type" runat="server" />
                        <%--文件原文件名称--%>
                        <asp:HiddenField ID="hf_name" runat="server" />
                        <%--文件大小--%>
                        <asp:HiddenField ID="hf_size" runat="server" />


                        <%--调用编辑器的上传--%>


                        <asp:TextBox ID="txtFile" runat="server" CssClass="required " Style="display: none"></asp:TextBox>

                        <div class="simulationPanel_00">
                            <div class="infoField mainInfo ">
                                <div class="img">
                                    <span>
                                        <img src="../images/blank.gif" runat="server" id="img_show" >
                                    </span>
                                    <div class="tip"><%=id==0?"上传图片":"" %></div>
                                </div>
                            </div>
                        </div>
                        <input type="button" id="btnUpload" class="btn btn6" value="浏览...">
                        <%--自己上传调用方法，Upload_File方法在function.js里需引用，目前file控件样式有问题，这里暂不用--%>
                        <%--<input type="file" class="file" id="file_img" name="file_img" onchange="Upload_File('file_img','txtFile','hf_type','hf_size','hf_name','<%=folder %>',1,1,<%=(int)KDWechat.Common.media_type.素材图片库%>)">--%>
                        <i>*</i><em>图片建议大小：不超过2M,格式：bmp, png, jpeg, jpg, gif</em>
                        
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
        <script src="../Scripts/jquery.form.js"></script>
        <script src="../scripts/controls.js" type="text/javascript"></script>

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
                    uploadJson: '../handles/upload_ajax.ashx?action=EditorFile&IsWater=1&folder=<%=folder%>&upload_type=<%=(int)KDWechat.Common.media_type.素材图片库%>&write=0&is_public=<%=is_public%>',
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
                    url: "/handles/wechat_ajax.ashx?action=check_exists_name&channel_id=1",
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
                        showTip.show("请输入图片名称", true);

                    } else {
                        if (!ckName($(this).val())) {
                            showTip.show("图片名称已存在", true);
                        }

                    }
                })

                $("#btnSubmit").click(function () {

                    if (!$("#txtTitle").val()) {
                        showTip.show("请输入图片名称", true);
                    
                        return false;
                    } else if (!ckName($("#txtTitle").val())) {
                        showTip.show("图片名称已存在", true);
                        $("#txtTitle").focus();
                        return false;
                    } else if ($("#txtFile").val() == "") {
                        showTip.show("请上传图片", true);
                        return false;
                    }
                    dialogue.dlLoading();//显示Loading
                    form.submit();

                });

                nav.change('<%=m_id%>');
            });
        </script>

    </form>
</body>
</html>
