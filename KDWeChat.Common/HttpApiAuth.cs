using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDWechat.Common
{
    public class HttpApiAuth
    {

        public static string GetSignature(string timestamp, string nonce, string token = null)
        {
            var arr = new[] { token, timestamp, nonce }.OrderBy(z => z).ToArray();
            var arrString = string.Join("", arr);
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var sha1Arr = sha1.ComputeHash(Encoding.UTF8.GetBytes(arrString));
            StringBuilder enText = new StringBuilder();
            foreach (var b in sha1Arr)
            {
                enText.AppendFormat("{0:x2}", b);
            }
            return enText.ToString();
        }

        public static bool Auth(string token, DateTime stramp, string nonce, string sign)
        {
            bool isAuth = false;
            var totalSeconds = (DateTime.Now - stramp).TotalSeconds;
            if (totalSeconds > 0 && totalSeconds < 15)
            {
                var vertifyTime = stramp.Date.AddHours(stramp.Hour).AddMinutes((stramp.Minute / 10) * 10 + 7).AddSeconds(stramp.Second).AddMilliseconds(stramp.Millisecond).ToString("yyyyMMddHHmmssfff");
                var vertSign = GetSignature(vertifyTime, nonce, token);
                if (sign == vertSign)
                    isAuth = true;
            }
            return isAuth;
        }

        public static DateTime GetDateStramp(string stramp)
        {
            DateTime dt = DateTime.Parse("1970-1-1");
            try
            {
                dt = DateTime.ParseExact(stramp, "yyyyMMddHHmmssfff", null);
            }
            catch
            {}
            return dt;
        }
    }
}
