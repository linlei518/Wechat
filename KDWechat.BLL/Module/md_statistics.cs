using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KDWechat.DAL;
using EntityFramework.Extensions;

namespace KDWechat.BLL.Module
{
    public class md_statistics
    {
        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="_id">id</param>
        /// <returns></returns>
        public static t_statistics GetModel(int _id)
        {
            t_statistics sticsModel = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                sticsModel = db.t_statistics.Where(x => x.id == _id).FirstOrDefault();
            }
            return sticsModel;
        }

        /// <summary>
        /// 删除单条数据
        /// </summary>
        /// <param name="_id">id</param>
        /// <returns></returns>
        public static bool Delete(int _id)
        {
            bool isFinish = false;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                isFinish = db.t_statistics.Where(x => x.id == _id).Delete() > 0;
            }
            return isFinish;
        }

        /// <summary>
        /// 添加一条消息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_statistics Add(t_statistics model)
        {
            return EFHelper.AddModule<t_statistics>(model);
        }

        /// <summary>
        /// 修改一条消息
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_statistics Update(t_statistics model)
        {
            return EFHelper.UpdateModule<t_statistics>(model);
        }

        /// <summary>
        /// 统计投票人数
        /// </summary>
        public int GetCount(int q_id, string wx_ogid)
        {
            int count = 0;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                count = db.t_statistics.Where(x => x.q_id == q_id).Count();
            }
            return count;
        }

        /// <summary>
        /// 查找是否存在投票记录
        /// </summary>
        public static int Exists(int quesid, string openId)
        {
            int count = 0;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                count = db.t_statistics.Where(x => x.q_id == quesid && x.open_id == openId).Count();
            }
            return count;
        }

        public static int ExistsCount(int q_id)
        {
            int count = 0;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                count = db.t_statistics.Where(x => x.q_id == q_id).Count();
            }
            return count;
        }


        public static int ExistsImgCount(int t_id)
        {
            int count = 0;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                count = db.t_statistics.Where(x => x.t_id == t_id).Count();
            }
            return count;
        }
    }
}
