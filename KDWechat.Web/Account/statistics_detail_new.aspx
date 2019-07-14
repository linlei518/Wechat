<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="statistics_detail_new.aspx.cs" Inherits="KDWechat.Web.Account.statistics_detail_new" %>

<%@ Register TagName="TopControl" Src="~/UserControl/TopControl.ascx" TagPrefix="uc" %>

<%@ Register Src="~/UserControl/MenuList.ascx" TagName="Sys_menulist" TagPrefix="uc2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <title><%=pageTitle %></title>
    <script src="../scripts/html5.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet" />
 <script src="../Scripts/DatePicker/WdatePicker.js"></script>
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>
<body>
    <uc:TopControl runat="server" />
    <uc2:Sys_menulist ID="MenuList1" runat="server" />
    <form id="form1" runat="server">
        <section id="main">
            <%=NavigationName %>
            <div class="titlePanel_01">
                <div class="btns">
                    <input class="btn btn5" value="数据对比" type="button" onclick="statisticsContrast.open(this,'1')"><!---注意 调用方法 第二个参数是默认选中的那一项的ID！！！如果不要默认选中 这项留空--->
                </div>
                <%--		        <h1><%=tagName %>统计</h1>--%>
            </div>


            <div class="statisticsListPanel_01">
                <div class="listField">
                <div class="statistics">
                    <div class="title">
                        <h2><%=tagName %>总数：<b><%=count1 %></b><%=unit %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%=tagName2 %>总数：<b><%=count2 %></b><%=unit %></h2>
                    </div>
                    
                    </div>
                </div>
            </div>
            <div class="statisticsListPanel_01">
                <div class="listField">
                    <div class="title">
                        <div class="filter">
                            统计时间：<input type="text" class="txt date" runat="server" id="txt_date_show" placeholder="按关注时间筛选" onfocus="selectStartDate();" />
                            <%--这两个文本框是必须要的 ,一个是开始日期，一个是结束日期--%>
                            <asp:TextBox ID="txtbegin_date" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;"></asp:TextBox>
                            <asp:TextBox ID="txtend_date" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;" onfocus="selectEndDate();"></asp:TextBox>
                            <asp:Button ID="btnRegDate" class="btn btn5" runat="server" Text="确定" OnClick="Button1_Click" />
                        </div>
                        <h2><%=tagName %>统计</h2>
                    </div>
                    <div class="statistics">
                        <h3>趋势图 - <%=tagName %></h3>
                        <div style=" width:680px; height:400px;margin:0 auto"  id="containerTrend">
                        </div>
                    </div>
                    <div class="title">
                        <h2>用户属性</h2>
                    </div>
                    <div class="statistics">
                        <h3>性别分布</h3>
                        <div id="sexContainer" style=" width:680px; height:200px; margin:0 auto">
                        </div>
                    </div>
                    <div class="statistics">
                        <h3>语言分布</h3>
                        <div id="languageContainer" style=" width:680px; height:200px;margin:0 auto">
                        </div>
                    </div>
                    <div class="statistics">
                        <h3>地区分布</h3>
                        <div class="content" style=" width:680px; height:400px;margin:0 auto" id="container">
                        </div>
                    </div>
                    
                </div>


            </div>


            <div class="tablePanel_01" >
                <div class="title">
                    <h2>详细数据</h2>
                </div>

                <div class="tableNTab">
                    <a href="javascript:tableNTab('.tablePanel_01 .table','.tablePanel_01 .nTabBtn',0);" class="btn nTabBtn current"><%=tagName %>统计</a>
                    <a href="javascript:tableNTab('.tablePanel_01 .table','.tablePanel_01 .nTabBtn',1);" class="btn nTabBtn"><%=tagName2 %>统计</a>
                </div>
                <table cellpadding="0" cellspacing="0" class="table">
                    <thead>
                        <tr>
                            <th class="name" style="width: 120px">统计方式</th>
                            <th class="info info2">人数</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td class="name" style="width: 120px">按国家统计：</td>
                            <td class="info info2">
                                <div class="info2List">
                                    <asp:Repeater ID="repCountry" runat="server">
                                        <ItemTemplate><em title="<%#Eval("key") %>">
                                            <div style="overflow: hidden; float: left; max-width: 60px"><%#Eval("key") %></div>
                                            ：<%#Eval("count") %>人</em></ItemTemplate>
                                    </asp:Repeater>
                                </div>
                                <div class="btns">
                                    <a href="#" class="btn btnInfo"><i class="open"></i>展开</a>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="name" style="width: 120px">按省份统计：</td>
                            <td class="info info2">
                                <div class="info2List">
                                    <asp:Repeater ID="repProvince" runat="server">
                                        <ItemTemplate><em title="<%#Eval("key") %>">
                                            <div style="overflow: hidden; float: left; max-width: 60px"><%#Eval("key") %></div>
                                            ：<%#Eval("count") %>人</em></ItemTemplate>
                                    </asp:Repeater>
                                </div>
                                <div class="btns">
                                    <a href="#" class="btn btnInfo"><i class="open"></i>展开</a>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="name" style="width: 120px">按城市统计：</td>
                            <td class="info info2">
                                <div class="info2List">
                                    <asp:Repeater ID="repCity" runat="server">
                                        <ItemTemplate><em title="<%#Eval("key") %>">
                                            <div style="overflow: hidden; float: left; max-width: 60px"><%#Eval("key") %></div>
                                            ：<%#Eval("count") %>人</em></ItemTemplate>
                                    </asp:Repeater>
                                </div>
                                <div class="btns">
                                    <a href="#" class="btn btnInfo"><i class="open"></i>展开</a>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="name" style="width: 120px">按性别：</td>
                            <td class="info info2">
                                <div class="info2List">
                                    <asp:Repeater ID="repSex" runat="server">
                                        <ItemTemplate><em title="<%#Eval("key") %>">
                                            <div style="overflow: hidden; float: left; max-width: 60px"><%#Eval("key") %></div>
                                            ：<%#Eval("count") %>人</em></ItemTemplate>
                                    </asp:Repeater>
                                </div>
                                <div class="btns">
                                    <a href="#" class="btn btnInfo"><i class="open"></i>展开</a>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="name" style="width: 120px">按语言分布：</td>
                            <td class="info info2">
                                <div class="info2List">
                                    <asp:Repeater ID="repLanguage" runat="server">
                                        <ItemTemplate><em title="<%#Eval("key") %>">
                                            <div style="float: left;"><%#Eval("key") %></div>
                                            ：<%#Eval("count") %>人</em></ItemTemplate>
                                    </asp:Repeater>
                                </div>
                                <div class="btns">
                                    <a href="#" class="btn btnInfo"><i class="open"></i>展开</a>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <table cellpadding="0" cellspacing="0" class="table hidden">
                    <thead>
                        <tr>
                            <th class="name" style="width: 120px">统计方式</th>
                            <th class="info info2">人数</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td class="name" style="width: 120px">按国家统计：</td>
                            <td class="info info2">
                                <div class="info2List">
                                    <asp:Repeater ID="repUnCountry" runat="server">
                                        <ItemTemplate><em title="<%#Eval("key") %>">
                                            <div style="overflow: hidden; float: left; max-width: 60px"><%#Eval("key") %></div>
                                            ：<%#Eval("count") %>人</em></ItemTemplate>
                                    </asp:Repeater>
                                </div>
                                <div class="btns">
                                    <a href="#" class="btn btnInfo"><i class="open"></i>展开</a>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="name" style="width: 120px">按省份统计：</td>
                            <td class="info info2">
                                <div class="info2List">
                                    <asp:Repeater ID="repUnProvince" runat="server">
                                        <ItemTemplate><em title="<%#Eval("key") %>">
                                            <div style="overflow: hidden; float: left; max-width: 60px"><%#Eval("key") %></div>
                                            ：<%#Eval("count") %>人</em></ItemTemplate>
                                    </asp:Repeater>
                                </div>
                                <div class="btns">
                                    <a href="#" class="btn btnInfo"><i class="open"></i>展开</a>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="name" style="width: 120px">按城市统计：</td>
                            <td class="info info2">
                                <div class="info2List">
                                    <asp:Repeater ID="repUnCity" runat="server">
                                        <ItemTemplate><em title="<%#Eval("key") %>">
                                            <div style="overflow: hidden; float: left; max-width: 60px"><%#Eval("key") %></div>
                                            ：<%#Eval("count") %>人</em></ItemTemplate>
                                    </asp:Repeater>
                                </div>
                                <div class="btns">
                                    <a href="#" class="btn btnInfo"><i class="open"></i>展开</a>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="name" style="width: 120px">按性别：</td>
                            <td class="info info2">
                                <div class="info2List">
                                    <asp:Repeater ID="repUnSex" runat="server">
                                        <ItemTemplate><em title="<%#Eval("key") %>">
                                            <div style="overflow: hidden; float: left; max-width: 60px"><%#Eval("key") %></div>
                                            ：<%#Eval("count") %>人</em></ItemTemplate>
                                    </asp:Repeater>
                                </div>
                                <div class="btns">
                                    <a href="#" class="btn btnInfo"><i class="open"></i>展开</a>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="name" style="width: 120px">按语言分布：</td>
                            <td class="info info2">
                                <div class="info2List">
                                    <asp:Repeater ID="repUnLanguage" runat="server">
                                        <ItemTemplate><em title="<%#Eval("key") %>">
                                            <div style="overflow: hidden; float: left; max-width: 60px"><%#Eval("key") %></div>
                                            ：<%#Eval("count") %>人</em></ItemTemplate>
                                    </asp:Repeater>
                                </div>
                                <div class="btns">
                                    <a href="#" class="btn btnInfo"><i class="open"></i>展开</a>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>

            </div>


        </section>
    </form>

    <script src="../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../scripts/controls.js"></script>
    <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
    <script src="../scripts/Bombbox.js"></script>
    <script src="../scripts/statisticsContrast.js"></script>
    <!--弹出框JS 调用方法：1.开启弹出框：bombbox.openBox('链接地址，可以带参')，2.关闭弹出框：bombbox.closeBox();注意：此方法无需在弹出框里面的页面引用-->
    <script src="../Scripts/HighChart/js/highcharts.js"></script>
  <script src="../Scripts/HighChart/js/modules/map.js"></script>
  <script src="../Scripts/HighChart/js/exporting.js"></script>
   <script src="../Scripts/HighChart/js/modules/cn-all-sar-taiwan.js"></script>


    <script>
       $("em div").each(function(){
            //$(this).html(this.html());
            if(this.textContent.length>4)
                this.textContent=this.textContent.substring(0,4)+"...";
        });

        //右上角对比
        statisticsContrast.readAccount(<%=strWxlist%>);//这个数组是所有的标签名字
        statisticsContrast.accountSubmit = function (submitValue) {
            var ids = '';
            var names = '';
            for (var i in submitValue) {
                ids += submitValue[i].id + ',';
            }
            if (ids == '') {
                alert('至少选中一项');
            }
            else
            {
                var url = 'fans_statistics.aspx?Ids=' + encodeURI(ids)+'&m_id=<%=m_id%>';
                location.href = url;
            }
        }

        $(function () {
            //highChart-start
            var data = <%=jsonData2%>;

            <%--地图--%>
            // Initiate the chart
         $('#container').highcharts('Map', {
                title: {
                    text: ''
                },
                colorAxis: {
                    min: 0
                },
                series: [{
                    data: data,
                    mapData: Highcharts.maps['countries/cn/custom/cn-all-sar-taiwan'],
                    joinBy: 'hc-key',
                    name: '<%=tagName%>人数',
                    showInLegend: false,
                    states: {
                        hover: {
                            color: '#BADA55'
                        }
                    }
                }],
                credits: {
                    enabled:false
                }
            });
            //highChart-end



            //语言
            $('#languageContainer').highcharts({
                chart: {
                    type: 'bar'
                },
                title: {
                    text: ''
                },
                xAxis: {
                    categories: ['<%=DateTime.Now.ToString("MM-dd")%>']
                },
                yAxis: {
                    title: {
                        text: '关注人数'
                    }
                },
                legend: {
                    enabled: false
                },
                plotOptions: {
                    series: {
                        stacking: 'normal'
                    }
                },
                series:<%=jsonLang%>
                    ,
                credits: {
                    enabled:false
                }
            });


            //性别筛选
           $('#sexContainer').highcharts({
                chart: {
                    type: 'bar'
                },
                title: {
                    text: ''
                },
                xAxis: {
                    categories: ['<%=DateTime.Now.ToString("MM-dd")%>']
                },
                yAxis: {

                    title: {
                        text: '关注人数'
                    }
                },
                legend: {
                    enabled: false
                },
                plotOptions: {
                    series: {
                        stacking: 'normal'
                    }
                },
                series:<%=jsonSex%>
                    ,
                credits: {
                    enabled:false
                }
            });

            $('#containerTrend').highcharts({
                chart: {
                    type: 'area',         <%--图表类型：区域图（阴影图）--%>
                    zoomType: 'x'
                },
                title: {
                    text: '关注人数统计',       <%--图表标题--%>
                    style:{"display":"none"}
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
                    data: [<%=chartSeris2%>]
                }],
                credits: {
                    enabled:false
                }
            });
            
            

           
        });
       

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
                    } else {
                        txt_date_show.value = dp.cal.getNewDateStr();
                    }

                }
            });
        }

        //田字号的展开
        function tableNTab(node,btns,num){
            node = $(node);
            btns = $(btns);
            node.addClass('hidden').eq(num).removeClass('hidden');
            btns.removeClass('current').eq(num).addClass('current');
        }

        function checkTableOpen(){
            $('.table').each(function(){
                if(this.className.indexOf('hidden')==-1){
                    $(this).find('td.info2 .btnInfo i').removeClass('close');
                    $(this).find('td.info2 .info2List').each(function(){
                        $(this).removeClass('close');
                        if(this.offsetHeight>90){
                            $(this).addClass('close');
                            $(this).next('.btns').show();
                        }
                    });
                }
            });
        }
        $('.table').find('td.info2 .btnInfo').click(function(){
            $(this).find('i').toggleClass('close');
            $(this).parent().prev('.info2List').toggleClass('close');
        });

        checkTableOpen();

    nav.change(<%=m_id%>);
    </script>


</body>
</html>
