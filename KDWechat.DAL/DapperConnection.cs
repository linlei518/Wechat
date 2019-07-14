using System.Configuration;
using System.Data.SqlClient;

namespace KDWechat.DAL
{
    public class DapperConnection
    {
        /// <summary>
        /// 久事数据库链接对象
        /// </summary>
        public static SqlConnection minebea
        {
            get
            {
                var connStr = ConfigurationManager.ConnectionStrings["ConnectionString_Wechats"].ConnectionString;
                var con = new SqlConnection(connStr);
                return con;
            }
        }

        /// <summary>
        /// 美蓓亚数据库链接对象
        /// </summary>
        public static SqlConnection temp_minebea
        {
            get
            {
                var connStr = ConfigurationManager.ConnectionStrings["ConnectionString_Wechats"].ConnectionString;
                var con = new SqlConnection(connStr);
                return con;
            }
        }


        /// <summary>
        /// 锐速会员
        /// </summary>
        public static SqlConnection minebeaMember
        {
            get
            {
                var connStr = ConfigurationManager.ConnectionStrings["db_member"].ConnectionString;
                var con = new SqlConnection(connStr);
                return con;
            }
        }
    }
}
