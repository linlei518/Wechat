using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDWechat.BLL.Entity
{
    public class FansForApi
    {
        /// <summary>
        /// guid
        /// </summary>
        public string guid { get; set; }
        /// <summary>
        /// opid
        /// </summary>
        public string open_id { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string nick_name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string wx_sex { get; set; }
        /// <summary>
        /// 国籍
        /// </summary>
        public string wx_country { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string wx_province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string wx_city { get; set; }
        /// <summary>
        /// 地区
        /// </summary>
        public string wx_area { get; set; }
        /// <summary>
        /// 语言
        /// </summary>
        public string language { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string headimgurl { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 关注时间
        /// </summary>
        public DateTime subscribe_time { get; set; }
    }
}
