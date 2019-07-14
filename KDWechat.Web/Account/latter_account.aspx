<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="latter_account.aspx.cs" Inherits="KDWechat.Web.Account.latter_account" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="UTF-8" />
    <title><%=pageTitle %></title>
    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../scripts/html5.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet" />
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>
<body>
    <form runat="server" id="form1">


            <header id="bombboxTitle">
                <div class="titlePanel_01">
                    <h1>站内信送达账号</h1>
                </div>
            </header>
            <section id="bombboxMain">

                <div class="tablePanel_01">
                    <asp:Repeater ID="repItem" runat="server">
                        <HeaderTemplate>
                            <table cellpadding="0" cellspacing="0" class="table">
                                <thead>
                                    <tr>
                                        <th class="name">账号名称</th>
                                        <th class="info info1"  style=" width:20%">负责人</th>
                                        <th class="info info1"  style=" width:40px">状态</th>
                                        <th class="time" style=" width:115px">最后上线时间</th>

                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>

                        <ItemTemplate>
                            <tr>
                                <td class="name"><%#Eval("user_name") %></td>
                                <td class="info info1"  style=" width:20%"><%#Eval("real_name") %></td>
                                <td class="info info1"  style=" width:40px"><%#((KDWechat.Common.Status)int.Parse(Eval("status").ToString())).ToString() %></td>
                                <td class="time"  style=" width:115px"><%#Eval("login_time") %></td>

                            </tr>
                        </ItemTemplate>

                        <FooterTemplate>
                            <%# repItem.Items.Count == 0 ? "<tr><td style=\"text-align:center;\" colspan=\"4\">暂无数据</td></tr>" : ""%>
                            </tbody>
		            </table>
                        </FooterTemplate>
                    </asp:Repeater>
               
                 <div class="pageNum" id="div_page" runat="server">
                    </div> 
               </div>
            </section>
        <script src="../scripts/controls.js"></script>
        <script src="../Scripts/function.js"></script>
        <script>
            function rediec(id) {
                parent.bombbox.closeBox();
                parent.location.href = "ChildrenAccount_Edit.aspx?id=" + id + "&m_id=<%=m_id %>";
                return false;
            }
            nav.change('<%=m_id%>');
			
			 var offsetSize = {//这玩意定义弹出框的高宽
                     width: 900,
                     height: 560
                 }
			
        </script>

    </form>
</body>
</html>
