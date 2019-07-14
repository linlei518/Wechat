using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiDuMapAPI
{
    /// <summary>
    /// pois 周边poi数组
    /// </summary>
    public class Pois
    {
        /// <summary>
        /// 地址信息
        /// </summary>
        public string addr { get; set; }
        /// <summary>
        /// 数据来源
        /// </summary>
        public string cp { get; set; }
        /// <summary>
        /// 离坐标点距离
        /// </summary>
        public string distance { get; set; }
        /// <summary>
        /// poi名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// poi类型，如’ 办公大厦,商务大厦’
        /// </summary>
        public string poiType { get; set; }
        /// <summary>
        /// poi坐标{x,y}
        /// </summary>
        public Point point { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string tel { get; set; }
        /// <summary>
        /// poi唯一标识
        /// </summary>
        public string uid { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public string zip { get; set; }
    }
}
