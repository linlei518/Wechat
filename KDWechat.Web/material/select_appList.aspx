<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="select_appList.aspx.cs" Inherits="KDWechat.Web.material.select_appList" %>

<!doctype html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <title><%=pageTitle %></title>
    <script src="../scripts/html5.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet" />
    <link type="text/css" href="../styles/style.css" rel="stylesheet" />
    <!--[if lt IE 9 ]><link href="/styles/ie8Fix.css" rel="stylesheet" type="text/css"/><![endif]-->
    <!--[if lt IE 8 ]><link href="/styles/ie7Fix.css" rel="stylesheet" type="text/css"/><![endif]-->
    <!--[if lt IE 7 ]><link href="/styles/ie6Fix.css" rel="stylesheet" type="text/css"/><![endif]-->
</head>
<body class="bombbox">
    <header id="bombboxTitle">
        <div class="titlePanel_01">
            <h1>选择应用</h1>
        </div>
    </header>
    <form id="form1" runat="server">
        <section id="bombboxMain">

            <div class="appListPanel_01">
                <div class="listNTab">

                    <a href="select_appList.aspx?tag=0" class="btn nTabBtn <%=tag==0?" current":"" %>">全部应用</a>
                    <a href="select_appList.aspx?tag=1" class="btn nTabBtn <%=tag==1?" current":"" %>">资讯应用</a>
                    <a href="select_appList.aspx?tag=2" class="btn nTabBtn <%=tag==2?" current":"" %>">活动应用</a>
                    <a href="select_appList.aspx?tag=3" class="btn nTabBtn <%=tag==3?" current":"" %>">业务应用</a>
                    <a href="select_appList.aspx?tag=4" class="btn nTabBtn <%=tag==4?" current":"" %>">其他</a>
                </div>
                <div class="listField">
                    <ul>

                        <asp:Repeater ID="repAllSysModule" runat="server"  >
                            <itemtemplate>
                                <li  >
                                    <div class="img">
                                        <span><img src="<%#Eval("img_url") %>" alt=""></span>
                                    </div>
                                    <div class="info">
                                        <div class="title">
                                            <h2><%#Eval("title") %></h2>
                                        </div>
                                        <div class="text">
                                            <p><%#KDWechat.Common.Utils.CutString(Eval("description"),60) %></p>
                                        </div>
                                        <div class="btns">
                                    
                                            <a href="select_appchildlist.aspx?id=<%#Eval("id") %>" class="btn">选择</a>
                                        </div>
                                    </div>
                                </li>
                            </itemtemplate>
                        </asp:Repeater>
                        <%=repAllSysModule.Items.Count==0?" <li style='height:100px; width:300px; margin:auto'>暂无功能应用</li>":"" %>
                        
                    </ul>
                </div>
                   <div class="pageNum" id="div_page" runat="server">
			 
		        </div>
            </div>

        </section>
        <script src="../scripts/jquery-1.10.2.min.js"></script>
        <script src="../scripts/controls.js"></script><!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
        <!--如果页面table标签内内容更新 请在更新好后调用一次setupTable方法-->
        <script>
            var offsetSize = {
                width: 880,
                height: 420
            }
        </script>
    </form>
</body>
</html>