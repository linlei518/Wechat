﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;
using KDWechat.DAL;
using KDWechat.BLL.Chats;

namespace KDWechat.Web.Statistics
{
    public partial class menu_click_statistics : Web.UI.BasePage
    {
        protected int hitCount = 0;//统计总数1
        #region chart porperty
        protected string barChartData = "";
        protected string hitChartData = "";
        #endregion
        protected int chartHeight = 160;
        protected DateTime startTime;
        protected DateTime endTime;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckUserAuthority("menu_click_statistics", RoleActionType.View);
                InitData();//初始化数据
            }
        }

        private void InitData()
        {
            startTime = Utils.StrToDateTime(txtbegin_date.Text, DateTime.Now.AddDays(-7)).Date;
            endTime = Utils.StrToDateTime(txtend_date.Text, DateTime.Now).Date.AddDays(1);

            hitCount = Companycn.Core.EntityFramework.EFHelper.GetCount<creater_wxEntities, t_st_diymenu_click>(x => x.wx_og_id == wx_og_id && x.add_time > startTime && x.add_time < endTime);

            BindMenuClickPercentBar();

        }

        //绑定图形
        private void BindMenuClickPercentBar()
        {
            var clickList = Companycn.Core.EntityFramework.EFHelper.GetGroupBy<creater_wxEntities, t_st_diymenu_click, string, CountStatistics<string>>(x => x.wx_og_id == wx_og_id && x.add_time > startTime && x.add_time < endTime, x => x.menu_name, x => new CountStatistics<string> { key = x.Key, count = x.Count() });
            chartHeight += clickList.Count * 70;
            clickList.ForEach(x => {
                hitChartData += string.Format("{0:N2}", (x.count * 100.0 / hitCount)) + ",";
                barChartData += "'" + x.key + "',";
            });
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            InitData();
        }

    }
}