using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using KDWechat.Common;
using Microsoft.Security.Application;
using System.Reflection;
using System.Linq.Expressions;
using System.Data;


namespace KDWechat.Dapper
{
    public class DapperHelper
    {



        #region 分页方法
        /// <summary>
        /// Dapper获取分页列表
        /// </summary>
        /// <typeparam name="T">获取的列表类型</typeparam>
        /// <param name="con">直接调用DapperConnection类</param>
        /// <param name="sql">sql语句（不包含orderby以外的部分）</param>
        /// <param name="orderby">orderby的字段，如果多个可用,分隔，逆序可用desc</param>
        /// <param name="pagesize">页大小</param>
        /// <param name="pageindex">当前页</param>
        /// <param name="totalCount">数据总数</param>
        /// <returns></returns>
        public static List<T> GetList<T>(SqlConnection con, string sql, string orderby, int pagesize, int pageindex, out int totalCount)
        {
            var safeSql = GetAntiXssSql(sql);
            totalCount = con.Query<int>(PagingHelper.CreateCountingSql(safeSql)).First();
            var pagingSql = PagingHelper.CreatePagingSql(totalCount, pagesize, pageindex, safeSql, orderby);
            return con.Query<T>(pagingSql).ToList();
        }

        /// <summary>
        /// Dapper获取分页列表
        /// </summary>
        /// <typeparam name="T">获取的列表类型</typeparam>
        /// <param name="con">直接调用DapperConnection类</param>
        /// <param name="sql">sql语句（不包含orderby以外的部分）</param>
        /// <param name="orderby">orderby的字段，如果多个可用,分隔，逆序可用desc</param>
        /// <param name="pagesize">页大小</param>
        /// <param name="pageindex">当前页</param>
        /// <param name="totalCount">数据总数</param>
        /// <returns></returns>
        public static List<T> GetList<T>(SqlConnection con, string sql, string orderby, int pagesize, int pageindex, out int totalCount, object param)
        {
            var safeSql = GetAntiXssSql(sql);
            totalCount = con.Query<int>(PagingHelper.CreateCountingSql(safeSql), param).First();
            var pagingSql = PagingHelper.CreatePagingSql(totalCount, pagesize, pageindex, safeSql, orderby);
            return con.Query<T>(pagingSql, param).ToList();
        }

        /// <summary>
        /// Dapper获取列表
        /// </summary>
        /// <typeparam name="T">获取的列表类型</typeparam>
        /// <param name="con">直接调用DapperConnection类</param>
        /// <param name="sql">sql语句含orderby</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public static List<T> GetList<T>(SqlConnection con, string sql, object param)
        {

            var safeSql = GetAntiXssSql(sql);
            return con.Query<T>(safeSql, param).ToList();
        }
        #endregion



        #region 执行sql语句
        /// <summary>
        /// 执行sql并返回单个对象
        /// </summary>
        /// <typeparam name="T">model类型</typeparam>
        /// <param name="con">直接调用DapperConnection类</param>
        /// <param name="sql">sql语句</param>
        /// <returns>查询到的对象，可能为空</returns>
        public static T ExcuteSingle<T>(SqlConnection con, string sql)
        {
            return con.Query<T>(GetAntiXssSql(sql)).FirstOrDefault();
        }

        /// <summary>
        /// 执行sql并返回单个对象
        /// </summary>
        /// <typeparam name="T">model类型</typeparam>
        /// <param name="con">直接调用DapperConnection类</param>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql里面用的参数，假设用到@a，则传入new {a='xxx'},传入对象即可，这里不是memberinitexpression</param>
        /// <returns>查询到的对象，可能为空</returns>
        public static T ExcuteSingle<T>(SqlConnection con, string sql, object param)
        {
            if (!sql.Contains("@"))
                throw new Exception("请使用参数查询");
            return con.Query<T>(GetAntiXssSql(sql), param).FirstOrDefault();
        }

        /// <summary>
        /// 获取总数
        /// </summary>
        /// <param name="con">直接调用DapperConnection类</param>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql里面用的参数，假设用到@a，则传入new {a='xxx'},传入对象即可</param>
        /// <returns></returns>
        public static int GetCount(SqlConnection con, string sql, object param)
        {
            int count = con.Query<int>(sql, param).First();
            return count;
        }

