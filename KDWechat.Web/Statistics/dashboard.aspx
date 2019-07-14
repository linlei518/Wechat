<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="KDWechat.Web.Statistics.dashboard" %>

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
    <style>
        .marLeft {
            margin-left: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <uc1:TopControl ID="TopControl1" runat="server" />
        <uc2:Sys_menulist ID="MenuList1" runat="server" />

        <section id="main">
            <%=NavigationName %>
                <div class="titlePanel_01">
                    <div class="btns" runat="server" id="div_add">
                        <input class="btn btn5" value="查看相关统计数据" type="button" onClick="statisticsContrast.open(this, [])">
                    </div>
                    <h1> <%=GetShowName() %></h1>
                </div>
            <div class="statisticsListPanel_01">
                <div class="listField">
                    <div class="title">
                        <h2>公众号绑定情况</h2>
                    </div>
                    <div class="statistics">
                        <div class="pie" id="containerPie">
                        </div>
                        <div class="info" style="padding-left: 50px">
                            <table cellpadding="0" cellspacing="0" class="table2">

                                <tr>
                                    <th class="name">微信公众账号</th>
                                    <th>数量(共:<%=allCount %>)</th>
                                    <th>操作</th>
                                </tr>
                                <asp:Literal ID="lit_table" runat="server"></asp:Literal>
                            </table>
                        </div>
                    </div>
                    <div class="statisticsListPanel_01">
                        <div class="listField">
                            <div class="title">
                                <h2><%=DateTime.Now.Date.AddDays(-1).AddMonths(-1).ToString("yyyy-MM-dd") %> 至 <%=DateTime.Now.Date.AddDays(-1).ToString("yyyy-MM-dd") %>粉丝活跃度统计<input type="button" id="cityStatisticBtn" class="btn btn6 marLeft" onclick="location.href='vitality_bydate.aspx?m_id=<%=m_id%>&Ids=<%=IDs%>'" value="查看更多时间" /><br /></h2>
                            </div>
                            <div class="statistics">
                                <div class="content" id="containerVitalityBar" style="height:<%=vitalityHeight%>px">
                                </div>

                            </div>
                        </div>
                    </div>
                    <div class="title">
                        <h2><%=DateTime.Now.Date.AddDays(-1).AddMonths(-1).ToString("yyyy-MM-dd") %> 至 <%=DateTime.Now.Date.AddDays(-1).ToString("yyyy-MM-dd") %>群发统计<input type="button" id="cityStatisticBtn" class="btn btn6 marLeft" onclick="location.href='group_message_bydate.aspx?m_id=<%=m_id%>&Ids=<%=IDs%>'" value="查看更多时间" /></h2>
                    </div>
                    <div class="statistics">

                        <div class="pie" id="containerNewsReadPercentPie" style="height:<%=newsHeight%>px" >
                        </div>
                        <div class="info" style="padding-left: 50px">
                            <table cellpadding="0" cellspacing="0" class="table2">

                                <tr>
                                    <th class="name">公众账号</th>
                                    <th>阅读比例</th>
                                    <th>单图文数量(共:<%=newsAllCount %>)</th>
                                    <th>操作</th>
                                </tr>
                                <asp:Literal ID="news_read_lit_table" runat="server"></asp:Literal>
                            </table>
                        </div>
                    </div>
                </div>
            </div>

            <div class="tablePanel_01" style="display: none">
                <table cellpadding="0" cellspacing="0" class="table">
                    <thead>
                        <tr>
                            <th class="name">公众号</th>
                            <th class="info info1" style="width: 55px">目前接入</th>
                            <th class="info info1" style="width: 80px">本周新增会员</th>
                            <th class="info info1" style="width: 80px">本周流失会员</th>
                            <th class="time" style="width: 115px">最后互动时间</th>
                            <th class="contro2" style="width: 60px; padding: 0 10px">互动详情</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="Repeater1" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td class="name">
                                        <a href="javascript:bombbox.openBox('binding_detail.aspx?id=<%#Eval("id") %>');"><%#Eval("wx_pb_name") %></a>
                                    </td>
                                    <td class="info info1"><%#GetStatus(Eval("id")) %></td>
                                    <td class="info info1"><%#GetSCount(Eval("id")) %></td>
                                    <td class="info info1"><%#GetUSCount(Eval("id")) %></td>
                                    <td class="time" style="width: 115px"><%#GetLastTime( Eval("id")) %></td>
                                    <td class="contro2" style="width: 110px; padding: 0 10px">
                                        <a class="btn btn6" href="javascript:bombbox.openBox('binding_detail.aspx?id=<%#Eval("id") %>');">查看详情</a>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                <%# Repeater1.Items.Count == 0 ? "<tr><td style=\"text-align:center;\" colspan=\"4\">暂无数据</td></tr>" : ""%>
                            </FooterTemplate>
                        </asp:Repeater>
                        <asp:HiddenField ID="hfReturlUrl" runat="server" />
                    </tbody>
                </table>
            </div>

<%--            <div class="statisticsListPanel_01">
                <div class="listField">
                    <div class="title">
                        <h2>公众号一周关键词命中统计<input type="button" class="btn btn6 marLeft" onclick="location.href='reply_bydate.aspx?m_id=<%=m_id%>'" value="查看更多时间" /></h2>
                    </div>
                    <div class="statistics">
                        <div class="content" id="containerKeywordHitBar" style="height: 1800px">
                        </div>

                    </div>
                </div>
            </div>--%>

            <div class="statisticsListPanel_01">
                <div class="listField">
                    <div class="title">
                        <h2>公众号一周回复人数统计<input type="button" id="cityStatisticBtn" class="btn btn6 marLeft" onclick="location.href='reply_bydate.aspx?m_id=<%=m_id%>&Ids=<%=IDs%>'" value="查看更多时间" /></h2>
                    </div>
                    <div class="statistics">
                        <div class="content" id="containerReplyBar" style="height: <%=otherHeght%>px">
                        </div>

                    </div>
                </div>
            </div>

            <div class="statisticsListPanel_01" id="sexDiv">
                <div class="listField">
                    <div class="title">
                        <h2>粉丝性别统计<input type="button" id="sexStatisticBtn" class="btn btn6 marLeft" onclick="javascript:toggleList();" value="查看城市统计" /></h2>
                    </div>
                    <div class="statistics">
                        <div class="content" id="containerSexBar" style="height: <%=otherHeght%>px">
                        </div>
                    </div>
                </div>
            </div>

            <div class="statisticsListPanel_01" id="cityDiv">
                <div class="listField">
                    <div class="title">
                        <h2>粉丝城市统计<input type="button" id="cityStatisticBtn" class="btn btn6 marLeft" onclick="javascript:toggleList();" value="查看性别统计" /></h2>
                    </div>
                    <div class="statistics">
                        <div class="content" id="containerCityBar" style="height: <%=otherHeght%>px">
                        </div>
                    </div>
                </div>
            </div>

            <div class="statisticsListPanel_01">
                <div class="listField">
                    <div class="title">
                        <h2>公众号一周粉丝变化统计<input type="button" id="cityStatisticBtn" class="btn btn6 marLeft" onclick="location.href='users_bydate.aspx?m_id=<%=m_id%>&Ids=<%=IDs%>'" value="查看更多时间" /></h2>
                    </div>
                    <div class="statistics">
                        <div class="content" id="containerBar" style="height: <%=otherHeght%>px">
                        </div>

                    </div>
                </div>
            </div>
          

            <!-- <div class="filterPanel_01">
                  <%-- <div class="btns" style="padding:50px">
                    <br />
                </div>--%>
                <div class="listField">
                    <div class="title">
                        <div class="filter">
                            <input class="btn btn5" value="选择公众号" type="button" onclick="statisticsContrast.open(this, []);"><%-- 注意 调用方法 第二个参数是默认选中的那一项的ID！！！如果不要默认选中 这项留空--%>
                            &nbsp;<asp:Label ID="lbl_wx" runat="server" Text=""></asp:Label>
                           &nbsp;&nbsp; 时间： <input type="text" class="txt date" runat="server" id="txt_date_show" placeholder="按时间筛选" onfocus="selectStartDate();"/>
                             <%--这两个文本框是必须要的 ,一个是开始日期，一个是结束日期--%>
                    <asp:TextBox ID="txtbegin_date" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;"></asp:TextBox>
                    <asp:TextBox ID="txtend_date" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;" onfocus="selectEndDate();"></asp:TextBox>
                            &nbsp;对比&nbsp;
                    <input type="text" class="txt date" runat="server" id="txt_date_showD" placeholder="按时间筛选" onfocus="selectStartDateD();"/>
                    <asp:TextBox ID="txtbegin_dateD" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;"></asp:TextBox>
                    <asp:TextBox ID="txtend_dateD" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;" onfocus="selectEndDateD();"></asp:TextBox>
                        </div>
                       <%-- <h2>关注人数</h2>--%>
                          <dl class="selectedList selected"><dt>已选择：</dt><dd class="btns"> <a href="javascript:RemoveTag(1)" class="btn filterCancel">全部撤销</a></dd><dd id="dou"></dd></dl>
                <dl runat="server" id="dlArea">
			        <dt>统计内容：</dt>
			        <dd>
                        <a href="javascript:getList(0)" class="btn filterSelect">粉丝人数</a>
                        <a href="javascript:getList(1)" class="btn filterSelect">新增人数</a>
                        <a href="javascript:getList(2)" class="btn filterSelect">流失人数</a>
                    
			        </dd>
		        </dl>
                    </div>
                    <div class="statistics">
                        <h3>用户数量统计</h3>
                        <div class="content" id="containerAuto" style="height:500px">
                            
                        </div>
                    </div>
                 
                </div>
            </div>-->
            <asp:HiddenField ID="hidType" runat="server" />
            <asp:HiddenField ID="hidWX" runat="server" />
            <asp:HiddenField ID="hidMid" runat="server" />
        </section>
        <script src="../scripts/jquery-1.10.2.min.js"></script>
        <script src="../scripts/controls.js"></script>
        <script src="../scripts/Bombbox.js"></script>
        <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
        <script src="../scripts/statisticsContrast.js"></script>
        <!--弹出框JS 调用方法：1.开启弹出框：bombbox.openBox('链接地址，可以带参')，2.关闭弹出框：bombbox.closeBox();注意：此方法无需在弹出框里面的页面引用-->
        <script src="../scripts/highchart/js/highcharts.js"></script>
        
        <script src="../Scripts/HighChart/js/modules/heatmap.js"></script>

        <!--统计所需的JS-->
        <script src="../scripts/highchart/js/modules/exporting.js"></script>
        <!--统计所需的JS-->
        <script src="../Scripts/DatePicker/WdatePicker.js"></script>
        <script src="../Scripts/dashboard.js"></script>
        <script src="../scripts/dashboardStatis.js"></script>
        <script>



            //---------------选择对比弹出框开始--------------------------------------
            //statisticsContrast.readAccount(<%=strWxlist%>);//这个数组是所有的标签名字
            //statisticsContrast.accountSubmit = function (submitValue) {
            //    var id = '';
            //    var names = '';

            //    id = submitValue[0].id;
            //    $("#lbl_wx").html(submitValue[0].name);

            //    if (id == '') {
            //        alert('请选择公众号');

            //    }
            //    else {
            //        $("#hidWX").val(id);
            //        //alert( $("#hidWX").val());
            //    }

            //}


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
                    var url = 'dashboard.aspx?Ids=' + encodeURI(ids)+'&m_id=<%=m_id%>';
                    location.href = url;
                }
                // alert('选中的是：' + names + ' 他们的ID分别是：' + ids);
            }

            //---------------选择对比弹出框结束--------------------------------------

            //---------------加载图形报表开始--------------------------------------
            //饼状图
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
                        name: '公众号数量',
                        data: [<%= pieChartData%>]
                    }],
                    credits: {
                        enabled: false         <%--禁用右下角的水印--%>
                    }
                });

                
                $(function () {

                    $('#containerModule').highcharts({

                        chart: {
                            type: 'heatmap',
                            marginTop: 40,
                            marginBottom: 40
                        },


                        title: {
                            text: '模块开启情况（蓝色为已开启）'
                        },

                        xAxis: {
                            categories: [<%=moduleArrayData.TrimEnd(',')%>]
                        },

                        yAxis: {
                            categories: [<%=barChartData%>],
                            title: null
                        },

                        colorAxis: {
                            min: 0,
                            minColor: '#ffffff',
                            maxColor: Highcharts.getOptions().colors[0]
                        },

                        legend: {
                            enabled:false
                        },

                        tooltip: {
                            formatter: function () {
                                var strToReturn = " 未开启 ";
                                if (this.point.value == 1)
                                {
                                    strToReturn = " 已开启 ";
                                }
                    

                                return "<b>"+ this.series.yAxis.categories[this.point.y] +
                                    strToReturn  + this.series.xAxis.categories[this.point.x]+"</b>";
                            }
                        },

                        series: [{
                            name: '模块开启数据',
                            borderWidth: 1,
                            data:[<%=moduleOpenData.TrimEnd(',')%>],
                            <%--data: [[0, 0, 1], [0, 1, 1], [0, 2, 1], [0, 3, 1], [0, 4, 1], [1, 0, 1], [1, 1, 1], [1, 2, 1], [1, 3, 1], [1, 4, 1], [2, 0, 1], [2, 1, 0], [2, 2, 0], [2, 3, 0], [2, 4, 0], [3, 0, 0], [3, 1, 0], [3, 2, 0], [3, 3, 1], [3, 4, 1], [4, 0, 1], [4, 1, 1], [4, 2, 1], [4, 3, 1], [4, 4, 1], [5, 0, 1], [5, 1, 1], [5, 2, 1], [5, 3, 1], [5, 4, 1], [6, 0, 1], [6, 1, 1], [6, 2, 1], [6, 3, 1], [6, 4, 1], [7, 0, 1], [7, 1, 1], [7, 2, 1], [7, 3, 1], [7, 4, 1], [8, 0, 1], [8, 1, 1], [8, 2, 1], [8, 3, 1], [8, 4, 0], [9, 0, 1], [9, 1, 0], [9, 2, 1], [9, 3, 1], [9, 4, 1]],--%>

                        }],
                        credits: {
                            enabled: false       
                        }

                    });
                });





                //群发数量统计
                //              $('#containerNewsPie').highcharts({
                //                  chart: {
                //                    plotBackgroundColor: null,    <%--背景色--%>
                //                  plotBorderWidth: null,        <%--宽度--%>
                //                plotShadow: false             <%--阴影--%>
                //          },
                //        title: {
                //          text: ''           <%--标题--%>
                //    },
                //  tooltip: {
                //    pointFormat: '<b>{point.percentage:.1f}%</b>'
                //                    },
                //                   plotOptions: {
                //                      pie: {
                //                         allowPointSelect: true,  <%--允许选中--%>
                //                        cursor: 'pointer',
                //                       dataLabels: {
                //                          enabled: true,
                //                         format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                //                        style: {
                //                           color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                //                      }
                //                 }
                //            }
                //       },
                //      series: [],
                //     credits: {
                //        enabled: false         <%--禁用右下角的水印--%>
                //   }
                //});


                //群发图文打开数数量统计
                $('#containerNewsReadPercentPie').highcharts({
                    chart: {
                        plotBackgroundColor: null,    <%--背景色--%>
                        plotBorderWidth: null,        <%--宽度--%>
                        plotShadow: false             <%--阴影--%>
                    },
                    xAxis: {
                        categories: [<%=groupWxXSeris.TrimEnd(',')%>],
                        title: {
                            text: null
                        }
                    },
                    yAxis: [{
                        min: 0,
                        title: {
                            text: '',
                            align: 'high'
                        },
                        labels: {
                            format: '{value} %',
                            overflow: 'justify'
                        }
                    }, { // Secondary yAxis
                        title: {
                            text: ''
                        },
                        labels: {
                            format: '{value} ',
                            overflow: 'justify'
                        },
                        opposite: true
                    }],
                    title: {
                        text: ''           <%--标题--%>
                    },
                    legend: {
                        align: 'center',
                        verticalAlign: 'top',
                        title: {
                            text: "点击色块进行筛选："
                        },
                        shadow:true
                    },
                    plotOptions: {
                        bar: {
                            dataLabels: {
                                enabled: true
                            }
                        }
                    },
                    series: [{
                        type: 'bar',
                        name: '群发图文阅读比例',
                        yAxis: 0,
                        data: [<%= newsOpenPercentChartData.TrimEnd(',')%>],
                        tooltip: {
                            valueSuffix: '%'
                        }
                    },{
                        type: 'bar',
                        name: '群发图文条数',
                        yAxis: 1,
                        data: [<%= newsPieChartData.TrimEnd(',')%>],
                        tooltip: {
                            valueSuffix: ' 条'
                        }
                    }],
                    credits: {
                        enabled: false         <%--禁用右下角的水印--%>
                    }
                });

            });
            
