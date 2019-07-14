using System;
using System.Collections.Generic;
using KDWechat.BLL.Chats;
using KDWechat.BLL.Logs;
using KDWechat.DAL;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.GoogleMap;
using Senparc.Weixin.MP.Helpers;
using BaiDuMapAPI;
using Newtonsoft.Json;
using System.Linq;

namespace KDWechat.MP
{
    public class LocationService
    {
        public IResponseMessageBase GetResponseMessage(RequestMessageLocation requestMessage)
        {
            var wechat = wx_wechats.GetWeChatByogID(requestMessage.ToUserName);
            if (null != wechat)
            {
                string address = requestMessage.Label;
                if (address.Trim().Length==0)
                {
                    string jsonStr = BaiDuGeocodingAPI.GetAddressByLocation(requestMessage.Location_X.ToString() + "," + requestMessage.Location_Y.ToString());
                    var forwardText = JsonConvert.DeserializeObject<GeocodingReverseResolved>(jsonStr);
                    address = forwardText.result.formatted_address;
                }
               

                t_wx_fans_hislocation his_location = new t_wx_fans_hislocation()
                {
                    address = address,
                    create_time = DateTime.Now,
                     lng= (decimal) requestMessage.Location_Y,
                    lat = (decimal)requestMessage.Location_X,
                    open_id = requestMessage.FromUserName,
                    wx_id = wechat.id,
                    wx_og_id = requestMessage.ToUserName
                };
                wx_fans_hislocation.CreateFansHislocation(his_location);

            }

            string reText = BaiDuLBS.GetNearBy(requestMessage.ToUserName,requestMessage.Location_X.ToString(),requestMessage.Location_Y.ToString(),wechat.lbs_radius.ToString());
            var RootText = JsonConvert.DeserializeObject<LBSNearBy>(reText);

            if (RootText.status == 0)
            {
                if (RootText.total > 0)
                {
                    var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageNews>(requestMessage);
                    Common.Config.wechatconfig _config = new BLL.Config.wechat_config().loadConfig();

                    var lbsList = RootText.contents;
                    lbsList = lbsList.OrderByDescending(x => x.distance).ToList();
                    var top = (from x in lbsList where x.w_top == "1" select x).FirstOrDefault();
                    if (top != null)
                    {
                        responseMessage.Articles.Add(new Article()
                        {
                            Description = top.contents,
                            PicUrl = _config.domain + top.ImgUrl,
                            Title = top.title,
                            Url = string.IsNullOrEmpty(top.w_url) ? getUrl(_config.domain, _config.lbs_templat_path, top.w_id, requestMessage.FromUserName) : top.w_url.Trim()//"http://www.baidu.com"
                        });
                        lbsList.Remove(top);
                    }


                    foreach (var s in lbsList)
                    {
                        responseMessage.Articles.Add(new Article()
                        {
                            Description = s.contents,
                            PicUrl = _config.domain+s.ImgUrl,
                            Title = s.title,
                            Url = string.IsNullOrEmpty(s.w_url) ? getUrl(_config.domain, _config.lbs_templat_path, s.w_id, requestMessage.FromUserName) : s.w_url.Trim()//"http://www.baidu.com"
                        });
                    }
                    return responseMessage;
                }
            }
            var responseMes = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMes.Content = "暂未找到附近的LBS信息";
            return responseMes;

        }


        private string getUrl(string domain, string templat_path, string id, string openId)
        {
            string URL = "";
            if (templat_path.Length > 0)
            {
                if (templat_path.Contains("http"))
                {
                    URL = templat_path + "?lbs=" + id + "&openId=" + openId;
                }
                else if (templat_path.Substring(0, 1) == "/")
                {
                    URL = domain + templat_path + "?lbs=" + id + "&openId=" + openId;
                }
                else
                {
                    URL = domain + "/" + templat_path + "?lbs=" + id + "&openId=" + openId;
                }
            }
            return URL;
        }
    }
}