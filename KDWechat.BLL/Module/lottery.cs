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
using Companycn.Core.DbHelper;
using System.Data;

namespace KDWechat.BLL.Module
{
    public class lottery
    {
        /// <summary>
        /// 根据类别查询抽奖活动列表
        /// </summary>
        /// <param name="channel_id"></param>
        /// <returns></returns>
        public static List<t_Lottery> GetList(int channel_id)
        {
            List<t_Lottery> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                list = (from x in db.t_Lottery where x.channel_id == channel_id select x).OrderByDescending(y => y.id).ToList();
            }
            return list;
        }

        /// <summary>
        /// 根据编号查询抽奖信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static t_Lottery GetModel(int id)
        {
            t_Lottery model = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                model = db.t_Lottery.Where(x => x.id == id).FirstOrDefault();
            }
            return model;
        }

        /// <summary>
        /// 添加一条抽奖活动
        /// </summary>
        /// <param name="lotModel"></param>
        /// <param name="prizeModel"></param>
        /// <returns></returns>
        public static int addTrans(t_Lottery lotModel, List<t_prizeinfo> list)
        {


            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into t_lottery(");
            strSql.Append(" channel_id, title, star_desction, end_desction, dj_information, theam, star_time, end_time, str_img, end_img, more_replies, p_num, lottery_num, is_lock,is_continue,out_link,is_out,wx_ogid,is_everyDay )");
            strSql.Append(" values (");
            strSql.Append("@channel_id, @title, @star_desction, @end_desction, @dj_information, @theam, @star_time, @end_time, @str_img, @end_img, @more_replies, @p_num, @lottery_num, @is_lock,@is_continue,@out_link,@is_out,@wx_ogid,@is_everyDay)");
            strSql.Append(";set @ReturnValue= @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@channel_id", SqlDbType.Int,4),
					new SqlParameter("@title", SqlDbType.NVarChar,50),
					new SqlParameter("@star_desction", SqlDbType.NVarChar),
					new SqlParameter("@end_desction", SqlDbType.NVarChar),
					new SqlParameter("@dj_information", SqlDbType.NVarChar,1000),
					new SqlParameter("@theam", SqlDbType.NVarChar,1000),
					new SqlParameter("@star_time", SqlDbType.DateTime),
					new SqlParameter("@end_time", SqlDbType.DateTime),
					new SqlParameter("@str_img", SqlDbType.NVarChar,255),
					new SqlParameter("@end_img", SqlDbType.NVarChar,255),
					new SqlParameter("@more_replies", SqlDbType.NVarChar,50),
					new SqlParameter("@p_num", SqlDbType.Int,8),
					new SqlParameter("@lottery_num", SqlDbType.Int,4),
					new SqlParameter("@is_lock", SqlDbType.Int,4),
                    new SqlParameter("@is_continue", SqlDbType.Int,4),
                    new SqlParameter("@out_link", SqlDbType.NVarChar,255),
                    new SqlParameter("@is_out", SqlDbType.Int,4),
                    new SqlParameter("@wx_ogid", SqlDbType.NVarChar,80),
                    new SqlParameter("@is_everyDay", SqlDbType.Int,4),
                    new SqlParameter("@ReturnValue",SqlDbType.Int)};
            parameters[0].Value = lotModel.channel_id;
            parameters[1].Value = lotModel.title;
            parameters[2].Value = lotModel.star_desction;
            parameters[3].Value = lotModel.end_desction;
            parameters[4].Value = lotModel.dj_information;
            parameters[5].Value = lotModel.theam;
            parameters[6].Value = lotModel.star_time;
            parameters[7].Value = lotModel.end_time;
            parameters[8].Value = lotModel.str_img;
            parameters[9].Value = lotModel.end_img;
            parameters[10].Value = lotModel.more_replies;
            parameters[11].Value = lotModel.p_num;
            parameters[12].Value = lotModel.lottery_num;
            parameters[13].Value = lotModel.is_lock;
            parameters[14].Value = lotModel.is_continue;
            parameters[15].Value = lotModel.out_link;
            parameters[16].Value = lotModel.is_out;
            parameters[17].Value = lotModel.wx_ogid;
            parameters[18].Value = lotModel.is_everyDay;
            parameters[19].Direction = ParameterDirection.Output;

            List<CommandInfo> sqllist = new List<CommandInfo>();
            CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
            sqllist.Add(cmd);

            if (list.Count > 0)
            {
                StringBuilder strSql2;
                foreach (var item in list)
                {
                    strSql2 = new StringBuilder();
                    strSql2.Append("insert into t_prizeinfo(");
                    strSql2.Append("lottery_id,prize_type,prize_name,probability,prize_num,prize_url)");
                    strSql2.Append(" values (");
                    strSql2.Append("@lottery_id,@prize_type,@prize_name,@probability,@prize_num,@prize_url)");

                    SqlParameter[] parameters2 = {
					        new SqlParameter("@lottery_id", SqlDbType.Int,4),
					        new SqlParameter("@prize_type", SqlDbType.NVarChar,50),
                            new SqlParameter("@prize_name", SqlDbType.NVarChar,50),
                            new SqlParameter("@probability", SqlDbType.Float,8),
					        new SqlParameter("@prize_num", SqlDbType.Int,4),
                            new SqlParameter("@prize_url", SqlDbType.NVarChar,255)};

                    parameters2[0].Direction = ParameterDirection.InputOutput;
                    parameters2[1].Value = item.prize_type;
                    parameters2[2].Value = item.prize_name;
                    parameters2[3].Value = item.probability;
                    parameters2[4].Value = item.prize_num;
                    parameters2[5].Value = item.prize_url;
                    cmd = new CommandInfo(strSql2.ToString(), parameters2);
                    sqllist.Add(cmd);
                }
            }

            DbHelperSQLModule.ExecuteSqlTranWithIndentity(sqllist);
            return (int)parameters[19].Value;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="lotModel"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool updateTrans(t_Lottery lotModel, List<t_prizeinfo> list)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQLModule.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update t_Lottery set ");
                        strSql.Append("channel_id=@channel_id,");
                        strSql.Append("title=@title,");
                        strSql.Append("star_desction=@star_desction,");
                        strSql.Append("end_desction=@end_desction,");
                        strSql.Append("dj_information=@dj_information,");
                        strSql.Append("theam=@theam,");
                        strSql.Append("star_time=@star_time,");
                        strSql.Append("end_time=@end_time,");
                        strSql.Append("str_img=@str_img,");
                        strSql.Append("end_img=@end_img,");
                        strSql.Append("more_replies=@more_replies,");
                        strSql.Append("p_num=@p_num,");
                        strSql.Append("lottery_num=@lottery_num,");
                        strSql.Append("is_lock=@is_lock,");
                        strSql.Append("is_continue=@is_continue,");
                        strSql.Append("out_link=@out_link,");
                        strSql.Append("is_out=@is_out,");
                        strSql.Append("is_everyDay=@is_everyDay,");
                        strSql.Append("wx_ogid=@wx_ogid");
                        strSql.Append(" where id=@id");
                            SqlParameter[] parameters = {
					        new SqlParameter("@channel_id", SqlDbType.Int,4),
					        new SqlParameter("@title", SqlDbType.NVarChar,50),
					        new SqlParameter("@star_desction", SqlDbType.NVarChar),
					        new SqlParameter("@end_desction", SqlDbType.NVarChar),
					        new SqlParameter("@dj_information", SqlDbType.NVarChar,1000),
					        new SqlParameter("@theam", SqlDbType.NVarChar,1000),
					        new SqlParameter("@star_time", SqlDbType.DateTime),
					        new SqlParameter("@end_time", SqlDbType.DateTime),
					        new SqlParameter("@str_img", SqlDbType.NVarChar,255),
					        new SqlParameter("@end_img", SqlDbType.NVarChar,255),
					        new SqlParameter("@more_replies", SqlDbType.NVarChar,50),
					        new SqlParameter("@p_num", SqlDbType.Int,8),
					        new SqlParameter("@lottery_num", SqlDbType.Int,4),
					        new SqlParameter("@is_lock", SqlDbType.Int,4),
                            new SqlParameter("@is_continue", SqlDbType.Int,4),
                            new SqlParameter("@out_link", SqlDbType.NVarChar,255),
                            new SqlParameter("@is_out", SqlDbType.Int,4),
                            new SqlParameter("@is_everyDay", SqlDbType.Int,4),
                            new SqlParameter("@wx_ogid", SqlDbType.NVarChar,80),
                            new SqlParameter("@id", SqlDbType.Int,8)};
                                parameters[0].Value = lotModel.channel_id;
                                parameters[1].Value = lotModel.title;
                                parameters[2].Value = lotModel.star_desction;
                                parameters[3].Value = lotModel.end_desction;
                                parameters[4].Value = lotModel.dj_information;
                                parameters[5].Value = lotModel.theam;
                                parameters[6].Value = lotModel.star_time;
                                parameters[7].Value = lotModel.end_time;
                                parameters[8].Value = lotModel.str_img;
                                parameters[9].Value = lotModel.end_img;
                                parameters[10].Value = lotModel.more_replies;
                                parameters[11].Value = lotModel.p_num;
                                parameters[12].Value = lotModel.lottery_num;
                                parameters[13].Value = lotModel.is_lock;
                                parameters[14].Value = lotModel.is_continue;
                                parameters[15].Value = lotModel.out_link;
                                parameters[16].Value = lotModel.is_out;
                                parameters[17].Value = lotModel.is_everyDay;
                                parameters[18].Value = lotModel.wx_ogid;
                                parameters[19].Value = lotModel.id;

                            DbHelperSQLModule.ExecuteSql(conn, trans, strSql.ToString(), parameters);

                            StringBuilder strsql3 = new StringBuilder(); ;
                            strsql3.Append("delete t_prizeinfo where lottery_id=@lottery_id");
                            SqlParameter[] parameters3={new SqlParameter("@lottery_id", SqlDbType.Int,4)};
                            parameters3[0].Value = lotModel.id;
                            DbHelperSQLModule.ExecuteSql(conn, trans, strsql3.ToString(), parameters3);

                            if (list.Count > 0)
                            {
                                StringBuilder strSql2;
                                foreach (var item in list)
                                {
                                    //if (item.id > 0)
                                    //{
                                    //    strSql2 = new StringBuilder();
                                    //    strSql2.Append("update t_prizeinfo set ");
                                    //    strSql2.Append("lottery_id=@lottery_id,");
                                    //    strSql2.Append("prize_type=@prize_type,");
                                    //    strSql2.Append("prize_name=@prize_name,");
                                    //    strSql2.Append("probability=@probability,");
                                    //    strSql2.Append("prize_num=@prize_num,");
                                    //    strSql2.Append("prize_url=@prize_url");
                                    //    strSql2.Append(" where id=@id");
                                    //    SqlParameter[] parameters2 = {
                                    //        new SqlParameter("@lottery_id", SqlDbType.Int,4),
                                    //        new SqlParameter("@prize_type", SqlDbType.NVarChar,50),
                                    //        new SqlParameter("@prize_name", SqlDbType.NVarChar,50),
                                    //        new SqlParameter("@probability", SqlDbType.Float,8),
                                    //        new SqlParameter("@prize_num", SqlDbType.Int,4),
                                    //        new SqlParameter("@prize_url", SqlDbType.NVarChar,255),
                                    //        new SqlParameter("@id", SqlDbType.Int,4)};

                                    //    parameters2[0].Value = item.lottery_id;
                                    //    parameters2[1].Value = item.prize_type ;
                                    //    parameters2[2].Value = item.prize_name ;
                                    //    parameters2[3].Value = item.probability;
                                    //    parameters2[4].Value = item.prize_num;
                                    //    parameters2[5].Value = item.prize_url;
                                    //    parameters2[6].Value = item.id;
                                    //    DbHelperSQLModule.ExecuteSql(conn, trans, strSql2.ToString(), parameters2);
                                    //}
                                    //else
                                    //{
                                        strSql2 = new StringBuilder();
                                        strSql2.Append("insert into t_prizeinfo(");
                                        strSql2.Append("lottery_id,prize_type,prize_name,probability,prize_num,pazz_num,prize_url)");
                                        strSql2.Append(" values (");
                                        strSql2.Append("@lottery_id,@prize_type,@prize_name,@probability,@prize_num,@pazz_num,@prize_url)");

                                        SqlParameter[] parameters2 = {
					                        new SqlParameter("@lottery_id", SqlDbType.Int,4),
                                            new SqlParameter("@prize_type", SqlDbType.NVarChar,50),
					                        new SqlParameter("@prize_name", SqlDbType.NVarChar,50),
                                            new SqlParameter("@probability", SqlDbType.Float,8),
					                        new SqlParameter("@prize_num", SqlDbType.Int,4),
                                            new SqlParameter("@pazz_num", SqlDbType.Int,4),
                                            new SqlParameter("@prize_url", SqlDbType.NVarChar,255)};

                                        parameters2[0].Value = item.lottery_id;
                                        parameters2[1].Value = item.prize_type;
                                        parameters2[2].Value = item.prize_name; 
                                        parameters2[3].Value = item.probability;
                                        parameters2[4].Value = item.prize_num;
                                        parameters2[5].Value = item.pazz_num;
                                        parameters2[6].Value = item.prize_url;
                                        DbHelperSQLModule.ExecuteSql(conn, trans, strSql2.ToString(), parameters2);
                                   // }
                                }
                            }
                            trans.Commit();
                    }
                    catch 
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int DelTrans(int id)
        {
            int ret = 0;
            List<CommandInfo> sqllist = new List<CommandInfo>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from t_Lottery ");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,8)
            };
            parameters[0].Value = id;
            CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
            sqllist.Add(cmd);

            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append("delete from t_prizeinfo ");
            strSql2.Append(" where lottery_id=@lottery_id");
            SqlParameter[] parameters2 = {
					new SqlParameter("@lottery_id", SqlDbType.Int,8)
            };
            parameters2[0].Value = id;
            cmd = new CommandInfo(strSql2.ToString(), parameters2);
            sqllist.Add(cmd);
            ret =DbHelperSQLModule.ExecuteSqlTran(sqllist);
            return ret;
        }
        /// <summary>
        /// 添加信息是同步添加
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="modeule_id"></param>
        /// <param name="ogId"></param>
        /// <param name="wxId"></param>
        /// <param name="appId"></param>
        /// <param name="parentId"></param>
        /// <param name="tableName"></param>
        /// <param name="appName"></param>
        /// <param name="appImg"></param>
        /// <param name="linkUrl"></param>
        /// <param name="remark"></param>
        /// <param name="u_id"></param>
        /// <param name="status"></param>
        public static void insertWechat(int channelId,int modeule_id,string ogId,int wxId,int appId,int parentId,string tableName,string appName,string appImg,string linkUrl,string remark,int u_id,int status)
        {
            DAL.t_module_wechat modelWec = new DAL.t_module_wechat();
            modelWec.channel_id = channelId;
            modelWec.module_id = modeule_id;
            modelWec.wx_og_id = ogId;
            modelWec.wx_id = wxId;
            modelWec.app_id = appId;
            modelWec.app_parent_id = parentId;
            modelWec.app_table = tableName;
            modelWec.app_name = appName;
            modelWec.app_img_url = appImg;
            modelWec.app_link_url = linkUrl;
            modelWec.app_remark = remark;
            modelWec.u_id = u_id;
            modelWec.status = status;
            modelWec.create_time = DateTime.Now;
            modelWec = BLL.Chats.module_wechat.Add(modelWec);
        }
        /// <summary>
        /// 修改时同步修改
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="modeule_id"></param>
        /// <param name="ogId"></param>
        /// <param name="wxId"></param>
        /// <param name="appId"></param>
        /// <param name="parentId"></param>
        /// <param name="tableName"></param>
        /// <param name="appName"></param>
        /// <param name="appImg"></param>
        /// <param name="linkUrl"></param>
        /// <param name="remark"></param>
        /// <param name="u_id"></param>
        /// <param name="status"></param>
        public static void UpdateWechat(int channelId, int modeule_id, string ogId, int wxId, int appId, int parentId, string tableName, string appName, string appImg, string linkUrl, string remark, int u_id, int status)
        {
            DAL.t_module_wechat modelWec = BLL.Chats.module_wechat.GetModelForViewBuid(wxId, appId, parentId, tableName);
            if (modelWec != null)
            {
                modelWec.channel_id = channelId;
                modelWec.module_id = modeule_id;
                modelWec.wx_og_id = ogId;
                modelWec.wx_id = wxId;
                modelWec.app_id = appId;
                modelWec.app_parent_id = parentId;
                modelWec.app_table = tableName;
                modelWec.app_name = appName;
                modelWec.app_img_url = appImg;
                modelWec.app_link_url = linkUrl;
                modelWec.app_remark = remark;
                modelWec.u_id = u_id;
                modelWec.status = status;
                modelWec.create_time = DateTime.Now;
                modelWec = BLL.Chats.module_wechat.Update(modelWec);
            }
            else
            {

                DAL.t_module_wechat modelWechat = new DAL.t_module_wechat();
                modelWechat.channel_id = channelId;
                modelWechat.module_id = modeule_id;
                modelWechat.wx_og_id = ogId;
                modelWechat.wx_id = wxId;
                modelWechat.app_id = appId;
                modelWechat.app_parent_id = parentId;
                modelWechat.app_table = tableName;
                modelWechat.app_name = appName;
                modelWechat.app_img_url = appImg;
                modelWechat.app_link_url = linkUrl;
                modelWechat.app_remark = remark;
                modelWechat.u_id = u_id;
                modelWechat.status = status;
                modelWechat.create_time = DateTime.Now;
                modelWechat = BLL.Chats.module_wechat.Add(modelWechat);
            }
          
        }
    }
}
