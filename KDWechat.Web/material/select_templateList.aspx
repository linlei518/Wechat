<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="select_templateList.aspx.cs" Inherits="KDWechat.Web.material.select_templateList" %>


<!doctype html>
<html>
<head>
    <meta charset="UTF-8">
    <title>选择图文消息</title>
    <script src="../scripts/html5.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet">
    <!--[if lt IE 9 ]><link href="/styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="/styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="/styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>
<body class="bombbox">
    <header id="bombboxTitle">
        <div class="titlePanel_01">
            <h1>选择图文模板</h1>
        </div>
    </header>
    <form id="form1" runat="server">
        <section id="bombboxMain">

            <div class="searchPanel_01">
                <div class="searchField">
                    <input type="text" class="txt searchTxt" placeholder="搜索模板名称..." runat="server" id="txtKey">
                    <asp:Button ID="btnSearch" runat="server" Text="" CssClass="btn searchBtn" ToolTip="点击搜索" OnClick="btnSearch_Click"></asp:Button>
                </div>
                <div class="filterField">
                    <asp:DropDownList ID="ddlType" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" CssClass="select" runat="server">
                        <asp:ListItem Value="-2">全部模板</asp:ListItem>
                        <asp:ListItem Value="0">系统模板</asp:ListItem>
                        <asp:ListItem Value="-1">自定义模板</asp:ListItem>
                    </asp:DropDownList>
                    <input type="text" class="txt date" runat="server" id="txt_date_show" placeholder="按修改时间筛选" onfocus="selectStartDate();">
                    <%--这两个文本框是必须要的 ,一个是开始日期，一个是结束日期--%>
                    <asp:TextBox ID="txtbegin_date" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;"></asp:TextBox>
                    <asp:TextBox ID="txtend_date" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;" onfocus="selectEndDate();"></asp:TextBox>

                </div>
            </div>

            <div class="templateListPanel_01">
                <div class="picList">
                    <ul>   
                         <asp:Repeater ID="repList" runat="server" >
                            <ItemTemplate>
                        <li  <%# Eval("is_default").ToString()=="1"?"class=\"current\"":"" %>>
                            <div class="img">
                                <a href="javascript: window.parent.materialAddModule.selectTemplate({ id: '<%# Eval("template_id") %>', title: '<%# Eval("title") %>', img: '<%# Eval("img_url") %>' }); window.parent.bombbox.closeBox();">
                                    <img src="<%# Eval("img_url") %>" alt=""></a>
                            </div>
                               <div class="defaultIcon"></div>
                                    <div class="text">
                                        <h2><%# Eval("title") %></h2>
                                        <p><%# Eval("remark") %></p>
                                    </div>
                            <div class="control">
                                <input type="button" class="btn choose" value="选择" onclick="javascript:dlalert(); window.parent.materialAddModule.selectTemplate({ id: '<%# Eval("template_id") %>        ', title: '<%# KDWechat.Common.Utils.ToHtml( Eval("title").ToString()) %>        ', img: '<%# Eval("img_url") %>        ' }); window.parent.bombbox.closeBox();">
                                <a href="javascript:" class="btn list" title="简介" type="button"></a>
                            </div>
                            <div class="info">
                                <h2><%# Eval("title") %><%# Eval("cate_id").ToString()=="-1"?"":"(系统模板)" %></h2>
                            </div>
                        </li>
                           </ItemTemplate>
                        </asp:Repeater>
                        
                    </ul>
                </div>
            </div>
        </section>
        <script src="../scripts/jquery-1.10.2.min.js"></script>
        <script src="../scripts/controls.js"></script>
        <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
        <script src="../Scripts/function.js"></script>
        <script src="../Scripts/DatePicker/WdatePicker.js"></script>
        <script type="text/javascript">

            function dlalert()
            {
                if ("<%=wchatConfig.is_use_default_template%>"=="1") {
                    dialogue.dlAlert("总部启用了统一模板设置，所有图文将采用总部设置的模板，在此期间您选择的模板将无法生效。");
                }
               
            }

            function selectStartDate() {
                var txtbegin_date = $dp.$('txtbegin_date');
                var txtend_date = $dp.$('txtend_date');
                var txt_date_show = $dp.$('txt_date_show');

                WdatePicker(
                {
                    position: { left: -212, top: 10 },
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
                        var urll = "select_templateList.aspx?key=" + $("#txtKey").val() + "&beginDate=&endDate=&type=" + $("#ddlType").val() + "&m_id=<%=m_id%>";
                location.href = urll;
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
                    location.replace("select_templateList.aspx?key=" + $("#txtKey").val() + "&beginDate=" + $("#txtbegin_date").val() + "&endDate=" + $("#txtend_date").val() + "&type=" + $("#ddlType").val() + "&m_id=<%=m_id%>");
                } else {
                    txt_date_show.value = dp.cal.getNewDateStr();
                }

            }
        });
    }
        </script>
           <script>
               $('.templateListPanel_01 li .control .list').hover(function () {
                   $(this).parents('li').eq(0).find('.text').stop().animate({
                       top: 0,
                       height: 358
                   }, 500);
               }, function () {
                   $(this).parents('li').eq(0).find('.text').stop().animate({
                       top: 358,
                       height: 0
                   }, 500);
               })
</script>
    </form>
</body>
</html>
