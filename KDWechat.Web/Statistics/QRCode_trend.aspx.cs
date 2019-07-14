using KDWechat.BLL.Chats;
using KDWechat.Common;
using KDWechat.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.Statistics
{
    public partial class QRCode_trend : Web.UI.BasePage
    {
        protected string chartDateStr = "";//前台筛选日期
        protected string chartSeris1 = "";//饼图数据
        protected Int32 count = 0;//前台统计总数
        protected DateTime startTime;
        protected DateTime endTime;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                InitData();
        }

        public void InitData()
        {
            startTime = Utils.StrToDateTime(txtbegin_date.Text, DateTime.Now.AddDays(-6));
            endTime = Utils.StrToDateTime(txtend_date.Text, DateTime.Now).AddDays(1);
            chartDateStr = startTime.ToString("yyyy-MM-dd ") + "至 " + endTime.ToString("yyyy-MM-dd ");
            var fans_list = Companycn.Core.EntityFramework.EFHelper.GetGroupBy<creater_wxEntities, t_wx_fans, int?, CountStatistics<int?>>(x => x.source_id != 0 && x.wx_id == wx_id && x.subscribe_time > startTime && x.subscribe_time < endTime, x => x.source_id, x => new CountStatistics<int?> { key = x.Key, count = x.Count() });
            var qrcodeList = Companycn.Core.EntityFramework.EFHelper.GetList<creater_wxEntities, t_wx_qrcode, int>(x => x.wx_id == wx_id && x.q_type == 1, x => x.id, int.MaxValue, 1);
            var result_list = fans_list.Select(x => new { name = (qrcodeList.Where(y => y.souce_id == x.key).FirstOrDefault() ?? new t_wx_qrcode { q_name = "未知" }).q_name, count = x.count, source_id = x.key }).ToList();
            string str = "";
            for (int i = 0; i < result_list.Count; i++)
            {
                str += "['" + result_list[i].name + "'," + result_list[i].count.ToString() + "],";
            }
            if (str.Length > 0)
                str = str.Substring(0, str.Length - 1);
            chartSeris1 = str;
            //int[] weekList;
            //weekList = result_list.Select(z => z.count).ToArray();
            //chartSeris1 = JsonConvert.SerializeObject(weekList);
            count = result_list.Select(z => z.count).Sum();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            InitData();
        }
    }
}