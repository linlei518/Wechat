using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KDWechat.DAL;
using System.Linq.Expressions;
using EntityFramework.Extensions;
using LinqKit;

namespace KDWechat.BLL.Module
{
    public class prizeinfo
    {
        /// <summary>
        /// 查询当前活动的所有奖项设置
        /// </summary>
        /// <param name="lotteryId"></param>
        /// <returns></returns>
        public static List<t_prizeinfo> GetList(int lotteryId)
        {
            List<t_prizeinfo> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                list = (from x in db.t_prizeinfo where x.lottery_id == lotteryId select x).OrderBy(y => y.id).ToList();
            }
            return list;
        }

        /// <summary>
        /// 根据id查询奖项信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static KDWechat.DAL.t_prizeinfo GetModel(int id)
        {
            t_prizeinfo model = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                model = db.t_prizeinfo.Where(x => x.id == id).FirstOrDefault();
            }
            return model;
        }
        /// <summary>
        /// 中奖后修改奖品数量
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int UpdateNumById(int id)
        {
            int num = 0;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                var model = db.t_prizeinfo.Where(x => x.id == id).FirstOrDefault();
                if (null != model)
                {
                    model.prize_num = (int)model.prize_num - 1;
                    model.pazz_num = Common.Utils.ObjToInt(model.pazz_num,0) + 1;
                    num = db.SaveChanges();
                }
            }
            return num;
        }
    }
}
