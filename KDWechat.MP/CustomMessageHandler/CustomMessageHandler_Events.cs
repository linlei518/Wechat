using System;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Senparc.Weixin.MP.Agent;
using Senparc.Weixin.Context;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.MP.MessageHandlers;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.AdvancedAPIs;
using KDWechat.Common;
using KDWechat.DAL;
using System.Collections.Generic;
using KDWechat.BLL.Users;
using Senparc.Weixin.MP.Entities;
using KDWechat.BLL.Logs;
using KDWechat.BLL.Chats;
using BaiDuMapAPI;
using Newtonsoft.Json;
using System.Data;
using System.Configuration;

namespace KDWechat.MP.CustomMessageHandler
{
    /// <summary>
    /// 自定义MessageHandler
    /// </summary>
    public partial class CustomMessageHandler
    {
        private string GetWelcomeInfo()
        {
            //获取Senparc.Weixin.MP.dll版本信息
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(HttpContext.Current.Server.MapPath("~/bin/Senparc.Weixin.MP.dll"));
            var version = string.Format("{0}.{1}", fileVersionInfo.FileMajorPart, fileVersionInfo.FileMinorPart);
            return string.Format(
@"欢迎关注【Senparc.Weixin.MP 微信公众平台SDK】，当前运行版本：v{0}。
您可以发送【文字】【位置】【图片】【语音】等不同类型的信息，查看不同格式的回复。

您也可以直接点击菜单查看各种类型的回复。

SDK官方地址：http://weixin.senparc.com
源代码及Demo下载地址：https://github.com/JeffreySu/WeiXinMPSDK
Nuget地址：https://www.nuget.org/packages/Senparc.Weixin.MP",
                version);
        }

        public override IResponseMessageBase OnTextOrEventRequest(RequestMessageText requestMessage)
        {
            // 预处理文字或事件类型请求。
            // 这个请求是一个比较特殊的请求，通常用于统一处理来自文字或菜单按钮的同一个执行逻辑，
            // 会在执行OnTextRequest或OnEventRequest之前触发，具有以下一些特征：
            // 1、如果返回null，则继续执行OnTextRequest或OnEventRequest
            // 2、如果返回不为null，则终止执行OnTextRequest或OnEventRequest，返回最终ResponseMessage
            // 3、如果是事件，则会将RequestMessageEvent自动转为RequestMessageText类型，其中RequestMessageText.Content就是RequestMessageEvent.EventKey

            if (requestMessage.Content == "OneClick")
            {
                var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                strongResponseMessage.Content = "您点击了底部按钮。\r\n为了测试微信软件换行bug的应对措施，这里做了一个——\r\n换行";
                return strongResponseMessage;
            }
            return null;//返回null，则继续执行OnTextRequest或OnEventRequest
        }

