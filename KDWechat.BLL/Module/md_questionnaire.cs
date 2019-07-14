using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KDWechat.DAL;
using EntityFramework.Extensions;

namespace KDWechat.BLL.Module
{
    public class md_questionnaire
    {
        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="_id">id</param>
        /// <returns></returns>
        public static t_questionnaire GetModel(int _id)
        {
            t_questionnaire modelQuest = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                modelQuest = db.t_questionnaire.Where(x => x.id == _id).FirstOrDefault();
            }
            return modelQuest;
        }

        public static t_questionnaire GetModelStatus(int _id, string _wx_ogid)
        {
            t_questionnaire modelQuest = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                modelQuest = db.t_questionnaire.Where(x => x.id == _id && x.wx_ogid == _wx_ogid && x.is_look == 1).FirstOrDefault();
            }
            return modelQuest;
        }

        public static int Exists(string wx_og_id, string _name)
        {
            int isFinish = 0;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                isFinish = db.t_questionnaire.Where(x => x.title == _name && x.wx_ogid == wx_og_id).Count();
            }
            return isFinish;
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
                isFinish = db.t_questionnaire.Where(x => x.id == _id).Delete() > 0;
                if (isFinish)
                {
                    bool a = db.t_questionnaire_topic.Where(x => x.q_id == _id).Delete() > 0;
                    bool b = db.t_subject_options.Where(x => x.q_id == _id).Delete() > 0;
                    bool c = db.t_statistics.Where(x => x.q_id == _id).Delete() > 0;

                    using (kd_wechatsEntities db2 = new kd_wechatsEntities())
                    {
                        bool d = db2.t_module_wechat.Where(x => x.app_id == _id && x.module_id == 1 && x.app_table == "t_questionnaire").Delete() > 0;
                    }
                }
            }
            return isFinish;
        }

        /// <summary>
        /// 添加一条消息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_questionnaire Add(t_questionnaire model)
        {
            return EFHelper.AddModule<t_questionnaire>(model);
        }

        /// <summary>
        /// 修改一条消息
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_questionnaire Update(t_questionnaire model)
        {
            return EFHelper.UpdateModule<t_questionnaire>(model);
        }

        public static t_module_wechat GetWechatModel(string _wx_og_id)
        {
            t_module_wechat model = null;
            using (kd_wechatsEntities db = new kd_wechatsEntities())
            {
                model = db.t_module_wechat.Where(x => x.wx_og_id == _wx_og_id && x.module_id == 1).FirstOrDefault();
            }
            return model;
        }

        public static t_module_wechat GetWechatModel(int wxID, int appID, int moduleID)
        {
            t_module_wechat material = null;
            using (kd_wechatsEntities db = new kd_wechatsEntities())
            {
                material = db.t_module_wechat.Where(x => x.wx_id == wxID && x.app_id == appID && x.module_id == moduleID).FirstOrDefault();
            }
            return material;
        }

        public static t_module_wechat GetWechatModel(int appID, int moduleID)
        {
            t_module_wechat material = null;
            using (kd_wechatsEntities db = new kd_wechatsEntities())
            {
                material = db.t_module_wechat.Where(x => x.app_id == appID && x.module_id == moduleID).FirstOrDefault();
            }
            return material;
        }

        public static t_module_wechat Update(t_module_wechat model)
        {
            if (model != null)
            {
                EFHelper.UpdateWeChat<t_module_wechat>(model);
            }
            return model;
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