<%--            //关键词命中条形图
            $(function () {
                $('#containerKeywordHitBar').highcharts({
                    chart: {
                        type: 'bar'
                    },
                    title: {
                        text: ''
                    },
                    xAxis: {
                        categories: [<%=barChartData%>],
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
                        name: '命中率',
                        data: [<%=hitChartData.TrimEnd(',')%>],
                        tooltip: {
                            valueSuffix: '%'
                        }
                    }]
                });
            });--%>
            


            //人数条形图
            $(function () {
                $('#containerBar').highcharts({
                    chart: {
                        type: 'bar'
                    },
                    title: {
                        text: ''
                    },
                    xAxis: {
                        categories: [<%=barChartData%>],
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
                        align: 'center',
                        verticalAlign: 'top',
                        title: {
                            text: "点击色块进行筛选："
                        },
                        shadow:true
                    },
                    credits: {
                        enabled: false
                    },
                    series: [{
                        name: '累计粉丝人数',
                        data: [<%=userChartData%>]
                    }, {
                        name: '一周新增人数',
                        data: [<%=subChartData%>]
                    }, {
                        name: '一周流失人数',
                        data: [<%=unsubChartData%>]
                    }, {
                        name: '粉丝净增长',
                        data: [<%=userIncreaseData%>]
                    }]
                });
            });
            $(function () {
                $('#containerVitalityBar').highcharts({
                    chart: {
                        type: 'bar'
                    },
                    title: {
                        text: ''
                    },
                    xAxis: {
                        categories: [<%=barChartData%>],
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
                        align: 'center',
                        verticalAlign: 'top',
                        title: {
                            text: "点击色块进行筛选："
                        },
                        shadow:true
                    },
                    credits: {
                        enabled: false
                    },
                    series: [{
                        name: '活跃度',
                        data: [<%=vitalityChartData.TrimEnd(',')%>]
                    }]
                });
            });

            //回复人数条形图
            $(function () {
                $('#containerReplyBar').highcharts({
                    chart: {
                        type: 'bar'
                    },
                    title: {
                        text: ''
                    },
                    xAxis: {
                        categories: [<%=barChartData%>],
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
                        align: 'center',
                        verticalAlign: 'top',
                        title: {
                            text: "点击色块进行筛选："
                        },
                        shadow:true
                    },
                    credits: {
                        enabled: false
                    },
                    series: [{
                        name: '回复人数',
                        data: [<%=replyMsgData.TrimEnd(',')%>]
                    }, {
                        name: '收到的留言人数',
                        data: [<%=reciveMsgData.TrimEnd(',')%>]
                    }]
                });
            });
            
            //粉丝性别总计
            $(function () {
                $('#containerSexBar').highcharts({
                    chart: {
                        type: 'bar'
                    },
                    title: {
                        text: ''
                    },
                    xAxis: {
                        categories: [<%=barChartData%>],
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
                        series: {
                            stacking: 'normal'
                        }
                    },
                    legend: {
                        align: 'center',
                        verticalAlign: 'top',
                        title: {
                            text: "点击色块进行筛选："
                        },
                        shadow:true
                    },
                    credits: {
                        enabled: false
                    },
                    series: [{
                        name: '男性人数',
                        data: [<%=maleData.TrimEnd(',')%>]
                    }, {
                        name: '女性人数',
                        data: [<%=famaleData.TrimEnd(',')%>]
                    }, {
                        name: '未知人数',
                        data: [<%=sexUnknowData.TrimEnd(',')%>]
                    }]
                });
            });
            //粉丝城市总计
            $(function () {
                $('#containerCityBar').highcharts({
                    chart: {
                        type: 'bar'
                    },
                    title: {
                        text: ''
                    },
                    xAxis: {
                        categories:  [<%=barChartData%>]
                    },
                    yAxis: {

                        title: {
                            text: ''
                        }
                    },
                    legend: {
                        align: 'center',
                        verticalAlign: 'top',
                        title: {
                            text: "点击色块进行筛选："
                        },
                        shadow:true
                    },
                    plotOptions: {
                        series: {
                            stacking: 'normal'
                        }
                    },
                    series:<%=userJsonCity%>
                    ,
                    credits: {
                        enabled:false
                    }
                });
            });

            //---------------加载图形报表结束--------------------------------------

            //4.日历控件
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
                            //selectDate();
                        } else {
                            txt_date_show.value = dp.cal.getNewDateStr();
                        }

                    }
                });

            }


            function selectStartDateD() {
                var txtbegin_date = $dp.$('txtbegin_dateD');
                var txtend_date = $dp.$('txtend_dateD');
                var txt_date_show = $dp.$('txt_date_showD');

                WdatePicker(
                {
                    position: { left: -198, top: 10 },
                    el: 'txtbegin_dateD',
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

            function selectEndDateD() {
                var txt_date_show = $dp.$('txt_date_showD');
                WdatePicker({
                    position: { left: -120, top: 10 },
                    doubleCalendar: true,
                    isShowClear: true,
                    readOnly: true,
                    dateFmt: 'yyyy-MM-dd',
                    minDate: '#F{$dp.$D(\'txtbegin_dateD\',{d:0});}',
                    maxDate: '%y-%M-%d}',
                    onpicked: function (dp) {
                        if (txt_date_show.value.length > 0) {
                            txt_date_show.value += " — " + dp.cal.getNewDateStr();
                            selectDate();
                        } else {
                            txt_date_show.value = dp.cal.getNewDateStr();
                        }

                    }
                });
            }



            function selectDate() //查询
            {
                var wxId = $("#hidWX").val();
                var type = $("#hidType").val();
                var sub_sdate = $("#txtbegin_date").val();
                var sub_edate = $("#txtend_date").val();
                var sub_sdateD = $("#txtbegin_dateD").val();
                var sub_edateD = $("#txtend_dateD").val();
                if (wxId != "") {

                    location.replace("dashboard.aspx?wxId=" + wxId + "&type=" + type + "&sub_sdate=" + sub_sdate + "&sub_edate=" + sub_edate + "&sub_sdateD=" + sub_sdateD + "&sub_edateD=" + sub_edateD + "&m_id=<%=m_id%>");
                }
                else {
                    alert('请选择公众号');
                }
            }

            //--------------------------日期控件结束-------------------------------

            nav.change('<%=m_id%>'); 
			
			
			function toggleList(){
				if($('#sexDiv').css('display')=='block'){
					$('#cityDiv').show();
					$('#sexDiv').hide();
				}else{
					$('#cityDiv').hide();
					$('#sexDiv').show();
				}
			}
			$(document).ready(function(){
				setTimeout(function(){toggleList();},50);
			});

        </script>
        <script src="../Scripts/HighChart/js/themes/KDTheme.js"></script>
    </form>
</body>
</html>
