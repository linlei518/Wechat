using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;
using KDWechat.Common.Config;

namespace KDWechat.Web.wxpage
{
    public partial class news_template : System.Web.UI.Page
    {

        protected string openId { get { return RequestHelper.GetQueryString("openId"); } }
        protected int id { get { return RequestHelper.GetQueryInt("id", 0); } }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //取出微信


                //取出图文
                DAL.t_wx_news_materials model = BLL.Chats.wx_news_materials.GetModel(id);
                if (model == null)
                {
                    Response.Write("请勿非法访问");
                    return;
                }

                KDWechat.DAL.t_wx_wechats wechat = BLL.Chats.wx_wechats.GetWeChatByID(model.wx_id);
                if (wechat == null)
                {
                    wechat = new DAL.t_wx_wechats();
                    wechat.header_pic = "";
                    wechat.wx_pb_name = "";
                    wechat.app_id = "wxe8c2d892dea7ec85"; //如果是总部图文的话， 给一个错的
                    wechat.country = "";
                    //Response.Write("请勿非法访问");
                    //return;
                }

                //判断图文的所属的微信号是否跟传过来的微信号是同一个
                //if (model.wx_id!=wechat.id) 
                //{
                //    Response.Write("请勿非法访问");
                //    return;
                //}

                DAL.t_wx_templates template = null;

                //判断图文自己是否选择了模板


                //读取配置文件，判断是否开启了使用默认模板
                Common.Config.wechatconfig _config = new BLL.Config.wechat_config().loadConfig();
                if (_config.is_use_default_template == "1")
                {
                    //取出系统默认的模板id
                    template = BLL.Chats.wx_templates.GetDefaultModel();
                }
                else
                {
                    if (model.template_id > 0)
                    {
                        template = BLL.Chats.wx_templates.GetModel(model.template_id, 1);
                        if (template == null)
                        {
                            //该微信号没有设置默认，取出系统默认
                            template = BLL.Chats.wx_templates.GetDefaultModel();
                        }
                    }
                    else
                    {
                        //读取微信号自己设置的默认模板
                        template = BLL.Chats.wx_templates.GetWXDefaultModel(model.wx_id);
                        if (template == null)
                        {
                            //该微信号没有设置默认，取出系统默认
                            template = BLL.Chats.wx_templates.GetDefaultModel();
                        }
                    }

                }

