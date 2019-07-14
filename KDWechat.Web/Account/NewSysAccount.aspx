<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewSysAccount.aspx.cs" Inherits="KDWechat.Web.NewAccount" %>

<%@ Register TagName="TopControl" Src="~/UserControl/TopControl.ascx" TagPrefix="uc" %>

<%@ Register Src="~/UserControl/MenuList.ascx" TagName="Sys_menulist" TagPrefix="uc2" %>

<!doctype html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <title><%=pageTitle %></title>
    <script src="../scripts/html5.js"></script>

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
            <div class="breadcrumbPanel_01">
                <h1><span>账号管理</span><i class="breadcrumbArrow"></i><em><%=this.id==0?"新建":"编辑" %>系统账号</em></h1>
            </div>
            <div class="titlePanel_01">
                <div class="btns">
                    <a href="<%=hfReturlUrl.Value %>" class="btn btn5"><i class="black back"></i>返回</a>
                </div>
                <!--     <h1><%=this.id==0?"新建":"编辑" %>系统账号</h1>-->
            </div>
            <div class="listPanel_01 bottomLine">
                <dl>
                    <dt>账号名称：</dt>
                    <dd>
                        <asp:TextBox MaxLength="20" CssClass="txt required" ID="txtAccountName" runat="server"></asp:TextBox><i>*</i></dd>
                </dl>
                <dl>
                    <dt>密　　码：</dt>
                    <dd>
                        <asp:TextBox ID="txtPassword" MaxLength="15" TextMode="Password" CssClass="txt required" runat="server"></asp:TextBox><i>*</i></dd>
                </dl>
                <dl>
                    <dt>账号类型：</dt>
                    <dd>
                        <asp:DropDownList ID="ddlUserFlag" CssClass="select" runat="server" onchange="change()">
                            <asp:ListItem Value="0">请选择</asp:ListItem>
                            <asp:ListItem Value="1">总部账号</asp:ListItem>
                            <asp:ListItem Value="2">地区账号</asp:ListItem>
                        </asp:DropDownList>
                        <label id="labUserFlag" class="error"></label>
                        <i>*</i>
                    </dd>
                </dl>
                <dl>
                    <dt>业务类型：</dt>
                    <dd>
                        <asp:DropDownList CssClass="select" ID="ddlBusssinessType" runat="server">
                            <asp:ListItem Value="">请选择</asp:ListItem>
                            <asp:ListItem Value="0">CLC</asp:ListItem>
                            <asp:ListItem Value="1">CMA</asp:ListItem>
                            <asp:ListItem Value="2">AScott</asp:ListItem>
                        </asp:DropDownList>
                        <label id="labBussinessType" class="error"></label>
                        <i>*</i>
                    </dd>
                </dl>
                <dl>
                    <dt>所在地区：</dt>
                    <dd>
                        <asp:DropDownList CssClass="select" ID="ddlAreaType" runat="server">
                            <asp:ListItem Value="">请选择</asp:ListItem>
                            <asp:ListItem Value="0">华东</asp:ListItem>
                            <asp:ListItem Value="1">华北</asp:ListItem>
                            <asp:ListItem Value="2">华南</asp:ListItem>
                            <asp:ListItem Value="3">西南</asp:ListItem>
                            <asp:ListItem Value="4">华西</asp:ListItem>
                            <asp:ListItem Value="5">东北</asp:ListItem>
                            
                        </asp:DropDownList>
                        <label id="labAreaType" class="error"></label>
                        <i>*</i>
                    </dd>
                </dl>
                <dl>
                    <dt>负责人：</dt>
                    <dd>
                        <asp:TextBox ID="txtRealName" MaxLength="20" CssClass="txt required" runat="server"></asp:TextBox><i>*</i></dd>
                </dl>
                <dl>
                    <dt>所在部门：</dt>
                    <dd>
                        <asp:TextBox ID="txtDepartment" MaxLength="20" CssClass="txt required" runat="server"></asp:TextBox><i>*</i></dd>
                </dl>
                <dl>
                    <dt>电　　话：</dt>
                    <dd>
                        <asp:TextBox ID="txtTel" MaxLength="20" CssClass="txt required" runat="server"></asp:TextBox><%--<i>*</i>--%></dd>
                </dl>
                <dl>
                    <dt>手　　机：</dt>
                    <dd>
                        <asp:TextBox ID="txtMobil" MaxLength="15" CssClass="txt required" runat="server"></asp:TextBox><%--<i>*</i>--%></dd>
                </dl>
                <dl>
                    <dt>邮　　箱：</dt>
                    <dd>
                        <asp:TextBox ID="txtEmail" MaxLength="50" runat="server" CssClass="txt required"></asp:TextBox><i>*</i></dd>
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
                <dl id="dd_role" style="display: none" runat="server">
                    <dt>权限：
                    </dt>
                    <dd>
                        <div class="jurisdictionSet">
                            <ul>
                                <asp:Literal ID="litRole" runat="server"></asp:Literal>
                            </ul>
                            <asp:HiddenField ID="hfactions" runat="server" />
                        </div>
                    </dd>
                </dl>
            </div>
            <%--前连接地址--%>
            <asp:HiddenField ID="hfReturlUrl" runat="server" />
            <asp:HiddenField ID="hidRole" runat="server" />
            <asp:HiddenField ID="hfhas" runat="server" />

            <div class="btnPanel_01">
                <asp:Button ID="btnSubmit" CssClass="btn1 btn" runat="server" Text="保存" OnClientClick="return getRole()" OnClick="SubmitButtom_Click" />
                <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn btn2" OnClick="CancelButton_Click">取消</asp:LinkButton>
            </div>
        </section>

        <script src="../scripts/jquery-1.10.2.min.js"></script>
        <script src="../scripts/controls.js"></script>

        <script type="text/javascript">
            $(document).ready(function () {
                change();
                $("#btnSubmit").click(function () {
                    var tel = /((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)/;
                    var mobile = /^(13[0-9]{9})|(15[0-9]{9})|(18[0-9]{9})|(14[0-9]{9})|(170[0-9]{8})$/;
                    var email = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
                    if (!$("#txtAccountName").val()) {
                        showTip.show("请输入账号名称", true);
                        $("#txtAccountName").focus();
                        return false;
                    } else if (!ck_name($("#txtAccountName").val())) {
                        showTip.show("账号名称已存在", true);
                        $("#txtAccountName").focus();
                        return false;
                    } else if (!$("#txtPassword").val()) {
                        showTip.show("请输入密码", true);
                        $("#txtPassword").focus();
                        return false;
                    } else if ($("#txtPassword").val().length<6) {
                        showTip.show("密码长度在6-15之间", true);
                        $("#txtPassword").focus();
                        return false;
                    } else if ($("#ddlUserFlag").val() == "0") {
                        showTip.show("请选择帐号类型", true);
                        $("#ddlUserFlag").focus();
                        return false;
                    } else if ($("#ddlBusssinessType").val() == "") {
                        showTip.show("请选择业务类型", true);
                        $("#ddlBusssinessType").focus();
                        return false;
                    } else if ($("#ddlAreaType").val() == "") {
                        showTip.show("请选择所在地区", true);
                        $("#ddlAreaType").focus();
                        return false;
                    } else if (!$("#txtRealName").val()) {
                        showTip.show("请输入负责人姓名", true);
                        $("#txtRealName").focus();
                        return false;
                    } else if (!$("#txtDepartment").val()) {
                        showTip.show("请输入所在部门", true);
                        $("#txtDepartment").focus();
                        return false;
                    }
                    
                    else if (!$("#txtEmail").val()) {
                        showTip.show("请输入邮箱", true);
                        $("#txtEmail").focus();
                        return false;
                    } else if (!email.test($("#txtEmail").val())) {
                        showTip.show("请输入正确邮箱格式", true);
                        $("#txtEmail").focus();
                        return false;
                    }else if ($("#ddlUserFlag").val()=="1" && $("input:checkbox:checked").length <= 0) {
                        showTip.show("请选择权限", true);
                        return false;
                    }

                    var strNav="";
                    $('.jurisdictionSet .checkbox[name="root"]').each(function(){
                        if ($(this).prop("checked") == true) 
                        {
                            $(this).parent().parent().parent().find('.checkbox').not(this).each(function(){
                                if ($(this).prop("checked") == true) 
                                { 
                                    var nav=$(this).prop("id");
                                    strNav += nav + ",";
                                }
                            });
                        }
                    });
                    $("#hfactions").val(strNav);
                    dialogue.dlLoading();
                })


              
            });

            function ck_name(u_name) {
                //验证子帐号帐号名是否重复
                $.ajax({
                    type: "post",
                    url: "../handles/wechat_ajax.ashx?action=validateAccount&time=" + Math.random(),
                    data:"new_name=" + u_name,
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        res = data;
                        if (parseInt("<%=this.id%>") > 0) {
                        res = "0";
                    }
                    if (res == "0") {
                        $("#hfhas").val("0");
                    }
                    else {
                        $("#hfhas").val("1");
                    }

                }
            });
            if ($("#hfhas").val() == "0") {
                return true;
            } else {
                return false;
            }
        }

      

        function change() {
            var str = $(".selected").html();
            if (str == "总部账号") {
                $("#dd_role").show();
            }
            else {
                $("#dd_role").hide();
            }
        }
        </script>

        <script>
            nav.change('<%=m_id%>'); 
            $('.jurisdictionSet .btnOpen').click(function(){
                if($(this).parent().next('ul').length){
                    var childList = $(this).parent().next('ul');
                    if($(this).is('.open')){
                        $(this).removeClass('open');
                        childList.hide();
                    }else{
                        $(this).addClass('open');
                        childList.show();
                    }
                }
            });

            $('.jurisdictionSet .checkbox[name="root"]').change(function(){
                var flag = !!this.checked;
                $(this).parent().parent().parent().find('.checkbox').not(this).each(function(){
                    //if(this.disabled){
                    this.checked = flag;
                    //}
                });
            });
            $('.jurisdictionSet .checkbox').not('[name="root"]').change(function(){
                var flag = !!this.checked;
                $(this).parent().parent().parent().find('.checkbox').not(this).each(function(){
                    this.checked = flag;
                });
                if(flag){
                    $(this).parents('li').children('h3').find('.checkbox').each(function(){
                        this.checked = flag;
                    });;
                }else{
                    var flag2 = false;
                    $(this).parents('ul').eq(0).children('li').children('h3').find('.checkbox').each(function(){
                        if(this.checked){
                            flag2 = true;
                        }
                    });
                    if(!flag2){
                        $(this).parents('li').eq(1).children('h3').find('.checkbox').each(function(){
                            this.checked = false;
                        });
                    }
                }
            });
        </script>
    </form>
</body>
</html>
