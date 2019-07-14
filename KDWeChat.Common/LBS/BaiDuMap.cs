using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiDuMapAPI
{
    public class BaiDuMap
    {
        public static string BASEURL = "http://api.map.baidu.com/place/v2/search";
        public static string MAPKEY = "PnRrsvtCDBdGi790aerwcsOw";
        public static string RADIUS = "5000";//周围五公里
        public static string OUTPUTFORMAT = "json";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query">关键字</param>
        /// <param name="lat">纬度值</param>
        /// <param name="lng">经度值</param>
        /// <returns></returns>
        public static string GetPlace(string query, string lat, string lng)
        {
            string url = PlaceReqUrl(query, lat, lng);
            string returnText = HttpUtility.RequestUtility.HttpGet(url, Encoding.UTF8);
            return returnText;
        }

        public static string PlaceReqUrl(string query, string lat, string lng)
        {

            String url = BASEURL + "?query=" + query + "&ak="
                + MAPKEY + "&location=" + lat + "," + lng + "&radius=" + RADIUS + "&output=" + OUTPUTFORMAT;
            return url;
        }


    }
}
