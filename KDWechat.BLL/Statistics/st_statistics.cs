using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KDWechat.DAL;
using KDWechat.DBUtility;

namespace KDWechat.BLL.Statistics
{
    /// <summary>
    /// 所有模块的浏览记录、分享、点击操作
    /// </summary>
    public class st_statistics
    {

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool Add(Common.Statistics.st_statistics model)
        {
            bool isc = false;
            if (model != null)
            {
                CreateDataTable(model.db_table_name);

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into " + model.db_table_name + "(");
                strSql.Append("wx_id,wx_og_id,open_id,from_open_id,page_url,page_name,add_time,user_ip,obj_id,obj_name,url_referrer)");
                strSql.Append(" values (");
                strSql.Append("@wx_id,@wx_og_id,@open_id,@from_open_id,@page_url,@page_name,@add_time,@user_ip,@obj_id,@obj_name,@url_referrer)");
                // strSql.Append(";select @@IDENTITY");
                SqlParameter[] parameters = {
					new SqlParameter("@wx_id", SqlDbType.Int,4),
					new SqlParameter("@wx_og_id", SqlDbType.NVarChar,50),
					new SqlParameter("@open_id", SqlDbType.NVarChar,50),
					new SqlParameter("@from_open_id", SqlDbType.NVarChar,50),

					new SqlParameter("@page_url", SqlDbType.NVarChar,500),
					new SqlParameter("@page_name", SqlDbType.NVarChar,50),
					new SqlParameter("@add_time", SqlDbType.DateTime),
					new SqlParameter("@user_ip", SqlDbType.NVarChar,50),
					new SqlParameter("@obj_id", SqlDbType.Int,4),
					new SqlParameter("@obj_name", SqlDbType.NVarChar,50)   ,
                    new SqlParameter("@url_referrer", SqlDbType.NVarChar,250),
               };
                parameters[0].Value = model.wx_id;
                parameters[1].Value = model.wx_og_id;
                parameters[2].Value = model.open_id;
                parameters[3].Value = model.from_open_id;
                parameters[4].Value = model.page_url;
                parameters[5].Value = model.page_name;
                parameters[6].Value = model.add_time;
                parameters[7].Value = model.user_ip;
                parameters[8].Value = model.obj_id;
                parameters[9].Value = model.obj_name;
                parameters[10].Value = model.url_referrer;

                int num = KDWechat.DBUtility.DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
                if (num>0)
                {
                    if (model.db_table_name == "t_st_graphic_share")
                    {
                        string sql = "update t_wx_news_materials set share_count=share_count+1   where id=" + model.obj_id;
                        KDWechat.DBUtility.DbHelperSQL.ExecuteSql(sql);
                    }
                    else if (model.db_table_name == "t_st_graphic_view")
                    {
                        string sql = "update t_wx_news_materials set view_count=view_count+1   where id=" + model.obj_id;
                        KDWechat.DBUtility.DbHelperSQL.ExecuteSql(sql);
                    }
                    else if (model.db_table_name == "t_st_graohic_click")
                    {
                        string sql = "update t_wx_news_materials set click_count=click_count+1   where id=" + model.obj_id;
                        KDWechat.DBUtility.DbHelperSQL.ExecuteSql(sql);
                    }
                    
                }
                isc = num > 0;
            }


            return isc;
        }


        /// <summary>
        /// 创建数据库表
        /// </summary>
        /// <param name="db_table_name"></param>
        /// <returns></returns>
        private static bool CreateDataTable(string db_table_name)
        {
            bool isc = true;
            string sql = @"if not exists(select * from sysobjects where name='" + db_table_name + "' )";
            sql +=" begin ";
            sql += "CREATE TABLE  " + db_table_name + "";
                   sql+= @"(
	                [id] [bigint] IDENTITY(1,1) NOT NULL,
	                [wx_id] [int] NOT NULL,
	                [wx_og_id] [nvarchar](50) NOT NULL,
	                [open_id] [nvarchar](50) NOT NULL,
	                [from_open_id] [nvarchar](50) NOT NULL,
                    [url_referrer] [nvarchar](250) NOT NULL,
	                [page_url] [nvarchar](500) NOT NULL,
	                [page_name] [nvarchar](50) NOT NULL,
	                [add_time] [datetime] NOT NULL  DEFAULT (getdate()),
	                [user_ip] [nvarchar](50) NOT NULL,
	                [obj_id] [int] NOT NULL  DEFAULT (0),
	                [obj_name] [nvarchar](50) NOT NULL
	                )";

            sql += " EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'信息ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'" + db_table_name + "', @level2type=N'COLUMN',@level2name=N'id'";
            sql += "EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属微信ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'" + db_table_name + "', @level2type=N'COLUMN',@level2name=N'wx_id'";
            sql += "EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原始id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'" + db_table_name + "', @level2type=N'COLUMN',@level2name=N'wx_og_id'";
            sql += "EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关注用户OPENID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'" + db_table_name + "', @level2type=N'COLUMN',@level2name=N'open_id'";
            sql += "EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'转发过来的用户OPENID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'" + db_table_name + "', @level2type=N'COLUMN',@level2name=N'from_open_id'";
            sql += "EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'url来源' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'" + db_table_name + "', @level2type=N'COLUMN',@level2name=N'url_referrer'";
            sql += " EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'浏览页面Url地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'" + db_table_name + "', @level2type=N'COLUMN',@level2name=N'page_url'";
            sql += "EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'浏览页名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'" + db_table_name + "', @level2type=N'COLUMN',@level2name=N'page_name'";
            sql += "EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'浏览时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'" + db_table_name + "', @level2type=N'COLUMN',@level2name=N'add_time'";
            sql += "EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户的ip地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'" + db_table_name + "', @level2type=N'COLUMN',@level2name=N'user_ip'";
            sql += "EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'对应的数据表id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'" + db_table_name + "', @level2type=N'COLUMN',@level2name=N'obj_id'";
            sql += "EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'对应的数据表名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'" + db_table_name + "', @level2type=N'COLUMN',@level2name=N'obj_name'";
            sql += " end";

            KDWechat.DBUtility.DbHelperSQL.ExecuteSql(sql);
            return isc;
        }



 

