using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDWechat.Dapper
{
    /// <summary>
    /// 数据库链接
    /// </summary>
    public class DapperConnection
    {

        /// <summary>
        /// 微信主库
        /// </summary>
        public static SqlConnection kd_wechats
        {
            get
            {
                var connStr = ConfigurationManager.ConnectionStrings["ConnectionString_Wechats"].ConnectionString;
                var con = new SqlConnection(connStr);
                return con;//ConnectionContainer.GetConnection("kd_mall");
            }
        }

        /// <summary>
        /// 微信用户库
        /// </summary>
        public static SqlConnection kd_users
        {
            get
            {
                var connStr = ConfigurationManager.ConnectionStrings["ConnectionString_Users"].ConnectionString;
                var con = new SqlConnection(connStr);
                return con;// ConnectionContainer.GetConnection("kd_mall_extend_attribute");
            }
        }

        /// <summary>
        /// 日志数据库
        /// </summary>
        public static SqlConnection kd_logs
        {
            get
            {
                var connStr = ConfigurationManager.ConnectionStrings["ConnectionString_Logs"].ConnectionString;
                var con = new SqlConnection(connStr);
                return con;//ConnectionContainer.GetConnection("kd_log");
            }
        }

        /// <summary>
        /// 模块数据库
        /// </summary>
        public static SqlConnection kd_modules
        {
            get
            {
                var connStr = ConfigurationManager.ConnectionStrings["ConnectionString_Module"].ConnectionString;
                var con = new SqlConnection(connStr);
                return con;// ConnectionContainer.GetConnection("kd_user");
            }
        }


        /// <summary>
        /// 统计数据库
        /// </summary>
        public static SqlConnection kd_statistics
        {
            get
            {
                var connStr = ConfigurationManager.ConnectionStrings["ConnectionString_Statistics"].ConnectionString;
                var con = new SqlConnection(connStr);
                return con;// ConnectionContainer.GetConnection("kd_user");
            }
        }

    }
}
