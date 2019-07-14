using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KDWechat.Common;
using KDWechat.DAL;

namespace KDWechat.Web.UI
{
    public class BaseControl : System.Web.UI.UserControl
    {
        protected BasePage bp;
        public BaseControl()
        {
            bp = new BasePage();
        }

        /// <summary>
        /// 登录用户的id(小于0的时候表示登录超时)
        /// </summary>
        public int u_id
        {
            get
            {
                //return bp.u_id;
                return 59;
            }
        }

        /// <summary>
        /// 登录用户的用户名(为空时候表示登录超时)
        /// </summary>
        public string u_name
        {
            get
            {
                return bp.u_name;
            }
        }

        /// <summary>
        /// 登录用户的类型(1-总部，2-负责人，3-子账号，4超级管理员)
        /// </summary>
        public int u_type
        {
            get
            {
                //return bp.u_type;
                return 4;
            }
        }

        /// <summary>
        /// 当前的微信公众号id(等于-1的时候表示登录超时,等于0的时候表示没有选择任何公众号)
        /// </summary>
        public int wx_id
        {
            get
            {
                return bp.wx_id;
            }
        }

        /// <summary>
        /// 当前的微信公众号guid(为空时候表示登录超时或选择的公众号已过期)
        /// </summary>
        public string wx_og_id
        {
            get
            {
                return bp.wx_og_id;
            }
        }

        /// <summary>
        /// 当前的微信公众号显示的名称(为空时候表示登录超时或选择的公众号已过期)
        /// </summary>
        public string wx_name
        {
            get
            {
                return bp.wx_name;
            }
        }

        /// <summary>
        /// 分组id
        /// </summary>
        protected int m_id
        {
            get { return RequestHelper.GetQueryInt("m_id", 0); }
        }

        /// <summary>
        /// 地区号能管理的子系统
        /// </summary>
        public DAL.t_sys_user_manage_child_sys user_manage_child_sys;

        /// <summary>
        /// 获取地区帐号能管理哪些子系统的权限
        /// </summary>
        protected void load_user_manage_child_sys()
        {

            if (user_manage_child_sys == null)
            {
                if (u_type == (int)UserFlag.地区账号 || u_type == (int)UserFlag.总部账号)
                {
                    user_manage_child_sys = Companycn.Core.EntityFramework.EFHelper.GetModel<DAL.creater_wxEntities, DAL.t_sys_user_manage_child_sys>(x => x.u_id == u_id);
                    if (user_manage_child_sys == null)
                    {
                        user_manage_child_sys = new t_sys_user_manage_child_sys();
                        user_manage_child_sys.nav_type_ids = "";
                    }
                }
                else if (u_type == (int)UserFlag.子账号)
                {
                    var userinfo = Companycn.Core.EntityFramework.EFHelper.GetModel<DAL.creater_wxEntities, DAL.t_sys_users>(x => x.id == u_id);
                    if (userinfo != null)
                    {
                        user_manage_child_sys = Companycn.Core.EntityFramework.EFHelper.GetModel<DAL.creater_wxEntities, DAL.t_sys_user_manage_child_sys>(x => x.u_id == userinfo.parent_id);
                        if (user_manage_child_sys == null)
                        {
                            user_manage_child_sys = new t_sys_user_manage_child_sys();
                            user_manage_child_sys.nav_type_ids = "";
                        }
                    }
                }
                else
                {

                    //超级管理员
                    List<DAL.t_sys_navigation> list_child_sys = Companycn.Core.EntityFramework.EFHelper.GetList<DAL.creater_wxEntities, DAL.t_sys_navigation, int?>(x => x.parent_id == 0 && x.is_lock == 0 && (x.type_id == 0 || x.type_id == 2), x => x.sort_id, int.MaxValue, 1);

                    user_manage_child_sys = new t_sys_user_manage_child_sys();
                    user_manage_child_sys.nav_type_ids = ",";
                    foreach (var item in list_child_sys)
                    {
                        user_manage_child_sys.nav_type_ids += item.id + ",";
                    }

                }

            }
        }

    }
}
