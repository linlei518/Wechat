<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="menu_click_statistics.aspx.cs" Inherits="KDWechat.Web.Statistics.menu_click_statistics" %>

<%@ Register Src="../UserControl/TopControl.ascx" TagName="TopControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/MenuList.ascx" TagName="Sys_menulist" TagPrefix="uc2" %>


<!doctype html>
<html>
<head>
<meta charset="UTF-8">
<title>收发信息统计-微信管理后台</title>
<script src="../scripts/html5.js"></script>
<link type="text/css" href="../styles/global.css" rel="stylesheet">
<!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
<!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
<!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>

<body>
    <uc1:TopControl ID="TopControl1" runat="server" />
    <uc2:Sys_menulist ID="MenuList1" runat="server" />
    <form runat="server">
    <section id="main">
        <%=NavigationName %>		
		
	    <div class="statisticsListPanel_01">
<%--			<div class="listNTab">
			    <a href="users_trend.aspx?m_id=<%=m_id %>" class="btn nTabBtn current">用户增长</a>
			    <a href="users_property.aspx?m_id=<%=m_id %>" class="btn nTabBtn">用户属性</a>
		    </div>--%>
		    <div class="listField">
			    <div class="statistics">
				    <h2><%=startTime.ToShortDateString()+" 至 "+endTime.ToShortDateString() %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;菜单点击次数： <%=hitCount %>次</h2>
			    </div>
		    </div>
	    </div>
	    <div class="statisticsListPanel_01">
		    <div class="listField">
			    <div class="title">
				    <div class="filter">
                        <asp:Button ID="btnRegDate" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;" Text="" OnClick="Button1_Click" />
					    <a href="javascript:DaysCLick(7)" class="btn btn5">7天</a> <a href="javascript:DaysCLick(14)" class="btn btn5">14天</a> <a href="javascript:DaysCLick(30)" class="btn btn5">30天</a><%-- <input class="txt date" type="text" value="2014/10/11 - 2014/11/11"> <input type="button" class="btn btn5" value="选择">--%>
                        <asp:TextBox ID="txtbegin_date" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;"></asp:TextBox>
                        <asp:TextBox ID="txtend_date" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;" onfocus="selectEndDate();"></asp:TextBox>
                        <input type="text" class="txt date" runat="server" id="txt_date_show" placeholder="按时间筛选" onfocus="selectStartDate();" />
				    </div>
				    <h2>菜单点击分析</h2>
			    </div>
			    <div class="statistics">
				    <h3>菜单点击统计</h3>
				    <div class="content" style="height:<%=chartHeight%>px">
				    </div>
			    </div>
		    </div>
	    </div>
	
    </section>
    </form>

    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../scripts/controls.js"></script><!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
    <script src="../scripts/Bombbox.js"></script>
    <script src="../scripts/statisticsContrast.js"></script>
    <script src="../Scripts/HighChart/js/highcharts.js"></script>
    <script src="../Scripts/HighChart/js/exporting.js"></script>
    <script src="../Scripts/DatePicker/WdatePicker.js"></script>
    <!--弹出框JS 调用方法：1.开启弹出框：bombbox.openBox('链接地址，可以带参')，2.关闭弹出框：bombbox.closeBox();注意：此方法无需在弹出框里面的页面引用-->
    <script>

    function tableNTab(node,btns,num){
	    node = $(node);
	    btns = $(btns);
	    node.addClass('hidden').eq(num).removeClass('hidden');
	    btns.removeClass('current').eq(num).addClass('current');
    }

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
                type: 'bar'
            },
            title: {
                text: ''
            },
            xAxis: {
                categories: [<%=barChartData.TrimEnd(',')%>],
                title: {
                    text: null
                }
            },
            yAxis: {
                min: 0,
                title: {
                    text: '',
                    align: 'high'
                },
                labels: {
                    overflow: 'justify'
                }
            },
            tooltip: {
                valueSuffix: ' '
            },
            plotOptions: {
                bar: {
                    dataLabels: {
                        enabled: true
                    }
                }
            },
            legend: {
                enabled:false
            },
            credits: {
                enabled: false
            },
            series: [{
                name: '点击率',
                data: [<%=hitChartData.TrimEnd(',')%>],
                tooltip: {
                    valueSuffix: '%'
                }
            }]
        });
    });

        nav.change(<%=m_id%>);

    </script>
    <script src="../Scripts/HighChart/js/themes/KDTheme.js"></script>
</body>
</html>