        public override IResponseMessageBase OnEvent_ClickRequest(RequestMessageEvent_Click requestMessage)
        {
            IResponseMessageBase responseMessage = null;
            //菜单点击，需要跟创建菜单时的Key匹配

            string wx_og_id = requestMessage.ToUserName;
            string openId = requestMessage.FromUserName;
            string menu_key = requestMessage.EventKey;

            wx_fans.SetLastTime(openId);

            Common.Config.wechatconfig _config = new BLL.Config.wechat_config().loadConfig();

            #region 获取菜单
            t_wx_diy_menus menu = KDWechat.BLL.Chats.wx_diy_menus.GetModel(wx_og_id, menu_key);
            if (menu != null)
            {
                wx_fans.ExitSell(openId);
                int reply_type = Common.Utils.ObjToInt(menu.reply_type, -1);
                //添加点击记录Start--Damos
                Companycn.Core.EntityFramework.EFHelper.AddModel<creater_wxEntities, t_st_diymenu_click>(new t_st_diymenu_click()
                    {
                        add_time = DateTime.Now,
                        menu_action = menu.reply_type.ToString(),
                        menu_key = menu.menu_key,
                        menu_name = menu.menu_name,
                        open_id = openId,
                        wx_id = -1,
                        wx_og_id = wx_og_id
                    });
                //添加点击记录End

                //判断菜单类型
                switch (reply_type)
                {
                    case (int)msg_type.文本:

                        var responseMessageText = base.CreateResponseMessage<ResponseMessageText>();
                        responseMessage = responseMessageText;
                        string content = menu.contents.Replace("<br />", "<br>");
                        responseMessageText.Content = Common.Utils.DropHTMLOnly(content.Replace("<br>", "\r\n").Replace("&nbsp;", " "));
                        break;
                    case (int)msg_type.图片:

                        t_wx_media_materials pic = KDWechat.BLL.Chats.wx_media_materials.GetMediaMaterialByID((int)menu.soucre_id);
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
                        t_wx_media_materials voice = KDWechat.BLL.Chats.wx_media_materials.GetMediaMaterialByID((int)menu.soucre_id);
                        if (voice != null)
                        {

                            var responseMessageMusic = base.CreateResponseMessage<ResponseMessageNews>();
                            responseMessage = responseMessageMusic;
                            responseMessageMusic.Articles.Add(new Senparc.Weixin.MP.Entities.Article()
                            {
                                Description = "",//voice.remark,
                                PicUrl = _config.domain + "/images/audio.png",
                                Title = voice.title,
                                Url = getUrl(_config.domain, _config.voice_templat_path, voice.id, openId, wx_og_id)
                            });


                        }

                        break;
                    case (int)msg_type.视频:


                        t_wx_media_materials m = KDWechat.BLL.Chats.wx_media_materials.GetMediaMaterialByID((int)menu.soucre_id);
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


                        t_wx_news_materials news_alone = KDWechat.BLL.Chats.wx_news_materials.GetModel((int)menu.soucre_id);
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
                        t_wx_news_materials multi = KDWechat.BLL.Chats.wx_news_materials.GetModel((int)menu.soucre_id);
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
                     
                    default:
                        break;
                }
            }
            #endregion



            return responseMessage;
        }

        public override IResponseMessageBase OnEvent_EnterRequest(RequestMessageEvent_Enter requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = "您刚才发送了ENTER事件请求。";
            return responseMessage;
        }

        public override IResponseMessageBase OnEvent_LocationRequest(RequestMessageEvent_Location requestMessage)
        {
            var wechat = wx_wechats.GetWeChatByogID(requestMessage.ToUserName);
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "";
            if (null != wechat)
            {
                //responseMessage.Content = "wechat查找OK";
                string address = "";
                if (address.Trim().Length == 0)
                {
                    string jsonStr = BaiDuGeocodingAPI.GetAddressByLocation(requestMessage.Latitude.ToString() + "," + requestMessage.Longitude.ToString());
                    var forwardText = JsonConvert.DeserializeObject<GeocodingReverseResolved>(jsonStr);
                    address = forwardText.result.formatted_address;
                }
                //responseMessage.Content = "Address查找OK";
                //这里是微信客户端（通过微信服务器）自动发送过来的位置信息
                t_wx_fans_hislocation his_location = new t_wx_fans_hislocation()
                {
                    address = address,
                    create_time = DateTime.Now,
                    lng = (decimal)requestMessage.Longitude,
                    lat = (decimal)requestMessage.Latitude,
                    open_id = requestMessage.FromUserName,
                    wx_id = wechat.id,
                    wx_og_id = requestMessage.ToUserName
                };
                wx_fans_hislocation.CreateFansHislocation(his_location);
                //responseMessage.Content = "添加成功，address为："+address;
            }
            return responseMessage;//这里也可以返回null（需要注意写日志时候null的问题）
        }

