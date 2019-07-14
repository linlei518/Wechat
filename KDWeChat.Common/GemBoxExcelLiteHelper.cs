using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.Security;
using GemBox.ExcelLite;
using System.Data.OleDb;
using System.Collections;
using System.IO;
using System.Web.UI.WebControls;

namespace KDWechat.Common
{
    /// <summary>
    /// Excel 操作类
    /// 采用GemBox.ExcelLite第三方控件 
    /// </summary>
    public class GemBoxExcelLiteHelper
    {
        public GemBoxExcelLiteHelper() { }

        #region 数据导出至Excel文件

        /// <summary> 
        /// 导出Excel文件，自动返回可下载的文件流 
        /// </summary> 
        public static void DataTable1Excel(System.Data.DataTable dtData)
        {
            GridView gvExport = null;
            HttpContext curContext = HttpContext.Current;
            StringWriter strWriter = null;
            HtmlTextWriter htmlWriter = null;
            if (dtData != null)
            {
                curContext.Response.ContentType = "application/vnd.ms-excel";
                curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
                curContext.Response.Charset = "utf-8";
                strWriter = new StringWriter();
                htmlWriter = new HtmlTextWriter(strWriter);
                gvExport = new GridView();
                gvExport.DataSource = dtData.DefaultView;
                gvExport.AllowPaging = false;
                gvExport.DataBind();
                gvExport.RenderControl(htmlWriter);
                curContext.Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html;charset=gb2312\"/>" + strWriter.ToString());
                curContext.Response.End();
            }
        }

        /// <summary>
        /// 导出Excel文件，转换为可读模式
        /// </summary>
        public static void DataTable2Excel(System.Data.DataTable dtData)
        {
            DataGrid dgExport = null;
            HttpContext curContext = HttpContext.Current;
            StringWriter strWriter = null;
            HtmlTextWriter htmlWriter = null;

            if (dtData != null)
            {
                curContext.Response.ContentType = "application/vnd.ms-excel";
                curContext.Response.ContentEncoding = System.Text.Encoding.UTF8;
                curContext.Response.Charset = "";
                strWriter = new StringWriter();
                htmlWriter = new HtmlTextWriter(strWriter);
                dgExport = new DataGrid();
                dgExport.DataSource = dtData.DefaultView;
                dgExport.AllowPaging = false;
                dgExport.DataBind();
                dgExport.RenderControl(htmlWriter);
                curContext.Response.Write(strWriter.ToString());
                curContext.Response.End();
            }
        }

