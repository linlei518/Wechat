<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="video_list.aspx.cs" Inherits="KDWechat.Web.material.video_list" %>


<%@ Register Src="../UserControl/TopControl.ascx" TagName="TopControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/MenuList.ascx" TagName="MenuList" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/material_search.ascx" TagName="material_search" TagPrefix="uc3" %>
<!doctype html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=pageTitle %></title>
    <script src="../scripts/html5.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet">
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>

<body>
    <form id="form1" runat="server">
        <uc1:TopControl ID="TopControl1" runat="server" />
        <uc2:MenuList ID="MenuList1" runat="server" />


        <section id="main">
            <%=NavigationName %>
            <div class="titlePanel_01">
                <div class="btns">
                    <%=isAdd==true?"<a href=\"video_add.aspx?m_id="+m_id+"&is_pub="+is_pub+"\" class=\"btn btn3\"><i class=\"add\"></i>新建视频</a>":"" %>
                </div>
                <h1>视频列表</h1>
            </div>

            <uc3:material_search ID="material_search1" page_link="video_list.aspx" isshow_group="1" runat="server" />

            <div class="tablePanel_01">
                <table cellpadding="0" cellspacing="0" class="table">
                    <thead>
                        <tr>
                            <th class="check file"></th>
                            <th class="name">视频标题</th>
                            <th class="info info1" style="width: 20%">所属分组</th>
                            <th class="info info1" style="width: 60px">视频类型</th>
                            <th class="info info1" style="width: 40px">预览</th>
                            <th class="time" style="width: 115px">创建时间</th>
                            <th class="control2" style="width: 106px; padding: 0 10px; text-align: center">操作</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="repList" runat="server" OnItemCommand="repList_ItemCommand">
                            <ItemTemplate>
                                <tr>
                                    <td class="check file">
                                        <asp:CheckBox ID="chkId" CssClass="checkall" runat="server" />
                                        <asp:HiddenField ID="hidId" Value='<%#Eval("id")%>' runat="server" />
                                        <asp:HiddenField ID="hf_img" Value='<%#Eval("hq_music_url")%>' runat="server" />
                                    </td>
                                    <td class="name">
                                        <a href="video_add.aspx?id=<%# Eval("id") %>&is_pub=<%=is_pub %>&m_id=<%=m_id %>">
                                            <asp:Literal ID="lblTitle" runat="server" Text='<%# Eval("title") %>'></asp:Literal></a>
                                    </td>
                                    <td class="info info1" style="width: 20%"><%# Eval("group_name") %></td>
                                    <td class="info info1" style="width: 60px"><%# Eval("video_type").ToString()=="1"?"本地视频":"微视" %></td>
                                    <td class="info info1" style="width: 40px"><a href="<%# Eval("file_url") %>" target="_blank">播放</a></td>
                                    <td class="time" style="width: 115px"><%# Eval("create_time","{0:yyyy/MM/dd HH:mm}") %></td>

                                    <td class="control2" style="width: 106px; padding: 0 10px;">

                                           <%# isEdit==true?"<a href=\"video_add.aspx?m_id="+m_id+"&id="+Eval("id")+"&is_pub="+is_pub+"\" class=\"btn btn6\"   >编辑</a>":"" %>

                                        <asp:LinkButton ID="lbtnDelete" CssClass="btn btn6" CommandArgument='<%# Eval("id") %>'  Visible='<%# isDelete %>' CommandName="del" OnClientClick="return confirm('您确认要删除吗?');" runat="server" Text='删除'></asp:LinkButton>
                                    </td>
                                </tr>

                            </ItemTemplate>
                            <FooterTemplate>
                                <%#repList.Items.Count == 0 ? "<tr><td style=\"text-align:center;\" colspan=\"6\">暂无数据</td></tr>" : ""%>
                            </FooterTemplate>
                        </asp:Repeater>


                    </tbody>
                </table>
                <%-- 需要引用function.js--%>
                <div class="pageNum" id="div_page" runat="server">
                </div>
            </div>

        </section>
        <script src="../Scripts/jquery-1.10.2.min.js"></script>
        <script src="../Scripts/function.js"></script>
        <script src="../scripts/controls.js"></script>
        <script>nav.change('<%=m_id%>');</script>
    </form>
</body>
</html>

