using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;

namespace KDWechat.Web.Main
{
    public partial class transit : UI.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (RequestHelper.GetQueryString("type")=="load")
            {
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                //禁止缓存
                Response.Expires = -1;//相对过期时间
                Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);//绝对过期时间
                Response.CacheControl = "no-cache";
                Response.AddHeader("pragma", "no-cache");
                Response.AddHeader("cache-control", "private");
                Response.ContentType = "text/plain";
                string url = RequestHelper.GetQueryString("returnUrl");
                string loginStr = u_id + "|" + u_type + "|" + u_name + "|" + Guid.NewGuid();
                if (url.Contains("?"))
                {
                    url += "&t=" + DESEncrypt.Encrypt(loginStr, "kd_sys_login");
                }
                else
                {
                    url += "?t=" + DESEncrypt.Encrypt(loginStr, "kd_sys_login");
                }

                Response.Write(url);
                Response.End();
            }
        }
    }
}