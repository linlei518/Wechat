using KDWechat.BLL.Chats;
using KDWechat.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.Account
{
    public partial class api_token_detail : Web.UI.BasePage
    {

        protected int id
        {
            get { return RequestHelper.GetQueryInt("id", 0); }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (u_type==2 || u_type==3)
            {
                CheckUserAuthority("region_wechat");
            }
            if (!IsPostBack)
            {
                if (id > 0)
                    InitData();
            }
        }

        private void InitData()
        {
            var wechat = wx_wechats.GetWeChatByID(id);

            if (null != wechat)
            {
                txtApiUrl.Text = wechat.api_url;
                txtToken.Text = wechat.token;
                txtApiUrl.Enabled = txtToken.Enabled = false;
            }


        }
    }
}