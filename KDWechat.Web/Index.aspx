<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="KDWechat.Web.Index" %>

<%@ Register TagName="TopControl" Src="~/UserControl/TopControl.ascx" TagPrefix="uc" %>

<%@ Register TagName="MenuList" Src="~/UserControl/MenuList.ascx" TagPrefix="uc" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <title>公众号信息-<%=wx_name %>微信管理后台</title>
    <link type="text/css" href="styles/global.css" rel="stylesheet" />
    <script src="scripts/html5.js"></script>

    <!--[if lt IE 9 ]><link href="styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->

</head>
<body>
    
    <uc:TopControl ID="TopControl1" runat="server" />
    <uc:MenuList ID="MenuList1" runat="server" />
    <form id="form1" runat="server">
        <section id="main">
            <div class="titlePanel_01">
                <h1>公众号信息</h1>
            </div>
            <div class="weixinInfoPanel_01">
                <div class="logoField">
                    <span>
                        <img src="<%=head_url %>" width="180" height="180" alt="" /></span>
                </div>
                <div class="listField">
                    <dl>
                        <dt>公众号名称：</dt>
                        <dd><%= wx_pb_name %></dd>
                    </dl>
                    <dl>
                        <dt>公众号类型：</dt>
                        <dd><%= wxType %></dd>
                    </dl>
                    <dl>
                        <dt>公众号原始ID：</dt>
                        <dd><%=wx_og_id %></dd>
                    </dl>
                    <dl>
                        <dt>微信号：</dt>
                        <dd><%=wx_name %></dd>
                    </dl>
                    <dl>
                        <dt>默认接口地址：</dt>
                        <dd><span id="ddApi"><%= wx_apiurl %></span>
                            <a href="javascript:copyString.copy('<%=wx_apiurl %>')" style="margin-left: 8px;" class="btn btn6">复制</a>
                        </dd>
                    </dl>
                    <dl>
                        <dt>默认Token：</dt>
                        <dd><span id="ddToken"><%=wx_token %></span>

                            <a href="javascript:copyString.copy('<%=wx_token %>')" style="margin-left: 8px;" class="btn btn6">复制</a>
                        </dd>
                    </dl>
                </div>
            </div>
        </section>
        <script src="Scripts/jquery-1.10.2.min.js"></script>
        <script src="Scripts/controls.js"></script>
        <script src="scripts/main.js"></script>
        <script>
            nav.change('<%=m_id%>'); 
        </script>
    </form>
</body>
</html>
