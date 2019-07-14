using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KDWeChat.Common
{

    /// <summary>
    /// 会员实体类
    /// </summary>
    public class t_member
    {
        public int id { get; set; }//ID，0
        public string phone { get; set; }//手机号 必填
        public string pwd { get; set; }//密码（未加密
        public string salt { get; set; }//盐（可以给空
        public string nick_name { get; set; }//昵称（可以给空
        public string e_mail { get; set; }//email可空
        public string username { get; set; }//用户名 可空
        public Nullable<int> capital_star_id { get; set; }//凯德购物星ID 可空
        public System.DateTime add_time { get; set; }//添加时间，可空
        public System.DateTime login_time { get; set; }//最后登录时间，可空
        public string login_ip { get; set; }//登录IP 不可空
        public int status { get; set; } //账号状态 1为正常 0为禁用
        public int m_from { get; set; } //3为全民经纪人

    }


    /// <summary>
    /// 会员接口返回的结果数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MemberApiResultData<T>
    {

        /// <summary>
        /// 执行结果 1-成功，0-失败
        /// </summary>
        public int result { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 返回的对象
        /// </summary>
        public T data { get; set; }

    }





}