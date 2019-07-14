using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;

namespace KDWechat.Web.main
{
    public partial class topAndmenu : UI.BaseControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (u_type)
            {
                case (int)UserFlag.超级管理员:
                    lblMenu.Text = "<li><a href=\"ManageAccount.aspx?m_id=2\">系统帐号管理</a></li>";
                    lblMenu2.Text = " <li id=\"2\"><a id=\"2\" onclick=\"dialogue.dlLoading();\" href=\"ManageAccount.aspx?m_id=2\"><i class=\"navAccount\"></i>系统帐号管理</a></li>";
                    break;

                case (int)UserFlag.总部账号:
                    
                    var admin=bp.GetAdminInfo();
                    if (admin!=null && admin.is_manage_user_role==1)
                    {
                        lblMenu.Text = "<li><a href=\"ManageAccount.aspx?m_id=2\">系统帐号管理</a></li>";
                        lblMenu2.Text = " <li id=\"2\"><a id=\"2\" onclick=\"dialogue.dlLoading();\" href=\"ManageAccount.aspx?m_id=2\"><i class=\"navAccount\"></i>系统帐号管理</a></li>";
                    }

                 
                    break;

                case (int)UserFlag.地区账号:
                    lblMenu.Text = "<li><a href=\"regoin_account.aspx?m_id=3\">子帐号管理</a></li>";
                    lblMenu2.Text = " <li id=\"3\"><a id=\"3\" onclick=\"dialogue.dlLoading();\" href=\"regoin_account.aspx?m_id=3\"><i class=\"navAccount\"></i>子帐号管理</a></li>";
                    break;
            }
        }
    }
}