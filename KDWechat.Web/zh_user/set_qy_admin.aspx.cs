using KDWechat.Common;
using KDWechat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.zh_user
{
    public partial class set_qy_admin : Web.UI.BasePage
    {
        protected Common.Config.wechatconfig wechatConfig = new BLL.Config.wechat_config().loadConfig();

        protected int wx_id
        {
            get { return RequestHelper.GetQueryInt("wx_id", 0); }
        }

        protected int id
        {
            get { return RequestHelper.GetQueryInt("id", 0); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //权限相关，发布开启
            CheckUserAuthority("groupmsg_manager_list");
            if (!IsPostBack)
            {
                try
                {
                    //var accessToken = Senparc.Weixin.QY.CommonAPIs.CommonApi.GetToken(wechatConfig.qy_app_id, wechatConfig.qy_app_secret);
                    //var departmentID = Utils.StrToInt(wechatConfig.qy_manage_group, 0);
                    //var list = Senparc.Weixin.QY.AdvancedAPIs.Member.GetDepartmentMemberInfo(accessToken.access_token, departmentID, 1, 1);

                }
                catch
                {
                    JsHelper.RegisterScriptBlock(Page, "showTip.show('企业号接口调用失败，请检查配置后再试。', true);");
                }
            }
        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "sel")
            {
                var userID = e.CommandArgument.ToString();
                var user = DapperConnection.minebea.GetModel<t_zh_user>(new { id = userID});
                if (user != null)
                {
                    Response.Write($"<script>parent.SetQyManager('{user.user_code}','{user.user_name}','{user.user_tel}','{user.user_dpt}','{user.plate_number}','{user.id}')</script>");
                }
                else
                {
                    JsHelper.RegisterScriptBlock(Page, "showTip.show('用户选择失败，请刷新后重试！', true);");
                }
            }
            else
            {
                JsHelper.RegisterScriptBlock(Page, "showTip.show('请勿非法进入！', true);");
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            object where_obj = new { user_name = txtKeywords.Value };
            if (!string.IsNullOrEmpty(txtKeywords.Value))
            {
                where_obj = new {  user_name = txtKeywords.Value };
            }

            var list = DapperConnection.minebea.GetList<t_zh_user>(null, where_obj);

            Repeater1.DataSource = list;
            Repeater1.DataBind();
        }
    }
}