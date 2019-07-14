using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;

namespace KDWechat.Web
{
    public partial class select_wechat : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int id = KDWechat.Common.RequestHelper.GetQueryInt("id",0);
                if (id>0)
                {
                    KDWechat.DAL.t_wx_wechats model = KDWechat.BLL.Chats.wx_wechats.GetWeChatByID(id);
                    if (model!=null)
                    {

                        Utils.WriteCookie(KDKeys.COOKIE_WECHATS_ID, model.id.ToString());
                        Utils.WriteCookie(KDKeys.COOKIE_WECHATS_WX_OG_ID, model.wx_og_id);
                        Utils.WriteCookie(KDKeys.COOKIE_WECHATS_NAME, model.wx_pb_name);
                        Utils.WriteCookie(KDKeys.COOKIE_WECHATS_HEADIMG, model.header_pic);
                        Utils.WriteCookie(KDKeys.COOKIE_WECHATS_TYPE, model.type_id.ToString());
                        Response.Redirect("~/Index.aspx?m_id=33");
                    }
                }
            }
        }
    }
}