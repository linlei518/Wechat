using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using KDWechat.Common;
using KDWechat.DAL;
using KDWechat.Web.UI;

namespace KDWechat.Web.activity
{
    public partial class contact_list : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckUserAuthority("contact_list");
                BindList();
            }
        }


        private void BindList()
        {
            txtKeywords.Value = key;
            var where = " 1=1 ";

            if (!string.IsNullOrEmpty(key))
            {
                where += "  and company_name like  '%'+@name+'%'  ";
                txtKeywords.Value = key;
            }
            //if (status > -1)
            //{
            //    where += "  and type=@status  ";
            //    ddlStatus.SelectedValue = status.ToString();
            //}
            //if (status > -1)
            //{
            //    where += "  and status=@status  ";
            //    ddlStatus.SelectedValue = status.ToString();
            //}
            var list = DapperConnection.minebea.GetList<t_contact>(null, where, new { name = key, status = status }, new { create_date = false}, pageSize, page, out totalCount);

            this.rptList.DataSource = list;
            this.rptList.DataBind();

          
            string pageUrl = $"contact_list.aspx?status={status}&key={key}&page=__id__" + "&m_id=" + m_id;
            div_page.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);



        }


     

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect($"contact_list.aspx?key={txtKeywords.Value.Trim()}" + "&m_id=" + m_id);
        }

        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            var url = $"contact_list.aspx?status={status}&key={key}" + "&m_id=" + m_id;
            var id = Utils.StrToInt(e.CommandArgument.ToString(), 0);
            switch (e.CommandName)
            {
                case "del":
                    DapperConnection.minebea.DeleteModel<t_contact>( new { id = id });
                    AddLog($"删除咨询信息：{id}", LogType.删除);
                    break;
            }
            Response.Redirect(url);
        }


        //保存排序
        protected void btnSort_Click(object sender, EventArgs e)
        {
            string log_title = "保存积分商品信息排序";
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                int id = Convert.ToInt32(((HiddenField)rptList.Items[i].FindControl("hidId")).Value);
                int sortId;
                if (!int.TryParse(((TextBox)rptList.Items[i].FindControl("txtSortId")).Text.Trim(), out sortId))
                {
                    sortId = 99;
                }
                DapperConnection.minebea.UpdateModel<t_contact>(new { sort_id = sortId }, new { type_id = id });
            }
            AddLog(log_title, LogType.修改); //记录日志
            JscriptMsg("保存排序成功！", "contact_list.aspx?key=" + txtKeywords.Value + "&status=" + status + "&m_id=" + m_id);
        }



        protected void btnDelete_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                int id = Convert.ToInt32(((HiddenField)rptList.Items[i].FindControl("hidId")).Value);
                CheckBox cb = (CheckBox)rptList.Items[i].FindControl("chkId");
                if (cb.Checked)
                {
                    DapperConnection.minebea.ExecuteSql("delete from  t_contact  where id=@id", new { id = id  });
                }
            }
            AddLog("删除咨询信息[咨询信息ID]" + id, LogType.删除);
            JscriptMsg("删除咨询信息信息成功！", "contact_list.aspx?key=" + txtKeywords.Value + "&status=" + status + "&m_id=" + m_id);
        }


        protected void btnExport_Click(object sender, EventArgs e)
        {
            txtKeywords.Value = key;
            var where = " where 1=1 ";

            if (!string.IsNullOrEmpty(key))
            {
                where += "  and company_name like  '%'+@name+'%'  ";
                txtKeywords.Value = key;
            }
            //if (status > -1)
            //{
            //    where += "  and type=@status  ";
            //    ddlStatus.SelectedValue = status.ToString();
            //}
           var list = DapperConnection.minebea.GetListBySql<export_contact>("select * from t_contact "+ where ,"  id desc ", 10000, 1, out totalCount);
            Dictionary<string, string> titles = new Dictionary<string, string>();

            titles.Add("content", "咨询内容");
            titles.Add("product_menu", "产品栏目");
            titles.Add("product_name", "产品名称");
            titles.Add("user_name", "姓名");
            titles.Add("company_name", "公司名称");
            titles.Add("phone", "手机号码");
            titles.Add("email", "邮箱");
            titles.Add("dpt_name", "部门");
            titles.Add("address", "地址");
            titles.Add("create_date", "申请时间");
           
            bool isc = GemBoxExcelLiteHelper.SaveExcel<export_contact>(Server.MapPath("/activity/excel/contact_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xls"), this.Page, true, true, titles, list);
        }

        public class export_contact : t_contact
        {
            public string type_name => type == 0 ? "关于产品" : "关于环保";
        }

    }
   
}
