using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web.Configuration;
using Senparc.Weixin.MP.Agent;
using Senparc.Weixin.Context;
using Senparc.Weixin.MP.MessageHandlers;
using Senparc.Weixin.MP.Helpers;
using KDWechat.DAL;
using System.Linq;
using KDWechat.Common;
using System.Collections.Generic;
using BaiDuMapAPI;
using Newtonsoft.Json;
using System.Xml.Linq;
using KDWechat.BLL.Users;
using System.Linq.Expressions;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using System.Text.RegularExpressions;
using KDWeChat.Common;
using System.Web;

namespace KDWechat.MP.CustomMessageHandler
{
    /// <summary>
    /// 自定义MessageHandler
    /// 把MessageHandler作为基类，重写对应请求的处理方法
    /// </summary>
    public partial class CustomMessageHandler : MessageHandler<CustomMessageContext>
    {
        /*
         * 重要提示：v1.5起，MessageHandler提供了一个DefaultResponseMessage的抽象方法，
         * DefaultResponseMessage必须在子类中重写，用于返回没有处理过的消息类型（也可以用于默认消息，如帮助信息等）；
         * 其中所有原OnXX的抽象方法已经都改为虚方法，可以不必每个都重写。若不重写，默认返回DefaultResponseMessage方法中的结果。
         */


#if DEBUG
        string agentUrl = "http://localhost:12222/App/Weixin/4";
        string agentToken = "27C455F496044A87";
        string wiweihiKey = "CNadjJuWzyX5bz5Gn+/XoyqiqMa5DjXQ";
#else
        //下面的Url和Token可以用其他平台的消息，或者到www.weiweihi.com注册微信用户，将自动在“微信营销工具”下得到
        private string agentUrl = WebConfigurationManager.AppSettings["WeixinAgentUrl"];//这里使用了www.weiweihi.com微信自动托管平台
        private string agentToken = WebConfigurationManager.AppSettings["WeixinAgentToken"];//Token
        private string wiweihiKey = WebConfigurationManager.AppSettings["WeixinAgentWeiweihiKey"];//WeiweihiKey专门用于对接www.Weiweihi.com平台，获取方式见：http://www.weiweihi.com/ApiDocuments/Item/25#51
#endif

        //public KDWechat.DAL.t_wx_wechats wechat = null;

        public CustomMessageHandler(Stream inputStream, PostModel postModel, int maxRecordCount = 0)
            : base(inputStream, postModel, maxRecordCount)
        {

            //public CustomMessageHandler(Stream inputStream, int maxRecordCount = 0)
            //    : base(inputStream, maxRecordCount)
            //{
            //wechat = KDWechat.BLL.Chats.wx_wechats.GetWeChatByogID(RequestMessage.ToUserName);
            //这里设置仅用于测试，实际开发可以在外部更全局的地方设置，
            //比如MessageHandler<MessageContext>.GlobalWeixinContext.ExpireMinutes = 3。
            WeixinContext.ExpireMinutes = 3;
        }

        public override void OnExecuting()
        {
            //测试MessageContext.StorageData
            if (CurrentMessageContext.StorageData == null)
            {
                CurrentMessageContext.StorageData = 0;
            }
            //this.FinalResponseDocument = null;
            base.OnExecuting();
        }

        public override void OnExecuted()
        {
            base.OnExecuted();

            CurrentMessageContext.StorageData = ((int)CurrentMessageContext.StorageData) + 1;
        }


        private string getUrl(string domain, string templat_path, int id, string openId, string wx_og_id)
        {
            string URL = "";
            if (templat_path.Length > 0)
            {
                if (templat_path.Contains("http"))
                {
                    URL = templat_path + "?id=" + id + "&openId=" + openId + "&wx_og_id=" + wx_og_id;
                }
                else if (templat_path.Substring(0, 1) == "/")
                {
                    URL = domain + templat_path + "?id=" + id + "&openId=" + openId + "&wx_og_id=" + wx_og_id;
                }
                else
                {
                    URL = domain + "/" + templat_path + "?id=" + id + "&openId=" + openId + "&wx_og_id=" + wx_og_id;
                }
            }
            return URL;
        }
        //构造模块链接
        private string getMDUrl(string domain, string _checkUrl, string wx_og_id, string openId)
        {
            string URL = "";
            if (_checkUrl.Length > 0)
            {
                if (_checkUrl.Contains("http"))
                {
                    if (_checkUrl.Contains("?"))
                    {
                        URL = _checkUrl + "&wx_og_id=" + wx_og_id + "&openId=" + openId;
                    }
                    else
                    {
                        URL = _checkUrl + "?wx_og_id=" + wx_og_id + "&openId=" + openId;
                    }
                }
                else if (_checkUrl.Substring(0, 1) == "/")
                {
                    if (_checkUrl.Contains("?"))
                    {
                        URL = domain + _checkUrl + "&wx_og_id=" + wx_og_id + "&openId=" + openId;
                    }
                    else
                    {
                        URL = domain + _checkUrl + "?wx_og_id=" + wx_og_id + "&openId=" + openId;
                    }
                }
                else
                {
                    if (_checkUrl.Contains("?"))
                    {
                        URL = domain + "/" + _checkUrl + "&wx_og_id=" + wx_og_id + "&openId=" + openId;
                    }
                    else
                    {
                        URL = domain + "/" + _checkUrl + "?wx_og_id=" + wx_og_id + "&openId=" + openId;
                    }
                }
            }
            return URL;
        }
        /// <summary>
        /// 处理文字请求
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            IResponseMessageBase responseMessage = null;

