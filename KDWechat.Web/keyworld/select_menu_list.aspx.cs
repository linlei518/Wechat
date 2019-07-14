using KDWechat.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.keyworld
{
    public partial class select_menu_list : System.Web.UI.Page
    {
       
        /// <summary>
        ///  外链=6 授权=7 模块=8
        /// </summary>
        protected int channel_id
        {
            get { return RequestHelper.GetQueryInt("channel_id", 0); }
        }
        protected string channel_name = string.Empty;

        protected string _channel_name
        {
            get
            {
                string _name = "";

                switch (channel_id)
                {
                    case 6:
                        _name = "外链";                    
                        break;
                    case 7:
                        _name = "授权";
                        break;
                    case 8:
                        _name = "模块";
                        break;
                    default:
                        JsHelper.RegisterScriptBlock(this, "closeBox();parent.location.replace(parent.location);");
                        break;
                }
                return _name;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            channel_name = _channel_name;
        }
    }
}