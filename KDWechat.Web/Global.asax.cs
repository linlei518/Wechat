using KDWechat.BLL.Users;
using KDWechat.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Text;
using KDWechat.Web.zh_user;

namespace KDWechat.Web
{
    public class Global : System.Web.HttpApplication
    {

        void Application_Start(object sender, EventArgs e)
        {
            

            // 在应用程序启动时运行的代码
            int time = 60*1000*5;//40分钟运行一次
            System.Timers.Timer myTimer = new System.Timers.Timer();
            myTimer.Elapsed += new System.Timers.ElapsedEventHandler(send_email);
            myTimer.Interval = time;
            myTimer.Enabled = true;

        }

    



        void Application_End(object sender, EventArgs e)
        {
            //  在应用程序关闭时运行的代码
            //下面的代码是关键，可解决IIS应用程序池自动回收的问题 
            Thread.Sleep(1000);
            //这里设置你的web地址，可以随便指向你的任意一个aspx页面甚至不存在的页面，目的是要激发Application_Start 
            string url = "http://localhost//wxlogin/login.aspx";
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            Stream receiveStream = myHttpWebResponse.GetResponseStream();//得到回写的字节流 
        }

        void Application_Error(object sender, EventArgs e)
        {
            // 在出现未处理的错误时运行的代码 
            Exception ex = Server.GetLastError().GetBaseException();
            StringBuilder str = new StringBuilder();
            DateTime date = DateTime.Now;
            str.Append("\r\n" + date.ToString("yyyy.MM.dd HH:mm:ss"));
            str.Append("\r\n.客户信息：");

            string ip = "";
            if (Request.ServerVariables.Get("HTTP_X_FORWARDED_FOR") != null)
            {
                ip = Request.ServerVariables.Get("HTTP_X_FORWARDED_FOR").ToString().Trim();
            }
            else
            {
                ip = Request.ServerVariables.Get("Remote_Addr").ToString().Trim();
            }
            str.Append("\r\n\tIp:" + ip);
            str.Append("\r\n\t浏览器:" + Request.Browser.Browser.ToString());
            str.Append("\r\n\t浏览器版本:" + Request.Browser.MajorVersion.ToString());
            str.Append("\r\n\t操作系统:" + Request.Browser.Platform.ToString());
            str.Append("\r\n.错误信息：");
            str.Append("\r\n\t页面：" + Request.Url.ToString());
            str.Append("\r\n\t错误信息：" + ex.Message);
            str.Append("\r\n\t错误源：" + ex.Source);
            str.Append("\r\n\t异常方法：" + ex.TargetSite);
            str.Append("\r\n\t堆栈信息：" + ex.StackTrace);
            str.Append("\r\n--------------------------------------------------------------------------------------------------");
            //保存错误日志 
            KDWechat.DAL.t_wx_error_logs logs = new DAL.t_wx_error_logs();
            logs.user_id = 0;
            logs.login_name = "";
            logs.content = str.ToString();
            logs.add_time = date;
            BLL.Logs.wx_error_log.CreateWxErrorLog(logs);
        }





        public void send_email(object sender, EventArgs e)
        {
            KDWechat.DAL.t_wx_error_logs logs = new DAL.t_wx_error_logs();
            var d_now = DateTime.Now;
            if (d_now.Date == Utils.ObjectToDateTime(d_now.AddMonths(1).ToString("yyyy-MM-01")).AddDays(-1).Date)//月底最后一天
            {
                if (d_now.Hour== 23&&d_now.Minute>=55)  //if (d_now.Hour>=12) 
                {
                    logs.user_id = 0;
                    logs.login_name = "";
                    logs.content = "进入邮件发送";
                    logs.add_time = d_now;
                    BLL.Logs.wx_error_log.CreateWxErrorLog(logs);

                    new report_temp().create_report_month();
                    try
                    {
                        sendMail("smtp.mxhichina.com", "info@shuwentech.com", "AAbbcc123", "署文信息科技",
                            "info@shuwentech.com", "zhouxiaochen@zpmc.com", "停车场月度报表", "您好，这是停车场月度停车天数低于三分之一的报表");

                    }
                    catch (Exception ex)
                    {
                        //保存错误日志 
                      
                        logs.user_id = 0;
                        logs.login_name = "";
                        logs.content = ex.Message;
                        logs.add_time = d_now;
                        BLL.Logs.wx_error_log.CreateWxErrorLog(logs);
                    }


                }
            }



        }

        #region 发送电子邮件
        /// <summary>
        /// 发送电子邮件
        /// </summary>
        /// <param name="smtpserver">SMTP服务器</param>
        /// <param name="userName">登录帐号</param>
        /// <param name="pwd">登录密码</param>
        /// <param name="nickName">发件人昵称</param>
        /// <param name="strfrom">发件人</param>
        /// <param name="strto">收件人</param>
        /// <param name="subj">主题</param>
        /// <param name="bodys">内容</param>
        public static void sendMail(string smtpserver, string userName, string pwd, string nickName, string strfrom, string strto, string subj, string bodys)
        {

            SmtpClient _smtpClient = new SmtpClient();
            _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式
            _smtpClient.Host = smtpserver;//指定SMTP服务器
            _smtpClient.Credentials = new System.Net.NetworkCredential(userName, pwd);//用户名和密码
            _smtpClient.Port = 25;
            //MailMessage _mailMessage = new MailMessage(strfrom, strto);
            MailAddress _from = new MailAddress(strfrom, nickName);
            MailAddress _to = new MailAddress(strto);
            MailMessage _mailMessage = new MailMessage(_from, _to);
            _mailMessage.Subject = subj;//主题
            _mailMessage.Body = bodys;//内容
            _mailMessage.BodyEncoding = System.Text.Encoding.Default;//正文编码
            _mailMessage.IsBodyHtml = true;//设置为HTML格式
            _mailMessage.Priority = MailPriority.Normal;//优先级
            var Attachment = new Attachment(AppDomain.CurrentDomain.BaseDirectory + ("/zh_user/month_less_park_list" + ".xls"));
            _mailMessage.Attachments.Add(Attachment);
            _smtpClient.Send(_mailMessage);

        }




        #endregion



        //void Session_Start(object sender, EventArgs e)
        //{
        //    // 在新会话启动时运行的代码

        //}

        //void Session_End(object sender, EventArgs e)
        //{
        //    //下面的代码是关键，可解决IIS应用程序池自动回收的问题 
        //    Thread.Sleep(1000);
        //    //这里设置你的web地址，可以随便指向你的任意一个aspx页面甚至不存在的页面，目的是要激发Application_Start 
        //    string url = "http://kdwechat.companycn.net/kdlogin/login.aspx";
        //    HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        //    HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
        //    Stream receiveStream = myHttpWebResponse.GetResponseStream();//得到回写的字节流 
        //    myHttpWebResponse.Dispose();

        //}

    }
}
