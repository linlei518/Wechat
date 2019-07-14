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
    public partial class product_type_list : BasePage
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
            var where = " 1=1 ";

            if (!string.IsNullOrEmpty(key))
            {
                where += "  and type_name like  '%'+@name+'%'  ";
                txtKeywords.Value = key;
            }
            if (status > -1)
            {
                where += "  and is_lock=@status  ";
                ddlStatus.SelectedValue = status.ToString();
            }
            
            var list = DapperConnection.minebea.GetList<t_product_type>(null, where, new {  name = key,product_id= product_id,status= status}, new { sort_id = true,type_id = true }, pageSize, page, out totalCount);

            this.rptList.DataSource = list;
            this.rptList.DataBind();

          
            string pageUrl = $"product_type_list.aspx?status={status}&key={key}&page=__id__" + "&m_id=" + m_id;
            div_page.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);


            //EnumHelper.BindEnumDDLSource(typeof(Enums.Biz_Status), ddlStatus);//绑定产品型号状态

        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect($"product_type_list.aspx?status={ddlStatus.SelectedValue}&key={txtKeywords.Value.Trim()}" + "&m_id=" + m_id+ "&product_id="+ product_id);
        }

        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            var url = $"product_type_list.aspx?status={status}&key={key}" + "&m_id=" + m_id + "&product_id=" + product_id;
            var id = Utils.StrToInt(e.CommandArgument.ToString(), 0);
            switch (e.CommandName)
            {
                case "del":
                    DapperConnection.minebea.DeleteModel<t_product_type>( new { id = id });
                    AddLog($"删除产品型号：{ ((e.Item.FindControl("lblTitle") as Literal).Text)}",LogType.删除);
                    break;
                case "push":
                    if (DapperConnection.minebea.UpdateModel<t_product_type>(new { is_lock = (int)Enums.YesOrNo.是 }, new { type_id = id }))
                    {
                        AddLog(string.Format("发布商品类型：{0}", ((e.Item.FindControl("lblTitle") as Literal).Text)), LogType.修改);
                        JsHelper.AlertAndRedirect("发布商品类型成功！", "product_type_list.aspx?key=" + txtKeywords.Value + "&status=" + status + "&m_id=" + m_id);
                    }
                    else
                    {
                        JsHelper.AlertAndRedirect("发布商品类型失败！", "product_type_list.aspx?key=" + txtKeywords.Value + "&status=" + status + "&m_id=" + m_id);
                    }
                    break;
                case "unpush":
                    if (DapperConnection.minebea.UpdateModel<t_product_type>(new { is_lock = (int)Enums.YesOrNo.否 }, new { type_id = id }))
                    {
                        AddLog(string.Format("下线商品类型：{0}", ((e.Item.FindControl("lblTitle") as Literal).Text)), LogType.修改);
                        JsHelper.AlertAndRedirect("下线商品类型成功！", "product_type_list.aspx?key=" + txtKeywords.Value + "&status=" + status + "&m_id=" + m_id);
                    }
                    else
                    {
                        JsHelper.AlertAndRedirect("下线商品类型失败！", "product_type_list.aspx?key=" + txtKeywords.Value + "&status=" + status + "&m_id=" + m_id);
                    }
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
                DapperConnection.minebea.UpdateModel<t_product_type>(new { sort_id = sortId }, new { id = id });
            }
            AddLog(log_title, LogType.修改); //记录日志
            JsHelper.AlertAndRedirect("保存排序成功！", "product_type_list.aspx?key=" + txtKeywords.Value + "&status=" + status + "&m_id=" + m_id + "&product_id=" + product_id);
        }

       

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                int id = Convert.ToInt32(((HiddenField)rptList.Items[i].FindControl("hidId")).Value);
                CheckBox cb = (CheckBox)rptList.Items[i].FindControl("chkId");
                if (cb.Checked)
                {
                    DapperConnection.minebea.DeleteModel<t_product_type>(new { id = id });
                }
            }
            AddLog("删除产品类型[产品类型]" + id, LogType.删除);
            JsHelper.AlertAndRedirect("删除产品类型信息成功！", "product_type_list.aspx?key=" + txtKeywords.Value + "&status=" + status + "&m_id=" + m_id + "&product_id=" + product_id);
        }

       
    }
}