using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;
using KDWechat.DAL;
using KDWechat.Web.logs;
using KDWechat.Web.UI;
using QuickMark;

namespace KDWechat.Web.zh_user
{
    public partial class park_list_report : BasePage
    {

        /// <summary>
        /// 起始时间
        /// </summary>
        public string beginDate
        {
            get { return RequestHelper.GetQueryString("beginDate"); }
        }


        /// <summary>
        ///结束时间
        /// </summary>
        public string endDate
        {
            get { return RequestHelper.GetQueryString("endDate"); }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckUserAuthority("park_list_report");
              
                txt_date_show.Value = DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + " — " + DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
                BindList();
            }
        }


        private void BindList()
        {
          


            this.rptList.DataSource = GetData();
            this.rptList.DataBind();


            string pageUrl = $"park_list_report.aspx?status={status}&key={key}&page=__id__&m_id=" + m_id;

            //div_page.InnerHtml = Utils.OutPageList(pageSize, page, totalCount, pageUrl, 8);


            //EnumHelper.BindEnumDDLSource(typeof(Enums.ParkingStatus), ddlStatus);//车位状态

        }



        public List<CarInfo> GetData()
        {
            txtKeywords.Value = key;
            var where = " where 1=1  ";

            var where_str = "";
            if (!string.IsNullOrWhiteSpace(beginDate))
            {
                txtbegin_date.Text = beginDate;
            }

            if (!string.IsNullOrWhiteSpace(endDate))
            {
                txtend_date.Text = endDate;
                txt_date_show.Value = beginDate + " — " + endDate;
            }

            if (status > 0)
            {
                ddlStatus.SelectedValue = status.ToString();
            }

            var user_list = DapperConnection.minebea.GetList<t_zh_user>(); //所有用户
            var park_list = DapperConnection.minebea.GetList<t_parking>(); //所有车位

            var park_key = System.Configuration.ConfigurationManager.AppSettings["park_key"];
            var iv = DateTime.Now.ToString("yyyyMMdd");

            //type进出标志 必填 0:全部。1:入场 2:离场

            var url = System.Configuration.ConfigurationManager.AppSettings["park_api_url"] + "/api/pay/GetCapImgInfo";

            //var url = "http://180.169.32.157:8099/api/pay/GetCapImgInfo";

            //var where_str = "{\"plateNo\":\"" + key + "\",\"type\":" + status + ",\"startTime\":\"" + DateTime.Now.AddMinutes(-50000).ToString("yyyy-MM-dd HH:mm:ss") + "\",\"endTime\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\",\"pageIndex\":1,\"pageCount\":1000}";
            where_str = "{\"plateNo\":\"" + key + "\",\"type\":" + status + ",\"startTime\":\"" +
                        Utils.StrToDateTime(beginDate).ToString("yyyy-MM-dd 00:00:00") + "\",\"endTime\":\"" +
                        Utils.StrToDateTime(endDate).ToString("yyyy-MM-dd 23:59:59") +
                        "\",\"pageIndex\":1,\"pageCount\":500000}";

            if (string.IsNullOrWhiteSpace(endDate))
            {
                where_str = "{\"plateNo\":\"" + key + "\",\"type\":" + status + ",\"startTime\":\"" +
                            DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "\",\"endTime\":\"" +
                            DateTime.Now.ToString("yyyy-MM-dd 23:59:59") + "\",\"pageIndex\":1,\"pageCount\":500000}";
            }
            var content = "{\"data\":\"" + zh_helper.Encrypt3Des(where_str, park_key, CipherMode.ECB, iv) + "\"}";
            var result_str = zh_helper.Post(url, content);



            var result = JsonHelper.JSONToObject<ParkResult<List<CarInfo>>>(result_str);
            var data_list = new List<CarInfo>();


            foreach (var item in result.data.Select(x => x.plateNo).Distinct().ToList())
            {
                if (string.IsNullOrWhiteSpace(item))
                {
                    continue;
                }

                var car_list = result.data.Where(x => x.plateNo == item).ToList();
                var car_model = car_list.OrderByDescending(x => Utils.StrToDateTime(x.capTime)).FirstOrDefault();
                if (car_model == null)
                {
                    continue;
                }
                var park_model =
                    DapperConnection.minebea.GetList<t_parking>()
                        .FirstOrDefault(x => x.plate_number != null && x.plate_number.Split('|').Contains(item));
                var new_car_model = new CarInfo
                {
                    plateNo = car_model.plateNo,
                    type = car_model.type,
                    capTime = car_model.capTime
                };
                if (car_model.type == "1")
                {
                    new_car_model.count =
                        car_list.Where(x => x.type == "1")
                            .GroupBy(x => Utils.StrToDateTime(x.capTime).ToString("yyyy-MM-dd"))
                            .Count();
                }

                if (car_model.type == "2")
                {
                    new_car_model.count =
                        car_list.Where(x => x.type == "2")
                            .GroupBy(x => Utils.StrToDateTime(x.capTime).ToString("yyyy-MM-dd"))
                            .Count();
                }

                if (park_model != null)
                {
                    new_car_model.user_name = park_model.user_name;
                    new_car_model.user_code = park_model.user_code;
                    new_car_model.park_id = park_model.park_id.ToString();
                    new_car_model.park_num = park_model.parking_num;
                    var tZhUser = user_list.FirstOrDefault(x => x.user_tel == park_model.user_tel);
                    if (tZhUser != null)
                        new_car_model.user_dpt = tZhUser.user_dpt;
                }
                data_list.Add(new_car_model);
            }

           
            foreach (var item in park_list)
            {
                if (string.IsNullOrWhiteSpace(item.plate_number))
                {
                    continue;
                }
                foreach (var plate_number in item.plate_number.Split('|'))
                {
                    if (!string.IsNullOrWhiteSpace(plate_number))
                    {
                        var data_list_plateNo = data_list.Select(x => x.plateNo).ToArray();
                        if (!data_list_plateNo.Contains(plate_number))
                        {

                            var new_car_model = new CarInfo
                            {
                                plateNo = plate_number,
                                type = "/",
                                capTime = "/"
                            };
                            new_car_model.user_name = item.user_name;
                            new_car_model.user_code = item.user_code;
                            new_car_model.park_id = item.park_id.ToString();
                            new_car_model.park_num = item.parking_num;
                            var tZhUser = user_list.FirstOrDefault(x => x.user_tel == item.user_tel);
                            if (tZhUser != null)
                                new_car_model.user_dpt = tZhUser.user_dpt;

                            data_list.Add(new_car_model);


                        }
                    }

                }
            }



            data_list.ForEach(x =>
            {
                switch (x.park_id)
                {
                    case "0":
                        x.park_id = "地下一层";
                        break;
                    case "1":
                        x.park_id = "地下二层";
                        break;
                    case "2":
                        x.park_id = " C-B1";
                        break;
                    case "3":
                        x.park_id = " C-B2";
                        break;
                }

            });


            return data_list;
        }



        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect($"park_list_report.aspx?status={ddlStatus.SelectedValue}&key={txtKeywords.Value.Trim()}" + "&m_id=" + m_id+ "&beginDate="+txtbegin_date.Text+"&endDate="+txtend_date.Text );
        }

        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

           

        }

   
      


        public class ParkResult<T>
        {
            public string resCode { get; set; }
            public string resMsg { get; set; }
            public string totalNum { get; set; }

            public T data { get; set; }
        }


        public class CarInfo
        {
            public string id { get; set; }
            public string plateNo { get; set; }

            /// <summary>
            /// //1：入场 2：离场
            /// </summary>
            public string type { get; set; }

            public string capTime { get; set; }
            public string capPlace { get; set; }
            public string imgName { get; set; }


            public string user_name { get; set; }
            public string user_code { get; set; }
            public string user_dpt { get; set; }
            public int count { get; set; }
            public string  park_id { get; set; }
            public string park_num { get; set; }

        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            var list = GetData();
            list.ForEach(x =>
            {
                switch (x.park_id)
                {
                    case "0":
                        x.park_id = "地下一层";
                        break;
                    case "1":
                        x.park_id = "地下二层";
                        break;
                    case "2":
                        x.park_id = " C-B1";
                        break;
                    case "3":
                        x.park_id = " C-B2";
                        break;
                }
                x.type=x.type == "1" ? "进场" : "出场";
            });
            Dictionary<string, string> titles = new Dictionary<string, string>();


            titles.Add("plateNo", "车牌号");
            titles.Add("user_dpt", "员工部门");
            titles.Add("user_code", "员工编号");
            titles.Add("user_name", "员工姓名");
            titles.Add("park_id", "停车场");
            titles.Add("park_num", "员工车位");
            titles.Add("type", "进出场状态");
            titles.Add("count", "停车天数");
            titles.Add("capTime", "最后出入场时间");



            bool isc = GemBoxExcelLiteHelper.SaveExcel<CarInfo>(
                    Server.MapPath("park_report_list_" + DateTime.Now.ToString("yyyy_MM_dd_HHmmss") + ".xls"),
                    this.Page, true, true, titles, list);
        }
    }
}