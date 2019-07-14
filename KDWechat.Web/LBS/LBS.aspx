<%@ Page enableEventValidation="false" validateRequest="false" Language="C#" AutoEventWireup="true" CodeBehind="LBS.aspx.cs" Inherits="KDWechat.Web.LBS" %>

<%@ Register TagName="TopControl" TagPrefix="uc" Src="~/UserControl/TopControl.ascx" %>
<%@ Register TagName="MenuList" TagPrefix="uc" Src="~/UserControl/MenuList.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title><%=pageTitle %></title>
    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../scripts/html5.js"></script>
    <script src="../Scripts/jquery.validate/jquery.validate.js"></script>
    <script src="../Scripts/jquery.validate/jquery.metadata.js"></script>
    <script src="../Scripts/jquery.validate/messages_cn.js"></script>
    <script type="text/javascript" src="http://api.map.baidu.com/api?v=2.0&ak=DD610e50dbdc31173edc1d5c92be4076"></script>

    <script>
        $(document).ready(function () {
            $("#form1").validate(
                {
                    submitHandler: function (form) {
                    var errorlabel = $("#errorLabel");
                    errorlabel.html("");
                    if ($("#Province").val() == "选择省份" || $("#City").val() == "选择城市" || $("#Area").val() == "选择地区" || $("#lat").val() == "")
                    {
                        errorlabel.html("请选择完整县市并点击定位");
                        errorlabel.show();
                        errorlabel.focus();
                        return false;
                    }
                    if ($("#txtWUrl").val() != "" && ($("#txtWUrl").val().indexOf("http://") == -1 && $("#txtWUrl").val().indexOf("https://") == -1))
                    {
                        alert("外链地址不对！");
                        return false;
                    }
                    form.submit();
                }
            });
        });
    </script>

    <link type="text/css" href="../styles/global.css" rel="stylesheet"/>
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->

