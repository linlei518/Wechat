using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KDWechat.DAL;
using EntityFramework.Extensions;
using LinqKit;
using System.Linq.Expressions;
using KDWechat.Common;
using System.Data;
using Companycn.Core.DbHelper;

namespace KDWechat.BLL.Module
{
    public class md_sale_project
    {
        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static t_md_sale_project GetModel(int id)
        {
            t_md_sale_project model = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                model = db.t_md_sale_project.Where(x => x.id == id).FirstOrDefault();
            }
            return model;
        }
        /// <summary>
        /// 添加一条信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_md_sale_project Add(t_md_sale_project model)
        {

            model = EFHelper.AddModule<t_md_sale_project>(model);
            return model;//返回添加后的信息
        }

        /// <summary>
        /// 修改一条信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_md_sale_project Update(t_md_sale_project model)
        {
            return EFHelper.UpdateModule<t_md_sale_project>(model);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="T">orderby的字段类型</typeparam>
        /// <param name="where">where条件，可拼接</param>
        /// <param name="orderBy">orderby的字段</param>
        /// <param name="count">where条件下的总数</param>
        /// <param name="pagesize">页大小</param>
        /// <param name="pageindex">页号</param>
        /// <param name="ordrebyDesc">是否倒序</param>
        /// <returns></returns>
        public static List<t_md_sale_project> GetList<T>(Expression<Func<t_md_sale_project, bool>> where, Expression<Func<t_md_sale_project, T>> orderBy,out int count, int pagesize, int pageindex,bool ordrebyDesc=false)
        {
            List<t_md_sale_project> chatList = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                var query = db.t_md_sale_project.Where(where.Expand());
                count = query.Count();
                if (!ordrebyDesc)
                    chatList = query.OrderBy(orderBy).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                else
                    chatList = query.OrderByDescending(orderBy).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            }
            return chatList;
        }
        /// <summary>
        /// 获取单条
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static t_md_sale_project GetModel(Expression<Func<t_md_sale_project, bool>> where)
        {
            t_md_sale_project project = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                project = db.t_md_sale_project.Where(where.Expand()).FirstOrDefault();
            }
            return project;
        }

        /// <summary>
        /// 根据时间倒叙取销售列表
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public static List<t_md_sale_project> GetListBySizeAndIndex(int pagesize, int pageindex, int wx_id)
        {
            List<t_md_sale_project> chatList = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                chatList = (from x in db.t_md_sale_project where x.wx_id == wx_id orderby x.id descending select x).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            }
            return chatList;
        }
        /// <summary>
        /// 修改项目状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static bool SetProStatus(int id, Status status)
        {
            bool isFinish = false;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                var pro = db.t_md_sale_project.Where(x => x.id == id).FirstOrDefault();
                if (null != pro)
                {
                    pro.status = (int)status;
                    isFinish = db.SaveChanges() > 0;
                }
            }
            return isFinish;
        }
        /// <summary>
        /// 根据sql语句返回DataTable
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DataTable GetListByQuery(string query)
        {
            DataSet ds = DbHelperSQLModule.Query(query);
            DataTable dt = ds.Tables[0] as DataTable;
            return dt;
        }
        /// <summary>
        /// 根据sql语句返回受影响条数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static int GetCountByQuery(string query)
        {
            int res = Utils.ObjToInt(DbHelperSQLModule.GetSingle(query),0);
            return res;
        }
        /// <summary>
        /// 修改菜单排序
        /// </summary>
        /// <param name="id">菜单Id</param>
        /// <param name="sort_id">排序值</param>
        /// <returns></returns>
        public static bool UpdateSort(int id, int sort_id)
        {
            bool isFinish = false;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                var pro = db.t_md_sale_project.Where(x => x.id == id).FirstOrDefault();
                if (null != pro)
                {
                    pro.sort_id = sort_id;
                    isFinish = db.SaveChanges() > 0;
                }
            }
            return isFinish;
        }
        /// <summary>
        /// 检查项目名称是否可用
        /// </summary>
        /// <param name="userName">项目名称</param>
        /// <returns>项目名称是否可用</returns>
        public static bool CheckPName(string p_name,int wx_id)
        {
            bool isChecked = false;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                isChecked = (from x in db.t_md_sale_project where x.p_name == p_name && x.wx_id==wx_id select x).FirstOrDefault() == null;
            }
            return isChecked;
        }

        public static bool SetManager(int pro_id,int manager_id)
        {
            bool isFinish = false;
            using(kd_moduleEntities db = new kd_moduleEntities())
            {
                var pro = db.t_md_sale_project.Where(x => x.id == pro_id).FirstOrDefault();
                if (null != pro)
                {
                    pro.manager_id = manager_id;
                    isFinish = db.SaveChanges()>0;
                }
            }
            return isFinish;
        }

    }
}
