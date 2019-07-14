using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KDWechat.DAL;
using EntityFramework.Extensions;

namespace KDWechat.BLL.Module
{
    public class md_subject_options
    {
        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="_id">id</param>
        /// <returns></returns>
        public static t_subject_options GetModel(int _id)
        {
            t_subject_options subMdeol = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                subMdeol = db.t_subject_options.Where(x => x.id == _id).FirstOrDefault();
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
                isFinish = db.t_subject_options.Where(x => x.id == _id).Delete() > 0;
            }
            return isFinish;
        }

        /// <summary>
        /// 添加一条消息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_subject_options Add(t_subject_options model)
        {
            return EFHelper.AddModule<t_subject_options>(model);
        }

        /// <summary>
        /// 修改一条消息
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_subject_options Update(t_subject_options model)
        {
            return EFHelper.UpdateModule<t_subject_options>(model);
        }
    }
}
