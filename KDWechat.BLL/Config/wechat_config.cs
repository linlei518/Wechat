using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KDWechat.Common;

namespace KDWechat.BLL.Config
{
  public  class wechat_config
    {
        private static object lockHelper = new object();
        /// <summary>
        ///  读取配置文件
        /// </summary>
        public KDWechat.Common.Config.wechatconfig loadConfig()
        {
            KDWechat.Common.Config.wechatconfig model = CacheHelper.Get<KDWechat.Common.Config.wechatconfig>(KDKeys.WECHAT_CONFIG);
            if (model == null)
            {
                CacheHelper.Insert(KDKeys.WECHAT_CONFIG, SerializationHelper.Load(typeof(KDWechat.Common.Config.wechatconfig), Utils.GetXmlMapPath(KDKeys.FILE_WECHAT_XML_CONFING)),
                    Utils.GetXmlMapPath(KDKeys.FILE_WECHAT_XML_CONFING));
                model = CacheHelper.Get<KDWechat.Common.Config.wechatconfig>(KDKeys.WECHAT_CONFIG);
            }
            return model;
        }

        /// <summary>
        ///  保存配置文件
        /// </summary>
        public KDWechat.Common.Config.wechatconfig saveConifg(KDWechat.Common.Config.wechatconfig model)
        {
            lock (lockHelper)
            {
                SerializationHelper.Save(model, Utils.GetXmlMapPath(KDKeys.FILE_WECHAT_XML_CONFING));
            }
            return model;

        }
    }
}
