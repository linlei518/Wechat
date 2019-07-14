using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using LinqKit;
using KDWechat.DAL;
using EntityFramework.Extensions;
using KDWechat.Common;
using System.Data;
using Companycn.Core.DbHelper;


namespace KDWechat.BLL.Module
{
    public class md_sale_chats
    {
        /// <summary>
        /// 添加一条
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_md_sale_chats AddModel(t_md_sale_chats model)
        {
            return EFHelper.AddModule<t_md_sale_chats>(model);
        }

        /// <summary>
        /// 返回统计结果
        /// </summary>
        /// <param name="timeLine"></param>
        /// <returns>索引0-销售ID，1-接待数量，2-对应的日期列表索引</returns>
        public static List<int[]> GetChatStatisticByWx_id(List<DateTime> timeLine, int wxID)
        {
            List<int[]> result = new List<int[]>();
            if (timeLine.Count > 0)
            {
                for (int i = 0; i < timeLine.Count; i++)
                {
                    var start = timeLine[i];
                    DateTime end = i + 1 == timeLine.Count ? timeLine[i].AddDays(1) : timeLine[i + 1];
                    var query = "select seller_id,count(distinct user_openid) as coun from t_md_sale_chats where creat_time>'" + start.ToString() + "' and creat_time<'" + end.ToString() + "' and wx_id=" + wxID + " group by seller_id";
                    var tb = DbHelperSQLModule.Query(query).Tables[0];
                    if (tb != null && tb.Rows.Count > 0)
                    {
                        foreach (DataRow rw in tb.Rows)
                        {
                            int seller_id = Utils.StrToInt(rw["seller_id"].ToString(), 0);
                            int count = Utils.StrToInt(rw["coun"].ToString(), 0);
                            result.Add(new int[3] { seller_id, count, i });
                        }
                    }
                }
            }
            return result;

        }        
        
        /// <summary>
        /// 返回统计结果
        /// </summary>
        /// <param name="timeLine"></param>
        /// <returns>索引0-销售ID，1-接待数量，2-对应的日期列表索引</returns>
        public static List<int[]> GetChatStatisticByWx_id(List<DateTime> timeLine, int wxID,int[] uids)
        {
            List<int[]> result = new List<int[]>();
            if (timeLine.Count > 0)
            {
                for (int i = 0; i < timeLine.Count; i++)
                {
                    var start = timeLine[i];
                    DateTime end = i + 1 == timeLine.Count ? timeLine[i].AddDays(1) : timeLine[i + 1];
                    var query = "select seller_id,count(distinct user_openid) as coun from t_md_sale_chats where creat_time>'" + start.ToString() + "' and creat_time<'" + end.ToString() + "' and wx_id=" + wxID + " group by seller_id";
                    var tb = DbHelperSQLModule.Query(query).Tables[0];
                    if (tb != null && tb.Rows.Count > 0)
                    {
                        foreach (DataRow rw in tb.Rows)
                        {
                            int seller_id = Utils.StrToInt(rw["seller_id"].ToString(), 0);
                            int count = Utils.StrToInt(rw["coun"].ToString(), 0);
                            if(uids.Contains(seller_id))
                                result.Add(new int[3] { seller_id, count, i });
                        }
                    }
                }
            }
            return result;

        }




