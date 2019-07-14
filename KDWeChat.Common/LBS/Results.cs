using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiDuMapAPI
{
    /// <summary>
    /// Place API 检索返回结果字段。
    /// </summary>
    public class Results
    {
        /// <summary>
        /// poi名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// poi经纬度坐标
        /// </summary>
        public Location location { get; set; }
        /// <summary>
        /// poi地址信息
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// poi电话信息
        /// </summary>
        public string telephone { get; set; }
        /// <summary>
        /// poi的唯一标示
        /// </summary>
        public string uid { get; set; }
        /// <summary>
        /// poi的扩展信息，仅当scrope=2时，显示该字段，不同的poi类型，显示的detail_info字段不同。
        /// </summary>
        public DetailInfo detail_info { get; set; }

    }
}
