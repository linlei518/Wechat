using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KDWechat.Common;

namespace KDWechat.BLL.Config
{
   public class statistics_config
    {
        private static object lockHelper = new object();
        /// <summary>
        ///  读取配置文件
        /// </summary>
        public KDWechat.Common.Config.statisticsconfig loadConfig()
        {
            KDWechat.Common.Config.statisticsconfig model = CacheHelper.Get<KDWechat.Common.Config.statisticsconfig>(KDKeys.CACHE_STATISTICS_CONFIG);
            if (model == null)
            {
                CacheHelper.Insert(KDKeys.CACHE_STATISTICS_CONFIG, SerializationHelper.Load(typeof(KDWechat.Common.Config.statisticsconfig), Utils.GetXmlMapPath(KDKeys.FILE_STATISTICS_XML_CONFING)),
                    Utils.GetXmlMapPath(KDKeys.FILE_STATISTICS_XML_CONFING));
                model = CacheHelper.Get<KDWechat.Common.Config.statisticsconfig>(KDKeys.CACHE_STATISTICS_CONFIG);
            }
            return model;
        }

        /// <summary>
        ///  保存配置文件
        /// </summary>
        public KDWechat.Common.Config.statisticsconfig saveConifg(KDWechat.Common.Config.statisticsconfig model)
        {
            lock (lockHelper)
            {
                SerializationHelper.Save(model, Utils.GetXmlMapPath(KDKeys.FILE_STATISTICS_XML_CONFING));
            }
            return model;

        }

        public static string GetTableName(KDWechat.Common.Config.statisticsconfig config,string id)
        {
            string table_name = "";
            if (config!=null)
            {
                foreach (KDWechat.Common.Config.model m in config.dbtables)
                {
                    if (id.Trim()==m.id.Trim())
                    {
                        table_name = m.name.Trim();
                        break;
                    }
                }
            }
            return table_name;
        }

    }
}
