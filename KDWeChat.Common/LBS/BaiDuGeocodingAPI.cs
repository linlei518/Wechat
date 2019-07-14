using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiDuMapAPI
{
    /*
     * 提供从地址到经纬度坐标或者从经纬度坐标到地址的转换服务
     * */
    public class BaiDuGeocodingAPI : BaiDuMapMain
    {

        #region 定义变量
        private readonly string output = OUTPUTFORMATJSON;//输出格式
        private readonly string ak = MAPKEY;//APP KEY
        private string callback { get; set; }//callback=showLocation(JavaScript函数名);将json格式的返回值通过callback函数返回以实现jsonp功能
        public string address { get; set; }//根据指定地址进行坐标的反定向解析   ->（用于地理编码服务）
        public string city { get; set; }//地址所在的城市名       ->（用于地理编码服务）           
        private string coordtype { get; set; }//坐标的类型，目前支持的坐标类型包括：bd09ll（百度墨卡托坐标）、gcj02ll（国测局墨卡托坐标）、wgs84ll（ GPS经纬度）->（用于逆地理编码服务）

        public string lat { get; set; }//纬度
        public string lng { get; set; }//经度
        private string location { get { return lat + "," + lng; } }//根据经纬度坐标获取地址->（用于逆地理编码服务）
        public string pois { get; set; }//是否显示指定位置周边的poi，0为不显示，1为显示。当值为1时，显示周边100米内的poi。->（用于逆地理编码服务）
        private readonly string REQUESTURL = BASEURL + GEOCODERSERVICES + "/" + VERSION + "/";
        #endregion

        /// <summary>
        /// 正向地理编码(初始参数为address,city)
        /// </summary>
        /// <returns></returns>
        public string ForwardResolved()
        {
            //http://api.map.baidu.com/geocoder/v2/?ak=您的密钥&callback=renderOption&output=json&address=福德大厦&city=上海市
            string url = REQUESTURL + "?ak=" + ak + "&callback=" + callback + "&output=" + output + "&address=" + address + "&city=" + city;
            string returnText = HttpUtility.RequestUtility.HttpGet(url, Encoding.UTF8);
            return returnText;
        }
        //反向地理编码
        public string ReverseResolved()
        {
            //http://api.map.baidu.com/geocoder/v2/?ak=您的密钥&callback=renderReverse&location=39.983424,116.322987&output=json&pois=0
            string url = REQUESTURL + "?ak=" + ak + "&callback=" + callback + "&output=" + output + "&location=" + location + "&pois=" + pois;
            string returnText = HttpUtility.RequestUtility.HttpGet(url, Encoding.UTF8);
            return returnText;

        }

        /// <summary>
        /// 通过坐标换取地址
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static string GetAddressByLocation(string locat)
        {
            //?ak=您的密钥&callback=&location=39.983424,116.322987&output=json&pois=0
            string url = "http://api.map.baidu.com/geocoder/v2/?ak=DD610e50dbdc31173edc1d5c92be4076&callback=&output=json&location=" + locat + "&pois=0";
            string returnText = HttpUtility.RequestUtility.HttpGet(url, Encoding.UTF8);
            return returnText;
        }

        //返回码状态
        public string ReturnCodeStr(int status)
        {
            string reStr = "未知";
            switch (status)
            {
                case 0:
                    reStr = "正常";
                    break;
                case 1:
                    reStr = "服务器内部错误";
                    break;
                case 2:
                    reStr = "请求参数非法";
                    break;
                case 3:
                    reStr = "权限校验失败";
                    break;
                case 4:
                    reStr = "配额校验失败";
                    break;
                case 5:
                    reStr = "ak不存在或者非法";
                    break;
                case 101:
                    reStr = "服务禁用";
                    break;
                case 102:
                    reStr = "不通过白名单或者安全码不对";
                    break;

            }
            return reStr;

        }

    }
}
