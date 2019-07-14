using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KDWechat.DAL;
using EntityFramework.Extensions;

namespace KDWechat.BLL.Module
{
    public class md_activity
    {
        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="_id">id</param>
        /// <returns></returns>
        public static t_activity GetModel(int _id)
        {
            t_activity modelQuest = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                modelQuest = db.t_activity.Where(x => x.id == _id).FirstOrDefault();
            }
            return modelQuest;
        }

        public static t_activity GetModelPrompt(int _id, string _wx_ogid)
        {
            t_activity modelQuest = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                modelQuest = db.t_activity.Where(x => x.id == _id && x.wx_ogid == _wx_ogid).FirstOrDefault();
            }
            return modelQuest;
        }

        public static t_activity GetModelStatus(int _id,string _wx_ogid)
        {
            t_activity modelQuest = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                modelQuest = db.t_activity.Where(x => x.id == _id && x.wx_ogid == _wx_ogid && x.status == 1).FirstOrDefault();
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
        /// 删除单条数据
        /// </summary>
        /// <param name="_id">id</param>
        /// <returns></returns>
        public static bool Delete(int _id)
        {
            bool isFinish = false;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                isFinish = db.t_activity.Where(x => x.id == _id).Delete() > 0;
                if (isFinish)
                {
                    bool a = db.t_activity_topic.Where(x => x.a_id == _id).Delete() > 0;
                    bool b = db.t_activity_options.Where(x => x.a_id == _id).Delete() > 0;
                    bool c = db.t_activity_statistics.Where(x => x.a_id == _id).Delete() > 0;
                    using (kd_wechatsEntities db2 = new kd_wechatsEntities())
                    {
                        bool d = db2.t_module_wechat.Where(x => x.app_id == _id && x.module_id == 6 && x.app_table == "t_activity").Delete() > 0;
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
        public static t_activity Add(t_activity model)
        {
            return EFHelper.AddModule<t_activity>(model);
        }

        /// <summary>
        /// 修改一条消息
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_activity Update(t_activity model)
        {
            return EFHelper.UpdateModule<t_activity>(model);
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