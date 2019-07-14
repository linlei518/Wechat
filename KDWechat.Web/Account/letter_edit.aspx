<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="letter_edit.aspx.cs" ValidateRequest="false" Inherits="KDWechat.Web.Account.letter_edit" %>

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
                <h1>新建站内信</h1>
            </div>
            <div class="listPanel_01 bottomLine">

                <dl>
                    <dt>账号类型：</dt>
                    <dd>
                        <asp:DropDownList ID="ddlUserFlag" CssClass="select" runat="server">
                            <asp:ListItem Value="0">==全部==</asp:ListItem>
                            <asp:ListItem Value="1">总部账号</asp:ListItem>
                            <asp:ListItem Value="2">地区账号</asp:ListItem>
                            <asp:ListItem Value="3">子帐号</asp:ListItem>
                        </asp:DropDownList>
                    </dd>
                </dl>
                <dl>
                    <dt>业务类型：</dt>
                    <dd>
                        <asp:DropDownList CssClass="select" ID="ddlBusssinessType" runat="server">
                            <asp:ListItem>==全部==</asp:ListItem>
                            <asp:ListItem Value="0">CLC</asp:ListItem>
                            <asp:ListItem Value="1">CMA</asp:ListItem>
                            <asp:ListItem Value="2">AScott</asp:ListItem>
                        </asp:DropDownList>
                    </dd>
                </dl>
                <dl>
                    <dt>所在地区：</dt>
                    <dd>
                        <asp:DropDownList CssClass="select" ID="ddlAreaType" runat="server">
                            <asp:ListItem>==全部==</asp:ListItem>
                            <asp:ListItem Value="0">华东</asp:ListItem>
                            <asp:ListItem Value="1">华北</asp:ListItem>
                            <asp:ListItem Value="2">华南</asp:ListItem>
                            <asp:ListItem Value="3">西南</asp:ListItem>
                            <asp:ListItem Value="4">凯德城镇开发</asp:ListItem>
                        </asp:DropDownList>
                    </dd>
                </dl>
                <dl>
                    <dt>站内信标题：</dt>
                    <dd>
                        <asp:TextBox ID="txtTitle" CssClass="txt" runat="server" MaxLength="50"></asp:TextBox>
                    </dd>

                </dl>
                <dl>
                    <dt>站内信内容：</dt>
                    <dd>
                        <textarea id="txtContents" name="txtContents" style="position:relative;z-index:8;" class="jsText" maxlength="80000" runat="server"></textarea>

                    </dd>



                </dl>

            </div>
            <%--前连接地址--%>
            <asp:HiddenField ID="hfReturlUrl" runat="server" />
            <asp:HiddenField ID="hidRole" runat="server" />
            <div class="btnPanel_01">
                <asp:Button ID="btnSubmit" CssClass="btn1 btn" runat="server" Text="确定" OnClientClick="return getRole()" OnClick="SubmitButtom_Click" />
                <%--   <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn btn2" OnClick="CancelButton_Click">取消</asp:LinkButton>--%>
            </div>
        </section>
    </form>
      <script src="../scripts/jquery-1.10.2.min.js"></script>
   
    <script src="../scripts/controls.js"></script>
    <script src="../editor/kindeditor-min.js" type="text/javascript"></script>
    <script src="../editor/lang/zh_CN.js" type="text/javascript"></script>
    <script src="../Ueditor/ueditor.config.js"></script>
    <script src="../Ueditor/ueditor.all.js"></script>
    <script src="../Ueditor/zh-cn.js"></script>
     <script type="text/javascript">
         $(document).ready(function () {
             $("#btnSubmit").click(function () {
                 var html = ue.getContent();                 
                 $(".jsText").val(html);//赋值
                 if ($.trim($("#txtTitle").val()) == "") {
                     showTip.show("请输入站内信标题", true);
                     $("#txtTitle").focus();
                     return false;
                 } else if ($.trim($('#txtContents').val()) == "") {
                     showTip.show("请输入站内信内容", true);
                     $("#txtContents").focus();
                     return false;
                 }
                 dialogue.dlLoading();
             })

         });


         /*region实例化ue编辑器*/
         var ue = new UE.getEditor('txtContents');
         ue.ready(function () {
             //上传图片需要传入参数列表
             ue.execCommand('serverparam', {
                 'IsWater':1,
                 'folder':'<%=folder%>',
                 'upload_type':<%=(int)KDWechat.Common.media_type.站内信图片%>,
                 'wx_id':<%=wx_id%>,
                 'u_id':<%=u_id%>
                 });             
         });
         /*regionEnd实例化ue编辑器*/


    </script>
    <script>

       
        nav.change('<%=m_id%>'); 
    </script>
</body>
</html>
