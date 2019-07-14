using KDWechat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityFramework.Extensions;
using System.Linq.Expressions;
using LinqKit;
using KDWechat.DBUtility;
using System.Data.SqlClient;
using System.Data;

namespace KDWechat.BLL.Chats
{
    public class modules
    {
        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static t_modules GetModel(int id)
        {
            t_modules material = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                material = db.t_modules.Where(x => x.id == id).FirstOrDefault();
            }
            return material;
        }

        //提取列表
        public static List<t_modules> GetList(Expression<Func<t_modules, bool>> where,int pagesize,int pageinde,out int totalCount)
        {
            List<t_modules> list = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                var query = db.t_modules.Where(where.Expand());
                totalCount = query.Count();
                list = query.OrderByDescending(x => x.id).Skip((pageinde - 1) * pagesize).Take(pagesize).ToList();
            }
            return list;

        }

        //移除或添加module(禁用/启用)
        public static t_modules RemoveOrAddModule(int id)
        {
            t_modules module = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                module = (from x in db.t_modules where x.id == id select x).FirstOrDefault();
                if (module != null)
                {
                    if (module.status == (int)Common.Status.禁用)
                        module.status = (int)Common.Status.正常;
                    else
                        module.status = (int)Common.Status.禁用;
                    db.SaveChanges();
                }
            }
            return module;
        }



        /// <summary>
        /// 删除一条消息
        /// </summary>
        /// <param name="id">消息ID</param>
        /// <returns>是否删除成功</returns>
        public static bool Delete(int id)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isFinish = db.t_modules.Where(x => x.id == id).Delete() > 0;
            }
            return isFinish;
        }

        /// <summary>
        /// 添加一条消息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_modules Add(t_modules model)
        {

            model = EFHelper.AddWeChat<t_modules>(model);
            return model;//返回添加后的消息
        }

        /// <summary>
        /// 修改一条消息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_modules Update(t_modules model)
        {
            if (model != null)
            {
                EFHelper.UpdateWeChat<t_modules>(model);
            }
            return model;
        }

        /// <summary>
        /// 获取微信号下该应用的授权用户id列表
        /// </summary>
        /// <param name="module_id"></param>
        /// <param name="wx_id"></param>
        /// <returns></returns>
        public static List<string> GetModuleWeChatUser(int module_id, int wx_id)
        {
            List<string> list = new List<string>();
            using (creater_wxEntities db = new creater_wxEntities())
            {
                int[] result = (from x in db.t_module_wx_user_role where x.module_id == module_id && x.wx_id == wx_id select x.user_id).ToArray();
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

        public static int AddModuleWeChatUser(List<t_module_wx_user_role> list, int module_id, int wx_id)
        {
            int result_id = -1;
            if (list != null)
            {
                List<CommandInfo> sqllist = new List<CommandInfo>();

                StringBuilder strSql = new StringBuilder();
                strSql.Append("DELETE  t_module_wx_user_role ");
                strSql.Append(" where module_id=@module_id and wx_id=@wx_id");

                SqlParameter[] parameters = {
					new SqlParameter("@module_id", SqlDbType.Int,4) ,new SqlParameter("@wx_id", SqlDbType.Int,4) };
                parameters[0].Value = module_id;
                parameters[1].Value = wx_id;

                CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
                sqllist.Add(cmd);

                foreach (t_module_wx_user_role m in list)
                {
                    StringBuilder strSqlReply = new StringBuilder();
                    strSqlReply.Append("insert into t_module_wx_user_role(");
                    strSqlReply.Append("wx_id,module_id,user_id,role)");
                    strSqlReply.Append(" values (");
                    strSqlReply.Append("@wx_id,@module_id,@user_id,@role)");
                    SqlParameter[] parametersReply = {
					new SqlParameter("@wx_id", SqlDbType.Int,4),
					new SqlParameter("@role", SqlDbType.NVarChar,250),
					new SqlParameter("@module_id", SqlDbType.Int,4),
					new SqlParameter("@user_id", SqlDbType.Int,4) };
                    parametersReply[0].Value = m.wx_id;
                    parametersReply[1].Value = m.role;
                    parametersReply[2].Value = m.module_id;
                    parametersReply[3].Value = m.user_id;
                    cmd = new CommandInfo(strSqlReply.ToString(), parametersReply);
                    sqllist.Add(cmd);
                }

                result_id = KDWechat.DBUtility.DbHelperSQL.ExecuteSqlTran(sqllist);

            }
            return result_id;
        }
    }
}
