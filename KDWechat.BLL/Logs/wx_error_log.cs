using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KDWechat.DAL;

namespace KDWechat.BLL.Logs
{
    public class wx_error_log
    {
        public static t_wx_error_logs CreateWxErrorLog(t_wx_error_logs log)
        {
            return EFHelper.AddLog<t_wx_error_logs>(log);
        }
    }
}
