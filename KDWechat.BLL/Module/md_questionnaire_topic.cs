using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KDWechat.DAL;
using EntityFramework.Extensions;

namespace KDWechat.BLL.Module
{
    public class md_questionnaire_topic
    {
        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="_id">id</param>
        /// <returns></returns>
        public static t_questionnaire_topic GetModel(int _id)
        {
            t_questionnaire_topic topicModel = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                topicModel = db.t_questionnaire_topic.Where(x => x.id == _id).FirstOrDefault();
            }
            return topicModel;
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
                isFinish = db.t_questionnaire_topic.Where(x => x.id == _id).Delete() > 0;
                if (isFinish)
                {
                    bool a = db.t_subject_options.Where(x => x.t_id == _id).Delete() > 0;
                }
            }
            return isFinish;
        }

        /// <summary>
        /// 删除单条数据
        /// </summary>
        /// <param name="_id">id</param>
        /// <returns></returns>
        public static bool DeleteTopic(int q_id)
        {
            bool isFinish = false;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                isFinish = db.t_questionnaire_topic.Where(x => x.q_id == q_id).Delete() > 0;
                if (isFinish)
                {
                    bool a = db.t_subject_options.Where(x => x.q_id == q_id).Delete() > 0;
                    bool b = db.t_statistics.Where(x => x.q_id == q_id).Delete() > 0;
                }
            }
            return isFinish;
        }

        /// <summary>
        /// 添加一条消息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_questionnaire_topic Add(t_questionnaire_topic model)
        {
            return EFHelper.AddModule<t_questionnaire_topic>(model);
        }

        /// <summary>
        /// 修改一条消息
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_questionnaire_topic Update(t_questionnaire_topic model)
        {
            return EFHelper.UpdateModule<t_questionnaire_topic>(model);
        }

        public static int ExistsTopic(int q_id)
        {
            int isFinish = 0;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                isFinish = db.t_questionnaire_topic.Where(x => x.q_id == q_id).Count();
            }
            return isFinish;
        }

        public static int ExistsTitle(int id, string title)
        {
            int isFinish = 0;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                isFinish = db.t_questionnaire_topic.Where(x => x.q_id == id && x.topic_title == title).Count();
            }
            return isFinish;
        }
    }
}
