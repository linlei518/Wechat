using KDWechat.BLL.Logs;
using KDWechat.BLL.Users;
using KDWechat.Common;
using KDWechat.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiDuMapAPI.HttpUtility;
using System.Linq.Expressions;

namespace KDWechat.Web.Statistics
{
    public partial class group_message_bydate : Web.UI.BasePage
    {

        protected string newsPieChartData="";                  //群发饼状图数据
        protected string newsOpenPercentChartData="";    //群发打开率饼状图数据
        protected string groupWxXSeris="";                            //群发X轴
        protected int newsAllCount;                               //图文总数
        protected DateTime startTime;
        protected DateTime endTime;
        protected List<DAL.t_wx_group_msgs> group_msg_list = null;
        protected List<DAL.t_wx_wechats> wxList = null;
        protected Dictionary<string, int> groupNewsList = null;
        protected string IDs = RequestHelper.GetQueryString("Ids");
        protected int newsHeight = 0;
        protected int retainHeight = 160;                          //图表保留高度


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckUserAuthority("sys_dashboard");
                InitData();//初始化数据            
            }
        }

        private void InitData()
        {
            startTime = Utils.StrToDateTime(txtbegin_date.Text, DateTime.Now.AddDays(-7));
            endTime = Utils.StrToDateTime(txtend_date.Text, DateTime.Now);
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
            wxList = Companycn.Core.EntityFramework.EFHelper.GetList<creater_wxEntities, t_wx_wechats, int>(wx_list_where, x => x.id, int.MaxValue, 1);//KDWechat.BLL.Chats.wx_wechats.GetUseList();
            BindGroupNewsPie();
            BindGroupNewsReadPercentPie();
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            InitData();
        }

        //群发饼图
        private void BindGroupNewsPie()
        {
            var startDate = startTime;
            var endDate = endTime;
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


        private void BindGroupNewsReadPercentPie()
        {
            var statisticsList = new List<t_wx_group_msg_read_percent_statistics>();
            var wx_id_array = group_msg_list.GroupBy(x => x.wx_id).Select(x => x.Key).ToList();//取所有群发过的微信号
            wx_id_array.ForEach(x =>
            {
                var total_send_count = 0;
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
                        var url = "https://api.weixin.qq.com/datacube/getarticletotal?access_token=" + accessToken;//获取数据
                        var date = y;
                        string data = string.Format("{{\"begin_date\":\"{0}\",\"end_date\":\"{0}\"}}", date.Date.ToString("yyyy-MM-dd"));
                        var databytes = Encoding.UTF8.GetBytes(data);
                        MemoryStream ms = new MemoryStream(databytes);
                        try
                        {
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

                decimal read_percent = (decimal)((total_read_count * 1.0) / (total_send_count+1)) * 100;
                var statisToAdd = new t_wx_group_msg_read_percent_statistics { creat_time = DateTime.Now, read_persent = read_percent, wx_id = x, wx_og_id = "dummy" };
                statisticsList.Add(statisToAdd);
            });
            if (statisticsList.Count == 0)
            {
                news_read_lit_table.Text = "";
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
                    strHtml.Append("<td><a href=\"group_msg_list.aspx?wx_id=" + x.wx_id + "&m_id=" + m_id.ToString() + "&sdate=" + DESEncrypt.Encrypt(startTime.ToString(), "sdate") + "&edate=" + DESEncrypt.Encrypt(endTime.ToString(), "edate") + "\">查看详情</a></td>");
                    strHtml.Append("</tr>");
                }
            });
            news_read_lit_table.Text = strHtml.ToString();

        }


        protected string GetShowName()
        {
            return BLL.Chats.Statistics_Dashboard.GetDashboardCompareString(wxList);
        }





    }
}