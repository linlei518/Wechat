using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.DAL;
using EFHelper = Companycn.Core.EntityFramework.EFHelper;

namespace KDWechat.Web.GroupMsg
{
    public partial class groupmsg_overview : Web.UI.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            dlResult.Attributes.Remove("style");
            Expression<Func<t_wx_fans, bool>> where = x => x.nick_name.Contains(txtNickName.Text) && x.wx_id == wx_id;
            var list = EFHelper.GetList<creater_wxEntities, t_wx_fans, int>(where, x => x.id, int.MaxValue, 1, true);
            Repeater1.DataSource = list;
            Repeater1.DataBind();
        }
    }
}