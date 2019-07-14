using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;
using KDWechat.DAL;

namespace KDWechat.Web
{
    public partial class SignIn : System.Web.UI.Page
    {

        public int is_show = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                is_show = RequestHelper.GetQueryInt("is_show", 0);
            }
            else
            {
                var open_id = RequestHelper.GetString("open_id");
                var model = DapperConnection.minebea.GetModel<t_invitation>(new { open_id = open_id });
                if (string.IsNullOrEmpty(open_id)||model.id<=0)
                {
                    is_show = 1;
                    JsHelper.AlertAndRedirect("参数错误！", "/SignIn.aspx?is_show="+is_show);
                    return;
                }
                if (model.status == 1)
                {
                    is_show = 1;
                    JsHelper.AlertAndRedirect("您已签到！", "/SignIn.aspx?is_show=" + is_show);
                    return;
                }
                model.status = 1;
                var success=DapperConnection.minebea.UpdateModel(model, "id");
                if (success)
                {
                    is_show = 1;
                    JsHelper.AlertAndRedirect("签到成功！", "/SignIn.aspx?is_show=" + is_show);
                    return;
                }

            }
        }
    }
}