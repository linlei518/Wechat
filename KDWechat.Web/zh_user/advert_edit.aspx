<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="advert_edit.aspx.cs" Inherits="KDWechat.Web.zh_user.advert_edit"  ValidateRequest="false"  %>



<%@ Import Namespace="KDWechat.Common" %>
<%@ Register Src="../UserControl/TopControl.ascx" TagName="TopControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/MenuList.ascx" TagName="MenuList" TagPrefix="uc2" %>

<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title><%=pageTitle %></title>
      <script src="../scripts/html5.js"></script>
     <link type="text/css" href="../styles/global.css" rel="stylesheet">
</head>

<body >
    <form id="form1" runat="server">
      <uc1:TopControl ID="TopControl1" runat="server" />
    <uc2:MenuList ID="MenuList1" runat="server" />
         <section id="main">
        <!--内容-->
       <div class="breadcrumbPanel_01">
                <h1><span>广告位列表</span><i class="breadcrumbArrow"></i><em><%=this.id==0?"新建":"编辑" %>广告位</em></h1>
            </div>

       <div class="listPanel_01 bottomLine">
            <dl>
                <dt>标题</dt>
                <dd>
                    <asp:TextBox ID="txt_title" runat="server" CssClass="txt required" datatype="*2-20" sucmsg=" "></asp:TextBox>
                    <span class="Validform_checktip">*标题，20个字符内。</span></dd>
            </dl>
            <dl>
                <dt>链接地址</dt>
                <dd>
                    <asp:TextBox ID="txt_link_url" runat="server" CssClass="txt required" datatype="*1-100" sucmsg=" "></asp:TextBox>
                    <span class="Validform_checktip">*跳转链接地址，100个字符内，不跳转输入#。</span></dd>
            </dl>
            <dl>
                 <dt>上传图片：</dt>
                    <dd>
                        <input id="txtFile" runat="server" type="hidden" />
                        <%--调用编辑器的上传--%>
                        <div class="simulationPanel_02" style="margin-top: 5px;">
                            <div class="infoField mainInfo ">
                                <div class="img" >
                                    <span>
                                        <img src="../images/blank.gif" runat="server" id="img_show" width="375" height="165"/>
                                    </span>
                                    <div class="tip" style="margin-top:57px"><%=txtFile.Value==""?"上传图片":"" %></div>
                                </div>
                            </div>
                        </div>

                        <input type="button"  id="btnUpload" class="btn btn6" value="  上传  " />
                        <p>图片建议尺寸750像素*330像素，大小:不超过1M ，格式：bmp, jpeg, jpg, gif</p>
                    </dd>
            </dl>
           
            <dl>
                <dt>排序号</dt>
                <dd>
                    <asp:TextBox ID="txt_sort" runat="server" CssClass="txt required" datatype="number" sucmsg=" "></asp:TextBox>
                    <span class="Validform_checktip">*排序号,数字。</span></dd>
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
        </div>
        <!--/内容-->

        <!--工具栏-->
        <div class="btnPanel_01">
                <asp:HiddenField ID="hfReturnUrl" runat="server" />
                
                <asp:Button ID="btnSubmit" runat="server" Text="提交保存" CssClass="btn1 btn"  CommandArgument="save" OnClick="btnSubmit_Click"  OnClientClick="return btn_check()"/>
                <input name="btnReturn" type="button" value="返回上一页" class="btn btn2" onclick="javascript: location.href = '<%=hfReturnUrl.Value%>    '" />
            </div>
       
  </section>

   
        
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
           <script src="../scripts/controls.js"></script>
    <script src="../editor/kindeditor-min.js" type="text/javascript"></script>
    <script src="../editor/lang/zh_CN.js" type="text/javascript"></script>
    <script src="../Scripts/function.js" type="text/javascript"></script>
    <script src="../Scripts/ZeroClipboard.js"></script>


   <script type="text/javascript">
        KindEditor.ready(function (K) {
            <%--调用编辑器的上传加载开始,folder参数表示当前微信号的文件夹--%>
            var editor = K.editor({
                uploadJson: '../handles/upload_ajax.ashx?action=EditorFile&IsWater=1&folder=<%=folder%>&upload_type=<%=(int)KDWechat.Common.media_type.手机站广告位图片%>&wx_id=<%=m_id==59?wx_id:0%>',
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

        <script type="text/javascript">
            $(function () {
                //初始化表单验证
                $("#form1").initValidform();

            });



        </script>
       
        
        
<%--调用编辑器多张图片上传--%>
        <script type="text/javascript">

            var editor = UE.getEditor('myeditor');
            editor.ready(function () {
                //上传图片需要传入参数列表
                editor.execCommand('serverparam', {
                    'IsWater':0,
                    'serial':'Product<%=m_id%>',
                    'upload_type':<%=(int)Enums.upload_type.积分商品图片%>
                });   
              
                //隐藏编辑器，因为不会用到这个编辑器实例，所以要隐藏
                editor.hide();
                //侦听图片上传
                editor.addListener('beforeInsertImage', function(t, arg) {
                    //for (var i = 0; i < arg.length; i++) {
                        //addUEImage("div_img", arg[0].src, "", false);
                    //}
                    var newLi = $('<li>'
    + '<input type="hidden" name="hid_photo_name" value="0|' + arg[0].src + '" />'
    + '<div class="img-box" onclick="setFocusImg(this);">'
    + '<img src="' + arg[0].src + '"  />'
    + '</div>'
    + '<a href="javascript:;" onclick="delImg(this);">删除</a>'
    + '</li>');
                    $("#div_img").children("ul").html(newLi);

                });

            });

            

            function upImage() {
                var myImage = editor.getDialog("insertimage");
                myImage.open();
            }
          
            function btn_check() {
              
                return true;
            }
        </script>
        
        

    </form>
</body>
    
    
    
    

</html>


