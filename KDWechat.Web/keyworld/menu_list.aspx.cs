using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.BLL;
using KDWechat.DAL;
using System.Data;
using Senparc.Weixin.MP.Entities.Menu;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.CommonAPIs;
using KDWechat.Common;
using System.Text;
using Senparc.Weixin.Exceptions;
namespace KDWechat.Web.keyworld
{
    public partial class menu_list : KDWechat.Web.UI.BasePage
    {
        protected string strLength = "600";
        protected string action = "";//动作
        protected int msgId = 0;
        protected t_wx_wechats wx_wechat;
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckUserAuthority("wechat_diymenu");
            action = RequestHelper.GetQueryString("action");
            wx_wechat = BLL.Chats.wx_wechats.GetWeChatByID(wx_id);

            if (Request.QueryString["id"] != "")
            {
                msgId = RequestHelper.GetQueryInt("id",0);
            }

            btnPublish.Visible = isRelease;
            btnDelete.Visible = isDelete;
        }
       
        /// <summary>
        /// 一键生成菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCreate_Click(object sender, EventArgs e)
        {
            
            string log_title = "一键生成菜单";
            var wx_wechat = BLL.Chats.wx_wechats.GetWeChatByID(wx_id);
            if (wx_wechat!=null)
            {

                List<DAL.t_wx_diy_menus> dtparents = BLL.Chats.wx_diy_menus.GetListByWxIdAndParentId(wx_id, 0);
                if (dtparents.Count==0)
                {
                    JsHelper.AlertAndRedirect("请添加菜单后在进行生成", "menu_list.aspx?m_id=" + m_id,"fail");
                    return;
                }
             
                var btns = new ButtonGroup();
               

                foreach (t_wx_diy_menus row in dtparents)
                {

                    if (row.reply_type.ToString() == "-1")
                    {
                        string menuname =  HttpUtility.HtmlDecode( row.menu_name.ToString());
                        var btn2 = new SubButton(menuname);

                        int par_id = row.id;
                        List<DAL.t_wx_diy_menus> dtsubmenu = BLL.Chats.wx_diy_menus.GetListByWxIdAndParentId(wx_id, par_id);
                        foreach (t_wx_diy_menus subrow in dtsubmenu)
                        {
                            if (subrow.menu_type == "view")
                            {

                                var btn2_1 = new SingleViewButton
                                {
                                    name =  HttpUtility.HtmlDecode( subrow.menu_name.ToString()),
                                    url = GetShouQuanUrl(subrow.menu_url, Convert.ToInt32(subrow.reply_type))
                                };

                                btn2.sub_button.Add(btn2_1);
                            }
                            else if (subrow.menu_type == "click")
                            {
                                var btn2_1 = new SingleClickButton
                                {
                                    name = HttpUtility.HtmlDecode( subrow.menu_name),
                                    key = subrow.menu_key
                                };
                                btn2.sub_button.Add(btn2_1);
                            }
                            else
                            {
                                JsHelper.AlertAndRedirect("请给二级菜单添加一个动作", "menu_list.aspx?m_id=" + m_id);
                            }
                        }
                        btns.button.Add(btn2);
                    }
                    else
                    {

                        if (row.menu_type.ToString() == "")
                        {
                            JsHelper.AlertAndRedirect("请给没有子菜单的一级菜单添加一个动作", "menu_list.aspx?m_id=" + m_id);
                        }
                        else if (row.menu_type.ToString() == "click")
                        {
                            var btn1 = new SingleClickButton
                            {
                                name =  HttpUtility.HtmlDecode( row.menu_name),
                                key = row.menu_key
                            };
                            btns.button.Add(btn1);
                        }
                        else //view
                        {
                            var btn1 = new SingleViewButton
                            {
                                name =  HttpUtility.HtmlDecode( row.menu_name),
                                url = GetShouQuanUrl(row.menu_url, Convert.ToInt32(row.reply_type)),
                            };

                            btns.button.Add(btn1);
                        }
                    }

                }
                try
                {
                    string token = BLL.Chats.wx_wechats.GetAccessToken(wx_wechat.id, wx_wechat);//  AccessTokenContainer.TryGetToken(wx_wechat.app_id, wx_wechat.app_secret);
                    if (!token.Contains("Error:"))
                    {
                        Senparc.Weixin.MP.CommonAPIs.CommonApi.CreateMenu(token, btns);

                        AddLog(log_title, LogType.修改);
                    }
                    else
                    {
                        JsHelper.AlertAndRedirect(token.Replace("Error:", ""), "menu_list.aspx?m_id=" + m_id, "fail");
                    }
                }
                catch (ErrorJsonResultException ex)
                {
                    JsHelper.AlertAndRedirect("微信服务器繁忙，请稍后再试", "menu_list.aspx?m_id=" + m_id, "fail");
                    return;
                   
                }
               
                JsHelper.AlertAndRedirect("发布成功", "menu_list.aspx?m_id=" + m_id);
            }
           
        }
        //获取回复类型
        protected string GetReplyType(object r_type)
        {
            int type = Common.Utils.ObjToInt(r_type, -1);
            switch (type)
            {
                case (int)msg_type.文本:
                    return "文本";
                case (int)msg_type.图片:
                    return "图片";
                case (int)msg_type.语音:
                    return "语音";
                case (int)msg_type.视频:
                    return "视频";
                case (int)msg_type.单图文:
                    return "单图文";
                case (int)msg_type.多图文:
                    return "多图文";
                case (int)msg_type.外链:
                    return "外链";
                case (int)msg_type.授权:
                    return "授权";
                case (int)msg_type.模块:
                    return "模块";
                case (int)msg_type.多客服:
                    return "多客服";
                default:
                    return "无";
            }
        }
      
        //获取授权链接
        private string GetShouQuanUrl(string url, int type)
        {
            if (type ==(int)msg_type.授权)
            {
                StringBuilder strUrl = new StringBuilder();
                strUrl.Append("https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + wx_wechat.app_id);
                strUrl.Append("&redirect_uri=" + HttpUtility.UrlEncode(url));
                strUrl.Append("&response_type=code&scope=snsapi_base&state=frank#wechat_redirect");
                return strUrl.ToString();
            }
            return url;

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
             var wx_wechat = BLL.Chats.wx_wechats.GetWeChatByID(wx_id);
             if (wx_wechat != null)
             {
                 try
                 {
                     string token = BLL.Chats.wx_wechats.GetAccessToken(wx_wechat.id, wx_wechat);//  AccessTokenContainer.TryGetToken(wx_wechat.app_id, wx_wechat.app_secret);
                     if (!token.Contains("Error:"))
                     {
                         Senparc.Weixin.MP.CommonAPIs.CommonApi.DeleteMenu(token);
                         AddLog("删除自定义菜单", LogType.修改);
                         JsHelper.AlertAndRedirect("关闭成功", "menu_list.aspx?m_id=" + m_id);
                     }
                     else
                     {
                         JsHelper.AlertAndRedirect(token.Replace("Error:", ""), "menu_list.aspx?m_id=" + m_id, "fail");
                     }
                 }
                 catch (ErrorJsonResultException ex)
                 {
                     JsHelper.AlertAndRedirect("微信服务器繁忙，请稍后再试", "menu_list.aspx?m_id=" + m_id, "fail");
                 }
             }
                    
        }

      
    }
}