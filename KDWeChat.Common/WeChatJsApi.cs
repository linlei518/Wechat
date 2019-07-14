using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KDWechat.Common
{
    public class WeChatJsApi
    {
        /// <summary>
        /// 获取jsapi的Token
        /// </summary>
        /// <param name="accessToken">公众号的accessToken</param>
        /// <returns>jsApiTicket。如果出现错误则返回null</returns>
        public static string GetJsApiTicket(string accessToken)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token="+accessToken+"&type=jsapi";
            WebClient wc = new WebClient();
            string result = wc.DownloadString(url);
            wc.Dispose();
            try {
                var jsonResult = JsonConvert.DeserializeObject<JsApiTicketJsonResult>(result);
                if (jsonResult.errmsg == "ok")
                    return jsonResult.ticket;
                else
                    return null;
            }
            catch {
                return null;
            }
        }

        /// <summary>
        /// 微信sha1操作
        /// </summary>
        /// <param name="input">需要Sha1的值</param>
        /// <returns>sha1之后的值</returns>
        public static string WeChatSha1(string input)
        {
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var sha1Arr = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder enText = new StringBuilder();
            foreach (var b in sha1Arr)
            {
                enText.AppendFormat("{0:x2}", b);
            }
            return enText.ToString();
        }


        /// <summary>
        /// 获取签名
        /// </summary>
        /// <param name="nonceStr">随机字符串</param>
        /// <param name="jsapi_ticket">jsapi_ticket,可通过GetJsApiTicket()方法获取</param>
        /// <param name="timeStamp">时间戳，当前时间减去1970-1-1，转换为秒</param>
        /// <param name="url">申请验证的地址全称（例如：http://abc.companycn.net/login.aspx）</param>
        /// <returns></returns>
        public static string GetSignature(string nonceStr,string jsapi_ticket,string timeStamp,string url)
        {
            string oriString = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}",jsapi_ticket,nonceStr,timeStamp,url);
            return WeChatSha1(oriString);
        }
    }

    class JsApiTicketJsonResult
    {
        public int errcode { get; set; }
        public String errmsg { get; set; }
        public String ticket { get; set; }
        public int expires_in { get; set; }
    }
}
