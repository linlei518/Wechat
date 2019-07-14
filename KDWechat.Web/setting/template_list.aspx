<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="template_list.aspx.cs" Inherits="KDWechat.Web.setting.template_list" %>



<%@ Register Src="../UserControl/TopControl.ascx" TagName="TopControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/MenuList.ascx" TagName="MenuList" TagPrefix="uc2" %>
<!doctype html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=pageTitle %></title>
    <script src="../scripts/html5.js"></script>
    <link type="text/css" href="../styles/global.css" rel="stylesheet">
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->
</head>

<body>
    <form id="form1" runat="server">
        <uc1:TopControl ID="TopControl1" runat="server" />
        <uc2:MenuList ID="MenuList1" runat="server" />


        <section id="main">
            <%=NavigationName %>
            <div class="titlePanel_01">
                <div class="btns">
                     <%=isAdd==true?"<a href=\"template_add.aspx?m_id="+m_id+"\" class=\"btn btn3\"><i class=\"add\"></i>新建图文模板</a>":"" %>
                </div>
                <h1>模板列表</h1>
            </div>

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
                    <input type="text" class="txt date" runat="server" id="txt_date_show" placeholder="按时间筛选" onfocus="selectStartDate();">
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
                                        <a href='<%# Eval("cate_id").ToString()=="0"?"javascript:void(0);":"template_add.aspx?id="+Eval("template_id")+"&m_id="+m_id+"" %>'>
                                            <img src='<%# Eval("img_url") %>' alt=""></a>
                                          <asp:HiddenField ID="hf_img" Value='<%#Eval("img_url")%>' runat="server" />
                                    </div>
                                    <div class="defaultIcon"></div>
                                    <div class="text">
                                        <h2><%# Eval("title") %></h2>
                                        <p><%# Eval("remark") %></p>
                                    </div>
                                    <div class="control">
                                        <asp:Button ID="lbtnStatus" CssClass="btn choose" OnClientClick="dialogue.dlLoading();" CommandArgument='<%# Eval("template_id") %>' Enabled='<%# Eval("is_default").ToString()=="1"?false:true %>' CommandName="is_default" runat="server" Text="设为默认模板"></asp:Button>
                                        <a href="javascript:void(0);" class="btn list" title="简介" type="button"></a>

                                        <asp:LinkButton ID="lbtnEdit" CssClass="btn edit" CommandArgument='<%# Eval("template_id") %>' Visible='<%# Eval("cate_id").ToString()=="-1"?isEdit:false %>' CommandName="edit" runat="server" ToolTip='编辑'></asp:LinkButton>

                                        <asp:LinkButton ID="lbtnDelete" CssClass="btn delete" CommandArgument='<%# Eval("template_id") %>' Visible='<%# Eval("cate_id").ToString()=="-1"?isDelete:false %>' CommandName="del" OnClientClick="return confirm('您确认要删除吗?');" runat="server" ToolTip='删除'></asp:LinkButton>
                                    </div>
                                    <div class="info">
                                        <h2>
                                            <asp:HiddenField ID="hidid" Value='<%#Eval("id")%>' runat="server" />
                                            <asp:Literal ID="lblTitle" runat="server" Text='<%# Eval("title") %>'></asp:Literal><%# Eval("cate_id").ToString()=="-1"?"":"(系统模板)" %></h2>
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
        <script src="../Scripts/jquery-1.10.2.min.js"></script>
        <script src="../scripts/jquery.ba-resize.min.js"></script>
        <script src="../Scripts/function.js"></script>
        <script src="../scripts/controls.js"></script>
        <script>nav.change('<%=m_id%>');</script>
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
                        var urll = "template_list.aspx?key=" + $("#txtKey").val() + "&beginDate=&endDate=&type=" + $("#ddlType").val() + "&m_id=<%=m_id%>";
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
                    location.replace("template_list.aspx?key=" + $("#txtKey").val() + "&beginDate=" + $("#txtbegin_date").val() + "&endDate=" + $("#txtend_date").val() + "&type=" + $("#ddlType").val() + "&m_id=<%=m_id%>");
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

            var al = "<%=KDWechat.Common.RequestHelper.GetQueryInt("al",0)%>";
            if (al=="1") {
                if ("<%=wchatConfig.is_use_default_template%>" == "1") {
                    dialogue.dlAlert("总部启用了统一模板设置，所有图文将采用总部设置的模板，在此期间您选择的模板将无法生效。");
                }

            }
</script>
    </form>
</body>
</html>

