﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sys_module_edit.aspx.cs" Inherits="KDWechat.Web.module.sys_module_edit" %>

<%@ Register TagName="TopControl" Src="~/UserControl/TopControl.ascx" TagPrefix="uc" %>

<%@ Register Src="~/UserControl/MenuList.ascx" TagName="Sys_menulist" TagPrefix="uc2" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="UTF-8" />
    <title><%=pageTitle %></title>
    <script src="../scripts/html5.js"></script>
    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/jquery.validate/jquery.validate.js"></script>
    <script src="../Scripts/jquery.validate/jquery.metadata.js"></script>
    <script src="../Scripts/jquery.validate/messages_cn.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {

        });


    </script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet" />
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>
<body>
    <uc:TopControl runat="server" />
    <uc2:Sys_menulist ID="MenuList1" runat="server" />
    <form id="form1" runat="server">
        <section id="main">
                    <%=NavigationName %>
            <div class="titlePanel_01">
                <h1>添加模块</h1>
            </div>
            <div class="listPanel_01 bottomLine">
                <dl>
                    <dt>模块名：</dt>
                    <dd>
                        <asp:TextBox CssClass="txt required" ID="txtModuleName" runat="server"></asp:TextBox>
                    </dd>
                </dl>
                <dl>
                    <dt>调用名：</dt>
                    <dd>
                        <asp:TextBox ID="txtCallName" CssClass="txt required" runat="server"></asp:TextBox>
                    </dd>
                </dl>
                <dl>
                    <dt>模块类型：</dt>
                    <dd>
                        <asp:DropDownList CssClass="select" ID="ddlModuleType" runat="server">
                            <asp:ListItem>请选择</asp:ListItem>
                            <asp:ListItem Value="1">咨询模块</asp:ListItem>
                            <asp:ListItem Value="2">活动模块</asp:ListItem>
                            <asp:ListItem Value="3">业务模块</asp:ListItem>
                            <asp:ListItem Value="4">其他</asp:ListItem>
                        </asp:DropDownList>
                        <label id="labModuleType" class="error"></label>
                    </dd>
                </dl>
                <dl>
                    <dt>图片地址：</dt>
                    <dd>
                        <asp:TextBox ID="txtImgUrl"  CssClass="txt required" runat="server"></asp:TextBox>
                        <div class="simulationPanel_02" style="margin-top:5px;">
                            <div class="infoField mainInfo ">
                            <div class="img">
					            <span>
						            <img src="../images/blank.gif" runat="server"  id="img_show" width="120" height="80"/>
					            </span>
					            <div class="tip"><%=id==0?"上传图片":"" %></div>
				            </div>
                           </div>
                         </div> 

                        <input type="button" id="btnUpload" class="btn btn6" value="浏览..." />
                        <p>推荐图片上传尺寸( 宽720像素，高400像素） 小于500k;</p>
                        <p>如选择外部图片，请直接在文本框内输入图片地址</p>
                    </dd>
                </dl>
                <dl>
                    <dt>管理地址：</dt>
                    <dd>
                        <asp:TextBox ID="txtManageUrl" CssClass="txt required" runat="server"></asp:TextBox>
                    </dd>
                </dl>
                <dl>
                    <dt>查看地址：</dt>
                    <dd>
                        <asp:TextBox ID="txtCheckUrl" CssClass="txt required" runat="server"></asp:TextBox>
                    </dd>
                </dl>
                <dl>
                    <dt>描述：</dt>
                    <dd>
                        <asp:TextBox ID="txtDescription" TextMode="MultiLine" CssClass="textarea required" runat="server"></asp:TextBox>
                    </dd>
                </dl>
                <dl>
                    <dt>状　　态：</dt>
                    <dd>
                        <label class="radioArea">
                            <input runat="server" id="radStatusOk" type="radio" class="radio" name="accountState" />正常</label>
                        <label class="radioArea">
                            <input type="radio" id="radStatusFalse" runat="server" class="radio" name="accountState" />禁用</label>
                        <label class="error" id="labState"></label>
                    </dd>
                </dl>
                <dl>
                    <dt>设定限制：</dt>
                    <dd>
                        <asp:CheckBoxList ID="chbAllowWechats" RepeatDirection="Horizontal"  runat="server"></asp:CheckBoxList>
                    </dd>
                </dl>

            </div>

            <%--前连接地址--%>
            <asp:HiddenField ID="hfReturlUrl" runat="server" />
            <div class="btnPanel_01">
                <asp:Button ID="btnSubmit" CssClass="btn1 btn" runat="server" Text="确定" OnClick="SubmitButtom_Click" />
                <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn btn2" OnClick="CancelButton_Click">取消</asp:LinkButton>
            </div>
        </section>
    </form>
    <script src="../scripts/controls.js"></script>
    <script src="../editor/kindeditor-min.js" type="text/javascript"></script>
    <script src="../editor/lang/zh_CN.js" type="text/javascript"></script>
    <script src="../Scripts/function.js" type="text/javascript"></script>
    <script type="text/javascript">
        KindEditor.ready(function (K) {
            <%--调用编辑器的上传加载开始,folder参数表示当前微信号的文件夹--%>
            var editor = K.editor({
                uploadJson: '../handles/upload_ajax.ashx?action=EditorFile&IsWater=1&folder=<%=folder%>',
                fileManagerJson: '../handles/upload_ajax.ashx?action=ManagerFile&folder=<%=folder%>',
                allowFileManager: true
            });
            K('#btnUpload').click(function () {
                editor.loadPlugin('image', function () {
                    editor.plugin.imageDialog({
                        imageUrl: K('#txtImgUrl').val(),
                        clickFn: function (url, title, width, height, border, align) {
                            K('#txtImgUrl').val(url);
                            K('.tip').html("");
                            K('#img_show').attr("src", url);
                            editor.hideDialog();

                        }
                    });
                });
            });


            <%--调用编辑器的上传事件结束--%>



            K.create('#txtContent', {
                resizeType: 1,
                allowPreviewEmoticons: false,
                allowImageUpload: false,
                items: [
				     'link', 'unlink']
            });

        });
        nav.change('<%=m_id%>'); 

        
    </script>
</body>

</html>