using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.IO;

using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace KDWechat.Common
{
    public class GetApiInformationcs
    {
        public static JsonMemberErrorResult GetMemberJsonResult<T>(string data)
               where T : JsonMemberErrorResult
        {
            if (!string.IsNullOrWhiteSpace(data))
            {
                var result = JsonConvert.DeserializeObject<JsonMemberErrorResult>(data);
                if (result != null && result.result == 0)
                    return result;
                return JsonConvert.DeserializeObject<T>(data);
            }
            return new JsonMemberErrorResult { result = -1, msg = "json解析失败" };
        }
        public static JQJsonErrorResult GetQJJsonResult<T>(string data)
  where T : JQJsonErrorResult
        {
            if (!string.IsNullOrWhiteSpace(data))
            {
                var result = JsonConvert.DeserializeObject<JQJsonErrorResult>(data);
                if (result != null && result.code == "fail")
                    return result;
                return JsonConvert.DeserializeObject<T>(data);
            }
            return new JQJsonErrorResult { code = "-1", detail = "json解析失败" };
        }

        public static JsonErrorResultScan GetJsonResultScan<T>(string data)
         where T : JsonErrorResultScan
        {
            if (!string.IsNullOrWhiteSpace(data))
            {
                var result = JsonConvert.DeserializeObject<JsonErrorResultScan>(data);
                if (result != null && result.result == 0)
                    return result;
                return JsonConvert.DeserializeObject<T>(data);
            }
            return new JsonErrorResultScan { result = -1, msg = "json解析失败" };
        }
    }
    #region

    public class JQJsonDataResult<T> : JQJsonErrorResult
    {
        /// <summary>
        /// 返回的对象
        /// </summary>
        public T data { get; set; }

    }


    public class JQJsonErrorResult
    {
        /// <summary>
        /// 执行结果 success-成功，fail-失败
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string detail { get; set; }
    }
    #endregion
    public class JsonMemberDataResult<T> : JsonMemberErrorResult
    {
        /// <summary>
        /// 返回的对象
        /// </summary>
        public T data { get; set; }

    }


    public class JsonDataResultScan<T> : JsonErrorResultScan
    {
        /// <summary>
        /// 返回的对象
        /// </summary>
        public T data { get; set; }

    }

    public class JsonErrorResultScan
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
        /// 返回的对象
        /// </summary>
        public string data { get; set; }

    }

    public class JsonMemberErrorResult
    {
        /// <summary>
        /// 执行结果 1-成功，0-失败
        /// </summary>
        public int result { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string msg { get; set; }
    }

    public class t_member
    {
        public int id { get; set; }//ID，0
        public string phone { get; set; }//手机号 必填
        public string pwd { get; set; }//密码（未加密
        public string salt { get; set; }//盐（可以给空
        public string nick_name { get; set; }//昵称（可以给空
        public string e_mail { get; set; }//email可空
        public string username { get; set; }//用户名 可空
        public Nullable<int> capital_star_id { get; set; }//凯德购物星ID 可空
        public System.DateTime add_time { get; set; }//添加时间，可空
        public System.DateTime login_time { get; set; }//最后登录时间，可空
        public string login_ip { get; set; }//登录IP 不可空
        public int status { get; set; } //账号状态 1为正常 0为禁用
        public int m_from { get; set; } //3为全民经纪人

        public string capital_member_card { get; set; }

        public string profile_token { get; set; }

    }

    public class MemberForApi
    {
        public int id { get; set; }
        public string phone { get; set; }
        public string pwd { get; set; }
        public string salt { get; set; }
        public string nick_name { get; set; }
        public string e_mail { get; set; }
        public string username { get; set; }
        public Nullable<int> capital_star_id { get; set; }
        public System.DateTime add_time { get; set; }
        public System.DateTime login_time { get; set; }
        public string login_ip { get; set; }
        public Nullable<int> sex { get; set; }
        public Nullable<int> age { get; set; }
        public Nullable<int> income { get; set; }
        public string nation { get; set; }
        public Nullable<System.DateTime> birth { get; set; }
        public string ID_No { get; set; }
        public Nullable<int> ID_Type { get; set; }
    }

}
