<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="fans_filter.aspx.cs" Inherits="KDWechat.Web.fans.fans_filter" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../Scripts/html5.js"></script>
    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/ajaxfileupload.js"></script>
    <script src="../Scripts/jquery.form.js"></script>
    <script src="../Scripts/jquery.validate/jquery.validate.js"></script>
    <script src="../Scripts/jquery.validate/jquery.metadata.js"></script>
    <script src="../Scripts/jquery.validate/messages_cn.js"></script>
    <script src="../scripts/selectAddress.js"></script>
    <script lang="javascript" type="text/javascript" src="../Scripts/DatePicker/WdatePicker.js"></script>

    <!--三级联动选择地址的JS-->
    <script src="../scripts/controls.js"></script>
    <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
    <link type="text/css" href="../styles/global.css" rel="stylesheet" />

</head>
<body class="bombbox">
    <form id="form1" runat="server">
            <header id="bombboxTitle">
                <div class="titlePanel_01">
                    <h1>选择高级筛选条件</h1>
                </div>
            </header>
            <section id="bombboxMain">
            <div class="filterShowPanel_01">
		        <div class="prev"></div>
		        <dl class="selectedList">
			        <dt>已选条件：</dt>
			        <dd  id="ddShow">
                        <%=filterString %>
			        </dd>
		        </dl>
	        </div>

                <div class="filterPanel_01">
                    <div class="listNTab">
			            <a href="javascript:changeList('.filterPanel_01 .listNTab .nTabBtn','.filterPanel_01 .child',0)" class="btn nTabBtn current">微信资料筛选</a>
			            <a href="javascript:changeList('.filterPanel_01 .listNTab .nTabBtn','.filterPanel_01 .child',1)" class="btn nTabBtn">真实资料筛选</a>
		            </div>
                    <div class="child current">
                    <dl>
                        <dt>分组：</dt>
                        <dd>
                            <asp:Literal ID="litGroup" runat="server"></asp:Literal>
                        </dd>
                    </dl>
                    <dl>
                        <dt>标签：</dt>
                        <dd>
                            <asp:Literal ID="litTag" runat="server"></asp:Literal>
                        </dd>
                    </dl>
                    <dl>
                        <dt>微信国籍：</dt>
                        <dd>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="WeChatCountry" pro="中国" >中国</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="WeChatCountry" pro="中国香港" >中国香港</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="WeChatCountry" pro="中国澳门" >中国澳门</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="WeChatCountry" pro="中国台湾" >中国台湾</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="WeChatCountry" pro="新加坡" >新加坡</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="WeChatCountry" pro="0" >其他</a>
                        </dd>
                    </dl>
                    <dl>
                        <dt>微信地区：</dt>
                        <dd>
                            <select class="select" name="WeChatProvince" id="WeChatProvince">
				            </select><select class="select" name="WeChatCity" id="WeChatCity">
				            </select><select class="select" name="WeChatArea" id="WeChatArea">
				            </select>
                            <input type="button" class="btn btn6 areaBtn" sh="WeChatArea" value="添加筛选" />
                        </dd>
                    </dl>

                    <dl>
                        <dt>微信性别：</dt>
                        <dd>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="WeChatSex" pro="0" >未知</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="WeChatSex" pro="1" >男</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="WeChatSex" pro="2" >女</a>
                        </dd>
                    </dl>
                    <dl>
                        <dt>关注时间：</dt>
                        <dd>
                            <input type="text" class="txt date" runat="server" id="txtSuscribeTime" placeholder="按关注时间筛选" onfocus="selectStartDate('txtSuscribeTime','txtSusStart','txtSusEnd');" />
                            <asp:TextBox ID="txtSusStart" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;"></asp:TextBox>
                            <asp:TextBox ID="txtSusEnd" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;" onfocus="selectEndDate('txtSusStart','txtSuscribeTime');"></asp:TextBox>
                            <input type="button" class="btn btn6 timeBtn" pro="txtSuscribeTime" sh="SuscribeTime" value="添加筛选" />
                        </dd>
                    </dl>
                    <dl>
                        <dt>关注来源：</dt>
                        <dd>
                            <asp:Literal ID="litFrom" runat="server"></asp:Literal>
                        </dd>
                    </dl>

                    </div>
                    <div class="child">
                    <dl>
                        <dt>性别：</dt>
                        <dd>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Sex" pro="0" >未知</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Sex" pro="1" >男</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Sex" pro="2" >女</a>
                        </dd>
                    </dl>
                    <dl>
                        <dt>信息完整度：</dt>
                        <dd>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="MsgContain" pro="0" >手机</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="MsgContain" pro="1" >姓名</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="MsgContain" pro="2" >身份证</a>
                        </dd>
                    </dl>
                    <dl>
                        <dt>证件类型：</dt>
                        <dd>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="CardType" pro="身份证">身份证</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="CardType" pro="港澳通行证">港澳通行证</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="CardType" pro="护照">护照</a>
                        </dd>
                    </dl>
                    <dl>
                        <dt>用户地区：</dt>
                        <dd>
                            <select class="select" name="Province" id="Province">
				            </select><select class="select" name="City" id="City">
				            </select><select class="select" name="Area" id="Area">
				            </select>
                            <input type="button" class="btn btn6 areaBtn" sh="Area" value="添加筛选" />
                        </dd>
                    </dl>
                    <dl>
                        <dt>生日：</dt>
                        <dd>
                            <input type="text" class="txt date" runat="server" id="txtBirth" placeholder="按关注时间筛选" onfocus="selectStartDate2('txtBirth','txtbirStart','txtbirEnd');" />
                            <asp:TextBox ID="txtbirStart" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;"></asp:TextBox>
                            <asp:TextBox ID="txtbirEnd" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;" onfocus="selectEndDate2('txtbirStart','txtBirth');"></asp:TextBox>
                            <input type="button" class="btn btn6 timeBtn" pro="txtBirth" sh="Birth" value="添加筛选" />
                        </dd>
                    </dl>
                    <dl>
                        <dt>出生日期：</dt>
                        <dd>
                            <input type="text" class="txt date" runat="server" id="txtBirthDay" placeholder="按关注时间筛选" onfocus="selectStartDate('txtBirthDay','txtBirthStart','txtBirthEnd');" />
                            <asp:TextBox ID="txtBirthStart" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;"></asp:TextBox>
                            <asp:TextBox ID="txtBirthEnd" runat="server" Style="width: 1px; border: none; background-color: transparent; color: transparent;" onfocus="selectEndDate('txtBirthStart','txtBirthDay');"></asp:TextBox>
                            <input type="button" class="btn btn6 timeBtn" pro="txtBirthDay" sh="BirthDay" value="添加筛选" />
                        </dd>
                    </dl>
                    <dl>
                        <dt>收入：</dt>
                        <dd>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Income" pro="未知">未知</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Income" pro=">2000">&lt;2000</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Income" pro="2001-3000">2001-3000</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Income" pro="3001-5000">3001-5000</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Income" pro="5001-7000">5001-7000</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Income" pro="7001-10000">7001-10000</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Income" pro=">10001">&gt;10001</a>
                        </dd>
                    </dl>
                    <dl>
                        <dt>婚姻状况：</dt>
                        <dd>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Marriage" pro="0" >未知</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Marriage" pro="1" >未婚</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Marriage" pro="2" >已婚</a>
                        </dd>
                    </dl>
                    <dl>
                        <dt>儿女数量：</dt>
                        <dd>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="ChildNo" pro="-1">未知</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="ChildNo" pro="0">0</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="ChildNo" pro="1">1</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="ChildNo" pro="2">2</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="ChildNo" pro="3">3</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="ChildNo" pro="4">4</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="ChildNo" pro="5">5</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="ChildNo" pro="6">更多</a>
                        </dd>
                    </dl>
                    <dl>
                        <dt>是否凯德员工：</dt>
                        <dd>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="IsKdWorker" pro="0" >否</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="IsKdWorker" pro="1" >是</a>
                        </dd>
                    </dl>
                    <dl>
                        <dt>是否业主：</dt>
                        <dd>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="IsKdOwner" pro="0" >否</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="IsKdOwner" pro="1" >是</a>
                        </dd>
                    </dl>
                    <dl>
                        <dt>国籍：</dt>
                        <dd>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Country" pro="中国" >中国</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Country" pro="中国香港" >中国香港</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Country" pro="中国澳门" >中国澳门</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Country" pro="中国台湾" >中国台湾</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Country" pro="新加坡" >新加坡</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Country" pro="0" >其他</a>
                        </dd>
                    </dl>
                    <dl>
                        <dt>民族：</dt>
                        <dd>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="未知">未知</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="汉族">汉族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="蒙古族">蒙古族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="彝族">彝族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="侗族">侗族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="哈萨克族">哈萨克族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="畲族">畲族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="纳西族">纳西族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="仫佬族">仫佬族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="仡佬族">仡佬族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="怒族">怒族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="保安族">保安族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="鄂伦春族">鄂伦春族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="回族">回族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="壮族">壮族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="瑶族">瑶族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="傣族">傣族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="高山族">高山族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="景颇族">景颇族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="羌族">羌族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="锡伯族">锡伯族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="乌孜别克族">乌孜别克族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="裕固族">裕固族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="赫哲族">赫哲族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="藏族">藏族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="布依族">布依族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="白族">白族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="黎族">黎族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="拉祜族">拉祜族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="柯尔克孜族">柯尔克孜族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="布朗族">布朗族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="阿昌族">阿昌族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="俄罗斯族">俄罗斯族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="京族">京族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="门巴族">门巴族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="维吾尔族">维吾尔族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="朝鲜族">朝鲜族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="土家族">土家族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="傈僳族">傈僳族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="水族">水族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="土族">土族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="撒拉族">撒拉族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="普米族">普米族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="鄂温克族">鄂温克族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="塔塔尔族">塔塔尔族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="珞巴族">珞巴族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="苗族">苗族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="满族">满族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="哈尼族">哈尼族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="佤族">佤族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="东乡族">东乡族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="达斡尔族">达斡尔族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="毛南族">毛南族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="塔吉克族">塔吉克族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="德昂族">德昂族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="独龙族">独龙族</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="Nation" pro="基诺族">基诺族</a>
                        </dd>
                    </dl>
                    <dl>
                        <dt>已关注公众号：</dt>
                        <dd>
                            <asp:CheckBoxList RepeatDirection="Horizontal" ID="cblWeChatNo" runat="server">
                            </asp:CheckBoxList>
                        </dd>
                    </dl>
                    <dl>
                        <dt>是否注册会员：</dt>
                        <dd>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="IsMember" pro="0" >未知</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="IsMember" pro="1" >是</a>
                            <a href="javascript:void(0);" class="btn filterSelect" sh="IsMember" pro="2" >否</a>
                        </dd>
                    </dl>
                    <dl>
                        <dt>兴趣爱好：</dt>
                        <dd>
                            <asp:Literal ID="litHobby" runat="server"></asp:Literal>
                        </dd>
                    </dl>
                    </div>
                </div>


                <div class="btnPanel_01">
                    <asp:Button ID="Button1" runat="server" OnClientClick="return checkForm()" CssClass="btn btn1" Text="确定"/>
                    <asp:LinkButton ID="btnCancel" Name="btnCancel" runat="server" CssClass="btn btn2">取消</asp:LinkButton>
                </div>

            </section>
    </form>
    <script src="../Scripts/user_filter.js"></script>
    <script>

        $(function () {
            setTimeout(function () {
                setFilterSize();
            }, 1000);
        });

        <%=scriptToRun %>

        function selectStartDate(show,start,end) {
            var txtbegin_date = $dp.$(start);
            var txtend_date = $dp.$(end);
            var txt_date_show = $dp.$(show);

            WdatePicker(
            {
                position: { left: -198, top: 10 },
                el: start,
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

        function selectEndDate(start,show) {
            var txt_date_show = $dp.$(show);
            WdatePicker({
                position: { left: -120, top: 10 },
                doubleCalendar: true,
                isShowClear: true,
                readOnly: true,
                dateFmt: 'yyyy-MM-dd',
                minDate: '#F{$dp.$D(\''+start+'\',{d:0});}',
                maxDate: '%y-%M-%d',
                onpicked: function (dp) {
                    if (txt_date_show.value.length > 0) {
                        txt_date_show.value += " — " + dp.cal.getNewDateStr();
                    } else {
                        txt_date_show.value = dp.cal.getNewDateStr();
                    }

                }
            });
        }

        function selectStartDate2(show, start, end) {
            var txtbegin_date = $dp.$(start);
            var txtend_date = $dp.$(end);
            var txt_date_show = $dp.$(show);

            WdatePicker(
            {
                position: { left: -198, top: 10 },
                el: start,
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
                dateFmt: 'MM-dd',

            });
        }

        function selectEndDate2(start, show) {
            var txt_date_show = $dp.$(show);
            WdatePicker({
                position: { left: -120, top: 10 },
                doubleCalendar: true,
                isShowClear: true,
                readOnly: true,
                minDate: '#F{$dp.$D(\'' + start + '\',{d:0});}',
                dateFmt: 'MM-dd',
                onpicked: function (dp) {
                    if (txt_date_show.value.length > 0) {
                        txt_date_show.value += " — " + dp.cal.getNewDateStr();
                    } else {
                        txt_date_show.value = dp.cal.getNewDateStr();
                    }

                }
            });
        }

        $("#btnCancel").click(function () {
            parent.bombbox.closeBox();
        });

        new PCAS("Province", "City", "Area");
        new PCAS("WeChatProvince", "WeChatCity", "WeChatArea");
        function changeList(obj, list, num) {
            obj = $(obj);
            list = $(list);
            obj.removeClass('current').eq(num).addClass('current');
            list.removeClass('current').eq(num).addClass('current');
        }

    </script>

</body>
</html>
