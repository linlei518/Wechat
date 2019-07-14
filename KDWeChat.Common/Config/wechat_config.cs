using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KDWechat.Common.Config
{  /// <summary>
    /// 站点配置实体类
    /// </summary>
    [Serializable]
    public class wechatconfig
    {
        public wechatconfig() { }

        private string _domain="";
        private string _is_use_default_template = "0";

        private string _news_templat_path="";

        private string _lbs_templat_path = "";

        private string _pic_templat_path="";

        private string _video_templat_path="";

        private string _voice_templat_path;

        private string _templat_share_html="";

        private string _qy_app_id = "";

        private string _qy_app_secret = "";

        private string _qy_agent_id = "";

        private string _qy_manage_group = "";

        private string _templat_good_event="";

       
        /// <summary>
        /// 域名，后面不带/
        /// </summary>
        public string domain
        {
            get { return _domain; }
            set { _domain = value; }
        }
        /// <summary>
        /// 是否使用默认模板
        /// </summary>
        public string is_use_default_template
        {
            get { return _is_use_default_template; }
            set { _is_use_default_template = value; }
        }

        /// <summary>
        /// 图文详细页的模板地址 
        /// </summary>
        public string news_templat_path
        {
            get { return _news_templat_path; }
            set { _news_templat_path = value; }
        }

        public string lbs_templat_path
        {
            get { return _lbs_templat_path; }
            set { _lbs_templat_path = value; }
        }

        /// <summary>
        /// 图片详细页的模板地址 
        /// </summary>
        public string pic_templat_path
        {
            get { return _pic_templat_path; }
            set { _pic_templat_path = value; }
        }
        /// <summary>
        /// 视频详细页的模板地址 
        /// </summary>
        public string video_templat_path
        {
            get { return _video_templat_path; }
            set { _video_templat_path = value; }
        }
        /// <summary>
        /// 音频详细页的模板地址
        /// </summary>
        public string voice_templat_path
        {
            get { return _voice_templat_path; }
            set { _voice_templat_path = value; }
        }

        /// <summary>
        /// 模板分享代码
        /// </summary>
        public string templat_share_html
        {
            get { return _templat_share_html; }
            set { _templat_share_html = value; }
        }

        /// <summary>
        /// 企业号APPID
        /// </summary>
        public string templat_good_event
        {
            get { return _templat_good_event; }
            set { _templat_good_event = value; }
        }

        /// <summary>
        /// 企业号管理组
        /// </summary>
        public string qy_manage_group
        {
            get { return _qy_manage_group; }
            set {_qy_manage_group = value;}
        }

        /// <summary>
        /// 企业号应用ID
        /// </summary>
        public string qy_agent_id
        {
            get { return _qy_agent_id; }
            set { _qy_agent_id = value; }
        }

        /// <summary>
        /// 企业号appsecret
        /// </summary>
        public string qy_app_id
        {
            get { return _qy_app_id; }
            set { _qy_app_id = value; }
        }        
        
        /// <summary>
        /// 域名，后面不带
        /// </summary>
        public string qy_app_secret
        {
            get { return _qy_app_secret; }
            set { _qy_app_secret = value; }
        }
    }
}
