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
    ///图文模板表
    /// </summary>
    public class wx_templates
    {
       
        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static t_wx_templates GetModel(int id)
        {
            t_wx_templates model = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                model = db.t_wx_templates.Where(x => x.id == id).FirstOrDefault();
            }
            return model;
        }

        /// <summary>
        /// 检测是否存在
        /// </summary>
        /// <param name="title"></param>
        /// <param name="?"></param>
        /// <param name="wx_id"></param>
        /// <returns></returns>
        public static bool Exists(string title, int channel_id,int cate_id, int wx_id)
        {
            bool isc = false;
            string sql = "select count(*) from t_wx_templates where id in(select template_id from t_wx_templates_wechats where wx_id=" + wx_id + ") and title='" + title + "' and channel_id=" + channel_id + " and cate_id=" + cate_id;
            isc = KDWechat.DBUtility.DbHelperSQL.Exists(sql);
            return isc;
        }


        /// <summary>
        /// 获取分配的微信id
        /// </summary>
        /// <param name="template_id"></param>
        /// <returns></returns>
        public static List<string> GetWxIdListBytemplateId(int template_id)
        {
            List<string> list = new List<string>();
            using (creater_wxEntities db = new creater_wxEntities())
            {
                int[] result = (from x in db.t_wx_templates_wechats where x.template_id == template_id && x.channel_id == 1 select x.wx_id).ToArray();
                if (result != null)
                {
                    for (int i = 0; i < result.Length; i++)
                    {
                        list.Add(result[i].ToString());
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static t_wx_templates GetModel(int id, int status)
        {
            t_wx_templates model = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                model = db.t_wx_templates.Where(x => x.id == id && x.status == status).FirstOrDefault();
            }
            return model;
        }
        /// <summary>
        /// 获取当前默认的模板
        /// </summary>
        /// <returns></returns>
        public static t_wx_templates GetDefaultModel()
        {
            t_wx_templates model = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                model = db.t_wx_templates.Where(x => x.cate_id == 0 && x.channel_id == 1 && x.is_default == 1 && x.status == 1).Take(1).FirstOrDefault();
            }
            return model;
        }

        /// <summary>
        /// 获取微信号当前默认的模板
        /// </summary>
        /// <returns></returns>
        public static t_wx_templates GetWXDefaultModel(int wx_id)
        {
            t_wx_templates model = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                int template_id = 0;
                template_id = (from x in db.t_wx_templates_wechats where x.wx_id == wx_id && x.is_default == 1 && x.channel_id == 1 select x.template_id).Take(1).FirstOrDefault();
                if (template_id > 0)
                {
                    model = db.t_wx_templates.Where(x => x.id == template_id && x.channel_id == 1 && x.status == 1).FirstOrDefault();
                }

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
                isFinish = db.t_wx_templates.Where(x => x.id == id).Delete() > 0;
                if (isFinish)
                {
                    //删除关联
                    db.t_wx_templates_wechats.Where(x => x.template_id == id).Delete();
                }
            }
            return isFinish;
        }

        /// <summary>
        /// 添加一条系统模版
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_wx_templates Add(t_wx_templates model)
        {

            model = EFHelper.AddWeChat<t_wx_templates>(model,false);

            return model;//返回添加后的信息
        }

        /// <summary>
        /// 添加一条自定义模版
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int Add(t_wx_templates model, t_wx_templates_wechats templates_wechats)
        {

            int result_id = -1;
            var wechat = wx_wechats.GetWeChatByID(templates_wechats.wx_id);//获取微信号信息
            if (wechat != null)
            {
                #region 模版主表
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into t_wx_templates(");
                strSql.Append("title,channel_id,cate_id,img_url,file_path,contents,is_default,status,create_time,sort_id)");
                strSql.Append(" values (");
                strSql.Append("@title,@channel_id,@cate_id,@img_url,@file_path,@contents,@is_default,@status,@create_time,@sort_id)");
                strSql.Append(";set @ReturnValue= @@IDENTITY");
                SqlParameter[] parameters = {
                    new SqlParameter("@title", SqlDbType.NVarChar,50),
					new SqlParameter("@channel_id", SqlDbType.Int,4),
					new SqlParameter("@cate_id", SqlDbType.Int,4),
                    new SqlParameter("@img_url", SqlDbType.NVarChar,500),
                    new SqlParameter("@file_path", SqlDbType.NVarChar,500),
                     new SqlParameter("@contents", SqlDbType.NVarChar),
					new SqlParameter("@is_default", SqlDbType.Int,4),
					new SqlParameter("@status", SqlDbType.Int,4),
					new SqlParameter("@create_time", SqlDbType.DateTime),
                    new SqlParameter("@sort_id", SqlDbType.Int,4),
                    new SqlParameter("@ReturnValue",SqlDbType.Int)};
                parameters[0].Value = model.title;
                parameters[1].Value = model.channel_id;
                parameters[2].Value = model.cate_id;
                parameters[3].Value = model.img_url;
                parameters[4].Value = model.file_path;
                parameters[5].Value = model.contents;
                parameters[6].Value = model.is_default;
                parameters[7].Value = model.status;
                parameters[8].Value = model.create_time;
                parameters[9].Value = model.sort_id;
                parameters[10].Direction = ParameterDirection.Output;
                List<CommandInfo> sqllist = new List<CommandInfo>();
                CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
                sqllist.Add(cmd);

                #endregion



                #region 模版与微信公众号关系表
                if (templates_wechats != null)
                {
                    StringBuilder strSqlReply = new StringBuilder();
                    strSqlReply.Append("insert into t_wx_templates_wechats(");
                    strSqlReply.Append("channel_id,wx_id,wx_og_id,template_id,is_default)");
                    strSqlReply.Append(" values (");
                    strSqlReply.Append("@channel_id,@wx_id,@wx_og_id,@template_id,@is_default)");
                    SqlParameter[] parametersReply = {
					new SqlParameter("@wx_id", SqlDbType.Int,4),
					new SqlParameter("@wx_og_id", SqlDbType.NVarChar,50),
					new SqlParameter("@template_id", SqlDbType.Int,4),
					new SqlParameter("@is_default", SqlDbType.Int,4),new SqlParameter("@channel_id", SqlDbType.Int,4) };
                    parametersReply[0].Value = templates_wechats.wx_id;
                    parametersReply[1].Value = wechat.wx_og_id;
                    parametersReply[2].Direction = ParameterDirection.InputOutput;
                    parametersReply[3].Value = templates_wechats.is_default;
                    parametersReply[4].Value = templates_wechats.channel_id;
                    cmd = new CommandInfo(strSqlReply.ToString(), parametersReply);
                    sqllist.Add(cmd);
                }
                #endregion

                KDWechat.DBUtility.DbHelperSQL.ExecuteSqlTranWithIndentity(sqllist,false);
                result_id = (int)parameters[10].Value;
            }
            return result_id;
        }

        /// <summary>
        /// 添加模板与公众号的关系
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static int AddTemplatesWechats(List<t_wx_templates_wechats> list, int template_id)
        {
            int result_id = -1;
            if (list != null)
            {
                List<CommandInfo> sqllist = new List<CommandInfo>();

                StringBuilder strSql = new StringBuilder();
                strSql.Append("DELETE  t_wx_templates_wechats ");
                strSql.Append(" where template_id=@template_id and channel_id=1");

                SqlParameter[] parameters = {
					new SqlParameter("@template_id", SqlDbType.Int,4) };
                parameters[0].Value = template_id;


                CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
                sqllist.Add(cmd);

                foreach (t_wx_templates_wechats m in list)
                {
                    StringBuilder strSqlReply = new StringBuilder();
                    strSqlReply.Append("insert into t_wx_templates_wechats(");
                    strSqlReply.Append("channel_id,wx_id,wx_og_id,template_id,is_default)");
                    strSqlReply.Append(" values (");
                    strSqlReply.Append("@channel_id,@wx_id,@wx_og_id,@template_id,@is_default)");
                    SqlParameter[] parametersReply = {
					new SqlParameter("@wx_id", SqlDbType.Int,4),
					new SqlParameter("@wx_og_id", SqlDbType.NVarChar,50),
					new SqlParameter("@template_id", SqlDbType.Int,4),
					new SqlParameter("@is_default", SqlDbType.Int,4),new SqlParameter("@channel_id", SqlDbType.Int,4)  };
                    parametersReply[0].Value = m.wx_id;
                    parametersReply[1].Value = m.wx_og_id;
                    parametersReply[2].Value = m.template_id;
                    parametersReply[3].Value = m.is_default;
                    parametersReply[4].Value = m.channel_id;
                     cmd = new CommandInfo(strSqlReply.ToString(), parametersReply);
                    sqllist.Add(cmd);
                }

                result_id = KDWechat.DBUtility.DbHelperSQL.ExecuteSqlTran(sqllist);

            }
            return result_id;
        }

        /// <summary>
        /// 修改一条信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool Update(t_wx_templates model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update t_wx_templates set ");
            strSql.Append("title=@title,");
            strSql.Append("channel_id=@channel_id,");
            strSql.Append("cate_id=@cate_id,");
            strSql.Append("img_url=@img_url,");
            strSql.Append("file_path=@file_path,");
            strSql.Append("remark=@remark,");
            strSql.Append("contents=@contents,");
            strSql.Append("is_default=@is_default,");
            strSql.Append("status=@status,");
            strSql.Append("sort_id=@sort_id");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
					new SqlParameter("@title", SqlDbType.NVarChar,50),
					new SqlParameter("@channel_id", SqlDbType.Int,4),
					new SqlParameter("@cate_id", SqlDbType.Int,4),
					new SqlParameter("@img_url", SqlDbType.NVarChar,500),
					new SqlParameter("@file_path", SqlDbType.NVarChar,500),
					new SqlParameter("@remark", SqlDbType.NVarChar,500),
					new SqlParameter("@contents", SqlDbType.NVarChar,-1),
					new SqlParameter("@is_default", SqlDbType.Int,4),
					new SqlParameter("@status", SqlDbType.Int,4),
					new SqlParameter("@sort_id", SqlDbType.Int,4),
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = model.title;
            parameters[1].Value = model.channel_id;
            parameters[2].Value = model.cate_id;
            parameters[3].Value = model.img_url;
            parameters[4].Value = model.file_path;
            parameters[5].Value = model.remark;
            parameters[6].Value = model.contents;
            parameters[7].Value = model.is_default;
            parameters[8].Value = model.status;
            parameters[9].Value = model.sort_id;
            parameters[10].Value = model.id;

            int rows = KDWechat.DBUtility.DbHelperSQL.ExecuteSql(strSql.ToString(), parameters,false);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 设置系统模板的默认模板
        /// </summary>
        /// <param name="id"></param>
        public static void SetDefaultWithSystem(int id)
        {
            using (creater_wxEntities db = new creater_wxEntities())
            {
                int num = db.t_wx_templates.Where(x => x.id == id).Update(x => new t_wx_templates { is_default = 1 });
                if (num > 0)
                {
                    db.t_wx_templates.Where(x => x.channel_id == 1 && x.cate_id == 0 && x.id != id).Update(x => new t_wx_templates { is_default = 0 });
                }
            }
        }

        /// <summary>
        /// 设置公众号的默认模板
        /// </summary>
        /// <param name="id">关系i的</param>
        /// <param name="template_id">模板id</param>
        /// <param name="wx_id">微信id</param>
        public static void SetDefaultWithWX(int id, int template_id, int wx_id)
        {
            using (creater_wxEntities db = new creater_wxEntities())
            {
                int num = db.t_wx_templates_wechats.Where(x => x.template_id == template_id && x.wx_id == wx_id).Update(x => new t_wx_templates_wechats { is_default = 1 });
                if (num > 0)
                {
                    db.t_wx_templates_wechats.Where(x => x.wx_id == wx_id && x.id != id).Update(x => new t_wx_templates_wechats { is_default = 0 });
                }
            }
        }

        public static void SetStatus(int id, int status)
        {
            using (creater_wxEntities db = new creater_wxEntities())
            {
                int num = db.t_wx_templates.Where(x => x.id == id).Update(x => new t_wx_templates { status = status });

            }
        }
        /// <summary>
        /// 删除模板与微信号的关联
        /// </summary>
        /// <param name="p"></param>
        public static bool DeleteTemplateWechat(int id)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                //删除关联
                isFinish = db.t_wx_templates_wechats.Where(x => x.id == id).Delete() > 0;
            }
            return isFinish;
        }

        public static t_wx_templates_wechats GetTemplateWechatModel(int template_id, int wx_id)
        {
            t_wx_templates_wechats model = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                model = db.t_wx_templates_wechats.Where(x => x.template_id == template_id && x.wx_id == wx_id).Take(1).FirstOrDefault();
            }
            return model;

        }

        public static List<t_wx_templates> GetList(int channel_id)
        {
            List<t_wx_templates> list = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                list = db.t_wx_templates.Where(x => x.channel_id == channel_id).ToList();
            }
            return list;
        }


        /// <summary>
        /// 根据项目类型获取项目列表的模版
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        public static DataTable GetModelByTypeID(int tp)
        {
            string sql = "select * from t_wx_templates where id= (select top 1 seo_keywords from t_category  where channel_id=2 and call_index="+tp+")";
            return KDWechat.DBUtility.DbHelperSQL.Query(sql).Tables[0];
        }
    }
}
