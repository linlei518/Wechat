using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KDWechat.DAL;
using EntityFramework.Extensions;
using System.Linq.Expressions;
using KDWechat.BLL.Chats;
using LinqKit;
using KDWechat.DBUtility;


namespace KDWechat.BLL.Logs
{
    public class wx_fans_chats
    {
        public static t_wx_fans_chats GetFansChatsByID(int id)
        {
            t_wx_fans_chats tag = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                tag = (from x in db.t_wx_fans_chats where x.id == id select x).FirstOrDefault();
            }
            return tag;
        }

        public static t_wx_fans_chats GetFansChatsByID(string open_id)
        {
            t_wx_fans_chats tag = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                tag = (from x in db.t_wx_fans_chats where x.open_id==open_id orderby x.id descending select x).FirstOrDefault();
            }
            return tag;
        }


        public static t_wx_fans_chats CreateFansChat(t_wx_fans_chats tag)
        {
            if (tag.wx_id==0)
            {
                t_wx_wechats model = BLL.Chats.wx_wechats.GetWeChatByogID(tag.wx_og_id);
                if (model!=null)
                {
                    tag.wx_id = model.id;
                }
            }
            return EFHelper.AddLog<t_wx_fans_chats>(tag);
        }

        public static t_wx_fans_chats CreateSubscribeFansChat(t_wx_fans_chats tag)
        {
            using (var transaction = new System.Transactions.TransactionScope())
            {
                if (tag.wx_id == 0)
                {
                    t_wx_wechats model = BLL.Chats.wx_wechats.GetWeChatByogID(tag.wx_og_id);
                    if (model != null)
                    {
                        tag.wx_id = model.id;
                    }
                }
                using (creater_wxEntities db = new creater_wxEntities())
                {
                    DateTime start = DateTime.Now.AddMinutes(-1);
                    DateTime end = DateTime.Now.AddMinutes(1);
                    var s = (from x in db.t_wx_fans_chats where x.create_time > start && x.create_time < end && x.contents == "关注成功！" select x).FirstOrDefault();
                    if (s == null)
                    {
                        db.t_wx_fans_chats.Add(tag);
                        db.SaveChanges();
                    }
                    else
                        tag = s;
                }
                transaction.Complete();
            }
            return tag;
        }

        public static bool DeleteFansChatByID(int id)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isFinish = db.t_wx_fans_chats.Where(x => x.id == id).Delete() > 0;
            }
            return isFinish;
        }

        public static bool UpdateFansChat(t_wx_fans_chats tag)
        {
            return EFHelper.UpdateLog<t_wx_fans_chats>(tag);
        }

        /// <summary>
        /// 根据openid获取最后一条聊记录
        /// </summary>
        /// <param name="open_id"></param>
        /// <returns></returns>
        public static t_wx_fans_chats GetLastFansChatsByOpenid(string open_id)
        {
            t_wx_fans_chats model = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                model = (from x in db.t_wx_fans_chats where x.open_id == open_id orderby x.create_time descending select x).FirstOrDefault();
                if (null != model && model.from_type == 2)
                {
                    var model2 = db.t_wx_fans_chats.Where(x => x.from_type == 1).OrderByDescending(x => x.create_time).First();
                    model.create_time = model2.create_time;
                }
            }
            return model;
        }
        /// <summary>
        /// 获取公众号未回复的消息数量
        /// </summary>
        /// <param name="wx_id"></param>
        /// <returns></returns>
        public static int GetNoReplyCount(int wx_id)
        {
            int count = 0;

            //string sql = string.Format("select  count(distinct open_id) from t_wx_fans_chats where  wx_id={0} and  from_type=1 and DATEDIFF(hh, create_time, getdate())<48 and open_id in (select  open_id from t_wx_fans_chats where from_type=2 and wx_id={0} and DATEDIFF(hh, create_time, getdate())<48 )", wx_id);
            string sql = "select count(1) from (select max(id) as id,open_id from t_wx_fans_chats group by open_id) s left join t_wx_fans_chats k on s.id=k.id  where from_type=1 and k.wx_id="+wx_id+" and k.open_id in(select open_id from t_wx_fans where status=1  and DATEDIFF(hh, last_interact_time, getdate())<48) and k.contents!='关注成功！'";
      
            count = Common.Utils.ObjToInt(KDWechat.DBUtility.DbHelperSQL.GetSingle(sql), 0);
            return count;
        }

        /// <summary>
        /// 获取一周的用户消息数
        /// </summary>
        /// <param name="wxid"></param>
        /// <returns></returns>
        public static string[] GetChatCountWeekly(int wxid,Common.FromUserType fromType)
        {
            string[] weekList = new string[7];
            using (creater_wxEntities db = new creater_wxEntities())
            {
                int count;
                for (int i = 0; i < 7; i++)
                {
                    DateTime startDate = DateTime.Now.Date.AddDays(-i);
                    DateTime endDate = DateTime.Now.Date.AddDays(1 - i);
                    count = (from x in db.t_wx_fans_chats where x.wx_id == wxid && x.from_type==(int)fromType &&x.create_time > startDate && x.create_time < endDate select 1).Count();
                    weekList[6 - i] = count.ToString();
                }
            }
            return weekList;
        }
   

        /// <summary>
        /// 通过连续日期列表取统计数据
        /// </summary>
        /// <param name="dateSpan"></param>
        /// <param name="wxID"></param>
        /// <param name="fromType"></param>
        /// <returns></returns>
        public static string[] GetChatCountByDateList(List<DateTime> dateSpan, int wxID,Common.FromUserType fromType)
        {
            string[] list = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                if (dateSpan.Count > 0)
                {
                    list = new string[dateSpan.Count];
                    string count = "0";
                    for (int i = 0; i < list.Length; i++)
                    {
                        var start = dateSpan[i];
                        DateTime end = i + 1 == dateSpan.Count ? dateSpan[i].AddDays(1) : dateSpan[i + 1];
                        count = (from x in db.t_wx_fans_chats where x.create_time > start && x.create_time < end && x.from_type == (int)fromType && x.wx_id == wxID select x).Count().ToString();
                        list[i] = count;
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 通过连续日期列表取统计数据
        /// </summary>
        /// <param name="dateSpan"></param>
        /// <param name="wxID"></param>
        /// <param name="fromType"></param>
        /// <returns></returns>
        public static int[] GetTrendCountByDateList(List<DateTime> dateSpan, int wxID, Common.FromUserType fromType)
        {
            int[] list = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                if (dateSpan.Count > 0)
                {
                    list = new int[dateSpan.Count];
                    int count = 0;
                    for (int i = 0; i < list.Length; i++)
                    {
                        var start = dateSpan[i];
                        DateTime end = i + 1 == dateSpan.Count ? dateSpan[i].AddDays(1) : dateSpan[i + 1];
                        count = (from x in db.t_wx_fans_chats where x.create_time > start && x.create_time < end && x.from_type == (int)fromType && x.wx_id == wxID select x).Count();
                        list[i] = count;
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 通过连续日期列表取统计数据
        /// </summary>
        /// <param name="dateSpan"></param>
        /// <param name="wxID"></param>
        /// <param name="fromType"></param>
        /// <returns></returns>
        public static int[] GetTrendGroupCountByDateList<T>(List<DateTime> dateSpan, int wxID, Common.FromUserType fromType,Expression<Func<t_wx_fans_chats,T>> groupBy)
        {
            int[] list = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                if (dateSpan.Count > 0)
                {
                    list = new int[dateSpan.Count];
                    int count = 0;
                    for (int i = 0; i < list.Length; i++)
                    {
                        var start = dateSpan[i];
                        DateTime end = i + 1 == dateSpan.Count ? dateSpan[i].AddDays(1) : dateSpan[i + 1];
                        count = db.t_wx_fans_chats.Where(x => x.create_time > start && x.create_time < end && x.from_type == (int)fromType && x.wx_id == wxID).GroupBy(groupBy.Expand()).Count();
                        //count = (from x in db.t_wx_fans_chats where x.create_time > start && x.create_time < end && x.from_type == (int)fromType && x.wx_id == wxID select x).Count();
                        list[i] = count;
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 通过连续日期获取未选中公众号发送（接收）消息数据
        /// </summary>
        /// <param name="dateSpan"></param>
        /// <param name="wxID"></param>
        /// <param name="fromType"></param>
        /// <returns></returns>
        public static string[] GetChatCountByDateListNoSel(List<DateTime> dateSpan, int[] wxIDs, Common.FromUserType fromType)
        {
            string[] list = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                if (dateSpan.Count > 0)
                {
                    list = new string[dateSpan.Count];
                    string count = "0";
                    for (int i = 0; i < list.Length; i++)
                    {
                        var start = dateSpan[i];
                        DateTime end = i + 1 == dateSpan.Count ? dateSpan[i].AddDays(1) : dateSpan[i + 1];
                        count = (from x in db.t_wx_fans_chats where x.create_time > start && x.create_time < end && x.from_type == (int)fromType && wxIDs.Contains(x.wx_id) select x).Count().ToString();
                        list[i] = count;
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 获取数量 -Damos
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static int GetCount(Expression<Func<t_wx_fans_chats, bool>> where)
        {
            int count = 0;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                count = db.t_wx_fans_chats.Where(where.Expand()).Count();
            }
            return count;
        }

        /// <summary>
        /// 获取openid列表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static string[] GetFansOpenIDArray(Expression<Func<t_wx_fans_chats, bool>> where)
        {
            string[] list = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                list = db.t_wx_fans_chats.Where(where).GroupBy(x => x.open_id).Select(x => x.Key).ToArray();
                list.Distinct();
            }
            return list;
        }

        /// <summary>
        /// 获取统计数据 -Damos
        /// </summary>
        /// <param name="groupBy"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static List<CountStatistics> GetCountStatistics(Expression<Func<t_wx_fans_chats, string>> groupBy, Expression<Func<t_wx_fans_chats, bool>> where)
        {
            List<Chats.CountStatistics> list = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                list = db.t_wx_fans_chats.Where(where.Expand()).GroupBy(groupBy.Expand()).Select(x => new CountStatistics { count = x.Count(), key = x.Key == "" ? "未知" : x.Key }).ToList();
            }
            return list;
        }
    }
}


