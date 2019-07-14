using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KDWechat.DAL;
using EntityFramework.Extensions;

namespace KDWechat.BLL.Module
{
    public class md_activity_options
    {
        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="_id">id</param>
        /// <returns></returns>
        public static t_activity_options GetModel(int _id)
        {
            t_activity_options subMdeol = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                subMdeol = db.t_activity_options.Where(x => x.id == _id).FirstOrDefault();
            }
            return subMdeol;
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
                isFinish = db.t_activity_options.Where(x => x.id == _id).Delete() > 0;
            }
            return isFinish;
        }

        /// <summary>
        /// 添加一条消息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_activity_options Add(t_activity_options model)
        {
            return EFHelper.AddModule<t_activity_options>(model);
        }

        /// <summary>
        /// 修改一条消息
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_activity_options Update(t_activity_options model)
        {
            return EFHelper.UpdateModule<t_activity_options>(model);
        }
    }
}