        /// <summary>
        /// 获取单个值数据
        /// </summary>
        /// <param name="con">直接调用DapperConnection类</param>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql里面用的参数，假设用到@a，则传入new {a='xxx'},传入对象即可</param>
        /// <returns></returns>
        public static object GetScalarValue(SqlConnection con, string sql, object param)
        {
            return con.ExecuteScalar(sql, param);
        }

        #endregion

        #region insert 方法



        /// <summary>
        /// insert 一个实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="con">直接调用DapperConnection类</param>
        /// <param name="model">模型</param>
        /// <param name="fieldWithOutInsert">不需要插入的字段，主要是针对自增主键</param>
        /// <returns></returns>
        public static T AddModel<T>(SqlConnection con, T model, string fieldWithOutInsert = "", bool is_filter = true)
        where T : class
        {
            var insertParameterSql = GetInsertParamSql(typeof(T), fieldWithOutInsert);
            if (is_filter)
            {
                model = ReturnSecurityObject(model) as T;
            }

            var identify = ExcuteSingle<int?>(con, insertParameterSql, model);
            if (!string.IsNullOrWhiteSpace(fieldWithOutInsert))
                model = SetIdentify<T>(model, fieldWithOutInsert, identify);
            return model;
        }
        #endregion

        #region update方法
        /// <summary>
        /// update一个实体,实体是先查询出来的
        /// </summary>
        /// <param name="con">直接调用DapperConnection类</param>
        /// <param name="model">需要更新的模型</param>
        /// <param name="fieldWithOutUpdate">不需要更新的字段，主要是针对自增主键</param>
        /// <returns></returns>
        public static bool UpdateModel<T>(SqlConnection con, T model, string fieldWithOutUpdate = "", bool is_filter = true) where T : class
        {
            var updateParameterSql = GetUpdateParamSql(typeof(T), fieldWithOutUpdate);
            try
            {
                if (is_filter)
                {
                    model = ReturnSecurityObject(model) as T;
                }
                ExcuteSingle<int>(con, updateParameterSql, model);
                return true;
            }
            catch
            { return false; }
        }

        /// <summary>
        /// update 一条sql语句，必须用参数化
        /// </summary>
        /// <param name="con">直接调用DapperConnection类</param>
        /// <param name="sql">TSQL语句</param>
        /// <param name="param">sql里面用的参数，假设用到@a，则传入new {a='xxx'},传入对象即可</param>
        /// <returns></returns>
        public static bool UpdateSql(SqlConnection con, string sql, object param)
        {
            string filterSql = GetAntiXssSql(sql);
            int num = con.Execute(filterSql, param);
            return num > 0;
        }
        #endregion


        #region delete 方法


        /// <summary>
        /// 执行一条sql语句(增删改)
        /// </summary>
        /// <param name="con">直接调用DapperConnection类</param>
        /// <param name="sql">TSQL语句</param>
        /// <param name="param">sql里面用的参数，假设用到@a，则传入new {a='xxx'},传入对象即可</param>
        /// <returns></returns>
        public static bool ExecuteSql(SqlConnection con, string sql, object param)
        {
            string filterSql = GetAntiXssSql(sql);
            int num = con.Execute(filterSql, param);
            return num > 0;
        }
        #endregion


        #region 事务处理

        /// <summary>
        /// 事务处理，用于增删改
        /// </summary>
        /// <param name="con">直接调用DapperConnection类</param>
        /// <param name="sql">需要执行语句的集合</param>
        /// <returns></returns>
        public static bool ExecuteTransaction(SqlConnection con, List<string> sql)
        {
            int num = 0;
            bool result = false;
            bool wasClosed = con.State == ConnectionState.Closed;
            if (wasClosed) con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {

                for (int i = 0; i < sql.Count; i++)
                {
                    num = con.Execute(sql[i], null, tran);
                }
                tran.Commit();
                result = true;

            }
            catch (Exception)
            {
                tran.Rollback();

            }
            finally
            {
                tran.Dispose();
                if (wasClosed) con.Close();
            }



            return result;
        }
        #endregion

