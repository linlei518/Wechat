using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;
using KDWechat.BLL.Chats;

namespace KDWechat.Web.module
{
    public partial class show_description : System.Web.UI.Page
    {
        protected string description = "";
        protected int tp { get { return RequestHelper.GetQueryInt("tp", -1); } }
        protected int id { get { return RequestHelper.GetQueryInt("id", -1); } }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                InitData();
        }

        private void InitData()
        {
            if (tp == 1)
            {
                var model = modules.GetModel(id);
                description = model == null ? "" : model.description;
            }
            else
            {
                //var model = module_wechat.GetModel(id);
                //description = model == null ? "" : model.description;
                if (id > 0)
                {
                    var log = BLL.Logs.wx_logs.GetModel<int>(x => x.id == id, x => x.id, true);
                    if(log!=null)
                    {
                        description = log.contents;
                    }
                }
            }
        }
    }
}