            string wx_og_id = requestMessage.ToUserName;
            string keyword = requestMessage.Content;
            string openId = requestMessage.FromUserName;
           
            BLL.Users.wx_fans.SetLastTime(openId);

            Common.Config.wechatconfig _config = new BLL.Config.wechat_config().loadConfig();

            bool no_match = false; //标识是否无匹配


            #region 关键词回复

            //根据关键词查询一条规则 ,优先级为：图文、视频、语音、图片、文本

            t_wx_rules rule = KDWechat.BLL.Chats.wx_rules.GetModel(wx_og_id, keyword);
           
            if (rule != null)
            {
                //有规则，查找回复信息
                t_wx_rule_reply reply = KDWechat.BLL.Chats.wx_rule_reply.GetModelByRid(rule.id, wx_og_id);
               
                if (reply != null)
                {
                   

                    #region 判断回复类型

                    switch (reply.reply_type)
                    {
                        

                        case (int)msg_type.文本:

                            var responseMessageText = base.CreateResponseMessage<ResponseMessageText>();
                            responseMessage = responseMessageText;
                            string content = reply.contents.Replace("<br />", "<br>");
                            responseMessageText.Content = Common.Utils.DropHTMLOnly(content.Replace("<br>", "\r\n").Replace("&nbsp;", " "));
                            break;
                        case (int)msg_type.图片:

                            t_wx_media_materials pic = KDWechat.BLL.Chats.wx_media_materials.GetMediaMaterialByID((int)reply.source_id);
                            if (pic != null)
                            {
                                var responseMessageImage = base.CreateResponseMessage<ResponseMessageNews>();
                                responseMessage = responseMessageImage;
                                string URl = _config.domain + pic.file_url;
                                if (_config.pic_templat_path.Length > 0)
                                {
                                    URl = getUrl(_config.domain, _config.pic_templat_path, pic.id, openId, wx_og_id);
                                }
                                responseMessageImage.Articles.Add(new Senparc.Weixin.MP.Entities.Article()
                                {
                                    Description = "",// pic.remark,
                                    PicUrl = _config.domain + pic.file_url,
                                    Title = pic.title,
                                    Url = URl
                                });
                            }
                            break;
                        case (int)msg_type.语音:
                            t_wx_media_materials voice = KDWechat.BLL.Chats.wx_media_materials.GetMediaMaterialByID((int)reply.source_id);
                            if (voice != null)
                            {

                                var responseMessageMusic = base.CreateResponseMessage<ResponseMessageNews>();
                                responseMessage = responseMessageMusic;
                                responseMessageMusic.Articles.Add(new Senparc.Weixin.MP.Entities.Article()
                                {
                                    Description = "",// voice.remark,
                                    PicUrl = _config.domain + "/images/audio.png",
                                    Title = voice.title,
                                    Url = getUrl(_config.domain, _config.voice_templat_path, voice.id, openId, wx_og_id)
                                });


                            }

                            break;
                        case (int)msg_type.视频:


                            t_wx_media_materials m = KDWechat.BLL.Chats.wx_media_materials.GetMediaMaterialByID((int)reply.source_id);
                            if (m != null)
                            {
                                var responseMessageVideo = base.CreateResponseMessage<ResponseMessageNews>();
                                responseMessage = responseMessageVideo;
                                responseMessageVideo.Articles.Add(new Senparc.Weixin.MP.Entities.Article()
                                {
                                    Description = " ",// m.remark,
                                    PicUrl = _config.domain + m.hq_music_url,
                                    Title = m.title,
                                    Url = m.video_type == (int)video_type.本地视频 ? getUrl(_config.domain, _config.video_templat_path, m.id, openId, wx_og_id) : m.file_url
                                });

                            }

                            break;

                        case (int)msg_type.单图文:


                            t_wx_news_materials news_alone = KDWechat.BLL.Chats.wx_news_materials.GetModel((int)reply.source_id);
                            if (news_alone != null)
                            {
                                var responseMessageNews = base.CreateResponseMessage<ResponseMessageNews>();
                                responseMessage = responseMessageNews;

                                responseMessageNews.Articles.Add(new Senparc.Weixin.MP.Entities.Article()
                                {
                                    Description = news_alone.summary,
                                    PicUrl = (news_alone.cover_img.Contains("http") == true ? "" : _config.domain) + news_alone.cover_img,
                                    Title = news_alone.title,
                                    Url = getNewsUrl(news_alone.push_type, news_alone.link_url, news_alone.app_link, _config.domain, _config.news_templat_path, news_alone.id, openId, wx_og_id)
                                });
                            }
                            break;
                        case (int)msg_type.多图文:
                            t_wx_news_materials multi = KDWechat.BLL.Chats.wx_news_materials.GetModel((int)reply.source_id);
                            if (multi != null)
                            {
                                var responseMessageMutliNews = base.CreateResponseMessage<ResponseMessageNews>();
                                responseMessage = responseMessageMutliNews;
                                responseMessageMutliNews.Articles.Add(new Senparc.Weixin.MP.Entities.Article()
                                {
                                    Description = multi.summary,
                                    PicUrl = (multi.cover_img.Contains("http") == true ? "" : _config.domain) + multi.cover_img,
                                    Title = multi.title,
                                    Url = getNewsUrl(multi.push_type, multi.link_url, multi.app_link, _config.domain, _config.news_templat_path, multi.id, openId, wx_og_id)
                                });
                                //取出子级图文
                                string child_str = string.Empty;
                                List<t_wx_news_materials> list = KDWechat.BLL.Chats.wx_news_materials.GetChildList(multi.id);
                                if (list != null)
                                {
                                    foreach (t_wx_news_materials item in list)
                                    {
                                        responseMessageMutliNews.Articles.Add(new Senparc.Weixin.MP.Entities.Article()
                                        {
                                            Description = item.summary,
                                            PicUrl = (item.cover_img.Contains("http") == true ? "" : _config.domain) + item.cover_img,
                                            Title = item.title,
                                            Url = getNewsUrl(item.push_type, item.link_url, item.app_link, _config.domain, _config.news_templat_path, item.id, openId, wx_og_id)
                                        });
                                    }
                                }

                            }
                            break;
                        case (Int32)msg_type.多客服:  //Damos -add at 2015-4-23 11:19 多客服触发。
                            responseMessage = base.CreateResponseMessage<ResponseMessageTransfer_Customer_Service>();
                            break;
                    }
                    #endregion

                    //添加关键词命中记录Start--Damos
                    Companycn.Core.EntityFramework.EFHelper.AddModel<creater_wxEntities, t_st_keyword_view>(new t_st_keyword_view()
                    {
                        add_time = DateTime.Now,
                        keyword = keyword,
                        keyword_action = reply.reply_type.ToString(),
                        open_id = openId,
                        wx_id = -1,
                        wx_og_id = wx_og_id
                    });
                    //添加关键词命中记录End--Damos

                }
                else
                {
                    no_match = true;
                }

            }
            else
            {
                no_match = true;
            }
            #endregion