        /// <summary>
        /// 扫描二维码
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_ScanRequest(RequestMessageEvent_Scan requestMessage)
        {

            //通过扫描关注
            ResponseMessageBase responseMessage = null;
            string wx_og_id = requestMessage.ToUserName;
            string strwx_og_id = "gh_08c3eeb3c689,gh_b8f2d691ac4c,gh_67f63d878114";//齐家网专用
            string openId = requestMessage.FromUserName;
            Common.Config.wechatconfig _config = new BLL.Config.wechat_config().loadConfig();
            var qijiaCode = System.Configuration.ConfigurationManager.AppSettings["qijiaCode"];
            int id = 0;
            Int32.TryParse(requestMessage.EventKey, out id);

            if (id > 0)
            {
                #region Damos通用二维码响应，可关联拓客二维码
                var qrConfigStr = ConfigurationManager.AppSettings["qrSetting"];
                if (!string.IsNullOrWhiteSpace(qrConfigStr))
                {
                    var qrInfoList = qrConfigStr.Split('|');

                    for (int i = 0; i < qrInfoList.Length; i++)
                    {
                        var x = qrInfoList[i];
                        var infoArr = x.Split(',');
                        if (infoArr.Length == 3)
                        {
                            var qrWxOgID = infoArr[0];
                            var qrNewsId = Utils.StrToInt(infoArr[1], 0);
                            var qrSourceId = Utils.StrToInt(infoArr[2], 0);
                            if (!string.IsNullOrWhiteSpace(qrWxOgID) && qrNewsId > 0 && qrSourceId > 0 && qrSourceId == id)
                            {
                                t_wx_news_materials qrNews = wx_news_materials.GetModel(qrNewsId);//图文id
                                if (qrNews != null)
                                {
                                    var qrArticalList = new List<Article>();
                                    qrArticalList.Add(new Article()
                                    {
                                        Description = qrNews.summary,
                                        PicUrl = (qrNews.cover_img.Contains("http") == true ? "" : _config.domain) + qrNews.cover_img,
                                        Title = qrNews.title,
                                        Url = getNewsUrl(qrNews.push_type, qrNews.link_url, qrNews.app_link, _config.domain, _config.news_templat_path, qrNews.id, openId, wx_og_id)
                                    });
                                    if (qrNews.channel_id == (int)ResponseNewsType.多图文)
                                    {
                                        //取出子级图文

                                        List<t_wx_news_materials> list = wx_news_materials.GetChildList(qrNews.id);
                                        if (list != null)
                                        {
                                            foreach (t_wx_news_materials item in list)
                                            {
                                                qrArticalList.Add(new Article()
                                                {
                                                    Description = item.summary,
                                                    PicUrl = (item.cover_img.Contains("http") == true ? "" : _config.domain) + item.cover_img,
                                                    Title = item.title,
                                                    Url = getNewsUrl(item.push_type, item.link_url, item.app_link, _config.domain, _config.news_templat_path, item.id, openId, wx_og_id)
                                                });
                                            }
                                        }
                                    }
                                    var responseMessageNews = base.CreateResponseMessage<ResponseMessageNews>();
                                    responseMessage = responseMessageNews;
                                    responseMessageNews.Articles = qrArticalList;
                                    return responseMessage;
                                }
                            }
                        }
                    }
                }
                #endregion
               
            }


            return responseMessage;
        }

        public override IResponseMessageBase OnEvent_ViewRequest(RequestMessageEvent_View requestMessage)
        {
            //说明：这条消息只作为接收，下面的responseMessage到达不了客户端，类似OnEvent_UnsubscribeRequest
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "您点击了view按钮，将打开网页：" + requestMessage.EventKey;
            //添加点击记录Start--Damos
            Companycn.Core.EntityFramework.EFHelper.AddModel<creater_wxEntities, t_st_diymenu_click>(new t_st_diymenu_click()
            {
                add_time = DateTime.Now,
                menu_action = requestMessage.EventKey,
                menu_key = "view",
                menu_name = "view",
                open_id = requestMessage.FromUserName,
                wx_id = -1,
                wx_og_id = requestMessage.ToUserName
            });
            //添加点击记录End
            return responseMessage;
        }

