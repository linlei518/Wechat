using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using KDWechat.Common;
using KDWechat.DAL;
using KDWechat.Web.UI;

namespace KDWechat.Web.activity
{
    public partial class activity_apply_list : BasePage
    {

        /// <summary>
        /// 起始时间
        /// </summary>
        public string beginDate
        {
            get { return RequestHelper.GetQueryString("beginDate"); }
        }


        /// <summary>
        ///结束时间
        /// </summary>
        public string endDate
        {
            get { return RequestHelper.GetQueryString("endDate"); }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckUserAuthority("activity_apply_list");
                BindList();
            }
        }


        private void BindList()
        {
            txtKeywords.Value = key;
            var where = new StringBuilder();
            where.Append(" 1=1 ");
            if (!string.IsNullOrEmpty(key))
            {
                where.Append("  and phone like  '%'+@phone+'%'  ");
                txtKeywords.Value = key;
            }

            if (beginDate.Trim() != "" && endDate.Trim() != "")
            {
                where.Append(" and convert(varchar(10),create_date,120) >= @beginDate and convert(varchar(10),create_date,120)<=@endDate ");
                txt_date_show.Value = beginDate + " — " + endDate;
            }
            else if (beginDate.Trim() != "")
            {
                where.Append(" and create_date between @beginDate  and getdate()");
                txt_date_show.Value = beginDate;
            }
            else if (endDate.Trim() != "")
            {
                where.Append(" and convert(varchar(10),create_date) <=@endDate ");
                txt_date_show.Value = endDate;
            }
            if (status > -1)
            {
                if (status == 0)
                {
                    where.Append(" and prize_name is not null ");
                }
                else
                {
                    where.Append(" and prize_name is null ");
                }

                ddlStatus.SelectedValue = status.ToString();
            }
            var list = DapperConnection.temp_minebea.GetList<t_draw_list>(null, where.ToString(), new { phone = key, status = status, endDate= endDate, beginDate= beginDate }, new { create_date = false}, pageSize, page, out totalCount);

            this.rptList.DataSource = list;
            this.rptList.DataBind();

          
            string pageUrl = $"activity_apply_list.aspx?status={status}&key={key}&page=__id__" + "&m_id=" + m_id +"&beginDate=" +beginDate+"&endDate=" + endDate;
            div_page.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);



        }


     

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect($"activity_apply_list.aspx?key={txtKeywords.Value.Trim()}&status={ddlStatus.SelectedValue}" + "&m_id=" + m_id + "&beginDate=" + beginDate + "&endDate=" + endDate);
        }

        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            var url = $"activity_apply_list.aspx?status={status}&key={key}" + "&m_id=" + m_id + "&beginDate=" + beginDate + "&endDate=" + endDate;
            var id = Utils.StrToInt(e.CommandArgument.ToString(), 0);
            switch (e.CommandName)
            {
                case "del":
                    DapperConnection.temp_minebea.DeleteModel<t_draw_list>( new { id = id });
                    AddLog($"删除报名信息：{ ((e.Item.FindControl("lblTitle") as Literal).Text)}", LogType.删除);
                    break;
                case "checked":
                    var model = DapperConnection.temp_minebea.GetModel<t_draw_list>(new {id = id});
                    model.status = 1;
                    DapperConnection.temp_minebea.UpdateModel<t_draw_list>(model);
                    AddLog($"修改报名信息：{ ((e.Item.FindControl("lblTitle") as Literal).Text)}", LogType.修改);
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
                DapperConnection.temp_minebea.UpdateModel<t_invitation>(new { sort_id = sortId }, new { type_id = id });
            }
            AddLog(log_title, LogType.修改); //记录日志
            JscriptMsg("保存排序成功！", "activity_apply_list.aspx?key=" + txtKeywords.Value + "&status=" + status + "&m_id=" + m_id + "&beginDate=" + beginDate + "&endDate=" + endDate);
        }



        protected void btnDelete_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                int id = Convert.ToInt32(((HiddenField)rptList.Items[i].FindControl("hidId")).Value);
                CheckBox cb = (CheckBox)rptList.Items[i].FindControl("chkId");
                if (cb.Checked)
                {
                    DapperConnection.temp_minebea.ExecuteSql("delete from  t_invitation  where id=@id", new { id = id  });
                }
            }
            AddLog("删除报名信息[报名信息ID]" + id, LogType.删除);
            JscriptMsg("删除报名信息信息成功！", "activity_apply_list.aspx?key=" + txtKeywords.Value + "&status=" + status + "&m_id=" + m_id + "&beginDate=" + beginDate + "&endDate=" + endDate);
        }


        protected void btnExport_Click(object sender, EventArgs e)
        {
            var where = new StringBuilder();
            where.Append(" 1=1 ");
            if (!string.IsNullOrEmpty(key))
            {
                where.Append("  and phone like  '%'+@phone+'%'  ");
                txtKeywords.Value = key;
            }

            if (beginDate.Trim() != "" && endDate.Trim() != "")
            {
                where.Append(" and convert(varchar(10),create_date,120) >= @beginDate and convert(varchar(10),create_date,120)<=@endDate ");
                txt_date_show.Value = beginDate + " — " + endDate;
            }
            else if (beginDate.Trim() != "")
            {
                where.Append(" and create_date between @beginDate  and getdate()");
                txt_date_show.Value = beginDate;
            }
            else if (endDate.Trim() != "")
            {
                where.Append(" and convert(varchar(10),create_date) <=@endDate ");
                txt_date_show.Value = endDate;
            }
            if (status > -1)
            {
                if (status == 0)
                {
                    where.Append(" and prize_name is not null ");
                }
                else
                {
                    where.Append(" and prize_name is null ");
                }

                ddlStatus.SelectedValue = status.ToString();
            }

            var list = DapperConnection.temp_minebea.GetList<t_draw_list>(null, where.ToString(), new { phone = key, status = status, endDate = endDate, beginDate = beginDate }, new { create_date = false }, 10000, page, out totalCount);
            Dictionary<string, string> titles = new Dictionary<string, string>();
            titles.Add("nick_name", "昵称");
            titles.Add("user_name", "姓名");
            titles.Add("phone", "手机号码");
            titles.Add("prize_name", "奖品");
            titles.Add("create_date", "报名时间");

            bool isc = Common.GemBoxExcelLiteHelper.SaveExcel<t_draw_list>(Server.MapPath("/activity/excel/draw_list_" + DateTime.Now.ToString("yyyy_MM_dd") + ".xls"), this.Page, true, true, titles, list);
        }

        protected void btn_checked_Click(object sender, EventArgs e)
        {
            var url = $"activity_apply_list.aspx?status={status}&key={key}" + "&m_id=" + m_id + "&beginDate=" + beginDate + "&endDate=" + endDate;
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                int id = Convert.ToInt32(((HiddenField)rptList.Items[i].FindControl("hidId")).Value);
                CheckBox cb = (CheckBox)rptList.Items[i].FindControl("chkId");
                if (cb.Checked)
                {
                    var model = DapperConnection.temp_minebea.GetModel<t_draw_list>(new { id = id });
                    model.status = 1;
                    DapperConnection.temp_minebea.UpdateModel<t_draw_list>(model);
                }
            }
            JsHelper.AlertAndRedirect("领取成功！", url);
        }
    }
}