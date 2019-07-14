using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDWechat.BLL.Entity.JsonResult
{
    public class JsonApi
    {
        public static JsonErrorResult GetJsonResult<T>(string data)
              where T : JsonErrorResult
        {
            if (!string.IsNullOrWhiteSpace(data))
            {
                var result = JsonConvert.DeserializeObject<JsonErrorResult>(data);
                if (result != null && result.result == 0)
                    return result;
                return JsonConvert.DeserializeObject<T>(data);
            }
            return new JsonErrorResult { result = -1, msg = "json解析失败" };
        }
    }
}