        /// <summary>
        /// 导出Excel文件，并自定义文件名
        /// </summary>
        public static void DataTable3Excel(System.Data.DataTable dtData, String FileName)
        {
            GridView dgExport = null;
            HttpContext curContext = HttpContext.Current;
            StringWriter strWriter = null;
            HtmlTextWriter htmlWriter = null;

            if (dtData != null)
            {
                HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8);
                curContext.Response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8) + ".xls");
                curContext.Response.ContentType = "application nd.ms-excel";
                curContext.Response.ContentEncoding = System.Text.Encoding.UTF8;
                curContext.Response.Charset = "utf-8";
                strWriter = new StringWriter();
                htmlWriter = new HtmlTextWriter(strWriter);
                dgExport = new GridView();
                dgExport.DataSource = dtData.DefaultView;
                dgExport.AllowPaging = false;
                dgExport.DataBind();
                dgExport.RenderControl(htmlWriter);
                curContext.Response.Write(strWriter.ToString());
                curContext.Response.End();
            }
        }

        /// <summary>
        /// 将数据导出至Excel文件
        /// </summary>
        /// <param name="Table">DataTable对象</param>
        /// <param name="ExcelFilePath">Excel文件路径</param>
        public static bool OutputToExcel(System.Data.DataTable Table, string ExcelFilePath)
        {
            if (File.Exists(ExcelFilePath))
            {
                throw new Exception("该文件已经存在！");
            }

            if ((Table.TableName.Trim().Length == 0) || (Table.TableName.ToLower() == "table"))
            {
                Table.TableName = "Sheet1";
            }

            //数据表的列数
            int ColCount = Table.Columns.Count;

            //用于记数，实例化参数时的序号
            int i = 0;

            //创建参数
            OleDbParameter[] para = new OleDbParameter[ColCount];

            //创建表结构的SQL语句
            string TableStructStr = @"Create Table " + Table.TableName + "(";

            //连接字符串
            string connString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ExcelFilePath + ";Extended Properties=Excel 8.0;";
            OleDbConnection objConn = new OleDbConnection(connString);

            //创建表结构
            OleDbCommand objCmd = new OleDbCommand();

            //数据类型集合
            ArrayList DataTypeList = new ArrayList();
            DataTypeList.Add("System.Decimal");
            DataTypeList.Add("System.Double");
            DataTypeList.Add("System.Int16");
            DataTypeList.Add("System.Int32");
            DataTypeList.Add("System.Int64");
            DataTypeList.Add("System.Single");

            //遍历数据表的所有列，用于创建表结构
            foreach (DataColumn col in Table.Columns)
            {
                //如果列属于数字列，则设置该列的数据类型为double
                if (DataTypeList.IndexOf(col.DataType.ToString()) >= 0)
                {
                    para[i] = new OleDbParameter("@" + col.ColumnName, OleDbType.Double);
                    objCmd.Parameters.Add(para[i]);

                    //如果是最后一列
                    if (i + 1 == ColCount)
                    {
                        TableStructStr += col.ColumnName + " double)";
                    }
                    else
                    {
                        TableStructStr += col.ColumnName + " double,";
                    }
                }
                else
                {
                    para[i] = new OleDbParameter("@" + col.ColumnName, OleDbType.VarChar);
                    objCmd.Parameters.Add(para[i]);

                    //如果是最后一列
                    if (i + 1 == ColCount)
                    {
                        TableStructStr += col.ColumnName + " varchar)";
                    }
                    else
                    {
                        TableStructStr += col.ColumnName + " varchar,";
                    }
                }
                i++;
            }

            //创建Excel文件及文件结构
            try
            {
                objCmd.Connection = objConn;
                objCmd.CommandText = TableStructStr;

                if (objConn.State == ConnectionState.Closed)
                {
                    objConn.Open();
                }
                objCmd.ExecuteNonQuery();
            }
            catch (Exception exp)
            {
                throw exp;
            }

            //插入记录的SQL语句
            string InsertSql_1 = "Insert into " + Table.TableName + " (";
            string InsertSql_2 = " Values (";
            string InsertSql = "";

            //遍历所有列，用于插入记录，在此创建插入记录的SQL语句
            for (int colID = 0; colID < ColCount; colID++)
            {
                if (colID + 1 == ColCount)  //最后一列
                {
                    InsertSql_1 += Table.Columns[colID].ColumnName + ")";
                    InsertSql_2 += "@" + Table.Columns[colID].ColumnName + ")";
                }
                else
                {
                    InsertSql_1 += Table.Columns[colID].ColumnName + ",";
                    InsertSql_2 += "@" + Table.Columns[colID].ColumnName + ",";
                }
            }

            InsertSql = InsertSql_1 + InsertSql_2;

            //遍历数据表的所有数据行
            for (int rowID = 0; rowID < Table.Rows.Count; rowID++)
            {
                for (int colID = 0; colID < ColCount; colID++)
                {
                    if (para[colID].DbType == DbType.Double && Table.Rows[rowID][colID].ToString().Trim() == "")
                    {
                        para[colID].Value = 0;
                    }
                    else
                    {
                        para[colID].Value = Table.Rows[rowID][colID].ToString().Trim();
                    }
                }
                try
                {
                    objCmd.CommandText = InsertSql;
                    objCmd.ExecuteNonQuery();
                }
                catch (Exception exp)
                {
                    string str = exp.Message;
                }
            }
            try
            {
                if (objConn.State == ConnectionState.Open)
                {
                    objConn.Close();
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            return true;
        }

        /// <summary>
        /// 将数据导出至Excel文件
        /// </summary>
        /// <param name="Table">DataTable对象</param>
        /// <param name="Columns">要导出的数据列集合</param>
        /// <param name="ExcelFilePath">Excel文件路径</param>
        public static bool OutputToExcel(System.Data.DataTable Table, ArrayList Columns, string ExcelFilePath)
        {
            if (File.Exists(ExcelFilePath))
            {
                throw new Exception("该文件已经存在！");
            }

            //如果数据列数大于表的列数，取数据表的所有列
            if (Columns.Count > Table.Columns.Count)
            {
                for (int s = Table.Columns.Count + 1; s <= Columns.Count; s++)
                {
                    Columns.RemoveAt(s);   //移除数据表列数后的所有列
                }
            }

            //遍历所有的数据列，如果有数据列的数据类型不是 DataColumn，则将它移除
            DataColumn column = new DataColumn();
            for (int j = 0; j < Columns.Count; j++)
            {
                try
                {
                    column = (DataColumn)Columns[j];
                }
                catch (Exception)
                {
                    Columns.RemoveAt(j);
                }
            }
            if ((Table.TableName.Trim().Length == 0) || (Table.TableName.ToLower() == "table"))
            {
                Table.TableName = "Sheet1";
            }

            //数据表的列数
            int ColCount = Columns.Count;

            //创建参数
            OleDbParameter[] para = new OleDbParameter[ColCount];

            //创建表结构的SQL语句
            string TableStructStr = @"Create Table " + Table.TableName + "(";

            //连接字符串
            string connString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ExcelFilePath + ";Extended Properties=Excel 8.0;";
            OleDbConnection objConn = new OleDbConnection(connString);

            //创建表结构
            OleDbCommand objCmd = new OleDbCommand();

            //数据类型集合
            ArrayList DataTypeList = new ArrayList();
            DataTypeList.Add("System.Decimal");
            DataTypeList.Add("System.Double");
            DataTypeList.Add("System.Int16");
            DataTypeList.Add("System.Int32");
            DataTypeList.Add("System.Int64");
            DataTypeList.Add("System.Single");

            DataColumn col = new DataColumn();

            //遍历数据表的所有列，用于创建表结构
            for (int k = 0; k < ColCount; k++)
            {
                col = (DataColumn)Columns[k];

                //列的数据类型是数字型
                if (DataTypeList.IndexOf(col.DataType.ToString().Trim()) >= 0)
                {
                    para[k] = new OleDbParameter("@" + col.Caption.Trim(), OleDbType.Double);
                    objCmd.Parameters.Add(para[k]);

                    //如果是最后一列
                    if (k + 1 == ColCount)
                    {
                        TableStructStr += col.Caption.Trim() + " Double)";
                    }
                    else
                    {
                        TableStructStr += col.Caption.Trim() + " Double,";
                    }
                }
                else
                {
                    para[k] = new OleDbParameter("@" + col.Caption.Trim(), OleDbType.VarChar);
                    objCmd.Parameters.Add(para[k]);

                    //如果是最后一列
                    if (k + 1 == ColCount)
                    {
                        TableStructStr += col.Caption.Trim() + " VarChar)";
                    }
                    else
                    {
                        TableStructStr += col.Caption.Trim() + " VarChar,";
                    }
                }
            }

            //创建Excel文件及文件结构
            try
            {
                objCmd.Connection = objConn;
                objCmd.CommandText = TableStructStr;

                if (objConn.State == ConnectionState.Closed)
                {
                    objConn.Open();
                }
                objCmd.ExecuteNonQuery();
            }
            catch (Exception exp)
            {
                throw exp;
            }

            //插入记录的SQL语句
            string InsertSql_1 = "Insert into " + Table.TableName + " (";
            string InsertSql_2 = " Values (";
            string InsertSql = "";

            //遍历所有列，用于插入记录，在此创建插入记录的SQL语句
            for (int colID = 0; colID < ColCount; colID++)
            {
                if (colID + 1 == ColCount)  //最后一列
                {
                    InsertSql_1 += Columns[colID].ToString().Trim() + ")";
                    InsertSql_2 += "@" + Columns[colID].ToString().Trim() + ")";
                }
                else
                {
                    InsertSql_1 += Columns[colID].ToString().Trim() + ",";
                    InsertSql_2 += "@" + Columns[colID].ToString().Trim() + ",";
                }
            }

            InsertSql = InsertSql_1 + InsertSql_2;

            //遍历数据表的所有数据行
            DataColumn DataCol = new DataColumn();
            for (int rowID = 0; rowID < Table.Rows.Count; rowID++)
            {
                for (int colID = 0; colID < ColCount; colID++)
                {
                    //因为列不连续，所以在取得单元格时不能用行列编号，列需得用列的名称
                    DataCol = (DataColumn)Columns[colID];
                    if (para[colID].DbType == DbType.Double && Table.Rows[rowID][DataCol.Caption].ToString().Trim() == "")
                    {
                        para[colID].Value = 0;
                    }
                    else
                    {
                        para[colID].Value = Table.Rows[rowID][DataCol.Caption].ToString().Trim();
                    }
                }
                try
                {
                    objCmd.CommandText = InsertSql;
                    objCmd.ExecuteNonQuery();
                }
                catch (Exception exp)
                {
                    string str = exp.Message;
                }
            }
            try
            {
                if (objConn.State == ConnectionState.Open)
                {
                    objConn.Close();
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            return true;
        }
        #endregion

        /// <summary>
        /// 导出CSV格式
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <param name="fileName">导出的名称</param>
        public static void DataTableToCsv(System.Data.DataTable dt, string fileName)
        {
            //Clear <div  id="loading" ..
            HttpContext.Current.Response.Clear();

            #region Export Grid to CSV
            // Create the CSV file to which grid data will be exported.
            System.IO.StringWriter sw = new System.IO.StringWriter();
            // First we will write the headers.
            int iColCount = dt.Columns.Count;
            for (int i = 0; i < iColCount; i++)
            {
                sw.Write("\"" + dt.Columns[i] + "\"");
                if (i < iColCount - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);
            // Now write all the rows.
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < iColCount; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        if (i == 12)
                        {
                            sw.Write("\"" + (dr[i].ToString().Trim() == "" ? "  " : "`" + dr[i].ToString().Trim()) + "    \"");
                        }
                        else
                        {
                            sw.Write("\"" + (dr[i].ToString().Trim() == "" ? "  " : dr[i].ToString().Trim()) + "\"");
                        }

                    }
                    else
                        sw.Write("\" \"");

                    if (i < iColCount - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
            #endregion

            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".csv");
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            HttpContext.Current.Response.Write(sw);
            HttpContext.Current.Response.End();

        }



        /// <summary>
        /// 在线生成Excel
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="page"></param>
        /// <param name="gridView"></param>

        public static void OnlineSaveExcel(string fileName, System.Web.UI.Page page, System.Web.UI.WebControls.GridView gridView)
        {
            page.Response.Clear();
            page.Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            page.Response.Charset = "gb2312";
            page.Response.ContentType = "application/vnd.xls";
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            //先设置分页为false
            gridView.AllowPaging = false;
            gridView.RenderControl(htmlWrite);
            page.Response.Write(stringWrite.ToString());
            page.Response.End();
        }



        public static bool SaveExcel<T>(string path, System.Web.UI.Page page, bool isDownload, bool isDelete, Dictionary<string, string> titles, List<T> list)
        {
            bool isc = true;
            try
            {
                //保存在本地
                isc = SaveToXls<T>(path, titles, list);
                if (isc)
                {
                    if (isDownload)
                    {
                        //提供下载
                        UploadExcel(path, page, isDelete);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isc;
        }


        /// <summary>
        /// 生成Excel
        /// </summary>
        /// <param name="path">绝对路径</param>
        /// <param name="page"></param>
        /// <param name="isDownload">是否提供下载,true是,false否</param>
        /// <param name="isDelete">是否删除本地生成的Excel,true是,false否</param>
        /// <param name="titles">Excel标题</param>
        /// <param name="gridView">数据</param>
        public static void SaveExcel(string path, System.Web.UI.Page page, bool isDownload, bool isDelete, System.Web.UI.WebControls.GridView gridView)
        {
            try
            {
                //保存在本地
                SaveToXls(path, gridView);
                if (isDownload)
                {
                    //提供下载
                    UploadExcel(path, page, isDelete);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 生成Excel
        /// </summary>
        /// <param name="path">绝对路径</param>
        /// <param name="page"></param>
        /// <param name="isDownload">是否提供下载,true是,false否</param>
        /// <param name="isDelete">是否删除本地生成的Excel,true是,false否</param>
        /// <param name="titles">Excel标题</param>
        /// <param name="gridView">数据</param>
        public static void SaveExcel(string path, System.Web.UI.WebControls.GridView gridView)
        {
            try
            {
                //保存在本地
                SaveToXls(path, gridView);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary> 
        /// 将一组对象导出成EXCEL 
        /// </summary> 
        /// <typeparam name="T">要导出对象的类型</typeparam> 
        /// <param name="objList">一组对象</param> 
        /// <param name="FileName">导出后的文件名</param> 
        /// <param name="columnInfo">列名信息</param> 
        public static void ExExcel<T>(List<T> objList, string FileName, Dictionary<string, string> columnInfo)
        {
            if (columnInfo.Count == 0) { return; }
            if (objList.Count == 0) { return; }
            //生成EXCEL的HTML 
            string excelStr = "";
            Type myType = objList[0].GetType();
            //根据反射从传递进来的属性名信息得到要显示的属性 
            List<System.Reflection.PropertyInfo> myPro = new List<System.Reflection.PropertyInfo>();
            foreach (string cName in columnInfo.Keys)
            {
                System.Reflection.PropertyInfo p = myType.GetProperty(cName);
                if (p != null)
                {
                    myPro.Add(p);
                    excelStr += columnInfo[cName] + "\t";
                }
            }
            //如果没有找到可用的属性则结束 
            if (myPro.Count == 0) { return; }
            excelStr += "\n";
            foreach (T obj in objList)
            {
                foreach (System.Reflection.PropertyInfo p in myPro)
                {
                    excelStr += Utils.RemoveQuotes(p.GetValue(obj, null).ToString()) + "\t";
                }
                excelStr += "\n";
            }
            excelStr = excelStr.TrimEnd('\n');
            //输出EXCEL 
            HttpResponse rs = System.Web.HttpContext.Current.Response;
            rs.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            rs.AppendHeader("Content-Disposition", "attachment;filename=" + FileName);
            rs.ContentType = "application/ms-excel";
            rs.Write(excelStr);
            rs.End();
        }

        /// <summary>
        /// 生成Excel
        /// </summary>
        /// <param name="path">绝对路径</param>
        /// <param name="page"></param>
        /// <param name="isDownload">是否提供下载,true是,false否</param>
        /// <param name="isDelete">是否删除本地生成的Excel,true是,false否</param>
        /// <param name="titles">Excel标题</param>
        /// <param name="ds">数据</param>
        public static void SaveExcel(string path, System.Web.UI.Page page, bool isDownload, bool isDelete, IList<String> titles, DataTable ds)
        {
            try
            {
                //保存在本地
                SaveToXls(path, titles, ds);
                if (isDownload)
                {
                    //提供下载
                    UploadExcel(path, page, isDelete);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 提供下载
        /// </summary>
        /// <param name="path"></param>
        /// <param name="page"></param>
        ///  <param name="isDelete"></param>
        private static void UploadExcel(string path, System.Web.UI.Page page, bool isDelete)
        {
            System.IO.FileInfo file = new System.IO.FileInfo(path);
            page.Response.Clear();
            page.Response.Charset = "GB2312";
            page.Response.ContentEncoding = System.Text.Encoding.UTF8;
            // 添加头信息，为"文件下载/另存为"对话框指定默认文件名 
            page.Response.AddHeader("Content-Disposition", "attachment; filename=" + page.Server.UrlEncode(file.Name));
            // 添加头信息，指定文件大小，让浏览器能够显示下载进度 
            page.Response.AddHeader("Content-Length", file.Length.ToString());

            // 指定返回的是一个不能被客户端读取的流，必须被下载 
            page.Response.ContentType = "application/ms-excel";

            // 把文件流发送到客户端 
            page.Response.WriteFile(file.FullName);

            page.Response.Flush();
            if (isDelete)
            {
                System.IO.File.Delete(path);
            }

            // 停止页面的执行 
            page.Response.End();



        }

        /// <summary>
        /// 保存在本地
        /// </summary>
        /// <param name="path">绝对路径</param>
        /// <param name="gridView">数据</param>
        private static void SaveToXls(string path, System.Web.UI.WebControls.GridView gridView)
        {

            ExcelFile excelFile = new ExcelFile();
            ExcelWorksheet sheet = excelFile.Worksheets.Add("Sheet1");

            int columns = gridView.Columns.Count;
            int rows = gridView.Rows.Count;
            for (int j = 0; j < columns; j++)
            {
                sheet.Cells[0, j].Value = gridView.Columns[j].HeaderText;
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    sheet.Cells[i + 1, j].Value = gridView.Rows[i].Cells[j].Text;
                }
            }
            excelFile.SaveXls(path);
        }


        private static bool SaveToXls<T>(string path, Dictionary<string, string> titles, List<T> list)
        {
            if (titles.Count == 0) { return false; }
            if (list.Count == 0) { return false; }
            int index = 1;
            int c = 0;
            ExcelFile excelFile = new ExcelFile();
            ExcelWorksheet sheet = excelFile.Worksheets.Add("Sheet1");

            Type myType = list[0].GetType();
            //根据反射从传递进来的属性名信息得到要显示的属性 
            List<System.Reflection.PropertyInfo> myPro = new List<System.Reflection.PropertyInfo>();
            foreach (string ckey in titles.Keys)
            {
                sheet.Cells[0, c].Value = titles[ckey];
                c++;
                System.Reflection.PropertyInfo p = myType.GetProperty(ckey);
                if (p != null)
                {
                    myPro.Add(p);
                }
            }


            int i = 0;
            foreach (T obj in list)
            {
                int j = 0;
                foreach (System.Reflection.PropertyInfo p in myPro)
                {
                    Encoding ascii = Encoding.Default;
                    Encoding unicode = Encoding.Unicode;
                    string _value = "";
                    if (p.GetValue(obj, null) != null)
                    {
                        _value = p.GetValue(obj, null).ToString();
                    }
                    string unicodeString = Common.Utils.DropHTML(_value).Replace("\r", "").Replace("\n", "").Replace("\t", "").TrimEnd().TrimStart();
                    // Convert the string into a byte[].
                    byte[] unicodeBytes = unicode.GetBytes(unicodeString);

                    // Perform the conversion from one encoding to the other.
                    byte[] asciiBytes = Encoding.Convert(unicode, ascii, unicodeBytes);

                    // Convert the new byte[] into a char[] and then into a string.
                    // This is a slightly different approach to converting to illustrate
                    // the use of GetCharCount/GetChars.
                    char[] asciiChars = new char[ascii.GetCharCount(asciiBytes, 0, asciiBytes.Length)];
                    ascii.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0);
                    string asciiString = new string(asciiChars);

                    // Display the strings created before and after the conversion.

                    sheet.Cells[i + index, j].Value = asciiString;
                    j++;
                }
                i++;
            }
            excelFile.SaveXls(path);
            return true;
        }



        /// <summary>
        /// 保存在本地 modify by ceiling(解决导出的字段内容的问题，如编码换行等) 
        /// </summary>
        /// <param name="path">绝对路径</param>
        /// <param name="titles">Excel标题</param>
        /// <param name="ds">数据</param>
        private static void SaveToXls(string path, IList<String> titles, DataTable ds)
        {
            System.Data.DataTable dt = ds;
            ExcelFile excelFile = new ExcelFile();
            ExcelWorksheet sheet = excelFile.Worksheets.Add("Sheet1");


            int columns = dt.Columns.Count;
            int rows = dt.Rows.Count;
            int index = 0;
            if (titles != null && titles.Count > 0)
            {
                for (int i = 0; i < titles.Count; i++)
                {

                    sheet.Cells[0, i].Value = titles[i];
                }
                index++;
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    // Create two different encodings.
                    Encoding ascii = Encoding.Default;
                    Encoding unicode = Encoding.Unicode;
                    string unicodeString = Common.Utils.DropHTML(dt.Rows[i][j].ToString().Replace("\r", "").Replace("\n", "").Replace("\t", "")).TrimEnd().TrimStart();
                    // Convert the string into a byte[].
                    byte[] unicodeBytes = unicode.GetBytes(unicodeString);

                    // Perform the conversion from one encoding to the other.
                    byte[] asciiBytes = Encoding.Convert(unicode, ascii, unicodeBytes);

                    // Convert the new byte[] into a char[] and then into a string.
                    // This is a slightly different approach to converting to illustrate
                    // the use of GetCharCount/GetChars.
                    char[] asciiChars = new char[ascii.GetCharCount(asciiBytes, 0, asciiBytes.Length)];
                    ascii.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0);
                    string asciiString = new string(asciiChars);

                    // Display the strings created before and after the conversion.
                    Console.WriteLine("Original string: {0}", unicodeString);
                    Console.WriteLine("Ascii converted string: {0}", asciiString);

                    //sheet.Cells[i + index, j].Value =HttpUtility.UrlEncode(dt.Rows[i][j].ToString(),System.Text.Encoding.GetEncoding("gb2312"));
                    sheet.Cells[i + index, j].Value = asciiString;
                    //sheet.Cells[i + index, j].Style.WrapText = true;
                    //sheet.Cells[i + index, j].Style.IsTextVertical = true;
                }
            }
            excelFile.SaveXls(path);

        }

        /// <summary>
        /// 将Excel文件导出至DataTable(第一行作为表头)
        /// </summary>
        /// <param name="ExcelFilePath">Excel文件路径</param>
        /// <param name="TableName">数据表名，如果数据表名错误，默认为第一个数据表名</param>
        public static DataTable InputFromExcel(string ExcelFilePath, string TableName)
        {
            if (!File.Exists(ExcelFilePath))
            {
                throw new Exception("Excel文件不存在！");
            }

            //如果数据表名不存在，则数据表名为Excel文件的第一个数据表
            ArrayList TableList = new ArrayList();
            TableList = GetExcelTables(ExcelFilePath);

            if (TableName.Trim() == "")
            {
                TableName = TableList[0].ToString().Trim();
            }

            DataTable table = new DataTable();
            var ext = Path.GetExtension(ExcelFilePath);

            OleDbConnection dbcon = new OleDbConnection();
            //if(ext.ToLower()==".xls")
            //    dbcon.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ExcelFilePath + ";Extended Properties=Excel 8.0";
            //else if(ext.ToLower()==".xlsx")
                dbcon.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + ExcelFilePath + ";Extended Properties='Excel 12.0;HDR=YES';";
            OleDbCommand cmd = new OleDbCommand("select * from [" + TableName + "$]", dbcon);
            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);

            try
            {
                if (dbcon.State == ConnectionState.Closed)
                {
                    dbcon.Open();
                }
                adapter.Fill(table);
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (dbcon.State == ConnectionState.Open)
                {
                    dbcon.Close();
                }
            }
            return table;
        }
        /// <summary>
        /// 获取Excel文件数据表列表
        /// </summary>
        public static ArrayList GetExcelTables(string ExcelFileName)
        {
            DataTable dt = new DataTable();
            ArrayList TablesList = new ArrayList();
            if (File.Exists(ExcelFileName))
            {
                var ext = Path.GetExtension(ExcelFileName);

                OleDbConnection dbcon = new OleDbConnection();
                //if (ext.ToLower() == ".xls")
                //    dbcon.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ExcelFileName + ";Extended Properties=Excel 8.0";
                //else if (ext.ToLower() == ".xlsx")
                    dbcon.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + ExcelFileName + ";Extended Properties='Excel 12.0;HDR=YES';";

                //OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Extended Properties=Excel 8.0;Data Source=" + ExcelFileName)
                
                try
                {
                    dbcon.Open();
                    dt = dbcon.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                }
                catch (Exception exp)
                {
                    throw exp;
                }

                //获取数据表个数
                int tablecount = dt.Rows.Count;
                for (int i = 0; i < tablecount; i++)
                {
                    string tablename = dt.Rows[i][2].ToString().Trim().TrimEnd('$');
                    if (TablesList.IndexOf(tablename) < 0)
                    {
                        TablesList.Add(tablename);
                    }
                }
                dbcon.Close();
            }
            return TablesList;
        }


    }

}
