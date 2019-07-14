using KDWechat.BLL.Chats;
using KDWechat.BLL.Users;
using KDWechat.Common;
using KDWechat.DAL;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.Statistics
{
    public partial class user_property : Web.UI.BasePage
    {
        public string strWxlist = "";//拼接微信号JSON的string
        #region chart property
        protected string jsonData2 = "";
        protected string jsonSex = "";
        protected string jsonLang = "";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                InitData();
        }

        private void InitData()
        {
            List<BLL.Chats.FansStatistics> fansList = wx_fans.GetFansListByWxID(wx_id);
            Expression<Func<BLL.Chats.FansStatistics, bool>> where = x => x.status == (int)Status.正常;
            BindStringStatisticsData(where, fansList); //绑定关注用户数据
            BindWxlist();
        }

        void BindStringStatisticsData(Expression<Func<BLL.Chats.FansStatistics, bool>> where, List<BLL.Chats.FansStatistics> fansList)
        {
            var list = GetCountStatistics(where, x => x.country, fansList);
            BindRepeater(repCountry, list);
            list = GetCountStatistics(where, x => x.city, fansList);
            BindRepeater(repCity, list);

            var langList = GetCountStatistics(where, x => x.language, fansList);
            List<SerisData> langDataList = new List<SerisData>();
            foreach (var lang in langList)
            {
                lang.key = Utils.GetNameByLanguageSymbol(lang.key);
                SerisData sexData = new SerisData() { name = lang.key, data = new int[] { lang.count } };
                langDataList.Add(sexData);
            }
            jsonLang = JsonConvert.SerializeObject(langDataList);
            BindRepeater(repLanguage, langList);


            list = GetCountStatistics(where, x => x.province, fansList);
            jsonData2 = "[{ 'hc_key': 'cn-sh', value: 0 },{ 'hc_key': 'cn-zj', value: 0 },{ 'hc_key': 'tw-ph', value: 0 },{ 'hc_key': 'tw-km', value: 0 },{ 'hc_key': 'tw-lk', value: 0 },{ 'hc_key': 'tw-tw', value: 0 },{ 'hc_key': 'tw-cs', value: 0 },{ 'hc_key': 'cn-3664', value: 0 },{ 'hc_key': 'cn-3681', value: 0 },{ 'hc_key': 'tw-tp', value: 0 },{ 'hc_key': 'tw-ch', value: 0 },{ 'hc_key': 'tw-tt', value: 0 },{ 'hc_key': 'tw-pt', value: 0 },{ 'hc_key': 'cn-6657', value: 0 },{ 'hc_key': 'cn-6663', value: 0 },{ 'hc_key': 'cn-6665', value: 0 },{ 'hc_key': 'cn-6666', value: 0 },{ 'hc_key': 'cn-6667', value: 0 },{ 'hc_key': 'cn-gs', value: 0 },{ 'hc_key': 'cn-6669', value: 0 },{ 'hc_key': 'cn-6670', value: 0 },{ 'hc_key': 'cn-6671', value: 0 },{ 'hc_key': 'tw-kh', value: 0 },{ 'hc_key': 'tw-hs', value: 0 },{ 'hc_key': 'tw-hh', value: 0 },{ 'hc_key': 'cn-nx', value: 0 },{ 'hc_key': 'cn-sa', value: 0 },{ 'hc_key': 'tw-cl', value: 0 },{ 'hc_key': 'cn-3682', value: 0 },{ 'hc_key': 'tw-ml', value: 0 },{ 'hc_key': 'cn-6655', value: 0 },{ 'hc_key': 'cn-ah', value: 0 },{ 'hc_key': 'cn-hu', value: 0 },{ 'hc_key': 'tw-ty', value: 0 },{ 'hc_key': 'cn-6656', value:0 },{ 'hc_key': 'tw-cg', value: 0 },{ 'hc_key': 'cn-6658', value: 0 },{ 'hc_key': 'tw-hl', value: 0 },{ 'hc_key': 'tw-nt', value: 0 },{ 'hc_key': 'tw-th', value: 0 },{ 'hc_key': 'cn-6659', value: 0 },{ 'hc_key': 'cn-6660', value: 0 },{ 'hc_key': 'cn-6661', value: 0 },{ 'hc_key': 'tw-yl', value: 0 },{ 'hc_key': 'cn-6662', value: 0 },{ 'hc_key': 'cn-6664', value: 0 },{ 'hc_key': 'cn-6668', value: 0 },{ 'hc_key': 'cn-gd', value: 0 },{ 'hc_key': 'cn-fj', value: 0 },{ 'hc_key': 'cn-bj', value: 0 },{ 'hc_key': 'cn-hb', value: 0 },{ 'hc_key': 'cn-sd', value: 0 },{ 'hc_key': 'tw-tn', value: 0 },{ 'hc_key': 'cn-tj', value: 0 },{ 'hc_key': 'tw-il', value: 0 },{ 'hc_key': 'cn-js', value: 0 },{ 'hc_key': 'cn-ha', value: 0 },{ 'hc_key': 'cn-qh', value: 0 },{ 'hc_key': 'cn-jl', value: 0 },{ 'hc_key': 'cn-xz', value: 0 },{ 'hc_key': 'cn-xj', value: 0 },{ 'hc_key': 'cn-he', value: 0 },{ 'hc_key': 'cn-nm', value: 0 },{ 'hc_key': 'cn-hl', value: 0 },{ 'hc_key': 'cn-yn', value: 0 },{ 'hc_key': 'cn-gx', value: 0 },{ 'hc_key': 'cn-ln', value: 0 },{ 'hc_key': 'cn-sc', value: 0 },{ 'hc_key': 'cn-cq', value: 0 },{ 'hc_key': 'cn-gz', value: 0 },{ 'hc_key': 'cn-hn', value: 0 },{ 'hc_key': 'cn-sx', value: 0 },{ 'hc_key': 'cn-jx', value: 0 }]";
            List<RegionData> listse = JsonConvert.DeserializeObject<List<RegionData>>(jsonData2);
            var list2 = new List<PercentCountStatistics>();
            int lastCount = fansList.Count;
            
            var unKnownData = (from x in list where x.key == "未知" select x).FirstOrDefault();
            if (unKnownData != null)
            {
                list2.Add(unKnownData);
                lastCount -= unKnownData.count;
            }
            int maxCount = 0;
            if (list.Count >= 8)
                maxCount = 8;
            else
                maxCount = list.Count;
            for (int i = 0; i < maxCount; i++)
            {
                if (list[i].key != "未知")
                {
                    list2.Add(list[i]);
                    lastCount -= list[i].count;
                }
            }
            list2.Add(new PercentCountStatistics()
                {
                    count = lastCount,
                    key = "其他",
                    percent = "12.0"
                });
            BindRepeater(repMapProv, list2);
            foreach (var s in list)
            {
                string keyName = Utils.GetKeyByProvinceName(s.key);
                if (!string.IsNullOrEmpty(keyName))
                {
                    var region = listse.Where(x => x.hc_key == keyName).FirstOrDefault();
                    if (region != null)
                    {
                        region.value = s.count;
                    }
                }
            }
            jsonData2 = JsonConvert.SerializeObject(listse).Replace("hc_key", "hc-key");
            BindRepeater(repProvince, list);



            list = GetCountStatistics(where, x => x.sex, fansList);
            List<SerisData> sexList = new List<SerisData>();
            foreach (var s in list)
            {
                SerisData sexData = new SerisData() { name = s.key, data = new int[] { s.count } };
                sexList.Add(sexData);
            }
            jsonSex = JsonConvert.SerializeObject(sexList);
            BindRepeater(repSex, list);


        }


        #region 获取统计
        List<PercentCountStatistics> GetCountStatistics(Expression<Func<BLL.Chats.FansStatistics, bool>> where, Expression<Func<BLL.Chats.FansStatistics, string>> groupBy, List<BLL.Chats.FansStatistics> fansList)
        {
            return fansList.Where(where.Compile()).GroupBy(groupBy.Compile()).OrderByDescending(x => x.Count()).Select(x => new PercentCountStatistics { count = x.Count(), key = x.Key == "" ? "未知" : x.Key ,percent = ((x.Count()*100.0)/fansList.Count()).ToString("0.00")}).ToList();
        }
        List<PercentCountStatistics> GetCountStatistics(Expression<Func<BLL.Chats.FansStatistics, bool>> where, Expression<Func<BLL.Chats.FansStatistics, int?>> groupBy, List<BLL.Chats.FansStatistics> fansList)
        {
            return fansList.Where(where.Compile()).GroupBy(groupBy.Compile()).OrderByDescending(x => x.Count()).Select(x => new PercentCountStatistics { count = x.Count(), key = ((WeChatSex)x.Key).ToString(), percent = ((x.Count() * 100.0) / fansList.Count()).ToString("0.00") }).ToList();
        }
        #endregion

        #region 数据绑定
        void BindRepeater(Repeater rep, IEnumerable data)
        {
            rep.DataSource = data;
            rep.DataBind();
        }
        #endregion

        #region 右上角的微信对比绑定
        protected void BindWxlist()
        {
            
            List<t_wx_wechats> listAll;
            if (u_type == (int)UserFlag.子账号)
                listAll = BLL.Chats.wx_wechats.GetListByChildUid(u_id, int.MaxValue, 1, out totalCount);
            else
                listAll = BLL.Chats.wx_wechats.GetListByUid(u_id, int.MaxValue, 1, out totalCount);
            StringBuilder strJson = new StringBuilder();
            strJson.Append("[");
            foreach (DAL.t_wx_wechats wechat in listAll)
            {
                strJson.Append("{ id: '" + wechat.id + "', name: '" + wechat.wx_pb_name + "' }, ");
            }

            if (strJson.Length > 0)
            {
                strJson.Remove(strJson.Length - 1, 1);
                if (strJson.Length > 0)
                strJson.Append("]");
            }

            strWxlist = strJson.ToString();
        }
        #endregion

    }


    #region 实体类
    public class RegionData
    {
        public string hc_key { get; set; }
        public int value { get; set; }
    }

    public class SerisData
    {
        public string name { get; set; }
        public IEnumerable<int> data { get; set; }
    }
    #endregion
}