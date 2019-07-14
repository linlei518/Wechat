using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KDWechat.Common;
using KDWechat.DAL;
using Newtonsoft.Json;
using KDWechat.BLL.Entity.JsonResult;
using KDWechat.BLL.Entity;

namespace KDWechat.Web.api
{
    /// <summary>
    /// fans 的摘要说明
    /// </summary>
    public class fans : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Clear();
            context.Response.ClearContent();
            context.Response.ClearHeaders();
            //禁止缓存
            context.Response.Expires = -1;//相对过期时间
            context.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);//绝对过期时间
            context.Response.CacheControl = "no-cache";
            context.Response.AddHeader("pragma", "no-cache");
            context.Response.AddHeader("cache-control", "private");
            context.Response.ContentType = "application/json";

            

            var Request = context.Request;
            var Response = context.Response;
            if (Request.HttpMethod != "POST")
                return;

            var nonce = Request.QueryString["nonce"];
            var sign = Request.QueryString["sign"];
            var stramp = Request.QueryString["stramp"];

            if (stramp.Length != 17)
                return;
            if (nonce.Length != 10)
                return;

            var strampTime = HttpApiAuth.GetDateStramp(stramp);
            if(strampTime<DateTime.Now.AddSeconds(-15))
                return;
            
            var wx_id = Utils.StrToInt(Request.QueryString["wid"],-1);
            if (wx_id <= 0)
                return;
            var token = Request.QueryString["token"];
            var ser = Companycn.Core.EntityFramework.EFHelper.GetModel<creater_wxEntities, t_retrans_server>(x => x.wx_id == wx_id && x.token == x.token);
            if (ser == null)
                return;
            if (!HttpApiAuth.Auth(token, strampTime, nonce, sign))
                return;

            var method = Request.QueryString["m"];
            var output = "";
            switch (method)
            {
                case "getOne":
                    output = GetOne(wx_id);
                    break;
                case "getListByPage":
                    output = GetList(wx_id);
                    break;
            }
            Response.Write(output);
        }

        private string GetList(int wx_id)
        {
            var pageIndex = RequestHelper.GetQueryInt("pageindex", -1);

            var resultCode = 0;
            var resultMsg = "查询失败";
            List<FansForApi> data = null;
            if (pageIndex != -1)
            {
                var fanList = Companycn.Core.EntityFramework.EFHelper.GetList<creater_wxEntities, t_wx_fans, int>(x => x.wx_id == wx_id, x => x.id, 10, 1, true);
                if (fanList != null&&fanList.Count>0)
                {
                    resultCode = 1;
                    resultMsg = "查询成功！";
                    data = new List<FansForApi>();
                    foreach (var fan in fanList)
                    {
                        var fanToAdd = new FansForApi()
                        {
                            headimgurl = fan.headimgurl,
                            guid = fan.guid,
                            language = fan.language,
                            nick_name = fan.nick_name,
                            open_id = fan.open_id,
                            status = fan.status == 1 ? "关注中" : "已取消关注",
                            subscribe_time = fan.subscribe_time,
                            wx_area = fan.wx_area,
                            wx_city = fan.wx_city,
                            wx_sex = ((WeChatSex)(fan.wx_sex ?? 0)).ToString(),
                            wx_country = fan.wx_country,
                            wx_province = fan.wx_province
                        };
                        data.Add(fanToAdd);
                    }
                }
                else
                {
                    resultMsg = "没有更多用户。";
                }

            }
            else
            {
                resultMsg = "页码不能为空。";
            }

            if (resultCode == 0)
                return JsonConvert.SerializeObject(new JsonErrorResult() { result = resultCode, msg = resultMsg });
            else
                return JsonConvert.SerializeObject(new JsonListResult<FansForApi> { result = resultCode, msg = resultMsg, data = data });
        }

        private string GetOne(int wx_id)
        {
            var opid = RequestHelper.GetQueryString("opid");
            var resultCode = 0;
            var resultMsg = "查询失败";
            FansForApi data = null;
            if (!string.IsNullOrEmpty(opid))
            {
                var fan = Companycn.Core.EntityFramework.EFHelper.GetModel<creater_wxEntities, t_wx_fans>(x => x.wx_id == wx_id && x.open_id == opid);
                if (fan != null)
                {
                    resultCode = 1;
                    resultMsg = "查询成功！";
                    data = new FansForApi()
                    {
                        headimgurl = fan.headimgurl,
                        guid = fan.guid,
                        language = fan.language,
                        nick_name = fan.nick_name,
                        open_id = fan.open_id,
                        status = fan.status == 1 ? "关注中" : "已取消关注",
                        subscribe_time = fan.subscribe_time,
                        wx_area = fan.wx_area,
                        wx_city = fan.wx_city,
                        wx_sex = ((WeChatSex)(fan.wx_sex ?? 0)).ToString(),
                        wx_country = fan.wx_country,
                        wx_province = fan.wx_province
                    };
                }
                else
                {
                    resultMsg = "暂无此人。";
                }
            }
            if (resultCode == 0)
                return JsonConvert.SerializeObject(new JsonErrorResult(){ result = resultCode, msg = resultMsg });
            else
                return JsonConvert.SerializeObject(new JsonDataResult<FansForApi> { result = resultCode, msg = resultMsg, data = data });
            
        }















        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}