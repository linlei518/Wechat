using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KDWechat.BLL.Chats
{

    public class wechat_binding
    {
        public static List<TimeStatistics<int>> GetFansTimeStatistics()
        {
            List<TimeStatistics<int>> list = null;
            using (KDWechat.DAL.creater_wxEntities db = new DAL.creater_wxEntities())
            {
                list = db.t_wx_fans.GroupBy(x => x.wx_id).Select(x => new TimeStatistics<int>() { last_time = x.Max(y => y.subscribe_time), key = x.Key }).ToList();
            }
            return list;
        }

        public static List<TimeStatistics<int>> GetViewTimeStatistics()
        {
            List<TimeStatistics<int>> list = null;
            using (KDWechat.DAL.creater_wxEntities db = new DAL.creater_wxEntities())
            {
                list = db.t_wx_fans_hisview.GroupBy(x => x.wx_id).Select(x => new TimeStatistics<int>() { last_time = x.Max(y => y.view_time), key = x.Key }).ToList();
            }
            return list;
        }

        public static List<TimeStatistics<int>> GetLocationTimeStatistics()
        {
            List<TimeStatistics<int>> list = null;
            using (KDWechat.DAL.creater_wxEntities db = new DAL.creater_wxEntities())
            {
                list = db.t_wx_fans_hislocation.GroupBy(x => x.wx_id).Select(x => new TimeStatistics<int>() { last_time = x.Max(y => y.create_time), key = x.Key }).ToList();
            }
            return list;
        }
        public static List<TimeStatistics<int>> GetChatsTimeStatistics()
        {
            List<TimeStatistics<int>> list = null;
            using (KDWechat.DAL.creater_wxEntities db = new DAL.creater_wxEntities())
            {
                list = db.t_wx_fans_chats.GroupBy(x => x.wx_id).Select(x => new TimeStatistics<int>() { last_time = x.Max(y => y.create_time), key = x.Key }).ToList();
            }
            return list;
        }
        public static List<CountStatistics<int>> GetWeeklyFansCount(bool suscribe)
        {
            List<CountStatistics<int>> list = null;
            using (DAL.creater_wxEntities db = new DAL.creater_wxEntities())
            {
                //var query = db.t_wx_fans
                var time = DateTime.Now.AddDays(-7);
                if (suscribe)
                    list = db.t_wx_fans.Where(x => x.subscribe_time > time).GroupBy(x => x.wx_id).Select(x => new CountStatistics<int> { count = x.Count(), key = x.Key }).ToList();  
                else
                    list = db.t_wx_fans.Where(x => x.remove_time > time).GroupBy(x => x.wx_id).Select(x => new CountStatistics<int> { count = x.Count(), key = x.Key }).ToList();  
            }
            return list;
        }


    }

    public class Statistics_Dashboard
    {
        public static string GetDashboardCompareString(List<DAL.t_wx_wechats> wxList)
        {
            if (wxList.Count == 1)
                return wxList.First().wx_pb_name + "数据统计";
            else if (wxList.Count >= 2 && wxList.Count <= 3)
            {
                var strToReturn = "";
                wxList.ForEach(x => strToReturn += x.wx_pb_name + "、");
                strToReturn = strToReturn.TrimEnd('、') + "数据对比";
                return strToReturn;
            }
            return "";
        }
    }


    public class TimeStatistics<T>
    {
        public DateTime last_time { get; set; }
        public T key { get; set; }
    }

    public class CountStatistics<T>
    {
        public int count { get; set; }
        public T key { get; set; }

    }

    public class CountStatistics
    {
        public int count { get; set; }
        public string key { get; set; }
    }
    public class PercentCountStatistics
    {
        public int count { get; set; }
        public string key { get; set; }
        public string percent { get; set; }
    }

    public class SeriesStatistics
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<int> data { get; set; }
    }

    public class FansStatistics
    {
        public string country { get; set; }
        public string city { get; set; }
        public string language { get; set; }
        public string province { get; set; }
        public int sex { get; set; }
        public int status { get; set; }
        public DateTime subscribe_time { get; set; }
        public DateTime? remove_time { get; set; }
    }


}
