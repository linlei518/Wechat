using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiDuMapAPI
{
    public class BaiDuLBSAPI : BaiDuMapMain
    {
        private readonly string ak = "DD610e50dbdc31173edc1d5c92be4076";
        /// <summary>
        /// geotable主键
        /// </summary>
        public int geotable_id { get; set; }
        public string q { get; set; }
        public string lat { get; set; }//纬度
        public string lng { get; set; }//经度
        /// <summary>
        /// 检索的中心点
        /// </summary>
        private string location { get { return lng + "," + lat; } }
        /// <summary>
        /// 检索半径
        /// </summary>
        public int radius { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string tags { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string sortby { get; set; }
        /// <summary>
        /// 过滤条件
        /// </summary>
        private string filter { get; set; }
        /// <summary>
        /// 分页索引
        /// </summary>
        public int page_index { get; set; }
        /// <summary>
        /// 分页数量
        /// </summary>
        public int page_size { get; set; }
        /// <summary>
        /// 回调函数
        /// </summary>
        private string callback { get; set; }
        /// <summary>
        /// 用户的权限签名
        /// </summary>
        private string sn { get; set; }
        private readonly string LBSNEARBYURL = "http://api.map.baidu.com/geosearch/v3/nearby/nearby";
        /*public static string radius = "50000";//周围五公里
        public static string output = "json";
        public static string LBSNEARBYURL = "http://api.map.baidu.com/geosearch/v2/nearby";//  GET请求 poi周边搜索
        public static string LBSLOCALURL = "http://api.map.baidu.com/geosearch/v2/local";//  GET请求   poi本地检索
        public static string LBSBOUNDURL = "http://api.map.baidu.com/geosearch/v2/bound";//  GET请求   poi矩形检索
        public static string GEOTABLEID = "32236";*/

        public string GetNearBy()
        {

            string url = PlaceReqUrl(q, lat, lng);
            string returnText = HttpUtility.RequestUtility.HttpGet(url, Encoding.UTF8);
            return returnText;

        }
        public string GetNearBy(string lat, string lng)
        {
            string query = "";
            string url = PlaceReqUrl(query, lat, lng);
            string returnText = HttpUtility.RequestUtility.HttpGet(url, Encoding.UTF8);
            return returnText;

        }
        public string PlaceReqUrl(string query, string lat, string lng)
        {
            //string tags = "上海";"&tag=" + p.Server.UrlDecode(tags) +
            System.Web.UI.Page p = new System.Web.UI.Page();

            /*String url = LBSNEARBYURL + "?q=" + p.Server.UrlDecode(query) + "&ak="
                + MAPKEY + "&location=" + lng + "," + lat + "&radius=" + RADIUS +  "&geotable_id=" + GEOTABLEID + "&sortby=" + "distance:1";*/
            string url = LBSNEARBYURL + "?q=" + p.Server.UrlEncode(query) + "&location=" + location + "&geotable_id=" + geotable_id + "&ak=" + ak + "&radius=" + radius + "&sortby=distance:1&page_size="+page_size;
            return url;
        }
    }
}
