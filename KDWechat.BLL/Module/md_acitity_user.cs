using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KDWechat.DAL;

namespace KDWechat.BLL.Module
{
    public class md_activity_user
    {
        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="_id">id</param>
        /// <returns></returns>
        public static t_activity_user GetModel(int _id)
        {
            t_activity_user modelQuest = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                modelQuest = db.t_activity_user.Where(x => x.id == _id).FirstOrDefault();
            }
            return modelQuest;
        }

        public static int Exists(string wx_og_id, string _name)
        {
            int isFinish = 0;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                isFinish = db.t_activity.Where(x => x.title == _name && x.wx_ogid == wx_og_id).Count();
            }
            return isFinish;
        }

        /// <summary>
        /// 添加一条消息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_activity_user Add(t_activity_user model)
        {
            return EFHelper.AddModule<t_activity_user>(model);
        }

        /// <summary>
        /// 修改一条消息
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_activity_user Update(t_activity_user model)
        {
            return EFHelper.UpdateModule<t_activity_user>(model);
        }

        /// <summary>
        /// 统计投票人数
        /// </summary>
        public static int GetCount(int a_id)
        {
            int count = 0;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                count = db.t_activity_user.Where(x => x.a_id == a_id).Count();
            }
            return count;
        }

        /// <summary>
        /// 查找是否存在报名记录
        /// </summary>
        public static int Exists(int actid, string openId)
        {
            int count = 0;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                count = db.t_activity_user.Where(x => x.a_id == actid && x.open_id == openId).Count();
            }
            return count;
        }

        /// <summary>
        /// 通过openID取得粉丝
        /// </summary>
        /// <param name="id">粉丝openID</param>
        /// <returns>取得的粉丝</returns>
        public static t_wx_fans GetFansByID(string opid, string wx_og_id)
        {
            t_wx_fans fan = null;
            using (kd_usersEntities db = new kd_usersEntities())
            {
                fan = (from x in db.t_wx_fans where x.open_id == opid && x.wx_og_id == wx_og_id select x).FirstOrDefault();
            }
            return fan;
        }
    }
}