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
    /// 关注与无匹配回复信息表
    /// </summary>
    public class wx_basic_reply
    {
        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static t_wx_basic_reply GetModel(int id)
        {
            t_wx_basic_reply model = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                model = db.t_wx_basic_reply.Where(x => x.id == id).FirstOrDefault();
            }
            return model;
        }

        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static t_wx_basic_reply GetModel(int wx_id, string wx_og_id, int channel_id)
        {
            t_wx_basic_reply model = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                model = db.t_wx_basic_reply.Where(x => x.wx_og_id == wx_og_id && x.wx_id == wx_id && x.channel_id == channel_id).FirstOrDefault();
            }
            return model;
        }


        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="wx_id">微信id</param>
        /// <param name="wx_og_id">微信guid</param>
        /// <param name="reply_type">区分ID（1-关注时，2-无匹配时）</param>
        /// <returns></returns>
        public static t_wx_basic_reply GetModel(int wx_id, string wx_og_id, AutoReply reply_type)
        {
            t_wx_basic_reply model = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                model = (from x in db.t_wx_basic_reply where x.wx_id == wx_id && x.wx_og_id == wx_og_id && x.channel_id == (int)reply_type && x.status == 1 select x).Take(1).FirstOrDefault();
            }
            return model;
        }


        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="wx_og_id">微信guid</param>
        /// <param name="reply_type">区分ID（1-关注时，2-无匹配时）</param>
        /// <returns></returns>
        public static t_wx_basic_reply GetModel(string wx_og_id, int reply_type)
        {
            t_wx_basic_reply model = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                model = (from x in db.t_wx_basic_reply where   x.wx_og_id == wx_og_id && x.channel_id == reply_type && x.status == 1 select x).Take(1).FirstOrDefault();
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
                isFinish = db.t_wx_basic_reply.Where(x => x.id == id).Delete() > 0;
            }
            return isFinish;
        }

        /// <summary>
        /// 添加一条信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_wx_basic_reply Add(t_wx_basic_reply model)
        {

            model = EFHelper.AddWeChat<t_wx_basic_reply>(model);
            return model;//返回添加后的信息
        }

        /// <summary>
        /// 修改一条信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_wx_basic_reply Update(t_wx_basic_reply model)
        {
            return EFHelper.UpdateWeChat<t_wx_basic_reply>(model);
        }

        public static bool Delete(int wx_id, string wx_og_id, int channel_id)
        {
            int num = 0;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                num = db.t_wx_basic_reply.Where(x => x.wx_og_id == wx_og_id && x.wx_id == wx_id && x.channel_id == channel_id).Delete();
            }
            return num > 0;
        }
    }
}
