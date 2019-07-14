using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;
using KDWechat.DAL;

namespace KDWechat.Web.zh_user
{
    public partial class report_temp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        private List<t_parking> GetData(int pagesize)
        {
           
            var where = " where 1=1  ";

            int totalCount = 0;
            var list = DapperConnection.minebea.GetListBySql<t_parking>(" select * from t_parking " + where, " id desc ", pagesize, 1, out totalCount);
            list.ForEach(x =>
            {
                switch (x.park_id)
                {
                    case 0:
                        x.create_by = "地下一层";
                        break;
                    case 1:
                        x.create_by = "地下二层";
                        break;
                    case 2:
                        x.create_by = " C-B1";
                        break;
                    case 3:
                        x.create_by = " C-B2";
                        break;
                }

            });
            return list;
        }


        public void create_report_month()
        {

            var list = GetData(10000);



            list.ForEach(x =>
            {
                switch (x.park_id)
                {
                    case 0:
                        x.create_by = "地下一层";
                        break;
                    case 1:
                        x.create_by = "地下二层";
                        break;
                    case 2:
                        x.create_by = " C-B1";
                        break;
                    case 3:
                        x.create_by = " C-B2";
                        break;
                }


            });


            DataTable dt = new DataTable();
            var titles = new List<string>();
            var begin_date = DateTime.Now.ToString("yyyy-MM-01 00:00:00");
            var end_date = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
            //var begin_date = DateTime.Now.ToString("2018-04-01 00:00:00");
            //var end_date = DateTime.Now.ToString("2018-04-30 23:59:59");
            titles.Add("车位");
            titles.Add("车牌");
            titles.Add("停车天数");
            titles.Add("部门");
            titles.Add("姓名");
         

            dt.Columns.Add("车位", Type.GetType("System.String"));
            dt.Columns.Add("车牌", Type.GetType("System.String"));
            dt.Columns.Add("停车次数", Type.GetType("System.String"));
            dt.Columns.Add("部门", Type.GetType("System.String"));
            dt.Columns.Add("姓名", Type.GetType("System.String"));
        

            TimeSpan d3 = Utils.ObjectToDateTime(end_date).Subtract(Utils.ObjectToDateTime(begin_date));
            var days = d3.Days;
            //for (int i = 0; i < days; i++)
            //{
            //    titles.Add(Utils.ObjectToDateTime(begin_date).AddDays(i).ToString("yyyy年MM月dd日"));
            //    dt.Columns.Add(Utils.ObjectToDateTime(begin_date).AddDays(i).ToString("yyyy年MM月dd日"), Type.GetType("System.String"));
            //}


            var url = System.Configuration.ConfigurationManager.AppSettings["park_api_url"] + "/api/pay/GetCapImgInfo";
            var iv = DateTime.Now.ToString("yyyyMMdd"); var park_key = System.Configuration.ConfigurationManager.AppSettings["park_key"];
            var num = 0;


            var user_list = DapperConnection.minebea.GetList<t_zh_user>();

            foreach (var item in list)
            {
                var rows = dt.NewRow();
                park_list_report.ParkResult<List<park_list_report.CarInfo>> result = null;

                rows["车位"] = item.create_by + "-" + item.parking_num;
                rows["车牌"] = item.plate_number;
                var tZhUser = user_list.FirstOrDefault(x => x.id == item.user_id);
                rows["部门"] = tZhUser != null ? tZhUser.user_dpt : "";
                rows["姓名"] = item.user_name;

                if (string.IsNullOrWhiteSpace(item.plate_number))
                {
                    //for (int i = 0; i < days; i++)
                    //{
                    //    rows[Utils.ObjectToDateTime(begin_date).AddDays(i).ToString("yyyy年MM月dd日")] = "0";
                    //}
                    //dt.Rows.Add(rows);
                    continue;
                }


                foreach (var item_number in item.plate_number.Split('|'))
                {
                    var where_str = "{\"plateNo\":\"" + item_number.Substring(1, item_number.Length - 1) + "\",\"type\":0,\"startTime\":\"" + Utils.ObjectToDateTime(begin_date).AddDays(-10).ToString("yyyy-MM-dd 00:00:00") + "\",\"endTime\":\"" + Utils.ObjectToDateTime(end_date).ToString("yyyy-MM-dd 23:59:59") + "\",\"pageIndex\":1,\"pageCount\":1000}";
                    var content = "{\"data\":\"" + zh_helper.Encrypt3Des(where_str, park_key, CipherMode.ECB, iv) + "\"}";
                    var result_str = zh_helper.Post(url, content);
                    if (item_number == item.plate_number.Split('|')[0])
                    {
                        result =
                            JsonHelper.JSONToObject<park_list_report.ParkResult<List<park_list_report.CarInfo>>>(
                                result_str);
                    }
                    else
                    {
                        result.data.AddRange(JsonHelper.JSONToObject<park_list_report.ParkResult<List<park_list_report.CarInfo>>>(result_str).data);
                    }
                }



                var in_num = 0;

                for (int i = 0; i < days; i++)
                {
                    if (result.data != null)
                    {
                        //查询 今天的入场数据
                        num = result.data.Count(y => Utils.ObjectToDateTime(y.capTime).ToString("yyyy-MM-dd") == Utils.ObjectToDateTime(begin_date).AddDays(i).ToString("yyyy-MM-dd") && y.type == "1");

                        //查询最后数据是否入场
                        if (num == 0)
                        {
                            var max_model = result.data.OrderByDescending(y => Utils.ObjectToDateTime(y.capTime)).FirstOrDefault(y => Utils.ObjectToDateTime(y.capTime) <= Utils.ObjectToDateTime(begin_date + " 23:59:59"));
                            if (max_model != null)
                            {
                                if (max_model.type == "1")//最后是入场
                                {
                                    num = 1;

                                }
                            }
                        }
                    }

                    if (num > 0)
                    {
                        in_num++;
                        //rows[Utils.ObjectToDateTime(begin_date).AddDays(i).ToString("yyyy年MM月dd日")] = "1";
                    }
                    else
                    {
                        //rows[Utils.ObjectToDateTime(begin_date).AddDays(i).ToString("yyyy年MM月dd日")] = "0";
                    }


                }
                if (in_num < days / 3)
                {
                    rows["停车次数"] = in_num;
                    dt.Rows.Add(rows);
                }

            }

            System.IO.File.Delete(System.AppDomain.CurrentDomain.BaseDirectory +
                                  ("/zh_user/month_less_park_list" + ".xls"));
            GemBoxExcelLiteHelper.SaveExcel(
                     System.AppDomain.CurrentDomain.BaseDirectory+("/zh_user/month_less_park_list" + ".xls"),
                     this.Page, false, false, titles, dt);

        }


    }

   
}