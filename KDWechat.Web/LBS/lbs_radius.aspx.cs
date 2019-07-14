using KDWechat.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web
{
    public partial class lbs_radius : Web.UI.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckUserAuthority("wechat_lbsset");
            if (!IsPostBack)
                InitData();
        }

        private void InitData()
        {
            if (wx_id > 0)
            {
                var wechat = BLL.Chats.wx_wechats.GetWeChatByID(wx_id);
                txtRadius.Text = wechat.lbs_radius.ToString();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            
            if (wx_id > 0)
            {
                var wechat = BLL.Chats.wx_wechats.GetWeChatByID(wx_id);
                int rad = Common.Utils.StrToInt(txtRadius.Text, 5000);
                wechat.lbs_radius = rad;
                BLL.Chats.wx_wechats.UpdateWeChat(wechat);
                JsHelper.RegisterScriptBlock(this, "alert('半径修改成功');parent.bombbox.closeBox();");
            }
        }
    }
}