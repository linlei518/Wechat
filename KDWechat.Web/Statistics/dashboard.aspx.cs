using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.DAL;
using KDWechat.Common;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using BaiDuMapAPI.HttpUtility;
using KDWechat.BLL.Chats;
using System.Linq.Expressions;

namespace KDWechat.Web.Statistics
{
    public partial class dashboard : Web.UI.BasePage
    {
        protected List<BLL.Chats.TimeStatistics<int>> lastTimeList = null;
        protected List<BLL.Chats.CountStatistics<int>> suscribtList = null;
        protected List<BLL.Chats.CountStatistics<int>> unSuscribtList = null;
        protected List<DAL.t_wx_wechats> wxList = null;
        protected List<DAL.t_wx_fans_chats> chatList = null;
        protected List<DAL.t_wx_group_msgs> group_msg_list = null;
        protected Dictionary<string, int> groupNewsList = null;
        protected int allCount = 0;                                   //公众号总数
        protected int newsAllCount = 0;                                 //图文总数
        protected string vitalityChartData = "";               //活跃度条形图数据
        protected string pieChartData = "";                          //饼状图数据
        protected string newsPieChartData = "";                  //群发饼状图数据
        protected string newsOpenPercentChartData = "";    //群发打开率饼状图数据
        protected string hitChartData = "";                //关键词命中柱状图数据
        protected string moduleArrayData = "";                     //模块名称数组
        protected string moduleOpenData = "";                //公众号模块开启数据
        protected string barChartData = "";                          //条形图数据
        protected string userChartData = "";                       //累计用户数据
        protected string subChartData = "";                    //一周关注用户数据
        protected string unsubChartData = "";              //一周取消关注用户数据
        protected string userIncreaseData = "";              //一周用户净增长数据
        protected string replyMsgData = "";                //一周回复过的用户数据
        protected string reciveMsgData = "";             //一周收到的留言用户数据
        protected string maleData = "";                        //所有粉丝男性数据
        protected string famaleData = "";                      //所有粉丝女性数据
        protected string sexUnknowData = "";                   //所有粉丝未知数据
        protected string userJsonCity = "";                    //所有粉丝城市数据
        protected string groupWxXSeris = "";                            //群发X轴
        protected string strWxlist = "";                               //微信列表
        protected string sub_Sdate = "";                  //关注用户统计开始时间
        protected string sub_Edate = "";                  //关注用户统计结束时间
        protected string sub_SdateD = "";                 //关注用户统计开始时间
        protected string sub_EdateD = "";                 //关注用户统计结束时间
        protected int bindCount = 0;                           //绑定总数--Damos
        protected int retainHeight = 160;                          //图表保留高度
        protected int vitalityHeight = 0;                        //活跃度图表高度
        protected int newsHeight = 0;                          //群发图文图表高度
        protected int otherHeght = 0;                          //其他图文图表高度

        protected string chartDateRange;
        protected string chartXInterval;
        protected string JsonData;


        protected int type = RequestHelper.GetQueryInt("type", -1);
        protected string IDs = RequestHelper.GetQueryString("Ids");

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckUserAuthority("sys_dashboard");
            if (!IsPostBack)
            {
                hidMid.Value = m_id.ToString();

                if (type > -1)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "getListArea", "getListArea(" + type + ")", true);
                }
                else
                {
                    type = 0;
                }



                InitData();//去除分页，表格与图形数据相同，优先加载表格，图表数据来自于表格--Damos
                BindBar();
                BindPie();
                BindGroupNewsPie();
                BindReplyBar();
                BindFansRatio();
                BindGroupNewsReadPercentPie();
                BindVitality();
              
