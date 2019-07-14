using KDWechat.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.QY
{
    public partial class Select_QY_Group : Web.UI.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitData();
            }
        }

        private void InitData()
        {
            var configManager = new BLL.Config.wechat_config();
            var wechatConfig = configManager.loadConfig();
            var accessToken = Senparc.Weixin.QY.CommonAPIs.CommonApi.GetToken(wechatConfig.qy_app_id, wechatConfig.qy_app_secret);
            var departmentID = Utils.StrToInt(wechatConfig.qy_manage_group, 0);
            var list = Senparc.Weixin.QY.AdvancedAPIs.Department.GetDepartmentList(accessToken.access_token);
            DataRepeater.DataSource = list.department;
            DataRepeater.DataBind();
        }


        protected void DataRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "set")
            {
                var departID = KDWechat.Common.Utils.StrToInt(e.CommandArgument.ToString(),-1);
                if (departID != -1)
                {
                    var configManager =new BLL.Config.wechat_config();
                    var config = configManager.loadConfig();
                    if (config.qy_manage_group == "-1")
                    {
                        config.qy_manage_group = departID.ToString();
                        configManager.saveConifg(config);
                        JsHelper.RegisterScriptBlock(Page, "showTip.show('管理组设置成功', true);");
                    }
                    else
                    {
                        JsHelper.RegisterScriptBlock(Page, "showTip.show('您已设置过管理组，无法重复设置', true);");
                    }
                }
            }
        }
    }
}