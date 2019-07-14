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
    public partial class user_list : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                CheckUserAuthority("user_list");
                BindList();
            }
        }


        private void BindList()
        {
            txtKeywords.Value = key;
            var where = " where 1=1  ";

            if (!string.IsNullOrEmpty(key))
            {
                where += "  and (user_name like  '%'+@name+'%' or plate_number like  '%'+@name+'%' ) ";
                txtKeywords.Value = key;
            }
          
            if (status > -1)
            {
                where += "  and status=@status  ";
                ddlStatus.SelectedValue = status.ToString();
            }
            
            var list = DapperConnection.minebea.GetListBySql<t_zh_user>(" select * from t_zh_user " + where, " id desc ", pageSize, page, out totalCount, new { name = key, status = status });
           
            this.rptList.DataSource = list;
            this.rptList.DataBind();

          
            string pageUrl = $"user_list.aspx?status={status}&key={key}&page=__id__&m_id=" +m_id;
         
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
        //    Response.Redirect($"user_list.aspx?status={status}&key={key}");
        //}


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect($"user_list.aspx?status={ddlStatus.SelectedValue}&key={txtKeywords.Value.Trim()}" + "&m_id=" + m_id);
        }

        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            var url = $"user_list.aspx?status={status}&key={key}" + "&m_id=" + m_id;
            var id = Utils.StrToInt(e.CommandArgument.ToString(), 0);
            switch (e.CommandName)
            {
                case "del":
                    if (DapperConnection.minebea.DeleteModel<t_zh_user>(new { id = id }))
                    {
                        AddLog(String.Format("删除员工：{0}", ((e.Item.FindControl("lblTitle") as Literal).Text)), LogType.删除);
                        JsHelper.AlertAndRedirect("删除员工成功！", "user_list.aspx?key=" + txtKeywords.Value + "&status=" + status+ "&m_id = " +m_id);
                        return;
                    }
                    else
                    {
                        JsHelper.AlertAndRedirect("删除员工失败！", "user_list.aspx?key=" + txtKeywords.Value + "&status=" + status+"&m_id=" + m_id); return;
                    }
                    break;
                case "unbound":
                    if (DapperConnection.minebea.UpdateModel<t_zh_user>(new { open_id = string.Empty, status=0, pwd = DESEncrypt.Encrypt("123456") }, new { id = id }))
                    {
                        AddLog(string.Format("解除用户绑定信息：{0}", ((e.Item.FindControl("lblTitle") as Literal).Text)), LogType.修改);
                        JsHelper.AlertAndRedirect("解除用户绑定信息成功！", "user_list.aspx?key=" + txtKeywords.Value + "&status=" + status + "&m_id=" + m_id); return;
                    }
                    else
                    {
                        JsHelper.AlertAndRedirect("解除用户绑定信息失败！", "user_list.aspx?key=" + txtKeywords.Value + "&status=" + status + "&m_id=" + m_id); return;
                    }
                    break;
                case "unpush":
                    if (DapperConnection.minebea.UpdateModel<t_product>(new { is_publish = (int)Enums.YesOrNo.否 }, new { product_id = id }))
                    {
                        AddLog(string.Format("下线商品：{0}", ((e.Item.FindControl("lblTitle") as Literal).Text)), LogType.修改);
                        JsHelper.AlertAndRedirect("下线商品成功！", "user_list.aspx?key=" + txtKeywords.Value + "&status=" + status + "&m_id=" + m_id);
                    }
                    else
                    {
                        JsHelper.AlertAndRedirect("下线商品失败！", "user_list.aspx?key=" + txtKeywords.Value + "&status=" + status + "&m_id=" + m_id);
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
            JsHelper.AlertAndRedirect("保存排序成功！", "user_list.aspx?key=" + txtKeywords.Value + "&status=" + status + "&m_id=" + m_id);
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
            JsHelper.AlertAndRedirect("审核产品信息成功！", "user_list.aspx?key=" + txtKeywords.Value + "&status=" + status + "&m_id=" + m_id);
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
            JsHelper.AlertAndRedirect("删除产品信息成功！", "user_list.aspx?key=" + txtKeywords.Value + "&status=" + status + "&m_id=" + m_id);
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

            //上传文件的全路径   
            string path = fup.PostedFile.FileName;

            if (!string.IsNullOrWhiteSpace(path))
            {
                //文件全名   
                path = path.Substring(path.LastIndexOf("\\") + 1);
                //文件后缀   
                path = path.Substring(path.LastIndexOf("."));
                //判断excel文件   
                if (path.ToLower() != ".xls" && path.ToLower() != ".xlsx")
                {
                    //JscriptMsg("请选择excel文件", "");
                    return;
                }

                //上传文件新名,防止重名   
                path = DateTime.Now.ToString("yyyyMMddmmssffff") + path;

                //创建excel文件保存路径
                string serverPath = Server.MapPath("zh_user");
                if (!Directory.Exists(serverPath))
                {
                    Directory.CreateDirectory(serverPath); //创建ExcelFiles文件夹   
                }
                //保存路径   
                path = serverPath + "/" + path;
                //保存   
                fup.PostedFile.SaveAs(path);
                List<string> sql_str = new List<string>();
                List<object> sql_param = new List<object>();

                //读取文件 返回数据集合
                var table = GemBoxExcelLiteHelper.InputFromExcel(path, "");

                #region 员工导入

                var list_data = new List<t_zh_user>();



                for (int i = 0; i < table.Rows.Count; i++)
                {
                    var user_tel = table.Rows[i]["手机"].ToString().Trim();
                    if (string.IsNullOrWhiteSpace(table.Rows[i]["手机"].ToString().Trim()))
                    {
                        user_tel = "13800000000";
                    }
                    list_data.Add(new t_zh_user
                    {
                        user_code = table.Rows[i]["员工编号"].ToString().Trim(),
                        user_name = table.Rows[i]["姓名"].ToString().Trim(),
                        user_dpt = table.Rows[i]["部门"].ToString().Trim(),
                        user_mail = table.Rows[i]["邮箱"].ToString().Trim(),
                        user_position = table.Rows[i]["职位"].ToString().Trim(),
                        user_tel = user_tel,
                        plate_number = table.Rows[i]["车牌号码"].ToString().Trim(),
                        status = 0,
                        pwd = DESEncrypt.Encrypt("123456"),
                        create_date = DateTime.Now,
                        create_by = "import data",
                    });
                }

                foreach (var item in list_data)
                {
                    sql_str.Add(DapperHelper.GetInsertParamSql(typeof(t_zh_user), "id"));
                    sql_param.Add(item);
                }


                DapperConnection.minebea.ExecuteTransaction(sql_str, sql_param);


                sql_str = new List<string>();
                 sql_param = new List<object>();

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    var num = table.Rows[i]["车位编号"].ToString().Trim();
                    var user_tel = table.Rows[i]["手机"].ToString().Trim();
                    var plate_number = table.Rows[i]["车牌号码"].ToString().Trim();

                    //if (string.IsNullOrWhiteSpace(table.Rows[i]["手机"].ToString().Trim()))
                    //{
                    //    continue;
                    //}

                    var park_id = 0;
                    var parking_num ="";

                    if (num.Contains("C座地下二层"))
                    {
                        park_id = 3;
                    }
                    else if (num.Contains("C座地下一层"))
                    {
                        park_id = 2;
                    }
                    else if (num.Contains("地下二层"))
                    {
                        park_id = 1;

                    }
                    else if(num.Contains("地下一层"))
                    {
                        park_id = 0;
                    }
                  

                    foreach (var item in num.Split('|'))
                    {
                        if (string.IsNullOrWhiteSpace(item))
                        {
                            continue;
                        }

                       var  num_1 = item.Trim().Replace("C座地下二层", "").Replace("C座地下一层","").Replace("地下二层", "").Replace("地下一层", "");

                        var parking_model = DapperConnection.minebea.GetModel<t_parking>(new { park_id = park_id, parking_num = num_1 });
                        if (parking_model == null)
                        {
                            continue;
                        }
                        var user_model= DapperConnection.minebea.GetModel<t_zh_user>(new { plate_number = plate_number });

                        parking_model.user_code = user_model.user_code;
                        parking_model.user_id = user_model.id;
                        parking_model.user_tel = user_tel;
                        parking_model.plate_number = user_model.plate_number;
                        parking_model.user_name = user_model.user_name;
                        parking_model.updata_date = DateTime.Now;
                        sql_str.Add(DapperHelper.GetUpdateParamSql(typeof(t_parking), "id"));
                        sql_param.Add(parking_model);

                    }
                }
                DapperConnection.minebea.ExecuteTransaction(sql_str, sql_param);



                #endregion


                //删除上传的excel文件。   
                File.Delete(path);
                AddLog("导入员工数据", LogType.添加);
                JsHelper.AlertAndRedirect("导入员工数据成功！", "user_list.aspx?key=" + txtKeywords.Value + "&status=" + status + "&m_id=" + m_id);
            }
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