<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="filter_export.aspx.cs" Inherits="KDWechat.Web.fans.filter_export" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
        <script src="../Scripts/html5.js"></script>
        <link type="text/css" href="../styles/global.css" rel="stylesheet"/>
</head>
<body class="bombbox" >
    <form id="form1" runat="server">
        <header id="bombboxTitle">
            <div class="titlePanel_01">
                <h1>选择导出的字段</h1>
            </div>
        </header>
        <section id="bombboxMain">
            <div class="listPanel_01 bottomLine">
                <dl>
                    <dd>
                        <input id="checkall" value="全部" type="checkbox" onclick="checkAll(this)" /> 全部
                        <asp:CheckBoxList RepeatColumns="5" ID="chbAllowWechats" RepeatDirection="Horizontal"  runat="server">
                            <asp:ListItem Value="open_id">openid</asp:ListItem>
                            <asp:ListItem Value="nick_name">昵称</asp:ListItem>
                            <asp:ListItem Value="wx_sex">微信性别</asp:ListItem>
                            <asp:ListItem Value="wx_country">微信国籍</asp:ListItem>
                            <asp:ListItem Value="wx_province">微信省份</asp:ListItem>
                            <asp:ListItem Value="wx_city">微信城市</asp:ListItem>
                            <asp:ListItem Value="sex">性别</asp:ListItem>
                            <asp:ListItem Value="country">国籍</asp:ListItem>
                            <asp:ListItem Value="ethnic">民族</asp:ListItem>
                            <asp:ListItem Value="province">省份</asp:ListItem>
                            <asp:ListItem Value="city">城市</asp:ListItem>
                            <asp:ListItem Value="language">语言</asp:ListItem>
                            <asp:ListItem Value="headimgurl">头像地址</asp:ListItem>
                            <asp:ListItem Value="is_kd_owner">是否凯德业主</asp:ListItem>
                            <asp:ListItem Value="is_kd_worker">是否凯德员工</asp:ListItem>
                            <asp:ListItem Value="buy_project">购买过的项目</asp:ListItem>
                            <asp:ListItem Value="real_name">真名</asp:ListItem>
                            <asp:ListItem Value="email">邮箱</asp:ListItem>
                            <asp:ListItem Value="mobile">手机</asp:ListItem>
                            <asp:ListItem Value="qq">qq</asp:ListItem>
                            <asp:ListItem Value="wechat_no">微信号</asp:ListItem>
                            <asp:ListItem Value="weibo_name">微博名</asp:ListItem>
                            <asp:ListItem Value="birth">生日</asp:ListItem>
                            <asp:ListItem Value="id_card_type">证件类型</asp:ListItem>
                            <asp:ListItem Value="id_card">证件号</asp:ListItem>
                            <asp:ListItem Value="home_address">住址</asp:ListItem>
                            <asp:ListItem Value="company">公司</asp:ListItem>
                            <asp:ListItem Value="post_title">职称</asp:ListItem>
                            <asp:ListItem Value="job">工作</asp:ListItem>
                            <asp:ListItem Value="month_income">月收入</asp:ListItem>
                            <asp:ListItem Value="office_tel">办公电话</asp:ListItem>
                            <asp:ListItem Value="marriage">是否已婚</asp:ListItem>
                            <asp:ListItem Value="have_child">是否有子女</asp:ListItem>
                            <asp:ListItem Value="family_size">子女数量</asp:ListItem>
                            <asp:ListItem Value="family_month_income">家庭月收入</asp:ListItem>
                            <asp:ListItem Value="spending_power">消费能力</asp:ListItem>
                            <asp:ListItem Value="hobby">兴趣</asp:ListItem>
                            <asp:ListItem Value="remark">备注</asp:ListItem>
                            <asp:ListItem Value="status">状态</asp:ListItem>
                            <asp:ListItem Value="subscribe_time">关注时间</asp:ListItem>
                            <asp:ListItem Value="remove_time">取消关注时间</asp:ListItem>
                            <asp:ListItem Value="interest_project">感兴趣的项目</asp:ListItem>
                        </asp:CheckBoxList>
                    </dd>
                </dl>
            </div>
            <div class="btnPanel_01">

                <asp:Button ID="btnSubmit" CssClass="btn btn1" runat="server" Text="确定" OnClick="btnSubmit_Click" />
                <asp:LinkButton ID="btnCancel" Name="btnCancel" runat="server" CssClass="btn btn2">取消</asp:LinkButton>
            </div>

        </section>
    </form>

    <script src="../scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../scripts/controls.js" type="text/javascript"></script>
    <script>
        $("#btnCancel").click(function () {
            parent.bombbox.closeBox();
        });
        var offsetSize = {//这玩意定义弹出框的高宽
            width: 820,
            height: 510
        }

        function checkAll(btn) {
            var list = $("#chbAllowWechats [type='checkbox']");
            if (!btn.checked)
            {
                for (var i = 0; i < list.length; i++)
                    list[i].checked = false;
            }
            else {
                for (var i = 0; i < list.length; i++)
                    list[i].checked = true;
            }
        }
    </script>
</body>
</html>
