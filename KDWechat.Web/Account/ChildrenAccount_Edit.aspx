<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChildrenAccount_Edit.aspx.cs" Inherits="KDWechat.Web.Account.ChildrenAccount_Edit" %>

<%@ Register TagName="TopControl" Src="~/UserControl/TopControl.ascx" TagPrefix="uc" %>

<%@ Register Src="../UserControl/MenuList.ascx" TagName="Sys_menulist" TagPrefix="uc2" %>

<!doctype html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="UTF-8" />
    <title><%=pageTitle %></title>
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
            <div class="breadcrumbPanel_01">
                <h1><span>子账号管理</span><i class="breadcrumbArrow"></i><em><%=this.id==0?"新建":"编辑" %>子账号</em></h1>
            </div>
            <div class="titlePanel_01">
                <div class="btns">
                    <a href="regoin_account.aspx?m_id=60" class="btn btn5"><i class="black back"></i>返回</a>
                </div>
                <!--          <h1><%=this.id==0?"新建":"编辑" %>子账号</h1>-->
            </div>
            <div class="listPanel_01 bottomLine">
                <dl>
                    <dt>账号名称：</dt>
                    <dd>
                        <asp:HiddenField ID="hfhas" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hftitle" runat="server"></asp:HiddenField>
                        <asp:TextBox CssClass="txt required" ID="txtAccountName" MaxLength="50" runat="server"></asp:TextBox><i>*</i><em>不超过50个字</em></dd>
                </dl>
                <dl>
                    <dt>密　　码：</dt>
                    <dd>
                        <asp:TextBox ID="txtPassword" MaxLength="15" TextMode="Password" CssClass="txt required" runat="server"></asp:TextBox><i>*</i><em>5-15位</em></dd>
                </dl>
                <dl>
                    <dt>负责人：</dt>
                    <dd>
                        <asp:TextBox ID="txtRealName" MaxLength="20" CssClass="txt required" runat="server"></asp:TextBox><i>*</i><em>不超过20个字</em></dd>
                </dl>
                <dl>
                    <dt>电　　话：</dt>
                    <dd>
                        <asp:TextBox ID="txtTel" MaxLength="20" CssClass="txt required" runat="server"></asp:TextBox><i></i><em>如：021-88888888</em></dd>
                </dl>
                <dl>
                    <dt>手　　机：</dt>
                    <dd>
                        <asp:TextBox ID="txtMobil" MaxLength="11" CssClass="txt required" runat="server"></asp:TextBox><i></i><em>如：13800138000</em></dd>
                </dl>
                <dl>
                    <dt>邮　　箱：</dt>
                    <dd>
                        <asp:TextBox ID="txtEmail" MaxLength="50" runat="server" CssClass="txt required"></asp:TextBox><i>*</i><em>如：xxxx@163.com</em></dd>
                </dl>
                 <dl>
                    <dt>第三方管理员：</dt>
                    <dd>
                        <asp:RadioButtonList ID="rbl_only_op_self" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                            <asp:ListItem Value="1">是</asp:ListItem>
                            <asp:ListItem Value="0" Selected="True" >否</asp:ListItem>
                        </asp:RadioButtonList> &nbsp;&nbsp;<em>仅针对素材管理，设置后该管理员仅能查看和管理自己上传的素材。</em></dd>
                </dl>
                <div class="title">
                    <h2>负责微信公众号</h2>
                </div>
                <div class="jurisdictionSet">
                    <ul>
                        <asp:Literal ID="litRole" runat="server"></asp:Literal>
                    </ul>
                    <asp:HiddenField ID="hfactions" runat="server" />
                </div>
                <dl style="display: none">
                    <dt>公众账号</dt>
                    <dd>
                        <asp:Repeater ID="repItem" runat="server">
                            <ItemTemplate>
                                <input id="<%#Eval("id") %>" class="wechat" type="checkbox" onclick="getFristNav(<%#Eval("id").ToString()%>)" /><span id="sp_<%#Eval("id") %>"><%#Eval("wx_pb_name") %></span>
                            </ItemTemplate>
                            <FooterTemplate>
                                <%# repItem.Items.Count == 0 ? "<tr><td style=\"text-align:center;\">暂无数据</td></tr>" : ""%>
                            </FooterTemplate>
                        </asp:Repeater>
                    </dd>
                    <dd>
                        <div id="div_nav">
                        </div>
                        <div id="div_temp">
                        </div>
                    </dd>

                </dl>
            </div>
            <%--前连接地址--%>
            <asp:HiddenField ID="hfReturlUrl" runat="server" />
            <div class="btnPanel_01">
                <asp:Button ID="btnSubmit" runat="server" CssClass="btn1 btn" Text="保存" OnClick="btnSubmit_Click" />
                <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn btn2">取消</asp:LinkButton>
            </div>

        </section>


        <script src="../scripts/jquery-1.10.2.min.js"></script>
        <script src="../scripts/controls.js"></script>



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
                if(flag){
                    $(this).parent().parent().parent().find('ul li').eq(0).find('.checkbox').each(function(){
                        if(flag){
                            this.checked = flag;
                        }
                    });
                }else{
                    $(this).parent().parent().parent().find('.checkbox').not(this).each(function(){
                        if(!flag){
                            this.checked = flag;
                        }
                    });
                }
            });
            $('.jurisdictionSet .checkbox').not('[name="root"]').change(function(){
                var flag = !!this.checked;
                $(this).parent().parent().parent().find('.checkbox').not(this).each(function(){
                    this.checked = flag;
                });
                if(flag){
                    setParentChecked(this);
                }else{
                    setParentNoChecked(this);
                }
            });

            function setParentChecked(obj){
                $(obj).parents('li').children('h3').find('.checkbox').each(function(){
                    this.checked = true;
                });;
            }

            function setParentNoChecked(obj){
                if($(obj).attr('name')=='root')return false;
                var flag = false;
                $(obj).parents('ul').eq(0).children('li').children('h3').find('.checkbox').each(function(){
                    if(this.checked){
                        flag = true;
                    }
                });
                if(!flag){
                    var parentCheckbox = $(obj).parents('li').eq(1).children('h3').find('.checkbox').get(0);
                    parentCheckbox.checked = false;
                    setParentNoChecked(parentCheckbox);
                }
            }


