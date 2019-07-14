using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KDWechat.DAL;
using EntityFramework.Extensions;
using KDWechat.Common;


namespace KDWechat.BLL.Chats
{
    /// <summary>
    /// 自定义菜单表
    /// </summary>
    public class wx_diy_menus
    {


        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static t_wx_diy_menus GetModel(int id)
        {
            t_wx_diy_menus model = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                model = db.t_wx_diy_menus.Where(x => x.id == id).FirstOrDefault();
            }
            return model;
        }

        /// <summary>
        /// 根据菜单的key获取微信公共帐号的菜单信息
        /// </summary>
        /// <param name="wx_id"></param>
        /// <param name="wx_og_id"></param>
        /// <param name="menu_key"></param>
        /// <returns></returns>
        public static t_wx_diy_menus GetModel(int wx_id, string wx_og_id, string menu_key)
        {
            t_wx_diy_menus model = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                model = (from x in db.t_wx_diy_menus where x.wx_id == wx_id && x.wx_og_id == wx_og_id && x.menu_key == menu_key select x).FirstOrDefault();
            }
            return model;
        }

        /// <summary>
        /// 根据菜单的key获取微信公共帐号的菜单信息
        /// </summary>
        /// <param name="wx_og_id"></param>
        /// <param name="menu_key"></param>
        /// <returns></returns>
        public static t_wx_diy_menus GetModel( string wx_og_id, string menu_key)
        {
            t_wx_diy_menus model = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                model = (from x in db.t_wx_diy_menus where  x.wx_og_id == wx_og_id && x.menu_key == menu_key select x).FirstOrDefault();
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
                isFinish = db.t_wx_diy_menus.Where(x => x.id == id).Delete() > 0;
                if (isFinish)
                {
                    db.t_wx_diy_menus.Where(x => x.parent_id == id).Delete();
                }
            }
            return isFinish;
        }


        /// <summary>
        /// 根据父级ID删除一条信息
        /// </summary>
        /// <param name="parent_id">父级ID</param>
        /// <returns>是否删除成功</returns>
        public static bool DeleteByParId(int parent_id)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isFinish = db.t_wx_diy_menus.Where(x => x.parent_id == parent_id).Delete() > 0;
            }
            return isFinish;
        }

        /// <summary>
        /// 添加一条信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_wx_diy_menus Add(t_wx_diy_menus model)
        {

            model = EFHelper.AddWeChat<t_wx_diy_menus>(model, false);
            return model;//返回添加后的信息
        }

        /// <summary>
        /// 修改一条信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_wx_diy_menus Update(t_wx_diy_menus model)
        {
            return EFHelper.UpdateWeChat<t_wx_diy_menus>(model,false);
        }

        /// <summary>
        /// 根据所属微信ID和父级ID获取自定义菜单
        /// </summary>
        /// <param name="wx_id">微信ID</param>
        /// <param name="parent_id">父级ID</param>
        /// <returns></returns>
        public static List<t_wx_diy_menus> GetListByWxIdAndParentId(int wx_id, int parent_id)
        {
            List<t_wx_diy_menus> chatList = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                chatList = (from x in db.t_wx_diy_menus where x.wx_id == wx_id && x.parent_id == parent_id orderby x.sort_id select x).ToList();
            }
            return chatList;
        }



        /// <summary>
        /// 根据所属微信ID和父级ID获取自定义菜单数量
        /// </summary>
        /// <param name="wx_id">微信ID</param>
        /// <param name="parent_id">父级ID</param>
        /// <returns></returns>
        public static int GetCountByWxIdAndParentId(int wx_id, int parent_id)
        {
            List<t_wx_diy_menus> chatList = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                chatList = (from x in db.t_wx_diy_menus where x.wx_id == wx_id && x.parent_id == parent_id orderby x.id descending select x).ToList();
            }
            if (chatList.Count > 0)
            {
                return chatList.Count;
            }
            else
            {
                return 0;
            }
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
            using (creater_wxEntities db = new creater_wxEntities())
            {
                var diy_menus = db.t_wx_diy_menus.Where(x => x.id == id).FirstOrDefault();
                if (null != diy_menus)
                {
                    diy_menus.sort_id = sort_id;
                    isFinish = db.SaveChanges() > 0;
                }
            }
            return isFinish;
        }
        /// <summary>
        /// 添加子菜单时修改父菜单状态
        /// </summary>
        /// <param name="id">菜单Id</param>
        /// <param name="menu_type">菜单类型</param>
        /// <param name="replay_type">回复类型</param>
        /// <returns></returns>
        public static bool UpdateParent(int id, string menu_type, int replay_type)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                var diy_menus = db.t_wx_diy_menus.Where(x => x.id == id).FirstOrDefault();
                if (null != diy_menus)
                {
                    diy_menus.menu_type = menu_type;
                    diy_menus.reply_type = replay_type;
                    isFinish = db.SaveChanges() > 0;
                }
            }
            return isFinish;
        }

        public static bool UpdateStatus(int parent_id)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isFinish = db.t_wx_diy_menus.Where(x => x.id == parent_id).Update(x => new t_wx_diy_menus { menu_type = "default",reply_type=-1,contents="",menu_url="" }) > 0;
            }
            return isFinish;
        }


        public static bool Exists(t_wx_diy_menus model)
        {
            t_wx_diy_menus obj = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                obj = (from x in db.t_wx_diy_menus where x.wx_id == model.wx_id && x.parent_id == model.parent_id && x.menu_name==model.menu_name select x).FirstOrDefault();
            }
            return obj != null;
        }
    }
}
