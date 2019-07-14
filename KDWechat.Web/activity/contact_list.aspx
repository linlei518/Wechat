<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="contact_list.aspx.cs" Inherits="KDWechat.Web.activity.contact_list" %>


<%@ Import Namespace="KDWechat.Common" %>

<!DOCTYPE html>
<%@ Register Src="../UserControl/TopControl.ascx" TagName="TopControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/MenuList.ascx" TagName="MenuList" TagPrefix="uc2" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title><%=pageTitle %></title>
     <script src="../scripts/html5.js"></script>
      <link type="text/css" href="../styles/global.css" rel="stylesheet">
    <link type="text/css" href="../styles/style.css" rel="stylesheet">
</head>



     




<body class="mainbody">
    <form id="form2" runat="server">
           <uc1:TopControl ID="TopControl1" runat="server" />
        <uc2:MenuList ID="MenuList1" runat="server" />
        <!--导航栏-->
         <section id="main">
                    <%=NavigationName %>
                <div class="titlePanel_01">
                    <div class="btns" runat="server" id="div_add">
                       <asp:LinkButton ID="btnExport" runat="server"  CssClass="btn btn3" OnClick="btnExport_Click"><i class="add"></i>批量导出</asp:LinkButton>
                       
                    </div>
                    <h1>咨询信息管理列表</h1>
                </div>
                  <div class="searchPanel_01">
                <div class="searchField">
                    <input type="text" class="txt searchTxt" placeholder="公司名称..." runat="server" id="txtKeywords">
                    <asp:Button ID="btnSearch" runat="server" Text="" CssClass="btn searchBtn" ToolTip="点击搜索" OnClick="btnSearch_Click"></asp:Button>
                </div>
                <div class="filterField">

                    <%--<asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="True" AppendDataBoundItems="true" OnSelectedIndexChanged="btnSearch_Click"  CssClass="select">
                                  <asp:ListItem Value="-1" Selected="True">咨询类容</asp:ListItem>
                                    <asp:ListItem Value="0" >关于产品</asp:ListItem>
                                    <asp:ListItem Value="1" >关于环保</asp:ListItem>
                                    
                                </asp:DropDownList>--%>

                </div>
            </div>
                <div class="tablePanel_01">
                    <table cellpadding="0" cellspacing="0" class="table">
                        <thead>
                            <tr>
                           <%--  <th width="5%">选择</th>--%>
                          <%--  <th align="left"width="10%">咨询内容</th>--%>
                            <th align="left"width="10%">产品栏目</th>
                             <th align="left"width="10%">产品名称</th>
                            <th align="left"width="5%">姓名</th>
                                <th align="left"width="15%">公司名</th>
                              <th align="left"width="5%">电话号码</th>
                            <th align="left" width="5%">申请时间</th>
                            <th width="10%">操作</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptList" runat="server" OnItemCommand="rptList_ItemCommand">
                                <ItemTemplate>
                                    <tr>
                     
                       <%-- <td><%#Utils.ObjToInt(Eval("type"),0)==0?"关于产品":"关于环保"%> </td>--%>
                        <td ><%#Eval("product_menu")%></td>
                         <td ><%#Eval("product_name")%></td>
                         <td ><%#Eval("user_name")%></td>
                          <td ><%#Eval("company_name")%></td>
                        <td ><%#Eval("phone")%></td>
                        <td ><%#string.Format("{0:g}",Eval("create_date"))%></td>
                        <td align="center">
                            <%# (isEdit?" <a  class=\"btn btn6\" href=\"contact_edit.aspx?action="+Enums.RoleActionType.Edit+"&id="+Eval("id")+"&m_id="+m_id+"\">查看</a>":"") %>
                             <asp:LinkButton CssClass="btn btn6" ID="lbtnDel" runat="server" Visible='<%#true %>' OnClientClick="return confirm('您确定要删除吗？删除后咨询信息将无法恢复！');" ToolTip='删除'  CommandArgument='<%# Eval("id") %>' CommandName="del">删除</asp:LinkButton>
                        </td>
                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <%# rptList.Items.Count == 0 ? "<tr><td style=\"text-align:center;\" colspan=\"8\">暂无数据</td></tr>" : ""%>
                                </FooterTemplate>
                            </asp:Repeater>
                            <asp:HiddenField ID="hfReturlUrl" runat="server" />
                        </tbody>
                    </table>
                    <div class="pageNum" id="div_page" runat="server">
                    </div>
                </div>
        </section>
    </form>

   <script src="../scripts/jquery-1.10.2.min.js"></script>
        <script src="../scripts/controls.js"></script><!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
        <script src="../scripts/Bombbox.js"></script>
        <script src="../scripts/statisticsContrast.js"></script>
        <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
        <script>
           

            nav.change('<%=m_id%>'); 
        </script>
</body>

<script>
        //创建窗口
         function showdialog(id) {
            var objNum = arguments.length;
            var d = top.dialog({
                title: '积分商品组织机构',
                url: '/product/product_dpt.aspx?id=' + id,
                width: 600,
                height: 440,
                onclose: function () {
                }
            }).showModal();

        }
    </script>