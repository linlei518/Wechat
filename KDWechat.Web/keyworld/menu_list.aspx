<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="menu_list.aspx.cs" ValidateRequest="false" Inherits="KDWechat.Web.keyworld.menu_list" %>

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

            <div class="navSettingModule_01">
                <div class="navListPanel_01">
                    <div class="titleField">
                        <h2>菜单管理</h2>
                        <div class="btns">
                            <input type="button" class="btn add" onclick="javascript: addMainNavList();" title="添加导航项">
                            <input type="button" class="btn sort" onclick="javascript: navList.sorttingStart();" title="导航项排序" <%=isEdit==false?"style='display:none'":"" %>>
                        </div>
                        <div class="sorts">
                            <input type="button" class="btn btn1" onclick="javascript: navList.sorttingOk();" value="确定">
                            <input type="button" class="btn btn2" onclick="javascript: navList.sorttingCancel();" value="取消">
                        </div>
                    </div>
                    <div class="listField">
                    </div>
                </div>
                <div class="controlArea">
                    <div class="areaTitle">
                        <h2>设置动作</h2>
                    </div>
                    <div class="areaContent messageEditor">

                        <h2>订阅者将收到以下消息</h2>

                        <div class="messageNTabPanel_01">
                            <ul>
                                <li><a href="javascript:void(0)" title="文本" class="current"><i class="message1"></i>文本消息</a></li>
                                <li><a href="javascript:bombbox.openBox('select_pic.aspx?channel_id=1');" title="图片"><i class="message2"></i>图片消息</a></li>
                                <li><a href="javascript:bombbox.openBox('select_news.aspx?channel_id=4');" title="单图文"><i class="message3"></i>单图文消息</a></li>
                                <li><a href="javascript:bombbox.openBox('select_multi-news.aspx?channel_id=5');" title="多图文"><i class="message4"></i>多图文消息</a></li>
                                <li><a href="javascript:bombbox.openBox('select_material_list.aspx?channel_id=2');" title="语音"><i class="message5"></i>语音消息</a></li>
                                <li><a href="javascript:bombbox.openBox('select_material_list.aspx?channel_id=3');" title="视频"><i class="message6"></i>视频消息</a></li>
                                <li><a href="javascript:void(0)" onclick="selectList(6)" title="外部链接"><i class="message7"></i>外部链接</a></li>
                             <%--   <li><a href="javascript:void(0)" onclick="selectList(7)" title="授权模块"><i class="message8"></i>授权链接</a></li>
                                <li><a href="javascript:bombbox.openBox('select_module.aspx?channel_id=8');" title="应用"><i class="message9"></i>应用</a></li>
                                <li><a href="javascript:void(0)" title="多客服"><i class="message11"></i>多客服</a></li>--%>
                            </ul>
                        </div>
                        <div class="children">


                            <div class="articleEditor noBorder" id="div_text">
                                <div class="text">
                                    <textarea class="textarea" id="txtContents" runat="server" maxlength="600"></textarea>
                                </div>
                            </div>
                            <%--图片消息--%>
                            <div class="simulationPanel_00 simulation" id="div_pic" style="display: none"></div>
                            <%--单图文消息--%>
                            <div class="simulationPanel_01 simulation" id="div_news" style="display: none"></div>
                            <%--多图文消息--%>
                            <div class="simulationPanel_02 simulation" id="div_multi_news" style="display: none"></div>
                            <%--音频消息--%>
                            <div class="simulationPanel_03 simulation" id="div_voice" style="display: none"></div>
                            <%--视频消息--%>
                            <div class="simulationPanel_01 simulation" id="div_video" style="display: none"></div>
                            <%--外链消息--%>
                            <div class="listPanel_01" id="div_link" style="display: none">
                                <dl>
                                    <dt>外链地址：</dt>
                                    <dd>
                                        <input type="text" name="txtlike" style="width: 500px" id="txtlike" class="txt" maxlength="225" placeholder="http://" />
                                        <br />
                                        <span style="color: red">例如:http://www.capitaland.com.cn</span>
                                    </dd>
                                </dl>
                            </div>
                            <%-- Damos add at 2015-4-23 13:47 多客服--%>
                            <div class="listPanel_01" id="div_multiCustomer" style="display: none">
                                <dl>
                                    <dt>多客服：</dt>
                                    <dd>
                                        <span style="color: red">点击此菜单将会激活多客服</span>
                                    </dd>
                                </dl>
                            </div>
                            <%--授权消息--%>
                            <div class="listPanel_01" id="div_author" style="display: none">
                                <dl>
                                    <dt>授权地址：</dt>
                                    <dd>
                                        <input type="text" name="txtauthor" style="width: 500px" id="txtauthor" class="txt" maxlength="225" placeholder="http://" />
                                        <br />
                                        <span style="color: red">例如:http://www.capitaland.com.cn，注意：要使用授权，必须要在微信公众平台上设置授权回调页面的域名（回调域名：kdwechat.companycn.net），否则页面将无法打开。</span>
                                    </dd>
                                </dl>
                            </div>
                            <%--模块消息--%>
                            <div class="simulationPanel_01 simulation" id="div_module" style="display: none"></div>

                            <%--文本内容--%>
                            <div class="simulationPanel_04 simulation" id="divtxt" style="display: none"></div>

                        </div>

                        <div class="btnPanel_01 btns1">
                            <input type="button" value="保存" <%=isEdit==false?"style='display:none'":"" %> class="btn btn1" onclick="javascript: editPushOk();">
                            <input type="button" value="取消" <%=isEdit==false?"style='display:none'":"" %> class="btn btn2" onclick="javascript: editPushCancel();">
                        </div>

                        <div class="btnPanel_01 btns2">
                            <input type="button" value="修改" <%=isEdit==false?"style='display:none'":"" %> class="btn btn5" onclick="javascript: editPush();">
                        </div>


                    </div>

                    <div class="areaContent tip tip1">
                        <h2>你可以先添加一个菜单，然后点击菜单项，为其设置响应动作</h2>
                    </div>

                    <div class="areaContent tip tip2">
                        <h2>已有子菜单，无法设置动作</h2>
                    </div>

                </div>
            </div>
            <asp:HiddenField ID="hfdel" runat="server" Value="0" />
            <%--保存消息类型--%>
            <asp:HiddenField ID="hftype" runat="server" Value="0" />
            <%--保存素材id--%>
            <asp:HiddenField ID="hfid" runat="server" Value="0" />
            <div class="btnPanel_01">
                <asp:Button ID="btnPublish" runat="server" Text="发布自定义菜单" OnClientClick="return ck_publish();" OnClick="btnCreate_Click" CssClass="btn btn1" />
                <asp:Button ID="btnDelete" runat="server" Text="" Style="display: none" OnClick="btnDelete_Click" />
                <input type="button" value="关闭菜单" onclick="return clareMenu();" <%=isDelete==false?"style='display:none'":"" %> class="btn btn1">

                <input type="button" value="个人中心“我”栏目" onclick="javascript:showMyMenu();" class="btn btn1">
            </div>
        </section>
        <script src="../scripts/jquery-1.10.2.min.js"></script>
        <script src="../scripts/controls.js"></script>
        <!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
        <script src="../scripts/Bombbox.js"></script>
        <!--弹出框JS 调用方法：1.开启弹出框：bombbox.openBox('链接地址，可以带参')，2.关闭弹出框：bombbox.closeBox();注意：此方法无需在弹出框里面的页面引用-->
        <script src="../scripts/swfobject.js"></script>
        <script src="../scripts/audio.js"></script>
        <script src="../scripts/video.js"></script>
        <script src="../Scripts/selectMenu.js" type="text/javascript"></script>

        <script>

            nav.change('<%=m_id%>');//此处填写被激活的导航项的ID
            loadList();
           
            function showMyMenu()
            {
                var host = window.location.host;
                var protocol = (("https:" == document.location.protocol) ? " https://" : " http://");
                var msg = "<div style='text-align:left'>个人信息：" + protocol + host + "/project/member/myinfo.aspx?wx_id=<%=wx_id%>";
                msg += "<br>我的活动：" + protocol + host + "/project/member/myactivity.aspx?wx_id=<%=wx_id%>";
                msg += "<br>我的预约：" + protocol + host + "/project/member/myreserve.aspx?wx_id=<%=wx_id%>";
                msg += "<br>我的推荐：" + protocol + host + "/project/member/myrecommend.aspx?wx_id=<%=wx_id%>";
                msg += "<br>我的分享：" + protocol + host + "/project/member/myshare.aspx?wx_id=<%=wx_id%></div>";
                dialogue.dlAlert(msg, {width:700});
            }

            $(function () {
                $(".message1").click(function () {
                    firstTextClick();
                });
                $(".message11").click(function () {
                    multiCustomerClick(9);
                });
                
            });

            window.NavList = function () {
                this.navListArea = $('.navSettingModule_01 .navListPanel_01');
                this.controlArea = $('.navSettingModule_01 .controlArea');
                this.selector = {
                    title: 'titleField',
                    mainNavList: '.listField dl',
                    mainNavs: 'dt',
                    subNavs: 'dd',
                    btns: {
                        addBtn: '.add',
                        editBtn: '.edit',
                        deleteBtn: '.delete',
                        upBtn: '.up',
                        downBtn: '.down',
                        sortting: 'sort',
                        ok: 'btn1',
                        cancel: 'btn2'
                    },
                    controlArea: {
                        editor: '.messageEditor',
                        tip1: '.tip1',
                        tip2: '.tip2'
                    }
                }
                this.oldMainNavList = [];
                this.oldSubNavList = [];
                this.isSortted = false;
                this.sorttingClass = 'sortting';
                this.editOkClass = 'editOk';
            }
            NavList.prototype = {
                sorttingStart: function () {
                    this.clearEditNav();
                    var _this = this;
                    this.navListArea.addClass(this.sorttingClass);
                    this.oldMainNavList = [];
                    this.oldSubNavList = [];
                    this.isSortted = false;
                    this.navListArea.find(this.selector.mainNavList).each(function (i) {
                        var obj = this;
                        $(obj).find(_this.selector.mainNavs).find(_this.selector.btns.upBtn).unbind().click(function () {
                            _this.up(obj);
                        });
                        $(obj).find(_this.selector.mainNavs).find(_this.selector.btns.downBtn).unbind().click(function () {
                            _this.down(obj);
                        });
                        _this.oldMainNavList.push(obj);
                    });
                    this.navListArea.find(this.selector.mainNavList).find(this.selector.subNavs).each(function (i) {
                        var obj = this;
                        $(obj).find(_this.selector.btns.upBtn).unbind().click(function () {
                            _this.up(obj);
                        });
                        $(obj).find(_this.selector.btns.downBtn).unbind().click(function () {
                            _this.down(obj);
                        });
                        _this.oldSubNavList.push(obj);
                    });
                },
                up: function (obj) {
                    this.isSortted = true;
                    var tag = obj.tagName;
                    if ($(obj).prev(tag).length > 0) {
                        $(obj).insertBefore($(obj).prev(tag));
                    } else {
                        showTip.show('已经到顶了', true);
                    }
                },
                down: function (obj) {
                    this.isSortted = true;
                    var tag = obj.tagName;
                    if ($(obj).next(tag).length > 0) {
                        $(obj).insertAfter($(obj).next(tag));
                    } else {
                        showTip.show('已经到底了', true);
                    }
                },
                getData: function () {
                    var _this = this;
                    var data = [];
                    this.navListArea.find(this.selector.mainNavList).each(function () {
                        var mainNav = $(this).find(_this.selector.mainNavs).eq(0);
                        var id = mainNav.attr('id');
                        var text = mainNav.find('.text a').html();
                        var hasData = mainNav.hasClass('hasData');

                        var subNavListData = [];
                        $(this).find(_this.selector.subNavs).each(function () {
                            var subId = $(this).attr('id');
                            var subText = $(this).find('.text a').html();
                            var subHasData = $(this).hasClass('hasData');

                            subNavListData.push({
                                id: subId,
                                text: subText,
                                hasData: subHasData
                            });

                        });
                        data.push({
                            id: id,
                            text: text,
                            hasData: hasData,
                            subNavs: subNavListData
                        });

                    });

                    return data;
                },
                sorttingOk: function () {

                    if (!this.isSortted) {
                        this.navListArea.removeClass(this.sorttingClass);
                        return false;
                    }

                    var _this = this;
                    var data = this.getData();
                    sortting(data, function (fail) {
                        fail = fail || false;
                        if (!fail) {
                            _this.navListArea.removeClass(_this.sorttingClass);

                            showTip.show('排序成功');
                        } else {
                            showTip.show('排序提交失败，请稍后提交', true);
                        }
                    });
                },
                sorttingCancel: function () {
                    if (!this.isSortted) {
                        this.navListArea.removeClass(this.sorttingClass);
                        return false;
                    }
                    var _this = this;
                    dialogue.dlAlert({
                        content: '是否要取消排序操作？',
                        btns: [{
                            text: '确认',
                            fn: function () {
                                _this.sorttingReset();
                                dialogue.closeAll();
                            }
                        }, {
                            text: '取消',
                            fn: function () {
                                dialogue.closeAll();
                            }
                        }]
                    });
                },
                sorttingReset: function () {
                    for (var i in this.oldMainNavList) {
                        $(this.oldMainNavList[i]).appendTo($(this.oldMainNavList[i]).parent());
                    }
                    for (var i in this.oldSubNavList) {
                        $(this.oldSubNavList[i]).appendTo($(this.oldSubNavList[i]).parent());
                    }
                    this.navListArea.removeClass(this.sorttingClass);
                },
                getMainNavCount: function () {
                    return this.navListArea.find(this.selector.mainNavList).length;
                },
                clearEditNav: function () {
                    this.navListArea.find(this.selector.mainNavs).removeClass('current');
                    this.navListArea.find(this.selector.subNavs).removeClass('current');
                    this.showTip1();
                    nowEditingId = '';
                },
                showTip1: function () {
                    this.controlArea.find(this.selector.controlArea.editor).hide();
                    this.controlArea.find(this.selector.controlArea.tip2).hide();
                    this.controlArea.find(this.selector.controlArea.tip1).show();
                },
                showTip2: function () {
                    this.controlArea.find(this.selector.controlArea.editor).hide();
                    this.controlArea.find(this.selector.controlArea.tip1).hide();
                    this.controlArea.find(this.selector.controlArea.tip2).show();
                },
                showEditor1: function () {
                    this.controlArea.find(this.selector.controlArea.tip1).hide();
                    this.controlArea.find(this.selector.controlArea.tip2).hide();
                    this.controlArea.find(this.selector.controlArea.editor).addClass(this.editOkClass).show();
                    this.controlArea.find(this.selector.controlArea.editor).find('.children input').each(function () {
                        this.disabled = true;
                    });
                    this.controlArea.find(this.selector.controlArea.editor).find('.children .select').each(function () {
                        this.selectArea.disableAll();
                    });
                },
                showEditor2: function () {
                    this.controlArea.find(this.selector.controlArea.tip1).hide();
                    this.controlArea.find(this.selector.controlArea.tip2).hide();
                    this.controlArea.find(this.selector.controlArea.editor).removeClass(this.editOkClass).show();
                    this.controlArea.find(this.selector.controlArea.editor).find('.children input').each(function () {
                        this.disabled = false;
                    });
                    this.controlArea.find(this.selector.controlArea.editor).find('.children .select').each(function () {
                        this.selectArea.enableAll();
                    });
                }
            }

            var navList = new NavList();



            function addMainNavList() {
                if (navList.getMainNavCount() >= 3) {
                    dialogue.dlAlert('主菜单不能超过3条');
                } else {
                    changeNav();
                }
            }

            function clareMenu() {
                $("#hfdel").val("0");
                dialogue.dlAlert({
                    content: '您确定要删除已发布的菜单吗？',
                    btns: [{
                        text: '确认',
                        fn: function () {
                            dialogue.closeAll();
                            $("#btnDelete").click();
                        }
                    }, {
                        text: '取消',
                        fn: function () {
                            dialogue.closeAll();
                        }
                    }]
                });

            }

            function addSubNavList(obj) {
                var mainNav = $(obj).parents('dt').eq(0);
                var list = $(obj).parents('dl').eq(0).find('dd');
                var id = mainNav.attr('id');
                if (mainNav.hasClass('hasData')) {
                    dialogue.dlAlert({
                        content: '已经为此菜单设置了推送内容<br>是否取消推送内容，转为设置子菜单？',
                        btns: [{
                            text: '确认',
                            fn: function () {
                                mainNav.removeClass('hasData');
                                changeNav({
                                    parent: id
                                })
                                dialogue.closeAll();
                            }
                        }, {
                            text: '取消',
                            fn: function () {
                                dialogue.closeAll();
                            }
                        }]
                    });
                } else if (list.length >= 5) {
                    dialogue.dlAlert('子菜单不能超过5条');
                } else {
                    changeNav({
                        parent: id
                    });
                }
            }

            function editNavList(obj) {
                var navOption = $(obj).parents('dt').eq(0);
                if (navOption.length <= 0) {
                    navOption = $(obj).parents('dd').eq(0);
                }
                var id = navOption.attr('id');
                changeNav({
                    id: id
                });
            }

            function deleteNavList(obj) {
                var flag = false;
                var navOption = $(obj).parents('dt').eq(0);
                if (navOption.length <= 0) {
                    flag = true;
                    navOption = $(obj).parents('dd').eq(0);
                }
                var id = navOption.attr('id');
                var text = '若删除主菜单，该主菜单下的子菜单也将一并被删除<br>操作不可恢复，是否删除该主菜单？';

                if (flag) {
                    text = '是否删除子菜单？该操作不可恢复';
                }

                var isdelete = "<%=isDelete%>";
                if (isdelete == "False") {
                    dialogue.dlAlert('您没有删除权限');
                    return;
                }

                dialogue.dlAlert({
                    content: text,
                    btns: [{
                        text: '确认',
                        fn: function () {
                            if (navOption.hasClass('current')) {
                                navList.clearEditNav();
                            }
                            deleteNav(id, function (fail) {
                                fail = fail || false;
                                if (!fail) {
                                    showTip.show('删除成功');
                                } else {
                                    showTip.show('删除失败，请稍后再操作一次', true);
                                }
                                dialogue.closeAll();
                            })

                        }
                    }, {
                        text: '取消',
                        fn: function () {
                            dialogue.closeAll();
                        }
                    }]
                });
            }

            var nowEditingId = '';
            var oldPushHtml = [];

            var simulations = $('.navSettingModule_01 .simulation');


            function editNavPush(obj) {
                if (navList.navListArea.hasClass('sortting')) return false;
                $("#div_text").hide();
                navList.clearEditNav();
                var flag = false;
                var navOption = $(obj).parents('dt').eq(0);
                if (navOption.length <= 0) {
                    flag = true;
                    navOption = $(obj).parents('dd').eq(0);
                }
                var id = navOption.attr('id');
                nowEditingId = id;
                navOption.addClass('current');


                var willLoading = true;
                if (!flag) {
                    var list = $(obj).parents('dl').eq(0).find('dd');
                    if (list.length > 0) {
                        willLoading = false;
                        navList.showTip2();
                    }
                }
                if (willLoading) {
                    if (navOption.hasClass('hasData')) {
                        navList.showEditor1();

                    } else {
                        navList.showEditor2(); //没动作
                        
                        firstTextClick();
                    }
                    showPush(id, function (fail) {
                        fail = fail || false;
                        if (!fail) {
                        } else {
                            navList.clearEditNav();
                            showTip.show('获取数据失败，请稍后再操作一次', true);
                        }
                    });
                }
            }


            function editPush() {
                //pushDom = navList.controlArea.find(navList.selector.controlArea.editor).find('.children');
                oldPushHtml = [];
                simulations.each(function () {
                    oldPushHtml.push({
                        style: $(this).attr('style'),
                        html: $(this).html()
                    });
                });
                //oldPushHtml = pushDom.html();
                //console.log(oldPushHtml);
                navList.showEditor2();
                if ($("#hftype").val() == "0") {
                    $("#hftype").val("0")
                    firstTextClick();
                    
                }
            }

            function editPushCancel() {
                // pushDom = navList.controlArea.find(navList.selector.controlArea.editor).find('.children');
                // pushDom.html(oldPushHtml);

                simulations.each(function (i) {
                    $(this).attr('style', oldPushHtml[i].style);
                    $(this).html(oldPushHtml[i].html);
                });

                if ($("#hftype").val() == "0") {
                    var content = $("#divtxt").html();
                    clareShow();
                    $("#hftype").val("0");
                    $("#divtxt").show();
                    $("#divtxt").html(content);
                }
                navList.showEditor1();
            }

            function editPushOk() {

                editPushSubmit(nowEditingId, function (fail) {
                    fail = fail || false;
                    if (!fail) {
                        showTip.show('修改推送数据成功');
                        if ($("#hftype").val() == "0") {
                            var content = $("#divtxt").html();
                            clareShow();
                            $("#hftype").val("0");
                            $("#divtxt").show();
                            $("#divtxt").html(content);
                        }
                        navList.showEditor1();
                    } else {
                        showTip.show('修改推送失败，请稍后再操作一次', true);
                    }
                })

            }




            function showPush(id, callback) {
                dialogue.dlLoading();//显示Loading
                $("#txtContents").val("");
                $("#txtlike").val("");
                $("#txtauthor").val("");
                $.ajax({
                    type: "POST",
                    url: "/handles/wechat_ajax.ashx?action=diy_meun&type=show&id=" + id,
                    timeout: 60000,
                    dataType: 'json',
                    success: function (data) {
                        dialogue.closeAll();//隐藏Loading
                        if (data.status == 1) {

                            switch (data.channel_id) {
                                case 0:
                                    clareShow();
                                    $("#hftype").val("0");
                                    var _contents = data.contents;
                                    //console.log(_contents);
                                    $("#txtContents").val(_contents);
                                    $("#divtxt").show();
                                    _contents = _contents.replace(/\n/g, "<br>").replace(/\r\n/g, "<br>").replace(/\r/g, "<br>")
                                    
                                    $("#divtxt").html(_contents);

                                
                                    break;
                                case 6:
                                    clareShow();
                                    $("#hftype").val("6");
                                    selectList(data.channel_id);
                                    $("#txtlike").val(data.contents);
                                    break;
                                case 7:
                                    clareShow();
                                    $("#hftype").val("7");
                                    selectList(data.channel_id);
                                    $("#txtauthor").val(data.contents);
                                    break;
                                case 1:
                                case 2:
                                case 3:
                                case 4:
                                case 5:
                                case 8:
                                    selectResult(data.channel_id, data.material_id, data._path_link, data._title, data._create_time, data._suumary, data.is_close, data._multi_list, data.video_img, data.video_type);
                                    break;
                                case 10:
                                    clareShow();
                                    $("#hftype").val("10");
                                    $("#hfid").val("10");
                                    $("#div_multiCustomer").show();

                            }


                        }
                        callback(false);
                    },
                    error: function (data, status, e) {
                        dialogue.closeAll();
                        callback(false);
                    }
                });


            }


            function editPushSubmit(id, callback) {

                //验证用户输入
                if (btnSubmitClick()) {
                    dialogue.dlLoading();//显示Loading
                    var contents = $("#txtContents").val();
                    $("#divtxt").html($("#txtContents").val());
                    var type = $("#hftype").val();
                    if (type == 6) {
                        contents = $("#txtlike").val();
                    } else if (type == 7) {
                        contents = $("#txtauthor").val();
                    }
                    $.ajax({
                        type: "POST",
                        url: "/handles/wechat_ajax.ashx?action=diy_meun&type=event&id=" + id + "&channel_id=" + type + "&source_id=" + $("#hfid").val() ,
                        timeout: 60000,
                        data: { contents: contents },
                        timeout: 60000,
                        dataType: 'json',
                        success: function (data) {
                            dialogue.closeAll();//隐藏Loading
                            if (data.status == 1) {
                                loadList();
                                callback(false);
                            } else {
                                callback(true);
                            }
                        },
                        error: function (data, status, e) {
                            dialogue.closeAll();
                            callback(true);
                        }
                    });
                }

            }



            function changeNav(config) {//请程序员修改，这里是弹出框来添加或修改子菜单，操作完成后请更新菜单列表
                var isAdd = false;
                var isAddMainNav = false;
                if (typeof config == 'undefined') {
                    isAdd = true;
                    isAddMainNav = true;
                    config = {};
                } else if ((typeof config.parent != 'undefined') || (typeof config.id == 'undefined')) {
                    isAdd = true;
                }
                var parent = config.parent || ''//新增菜单时，父级的ID
                var id = config.id || ''//修改菜单时，该菜单项的ID


                bombbox.openBox('menu_edit_name.aspx?id=' + id + '&parent_id=' + parent);
            }


            function deleteNav(id, callback) {//请程序员修改，这里是删除菜单，删除后请更新菜单列表

                dialogue.dlLoading();//显示Loading

                $.ajax({
                    type: "POST",
                    url: "/handles/wechat_ajax.ashx?action=diy_meun&type=del&id=" + id,
                    timeout: 60000,
                    dataType: 'json',
                    success: function (data) {
                        dialogue.closeAll();//隐藏Loading
                        if (data.status == 1) {
                            loadList();
                            callback(false);
                        } else {
                            callback(true);
                        }
                    },
                    error: function (data, status, e) {
                        dialogue.closeAll();
                        callback(true);
                    }
                });

            }


            function sortting(data, callback) {
                //这是排序，点击确定后的方法，请程序猿改写此方法
                //注意，用户必须进行过排序动作修改了顺序，才能进入此方法！！
                dialogue.dlLoading();//显示Loading
                //console.log(data);
                for (var i = 0; i < data.length; i++) {

                    var id = data[i]["id"];
                    $.post("../handles/wechat_ajax.ashx?action=menu_sort&cid=" + id + "&px=" + i + "&time=" + Math.random(), function (res) {
                        if (res.msg == 1) {

                        }

                    }, 'json');
                    var subNavs = data[i]["subNavs"];
                    for (var j = 0; j < subNavs.length; j++) {

                        $.post("../handles/wechat_ajax.ashx?action=menu_sort&cid=" + subNavs[j]["id"] + "&px=" + j + "&time=" + Math.random(), function (res) {
                            if (res.msg == 1) {

                            }

                        }, 'json');
                    }
                }

                dialogue.closeAll();//隐藏Loading
                callback(false);

            }


            function loadList() {

                $.ajax({
                    type: "POST",
                    url: "/handles/wechat_ajax.ashx?action=diy_meun&type=list",
                    timeout: 60000,
                    contentType: 'text/html; charset=utf-8;',
                    beforeSend: function () {
                        dialogue.dlLoading();//显示Loading
                    },
                    success: function (data) {
                        dialogue.closeAll();//隐藏Loading
                        $(".listField").html(data);
                    },
                    error: function (data, status, e) {
                        showTip.show("数据加载失败", true);
                    }
                });

            }

            function ck_publish() {
                var data = navList.getData();
               // console.log(data);
                var isc = true;
                for (var i = 0; i < data.length; i++) {

                    var id = data[i]["id"];
                    var hasData = data[i]["hasData"];
                    var subNavs = data[i]["subNavs"];
                    if (hasData == false && subNavs.length == 0) {
                        showTip.show("请给没有子菜单的一级菜单添加一个动作", true);
                        isc = false;
                        break;
                    }
                    if (subNavs.length > 0) {
                        for (var j = 0; j < subNavs.length; j++) {
                            var c_hasData = subNavs[j]["hasData"];
                            if (!c_hasData) {

                                showTip.show("请给二级菜单添加一个动作", true);
                                isc = false;
                                break;
                            }
                        }
                    }

                } if (isc) {
                    dialogue.dlLoading();//显示Loading
                }


                return isc;
            }


            //下面是顶部提示栏的用法：


            /*
            showTip.show({								//这里是显示顶部提示的代码
                title:'这里是测试数据',					//标题，必填
                content:'某一条测试数据成功失败！',		//内容，选填
                control:true,							//是否显示一个 X 用来关闭提示，默认为否，选填
                delay:4000								//延迟多久自动消失，默认为3000（毫秒），填0则永远不消失，选填
            },true);									//是否显示警告（true为红色框，false或者不填为绿色框）
            //这个提示的另一种用法是在url地址栏 最后面 加 “#success=内容” 或者 “#fail=内容” 或者 “?success=内容” 或者 “&success=内容” 用于刷新、切换页面时做提示
            */


</script>

    </form>
</body>
</html>
