using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.QY
{
    public partial class Set_Qy_Secret : Web.UI.BasePage
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
            var config = configManager.loadConfig();
            txtAppId.Text = config.qy_app_id;
            txtAppSecret.Text = config.qy_app_secret;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var configManager = new BLL.Config.wechat_config();
            var config = configManager.loadConfig();
            config.qy_app_id = txtAppId.Text;
            config.qy_app_secret = txtAppSecret.Text;
            configManager.saveConifg(config);
            Response.Redirect("select_qy_group.aspx");
        }
    }
}