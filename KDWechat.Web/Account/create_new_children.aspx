<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="create_new_children.aspx.cs" Inherits="KDWechat.Web.Account.create_new_children" %>

<%@ Register TagName="TopControl" Src="~/UserControl/TopControl.ascx" TagPrefix="uc" %>

<%@ Register Src="~/UserControl/MenuList.ascx" TagName="Sys_menulist" TagPrefix="uc2" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8"/>
    <title><%=pageTitle %></title>
    <script src="../scripts/html5.js"></script>
    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/jquery.validate/jquery.validate.js"></script>
    <script src="../Scripts/jquery.validate/jquery.metadata.js"></script>
    <script src="../Scripts/jquery.validate/messages_cn.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#form1").validate({
                submitHandler: function (form) {
                    var bussinesstype = $("#labBussinessType");
                    var areatype = $("#labAreaType");
                    <%--获取提示的label--%>
                    areatype.html("");
                    bussinesstype.html("");
                    <%--将label之前的内容清空--%>
                    var arr = [bussinesstype, areatype];
                    for (i = 0; i < 2; i++) {
                        var element = $(".selected");
                        if (element.eq(i).html() == "请选择") {
                            arr[i].html("请选择分组");
                            arr[i].show();
                            return false;
                        }
                    }
                    <%--循环判断3个ddl是否有选择--%>
                    if ($("input:radio[name='accountState']:checked").val() == null) {
                        $("#labState").html("请选择是否启用");
                        $("#labState").show();
                        return false;
                    }
                    <%--判断radio是否有选择--%>
                    else {
                        form.submit();
                    }
                }
            });
        });
    </script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet"/>
    <!--[if lt IE 9 ]><link href="styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>
<body>
        <uc:TopControl ID="TopControl1" runat="server" />
    <uc2:Sys_menulist ID="MenuList1" runat="server" />
    <form id="form1" runat="server">
        <section id="main">
	        <div class="titlePanel_01">
                <div class="btns">
                    <a href="<%=hfReturlUrl.Value %>" class="btn btn5"><i class="black back"></i>返回</a>
                </div>

		        <h1>新建系统账号</h1>
	        </div>
            <div class="listPanel_01 bottomLine">
		        <dl>
			        <dt>账号名称：</dt>
			        <dd><asp:TextBox CssClass="txt required" MaxLength="50" ID="txtAccountName" runat="server"></asp:TextBox></dd>
		        </dl>
		        <dl>
			        <dt>密　　码：</dt>
			        <dd><asp:TextBox ID="txtPassword" MaxLength="20" CssClass="txt required" TextMode="Password" runat="server"></asp:TextBox></dd>
		        </dl>
		        <dl>
			        <dt>业务类型：</dt>
			        <dd>
                        <asp:DropDownList CssClass="select" ID="ddlBusssinessType" runat="server">
                            <asp:ListItem>请选择</asp:ListItem>
                            <asp:ListItem Value="0">CLC</asp:ListItem>
                            <asp:ListItem Value="1">CMA</asp:ListItem>
                            <asp:ListItem Value="2">AScott</asp:ListItem>
                        </asp:DropDownList>
                        <label id="labBussinessType" class="error"></label>
			        </dd>
		        </dl>
		        <dl>
			        <dt>所在地区：</dt>
			        <dd>
                        <asp:DropDownList CssClass="select" ID="ddlAreaType" runat="server">
                            <asp:ListItem>请选择</asp:ListItem>
                            <asp:ListItem Value="0">华东</asp:ListItem>
                            <asp:ListItem Value="1">华北</asp:ListItem>
                            <asp:ListItem Value="2">华南</asp:ListItem>
                            <asp:ListItem Value="3">西南</asp:ListItem>
                            <asp:ListItem Value="4">凯德城镇开发</asp:ListItem>
                        </asp:DropDownList>
                        <label id="labAreaType" class="error"></label>
			        </dd>
		        </dl>
		        <dl>
			        <dt>负责人：</dt>
			        <dd><asp:TextBox ID="txtRealName" MaxLength="20" CssClass="txt required" runat="server"></asp:TextBox></dd>
		        </dl>
		        <dl>
			        <dt>所在部门：</dt>
			        <dd><asp:TextBox ID="txtDepartment" MaxLength="20" CssClass="txt required" runat="server"></asp:TextBox></dd>
		        </dl>
		        <dl>
			        <dt>电　　话：</dt>
			        <dd><asp:TextBox ID="txtTel" MaxLength="15" CssClass="txt required" runat="server"></asp:TextBox></dd>
		        </dl>
		        <dl>
			        <dt>手　　机：</dt>
			        <dd><asp:TextBox ID="txtMobil" MaxLength="15" CssClass="txt required" runat="server"></asp:TextBox></dd>
		        </dl>
		        <dl>
			        <dt>邮　　箱：</dt>
			        <dd><asp:TextBox ID="txtEmail" MaxLength="50" runat="server" CssClass="txt required"></asp:TextBox></dd>
		        </dl>
		        <dl>
			        <dt>状　　态：</dt>
			        <dd>
				        <label class="radioArea"><input runat="server" id="radStatusOk" type="radio" class="radio" name="accountState"/>正常</label>
				        <label class="radioArea"><input type="radio" id="radStatusFalse" runat="server" class="radio" name="accountState"/>禁用</label>
                        <label class="error" id="labState"></label>
			        </dd>
		        </dl>
	        </div>
            <%--前连接地址--%>
            <asp:HiddenField ID="hfReturlUrl" runat="server" />
	        <div class="btnPanel_01">
                <asp:Button ID="btnSubmit" CssClass="btn1 btn" runat="server" Text="确定" OnClick="SubmitButtom_Click" />
                <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn btn2"  OnClick="CancelButton_Click">取消</asp:LinkButton>
	        </div>
        </section>
    </form>
    <script src="../scripts/controls.js"></script>
    <script>
        nav.change('<%=m_id%>'); 
    </script>
</body>
</html>
