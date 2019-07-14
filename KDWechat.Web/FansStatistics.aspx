<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FansStatistics.aspx.cs" Inherits="KDWechat.Web.FansStatistics" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <script src="Scripts/jquery-1.10.2.min.js"></script>
    <script type="text/javascript">
        <%--$(function () {
            
        }); //饼图--%>
        $(function () {
            //Damos 加
            $('#containerPie').highcharts({
                chart: {
                    plotBackgroundColor: null,    <%--背景色--%>
                    plotBorderWidth: null,        <%--宽度--%>
                    plotShadow: false             <%--阴影--%>
                },
                title: {
                    text: '人数统计'           <%--标题--%>
                },
                tooltip: {
                    formatter: function () {
                        return '订阅人数：<b>'+Highcharts.numberFormat(this.y, 0, ',') + '</b>(人)'; <%--鼠标覆盖时显示的内容--%>
                    }
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,  <%--允许选中--%>
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                            style: {
                                color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                            }
                        },
                        showInLegend:true
                    }
                },
                series: [{
                    type: 'pie',
                    name: '总关注人数',
                    data: [<%= pieChartData%>]
                }],
                credits: {
                    enabled:false         <%--禁用右下角的水印--%>
                }
            });
            $('#container').highcharts({
                chart: {
                    type: 'area',         <%--图表类型：区域图（阴影图）--%>
                    zoomType: 'x'
                },
                title: {
                    text: '<%=chartName%>'       <%--图标标题--%>
                },
                subtitle: {
                    text: '<%=chartSubName%>',        <%--图表副标题--%>
                    floating: true               <%--设置浮动，标题漂浮于图表上，有效利用空间--%>
                },
                xAxis: {
                    categories: [<%= chartDateRange %>],   <%--图表的x轴，现在指定为一周日期--%>
                    tickInterval: <%= chartXInterval%>      <%--X轴显示间隔--%>
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
                    enable: false
                },
                series: [{
                    name: '<%=chartSerisName1%>',     <%--序列1的名字--%>
                    type:"area",
                    data: [<%=chartSeris1%>]          <%--序列1的数据--%>
                },
                {
                    name: '<%=chartSerisName2%>',<%--同上--%>
                    type:"line",
                    data: [<%=chartSeris2%>]
                }],
                credits: {
                    text: "中华企业网", <%--这里可以手动设置版权信息，如果不需要，可采用enabled=false--%>
                    href: "http://www.companycn.com" <%--这里设置版权地址--%>
                }
            });

            $('#containerAuto').highcharts({
                chart: {
                    type: 'area', <%--图表类型：区域图（阴影图）--%>
                    zoomType: 'x'
                },
                title: {
                    text: '关注人数对比'                <%--图标标题--%>
                },
                subtitle: {
                    text: '公众号之间的关注人数对比',    <%--图表副标题--%>
                    floating: true                         <%--设置浮动，标题漂浮于图表上，有效利用空间--%>
                },
                xAxis: {
                    categories: [<%= chartDateRange2 %>],  <%--图表的x轴，现在指定为一周日期--%>
                    tickInterval: <%= chartXInterval2%>    <%--X轴显示间隔--%>
                    },
                yAxis: {
                    title: {
                        text: '人数（人）'                <%--图表的Y轴名--%>
                    },
                    plotLines: [{                          <%--Y轴提示线，可指定样式或隐藏--%>
                        value: 0,
                        width: 1,
                        color: '#e8e8e8'
                    }]
                },
                tooltip: {
                    valueSuffix: '人',                      <%--鼠标悬浮在点上时，提示的单位--%>
                    crosshairs: true,                       <%--设置穿透线--%>
                    shared: true                            <%--在tooltip里显示两条线的值--%>
                },
                legend: {
                    enable: false
                },
                series: [<%=JsonData%>],
                credits: {
                    text: "中华企业网",                 <%--这里可以手动设置版权信息，如果不需要，可采用enabled=false--%>
                    href: "http://www.companycn.com"     <%--这里设置版权地址--%>
                }
            });

            //柱状图
            $('#containerColumn').highcharts({
                chart: {
                    type: 'bar',                         <%--类型柱状图--%>
                    zoomType: 'x'                        <%--允许缩放--%>
                },
                title: {
                    text: '关注人数'
                },
                xAxis: {
                    categories: [<%= chartDateRange %>],
                    title: {
                        text: null
                    }
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: '人数 (人)',
                        align: 'high'
                    },
                    labels: {
                        overflow: 'justify'
                    }
                },
                tooltip: {
                    valueSuffix: ' 人'<%--单位--%>
                },
                plotOptions: {
                    bar: {
                        dataLabels: {
                            enabled: true
                        }
                    }
                },
                legend: {<%--这个是比例尺，下面的属性比较好理解，不做一一解释--%>
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'top',
                    x: -40,
                    y: 100,
                    borderWidth: 1,
                    backgroundColor: (Highcharts.theme && Highcharts.theme.legendBackgroundColor || '#FFFFFF'),
                    shadow: true
                },
                credits: {
                    enabled: false
                },
                series: [<%=JsonData%>]
            });

        }
            );
	</script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <script src="scripts/highchart/js/highcharts.js"></script>
        <script src="scripts/highchart/js/modules/exporting.js"></script>
        <input type="text" id="days" /><input type="button" value="查询" onclick="location.href='fansstatistics.aspx?day='+document.getElementById('days').value;"/>
        <a href="FansStatistics.aspx?day=7">7天</a>&nbsp;&nbsp;&nbsp;&nbsp;<a href="FansStatistics.aspx?day=14">14天</a>&nbsp;&nbsp;&nbsp;&nbsp;<a href="FansStatistics.aspx?day=30">30天</a>
        <div id="container" style="width:100%; height: 400px;margin: 0 auto"></div>
        <div id="containerPie" style="width:500px; height: 400px;margin: 0 auto"></div>
        <div id="containerAuto" style="width:100%; height: 400px;margin: 0 auto"></div>
        <div id="containerColumn" style="height:1000px"></div>

    </div>
    </form>
</body>
</html>
