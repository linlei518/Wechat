using KDWechat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityFramework.Extensions;
using KDWechat.Common;
using KDWechat.BLL.Chats;
using System.Linq.Expressions;
using Senparc.Weixin.MP.AdvancedAPIs;
using System.Data.Entity.Validation;
using LinqKit;
using Senparc.Weixin.MP.CommonAPIs;
using KDWechat.DBUtility;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace KDWechat.BLL.Users
{
    public class wx_fans
    {
        #region 公开方法
        /// <summary>
        /// 通过ID取得粉丝
        /// </summary>
        /// <param name="id">粉丝ID</param>
        /// <returns>取得的粉丝</returns>
        public static t_wx_fans GetFansByID(int id)
        {
            t_wx_fans fan = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                fan = (from x in db.t_wx_fans where x.id == id select x).FirstOrDefault();
            }
            return fan;
        }
        /// <summary>
        /// 通过openID取得粉丝
        /// </summary>
        /// <param name="id">粉丝openID</param>
        /// <returns>取得的粉丝</returns>
        public static t_wx_fans GetFansByID(string opid)
        {
            t_wx_fans fan = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                fan = (from x in db.t_wx_fans where x.open_id == opid select x).FirstOrDefault();
            }
            return fan;
        }

        /// <summary>
        /// 设置回复状态与最后互动时间
        /// </summary>
        /// <param name="fans_id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static bool SetReplyStateAndTime(int fans_id, FansReplyState state)
        {
            bool isComplete = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                var query = db.t_wx_fans.Where(x => x.id == fans_id);
                if (state == FansReplyState.已回复)
                    isComplete = query.Update(x => new t_wx_fans() { reply_state = (int)state }) > 0;
                else
                    isComplete = query.Update(x => new t_wx_fans() { last_interact_time = DateTime.Now, reply_state = (int)state }) > 0;
            }
            return isComplete;
        }

        /// <summary>
        /// 设置回复状态与最后互动时间
        /// </summary>
        /// <param name="fans_id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static bool SetReplyStateAndTime(string openID, FansReplyState state)
        {
            bool isComplete = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                var query = db.t_wx_fans.Where(x => x.open_id == openID);
                if (state == FansReplyState.已回复)
                    isComplete = query.Update(x => new t_wx_fans() { reply_state = (int)state }) > 0;
                else
                    isComplete = query.Update(x => new t_wx_fans() { last_interact_time = DateTime.Now, reply_state = (int)state }) > 0;
            }
            return isComplete;
        }

        /// <summary>
        /// 设置最后互动时间
        /// </summary>
        /// <param name="fans_id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static bool SetLastTime(int fans_id)
        {
            bool isComplete = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                var query = db.t_wx_fans.Where(x => x.id == fans_id);
                isComplete = query.Update(x => new t_wx_fans() { last_interact_time = DateTime.Now }) > 0;
            }
            return isComplete;
        }

        /// <summary>
        /// 设置最后互动时间
        /// </summary>
        /// <param name="fans_id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static bool SetLastTime(string open_id)
        {
            bool isComplete = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                var query = db.t_wx_fans.Where(x => x.open_id == open_id);
                isComplete = query.Update(x => new t_wx_fans() { last_interact_time = DateTime.Now }) > 0;
            }
            return isComplete;
        }

        public static bool CheckFans(string opid)
        {

            System.Data.DataTable table = KDWechat.DBUtility.DbHelperSQL.Query("select top 1 id from t_wx_fans where open_id='" + opid + "'").Tables[0];
            if (table != null && table.Rows.Count > 0)
                return true;
            return false;
        }

        /// <summary>
        /// 获取openid列表
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public static string[] GetOpenIDs(Expression<Func<t_wx_fans, bool>> where)
        {
            string[] list = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                list = db.t_wx_fans.Where(where.Expand()).Select(x => x.open_id).ToArray();
            }
            return list;
        }


        /// <summary>
        /// 提取粉丝列表
        /// </summary>
        /// <param name="pagesize">每页容量</param>
        /// <param name="pageindex">当前页码</param>
        /// <returns>对应的粉丝列表</returns>
        public static List<t_wx_fans> GetFansListBySizeAndIndex(int pagesize, int pageindex)
        {
            List<t_wx_fans> fanList = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                fanList = (from x in db.t_wx_fans orderby x.id descending select x).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            }
            return fanList;
        }

        /// <summary>
        /// 根据OPENID列表获取粉丝列表
        /// </summary>
        /// <param name="openids">openid列表</param>
        /// <returns></returns>
        public static List<t_wx_fans> GetFansListByOpenIDs(string[] openids)
        {
            List<t_wx_fans> fanList = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                fanList = (from x in db.t_wx_fans where openids.Contains(x.open_id) select x).ToList();
            }
            return fanList;
        }



        /// <summary>
        /// 提取粉丝列表
        /// </summary>
        /// <param name="wx_id">微信ID</param>
        /// <param name="pagesize">每页容量</param>
        /// <param name="pageindex">当前页码</param>
        /// <returns>对应的粉丝列表</returns>
        public static List<t_wx_fans> GetFansListByWxID(int wx_id, int pagesize, int pageindex)
        {
            List<t_wx_fans> fanList = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                fanList = (from x in db.t_wx_fans where x.wx_id == wx_id orderby x.id descending select x).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            }
            return fanList;
        }

        /// <summary>
        /// 提取粉丝列表
        /// </summary>
        /// <param name="wx_id">微信ID</param>
        /// <param name="pagesize">每页容量</param>
        /// <param name="pageindex">当前页码</param>
        /// <returns>对应的粉丝列表</returns>
        public static List<FansStatistics> GetFansListByWxID(int wx_id)
        {
            List<FansStatistics> fanList = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                fanList = (from x in db.t_wx_fans where x.wx_id == wx_id orderby x.id descending select new FansStatistics { city = x.city, language = x.language, sex = x.sex ?? 2, country = x.country, province = x.province, status = x.status, subscribe_time = x.subscribe_time, remove_time = x.remove_time }).ToList();
            }
            return fanList;
        }

        public static List<FansStatistics> GetFansListByWxID(int wx_id, DateTime start, DateTime end)
        {
            List<FansStatistics> fanList = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                fanList = (from x in db.t_wx_fans where x.wx_id == wx_id && ((x.subscribe_time >= start && x.subscribe_time <= end) || (x.remove_time >= start && x.remove_time <= end)) orderby x.id descending select new FansStatistics { city = x.city, language = x.language, sex = x.sex ?? 2, country = x.country, province = x.province, status = x.status, subscribe_time = x.subscribe_time, remove_time = x.remove_time }).ToList();
            }
            return fanList;

        }


        /// <summary>
        /// 通过微信ID获取粉丝列表
        /// </summary>
        /// <param name="where"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<Entity.user_list_model> GetFansListByWxID(Expression<Func<t_wx_fans, bool>> where, int pagesize, int pageindex, out int count)
        {
            List<Entity.user_list_model> fanList = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                var query = db.t_wx_fans.Where(where.Expand());
                fanList = query.OrderByDescending(x => x.id).Skip((pageindex - 1) * pagesize).Take(pagesize).Select(x => new Entity.user_list_model { wx_id = x.wx_id, id = x.id, reply_state = x.reply_state, headimgurl = x.headimgurl, last_interact_time = x.last_interact_time, nick_name = x.nick_name, open_id = x.open_id, unionid = x.unionid, group_id = x.group_id }).ToList();
                count = query.Count();
            }
            return fanList;

        }

        /// <summary>
        /// 通过微信ID获取粉丝列表
        /// </summary>
        /// <param name="where"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<t_wx_fans_export> GetAllFansList(Expression<Func<t_wx_fans, bool>> where)
        {
            List<t_wx_fans_export> fanList = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                var query = db.t_wx_fans.Where(where.Expand()).Select(x => new t_wx_fans_export { openid = x.open_id, city = x.city, province = x.province, country = x.country, language = x.language, nick_name = x.nick_name, sex = x.sex ?? 0 });
                fanList = query.ToList();
            }
            return fanList;

        }

        /// <summary>
        /// 删除粉丝信息
        /// </summary>
        /// <param name="id">粉丝ID</param>
        /// <returns>删除是否成功</returns>
        public static bool DeleteFanByID(int id)
        {
            creater_wxEntities db = new creater_wxEntities();
            bool isFinish = (from x in db.t_wx_fans where x.id == id select x).Delete() > 0;
            db.Dispose();
            return isFinish;
        }




        #endregion





        /// <summary>
        /// 添加一个粉丝
        /// </summary>
        /// <param name="fan"></param>
        /// <returns></returns>
        public static t_wx_fans InsertFans(t_wx_fans fan)
        {
            using (var transaction = new System.Transactions.TransactionScope())
            {
                using (creater_wxEntities db = new creater_wxEntities())
                {
                    var searFan = db.t_wx_fans.Where(x => x.open_id == fan.open_id).FirstOrDefault();
                    if (searFan != null)
                        fan = searFan;
                    else
                    {
                        db.t_wx_fans.Add(fan);
                        db.SaveChanges();
                    }
                }
                transaction.Complete();
            }
            return fan;
        }

        public static bool InsertFansBySql(t_wx_fans user)
        {

            var insertSql = "insert into t_wx_fans (city,sex,province,open_id,nick_name,country,language,subscribe_time,headimgurl,guid,wx_og_id,wx_id,status,wx_country,wx_city,wx_sex,wx_province,source_id) values (@city,@sex,@province,@open_id,@nick_name,@country,@language,@subscribe_time,@headimgurl,@guid,@wx_og_id,@wx_id,@status,@wx_country,@wx_city,@wx_sex,@wx_province,@source_id)";
            System.Data.SqlClient.SqlParameter[] Sqlparams = new System.Data.SqlClient.SqlParameter[18];

            Sqlparams[0] = new System.Data.SqlClient.SqlParameter("@city", user.city);
            Sqlparams[1] = new System.Data.SqlClient.SqlParameter("@sex", user.sex);
            Sqlparams[2] = new System.Data.SqlClient.SqlParameter("@province", user.province);
            Sqlparams[3] = new System.Data.SqlClient.SqlParameter("@open_id", user.open_id);
            Sqlparams[4] = new System.Data.SqlClient.SqlParameter("@nick_name", user.nick_name);
            Sqlparams[4].Size = 80;
            Sqlparams[5] = new System.Data.SqlClient.SqlParameter("@country", user.country);
            Sqlparams[6] = new System.Data.SqlClient.SqlParameter("@language", user.language);
            Sqlparams[7] = new System.Data.SqlClient.SqlParameter("@subscribe_time", user.subscribe_time);
            Sqlparams[8] = new System.Data.SqlClient.SqlParameter("@headimgurl", user.headimgurl);
            Sqlparams[9] = new System.Data.SqlClient.SqlParameter("@guid", user.guid);
            Sqlparams[10] = new System.Data.SqlClient.SqlParameter("@wx_og_id", user.wx_og_id);
            Sqlparams[11] = new System.Data.SqlClient.SqlParameter("@wx_id", user.wx_id);
            Sqlparams[12] = new System.Data.SqlClient.SqlParameter("@status", user.status);
            Sqlparams[13] = new System.Data.SqlClient.SqlParameter("@wx_country", user.wx_country);
            Sqlparams[14] = new System.Data.SqlClient.SqlParameter("@wx_city", user.wx_city);
            Sqlparams[15] = new System.Data.SqlClient.SqlParameter("@wx_sex", user.wx_sex);
            Sqlparams[16] = new System.Data.SqlClient.SqlParameter("@wx_province", user.wx_province);
            Sqlparams[17] = new System.Data.SqlClient.SqlParameter("@source_id", user.source_id);

            var query = "if not exists (select 1 from t_wx_fans where open_id=@open_id) begin " + insertSql + " end";

            int count = KDWechat.DBUtility.DbHelperSQL.ExecuteSql(query, Sqlparams);
            return count > 0;
        }
        /// <summary>
        /// 更新一个粉丝
        /// </summary>
        /// <param name="fan"></param>
        /// <returns></returns>
        public static bool UpdateFans(t_wx_fans fan)
        {
            return EFHelper.UpdateUser<t_wx_fans>(fan);
        }

        /// <summary>
        /// 批量修改用户分组
        /// </summary>
        /// <param name="fansList"></param>
        /// <param name="group_id"></param>
        /// <returns></returns>
        public static bool UpdateFansGroupList(string[] fansList, int group_id)
        {
            int result = 0;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                int[] fans = new int[fansList.Length];
                for (int i = 0; i < fans.Length; i++)
                {
                    fans[i] = Utils.StrToInt(fansList[i], 0);
                }
                foreach (var fans_id in fans)
                {
                    if (fans_id > 0 && group_id > -1)
                    {
                        result = db.t_wx_fans.Where(x => x.id == fans_id).Update(x => new t_wx_fans { group_id = group_id });
                    }
                }
            }

            return result > 0;
        }

        /// <summary>
        /// 更新粉丝分组
        /// </summary>
        /// <param name="fans_id"></param>
        /// <param name="group_id"></param>
        /// <returns></returns>
        public static bool UpdateFansGroup(int fans_id, int group_id)
        {
            int result = 0;
            if (fans_id > 0 && group_id > -1)
            {
                using (creater_wxEntities db = new creater_wxEntities())
                {
                    result = db.t_wx_fans.Where(x => x.id == fans_id).Update(x => new t_wx_fans { group_id = group_id });
                }
            }

            return result > 0;
        }

        /// <summary>
        /// 获取粉丝的聊天状态
        /// </summary>
        /// <param name="open_id">粉丝的openid</param>
        /// <param name="lastTime">最后的互动时间</param>
        /// <returns></returns>
        public static FansChatStatus GetFansChatStatus(string open_id, ref DateTime lastTime)
        {
            KDWechat.DAL.t_wx_fans_chats fans_chat = KDWechat.BLL.Logs.wx_fans_chats.GetLastFansChatsByOpenid(open_id);
            if (fans_chat != null)
            {
                lastTime = fans_chat.create_time;
                if (fans_chat.from_type == 2)
                {
                    return FansChatStatus.已回复;
                }
                else
                {
                    TimeSpan ts = DateTime.Now - fans_chat.create_time;
                    int hours = ts.Hours;
                    hours += ts.Days * 24;
                    if (hours > 48)
                    {
                        return FansChatStatus.已过期;
                    }
                    else
                    {
                        return FansChatStatus.未回复;
                    }
                }
            }
            else
            {
                lastTime = DateTime.Now.AddDays(-7);
                return FansChatStatus.暂无;
            }
        }

        /// <summary>
        /// 获取聊天状态
        /// </summary>
        /// <param name="open_id"></param>
        /// <returns></returns>
        public static FansChatStatus GetFansChatStatus(string open_id)
        {
            DateTime lastTime = DateTime.Now;
            KDWechat.DAL.t_wx_fans_chats fans_chat = KDWechat.BLL.Logs.wx_fans_chats.GetLastFansChatsByOpenid(open_id);
            if (fans_chat != null)
            {
                lastTime = fans_chat.create_time;
                if (fans_chat.from_type == 2)
                {
                    return FansChatStatus.已回复;
                }
                else
                {
                    TimeSpan ts = DateTime.Now - fans_chat.create_time;
                    int hours = ts.Hours;
                    hours += ts.Days * 24;
                    if (hours > 48)
                    {
                        return FansChatStatus.已过期;
                    }
                    else
                    {
                        return FansChatStatus.未回复;
                    }
                }
            }
            else
            {
                lastTime = DateTime.Now.AddDays(-7);
                return FansChatStatus.暂无;
            }
        }

        /// <summary>
        /// 获取聊天状态
        /// </summary>
        /// <param name="open_id"></param>
        /// <returns></returns>
        public static FansChatsTypeNew GetFansChatStatus(object last_interact_time, object replyState)
        {
            if (last_interact_time == null || replyState == null)
                return FansChatsTypeNew.暂无;
            FansReplyState reply_state = (FansReplyState)Utils.StrToInt(replyState.ToString(), 0);
            if (reply_state == FansReplyState.暂无)
                return FansChatsTypeNew.暂无;
            else
            {
                DateTime last_time = Utils.StrToDateTime(last_interact_time.ToString(), DateTime.Now.AddDays(-100));
                if (last_time.AddDays(2) > DateTime.Now)//未过期
                {
                    if (reply_state == FansReplyState.已回复)
                        return FansChatsTypeNew.已回复;
                    else
                        return FansChatsTypeNew.未回复;
                }
                else//已过期
                {
                    if (reply_state == FansReplyState.已回复)
                        return FansChatsTypeNew.已过期;
                    else
                        return FansChatsTypeNew.未回复已过期;
                }
            }
        }

        /// <summary>
        /// 获取粉丝的最后聊天时间
        /// </summary>
        /// <param name="open_id">粉丝的openid</param>
        /// <returns></returns>
        public static DateTime GetFansChatLastTime(string open_id)
        {
            DateTime lastTime = DateTime.Now;
            KDWechat.DAL.t_wx_fans_chats fans_chat = KDWechat.BLL.Logs.wx_fans_chats.GetLastFansChatsByOpenid(open_id);
            if (fans_chat != null)
            {
                lastTime = fans_chat.create_time;
            }
            else
            {
                lastTime = DateTime.Now.AddDays(-7);
            }
            return lastTime;
        }

        /// <summary>
        /// 获取粉丝的guid获取绑定的会员名称
        /// </summary>
        /// <param name="unionid">粉丝表里的unionid</param>
        /// <returns></returns>
        public static string GetMemberName(string unionid)
        {
            string name = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                if (unionid != null)
                {
                    name = (from x in db.t_member_info where x.unionid == unionid select x.user_name).FirstOrDefault();
                }
            }
            return name;
        }

        /// <summary>
        /// 获取时间段订阅获取消订阅数量总数
        /// </summary>
        /// <param name="wxid"></param>
        /// <param name="Start"></param>
        /// <param name="End"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static int GetFansCountByDateAndStatus(int wxid, DateTime Start, DateTime End, Status state)
        {
            DateTime startDate = Start.AddDays(-1);
            DateTime endDate = End.AddDays(1);
            int count = 0;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                count = (from x in db.t_wx_fans where x.wx_id == wxid && x.status == (int)state && x.subscribe_time > startDate && x.subscribe_time < endDate select x.id).Count();
            }
            return count;
        }

        /// <summary>
        /// 获取时间段累计粉丝总数
        /// </summary>
        /// <param name="wxid"></param>
        /// <param name="Start"></param>
        /// <param name="End"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static int GetFansCountByDate(int wxid, DateTime Start, DateTime End, Status state = Status.正常)
        {
            DateTime startDate = Start.AddDays(-1);
            DateTime endDate = End.AddDays(1);
            int count = 0;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                count = (from x in db.t_wx_fans where x.wx_id == wxid && x.status == (int)state && x.subscribe_time > startDate && x.subscribe_time < endDate select x.id).Count();
            }
            return count;
        }



        /// <summary>
        /// 获取每周订阅数的统计
        /// </summary>
        /// <param name="wxid"></param>
        /// <returns></returns>
        public static string[] GetFansCountWeekly(int wxid, Status state = Status.正常)
        {
            string[] weekList = new string[7];
            using (creater_wxEntities db = new creater_wxEntities())
            {
                int count;
                for (int i = 0; i < 7; i++)
                {
                    DateTime startDate = DateTime.Now.Date.AddDays(-i);
                    DateTime endDate = DateTime.Now.Date.AddDays(1 - i);
                    count = (from x in db.t_wx_fans where x.wx_id == wxid && x.status == (int)state && x.subscribe_time > startDate && x.subscribe_time < endDate select x.id).Count();
                    weekList[6 - i] = count.ToString();
                }
            }
            return weekList;
        }

        //获取关注总人数
        public static string[] GetAllFansCountByDateList(List<DateTime> dateSpan, int wxID)
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
                        count = (from x in db.t_wx_fans where x.subscribe_time < end && x.status == (int)Status.正常 && x.wx_id == wxID select x).Count().ToString();
                        list[i] = count;
                    }
                }
            }
            return list;
        }


        public static string[] GetFansCountByDateList(List<DateTime> dateSpan, int wxID)
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
                        count = (from x in db.t_wx_fans where x.subscribe_time > start && x.subscribe_time < end && x.status == (int)Status.正常 && x.wx_id == wxID select x).Count().ToString();
                        list[i] = count;
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 获取人数趋势图统计
        /// </summary>
        /// <param name="dateSpan"></param>
        /// <param name="wxID"></param>
        /// <returns></returns>
        public static int[] GetTrendByDateList(List<DateTime> dateSpan, List<FansStatistics> fansList, Status status)
        {
            int[] list = null;
            if (dateSpan.Count > 0)
            {
                list = new int[dateSpan.Count];
                int count = 0;
                for (int i = 0; i < list.Length; i++)
                {
                    var start = dateSpan[i];
                    DateTime end = i + 1 == dateSpan.Count ? dateSpan[i].AddDays(1) : dateSpan[i + 1];
                    if (status == Status.正常)
                        count = (from x in fansList where x.subscribe_time > start && x.subscribe_time < end select x).Count();
                    else
                        count = (from x in fansList where x.remove_time > start && x.remove_time < end select x).Count();
                    list[i] = count;
                }
            }
            return list;
        }
        /// <summary>
        /// 按时间获取未选中微信号的订阅数量
        /// </summary>
        /// <param name="dateSpan"></param>
        /// <param name="wxID"></param>
        /// <returns></returns>
        public static string[] GetFansCountByDateListNoSel(List<DateTime> dateSpan, int[] wxIDs)
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
                        count = (from x in db.t_wx_fans where x.subscribe_time > start && x.subscribe_time < end && x.status == (int)Status.正常 && wxIDs.Contains(x.wx_id) select x).Count().ToString();
                        list[i] = count;
                    }
                }
            }
            return list;
        }

        //获取取消订阅数量
        public static string[] GetUnFansCountByDateList(List<DateTime> dateSpan, int wxID)
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
                        count = (from x in db.t_wx_fans where x.remove_time > start && x.remove_time < end && x.status == (int)Status.禁用 && x.wx_id == wxID select x).Count().ToString();
                        list[i] = count;
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// 按时间获取未选中微信号的退订数量
        /// </summary>
        /// <param name="dateSpan"></param>
        /// <param name="wxID"></param>
        /// <returns></returns>
        public static string[] GetUnFansCountByDateListNoSel(List<DateTime> dateSpan, int[] wxIDs)
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
                        count = (from x in db.t_wx_fans where x.remove_time > start && x.remove_time < end && x.status == (int)Status.禁用 && wxIDs.Contains(x.wx_id) select x).Count().ToString();
                        list[i] = count;
                    }
                }
            }
            return list;
        }

        public static int GetTotalCountByWxID(int wxID)
        {
            int count = 0;
            if (wxID > 0)
            {
                using (creater_wxEntities db = new creater_wxEntities())
                {
                    count = (from x in db.t_wx_fans where x.wx_id == wxID && x.status == (int)Status.正常 select 1).Count();
                }
            }
            return count;
        }

        /// <summary>
        /// 获取未选中公众号的数量
        /// </summary>
        /// <param name="wxID"></param>
        /// <returns></returns>
        public static int GetTotalCountByNoSel(int[] idList)
        {
            int count = 0;

            using (creater_wxEntities db = new creater_wxEntities())
            {
                count = (from x in db.t_wx_fans where idList.Contains(x.wx_id) && x.status == (int)Status.正常 select 1).Count();
            }

            return count;
        }


        /// <summary>
        /// 获取数量
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static int GetCount(Expression<Func<t_wx_fans, bool>> where)
        {
            int count = 0;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                count = db.t_wx_fans.Where(where).Count();
            }
            return count;
        }

        /// <summary>
        /// 获取粉丝表相关的统计数量
        /// </summary>
        /// <param name="groupBy"></param>
        /// <param name="wx_id"></param>
        /// <returns></returns>
        public static List<Chats.CountStatistics> GetCountStatistics(Expression<Func<t_wx_fans, string>> groupBy, Expression<Func<t_wx_fans, bool>> where)
        {
            List<Chats.CountStatistics> list = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                list = db.t_wx_fans.Where(where.Expand()).GroupBy(groupBy.Expand()).Select(x => new CountStatistics { count = x.Count(), key = x.Key == "" ? "未知" : x.Key }).ToList();
            }
            return list;
        }

        /// <summary>
        /// 获取粉丝表相关的统计数量
        /// </summary>
        /// <param name="groupBy"></param>
        /// <param name="wx_id"></param>
        /// <returns></returns>
        public static List<Chats.CountStatistics> GetSexCountStatistics(Expression<Func<t_wx_fans, int>> groupBy, Expression<Func<t_wx_fans, bool>> where)
        {
            List<Chats.CountStatistics> list = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                list = db.t_wx_fans.Where(where.Expand()).GroupBy(groupBy.Expand()).Select(x => new CountStatistics { count = x.Count(), key = ((WeChatSex)x.Key).ToString() }).ToList();
            }
            return list;
        }

        /// <summary>
        /// 更新一个公众号的所有粉丝
        /// </summary>
        /// <param name="wx_id"></param>
        /// <returns></returns>
        public static Tuple<int, int> UpdateAllFans(int wx_id)
        {
            int allCount = 0;
            int totalCount = 0;
            bool isComplete = false;//返回值
            if (wx_id > 0)
            {
                var wechat = Chats.wx_wechats.GetWeChatByID(wx_id);//获取公众号信息
                if (null != wechat)
                {
                    string accessToken = BLL.Chats.wx_wechats.GetAccessToken(wechat.id, wechat);//  Senparc.Weixin.MP.CommonAPIs.AccessTokenContainer.TryGetToken(wechat.app_id,wechat.app_secret);//获取accesstoken,如果人数多，可以考虑放入循环中。
                    System.Net.WebClient wc = new System.Net.WebClient();//新建一个wc
                    string nextOpenID = "";//下一个openid
                    openid_result result = null;//返回的结果
                    do
                    {
                        string json = wc.DownloadString("https://api.weixin.qq.com/cgi-bin/user/get?access_token=" + accessToken + "&next_openid=" + nextOpenID);//获取结果JSON
                        result = Newtonsoft.Json.JsonConvert.DeserializeObject<openid_result>(json);//逆解析JSON
                        if (result.total > 0)//取到了结果
                        {
                            allCount = result.total;
                            isComplete = true;//标为true
                            var addedOpid = Companycn.Core.EntityFramework.EFHelper.GetArray<creater_wxEntities, t_wx_fans, string>(x => result.data.openid.Contains(x.open_id), x => x.open_id);
                            var trulyOpid = result.data.openid.Where(x => !addedOpid.Contains(x)).ToArray();
                            foreach (var opid in trulyOpid)//循环访问所有OPENID
                            {
                                //var fans = GetFansByID(opid);//获取粉丝
                                if (CheckOpid(opid))//为空的时候添加新粉丝
                                {
                                    var user = User.Info(accessToken, opid);
                                    var fans = new KDWechat.DAL.t_wx_fans()
                                    {
                                        city = user.city,
                                        sex = user.sex,
                                        province = user.province,
                                        open_id = opid,
                                        nick_name = user.nickname,
                                        country = user.country,
                                        language = user.language,
                                        subscribe_time = DateTime.Parse("1970-1-1").AddSeconds(user.subscribe_time),
                                        headimgurl = user.headimgurl,
                                        guid = Guid.NewGuid().ToString().Replace("-", ""),
                                        wx_og_id = wechat.wx_og_id,
                                        wx_id = wechat.id,
                                        status = user.subscribe,

                                        wx_country = user.country,
                                        wx_city = user.city,
                                        wx_sex = user.sex,
                                        wx_province = user.province,
                                        source_id = 0

                                    };
                                    if (user.subscribe == 0)
                                    {
                                        fans.remove_time = fans.subscribe_time;
                                    }
                                    InsertFansBySql(fans);
                                    totalCount++;
                                }
                            }
                            nextOpenID = result.next_openid;
                        }
                    }
                    while (result.count == 10000);
                    wc.Dispose();
                }
            }
            return new Tuple<int, int>(allCount, totalCount);
        }

        private static bool CheckOpid(string opid)
        {

            var table = KDWechat.DBUtility.DbHelperSQL.Query("select top 1 id from t_wx_fans where open_id='" + opid + "'").Tables[0];
            return table == null || table.Rows.Count <= 0;
        }


        /// <summary>
        /// 根据OPENID计算用户是否在客服状态
        /// </summary>
        /// <param name="opid"></param>
        /// <returns>true:</returns>
        public static FansState CheckUser(string opid)
        {
            FansState state = FansState.自动回复状态;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                var fans = db.t_wx_fans.Where(x => x.open_id == opid).FirstOrDefault();
                if (fans != null)
                {
                    //var span = DateTime.Now - (fans.active_time ?? DateTime.Now.AddDays(-7));
                    //if (span.TotalMinutes <= 10)
                    state = fans.state == null ? FansState.自动回复状态 : (FansState)fans.state;
                }
            }
            return state;
        }


        public static void ExitSell(string openid)
        {
            using (creater_wxEntities db = new creater_wxEntities())
            {
                var fans = db.t_wx_fans.Where(x => x.open_id == openid).FirstOrDefault();
                if (fans != null && (fans.state == (int)FansState.客服聊天状态 || fans.state == (int)FansState.选择项目状态))
                {
                    wx_fans.SetState(fans.open_id, FansState.自动回复状态);
                    //fans.state = (int)FansState.自动回复状态;
                    var wx_wechat = wx_wechats.GetWeChatByID(fans.wx_id);
                    if (wx_wechat != null)
                    {
                        string accessToken = wx_wechats.GetAccessToken(wx_wechat.id);//wx_wechat.access_token;// AccessTokenContainer.TryGetToken(wx_wechat.app_id, wx_wechat.app_secret);
                        var resultText = Custom.SendText(accessToken, openid, "您已退出销售客服模式，我们的销售客服将不能接收到您之后的消息，关键词回复功能开启。");
                    }
                    db.SaveChanges();
                }
            }

        }

        /// <summary>
        /// 给用户设置状态
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="fansState"></param>
        public static void SetState(string openId, FansState fansState, string mobile = "")
        {
            using (creater_wxEntities db = new creater_wxEntities())
            {
                var fans = db.t_wx_fans.Where(x => x.open_id == openId).FirstOrDefault();
                if (fans != null)
                {
                    fans.state = (int)fansState;
                    if (!string.IsNullOrEmpty(mobile))
                    {
                        fans.mobile = mobile;
                    }
                    db.SaveChanges();
                }
            }
        }
        /// <summary>
        /// 给用户设置状态
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="fansState"></param>
        public static void SetState(FansState fansState)
        {
            using (creater_wxEntities db = new creater_wxEntities())
            {
                //var fans = db.t_wx_fans.Where(x => x.state==(int)FansState.客服聊天状态||x.state==(int)FansState.选择项目状态).ToList();
                db.t_wx_fans.Where(x => x.state == (int)FansState.客服聊天状态 || x.state == (int)FansState.选择项目状态).Update(x => new t_wx_fans() { state = (int)fansState });

                //creater_wxEntities db2 = new creater_wxEntities();
                //db2.t_md_sale_users_fans_relation.Where(x => x.status == (int)Status.正常).Update(x => new t_md_sale_users_fans_relation { status = (int)Status.禁用 });
                //db2.SaveChanges();
                //db2.Dispose();
                //db.SaveChanges();
            }
        }

        public static Tuple<string, List<Fans_Filter>> GetAdvanceQuery(string json, int wx_id, int u_id)
        {
            var query = "select group_id,id,headimgurl,nick_name,unionid,last_interact_time,reply_state,open_id from t_wx_fans where wx_id=" + wx_id + " ";
            List<Fans_Filter> filterList = JsonConvert.DeserializeObject<List<Fans_Filter>>(json);//获取所有查询条件的列表
            List<Fans_Filter_Collection> collectionList = new List<Fans_Filter_Collection>();
            foreach (var filter in filterList)//将同类型条件合并
            {
                var item = collectionList.Where(x => x.category == filter.sh).FirstOrDefault();
                if (item == null)
                {
                    collectionList.Add(new Fans_Filter_Collection() { category = filter.sh, properties = (filter.values + "|") });
                }
                else
                    item.properties += (filter.values + "|");
            }

            foreach (var col in collectionList)//遍历所有条件，拼接查询语句
            {
                col.properties = col.properties.TrimEnd('|');
                switch (col.category)
                {
                    case "WeChatSex":
                        query += string.Format(" and [wx_sex] in ({0}) ", col.properties.Replace('|', ','));
                        break;
                    case "MsgContain":
                        var containList = col.properties.Split('|');
                        foreach (var x in containList)
                        {
                            switch (x)
                            {
                                case "0":
                                    query += " and [tel] is not null and [tel]!='' ";
                                    break;
                                case "1":
                                    query += " and [real_name] is not null and [real_name]!='' ";
                                    break;
                                case "2":
                                    query += " and [id_card] is not null and [id_card]!='' ";
                                    break;
                            }
                        }
                        break;
                    case "SuscribeTime":
                        var susTime = col.properties.Split('—');
                        if (susTime.Length == 2)
                        {
                            query += string.Format(" and [subscribe_time] between '{0}' and '{1}' ", susTime[0].Replace(" ", ""), susTime[1].Replace(" ", ""));
                        }
                        break;
                    case "WeChatArea":
                        var wechatAreaList = col.properties.Replace("省", "").Replace("市", "").Replace("区", "").Replace("县", "").Replace("旗", "").Split('|');
                        if (wechatAreaList.Length > 0)
                        {
                            query += " and (1=2 ";
                            foreach (var area in wechatAreaList)
                            {
                                var adrList = area.Split('-');
                                if (adrList.Length == 3)
                                {
                                    var province = adrList[0];
                                    var city = adrList[1];
                                    if (province.Contains("上海") || province.Contains("北京") || province.Contains("天津") || province.Contains("重庆"))
                                    {
                                        city = adrList[2];
                                    }
                                    query += " or ( ";
                                    if (province != "全部")
                                        query += string.Format(" [wx_province] ='{0}' ", province);
                                    if (city != "全部")
                                        query += string.Format(" and [wx_city]='{0}' ", city);
                                    query += ")";
                                }
                            }
                            query += ")";
                        }
                        break;
                    case "Nation":
                        var nationList = col.properties.Replace("|", "','");
                        query += string.Format(" and [ethnic] in ('{0}')", nationList);
                        break;
                    case "CardType":
                        var cardTpList = col.properties.Replace("|", "','");
                        query += string.Format(" and [id_card_type] in ('{0}') ", cardTpList);
                        break;
                    case "Birth":
                        var birth = col.properties.Replace(" ", "").Split('—');
                        if (birth.Length == 2)
                        {
                            query += string.Format(" and CONVERT(varchar(5),birth,110) between '{0}' and '{1}' ", birth[0], birth[1]);
                        }
                        break;
                    case "BirthDay":
                        var birthday = col.properties.Replace(" ", "").Split('—');
                        if (birthday.Length == 2)
                        {
                            query += string.Format(" and [birth] between '{0}' and '{1}' ", birthday[0], birthday[1]);
                        }
                        break;
                    case "Income":
                        var incomeList = col.properties.Replace("|", "','");
                        query += string.Format(" and [family_month_income] in ('{0}')", incomeList);
                        break;
                    case "Marriage":
                        var marriageList = col.properties.Replace("|", ",");
                        query += string.Format(" and [marriage] in ({0})", marriageList);
                        break;
                    case "ChildNo":
                        var childNoList = col.properties.Replace("|", ",");
                        query += string.Format(" and [family_size] in ({0})", childNoList);
                        break;
                    case "IsKdWorker":
                        var workerList = col.properties.Replace("|", ",");
                        query += string.Format(" and [is_kd_worker] in ({0})", workerList);
                        break;
                    case "IsKdOwner":
                        var isOwnerList = col.properties.Replace("|", ",");
                        query += string.Format(" and [is_kd_owner] in ({0})", isOwnerList);
                        break;
                    case "Sex":
                        var sexList = col.properties.Replace("|", ",");
                        query += string.Format(" and sex in ({0})", sexList);
                        break;
                    case "Area":
                        var areaList = col.properties.Replace("省", "").Replace("市", "").Replace("区", "").Replace("县", "").Replace("旗", "").Split('|');
                        if (areaList.Length > 0)
                        {
                            query += " and (1=2 ";
                            foreach (var area in areaList)
                            {
                                var adrList = area.Split('-');
                                if (adrList.Length == 3)
                                {
                                    var province = adrList[0];
                                    var city = adrList[1];
                                    if (province.Contains("上海") || province.Contains("北京") || province.Contains("天津") || province.Contains("重庆"))
                                    {
                                        city = adrList[2];
                                    }
                                    query += " or ( ";
                                    if (province != "全部")
                                        query += string.Format(" [province] ='{0}' ", province);
                                    if (city != "全部")
                                        query += string.Format(" and [city]='{0}' ", city);
                                    query += ")";
                                }
                            }
                            query += ")";
                        }
                        break;
                    case "IsMember":
                        if (col.properties.Contains("0") || col.properties.Contains("2"))
                            break;
                        else if (col.properties.Contains("1"))
                            query += " and [unionid] is not null and [unionid]!='' ";
                        break;
                    case "Group":
                        var groupList = col.properties.Replace("|", ",");
                        query += string.Format(" and [group_id] in ({0})", groupList);
                        break;
                    case "Tag":
                        var tagList = col.properties.Split('|');
                        List<string> IDs = new List<string>();
                        foreach (var tagID in tagList)
                        {
                            int[] fansIDs = wx_fans_tags.GetFansIDListByGroupID(wx_id, Utils.StrToInt(tagID, 0));
                            foreach (var x in fansIDs)
                            {
                                IDs.Add(x.ToString());
                            }
                        }
                        var IDsArray = IDs.Distinct().ToArray();
                        string str = Utils.GetArrayStr(IDsArray, ",");
                        query += string.Format(" and id in ({0})", str);
                        break;
                    case "From":
                        var fromList = col.properties.Replace("|", ",");
                        query += string.Format(" and [source_id] in ({0})", fromList);
                        break;
                    case "WeChatCountry":
                        if (col.properties.Contains("0"))
                        {
                            query += string.Format("and wx_country not in ('中国','中国香港','中国台湾','中国澳门','新加坡')");
                        }
                        else
                        {
                            var countryList = col.properties.Replace("|", "','");
                            query += string.Format(" and [wx_country] in ('{0}')", countryList);
                        }
                        break;
                    case "Country":
                        if (col.properties.Contains("0"))
                        {
                            query += string.Format("and country not in ('中国','中国香港','中国台湾','中国澳门','新加坡')");
                        }
                        else
                        {
                            var countryList = col.properties.Replace("|", "','");
                            query += string.Format(" and [country] in ('{0}')", countryList);
                        }
                        break;
                    case "Hobby":
                        var hobbyList = col.properties.Split('|');
                        if (hobbyList.Length > 0)
                        {
                            query += " and (1=2 ";
                            foreach (var hobby in hobbyList)
                            {
                                query += string.Format(" or hobby in ('{0}') ", "," + hobby + ",");
                            }
                            query += ")";
                        }
                        break;
                }

            }

            CacheHelper.Insert("adwanced_query_" + u_id, query);
            CacheHelper.Insert("filter_json_" + u_id, filterList);
            return new Tuple<string, List<Fans_Filter>>(query, filterList);
        }
        public static Tuple<string, List<Fans_Filter>> GetAdvanceQuery(List<Fans_Filter> filterList, int wx_id, int u_id)
        {
            var query = "select group_id,id,headimgurl,nick_name,unionid,last_interact_time,reply_state,open_id from t_wx_fans where wx_id=" + wx_id + " ";
            List<Fans_Filter_Collection> collectionList = new List<Fans_Filter_Collection>();
            foreach (var filter in filterList)//将同类型条件合并
            {
                var item = collectionList.Where(x => x.category == filter.sh).FirstOrDefault();
                if (item == null)
                {
                    collectionList.Add(new Fans_Filter_Collection() { category = filter.sh, properties = (filter.values + "|") });
                }
                else
                    item.properties += (filter.values + "|");
            }

            foreach (var col in collectionList)//遍历所有条件，拼接查询语句
            {
                col.properties = col.properties.TrimEnd('|');
                switch (col.category)
                {
                    case "WeChatSex":
                        query += string.Format(" and [wx_sex] in ({0}) ", col.properties.Replace('|', ','));
                        break;
                    case "MsgContain":
                        var containList = col.properties.Split('|');
                        foreach (var x in containList)
                        {
                            switch (x)
                            {
                                case "0":
                                    query += " and [tel] is not null and [tel]!='' ";
                                    break;
                                case "1":
                                    query += " and [real_name] is not null and [real_name]!='' ";
                                    break;
                                case "2":
                                    query += " and [id_card] is not null and [id_card]!='' ";
                                    break;
                            }
                        }
                        break;
                    case "SuscribeTime":
                        var susTime = col.properties.Split('—');
                        if (susTime.Length == 2)
                        {
                            query += string.Format(" and [subscribe_time] between '{0}' and '{1}' ", susTime[0].Replace(" ", ""), susTime[1].Replace(" ", ""));
                        }
                        break;
                    case "WeChatArea":
                        var wechatAreaList = col.properties.Replace("省", "").Replace("市", "").Replace("区", "").Replace("县", "").Replace("旗", "").Split('|');
                        if (wechatAreaList.Length > 0)
                        {
                            query += " and (1=2 ";
                            foreach (var area in wechatAreaList)
                            {
                                var adrList = area.Split('-');
                                if (adrList.Length == 3)
                                {
                                    var province = adrList[0];
                                    var city = adrList[1];
                                    if (province.Contains("上海") || province.Contains("北京") || province.Contains("天津") || province.Contains("重庆"))
                                    {
                                        city = adrList[2];
                                    }
                                    query += " or ( ";
                                    if (province != "全部")
                                        query += string.Format(" [wx_province] ='{0}' ", province);
                                    if (city != "全部")
                                        query += string.Format(" and [wx_city]='{0}' ", city);
                                    query += ")";
                                }
                            }
                            query += ")";
                        }
                        break;
                    case "Nation":
                        var nationList = col.properties.Replace("|", "','");
                        query += string.Format(" and [ethnic] in ('{0}')", nationList);
                        break;
                    case "CardType":
                        var cardTpList = col.properties.Replace("|", "','");
                        query += string.Format(" and [id_card_type] in ('{0}') ", cardTpList);
                        break;
                    case "Birth":
                        var birth = col.properties.Replace(" ", "").Split('—');
                        if (birth.Length == 2)
                        {
                            query += string.Format(" and CONVERT(varchar(5),birth,110) between '{0}' and '{1}' ", birth[0], birth[1]);
                        }
                        break;
                    case "BirthDay":
                        var birthday = col.properties.Replace(" ", "").Split('—');
                        if (birthday.Length == 2)
                        {
                            query += string.Format(" and [birth] between '{0}' and '{1}' ", birthday[0], birthday[1]);
                        }
                        break;
                    case "Income":
                        var incomeList = col.properties.Replace("|", "','");
                        query += string.Format(" and [family_month_income] in ('{0}')", incomeList);
                        break;
                    case "Marriage":
                        var marriageList = col.properties.Replace("|", ",");
                        query += string.Format(" and [marriage] in ({0})", marriageList);
                        break;
                    case "ChildNo":
                        var childNoList = col.properties.Replace("|", ",");
                        query += string.Format(" and [family_size] in ({0})", childNoList);
                        break;
                    case "IsKdWorker":
                        var workerList = col.properties.Replace("|", ",");
                        query += string.Format(" and [is_kd_worker] in ({0})", workerList);
                        break;
                    case "IsKdOwner":
                        var isOwnerList = col.properties.Replace("|", ",");
                        query += string.Format(" and [is_kd_owner] in ({0})", isOwnerList);
                        break;
                    case "Sex":
                        var sexList = col.properties.Replace("|", ",");
                        query += string.Format(" and sex in ({0})", sexList);
                        break;
                    case "Area":
                        var areaList = col.properties.Replace("省", "").Replace("市", "").Replace("区", "").Replace("县", "").Replace("旗", "").Split('|');
                        if (areaList.Length > 0)
                        {
                            query += " and (1=2 ";
                            foreach (var area in areaList)
                            {
                                var adrList = area.Split('-');
                                if (adrList.Length == 3)
                                {
                                    var province = adrList[0];
                                    var city = adrList[1];
                                    if (province.Contains("上海") || province.Contains("北京") || province.Contains("天津") || province.Contains("重庆"))
                                    {
                                        city = adrList[2];
                                    }
                                    query += " or ( ";
                                    if (province != "全部")
                                        query += string.Format(" [province] ='{0}' ", province);
                                    if (city != "全部")
                                        query += string.Format(" and [city]='{0}' ", city);
                                    query += ")";
                                }
                            }
                            query += ")";
                        }
                        break;
                    case "IsMember":
                        if (col.properties.Contains("0") || col.properties.Contains("2"))
                            break;
                        else if (col.properties.Contains("1"))
                            query += " and [unionid] is not null and [unionid]!='' ";
                        break;
                    case "Group":
                        var groupList = col.properties.Replace("|", ",");
                        query += string.Format(" and [group_id] in ({0})", groupList);
                        break;
                    case "Tag":
                        var tagList = col.properties.Split('|');
                        List<string> IDs = new List<string>();
                        foreach (var tagID in tagList)
                        {
                            int[] fansIDs = wx_fans_tags.GetFansIDListByGroupID(wx_id, Utils.StrToInt(tagID, 0));
                            foreach (var x in fansIDs)
                            {
                                IDs.Add(x.ToString());
                            }
                        }
                        var IDsArray = IDs.Distinct().ToArray();
                        string str = Utils.GetArrayStr(IDsArray, ",");
                        query += string.Format(" and id in ({0})", str);
                        break;
                    case "From":
                        var fromList = col.properties.Replace("|", ",");
                        query += string.Format(" and [source_id] in ({0})", fromList);
                        break;
                    case "WeChatCountry":
                        if (col.properties.Contains("0"))
                        {
                            query += string.Format("and wx_country not in ('中国','中国香港','中国台湾','中国澳门','新加坡')");
                        }
                        else
                        {
                            var countryList = col.properties.Replace("|", "','");
                            query += string.Format(" and [wx_country] in ('{0}')", countryList);
                        }
                        break;
                    case "Country":
                        if (col.properties.Contains("0"))
                        {
                            query += string.Format("and country not in ('中国','中国香港','中国台湾','中国澳门','新加坡')");
                        }
                        else
                        {
                            var countryList = col.properties.Replace("|", "','");
                            query += string.Format(" and [country] in ('{0}')", countryList);
                        }
                        break;
                    case "Hobby":
                        var hobbyList = col.properties.Split('|');
                        if (hobbyList.Length > 0)
                        {
                            query += " and (1=2 ";
                            foreach (var hobby in hobbyList)
                            {
                                query += string.Format(" or hobby in ('{0}') ", "," + hobby + ",");
                            }
                            query += ")";
                        }
                        break;
                }

            }

            CacheHelper.Insert("adwanced_query_" + u_id, query);
            CacheHelper.Insert("filter_json_" + u_id, filterList);
            return new Tuple<string, List<Fans_Filter>>(query, filterList);
        }

        /// <summary>
        /// 获取所有公众号数量(2015-1-15/yzl)
        /// </summary>
        /// <returns></returns>
        public static int GetWXCount()
        {
            int res = 0;
            string sql = "select count(id) as res_count from  t_wx_wechats where status=" + (int)Status.正常 + "";
            res = Convert.ToInt32(KDWechat.DBUtility.DbHelperSQL.GetSingle(sql));
            return res;
        }

        /// <summary>
        /// 获取绑定的公众号的数量(2015-1-15/yzl)
        /// </summary>
        /// <returns></returns>
        public static int GetBindWXCount()
        {
            int res = 0;
            string sql = "select COUNT(wx_id) as res_count from( select wx_id,MAX(last_interact_time) as last_interact_time from t_wx_fans where datediff(DAY,last_interact_time,GETDATE())<3  group by wx_id) t";
            res = Convert.ToInt32(KDWechat.DBUtility.DbHelperSQL.GetSingle(sql));
            return res;
        }

        /// <summary>
        /// 获取性别列表
        /// property别，微信ID)
        /// </summary>
        /// <returns></returns>
        public static List<Entity.All_Property_Statistics<int>> GetAllSexStatistic()
        {
            List<Entity.All_Property_Statistics<int>> list = null;
            using (var db = new creater_wxEntities())
            {
                list = db.t_wx_fans.GroupBy(x => new { x.sex, x.wx_id }).Select(x => new Entity.All_Property_Statistics<int>() { count = x.Count(), property = (x.Key.sex ?? 0), wx_id = x.Key.wx_id }).ToList();
            }
            return list;
        }

        /// <summary>
        /// 获取城市列表
        /// tuple(数量，城市名，微信ID)
        /// </summary>
        /// <returns></returns>
        public static List<Entity.All_Property_Statistics<string>> GetAllCityStatistic()
        {
            List<Entity.All_Property_Statistics<string>> list = null;
            using (var db = new creater_wxEntities())
            {
                list = db.t_wx_fans.GroupBy(x => new { x.city, x.wx_id }).Select(x => new Entity.All_Property_Statistics<string>() { count = x.Count(), property = x.Key.city, wx_id = x.Key.wx_id }).ToList();
            }
            return list;
        }

        /// <summary>
        /// 获取公众号是否绑定
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="wx_og_id"></param>
        /// <returns></returns>
        public static int GetMemberFansRelationCount(string openId, string wx_og_id)
        {
            int res = 0;
            string sql = "select count(1) as count_exists from  t_member_fans_relation where wx_og_id='" + wx_og_id + "' and openid='" + openId + "'";
            res = Convert.ToInt32(KDWechat.DBUtility.DbHelperSQL.GetSingle(sql));
            return res;
        }


        /// <summary>
        /// 设置注册过期
        /// </summary>
        public static void SetRegFansState()
        {
            //过期时间
            var expireTime = DateTime.Now.AddMinutes(-5);
            //已过期的最后聊天的ID数组
            creater_wxEntities user_db = new creater_wxEntities();
            //聊天，选择项目中的粉丝的opid,wxid列表
            var fanList = user_db.t_wx_fans.Where(x => x.state == (int)FansState.手机注册发送验证码状态 && x.last_interact_time < expireTime).Select(x => new { opid = x.open_id, wx_id = x.wx_id }).ToList();
            //已过期且处于注册状态的，选择项目的粉丝的opid数组
            var opidArray = fanList.Select(x => x.opid).ToArray();
            if (opidArray.Length > 0)
            {
                //更新所有状态
                user_db.t_wx_fans.Where(x => opidArray.Contains(x.open_id)).Update(x => new t_wx_fans() { state = 2 });
            }

            //根据accesstoken的临时容器
            Dictionary<int, string> accesstokenDic = new Dictionary<int, string>();
            //通过wx_id填充accesstoken容器
            var wx_idArray = fanList.Select(x => x.wx_id).Distinct().ToArray();
            foreach (var wxid in wx_idArray)
            {
                accesstokenDic.Add(wxid, BLL.Chats.wx_wechats.GetAccessToken(wxid));
            }

            foreach (var opid in opidArray)
            {
                var wxid = fanList.Where(x => x.opid == opid).First().wx_id;
                var token = accesstokenDic[wxid];
                try
                {
                    Senparc.Weixin.MP.AdvancedAPIs.Custom.SendText(token, opid, "由于您5分钟内没有操作，已自动退出注册状态。");
                }
                catch
                {
                    continue;
                }
            }

            user_db.Dispose();
        }

        public static void SetUploadTickerFansState()
        {
            //过期时间
            var expireTime = DateTime.Now.AddMinutes(-5);
            //已过期的最后聊天的ID数组
            creater_wxEntities user_db = new creater_wxEntities();
            //聊天，选择项目中的粉丝的opid,wxid列表
            var fanList = user_db.t_wx_fans.Where(x => x.state == (int)FansState.上传小票状态 && x.last_interact_time < expireTime).Select(x => new { opid = x.open_id, wx_id = x.wx_id }).ToList();
            //已过期且处于注册状态的，选择项目的粉丝的opid数组
            var opidArray = fanList.Select(x => x.opid).ToArray();
            if (opidArray.Length > 0)
            {
                //更新所有状态
                user_db.t_wx_fans.Where(x => opidArray.Contains(x.open_id)).Update(x => new t_wx_fans() { state = 2 });
            }

            user_db.Dispose();
        }



        /// <summary>
        /// 根据openid获取会员信息
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static t_member GetMermberInfo(string openId, ref string msg)
        {
            msg = "1";
            t_member member = null;

            //openid的绑定关系
            DataTable dt_member_fans_relation = KDWechat.DBUtility.DbHelperSQL.Query(string.Format("SELECT *  from	t_member_fans_relation WHERE openid='{0}'", openId)).Tables[0];
            if (dt_member_fans_relation != null && dt_member_fans_relation.Rows.Count > 0)
            {
                //检测会员是否有效
                var member_url = System.Configuration.ConfigurationManager.AppSettings["member_url"];
                var result = Common.KDHttpRequest.GetJsonByPost<JsonMemberDataResult<t_member>>(member_url + "?fp=1000&Mt=get_member_primary_info&cc=1&mid=" + DESEncrypt.Encrypt(dt_member_fans_relation.Rows[0]["member_id"].ToString(), "KDMember"));
                if (result != null)
                {
                    if (result.result == -1 || result.data == null)
                    {
                        //会员有异常或需要重新登录

                        KDWechat.DBUtility.DbHelperSQL.ExecuteSql(string.Format("delete t_oauth_fans_relation where member_id={0}     and wx_fans_id=0 ", dt_member_fans_relation.Rows[0]["member_id"].ToString()));  //删掉授权关系
                        return member;
                    }
                }

                //查询openid与授权账户的关系
                DataTable dt_openid_oauth_relation = KDWechat.DBUtility.DbHelperSQL.Query(string.Format("SELECT * from	t_oauth_fans_relation WHERE wx_openId='{0}'  AND member_id={1} ", openId, dt_member_fans_relation.Rows[0]["member_id"])).Tables[0];
                if (dt_openid_oauth_relation != null && dt_openid_oauth_relation.Rows.Count > 0)
                {
                    //查询是否绑定登录
                    var dt_login_oauth = KDWechat.DBUtility.DbHelperSQL.Query(string.Format("SELECT * from t_oauth_fans_relation  WHERE   wx_fans_id=0 AND wx_openId='' AND member_id={0} and union_openId='{1}' ", dt_member_fans_relation.Rows[0]["member_id"], dt_openid_oauth_relation.Rows[0]["union_openId"])).Tables[0];
                    if (dt_login_oauth != null && dt_login_oauth.Rows.Count > 0)
                    {

                        #region 查询会员
                        DataTable dt = KDWechat.DBUtility.DbHelperSQL.Query(string.Format(" select * from	kd_member.dbo.t_member WHERE  id={0} AND status=1", dt_member_fans_relation.Rows[0]["member_id"])).Tables[0];
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            msg = "0";
                            member = new t_member();
                            member.id = Utils.StrToInt(dt.Rows[0]["id"].ToString(), 0);
                            member.phone = dt.Rows[0]["phone"].ToString();
                            member.capital_star_id = Utils.StrToInt(dt.Rows[0]["capital_star_id"].ToString(), 0);
                            member.nick_name = dt.Rows[0]["nick_name"].ToString();
                            member.e_mail = dt.Rows[0]["e_mail"].ToString();
                            member.m_from = Utils.StrToInt(dt.Rows[0]["m_from"].ToString(), 0);
                            member.capital_member_card = dt.Rows[0]["capital_member_card"].ToString();
                            member.profile_token = dt.Rows[0]["profile_token"].ToString();
                            member.username = dt.Rows[0]["username"].ToString();
                        }
                        #endregion
                    }
                    else
                    {
                        msg = "3";
                    }
                }
                else
                {
                    msg = "2";
                }
            }



            return member;
        }

        /// <summary>
        /// 添加小票记录
        /// </summary>
        /// <param name="member_id"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static int AddScanLog(int member_id, string msg)
        {
            int num = 0;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("INSERT INTO [kd_mall].[dbo].[t_scan_no] ([member_id] ,[ScanNo] ,[add_time])");
            sb.AppendLine(" VALUES  (@member_id,@ScanNo,@add_time)");

            SqlParameter[] parmeter = new SqlParameter[]{
                new SqlParameter("@member_id",SqlDbType.Int,4),
                new SqlParameter("@ScanNo",SqlDbType.NVarChar,200),
                new SqlParameter("@add_time",SqlDbType.DateTime)
            };
            parmeter[0].Value = member_id;
            parmeter[1].Value = msg;
            parmeter[2].Value = DateTime.Now;

            num = KDWechat.DBUtility.DbHelperSQL.ExecuteSql(sb.ToString(), parmeter);

            return num;
        }
    }



    public class t_wx_fans_export
    {
        //dic.Add("nick_name","昵称");
        //dic.Add("country", "国家");
        //dic.Add("province", "省份");
        //dic.Add("city", "城市");
        //dic.Add("language", "语言");
        //dic.Add("sex", "性别 0-未知，1-男，2-女");

        public string openid { get; set; }
        public string nick_name { get; set; }
        public string country { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string language { get; set; }
        public int sex { get; set; }
    }
}
