using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using KDWechat.Common;
using KDWechat.DBUtility;
using KDWechat.DBUtility;

namespace KDWechat.BLL
{
    /// <summary>
    /// 分页类
    /// </summary>
    public class PageHelper
    {
        /// <summary>
        /// 存储过程分页
        /// </summary>
        /// <param name="dbType">数据库（1、操作日志库 2、用户信息库 3、微信架构库）</param>
        /// <param name="QueryStr">查询数据</param>
        /// <param name="PageSize">分页大小</param>
        /// <param name="PageCurrent">当前页</param>
        /// <param name="FdShow">显示的字段</param>
        /// <param name="FdOrder">排序字段</param>
        /// <param name="rowCount">数据总条数</param>
        /// <returns></returns>
        public static DataTable GetPageList(DbDataBaseEnum dbType, string QueryStr, int PageSize, int PageCurrent, string FdShow, string FdOrder, ref int rowCount) 
        {
            SqlParameter[] par ={
                 new SqlParameter("@QueryStr",SqlDbType.VarChar),
                 new SqlParameter("@PageSize",SqlDbType.Int),
                 new SqlParameter("@PageCurrent",SqlDbType.Int),
                 new SqlParameter("@FdShow",SqlDbType.VarChar,100),
                 new SqlParameter("@FdOrder",SqlDbType.VarChar,100),
                 new SqlParameter("@Rows",SqlDbType.Int)
             };

            par[0].Value =Common.Utils.DropHTML( QueryStr,true);
            par[1].Value = PageSize;
            par[2].Value = PageCurrent;
            par[3].Value = FdShow;
            par[4].Value = FdOrder;
            //par[5].Value = rowCount;

            //DataSet ds = KDWechat.DBUtility.DbHelperSQL.ExecuteDataSet(CommandType.StoredProcedure, "P_PageView", par);
          
            DataSet ds = null;
          
         ds = KDWechat.DBUtility.DbHelperSQL.RunProcedure("P_PageView", par, "ds");
                   
            if (rowCount > 0)
                par[5].Value = rowCount;
            else
                par[5].Value = DBNull.Value;
            if (rowCount <= 0)
            {
                rowCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
            }
            return ds.Tables[0];
        }
    }
}
