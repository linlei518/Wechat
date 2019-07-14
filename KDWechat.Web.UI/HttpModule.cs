using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Configuration;
using System.Xml;
using KDWechat.Common;

namespace KDWechat.Web.UI
{
    /// <summary>
    /// DTcms的HttpModule类
    /// </summary>
    public class HttpModule : System.Web.IHttpModule
    {
        /// <summary>
        /// 实现接口的Init方法
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(ReUrl_BeginRequest);
        }

        /// <summary>
        /// 实现接口的Dispose方法
        /// </summary>
        public void Dispose()
        { }


        public  string DropHTML(string Htmlstring)
        {
            if (string.IsNullOrEmpty(Htmlstring)) return "";
            //删除脚本  
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"alert[(][\s\S]*[)]", "", RegexOptions.IgnoreCase);  //alert的字符
            Htmlstring = Regex.Replace(Htmlstring, @"<iframe[^>]*?>.*?</iframe>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"iframe[\s\S]*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"javascript:[\s\t\r\n]*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"[+][\s\S]*[+]", "", RegexOptions.IgnoreCase);  //加号内的字符

            Htmlstring = Regex.Replace(Htmlstring, @"[""][\s\S]*[""]", "", RegexOptions.IgnoreCase);  //双引号内的字符
            //删除HTML  
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "xp_cmdshell", "", RegexOptions.IgnoreCase);

            //删除与数据库相关的词
            Htmlstring = Regex.Replace(Htmlstring, "select", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "insert", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "delete from", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "count''", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "drop table", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "truncate", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, " asc ", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, " mid ", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, " char ", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "xp_cmdshell", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "exec master", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "net localgroup administrators", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, " and ", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, " or ", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "net user", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "%20", "", RegexOptions.IgnoreCase);
            //
            Htmlstring = Regex.Replace(Htmlstring, "==", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "--", "", RegexOptions.IgnoreCase);

            Htmlstring=Htmlstring.Replace("<", "");
            Htmlstring = Htmlstring.Replace("'", "");
            Htmlstring = Htmlstring.Replace("‘", "");
            Htmlstring = Htmlstring.Replace("“", "");
            Htmlstring = Htmlstring.Replace("”", "");
            Htmlstring = Htmlstring.Replace(";", "");
            Htmlstring = Htmlstring.Replace("；", "");
           // Htmlstring = Htmlstring.Replace("/", "");
            Htmlstring = Htmlstring.Replace("\"", "");
            Htmlstring = Htmlstring.Replace(">", "");
            Htmlstring = Htmlstring.Replace("\r\n", "");
            return Htmlstring;
        }

        #region 过滤URL参数
        /// <summary>
        /// 页面请求事件处理 
        /// </summary>
        /// <param name="sender">事件的源</param>
        /// <param name="e">包含事件数据的 EventArgs</param>
        private void ReUrl_BeginRequest(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;
            string requestPath = context.Request.Path; //获得当前页面，包含目录
            string requestPage = requestPath.Substring(requestPath.LastIndexOf("/")); //获得当前页面，不包含目录
            string url=context.Request.Url.ToString();
            string Params = "";
            if (url.Contains("?"))
            {
                Params =context.Request.QueryString.ToString() ;//url.Substring(url.LastIndexOf('?'));
                 Params = DropHTML(Params);
            }

            if (requestPage.Contains(".aspx") || requestPage.Contains(".ashx"))
            {
                string new_params = "";
                string[] para = Params.Replace("?", "").Split(new char[]{'&'});
                if (para.Length>0)
                {
                    for (int i = 0; i < para.Length; i++)
                    {
                        if (para[i].Trim().Length>0)
                        {
                            string[] temp = para[i].Split(new char[] { '=' });
                            string s1 = temp[0];
                            string s2 ="";
                            
                            if (temp.Length==2)
                            {
                                s2 = temp[1];
                                int s3 = 0;
                                Int32.TryParse(s2, out s3);
                                if (s3<=0)
                                {
                                    s2 = HttpUtility.UrlDecode(s2);
                                    s2 = HttpUtility.UrlEncode(s2);
                                }
                            }
                            
                            if (new_params.Trim().Length>0)
                            {
                                new_params += "&"+s1+"="+s2;
                            }
                            else
                            {
                                new_params += s1 + "=" + s2;
                            }
                           
                        }
                       
                    }
                }
                if (new_params.Trim().Length>0)
                {
                    new_params = "?" + new_params;
                }
                context.RewritePath(requestPath + new_params);
            }
           
          

        }
        #endregion

    }



}