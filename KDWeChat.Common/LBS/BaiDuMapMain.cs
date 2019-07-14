using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace BaiDuMapAPI
{
    public class BaiDuMapMain : System.Web.UI.Page
    {

        public static readonly string BASEURL = "http://api.map.baidu.com/"; //百度API域名
        public static readonly string MAPKEY = ConfigurationManager.AppSettings["BaiDuMapKey"].ToString();//百度API Key
        public static readonly string OUTPUTFORMATJSON = "json";//{ get { return "json"; } }//输出格式
        public static readonly string OUTPUTFORMATXML = "xml";
        public static readonly string VERSION = "v2";//API版本号
        public static readonly string GEOCODERSERVICES = "geocoder";//Geocoding API 服务名称

        public static readonly string PLACESERVICES = "place";//Place API 服务名称

        public static readonly string GEOSEARCHSERVICES = "geosearch";//LBS云检索 API 服务名称
        public static readonly string GEOSEARCHSERVICESVERSION = "v3";
        /// <summary>
        /// 父类的构造函数
        /// </summary>
        public BaiDuMapMain()
        {

        }
    }
}
