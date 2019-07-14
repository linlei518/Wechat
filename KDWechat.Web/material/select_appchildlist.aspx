<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="select_appchildlist.aspx.cs" Inherits="KDWechat.Web.material.select_appchildlist" %>

<!doctype html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <title>选择应用</title>
    <script src="../scripts/html5.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet" />
    <!--[if lt IE 9 ]><link href="/styles/ie8Fix.css" rel="stylesheet" type="text/css"/><![endif]-->
    <!--[if lt IE 8 ]><link href="/styles/ie7Fix.css" rel="stylesheet" type="text/css"/><![endif]-->
    <!--[if lt IE 7 ]><link href="/styles/ie6Fix.css" rel="stylesheet" type="text/css"/><![endif]-->
</head>
<body>
    <%--<form id="form1" runat="server">--%>
        <header id="bombboxTitle">
            <div class="titlePanel_01">
                <h1>
                    <asp:Literal ID="lblTypeName" runat="server"></asp:Literal></h1>
            </div>
        </header>
        <section id="bombboxMain">


            <div class="tablePanel_01">
                <table cellpadding="0" cellspacing="0" class="table">
                    <thead>
                        <tr>
                            <th class="name">标题</th>
                            <th class="info info1"  <%=is_360?"":"style='display:none'" %>>类型</th>
                            <th class="time">创建时间</th>
                            <th class="selectControl">操作</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="repList" runat="server">
                            <ItemTemplate>
                                   <tr>
                                    <td class="name"><span <%# GetClassName(Eval("level")) %>><%# Eval("app_name") %></span></td>
                                    <td class="info info1"  <%=is_360?"":"style='display:none'" %>><%# GetRoomType(Eval("app_table")) %></td>
                                    <td class="time"><%# Eval("create_time","{0:yyyy/MM/dd HH:mm}") %></td>
                                    <td class="selectControl">
                                        <input type="button" value="选择" class="btn btn5" onclick="javascript: window.parent.materialAddModule.selectApp({ id: '<%# Eval("app_id") %>', title: '<%# lblTypeName.Text %>', content: '<%# KDWechat.Common.Utils.ToHtml( Eval("app_all_name").ToString()) %> ', img: '<%# Eval("app_img_url") %>',link:'<%# Eval("app_link_url") %> ' }); window.parent.bombbox.closeBox();">
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                         
                    </tbody>
                </table>
                <div class="pageNum" id="div_page" runat="server">
                </div>
            </div>

        </section>
        <script src="../scripts/jquery-1.10.2.min.js"></script>
<script src="../scripts/controls.js"></script><!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
<!--如果页面table标签内内容更新 请在更新好后调用一次setupTable方法-->
<script>
    var offsetSize = {
        width:880,
        height:420
    }
</script>
   <%-- </form>--%>
</body>
</html>
