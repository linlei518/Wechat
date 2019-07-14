using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace BaiDuMapAPI
{
    public class BaiDuLBS
    {
        public const string BASEURL = "http://api.map.baidu.com/";
        public const string MAPKEY = "DD610e50dbdc31173edc1d5c92be4076";
        public const string RADIUS = "5000";//周围五公里
        public const string OUTPUTFORMAT = "json";
        public const string LBSNEARBYURL = "http://api.map.baidu.com/geosearch/v3/nearby";//  GET请求 poi周边搜索
        public const string LBSLOCALURL = "http://api.map.baidu.com/geosearch/v3/local";//  GET请求   poi本地检索
        public const string LBSBOUNDURL = "http://api.map.baidu.com/geosearch/v3/bound";//  GET请求   poi矩形检索
        public const string GEOTABLEID = "73467";
        public const string LBSPOICREATE = "http://api.map.baidu.com/geodata/v3/poi/create";
        public const string LBSPOIUPDATE = "http://api.map.baidu.com/geodata/v3/poi/update";
        public const string LBSGEOLISTURL = "http://api.map.baidu.com/geodata/v3/poi/list";
        public const string LBSDELETEURL = "http://api.map.baidu.com/geodata/v3/poi/delete";


        public static GeoList GetList()
        {
            string listText = HttpUtility.RequestUtility.HttpGet(LBSGEOLISTURL + "?ak=" + MAPKEY + "&geotable_id=" + GEOTABLEID);
            return JsonConvert.DeserializeObject<GeoList>(listText);
        }

        public static string GetNearBy(string query,string lat, string lng,string radius)
        {
            string url = PlaceReqUrl(query, lat, lng,radius);
            string returnText = HttpUtility.RequestUtility.HttpGet(url, Encoding.UTF8);
            return returnText;
        }

        public static string CreatePoint(string address, string contents, string title, string lat, string lng, string tags, string imgurl, string origin,string w_url,string isTop)
        {

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("address", address);
            dic.Add("ak", MAPKEY);
            dic.Add("contents", contents);
            dic.Add("coord_type", "1");
            dic.Add("title", title);
            dic.Add("geotable_id", GEOTABLEID);
            dic.Add("latitude", lat);
            dic.Add("longitude", lng);
            dic.Add("tags", tags);
            dic.Add("ImgUrl", imgurl);
            dic.Add("wx_og_id", origin);
            dic.Add("w_url", w_url);
            dic.Add("w_top", isTop);
            return HttpUtility.RequestUtility.HttpPost(LBSPOICREATE, formData: dic);
        }

        public static string UpdatePoint(string id, string address, string contents, string title, string lat, string lng, string tags, string imgurl, string origin,string w_url,string wid,string isTop)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("id", id);
            dic.Add("address", address);
            dic.Add("ak", MAPKEY);
            dic.Add("contents", contents);
            dic.Add("coord_type", "1");
            dic.Add("title", title);
            dic.Add("geotable_id", GEOTABLEID);
            dic.Add("latitude", lat);
            dic.Add("longitude", lng);
            dic.Add("tags", tags);
            dic.Add("ImgUrl", imgurl);
            dic.Add("wx_og_id", origin);
            dic.Add("w_url", w_url);
            dic.Add("w_id", wid);
            dic.Add("w_top", isTop);
            return HttpUtility.RequestUtility.HttpPost(LBSPOIUPDATE, formData: dic);
        }

        public static bool DeletePoint(string id)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("id", id);
            dic.Add("ak", MAPKEY);
            dic.Add("geotable_id", GEOTABLEID);
            string data = HttpUtility.RequestUtility.HttpPost(LBSDELETEURL, formData: dic);
            return data.Contains("\"status\":0");
        }

        public static string PlaceReqUrl(string query, string lat, string lng,string radius="5000")
        {
            System.Web.UI.Page p = new System.Web.UI.Page();
            string url = LBSNEARBYURL + "?q=" + p.Server.UrlEncode(query) + "&location=" + lng + "," + lat + "&geotable_id=" + GEOTABLEID + "&ak=" + MAPKEY + "&radius=" + radius + "&sortby=distance:1";
            return url;
        }

    }
}
