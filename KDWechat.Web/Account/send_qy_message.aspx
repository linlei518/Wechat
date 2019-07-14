<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeBehind="send_qy_message.aspx.cs" Inherits="KDWechat.Web.Account.send_qy_message" %>

<%@ Register TagName="TopControl" Src="~/UserControl/TopControl.ascx" TagPrefix="uc" %>

<%@ Register Src="~/UserControl/MenuList.ascx" TagName="Sys_menulist" TagPrefix="uc2" %>

<!doctype html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="UTF-8" />
    <title><%=pageTitle%></title>
    <script src="../scripts/html5.js"></script>
  
    <link type="text/css" href="../styles/global.css" rel="stylesheet" />
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>
<body>
    <uc:TopControl ID="TopControl1" runat="server" />
    <uc2:Sys_menulist ID="MenuList1" runat="server" />
    <form id="form1" runat="server">
        <section id="main">
            <%=NavigationName%>
            <div class="titlePanel_01">
                <h1>新建上线通知（发送企业号消息）</h1>
            </div>
            <div class="listPanel_01 bottomLine">
                <dl>
                    <dt>内容：</dt>
                    <dd>
                        <textarea id="txtContents" name="txtContents" style="width: 100%; height: 338px; visibility: hidden;" class="textarea jsText" maxlength="80000" runat="server"></textarea>

                    </dd>
                </dl>

            </div>
            <%--前连接地址--%>
            <asp:HiddenField ID="hfReturlUrl" runat="server" />
            <asp:HiddenField ID="hidRole" runat="server" />
            <div class="btnPanel_01">
                <asp:Button ID="btnSubmit" CssClass="btn1 btn" runat="server" Text="确定" OnClick="btnSubmit_Click" />
                <%--   <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn btn2" OnClick="CancelButton_Click">取消</asp:LinkButton>--%>
            </div>
        </section>
    </form>
      <script src="../scripts/jquery-1.10.2.min.js"></script>
   
    <script src="../scripts/controls.js"></script>
    <script src="../editor/kindeditor-min.js" type="text/javascript"></script>
    <script src="../editor/lang/zh_CN.js" type="text/javascript"></script>
     <script type="text/javascript">
         $(document).ready(function () {
             $("#btnSubmit").click(function () {
                  if ($.trim($('#txtContents').val()) == "") {
                     showTip.show("请输入站内信内容", true);
                     $("#txtContents").focus();
                     return false;
                 }
                 dialogue.dlLoading();
             })

         });


    </script>
    <script>

        KindEditor.ready(function (K) {
            <%--初始化文本框的编辑器--%>
            var editor = K.create('textarea[name="txtContents"]', {
                resizeType: 1,
                allowPreviewEmoticons: false,
                allowImageUpload: true,
                width: '100%',
                allowFileManager: true,
                afterBlur: function () { this.sync(); },
                uploadJson: '../handles/upload_ajax.ashx?action=EditorFile&IsWater=1&folder=<%=folder%>&upload_type=<%=(int)KDWechat.Common.media_type.站内信图片%>&wx_id=<%=wx_id%>&u_id=<%=u_id%>',
                fileManagerJson: '../handles/upload_ajax.ashx?action=ManagerFile&folder=<%=folder%>&upload_type=<%=(int)KDWechat.Common.media_type.站内信图片%>',
                items: ['source', 'forecolor', 'hilitecolor', 'bold', 'italic', 'underline', 'removeformat', 'justifyleft', 'justifycenter', 'justifyright', 'image', 'link', 'unlink'],
                afterChange: function () {
                    if (editor != null) {
                        console.log(editor.html());
                        var strValue = editor.html();
                        $('.jsText').val(strValue);
                        $(".jsText").get(0).onchange();

                    }


                }
            });
        });
        nav.change('<%=m_id%>'); 
    </script>
</body>
</html>

