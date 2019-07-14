<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="nav_edit.aspx.cs" Inherits="KDWechat.Web.setting.nav_edit" %>

<%@ Register Src="../UserControl/TopControl.ascx" TagName="TopControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/MenuList.ascx" TagName="Sys_menulist" TagPrefix="uc2" %>

<!doctype html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=pageTitle %></title>
    <script src="../scripts/html5.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet">
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>

<body>
    <form id="form1" runat="server">
        <uc1:TopControl ID="TopControl1" runat="server" />
        <uc2:Sys_menulist ID="MenuList1" runat="server" />
        <section id="main">

            <div class="titlePanel_01">
                <h1><%=id==0?"新增":"编辑" %>导航菜单</h1>
            </div>
            <div class="listPanel_01 bottomLine">
                <dl>
                    <dt>菜单类型：</dt>
                    <dd>
                        <asp:DropDownList ID="ddlMenuType" runat="server" CssClass="select required" OnSelectedIndexChanged="ddlMenuType_SelectedChanged" AutoPostBack="true">
                            <asp:ListItem Value="3">公众号菜单</asp:ListItem>
                            <asp:ListItem Value="1">总部账号</asp:ListItem>
                            <asp:ListItem Value="2">地区账号</asp:ListItem>
                            <asp:ListItem Value="4">推荐达人平台</asp:ListItem>
                            <asp:ListItem Value="5">租客平台</asp:ListItem>
                            <asp:ListItem Value="6">凯德商用系统（总部）</asp:ListItem>
                            <asp:ListItem Value="7">凯德商用系统（地区）</asp:ListItem>
                        </asp:DropDownList>

                    </dd>

                </dl>
                <dl>
                    <dt>上级导航：</dt>
                    <dd>
                        <asp:DropDownList ID="ddlParentId" runat="server" CssClass="select required"></asp:DropDownList>

                    </dd>

                </dl>

                <dl>
                    <dt>导航标题：</dt>
                    <dd>
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="txt required" MaxLength="100"></asp:TextBox>
                        <label id="lblTitle" class="error" style="display: inline;"></label>
                        <span>*导航中文标题，100字符内</span>
                    </dd>
                </dl>

                <dl>
                    <dt>调用别名：</dt>
                    <dd>
                        <asp:TextBox ID="txtName" runat="server" CssClass="txt required" datatype="/^[a-zA-Z0-9\-\_]{2,50}$/"></asp:TextBox>
                        <span>权限控制名称，只允许字母、数字、下划线</span>
                        <label id="lblName" class="error" style="display: inline;"></label>
                    </dd>
                </dl>
                <dl>
                    <dt>菜单类型：</dt>
                    <dd>
                        <asp:DropDownList ID="ddlType" runat="server" CssClass="select required">
                            <asp:ListItem Value="-1">请选择</asp:ListItem>
                            <asp:ListItem Value="0">总部菜单</asp:ListItem>
                            <asp:ListItem Value="1">地区菜单</asp:ListItem>
                              <asp:ListItem Value="2">地区和总部公用菜单</asp:ListItem>
                        </asp:DropDownList>
                        <span>如是根目录菜单，必选选择一个类型</span>
                    </dd>
                </dl>
                <dl>
                    <dt>菜单打开方式：</dt>
                    <dd>
                        <asp:DropDownList ID="ddlTargetType" runat="server" CssClass="select required">
                            <asp:ListItem Value="">当前窗口</asp:ListItem>
                            <asp:ListItem Value="target='_blank'">新窗口</asp:ListItem>
                        </asp:DropDownList>
                        <span>针对根目录菜单</span>

                    </dd>
                </dl>
                <dl>
                    <dt>排序数字：</dt>
                    <dd>
                        <asp:TextBox ID="txtSortId" runat="server" CssClass="txt required" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')">99</asp:TextBox>
                        <span>*数字，越小越向前</span>

                    </dd>
                </dl>
                <dl>
                    <dt>链接地址：</dt>
                    <dd>
                        <asp:TextBox ID="txtLinkUrl" runat="server" MaxLength="255" CssClass="txt" />

                        <span>当前管理目录，有子导航不用填</span>
                    </dd>
                </dl>
                <dl>
                    <dt>子系统登录地址：</dt>
                    <dd>
                        <asp:TextBox ID="txtSubTitle" runat="server" MaxLength="255" CssClass="txt" />

                        <span>添加子系统根目录时需要填写子系统同步登录的地址</span>
                    </dd>
                </dl>
                <dl>
                    <dt>系统菜单：</dt>
                    <dd>
                        <asp:RadioButtonList ID="rbtnIssystem" runat="server" CssClass="select required" RepeatDirection="Horizontal" RepeatLayout="Flow">
                            <asp:ListItem Value="1">是</asp:ListItem>
                            <asp:ListItem Selected="True" Value="0">否</asp:ListItem>
                        </asp:RadioButtonList>
                    </dd>
                </dl>

                <dl>
                    <dt>是否禁用：</dt>
                    <dd>
                        <asp:RadioButtonList ID="rbtnIshide" runat="server" CssClass="select required" RepeatDirection="Horizontal" RepeatLayout="Flow">
                            <asp:ListItem Value="1">是</asp:ListItem>
                            <asp:ListItem Selected="True" Value="0">否</asp:ListItem>
                        </asp:RadioButtonList>
                    </dd>
                </dl>
                <dl>
                    <dt>图标样式</dt>
                    <dd>
                        <asp:TextBox ID="txtRemark" runat="server" CssClass="txt"></asp:TextBox>
                        <span>非必填，可为空</span>
                    </dd>
                </dl>
                <dl>
                    <dt>权限资源</dt>
                    <dd>
                        <div class="rule-multi-porp">
                            <asp:CheckBoxList ID="cblActionType" runat="server" RepeatDirection="Horizontal" CssClass="select required" RepeatLayout="Flow"></asp:CheckBoxList>
                        </div>
                    </dd>
                </dl>
            </div>

            <div class="btnPanel_01">
                <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn1" Text="确定" OnClick="btnSubmit_Click"></asp:Button>
                <input id="btnReset" type="reset" value="取消" class="btn btn2" onclick="location.href = 'nav_list.aspx?m_id=<%=m_id%>    '" />
            </div>

        </section>
        <%--  <asp:HiddenField ID="hfReturlUrl" runat="server" />--%>
        <script src="../scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
        <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
        <script src="../Scripts/jquery.validate/jquery.validate.js" type="text/javascript"></script>
        <script src="../Scripts/jquery.validate/jquery.metadata.js" type="text/javascript"></script>
        <script src="../Scripts/jquery.validate/messages_cn.js" type="text/javascript"></script>
        <script src="../scripts/controls.js" type="text/javascript"></script>
        <script type="text/javascript">

            //选中菜单
            nav.change('<%=m_id%>'); 
        </script>
        <script type="text/javascript">
            $(function () {

                $("#form1").validate({
                    rules: {
                        txtSortId: {
                            required: true,
                            minlength: 0,
                            maxlength: 999
                        },
                        txtName: "required",
                        txtTitle: "required"
                    },
                    messages: {
                        txtSortId: "请输入排序数字",
                        txtName: "请输入调用别名",
                        txtTitle: "请输入导航标题"
                    }, submitHandler: function (form) {

                        //发送AJAX请求,检查引用ID是否可用
                        var oldname ="<%=oldnavname%>";
                        var navname = $("#txtName").val().trim();
                        var res = false;
                        $.ajax({
                            type: "post",
                            url: "../handles/wechat_ajax.ashx?action=navigation_validate&name="+navname+"&old_name="+oldname+"&time=" + Math.random(),
                            dataType: "json",
                            async: false,
                            success: function (data) {
                                
                               
                                if (data.status == "y") {
                                    $("#lblName").text(data.info);
                                    $("#lblName").show();
                                    res = true;
                                }
                                else
                                {
                                   
                                    $("#lblName").text(data.info);
                                    $("#lblName").show();
                                    res= false;
                                }

                            }
                        });
                        if (res)
                        {
                            form.submit();
                        }
                       
                       
                    }
                });
            });

        </script>
    </form>
</body>
</html>
