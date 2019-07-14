using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

 


namespace KDWechat.Common
{

    /// <summary>
    /// 接口请求类
    /// </summary>
    public class KDHttpRequest 
    {



        /// <summary>
        /// 将json数据转为对象
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="returnText">json数据</param>
        /// <returns></returns>
        public static T GetResult<T>(string returnText)
        {
            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                T result = js.Deserialize<T>(returnText);
                return result;
            }
            catch (Exception)
            {
                return default(T);
            }



        }


        /// <summary>
        /// 发起常规Post请求
        /// </summary>
        /// <typeparam name="T">返回数据类型（Json对应的实体）</typeparam>
        /// <param name="url">请求Url,包含参数</param>
        /// <returns></returns>
        public static T GetJsonByPost<T>(string url)
        {
            string returnText = HttpPost(url);
            var result = GetResult<T>(returnText);
            return result;
        }

        /// <summary>
        /// 发起常规Get请求,
        /// </summary>
        /// <typeparam name="T">返回数据类型（Json对应的实体）</typeparam>
        /// <param name="url">请求Url</param>
        /// <param name="postDataStr">接口参数</param>
        /// <returns></returns>
        public static T GetJsonByGet<T>(string url, string postDataStr = "")
        {
            string returnText = HttpGet(url, postDataStr);
            var result = GetResult<T>(returnText);
            return result;
        }



        /// <summary>
        /// 发起HttpClient请求,获取接口返回的json数据
        /// </summary>
        /// <typeparam name="T">返回数据类型（Json对应的实体）</typeparam>
        /// <typeparam name="T2">接口所需要的参数对象类型</typeparam>
        /// <param name="url">接口地址</param>
        /// <param name="obj">接口参数对象</param>
        /// <returns></returns>
        public static T GetJsonByPostAsync<T, T2>(string url, T2 obj)
        {
            string returnText = PostAsync<T2>(url, obj);
            var result = GetResult<T>(returnText);
            return result;
        }

        /// <summary>
        /// 发起HttpClient请求,获取接口返回的json数据
        /// </summary>
        /// <typeparam name="T">返回数据类型（Json对应的实体）</typeparam>
        /// <param name="url">接口Url</param>
        /// <param name="postDataStr">json参数</param>
        /// <returns></returns>
        public static T GetJsonByPostAsync<T>(string url, string jsonObj)
        {
            string returnText = PostAsync(url, jsonObj);
            var result = GetResult<T>(returnText);
            return result;
        }




        /// <summary>
        /// 通过GET方式发送数据
        /// </summary>
        /// <param name="Url">接口地址</param>
        /// <param name="postDataStr">参数</param>
        /// <returns></returns>
        public static string HttpGet(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);


