using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiDuMapAPI
{
    /// <summary>
    /// addressComponent Geocoding API 逆地理编码返回
    /// </summary>
    public class AddressComponent
    {
        /// <summary>
        /// 城市名
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 区县名
        /// </summary>
        public string district { get; set; }
        /// <summary>
        /// 省名
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 街道名
        /// </summary>
        public string street { get; set; }
        /// <summary>
        /// 街道门牌号
        /// </summary>
        public string street_number { get; set; }
    }
}
