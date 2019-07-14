using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDWechat.BLL.Entity.JsonResult
{
    public class JsonDataResult<T>:JsonErrorResult
    {
        /// <summary>
        /// 返回的对象
        /// </summary>
        public T data { get; set; }

    }
}
