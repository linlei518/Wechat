using KDWechat.Common;
using KDWechat.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
namespace KDWechat.Web.Account
{
    public partial class letter_list_rec : BasePage
    {

        protected int mid { get { return RequestHelper.GetInt("m_id", 0); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                BindData();
            }
        }

     
        /// <summary>
        /// 绑定数据
        /// </summary>
        protected void BindData() 
        {
            StringBuilder Query = new StringBuilder();
            Query.Append(string.Format("select le.id as id, le.title,le.contents,le.create_time,lr.id as lrId,lr.u_id,lr.status from t_sys_letter le,t_sys_letter_receiver lr where lr.l_id=le.id and lr.status!="+(int)LetterType.删除+" and lr.u_id='{0}'", u_id));

            repItem.DataSource = GetPageList(DbDataBaseEnum.KD_USERS, Query.ToString(), pageSize, page, "*", "id desc", ref totalCount);
            repItem.DataBind();
            string pageUrl = string.Format("letter_list_rec.aspx?m_id={0}&page=__id__", m_id);
            div_page.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        protected void repItem_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                int id = int.Parse(e.CommandArgument.ToString());
                if (BLL.Users.sys_letter.DeleteByRec(id)>0)
                {
                    string name = (e.Item.FindControl("lblTitle") as Literal).Text;
                    AddLog(string.Format("删除站内信：{0}", name), LogType.删除);
                    JsHelper.AlertAndRedirect("删除成功", "letter_list_rec.aspx?m_id=" + m_id + "&page=" + page);
                }
                else
                    JsHelper.Alert(Page, "删除失败", "true");
            }
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string ids="";
            string name = "";
            for (int i = 0; i < repItem.Items.Count; i++)
            {
                int id = Convert.ToInt32(((HiddenField)repItem.Items[i].FindControl("hidId")).Value);
                
                CheckBox cb = (CheckBox)repItem.Items[i].FindControl("chkId");
                if (cb.Checked)
                {
                    name += ((Literal)repItem.Items[i].FindControl("lblTitle")).Text+",";
                    if (BLL.Users.sys_letter.DeleteByRec(id) > 0)
                    {
                        ids += id.ToString() + ",";
                    }
                    else
                        JsHelper.Alert(Page,"删除失败","true");
                }
               
            }
            if (!string.IsNullOrEmpty(ids))
            {
                ids = ids.Remove(ids.Length - 1, 1);
                AddLog(string.Format("删除站内信：{0}",name.TrimEnd(',')), LogType.删除);
                JsHelper.AlertAndRedirect("删除成功", "letter_list_rec.aspx?m_id=" + m_id+"&page="+page);
            }
        }
    }
}