        public static void DeleteClcik(string new_table_name, long id,int news_id=0)
        {
            string sql = "delete " + new_table_name + " where id="+id;
            KDWechat.DBUtility.DbHelperSQL.ExecuteSql(sql);
            if (new_table_name == "t_st_graohic_click")
            {
                sql = "update t_wx_news_materials set click_count=click_count-1   where id=" + news_id;
                KDWechat.DBUtility.DbHelperSQL.ExecuteSql(sql);
            }
        }

        public static Common.Statistics.st_statistics GetModel(string new_table_name, string openId, int obj_id,string ip)
        {
            CreateDataTable(new_table_name);
            string sql = "select top 1 * from " + new_table_name + " where open_id='"+openId+"' and obj_id='"+obj_id+"'";
            if (openId=="")
            {
                sql = "select top 1 * from " + new_table_name + " where user_ip='" + ip + "' and obj_id='" + obj_id + "'";
            }
            DataTable dt = KDWechat.DBUtility.DbHelperSQL.Query(sql).Tables[0];
             Common.Statistics.st_statistics model= GetModel(dt);
             if (model!=null)
             {
                 model.db_table_name = new_table_name;
             }
             return model;
        }

        private static Common.Statistics.st_statistics GetModel(DataTable dt)
        {
            Common.Statistics.st_statistics model = null;
            if (dt!=null)
            {
                if (dt.Rows.Count>0)
                {
                    model = new Common.Statistics.st_statistics();
                    model.add_time = Common.Utils.StrToDateTime(dt.Rows[0]["add_time"].ToString(), DateTime.Now);
                    model.from_open_id = dt.Rows[0]["from_open_id"].ToString();
                    model.db_table_name = "";
                    model.id = Common.Utils.StrToInt(dt.Rows[0]["id"].ToString(),0);
                    model.obj_id = Common.Utils.StrToInt(dt.Rows[0]["obj_id"].ToString(), 0);
                    model.obj_name = dt.Rows[0]["obj_name"].ToString();
                    model.open_id = dt.Rows[0]["open_id"].ToString();
                   
                    model.open_id = dt.Rows[0]["open_id"].ToString();
                    model.page_name = dt.Rows[0]["page_name"].ToString();
                    model.page_url = dt.Rows[0]["page_url"].ToString();
                    model.user_ip = dt.Rows[0]["user_ip"].ToString();
                    model.wx_id = Common.Utils.StrToInt(dt.Rows[0]["wx_id"].ToString(), 0);
                    model.wx_og_id = dt.Rows[0]["wx_og_id"].ToString();
                }
            }

            return model;
        }
    }
}
