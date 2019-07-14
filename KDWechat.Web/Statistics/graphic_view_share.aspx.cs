using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;

namespace KDWechat.Web.Statistics
{
    public partial class graphic_view_share : UI.BasePage
    {
        protected int viewCount = 0;//阅读篇数
        protected int shareCount = 0;//分享篇数

        #region chart porperty
        protected string barChartData = "";
        protected string viewChartData = "";
        protected string shareChartData = "";
        #endregion
        protected int chartHeight = 160;
        protected DateTime startTime;
        protected DateTime endTime;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckUserAuthority("graphic_statistics", RoleActionType.View);
                InitData();//初始化数据
            }
        }

        private void InitData()
        {
            startTime = Utils.StrToDateTime(txtbegin_date.Text, DateTime.Now.AddDays(-7)).Date;
            endTime = Utils.StrToDateTime(txtend_date.Text, DateTime.Now).Date.AddDays(1);
            List<DAL.p_graphi_statistics_Result> list = null;
            using (DAL.creater_wxEntities db = new DAL.creater_wxEntities())
            {
                list = db.p_graphi_statistics(startTime, endTime, wx_id).ToList();
            }

            if (list != null && list.Count > 0)
            {
                viewCount = list.Count;
                shareCount = list.Count(x => x.share_count > 0);
                chartHeight += list.Count() * 80;
                foreach (var item in list)
                {
                    barChartData += "'" + item.obj_name + "',";
                    viewChartData += item.view_count + ",";
                    shareChartData += item.share_count + ",";
                }
            }



        }




        protected void Button1_Click(object sender, EventArgs e)
        {
            InitData();
        }
    }
}