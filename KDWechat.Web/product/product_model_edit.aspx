<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="product_model_edit.aspx.cs" Inherits="KDWechat.Web.product.product_model_edit"  %>
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

<body class="mainbody">
    <form id="form1" runat="server">
          <uc1:TopControl ID="TopControl1" runat="server" />
    <uc2:MenuList ID="MenuList1" runat="server" />
       <section id="main">
        <!--内容-->
       <div class="breadcrumbPanel_01">
                <h1><span>产品列表</span><i class="breadcrumbArrow"></i><em><%=this.id==0?"新建":"编辑" %>产品型号</em></h1>
            </div>

       <div class="listPanel_01 bottomLine">
            <dl>
                <dt>产品型号名称</dt>
                <dd>
                    <asp:TextBox ID="txtName" runat="server" CssClass="txt required" datatype="*2-20" sucmsg=" "></asp:TextBox>
                    <span class="Validform_checktip">*产品型号名称，20个字符内。</span></dd>
            </dl>
             <dl>
                <dt>产品型号PDF链接</dt>
                <dd>
                    <asp:TextBox ID="txt_link" runat="server" CssClass="txt required" datatype="*2-100" sucmsg=" "></asp:TextBox>
                    <span class="Validform_checktip">*产品PDF链接，100个字符内。</span></dd>
            </dl>
              <dl>
                <dt>排序数字</dt>
                <dd>
                    <asp:TextBox ID="txtSortId" runat="server" CssClass="txt required" Width="50px" datatype="n" sucmsg=" ">99</asp:TextBox>
                    <span class="Validform_checktip">*数字，越小越向前</span>
                </dd>
            </dl>
            <dl>
                <dt>是否启用</dt>
                <dd>
                    <div class="rule-single-checkbox">
                        <asp:CheckBox ID="cbIsLock" runat="server" Checked="True" />
                    </div>
                    <span class="Validform_checktip">*隐藏后不显示在前台分类中。</span>
                </dd>
            </dl>
        </div>
        <!--/内容-->

        <!--工具栏-->
        <div class="page-footer">
            <div class="-wrap">
                <asp:HiddenField ID="hfReturnUrl" runat="server" />
                 <% if (RequestHelper.GetQueryString("action").ToUpper()  == "EDIT"||RequestHelper.GetQueryString("action").ToUpper() == "ADD")
                    { %>
                <asp:Button ID="btnSubmit" runat="server" Text="提交保存" CssClass="btn1 btn"  CommandArgument="save" OnClick="btnSubmit_Click"  />
                <% } %>
                <input name="btnReturn" type="button" value="返回上一页" class="btn btn2" onclick="javascript: location.href = '<%=hfReturnUrl.Value%>    '" />
            </div>
        </div>
           </section>

         <script src="../scripts/jquery-1.10.2.min.js"></script>
        <script src="../scripts/controls.js"></script><!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
        <script src="../scripts/Bombbox.js"></script>
        <script src="../scripts/statisticsContrast.js"></script>
        <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
        <script>
           

            nav.change('<%=m_id%>'); 
        </script>
        

    </form>
</body>
</html>