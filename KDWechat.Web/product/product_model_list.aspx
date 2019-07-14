<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="product_model_list.aspx.cs" Inherits="KDWechat.Web.product.product_model_list" %>

<%@ Import Namespace="KDWechat.Common" %>
<%@ Register Src="../UserControl/TopControl.ascx" TagName="TopControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/MenuList.ascx" TagName="MenuList" TagPrefix="uc2" %>
  

<!DOCTYPE html>

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
                        <a  href="product_model_edit.aspx?action=ADD&product_id=<%=product_id %>&m_id=<%=m_id %>" class="btn btn3"> <i  class="add"></i>新增</a>
                         <asp:LinkButton ID="LinkButton1" runat="server" OnClick="btnSort_Click" CssClass="btn btn3"><i class="add"></i>保存排序</asp:LinkButton>
                         <input name="btnReturn" type="button" value="返回上一页" class="btn btn5" onclick="javascript: history.go(-1);" />
                    </div>
                    <h1>产品列表</h1>
                </div>
                  <div class="searchPanel_01">
                <div class="searchField">
                    <input type="text" class="txt searchTxt" placeholder="产品型号名称..." runat="server" id="txtKeywords">
                    <asp:Button ID="btnSearch" runat="server" Text="" CssClass="btn searchBtn" ToolTip="点击搜索" OnClick="btnSearch_Click"></asp:Button>
                </div>
                <div class="filterField">

                    <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="True" AppendDataBoundItems="true" OnSelectedIndexChanged="btnSearch_Click"  CssClass="select">
                                    <asp:ListItem Value="-1" Selected="True">发布状态</asp:ListItem>
                                     <asp:ListItem Value="0" >否</asp:ListItem>
                                    <asp:ListItem Value="1" >是</asp:ListItem>
                                </asp:DropDownList>

                </div>
            </div>
                <div class="tablePanel_01">
                    <table cellpadding="0" cellspacing="0" class="table">
                        <thead>
                            <tr>
                            <th width="5%">选择</th>
                            <th align="left"width="25%">型号名称</th>
                            <th align="left"width="5%">启用状态</th>
                            <th align="left" width="5%">创建时间</th>
                             <th align="left" width="5%">排序</th>
                            <th width="10%">操作</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptList" runat="server" OnItemCommand="rptList_ItemCommand">
                                <ItemTemplate>
                                    <tr>
                         <td align="center">
                            <asp:CheckBox ID="chkId" CssClass="checkall" runat="server" Style="vertical-align: middle;" />
                            <asp:HiddenField ID="hidId" Value='<%#Eval("id")%>' runat="server" />
                        </td>
                        <td>
                            <asp:Literal ID="lblTitle" runat="server" Text='<%#true?" <a href=\"product_model_edit.aspx?action="+Enums.RoleActionType.Edit+"&id="+Eval("id")+"\">"+Eval("name")+"</a>":Eval("name") %>'></asp:Literal></td>
                         <td ><%#Utils.ObjToInt(Eval("is_publish"),0)==0?"否":"是"%></td>
                        <td ><%#string.Format("{0:g}",Eval("create_date"))%></td>
                        <td style="text-align: left;">
                                <asp:TextBox ID="txtSortId" runat="server" Text='<%#Eval("sort_id")%>' CssClass="txt" Width="50px" onkeydown="return checkNumber(event);" /></td>
                        <td align="center">
                            <a href="model_dic_list.aspx?model_id=<%#Eval("id")%>&m_id=<%=m_id %>">属性管理</a>
                            <%# (true?" <a href=\"product_model_edit.aspx?action="+Enums.RoleActionType.Edit+"&id="+Eval("id")+"&product_id="+Eval("product_id")+"&m_id="+m_id+"\">修改</a>":"") %>
                            <asp:LinkButton ID="lbtnDel" runat="server" OnClientClick="return confirmstatus(this,'您确定要{0}吗？','删除','删除后用户将无法恢复！');" ToolTip='删除'  CommandArgument='<%# Eval("id") %>' CommandName="del">删除</asp:LinkButton>
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
    
    

</html>

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