        /// <summary>
        /// 订阅（关注）事件
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {

            ResponseMessageBase responseMessage = null;
            string wx_og_id = requestMessage.ToUserName;
            string openId = requestMessage.FromUserName;
            t_wx_wechats wechat = KDWechat.BLL.Chats.wx_wechats.GetWeChatByogID(requestMessage.ToUserName);
            Common.Config.wechatconfig _config = new BLL.Config.wechat_config().loadConfig();

            string accesstoken = BLL.Chats.wx_wechats.GetAccessToken(wechat.id, wechat);// AccessTokenContainer.TryGetToken(wechat.app_id, wechat.app_secret);
            var fans = KDWechat.BLL.Users.wx_fans.GetFansByID(requestMessage.FromUserName);
            int source_id = 0;
            if (requestMessage.EventKey.Contains("qrscene"))
            {
                source_id = Common.Utils.StrToInt(requestMessage.EventKey.Split(new char[] { '_' })[1], 0);
            }
            if (null == fans)
            {

                // danny edit
                UserInfoJson user = null;
                try
                {
                    if (!accesstoken.Contains("Error:"))
                    {
                        user = User.Info(accesstoken, requestMessage.FromUserName);
                    }

                }
                catch (Senparc.Weixin.Exceptions.ErrorJsonResultException ex)
                {

                }
                if (user == null)
                {
                    user = new UserInfoJson();
                    user.city = "";
                    user.sex = -1;
                    user.nickname = "";
                    user.language = "zh_cn";
                    user.country = "中国";
                    user.province = "";
                    user.headimgurl = "";
                }
                fans = new KDWechat.DAL.t_wx_fans()
                {
                    city = user.city,
                    sex = user.sex,
                    province = user.province,
                    open_id = requestMessage.FromUserName,
                    nick_name = user.nickname,
                    country = user.country,
                    language = user.language,
                    subscribe_time = DateTime.Now,
                    headimgurl = user.headimgurl,
                    guid = Guid.NewGuid().ToString().Replace("-", ""),
                    wx_og_id = wechat.wx_og_id,
                    wx_id = wechat.id,
                    status = (int)KDWechat.Common.Status.正常,
                    wx_country = user.country,
                    wx_city = user.city,
                    wx_sex = user.sex,
                    wx_province = user.province,
                    source_id = source_id
                };
                fans = KDWechat.BLL.Users.wx_fans.InsertFans(fans);//Damos -edit at 2015-4-21 10:34 增加二维码扫码溯源统计
                Companycn.Core.EntityFramework.EFHelper.AddModel<creater_wxEntities, t_wx_qrcode_history>(new t_wx_qrcode_history { add_time = DateTime.Now, fans_id = fans.id, qr_code_id = source_id, wx_id = fans.wx_id, wx_og_id = fans.wx_og_id });
                
            }
            else
            {
                //老用户，需要更新关注的状态  , danny add
                fans.status = 1;
                fans.subscribe_time = DateTime.Now;
                fans.source_id = source_id;
                
                KDWechat.BLL.Users.wx_fans.UpdateFans(fans);
                Companycn.Core.EntityFramework.EFHelper.AddModel<creater_wxEntities, t_wx_qrcode_history>(new t_wx_qrcode_history { add_time = DateTime.Now, fans_id = fans.id, qr_code_id = source_id, wx_id = fans.wx_id, wx_og_id = fans.wx_og_id });



            }
 

         

            #region Damos通用二维码响应，可关联拓客二维码
            var qrConfigStr = ConfigurationManager.AppSettings["qrSetting"];
            if (!string.IsNullOrWhiteSpace(qrConfigStr))
            {
                var qrInfoList = qrConfigStr.Split('|').ToList();
                qrInfoList.ForEach(x => {
                    var infoArr = x.Split(',');
                    if (infoArr.Length == 3)
                    {
                        var qrWxOgID = infoArr[0];
                        var qrNewsId = Utils.StrToInt(infoArr[1],0);
                        var qrSourceId = Utils.StrToInt(infoArr[2],0);
                        if (!string.IsNullOrWhiteSpace(qrWxOgID) && qrNewsId > 0 && qrSourceId > 0&&qrSourceId == source_id)
                        {
                            t_wx_news_materials qrNews = wx_news_materials.GetModel(qrNewsId);//图文id
                            if (qrNews != null)
                            {
                                var qrArticalList = new List<Article>();
                                qrArticalList.Add(new Article()
                                {
                                    Description = qrNews.summary,
                                    PicUrl = (qrNews.cover_img.Contains("http") == true ? "" : _config.domain) + qrNews.cover_img,
                                    Title = qrNews.title,
                                    Url = getNewsUrl(qrNews.push_type, qrNews.link_url, qrNews.app_link, _config.domain, _config.news_templat_path, qrNews.id, openId, wx_og_id)
                                });
                                if (qrNews.channel_id == (int)ResponseNewsType.多图文)
                                {
                                    //取出子级图文

                                    List<t_wx_news_materials> list = wx_news_materials.GetChildList(qrNews.id);
                                    if (list != null)
                                    {
                                        foreach (t_wx_news_materials item in list)
                                        {
                                            qrArticalList.Add(new Article()
                                            {
                                                Description = item.summary,
                                                PicUrl = (item.cover_img.Contains("http") == true ? "" : _config.domain) + item.cover_img,
                                                Title = item.title,
                                                Url = getNewsUrl(item.push_type, item.link_url, item.app_link, _config.domain, _config.news_templat_path, item.id, openId, wx_og_id)
                                            });
                                        }
                                    }
                                }
                                Custom.SendNews(accesstoken, openId, qrArticalList);
                            }
                        }
                    }
                });
            }
            #endregion

 

        


            #region 添加一条聊天记录
            KDWechat.BLL.Logs.wx_fans_chats.CreateSubscribeFansChat(new t_wx_fans_chats()
            {
                contents = "关注成功！",
                create_time = DateTime.Now,
                from_type = (int)FromUserType.用户,
                is_sys_auto_reply = 0,
                media_id = "",
                msg_type = (int)msg_type.文本,
                open_id = requestMessage.FromUserName,
                wx_id = wechat.id,
                wx_og_id = requestMessage.ToUserName
            });
            #endregion

            

            bool is_aotu_replay = true;


            if (is_aotu_replay)
            {
                #region 正常关注自动回复

                t_wx_basic_reply model = KDWechat.BLL.Chats.wx_basic_reply.GetModel(wechat.id, wechat.wx_og_id, (int)AutoReply.关注时);
                if (model != null)
                {                    
                    #region 判断回复类型


                    switch (model.reply_type)
                    {
                        case (int)msg_type.文本:

                            var responseMessageText = base.CreateResponseMessage<ResponseMessageText>();
                            responseMessage = responseMessageText;
                            string content = model.contents.Replace("<br />", "<br>");
                            responseMessageText.Content = Common.Utils.DropHTMLOnly(content.Replace("<br>", "\n").Replace("&nbsp;", " "));
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
                                    Url = getUrl(_config.domain, _config.video_templat_path, m.id, openId, wx_og_id)
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
                else
                {
                    var responseText = base.CreateResponseMessage<ResponseMessageText>();
                    responseText.Content = "";
                    responseMessage = responseText;
                }
                #endregion
            }







            return responseMessage;
        }

        /// <summary>
        /// 退订
        /// 实际上用户无法收到非订阅账号的消息，所以这里可以随便写。
        /// unsubscribe事件的意义在于及时删除网站应用中已经记录的OpenID绑定，消除冗余数据。并且关注用户流失的情况。
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_UnsubscribeRequest(RequestMessageEvent_Unsubscribe requestMessage)
        {
            var fans = KDWechat.BLL.Users.wx_fans.GetFansByID(requestMessage.FromUserName);
            if (null != fans)
            {
                fans.status = 0;
                fans.remove_time = DateTime.Now;
                KDWechat.BLL.Users.wx_fans.UpdateFans(fans);

                //为明天取消关注转发  Damos -add at 2015-3-24 16:20

                var wmtOgID = System.Configuration.ConfigurationManager.AppSettings["WMTOgID"] ?? "gh_ca90998bc552";


                if (fans.wx_og_id == wmtOgID)
                {
                    var wmtApiUrl = ConfigurationManager.AppSettings["WMTApiUrl"].ToString() + "?mt=removeFans";
                    var byts = System.Text.Encoding.UTF8.GetBytes(fans.open_id);
                    var memoryStream = new System.IO.MemoryStream(byts);
                    BaiDuMapAPI.HttpUtility.RequestUtility.HttpPost(wmtApiUrl, memoryStream);
                }



            }

            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "退订成功！";
            return responseMessage;
        }
    }
}