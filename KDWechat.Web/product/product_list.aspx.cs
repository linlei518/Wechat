using System;
using System.Collections;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Web;
using System.Web.UI.WebControls;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using KDWechat.Common;
using KDWechat.DAL;
using KDWechat.Web.UI;
using QuickMark;

namespace KDWechat.Web.product
{
    public partial class product_list : BasePage
    {
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
            var where = " where 1=1  ";

            if (!string.IsNullOrEmpty(key))
            {
                where += "  and product_name like  '%'+@name+'%'  ";
                txtKeywords.Value = key;
            }
          
            if (status > -1)
            {
                where += "  and is_publish=@status  ";
                ddlStatus.SelectedValue = status.ToString();
            }
            
            var list = DapperConnection.minebea.GetListBySql<product_list_model>("select *,(select type_name from t_product_type where type_id=t_product.type_id) type_name from t_product  " + where, " sort_id asc,product_id desc ", pageSize, page, out totalCount, new { name = key, status = status });
            list.ForEach(x=>x.qr_code=CreateQrCode("http://61.129.33.240:88/product_info?id=" + x.product_id, x.product_name + x.product_id));
            this.rptList.DataSource = list;
            this.rptList.DataBind();

          
            string pageUrl = $"product_list.aspx?status={status}&key={key}&page=__id__&m_id=" +m_id;
         
            div_page.InnerHtml = Utils.OutPageList(pageSize, page, totalCount, pageUrl, 8);


            EnumHelper.BindEnumDDLSource(typeof(Enums.YesOrNo), ddlStatus);//发布状态

        }