            #region 无匹配
            if (no_match)
            {
                wx_fans.SetReplyStateAndTime(openId, FansReplyState.未回复);
                //无匹配回复
                t_wx_basic_reply model = KDWechat.BLL.Chats.wx_basic_reply.GetModel(wx_og_id, (int)AutoReply.无匹配时);
                if (model != null)
                {
                    #region 判断回复类型

                    switch (model.reply_type)
                    {
                        case (int)msg_type.文本:

                            var responseMessageText = base.CreateResponseMessage<ResponseMessageText>();
                            responseMessage = responseMessageText;
                            string content = model.contents.Replace("<br />", "<br>");
                            responseMessageText.Content = Common.Utils.DropHTMLOnly(content.Replace("<br>", "\r\n").Replace("&nbsp;", " "));

                            break;
                        case (int)msg_type.图片:

                            t_wx_media_materials pic = KDWechat.BLL.Chats.wx_media_materials.GetMediaMaterialByID((int)model.source_id);
                            if (pic != null)
                            {
                                var responseMessageImage = base.CreateResponseMessage<ResponseMessageNews>();
                                responseMessage = responseMessageImage;
                                string URl = _config.domain + pic.file_url;
                                if (_config.pic_templat_path.Length > 0)
                                {
                                    URl = getUrl(_config.domain, _config.pic_templat_path, pic.id, openId, wx_og_id);
                                }
                                responseMessageImage.Articles.Add(new Senparc.Weixin.MP.Entities.Article()
                                {
                                    Description = "",// pic.remark,
                                    PicUrl = _config.domain + pic.file_url,
                                    Title = pic.title,
                                    Url = URl
                                });
                            }
                            break;
                        case (int)msg_type.语音:
                            t_wx_media_materials voice = KDWechat.BLL.Chats.wx_media_materials.GetMediaMaterialByID((int)model.source_id);
                            if (voice != null)
                            {
                                //var responseMessageMusic = base.CreateResponseMessage<ResponseMessageMusic>();
                                //responseMessage = responseMessageMusic;
                                //responseMessageMusic.Music = new Senparc.Weixin.MP.Entities.Music() { Description = voice.remark, MusicUrl = _config.domain + voice.file_url, Title = voice.title, ThumbMediaId = "xW4bZxyAyeyD4cXivYkW685QbwkU8GVRrop8ytgSq3z3BisbDinop84Cn__gvpSW" };
                                var responseMessageMusic = base.CreateResponseMessage<ResponseMessageNews>();
                                responseMessage = responseMessageMusic;
                                responseMessageMusic.Articles.Add(new Senparc.Weixin.MP.Entities.Article()
                                {
                                    Description = "",// voice.remark,
                                    PicUrl = _config.domain + "/images/audio.png",
                                    Title = voice.title,
                                    Url = getUrl(_config.domain, _config.voice_templat_path, voice.id, openId, wx_og_id)
                                });
                            }

                            break;
                        case (int)msg_type.视频:


                            t_wx_media_materials m = KDWechat.BLL.Chats.wx_media_materials.GetMediaMaterialByID((int)model.source_id);
                            if (m != null)
                            {
                                var responseMessageVideo = base.CreateResponseMessage<ResponseMessageNews>();
                                responseMessage = responseMessageVideo;
                                responseMessageVideo.Articles.Add(new Senparc.Weixin.MP.Entities.Article()
                                {
                                    Description = "",// m.remark,
                                    PicUrl = _config.domain + m.hq_music_url,
                                    Title = m.title,
                                    Url = m.video_type == (int)video_type.本地视频 ? getUrl(_config.domain, _config.video_templat_path, m.id, openId, wx_og_id) : m.file_url
                                });

                            }

                            break;

                        case (int)msg_type.单图文:


                            t_wx_news_materials news_alone = KDWechat.BLL.Chats.wx_news_materials.GetModel((int)model.source_id);
                            if (news_alone != null)
                            {
                                var responseMessageNews = base.CreateResponseMessage<ResponseMessageNews>();
                                responseMessage = responseMessageNews;
                                responseMessageNews.Articles.Add(new Senparc.Weixin.MP.Entities.Article()
                                {
                                    Description = news_alone.summary,
                                    PicUrl = (news_alone.cover_img.Contains("http") == true ? "" : _config.domain) + news_alone.cover_img,
                                    Title = news_alone.title,
                                    Url = getNewsUrl(news_alone.push_type, news_alone.link_url, news_alone.app_link, _config.domain, _config.news_templat_path, news_alone.id, openId, wx_og_id)
                                });
                            }
                            break;
                        case (int)msg_type.多图文:
                            t_wx_news_materials multi = KDWechat.BLL.Chats.wx_news_materials.GetModel((int)model.source_id);
                            if (multi != null)
                            {
                                var responseMessageMutliNews = base.CreateResponseMessage<ResponseMessageNews>();
                                responseMessage = responseMessageMutliNews;
                                responseMessageMutliNews.Articles.Add(new Senparc.Weixin.MP.Entities.Article()
                                {
                                    Description = multi.summary,
                                    PicUrl = (multi.cover_img.Contains("http") == true ? "" : _config.domain) + multi.cover_img,
                                    Title = multi.title,
                                    Url = getNewsUrl(multi.push_type, multi.link_url, multi.app_link, _config.domain, _config.news_templat_path, multi.id, openId, wx_og_id)
                                });
                                //取出子级图文
                                string child_str = string.Empty;
                                List<t_wx_news_materials> list = KDWechat.BLL.Chats.wx_news_materials.GetChildList(multi.id);
                                if (list != null)
                                {
                                    foreach (t_wx_news_materials item in list)
                                    {
                                        responseMessageMutliNews.Articles.Add(new Senparc.Weixin.MP.Entities.Article()
                                        {
                                            Description = item.summary,
                                            PicUrl = (item.cover_img.Contains("http") == true ? "" : _config.domain) + item.cover_img,
                                            Title = item.title,
                                            Url = getNewsUrl(item.push_type, item.link_url, item.app_link, _config.domain, _config.news_templat_path, item.id, openId, wx_og_id)
                                        });
                                    }
                                }

                            }
                            break;
                         
                    }
                    #endregion
                }
                #region 记录用户输入的内容

                KDWechat.BLL.Logs.wx_fans_chats.CreateFansChat(new t_wx_fans_chats()
                {
                    contents = keyword,
                    create_time = DateTime.Now,
                    from_type = (int)FromUserType.用户,
                    is_sys_auto_reply = 0,
                    media_id = "",
                    msg_type = (int)msg_type.文本,
                    open_id = openId,
                    wx_id = 0,
                    wx_og_id = wx_og_id
                });
                #endregion
            }
            #endregion


