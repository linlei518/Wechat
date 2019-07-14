using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.DAL;
using KDWechat.Common;

namespace KDWechat.Web
{
    public partial class InitTest : Web.UI.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string token;
            CookieContainer cc = ExecLogin(TextBox1.Text.Trim(), TextBox2.Text.Trim(), out token);
            var list = getImgandTxt(cc, token);
            if (list != null && list.Count == 50)
            {
                for (int i = 49; true; i += 50)
                {
                    var appendList = getImgandTxt(cc, token,i.ToString());
                    if (appendList != null && appendList.Count > 0)
                    {
                        appendList.ForEach(x=>list.Add(x));
                    }
                    if (appendList.Count != 50)
                    {
                        break;
                    }
                }
            }
            int isOk = GetAllNews(list);
            Response.Write(string.Format("<script>alert('更新成功，本次同步了{0}条数据。');parent.bombbox.closeBox();parent.location.reload();</script>", isOk));
        }



        #region 将图文素材复制到素材库
        private int GetAllNews(List<WeiXinNews> list)
        {
            int updCount = 0;
            foreach (var x in list)
            {
                if (BLL.Chats.wx_news_materials.CheckContains(x.content_url))
                    continue;
                updCount++;
                KDWechat.DAL.t_wx_news_materials model = new KDWechat.DAL.t_wx_news_materials();
                model.title = x.title;
                model.wx_id = wx_id;
                model.wx_og_id = wx_og_id;
                model.author = x.author;
                model.channel_id = x.multi_item.Count>1?(int)ResponseNewsType.多图文:(int)ResponseNewsType.单图文;
                model.contents = x.digest;
                model.cover_img = x.img_url;
                model.create_time = Utils.GetWeChatDate(x.create_time);
                model.group_id = 0;
                model.is_public = (int)is_publicMode.否;
                model.link_url = x.content_url;
                model.par_id = 0;
                model.source_url = x.content_url;
                model.status = (int)Status.正常;
                model.summary = x.digest;
                model.template_id = 0;
                model.u_id = u_id;
                model.push_type = "article";
                if (!string.IsNullOrEmpty(x.content_url))
                {
                    model.push_type = "link";
                }
                model = BLL.Chats.wx_news_materials.Add(model);
                if (model!=null)
                {
                    int first = 0;
                    foreach (var y in x.multi_item)
                    {
                        if (first == 0)
                        {
                            first++;
                            continue;
                        }
                        #region 添加子级图文
                        var child = new KDWechat.DAL.t_wx_news_materials()
                        {
                            title = y.title,
                            wx_id = wx_id,
                            wx_og_id = wx_og_id,
                            author = y.author,
                            channel_id = (int)ResponseNewsType.多图文,
                            contents = y.digest,
                            cover_img = y.cover,
                            create_time = DateTime.Now,
                            group_id = 0,
                            is_public = (int)is_publicMode.否,
                            link_url = y.content_url,
                            par_id = model.id,
                            source_url = y.content_url,
                            status = (int)Status.正常,
                            summary = y.digest,
                            template_id = 0,
                            u_id = u_id
                        };
                        child.push_type = "article";
                        if (!string.IsNullOrEmpty(x.content_url))
                        {
                            child.push_type = "link";
                        }
                        if (first < 3)
                        {
                            model.content_html += " <div class=\"info\">";
                            model.content_html += "<div class=\"img\"> <span><img src=\"" + child.cover_img + "\" alt=\"\">  </span></div>";
                            model.content_html += "<div class=\"title\"> <h1>" + child.title + "</h1> </div> </div>";

                            model.multi_html += "<div class=\"infoField\"><div class=\"img\"> <span><img class=\"cover\" src=\"" + child.cover_img + "\" > </span> </div><div class=\"title\"><h1>" + child.title + "</h1></div> </div>";
                        }
                        first++;
                        BLL.Chats.wx_news_materials.Add(child);
                        #endregion
                    }
                    #region 生成二维码
                    CreateNewsQrCode(model.id);
                    #endregion
                    BLL.Chats.wx_news_materials.Update(model);
                }
               
               
            }
            return updCount;
        }
        #endregion

        #region 获取cookie以及token
        public CookieContainer ExecLogin(string name, string pass, out string token)//登陆微信公众平台函数
        {
            bool result = false;
            token = string.Empty;
            string password = GetMd5Str32(pass).ToUpper();
            string padata = "username=" + name + "&pwd=" + password + "&imgcode=&f=json";
            string url = "https://mp.weixin.qq.com/cgi-bin/login?lang=zh_CN ";//请求登录的URL
            CookieContainer cc = new CookieContainer();//接收缓存
            //cc.Add(new Cookie())
            try
            {

                byte[] byteArray = Encoding.UTF8.GetBytes(padata); // 转化
                HttpWebRequest webRequest2 = (HttpWebRequest)WebRequest.Create(url);  //新建一个WebRequest对象用来请求或者响应url
                webRequest2.CookieContainer = cc;                                      //保存cookie  
                webRequest2.Method = "POST";                                          //请求方式是POST
                webRequest2.ContentType = "application/x-www-form-urlencoded";       //请求的内容格式为application/x-www-form-urlencoded
                webRequest2.ContentLength = byteArray.Length;
                webRequest2.Referer = "https://mp.weixin.qq.com/";
                Stream newStream = webRequest2.GetRequestStream();           //返回用于将数据写入 Internet 资源的 Stream。
                // Send the data.
                newStream.Write(byteArray, 0, byteArray.Length);    //写入参数
                newStream.Close();
                HttpWebResponse response2 = (HttpWebResponse)webRequest2.GetResponse();
                StreamReader sr2 = new StreamReader(response2.GetResponseStream(), Encoding.Default);
                string text2 = sr2.ReadToEnd();
                //此处用到了newtonsoft来序列化
                WeiXinRetInfo retinfo = Newtonsoft.Json.JsonConvert.DeserializeObject<WeiXinRetInfo>(text2);
                if (retinfo.base_resp.err_msg.Length > 0)
                {
                    if (retinfo.base_resp.err_msg.Contains("ok"))
                    {
                        token = retinfo.redirect_url.Split(new char[] { '&' })[2].Split(new char[] { '=' })[1].ToString();//取得令
                        result = true;
                    }
                    else
                    {

                        result = false;
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.StackTrace);
            }
            return cc;
        }
        #endregion

        #region 获取所有素材
        public List<WeiXinNews> getImgandTxt(CookieContainer cc,string token2,string begin="0",string count="50")
        {

            CookieContainer cookie = cc;
            string token = token2;
            //cookie = WeiXinLogin.LoginInfo.LoginCookie;//取得cookie
            //token = WeiXinLogin.LoginInfo.Token;//取得token

            /* 1.token此参数为上面的token 2.pagesize此参数为每一页显示的记录条数

            3.pageid为当前的页数，4.groupid为微信公众平台的用户分组的组id*/
            string Url = "https://mp.weixin.qq.com/cgi-bin/appmsg?begin="+begin+"&count="+count+"&t=media/appmsg_list&type=10&action=list&token=" + token + "&lang=zh_CN";
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(Url);//Url为获取用户信息的链接
            webRequest.CookieContainer = cookie;
            webRequest.ContentType = "text/html; charset=UTF-8";
            webRequest.Method = "GET";
            webRequest.UserAgent = "Mozilla/5.0 (Windows NT 5.1; rv:2.0.1) Gecko/20100101 Firefox/4.0.1";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
            string text = sr.ReadToEnd();
            MatchCollection mc;
            Regex Rex = new Regex(@"(?<=\{""item"":).+(?=,""file_cnt"":)");
            mc = Rex.Matches(text);
            List<WeiXinNews> ImgandTxt = new List<WeiXinNews>();
            if (mc.Count != 0)
            {
                for (int i = 0; i < mc.Count; i++)
                {
                    ImgandTxt = JsonConvert.DeserializeObject<List<WeiXinNews>>(mc[i].Value);
                }
            }
            return ImgandTxt;

        }
        #endregion

        #region MD5算法
        string GetMd5Str32(string str) //MD5摘要算法
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            // Convert the input string to a byte array and compute the hash.  
            char[] temp = str.ToCharArray();
            byte[] buf = new byte[temp.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                buf[i] = (byte)temp[i];
            }
            byte[] data = md5Hasher.ComputeHash(buf);
            // Create a new Stringbuilder to collect the bytes  
            // and create a string.  
            StringBuilder sBuilder = new StringBuilder();
            // Loop through each byte of the hashed data   
            // and format each one as a hexadecimal string.  
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            // Return the hexadecimal string.  
            return sBuilder.ToString();
        }
        #endregion


    }

    #region 登陆相关的实体类
    public class WeiXinRetInfo//保存登录失败微信公众平台网页返回的信息
    {
        public string redirect_url { get; set; }
        public Base_Resp base_resp { get; set; }
    }

    public class Base_Resp
    {
        public string ret { get; set; }
        public string err_msg { get; set; }
        public string err_code { get; set; }
    }
    #endregion

    #region 图文实体类
    public class WeiXinNews
    {
        public int seq { get; set; }
        public int app_id { get; set; }
        public int file_id { get; set; }
        public string title { get; set; }
        public string digest { get; set; }
        public string create_time { get; set; }
        public List<Multi_Item> multi_item { get; set; }
        public string content_url { get; set; }
        public string img_url { get; set; }
        public string author { get; set; }
        public int show_cover_pic { get; set; }
    }

    public class Multi_Item
    {
        public int seq { get; set; }
        public string cover { get; set; }
        public string title { get; set; }
        public string digest { get; set; }
        public string content_url { get; set; }
        public int file_id { get; set; }
        public string source_url { get; set; }
        public string author { get; set; }
        public string show_cover_pic { get; set; }
    }
    #endregion

}