using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KDWechat.Common.Statistics
{
    /// <summary>
    /// 所有模块的浏览记录、分享、点击Model类
    /// </summary>
    public class st_statistics
    {
        /// <summary>
        /// id
        /// </summary>
        public long id { get; set; }
        /// <summary>
        /// 微信id
        /// </summary>
        public int wx_id { get; set; }
        /// <summary>
        /// 微信原始id
        /// </summary>
        public string wx_og_id { get; set; }
        /// <summary>
        /// 用户的openid
        /// </summary>
        public string open_id { get; set; }
        /// <summary>
        /// 由谁转发过来的openid
        /// </summary>
        public string from_open_id { get; set; }

        /// <summary>
        /// URL来源
        /// </summary>
        public string url_referrer { get; set; }
        /// <summary>
        /// 页面的url地址
        /// </summary>
        public string page_url { get; set; }
        /// <summary>
        /// 页面的title
        /// </summary>
        public string page_name { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public System.DateTime add_time { get; set; }
        /// <summary>
        /// 用户ip地址
        /// </summary>
        public string user_ip { get; set; }
        /// <summary>
        /// 对应的数据表id
        /// </summary>
        public int obj_id { get; set; }

        /// <summary>
        /// 对应的数据表名称
        /// </summary>
        public string obj_name { get; set; }

        /// <summary>
        /// 数据库表名
        /// </summary>
        public string db_table_name { get; set; }

       
    }
}