                //BindKeywordPercentBar();


            }
        }
 

        ////绑定命中率
        //private void BindKeywordPercentBar()
        //{
        //    var startTime = DateTime.Now.Date.AddDays(-7);
        //    var endTime = DateTime.Now.Date;
        //    var hitList = Companycn.Core.EntityFramework.EFHelper.GetGroupBy<creater_wxEntities, t_st_keyword_view, string, CountStatistics<string>>(x => x.add_time > startTime && x.add_time < endTime, x => x.wx_og_id, x => new CountStatistics<string> { key = x.Key, count = x.Count() });
        //    var commentsList = Companycn.Core.EntityFramework.EFHelper.GetGroupBy<creater_wxEntities, t_wx_fans_chats, int, CountStatistics<int>>(x => x.create_time > startTime && x.create_time < endTime, x => x.wx_id, x => new CountStatistics<int> { key = x.Key, count = x.Count() });
        //    wxList.ForEach(x => {
        //        var hitCount = (hitList.Where(y => y.key == x.wx_og_id).FirstOrDefault() ?? new CountStatistics<string> { count = 0 }).count;
        //        var commentsCount = (commentsList.Where(y => y.key == x.id).FirstOrDefault() ?? new CountStatistics<int> { count = 0 }).count;
        //        var totalCount = (hitCount + commentsCount) == 0 ? 1 : (hitCount + commentsCount);
        //        hitChartData += string.Format("['{1}',{0:F}]", hitCount * 100.0 / totalCount,x.wx_pb_name) + ",";
        //    });
        //}

        //绑定活跃度 -Damos add at 2014-4-1 14:40
        private void BindVitality()
        {
            var startDate = DateTime.Now.Date.AddDays(-1).AddMonths(-1);
            var endDate = DateTime.Now.Date.AddDays(-1);
            //var fansHisViewList = Companycn.Core.EntityFramework.EFHelper.GetGroupBy<creater_wxEntities, t_wx_fans_hisview, int, CountStatistics<int>>(x => x.view_time > startDate && x.view_time < endDate, x => x.wx_id, x => new CountStatistics<int> { count = x.Count(), key = x.Key });
            //var fansChatList = Companycn.Core.EntityFramework.EFHelper.GetGroupBy<creater_wxEntities, t_wx_fans_chats, int, CountStatistics<int>>(x => x.create_time > startDate && x.create_time < endDate, x => x.wx_id, x => new CountStatistics<int> { count = x.Count(), key = x.Key });
            //var fansLocationList = Companycn.Core.EntityFramework.EFHelper.GetGroupBy<creater_wxEntities, t_wx_fans_hislocation, int, CountStatistics<int>>(x => x.create_time > startDate && x.create_time < endDate, x => x.wx_id, x => new CountStatistics<int> { count = x.Count(), key = x.Key });
            //var fansMenuClickList = Companycn.Core.EntityFramework.EFHelper.GetGroupBy<creater_wxEntities, t_st_diymenu_click, string, CountStatistics<string>>(x => x.add_time > startDate && x.add_time < endDate, x => x.wx_og_id, x => new CountStatistics<string> { count = x.Count(), key = x.Key });

            wxList.ForEach(x =>
            {
                var total = 0;
                var fansHisViewList = Companycn.Core.EntityFramework.EFHelper.GetGroupBy<creater_wxEntities, t_wx_fans_hisview, string, string>(y => y.view_time > startDate && y.view_time < endDate && y.wx_id == x.id, y => y.open_id, y => y.Key);
                var fansChatList = Companycn.Core.EntityFramework.EFHelper.GetGroupBy<creater_wxEntities, t_wx_fans_chats, string, string>(y => y.create_time > startDate && y.create_time < endDate && y.wx_id == x.id, y => y.open_id, y => y.Key);
                var fansLocationList = Companycn.Core.EntityFramework.EFHelper.GetGroupBy<creater_wxEntities, t_wx_fans_hislocation, string, string>(y => y.create_time > startDate && y.create_time < endDate && y.wx_id == x.id, y => y.open_id, y => y.Key);
                var fansMenuClickList = Companycn.Core.EntityFramework.EFHelper.GetGroupBy<creater_wxEntities, t_st_diymenu_click, string, string>(y => y.add_time > startDate && y.add_time < endDate && y.wx_og_id == x.wx_og_id, y => x.wx_og_id, y => y.Key);
                
                var totalList = new List<string>();
                totalList.AddRange(fansHisViewList);
                totalList.AddRange(fansChatList);
                totalList.AddRange(fansLocationList);
                totalList.AddRange(fansMenuClickList);
                totalList = totalList.Distinct().ToList();
                total = totalList.Count();
                //total += (fansHisViewList.Where(y => y.key == x.id).FirstOrDefault() ?? new CountStatistics<int> { count = 0 }).count;
                //total += (fansChatList.Where(y => y.key == x.id).FirstOrDefault() ?? new CountStatistics<int> { count = 0 }).count;
                //total += (fansLocationList.Where(y => y.key == x.id).FirstOrDefault() ?? new CountStatistics<int> { count = 0 }).count;
                //total += (fansMenuClickList.Where(y => y.key == x.wx_og_id).FirstOrDefault() ?? new CountStatistics<string> { count = 0 }).count;

                var fansCount = Companycn.Core.EntityFramework.EFHelper.GetCount<creater_wxEntities, t_wx_fans>(y => y.wx_id == x.id);//(suscribtList.Where(y => y.key == x.id).FirstOrDefault() ?? new CountStatistics<int> { count = 1 }).count;
                fansCount = fansCount == 0 ? 1 : fansCount;
                var percent = ((total * 1.0 / fansCount) * 100);
                vitalityChartData += string.Format("{0:F}", percent) + ",";
            });


        }

        //绑定群发阅读率饼图
        private void BindGroupNewsReadPercentPie()
        {
            var startDate = DateTime.Now.Date;
            var endDate = DateTime.Now.Date.AddDays(1);
            var statisticsList = Companycn.Core.EntityFramework.EFHelper.GetList<creater_wxEntities, t_wx_group_msg_read_percent_statistics, int>(x => x.creat_time > startDate && x.creat_time < endDate, x => x.id, int.MaxValue, 1);

            if (statisticsList == null || statisticsList.Count == 0)
            {
                statisticsList = new List<t_wx_group_msg_read_percent_statistics>();
                var wx_id_array = group_msg_list.GroupBy(x => x.wx_id).Select(x => x.Key).ToList();//取所有群发过的微信号
                wx_id_array.ForEach(x =>
                {
                    var total_send_count = 1;
                    var total_read_count = 0;

                    var groupMsgList = group_msg_list.Where(y => y.wx_id == x).ToList();//取当前微信号的所有群发
                    var timeList = new List<DateTime>();//新建群发集合，用以收集群发日期
                    groupMsgList.ForEach(y =>
                    {
                        if (!timeList.Contains(y.create_time.Date))//如果不包含当前日期则添加进时间轴
                            timeList.Add(y.create_time.Date);
                    });
                    var accessToken = BLL.Chats.wx_wechats.GetAccessToken(x);//取当前微信号的ACCESSTOKEN
                    if (!accessToken.Contains("Error"))
                    {
                        timeList.ForEach(y =>
                        {
                            try
                            {
                                var url = "https://api.weixin.qq.com/datacube/getarticletotal?access_token=" + accessToken;//获取数据
                                var date = y;
                                string data = string.Format("{{\"begin_date\":\"{0}\",\"end_date\":\"{0}\"}}", date.Date.ToString("yyyy-MM-dd"));
                                var databytes = Encoding.UTF8.GetBytes(data);
                                MemoryStream ms = new MemoryStream(databytes);
                                var responses = BaiDuMapAPI.HttpUtility.RequestUtility.HttpPostWithEncoding(url, ms, Encoding.UTF8);
                                var news_result_list = JsonConvert.DeserializeObject<BLL.Entity.WeiXin_Group_Article_Total_List>(responses);
                                if (news_result_list != null && news_result_list.list != null && news_result_list.list.Count != 0)
                                {
                                    news_result_list.list.ForEach(z =>
                                    {
                                        var detail = z.details.OrderByDescending(m => m.stat_date).Take(1).FirstOrDefault() ?? new BLL.Entity.WeiXin_Group_Article_Total_Detail { target_user = 0, int_page_read_user = 0 };
                                        total_send_count += detail.target_user;
                                        total_read_count += detail.int_page_read_count;
                                    });
                                }
                            }
                            catch {
                            }
                        });
                    }
                    else
                    {
                        //accesstoken出错。不处理，直接continue
                    }
                    try
                    {
                        decimal read_percent = (decimal)((total_read_count * 1.0) / total_send_count) * 100;
                        var statisToAdd = new t_wx_group_msg_read_percent_statistics { creat_time = DateTime.Now, read_persent = read_percent, wx_id = x, wx_og_id = "dummy" };
                        Companycn.Core.EntityFramework.EFHelper.AddModel<creater_wxEntities, t_wx_group_msg_read_percent_statistics>(statisToAdd);
                        statisticsList.Add(statisToAdd);
                    }
                    catch {  }
                });
                if (statisticsList.Count == 0)
                    return;
            }


            var strHtml = new StringBuilder();
            statisticsList.ForEach(x =>
            {

                if (wxList.Where(y => y.id == x.wx_id).FirstOrDefault() != null)
                {
                    var pbName = (wxList.Where(y => y.id == x.wx_id).FirstOrDefault() ?? new t_wx_wechats { wx_pb_name = "未知" }).wx_pb_name;
                    groupWxXSeris += "'" + pbName + "',";
                    newsOpenPercentChartData += "['" + pbName + "'," + string.Format("{0:F}", x.read_persent) + "],"; ;
                    strHtml.Append("<tr>");
                    strHtml.Append("<td class=\"name\">" + pbName + "</td>");
                    strHtml.Append("<td>" + string.Format("{0:F}", x.read_persent) + "%</td>");
                    var count = groupNewsList.Where(y => y.Key == pbName).First().Value;
                    newsPieChartData += "['" + pbName + "'," + count.ToString() + "],";//这个是绑定数量相关的图表
                    strHtml.Append("<td style='text-align:center'>" + count.ToString() + "</td>");
                    strHtml.Append("<td><a href=\"group_msg_list.aspx?wx_id=" + x.wx_id + "&m_id=" + m_id.ToString() + "\">查看详情</a></td>");
                    strHtml.Append("</tr>");
                }
            });
            news_read_lit_table.Text = strHtml.ToString();

        }

        //用户性别、城市条形图
        private void BindFansRatio()
        {
            var sexList = BLL.Users.wx_fans.GetAllSexStatistic();
            var cityList = BLL.Users.wx_fans.GetAllCityStatistic();

            var maleList = sexList.Where(x => x.property == (int)WeChatSex.男).ToList();
            var famaleList = sexList.Where(x => x.property == (int)WeChatSex.女).ToList();
            var unKnowList = sexList.Where(x => x.property != (int)WeChatSex.男 && x.property != (int)WeChatSex.女).ToList();

            List<SerisData> cityData = new List<SerisData>();

            var beijingRegionArray = new string[] { "东城", "西城", "朝阳", "丰台", "石景山", "海淀", "门头沟", "房山", "通州", "顺义", "昌平", "大兴", "怀柔", "平谷", "密云", "延庆" };
            var tianjinRegionArray = new string[] { "和平", "河东", "河西", "南开", "河北", "红桥", "东丽", "西青", "津南", "北辰", "武清", "宝坻", "滨海新区", "宁河", "静海", "蓟县" };
            var shanghaiRegionArray = new string[] { "黄浦", "徐汇", "长宁", "静安", "普陀", "闸北", "虹口", "杨浦", "闵行", "宝山", "嘉定", "浦东新", "金山", "松江", "青浦", "奉贤", "崇明" };

            var shanghaiCountList = new List<int>();
            var beijingCountList = new List<int>();
            var chengduCountList = new List<int>();
            var ningboCountList = new List<int>();
            var wuhanCountList = new List<int>();
            var tianjinCountList = new List<int>();
            var guangzhouCountList = new List<int>();
            var shenzhenCountList = new List<int>();
            var elseCountList = new List<int>();

            var index = 0;
            foreach (var wechat in wxList)
            {
                maleData += (maleList.Where(x => x.wx_id == wechat.id).FirstOrDefault() ?? new BLL.Entity.All_Property_Statistics<int>() { count = 0 }).count + ",";
                famaleData += (famaleList.Where(x => x.wx_id == wechat.id).FirstOrDefault() ?? new BLL.Entity.All_Property_Statistics<int>() { count = 0 }).count + ",";
                sexUnknowData += (unKnowList.Where(x => x.wx_id == wechat.id).FirstOrDefault() ?? new BLL.Entity.All_Property_Statistics<int>() { count = 0 }).count + ",";

                var elseCount = cityList.Where(x => x.wx_id == wechat.id).Count();
                shanghaiCountList.Add(cityList.Where(x => x.wx_id == wechat.id && shanghaiRegionArray.Contains(x.property)).Count());
                beijingCountList.Add(cityList.Where(x => x.wx_id == wechat.id && beijingRegionArray.Contains(x.property)).Count());
                chengduCountList.Add(cityList.Where(x => x.wx_id == wechat.id && x.property == "成都").Count());
                ningboCountList.Add(cityList.Where(x => x.wx_id == wechat.id && x.property == "宁波").Count());
                wuhanCountList.Add(cityList.Where(x => x.wx_id == wechat.id && x.property == "武汉").Count());
                tianjinCountList.Add(cityList.Where(x => x.wx_id == wechat.id && tianjinRegionArray.Contains(x.property)).Count());
                guangzhouCountList.Add(cityList.Where(x => x.wx_id == wechat.id && x.property == "广州").Count());
                shenzhenCountList.Add(cityList.Where(x => x.wx_id == wechat.id && x.property == "深圳").Count());
                elseCountList.Add(elseCount - shanghaiCountList[index] - beijingCountList[index] - chengduCountList[index] - ningboCountList[index] - wuhanCountList[index] - tianjinCountList[index] - guangzhouCountList[index] - shenzhenCountList[index]);
                index++;
            }
            cityData.Add(new SerisData() { name = "其他", data = elseCountList });
            cityData.Add(new SerisData() { name = "上海", data = shanghaiCountList });
            cityData.Add(new SerisData() { name = "北京", data = beijingCountList });
            cityData.Add(new SerisData() { name = "成都", data = chengduCountList });
            cityData.Add(new SerisData() { name = "武汉", data = wuhanCountList });
            cityData.Add(new SerisData() { name = "天津", data = tianjinCountList });
            cityData.Add(new SerisData() { name = "广州", data = guangzhouCountList });
            cityData.Add(new SerisData() { name = "宁波", data = ningboCountList });
            userJsonCity = JsonConvert.SerializeObject(cityData);
        }

        //绑定回复用户条形图
        private void BindReplyBar()
        {
            DateTime endTime = DateTime.Now.Date.AddDays(1);
            DateTime startTime = endTime.AddDays(-6);
            chatList = Companycn.Core.EntityFramework.EFHelper.GetList<DAL.creater_wxEntities, t_wx_fans_chats, int>(x => x.create_time >= startTime && x.create_time <= endTime, x => x.id, int.MaxValue, 1);

            if (wxList.Count > 0)
            {

                foreach (var wechat in wxList)
                {
                    reciveMsgData += chatList.Where(x => x.wx_id == wechat.id && x.from_type == (int)Common.FromUserType.用户).GroupBy(x => x.open_id).Count().ToString() + ",";
                    replyMsgData += chatList.Where(x => x.wx_id == wechat.id && x.from_type == (int)Common.FromUserType.公众号).GroupBy(x => x.open_id).Count().ToString() + ",";
                }
            }
        }

        //群发饼图
        private void BindGroupNewsPie()
        {
            var startDate = DateTime.Now.Date.AddDays(-1).AddMonths(-1);
            var endDate = DateTime.Now.Date.AddDays(-1);
            //取所有图文群发
            group_msg_list = Companycn.Core.EntityFramework.EFHelper.GetList<creater_wxEntities, t_wx_group_msgs, int>(x => (x.msg_type == (int)msg_type.单图文 || x.msg_type == (int)msg_type.多图文) && x.is_send == (int)is_sendMode.是 && x.send_time > startDate && x.send_time < endDate && x.wx_id != 24, x => x.id, int.MaxValue, 1, true);

            //增加msg_count
            var group_msg_list_with_count = group_msg_list.Select(x => new BLL.Entity.Group_Msg_Statistics { wx_id = x.wx_id, id = x.id, msg_count = x.msg_type == (int)msg_type.单图文 ? 1 : 2, source_id = x.source_id ?? 0 }).ToList();

            //取实际count,总数计数
            group_msg_list_with_count.ForEach(x =>
            {
                var child_count = Companycn.Core.EntityFramework.EFHelper.GetCount<creater_wxEntities, t_wx_news_materials>(y => y.par_id == x.source_id);
                x.msg_count = child_count + 1;
            });

            //取群发过的微信号
            var list = group_msg_list_with_count.GroupBy(x => x.wx_id).Select(x => new BLL.Chats.CountStatistics<int>() { key = x.Key, count = x.Count() }).ToList();
            //微信号ID列
            var wx_id_array = list.Select(x => x.key).ToArray();
            //群发过的微信号详细列表
            var wx_list = wxList.Where(x => wx_id_array.Contains(x.id)).ToList();//Companycn.Core.EntityFramework.EFHelper.GetList<creater_wxEntities, t_wx_wechats, int>(x => wx_id_array.Contains(x.id), x => x.id, int.MaxValue, 1, true);

            groupNewsList = new Dictionary<string, int>();//生成一个list用于记录内容，最终并入表格
            newsHeight += retainHeight;
            wx_list.ForEach(x =>
            {
                var count = 0;
                group_msg_list_with_count.Where(y => y.wx_id == x.id).ToList().ForEach(y =>
                {
                    count += y.msg_count;
                });
                if (count != 0)
                    newsHeight += 35;
                groupNewsList.Add(x.wx_pb_name, count);
                newsAllCount += count;
            });
        }

        //绑定饼状图
        private void BindPie()
        {
            allCount = wxList.Count();//BLL.Users.wx_fans.GetWXCount();
            int noBindcount = allCount - bindCount;

            StringBuilder strHtml = new StringBuilder();
            strHtml.Append("<tr>");
            strHtml.Append("<td class=\"name\">已绑定</td>");
            strHtml.Append("<td>" + bindCount.ToString() + "</td>");
            strHtml.Append("<td><a href=\"Binding_List.aspx?Ids="+IDs+"&isB=1&m_id=" + m_id.ToString() + "\">查看详情</a></td>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<td class=\"name\">未绑定</td>");
            strHtml.Append("<td>" + noBindcount.ToString() + "</td>");
            strHtml.Append("<td><a href=\"Binding_List.aspx?Ids="+IDs+"&isB=2&m_id=" + m_id.ToString() + "\">查看详情</a></td>");
            strHtml.Append("</tr>");
            lit_table.Text = strHtml.ToString();

            pieChartData += "['已绑定'," + bindCount.ToString() + "],";
            pieChartData += "['未绑定'," + noBindcount.ToString() + "]";
        }

        //绑定关注条形图
        private void BindBar()
        {
            DateTime endTime = DateTime.Now.Date.AddDays(1);
            DateTime startTime = endTime.AddDays(-6);

            //wxList = KDWechat.BLL.Chats.wx_wechats.GetUseList();
            if (wxList.Count > 0)
            {
                BindWX();

                StringBuilder strJsonName = new StringBuilder();
                StringBuilder strJsonUser = new StringBuilder();
                StringBuilder strJsonSub = new StringBuilder();
                StringBuilder strJsonNoSub = new StringBuilder();

                foreach (var wechat in wxList)
                {
                    strJsonName.Append("'" + wechat.wx_pb_name + "'" + ",");

                    int Fcount = BLL.Users.wx_fans.GetTotalCountByWxID(wechat.id);
                    var Scount = GetSCount(wechat.id);//内存中取数据--Damos
                    var Uscount = GetUSCount(wechat.id);//内存中取数据--Damos
                    strJsonUser.Append(Fcount.ToString() + ",");
                    strJsonSub.Append((Scount) + ",");
                    strJsonNoSub.Append((Uscount) + ",");
                    userIncreaseData += (int.Parse(Scount) - int.Parse(Uscount)) + ",";
                }

                barChartData += strJsonName.ToString().TrimEnd(',');
                userChartData += strJsonUser.ToString().TrimEnd(',');
                subChartData += strJsonSub.ToString().TrimEnd(',');
                unsubChartData += strJsonNoSub.ToString().TrimEnd(',');
            }

        }

        //公众号绑定详细报表
        private void InitData()
        {
            pageSize = 10;

            lastTimeList = new List<BLL.Chats.TimeStatistics<int>>();
            var fansTimeList = BLL.Chats.wechat_binding.GetFansTimeStatistics();
            var viewTimeList = BLL.Chats.wechat_binding.GetViewTimeStatistics();
            var chatTimeList = BLL.Chats.wechat_binding.GetChatsTimeStatistics();
            var locationTimeList = BLL.Chats.wechat_binding.GetLocationTimeStatistics();
            suscribtList = BLL.Chats.wechat_binding.GetWeeklyFansCount(true);
            unSuscribtList = BLL.Chats.wechat_binding.GetWeeklyFansCount(false);

            lastTimeList.AddRange(viewTimeList);
            lastTimeList.AddRange(chatTimeList);
            lastTimeList.AddRange(locationTimeList);
            CacheHelper.Insert("s_fansTimeList", fansTimeList);
            CacheHelper.Insert("s_viewTimeList", viewTimeList);
            CacheHelper.Insert("s_chatTimeList", chatTimeList);
            CacheHelper.Insert("s_locationTimeList", locationTimeList);

            Expression<Func<t_wx_wechats, bool>> wx_list_where = x => x.status == (int)Status.正常;
            if (!string.IsNullOrWhiteSpace(IDs))
            {
                var idArray = IDs.Split(',');
                if (idArray.Length != 0)
                {
                    var wx_ids = new int[idArray.Length];
                    for (int i = 0; i < wx_ids.Length; i++)
                    {
                        wx_ids[i] = Utils.StrToInt(idArray[i], 0);
                    }
                    wx_list_where = wx_list_where.And(x => wx_ids.Contains(x.id));
                }
            }

            wxList = Companycn.Core.EntityFramework.EFHelper.GetList<creater_wxEntities, t_wx_wechats, int>(wx_list_where, x => x.id, int.MaxValue, 1, out totalCount, true);//去除分页，取所有--Damos
            otherHeght = retainHeight + wxList.Count * 61;
            vitalityHeight = retainHeight + wxList.Count * 33;
            Repeater1.DataSource = wxList;//这里不可以删，有其他数据依赖于此表绑定时的计算  --Damos
            Repeater1.DataBind();
        }
        protected string GetSCount(object objId)
        {
            var strToReturn = "0";
            var id = Utils.ObjToInt(objId, 0);
            if (id != 0)
            {
                var last_time = suscribtList.Where(x => x.key == id).FirstOrDefault();
                if (last_time != null)
                    strToReturn = last_time.count.ToString();
            }
            return strToReturn;
        }

        protected string GetUSCount(object objId)
        {
            var strToReturn = "0";
            var id = Utils.ObjToInt(objId, 0);
            if (id != 0)
            {
                var last_time = unSuscribtList.Where(x => x.key == id).FirstOrDefault();
                if (last_time != null)
                    strToReturn = last_time.count.ToString();
            }
            return strToReturn;

        }

        protected string GetLastTime(object objId)
        {
            var strToReturn = "无";
            var id = Utils.ObjToInt(objId, 0);
            if (id != 0)
            {
                var last_time = lastTimeList.Where(x => x.key == id).OrderByDescending(x => x.last_time).FirstOrDefault();
                if (last_time != null)
                    strToReturn = last_time.last_time.ToString();
            }
            return strToReturn;
        }

        protected string GetStatus(object objId)
        {
            var strToReturn = "否";
            var id = Utils.ObjToInt(objId, 0);
            if (id != 0)
            {
                var last_time = lastTimeList.Where(x => x.key == id).OrderByDescending(x => x.last_time).FirstOrDefault();
                if (last_time != null)
                {
                    var span = DateTime.Now - last_time.last_time;
                    if (span < TimeSpan.FromDays(10) && span > TimeSpan.FromDays(0))
                    {
                        strToReturn = "是";
                        bindCount++;//在表格绑定是统计已绑定至本平台的公众号数--Damos
                    }
                }
            }
            return strToReturn;
        }


        /// <summary>
        /// 绑定微信
        /// </summary>
        protected void BindWX()
        {
            string str = "[";
            StringBuilder strJson = new StringBuilder();

            var topWxList = BLL.Chats.wx_wechats.GetUseList();
            if (topWxList.Count > 0)
            {
                foreach (var wechat in topWxList)
                {
                    strJson.Append("{ id: '" + wechat.id + "', name: '" + wechat.wx_pb_name + "' },");
                }
            }

            if (strJson.Length > 0)
            {
                str += strJson.ToString().TrimEnd(',');

            }
            str += "]";

            strWxlist = str;
        }

        protected string GetShowName()
        {
            return BLL.Chats.Statistics_Dashboard.GetDashboardCompareString(wxList);
        }



    }
}