        #region 内部方法
        /// <summary>
        /// 设置主键
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="fieldWithOutInsert"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static T SetIdentify<T>(T model, string fieldWithOutInsert, int? value)
        where T : class
        {
            fieldWithOutInsert = fieldWithOutInsert.ToLower();
            var t = typeof(T);
            var properties = t.GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].Name.ToLower() == fieldWithOutInsert)
                {
                    properties[i].SetValue(model, value);
                    break;
                }
            }
            var fields = t.GetFields();
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].Name.ToLower() == fieldWithOutInsert)
                {
                    fields[i].SetValue(model, value);
                    break;
                }
            }
            return model;
        }

        /// <summary>
        /// 返回安全的entity对象
        /// </summary>
        /// <typeparam name="T">entity对象的类型</typeparam>
        /// <param name="model">entity对象</param>
        /// <returns>安全的entity对象</returns>
        private static object ReturnSecurityObject(object model)
        {

            Type t = model.GetType();//获取类型
            foreach (PropertyInfo mi in t.GetProperties())//遍历该类型下所有属性（非字段，字段需要另一方法，好在EF都是属性
            {
                if (mi.PropertyType == "".GetType())//如果属性为string类型
                {
                    var inputString = (mi.GetValue(model) ?? "").ToString();
                    var sx = Sanitizer.GetSafeHtmlFragment(inputString);//进行字符串过滤
                    sx = System.Web.HttpUtility.HtmlDecode(sx);
                    mi.SetValue(model, sx);//将过滤后的值设置给传入的对象
                }
            }
            return model;//返回安全对象
        }
        /// <summary>
        /// 获取更新sql
        /// </summary>
        /// <param name="type">需要更新的类型</param>
        /// <param name="fieldWithOutUpdate">需要更新的主键名</param>
        /// <returns></returns>
        private static string GetUpdateParamSql(Type type, string fieldWithOutUpdate)
        {
            var properties = type.GetProperties();
            var fields = type.GetFields();
            var paramSql = "update " + type.Name + " set ";
            fieldWithOutUpdate = (fieldWithOutUpdate ?? "").ToLower();
            if (properties != null && properties.Length > 0)
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    var name = properties[i].Name;
                    if (fieldWithOutUpdate != (name.ToLower()))
                        paramSql += name + "=@" + name + ",";
                    else
                        fieldWithOutUpdate = name;
                }
            }
            if (fields != null && fields.Length > 0)
            {
                for (int i = 0; i < fields.Length; i++)
                {
                    var name = fields[i].Name;
                    if (fieldWithOutUpdate != (name.ToLower()))
                        paramSql += name + "=@" + name + ",";
                    else
                        fieldWithOutUpdate = name;
                }
            }
            return paramSql.TrimEnd(',') + string.Format(" where {0}=@{0}", fieldWithOutUpdate);
        }
        /// <summary>
        /// 获取insert子句 insert into table （xxx,yyy,zzz） values (@xxx,@yyy,@zzz)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string GetInsertParamSql(Type type, string fieldWithOutInsert)
        {
            var properties = type.GetProperties();
            var fields = type.GetFields();
            var paramSql = "insert into " + type.Name + " ";
            var paramString = string.Empty;
            var fieldString = string.Empty;
            var allFieldsName = new List<string>();
            fieldWithOutInsert = (fieldWithOutInsert ?? "").ToLower();
            if (properties != null && properties.Length > 0)
                for (int i = 0; i < properties.Length; i++)
                    if (fieldWithOutInsert != (properties[i].Name.ToLower()))
                        allFieldsName.Add(properties[i].Name);
            if (fields != null && fields.Length > 0)
                for (int i = 0; i < fields.Length; i++)
                    if (fieldWithOutInsert != (fields[i].Name.ToLower()))
                        allFieldsName.Add(fields[i].Name);
            allFieldsName.ForEach(x => { fieldString += x + ","; paramString += "@" + x + ","; });
            paramSql += string.Format("({0}) values ({1});select @@identity;", fieldString.TrimEnd(','), paramString.TrimEnd(','));
            return paramSql;
        }

        /// <summary>
        /// 获取防JS攻击的sql
        /// </summary>
        /// <param name="inputString">输入的sql</param>
        /// <returns></returns>
        private static string GetAntiXssSql(string inputString)
        {
            var sx = Sanitizer.GetSafeHtmlFragment(inputString);//进行字符串过滤
            return System.Web.HttpUtility.HtmlDecode(sx);
        }
        #endregion

    }
}
