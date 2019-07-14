using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KDWechat.BLL.Users;
using KDWechat.Common;
namespace KDWechat.WindowsService
{
    public partial class Service1 : ServiceBase
    {

        protected System.Timers.Timer timer = new System.Timers.Timer(1000 * 60);//60秒一次检测



        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Log log = new Log();

            log.WitreLog("-----服务于" + DateTime.Now + "开始启动-----");
            log.Close();
            timer.Enabled = true;
            timer.Elapsed += timer_Elapsed;

        }

        /// <summary>
        /// 定时器事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Log log = new Log();
            try
            {
                log.WitreLog("");
                log.WitreLog("-----服务于" + DateTime.Now + "开始检测需要群发的数据-----");
                string path = System.Configuration.ConfigurationManager.AppSettings["uploadUrl"];
                var domain = System.Configuration.ConfigurationManager.AppSettings["domain"];
                int count = KDWechat.BLL.Chats.wx_group_msgs.SendTmerMsg(path, domain);
                log.WitreLog("-----服务于" + DateTime.Now + "检测到" + count + "条群发的数据，并已执行-----");
                if (DateTime.Now <= DateTime.Now.Date.AddHours(23).AddMinutes(59) && DateTime.Now >= DateTime.Now.Date.AddHours(23).AddMinutes(54))
                {
                    wx_fans.SetState(FansState.自动回复状态);
                    log.WitreLog("-----服务于" + DateTime.Now + "更改粉丝用户的自动回复状态-----");
                }
                BLL.Module.md_sale_chats.SetEntireFansState();
                log.WitreLog("-----服务于" + DateTime.Now + "更新职业顾问的状态，并已执行-----");
                BLL.Users.wx_fans.SetRegFansState();
                log.WitreLog("-----服务于" + DateTime.Now + "更新微信注册的状态，并已执行-----");
                //BLL.Users.wx_fans.SetUploadTickerFansState();
                //log.WitreLog("-----服务于" + DateTime.Now + "更新上传小票的状态，并已执行-----");
            }
            catch (Exception ex)
            {
                log.WitreLog("-----服务于" + DateTime.Now + " 出现错误：" + ex.Message + ex.ToString() + "-----");
            }
            log.WitreLog("-----服务于" + DateTime.Now + "结束检测数据-----");
            log.WitreLog("");
            log.Close();

        }


        protected override void OnStop()
        {
            Log log = new Log();
            log.WitreLog("-----服务于" + DateTime.Now + "试图停止-----");
            log.WitreLog("-----服务已停止-----");
            log.WitreLog("");
            log.Close();
        }

        protected override void OnContinue()
        {
            base.OnContinue();
            Log log = new Log();
            log.WitreLog("-----服务于" + DateTime.Now + "继续-----");
            log.Close();
        }

        protected override void OnPause()
        {
            base.OnPause();
            Log log = new Log();
            log.WitreLog("-----服务于" + DateTime.Now + "暂停-----");
            log.Close();
        }
    }
}
