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
    /// 规则回复信息表
    /// </summary>
    public class wx_rule_reply
    { /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static t_wx_rule_reply GetModel(int id)
        {
            t_wx_rule_reply model = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                model = db.t_wx_rule_reply.Where(x => x.id == id).FirstOrDefault();
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
                isFinish = db.t_wx_rule_reply.Where(x => x.id == id).Delete() > 0;
            }
            return isFinish;
        }

        /// <summary>
        /// 添加一条信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_wx_rule_reply Add(t_wx_rule_reply model)
        {

            model = EFHelper.AddWeChat<t_wx_rule_reply>(model);
            return model;//返回添加后的信息
        }

        /// <summary>
        /// 修改一条信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_wx_rule_reply Update(t_wx_rule_reply model)
        {
            return EFHelper.UpdateWeChat<t_wx_rule_reply>(model);
        }

        public static t_wx_rule_reply GetModelByRid(int r_id, int wx_id, string wx_og_id)
        {
            t_wx_rule_reply model = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                model = db.t_wx_rule_reply.Where(x => x.r_id == r_id && x.wx_id==wx_id && x.wx_og_id==wx_og_id ).FirstOrDefault();
            }
            return model;
        }

        public static t_wx_rule_reply GetModelByRid(int r_id,   string wx_og_id)
        {
            t_wx_rule_reply model = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                model = db.t_wx_rule_reply.Where(x => x.r_id == r_id  && x.wx_og_id == wx_og_id).FirstOrDefault();
            }
            return model;
        }
    }
}
