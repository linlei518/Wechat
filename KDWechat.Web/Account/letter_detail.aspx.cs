using KDWechat.Common;
using KDWechat.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.Account
{
    public partial class letter_detail : BasePage
    {
        protected int id { get { return RequestHelper.GetInt("id", 0); } }
        protected int lr_id { get { return RequestHelper.GetInt("lr_id", 0); } }
        protected int mid { get { return RequestHelper.GetInt("m_id", 0); } }
        protected string title;
        protected string time;
        protected string contents;
        protected string ReturlUrl;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(this.id>0)
            {
                BindData(this.id);
                if (lr_id > 0)
                {
                    ReturlUrl = "letter_list_rec.aspx?m_id=" + mid;
                    UpdateRead(lr_id);
                }
                else 
                {
                    ReturlUrl = "letter_list.aspx?m_id=" + mid;
                }
            }
        }
       
        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="id"></param>
        protected void BindData(int id)
        {
           DAL.t_sys_letter model= BLL.Users.sys_letter.GetModel(id);
            if(model!=null)
            {
                title = model.title;
                time = Utils.ObjectToDateTime(model.create_time).ToString("yyyy/MM/dd HH:mm");
                contents = model.contents;
            }
        }
        /// <summary>
        /// 设置已读
        /// </summary>
        /// <param name="lr_id"></param>
        protected void UpdateRead(int lr_id) 
        {
            BLL.Users.sys_letter.SetRead(lr_id,u_id);
        }
    }
}