            //存储 Internet 资源的凭据
            System.Net.CredentialCache myCache = new System.Net.CredentialCache();

            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            string retString = "";
            request.Timeout = 60000;
            HttpWebResponse response = null;
            Stream myResponseStream = null;
            StreamReader myStreamReader = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                myResponseStream = response.GetResponseStream();
                myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                retString = myStreamReader.ReadToEnd();

            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                retString = sr.ReadToEnd();

            }
            finally
            {
                if (myStreamReader != null)
                    myStreamReader.Close();

                if (myResponseStream != null)
                    myResponseStream.Close();

                if (response != null)
                    response.Close();
            }


            return retString;
        }


        /// <summary>
        /// Post提交数据
        /// </summary>
        /// <param name="url">接口地址，地址后面可以追加参数，如:http://www.baidu.com/api.aspx?string postDataStr = "retItemSort=EWH&retModel=储热式电热水器&retCode=sn1234</param>
        /// <returns></returns>
        public static string HttpPost(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = new CookieContainer();
            CookieContainer cookie = request.CookieContainer;//如果用不到Cookie，删去即可  

            //以下是发送的http头，随便加，其中referer挺重要的，有些网站会根据这个来反盗链  
            // request.Referer = "http://localhost/index.aspx";
            request.Accept = "Accept:text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.Headers["Accept-Language"] = "zh-CN,zh;q=0.";
            request.Headers["Accept-Charset"] = "GBK,utf-8;q=0.7,*;q=0.3";
            //request.Headers.Add(HttpRequestHeader.Authorization, GetAccessToken());//安全验证Te
            request.UserAgent = "User-Agent:Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.835.202 Safari/535.1";
            request.KeepAlive = false;
            //上面的http头看情况而定，但是下面俩必须加  
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";

            request.Timeout = 60000;
            Encoding encoding = Encoding.UTF8;//根据网站的编码自定义  
            string postDataStr="";
            byte[] postData = encoding.GetBytes(postDataStr);//postDataStr即为发送的数据，
            request.ContentLength = postData.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(postData, 0, postData.Length);

            string retString = "";


            HttpWebResponse response = null;
            Stream myResponseStream = null;
            StreamReader myStreamReader = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                //如果http头中接受gzip的话，这里就要判断是否为有压缩，有的话，直接解压缩即可  
                if (response.Headers["Content-Encoding"] != null && response.Headers["Content-Encoding"].ToLower().Contains("gzip"))
                {
                    responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                }
                StreamReader streamReader = new StreamReader(responseStream, encoding);
                retString = streamReader.ReadToEnd();

            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                retString = sr.ReadToEnd();
            }
            finally
            {
                if (myStreamReader != null)
                    myStreamReader.Close();

                if (myResponseStream != null)
                    myResponseStream.Close();

                if (response != null)
                    response.Close();
            }





            return retString;
        }




        /// <summary>
        /// post方式发送数据
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="postdata">参数数据</param>
        /// <returns>返回的结果</returns>
        public static string HttpPost2(string url, string postdata)
        {
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)WebRequest.Create(url);

            request.Method = WebRequestMethods.Http.Post;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Timeout = 1000 * 60 * 3;
            request.ReadWriteTimeout = 1000 * 60 * 3;
            request.KeepAlive = false;

            using (StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.UTF8))
            {
                writer.Write(postdata);
            }

            string result = "";
            HttpWebResponse response = null;
            try
            {
                response = (System.Net.HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        result = reader.ReadToEnd();
                    }
            }
            catch (WebException e)
            {
                response = (HttpWebResponse)e.Response;
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd();
            }
            finally
            {
                if (response != null)
                    response.Close();
            }

            return result;
        }

        /// <summary>
        /// post请求数据（注意，此方法只支持json传参）
        /// </summary>
        /// <param name="url">接口地址</param>
        /// <param name="obj">参数：对象，自动转成json格式</param>
        /// <returns>返回json格式的字符串</returns>
        public static string PostAsync<T>(string url, T obj)
        {
            string result = "";
            try
            {
                Uri postUrl = new Uri(url);

                System.Web.Script.Serialization.JavaScriptSerializer jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                string JsonStr = jsSerializer.Serialize(obj);
                Encoding encoding = Encoding.UTF8;
                HttpContent httpContent = new StringContent(JsonStr, encoding);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(0, 0, 60);
                result = httpClient.PostAsync(url, httpContent).Result.Content.ReadAsStringAsync().Result;

                //WebClient client = new WebClient();
                //client.Headers["content-type"] = "application/json";
                //if (is_Auth)
                //{
                //    KDWechat.API.Model.Auth authModel = new BLL.Config.auth_config().loadConfig();
                //    authModel = GetAuthConfig(authModel);
                //    client.Headers.Add("USERKEY", authModel.ClientId);
                //    client.Headers.Add("TOKEN", authModel.Token);
                //}
                //byte[] b = client.UploadData(postUrl, "POST", Encoding.UTF8.GetBytes(JsonStr));
                //result = System.Text.Encoding.UTF8.GetString(b);


            }
            catch (Exception)
            {

                result = "接口调用出错！";
            }
            return result;
        }

        /// <summary>
        /// post请求数据（注意，此方法只支持json传参）
        /// </summary>
        /// <param name="url">接口地址</param>
        /// <param name="requestJson">json参数</param>
        /// <returns>返回json格式的字符串</returns>
        public static string PostAsync(string url, string requestJson)
        {
            string result = "";
            try
            {
                Uri postUrl = new Uri(url);

                HttpContent httpContent = new StringContent(requestJson);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");



                var httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(0, 0, 60);
                result = httpClient.PostAsync(url, httpContent).Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception)
            {

                result = "接口调用出错！";
            }
            return result;
        }
    }
}
