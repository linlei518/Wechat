using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace KDWechat.Common
{
    /// <summary>
    /// 客户端脚本输出
    /// </summary>
    public class JsHelper
    { 
        /// <summary>
        /// 弹出信息,并跳转指定页面。
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="toURL">跳转的页面</param>
        /// <param name="type">默认成功（success=成功 ，fail=失败）</param>
        public static void AlertAndRedirect(string message, string toURL, string type = "success")
        {

            if (toURL.Contains(type))
            {
                HttpContext.Current.Response.Redirect(toURL.Substring(0, toURL.LastIndexOf(type)) + type + "=" + message, false);
            }
            else
            {
                HttpContext.Current.Response.Redirect(toURL + "#" + type + "=" + message,false);
            }

            //HttpContext.Current.Response.End();



        }

        /// <summary>
        /// 弹出信息,并返回历史页面
        /// </summary>
        public static void AlertAndGoHistory(string message, int value, string type = "success")
        {
            //string js = @"<Script language='JavaScript'>alert('{0}');history.go({1});</Script>";
            //HttpContext.Current.Response.Write(string.Format(js, message, value));
            //HttpContext.Current.Response.End();
            HttpContext.Current.Response.Redirect("var url=history.go({1});window.location.replace(url+'#" + type + "=" + message + "');",false);
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 直接跳转到指定的页面0904701X
        /// </summary>
        public static void Redirect(string toUrl)
        {
            string js = @"<script language=javascript>window.location.replace('{0}')</script>";
            HttpContext.Current.Response.Write(string.Format(js, toUrl));
        }

        /// <summary>
        /// 弹出信息 并指定到父窗口
        /// </summary>
        public static void AlertAndParentUrl(string message, string toURL, string type = "success")
        {
            //string js = "<script language=javascript>alert('{0}');window.top.location.replace('{1}')</script>";
            //HttpContext.Current.Response.Write(string.Format(js, message, toURL));
            AlertAndRedirect(message, toURL, type);
        }

        /// <summary>
        /// 返回到父窗口
        /// </summary>
        public static void ParentRedirect(string ToUrl)
        {
            string js = "<script language=javascript>window.top.location.replace('{0}')</script>";
            HttpContext.Current.Response.Write(string.Format(js, ToUrl));
        }

        /// <summary>
        /// 返回历史页面
        /// </summary>
        public static void BackHistory(int value)
        {
            string js = @"<Script language='JavaScript'>history.go({0});</Script>";
            HttpContext.Current.Response.Write(string.Format(js, value));
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 弹出信息
        /// </summary>
        public static void Alert(string message)
        {
            string js = "<script language=javascript> alert('" + message + "');</script>";
            HttpContext.Current.Response.Write(string.Format(js, message));
        }

     /// <summary>
        /// 弹出信息
     /// </summary>
     /// <param name="page">当前页对象</param>
     /// <param name="message">提示消息</param>
     /// <param name="type">默认成功（false=成功，true=失败）</param>
        public static void Alert(System.Web.UI.Page page, string message, string type = "false")
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), "scriptblock", "<script type='text/javascript'> dialogue.closeAll();showTip.show('" + message + "', " + type + ");</script>");
        }
        /// <summary>
        /// 注册脚本块
        /// </summary>
        public static void RegisterScriptBlock(System.Web.UI.Page page, string _ScriptString)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), "scriptblock", "<script type='text/javascript'>" + _ScriptString + "</script>");
        }



        #region 输出自定义脚本信息
        /// <summary>
        /// 输出自定义脚本信息
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="script">输出脚本</param>
        public static void ResponseScript(System.Web.UI.Page page, string script)
        {

            page.ClientScript.RegisterStartupScript(page.GetType(), "message", "<script language='javascript' defer>" + script + "</script>");
        }
        /// <summary>
        /// 输出自定义弹出脚本信息
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="script">输出脚本</param>
        public static void ResponseAlertScript(System.Web.UI.Page page, string script)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", "<script language='javascript' defer>alert('" + script + "')</script>");

        }




        #endregion
    }
}
