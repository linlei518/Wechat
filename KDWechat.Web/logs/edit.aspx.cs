using KDWechat.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.logs
{
    public partial class edit : KDWechat.Web.UI.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (id > 0)
            {
                BindInfo();
            }
        }

        protected int id
        {
            get { return RequestHelper.GetQueryInt("id", 0); }
        }

        private void BindInfo()
        {
            KDWechat.DAL.t_wx_logs model =KDWechat.BLL.Logs.wx_logs.GetWxLogsByID(id);
            if (null != model)
            {
                txtUName.InnerText = model.login_name;
                txtContents.InnerText = model.contents;
                txtIP.InnerText = model.ip;
                txtTime.InnerText = model.create_time.ToString();

                AddLog("查看操作日志："+model.id,LogType.添加);
            }
        }
    }
}