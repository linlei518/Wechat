using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using KDWechat.DAL;
using System.Text;
using KDWechat.BLL.Entity.JsonResult;

namespace KDWechat.Web.api
{
    /// <summary>
    /// wmt_activity 的摘要说明
    /// </summary>
    public class wmt_activity : IHttpHandler
    {
        HttpRequest Request;
        HttpResponse Response;
        string streamContent;


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



            if (string.IsNullOrWhiteSpace(method))
                WriteError("参数错误！");

            if (Request.InputStream.Length > 0)
            {
                byte[] byts = new byte[Request.InputStream.Length];//根据流长度新建byte数组
                Request.InputStream.Read(byts, 0, byts.Length);//把流读入byte数组
                streamContent = Encoding.UTF8.GetString(byts);
            }
            else
                WriteError("请不要提交空数据！");



            switch (method)
            {
                case "setActivity":
                    SetActivityVisit();
                    break;
            }


        }

        private void SetActivityVisit() 
        {
            try
            {
                var user_his_act = JsonConvert.DeserializeObject<t_wx_fans_hisactivity>(streamContent);
                user_his_act.id = 0;
                user_his_act = Companycn.Core.EntityFramework.EFHelper.AddModel<creater_wxEntities, t_wx_fans_hisactivity>(user_his_act);
                if (user_his_act.id <= 0)
                    WriteError("Json解析错误。");
                else
                    WriteSuccess("活动记录添加成功！");    
                }
            catch {
                WriteError("Json解析错误。");
            }
        }

        //输出一个值类型
        void WriteObj(ValueType obj)
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
            Response.Write(JsonConvert.SerializeObject(new JsonDataResult<T>{data=entity,msg=msg,result=1}));
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