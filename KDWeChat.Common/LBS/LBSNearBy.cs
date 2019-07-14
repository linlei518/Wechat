using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiDuMapAPI
{
    public class LBSNearBy
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 分页参数，当前页返回数量
        /// </summary>
        public int size { get; set; }
        /// <summary>
        /// 分页参数，所有召回数量
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// poi结果列表
        /// </summary>
        //public ArrayList contents { get; set; }
        public List<LBSNearByContents> contents { get; set; }
    }
}
