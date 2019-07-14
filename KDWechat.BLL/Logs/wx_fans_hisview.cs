using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KDWechat.DAL;
using EntityFramework.Extensions;

namespace KDWechat.BLL.Logs
{
    public class wx_fans_hisview
    {
        public static t_wx_fans_hisview GetFansHisviewsByID(int id)
        {
            t_wx_fans_hisview tag = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                tag = (from x in db.t_wx_fans_hisview where x.id == id select x).FirstOrDefault();
            }
            return tag;
        }


        public static t_wx_fans_hisview CreateFansHisview(t_wx_fans_hisview tag)
        {
            return EFHelper.AddLog<t_wx_fans_hisview>(tag);
        }

        public static bool DeleteFansHisviewByID(int id)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isFinish = db.t_wx_fans_hisview.Where(x => x.id == id).Delete() > 0;
            }
            return isFinish;
        }

        public static bool UpdateFansHisview(t_wx_fans_hisview tag)
        {
            return EFHelper.UpdateLog<t_wx_fans_hisview>(tag);
        }

        /// <summary>
        /// 获取统计
        /// </summary>
        /// <param name="id">新闻id</param>
        /// <param name="channel_id">1、微信 2微网站</param>
        /// <param name="type_id">1、浏览数 2、点赞数 3、分享数</param>
        /// <returns></returns>
        public static int GetCount(int id, int channel_id, int type_id)
        {
            int  count = 0;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                count = (from x in db.t_wx_fans_hisview where x.news_id == id && x.channel_id==channel_id && x.type_id==type_id select x.id).Count();
            }
            return count;
        }

        public static t_wx_fans_hisview GetModel(int wx_id, string open_id, int channel_id, int type_id, int news_id)
        {
            t_wx_fans_hisview model = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                model = (from x in db.t_wx_fans_hisview where x.wx_id == wx_id && x.open_id==open_id && x.channel_id==channel_id && x.type_id==type_id && x.news_id==news_id select x).FirstOrDefault();
            }
            return model;
        }

        public static int GetCount(int id, int channel_id, int type_id, string openId)
        {
            int count = 0;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                count = (from x in db.t_wx_fans_hisview where x.news_id == id && x.channel_id == channel_id && x.type_id == type_id && x.open_id == openId select x.id).Count();
            }
            return count;
        }
    }
}

