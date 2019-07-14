using KDWechat.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.Account
{
    public partial class latter_account : Web.UI.BasePage
    {
        protected int id
        {
            get { return RequestHelper.GetQueryInt("id", 0); }
        }
        protected int pagesize1 { get { return 10; } }
       

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                InitData();
            }
        }

       

        private void InitData()
        {
            StringBuilder Query = new StringBuilder();
            Query.Append(string.Format("select u_id from t_sys_letter_receiver where l_id={0}", this.id));

            BLL.Users.sys_letter bll = new BLL.Users.sys_letter();
            int[] uidList = bll.GetUserIdListFromRec(Query.ToString());
            int len = uidList.Length;

            string strIds = string.Empty;

            if(len>0)
            {
                for (int i = 0; i < len;i++ )
                {
                    strIds += uidList[i].ToString() + ",";
                }
            }

            if(!string.IsNullOrEmpty(strIds))
            {
                strIds = strIds.Remove(strIds.Length-1,1);

                string queryUser = "select id,user_name,real_name,status,login_time from t_sys_users where id in ("+strIds+")";
                repItem.DataSource = GetPageList(DbDataBaseEnum.KD_USERS, queryUser, pagesize1, page, "*", "id desc", ref totalCount);
                repItem.DataBind();
                string pageUrl = string.Format("latter_account.aspx?id={0}&page=__id__", this.id);
                div_page.InnerHtml = Utils.OutPageList(this.pagesize1, this.page, this.totalCount, pageUrl, 8);
            }
        }

      
    }
}