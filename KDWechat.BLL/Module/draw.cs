using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KDWechat.DAL;
using EntityFramework.Extensions;
using KDWechat.Common;

namespace KDWechat.BLL.Module
{
    public class draw
    {
        public static int getWxid(int tp)
        {
            switch (tp)
            {
                case 1:
                    return 35;
                case 2:
                    return 36;
                case 3:
                    return 37;
                case 4:
                    return 38;
            }
            return 35;
        }

        public static int getTp(int wxID)
        {
            switch (wxID)
            {
                case 35:
                    return 1;
                case 36:
                    return 2;
                case 37:
                    return 3;
                case 38:
                    return 4;
            }
            return 1;
        }

        public static bool SetUsed(int id)
        {
            var isOk = false;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                isOk = db.t_draw_winner.Where(x => x.id == id).Update(x => new t_draw_winner() { used = 1 })==1;
            }
            return isOk;
        }

        public static bool SetWinner(int act_id,List<t_draw_order> odList)
        {
            bool isOk = false;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                var awardList = db.t_draw_award.Where(x => x.act_id == act_id).ToList();
                foreach (var x in odList)
                {
                    var award = awardList.Where(y => y.count > 0).FirstOrDefault();
                    if (award != null)
                    {
                        db.t_draw_winner.Add(new t_draw_winner() { award_id = award.id, cellphone = x.telephone, create_time = DateTime.Now, name = x.name, order_id = x.id, used = 0, wx_id = x.wx_id, wx_og_id = x.wx_og_id });
                        award.count--;
                    }
                }
                isOk = db.SaveChanges() > awardList.Count;
                if (isOk)
                {
                    db.t_draw_activity.Where(x => x.id == act_id).Update(x => new t_draw_activity() { drawed = 1 });
                }
            }
            return isOk;
        }

        /// <summary>
        /// 抽奖
        /// </summary>
        /// <param name="id">需要抽奖的活动</param>
        /// <param name="wxID">需要抽奖的微信ID</param>
        /// <returns>1.是否成功，2.提示信息，3.成功后返回的中奖列表</returns>
        public static Tuple<bool,string,List<t_draw_order>> Draw(int id,int wxID)
        {
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                #region old code 忘了是多个微信号了，所以我决定采用不公平算法 (好吧，突然觉得这个改改还能用，懒得改+1
                //var MaxOrder = db.t_draw_order.OrderByDescending(x => x.id).FirstOrDefault();//获取最大订单ID
                var AllOrder = db.t_draw_order.Where(x => x.wx_id == wxID&& x.total_count>=810).ToList();//直接根据ID把对应微信号的订单全取出来

                
                if (AllOrder.Count==0)
                    return new Tuple<bool, string, List<t_draw_order>>(false, "没有提交了订单的用户", null);
                    //out------------

                var awardCountArray = db.t_draw_award.Where(x => x.act_id == id).Select(x => x.count).ToList();//获取对应奖项的数量
                int awardCount = 0;//奖项总数
                awardCountArray.ForEach(x => awardCount += x);//设定奖项总数
                if(awardCount==0)
                    return new Tuple<bool, string, List<t_draw_order>>(false, "奖品数量为0", null);
                    //out-----------
                var maxid = AllOrder.Count;
                var nagPhones = db.t_draw_winner.Where(x => x.wx_id == wxID).Select(x => x.cellphone).ToArray();//获取已中奖的用户手机
                var truthUserNo = AllOrder.GroupBy(x => x.telephone).ToList().Count-nagPhones.Length;
                if (awardCount > maxid)
                    return new Tuple<bool, string, List<t_draw_order>>(false, "订单数少于奖项数", null);
                    //out-----------
                else if (awardCount > truthUserNo)
                    return new Tuple<bool, string, List<t_draw_order>>(false, "实际参加人数少于奖项数", null);
                    //out-----------
                List<t_draw_order> orderList = new List<t_draw_order>();
                do
                {
                    var list = Utils.GetRandomNumbers(maxid+1, awardCount);
                    if (orderList.Count == 0)//第一次取
                        list.ForEach(x => orderList.Add(AllOrder[x-1]));
                    //orderList = AllOrder.Where(x => list.Contains(x.id)).ToList();//调取随机ID集合中的订单
                    else//去重之后
                    {
                        //var ids = orderList.Select(x => x.id).ToArray();//获取已有列表ID
                        //do
                        //{
                        list = Utils.GetRandomNumbers(maxid+1, awardCount - orderList.Count);//获取几个随机数
                        //foreach (var x in list)//循环新的随机数
                        //{
                        //if (ids.Contains(x))//如果已有列表ID包含新的随机数
                        //{
                        //list = Utils.GetRandomNumbers(maxid, awardCount - orderList.Count);//重新生成随机数
                        //break;
                        //}
                        //}
                        //} while (list.Count < awardCount - orderList.Count);
                        var ids = orderList.Select(x => x.id).ToArray();//获取已有列表ID
                        //var appendList = 
                        list.ForEach(x => orderList.Add(AllOrder[x-1]));//AllOrder.Where(x => list.Contains(x.id)).ToList();//获取新的随机数对应订单
                        //appendList.ForEach(x => { if (!ids.Contains(x.id))orderList.Add(x); });//将新订单添加到订单列表
                    }
                    orderList = orderList.Distinct(new Compare<t_draw_order>((x, y) => (x.telephone == y.telephone))).ToList();//根据手机号进行订单去重
                    orderList = orderList.Where(x => !nagPhones.Contains(x.telephone)).ToList();//订单中去除已获奖用户
                } while (orderList.Count <= awardCount - 1);//反正已经在内存里了，上面的方法决定删了得了，大不了多做几次 懒得重写+2
                return new Tuple<bool, string, List<t_draw_order>>(true, "抽奖成功！", orderList);
                #endregion
            }
        }
    }
}
