using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KDWechat.DAL;
using System.Linq.Expressions;
using EntityFramework.Extensions;
using LinqKit;
using System.Data.SqlClient;
using System.Data;
using Companycn.Core.DbHelper;

namespace KDWechat.BLL.Module
{
    public class v_invite
    {

        /// <summary>
        /// 删除微邀请活动(事务删除)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteInvite(int id)
        {
            bool result = true;
            using (SqlConnection conn = new SqlConnection(DbHelperSQLModule.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        #region 父级
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("delete from  t_v_invite_info   ");
                        strSql.Append(" where id=@id");
                        SqlParameter[] parameters = {
					             new SqlParameter("@id", SqlDbType.Int,4)   
                          };
                        parameters[0].Value = id;

                        DbHelperSQLModule.ExecuteSql(conn, trans, strSql.ToString(), parameters);

                        #endregion

                        #region 子级
                        StringBuilder strSql2 = new StringBuilder();
                        strSql2.Append("delete from t_v_invite_detail   ");
                        strSql2.Append(" where v_id=@id");
                        SqlParameter[] parameters2 = {
					             new SqlParameter("@id", SqlDbType.Int,4)   
                          };
                        parameters2[0].Value = id;
                        DbHelperSQLModule.ExecuteSql(conn, trans, strSql2.ToString(), parameters2);
                        #endregion

                        result = true;
                        trans.Commit();
                    }
                    catch (Exception)
                    {
                        result = false;
                        trans.Rollback();
                    }
                }
            }

            return result;
        }