            if (null == responseMessage)
            {
                var respons = base.CreateResponseMessage<ResponseMessageText>();
                respons.Content = string.Empty;
                responseMessage = respons;
            }

            return responseMessage;
        }
        /// <summary>
        /// 构造图文链接
        /// </summary>
        /// <param name="push_type"></param>
        /// <param name="link_url"></param>
        /// <param name="app_link"></param>
        /// <param name="domain"></param>
        /// <param name="news_templat_path"></param>
        /// <param name="newsid"></param>
        /// <param name="openId"></param>
        /// <param name="wx_og_id"></param>
        /// <returns></returns>
        private string getNewsUrl(string push_type, string link_url, string app_link, string domain, string news_templat_path, int newsid, string openId, string wx_og_id)
        {
            string url = "";
            switch (push_type)
            {
                case "article":
                    url = getUrl(domain, news_templat_path, newsid, openId, wx_og_id);
                    break;

                case "link":
                    url = link_url.Trim().Replace("$openId$", openId).Replace("$wx_og_id$", wx_og_id);
                    break;

                case "app":
                    url = app_link.Trim();
                    if (app_link.Trim().Contains("?"))
                    {
                        url += "&openId=" + openId + "&wx_og_id=" + wx_og_id;
                    }
                    else
                    {
                        url += "?openId=" + openId + "&wx_og_id=" + wx_og_id;
                    }
                    break;
            }

            return url;
        }

