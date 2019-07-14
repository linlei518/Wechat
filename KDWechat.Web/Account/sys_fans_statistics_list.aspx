<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sys_fans_statistics_list.aspx.cs" Inherits="KDWechat.Web.Account.sys_user_statics" %>

<%@ Register TagName="TopControl" Src="~/UserControl/TopControl.ascx" TagPrefix="uc" %>

<%@ Register TagName="MenuList" Src="~/UserControl/MenuList.ascx" TagPrefix="uc" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8"/>
    <title><%=pageTitle %></title>
    <link type="text/css" href="../styles/global.css" rel="stylesheet"/>
    <script src="../scripts/html5.js"></script>
    <script src="../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/HighChart/js/highcharts.js"></script>
    <script src="../Scripts/HighChart/js/modules/exporting.js"></script>
    <script src="../Scripts/sys_fans_statistics.js"></script>
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
	<script type="text/javascript">
        //Damos 加
		$(function () {
		    $('#container').highcharts({
		        chart: {
		            type: 'column' <%--图表类型：区域图（阴影图）--%>
		        },
		        title: {
		            text: '<%=chartName%>' <%--图标标题--%>
		        },
		        subtitle: {
		            text: '<%=chartSubName%>'<%--图表副标题--%>
		            //floating: true <%--设置浮动，标题漂浮于图表上，有效利用空间--%>
		        },
		        xAxis: {
		            categories: [<%= chartDateRange %>]<%--图表的x轴，现在指定为一周日期--%>
		        },
		        yAxis: {
		            title: {
		                text: '<%=chartYName%>' <%--图表的Y轴名--%>
		            },
		            plotLines: [{ <%--Y轴提示线，可指定样式或隐藏--%>
		                value: 0,
		                width: 1,
		                color: '#e8e8e8'
		            }]
		        },
		        tooltip: {
		            valueSuffix: '<%=chartUnit%>', <%--鼠标悬浮在点上时，提示的单位--%>
		            crosshairs: true,              <%--设置穿透线--%>
		            shared: true                   <%--在tooltip里显示两条线的值--%>
		        },
		        legend: {
		            enable:false
		        },
		        series: [{
		            name: '<%=chartSerisName1%>',     <%--序列1的名字--%>
		            data: [<%=chartSeris1%>]          <%--序列1的数据--%>
		        }<%--,
		        {
		            name: '<%=chartSerisName2%>',//同上
		            data: [<%=chartSeris2%>]
		        }--%>],
		        credits: {
                    enabled:false
		            //text: "中华企业网", <%--这里可以手动设置版权信息，如果不需要，可采用enabled=false--%>
                    //href:"http://www.companycn.com" <%--这里设置版权地址--%>
                }
		    });
		}
        );
	</script>
</head>
<body>
    <uc:TopControl ID="TopControl1" runat="server" />
    <uc:MenuList ID="MenuList1" runat="server" />
    <form id="form1" runat="server">
        <section id="main">
                    <%=NavigationName %>
            <div class="titlePanel_01">
                <h1>关注用户统计</h1>
            </div>

            <div class="filterPanel_01">
                <div id="container"></div>
                <dl class="selectedList selected"><dt>已选择：</dt><dd class="btns"> <a href="javascript:RemoveTag(1)" class="btn filterCancel">全部撤销</a></dd><dd id="dou"></dd></dl>
                <dl runat="server" id="dlArea">
			        <dt>所在区域：</dt>
			        <dd>
                        <a href="javascript:getList(0)" class="btn filterSelect">华东</a>
                        <a href="javascript:getList(1)" class="btn filterSelect">华北</a>
                        <a href="javascript:getList(2)" class="btn filterSelect">华南</a>
                        <a href="javascript:getList(3)" class="btn filterSelect">西南</a>
                        <a href="javascript:getList(4)" class="btn filterSelect">凯德城镇开发</a>
			        </dd>
		        </dl>
                <dl id="dlShow">
                    <dt>已选公众号：</dt>
                    <dd id="ddShow">

                    </dd>
                    <dd>
                        <input id="submi" type="button" class="btn btn6" onclick="ContrastAll()" value="对比" />
                    </dd>
                </dl>
                <table cellpadding="0" cellspacing="0" style="width:100%" class="table">
                        <thead>
                            <tr>
                                <th class="name">名称</th>
                                <th class="info info1" style=" width:20%">类型</th>
                                <th class="time" style=" width:135px">对接时间</th>
                                <th class="control" style=" width:185px">操作</th>
                            </tr>
                        </thead>
                        <tbody id="tbd">
                            <asp:Repeater ID="repWxList" runat="server" OnItemCommand="repWxList_ItemCommand" >
                                <ItemTemplate>
                                    <tr>
                                        <td class="name"><%#Eval("wx_pb_name") %></td>
                                        <td class="info info1"  style=" width:20%"> <%#(KDWechat.Common.WeChatServiceType)(int.Parse(Eval("type_id").ToString())) %></td>
                                        <td class="time" style=" width:135px"> <%#Eval("create_time") %></td>
                                        <td class="control"  style=" width:185px">
                                            <a href="sys_fans_wx_statistics.aspx?m_id=<%=m_id %>&id=<%#Eval("id") %>" class="btn btn6" >查看详细</a>
                                            <a href="javascript:AddChange('<%#Eval("wx_pb_name") %>',<%#Eval("id") %>);" id="aLabel<%#Eval("id") %>" class="btn btn6" >加入对比</a>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <%# repWxList.Items.Count == 0 ? "<tr><td style=\"text-align:center;\" colspan=\"4\">暂无数据</td></tr>" : ""%>
                                </FooterTemplate>
                            </asp:Repeater>
                            <asp:HiddenField ID="hfReturlUrl" runat="server" />
                        </tbody>
                    </table>
	        </div>
        </section>
        <script src="../Scripts/controls.js"></script>
        <script src="../scripts/main.js"></script>
        <script>
            nav.change('<%=m_id%>'); 
        </script>
    </form>
</body>
</html>


