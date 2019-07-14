<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="user_list.aspx.cs" Inherits="KDWechat.Web.zh_user.user_list" %>

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
    <form id="form1" runat="server">
           <uc1:TopControl ID="TopControl1" runat="server" />
        <uc2:MenuList ID="MenuList1" runat="server" />
        <!--导航栏-->
         <section id="main">
                    <%=NavigationName %>
                <div class="titlePanel_01">
                    <div class="btns" runat="server" id="div_add">
                         <asp:FileUpload ID="fup" runat="server" Width="150px"  CssClass='upload-box'  />
                       <asp:LinkButton ID="btnExport" runat="server"  CssClass="btn btn3" OnClick="btnExport_Click"><i class="add"></i>批量导入</asp:LinkButton>
                        <a  href="user_edit.aspx?action=ADD&m_id=<%=m_id %>" class="btn btn3"> <i  class="add"></i>新增</a>
                        
                    </div>
                    <h1>员工列表</h1>
                </div>
                  <div class="searchPanel_01">
                <div class="searchField">
                    <input type="text" class="txt searchTxt" placeholder="员工姓名/车牌号..." runat="server" id="txtKeywords">
                    <asp:Button ID="btnSearch" runat="server" Text="" CssClass="btn searchBtn" ToolTip="点击搜索" OnClick="btnSearch_Click"></asp:Button>
                </div>
                <div class="filterField">

                    <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="True" AppendDataBoundItems="true" OnSelectedIndexChanged="btnSearch_Click"  CssClass="select">
                                    <asp:ListItem Value="-1" Selected="True">绑定状态</asp:ListItem>
                                    
                   </asp:DropDownList>

                </div>
            </div>
                <div class="tablePanel_01">
                    <table cellpadding="0" cellspacing="0" class="table">
                        <thead>
                            <tr>
                            <th width="6%">选择</th>
                            <th align="left"width="5%">姓名</th>
                            <th align="left" width="5%">员工编号</th>
                            <th align="left" width="5%">手机</th>
                            <th align="left" width="5%">邮箱</th>
                            <th align="left" width="5%">部门</th>  
                            <th align="left" width="5%">职位</th>
                            <th align="left" width="15%">车牌号</th>
                           <th align="left" width="5%">绑定状态</th>
                            <th width="20%">操作</th>
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
                            <asp:Literal ID="lblTitle" runat="server" Text='<%#Eval("user_name") %>'></asp:Literal></td>
                        <td><%# Eval("user_code") ?? "/" %></td>
                        <td><%# Eval("user_tel")%></td>
                        <td ><%#Eval("user_mail")%></td>
                        <td><%# Eval("user_dpt") ?? "/" %></td>
                        <td><%# Eval("user_position") ?? "/" %></td>
                        <td><%# Eval("plate_number") ?? "/" %></td>
                        <td><%# Utils.ObjToInt(Eval("status"),0)==0?"否":"是"  %></td>
                        <td align="center">
                            <%# (true?" <a href=\"user_edit.aspx?action="+Enums.RoleActionType.Edit+"&id="+Eval("id")+"&m_id="+m_id+"\" class=\"btn btn6\">修改</a>":"") %>
                            <asp:LinkButton CssClass="btn btn6" ID="lbtnDel" runat="server" Visible='<%#true %>' OnClientClick="return confirm('您确定要删除吗？删除后用户将无法恢复！');" ToolTip='删除'  CommandArgument='<%# Eval("id") %>' CommandName="del">删除</asp:LinkButton>
                             <asp:LinkButton CssClass="btn btn6" ID="LinkButton1" runat="server" Visible='<%#true %>' OnClientClick="return confirm('您确定要解除微信绑定吗？');" ToolTip='解除绑定'  CommandArgument='<%# Eval("id") %>' CommandName="unbound">解除微信绑定</asp:LinkButton>

                        </td>
                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <%# rptList.Items.Count == 0 ? "<tr><td style=\"text-align:center;\" colspan=\"10\">暂无数据</td></tr>" : ""%>
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

             //dialogue.dlShowPic('/product/qr_code_show.aspx?id=' + id);
             //bombbox.openBox('/product/qr_code_show.aspx?id=' + id);
             //var objNum = arguments.length;
             //var d = top.dialog({
             //    title: '二维码显示',
             //    url: '/product/qr_code_show.aspx?id=' + id,
             //    width: 600,
             //    height: 440,
             //    onclose: function () {
             //    }
             //}).showModal();

         }
     </script>