        /// <summary>
        /// 处理位置请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnLocationRequest(RequestMessageLocation requestMessage)
        {
            //更新最后一次互动时间(2015-1-15/yzl)
            string openId = requestMessage.FromUserName;
            BLL.Users.wx_fans.SetLastTime(openId);

            var locationService = new LocationService();
            var responseMessage = locationService.GetResponseMessage(requestMessage as RequestMessageLocation);
            return responseMessage;
        }

        /// <summary>
        /// 处理图片请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnImageRequest(RequestMessageImage requestMessage)
        {
            //更新最后一次互动时间(2015-1-15/yzl)
            string openId = requestMessage.FromUserName;
            BLL.Users.wx_fans.SetLastTime(openId);

            #region 用户记录

            //  下载图片
            var fans = BLL.Users.wx_fans.GetFansByID(openId);
            t_wx_wechats wx_wechat = BLL.Chats.wx_wechats.GetWeChatByID(fans.wx_id);
            string accessToken = BLL.Chats.wx_wechats.GetAccessToken(wx_wechat.id);
            System.IO.MemoryStream ms = new MemoryStream();
            System.IO.MemoryStream new_ms = new MemoryStream();
            Senparc.Weixin.MP.AdvancedAPIs.Media.Get(accessToken, requestMessage.MediaId, ms);

            var img = System.Drawing.Image.FromStream(ms);
            img.Save(new_ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            img.Save(HttpContext.Current.Server.MapPath("/upload/wx_img/" + requestMessage.MediaId + ".jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);


            KDWechat.BLL.Logs.wx_fans_chats.CreateFansChat(new t_wx_fans_chats()
            {
                contents = requestMessage.PicUrl,
                create_time = DateTime.Now,
                from_type = (int)FromUserType.用户,
                is_sys_auto_reply = 0,
                media_id = requestMessage.MediaId,
                msg_type = (int)msg_type.图片,
                open_id = requestMessage.FromUserName,
                wx_id = 0,
                wx_og_id = requestMessage.ToUserName
            });


            #endregion


            //var fans = BLL.Users.wx_fans.GetFansByID(openId);
            //FansState state = fans.state == null ? FansState.自动回复状态 : (FansState)fans.state;// wx_fans.CheckUser(openId);
            //if (state == FansState.上传小票状态)
            //{
            //    IResponseMessageBase responseMessage = null;

            //    var responsemsg = base.CreateResponseMessage<ResponseMessageText>();
            //    //检测用户是否已经绑定openid了
            //    var msg = "";
            //    var member = wx_fans.GetMermberInfo(openId, ref msg);
            //    if (member == null)
            //    {
            //        wx_fans.SetState(openId, FansState.自动回复状态);
            //        var passport_url = ConfigurationManager.AppSettings["passport_url"];
            //        switch (msg)
            //        {
            //            case "1":
            //            case "2":
            //                responsemsg.Content = "您还没有绑定您的会员账号，所以您无法进入扫小票模式，请点击<a href=\"" + passport_url + "/BindWXRelationNew?openId=" + openId + "\">此处</a>绑定您的账号，绑定成功后，请在聊天窗口中输入“扫小票”，进入扫小票模式。";
            //                break;
            //            case "3":
            //                var vip_url = ConfigurationManager.AppSettings["vip_url"] + "/userCenter";
            //                responsemsg.Content = "您尚未登录，所以您无法进入扫小票模式，请点击<a href=\"" + vip_url + "\">此处</a>登录账号，登录成功后，请在聊天窗口中输入“扫小票”，进入扫小票模式。";
            //                break;
            //        }

            //    }
            //    else
            //    {

            //        var content = "上传小票失败";
            //        t_wx_wechats wx_wechat = BLL.Chats.wx_wechats.GetWeChatByID(fans.wx_id);
            //        if (wx_wechat != null)
            //        {
            //            //  下载图片
            //            string accessToken = BLL.Chats.wx_wechats.GetAccessToken(wx_wechat.id);
            //            System.IO.MemoryStream ms = new MemoryStream();
            //            System.IO.MemoryStream new_ms = new MemoryStream();
            //            Senparc.Weixin.MP.AdvancedAPIs.Media.Get(accessToken, requestMessage.MediaId, ms);


            //            var img = System.Drawing.Image.FromStream(ms);
            //            img.Save(new_ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            //            new_ms.Position = 0;
            //            if (new_ms.Length > 0)
            //            {
            //                var mall_url = ConfigurationManager.AppSettings["KDMallUrl"] + "mall?tp=getMallLocation&source=" + wx_wechat.mall_source;
            //                var LocationCode = Common.KDHttpRequest.HttpGet(mall_url, "");
            //                //将图片发送给CRM
            //                var member_url = System.Configuration.ConfigurationManager.AppSettings["member_url"];
            //                var appurl = member_url + "?fp=1000&mt=upload_receipts&member_id=" + DESEncrypt.Encrypt(member.id.ToString(), "KDMember") + "&LocationCode=" + LocationCode;
            //                var returnMember = Common.RequestUtility.HttpPost(appurl, new_ms);
            //                if (returnMember != null)
            //                {
            //                    var returnResult = Common.GetApiInformationcs.GetJsonResultScan<JsonDataResultScan<string>>(returnMember);

            //                    if (returnResult != null)
            //                    {
            //                        if (returnResult.result == -1)
            //                        {
            //                            wx_fans.SetState(openId, FansState.自动回复状态);
            //                            var passport_url = ConfigurationManager.AppSettings["passport_url"];
            //                            if (string.IsNullOrEmpty(passport_url))
            //                                passport_url = "http://passport.capitaland.com.cn";
            //                            passport_url += "/BindWXRelationNew?openId=" + openId;
            //                            content += "，您的会员帐号绑定已失效，请点击<a href=\"" + passport_url + "\">此处</a>重新绑定帐号，绑定成功后，请在聊天窗口中输入“扫小票”，进入扫小票模式。";
            //                        }
            //                        else if (returnResult.result == 1)
            //                        {
            //                            wx_fans.AddScanLog(member.id, returnResult.data.ToString());

            //                            try
            //                            {
            //                                img.Save(HttpContext.Current.Server.MapPath("/upload/tickets/" + returnResult.data.ToString() + ".jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);
            //                            }
            //                            catch { }
            //                            content = "小票上传成功！";
            //                        }

            //                    }

            //                }
            //            }

            //        }
            //        responsemsg.Content = content;
            //    }
            //    responseMessage = responsemsg;
            //    return responseMessage;
            //}
            return null;

        }

        /// <summary>
        /// 处理语音请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnVoiceRequest(RequestMessageVoice requestMessage)
        {
            //更新最后一次互动时间(2015-1-15/yzl)
            string openId = requestMessage.FromUserName;
            BLL.Users.wx_fans.SetLastTime(openId);

            IResponseMessageBase responseMessage = null;

            string contents = requestMessage.Recognition; //用户语音转换后的文本，注：由于客户端缓存，开发者开启或者关闭语音识别功能，对新关注者立刻生效，对已关注用户需要24小时生效
            string wx_og_id = requestMessage.ToUserName;
            //string openId = requestMessage.FromUserName;
            #region 用户记录


            if (contents.Trim() != "")
            {

                //公众号开启语音识别功能
                Common.Config.wechatconfig _config = new BLL.Config.wechat_config().loadConfig();

                #region 查找关键词回复

                //根据关键词查询一条规则 ,优先级为：图文、视频、语音、图片、文本

                t_wx_rules rule = KDWechat.BLL.Chats.wx_rules.GetModel(wx_og_id, contents);
                if (rule != null)
                {
                    //有规则，查找回复信息
                    t_wx_rule_reply reply = KDWechat.BLL.Chats.wx_rule_reply.GetModelByRid(rule.id, wx_og_id);
                    if (reply != null)
                    {
                        #region 判断回复类型

                        switch (reply.reply_type)
                        {
                            case (int)msg_type.文本:

                                var responseMessageText = base.CreateResponseMessage<ResponseMessageText>();
                                responseMessage = responseMessageText;
                                string content = reply.contents.Replace("<br />", "<br>");
                                responseMessageText.Content = Common.Utils.DropHTMLOnly(content.Replace("<br>", "\n").Replace("&nbsp;", " "));
                                break;
                            case (int)msg_type.图片:

                                t_wx_media_materials pic = KDWechat.BLL.Chats.wx_media_materials.GetMediaMaterialByID((int)reply.source_id);
                                if (pic != null)
                                {
                                    var responseMessageImage = base.CreateResponseMessage<ResponseMessageNews>();
                                    responseMessage = responseMessageImage;
                                    string URl = _config.domain + pic.file_url;
                                    if (_config.pic_templat_path.Length > 0)
                                    {
                                        URl = getUrl(_config.domain, _config.pic_templat_path, pic.id, openId, wx_og_id);
                                    }
                                    responseMessageImage.Articles.Add(new Senparc.Weixin.MP.Entities.Article()
                                    {
                                        Description = "",// pic.remark,
                                        PicUrl = _config.domain + pic.file_url,
                                        Title = pic.title,
                                        Url = URl
                                    });
                                }
                                break;
                            case (int)msg_type.语音:
                                t_wx_media_materials voice = KDWechat.BLL.Chats.wx_media_materials.GetMediaMaterialByID((int)reply.source_id);
                                if (voice != null)
                                {

                                    var responseMessageMusic = base.CreateResponseMessage<ResponseMessageNews>();
                                    responseMessage = responseMessageMusic;
                                    responseMessageMusic.Articles.Add(new Senparc.Weixin.MP.Entities.Article()
                                    {
                                        Description = "",// voice.remark,
                                        PicUrl = _config.domain + "/images/audio.png",
                                        Title = voice.title,
                                        Url = getUrl(_config.domain, _config.voice_templat_path, voice.id, openId, wx_og_id)
                                    });


                                }

                                break;
                            case (int)msg_type.视频:


                                t_wx_media_materials m = KDWechat.BLL.Chats.wx_media_materials.GetMediaMaterialByID((int)reply.source_id);
                                if (m != null)
                                {
                                    var responseMessageVideo = base.CreateResponseMessage<ResponseMessageNews>();
                                    responseMessage = responseMessageVideo;
                                    responseMessageVideo.Articles.Add(new Senparc.Weixin.MP.Entities.Article()
                                    {
                                        Description = "",// m.remark,
                                        PicUrl = _config.domain + m.hq_music_url,
                                        Title = m.title,
                                        Url = getUrl(_config.domain, _config.video_templat_path, m.id, openId, wx_og_id)
                                    });

                                }

                                break;

                            case (int)msg_type.单图文:


                                t_wx_news_materials news_alone = KDWechat.BLL.Chats.wx_news_materials.GetModel((int)reply.source_id);
                                if (news_alone != null)
                                {
                                    var responseMessageNews = base.CreateResponseMessage<ResponseMessageNews>();
                                    responseMessage = responseMessageNews;
                                    responseMessageNews.Articles.Add(new Senparc.Weixin.MP.Entities.Article()
                                    {
                                        Description = news_alone.summary,
                                        PicUrl = (news_alone.cover_img.Contains("http") == true ? "" : _config.domain) + news_alone.cover_img,
                                        Title = news_alone.title,
                                        Url = getNewsUrl(news_alone.push_type, news_alone.link_url, news_alone.app_link, _config.domain, _config.news_templat_path, news_alone.id, openId, wx_og_id)
                                    });
                                }
                                break;
                            case (int)msg_type.多图文:
                                t_wx_news_materials multi = KDWechat.BLL.Chats.wx_news_materials.GetModel((int)reply.source_id);
                                if (multi != null)
                                {
                                    var responseMessageMutliNews = base.CreateResponseMessage<ResponseMessageNews>();
                                    responseMessage = responseMessageMutliNews;
                                    responseMessageMutliNews.Articles.Add(new Senparc.Weixin.MP.Entities.Article()
                                    {
                                        Description = multi.summary,
                                        PicUrl = (multi.cover_img.Contains("http") == true ? "" : _config.domain) + multi.cover_img,
                                        Title = multi.title,
                                        Url = getNewsUrl(multi.push_type, multi.link_url, multi.app_link, _config.domain, _config.news_templat_path, multi.id, openId, wx_og_id)
                                    });
                                    //取出子级图文
                                    string child_str = string.Empty;
                                    List<t_wx_news_materials> list = KDWechat.BLL.Chats.wx_news_materials.GetChildList(multi.id);
                                    if (list != null)
                                    {
                                        foreach (t_wx_news_materials item in list)
                                        {
                                            responseMessageMutliNews.Articles.Add(new Senparc.Weixin.MP.Entities.Article()
                                            {
                                                Description = item.summary,
                                                PicUrl = (item.cover_img.Contains("http") == true ? "" : _config.domain) + item.cover_img,
                                                Title = item.title,
                                                Url = getNewsUrl(item.push_type, item.link_url, item.app_link, _config.domain, _config.news_templat_path, item.id, openId, wx_og_id)
                                            });
                                        }
                                    }

                                }
                                break;
                           
                        }
                        #endregion


                        //添加关键词命中记录Start--Damos
                        Companycn.Core.EntityFramework.EFHelper.AddModel<creater_wxEntities, t_st_keyword_view>(new t_st_keyword_view()
                        {
                            add_time = DateTime.Now,
                            keyword = contents,
                            keyword_action = reply.reply_type.ToString(),
                            open_id = openId,
                            wx_id = -1,
                            wx_og_id = wx_og_id
                        });
                        //添加关键词命中记录End--Damos
                    }


                }

                #endregion
            }
            #region 记录文本聊天记录
            KDWechat.BLL.Logs.wx_fans_chats.CreateFansChat(new t_wx_fans_chats()
            {
                contents = contents,
                create_time = DateTime.Now,
                from_type = (int)FromUserType.用户,
                is_sys_auto_reply = 0,
                media_id = requestMessage.MediaId,
                msg_type = (int)msg_type.语音,
                open_id = requestMessage.FromUserName,
                wx_id = 0,
                wx_og_id = wx_og_id
            });
            #endregion



            #endregion


            return responseMessage;
        }

        /// <summary>
        /// 处理视频请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnVideoRequest(RequestMessageVideo requestMessage)
        {
            //更新最后一次互动时间(2015-1-15/yzl)
            string openId = requestMessage.FromUserName;
            BLL.Users.wx_fans.SetLastTime(openId);

            #region 用户记录

            KDWechat.BLL.Logs.wx_fans_chats.CreateFansChat(new t_wx_fans_chats()
            {
                contents = "",
                create_time = DateTime.Now,
                from_type = (int)FromUserType.用户,
                is_sys_auto_reply = 0,
                media_id = requestMessage.MediaId,
                msg_type = (int)msg_type.视频,
                open_id = requestMessage.FromUserName,
                wx_id = 0,
                wx_og_id = requestMessage.ToUserName
            });


            #endregion


            return null;
        }

        /// <summary>
        /// 处理链接消息请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnLinkRequest(RequestMessageLink requestMessage)
        {
            //更新最后一次互动时间(2015-1-15/yzl)
            string openId = requestMessage.FromUserName;
            BLL.Users.wx_fans.SetLastTime(openId);

            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = string.Format(@"您发送了一条连接信息：
Title：{0}
Description:{1}
Url:{2}", requestMessage.Title, requestMessage.Description, requestMessage.Url);
            return responseMessage;
        }

        /// <summary>
        /// 处理事件请求（这个方法一般不用重写，这里仅作为示例出现。除非需要在判断具体Event类型以外对Event信息进行统一操作
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEventRequest(IRequestMessageEventBase requestMessage)
        {
            var eventResponseMessage = base.OnEventRequest(requestMessage);//对于Event下属分类的重写方法，见：CustomerMessageHandler_Events.cs
            //TODO: 对Event信息进行统一操作
            return eventResponseMessage;
        }

        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            /* 所有没有被处理的消息会默认返回这里的结果，
             * 因此，如果想把整个微信请求委托出去（例如需要使用分布式或从其他服务器获取请求），
             * 只需要在这里统一发出委托请求，如：
             * var responseMessage = MessageAgent.RequestResponseMessage(agentUrl, agentToken, RequestDocument.ToString());
             * return responseMessage;
             */

            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "这条消息来自DefaultResponseMessage。";
            return responseMessage;
        }
    }
}
