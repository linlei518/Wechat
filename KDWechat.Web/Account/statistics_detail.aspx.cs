using KDWechat.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.BLL.Users;
using System.Linq.Expressions;
using System.Text;
using KDWechat.DAL;
using KDWechat.BLL.Logs;
using KDWechat.BLL.Chats;
using System.Collections;


namespace KDWechat.Web.Account
{
    public partial class statistics_detail : Web.UI.BasePage
    {
        protected int count1 = 0;//统计总数1
        protected int count2 = 0;//统计总数2
        public string strWxlist = "";//拼接微信号JSON的string
        protected string tagName = "";//统计字段1的名称
        protected string tagName2 = "";//统计字段2的名称
        protected string unit = "";//计量单位
        #region query property
        protected int tag { get { return RequestHelper.GetQueryInt("tag", 0); } }//统计的类型，对应COMMON中的StatisticsType
        protected int wxID { get { return wx_id; } }
        #endregion
        #region chart property
        protected string chartDateRange = ""; //这些属性解释请见前台JS
        protected string chartUnit = "";
        protected string chartSerisName1 = "";
        protected string chartSeris1 = "";
        protected string chartYName = "";
        protected string chartXInterval = "";
        protected string chartSerisName2 = "";
        protected string chartSeris2 = "";
        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                InitData();//初始化数据
        }

        //初始化数据
        private void InitData()
        {
            if (wxID != -1)
            {
                BindTagName(tag);//根据tag绑定统计字段
                BindTotalCount(tag);//根据tag绑定总数
                //InitRepeaterData(tag);//初始化repeater数据
                
                var dateList = Utils.GetDateListByStartAndEnd(Utils.StrToDateTime(txtbegin_date.Text, DateTime.Now.AddDays(-7)), Utils.StrToDateTime(txtend_date.Text, DateTime.Now));//获取一个时间段内的所有日期 -Damos
                BindChart(dateList,tag);//绑定图表信息
                BindWxlist();//绑定左上角可选微信号JSON
            }
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            InitData();
        }


        #region BindData -Damos
        //绑定统计字段的名称以及计量单位
        private void BindTagName(int tag)
        {
            if (tag == (int)StatisticsType.关注用户)
            {
                tagName = "关注用户";
                tagName2 = "取消关注用户";
                unit = "人";
            }
            else if (tag == (int)StatisticsType.消息)
            {
                unit = "条";
                tagName = "接收消息";
                tagName2 = "发送消息";
            }
        }
        //绑定图表
        private void BindChart(List<DateTime> dateList, int tag)
        {
            string[] weekList;
            chartXInterval = (dateList.Count / 10).ToString(); //日期过多时，显示间隔为日期数/10 -Damos
            chartDateRange = Utils.GetDateXAxisByDateList(dateList);//获取X轴的值（短日期,分割） -Damos
            if (tag == (int)StatisticsType.关注用户)
            {
                chartUnit = "人"; //toolTip里面的单位 -Damos
                chartYName = "人数（人）";//y轴名称

                weekList = BLL.Users.wx_fans.GetFansCountByDateList(dateList, wxID);
                chartSeris1 = Utils.GetArrayStr(weekList, ",");
                chartSerisName1 = "订阅人数";

                weekList = BLL.Users.wx_fans.GetUnFansCountByDateList(dateList, wxID);
                chartSeris2 = Utils.GetArrayStr(weekList, ",");
                chartSerisName2 = "退订人数";
            }
            else if (tag == (int)StatisticsType.消息)
            {
                chartUnit = "条"; //toolTip里面的单位 -Damos
                chartYName = "条数（条）";//y轴名称

                weekList = BLL.Logs.wx_fans_chats.GetChatCountByDateList(dateList, wx_id, FromUserType.用户);
                chartSeris1 = Utils.GetArrayStr(weekList, ",");
                chartSerisName1 = "接收条数";

                weekList = BLL.Logs.wx_fans_chats.GetChatCountByDateList(dateList, wx_id, FromUserType.公众号);
                chartSeris2 = Utils.GetArrayStr(weekList, ",");
                chartSerisName2 = "发送条数";

            }
        }

