<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="product_edit.aspx.cs" Inherits="KDWechat.Web.product.product_edit"  ValidateRequest="false"  %>



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
                <h1><span>产品列表</span><i class="breadcrumbArrow"></i><em><%=this.id==0?"新建":"编辑" %>产品</em></h1>
            </div>

       <div class="listPanel_01 bottomLine">
            <dl>
                <dt>产品名称</dt>
                <dd>
                    <asp:TextBox ID="txtName" runat="server" CssClass="txt required" datatype="*2-20" sucmsg=" "></asp:TextBox>
                    <span class="Validform_checktip">*产品名称，20个字符内。</span></dd>
            </dl>
          
            <dl>
                <dt>产品分类</dt>
                <dd>
                      <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="True" AppendDataBoundItems="true"  CssClass="select required">
                                    <asp:ListItem Value="1" Selected="True">别墅</asp:ListItem>
                                    <asp:ListItem Value="2" >洋房</asp:ListItem>
                                    <asp:ListItem Value="2" >高层</asp:ListItem>
                      </asp:DropDownList>
                   
            </dl>
              <dl>
                <dt>产品价格</dt>
                <dd>
                    <asp:TextBox ID="txtPrice" runat="server" CssClass="txt required" datatype="n" sucmsg=" "></asp:TextBox>
                    <span class="Validform_checktip">*产品销售价格</span>
                </dd>
            </dl>
            <div class="article">
            <dl>
                <dt>产品用途说明</dt>
                <dd>
                    <asp:TextBox ID="txtapplication_describe" runat="server" CssClass="txt required" datatype="*1-2000" style="position: relative; z-index: 8; height: 400px; width: 80%;"  TextMode="MultiLine" MaxLength="2000" ></asp:TextBox>
                </dd>
            </dl>
                
                 <dl>
                <dt>产品概要</dt>
                <dd>
                    <asp:TextBox ID="txtDescribe" runat="server" CssClass="txt required" datatype="*1-2000" style="position: relative; z-index: 8; height: 400px; width: 80%;"  TextMode="MultiLine" MaxLength="2000" ></asp:TextBox>
                </dd>
            </dl>

            <dl>
               <dt>产品图片</dt>
                <dd>
                     <div class="photo-list" id="div_img">
                        <asp:HiddenField ID="hfImgList" runat="server" />
                        <ul>
                            <asp:Repeater ID="rptAlbumList" runat="server">
                                <ItemTemplate>
                                    <li>
                                        <input type="hidden" name="hid_photo_name" value="<%#Eval("img_url")%>" />
                                        <div class="img-box" onclick="setFocusImg(this);">
                                            <img src="<%#Eval("img_url")%>" onerror="javascript:this.src='/content/images/not_find_img.jpg';" />
                                        </div>
                                        <a href="javascript:;" onclick="delImg(this);">删除</a>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                    <script id="myeditor"></script>
              
                    <div style="clear: both">
                        <input type="button" class="btn1 btn" onclick="javascript:upImage();" value='上传图片' /><%-- 需要实例化一个独立的编辑器，见下面的js--%>
                    </div>
                    <div style="clear: both">*支持多图，图片尺寸：640px * 430px，大小不超过1MB，格式：bmp, jpeg, jpg, png，图片数最多6张</div>
                </dd>
            </dl> 

            </div>
              <dl>
                <dt>排序数字</dt>
                <dd>
                    <asp:TextBox ID="txtSortId" runat="server" CssClass="txt required" datatype="n" sucmsg=" ">99</asp:TextBox>
                    <span class="Validform_checktip">*数字，越小越向前</span>
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
                //多图
                var img_list="";
                $("#div_img ul li").each(function() {
                    var temp = $(this).find("img ").attr('src');
                    img_list += temp;
                    if ($(this).index() != $("#div_img ul li").length-1) {
                        img_list +="|";
                    }
                });
                if ($("#div_img ul li").length > 6) {
                    alert("添加产品图片数最多为6张");
                    return false;
                }
                if ($("#div_img ul li").length < 1) {
                    alert("请添加产品中文图片");
                    return false;
                }
                $("#hfImgList").val(img_list);
                return true;
            }
        </script>
        
        

    </form>
</body>
    
    
    
    

</html>


