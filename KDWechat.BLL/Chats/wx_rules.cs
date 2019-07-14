using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KDWechat.DAL;
using EntityFramework.Extensions;
using KDWechat.Common;
using System.Data.SqlClient;
using System.Data;
using KDWechat.DBUtility;
namespace KDWechat.BLL.Chats
{
    /// <summary>
    /// 关键词回复规则信息表
    /// </summary>
    public class wx_rules
    {
      
        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static t_wx_rules GetModel(int id)
        {
            t_wx_rules model = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                model = db.t_wx_rules.Where(x => x.id == id).FirstOrDefault();
            }
            return model;
        }



        /// <summary>
        /// 删除一条信息
        /// </summary>
        /// <param name="id">信息ID</param>
        /// <returns>是否删除成功</returns>
        public static bool Delete(int id)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isFinish = db.t_wx_rules.Where(x => x.id == id).Delete() > 0;
                if (isFinish)
                {
                    db.t_wx_rule_reply.Where(x => x.r_id == id).Delete();
                    db.t_wx_rules_keywords.Where(x => x.r_id == id).Delete();
                }
            }
            return isFinish;
        }

        /// <summary>
        /// 添加一条信息(事务处理)
        /// </summary>
        /// <param name="model">规则主表</param>
        /// <param name="model_key">规则关键词表</param>
        /// <param name="model_reply">规则的回复表</param>
        /// <returns>返回规则id，返回-1时表示获取微信公共号失败</returns>
        public static int Add(t_wx_rules model, List<t_wx_rules_keywords> list_key, t_wx_rule_reply reply)
        {
            int result_id = -1;
            var wechat = wx_wechats.GetWeChatByID(model.wx_id);//获取微信号信息
            if (wechat != null)
            {
                #region 规则主表
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into t_wx_rules(");
                strSql.Append("rule_name,wx_id,wx_og_id,u_id,reply_type,status,create_time,sort_id)");
                strSql.Append(" values (");
                strSql.Append("@rule_name,@wx_id,@wx_og_id,@u_id,@reply_type,@status,@create_time,@sort_id)");
                strSql.Append(";set @ReturnValue= @@IDENTITY");
                SqlParameter[] parameters = {
                    new SqlParameter("@rule_name", SqlDbType.NVarChar,50),
					new SqlParameter("@wx_id", SqlDbType.Int,4),
					new SqlParameter("@wx_og_id", SqlDbType.NVarChar,50),
					new SqlParameter("@u_id", SqlDbType.Int,4),
					new SqlParameter("@reply_type", SqlDbType.Int,4),
					new SqlParameter("@status", SqlDbType.Int,4),
					new SqlParameter("@create_time", SqlDbType.DateTime),
                    new SqlParameter("@sort_id", SqlDbType.Int,4),
                    new SqlParameter("@ReturnValue",SqlDbType.Int)};
                parameters[0].Value = model.rule_name;
                parameters[1].Value = model.wx_id;
                parameters[2].Value = model.wx_og_id;
                parameters[3].Value = model.u_id;
                parameters[4].Value = model.reply_type;
                parameters[5].Value = model.status;
                parameters[6].Value = model.create_time;
                parameters[7].Value = model.sort_id;
                parameters[8].Direction = ParameterDirection.Output;
                List<CommandInfo> sqllist = new List<CommandInfo>();
                CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
                sqllist.Add(cmd);

                #endregion

                #region 规则的关键字

                if (list_key != null)
                {
                    StringBuilder strSql_Child;
                    foreach (t_wx_rules_keywords m in list_key)
                    {
                        #region 循环加入关键词
                        strSql_Child = new StringBuilder();
                        strSql_Child.Append("insert into t_wx_rules_keywords(");
                        strSql_Child.Append("r_id,wx_id,wx_og_id,u_id,reply_type,key_words,eq_type)");
                        strSql_Child.Append(" values (");
                        strSql_Child.Append("@r_id,@wx_id,@wx_og_id,@u_id,@reply_type,@key_words,@eq_type)");
                        SqlParameter[] parameters_child = {
                                new SqlParameter("@r_id", SqlDbType.Int,4),
					            new SqlParameter("@wx_id", SqlDbType.Int,4),
					            new SqlParameter("@wx_og_id", SqlDbType.NVarChar,50),
					            new SqlParameter("@u_id", SqlDbType.Int,4),
					            new SqlParameter("@reply_type", SqlDbType.Int,4),
					            new SqlParameter("@key_words",SqlDbType.NVarChar,50),
					            new SqlParameter("@eq_type", SqlDbType.Int,4)
					          };
                        parameters_child[0].Direction = ParameterDirection.InputOutput;
                        parameters_child[1].Value = m.wx_id;
                        parameters_child[2].Value = m.wx_og_id;
                        parameters_child[3].Value = m.u_id;
                        parameters_child[4].Value = m.reply_type;
                        parameters_child[5].Value = m.key_words;
                        parameters_child[6].Value = m.eq_type;

                        cmd = new CommandInfo(strSql_Child.ToString(), parameters_child);
                        sqllist.Add(cmd);
                        #endregion
                    }
                }
                #endregion

                #region 规则的回复
                if (reply != null)
                {
                    StringBuilder strSqlReply = new StringBuilder();
                    strSqlReply.Append("insert into t_wx_rule_reply(");
                    strSqlReply.Append("r_id,wx_id,wx_og_id,contents,source_id,reply_type)");
                    strSqlReply.Append(" values (");
                    strSqlReply.Append("@r_id,@wx_id,@wx_og_id,@contents,@source_id,@reply_type)");
                    SqlParameter[] parametersReply = {
                    new SqlParameter("@r_id", SqlDbType.Int,4),
					new SqlParameter("@wx_id", SqlDbType.Int,4),
					new SqlParameter("@wx_og_id", SqlDbType.NVarChar,50),
					new SqlParameter("@contents", SqlDbType.NVarChar,1000),
					new SqlParameter("@source_id", SqlDbType.Int,4),
					new SqlParameter("@reply_type", SqlDbType.Int,4) };
                    parametersReply[0].Direction = ParameterDirection.InputOutput;
                    parametersReply[1].Value = reply.wx_id;
                    parametersReply[2].Value = reply.wx_og_id;
                    parametersReply[3].Value = reply.contents;
                    parametersReply[4].Value = reply.source_id;
                    parametersReply[5].Value = reply.reply_type;

                    cmd = new CommandInfo(strSqlReply.ToString(), parametersReply);
                    sqllist.Add(cmd);
                }
                #endregion

                KDWechat.DBUtility.DbHelperSQL.ExecuteSqlTranWithIndentity(sqllist);
                result_id = (int)parameters[8].Value;
            }
            return result_id;

        }

        /// <summary>
        /// 修改一条信息(事务处理)
        /// </summary>
        /// <param name="model">规则主表</param>
        /// <param name="model_key">规则关键词表</param>
        /// <param name="model_reply">规则的回复表</param>
        /// <returns></returns>
        public static bool Update(t_wx_rules model, List<t_wx_rules_keywords> list_key, t_wx_rule_reply reply)
        {
            bool result = true;


            using (SqlConnection conn = new SqlConnection(KDWechat.DBUtility.DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        #region 规则
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update t_wx_rules set ");
                        strSql.Append("rule_name=@rule_name,");
                        strSql.Append("u_id=@u_id,");
                        strSql.Append("reply_type=@reply_type,");
                        strSql.Append("status=@status,sort_id=@sort_id");
                        strSql.Append(" where id=@id");
                        SqlParameter[] parameters = {
					            new SqlParameter("@u_id", SqlDbType.Int,4),
					            new SqlParameter("@rule_name", SqlDbType.NVarChar,50),
					            new SqlParameter("@reply_type", SqlDbType.Int,4),
					            new SqlParameter("@status", SqlDbType.Int,4),
					            new SqlParameter("@id", SqlDbType.Int,4),
                                new SqlParameter("@sort_id", SqlDbType.Int,4)
                                                    };
                        parameters[0].Value = model.u_id;
                        parameters[1].Value = model.rule_name;
                        parameters[2].Value = model.reply_type;
                        parameters[3].Value = model.status;
                        parameters[4].Value = model.id;
                        parameters[5].Value = model.sort_id;
                        KDWechat.DBUtility.DbHelperSQL.ExecuteSql(conn, trans, strSql.ToString(), parameters);

                        #endregion

                        //删除关键字和回复数据
                        DeleteKeyAndReply(conn, trans, model.id);

                        #region 规则的关键字

                        if (list_key != null)
                        {
                            StringBuilder strSql_Child;
                            foreach (t_wx_rules_keywords m in list_key)
                            {
                                #region 循环加入关键词
                                strSql_Child = new StringBuilder();
                                strSql_Child.Append("insert into t_wx_rules_keywords(");
                                strSql_Child.Append("r_id,wx_id,wx_og_id,u_id,reply_type,key_words,eq_type)");
                                strSql_Child.Append(" values (");
                                strSql_Child.Append("@r_id,@wx_id,@wx_og_id,@u_id,@reply_type,@key_words,@eq_type)");
                                SqlParameter[] parameters_child = {
                                    new SqlParameter("@r_id", SqlDbType.Int,4),
					                new SqlParameter("@wx_id", SqlDbType.Int,4),
					                new SqlParameter("@wx_og_id", SqlDbType.NVarChar,50),
					                new SqlParameter("@u_id", SqlDbType.Int,4),
					                new SqlParameter("@reply_type", SqlDbType.Int,4),
					                new SqlParameter("@key_words",SqlDbType.NVarChar,50),
					                new SqlParameter("@eq_type", SqlDbType.Int,4)
					              };
                                parameters_child[0].Value = model.id;
                                parameters_child[1].Value = m.wx_id;
                                parameters_child[2].Value = m.wx_og_id;
                                parameters_child[3].Value = m.u_id;
                                parameters_child[4].Value = m.reply_type;
                                parameters_child[5].Value = m.key_words;
                                parameters_child[6].Value = m.eq_type;

                                KDWechat.DBUtility.DbHelperSQL.ExecuteSql(conn, trans, strSql_Child.ToString(), parameters_child);
                                #endregion
                            }
                        }
                        #endregion

                        #region 规则的回复
                        if (reply != null)
                        {
                            StringBuilder strSqlReply = new StringBuilder();
                            strSqlReply.Append("insert into t_wx_rule_reply(");
                            strSqlReply.Append("r_id,wx_id,wx_og_id,contents,source_id,reply_type)");
                            strSqlReply.Append(" values (");
                            strSqlReply.Append("@r_id,@wx_id,@wx_og_id,@contents,@source_id,@reply_type)");
                            SqlParameter[] parametersReply = {
                                    new SqlParameter("@r_id", SqlDbType.Int,4),
					                new SqlParameter("@wx_id", SqlDbType.Int,4),
					                new SqlParameter("@wx_og_id", SqlDbType.NVarChar,50),
					                new SqlParameter("@contents", SqlDbType.NVarChar,1000),
					                new SqlParameter("@source_id", SqlDbType.Int,4),
					                new SqlParameter("@reply_type", SqlDbType.Int,4) };
                            parametersReply[0].Value = model.id;
                            parametersReply[1].Value = reply.wx_id;
                            parametersReply[2].Value = reply.wx_og_id;
                            parametersReply[3].Value = reply.contents;
                            parametersReply[4].Value = reply.source_id;
                            parametersReply[5].Value = reply.reply_type;

                            KDWechat.DBUtility.DbHelperSQL.ExecuteSql(conn, trans, strSqlReply.ToString(), parametersReply);
                        }
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

        /// <summary>
        /// 删除关键字和回复数据
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="p"></param>
        private static void DeleteKeyAndReply(SqlConnection conn, SqlTransaction trans, int r_id)
        {
            if (r_id > 0)
            {
                KDWechat.DBUtility.DbHelperSQL.ExecuteSql(conn, trans, "delete from t_wx_rule_reply where  r_id=" + r_id); //删除回复信息
                KDWechat.DBUtility.DbHelperSQL.ExecuteSql(conn, trans, "delete from t_wx_rules_keywords where  r_id=" + r_id); //删除关键字信息
            }
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="id">需要更新的id</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public static bool UpdateStatus(int id, int status)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isFinish = db.t_wx_rules.Where(x => x.id == id).Update(x => new t_wx_rules() { status = status }) > 0;
            }
            return isFinish;
        }

        public static t_wx_rules GetModel(int wx_id, string wx_og_id, string keyword)
        {
            t_wx_rules model = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                int[] y = (from x in db.t_wx_rules_keywords where x.wx_og_id == wx_og_id && x.key_words == keyword select x.r_id).ToArray();
                if (y.Length>0)
                {
                    model = (from x in db.t_wx_rules where y.Contains(x.id) && x.wx_id == wx_id && x.wx_og_id == wx_og_id && x.status == 1 orderby x.sort_id descending select x).FirstOrDefault();
                }
               
                 
            }

            return model;
        }

        public static t_wx_rules GetModel( string wx_og_id, string keyword)
        {
            t_wx_rules model = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                int[] y = (from x in db.t_wx_rules_keywords where x.wx_og_id==wx_og_id &&  x.key_words == keyword select x.r_id).ToArray();
                if (y.Length > 0)
                {
                    model = (from x in db.t_wx_rules where y.Contains(x.id) &&  x.wx_og_id == wx_og_id && x.status == 1 orderby x.sort_id ascending,x.id descending select x).FirstOrDefault();
                }


            }

            return model;
        }
    }
}
