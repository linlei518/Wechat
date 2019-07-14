<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="user_edit.aspx.cs" Inherits="KDWechat.Web.zh_user.user_edit"  ValidateRequest="false"  %>



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
                <h1><span>员工列表</span><i class="breadcrumbArrow"></i><em><%=this.id==0?"新建":"编辑" %>员工</em></h1>
            </div>

       <div class="listPanel_01 bottomLine">
            <dl>
                <dt>员工号</dt>
                <dd>
                    <asp:TextBox ID="txt_user_code" runat="server" CssClass="txt required" datatype="*2-20" sucmsg=" "></asp:TextBox>
                    <span class="Validform_checktip">*员工号，20个字符内。</span></dd>
            </dl>
            <dl>
                <dt>员工姓名</dt>
                <dd>
                    <asp:TextBox ID="txt_user_name" runat="server" CssClass="txt required" sucmsg=" "></asp:TextBox>
                    <span class="Validform_checktip">*员工姓名，20个字符内。</span></dd>
            </dl>
            <dl>
                <dt>手机</dt>
                <dd>
                    <asp:TextBox ID="txt_user_tel" runat="server" CssClass="txt required" datatype="*2-20" sucmsg=" "></asp:TextBox>
                    <span class="Validform_checktip">*手机，20个字符内。</span></dd>
            </dl>
            <dl>
                <dt>密码</dt>
                <dd>
                    <asp:TextBox ID="txt_pwd" runat="server" CssClass="txt required" datatype="*2-20" sucmsg=" " TextMode="Password"></asp:TextBox>
                    <span class="Validform_checktip">*密码，20个字符内。</span></dd>
            </dl>
            <dl>
                <dt>邮箱</dt>
                <dd>
                    <asp:TextBox ID="txt_user_mail" runat="server" CssClass="txt required" datatype="*2-20" sucmsg=" "></asp:TextBox>
                    <span class="Validform_checktip">*邮箱，20个字符内。</span></dd>
            </dl>
            <dl>
                <dt>部门</dt>
                <dd>
                    <asp:TextBox ID="txt_user_dpt" runat="server" CssClass="txt required" datatype="*2-20" sucmsg=" "></asp:TextBox>
                    <span class="Validform_checktip">*部门，20个字符内。</span></dd>
            </dl>
            <dl>
                <dt>职位</dt>
                <dd>
                    <asp:TextBox ID="txt_position" runat="server" CssClass="txt required" datatype="*2-20" sucmsg=" "></asp:TextBox>
                    <span class="Validform_checktip">*职位，20个字符内。</span></dd>
            </dl>
           <dl>
                <dt>车牌号</dt>
                <dd>
                    <asp:TextBox ID="txt_plate_number" runat="server" CssClass="txt required" datatype="*2-200" sucmsg=" " Width="500px"></asp:TextBox>
                    <span class="Validform_checktip">*车牌号，200个字符内,多个车牌号用“|”分割。</span></dd>
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
        

        <%--  初始化编辑器--%>
            <script type="text/javascript">
               nav.change('<%=m_id%>'); 
                function selectDate() {
                    WdatePicker(
                    {
                        doubleCalendar: true,
                        dateFmt: 'yyyy-MM-dd HH:mm:ss'
                    });
                }



                /*region实例化ue编辑器*/
                var ue = new UE.getEditor('txtDescribe');
                ue.ready(function () {
                    //上传图片需要传入参数列表
                    ue.execCommand('serverparam', {
                        'IsWater':0,
                        'serial':'Product<%=m_id%>',
                                'upload_type':<%=(int)Enums.upload_type.积分商品图片%>
                                });             
                });

            /*region实例化ue编辑器*/
                var ue = new UE.getEditor('txtapplication_describe');
                ue.ready(function () {
                    //上传图片需要传入参数列表
                    ue.execCommand('serverparam', {
                        'IsWater':0,
                        'serial':'Product<%=m_id%>',
                                'upload_type':<%=(int)Enums.upload_type.积分商品图片%>
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
                var myreg = /^(((13[0-9]{1})|(15[0-9]{1})|(18[0-9]{1})|(17[0-9]{1})|(14[0-9]{1}))+\d{8})$/;
                if (!myreg.test($("#txt_user_tel").val())) {
                    alert('请输入有效的手机号码！');
                    return false;
                }
                return true;
            }
        </script>
        
        

    </form>
</body>
    
    
    
    

</html>


