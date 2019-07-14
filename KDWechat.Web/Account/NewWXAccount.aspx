<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewWXAccount.aspx.cs" Inherits="KDWechat.Web.Account.NewWeiXinAccount" %>

<%@ Register Src="../UserControl/TopControl.ascx" TagName="TopControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/MenuList.ascx" TagName="Sys_menulist" TagPrefix="uc2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <title><%=pageTitle %></title>
    <script src="../scripts/html5.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet" />
    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/jquery.validate/jquery.validate.js"></script>
    <script src="../Scripts/jquery.validate/jquery.metadata.js"></script>
    <script src="../Scripts/jquery.validate/messages_cn.js"></script>
    <script src="../scripts/Bombbox.js"></script>

    <script type="text/javascript">
				$(function () {
						$("#btnSubmit").click(function () {
							if ($("#txtPbName").val() == "") {
								showTip.show("请输入公众号名称", true);
								$("#txtPbName").focus();
								return false;
							} else if ($("#txtOGID").val() == "") {
								showTip.show("请输入公众号原始id", true);
								$("#txtOGID").focus();
								return false;
							} else if ($("#txtWxID").val() == "") {
								showTip.show("请输入微信号", true);
								$("#txtWxID").focus();
								return false;
							} 
                            else if ($("#ddlType").val() == "请选择") {
								showTip.show("请选择账号类型", true);
								return false;
                            }else if ($("#ddlCity").val()=="") {
                                showTip.show("请选所在城市", true);
                                return false;
                            }
                            else if ($("#txtAppID").val() == "") {
								showTip.show("请输入公众号APPID", true);
								$("#txtAppID").focus();
								return false;
							} else if ($("#txtAppSecret").val() == "") {
								showTip.show("公众号AppSecret", true);
								$("#txtAppSecret").focus();
								return false;
							}
		
						})
					})
	
 //       $(document).ready(function () {			
      //           $("#form1").validate({
        //          submitHandler: function (form) {
                   // $("#lblGroup").html("");
//                    if ($(".selected").html() == "请选择") {
//                        $("#lblGroup").html("请选择分组");
//                        $("#lblGroup").show();
//                        return false;
//                    }

