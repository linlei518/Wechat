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
    public partial class letter_edit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckUserAuthority("head_letter");
        }

        protected void SubmitButtom_Click(object sender, EventArgs e)
        {
         
            int userFlag = Common.Utils.StrToInt(ddlUserFlag.SelectedValue,0);
            int bType = Common.Utils.StrToInt(ddlBusssinessType.SelectedValue, 0);
            int areaType = Common.Utils.StrToInt(ddlAreaType.SelectedValue, 0);

            StringBuilder Query = new StringBuilder();
            Query.Append(string.Format("select id from t_sys_users where id!={0} and status={1}",u_id,1));
            if(userFlag>0)
            {
                Query.Append(" and flag="+userFlag);
            }
            if(bType>0)
            {
                Query.Append(" and type_id="+bType);
            }
            if(areaType>0)
            {
                Query.Append(" and area="+areaType);
            }

            BLL.Users.sys_letter bll = new BLL.Users.sys_letter();
            int[] uidList = bll.GetUserIdList(Query.ToString());

            if (uidList.Length == 1 && uidList[0] == 0)
            {
                Common.JsHelper.AlertAndRedirect("没有符合条件的用户", "letter_edit.aspx?m_id=" + m_id);
            }
            else 
            {
                DAL.t_sys_letter model = new DAL.t_sys_letter();
                model.title = txtTitle.Text.Trim();
                model.contents = txtContents.InnerText.Trim();
                model.u_id = u_id;
                model.create_time = DateTime.Now;
               int res= BLL.Users.sys_letter.PostLetter(model,uidList);
               if (res > 0)
               {
                   AddLog("发送站内信:" + txtTitle.Text.Trim() + "", Common.LogType.添加);
                   Common.JsHelper.AlertAndRedirect("站内信发送成功"+res+"人", "letter_edit.aspx?m_id=" + m_id);
               }
               else 
               {
                   Common.JsHelper.AlertAndRedirect("站内信发送失败", "letter_edit.aspx?m_id=" + m_id);
               }
            }

        }

        //protected void CancelButton_Click(object sender, EventArgs e)
        //{

        //}
    }
}