        /// <summary>
        /// 设置过期
        /// </summary>
        public static void SetEntireFansState()
        {
            //过期时间
            var expireTime = DateTime.Now.AddMinutes(-2);
            //已过期的最后聊天的ID数组
            kd_usersEntities user_db = new kd_usersEntities();
            //聊天，选择项目中的粉丝的opid,wxid列表
            var fanList =  user_db.t_wx_fans.Where(x=>(x.state==(int)FansState.客服聊天状态||x.state==(int)FansState.选择项目状态)&& x.last_interact_time < expireTime).Select(x=>new {opid = x.open_id,wx_id=x.wx_id}).ToList();
            //已过期且处于聊天，选择项目的粉丝的opid数组
            var opidArray = fanList.Select(x => x.opid).ToArray();
            if (opidArray.Length > 0)
            {
                //更新所有状态
                user_db.t_wx_fans.Where(x => opidArray.Contains(x.open_id)).Update(x => new t_wx_fans() { state = 2 });
                kd_moduleEntities db = new kd_moduleEntities();
                db.t_md_sale_users_fans_relation.Where(x => opidArray.Contains(x.fans_open_id)).Update(x => new t_md_sale_users_fans_relation { status = 0 });
                db.Dispose();
            }
            //根据accesstoken的临时容器
            Dictionary<int, string> accesstokenDic = new Dictionary<int, string>();
            //通过wx_id填充accesstoken容器
            var wx_idArray = fanList.Select(x => x.wx_id).Distinct().ToArray();
            foreach (var wxid in wx_idArray)
            {
                accesstokenDic.Add(wxid, BLL.Chats.wx_wechats.GetAccessToken(wxid));
            }

            foreach(var opid in opidArray)
            {
                var wxid = fanList.Where(x => x.opid == opid).First().wx_id;
                var token = accesstokenDic[wxid];
                try
                {
                    Senparc.Weixin.MP.AdvancedAPIs.Custom.SendText(token, opid, "由于您两分钟内没有互动，已自动退出客服聊天状态。");
                }
                catch
                {
                    continue;
                }
            }
            user_db.Dispose();
        }



        public static t_md_sale_chats GetModel<T>(Expression<Func<t_md_sale_chats, bool>> where, Expression<Func<t_md_sale_chats, T>> orderBy, bool orderByDesc = false)
        {
            t_md_sale_chats model = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                if (!orderByDesc)
                    model = db.t_md_sale_chats.Where(where.Expand()).OrderBy(orderBy.Expand()).FirstOrDefault();
                else
                    model = db.t_md_sale_chats.Where(where.Expand()).OrderByDescending(orderBy.Expand()).FirstOrDefault();
            }
            return model;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="T">orderby类型</typeparam>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalCount"></param>
        /// <param name="orderByDesc"></param>  
        /// <returns></returns>
        public static List<t_md_sale_chats> GetList<T>(Expression<Func<t_md_sale_chats, bool>> where, Expression<Func<t_md_sale_chats, T>> orderBy, int pagesize, int pageindex,out int totalCount, bool orderByDesc = false)
        {
            List<t_md_sale_chats> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                var query = db.t_md_sale_chats.Where(where.Expand());
                totalCount = query.Count();
                if (!orderByDesc)
                    query = query.OrderBy(orderBy.Expand());
                else
                    query = query.OrderByDescending(orderBy.Expand());
                list = query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            }
            return list;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="T">orderby类型</typeparam>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="totalCount"></param>
        /// <param name="orderByDesc"></param>
        /// <returns></returns>
        public static List<t_md_sale_chats_project_view> GetViewList<T>(Expression<Func<t_md_sale_chats_project_view, bool>> where, Expression<Func<t_md_sale_chats_project_view, T>> orderBy, int pagesize, int pageindex, out int totalCount, bool orderByDesc = false)
        {
            List<t_md_sale_chats_project_view> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                var query = db.t_md_sale_chats_project_view.Where(where.Expand());
                totalCount = query.Count();
                if (!orderByDesc)
                    query = query.OrderBy(orderBy.Expand());
                else
                    query = query.OrderByDescending(orderBy.Expand());
                list = query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            }
            return list;
        }


        public static T[] GetGroupBy<T>(Expression<Func<t_md_sale_chats, bool>> where, Expression<Func<t_md_sale_chats, T>> groupBy)
        {
            T[] list;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                list = db.t_md_sale_chats.Where(where).GroupBy(groupBy).Select(x => x.Key).ToArray();
            }
            return list;
        }

       //设为已读
        public static void SetIsRead(int[] ids)
        {
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                var query = db.t_md_sale_chats.Where(x => x.is_read == (int)SaleChatIsReadType.未读 && ids.Contains(x.id));
                if (query.Count() > 0)
                {
                    query.Update(x => new t_md_sale_chats { is_read = (int)SaleChatIsReadType.已读 });
                    // foreach (var x in list)
                    //   x.is_read = (int)SaleChatIsReadType.已读;
                    db.SaveChanges();
                }
            }
        }
    }
}
