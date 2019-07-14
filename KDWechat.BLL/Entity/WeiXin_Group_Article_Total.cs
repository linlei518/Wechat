using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDWechat.BLL.Entity
{
    public class WeiXin_Group_Article_Total_List
    {
        /// <summary>
        /// 蛋疼的list变量。。实际没用。里面包含的就是一个list
        /// </summary>
        public List<WeiXin_Group_Article_Total> list { get; set; }
    }


    public class WeiXin_Group_Article_Total
    {
        /// <summary>
        /// 日期
        /// </summary>
        public String ref_date { get; set; }
        /// <summary>
        /// 消息号
        /// </summary>
        public String msgid { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public String title { get; set; }
        /// <summary>
        /// 统计详情
        /// </summary>
        public List<WeiXin_Group_Article_Total_Detail> details { get; set; }
    }

    public class WeiXin_Group_Article_Total_Detail
    {
        /// <summary>
        /// 统计时间
        /// </summary>
        public String stat_date { get; set; }
        /// <summary>
        /// 目标用户数
        /// </summary>
        public int target_user { get; set; }
        /// <summary>
        /// 阅读人数
        /// </summary>
        public int int_page_read_user { get; set; }
        /// <summary>
        /// 阅读次数
        /// </summary>
        public int int_page_read_count { get; set; }
        /// <summary>
        /// 原文阅读人数
        /// </summary>
        public int ori_page_read_user { get; set; }
        /// <summary>
        /// 原文阅读次数
        /// </summary>
        public int ori_page_read_count { get; set; }
        /// <summary>
        /// 分享人数
        /// </summary>
        public int share_user { get; set; }
        /// <summary>
        /// 分享次数
        /// </summary>
        public int share_count { get; set; }
        /// <summary>
        /// 添加到收藏的人数
        /// </summary>
        public int add_to_fav_user { get; set; }
        /// <summary>
        /// 添加到收藏的次数
        /// </summary>
        public int add_to_fav_count { get; set; }
    }
}
