using KDWechat.BLL.Entity.JsonResult;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using KDWechat.DAL;
using KDWechat.BLL.Chats;
using KDWechat.Common;

namespace KDWechat.Web.Relay.Api
{
    /// <summary>
    /// relay 的摘要说明
    /// </summary>
    public class relay : IHttpHandler
    {
        HttpRequest Request;
        HttpResponse Response;
        string appid;
        //string streamContent;

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

            Request = context.Request;
            Response = context.Response;
            var method = Request.QueryString["mt"];
            appid = Request.QueryString["appid"];


            if (string.IsNullOrWhiteSpace(method) || string.IsNullOrEmpty(appid))
                WriteError("参数错误！");




            switch (method)
            {
                case "getAccessToken":
                    GetAccessToken();
                    break;
                case "getJsTicketJsonP":
                    GetJsTicketJsonP();
                    break;
                case "getJsTicket":
                    GetJsTicket();
                    break;
                case "getSignatureJsonP":
                    GetSignatureJsonP();
                    break;
            }
        }

        private void GetSignatureJsonP()
        {
            Response.ContentType = "text/plain";//jsonP修改contentType
            var callbackFunName = Request["callbackparam"];
            var requestUrl = HttpUtility.UrlDecode(Request["url"]);
            var ticket = BLL.Chats.wx_wechats.GetJsTicket(appid);
            var jsonToWrite = "";
            var stramp = ((int)((DateTime.Now - DateTime.Parse("1970-1-1")).TotalSeconds)).ToString();
            var nonceStr = Utils.Number(6);
            var signature = Common.WeChatJsApi.GetSignature(nonceStr, ticket, stramp, requestUrl);
            if (ticket.Contains("Error"))
                jsonToWrite = JsonConvert.SerializeObject(new JsonErrorResult { msg = ticket, result = 0 });
            else
                jsonToWrite = JsonConvert.SerializeObject(new { data = new { signature = signature, nonceStr = nonceStr, stramp = stramp, url = requestUrl }, msg = "success", result = 1 });
            Response.Write(string.Format("{0}({1})", callbackFunName, jsonToWrite));
            Response.End();
        }

        private void GetJsTicket()
        {
            CheckPostMethod();
            var ticket = BLL.Chats.wx_wechats.GetJsTicket(appid);
            if (!ticket.Contains("Error"))
                WriteJsonResult<string>("success", ticket);
            else
                WriteError(ticket);
        }

        private void GetJsTicketJsonP()
        {
            Response.ContentType = "text/plain";//jsonP修改contentType
            var callbackFunName = Request["callbackparam"];
            var ticket = BLL.Chats.wx_wechats.GetJsTicket(appid);
            var jsonToWrite = "";
            if (ticket.Contains("Error"))
                jsonToWrite = JsonConvert.SerializeObject(new JsonErrorResult { msg = ticket, result = 0 });
            else
                jsonToWrite = JsonConvert.SerializeObject(new JsonDataResult<string> { data = ticket, msg = "success", result = 1 });
            Response.Write(string.Format("{0}({1})", callbackFunName,jsonToWrite));
            Response.End();
        }

        private void GetAccessToken()
        {
            CheckPostMethod();
            var wechat = Companycn.Core.EntityFramework.EFHelper.GetModel<creater_wxEntities, t_wx_wechats>(x => x.app_id == appid);
            if (wechat != null)
            {
                var accessToken = wx_wechats.GetAccessToken(wechat.id, wechat);
                WriteJsonResult<string>("success",DESEncrypt.Encrypt(accessToken));
            }
            else
                WriteError("此APPID并未被本平台托管");
        }

        void CheckPostMethod()
        {
            if (Request.HttpMethod.ToUpper() != "POST")
                WriteError("非法请求");
        }


        //输出一个值类型
        void WriteObj(ValueType obj)
        {
            Response.Write(obj);
            Response.End();
        }
        void WriteObj(string obj)
        {
            Response.Write(obj);
            Response.End();
        }


        //返回成功实例（性能一般）
        void WriteJsonResult(object entity, string msg)
        {
            Response.Write(JsonConvert.SerializeObject(new { data = entity, msg = msg, result = 1 }));
            Response.End();
        }

        //返回成功实例（泛型，性能较好）
        void WriteJsonResult<T>(string msg, T entity)
        {
            Response.Write(JsonConvert.SerializeObject(new JsonDataResult<T> { data = entity, msg = msg, result = 1 }));
            Response.End();
        }

        //返回成功信息
        void WriteSuccess(string successMsg)
        {
            Response.Write(JsonConvert.SerializeObject(new JsonErrorResult { msg = successMsg, result = 1 }));
            Response.End();
        }

        //返回错误
        void WriteError(string errorMsg)
        {
            Response.Write(JsonConvert.SerializeObject(new JsonErrorResult { msg = errorMsg, result = 0 }));
            Response.End();
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