</script>

        <script type="text/javascript">
        
            $(function(){
                $("#btnSubmit").click(function(){
                    if($("#txtAccountName").val()=="")
                    {
                        showTip.show("请输入账号名称", true);
                        $("#txtAccountName").focus();
                        return false;
                    }
                    if($("#txtPassword").val()=="")
                    {
                        showTip.show("请输入密码", true);
                        $("#txtPassword").focus();
                        return false;
                    }
                    else  {
                        if($("#txtPassword").val().length<5) {
                            showTip.show("长度最少5位", true);
                            $("#txtPassword").focus();
                            return false;
                        }
                    }
                    if($("#txtRealName").val()=="")
                    {
                        showTip.show("请输入负责人", true);
                        $("#txtRealName").focus();
                        return false;
                    }
               

                    if($("#txtEmail").val()=="")
                    {
                        showTip.show("请输入邮箱", true);
                        $("#txtEmail").focus();
                        return false;
                    }else {
                   
                        var reg = /^([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$/;
                        //alert(tel.test($("#txtTel").val()));
                        if(!reg.test($("#txtEmail").val()))
                        {
                            showTip.show("请输入正确的邮箱", true);
                            $("#txtEmail").focus();
                            return false;
                        }
                    }

                    if ($("input:checkbox:checked").length <= 0) {
            
                        showTip.show("请选择要负责的公众号", true);
                        return false;
                    }
                    var strNav="";
                    $('.jurisdictionSet .checkbox[name="root"]').each(function(){
                        if ($(this).prop("checked") == true) 
                        {
                            var wx_id=$(this).prop("id");
                            var action_list="";
                            $(this).parent().parent().parent().find('.checkbox').not(this).each(function(){
                                if ($(this).prop("checked") == true) 
                                { 
                            
                                    var nav=$(this).prop("id");
                                    //alert(nav);
                                    action_list+=nav+",";
                            
                                }
                            });
                            strNav+=wx_id+"|"+ action_list+"~!@#";
                   
                        }
                    });
                    $("#hfactions").val(strNav);
                    dialogue.dlLoading();
 
                })
            
                $("#txtAccountName").blur(function(){
                 
                    $.ajax({
                        type: "POST",
                        async: false,
                        url: "/handles/wechat_ajax.ashx?action=validateAccount&time=" + Math.random(),
                        data:{new_name:$("#txtAccountName").val(),old_name:"<%=hftitle.Value%>"},
                        success: function (response) {
                            $("#hfhas").val(response);
                        }
                    });
                
                    if ($("#hfhas").val() == "0") {
                        return true;
                    } else {
                        showTip.show("账号名称已存在", true);
                        $("#txtAccountName").focus();
                        return false;
                    }
                    
                })
        
            })
      
        </script>
    </form>
</body>
</html>
