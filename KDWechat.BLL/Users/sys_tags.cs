using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KDWechat.DAL;
using LinqKit;
using EntityFramework.Extensions;
using KDWechat.Common;
using System.Linq.Expressions;
using KDWechat.DBUtility;

namespace KDWechat.BLL.Users
{
    /// <summary>
    /// 系统标签
    /// </summary>
    public class sys_tags
    {
        public static t_sys_tags GetModel(int id)
        {
            t_sys_tags tag = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                tag = (from x in db.t_sys_tags where x.id == id select x).FirstOrDefault();
            }
            return tag;
        }


        public static List<t_sys_tags> GetList(int channel_id)
        {
            List<t_sys_tags> list = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                list = (from x in db.t_sys_tags where x.channel_id == channel_id select x).ToList();
            }
            return list;
        }

        public static List<t_sys_tags> GetList<T>(Expression<Func<t_sys_tags, bool>> where)
        {
            List<t_sys_tags> list = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                list = db.t_sys_tags.Where(where.Expand()).OrderBy(x => x.sort_id).ThenByDescending(x => x.id).ToList();
            }
            return list;
        }


        public static t_sys_tags Add(t_sys_tags tag)
        {
            return EFHelper.AddUser<t_sys_tags>(tag);
        }

        public static bool Delete(int id)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isFinish = db.t_sys_tags.Where(x => x.id == id).Delete() > 0;
            }
            return isFinish;
        }

        /// <summary>
        /// 删除一条分组或标签
        /// </summary>
        /// <param name="id">分组id</param>
        /// <param name="channel_id">区分ID（1、兴趣爱好 2、地区 3、城市 4、类型 5、状态）</param>
        /// <param name="errorMsg">返回的错误消息</param>
        /// <returns></returns>
        public static int Delete(int id, int channel_id, ref string errorMsg)
        {
            int result = 0;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                switch (channel_id)
                {
                    case (int)SysTag.项目城市:
                        result = GetTagByProject("city_id=" + id);
                        if (result > 0)
                            errorMsg = "该项目城市不能删除，请先移除该项目城市下的所有项目";
                        break;
                    case (int)SysTag.项目地区:
                        result = GetTagByProject("region_id=" + id);
                        if (result > 0)
                            errorMsg = "该项目地区不能删除，请先移除该项目地区下的所有项目";
                        break;
                    case (int)SysTag.项目类型:
                        result = GetTagByProject("type_id=" + id);
                        if (result > 0)
                            errorMsg = "该项目类型不能删除，请先移除该项目类型下的所有项目";
                        break;
                    case (int)SysTag.项目状态:
                        result = GetTagByProject("status_id=" + id);
                        if (result > 0)
                            errorMsg = "该项目状态不能删除，请先移除该项目状态下的所有项目";
                        break;
                }
                if (result == 0)
                {
                    result = db.t_sys_tags.Where(x => x.id == id).Delete();
                }

            }
            return result;
        }

        private static int GetTagByProject(string where)
        {
            int result = 0;
            string sql = string.Format(" select COUNT(1) from t_projects where {0} ", where);

            result = Convert.ToInt32(KDWechat.DBUtility.DbHelperSQL.GetSingle(sql));
            return result;
        }

        public static bool Update(t_sys_tags tag)
        {
            bool isc = EFHelper.UpdateUser<t_sys_tags>(tag);
            if (isc)
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("update  t_project_groups set  region_name='" + tag.title + "' where region_id=" + tag.id+";\n");
                sql.Append("update  t_project_groups set  city_name='" + tag.title + "' where city_id=" + tag.id + ";\n");
                sql.Append("update  t_project_groups set  type_name='" + tag.title + "' where type_id=" + tag.id + ";\n");
                sql.Append("update  t_project_groups set  status_name='" + tag.title + "' where status_id=" + tag.id + ";\n");

                sql.Append("update  t_projects set  region_name='" + tag.title + "' where region_id=" + tag.id + ";\n");
                sql.Append("update  t_projects set  city_name='" + tag.title + "' where city_id=" + tag.id + ";\n");
                sql.Append("update  t_projects set  type_name='" + tag.title + "' where type_id=" + tag.id + ";\n");
                sql.Append("update  t_projects set  status_name='" + tag.title + "' where status_id=" + tag.id + ";\n");
                KDWechat.DBUtility.DbHelperSQL.ExecuteSql(sql.ToString());
            }
            return isc;
        }


        public static bool Exists(string title, int channel_id, int parent_id)
        {
            bool isOk = true;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isOk = db.t_sys_tags.Where(x => x.title == title && x.channel_id == channel_id && x.parent_id == parent_id).FirstOrDefault() != null;
            }
            return isOk;
        }

        /// <summary>
        /// 更改排序
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sort_id"></param>
        public static void UpdateSort(int id, int sort_id)
        {
            using (creater_wxEntities db = new creater_wxEntities())
            {
                db.t_sys_tags.Where(x => x.id == id).Update(x => new t_sys_tags(){ sort_id=sort_id});
            }
        }
    }
}
