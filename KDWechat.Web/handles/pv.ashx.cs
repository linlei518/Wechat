using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KDWechat.Common;

namespace KDWechat.Web.handles
{
    /// <summary>
    /// pv 的摘要说明
    /// </summary>
    public class pv : IHttpHandler
    {
        #region 属性
        /// <summary>
        /// 数据库表id
        /// </summary>
        public string table_id { get { return RequestHelper.GetQueryString("b"); } }

        /// <summary>
        /// 菜单code
        /// </summary>
        private string menu_code
        {
            get
            {
                return Common.DESEncrypt.Decrypt(Common.RequestHelper.GetQueryString("menu_code"));
            }
        }
       

        /// <summary>
        /// 微信原始id
        /// </summary>
        public string wx_og_id { get { return RequestHelper.GetQueryString("g"); } }

        /// <summary>
        /// 页面标题
        /// </summary>
        public string page_title { get { return HttpUtility.UrlDecode( RequestHelper.GetQueryString("t")); } }

        /// <summary>
        /// 页面子标题
        /// </summary>
        public string page_sub_title { get { return HttpUtility.UrlDecode(RequestHelper.GetQueryString("st")); } }

        /// <summary>
        /// 页面url
        /// </summary>
        public string page_url { get { return RequestHelper.GetQueryString("u").Replace("|","&"); } }


        /// <summary>
        /// 用户openid
        /// </summary>
        public string openId { get { return RequestHelper.GetQueryString("o"); } }

        /// <summary>
        ///  URL来源
        /// </summary>
        public string from_url { get { return RequestHelper.GetQueryString("fo").Replace("|", "&");  } }

        /// <summary>
        /// 微信id
        /// </summary>
        public int wx_id { get { return RequestHelper.GetQueryInt("w", 0); } }

        /// <summary>
        /// 对应的数据表id
        /// </summary>
        public int obj_id { get { return RequestHelper.GetQueryInt("i", 0); } }

        /// <summary>
        /// 对应数据库表的名称
        /// </summary>
        public string obj_name { get { return HttpUtility.UrlDecode(RequestHelper.GetQueryString("n")); } }
        #endregion


        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string ac = RequestHelper.GetQueryString("ac");
            switch (ac)
            {
                case "pv":
                case "sh": //分享和浏览量
                    pageview(context);
                    break;
                case "cl":  //图文点赞量
                    pageclick(context);
                    break;
                case "mc":  //菜单点击量
                    menu_click(context);
                    break;
            }

        }

        /// <summary>
        /// 菜单点击量
        /// </summary>
        /// <param name="context"></param>
        private void menu_click(HttpContext context)
        {
            if (menu_code.Length > 0)
            {
                int menu_id = Common.Utils.StrToInt(menu_code, 0);

                DAL.t_wx_diy_menus menu = BLL.Chats.wx_diy_menus.GetModel(menu_id);
                if (menu != null)
                {
                    //记录菜单点击量
                    DAL.t_st_diymenu_click model = new DAL.t_st_diymenu_click();
                    model.add_time = DateTime.Now;
                    switch (menu.reply_type)
                    {

                        case (int)msg_type.授权:
                            model.menu_action = "授权链接，链接地址：" + menu.menu_url;
                            break;
                        case (int)msg_type.外链:
                            model.menu_action = "外链，链接地址：" + menu.menu_url;
                            break;
                    }

                    model.menu_key = menu.menu_key;
                    model.menu_name = menu.menu_name;
                    model.open_id = "";
                    model.wx_id = menu.wx_id;
                    model.wx_og_id = menu.wx_og_id;
                    BLL.Statistics.st_diymenu_click.Add(model);
                    context.Response.Write(menu.menu_url);

                }
                else
                {
                    context.Response.Write("/wxpage/menu/jump_error.aspx?code=1");
                }
            }
            else
            {
                context.Response.Write("/wxpage/menu/jump_error.aspx?code=0");
            }
        }

        /// <summary>
        /// 点击率
        /// </summary>
        /// <param name="context"></param>
        private void pageclick(HttpContext context)
        {
            if (!string.IsNullOrEmpty(table_id))
            {
                Common.Config.statisticsconfig config = new BLL.Config.statistics_config().loadConfig();
                string new_table_name = BLL.Config.statistics_config.GetTableName(config, table_id);
                if (new_table_name.Length > 0)
                {
                    Common.Statistics.st_statistics model = BLL.Statistics.st_statistics.GetModel(new_table_name, openId, obj_id, Common.Utils.GetUserIp());
                    if (model != null)
                    {
                        BLL.Statistics.st_statistics.DeleteClcik(new_table_name,model.id,model.obj_id);
                        context.Response.Write("-1");
                    }
                    else
                    {
                        Common.Statistics.st_statistics model_add = new Common.Statistics.st_statistics()
                        {
                            add_time = DateTime.Now,
                            db_table_name = new_table_name,
                            from_open_id = "",
                            url_referrer=from_url,
                            obj_id = obj_id,
                            obj_name = obj_name,
                            open_id = openId,
                            page_name = page_title + (page_sub_title == "" ? "" : "-" + page_sub_title),
                            page_url = page_url,
                            user_ip = Common.Utils.GetUserIp(),
                            wx_id = wx_id,
                            wx_og_id = wx_og_id
                        };
                        if (BLL.Statistics.st_statistics.Add(model_add))
                        {
                            context.Response.Write("1");
                        }
                        else
                        {
                            context.Response.Write("0");
                        }
                        
                    }

                   
                }
            }
        }

        /// <summary>
        /// 记录浏览记录
        /// </summary>
        /// <param name="context"></param>
        private void pageview(HttpContext context)
        {
            if (!string.IsNullOrEmpty(table_id))
            {
               
                Common.Config.statisticsconfig config = new BLL.Config.statistics_config().loadConfig();
                string new_table_name = BLL.Config.statistics_config.GetTableName(config,table_id);
                if (new_table_name.Length>0)
                {
                    Common.Statistics.st_statistics model = new Common.Statistics.st_statistics()
                    {
                        add_time = DateTime.Now,
                        db_table_name = new_table_name,
                        from_open_id = "",
                        url_referrer=from_url,
                        obj_id=obj_id,
                        obj_name=obj_name,
                        open_id=openId,
                        page_name = page_title + (page_sub_title == "" ? "" : "-" + page_sub_title),
                        page_url=page_url,
                        user_ip=Common.Utils.GetUserIp(),
                        wx_id=wx_id,
                        wx_og_id=wx_og_id
                    };
                    BLL.Statistics.st_statistics.Add(model);
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}