using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiDuMapAPI
{
    /// <summary>
    /// poi的扩展信息，仅当scrope=2时，显示该字段，不同的poi类型，显示的detail_info字段不同。
    /// </summary>
    public class DetailInfo
    {
        /// <summary>
        /// 距离中心点的距离
        /// </summary>
        public int distance { get; set; }
        /// <summary>
        /// 所属分类，如’hotel’、’cater’。
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string tag { get; set; }
        /// <summary>
        /// poi的详情页
        /// </summary>
        public string detail_url { get; set; }
        /// <summary>
        /// poi商户的价格
        /// </summary>
        public string price { get; set; }
        /// <summary>
        /// 营业时间
        /// </summary>
        public string shop_hours { get; set; }
        /// <summary>
        /// 总体评分
        /// </summary>
        public string overall_rating { get; set; }
        /// <summary>
        /// 口味评分
        /// </summary>
        public string taste_rating { get; set; }
        /// <summary>
        /// 服务评分
        /// </summary>
        public string service_rating { get; set; }
        /// <summary>
        /// 环境评分
        /// </summary>
        public string environment_rating { get; set; }
        /// <summary>
        /// 星级（设备）评分
        /// </summary>
        public string facility_rating { get; set; }
        /// <summary>
        /// 卫生评分
        /// </summary>
        public string hygiene_rating { get; set; }
        /// <summary>
        /// 技术评分
        /// </summary>
        public string technology_rating { get; set; }
        /// <summary>
        /// 图片数
        /// </summary>
        public string image_num { get; set; }

        /// <summary>
        /// 团购数
        /// </summary>
        public int groupon_num { get; set; }
        /// <summary>
        /// 优惠数
        /// </summary>
        public int discount_num { get; set; }
        /// <summary>
        /// 评论数
        /// </summary>
        public string comment_num { get; set; }
        /// <summary>
        /// 收藏数
        /// </summary>
        public string favorite_num { get; set; }
        /// <summary>
        /// 签到数
        /// </summary>
        public string checkin_num { get; set; }

    }
}
