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
    ///关键词回复规则与关键词信息表
    /// </summary>
    public class wx_rules_keywords
    {
        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static t_wx_rules_keywords GetModel(int id)
        {
            t_wx_rules_keywords model = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                model = db.t_wx_rules_keywords.Where(x => x.id == id).FirstOrDefault();
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
                isFinish = db.t_wx_rules_keywords.Where(x => x.id == id).Delete() > 0;
            }
            return isFinish;
        }

        /// <summary>
        /// 添加一条信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_wx_rules_keywords Add(t_wx_rules_keywords model)
        {

            model = EFHelper.AddWeChat<t_wx_rules_keywords>(model);
            return model;//返回添加后的信息
        }

        /// <summary>
        /// 修改一条信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_wx_rules_keywords Update(t_wx_rules_keywords model)
        {
            return EFHelper.UpdateWeChat<t_wx_rules_keywords>(model);
        }

        /// <summary>
        /// 获取关键词列表，多个用|分割
        /// </summary>
        /// <param name="r_id"></param>
        /// <returns></returns>
        public static string GetKeywordLists(int r_id)
        {
            string key_list = string.Empty;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                var list = (from x in db.t_wx_rules_keywords where x.r_id == r_id select x.key_words).ToList();
                if (list!=null)
                {
                    foreach (var item in list)
                    {
                        key_list +=item+ "|";
                    }
                }
            }
            if (key_list.Length>0)
            {
                key_list = key_list.TrimEnd('|');
            }
            return key_list;
        }

        public static t_wx_rules_keywords GetModel(int wx_id, string wx_og_id, string keyword)
        {
            t_wx_rules_keywords model = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                model = db.t_wx_rules_keywords.Where(x => x.wx_id == wx_id && x.wx_og_id == wx_og_id && x.key_words==keyword ).OrderByDescending(x=>x.reply_type).FirstOrDefault();
            }

            return model;
        }
    }
}
