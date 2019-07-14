<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="users_property.aspx.cs" Inherits="KDWechat.Web.Statistics.user_property" %>
<%@ Register TagName="TopControl" Src="~/UserControl/TopControl.ascx" TagPrefix="uc" %>

<%@ Register Src="~/UserControl/MenuList.ascx" TagName="Sys_menulist" TagPrefix="uc2" %>

<!doctype html>
<html>
<head>
<meta charset="UTF-8">
<title>关注用户属性-微信管理后台</title>
<script src="../scripts/html5.js"></script>
<link type="text/css" href="../styles/global.css" rel="stylesheet">
<!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
<!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
<!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>

<body>
    <uc:TopControl runat="server" />
    <uc2:Sys_menulist ID="MenuList1" runat="server" />
<form runat="server">
    <section id="main">
        <%=NavigationName %>
	    <div class="titlePanel_01">
		    <div class="btns">
			    <input class="btn btn5" value="数据对比" type="button" onClick="statisticsContrast.open(this,'1')"><!---注意 调用方法 第二个参数是默认选中的那一项的ID！！！如果不要默认选中 这项留空--->
		    </div>
		    <h1></h1>
	    </div>
		
	
	    <div class="statisticsListPanel_01">
			<div class="listNTab">
			    <a href="users_trend.aspx?m_id=<%=m_id %>" class="btn nTabBtn">用户增长</a>
			    <a href="users_property.aspx?m_id=<%=m_id %>" class="btn nTabBtn current">用户属性</a>
		    </div>
		    <div class="listField">
			    <div class="title">
				    <h2>性别分布</h2>
			    </div>
			    <div class="statistics">
				    <div id="sexContainer" style="height:200px;" class="content">
					    
				    </div>
			    </div>
		    </div>
		    <div class="listField">
			    <div class="title">
				    <h2>语言分布</h2>
			    </div>
			    <div class="statistics">
				    <div id="languageContainer" style="height:200px;" class="content">
				    </div>
			    </div>
		    </div>
		    <div class="listField">
			    <div class="title">
				    <h2>省份分布</h2>
			    </div>
			    <div class="statistics">
				    <div id="container" style="margin-top:30px" class="pie">
					    
				    </div>
				    <div class="info">
					    <table cellpadding="0" cellspacing="0" class="table2">
						    <tr>
							    <th class="name" style="width:100px">省份</th>
							    <th>用户数</th>
						    </tr>
                            <asp:Repeater ID="repMapProv" runat="server">
                                <ItemTemplate>
                                    <tr>
							            <td class="name"><%#Eval("key") %></td>
							            <td><%#Eval("count") %></td>
						            </tr>
                                </ItemTemplate>
                            </asp:Repeater>
					    </table>
				    </div>
			    </div>
		    </div>
	    </div>
	    <div class="statisticsListPanel_01 statisticsDetail">
	
		    <div class="listNTab">
			    <a href="javascript:tableNTab('.statisticsDetail .nTabBtn','.statisticsDetail .listField',0);" class="btn nTabBtn current">性别</a>
			    <a href="javascript:tableNTab('.statisticsDetail .nTabBtn','.statisticsDetail .listField',1);" class="btn nTabBtn">语言</a>
			    <a href="javascript:tableNTab('.statisticsDetail .nTabBtn','.statisticsDetail .listField',2);" class="btn nTabBtn">国家</a>
			    <a href="javascript:tableNTab('.statisticsDetail .nTabBtn','.statisticsDetail .listField',3);" class="btn nTabBtn">省份</a>
			    <a href="javascript:tableNTab('.statisticsDetail .nTabBtn','.statisticsDetail .listField',4);" class="btn nTabBtn">城市</a>
		    </div>
		    <div class="listField current">
			    <div class="title">
				    <h2>详细数据</h2>
			    </div>
			    <table cellpadding="0" cellspacing="0" class="table" title="showLength:10,from:0">
				    <thead>
					    <tr>
						    <th class="name">性别</th>
						    <th class="info info2 sorts"><i class="sort"></i>用户数</th>
						    <th class="info info1">占比</th>
					    </tr>
				    </thead>
				    <tbody>
					    <asp:Repeater ID="repSex" runat="server">
                            <ItemTemplate>
                                <tr>
						            <td class="name"><%#Eval("key") %></td>
						            <td class="info info1"><%#Eval("count") %></td>
						            <td class="info info1"><%#Eval("percent") %>%</td>
					            </tr>
                            </ItemTemplate>
                        </asp:Repeater>
				    </tbody>
			    </table>
		    </div>
		    <div class="listField">
			    <div class="title">
				    <h2>详细数据</h2>
			    </div>
			    <table cellpadding="0" cellspacing="0" class="table" title="showLength:10,from:0">
				    <thead>
					    <tr>
						    <th class="name">语言</th>
						    <th class="info info2 sorts"><i class="sort"></i>用户数</th>
						    <th class="info info1">占比</th>
					    </tr>
				    </thead>
				    <tbody>
					    <asp:Repeater ID="repLanguage" runat="server">
                            <ItemTemplate>
                                <tr>
						            <td class="name"><%#Eval("key") %></td>
						            <td class="info info1"><%#Eval("count") %></td>
						            <td class="info info1"><%#Eval("percent") %>%</td>
					            </tr>
                            </ItemTemplate>
                        </asp:Repeater>
				    </tbody>
			    </table>
		    </div>
		    <div class="listField">
			    <div class="title">
				    <h2>详细数据</h2>
			    </div>
			    <table cellpadding="0" cellspacing="0" class="table" title="showLength:10,from:0">
				    <thead>
					    <tr>
						    <th class="name">国家</th>
						    <th class="info info2 sorts"><i class="sort"></i>用户数</th>
						    <th class="info info1">占比</th>
					    </tr>
				    </thead>
				    <tbody>
                        <asp:Repeater ID="repCountry" runat="server">
                            <ItemTemplate>
                                <tr>
						            <td class="name"><%#Eval("key") %></td>
						            <td class="info info1"><%#Eval("count") %></td>
						            <td class="info info1"><%#Eval("percent") %>%</td>
					            </tr>
                            </ItemTemplate>
                        </asp:Repeater>
				    </tbody>
			    </table>
		    </div>
		    <div class="listField">
			    <div class="title">
				    <h2>详细数据</h2>
			    </div>
			    <table cellpadding="0" cellspacing="0" class="table" title="showLength:10,from:0">
				    <thead>
					    <tr>
						    <th class="name">省份</th>
						    <th class="info info2 sorts"><i class="sort"></i>用户数</th>
						    <th class="info info1">占比</th>
					    </tr>
				    </thead>
				    <tbody>
					    <asp:Repeater ID="repProvince" runat="server">
                            <ItemTemplate>
                                <tr>
						            <td class="name"><%#Eval("key") %></td>
						            <td class="info info1"><%#Eval("count") %></td>
						            <td class="info info1"><%#Eval("percent") %>%</td>
					            </tr>
                            </ItemTemplate>
                        </asp:Repeater>
				    </tbody>
			    </table>
		    </div>
		    <div class="listField">
			    <div class="title">
				    <h2>详细数据</h2>
			    </div>
			    <table cellpadding="0" cellspacing="0" class="table" title="showLength:10,from:0">
				    <thead>
					    <tr>
						    <th class="name">城市</th>
						    <th class="info info2 sorts"><i class="sort"></i>用户数</th>
						    <th class="info info1">占比</th>
					    </tr>
				    </thead>
				    <tbody>
					    <asp:Repeater ID="repCity" runat="server">
                            <ItemTemplate>
                                <tr>
						            <td class="name"><%#Eval("key") %></td>
						            <td class="info info1"><%#Eval("count") %></td>
						            <td class="info info1"><%#Eval("percent") %>%</td>
					            </tr>
                            </ItemTemplate>
                        </asp:Repeater>
				    </tbody>
			    </table>
		    </div>
	    </div>
	
    </section>
</form>

    <script src="../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../scripts/controls.js"></script>
    <script src="../scripts/Bombbox.js"></script>
    <script src="../scripts/statisticsContrast.js"></script>
    <script src="../Scripts/HighChart/js/highcharts.js"></script>
    <script src="../Scripts/HighChart/js/modules/map.js"></script>
    <script src="../Scripts/HighChart/js/exporting.js"></script>
    <script src="../Scripts/HighChart/js/modules/cn-all-sar-taiwan.js"></script>


<script>
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
        else {
            var url = '../Account/fans_statistics.aspx?Ids=' + encodeURI(ids) + '&m_id=<%=m_id%>';
            location.href = url;
        }
    }

    function tableNTab(node,btns,num){
	    node = $(node);
	    btns = $(btns);
	    node.removeClass('current').eq(num).addClass('current');
	    btns.removeClass('current').eq(num).addClass('current');
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
                name: '关注人数',
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
                    text: ''
                }
            },
            legend: {
                enabled: true
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
                    text: ''
                }
            },
            legend: {
                enabled: true
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

    nav.change(<%=m_id%>);


</script>
    <script src="../Scripts/HighChart/js/themes/KDTheme.js"></script>
</body>
</html>


