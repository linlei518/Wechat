<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sys_group_msg_detail.aspx.cs" Inherits="KDWechat.Web.GroupMsg.sys_group_msg_detail" %>

<%@ Register Src="../UserControl/TopControl.ascx" TagName="TopControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/MenuList.ascx" TagName="MenuList" TagPrefix="uc2" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title><%=pageTitle %></title>
    <link type="text/css" href="../styles/style.css" rel="stylesheet"/>
    <link type="text/css" href="../styles/global.css" rel="stylesheet"/>
    <script src="../scripts/html5.js"></script>
    <script src="../scripts/jquery-1.10.2.min.js"></script> 
    <!--[if lt IE 9 ]><link href="../styles/ie8Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 8 ]><link href="../styles/ie7Fix.css" rel="stylesheet" type="text/css"><![endif]-->
    <!--[if lt IE 7 ]><link href="../styles/ie6Fix.css" rel="stylesheet" type="text/css"><![endif]-->

</head>
<body>
    <uc1:TopControl ID="TopControl1" runat="server" />
    <uc2:MenuList ID="MenuList1" runat="server" />
    <form id="form1" runat="server">
        <section id="main">
                    <%=NavigationName %>
            <div class="titlePanel_01">
                <div class="btns">
                    <a href="sys_group_msg_list.aspx?m_id=79" class="btn btn5"><i class="black back"></i>返回</a>
                </div>
                <h1>查看群发消息</h1>
            </div>
            <div class="listPanel_01">
                    <dl>
			           <dt>标题：</dt>
			            <dd>
                            <input class="txt required" id="txtTitle" placeholder="请输入标题" runat="server" type="text"/>
                        </dd>
		            </dl>
	            </div>
    <!--        <div class="newmassPannle">-->
                

<%--    <asp:Repeater Visible="false" ID="DataRepeater" runat="server" >
                    
                    <HeaderTemplate>
                        <div class="tablePanel_01">
                        <h3>接收人列表</h3>
                        <table cellpadding="0" cellspacing="0" class="table">
			                <thead>
				                <tr>
					                <th class="info info1">昵称</th>
					                <th class="info info1">openID</th>
					                <th class="info info1">国家</th>
					                <th class="info info1">省/直辖市</th>
                                    <th class="info info1">市</th>
				                </tr>
			                </thead>
			                <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                         <tr>
                            <td class="info info1"><%#Eval("nick_name") %></td>
					        <td class="info info1"><%#Eval("open_id") %></td>
					        <td class="info info1"><%#Eval("country") %></td>
					        <td class="info info1"><%#Eval("province") %></td>
					        <td class="info info1"><%#Eval("city") %></td>
				        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                                <%# DataRepeater.Items.Count == 0 ? "<tr><td style=\"text-align:center;\" colspan=\"8\">暂无数据</td></tr>" : ""%>
                                </tbody>
		                    </table>
                        </div>
                    </FooterTemplate>
                </asp:Repeater>--%>


                <div class="selectMessageModule_01"  >
		        <div class="messageNTabPanel_01">
			        <ul>
				        <li><a href="javascript:void(0)" title="文本" class="current"><i class="message1"></i>文本消息</a></li>
				        <li><a href="javascript:void(0);" title="图片"><i class="message2"></i>图片消息</a></li>
				        <li><a href="javascript:void(0);" title="单图文"><i class="message3"></i>单图文消息</a></li>
				        <li><a href="javascript:void(0);" title="多图文"><i class="message4"></i>多图文消息</a></li>
				        <li><a href="javascript:void(0);" title="语音"><i class="message5"></i>语音消息</a></li>
				        <li><a href="javascript:void(0);" title="视频"><i class="message6"></i>视频消息</a></li>
			        </ul>
		        </div>
		        <div class="children">
		        <!--文本消息-->
			        <div class="texchild" id="div_text" >
				          <textarea name="txtContents" runat="server" style="width:99.9%;height:338px;visibility:hidden;"  id="txtContents" class="textarea"></textarea>
			        </div>
		            <!--图片消息-->
			        <div class="simulationPanel_00" id="div_pic" style="display:none"> </div>
		            <!--单图文消息-->
			        <div class="simulationPanel_01" id="div_news" style="display:none"> </div>
		            <!--多图文消息-->
			        <div class="simulationPanel_02" id="div_multi_news" style="display:none"> </div>
		           <!-- 音频消息-->
			        <div class="simulationPanel_03" id="div_voice" style="display:none"> </div>
		           <!--  视频消息-->
			        <div class="simulationPanel_01" id="div_video" style="display:none"> </div>
		        </div>
	        </div>
                <div class="btnPanel_01">
                    <input type="button" value="返回" onclick="location.href='sys_group_msg_list.aspx?m_id=<%=m_id%>';" class="btn btn2"/>
                </div>
      <!--  </div>-->
    </section>

        <script src="../scripts/controls.js"></script><!--这个JS将普通的file控件 select控件转化为JS模拟的 不影响提交表单，如果需要更新页面上的select，请更新好基础控件后调用setupSelect方法--> 
        <script src="../scripts/swfobject.js"></script>
        <script src="../scripts/audio.js"></script>
        <script src="../scripts/video.js"></script>
        <script src="../editor/kindeditor-min.js" type="text/javascript"></script>
        <script src="../editor/lang/zh_CN.js" type="text/javascript"></script>
        <script src="../Scripts/selectMaterial.js" type="text/javascript"></script>


        <script type="text/javascript">
            KindEditor.ready(function (K) {
                var editor = K.create('textarea[name="txtContents"]', {
                    resizeType: 1,
                    pasteType: 1, //纯文本粘贴
                    allowPreviewEmoticons: false,
                    allowImageUpload: false,
                    items: ['emoticons'],
                    afterBlur: function () { this.sync(); },
                    afterCreate: function () {
                        $(".ke-container").attr("style", "border-style:none;");
                        $(".ke-toolbar").attr("style", "border-top:1px solid #CCC;")
                    }
                });
            });
            $(function () {

                $(".message1").click(function () {
                    firstTextClick();
                });

                $("#btnSubmit").click(function () {
                    return btnSubmitClick();
                });

                <%=loadjs%>

            });
            nav.change('<%=m_id%>'); 
        </script>

    </form>
</body>
</html>
