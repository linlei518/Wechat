<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="contact_edit.aspx.cs" Inherits="KDWechat.Web.activity.contact_edit"  %>




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
        <!--导航栏-->
        <section id="main">
        <!--内容-->
       <div class="breadcrumbPanel_01">
                <h1><span>咨询信息列表</span><i class="breadcrumbArrow"></i><em>查看咨询信息</em></h1>
            </div>

       <div class="listPanel_01 bottomLine">
           
            
             <dl>
                <dt>产品栏目</dt>
                <dd>
                    <asp:TextBox ID="txtproduct_menu" runat="server" CssClass="txt required" datatype="*2-20" sucmsg=" "></asp:TextBox>
            </dl>
            <dl>
                <dt>产品名称</dt>
                <dd>
                    <asp:TextBox ID="txtproduct_name" runat="server" CssClass="txt required" datatype="*2-20" sucmsg=" "></asp:TextBox>
            </dl>
            <dl>
                <dt>咨询内容</dt>
                <dd>
                    <asp:TextBox ID="txtcontent" runat="server" CssClass="txt required" datatype="*2-500" sucmsg=" " TextMode="MultiLine"></asp:TextBox>
            </dl>
              <dl>
                <dt>姓名</dt>
                <dd>
                    <asp:TextBox ID="txtname" runat="server" CssClass="txt required" datatype="*2-20" sucmsg=" "></asp:TextBox>
                  
                </dd>
            </dl>
              <dl>
                <dt>电话号码</dt>
               <dd>
                    <asp:TextBox ID="txtphone" runat="server" CssClass="txt required" datatype="*2-20" sucmsg=" "></asp:TextBox>
                  
                </dd>
            </dl>
             <dl>
                <dt>邮箱</dt>
               <dd>
                    <asp:TextBox ID="txtemail" runat="server" CssClass="txt required" datatype="*2-20" sucmsg=" "></asp:TextBox>
                  
                </dd>
            </dl> 
           <dl>
                <dt>公司名称</dt>
               <dd>
                    <asp:TextBox ID="txtcompany_name" runat="server" CssClass="txt required" datatype="*2-20" sucmsg=" "></asp:TextBox>
                  
                </dd>
            </dl>
             <dl>
                <dt>部门/职位</dt>
               <dd>
                    <asp:TextBox ID="txtdpt_name" runat="server" CssClass="txt required" datatype="*2-20" sucmsg=" "></asp:TextBox>
                  
                </dd>
            </dl>
             <dl>
                <dt>地址</dt>
               <dd>
                    <asp:TextBox ID="txtadress" runat="server" CssClass="txt required" datatype="*2-20" sucmsg=" "></asp:TextBox>
                  
                </dd>
            </dl>
            <dl>
                <dt>申请时间</dt>
               <dd>
                    <asp:TextBox ID="txtcreate_date" runat="server" CssClass="txt required" datatype="*2-20" sucmsg=" "></asp:TextBox>
                </dd>
            </dl>
        </div>
        <!--/内容-->

        <!--工具栏-->
        <div class="page-footer">
            <div class="-wrap">
                <asp:HiddenField ID="hfReturnUrl" runat="server" />
                <%-- <% if (RequestHelper.GetQueryString("action").ToUpper()  == "EDIT"||RequestHelper.GetQueryString("action").ToUpper() == "ADD")
                    { %>
                <asp:Button ID="btnSubmit" runat="server" Text="提交保存" CssClass="btn"  CommandArgument="save" OnClick="btnSubmit_Click"  />
                <% } %>--%>
                <input name="btnReturn" type="button" value="返回上一页" class="btn btn2" onclick="javascript: location.href = '<%=hfReturnUrl.Value%>'" />
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