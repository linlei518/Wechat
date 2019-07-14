using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace KDWechat.Common
{
    /// <summary>
    /// 振华停车场，接口类
    /// </summary>
  public  class zh_helper
    {
        public static string Post(string url, string content)
        {
            //接口测试
            SortedDictionary<string, string> list = new SortedDictionary<string, string>();
            list.Add("user", "ktapi");
            list.Add("pwd", "01067A");

            var webReqst = WebRequest.Create(url) as HttpWebRequest;

            foreach (var item in list)
            {
                webReqst.Headers.Add(item.Key, item.Value);
            }


            webReqst.Method = "POST";
            webReqst.ContentType = "application/json;charset=utf-8";
            webReqst.ContentLength = content.Length;
            webReqst.Host = "220.160.111.114:9099";
            webReqst.Timeout = 30000;
            webReqst.ReadWriteTimeout = 30000;

            byte[] data = Encoding.GetEncoding("GB2312").GetBytes(content);
            Stream stream = webReqst.GetRequestStream();
            stream.Write(data, 0, data.Length);


            HttpWebResponse webResponse = (HttpWebResponse)webReqst.GetResponse();
            stream = webResponse.GetResponseStream();
            var reader = new StreamReader(stream, Encoding.UTF8);
            var html = reader.ReadToEnd();
            return html;
        }


        #region 3des加密

        /// <summary>
        /// 3des ecb模式加密
        /// </summary>
        /// <param name="aStrString">待加密的字符串</param>
        /// <param name="aStrKey">密钥</param>
        /// <param name="iv">加密矢量：只有在CBC解密模式下才适用</param>
        /// <param name="mode">运算模式</param>
        /// <returns>加密后的字符串</returns>
        public static string Encrypt3Des(string aStrString, string aStrKey, CipherMode mode = CipherMode.ECB,
            string iv = "12345678")
        {
            try
            {
                var des = new TripleDESCryptoServiceProvider
                {
                    Key = Encoding.UTF8.GetBytes(aStrKey),

                };

                des.IV = Encoding.UTF8.GetBytes(iv);

                var desEncrypt = des.CreateEncryptor();
                byte[] buffer = Encoding.UTF8.GetBytes(aStrString);
                return Convert.ToBase64String(desEncrypt.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        #endregion

        #region 3des解密

        /// <summary>
        /// des 解密
        /// </summary>
        /// <param name="aStrString">加密的字符串</param>
        /// <param name="aStrKey">密钥</param>
        /// <param name="iv">解密矢量：只有在CBC解密模式下才适用</param>
        /// <param name="mode">运算模式</param>
        /// <returns>解密的字符串</returns>
        public static string Decrypt3Des(string aStrString, string aStrKey, CipherMode mode = CipherMode.ECB,
            string iv = "12345678")
        {
            try
            {
                var des = new TripleDESCryptoServiceProvider
                {
                    Key = Encoding.UTF8.GetBytes(aStrKey),
                    Mode = mode,
                    Padding = PaddingMode.PKCS7
                };
                if (mode == CipherMode.CBC)
                {
                    des.IV = Encoding.UTF8.GetBytes(iv);
                }
                var desDecrypt = des.CreateDecryptor();
                var result = "";
                byte[] buffer = Convert.FromBase64String(aStrString);
                result = Encoding.UTF8.GetString(desDecrypt.TransformFinalBlock(buffer, 0, buffer.Length));
                return result;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        #endregion

    }
}
