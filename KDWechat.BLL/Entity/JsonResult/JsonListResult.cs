using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDWechat.BLL.Entity.JsonResult
{
    public class JsonListResult<T>
    {
        /// <summary>
        /// 执行结果 1-成功，0-失败
        /// </summary>
        public int result { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 返回的对象集合
        /// </summary>
        public List<T> data { get; set; }

    }
}
