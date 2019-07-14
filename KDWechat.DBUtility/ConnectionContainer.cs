using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDWechat.Dapper
{
    public class ConnectionContainer
    {
        /// <summary>
        /// connection集合
        /// </summary>
        protected static Dictionary<string, SqlConnection> Connections = new Dictionary<string,SqlConnection>();

        /// <summary>
        /// 添加一个connection
        /// </summary>
        /// <param name="conName">connection名称</param>
        /// <param name="con">connection</param>
        /// <returns>false:当前connection已存在</returns>
        protected static bool AddConnection(string conName, SqlConnection con)
        {
            if (Connections.ContainsKey(conName))
                return false;
            Connections.Add(conName, con);
            return true;
        }

        /// <summary>
        /// 获取连接
        /// </summary>
        /// <param name="conName">config里面的connectionString名称</param>
        /// <returns></returns>
        public static SqlConnection GetConnection(string conName)
        {
            if (Connections.ContainsKey(conName))
                return Connections[conName];
            try
            {
                var connStr = ConfigurationManager.ConnectionStrings[conName].ConnectionString;
                var con = new SqlConnection(connStr);
                AddConnection(conName, con);
                return con;
            }
            catch
            {
                throw new Exception("config文件中需要包含名称为："+conName+"的连接字符串");
            }
        }

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="conName">config里面的connectionString名称</param>
        /// <returns></returns>
        public static string GetConnectionString(string conName)
        {
            return ConfigurationManager.ConnectionStrings[conName].ConnectionString;
        }

    }
}
