<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="select_pic.aspx.cs" Inherits="KDWechat.Web.select_pic" %>

<%@ Register Src="UserControl/material_search.ascx" TagName="material_search" TagPrefix="uc3" %>

<!doctype html>
<html>
<head>
    <meta charset="UTF-8">
    <title>选择图片</title>
    <script src="../scripts/html5.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet">
    <!--[if lt IE 9 ]><link href="styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>
<body class="bombbox">
    <form id="form1" runat="server">
        <header id="bombboxTitle">
            <div class="titlePanel_01">
                <div class="btns">
                    <asp:Literal ID="lblPublic" runat="server"></asp:Literal>
                </div>
                <h1>选择图片</h1>
            </div>
        </header>
        <section id="bombboxMain">

            <uc3:material_search ID="material_search1" runat="server" />
            <div class="picListPanel_01">
                <div class="picList">
                    <ul>
                        <asp:Repeater ID="repList" runat="server" OnItemCommand="repList_ItemCommand">
                            <ItemTemplate>
                                <li>
                                    <div class="img">
                                        <a href='<%# Eval("cover_img") %>' target="_blank">
                                            <img src='<%# Eval("cover_img") %>' alt=""></a>
                                    </div>
                                    <div class="info">
                                        <p><%--<%# GetPcSize(Eval("cover_img")) %>--%></p>
                                        <h2>
                                            <asp:Literal ID="lblTitle" runat="server" Text='<%# Eval("title") %>'></asp:Literal></h2>
                                    </div>
                                    <div class="control">
                                        <input type="button" class="btn choose" value="选择" onclick="selectThis('<%# Eval("cover_img") %>')">
                                        <asp:HiddenField ID="hffile" runat="server" Value='<%#Eval("cover_img") %>' />
                                        <a href='<%# Eval("cover_img") %>' target="_blank" class="btn view" title="查看" type="button"></a>


                                        <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return confirm('删除后将无法回复,您确定要删除吗?');" CommandArgument='<%# Eval("id") %>' CommandName="del" CssClass="btn delete" ToolTip="删除" type="button"></asp:LinkButton>
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>

                    </ul>
                </div>
                <div class="pageNum" id="div_page" runat="server">
                </div>
            </div>
        </section>

        <script src="../scripts/jquery-1.10.2.min.js"></script>
        <script src="../scripts/controls.js"></script>
        <script src="../Scripts/function.js"></script>
        <script>
            function selectThis(img)
            {
                var type="<%=type%>";
                if (type=="news") {
                    parent.getimgBynews(img);

                }else if(type=="project"){

                    var list_str =  parent.$("#<%=KDWechat.Common.RequestHelper.GetQueryString("hf")%>").val();
                    if (list_str == "") {
                        parent.$("#<%=KDWechat.Common.RequestHelper.GetQueryString("hf")%>").val(img);
                    } else {
                        parent.$("#<%=KDWechat.Common.RequestHelper.GetQueryString("hf")%>").val(list_str + "," + img);
                    }

                    parent.showimg('<%=KDWechat.Common.RequestHelper.GetQueryString("hf")%>','<%=KDWechat.Common.RequestHelper.GetQueryString("ul")%>',img);
                    parent.bombbox.closeBox();
                }else if(type=="invite_child"){

                    var list_str =  parent.$("#<%=KDWechat.Common.RequestHelper.GetQueryString("hf")%>").val();
                   // if (list_str == "") {
                        parent.$("#<%=KDWechat.Common.RequestHelper.GetQueryString("hf")%>").val(img);
                   // } else {
                      //  parent.$("#<%=KDWechat.Common.RequestHelper.GetQueryString("hf")%>").val(list_str + "," + img);
                   // }

                    parent.showimg('<%=KDWechat.Common.RequestHelper.GetQueryString("hf")%>','<%=KDWechat.Common.RequestHelper.GetQueryString("ul")%>',img);
                    parent.bombbox.closeBox();
                } else  if (type=="hfcard2") {
                    parent.getimg('txtEndFile','img_end',img);

                }else  if (type=="invite_logo") {
                    parent.getimg('txtLogo','logo_show',img);

                }else {
                    var c_text="<%=KDWechat.Common.RequestHelper.GetQueryString("hf")%>";
                    var c_img="<%=KDWechat.Common.RequestHelper.GetQueryString("img")%>";
                    if(c_text!="" && c_img!=""){
                        parent.getimg(c_text,c_img,img);
                    }else{
                        parent.getimg('txtFile','img_show',img);
                     }
                    
                }
               
           

            }

        </script>

    </form>
</body>
</html>
