using KDWechat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityFramework.Extensions;
using KDWechat.Common;

namespace KDWechat.BLL.Chats
{
    public class module_menu
    {
        /// <summary>
        /// 根据时间倒叙取公众号列表
        /// </summary>
        /// <returns></returns>
        public static List<t_module_menu> GetListWxId(int wx_id,int parent_id,int u_type=0,int u_id=0)
        {
            List<t_module_menu> chatList = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                if (u_type==(int)UserFlag.子账号)
                {
                    var modlue_ids = (from x in db.t_module_wx_user_role where x.wx_id == wx_id && x.user_id == u_id select x.module_id).ToList();
                    chatList = (from x in db.t_module_menu where x.wx_id == wx_id && x.parent_id == parent_id && modlue_ids.Contains((int)x.module_id)==true orderby x.sort select x).ToList();
                }
                else
                {
                    chatList = (from x in db.t_module_menu where x.wx_id == wx_id && x.parent_id == parent_id orderby x.sort select x).ToList();
                }

               
            }
            return chatList;
        }
        /// <summary>
        /// 添加一条消息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_module_menu Add(t_module_menu model)
        {

            model = EFHelper.AddWeChat<t_module_menu>(model);
            return model;//返回添加后的消息
        }
        /// <summary>
        /// 删除一条消息
        /// </summary>
        /// <param name="id">需要删除的ID</param>
        /// <returns>删除是否成功</returns>
        public static bool Delete(int id)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isFinish = db.t_module_menu.Where(x => x.id == id).Delete() > 0;
            }
            return isFinish;
        }
        /// <summary>
        /// 删除一条消息
        /// </summary>
        /// <param name="id">需要删除的模块ID</param>
        /// <returns>删除是否成功</returns>
        public static bool DeletebyMID(int Mid,int _wx_id)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isFinish = db.t_module_menu.Where(x => x.module_id== Mid && x.wx_id==_wx_id).Delete() > 0;
            }
            return isFinish;
        }
        /// <summary>
        /// 删除一条消息
        /// </summary>
        /// <param name="id">需要删除的模块ID</param>
        /// <returns>删除是否成功</returns>
        public static bool DeletebyMID(int Mid)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isFinish = db.t_module_menu.Where(x => x.module_id == Mid).Delete() > 0;
            }
            return isFinish;
        }
    }
}
