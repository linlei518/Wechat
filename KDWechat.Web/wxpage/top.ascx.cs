using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.wxpage
{
    public partial class top : System.Web.UI.UserControl
    {
        /// <summary>
        /// 微信名称
        /// </summary>
        public string wx_name
        {
            get;
            set;
        }
        /// <summary>
        /// 微信头像
        /// </summary>
        public string wx_head_pic
        {
            get;
            set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblwx_name.Text = wx_name;
            img_head.Src = wx_head_pic;
        }
    }
}