        //设置分页数量
        //protected void txtPageNum_TextChanged(object sender, EventArgs e)
        //{
        //    int _pagesize;
        //    if (int.TryParse(txtPageNum.Text.Trim(), out _pagesize))
        //    {
        //        if (_pagesize > 0)
        //        {
        //            Utils.WriteCookie(page_size_cook, _pagesize.ToString(), 43200);
        //        }
        //    }
        //    Response.Redirect($"product_list.aspx?status={status}&key={key}");
        //}


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect($"product_list.aspx?status={ddlStatus.SelectedValue}&key={txtKeywords.Value.Trim()}" + "&m_id=" + m_id);
        }

        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            var url = $"product_list.aspx?status={status}&key={key}" + "&m_id=" + m_id;
            var id = Utils.StrToInt(e.CommandArgument.ToString(), 0);
            switch (e.CommandName)
            {
                case "del":
                    if (DapperConnection.minebea.DeleteModel<t_product>(new { product_id = id }))
                    {
                        AddLog(String.Format("删除商品：{0}", ((e.Item.FindControl("lblTitle") as Literal).Text)), LogType.删除);
                        JsHelper.AlertAndRedirect("删除商品成功！", "product_list.aspx?key=" + txtKeywords.Value + "&status=" + status);
                    }
                    else
                    {
                        JsHelper.AlertAndRedirect("删除商品失败！", "product_list.aspx?key=" + txtKeywords.Value + "&status=" + status+"&m_id=" + m_id);
                    }
                    break;
                case "push":
                    if (DapperConnection.minebea.UpdateModel<t_product>(new { is_publish = (int)Enums.YesOrNo.是 }, new { product_id = id }))
                    {
                        AddLog(string.Format("发布商品：{0}", ((e.Item.FindControl("lblTitle") as Literal).Text)), LogType.修改);
                        JsHelper.AlertAndRedirect("发布商品成功！", "product_list.aspx?key=" + txtKeywords.Value + "&status=" + status + "&m_id=" + m_id); 
                    }
                    else
                    {
                        JsHelper.AlertAndRedirect("发布商品失败！", "product_list.aspx?key=" + txtKeywords.Value + "&status=" + status + "&m_id=" + m_id);
                    }
                    break;
                case "unpush":
                    if (DapperConnection.minebea.UpdateModel<t_product>(new { is_publish = (int)Enums.YesOrNo.否 }, new { product_id = id }))
                    {
                        AddLog(string.Format("下线商品：{0}", ((e.Item.FindControl("lblTitle") as Literal).Text)), LogType.修改);
                        JsHelper.AlertAndRedirect("下线商品成功！", "product_list.aspx?key=" + txtKeywords.Value + "&status=" + status + "&m_id=" + m_id);
                    }
                    else
                    {
                        JsHelper.AlertAndRedirect("下线商品失败！", "product_list.aspx?key=" + txtKeywords.Value + "&status=" + status + "&m_id=" + m_id);
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
            JsHelper.AlertAndRedirect("保存排序成功！", "product_list.aspx?key=" + txtKeywords.Value + "&status=" + status + "&m_id=" + m_id);
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
            JsHelper.AlertAndRedirect("审核产品信息成功！", "product_list.aspx?key=" + txtKeywords.Value + "&status=" + status + "&m_id=" + m_id);
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
            JsHelper.AlertAndRedirect("删除产品信息成功！", "product_list.aspx?key=" + txtKeywords.Value + "&status=" + status + "&m_id=" + m_id);
        }

        private class  product_list_model:t_product
        {
            public string type_name { get; set; }

            public string qr_code { get; set; }
        }



        /// <summary>
        /// 生成图文二维码
        /// </summary>
        /// <param name="link_url">二维码页面地址,都是外链</param>
        /// <param name="img_name">生成的二维码图片的名称</param>
        /// <param name="is_cover">是否覆盖之前</param>
        /// <returns></returns>
        public string CreateQrCode(string link_url, string img_name, bool is_cover = false)
        {
            string img_url = "/qr_codes/" + img_name + ".png";
            if (is_cover)
            {
                try { File.Delete(Server.MapPath(img_url)); }
                catch (Exception) { }
            }
            if (!File.Exists(Server.MapPath(img_url)))
            {
                QuickMark.CreateTwoCode ctc = new QuickMark.CreateTwoCode();
                string link = link_url;
                using (var bitmap = ctc.CreateCode(link, CreateTwoCode.CodeType.Byte, CreateTwoCode.Correct.M, 0, 15))
                {
                    if (bitmap != null)
                    {
                        //检查上传的物理路径是否存在，不存在则创建
                        if (!Directory.Exists(Server.MapPath("/qr_codes/")))
                            Directory.CreateDirectory(Server.MapPath("/qr_codes/"));
                        bitmap.Save(Server.MapPath(img_url), ImageFormat.Png);
                    }
                }


            }
            return img_url;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            dlZipDir(Server.MapPath("/qr_codes"),"qr_code_zip");

        }






        ZipOutputStream zos = null;
        String strBaseDir = "";

       public void dlZipDir(string strPath, string strFileName)
        {
            MemoryStream ms = null;
            Response.ContentType = "application/octet-stream";
            strFileName = HttpUtility.UrlEncode(strFileName).Replace("+","");
            Response.AddHeader("Content-Disposition", "attachment;   filename=" + strFileName + ".zip");
            ms = new MemoryStream();
            zos = new ZipOutputStream(ms);
            strBaseDir = strPath + "";
            addZipEntry(strBaseDir);
            zos.Finish();
            zos.Close();
            Response.Clear();
            Response.BinaryWrite(ms.ToArray());
            Response.End();
        }

        public void addZipEntry(string PathStr)
        {
            DirectoryInfo di = new DirectoryInfo(PathStr);
            foreach (DirectoryInfo item in di.GetDirectories())
            {
                addZipEntry(item.FullName);
            }
            foreach (FileInfo item in di.GetFiles())
            {
                FileStream fs = File.OpenRead(item.FullName);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                string strEntryName = item.FullName.Replace(strBaseDir, "");
                ZipEntry entry = new ZipEntry(strEntryName);
                zos.PutNextEntry(entry);
                zos.Write(buffer, 0, buffer.Length);
                fs.Close();
            }
        }


    }
}