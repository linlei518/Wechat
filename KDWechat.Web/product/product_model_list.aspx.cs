using System;
using System.Drawing.Imaging;
using System.IO;
using System.Web.UI.WebControls;
using KDWechat.Common;
using KDWechat.DAL;
using KDWechat.Web.UI;
using QuickMark;

namespace KDWechat.Web.product
{
    public partial class product_model_list : BasePage
    {
        public int product_id = RequestHelper.GetQueryInt("product_id", 0);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckUserAuthority("product_list");
                BindList();
            }
        }


        private void BindList()
        {
            txtKeywords.Value = key;
            var where = " product_id=@product_id ";

            if (!string.IsNullOrEmpty(key))
            {
                where += "  and type_name like  '%'+@name+'%'  ";
                txtKeywords.Value = key;
            }
            if (status > -1)
            {
                where += "  and is_publish=@status  ";
                ddlStatus.SelectedValue = status.ToString();
            }
            
            var list = DapperConnection.minebea.GetList<t_product_model>(null, where, new {  name = key,product_id= product_id,status= status}, new { sort_id = true,id = true }, pageSize, page, out totalCount);

            this.rptList.DataSource = list;
            this.rptList.DataBind();

          
            string pageUrl = $"product_model_list.aspx?status={status}&key={key}&page=__id__" + "&m_id=" + m_id + "&product_id=" + product_id;
            div_page.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);


            //EnumHelper.BindEnumDDLSource(typeof(Enums.Biz_Status), ddlStatus);//绑定产品型号状态

        }


      


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect($"product_model_list.aspx?status={ddlStatus.SelectedValue}&key={txtKeywords.Value.Trim()}" + "&m_id=" + m_id+ "&product_id="+ product_id);
        }

        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            var url = $"product_model_list.aspx?status={status}&key={key}" + "&m_id=" + m_id + "&product_id=" + product_id;
            var id = Utils.StrToInt(e.CommandArgument.ToString(), 0);
            switch (e.CommandName)
            {
                case "del":
                    DapperConnection.minebea.DeleteModel<t_product_model>( new { id = id });
                    AddLog($"删除产品型号：{ ((e.Item.FindControl("lblTitle") as Literal).Text)}",LogType.删除);
                    break;
            }
            Response.Redirect(url);
        }


        //保存排序
        protected void btnSort_Click(object sender, EventArgs e)
        {
            string log_title = "保存产品型号信息排序";
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                int id = Convert.ToInt32(((HiddenField)rptList.Items[i].FindControl("hidId")).Value);
                int sortId;
                if (!int.TryParse(((TextBox)rptList.Items[i].FindControl("txtSortId")).Text.Trim(), out sortId))
                {
                    sortId = 99;
                }
                DapperConnection.minebea.UpdateModel<t_product_model>(new { sort_id = sortId }, new { id = id });
            }
            AddLog(log_title, LogType.修改); //记录日志
            JsHelper.AlertAndRedirect("保存排序成功！", "product_model_list.aspx?key=" + txtKeywords.Value + "&status=" + status + "&m_id=" + m_id + "&product_id=" + product_id);
        }

       

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                int id = Convert.ToInt32(((HiddenField)rptList.Items[i].FindControl("hidId")).Value);
                CheckBox cb = (CheckBox)rptList.Items[i].FindControl("chkId");
                if (cb.Checked)
                {
                    DapperConnection.minebea.DeleteModel<t_product_model>(new { id = id });
                }
            }
            AddLog("删除产品型号[产品型号]" + id, LogType.删除);
            JsHelper.AlertAndRedirect("删除产品型号信息成功！", "product_model_list.aspx?key=" + txtKeywords.Value + "&status=" + status + "&m_id=" + m_id + "&product_id=" + product_id);
        }

       
    }
}