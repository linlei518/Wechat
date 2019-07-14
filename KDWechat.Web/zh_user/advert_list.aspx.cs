using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using ICSharpCode.SharpZipLib.Zip;
using KDWechat.Common;
using KDWechat.DAL;
using KDWechat.Web.UI;
using QuickMark;

namespace KDWechat.Web.zh_user
{
    public partial class advert_list : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //CheckUserAuthority("advert_list");
                BindList();
            }
        }


        private void BindList()
        {
            txtKeywords.Value = key;
            var where = " where 1=1  ";

            if (!string.IsNullOrEmpty(key))
            {
                where += "  and title like  '%'+@name+'%'  ";
                txtKeywords.Value = key;
            }
          
            if (status > -1)
            {
                where += "  and status=@status  ";
                ddlStatus.SelectedValue = status.ToString();
            }
            
            var list = DapperConnection.minebea.GetListBySql<t_zh_advert>(" select * from t_zh_advert " + where, " id desc ", pageSize, page, out totalCount, new { name = key, status = status });
           
            this.rptList.DataSource = list;
            this.rptList.DataBind();

          
            string pageUrl = $"advert_list.aspx?status={status}&key={key}&page=__id__&m_id=" +m_id;
         
            div_page.InnerHtml = Utils.OutPageList(pageSize, page, totalCount, pageUrl, 8);


            EnumHelper.BindEnumDDLSource(typeof(Enums.DataLockStatus), ddlStatus);//发布状态

        }


        public string get_imgs(string imgs_url)
        {
            var str = "";
            foreach (var item in imgs_url.Split('|'))
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    str += "<img src='" + RequestHelper.GetHost() + ":8011" + item + "' width='160px' height='80px' onclick='dialogue.dlShowPic(\"" + RequestHelper.GetHost() + ":8011"  + item + "\")' /> ";
                }
            }
            return str;
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect($"advert_list.aspx?status={ddlStatus.SelectedValue}&key={txtKeywords.Value.Trim()}" + "&m_id=" + m_id);
        }

        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            var url = $"advert_list.aspx?status={status}&key={key}" + "&m_id=" + m_id;
            var id = Utils.StrToInt(e.CommandArgument.ToString(), 0);
            switch (e.CommandName)
            {
                case "del":
                    if (DapperConnection.minebea.DeleteModel<t_zh_advert>(new { id = id }))
                    {
                        AddLog(String.Format("删除广告位：{0}", ((e.Item.FindControl("lblTitle") as Literal).Text)), LogType.删除);
                        JsHelper.AlertAndRedirect("删除广告位成功！", "advert_list.aspx?key=" + txtKeywords.Value + "&status=" + status+ "&m_id = " +m_id);return;
                    }
                    break;
                case "push":
                    if (DapperConnection.minebea.UpdateModel<t_zh_advert>(new { status = (int)Enums.DataLockStatus.启用 }, new { id = id }))
                    {
                        AddLog(string.Format("发布广告位：{0}", ((e.Item.FindControl("lblTitle") as Literal).Text)), LogType.修改);
                        JsHelper.AlertAndRedirect("发布广告位成功！", "advert_list.aspx?key=" + txtKeywords.Value + "&status=" + status + "&m_id=" + m_id); return;
                    }
                    else
                    {
                        JsHelper.AlertAndRedirect("发布广告位失败！", "advert_list.aspx?key=" + txtKeywords.Value + "&status=" + status + "&m_id=" + m_id); return;
                    }
                    break;
                case "unpush":
                    if (DapperConnection.minebea.UpdateModel<t_zh_advert>(new { status = (int)Enums.DataLockStatus.禁用 }, new { id = id }))
                    {
                        AddLog(string.Format("下线广告位：{0}", ((e.Item.FindControl("lblTitle") as Literal).Text)), LogType.修改);
                        JsHelper.AlertAndRedirect("下线广告位成功！", "advert_list.aspx?key=" + txtKeywords.Value + "&status=" + status + "&m_id=" + m_id); return;
                    }
                    else
                    {
                        JsHelper.AlertAndRedirect("下线广告位失败！", "advert_list.aspx?key=" + txtKeywords.Value + "&status=" + status + "&m_id=" + m_id); return;
                    }
                    break;
            }
            Response.Redirect(url);
        }


        //保存排序
        protected void btnSort_Click(object sender, EventArgs e)
        {
            string log_title = "保存产品信息排序";
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                int id = Convert.ToInt32(((HiddenField)rptList.Items[i].FindControl("hidId")).Value);
                int sortId;
                if (!int.TryParse(((TextBox)rptList.Items[i].FindControl("txtSortId")).Text.Trim(), out sortId))
                {
                    sortId = 99;
                }
                DapperConnection.minebea.UpdateModel<t_product>(new { sort_id = sortId }, new { product_id = id });
            }
            AddLog(log_title, LogType.修改); //记录日志
            JsHelper.AlertAndRedirect("保存排序成功！", "advert_list.aspx?key=" + txtKeywords.Value + "&status=" + status + "&m_id=" + m_id);
        }

        //提交审核
        protected void btnAudit_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                int id = Convert.ToInt32(((HiddenField)rptList.Items[i].FindControl("hidId")).Value);
                CheckBox cb = (CheckBox)rptList.Items[i].FindControl("chkId");
                if (cb.Checked)
                {
                    DapperConnection.minebea.ExecuteSql("update t_product set status=@status where product_id=@product_id", new { product_id = id, status=Enums.Biz_Status.已审核 });
                }
            }
            AddLog("审核产品[产品ID]" + id, LogType.修改); //记录日志
            JsHelper.AlertAndRedirect("审核产品信息成功！", "advert_list.aspx?key=" + txtKeywords.Value + "&status=" + status + "&m_id=" + m_id);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                int id = Convert.ToInt32(((HiddenField)rptList.Items[i].FindControl("hidId")).Value);
                CheckBox cb = (CheckBox)rptList.Items[i].FindControl("chkId");
                if (cb.Checked)
                {
                    DapperConnection.minebea.DeleteModel<t_product>( new { product_id = id });
                }
            }
            AddLog("删除产品[产品ID]" + id, LogType.删除);
            JsHelper.AlertAndRedirect("删除产品信息成功！", "advert_list.aspx?key=" + txtKeywords.Value + "&status=" + status + "&m_id=" + m_id);
        }

   





    }
}