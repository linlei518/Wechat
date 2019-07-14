using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KDWechat.DAL;
using EntityFramework.Extensions;
using System.Data.SqlClient;
using System.Data;
using KDWechat.Common;
using Companycn.Core.DbHelper;
namespace KDWechat.BLL.Module
{
    public class pazzrecord
    {
        /// <summary>
        /// 查询用户当天的抽奖次数
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static int GetUserCountByDay(string openId, int lotteryId)
        {
            int totalNum = 0;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                DateTime dtstar=DateTime.Now.Date;
                DateTime dtend=DateTime.Now.Date.AddDays(1);

                totalNum = db.t_PazzRecord.Where(x => x.openid == openId && x.lottery_id == lotteryId && x.add_time < dtend && x.add_time > dtstar).Count();
            }
            return totalNum;
        }

        /// <summary>
        /// 查询用户的总抽奖次数
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static int GetUserCountBylotteryId(string openId, int lotteryId)
        {
            int totalNum = 0;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                totalNum = db.t_PazzRecord.Where(x => x.openid == openId && x.lottery_id == lotteryId).Count();
            }
            return totalNum;
        }
        /// <summary>
        /// 根据openId和活动编号查询用户中奖次数
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="lotteryId"></param>
        /// <returns></returns>
        public static int GetPazzCount(string openId,int lotteryId)
        {
            int totalNum = 0;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                DateTime dtstar = DateTime.Now.Date;
                DateTime dtend = DateTime.Now.Date.AddDays(1);

                totalNum = db.t_PazzRecord.Where(x => x.openid == openId && x.lottery_id==lotteryId && x.add_time < dtend && x.add_time > dtstar && x.is_award !=0).Count();
            }
            return totalNum;
        }

        /// <summary>
        /// 添加抽奖记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_PazzRecord AddPazzrecord(t_PazzRecord model)
        {
            return EFHelper.AddModule<t_PazzRecord>(model);
        }

        /// <summary>
        /// 中奖后修改中奖记录
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_PazzRecord Update(t_PazzRecord model)
        {
            return EFHelper.UpdateModule<t_PazzRecord>(model);
        }

        /// <summary>
        /// 获取中奖记录信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static t_PazzRecord GetModelById(int id)
        {
            t_PazzRecord model = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                model = db.t_PazzRecord.Where(x => x.id == id).FirstOrDefault();
            }
            return model;
        }

        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_PazzRecord> GetList(string openId,int lottery_id)
        {
            List<t_PazzRecord> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                var query = (from x in db.t_PazzRecord where x.openid == openId &&x.lottery_id==lottery_id select x);
                list = query.ToList();

            }
            return list;
        }


        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static DataTable  GetList(string titles,string strWhere)
        {
            StringBuilder sb = new StringBuilder();
            
            sb.Append("  select openid,case channel_id when '1' then'刮刮卡' when '2' then '大转盘' when '3' then '砸金蛋' else '未知'end as type_name, ");
            if (!string.IsNullOrEmpty(titles))
            {
                sb.Append("  lotery_name='"+titles+"', prize_t_name, prize_name, p_code,  user_name,  user_tel, add_time, ");
            }
            sb.Append("  case Is_prized when'1' then'已领奖' else'未领奖' end as Is_prized, ");
            sb.Append("  remark");
            sb.Append("  from t_pazzrecord ");
            if (!string.IsNullOrEmpty(strWhere))
            {
                sb.Append(strWhere);
            }


            return DbHelperSQLModule.Query(sb.ToString()).Tables[0];
        }
    }
}
