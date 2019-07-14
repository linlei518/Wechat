using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KDWechat.DAL;
using KDWechat.DBUtility;
using KDWechat.DBUtility;

namespace KDWechat.BLL.Statistics
{
    public class st_diymenu_click
    {

        /// <summary>
        /// 添加一条点击记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static long Add(t_st_diymenu_click model)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into t_st_diymenu_click(");
            strSql.Append("wx_id,wx_og_id,open_id,menu_key,menu_name,menu_action,add_time)");
            strSql.Append(" values (");
            strSql.Append("@wx_id,@wx_og_id,@open_id,@menu_key,@menu_name,@menu_action,@add_time)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@wx_id", SqlDbType.Int,4),
					new SqlParameter("@wx_og_id", SqlDbType.NVarChar,50),
					new SqlParameter("@open_id", SqlDbType.NVarChar,50),
					new SqlParameter("@menu_key", SqlDbType.NVarChar,50),
					new SqlParameter("@menu_name", SqlDbType.NVarChar,50),
					new SqlParameter("@menu_action", SqlDbType.NVarChar,500),
					new SqlParameter("@add_time", SqlDbType.DateTime)};
            parameters[0].Value = model.wx_id;
            parameters[1].Value = model.wx_og_id;
            parameters[2].Value = model.open_id;
            parameters[3].Value = model.menu_key;
            parameters[4].Value = model.menu_name;
            parameters[5].Value = model.menu_action;
            parameters[6].Value = model.add_time;

            object obj = KDWechat.DBUtility.DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt64(obj);
            }
        }

    }
}
