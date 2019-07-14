using KDWechat.MP.CustomMessageHandler;
using Senparc.Weixin.MP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.Entities;
using System.Xml.Linq;
using KDWechat.DAL;

namespace KDWechat.Web
{
    public partial class KDWeChat : System.Web.UI.Page
    {
        private string Token = "weixin";//与微信公众账号后台的Token设置保持一致，区分大小写。

        public string GetSignature(string timestamp, string nonce, string token = null)
        {
            var arr = new[] { token, timestamp, nonce }.OrderBy(z => z).ToArray();
            var arrString = string.Join("", arr);
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var sha1Arr = sha1.ComputeHash(Encoding.UTF8.GetBytes(arrString));
            StringBuilder enText = new StringBuilder();
            foreach (var b in sha1Arr)
            {
                enText.AppendFormat("{0:x2}", b);
            }
            return enText.ToString();
        }



        protected void Page_Load(object sender, EventArgs e)
        {

            //System.IO.File.AppendAllText(Server.MapPath("~/app_data/a.txt"), DateTime.Now.Ticks.ToString() + "\n", Encoding.UTF8);

            string signature = Request["signature"];
            string timestamp = Request["timestamp"];
            string nonce = Request["nonce"];
            string echostr = Request["echostr"];
            string t = Common.RequestHelper.GetQueryString("t");
            if (t.Trim().Length > 0)
            {
                Token = KDWechat.BLL.Chats.wx_wechats.GetTokenByGuID(t);
                if (Request.HttpMethod == "GET")
                {
                    //get method - 仅在微信后台填写URL验证时触发
                    if (CheckSignature.Check(signature, timestamp, nonce, Token))
                    {
                        WriteContent(echostr); //返回随机字符串则表示验证通过
                    }
                    else
                    {
                        WriteContent("failed:" + signature + "," + CheckSignature.GetSignature(timestamp, nonce, Token) + "。" +
                                    "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。");
                    }
                    Response.End();
                }
                else
                {

                    //post method - 当有用户想公众账号发送消息时触发
                    if (!CheckSignature.Check(signature, timestamp, nonce, Token))
                    {
                        WriteContent("参数错误！");
                        return;
                    }


                    var postModel = new PostModel()
                    {
                        Signature = Request.QueryString["signature"],
                        Msg_Signature = Request.QueryString["msg_signature"],
                        Timestamp = Request.QueryString["timestamp"],
                        Nonce = Request.QueryString["nonce"],
                        //以下保密信息不会（不应该）在网络上传播，请注意
                        Token = Token,
                        EncodingAESKey = "mNnY5GekpChwqhy2c4NBH90g3hND6GeI4gii2YCvKLY",//根据自己后台的设置保持一致
                        AppId = "wx669ef95216eef885"//根据自己后台的设置保持一致
                    };


                    CustomMessageHandler messageHandler = null;

                    #region 混合模式

                    byte[] byts = new byte[Request.InputStream.Length];//根据流长度新建byte数组
                    Request.InputStream.Read(byts, 0, byts.Length);//把流读入byte数组
                    var ms = new MemoryStream(byts);//新建内存流，读入byte数组
                 

                  

                    XElement xml = XElement.Load(ms);//通过xelement读取xml
                    ms.Position = 0;//重置流
                    var msgTypeDoc = xml.Element("MsgType");//获取msgtype节点
                    

                   

                    //判断是否是连续转发的对象。   Damos-add at 2015-3-20 15:55
                    var openIDDoc = xml.Element("FromUserName");
                    if (openIDDoc != null)//有openid，需要连续转发的请求
                    {
                        var openid = openIDDoc.Value;
                        var times = CacheHelper.Get<retrans_times_server_view>("retrans_" + openIDDoc.Value);
                        if (times != null && times.times > 0)
                        {
                            signature = GetSignature(timestamp, nonce, times.token);//根据转发表token重新计算签名
                            string url = times.url + (times.url.Contains("?") ? "&" : "?") + "signature=" + signature + "&timestamp=" + timestamp + "&nonce=" + nonce;//根据转发表中的链接地址附加参数
                            var responses = BaiDuMapAPI.HttpUtility.RequestUtility.HttpPost(url, ms);//去第三方拿数据
                            ms.Dispose();//别忘了内存里面的流。。。

                            CacheHelper.Remove("retrans_" + openIDDoc.Value);
                            if (times.times > 1)
                            {
                                times.times -= 1;
                                CacheHelper.Insert("retrans_" + openIDDoc.Value, times);
                            }
                            Response.Write(responses);
                            Response.End();
                            return;//收工~
                        }
                    }


                    //判断是否是为明天的菜单和二维码    Damos-add at 2015-3-23 18:35
                    var ogid = "";
                    if (xml.Element("ToUserName") != null)
                    {
                        ogid = xml.Element("ToUserName").Value;
                    }

                    var wmtOgID = System.Configuration.ConfigurationManager.AppSettings["WMTOgID"]?? "gh_ca90998bc552";

                    
                  

                    if (msgTypeDoc != null && msgTypeDoc.Value == "event" && ogid == wmtOgID)//wx6a328193c8912b88 如果是为明天的事件请求
                    {
                        var eventDoc = xml.Element("Event");//事件类型
                      
                        if (eventDoc != null)
                        {
                            var doRetrans = false;//是否执行转发
                            if (eventDoc.Value == "CLICK")//点击菜单事件
                            {
                                doRetrans = true;//执行转发
                                
                            }
                            else if (eventDoc.Value == "SCAN")//扫描二维码事件
                            {
                                var evertKey = xml.Element("EventKey").Value;
                                var keyList = Companycn.Core.EntityFramework.EFHelper.GetArray<creater_wxEntities, t_wx_qrcode, int>(x => x.wx_og_id == ogid, x => x.souce_id);
                                if (!keyList.Contains(Utils.StrToInt(evertKey, 0)))
                                {
                                    doRetrans = true;//执行转发
                                }
                            }

                            if (doRetrans)//如果需要转发
                            {
                                var wmtToken = System.Configuration.ConfigurationManager.AppSettings["WMTToken"] ?? "wmtweixin";

                                signature = GetSignature(timestamp, nonce, wmtToken);//根据为明天token重新计算签名  如果为明天修改了token，这里也要修改

                                var wmtWxApiUrl = System.Configuration.ConfigurationManager.AppSettings["WMTWeiXinApi"] ?? "http://www.chfchina.org/weimingtian/wxapi.aspx";
                                string url = wmtWxApiUrl + "?signature=" + signature + "&timestamp=" + timestamp + "&nonce=" + nonce;//根据转发表中的链接地址附加参数
                                var responses = BaiDuMapAPI.HttpUtility.RequestUtility.HttpPost(url, ms);//去为明天拿数据
                                ms.Dispose();//别忘了内存里面的流。。。
                               

                                Response.Write(responses);
                                Response.End();
                                return;//收工~
                            }
                        }
                    }

                    #region 微信卡券通知
                    if (msgTypeDoc.Value == "event" && xml.Element("Event") != null)
                    {
                        var eventDoc = xml.Element("Event");//事件类型

                        switch (eventDoc.Value.ToLower())
                        {
                            case "card_pass_check": //卡券审核通过
                            case "card_not_pass_check"://卡券审核不通过
                            case "user_get_card"://领取事件推送
                            case "user_del_card"://删除事件推送
                            case "user_consume_card"://核销事件推送
                            case "user_pay_from_pay_cell"://买单事件推送

                                var api_timestamp = ((int)((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds)).ToString();  //时间戳
                                var api_nonce = Utils.Number(6);//随机数
                                var oldCode = api_timestamp + "-" + api_nonce + "-GetWxMessage";  //时间戳+随机数+接口方法名
                                var sn = Utils.ValidateApiTimeCode(oldCode); //生成sn签名
                                var apiUrl = System.Configuration.ConfigurationManager.AppSettings["mall_api_url"];
                                apiUrl += $"/WxCard/Api_WxCard?tp=GetWxMessage&timestamp={api_timestamp}&sn={sn}&nonce={api_nonce}";

                                var result = "";
                                try
                                {
                                    result = RequestUtility.HttpPost(apiUrl, Request.InputStream);
                                }
                                catch (Exception ex)
                                {

                                    result = ex.Message + "," + ex.StackTrace;
                                }
                                if (!Directory.Exists(Server.MapPath("~/App_Data/wechat_card/")))
                                    Directory.CreateDirectory(Server.MapPath("~/App_Data/wechat_card/"));
                                using (TextWriter tw = new StreamWriter(Server.MapPath("~/App_Data/wechat_card/" + openIDDoc.Value + "_" + DateTime.Now.Ticks + ".txt")))
                                {
                                    //byte[] byts_t = new byte[Request.InputStream.Length];//根据流长度新建byte数组
                                    //Request.InputStream.Read(byts_t, 0, byts_t.Length);//把流读入byte数组
                                    //var st = new MemoryStream(byts_t);//新建内存流，读入byte数组
                                    //StreamReader reader = new StreamReader(st);
                                    //string text = reader.ReadToEnd();
                                    //reader.Dispose();
                                    //st.Dispose();
                                    tw.WriteLine("请求：" + xml.ToString());
                                    tw.WriteLine("结果：" + result);
                                    
                                    tw.Flush();
                                    tw.Close();
                                }
                                break;
                        }



                    }
                    #endregion

                    //if (msgTypeDoc != null && msgTypeDoc.Value == "text")//判断节点存在且为text   edit-这里不加else，以确保为明天的正常event的转发需求
                    //{
                        //var guid = Request.QueryString["t"];//拿guid                       
                        //if (!string.IsNullOrEmpty(guid))//有GUID
                        //{
                        //    var wechat = Companycn.Core.EntityFramework.EFHelper.GetModel<creater_wxEntities, t_wx_wechats>(x => x.wx_guid == guid);//去wechat
                        //    if (wechat != null)//取的wechat
                        //    {
                        //        DapperConnection.minebea.AddModel(new t_wx_logs
                        //        {
                        //            ip = "1",
                        //            login_name = "1",
                        //            wx_id = 1,
                        //            u_id = 1,
                        //            wx_og_id = "1",
                        //            contents = msgTypeDoc.Value
                        //        }, "id");
                        //        var msg = xml.Element("Content").Value;//取收到关键字
                        //        var retransKeyword = Companycn.Core.EntityFramework.EFHelper.GetModel<creater_wxEntities, retrans_keyword_server_view>(x => x.keyword == msg && x.wx_id == wechat.id);//取转发表
                        //        if (retransKeyword != null)//转发表中存在，需要转发
                        //        {
                        //            if (retransKeyword.retrans_times != null && retransKeyword.retrans_times.Value > 0)//需要多次转发
                        //                CacheHelper.Insert("retrans_" + openIDDoc.Value, new retrans_times_server_view { server_id = retransKeyword.retrans_id, open_id = xml.Element("FromUserName").Value, times = retransKeyword.retrans_times.Value, title = retransKeyword.title, token = retransKeyword.token, url = retransKeyword.url });
                        //            signature = GetSignature(timestamp, nonce, retransKeyword.token);//根据转发表token重新计算签名
                        //            string url = retransKeyword.url + (retransKeyword.url.Contains("?") ? "&" : "?") + "signature=" + signature + "&timestamp=" + timestamp + "&nonce=" + nonce;//根据转发表中的链接地址附加参数
                        //            var responses = BaiDuMapAPI.HttpUtility.RequestUtility.HttpPost(url, ms);//去第三方拿数据
                        //            ms.Dispose();//别忘了内存里面的流。。。
                        //            Response.Write(responses);
                        //            Response.End();
                        //            return;//收工~
                        //        }
                        //    }
                    //}
                
                    else if (msgTypeDoc != null && msgTypeDoc.Value == "image")
                    {
                        var guid = Request.QueryString["t"];//拿guid                       
                        if (!string.IsNullOrEmpty(guid))//有GUID
                        {
                            var wechat = Companycn.Core.EntityFramework.EFHelper.GetModel<creater_wxEntities, t_wx_wechats>(x => x.wx_guid == guid);//去wechat
                            if (wechat != null)//取的wechat
                            {
                                var retransServer = Companycn.Core.EntityFramework.EFHelper.GetModel<creater_wxEntities, t_retrans_server>(x => x.wx_id == wechat.id && x.is_image_server == 1);
                                if (retransServer != null)
                                {
                                    if (retransServer.image_retrans_times != null && retransServer.image_retrans_times.Value > 0)
                                    {
                                        CacheHelper.Insert("retrans_" + openIDDoc.Value, new retrans_times_server_view { server_id = retransServer.id, open_id = xml.Element("FromUserName").Value, times = retransServer.image_retrans_times.Value, title = retransServer.title, token = retransServer.token, url = retransServer.url });
                                    }
                                    signature = GetSignature(timestamp, nonce, retransServer.token);//根据转发表token重新计算签名
                                    string url = retransServer.url + (retransServer.url.Contains("?") ? "&" : "?") + "signature=" + signature + "&timestamp=" + timestamp + "&nonce=" + nonce;//根据转发表中的链接地址附加参数
                                    var responses = BaiDuMapAPI.HttpUtility.RequestUtility.HttpPostWithEncoding(url, ms, Encoding.UTF8, "Mozilla/4.0");//.HttpPost(url, ms,);//去第三方拿数据
                                    ms.Dispose();//别忘了内存里面的流。。。
                                    Response.Write(responses);
                                    Response.End();
                                    return;//收工~
                                }
                            }
                        }
                    }

                    #endregion

                    messageHandler = new CustomMessageHandler(ms, postModel);//看来不需要转发，用ms重新建一个messagehandler
                    ms.Dispose();//这东西别忘了释放

                    #region weimob test
                    ///以下部分为转发到微盟的测试！
                    //else
                    //{
                    //    string url="http://api.weimob.com/api?t=3ead643825415f623ab4085086cd30f9==T&signature=" + signature + "&timestamp=" + timestamp + "&echostr=" + echostr + "&nonce=" + nonce;
                    //    try
                    //    {
                    //        signature = GetSignature(timestamp, nonce, "662245_k");

                    //        var responses = BaiDuMapAPI.HttpUtility.RequestUtility.HttpPost(url, Request.InputStream);
                    //        using (TextWriter tw = new StreamWriter(Server.MapPath("~/App_Data/Error_" + DateTime.Now.Ticks + ".txt")))
                    //        {
                    //            tw.WriteLine(url);
                    //            tw.WriteLine(responses);
                    //            tw.Flush();
                    //            tw.Close();
                    //        }
                    //        WriteContent("");
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        using (TextWriter tw = new StreamWriter(Server.MapPath("~/App_Data/Error_" + DateTime.Now.Ticks + ".txt")))
                    //        {
                    //            tw.WriteLine(url);
                    //            //tw.WriteLine(messageHandler.RequestMessage.FromUserName);
                    //            //tw.WriteLine(Request.InputStream.Length);
                    //            tw.WriteLine(ex.Message);
                    //            tw.WriteLine(ex.InnerException.Message);
                    //            //if (messageHandler.ResponseDocument != null)
                    //            //{
                    //                //tw.WriteLine(messageHandler.ResponseDocument.ToString());
                    //            //}
                    //            tw.WriteLine(signature);
                    //            tw.Flush();
                    //            tw.Close();
                    //        }
                    //        WriteContent("");
                    //    }
                    //    return;
                    //}
                    #endregion
                    try
                    {
                        //测试时可开启此记录，帮助跟踪数据，使用前请确保App_Data文件夹存在，且有读写权限。
                        //messageHandler.RequestDocument.Save(
                        //Server.MapPath("~/App_Data/" + DateTime.Now.Ticks + "_Request_" +
                        //messageHandler.RequestMessage.FromUserName + ".txt"));
                        //执行微信处理过程
                        messageHandler.Execute();
                        //测试时可开启，帮助跟踪数据
                        //messageHandler.ResponseDocument.Save(
                        //Server.MapPath("~/App_Data/" + DateTime.Now.Ticks + "_Response_" +
                        //                messageHandler.ResponseMessage.ToUserName + ".txt"));
                        if (messageHandler.ResponseMessage is ResponseMessageText)
                        {
                            var txtMsg = (ResponseMessageText)messageHandler.ResponseMessage;
                           
                            if (string.IsNullOrEmpty(txtMsg.Content))
                            {
                                WriteContent("");
                                return;
                            }
                        }
                        else if (messageHandler.ResponseMessage is ResponseMessageNews)
                        {
                            var newsMsg = (ResponseMessageNews)messageHandler.ResponseMessage;
                            if (newsMsg.ArticleCount == 0)
                            {
                                WriteContent("");
                                return;
                            }
                        }
                        WriteContent(messageHandler);
                        return;
                    }
                    catch (Exception ex)
                    {
                        //messageHandler.RequestDocument.Save(
                        //Server.MapPath("~/App_Data/" + DateTime.Now.Ticks + "_Request_" +
                        //messageHandler.RequestMessage.FromUserName + ".txt"));


                        using (TextWriter tw = new StreamWriter(Server.MapPath("~/App_Data/Error_" + DateTime.Now.Ticks + ".txt")))
                        {
                            tw.WriteLine(ex.Message);
                            tw.WriteLine(ex.InnerException.Message);
                            if (messageHandler.ResponseDocument != null)
                            {
                                tw.WriteLine(messageHandler.ResponseDocument.ToString());
                            }
                            tw.Flush();
                            tw.Close();
                        }
                        WriteContent("");

                    }
                    finally
                    {
                        Response.End();
                    }
                }
            }

        }

        private void WriteContent(string str)
        {
            Response.Output.Write(str);
        }

        private void WriteContent(CustomMessageHandler _messageHandlerDocument)
        {
            //System.IO.File.AppendAllText(Server.MapPath("~/app_data/a.txt"), DateTime.Now.Ticks.ToString()+"\n", Encoding.UTF8);
            var xml = _messageHandlerDocument.FinalResponseDocument.ToString().Replace("\r\n", "\n"); //腾
            using (MemoryStream ms = new MemoryStream())//迅
            {//真
                var bytes = Encoding.UTF8.GetBytes(xml);//的

                Response.OutputStream.Write(bytes, 0, bytes.Length);//很
            }//疼
             // Response.Output.Write(str);
        }

        /// <summary>
        /// 最简单的Page_Load写法（本方法仅用于演示过程，未实际使用到）
        /// </summary>
        private void MiniProcess()
        {
            string signature = Request["signature"];
            string timestamp = Request["timestamp"];
            string nonce = Request["nonce"];
            string echostr = Request["echostr"];

            if (Request.HttpMethod == "GET")
            {
                //get method - 仅在微信后台填写URL验证时触发
                if (CheckSignature.Check(signature, timestamp, nonce, Token))
                {
                    WriteContent(echostr); //返回随机字符串则表示验证通过
                }
                else
                {
                    WriteContent("failed:" + signature + "," + CheckSignature.GetSignature(timestamp, nonce, Token));
                }

            }
            else
            {
                //post method - 当有用户想公众账号发送消息时触发
                if (!CheckSignature.Check(signature, timestamp, nonce, Token))
                {
                    WriteContent("参数错误！");
                }

                var postModel = new PostModel()
                {
                    Signature = Request.QueryString["signature"],
                    Msg_Signature = Request.QueryString["msg_signature"],
                    Timestamp = Request.QueryString["timestamp"],
                    Nonce = Request.QueryString["nonce"],
                    //以下保密信息不会（不应该）在网络上传播，请注意
                    Token = Token,
                    EncodingAESKey = "mNnY5GekpChwqhy2c4NBH90g3hND6GeI4gii2YCvKLY",//根据自己后台的设置保持一致
                    AppId = "wx669ef95216eef885"//根据自己后台的设置保持一致
                };

                //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
                var messageHandler = new CustomMessageHandler(Request.InputStream, postModel);
                //执行微信处理过程
                messageHandler.Execute();
                //输出结果
                WriteContent(messageHandler.ResponseDocument.ToString());
            }
            Response.End();
        }
    }
}