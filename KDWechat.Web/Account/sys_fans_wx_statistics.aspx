<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sys_fans_wx_statistics.aspx.cs" Inherits="KDWechat.Web.Account.sys_fans_wx_statistics" %>
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
    <!--[if lt IE 9 ]><link href="styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
		<script type="text/javascript">
            //Damos 加
		    $(function () {
		        $('#container').highcharts({
		            chart: {
		                type: 'area' <%--图表类型：区域图（阴影图）--%>
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
		            },
		            {
		                name: '<%=chartSerisName2%>',<%--同上--%>
		                data: [<%=chartSeris2%>]
		            }],
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
                  <div class="btns">
                    <a href="sys_fans_statistics_list.aspx?m_id=87" class="btn btn5"><i class="black back"></i>返回</a>
                  </div>
                <h1>公众号信息</h1>
              </div>
              <div class="weixinInfoPanel_01">
		        <div class="logoField">
			        <span><img src="<%=head_url %>" width="180" height="180" alt=""></span>
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
				        <dd><%= wx_apiurl %></dd>
			        </dl>
			        <dl>
				        <dt>默认Token：</dt>
				        <dd><%=wx_token %></dd>
			        </dl>
		        </div>
	          </div>
            	<div class="statisticsListPanel_01">
		            <div class="listNTab">
                          <a runat="server" id="fansTag" class="btn nTabBtn"><i class="statistics"></i>关注用户统计</a>
			            <a runat="server" id="msgTag" class="btn nTabBtn"><i class="statistics"></i>消息统计</a>
                        <%--此处的统计暂时隐藏 -Damos<asp:HyperLink ID="columnTag" href="sys_fans_wx_statistics.aspx?tag=2" CssClass="btn nTabBtn" runat="server"><i class="statistics"></i>栏目访问统计</asp:HyperLink>--%>
		            </div>
		            <div class="listField">
			            <div class="statistics">
				            <div class="content">
                                <div style="width:auto; height:292px;" id="container"></div>
				            </div>
			            </div>
		            </div>
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

