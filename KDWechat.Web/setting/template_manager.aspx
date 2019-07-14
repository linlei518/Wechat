<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="template_manager.aspx.cs" Inherits="KDWechat.Web.setting.template_manager" %>

<%@ Register Src="../UserControl/TopControl.ascx" TagName="TopControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/MenuList.ascx" TagName="MenuList" TagPrefix="uc2" %>
<!doctype html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=pageTitle %></title>
    <script src="../scripts/html5.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet">
    <link type="text/css" href="../styles/style.css" rel="stylesheet">
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>

<body>
    <form runat="server" id="form1">
        <uc1:TopControl ID="TopControl1" runat="server" />
        <uc2:MenuList ID="MenuList1" runat="server" />
        <section id="main">

             <%=NavigationName %>
            <div class="searchPanel_01">

                <div>
                    仅用默认：
                <input id="rboStatus_0" runat="server" onclick="click_set(1)" type="radio" name="rboStatus" value="1">开启&nbsp;&nbsp;
                <input id="rboStatus_1" runat="server" onclick="click_set(0)" type="radio" name="rboStatus" value="0">关闭
                <span style="color: red;">&nbsp;&nbsp;&nbsp;说明：选择开启后，所有公众号的图文模板将会使用当前已选择的默认模板。</span>
                </div>
            </div>

           

            <div class="searchPanel_01">
                <div class="searchField">
                    <input type="text" class="txt searchTxt" placeholder="搜索模板名称..." runat="server" id="txtKey">
                    <asp:Button ID="btnSearch" runat="server" Text="" CssClass="btn searchBtn" ToolTip="点击搜索" OnClick="btnSearch_Click"></asp:Button>
                </div>
                <div class="filterField">

                    <input type="text" class="txt date" runat="server" id="txt_date_show" placeholder="按修改时间筛选" onfocus="selectStartDate();">
                    <%--这两个文本框是必须要的 ,一个是开始日期，一个是结束日期--%>
                    <asp:TextBox ID="txtbegin_date" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;"></asp:TextBox>
                    <asp:TextBox ID="txtend_date" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;" onfocus="selectEndDate();"></asp:TextBox>

                </div>
            </div>

            <div class="templateListPanel_01">
                <div class="picList">
                    <ul>
                        <asp:Repeater ID="repList" runat="server" OnItemCommand="repList_ItemCommand">
                            <ItemTemplate>
                                <li <%# Eval("is_default").ToString()=="1"?"class=\"current\"":"" %>>
                                    <div class="img">
                                        <a href='javascript:void(0);'>
                                            <img src='<%# Eval("img_url") %>' alt=""></a>
                                    </div>
                                     <div class="defaultIcon"></div>
                                    <div class="text">
                                        <h2><%# Eval("title") %></h2>
                                        <p><%# Eval("remark") %></p>
                                    </div>
                                    <div class="control">
                                        <asp:HiddenField ID="hfstatus" Value='<%# Eval("status") %>' runat="server" />
                                         <asp:HiddenField ID="hfisdefault" Value='<%# Eval("is_default") %>' runat="server" />
                                        <a href="javascript:bombbox.openBox('template_manager_select_wechat.aspx?template_id=<%# Eval("id") %>');" class="btn choose" style="text-align: center;">分配公众号</a>
                                        <asp:Button ID="lbtnStatus" CssClass="btn choose" OnClientClick="dialogue.dlLoading();" CommandArgument='<%# Eval("id") %>' Enabled='<%# Eval("is_default").ToString()=="1"?false:true %>' CommandName="is_default" runat="server" Text="设为默认模板"></asp:Button>
                                        <a href="javascript:void(0);" class="btn list" title="简介" type="button"></a>
                                    </div>
                                    <div class="info">
                                        <h2>
                                            <asp:Literal ID="lblTitle" runat="server" Text='<%# Eval("title") %>'></asp:Literal></h2>
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>

                    </ul>
                </div>
                <%-- 需要引用function.js--%>
                <div class="pageNum" id="div_page" runat="server">
                </div>
            </div>



        </section>

    </form>
    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../scripts/jquery.ba-resize.min.js"></script>
    <script src="../Scripts/function.js"></script>
    <script src="../scripts/Bombbox.js"></script>
    <!--三级联动选择地址的JS-->
    <script src="../scripts/controls.js"></script>
    <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
    <script src="../Scripts/DatePicker/WdatePicker.js"></script>
    <script type="text/javascript">
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
                    var urll = "template_manager.aspx?key=" + $("#txtKey").val() + "&beginDate=&endDate=&m_id=<%=m_id%>";
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
                        location.replace("template_manager.aspx?key=" + $("#txtKey").val() + "&beginDate=" + $("#txtbegin_date").val() + "&endDate=" + $("#txtend_date").val() + "&m_id=<%=m_id%>");
                } else {
                    txt_date_show.value = dp.cal.getNewDateStr();
                }

            }
        });
    }
    </script>
    <script>
        nav.change('<%=m_id%>');//此处填写被激活的导航项的ID


        function click_set(status) {
            $.ajax({
                type: "POST",
                url: "template_manager.aspx?action=set_default&status=" + status + "",
                timeout: 60000,
                contentType: 'text/html; charset=utf-8;',
                beforeSend: function () {
                },
                success: function (data) {
                    if (data == "0") {
                        showTip.show("操作失败", true);
                    } else {
                        if (status==1) {
                            showTip.show("已开启");
                        } else {
                            showTip.show("已关闭");
                        }
                       
                    }
                },
                error: function (data, status, e) {
                    showTip.show("暂无配置权限", true);
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
</body>
</html>

