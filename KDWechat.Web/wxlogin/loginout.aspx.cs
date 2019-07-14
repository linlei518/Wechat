using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;
// gh_4d06344cab1e
namespace KDWechat.Web.KDlogin
{
    public partial class loginout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.WriteCookie(KDKeys.COOKIE_WECHATS_ID, "0");
            Utils.WriteCookie(KDKeys.COOKIE_WECHATS_WX_OG_ID, "");
            Utils.WriteCookie(KDKeys.COOKIE_WECHATS_NAME, "");

            Session[KDKeys.SESSION_ADMIN_INFO] = null;
            //Session.Clear();
            Utils.WriteCookie(KDKeys.COOKIE_USER_NAME,"");
            Utils.WriteCookie(KDKeys.COOKIE_USER_PWD, "");   //ceiling 更新
            Utils.WriteCookie(KDKeys.COOKIE_USER_FlAG, "");//用户类型
            Response.Redirect("login.aspx?t=loginout");
        }
    }
}