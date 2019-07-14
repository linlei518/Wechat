<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="fans_statistics.aspx.cs" Inherits="KDWechat.Web.Account.fans_statistics" %>

<%@ Register Src="../UserControl/TopControl.ascx" TagName="TopControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/MenuList.ascx" TagName="Sys_menulist" TagPrefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="UTF-8" />
    <title><%=pageTitle %></title>
    <script src="../scripts/html5.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet" />
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>
<body>
    <form id="form1" runat="server">
        <uc1:TopControl ID="TopControl1" runat="server" />
        <uc2:Sys_menulist ID="MenuList1" runat="server" />
        <section id="main">
                    <%=NavigationName %>
            <div class="titlePanel_01">
                <div class="btns">
                    <input class="btn btn5" value="数据对比" type="button" onclick="statisticsContrast.open(this, [<%=strDefwx%>])"><!---注意 调用方法 第二个参数是默认选中的那一项的ID！！！如果不要默认选中 这项留空--->
                </div>
                <h1><%=strTitles %></h1>
            </div>


            <div class="statisticsListPanel_01">
                <div class="listField">
                    <div class="title">
                        <h2>关注用户总数</h2>
                    </div>
                    <div class="statistics">
                        <div class="pie" id="containerPie">
                         
                        </div>
                       <div class="info" style="padding-left:50px">
					<table cellpadding="0" cellspacing="0" class="table2">
                       
						<tr>
							<th class="name">微信公众账号</th>
							<th>人数(共:<%=allCount %>人)</th>
						</tr>
						 <asp:Literal ID="lit_table" runat="server"></asp:Literal>
					</table>
				</div>
                    </div>
                </div>
            </div>
            <div class="statisticsListPanel_01">
                <div class="listField">
                    <div class="title">
                        <div class="filter">
                            统计时间： <input type="text" class="txt date" runat="server" id="txt_date_show" placeholder="按时间筛选" onfocus="selectStartDate();">
                             <%--这两个文本框是必须要的 ,一个是开始日期，一个是结束日期--%>
                    <asp:TextBox ID="txtbegin_date" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;"></asp:TextBox>
                    <asp:TextBox ID="txtend_date" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;" onfocus="selectEndDate();"></asp:TextBox>
                        </div>
                        <h2>关注人数</h2>
                    </div>
                    <div class="statistics">
                        <h3>趋势图 - 关注人数</h3>
                        <div class="content" id="containerAuto">
                            
                        </div>
                    </div>
                    <div class="title">
                        <div class="filter">
                            统计时间：<input type="text" class="txt date" runat="server" id="txt_undate_show" placeholder="按时间筛选" onfocus="selectUnStartDate();">
                             <%--这两个文本框是必须要的 ,一个是开始日期，一个是结束日期--%>
                    <asp:TextBox ID="txtunbegin_date" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;"></asp:TextBox>
                    <asp:TextBox ID="txtunend_date" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;" onfocus="selectUnEndDate();"></asp:TextBox>
                        </div>
                        <h2>取消关注人数</h2>
                    </div>
                    <div class="statistics">
                        <h3>趋势图 - 取消关注人数</h3>
                        <div class="content" id="uncontainerAuto">
                         
                        </div>
                    </div>
                </div>
            </div>
            <div class="statisticsListPanel_01">
                <div class="listField">
                    <div class="title">
                        <div class="filter">
                            统计时间：<input type="text" class="txt date" runat="server" id="txt_Msgdate_show" placeholder="按时间筛选" onfocus="selectMsgStartDate();">
                             <%--这两个文本框是必须要的 ,一个是开始日期，一个是结束日期--%>
                    <asp:TextBox ID="txtMsgbegin_date" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;"></asp:TextBox>
                    <asp:TextBox ID="txtMsgend_date" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;" onfocus="selectMsgEndDate();"></asp:TextBox>
                        </div>
                        <h2>发送消息数据</h2>
                    </div>
                    <div class="statistics">
                        <h3>趋势图 - 发送消息数据</h3>
                        <div class="content" id="MsgcontainerAuto">
                           
                        </div>
                    </div>
                    <div class="title">
                        <div class="filter">
                            统计时间：<input type="text" class="txt date" runat="server" id="txt_Recdate_show" placeholder="按时间筛选" onfocus="selectRecStartDate();">
                             <%--这两个文本框是必须要的 ,一个是开始日期，一个是结束日期--%>
                    <asp:TextBox ID="txtRecbegin_date" runat="server" Style="width: 1px; border: none; background-color: transparent ; color: transparent;"></asp:TextBox>
                    <asp:TextBox ID="txtRecend_date" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;" onfocus="selectRecEndDate();"></asp:TextBox>
                        </div>
                        <h2>接收消息数据</h2>
                    </div>
                    <div class="statistics">
                        <h3>趋势图 - 接收消息数据</h3>
                        <div class="content" id="ReccontainerAuto">
                           
                        </div>
                    </div>
                </div>
            </div>
            <div class="statisticsListPanel_01" style="display:none">
                <div class="listField">
                    <div class="title">
                        <div class="filter">
                            统计时间：<input class="txt date" type="text" value="2014/10/11 - 2014/11/11">
                            <input type="button" class="btn btn5" value="选择">
                        </div>
                        <h2>栏目访问</h2>
                    </div>
                    <div class="statistics">
                        <div class="content">
                            <%--<img src="demo/demo_list_02.jpg" width="659" height="375" alt="demo">--%>
                        </div>
                    </div>
                </div>
            </div>

        </section>


        <script src="../scripts/jquery-1.10.2.min.js"></script>
        <script src="../scripts/controls.js"></script>
        <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
        <script src="../scripts/Bombbox.js"></script>
        <script src="../scripts/statisticsContrast.js"></script>
        <!--弹出框JS 调用方法：1.开启弹出框：bombbox.openBox('链接地址，可以带参')，2.关闭弹出框：bombbox.closeBox();注意：此方法无需在弹出框里面的页面引用-->
        <script src="../scripts/highchart/js/highcharts.js"></script> <!--统计所需的JS-->
        <script src="../scripts/highchart/js/modules/exporting.js"></script><!--统计所需的JS-->
        <script src="../Scripts/DatePicker/WdatePicker.js"></script>

        <script>

            //---------------选择对比弹出框开始--------------------------------------
            statisticsContrast.readAccount(<%=strWxlist%>);//这个数组是所有的标签名字
            statisticsContrast.accountSubmit = function (submitValue) {
                var ids = '';
                var names = '';
                for (var i in submitValue) {
                    ids += submitValue[i].id + ',';
                    //names += submitValue[i].name + ',';
                }
                if (ids == '') {
                    alert('至少选中一项');

                }
                else
                {
                    var url = 'fans_statistics.aspx?Ids=' + encodeURI(ids)+'&m_id=<%=m_id%>';
                     location.href = url;
                 }
                //alert('选中的是：' + names + ' 他们的ID分别是：' + ids);
            }
            //---------------选择对比弹出框结束--------------------------------------


            //---------------加载图形报表开始--------------------------------------
            $(function () {
                //总关注人数
                $('#containerPie').highcharts({
                    chart: {
                        plotBackgroundColor: null,    <%--背景色--%>
                        plotBorderWidth: null,        <%--宽度--%>
                        plotShadow: false             <%--阴影--%>
                    },
                    title: {
                        text: ''           <%--标题--%>
                    },
                    tooltip: {
                        pointFormat: '<b>{point.percentage:.1f}%</b>'
                        //formatter: function () {
                        //pointFormat:'{series.name} 关注人数：<b>{Highcharts.numberFormat(this.y, 0, ",")}</b>(人)'; <%--鼠标覆盖时显示的内容--%>
                        //}
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
                            }
                        }
                    },
                    series: [{
                        type: 'pie',
                        name: '关注人数',
                        data: [<%= pieChartData%>]
                    }],
                    credits: {
                        enabled:false         <%--禁用右下角的水印--%>
                    }
                });
            });


            //按日期关注人数
            $('#containerAuto').highcharts({
                chart: {
                    type: 'line', <%--图表类型：区域图（阴影图）--%>
                    zoomType: 'x'
                },
                title: {
                    text: ''                <%--图标标题--%>
                },
                subtitle: {
                    text: '',    <%--图表副标题--%>
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
                    enabled:false
                }
            });
            //按日期取消关注人数
            $('#uncontainerAuto').highcharts({
                chart: {
                    type: 'line', <%--图表类型：区域图（阴影图）--%>
                    zoomType: 'x'
                },
                title: {
                    text: ''                <%--图标标题--%>
                },
                subtitle: {
                    text: '',    <%--图表副标题--%>
                    floating: true                         <%--设置浮动，标题漂浮于图表上，有效利用空间--%>
                },
                xAxis: {
                    categories: [<%= unchartDateRange2 %>],  <%--图表的x轴，现在指定为一周日期--%>
                    tickInterval: <%= unchartXInterval2%>    <%--X轴显示间隔--%>
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
                series: [<%=unJsonData%>],
                credits: {
                    enabled:false
                }
            });
            //按日期统计发送消息数据
            $('#MsgcontainerAuto').highcharts({
                chart: {
                    type: 'line', <%--图表类型：区域图（阴影图）--%>
                    zoomType: 'x'
                },
                title: {
                    text: ''                <%--图标标题--%>
                },
                subtitle: {
                    text: '',    <%--图表副标题--%>
                    floating: true                         <%--设置浮动，标题漂浮于图表上，有效利用空间--%>
                },
                xAxis: {
                    categories: [<%= MsgchartDateRange2 %>],  <%--图表的x轴，现在指定为一周日期--%>
                    tickInterval: <%= MsgchartXInterval2%>    <%--X轴显示间隔--%>
                    },
                yAxis: {
                    title: {
                        text: '条数（条）'                <%--图表的Y轴名--%>
                    },
                    plotLines: [{                          <%--Y轴提示线，可指定样式或隐藏--%>
                        value: 0,
                        width: 1,
                        color: '#e8e8e8'
                    }]
                },
                tooltip: {
                    valueSuffix: '条',                      <%--鼠标悬浮在点上时，提示的单位--%>
                    crosshairs: true,                       <%--设置穿透线--%>
                    shared: true                            <%--在tooltip里显示两条线的值--%>
                },
                legend: {
                    enable: false
                },
                series: [<%=MsgJsonData%>],
                credits: {
                    enabled:false
                }
            });
            //按日期统计接收消息数据
            $('#ReccontainerAuto').highcharts({
                chart: {
                    type: 'line', <%--图表类型：区域图（阴影图）--%>
                    zoomType: 'x'
                },
                title: {
                    text: ''                <%--图标标题--%>
                },
                subtitle: {
                    text: '',    <%--图表副标题--%>
                    floating: true                         <%--设置浮动，标题漂浮于图表上，有效利用空间--%>
                },
                xAxis: {
                    categories: [<%= RecchartDateRange2 %>],  <%--图表的x轴，现在指定为一周日期--%>
                    tickInterval: <%= RecchartXInterval2%>    <%--X轴显示间隔--%>
                    },
                yAxis: {
                    title: {
                        text: '条数（条）'                <%--图表的Y轴名--%>
                    },
                    plotLines: [{                          <%--Y轴提示线，可指定样式或隐藏--%>
                        value: 0,
                        width: 1,
                        color: '#e8e8e8'
                    }]
                },
                tooltip: {
                    valueSuffix: '条',                      <%--鼠标悬浮在点上时，提示的单位--%>
                    crosshairs: true,                       <%--设置穿透线--%>
                    shared: true                            <%--在tooltip里显示两条线的值--%>
                },
                legend: {
                    enable: false
                },
                series: [<%=RecJsonData%>],
                credits: {
                    enabled:false
                }
            });
            //---------------加载图形报表结束--------------------------------------

            //--------------------------日期控件开始-------------------------------
            //1.关注日历控件
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
                    maxDate: '%y-%M-%d}',
                    onpicked: function (dp) {
                        if (txt_date_show.value.length > 0) {
                            txt_date_show.value += " — " + dp.cal.getNewDateStr();
                            selectSubDate();
                        } else {
                            txt_date_show.value = dp.cal.getNewDateStr();
                        }

                    }
                });
            }
            //2.取消关注日历控件
            function selectUnStartDate() {
                var txtbegin_date = $dp.$('txtunbegin_date');
                var txtend_date = $dp.$('txtunend_date');
                var txt_date_show = $dp.$('txt_undate_show');

                WdatePicker(
                {
                    position: { left: -198, top: 10 },
                    el: 'txtunbegin_date',
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

            function selectUnEndDate() {
                var txt_date_show = $dp.$('txt_undate_show');
                WdatePicker({
                    position: { left: -120, top: 10 },
                    doubleCalendar: true,
                    isShowClear: true,
                    readOnly: true,
                    dateFmt: 'yyyy-MM-dd',
                    minDate: '#F{$dp.$D(\'txtunbegin_date\',{d:0});}',
                    maxDate: '%y-%M-%d}',
                    onpicked: function (dp) {
                        if (txt_date_show.value.length > 0) {
                            txt_date_show.value += " — " + dp.cal.getNewDateStr();
                            selectUnDate();
                        } else {
                            txt_date_show.value = dp.cal.getNewDateStr();
                        }

                    }
                });
            }
            //3.发送消息日历控件
            function selectMsgStartDate() {
                var txtbegin_date = $dp.$('txtMsgbegin_date');
                var txtend_date = $dp.$('txtMsgend_date');
                var txt_date_show = $dp.$('txt_Msgdate_show');

                WdatePicker(
                {
                    position: { left: -198, top: 10 },
                    el: 'txtMsgbegin_date',
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

            function selectMsgEndDate() {
                var txt_date_show = $dp.$('txt_Msgdate_show');
                WdatePicker({
                    position: { left: -120, top: 10 },
                    doubleCalendar: true,
                    isShowClear: true,
                    readOnly: true,
                    dateFmt: 'yyyy-MM-dd',
                    minDate: '#F{$dp.$D(\'txtMsgbegin_date\',{d:0});}',
                    maxDate: '%y-%M-%d}',
                    onpicked: function (dp) {
                        if (txt_date_show.value.length > 0) {
                            txt_date_show.value += " — " + dp.cal.getNewDateStr();
                            selectMsgDate();
                        } else {
                            txt_date_show.value = dp.cal.getNewDateStr();
                        }

                    }
                });
            }
            //4.接收消息日历控件
            function selectRecStartDate() {
                var txtbegin_date = $dp.$('txtRecbegin_date');
                var txtend_date = $dp.$('txtRecend_date');
                var txt_date_show = $dp.$('txt_Recdate_show');

                WdatePicker(
                {
                    position: { left: -198, top: 10 },
                    el: 'txtRecbegin_date',
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

            function selectRecEndDate() {
                var txt_date_show = $dp.$('txt_Recdate_show');
                WdatePicker({
                    position: { left: -120, top: 10 },
                    doubleCalendar: true,
                    isShowClear: true,
                    readOnly: true,
                    dateFmt: 'yyyy-MM-dd',
                    minDate: '#F{$dp.$D(\'txtRecbegin_date\',{d:0});}',
                    maxDate: '%y-%M-%d}',
                    onpicked: function (dp) {
                        if (txt_date_show.value.length > 0) {
                            txt_date_show.value += " — " + dp.cal.getNewDateStr();
                            selectRecDate();
                        } else {
                            txt_date_show.value = dp.cal.getNewDateStr();
                        }

                    }
                });
            }
            //--------------------------日期控件结束-------------------------------

            //--------------------------日期选择事件开始---------------------------
            //1.关注
            function selectSubDate() //关注选择
            {
                var Ids=GetQueryString("Ids");
                var sub_sdate=$("#txtbegin_date").val();
                var sub_edate=$("#txtend_date").val();

                location.replace("fans_statistics.aspx?Ids="+Ids+"&sub_sdate="+sub_sdate+"&sub_edate="+sub_edate+"&m_id=<%=m_id%>");
            }
            //2.取消关注
            function selectUnDate() //关注选择
            {
                var Ids=GetQueryString("Ids");
                var un_sdate=$("#txtunbegin_date").val();
                var un_edate=$("#txtunend_date").val();

                location.href="fans_statistics.aspx?Ids="+Ids+"&un_sdate="+un_sdate+"&un_edate="+un_edate+"&m_id=<%=m_id%>";
            }
            //3.发送消息
            function selectMsgDate() //关注选择
            {
                var Ids=GetQueryString("Ids");
                var Msg_sdate=$("#txtMsgbegin_date").val();
                var Msg_edate=$("#txtMsgend_date").val();

                location.href="fans_statistics.aspx?Ids="+Ids+"&Msg_sdate="+Msg_sdate+"&Msg_edate="+Msg_edate+"&m_id=<%=m_id%>";
            }
            //4.接收消息
            function selectRecDate() //关注选择
            {
                var Ids=GetQueryString("Ids");
                var Rec_sdate=$("#txtRecbegin_date").val();
                var Rec_edate=$("#txtRecend_date").val();

                location.href="fans_statistics.aspx?Ids="+Ids+"&Rec_sdate="+Rec_sdate+"&Rec_edate="+Rec_edate+"&m_id=<%=m_id%>";
            }
            //--------------------------日期选择事件结束---------------------------

            //获取页面参数
            function GetQueryString(name) {

                var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");

                var r = window.location.search.substr(1).match(reg);

                if (r != null) return (r[2]); return null;

            }
            //--------------------------加载日期------------------------------------

            $(".text").css({ width: 80 });

            nav.change('<%=m_id%>'); 
</script>
    </form>
</body>
</html>
