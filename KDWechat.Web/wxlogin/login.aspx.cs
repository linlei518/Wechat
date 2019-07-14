using KDWechat.BLL.Users;
using KDWechat.Common;
using KDWechat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.KDlogin
{
    public partial class login : System.Web.UI.Page
    {
    
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod == "POST")
            {
                CheckUser();
                Response.End();
            }
            else
            {
               
            }
        }

        private void CheckUser()
        {
            //AJAX结果--“,”分割的字符串，第一位表示验证结果，成功为0，失败为1，成功时第二个参数表示链接地址，失败时表示弹出的信息

            string userName = Utils.Filter(Request.Form["txtUsername"]);
            string password = Utils.Filter(Request.Form["txtPassword"]);
            string identifyCode = Request.Form["txtIdentifyCode"];
            if (identifyCode.ToLower() != Session[KDKeys.SESSION_CODE].ToString().ToLower())
                Response.Write("0,验证码错误");
            else
            {

                t_sys_users model = sys_users.UserLogin(userName, password);
                if (null != model&&model.status!=(int)Status.禁用)
                {
                    Utils.WriteCookie(KDKeys.COOKIE_WECHATS_ID, "0");
                    Utils.WriteCookie(KDKeys.COOKIE_WECHATS_WX_OG_ID, "");
                    Utils.WriteCookie(KDKeys.COOKIE_WECHATS_NAME, "");

                    Session[KDKeys.SESSION_ADMIN_INFO] = model;
                    
                    Utils.WriteCookie(KDKeys.COOKIE_USER_NAME, model.user_name);
                    Utils.WriteCookie(KDKeys.COOKIE_USER_PWD, password);   //danny 更新
                    Utils.WriteCookie(KDKeys.COOKIE_USER_FlAG, model.flag.ToString());//用户类型
                    Utils.WriteCookie(KDKeys.COOKIE_USER_ID, DateTime.Now.ToString("yyyyMMdd")+model.id);   //danny 新加

                    //string url = "../mainpage/index.aspx?m_id=1";
                    //Session[KDKeys.SESSION_CODE] = "";
                    //Response.Write("1," + url);
                    if (model.flag == 1 || model.flag == 4)
                    {
                        string url = "../Account/ManageAccount.aspx?m_id=51";
                        Session[KDKeys.SESSION_CODE] = "";
                        Response.Write("1," + url);
                    }
                    else
                    {
                        string url = "../Account/region_wxlist.aspx?m_id=59";
                        Session[KDKeys.SESSION_CODE] = "";
                        Response.Write("1," + url);
                    }

                }
                else
                    Response.Write("0,用户名，密码错误或账号已被禁用。");
            }
        }

    }
}