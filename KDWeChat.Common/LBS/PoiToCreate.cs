using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KDWechat.Common.LBS
{
    class PoiToCreate
    {

        /// <summary>
        /// geotable_id
        /// </summary>
        public string geotable_id { get; set; }
        /// <summary>
        /// poi名称
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// poi地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double longitude { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double latitude { get; set; }
        /// <summary>
        /// 经纬度类型
        /// </summary>
        public int coord_type { get; set; }
        /// <summary>
        /// poi的标签
        /// </summary>
        public string tags { get; set; }
        /// <summary>
        /// ak
        /// </summary>
        public string ak { get; set; }
       

        /// <summary>
        /// 内容
        /// </summary>
        public string contents { get; set; }
      

    }
}