//                     form.submit();
  //                }
   //            });
  //       });
    </script>
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>
<body >
    <uc1:TopControl ID="TopControl1" runat="server" />
    <uc2:Sys_menulist ID="MenuList1" runat="server" />
    <section id="main">
        <div class="breadcrumbPanel_01"><h1><span>微信公众号管理</span><i class="breadcrumbArrow"></i><em><%=this.id==0?"新建":"编辑" %>微信公众号</em></h1></div>
        <div class="titlePanel_01">
            <div class="btns">
                <a href="<%=hfReturlUrl.Value %>" class="btn btn5"><i class="black back"></i>返回</a>
            </div>
          <!--  <h1><%=this.id==0?"新建":"编辑" %>微信公众号</h1>-->
        </div>
        <form runat="server" id="form1">
            <div class="listPanel_01 longTitle bottomLine ">
                <dl>
                    <dt>公众号名称：</dt>
                    <dd>
                        <asp:TextBox ID="txtPbName" runat="server" MaxLength="50" CssClass="txt required"></asp:TextBox><i>*</i><em>不超过50个字</em></dd>
                </dl>
                <dl>
                    <dt>公众号原始ID：</dt>
                    <dd>
                        <asp:TextBox ID="txtOGID" MaxLength="30" CssClass="txt required" runat="server"></asp:TextBox><i>*</i><em>如：gh_423dwjkeww3</em></dd>
                </dl>
                <dl>
                    <dt>微信号：</dt>
                    <dd>
                        <asp:TextBox ID="txtWxID" MaxLength="20" runat="server" CssClass="txt required"></asp:TextBox><i>*</i><em></em></dd>
                </dl>
                <dl>
                    <dt>头　　像：</dt>
                    <dd>
                        <input id="txtFile" runat="server" type="hidden" />
                        <%--调用编辑器的上传--%>
                        <div class="simulationPanel_02" style="margin-top: 5px;">
                            <div class="infoField mainInfo ">
                                <div class="img" style="height:268px">
                                    <span>
                                        <img src="../images/blank.gif" runat="server" id="img_show" width="120" height="120">
                                    </span>
                                    <div class="tip" style="margin-top:57px"><%=txtFile.Value==""?"上传图片":"" %></div>
                                </div>
                            </div>
                        </div>

                        <input type="button"  id="btnUpload" class="btn btn6" value="  上传  " />
                         <input type="button"  onclick="javascript:bombbox.openBox('/select_pic.aspx?channel_id=<%=(int)KDWechat.Common.media_type.公众号头像 %>');"  class="btn btn6" value="从图片库选择" />
                        <p>图片建议尺寸400像素*400像素，大小:不超过1M ，格式：bmp, jpeg, jpg, gif</p>
                    </dd>
                </dl>
                <dl>
                    <dt>公众号二维码：</dt>
                    <dd>
                        <input id="txtImg" runat="server" type="hidden" />
                        <%--调用编辑器的上传--%>
                        <div class="simulationPanel_02" style="margin-top: 5px;">
                            <div class="infoField mainInfo ">
                                <div class="img" style="height:268px">
                                    <span>
                                        <img src="../images/blank.gif" runat="server" id="img_erweima" width="120" height="120">
                                    </span>
                                    <div class="tip" style="margin-top:57px"><%=txtImg.Value==""?"上传图片":"" %></div>
                                </div>
                            </div>
                        </div>

                        <input type="button"  id="btnUpload2" class="btn btn6" value="  上传  " />
                         <input type="button"  onclick="javascript:bombbox.openBox('/select_pic.aspx?channel_id=<%=(int)KDWechat.Common.media_type.公众号头像 %>');"  class="btn btn6" value="从图片库选择" />
                        <p>图片建议尺寸400像素*400像素，大小:不超过1M ，格式：bmp, jpeg, jpg, gif</p>
                    </dd>
                </dl>
                <dl>
                    <dt>账号类型：</dt>
                    <dd>
                        <select id="ddlType" runat="server" class="select">
                            <option>请选择</option>
                            <option value="1">普通订阅号</option>
                            <option value="2">认证订阅号</option>
                            <option value="3">普通服务号</option>
                            <option value="4">认证服务号</option>
                        </select><i>*</i>
                        <label class="error" id="lblGroup"></label>
                    </dd>
                </dl>
                 <dl>
                    <dt>所在城市：</dt>
                    <dd>
                        <asp:DropDownList ID="ddlCity" CssClass="select" DataTextField="title" DataValueField="id" AppendDataBoundItems="true" runat="server">
                        <asp:ListItem Value="">请选择</asp:ListItem>
                        </asp:DropDownList><i>*</i>
                        
                    </dd>
                </dl>
                <dl>
                    <dt>公众号APPID：</dt>
                    <dd>
                        <asp:TextBox ID="txtAppID" MaxLength="100" runat="server" CssClass="txt required"></asp:TextBox><i>*</i><em></dd>
                </dl>
                <dl>
                    <dt>公众号AppSecret：</dt>
                    <dd>
                        <asp:TextBox ID="txtAppSecret" MaxLength="100" runat="server" CssClass="txt required"></asp:TextBox><i>*</i><em></dd>
                </dl>
                <asp:HiddenField ID="HiddenField1" runat="server" />
                  <asp:HiddenField ID="hfimg" runat="server" />
                <dl runat="server" id="dlApi" style="display: none">
                    <dt>接口地址：</dt>
                    <dd>
                        <asp:TextBox ID="txtApiUrl" CssClass="txt" runat="server"></asp:TextBox><input class="btn btn6" style="margin-left:2px;" type="button" id="txtcopy1" value="复制" /></dd>
                </dl>
                <dl runat="server" id="dlToken" style="display: none">
                    <dt>接口Token：</dt>
                    <dd>
                        <asp:TextBox ID="txtToken" CssClass="txt" runat="server"></asp:TextBox><input class="btn btn6" style="margin-left:2px;" type="button" id="txtcopy2" value="复制" /></dd>
                </dl>
                <dl>
                    <dt>签到管理员：</dt>
                    <dd>
                        <span id="qy_name_span"><%= string.IsNullOrEmpty(qy_nick_name)?"暂无":qy_nick_name %></span><input class="btn btn6" value="选择管理员" style="margin-left:30px;" onclick="bombbox.openBox('set_qy_admin.aspx?wx_id=<%=id%>')"/>
                        <input runat="server" id="qy_user_id" type="hidden" />
                        <input runat="server" id="qy_user_name" type="hidden" />
                    </dd>
                </dl>
            </div>
            <div class="btnPanel_01 ">
                <asp:Button ID="btnSubmit" class="btn btn1" runat="server" Text="保存" OnClick="btnSubmit_Click" />
                <asp:LinkButton ID="btnCancel" class="btn btn2" runat="server" OnClick="btnCancel_Click">取消</asp:LinkButton>
                <asp:HiddenField ID="hfReturlUrl" runat="server" />
            </div>
        </form>
    </section>
    <script src="../scripts/controls.js"></script>
    <script src="../editor/kindeditor-min.js" type="text/javascript"></script>
    <script src="../editor/lang/zh_CN.js" type="text/javascript"></script>
    <script src="../Scripts/function.js" type="text/javascript"></script>
    <script src="../Scripts/ZeroClipboard.js"></script>

    <script type="text/javascript">
        KindEditor.ready(function (K) {
            <%--调用编辑器的上传加载开始,folder参数表示当前微信号的文件夹--%>
            var editor = K.editor({
                uploadJson: '../handles/upload_ajax.ashx?action=EditorFile&IsWater=1&folder=<%=folder%>&upload_type=<%=(int)KDWechat.Common.media_type.公众号头像%>&wx_id=<%=m_id==59?wx_id:0%>',
                allowFileManager: true
            });
            K('#btnUpload').click(function () {
                editor.loadPlugin('image', function () {
                    editor.plugin.imageDialog({
                        showRemote: false,
                        imageUrl: K('#txtFile').val(),
                        clickFn: function (url, title, width, height, border, align) {
                            K('#txtFile').val(url);
                            K('.tip').html("");
                            K('#img_show').attr("src", url);
                            editor.hideDialog();

                        }
                    });
                });

            });

            K('#btnUpload2').click(function () {
                editor.loadPlugin('image', function () {
                    editor.plugin.imageDialog({
                        showRemote: false,
                        imageUrl: K('#txtImg').val(),
                        clickFn: function (url, title, width, height, border, align) {
                            K('#txtImg').val(url);
                            K('.tip').html("");
                            K('#img_erweima').attr("src", url);
                            editor.hideDialog();

                        }
                    });
                });

            });

            <%--调用编辑器的上传事件结束--%>

            <%--初始化文本框的编辑器--%>
            K.create('#txtContent', {
                resizeType: 1,
                allowPreviewEmoticons: false,
                allowImageUpload: false,
                items: ['link', 'unlink']
            });
        });
    </script>
    <script>

        $(function () {
            ZeroClipboard.setMoviePath("../scripts/ZeroClipboard.swf");//此地址是针对当前页面的相对地址
            var clip = new ZeroClipboard.Client();
            clip.setHandCursor(true);
            clip.setText($("#txtApiUrl").val().toString());
            clip.glue("txtcopy1");
            var clip2 = new ZeroClipboard.Client();
            var text = $("#txtToken").val();
            clip2.setText(text.toString());
            clip2.glue("txtcopy2");

            clip.addEventListener("complete", function () {
                alert("复制成功");
            });
            clip2.addEventListener("complete", function () {
                alert("复制成功");
            });

        });

        function SetQyManager(qy_user_id,qy_user_name)
        {
            $("#qy_user_id").val(qy_user_id);
            $("#qy_user_name").val(qy_user_name);
            $("#qy_name_span").html(qy_user_name);
            showTip.show("企业号管理员选择成功");
            bombbox.closeBox();
        }


        nav.change('<%=m_id%>'); 
    </script>
</body>
</html>
