<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="member_bind.aspx.cs" Inherits="KDWechat.Web.fans.member_bind" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>绑定凯德会员</title>
<link href="../AppointmentHouse/styles/global.css" rel="stylesheet" type="text/css"/>
<link href="../AppointmentHouse/mobiscroll/mobiscroll.core-2.6.2.css" rel="stylesheet" type="text/css"/>
<link href="../AppointmentHouse/mobiscroll/mobiscroll.android-ics-2.6.2.css" rel="stylesheet" type="text/css"/>

</head>
<body>
    <form id="form1" runat="server">
    <section id="contenter">
	    <div class="titleStyle"><span>绑定凯德会员</span></div>
        <div class="content">
            <div class="formlist"><input name="mobile" id="txtPhone" type="text"  runat="server" class="text" placeholder="手机"  onkeyup="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}" onafterpaste="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}" maxlength="11"/></div>
            <div class="formlist"><input name="password" id="txtPassword" type="password" runat="server" class="text" placeholder="密码" maxlength="20" /></div>
        </div>
        <div class="sendBtn">
            <asp:Button ID="btnSubmit" runat="server" CssClass="btn" Text="提交" OnClientClick="return Check()" OnClick="btnSubmit_Click" ></asp:Button><br /><br />
            <asp:Button ID="btnRegist" runat="server" CssClass="btn" Text="注册新会员" OnClick="btnRegist_Click" ></asp:Button>
            
        </div>  
    </section>
       
    <script type="text/javascript" src="../AppointmentHouse/scripts/jquery-1.10.2.min.js"></script>
    <script src="../AppointmentHouse/mobiscroll/mobiscroll.core-2.6.2.js"></script>
    <script src="../AppointmentHouse/mobiscroll/mobiscroll.core-2.6.2-zh.js"></script>
    <script src="../AppointmentHouse/mobiscroll/mobiscroll.datetime-2.6.2.js"></script>
    <script src="../AppointmentHouse/mobiscroll/mobiscroll.android-ics-2.6.2.js"></script>
    <script src="../Scripts/function.js" type="text/javascript"></script>
    <script src="../scripts/controls.js" type="text/javascript"></script>
    <script>
        function Check()
        {
            if ($("#txtPhone").val() == "") {
                alert("请填写手机号");
                return false;
            }
            if ($("#txtPassword").val() == "") {
                alert("请填写密码");
                return false;
            }
        }
    </script>
    </form>
</body>
</html>
