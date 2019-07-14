using System;
using System.Web.UI.WebControls;
using KDWechat.Common;
using KDWechat.DAL;
using KDWechat.Web.UI;

namespace KDWechat.Web.product
{
    public partial class model_dic_list : BasePage
    {
        public int model_id => RequestHelper.GetQueryInt("model_id", 0);
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
            var where = " model_id=@model_id";

            if (!string.IsNullOrEmpty(key))
            {
                where += "  and model_key like  '%'+@name+'%'  ";
                txtKeywords.Value = key;
            }
           
            //if (status > -1)
            //{
            //    where += "  and status=@status  ";
            //    ddlStatus.SelectedValue = status.ToString();
            //}
            var list = DapperConnection.minebea.GetList<t_product_model_dic>(null, where, new {  name = key, model_id= model_id }, new { sort_id = true,id = true }, pageSize, page, out totalCount);

            this.rptList.DataSource = list;
            this.rptList.DataBind();

           
            string pageUrl = $"model_dic_list.aspx?status={status}&key={key}&model_id={model_id}&page=__id__" + "&m_id=" + m_id;
            div_page.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);


            //EnumHelper.BindEnumDDLSource(typeof(Enums.Biz_Status), ddlStatus);//绑定积分商品状态

        }


     

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect($"model_dic_list.aspx?key={txtKeywords.Value.Trim()}" + "&m_id=" + m_id);
        }

        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            var url = $"model_dic_list.aspx?status={status}&key={key}&model_id={model_id}" + "&m_id=" + m_id;
            var id = Utils.StrToInt(e.CommandArgument.ToString(), 0);
            switch (e.CommandName)
            {
                case "del":
                    DapperConnection.minebea.UpdateModel<t_product_model_dic>(new { status =(int)Enums.Biz_Status.已删除 }, new { id = id });
                    AddLog($"删除积分商品：{ ((e.Item.FindControl("lblTitle") as Literal).Text)}", LogType.删除);
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
                DapperConnection.minebea.UpdateModel<t_product_model_dic>(new { sort_id = sortId }, new { id = id });
            }
            AddLog(log_title,LogType.修改); //记录日志
            JsHelper.AlertAndRedirect("保存排序成功！", "model_dic_list.aspx?key=" + txtKeywords.Value + "&model_id="+model_id + "&m_id=" + m_id);
        }

       

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                int id = Convert.ToInt32(((HiddenField)rptList.Items[i].FindControl("hidId")).Value);
                CheckBox cb = (CheckBox)rptList.Items[i].FindControl("chkId");
                if (cb.Checked)
                {
                    DapperConnection.minebea.DeleteModel<t_product_model_dic>(new { id = id });
                }
            }
            AddLog("删除积分商品分类[积分商品分类ID]" + id,LogType.删除);
            JsHelper.AlertAndRedirect("删除积分商品分类信息成功！", "model_dic_list.aspx?key=" + txtKeywords.Value + "&model_id=" + model_id + "&m_id=" + m_id);
        }

    }
}