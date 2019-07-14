<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="park_edit.aspx.cs" Inherits="KDWechat.Web.zh_user.park_edit"  ValidateRequest="false"  %>



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
                <dt>停车场</dt>
                <dd>
                     <asp:DropDownList ID="ddlpark_id" runat="server" AutoPostBack="True" AppendDataBoundItems="true"   CssClass="select required">
                                    <asp:ListItem Value="0" Selected="True">地下一层</asp:ListItem>
                                    <asp:ListItem Value="1" >地下二层</asp:ListItem>
                                    <asp:ListItem Value="2" >C-B1</asp:ListItem>
                                    <asp:ListItem Value="3" >C-B2</asp:ListItem>
                   </asp:DropDownList>
                  
            </dl>

            <dl>
                <dt>车位编号</dt>
                <dd>
                    <asp:TextBox ID="txt_parking_num" runat="server" CssClass="txt required" datatype="*2-20" sucmsg=" "></asp:TextBox>
                    <span class="Validform_checktip">*员工号，20个字符内。</span></dd>
            </dl>
            <dl>
                <dt>员工编号</dt>
                <dd>
                   <span id="qy_name_span"><asp:Label runat="server" ID="lab_user_code" Text=""></asp:Label></span><input class="btn btn6" value="选择员工" style="margin-left:30px;" onclick="bombbox.openBox('set_qy_admin.aspx?wx_id=<%=id%>')"/>
                        <input runat="server" id="hid_user_code" type="hidden" />
                    <input runat="server" id="hid_user_id" type="hidden" />
                        
            </dl> 
          <dl>
                <dt>员工姓名</dt>
                <dd>
                    <asp:TextBox ID="txt_user_name" runat="server" CssClass="txt required" datatype="*2-20" sucmsg=" " ReadOnly="True"></asp:TextBox>
                    <span class="Validform_checktip">*员工姓名，20个字符内。</span></dd>
            </dl>
            <dl>
                <dt>手机</dt>
                <dd>
                    <asp:TextBox ID="txt_user_tel" runat="server" CssClass="txt required" datatype="*2-20" sucmsg=" "></asp:TextBox>
                    <span class="Validform_checktip">*手机，20个字符内。</span></dd>
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
        


    </form>
</body>
    
    <script type="text/javascript">
        
        $(function () {
            //初始化表单验证
            $("#form1").initValidform();

        });

        function SetQyManager(user_code, user_name,user_tel,user_dpt,plate_number,user_id) {
            $("#lab_user_code").html(user_code);
            $("#hid_user_code").val(user_code);
            $("#txt_user_name").val(user_name);
            $("#txt_user_tel").val(user_tel);
            $("#txt_plate_number").val(plate_number);
            $("#hid_user_id").val(user_id);
            showTip.show("员工选择成功");
            bombbox.closeBox();
        }

    </script>
    
    

</html>


