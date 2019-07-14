using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiDuMapAPI
{
    /// <summary>
    /// Geocoding API 正向地理编码Josn Root
    /// </summary>
    public class GeocodingForwardResolved
    {
        /// <summary>
        /// 返回结果状态值， 成功返回0，
        /// </summary>
        public int status { get; set; }

        public GeocodingForwardResolvedResult result { get; set; }


    }
}
