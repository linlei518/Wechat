using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KDWechat.DAL;
using System.Data;
using KDWechat.DBUtility;
using KDWechat.Common;
using EntityFramework.Extensions;

namespace KDWechat.BLL.Users
{
    public class sys_navigation
    {
        #region 外部方法

   

        /// <summary>
        /// 根据页面id获取所有层级的导航名称
        /// </summary>
        /// <param name="m_id"></param>
        /// <returns></returns>
        public static DataTable GetNavigationName(int m_id)
        {
            DataTable dt = null;
            t_sys_navigation model = GetNavigationByID(m_id);
            if (model!=null)
            {
                string ids = model.class_list.Trim().TrimEnd(',').TrimStart(',');
                if (ids.Length>0)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("select id,title ");
                    strSql.Append(" FROM t_sys_navigation where id in(" + ids + ")");
                    strSql.Append(" order by id asc");

                    dt = KDWechat.DBUtility.DbHelperSQL.Query(strSql.ToString()).Tables[0];
                }
               
            }
            return dt;

          
        }

        /// <summary>
        /// 取得所有类别列表(已经排序好)
        /// </summary>
        /// <param name="parent_id">父ID</param>
        /// <param name="nav_type">导航类别</param>
        /// <returns>DataTable</returns>
        public DataTable GetList(int parent_id,int channel_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,nav_type,name,title,sub_title,link_url,sort_id,is_lock,remark,parent_id,class_list,class_layer,channel_id,action_type,is_sys,type_id,target_type");
            strSql.Append(" FROM t_sys_navigation");
            strSql.Append(" order by sort_id asc,id asc");
          
            DataSet ds = KDWechat.DBUtility.DbHelperSQL.Query(strSql.ToString());
            DataTable oldData = ds.Tables[0] as DataTable;
            if (oldData == null)
            {
                return null;
            }
            //复制结构
            DataTable newData = oldData.Clone();
            //调用迭代组合成DAGATABLE
            GetChilds(oldData, newData, parent_id,channel_id);
            return newData;
        }
        /// <summary>
        /// 提取一个导航菜单的所有信息
        /// </summary>
        /// <param name="id">导航菜单ID</param>
        /// <returns>该ID对应的导航菜单</returns>
        public static t_sys_navigation GetNavigationByID(int id)
        {
            t_sys_navigation navigation = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                navigation = (from x in db.t_sys_navigation where x.id == id select x).FirstOrDefault();
            }
            return navigation;
        }
        /// <summary>
        /// 提取一个导航菜单的所有信息
        /// </summary>
        /// <param name="id">导航菜单ID</param>
        /// <returns>该ID对应的导航菜单</returns>
        public static t_sys_navigation GetNavigationByName(string Name)
        {
            t_sys_navigation navigation = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                navigation = (from x in db.t_sys_navigation where x.name == Name select x).FirstOrDefault();
            }
            return navigation;
        }
        /// <summary>
        /// 检查菜单ID是否可用
        /// </summary>
        /// <param name="name">菜单ID</param>
        /// <returns>菜单是否可用</returns>
        public static bool CheckNavigationName(string name)
        {
            bool isChecked = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isChecked = (from x in db.t_sys_navigation where x.name == name select x).FirstOrDefault() == null;
            }
            return isChecked;
        }
        /// <summary>
        /// 修改菜单排序
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool UpdateSort(int id,int sort_id)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                var navigation = db.t_sys_navigation.Where(x => x.id == id).FirstOrDefault();
                if (null != navigation)
                {
                    navigation.sort_id = sort_id;
                    isFinish = db.SaveChanges() > 0;
                }
            }
            return isFinish;
        }
        /// <summary>
        /// 新建数据
        /// </summary>
        /// <param name="NavigationToInsert"></param>
        /// <returns></returns>
        public static t_sys_navigation InsertNavigation(t_sys_navigation NavigationToInsert)
        {
            var navigation= EFHelper.AddUser<t_sys_navigation>(NavigationToInsert);
            if (navigation.parent_id > 0)
            {
                var Oldnavigation = GetNavigationByID(Convert.ToInt32(navigation.parent_id));
                navigation.class_list = Oldnavigation.class_list + navigation.id + ",";
                navigation.class_layer = Oldnavigation.class_layer + 1;
                navigation.channel_id = Oldnavigation.channel_id;
                UpdateNavigation(navigation);
            }
            else 
            {
                navigation.class_list = "," + navigation.id + ",";
                navigation.class_layer = 0;
                UpdateNavigation(navigation);
            }
            return navigation;
        }
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="NavigationToUpdate"></param>
        /// <returns></returns>
        public static bool UpdateNavigation(t_sys_navigation NavigationToUpdate) 
        {
            if (NavigationToUpdate.parent_id > 0)
            {
                var Oldnavigation = GetNavigationByID(Convert.ToInt32(NavigationToUpdate.parent_id));
                NavigationToUpdate.class_list = Oldnavigation.class_list + NavigationToUpdate.id + ",";
                NavigationToUpdate.class_layer = Oldnavigation.class_layer + 1;
               
            }
            return EFHelper.UpdateUser<t_sys_navigation>(NavigationToUpdate);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteNavigationByID(int id)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isFinish = db.t_sys_navigation.Where(x => x.id == id).Delete() > 0;
            }
            return isFinish;
        }
        /// <summary>
        /// 根据父级删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteNavigationByParentID(int parent_id)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isFinish = db.t_sys_navigation.Where(x => x.parent_id == parent_id).Delete() > 0;
            }
            return isFinish;
        }
        #endregion

        #region 内部部方法

        /// <summary>
        /// 从内存中取得所有下级类别列表（自身迭代）
        /// </summary>
        private void GetChilds(DataTable oldData, DataTable newData, int parent_id, int channel_id)
        {
            string strWhere = "parent_id=" + parent_id;
            if(channel_id>0)
            {
                strWhere += " and channel_id=" + channel_id;
            }

            DataRow[] dr = oldData.Select(strWhere);
            for (int i = 0; i < dr.Length; i++)
            {
                //添加一行数据
                DataRow row = newData.NewRow();
                row["id"] = int.Parse(dr[i]["id"].ToString());
                row["nav_type"] = dr[i]["nav_type"].ToString();
                row["name"] = dr[i]["name"].ToString();
                row["title"] = dr[i]["title"].ToString();
                row["sub_title"] = dr[i]["sub_title"].ToString();
                row["link_url"] = dr[i]["link_url"].ToString();
                row["sort_id"] = int.Parse(dr[i]["sort_id"].ToString());
                row["is_lock"] = int.Parse(dr[i]["is_lock"].ToString());
                row["remark"] = dr[i]["remark"].ToString();
                row["parent_id"] = int.Parse(dr[i]["parent_id"].ToString());
                row["class_list"] = dr[i]["class_list"].ToString();
                row["class_layer"] = int.Parse(dr[i]["class_layer"].ToString());
                row["channel_id"] = int.Parse(dr[i]["channel_id"].ToString());
                row["action_type"] = dr[i]["action_type"].ToString();
                row["is_sys"] = int.Parse(dr[i]["is_sys"].ToString());
                row["target_type"]=dr[i]["target_type"].ToString();
                row["type_id"] = int.Parse(dr[i]["type_id"].ToString());
                newData.Rows.Add(row);
                //调用自身迭代
                this.GetChilds(oldData, newData, int.Parse(dr[i]["id"].ToString()),channel_id);
            }
        }
        /// <summary>
        /// 根据父级ID获取菜单
        /// </summary>
        /// <param name="parent_id">父级ID</param>
        /// <returns></returns>
        public static List<t_sys_navigation> GetListByParentId(int parent_id,int channel_id)
        {
            List<t_sys_navigation> chatList = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                chatList = (from x in db.t_sys_navigation where x.parent_id == parent_id && x.channel_id == channel_id && x.is_lock == 0 orderby x.sort_id select x).ToList();
            }
            return chatList;
        }

        /// <summary>
        /// 根据父级ID获取菜单
        /// </summary>
        /// <param name="parent_id">父级ID</param>
        /// <returns></returns>
        public static List<t_sys_navigation> GetListByParentId(int parent_id)
        {
            List<t_sys_navigation> chatList = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                chatList = (from x in db.t_sys_navigation where x.parent_id == parent_id && x.is_lock==0  orderby x.sort_id select x).ToList();
            }
            return chatList;
        }

        #endregion
    }
}
