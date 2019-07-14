using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiDuMapAPI
{
    public class GeocodingForwardResolvedResult
    {
        /// <summary>
        /// 经纬度坐标
        /// </summary>
        public Location location { get; set; }
        /// <summary>
        /// 位置的附加信息，是否精确查找。1为精确查找，0为不精确。
        /// </summary>
        public int precise { get; set; }
        /// <summary>
        /// 可信度
        /// </summary>
        public int confidence { get; set; }
        /// <summary>
        /// 地址类型
        /// </summary>
        public string level { get; set; }
    }
}
