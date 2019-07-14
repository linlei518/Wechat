using KDWechat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityFramework.Extensions;
using KDWechat.Common;
using KDWechat.BLL.Chats;
using System.Linq.Expressions;
using LinqKit;

namespace KDWechat.BLL.Users
{
    public class sys_users_power
    {

        /// <summary>
        /// 通过ID取得权限信息
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>取得的权限信息</returns>
        public static t_sys_users_power GetPowerByID(int id)
        {
            t_sys_users_power power = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                power = (from x in db.t_sys_users_power where x.id == id select x).FirstOrDefault();
            }
            return power;
        }
        /// <summary>
        /// 取权限信息
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>取得的权限信息</returns>
        public static t_sys_users_power GetModel(Expression<Func<t_sys_users_power,bool>> where)
        {
            t_sys_users_power power = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                power = db.t_sys_users_power.Where(where.Expand()).FirstOrDefault();
            }
            return power;
        }
        /// <summary>
        /// 添加一条权限信息
        /// </summary>
        /// <param name="users_power"></param>
        /// <returns></returns>
        public static t_sys_users_power InsertPower(t_sys_users_power users_power)
        {
            return EFHelper.AddUser<t_sys_users_power>(users_power);
        }
        /// <summary>
        /// 更新一条权限信息
        /// </summary>
        /// <param name="users_power"></param>
        /// <returns></returns>
        public static bool UpdatePower(t_sys_users_power users_power)
        {
            return EFHelper.UpdateUser<t_sys_users_power>(users_power);
        }
        /// <summary>
        /// 根据用户ID删除权限信息
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>删除是否成功</returns>
        public static bool DeletePowerByUID(int u_id)
        {
            creater_wxEntities db = new creater_wxEntities();
            bool isFinish = (from x in db.t_sys_users_power where x.u_id == u_id select x).Delete() > 0;
            db.Dispose();
            return isFinish;
        }
        /// <summary>
        /// 根据用户ID获取菜单
        /// </summary>
        /// <param name="parent_id">用户ID</param>
        /// <returns></returns>
        public static List<t_sys_users_power> GetListByUId(int u_id)
        {
            List<t_sys_users_power> pList = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                pList = (from x in db.t_sys_users_power where x.u_id == u_id select x).ToList();
            }
            return pList;
        }
        /// <summary>
        /// 判断是否有某微信号的权限
        /// </summary>
        /// <param name="id">WX_ID</param>
        /// <returns>取得的权限信息</returns>
        public static bool GetPowerRoleByWXID(int wx_id,int u_id)
        {
            t_sys_users_power power = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                power = (from x in db.t_sys_users_power where x.u_id==u_id && x.wx_id == wx_id select x).FirstOrDefault();
            }
            if (power != null)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
        /// <summary>
        /// 判断是否有某微信号的某个权限
        /// </summary>
        /// <param name="id">WX_ID</param>
        /// <returns>取得的权限信息</returns>
        public static bool GetPowerRoleByNavName(int wx_id, int u_id,string nav_name)
        {
            t_sys_users_power power = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                power = (from x in db.t_sys_users_power where x.u_id == u_id && x.wx_id == wx_id && x.nav_name==nav_name select x).FirstOrDefault();
            }
            if (power != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 获取用户的权限
        /// </summary>
        /// <param name="wx_id">微信id</param>
        /// <param name="u_id">用户id</param>
        /// <param name="nav_type_id">导航菜单的根节点id(地区公众号帐号=1 总部帐号=50 ，其他子系统参照导航菜单)</param>
        /// <returns></returns>
        public static t_sys_users_power GetPowerRole(int wx_id, int u_id,int nav_type_id)
        {
            t_sys_users_power power = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                power = (from x in db.t_sys_users_power where x.u_id == u_id && x.wx_id == wx_id && x.nav_type_id==nav_type_id select x).FirstOrDefault();
            }
            if (power==null)
            {
                power = new t_sys_users_power();
                power.action_type = "none";
                power.nav_name = "none";
                power.u_id = u_id;
                power.wx_id = wx_id;
            }
            return power;
        }

        /// <summary>
        /// 获取用户的权限
        /// </summary>
        /// <param name="wx_id">微信id</param>
        /// <param name="u_id">用户id</param>
        /// <param name="nav_type_id">导航菜单的根节点id(地区公众号帐号=1 总部帐号=50 ，其他子系统参照导航菜单)</param>
        /// <returns></returns>
        public static t_sys_users_power GetPowerRole(int wx_id, int u_id)
        {
            t_sys_users_power power = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                power = (from x in db.t_sys_users_power where x.u_id == u_id && x.wx_id == wx_id   select x).FirstOrDefault();
            }
            if (power == null)
            {
                power = new t_sys_users_power();
                power.action_type = "none";
                power.nav_name = "none";
                power.u_id = u_id;
                power.wx_id = wx_id;
            }
            return power;
        }

        /// <summary>
        /// 判断是否有某微信号的某个权限
        /// </summary>
        /// <param name="id">WX_ID</param>
        /// <returns>取得的权限信息</returns>
        public static bool GetPowerRoleByNavName(int wx_id, int u_id, string nav_name,int layer)
        {
            t_sys_users_power power = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                if(layer==3)
                {
                   int parId = Convert.ToInt32(sys_navigation.GetNavigationByName(nav_name).parent_id);
                   nav_name = sys_navigation.GetNavigationByID(parId).name;
                }
                power = (from x in db.t_sys_users_power where x.u_id == u_id && x.wx_id == wx_id && x.nav_name == nav_name select x).FirstOrDefault();
            }
            if (power != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        
    }
}
