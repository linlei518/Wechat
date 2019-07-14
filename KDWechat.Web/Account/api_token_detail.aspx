<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="api_token_detail.aspx.cs" Inherits="KDWechat.Web.Account.api_token_detail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="UTF-8" />
    <title><%=pageTitle %></title>
    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../scripts/html5.js"></script>
    <script src="../Scripts/controls.js"></script>

    <link type="text/css" href="../styles/global.css" rel="stylesheet" />
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>
<body>
    <form runat="server" id="form1">

            <section id="bombboxMain">

                <div class="tablePanel_01">
                    <dl>
                        <dt>接口地址：</dt>
                        <dd>
                            <asp:TextBox ID="txtApiUrl" CssClass="txt" runat="server"></asp:TextBox><input onclick="copyString.copy('<%=txtApiUrl.Text %>')" style="margin-left:2px;" class="btn btn6" type="button" id="txtcopy1" value="复制" /></dd>
                    </dl>
                    <dl>
                        <dt>接口Token：</dt>
                        <dd>
                            <asp:TextBox ID="txtToken" CssClass="txt" runat="server"></asp:TextBox><input onclick="copyString.copy('<%=txtToken.Text %>')" style="margin-left:2px;" class="btn btn6" type="button" id="txtcopy2" value="复制" />
                        </dd>
                    
                    </dl>
                    <br />
                    <input type="button" class="btn btn1" value="返回" onclick="parent.location.replace('region_wxlist.aspx?m_id=<%if (u_type == 1 || u_type == 4) { Response.Write("97"); } else { Response.Write("59"); } %>');" />                            
                    <label>配置完成后，请点击返回完成创建</label>

                </div>

                <asp:HiddenField ID="hfReturlUrl" runat="server" />

            </section>
    </form>
</body>
</html>

