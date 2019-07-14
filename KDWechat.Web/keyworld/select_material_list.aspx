<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="select_material_list.aspx.cs" Inherits="KDWechat.Web.keyworld.select_material_list" %>


<%@ Register Src="../UserControl/material_search.ascx" TagName="material_search" TagPrefix="uc3" %>

<!doctype html>
<html>
<head>
    <meta charset="UTF-8">
    <title>选择素材列表</title>
    <script src="../scripts/html5.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet">
    <!--[if lt IE 9 ]><link href="/styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="/styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="/styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>

<body class="bombbox">
 <form id="form1" runat="server">
        <header id="bombboxTitle">
	<div class="titlePanel_01">
        <div class="btns">
            <asp:Literal ID="lblPublic" runat="server"></asp:Literal>
		</div>
		<h1>选择<%=channel_name%></h1>
	</div>
</header>
 <section id="bombboxMain">
	<uc3:material_search ID="material_search1"     runat="server" />
	<div class="tablePanel_01 materialList selectTable">
		<table cellpadding="0" cellspacing="0" class="table">
			<thead>
				<tr>
					<th class="name">标题</th>
                    <%=style_name %>
					
					<th class="info info1">所属分组</th>
                    <%=media_style_name %>
                    
					<th class="time">创建时间</th>
					<th class="selectControl">操作</th>
				</tr>
			</thead>
			<tbody>
                <asp:Repeater ID="repList" runat="server">
                    <ItemTemplate>
                        	<tr>
					            <td class="name"><%# Eval("title") %></td>
	
                                <%# GetImageShow(Eval("cover_img")) %>
 					            <td class="info info1"><%# GetGroupName(Eval("group_id")) %></td>
                                <%# GetMediaShow(Eval("cover_img"),Eval("video_type")) %>
					            <td class="time"><%# Eval("create_time","{0:yyyy/MM/dd HH:mm}") %></td>
					            <td class="selectControl">

						            <input type="button" value="选择" class="btn btn5" onclick='selectThis(this)'>
                                    <input type="hidden" class="title" value="<%#Eval("title") %>" />
                                    <input type="hidden" class="cover_img" value="<%#Eval("cover_img") %>" />
                                    <input type="hidden" class="summary" value="<%# KDWechat.Common.Utils.DropHTML( Eval("summary").ToString(),40) %>" />
                                    <input type="hidden" class="create_time" value="<%# Eval("create_time","{0:yyyy-MM-dd}") %>" />
                                    <input type="hidden" class="id" value="<%#Eval("id") %>" />
                                    <input type="hidden" class="hq_music_url" value="<%#Eval("hq_music_url") %>" />
                                    <input type="hidden" class="child_list" value='<%# GetChildList(Eval("id")) %>' />
                                    <input type="hidden" class="video_type" value='<%#Eval("video_type") %>' />
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
 </form>
    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../scripts/controls.js"></script>
    <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
    <script src="../Scripts/function.js"></script>
    <script>
        function selectThis(obj_this)
        {
            var channel_id=<%=channel_id%>;
            parent.selectResult(channel_id, $(obj_this).parent().find(".id").val(),$(obj_this).parent().find(".cover_img").val() , $(obj_this).parent().find(".title").val(), $(obj_this).parent().find(".create_time").val(), $(obj_this).parent().find(".summary").val(), 1, $(obj_this).parent().find(".child_list").val(),$(obj_this).parent().find(".hq_music_url").val(),$(obj_this).parent().find(".video_type").val());
           
        }

    </script>
</body>
</html>
