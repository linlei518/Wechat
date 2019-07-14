using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using System.Web.Caching;
using KDWechat.Common;

namespace KDWechat.BLL.Config
{
    /// <summary>
    /// 系统配置文件
    /// </summary>
    public partial class siteconfig
    {

        private static object lockHelper = new object();
        /// <summary>
        ///  读取配置文件
        /// </summary>
        public  KDWechat.Common.Config.siteconfig loadConfig()
        {
            KDWechat.Common.Config.siteconfig model = CacheHelper.Get<KDWechat.Common.Config.siteconfig>(KDKeys.CACHE_SITE_CONFIG);
            if (model == null)
            {
                CacheHelper.Insert(KDKeys.CACHE_SITE_CONFIG, SerializationHelper.Load(typeof(KDWechat.Common.Config.siteconfig), Utils.GetXmlMapPath(KDKeys.FILE_SITE_XML_CONFING)),
                    Utils.GetXmlMapPath(KDKeys.FILE_SITE_XML_CONFING));
                model = CacheHelper.Get<KDWechat.Common.Config.siteconfig>(KDKeys.CACHE_SITE_CONFIG);
            }
            return model;
        }

        /// <summary>
        ///  保存配置文件
        /// </summary>
        public  KDWechat.Common.Config.siteconfig saveConifg(KDWechat.Common.Config.siteconfig model)
        {
            lock (lockHelper)
            {
                SerializationHelper.Save(model, Utils.GetXmlMapPath(KDKeys.FILE_SITE_XML_CONFING));
            } 
            return model;
            
        }

    }
}
