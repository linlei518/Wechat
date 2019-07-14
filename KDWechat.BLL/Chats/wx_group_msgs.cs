using KDWechat.DAL;
using KDWechat.Common;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.CommonAPIs;
using System;
using System.Collections.Generic;
using System.Text;
using EntityFramework.Extensions;
using System.Web;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities;
using System.Linq;
using KDWechat.BLL.Entity;


namespace KDWechat.BLL.Chats
{
    public class wx_group_msgs
    {

        #region 外部方法

        /// <summary>
        /// 发送定时消息
        /// </summary>
        public static int SendTmerMsg(string path = "",string appDomain="")
        {
            int count = 0;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                //查找所有应当转发的消息
                var list = db.t_wx_group_msgs.Where(x => x.is_timer == (int)is_timerMode.是 && x.send_time > DateTime.Now && x.is_send == (int)is_sendMode.否 && x.is_check == (int)GroupMsgCheckMode.已审核).ToList();
                foreach (var x in list)
                {
                    var span = x.send_time - DateTime.Now;//取时间差
                    if (span <= TimeSpan.FromMinutes(3) && span >= TimeSpan.FromSeconds(0))//3分钟内
                    {
                        int i = 0;//尝试次数
                        while (true)
                        {
                            if (i == 3)//三次失败则跳出
                                break;
                            var ok = SendGroupMsg(x, path,appDomain).Item1;//尝试发送
                            if (!ok)//发送失败，尝试次数+1，继续
                            {
                                i++;
                                continue;
                            }
                            else//发送成功，置为已发，跳出
                            {
                                //  x.is_timer = (int)is_timerMode.否;
                                x.is_send = (int)is_sendMode.是;
                                break;
                            }

                        }
                    }
                }
                if (list != null)
                {
                    count = list.Count;
                }
                db.SaveChanges();
            }
            return count;
        }

