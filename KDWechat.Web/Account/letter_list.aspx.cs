using KDWechat.Common;
using KDWechat.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.Account
{
    public partial class letter_list : BasePage
    {
        protected int uid { get { return u_id; } }
        protected int pagesize { get { return 10; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckUserAuthority("head_letter");
            if(!IsPostBack)
            {
                BindData();
            }
        }

        private void SetRefferUrl()
        {
            try
            {
                hfReturlUrl.Value = Request.UrlReferrer.ToString();
            }
            catch (Exception)
            {
                hfReturlUrl.Value = "letter_list.aspx?m_id=" + m_id;
            }
        }
        /// <summary>
        /// 绑定数据
        /// </summary>
        protected void BindData() 
        {
            var list = BLL.Users.sys_letter.GetListByUserId(uid, pagesize, page, out totalCount);
            Repeater1.DataSource = list;

            string url = "letter_list.aspx?page=__id__&m_id=" + m_id;
            div_page.InnerHtml = Utils.OutPageList(pagesize, page, totalCount, url, 8);
            Repeater1.DataBind();
        }
        /// <summary>
        /// 删除站内信
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string[] estr = e.CommandArgument.ToString().Split(',');
            int id = int.Parse(estr[0]);
            string title = estr[1];

            if (e.CommandName == "del")
            {
               
                if (BLL.Users.sys_letter.Delete(id))
                {
                    AddLog(string.Format("删除站内信:{0}",title),LogType.删除);
                    JsHelper.AlertAndRedirect("删除成功", "letter_list.aspx?page="+page+"&m_id="+m_id);
                }
                else
                    JsHelper.Alert("删除失败");
            }
        }
        /// <summary>
        /// 获取接收人用户名
        /// </summary>
        /// <param name="l_id"></param>
        protected string GetUsers(object l_id)
        {
            string res = "";

            int lid=Utils.ObjToInt(l_id,0);
            StringBuilder Query = new StringBuilder();
            Query.Append(string.Format("select u_id from t_sys_letter_receiver where l_id={0}",lid));

             BLL.Users.sys_letter bll = new BLL.Users.sys_letter();
             int[] uidList = bll.GetUserIdListFromRec(Query.ToString());

             if (uidList.Length == 1 && uidList[0] == 0)
             {
                 res = "无";
             }
             else
             {
                 return uidList.Length.ToString() + "人";
               
             } 
            
            return res;
        }
    }
}