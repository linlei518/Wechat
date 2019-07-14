using System;
using System.Collections.Generic;
using System.Text;

namespace KDWechat.Common
{
    /// <summary>
    /// 平台 Key
    /// </summary>
    public class KDKeys
    {
        #region Stsyem Kye

        /// <summary>
        /// 版本号全称
        /// </summary>
        public const string ASSEMBLY_VERSION = "1.0.0";
        /// <summary>
        /// 版本年号
        /// </summary>
        public const string ASSEMBLY_YEAR = "2014";
        //File======================================================

        /// <summary>
        /// 站点配置文件名
        /// </summary>
        public const string FILE_SITE_XML_CONFING = "Configpath";

        /// <summary>
        /// 统计配置文件名
        /// </summary>
        public const string FILE_STATISTICS_XML_CONFING = "StatisticsPath";

        /// <summary>
        /// 微信配置文件名
        /// </summary>
        public const string FILE_WECHAT_XML_CONFING = "WehatConfigpath";
        #endregion


        #region Cache Key

        /// <summary>
        /// 站点配置
        /// </summary>
        public const string CACHE_SITE_CONFIG = "kd_cache_site_config";


        /// <summary>
        /// 统计配置
        /// </summary>
        public const string CACHE_STATISTICS_CONFIG = "kd_cache_statistics_config";

        /// <summary>
        ///微信配置
        /// </summary>
        public const string WECHAT_CONFIG = "kd_cache_wechat_config";
        /// <summary>
        /// 用户配置
        /// </summary>
        public const string CACHE_USER_CONFIG = "kd_cache_user_config";

        /// HttpModule映射类
        /// </summary>
        public const string CACHE_SITE_HTTP_MODULE = "kd_cache_http_module";

        /// <summary>
        /// URL重写映射表
        /// </summary>
        public const string CACHE_SITE_URLS = "kd_cache_site_urls";

        #endregion


        #region Session Key

        /// <summary>
        /// 网页验证码
        /// </summary>
        public const string SESSION_CODE = "kd_session_code";
        /// <summary>
        /// 短信验证码
        /// </summary>
        public const string SESSION_SMS_CODE = "kd_session_sms_code";
        /// <summary>
        /// 后台管理员
        /// </summary>
        public const string SESSION_ADMIN_INFO = "kd_session_admin_info";
        /// <summary>
        /// 会员用户
        /// </summary>
        public const string SESSION_USER_INFO = "kd_session_user_info";

        #endregion



        #region Cookies Key

        #region 销售模块
        /// <summary>
        /// 销售模块用户名
        /// </summary>
        public const string COOKIE_SALE_USERNAME = "sale_user_name";

        /// <summary>
        /// 销售模块密码
        /// </summary>
        public const string COOKIE_SALE_PASSWORD = "sale_password";

        /// <summary>
        /// 销售ID
        /// </summary>
        public const string COOKIE_SALE_UID = "sale_uid";

        /// <summary>
        /// 用户
        /// </summary>
        public const string COOKIE_SALE_SELLER = "sale_seller";
        #endregion


        /// <summary>
        /// 会员用户名
        /// </summary>
        public const string COOKIE_USER_NAME="UserName";
        
        /// <summary>
        /// 会员用户密码 
        /// </summary>
        public const string COOKIE_USER_PWD ="UserPwd";

        /// <summary>
        /// 用户类型
        /// </summary>
        public const string COOKIE_USER_FlAG = "UserFlag";


      
       

        /// <summary>
        /// 记住会员用户名
        /// </summary>
        public const string COOKIE_USER_NAME_REMEMBER = "kd_cookie_user_name_remember";
        /// <summary>
        /// 记住会员密码
        /// </summary>
        public const string COOKIE_USER_PWD_REMEMBER = "kd_cookie_user_pwd_remember";

        /// <summary>
        /// 返回上一页
        /// </summary>
        public const string COOKIE_URL_REFERRER = "kd_cookie_url_referrer";

        /// <summary>
        /// 会员用户ID 
        /// </summary>
        public const string COOKIE_USER_ID = "UserID";
        

        //微信公众号===================================================================
        /// <summary>
        /// 微信公众号ID
        /// </summary>
        public const string COOKIE_WECHATS_ID="WechatId";

        /// <summary>
        /// 微信公众号原始id
        /// </summary>
        public const string COOKIE_WECHATS_WX_OG_ID="WxogId";

        /// <summary>
        /// 微信公众号名称
        /// </summary>
        public const string COOKIE_WECHATS_NAME="WxName";

        /// <summary>
        /// 微信号类型
        /// </summary>
        public const string COOKIE_WECHATS_TYPE = "WxType";

        /// <summary>
        /// 微信公众号的头像
        /// </summary>
        public const string COOKIE_WECHATS_HEADIMG = "WxIMG";

        #endregion

    }
}