        /// <summary>
        /// 删除群发
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static t_wx_group_msgs DeleteMsg(int id)
        {
            t_wx_group_msgs msg = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                msg = db.t_wx_group_msgs.Where(x => x.id == id).FirstOrDefault();
                if (msg != null)
                {
                    if(!string.IsNullOrWhiteSpace(msg.msg_id))
                    {
                        var accessToken = Chats.wx_wechats.GetAccessToken(msg.wx_id);
                        var arr = msg.msg_id.TrimStart(',').TrimEnd(',').Split(',').ToList();
                        arr.ForEach(x =>
                        {
                            DeleteSentGroupMsg(accessToken, x);
                        });
                    }
                    msg.is_send = (int)is_sendMode.已删除;
                    //db.t_wx_group_msgs.Remove(msg);
                    db.SaveChanges();
                }
            }
            return msg;
        }


        /// <summary>
        /// 发送预览
        /// </summary>
        /// <param name="msgType">素材类型</param>
        /// <param name="openID">接收预览的openid</param>
        /// <param name="wx_id">发送预览的公众号id</param>
        /// <param name="material_id">素材ID（文本素材请不要填此项或直接给0）</param>
        /// <param name="content">文本内容（media、图文素材请不要填此项）</param>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static Tuple<bool,string> OverView(msg_type msgType, string openID, int wx_id, int material_id = 0, string content = "",string path="")
        {

            var msgtype = "";

            if (material_id == 0 && content == "")
                return new Tuple<bool, string>(false, "请不要发送空内容");

            string accessToken = wx_wechats.GetAccessToken(wx_id);
            switch (msgType)
            {
                case msg_type.单图文:
                case msg_type.多图文:
                    msgtype = "mpnews";
                    t_wx_news_materials multi = KDWechat.BLL.Chats.wx_news_materials.GetModel(material_id);
                    if (multi == null)
                        return new Tuple<bool, string>(false, "素材已不存在，请刷新后再试");
                    UploadResultJson upResult = null;
                    try
                    {
                        upResult = UpNewsCover(accessToken, multi.cover_img, path);                        
                    }
                    catch (Exception e) {
                        return new Tuple<bool, string>(false, "素材上传失败，请稍后再试");
                    }
                    var wechatConfig = new Config.wechat_config().loadConfig();
                    if (string.IsNullOrEmpty(multi.contents) || !string.IsNullOrEmpty(multi.link_url))
                        return new Tuple<bool, string>(false, "首篇图文内容为空或关联了外链");
                    NewsModelWithoutTopicPic parent = null;
                    try
                    {
                        parent = new NewsModelWithoutTopicPic()
                        {
                            author = multi.author,
                            content = GetChangedContents(accessToken, multi.contents, path),
                            title = multi.title,
                            content_source_url = multi.source_url == null ? "" : multi.source_url.Replace("http://", "").Replace("https://", ""),
                            digest = Utils.DropHTML(multi.summary),
                            thumb_media_id = upResult.media_id,
                            show_cover_pic = "0"
                        };
                    }
                    catch(Exception exxx)
                    {
                        Companycn.Core.EntityFramework.EFHelper.AddModel<creater_wxEntities, t_wx_error_logs>(new t_wx_error_logs { add_time = DateTime.Now, login_name = "Damos", user_id = 0, content = exxx.Message });
                        return new Tuple<bool, string>(false, "根据腾讯群发规则，用于群发的图文内容中仅支持小于1MB的.jpg,.png图片，请检查您首篇图文中的图片格式");
                    }
                    string child_str = string.Empty;
                    List<t_wx_news_materials> list = KDWechat.BLL.Chats.wx_news_materials.GetChildList(multi.id);
                    NewsModelWithoutTopicPic[] lis = new NewsModelWithoutTopicPic[1];
                    if (list != null)
                    {
                        lis = new NewsModelWithoutTopicPic[list.Count + 1];
                        for (int i = 0; i < list.Count; i++)
                        {
                            var child = list[i];
                            try
                            {
                                upResult = UpNewsCover(accessToken, child.cover_img, path);
                            }
                            catch (Exception e)
                            {
                                Companycn.Core.EntityFramework.EFHelper.AddModel<creater_wxEntities, t_wx_error_logs>(new t_wx_error_logs { add_time = DateTime.Now, login_name = "Damos", user_id = 0, content = e.Message });
                                return new Tuple<bool, string>(false, "素材上传失败，请稍后再试");
                            }
                            if (string.IsNullOrEmpty(child.contents) || !string.IsNullOrEmpty(child.link_url))
                                return new Tuple<bool, string>(false, "子图文内容为空或关联了外链");
                            try
                            {
                                lis[i + 1] = new NewsModelWithoutTopicPic()
                                {
                                    author = child.author,
                                    content = GetChangedContents(accessToken, child.contents, path),
                                    title = child.title,
                                    content_source_url = child.source_url == null ? "" : child.source_url.Replace("http://", "").Replace("https://", ""),
                                    digest = Utils.DropHTML(child.summary),
                                    thumb_media_id = upResult.media_id,
                                    show_cover_pic = "0"
                                };
                            }                                 
                            catch(Exception exxx)
                            {
                                Companycn.Core.EntityFramework.EFHelper.AddModel<creater_wxEntities, t_wx_error_logs>(new t_wx_error_logs { add_time = DateTime.Now, login_name = "Damos", user_id = 0, content = exxx.StackTrace });
                                return new Tuple<bool, string>(false, "根据腾讯群发规则，用于群发的图文内容中仅支持小于1MB的.jpg,.png图片，请检查您子图文中的图片格式");
                            }
                        }
                    }
                    lis[0] = parent;
                    UploadMediaFileResult result = null;
                    try
                    {
                        result = UploadNews(accessToken, lis);
                    }
                    catch (Exception e)
                    {
                        Companycn.Core.EntityFramework.EFHelper.AddModel<creater_wxEntities, t_wx_error_logs>(new t_wx_error_logs { add_time = DateTime.Now, login_name = "Damos", user_id = 0, content = e.Message });
                        return new Tuple<bool, string>(false, "素材上传失败，请稍后再试");
                    }
                    if (result == null || string.IsNullOrEmpty(result.media_id))
                        return new Tuple<bool, string>(false, "素材上传失败，请稍后再试");
                    try
                    {
                        var previewOk = SendPreview(result.media_id, openID, msgtype, accessToken);
                        return new Tuple<bool, string>(previewOk, previewOk ? "预览发送成功" : "预览发送失败，请稍后再试");
                    }
                    catch (Exception e)
                    {
                        Companycn.Core.EntityFramework.EFHelper.AddModel<creater_wxEntities, t_wx_error_logs>(new t_wx_error_logs { add_time = DateTime.Now, login_name = "Damos", user_id = 0, content = e.Message });
                        return new Tuple<bool, string>(false, "预览发送失败，请稍后再试");
                    }

                case msg_type.图片:
                    msgtype = "image";
                    t_wx_media_materials imgMaterial = wx_media_materials.GetMediaMaterialByID(material_id);//获取多媒体素材
                    if (imgMaterial == null)
                        return new Tuple<bool, string>(false, "素材已不存在，请刷新后再试");
                    string title = imgMaterial.title;
                    string fullpath = "";
                    if (imgMaterial.file_url.Contains("http://") || imgMaterial.file_url.Contains("https://"))
                    {
                        using (System.Net.WebClient wc = new System.Net.WebClient())
                        {
                            try
                            {
                                fullpath = System.Web.HttpContext.Current.Server.MapPath("~/upload/" + System.IO.Path.GetFileName(imgMaterial.file_url));//获取素材文件地址
                            }
                            catch
                            {
                                fullpath = path + "/upload/" + System.IO.Path.GetFileName(imgMaterial.file_url);
                            }
                            if (string.IsNullOrEmpty(System.IO.Path.GetExtension(fullpath)))
                                fullpath += ".jpg";
                            wc.Headers.Add("User-Agent", "Chrome");
                            wc.DownloadFile(imgMaterial.file_url, fullpath);
                        }
                    }
                    else
                    {
                        try
                        {
                            fullpath = System.Web.HttpContext.Current.Server.MapPath(imgMaterial.file_url);//获取素材文件地址
                        }
                        catch
                        {
                            fullpath = path + imgMaterial.file_url;
                        }
                    }
                    UploadMediaFileType mediaType = UploadMediaFileType.image;
                    try
                    {
                        UploadResultJson imgResult = Media.Upload(accessToken, mediaType, fullpath);
                        var previewOk = SendPreview(imgResult.media_id, openID, msgtype, accessToken);
                        return new Tuple<bool, string>(previewOk, previewOk ? "预览发送成功" : "预览发送失败，请稍后再试");
                    }
                    catch (Exception)
                    {
                        return new Tuple<bool, string>(false, "预览发送失败，请稍后再试");
                    }
                case msg_type.语音:
                    msgtype = "voice";
                    t_wx_media_materials material = wx_media_materials.GetMediaMaterialByID(material_id);//获取多媒体素材
                    if (material == null)
                        return new Tuple<bool, string>(false, "素材已不存在，请刷新后再试");
                    try
                    {
                        fullpath = System.Web.HttpContext.Current.Server.MapPath(material.file_url);//获取素材文件地址
                    }
                    catch
                    {
                        fullpath = path + material.file_url;
                    }
                    UploadMediaFileType voiceType = UploadMediaFileType.voice;
                    try
                    {
                        UploadResultJson voiceResult = Media.Upload(accessToken, voiceType, fullpath);
                        var previewOk = SendPreview(voiceResult.media_id, openID, msgtype, accessToken);
                        return new Tuple<bool, string>(previewOk, previewOk ? "预览发送成功" : "预览发送失败，请稍后再试");
                    }
                    catch (Exception)
                    {
                        return new Tuple<bool, string>(false, "预览发送失败，请稍后再试");
                    }
                case msg_type.视频:
                    msgtype = "mpvideo";
                    t_wx_media_materials videoMaterial = wx_media_materials.GetMediaMaterialByID(material_id);//获取多媒体素材
                    if (videoMaterial == null)
                        return new Tuple<bool, string>(false, "素材已不存在，请刷新后再试");
                    try
                    {
                        fullpath = System.Web.HttpContext.Current.Server.MapPath(videoMaterial.file_url);//获取素材文件地址
                    }
                    catch
                    {
                        fullpath = path + videoMaterial.file_url;
                    }
                    try
                    {
                        UploadResultJson videoResult = Media.Upload(accessToken, UploadMediaFileType.image, fullpath);
                        NewsModel videoModel = new NewsModel()
                        {
                            author = "",
                            content = "<iframe src=\"" + videoMaterial.file_url + "\" scrolling=\"no\" style=\"width: 100%; height: 100%;\" frameborder=\"0\"></iframe>",
                            title = videoMaterial.title,
                            content_source_url = videoMaterial.file_url,
                            digest = videoMaterial.remark,
                            thumb_media_id = videoResult.media_id
                        };
                        UploadMediaFileResult video_file_Result = null;
                        try
                        {
                            video_file_Result = Media.UploadNews(accessToken, videoModel);
                        }
                        catch (Exception)
                        {
                            return new Tuple<bool, string>(false, "预览发送失败，请稍后再试");
                        }
                        if (video_file_Result == null || string.IsNullOrEmpty(video_file_Result.media_id))
                            return new Tuple<bool, string>(false, "预览发送失败，请稍后再试");
                        try
                        {
                            var previewOk = SendPreview(video_file_Result.media_id, openID, msgtype, accessToken);
                            return new Tuple<bool, string>(previewOk, previewOk ? "预览发送成功" : "预览发送失败，请稍后再试");
                        }
                        catch (Exception)
                        {
                            return new Tuple<bool, string>(false, "预览发送失败，请稍后再试");
                        }
                    }
                    catch (Exception)
                    {
                        return new Tuple<bool, string>(false, "预览发送失败，请稍后再试");
                    }
                case msg_type.文本:
                    {
                        msgtype = "text";
                        var previewOk = SendPreview("", openID, msgtype, accessToken, Utils.ChangeToWeChatEmotion(content));
                        return new Tuple<bool, string>(previewOk, previewOk ? "预览发送成功" : "预览发送失败，请稍后再试");
                    }
                default:
                    return new Tuple<bool, string>(false, "请重新选择您需要发送的素材");
            }

        }


        /// <summary>
        /// 获取可以群发的条数
        /// </summary>
        /// <param name="wxID"></param>
        /// <returns></returns>
        public static int GetSendNo(int wxID)
        {
            int canSendNo = 0;
            if (wxID != 0)
            {
                DateTime now = DateTime.Now;
                DateTime d1 = new DateTime(now.Year, now.Month, 1);
                DateTime d2 = d1.AddMonths(1).AddDays(-1);
                using (creater_wxEntities db = new creater_wxEntities())
                {
                    canSendNo = db.t_wx_group_msgs.Where(x => x.send_time >= d1 && x.send_time <= d2 && x.wx_id == wxID && x.is_send == (int)is_sendMode.是).Count();
                }
            }
            return canSendNo;
        }

        #region 添加一条群发



        /// <summary>
        /// 发送群发消息
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static Tuple<bool,string> SendGroupMsg(t_wx_group_msgs msg, string path = "",string appDomain="")
        {

            var wechat = wx_wechats.GetWeChatByID(msg.wx_id);
            if (null == wechat)
                return new Tuple<bool, string>(false, "您的账号状态已改变，请重新登录");

            var opList = new List<string[]>();//按照10000分组
            var allOpidList = msg.openIDs.Split(',');//取所有opid
            var maxSendCount =10000;//最大发送人数

            bool sendAll = (msg.is_send_all ?? 0) == 1;
            var msgtype = "";

            if (!sendAll)
            {
                var sendTimes = allOpidList.Length / maxSendCount;//需要发送的次数
                var restCount = allOpidList.Length % maxSendCount;//除不尽的剩余数量
                if (restCount != 0)//如果除不尽
                    sendTimes++;//增加一次发送
                for (int m = 0; m < sendTimes; m++)
                {
                    var arrayLength = maxSendCount;//发送人数数组的长度
                    if (m == sendTimes - 1 && restCount != 0)//如果是最后一次，并且剩余人数不为0
                        arrayLength = restCount;//最后的数组数量
                    var tempArray = new string[arrayLength];//新建数组
                    System.Array.Copy(allOpidList, m * maxSendCount, tempArray, 0, arrayLength);//拷贝数组内容
                    opList.Add(tempArray);//添加到发送列表
                }
            }  

            string accessToken = "";

            accessToken = BLL.Chats.wx_wechats.GetAccessToken(wechat.id, wechat);
            if (accessToken.Contains("Error:"))
                return new Tuple<bool,string>(false,"公众账号配置填写错误！");
                
            #region 群发处理
            if (msg.msg_type == (int)msg_type.文本)
            {
                msgtype = "text";
                try
                {
                    if (sendAll)
                    {
                        var result = SendGroupMessageByGroup(accessToken, "", "", Common.Utils.ChangeToEditorEmotion(msg.contents));
                        return new Tuple<bool, string>(result.errcode == 0, result.errcode == 0 ? "发送成功" : "发送失败");
                    }
                    else
                    {
                        var successCount = 0;
                        opList.ForEach(x =>
                        {
                            var result = SendGroupMessageByOpenId(accessToken,msgtype,"",x,Common.Utils.ChangeToWeChatEmotion(msg.contents));
                            successCount += result.errcode==0 ? 1 : 0;
                        });
                        return new Tuple<bool, string>(successCount > 0, successCount > 0 ? "发送成功" : "发送失败");
                    }
                }
                catch (Exception)
                {
                    return new Tuple<bool,string>(false,"发送失败，请检查您的认证状态后重试");
                }
            }
            else if (msg.msg_type == (int)msg_type.单图文 || msg.msg_type == (int)msg_type.多图文)
            {
                msgtype = "mpnews";
                t_wx_news_materials multi = KDWechat.BLL.Chats.wx_news_materials.GetModel(msg.source_id ?? 0);//获取图文
                if (multi == null)
                    return new Tuple<bool, string>(false, "用以群发的图文已被删除，请刷新后重试");
                UploadResultJson upResult = UpNewsCover(accessToken, multi.cover_img, path);
                if (upResult == null)
                    return new Tuple<bool,string>(false,"微信文件上传失败，请稍后重试");

                Common.Config.wechatconfig wechatConfig;
                if (appDomain == "")
                    wechatConfig = new Config.wechat_config().loadConfig();
                else
                    wechatConfig = new Common.Config.wechatconfig() { domain = appDomain };
                if (string.IsNullOrEmpty(multi.contents) || !string.IsNullOrEmpty(multi.link_url))
                    return new Tuple<bool,string>(false,"首篇图文内容为空或关联了外链，根据腾讯群发规则，此篇图文无法群发");
                NewsModelWithoutTopicPic parent = null;
                try
                {
                    parent = new NewsModelWithoutTopicPic()
                    {
                        author = multi.author,
                        content = GetChangedContents(accessToken, multi.contents, path),
                        title = multi.title,
                        content_source_url = multi.source_url == null ? "" : multi.source_url.Replace("http://", "").Replace("https://", ""),
                        digest = Utils.DropHTML(multi.summary),
                        thumb_media_id = upResult.media_id,
                        show_cover_pic = "0"
                    };
                }
                catch
                {
                    return new Tuple<bool,string>(false,"根据腾讯群发规则，用于群发的图文内容中仅支持小于1MB的.jpg,.png图片，请检查您首篇图文中的图片格式");
                }
                List<t_wx_news_materials> child_news_list = KDWechat.BLL.Chats.wx_news_materials.GetChildList(multi.id);
                NewsModelWithoutTopicPic[] lis = new NewsModelWithoutTopicPic[1];
                if (child_news_list != null)
                {
                    lis = new NewsModelWithoutTopicPic[child_news_list.Count + 1];
                    for (int i = 0; i < child_news_list.Count; i++)//循环子图文
                    {
                        var child = child_news_list[i];
                        upResult = UpNewsCover(accessToken, child.cover_img, path);
                        if (upResult == null)
                            return new Tuple<bool, string>(false, "微信文件上传失败，请稍后重试");


                        if (string.IsNullOrEmpty(child.contents) || !string.IsNullOrEmpty(child.link_url))
                            return new Tuple<bool, string>(false, "子图文中包含空内容或关联了外链，根据腾讯群发规则，此篇图文无法群发");
                        try
                        {
                            lis[i + 1] = new NewsModelWithoutTopicPic()
                            {
                                author = child.author,
                                content = GetChangedContents(accessToken, child.contents, path),
                                title = child.title,
                                content_source_url = child.source_url == null ? "" : child.source_url.Replace("http://", "").Replace("https://", ""),
                                digest = Utils.DropHTML(child.summary),
                                thumb_media_id = upResult.media_id,
                                show_cover_pic = "0"
                            };
                        }
                        catch
                        {
                            return new Tuple<bool, string>(false, "根据腾讯群发规则，用于群发的图文内容中仅支持小于1MB的.jpg,.png图片，请检查您子图文中的图片格式");
                        }
                    }
                }
                lis[0] = parent;
                UploadMediaFileResult result = null;
                try
                {
                    result = UploadNews(accessToken, lis);
                }
                catch (Exception)
                {
                    return new Tuple<bool, string>(false, "微信文件上传失败，请稍后重试");
                }
                if (result == null || string.IsNullOrEmpty(result.media_id))
                    return new Tuple<bool, string>(false, "微信文件上传失败，请稍后重试");

                try
                {
                    if (sendAll)
                    {
                        var sendResult = SendGroupMessageByGroup(accessToken, msgtype, result.media_id, "");
                        msg.msg_id = sendResult.msg_id.ToString();
                        Companycn.Core.EntityFramework.EFHelper.UpdateModel<creater_wxEntities, t_wx_group_msgs>(msg);
                        return new Tuple<bool, string>(sendResult.errcode == 0, sendResult.errcode == 0 ? "发送成功" : "发送失败");
                    }
                    else
                    {
                        var successCount = 0;
                        wx_group_msg_result_model sendResult = null;
                        opList.ForEach(x =>
                        {
                            sendResult = SendGroupMessageByOpenId(accessToken,msgtype, result.media_id, x);
                            successCount += sendResult.errcode!=0 ? 0 : 1;
                            msg.msg_id += sendResult.msg_id + ",";
                        });
                        if (successCount > 0)
                        {
                            msg.msg_id = msg.msg_id.TrimEnd(',');
                            var relationStr = "";
                            relationStr += sendResult.msg_data_id + "_1:" + multi.id + "|";
                            if (child_news_list != null && child_news_list.Count > 0)
                            {
                                for (int i = 1; i <= child_news_list.Count; i++)
                                {
                                    relationStr += string.Format("{0}_{1}:{2}|", sendResult.msg_data_id, (i + 1).ToString(), child_news_list[i - 1].id);
                                }
                            }
                            msg.msgID_to_SouceID = relationStr.TrimEnd('|');
                            Companycn.Core.EntityFramework.EFHelper.UpdateModel<creater_wxEntities, t_wx_group_msgs>(msg);
                            return new Tuple<bool, string>(true, "发送成功");
                        }
                        return new Tuple<bool, string>(false, "群发请求发起失败，请稍后重试");
                    }
                }
                catch (Exception e)
                {
                    Companycn.Core.EntityFramework.EFHelper.AddModel<creater_wxEntities, t_wx_error_logs>(new t_wx_error_logs { add_time = DateTime.Now, login_name = "Damos", user_id = 0, content = e.Message });
                    return new Tuple<bool, string>(false, "群发请求发起失败，请稍后重试");
                }
            }
            #region 视频群发，已禁用
            //else if (msg.msg_type == (int)msg_type.视频)
            //{
            //    t_wx_media_materials material = wx_media_materials.GetMediaMaterialByID(msg.source_id ?? 0);//获取多媒体素材
            //    if (material == null)
            //        return new Tuple<bool, string>(false, "用于群发的素材已被删除，请刷新后重试");
            //    string fullpath = "";
            //    try
            //    {
            //        fullpath = System.Web.HttpContext.Current.Server.MapPath(material.file_url);//获取素材文件地址
            //    }
            //    catch
            //    {
            //        fullpath = path + material.file_url;
            //    }
            //    try
            //    {
            //        UploadResultJson upResult = Media.Upload(accessToken, UploadMediaFileType.image, fullpath);
            //        NewsModel parent = new NewsModel()
            //        {
            //            author = wechat.wx_name,
            //            content = "<iframe src=\"" + material.file_url + "\" scrolling=\"no\" style=\"width: 100%; height: 100%;\" frameborder=\"0\"></iframe>",
            //            title = material.title,
            //            content_source_url = material.file_url,
            //            digest = material.remark,
            //            thumb_media_id = upResult.media_id
            //        };
            //        UploadMediaFileResult result = null;
            //        try
            //        {
            //            result = Media.UploadNews(accessToken, parent);
            //        }
            //        catch (Exception)
            //        {
            //            return new Tuple<bool, string>(false, "微信文件上传失败，请稍后重试");
            //        }
            //        if (result != null && !string.IsNullOrEmpty(result.media_id))
            //            return new Tuple<bool, string>(true, "发送成功");
            //        return new Tuple<bool, string>(false, "群发请求发起失败，请稍后重试");
            //    }
            //    catch (Exception)
            //    {
            //        return new Tuple<bool, string>(false, "群发请求发起失败，请稍后重试");
            //    }
            //}
            #endregion
            else
            {
                t_wx_media_materials material = wx_media_materials.GetMediaMaterialByID(msg.source_id ?? 0);//获取多媒体素材
                if (material == null)
                    return new Tuple<bool, string>(false, "用于群发的素材已被删除，请刷新后重试");
                string title = material.title;
                string fullpath = "";
                try
                {
                    fullpath = System.Web.HttpContext.Current.Server.MapPath(material.file_url);//获取素材文件地址
                }
                catch
                {
                    fullpath = path + material.file_url;
                }
                UploadMediaFileType mediaType = UploadMediaFileType.image;
                switch ((media_type)material.channel_id)
                {
                    case media_type.素材图片库:
                        mediaType = UploadMediaFileType.image;
                        msgtype = "image";
                        break;
                    case media_type.素材语音:
                        mediaType = UploadMediaFileType.voice;
                        msgtype = "voice";
                        break;
                }
                try
                {
                    UploadResultJson upResult = Media.Upload(accessToken, mediaType, fullpath);
                    if (sendAll)
                    {
                        var sendResult = SendGroupMessageByGroup(accessToken, msgtype, upResult.media_id, "");
                        return new Tuple<bool, string>(sendResult.errcode == 0, sendResult.errcode == 0 ? "发送成功" : "发送失败");
                    }
                    else
                    {
                        var successCount = 0;
                        opList.ForEach(x =>
                        {
                            var result = SendGroupMessageByOpenId(accessToken,msgtype, upResult.media_id, x);
                            successCount += result.errcode!=0 ? 0 : 1;
                        });
                        return new Tuple<bool, string>(successCount > 0, successCount > 0 ? "发送成功" : "发送失败");
                    }
                }
                catch (Exception)
                {
                    return new Tuple<bool, string>(false, "群发请求发起失败，请稍后重试");
                }
            }
            #endregion
        }

        /// <summary>
        /// 根据ID发送多媒体信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool SendGroupMsg(int id)
        {
            var msg = GetGroupMsgByID(id);
            return SendGroupMsg(msg).Item1;
        }

        #endregion

        #region 获取群发列表
        public static List<t_wx_group_msgs> GetGroupMsgListByWxID(int wxID, int pageindex, int pagesize, out int totalCount)
        {
            List<t_wx_group_msgs> msgList = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                var query = (from x in db.t_wx_group_msgs where x.wx_id == wxID && x.is_send!=(int)is_sendMode.已删除 orderby x.id descending select x);
                totalCount = query.Count();
                msgList = query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            }
            return msgList;
        }
        //总部账号管理页面
        public static List<wechat_groupmsg_view> GetGroupMsgList(int pageindex, int pagesize, out int totalCount)
        {
            List<wechat_groupmsg_view> msgList = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                var query = (from x in db.wechat_groupmsg_view orderby x.id descending select x).Skip((pageindex - 1) * pagesize).Take(pagesize);
                totalCount = query.Count();
                msgList = query.ToList();
            }
            return msgList;
        }
        #endregion

        #region 删除群发
        /// <summary>
        /// 删除群发
        /// </summary>
        /// <param name="id">需要删除的群发ID</param>
        /// <returns>删除是否成功</returns>
        public bool DeleteGroupMsgsByID(int id)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isFinish = db.t_wx_group_msgs.Where(x => x.id == id).Delete() > 0;
            }
            return isFinish;
        }
        #endregion

        #endregion

        //添加一条群发
        public static t_wx_group_msgs InsertGroupMsg(t_wx_group_msgs msg)
        {
            return EFHelper.AddWeChat<t_wx_group_msgs>(msg);
        }

        /// <summary>
        /// 通过ID获取群发信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static t_wx_group_msgs GetGroupMsgByID(int id)
        {
            t_wx_group_msgs msg = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                msg = (from x in db.t_wx_group_msgs where x.id == id select x).FirstOrDefault();
            }
            return msg;
        }

        /// <summary>
        /// 更新一条
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static t_wx_group_msgs UpdateGroupMsg(t_wx_group_msgs msg)
        {
            return EFHelper.UpdateWeChat<t_wx_group_msgs>(msg);
        }

        #region MP增强

        #region OPID群发
        /// <summary>
        /// 根据OpenId进行群发
        /// </summary>
        /// <param name="accessTokenOrAppId"></param>
        /// <param name="value">群发媒体文件时传入mediaId,群发文本消息时传入content,群发卡券时传入cardId</param>
        /// <param name="type"></param>
        /// <param name="openIds">openId字符串数组</param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <returns></returns>
        public static wx_group_msg_result_model SendGroupMessageByOpenId(string accessToken, string title, string media_id,string[] openIDs, string contents = "")
        {
            var url = "https://api.weixin.qq.com/cgi-bin/message/mass/send?access_token=" + accessToken;
            string data = "";
            if (title == "text")
            {
                data = "{\"touser\":[\"" + Utils.GetArrayStr(openIDs, "\",\"") + "\"],\"" + title + "\":{\"content\":\"" + contents + "\"},\"msgtype\":\"" + title + "\"}";
            }
            else
            {
                data = "{\"touser\":[\"" + Utils.GetArrayStr(openIDs, "\",\"") + "\"],\"" + title + "\":{\"media_id\":\"" + media_id + "\"},\"msgtype\":\"" + title + "\"}";
            }
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            var ms = new System.IO.MemoryStream(bytes);

            string result = BaiDuMapAPI.HttpUtility.RequestUtility.HttpPost(url, ms);

            Companycn.Core.EntityFramework.EFHelper.AddModel<creater_wxEntities, t_wx_error_logs>(new t_wx_error_logs { add_time = DateTime.Now, login_name = "Damos", user_id = 0, content = result });

            var result_model = Newtonsoft.Json.JsonConvert.DeserializeObject<wx_group_msg_result_model>(result);
            return result_model;
        }
        #endregion

        #region group群发
        /// <summary>
        /// 根据分组群发
        /// </summary>
        /// <param name="accessToken">公众账号对应的accesstoken</param>
        /// <param name="title">群发的类型</param>
        /// <param name="media_id">对应类型文件的media_id</param>
        /// <param name="contents">文本类型的文本</param>
        /// <returns></returns>
        static wx_group_msg_result_model SendGroupMessageByGroup(string accessToken, string title, string media_id, string contents = "")
        {

            var url = "https://api.weixin.qq.com/cgi-bin/message/mass/sendall?access_token="+accessToken;
            string data = "";
            string groupData = "";
            //if (group_id > 0)
                groupData = ",\"group_id\":\"0\"";
            if (title == "text")
            {
                data = "{\"filter\":{\"is_to_all\":true" + groupData + "},\"text\":{\"content\":\"" + contents + "\"},\"msgtype\":\"text\"}";
            }
            else
            {
                data = "{\"filter\":{\"is_to_all\":true" + groupData + "},\"" + title + "\":{\"media_id\":\"" + media_id + "\"},\"msgtype\":\"" + title + "\"}";
            }
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            var ms = new System.IO.MemoryStream(bytes);

            string result = BaiDuMapAPI.HttpUtility.RequestUtility.HttpPost(url, ms);

            Companycn.Core.EntityFramework.EFHelper.AddModel<creater_wxEntities, t_wx_error_logs>(new t_wx_error_logs { add_time = DateTime.Now, login_name = "Damos", user_id = 0, content = result });

            var result_model = Newtonsoft.Json.JsonConvert.DeserializeObject<wx_group_msg_result_model>(result);
            return result_model;

            //var isOk = result.ToLower().Contains("success");
            //Companycn.Core.EntityFramework.EFHelper.AddModel<creater_wxEntities, t_wx_error_logs>(new t_wx_error_logs { add_time = DateTime.Now, login_name = "Damos", user_id = 0, content = result });
            //return isOk ? new SendResult { msg_id = "100", errmsg = "success" } : new SendResult { msg_id = string.Empty };

            //Companycn.Core.EntityFramework.EFHelper.AddModel<creater_wxEntities, t_wx_error_logs>(new t_wx_error_logs { add_time = DateTime.Now, login_name = "Damos", user_id = 0, content = Newtonsoft.Json.JsonConvert.SerializeObject(data) });
            //return new SendResult { msg_id = "123" };
            //return CommonJsonSend.Send<SendResult>(accessToken, url, data);
        }
        #endregion

        #region 群发预览
        /// <summary>
        /// 发送预览
        /// </summary>
        /// <param name="media_id">mediaID</param>
        /// <param name="open_id">openID</param>
        /// <param name="title">msgtype</param>
        /// <param name="accessToken">accessToken</param>
        /// <param name="contents">文本内容</param>
        /// <returns></returns>
        static bool SendPreview(string media_id, string open_id, string title,string accessToken,string contents="")
        {
            bool isOk = false;
            var url = "https://api.weixin.qq.com/cgi-bin/message/mass/preview?access_token=" + accessToken;
            string data = "";
            if (title == "text")
            {
                data = "{\"touser\":\""+open_id+"\",\"text\":{\"content\":\""+contents+"\"},\"msgtype\":\"text\"}";
            }
            else
            {
                data = "{\"touser\":\""+open_id+"\",\""+title+"\":{\"media_id\":\""+media_id+"\"},\"msgtype\":\""+title+"\"}";
            }

            byte[] bytes = Encoding.UTF8.GetBytes(data);
            var ms = new System.IO.MemoryStream(bytes);

            string result = BaiDuMapAPI.HttpUtility.RequestUtility.HttpPost(url,ms);
            isOk = result.ToLower().Contains("success");
            return isOk;
        }
        #endregion
        
        #region 群发素材上传
        static UploadMediaFileResult UploadNews(string accessToken, NewsModelWithoutTopicPic[] news)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/media/uploadnews?access_token=" + accessToken;
            var data = new
            {
                articles = news
            };
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            byte[] bytes = Encoding.UTF8.GetBytes(jsonString);
            var ms = new System.IO.MemoryStream(bytes);

            string result = BaiDuMapAPI.HttpUtility.RequestUtility.HttpPost(url, ms);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<UploadMediaFileResult>(result);
        }


        public static upTemp UploadTemporaryMedia(string accessToken, string file)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/media/uploadimg?access_token=" + accessToken;
            var fileDictionary = new Dictionary<string, string>();
            fileDictionary["media"] = file;
            return Senparc.Weixin.HttpUtility.Post.PostFileGetJson<upTemp>(url, null, fileDictionary, null);
        }

        public static string GetChangedContents(string accessToken,string input,string path)
        {
            var changedContent = input;
            var imgList = Utils.GetHtmlImageSrcList(input);
            for (int i = 0; i < imgList.Length; i++)
            {
                var currentImg = imgList[i];
                string fullPath = "";
                if (currentImg.ToLower().Contains("http://") || currentImg.ToLower().Contains("https://"))
                {
                    using (System.Net.WebClient wc = new System.Net.WebClient())
                    {
                        try
                        {
                            fullPath = System.Web.HttpContext.Current.Server.MapPath("~/upload/" + System.IO.Path.GetFileName(currentImg));//获取素材文件地址
                        }
                        catch
                        {
                            fullPath = path + "/upload/" + System.IO.Path.GetFileName(fullPath);
                        }
                        if (string.IsNullOrEmpty(System.IO.Path.GetExtension(fullPath)))
                            fullPath += ".jpg";
                        wc.Headers.Add("User-Agent", "Chrome");
                        wc.DownloadFile(currentImg, fullPath);
                    }
                }
                else
                {
                    fullPath = HttpContext.Current.Server.MapPath(currentImg);
                }
                Companycn.Core.EntityFramework.EFHelper.AddModel<creater_wxEntities, t_wx_error_logs>(new t_wx_error_logs { add_time = DateTime.Now, login_name = "Damos", user_id = 0, content = currentImg });

                currentImg = UploadTemporaryMedia(accessToken, fullPath).url;
                changedContent = changedContent.Replace(imgList[i], currentImg);
            }
            return changedContent;
        }

        public static UploadResultJson UpNewsCover(string accessToken,string img,string path="")
        {
            UploadResultJson upResult = null;
            try
            {
                string fullpath = "";
                if (img.Contains("http://") || img.Contains("https://"))
                {
                    using (System.Net.WebClient wc = new System.Net.WebClient())
                    {
                        try
                        {
                            fullpath = System.Web.HttpContext.Current.Server.MapPath("~/upload/" + System.IO.Path.GetFileName(img));//获取素材文件地址
                        }
                        catch
                        {
                            fullpath = path + "/upload/" + System.IO.Path.GetFileName(img);
                        }
                        if (string.IsNullOrEmpty(System.IO.Path.GetExtension(fullpath)))
                            fullpath += ".jpg";
                        wc.Headers.Add("User-Agent", "Chrome");
                        wc.DownloadFile(img, fullpath);
                    }
                }
                else
                {
                    try
                    {
                        fullpath = System.Web.HttpContext.Current.Server.MapPath(img);//获取素材文件地址
                    }
                    catch
                    {
                        fullpath = path + img;
                    }
                }
                upResult = Media.Upload(accessToken, UploadMediaFileType.image, fullpath);
            }
            catch
            {
                return null;
            }
            return upResult;
        }
        #endregion

        #region 删除群发
        static bool DeleteSentGroupMsg(string accessToken,string msg_id)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/message/mass/delete?access_token=" + accessToken;
            string data ="{\"msg_id\":\""+msg_id+"\"}";

            byte[] bytes = Encoding.UTF8.GetBytes(data);
            var ms = new System.IO.MemoryStream(bytes);

            string result = BaiDuMapAPI.HttpUtility.RequestUtility.HttpPost(url, ms);

            Companycn.Core.EntityFramework.EFHelper.AddModel<creater_wxEntities, t_wx_error_logs>(new t_wx_error_logs { add_time = DateTime.Now, login_name = "Damos", user_id = 0, content = result });

            //var result_model = Newtonsoft.Json.JsonConvert.DeserializeObject<wx_group_msg_result_model>(result);
            return result.ToLower().Contains("ok");
        }
        #endregion

        #region 已废弃方法
        ///// <summary>
        ///// 根据OpenId进行群发
        ///// </summary>
        ///// <param name="accessToken"></param>
        ///// <param name="mediaId">用于群发的消息的media_id</param>
        ///// <param name="openIds">openId字符串数组</param>
        ///// <returns></returns>
        //static SendResult SendGroupTextMessageByOpenId(string accessToken, string content, params string[] openIds)
        //{
        //    const string urlFormat = "https://api.weixin.qq.com/cgi-bin/message/mass/send?access_token={0}";

        //    var data = new
        //    {
        //        touser = openIds,
        //        text = new
        //        {
        //            content = content
        //        },
        //        msgtype = "text"
        //    };
        //    return CommonJsonSend.Send<SendResult>(accessToken, urlFormat, data);
        //}

        ///// <summary>
        ///// 群发媒体信息
        ///// </summary>
        ///// <param name="accessToken"></param>
        ///// <param name="mediaId"></param>
        ///// <param name="openIds"></param>
        ///// <returns></returns>
        //public static SendResult SendGroupMessageByOpenId(string accessToken, media_type mediaType, string mediaId, params string[] openIds)
        //{
        //    const string urlFormat = "https://api.weixin.qq.com/cgi-bin/message/mass/send?access_token={0}";

        //    if (mediaType == media_type.素材语音)
        //    {
        //        var data = new
        //        {
        //            touser = openIds,
        //            voice = new
        //            {
        //                media_id = mediaId
        //            },
        //            msgtype = "voice"
        //        };
        //        return CommonJsonSend.Send<SendResult>(accessToken, urlFormat, data);
        //    }
        //    else if (mediaType == media_type.素材图片库)
        //    {
        //        var data = new
        //        {
        //            touser = openIds,
        //            image = new
        //            {
        //                media_id = mediaId
        //            },
        //            msgtype = "image"
        //        };
        //        return CommonJsonSend.Send<SendResult>(accessToken, urlFormat, data);
        //    }
        //    return null;
        //}
        #endregion


        #endregion

    }


    public class upTemp
    {
        public string url { get; set; }
    }

    class NewsModelWithoutTopicPic
    {
        public String thumb_media_id { get; set; }
        public String author { get; set; }
        public String title { get; set; }
        public String content_source_url { get; set; }
        public String content { get; set; }
        public String digest { get; set; }
        public String show_cover_pic { get; set; }
    }
}
