using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiDuMapAPI
{
    public class LBSNearByContents
    {
        /// <summary>
        /// 数据id
        /// </summary>
        public string uid { get; set; }
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
        /// poi所属省
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// poi所属城市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// poi所属区
        /// </summary>
        public string district { get; set; }
        /// <summary>
        /// 经纬度
        /// </summary>
        public ArrayList location { get; set; }
        /// <summary>
        /// poi的标签
        /// </summary>
        public string tags { get; set; }
        /// <summary>
        /// 距离
        /// </summary>
        public int distance { get; set; }
        /// <summary>
        /// 权重
        /// </summary>
        public int weight { get; set; }
       
        /// <summary>
        /// 数据创建时间
        /// </summary>
        public int create_time { get; set; }
        /// <summary>
        /// 数据编辑时间
        /// </summary>
        public int modify_time { get; set; }
        /// <summary>
        /// 自定义内容
        /// </summary>
        public string contents { get; set; }
        /// <summary>
        /// 自定义图片路径
        /// </summary>
        public string ImgUrl { get; set; }
        /// <summary>
        /// 自定义公众号原始ID
        /// </summary>
        public string wx_og_id { get; set; }

        /// <summary>
        /// 外链地址
        /// </summary>
        public string w_url { get; set; }

        /// <summary>
        /// 本地ID
        /// </summary>
        public string w_id { get; set; }

        /// <summary>
        /// 是否封面
        /// </summary>
        public string w_top { get; set; }
    }
}
