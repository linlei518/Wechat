<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QRCode_trend.aspx.cs" Inherits="KDWechat.Web.Statistics.QRCode_trend" %>

<%@ Register Src="../UserControl/TopControl.ascx" TagName="TopControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/MenuList.ascx" TagName="Sys_menulist" TagPrefix="uc2" %>


<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title>推广渠道管理二维码统计功能</title>
    <script src="../scripts/html5.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet">
</head>
<body>
    <uc1:TopControl ID="TopControl1" runat="server" />
    <uc2:Sys_menulist ID="MenuList1" runat="server" />
    <form id="form1" runat="server">
        <section id="main">
            <%=NavigationName %>
            <div class="statisticsListPanel_01">
                <div class="listField">

                    <div class="statistics">
                        <h2><%=chartDateStr %>  &nbsp&nbsp&nbsp 累计数量： <%=count %>条</h2>
                    </div>
                </div>
            </div>
            <div class="statisticsListPanel_01">
                <div class="listField">

                    <div class="title">
                        <div class="filter">
                            <asp:Button ID="btnRegDate" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;" Text="" OnClick="Button1_Click" />
                            <a href="javascript:DaysCLick(7)" class="btn btn5">7天</a> <a href="javascript:DaysCLick(14)" class="btn btn5">14天</a> <a href="javascript:DaysCLick(30)" class="btn btn5">30天</a>
                            <asp:TextBox ID="txtbegin_date" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;"></asp:TextBox>
                            <asp:TextBox ID="txtend_date" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;" onfocus="selectEndDate();"></asp:TextBox>
                            <input type="text" class="txt date" runat="server" id="txt_date_show" placeholder="按时间筛选" onfocus="selectStartDate();" />
                        </div>
                        <h2>二维码统计</h2>
                    </div>
                    <div class="statistics">
                        <h3></h3>
                        <div class="content">
                        </div>
                    </div>
                </div>
                <%--<div class="listField">
                    <div class="title">
                        <h2>详细数据</h2>
                    </div>
                    <table cellpadding="0" cellspacing="0" class="table" title="showLength:10,from:0">
                        <thead>
                            <tr>
                                <th class="name sorts sortUp"><i class="sort"></i>时间</th>
                                <th class="info info2 sorts"><i class="sort"></i>消息发送人数</th>
                                <th class="info info1 sorts"><i class="sort"></i>消息发送次数</th>
                                <th class="info info1 sorts"><i class="sort"></i>人均发送条数</th>
                            </tr>
                        </thead>
                        <tbody>
                            <%=detailTable %>
                        </tbody>
                    </table>

                </div>--%>
            </div>
        </section>
    </form>
    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../scripts/controls.js"></script>
    <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
    <script src="../scripts/Bombbox.js"></script>
    <script src="../scripts/statisticsContrast.js"></script>
    <script src="../Scripts/HighChart/js/highcharts.js"></script>
    <script src="../Scripts/HighChart/js/exporting.js"></script>
    <script src="../Scripts/DatePicker/WdatePicker.js"></script>
    <script>
         function DaysCLick(days)
            {
                $('#txtbegin_date').val(AddDays(0-days).toString().replace("年","-").replace("月","-").replace("日",""));
                $("#txt_date_show").val("");
                $("#btnRegDate").click();
            }

        function AddDays(n)
            {
                var time=new Date().getTime();
                var newTime=time+n*24*60*60*1000;
                return new Date(newTime).toLocaleDateString();
            }
        //日期选择
        function selectStartDate() {
            var txtbegin_date = $dp.$('txtbegin_date');
            var txtend_date = $dp.$('txtend_date');
            var txt_date_show = $dp.$('txt_date_show');

            WdatePicker(
            {
                position: { left: -198, top: 10 },
                el: 'txtbegin_date',
                onpicked: function (dp) {
                    txt_date_show.value = dp.cal.getNewDateStr();
                    txtend_date.value = "";
                    txtend_date.focus();
                },
                onclearing: function () {
                    txt_date_show.value = "";
                    txtend_date.value = "";
                    txtbegin_date.value = "";
                },
                doubleCalendar: true,
                isShowClear: true,
                readOnly: true,
                dateFmt: 'yyyy-MM-dd',
                maxDate: '%y-%M-%d'

            });
        }

        function selectEndDate() {
            var txt_date_show = $dp.$('txt_date_show');
            WdatePicker({
                position: { left: -120, top: 10 },
                doubleCalendar: true,
                isShowClear: true,
                readOnly: true,
                dateFmt: 'yyyy-MM-dd',
                minDate: '#F{$dp.$D(\'txtbegin_date\',{d:0});}',
                maxDate:'%y-%M-%d',
                onpicked: function (dp) {
                    if (txt_date_show.value.length > 0) {
                        txt_date_show.value += " — " + dp.cal.getNewDateStr();
                        $("#btnRegDate").click();
                    } else {
                        txt_date_show.value = dp.cal.getNewDateStr();
                    }

                }
            });
        }
        $(function () {
            $('.content').highcharts({
                chart: {                   
                },
                title: {
                    text: '推广渠道二维码统计'
                },
               <%-- xAxis: {
                    categories: [<%= chartDateRange %>]                   
                },--%>
                //yAxis: {
                //    title: {
                //        text: '二维码数量' //--图表的Y轴名-
                //    },
                //    plotLines: [{ //--Y轴提示线，可指定样式或隐藏-
                //        value: 0,
                //        width: 1,
                //        color: '#e8e8e8'
                //    }],
                //    allowDecimals:false//Y轴不显示小数                   
                //},
                plotOptions: {
                    pie: {
                        allowPointSelect: true,  <%--允许选中--%>
                        cursor: 'pointer',
                        dataLabels:{
                            enabled: true, // dataLabels设为true在图形上显示数字
                            format: '<b>{point.name}</b>: {y} ',
                            style: {
                                color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                            }
                        }
                    }
                },
                //tooltip: {
                //    valueSuffix: '个', //--鼠标悬浮在点上时，提示的单位
                //    crosshairs: true,              //--设置穿透线
                //    shared: true                   //--在tooltip里显示两条线的值-
                //},
                tooltip: {
                    pointFormat: '<b>{point.percentage:.1f}%</b>'
                },
                legend: {
                    enabled:false //不显示分类小图标
                },
                series: [{
                    name: '数量',     <%--序列1的名字--%>
                    type:"pie",
                    data: [<%=chartSeris1%>]      <%--序列1的数据--%>
                    }],
                credits: {
                    enabled: false         <%--禁用右下角的水印--%>
                }
            });
        });
        nav.change(<%=m_id%>);

    </script>
    <script src="../Scripts/HighChart/js/themes/KDTheme.js"></script>
</body>
</html>