                if (template != null)
                {
                    //取出内容
                    string _html = template.contents;
                    if (template.cate_id == 0)
                    {
                        //系统模板
                        _html = ReadFile(Server.MapPath(template.file_path), System.Text.Encoding.UTF8);
                    }

                    if (_html.Trim().Length > 0)
                    {
                        BLL.Config.wechat_config bll = new BLL.Config.wechat_config();
                        wechatconfig wchatConfig = bll.loadConfig();

                        #region 构造分享代码

                        var timestamp = ((int)((DateTime.Now - DateTime.Parse("1970-1-1")).TotalSeconds)).ToString();
                        var nonceStr = Common.Utils.Number(6);
                        var jsapi_ticket = BLL.Chats.wx_wechats.GetJsTicket(wechat.app_id); 
                        var signature = Common.WeChatJsApi.GetSignature(nonceStr, jsapi_ticket, timestamp, Request.Url.ToString());
                        string app_id = "";
                        if (wechat!=null)
                        {
                            app_id = wechat.app_id;
                        }

                        string share_html = "<script src=\"http://res.wx.qq.com/open/js/jweixin-1.0.0.js\"></script>";
                        share_html += "<script src=\"/wxpage/js/s.js?t=354547955828498&app_id=" + app_id + "&timestamp=" + timestamp + "&nonceStr=" + nonceStr + "&signature=" + signature + "&c=" + Common.Utils.DropHTML(model.summary, 60) + "" + (Common.RequestHelper.GetQueryInt("wx_id", 0) == 0 ? "&wx=" + model.wx_id + "" : "") + "" + (Common.RequestHelper.GetQueryString("wx_og_id") == "" ? "&og=" + model.wx_og_id + "" : "") + "\"></script>";
                        share_html += "<script src=\"/Scripts/SDKShare.js\"></script>";
                        string temp = @"<script>
                            var weixinShare = new WeixinShare({
                                            title: window.document.title,
                                            content: '{content}',
                                            linkUrl: window.location.href,
                                            imgUrl: '{img}',
                                            appId: '{appId}',
                                            debug: false,
                                            timestamp: '{timestamp}',
                                            nonceStr: '{nonceStr}',
                                            signature: '{signature}',
                                            shareAppMessageSuccessCallback: function () {
                                               CeilingIsGodShare.nothings();
                                            },
                                            shareTimelineSuccessCallback: function () {
                                              CeilingIsGodShare.nothings();
                                            },
                                            shareQQSuccessCallback: function () {
                                               CeilingIsGodShare.nothings();
                                            },
                                            shareWeiboSuccessCallback: function () {
                                                CeilingIsGodShare.nothings();
                                            }

                                        }); 
                                        </script> ";

                        temp = temp.Replace("{appId}", app_id).Replace("{nonceStr}", nonceStr).Replace("{timestamp}", timestamp).Replace("{signature}", signature).Replace("{img}", wchatConfig.domain + model.cover_img).Replace("{content}",model.summary);
                        share_html += temp;
                        share_html += "<script src=\"/wxpage/js/h.js?t=505160032019047" + (Common.RequestHelper.GetQueryInt("wx_id", 0) == 0 ? "&wx=" + model.wx_id + "" : "") + "" + (Common.RequestHelper.GetQueryString("wx_og_id") == "" ? "&og=" + model.wx_og_id + "" : "") + "\"></script>";
                        #endregion

                        #region 构造点赞代码
                        bool is_good_event = false;
                        string good_event = "<script src=\"/wxpage/js/c.js?t=958809765340067" + (Common.RequestHelper.GetQueryInt("wx_id", 0) == 0 ? "&wx=" + model.wx_id + "" : "") + "" + (Common.RequestHelper.GetQueryString("wx_og_id") == "" ? "&og=" + model.wx_og_id + "" : "") + "\"></script>";
                        if (_html.Contains("$good_event$"))
                        {
                            _html = _html.Replace("$good_event$", "good_event(this)");
                            is_good_event = true;
                        }

                        #endregion


                        //获取总浏览数和点赞数
                        int read_number = model.view_count;

                        int good_number = model.click_count;

                        //检测用户是否赞过
                        Common.Statistics.st_statistics click_model = BLL.Statistics.st_statistics.GetModel("t_st_graohic_click", openId, id, Utils.GetUserIp());
                        if (click_model != null)
                        {
                            _html = _html.Replace("$zan_class$", " span_zan");
                        }
                        else
                        {
                            _html = _html.Replace("$zan_class$", "");
                        }
                        _html = _html.Replace("$title$", model.title);
                        _html = _html.Replace("$erweima$",wechat.qrcode_img);
                        _html = _html.Replace("$contents$", model.contents);
                        DateTime date = (DateTime)model.create_time;
                        _html = _html.Replace("$year$", date.Year.ToString());
                        _html = _html.Replace("$month$", date.Month.ToString());
                        _html = _html.Replace("$day$", date.Day.ToString());
                        _html = _html.Replace("$remark$", model.summary);
                        _html = _html.Replace("$news_img$", model.cover_img);
                        _html = _html.Replace("$news_author$", model.author);
                        _html = _html.Replace("$wechat_head_img$", wechat.header_pic);
                        _html = _html.Replace("$wechat_name$", wechat.wx_pb_name);
                        _html = _html.Replace("$read_number$", read_number.ToString());
                        _html = _html.Replace("$good_number$", "<label id=\"good_event\">" + good_number + "</label>");

                        string other_html = share_html;
                        if (is_good_event)
                        {
                            other_html += good_event;
                        }
                        _html = _html.Replace("</body>", other_html + "</body>");
                        if (string.IsNullOrEmpty(model.source_url))
                        {
                            _html = _html.Replace("$original_link$", "original_link_reset");
                            string originalText = GetOriginalText(_html);
                            if (originalText.Trim().Length > 0)
                            {
                                _html = _html.Replace("original_link_reset", "javascript:");
                                _html = _html.Replace(originalText, "");
                            }

                            // _html = Regex.Replace(_html, @"<a[\s\S]*?href=""original_link_reset""[\s\S]*?>[\s\t\r\n]*?[\s\t\r\n]*(?<AUrl>[\s\S]*?)</a>", "", RegexOptions.IgnoreCase);
                        }
                        else
                        {
                            _html = _html.Replace("$original_link$", model.source_url);
                        }




                        Response.Write(_html);
                    }


                }
                else
                {
                    Response.Write("模板出错了!");
                }

            }
        }

        /// <summary>
        /// 读文件
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <returns></returns>
        protected string ReadFile(string Path, Encoding code)
        {
            string s = "";
            if (!System.IO.File.Exists(Path))
                s = "不存在相应的目录:" + Path;
            else
            {
                StreamReader f2 = new StreamReader(Path, code);
                s = f2.ReadToEnd();
                f2.Close();
                f2.Dispose();
            }
            return s;
        }

        /// <summary>
        /// 获取原文地址的
        /// </summary>
        /// <param name="sHtmlText"></param>
        /// <returns></returns>
        protected string GetOriginalText(string sHtmlText)
        {
            string name = "";
            Regex regImg = new Regex(@"<a[\s\S]*?href=""original_link_reset""[\s\S]*?>[\s\t\r\n]*?[\s\t\r\n]*(?<AUrl>[\s\S]*?)</a>", RegexOptions.IgnoreCase);
            //返回style

            // 搜索匹配的字符串
            MatchCollection matches = regImg.Matches(sHtmlText);
            int i = 0;
            string[] sUrlList = new string[matches.Count];
            if (sUrlList.Length > 0)
            {
                // 取得匹配项列表
                foreach (Match match in matches)
                    sUrlList[i++] = match.Groups["AUrl"].Value;
                name = sUrlList[0];

                return name;
            }
            else
            {
                return "";
            }
        }
    }
}