        public static bool UpdateStatus(int id, int status)
        {
            bool result = true;

            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                result = db.t_v_invite_info.Where(x => x.id == id).Update<t_v_invite_info>(x => new t_v_invite_info() { status = status }) > 0;
            }
            return result;
        }

        /// <summary>
        /// 获取所有的项目图片
        /// </summary>
        /// <param name="project_id"></param>
        /// <returns></returns>
        public static List<t_v_invite_images> GetProjectImgages(int vid)
        {
            List<t_v_invite_images> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                list = (from x in db.t_v_invite_images where x.v_id == vid && x.channel_id == 2 select x).ToList();
            }
            return list;
        }


        public static void AddIamge(List<t_v_invite_images> imgages)
        {
            StringBuilder strSql2 = null;
            CommandInfo cmd;
            List<CommandInfo> sqllist = new List<CommandInfo>();
            foreach (t_v_invite_images m in imgages)
            {
                #region 循环加入项目内容

                strSql2 = new StringBuilder();
                strSql2.Append("insert into t_v_invite_images(");
                strSql2.Append("v_id,channel_id,channel_name,category_name,img_name,img_url,img_remark,sort_id,status)");
                strSql2.Append(" values (");
                strSql2.Append("@v_id,@channel_id,@channel_name,@category_name,@img_name,@img_url,@img_remark,99,1)");
                SqlParameter[] parameters2 = {
					new SqlParameter("@v_id", SqlDbType.Int,4),
					new SqlParameter("@channel_id", SqlDbType.Int,4),
					new SqlParameter("@channel_name", SqlDbType.NVarChar,50),
					new SqlParameter("@category_name", SqlDbType.NVarChar,50),
					new SqlParameter("@img_name", SqlDbType.NVarChar,50),
					new SqlParameter("@img_url", SqlDbType.NVarChar,150),
					 new SqlParameter("@img_remark", SqlDbType.NVarChar,500) };
                parameters2[0].Value = m.v_id;
                parameters2[1].Value = m.channel_id;
                parameters2[2].Value = m.channel_name;
                parameters2[3].Value = m.category_name;
                parameters2[4].Value = m.img_name;
                parameters2[5].Value = m.img_url;
                parameters2[6].Value = m.img_remark;

                cmd = new CommandInfo(strSql2.ToString(), parameters2);
                sqllist.Add(cmd);
                #endregion
            }
            if (sqllist.Count() > 0)
            {
                DbHelperSQLModule.ExecuteSqlTranWithIndentity(sqllist);
            }

        }

        public static bool UpdateIamge(List<t_v_invite_images> imgages, int id)
        {
            bool result = true;


            using (SqlConnection conn = new SqlConnection(DbHelperSQLModule.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {

                        #region 项目图片
                        if (imgages != null)
                        {
                            //先清空原先的
                            DeleteImgages(conn, trans, id);
                            foreach (t_v_invite_images m in imgages)
                            {
                                StringBuilder strSql2 = new StringBuilder();
                                strSql2.Append("insert into t_v_invite_images(");
                                strSql2.Append("v_id,channel_id,channel_name,category_name,img_name,img_url,img_remark,sort_id,status)");
                                strSql2.Append(" values (");
                                strSql2.Append("@v_id,@channel_id,@channel_name,@category_name,@img_name,@img_url,@img_remark,99,1)");
                                SqlParameter[] parameters2 = {
					                new SqlParameter("@v_id", SqlDbType.Int,4),
					                new SqlParameter("@channel_id", SqlDbType.Int,4),
					                new SqlParameter("@channel_name", SqlDbType.NVarChar,50),
					                new SqlParameter("@category_name", SqlDbType.NVarChar,50),
					                new SqlParameter("@img_name", SqlDbType.NVarChar,50),
					                new SqlParameter("@img_url", SqlDbType.NVarChar,150),
					                 new SqlParameter("@img_remark", SqlDbType.NVarChar,500) };
                                parameters2[0].Value = m.v_id;
                                parameters2[1].Value = m.channel_id;
                                parameters2[2].Value = m.channel_name;
                                parameters2[3].Value = m.category_name;
                                parameters2[4].Value = m.img_name;
                                parameters2[5].Value = m.img_url;
                                parameters2[6].Value = m.img_remark;

                                DbHelperSQLModule.ExecuteSql(conn, trans, strSql2.ToString(), parameters2);
                            }
                        }
                        #endregion

                        result = true;
                        trans.Commit();
                    }
                    catch (Exception )
                    {
                        result = false;
                        trans.Rollback();
                    }
                }
            }

            return result;
        }

        private static void DeleteImgages(SqlConnection conn, SqlTransaction trans, int id)
        {
            DbHelperSQLModule.ExecuteSql(conn, trans, "delete from t_v_invite_images where v_id=" + id + " and channel_id=2"); //删除数据库
        }


        public static void AddDefaultChildList(List<t_v_invite_detail> list)
        {
            StringBuilder strSql2 = null;
            CommandInfo cmd;
            List<CommandInfo> sqllist = new List<CommandInfo>();
            foreach (t_v_invite_detail m in list)
            {
                #region 循环加入项目内容

                strSql2 = new StringBuilder();
                strSql2.Append("insert into t_v_invite_detail(");
                strSql2.Append("v_id,channel_id,channel_name,app_id,display_name,sort_id,link_url,contents,status,lng,lat,app_link_url,app_html,is_multi_page,img_url,raffic,address)");
                strSql2.Append(" values (");
                strSql2.Append("@v_id,@channel_id,@channel_name,@app_id,@display_name,@sort_id,@link_url,@contents,@status,@lng,@lat,'','',0,'','',@address)");
                SqlParameter[] parameters2 = {
					new SqlParameter("@v_id", SqlDbType.Int,4),
					new SqlParameter("@channel_id", SqlDbType.Int,4),
					new SqlParameter("@channel_name", SqlDbType.NVarChar,50),
					new SqlParameter("@app_id", SqlDbType.NVarChar,150),
					new SqlParameter("@display_name", SqlDbType.NVarChar,50),
					new SqlParameter("@sort_id", SqlDbType.Int,4),
					new SqlParameter("@link_url", SqlDbType.NVarChar,250),
					new SqlParameter("@contents", SqlDbType.NVarChar,-1),
					new SqlParameter("@status", SqlDbType.Int,4),new SqlParameter("@lng", SqlDbType.NVarChar,50),new SqlParameter("@lat", SqlDbType.NVarChar,50),new SqlParameter("@address", SqlDbType.NVarChar,250)};
                parameters2[0].Value = m.v_id;
                parameters2[1].Value = m.channel_id;
                parameters2[2].Value = m.channel_name;
                parameters2[3].Value = m.app_id;
                parameters2[4].Value = m.display_name;
                parameters2[5].Value = m.sort_id;
                parameters2[6].Value = m.link_url;
                parameters2[7].Value = m.contents;
                parameters2[8].Value = m.status;
                parameters2[9].Value = m.lng;
                parameters2[10].Value = m.lat;
                parameters2[11].Value = m.address;

                cmd = new CommandInfo(strSql2.ToString(), parameters2);
                sqllist.Add(cmd);
                #endregion
            }
            if (sqllist.Count() > 0)
            {
                DbHelperSQLModule.ExecuteSqlTranWithIndentity(sqllist);
            }
        }
    }
}
