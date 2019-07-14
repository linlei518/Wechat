using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KDWechat.DAL;
using EntityFramework.Extensions;
namespace KDWechat.BLL.Chats
{
    /// <summary>
    /// 站内信基本信息表
    /// </summary>
    public class messages
    {
        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static t_messages GetModel(int id)
        {
            t_messages material = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                material = db.t_messages.Where(x => x.id == id).FirstOrDefault();
            }
            return material;
        }

        /// <summary>
        /// 删除一条消息
        /// </summary>
        /// <param name="id">消息ID</param>
        /// <returns>是否删除成功</returns>
        public static bool Delete(int id)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isFinish = db.t_messages.Where(x => x.id == id).Delete() > 0;
            }
            return isFinish;
        }

        /// <summary>
        /// 添加一条消息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_messages Add(t_messages model)
        {

            model = EFHelper.AddWeChat<t_messages>(model);
            return model;//返回添加后的消息
        }

        /// <summary>
        /// 修改一条消息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_messages Update(int id, t_messages model)
        {
            var material = GetModel(id);//获取消息
            if (material != null)
            {
                //参数赋值
                material.title = model.title ?? material.title;
                material.contents = model.contents ?? material.contents;
                material.source_id = model.source_id;
                material.msg_type = model.msg_type;

                EFHelper.UpdateWeChat<t_messages>(material);
            }
            return material;
        }

    }
}