        void BindTotalCount(int tag)
        {
            if (tag == (int)StatisticsType.关注用户)
            {
                List<BLL.Chats.FansStatistics> fansList = wx_fans.GetFansListByWxID(wxID);
                count1 = fansList.Count(x => x.status == (int)Status.正常);
                count2 = fansList.Count(x => x.status == (int)Status.禁用);
                Expression<Func<BLL.Chats.FansStatistics, bool>> where = x=> x.status == (int)Status.正常;
                BindStringStatisticsData(where,fansList); //绑定关注用户数据
                where = x => x.status == (int)Status.禁用;
                BindUnStatisticsData(where,fansList);//绑定取消关注用户的数据

            }
            else if (tag == (int)StatisticsType.消息)
            {
                count1 = wx_fans_chats.GetCount(x => x.wx_id == wxID && x.from_type == (int)FromUserType.用户);
                count2 = wx_fans_chats.GetCount(x => x.wx_id == wxID && x.from_type == (int)FromUserType.公众号);
                Expression<Func<t_wx_fans_chats, bool>> where = x => x.wx_id == wxID && x.from_type == (int)FromUserType.用户;
                BindStringStatisticsData(where);//绑定接收到消息的数据
                where = x => x.wx_id == wx_id && x.from_type == (int)FromUserType.公众号;
                BindUnStatisticsData(where);//绑定发送消息的数据

            }

        }

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
                strJson.Append("]");
            }

            strWxlist = strJson.ToString();

        }
        #endregion


        #region fansData -Damos
        private void BindUnStatisticsData(Expression<Func<BLL.Chats.FansStatistics, bool>> where, List<BLL.Chats.FansStatistics> fansList)
        {
            repUnCountry.DataSource = GetCountStatistics(where,x => x.country,fansList); //wx_fans.GetCountStatistics(x => x.country, where);
            repUnCountry.DataBind();
            repUnCity.DataSource = GetCountStatistics(where, x => x.city, fansList);
            repUnCity.DataBind();
            var langList = GetCountStatistics(where, x => x.language, fansList);
            foreach (var lang in langList)
            {
                lang.key = Utils.GetNameByLanguageSymbol(lang.key);
            }
            repUnLanguage.DataSource = langList;
            repUnLanguage.DataBind();
            repUnProvince.DataSource = GetCountStatistics(where, x => x.province, fansList);
            repUnProvince.DataBind();
            repUnSex.DataSource = GetCountStatistics(where, x => x.sex, fansList);
            repUnSex.DataBind();

        }

        void BindStringStatisticsData(Expression<Func<BLL.Chats.FansStatistics, bool>> where, List<BLL.Chats.FansStatistics> fansList)
        {
            var list = GetCountStatistics(where, x => x.country, fansList);
            BindRepeater(repCountry, list);
            list = GetCountStatistics(where, x => x.city, fansList);
            BindRepeater(repCity, list);

            var langList = GetCountStatistics(where, x => x.language, fansList);
            foreach (var lang in langList)
            {
                lang.key = Utils.GetNameByLanguageSymbol(lang.key);
            }
            BindRepeater(repLanguage, langList);

            list = GetCountStatistics(where, x => x.province, fansList);
            BindRepeater(repProvince, list);

            list = GetCountStatistics(where, x => x.sex, fansList);
            BindRepeater(repSex, list);

        }
        #endregion
        #region messageData -Damos
        private void BindUnStatisticsData(Expression<Func<t_wx_fans_chats, bool>> where)
        {
            string[] openIDS = wx_fans_chats.GetFansOpenIDArray(where);
            Expression<Func<t_wx_fans, bool>> fansWhere = x => openIDS.Contains(x.open_id);
            repUnCountry.DataSource = wx_fans.GetCountStatistics(x => x.country, fansWhere);
            repUnCountry.DataBind();
            repUnCity.DataSource = wx_fans.GetCountStatistics(x => x.city, fansWhere);
            repUnCity.DataBind();
            var langList = wx_fans.GetCountStatistics(x => x.language, fansWhere);
            foreach (var lang in langList)
            {
                if (lang.key != "")
                    lang.key = Utils.GetNameByLanguageSymbol(lang.key);
                else
                    lang.key = "未知";
            }
            repUnLanguage.DataSource = langList;
            repUnLanguage.DataBind();
            repUnProvince.DataSource = wx_fans.GetCountStatistics(x => x.province, fansWhere);
            repUnProvince.DataBind();
            repUnSex.DataSource = wx_fans.GetSexCountStatistics(x => x.sex ?? 0, fansWhere);
            repUnSex.DataBind();
        }

        void BindStringStatisticsData(Expression<Func<t_wx_fans_chats, bool>> where)
        {
            string[] openIDS = wx_fans_chats.GetFansOpenIDArray(where);
            Expression<Func<t_wx_fans, bool>> fansWhere = x => openIDS.Contains(x.open_id);
            repCountry.DataSource = wx_fans.GetCountStatistics(x => x.country, fansWhere);
            repCountry.DataBind();
            repCity.DataSource = wx_fans.GetCountStatistics(x => x.city, fansWhere);
            repCity.DataBind();
            var langList = wx_fans.GetCountStatistics(x => x.language, fansWhere);
            foreach (var lang in langList)
            {
                if (lang.key != "")
                    lang.key = Utils.GetNameByLanguageSymbol(lang.key);
                else
                    lang.key = "未知";
            }
            repLanguage.DataSource = langList;
            repLanguage.DataBind();
            repProvince.DataSource = wx_fans.GetCountStatistics(x => x.province, fansWhere);
            repProvince.DataBind();
            repSex.DataSource = wx_fans.GetSexCountStatistics(x => x.sex ?? 0, fansWhere);
            repSex.DataBind();
        }
        #endregion

        #region 数据绑定
        void BindRepeater(Repeater rep, IEnumerable data)
        {
            rep.DataSource = data;
            rep.DataBind();
        }
        #endregion

        #region 格式化所有key
        List<CountStatistics> FormatKey(List<CountStatistics> list)
        {
            foreach (var x in list)
            {
                if (x.key.Length > 100)
                    x.key = x.key.Substring(0, 4) + "...";
            }
            return list;
        }
        #endregion

        #region 获取统计
        List<CountStatistics> GetCountStatistics(Expression<Func<BLL.Chats.FansStatistics, bool>> where, Expression<Func<BLL.Chats.FansStatistics, string>> groupBy, List<BLL.Chats.FansStatistics> fansList)
        {
            return fansList.Where(where.Compile()).GroupBy(groupBy.Compile()).Select(x => new CountStatistics { count = x.Count(), key = x.Key == "" ? "未知" : x.Key }).ToList();
        }
        List<CountStatistics> GetCountStatistics(Expression<Func<BLL.Chats.FansStatistics, bool>> where, Expression<Func<BLL.Chats.FansStatistics, int?>> groupBy, List<BLL.Chats.FansStatistics> fansList)
        {
            return fansList.Where(where.Compile()).GroupBy(groupBy.Compile()).Select(x => new CountStatistics { count = x.Count(), key = ((WeChatSex)x.Key).ToString() }).ToList();
        }
        #endregion

    }
}