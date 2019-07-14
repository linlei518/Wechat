using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDWechat.WindowsService
{
    public class Log
    {
        FileStream FS = null;
        StreamWriter sw = null;
        public Log()
        {
            string log_address = ConfigurationManager.AppSettings["log_address"].ToString();
            log_address +=DateTime.Now.ToString("yyyy-MM-dd HH")+ ".log";
            FS = new FileStream(log_address, FileMode.Append);
            sw = new StreamWriter(FS);
        }

        public void WitreLog(string log)
        {
            if (FS != null && sw != null)
            {
                sw.WriteLine(log);
                sw.WriteLine();
            }
        }

        public void Close()
        {
            if (FS != null && sw != null)
            {
                sw.Close();
                FS.Close();
            }
        }

    }
}
