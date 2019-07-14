using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiDuMapAPI
{
    /// <summary>
    /// 用于返回查询某个区域的某类POI数据，且提供单个POI的详情查询服务
    /// http://api.map.baidu.com/place/v2/search   //v2 place区域检索POI服务
    ///http://api.map.baidu.com/place/v2/detail   //v2 POI详情服务
    ///http://api.map.baidu.com/place/v2/eventsearch   //v2 团购信息检索服务
    //http://api.map.baidu.com/place/v2/eventdetail  //v2 商家团购信息查询
    /// </summary>
    public class BaiDuPlaceAPI : BaiDuMapMain
    {
        #region 定义变量
        /// <summary>
        /// //输出格式
        /// </summary>
        private readonly string output = OUTPUTFORMATJSON;
        /// <summary>
        /// APPKey
        /// </summary>
        private readonly string ak = MAPKEY;
        /// <summary>
        /// 检索关键字
        /// </summary>
        public string query { get; set; }
        /// <summary>
        /// 标签项，与q组合进行检索
        /// </summary>
        public string tag { get; set; }
        /// <summary>
        /// 检索结果详细程度。取值为1 或空，则返回基本信息；取值为2，返回检索POI详细信息
        /// </summary>
        public int scope { get; set; }
        /// <summary>
        /// 检索过滤条件，当scope取值为2时，可以设置filter进行排序。
        /// </summary>
        private string filter { get; set; }
        /// <summary>
        /// 返回记录数量，默认为10条记录，最大返回结果为20条。
        /// </summary>
        public int page_size { get; set; }
        /// <summary>
        /// 分页页码，默认为0,0代表第一页，1代表第二页，以此类推。
        /// </summary>
        public int page_num { get; set; }
        /// <summary>
        /// 用户的权限签名。
        /// </summary>
        private string sn { get; set; }
        /// <summary>
        /// 设置sn后该值必填
        /// </summary>
        private string timestamp { get; set; }

        /// <summary>
        /// 城市内检索请求参数(检索区域，如果取值为“全国”或某省份，则返回指定区域的POI。)
        /// </summary>
        public string region { get; set; }
        /// <summary>
        /// 矩形区域检索参数(38.76623,116.43213,39.54321,116.46773 lat,lng(左下角坐标),lat,lng(右上角坐标))
        /// </summary>
        public string bounds { get; set; }

        public string lat { get; set; }//纬度
        public string lng { get; set; }//经度
        /// <summary>
        /// 圆形区域检索参数(周边检索中心点，不支持多个点)格式如下：38.76623,116.43213 lat(纬度),lng(经度)
        /// </summary>
        private string location { get { return lat + "," + lng; } }
        /// <summary>
        /// 圆形区域检索参数（周边检索半径，单位为米）
        /// </summary>
        public int radius { get; set; }
        //public string uid { }
        /// <summary>
        /// 请求地址
        /// </summary>
        private readonly string REQUESTURL = BASEURL + PLACESERVICES + "/" + VERSION + "/";
        #endregion

        /// <summary>
        /// 城市内检索（query，region必填）
        /// </summary>
        /// <returns></returns>
        public string CitySearch()
        {//http://api.map.baidu.com/place/v2/search?ak=您的密钥&output=json&query=11&page_size=1&page_num=0&scope=1&region=上海
            string url = REQUESTURL + "search/?ak=" + ak + "&output=" + output + "&query=" + query + "&page_size=" + page_size + "&page_num=" + page_num + "&scope=" + scope + "&region=" + region;
            string returnText = HttpUtility.RequestUtility.HttpGet(url, Encoding.UTF8);
            return returnText;
        }
        /// <summary>
        /// 矩形区域检索（query，bounds必填）
        /// </summary>
        /// <returns></returns>
        public string CityBoundsSearch()
        {
            //ttp://api.map.baidu.com/place/v2/search?ak=您的密钥&output=json&query=雅诗阁&page_size=10&page_num=0&scope=1&bounds=39.915,116.404,39.975,116.414

            string url = REQUESTURL + "search/?ak=" + ak + "&output=" + output + "&query=" + query + "&page_size=" + page_size + "&page_num=" + page_num + "&scope=" + scope + "&bounds=" + bounds;
            string returnText = HttpUtility.RequestUtility.HttpGet(url, Encoding.UTF8);
            return returnText;
        }
        /// <summary>
        /// 园形区域检索（query，location必填）
        /// </summary>
        /// <returns></returns>
        public string CityLocationSearch()
        {
            //http://api.map.baidu.com/place/v2/search?ak=您的密钥&output=json&query=雅诗阁&page_size=10&page_num=0&scope=1&location=39.915,116.404&radius=2000

            string url = REQUESTURL + "search/?ak=" + ak + "&output=" + output + "&query=" + query + "&page_size=" + page_size + "&page_num=" + page_num + "&scope=" + scope + "&location=" + location;
            string returnText = HttpUtility.RequestUtility.HttpGet(url, Encoding.UTF8);
            return returnText;
        }
        /// <summary>
        /// 查询某个POI点的详情信息，如好评，评价
        /// </summary>
        /// <param name="uid">poi uid</param>
        /// <returns></returns>
        public string DetailSearch(string uid)
        {
            //http://api.map.baidu.com/place/v2/detail?uid=8ee4560cf91d160e6cc02cd7&ak=E4805d16520de693a3fe707cdc962045&output=json&scope=2
            string url = REQUESTURL + "search/?ak=" + ak + "&output=" + output + "&uid=" + uid + "&scope=2";
            string returnText = HttpUtility.RequestUtility.HttpGet(url, Encoding.UTF8);
            return returnText;
        }


    }
}