</head>
<body>
    <uc:TopControl runat="server" />
    <uc:MenuList runat="server" />
    <form id="form1" runat="server">
        <section id="main">
                    <%=NavigationName %>
	        <div class="titlePanel_01">
                <div class="btns">
                    <a href="LBSList.aspx?m_id=15" class="btn btn5"><i class="black back"></i>返回</a>
                </div>
		        <h1>LBS设置</h1>
	        </div>
	        <div class="listPanel_01 bottomLine">
		        <dl>
			        <dt>说　　明：</dt>
			        <dd>开启LBS功能后用户可在微信上发送地理位置获取您的地理位置信息。</dd>
		        </dl>
		        <dl>
			        <dt>名　　称：</dt>
			        <dd><asp:TextBox ID="TitleText" CssClass="txt required" MaxLength="50" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;50字以内</dd>
		        </dl>
		        <dl>
			        <dt>所在城市：</dt>
			        <dd id="areaSelect">
                        <asp:DropDownList ID="Province" CssClass="select required" runat="server">
                        </asp:DropDownList>
                        <asp:DropDownList ID="City" CssClass="select required" runat="server">
                        </asp:DropDownList>
                        <asp:DropDownList ID="Area" CssClass="select required" runat="server">
                        </asp:DropDownList>
                        <label id="errorLabel" class="error"></label>
			        </dd>
		        </dl>
		        <dl>
			        <dt>详细地址：</dt>
			        <dd>
				        <div class="subGroup">
                            <asp:TextBox ID="AddressText" MaxLength="100" CssClass="txt required" runat="server"></asp:TextBox>
					        <input id="loca" type="button" class="btn btn5" onclick="loca2()" value="定位" />
                            <input type="hidden" id="lat" runat="server" />
                            <input type="hidden" id="lng" runat="server" />
                            &nbsp;&nbsp;&nbsp;&nbsp;100字以内，不需要重复写上面的省、市、区
				        </div>
			        </dd>
		        </dl>
		
		        <dl>
		            <dd class="mapField">
                        <div id="allmap" style="width:678px;height:400px;"></div>
		            </dd>
		        </dl>
                <dl>
                    <dt>外链地址：</dt>
		            <dd>
                        <asp:TextBox ID="txtWUrl" CssClass="txt" runat="server"></asp:TextBox><span>填入外链地址后将不显示内容详情页。(外链地址格式：http://xxx.xxx.xxx)</span>
		            </dd>
		        </dl>
		
		        <dl>
			        <dt>简介说明：</dt>
			        <dd><textarea id="txtContent" class="required" style="width:99.9%;height:138px;visibility:hidden;" runat="server"></textarea><%--<asp:TextBox ID="txtContent" TextMode="MultiLine" CssClass="required" runat="server"></asp:TextBox>--%></dd>
		        </dl>
		        <dl>
			        <dt>上传图片：</dt>
			        <dd>
                        <%--原文件路劲，针对修改的时候--%>
                        <asp:HiddenField ID="hf_old_file" runat="server"></asp:HiddenField>
                        <%--文件后缀--%>
                        <asp:HiddenField ID="hf_type" runat="server" />
                        <%--文件原文件名称--%>
                        <asp:HiddenField ID="hf_name" runat="server" />
                        <%--文件大小--%>
                        <asp:HiddenField ID="hf_size" runat="server" />

                            <div class="img">
					            <span>
						            <img src="../images/blank.gif" runat="server"  id="img_show" width="120" height="80">
					            </span>
					            <input type="button"  id="btnUpload" class="btn btn5" value="浏览..."/>
				            </div>



                        <asp:TextBox ID="txtFile" runat="server" class="txt required" style="width:1px; height:1px; background-color:transparent;"></asp:TextBox>
                        <%--调用编辑器的上传--%>
                        
				        大小：不超过2M,格式：bmp, png, jpeg, jpg, gif
			        </dd>
		        </dl>
                <dl>
                    <dt>是否封面：</dt>
                    <dd>
                        <label class="radioArea">
                            <input runat="server" id="radStatusOk" type="radio" class="radio" name="accountState" />是</label>
                        <label class="radioArea">
                            <input type="radio" id="radStatusFalse" runat="server" class="radio" name="accountState" />否</label>
                        <label class="error" id="labState"></label>
                    </dd>
                </dl>
	        </div>
	        <div class="btnPanel_01">
                <asp:Button ID="SubmitButton" CssClass="btn btn1" runat="server" Text="确定" OnClick="SubmitButton_Click" />
                <asp:LinkButton ID="CancelButton" CssClass="btn btn2" OnClick="CancelButton_Click" runat="server" >取消</asp:LinkButton>
	        </div>
        </section>
    </form>
    <script src="../scripts/selectAddress.js"></script><!--三级联动选择地址的JS-->
    <script src="../scripts/controls.js"></script><!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法-->
    <script src="../editor/kindeditor-min.js" type="text/javascript"></script>
    <script src="../editor/lang/zh_CN.js" type="text/javascript"></script>
    <script src="../Scripts/function.js" type="text/javascript"></script>
    <script type="text/javascript">
        KindEditor.ready(function (K) {
            <%--调用编辑器的上传加载开始,folder参数表示当前微信号的文件夹--%>
            var editor = K.editor({
                uploadJson: '../handles/upload_ajax.ashx?action=EditorFile&IsWater=1&folder=<%=wx_og_id%>',
                fileManagerJson: '../handles/upload_ajax.ashx?action=ManagerFile&folder=<%=wx_og_id%>',
                allowFileManager: true
            });
            K('#btnUpload').click(function () {
                editor.loadPlugin('image', function () {
                    editor.plugin.imageDialog({
                        imageUrl: K('#txtFile').val(),
                        clickFn: function (url, title, width, height, border, align) {
                            K('#txtFile').val(url);
                            K('#img_show').attr("src", url);
                            editor.hideDialog();
                        }
                        
                    });
                });
            });            <%--调用编辑器的上传事件结束--%>
            <%--初始化文本框的编辑器--%>
            K.create('#txtContent', {
                //resizeType: 1,
                //pasteType: 1, //纯文本粘贴
                //allowPreviewEmoticons: false,
                //allowImageUpload: false,
                //items: ['link', 'unlink'],
                //
                resizeType: 1,
                allowPreviewEmoticons: false,
                allowImageUpload: true,
                allowFileManager: true,
                uploadJson: '../handles/upload_ajax.ashx?action=EditorFile&IsWater=1&folder=<%=folder%>&upload_type=<%=(int)KDWechat.Common.UploadType.图片%>&wx_id=<%=wx_id%>&u_id=<%=u_id%>',
                fileManagerJson: '../handles/upload_ajax.ashx?action=ManagerFile&folder=<%=folder%>&upload_type=<%=(int)KDWechat.Common.UploadType.图片%>',
                items: ['source', 'forecolor', 'hilitecolor', 'bold', 'italic', 'underline', 'removeformat',  'justifyleft', 'justifycenter', 'justifyright', 'image', 'link', 'unlink'],
                afterBlur: function () { this.sync(); }
            });
        });

    </script>
    <script>
        nav.change('<%=m_id%>'); 
    </script>
</body>
</html>
<script>
    var map = new BMap.Map("allmap"); <%--新建地图--%>
    map.centerAndZoom("上海", 12);  <%--设置地图中心--%>
    var overLay = new BMap.Marker();  <%--新建一个覆盖图--%>
    map.enableScrollWheelZoom();   <%--适用滚轮缩放--%>
    map.addControl(new BMap.NavigationControl()); <%--添加缩放控件--%>
    
    map.addEventListener("click", function (e) {  <%--添加鼠标点击事件--%>
        overLay.setPosition(e.point);    <%--将覆盖物的坐标设置为点击点的坐标--%>
        $("#lat").val(e.point.lat);  <%--控件赋值--%>
        $("#lng").val(e.point.lng);  <%--控件赋值--%>
        map.addOverlay(overLay);   <%--在地图中添加覆盖物--%>
    });

    function loca2() {

        var myGeo = new BMap.Geocoder();  <%--新建一个地址翻译类--%>
        var dizhi = $("#Province").val() + $("#City").val() + $("#Area").val() + $("#AddressText").val(); <%--组成地址--%>
        myGeo.getPoint(dizhi, function (point) {  <%--地址翻译成坐标--%>
            if (point) {
                map.centerAndZoom(point, 15);   <%--将地图中心转向坐标处--%>
                $("#lat").val(point.lat); <%--控件赋值--%>
                $("#lng").val(point.lng); <%--控件赋值--%>
                overLay.setPosition(point);  <%--给覆盖物赋坐标--%>
                map.addOverlay(overLay);   <%--在地图中添加覆盖物--%>
            }
        }, $("#province").value);
    };

    new PCAS("Province", "City", "Area");
    <%= load_script%>

</script>