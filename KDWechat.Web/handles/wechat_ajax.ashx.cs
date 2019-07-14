using KDWechat.Common;
using KDWechat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using LitJson;
using System.Data;
using Companycn.Core.DbHelper;
using System;

namespace KDWechat.Web.handles
{
    /// <summary>
    /// wechat_ajax 的摘要说明
    /// </summary>
    public class wechat_ajax : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string action = RequestHelper.GetQueryString("action");

            if (action == "menu_sort")   //自定义菜单排序
            {
                menu_sort(context);
            }
            if (action == "navigation_validate") //验证导航菜单ID是否重复
            {
                navigation_validate(context);
            }
            if (action == "CreateUsers")   //创建子帐号
            {
                CreateUsers(context);
            }
            if (action == "UpdateUsers")            //修改子帐号
            {
                UpdateUsers(context);
            }
            if (action == "validateAccount")       //验证帐号用户名
            {
                validateAccount(context);
            }
            if (action == "fans_tag_group") //更新粉丝标签和分组
            {
                fans_tag_group(context);
            }
            if (action == "check_group")
            {
                check_group(context);
            }
            
           
           
            if (action == "check_exists_name")
            {
                check_exists_name(context);
            }

            if (action == "diy_meun")
            {
                diy_meun(context); //自定义菜单
            }

            if (action == "share_page")  //页面分享记录
            {
                share_page(context);
            }

            if (action == "good_event")  //页面点赞
            {
                good_event(context);
            }

            if (action == "check_systag")
            {
                check_systag(context);
            }

       
        }

   

        private void check_systag(HttpContext context)
        {
            int chaneel_id = RequestHelper.GetQueryInt("channel_id", 0);
            string old_name = RequestHelper.GetQueryString("old_name");
            string new_name = RequestHelper.GetQueryString("new_name");
            if (chaneel_id > 0)
            {
                bool isc = false;
                if (old_name.Trim().Length > 0)
                {
                    if (old_name.Trim() != new_name.Trim())
                    {
                        isc = KDWechat.BLL.Users.sys_tags.Exists(new_name, chaneel_id, 0);
                    }

                }
                else
                {
                    isc = KDWechat.BLL.Users.sys_tags.Exists(new_name, chaneel_id, 0);
                }

                context.Response.Write((isc == true ? 1 : 0));
            }
            else
            {
                context.Response.Write(0);
            }
        }
        /// <summary>
        /// 页面点赞
        /// </summary>
        /// <param name="context"></param>
        private void good_event(HttpContext context)
        {
            int wx_id = RequestHelper.GetQueryInt("wx_id", 0);
            int news_id = RequestHelper.GetQueryInt("news_id", 0);
            int channel_id = RequestHelper.GetQueryInt("channel_id", 0);
            int type_id = RequestHelper.GetQueryInt("type_id", 0);
            string open_id = RequestHelper.GetQueryString("open_id");
            string page_url = DESEncrypt.Decrypt(RequestHelper.GetQueryString("page_url"));
            string page_name = RequestHelper.GetQueryString("page_name");
            string channel_name = RequestHelper.GetQueryString("channel_name");
            string wx_og_id = RequestHelper.GetQueryString("wx_og_id");
            KDWechat.DAL.t_wx_fans_hisview model = KDWechat.BLL.Logs.wx_fans_hisview.GetModel(wx_id, open_id, channel_id, type_id, news_id);
            if (model != null)
            {
                KDWechat.BLL.Logs.wx_fans_hisview.DeleteFansHisviewByID(model.id);
                context.Response.Write("-1");
            }
            else
            {
                model = new t_wx_fans_hisview();
                model.channel_id = channel_id;
                model.channel_name = channel_name;
                model.news_id = news_id;
                model.open_id = open_id;
                model.page_name = page_name;
                model.page_url = page_url;
                model.type_id = type_id;
                model.view_time = DateTime.Now;
                model.wx_id = wx_id;
                model.wx_og_id = wx_og_id;
                model = KDWechat.BLL.Logs.wx_fans_hisview.CreateFansHisview(model);
                if (model != null)
                {
                    context.Response.Write("1");
                }
                else
                {
                    context.Response.Write("0");
                }
            }

        }
        /// <summary>
        /// 页面分享记录
        /// </summary>
        /// <param name="context"></param>
        private void share_page(HttpContext context)
        {
            int wx_id = RequestHelper.GetQueryInt("wx_id", 0);
            int news_id = RequestHelper.GetQueryInt("news_id", 0);
            int channel_id = RequestHelper.GetQueryInt("channel_id", 0);
            int type_id = RequestHelper.GetQueryInt("type_id", 0);
            string open_id = RequestHelper.GetQueryString("open_id");
            string page_url = DESEncrypt.Decrypt(RequestHelper.GetQueryString("page_url"));
            string page_name = RequestHelper.GetQueryString("page_name");
            string channel_name = RequestHelper.GetQueryString("channel_name");
            string wx_og_id = RequestHelper.GetQueryString("wx_og_id");
            KDWechat.DAL.t_wx_fans_hisview model = new t_wx_fans_hisview();
            model.channel_id = channel_id;
            model.channel_name = channel_name;
            model.news_id = news_id;
            model.open_id = open_id;
            model.page_name = page_name;
            model.page_url = page_url;
            model.type_id = type_id;
            model.view_time = DateTime.Now;
            model.wx_id = wx_id;
            model.wx_og_id = wx_og_id;
            model = KDWechat.BLL.Logs.wx_fans_hisview.CreateFansHisview(model);
            if (model != null)
            {
                context.Response.Write("1");
            }
            else
            {
                context.Response.Write("0");
            }
        }
        /// <summary>
        /// 自定义菜单操作
        /// </summary>
        /// <param name="context"></param>
        private void diy_meun(HttpContext context)
        {
            string reslut = "";
            string type = RequestHelper.GetQueryString("type");
            KDWechat.Web.UI.BasePage basePage = new KDWechat.Web.UI.BasePage();
            int wx_id = basePage.wx_id;
            int u_id = basePage.u_id;
            int id = RequestHelper.GetQueryInt("id", 0);
            int parent_id = RequestHelper.GetQueryInt("parent_id", 0);
            string name = RequestHelper.GetFormString("menu_name");
            string old_name = RequestHelper.GetFormString("old_name");
            string nowEditingId = RequestHelper.GetQueryString("nowEditingId");
            switch (type)
            {
                case "list":
                    StringBuilder contents = new StringBuilder();
                    List<t_wx_diy_menus> list = KDWechat.BLL.Chats.wx_diy_menus.GetListByWxIdAndParentId(wx_id, 0);
                    if (list != null)
                    {
                        foreach (t_wx_diy_menus item in list)
                        {
                            #region 一级菜单
                            contents.Append("<dl>");
                            contents.Append("<dt id=\"" + item.id + "\" " + (item.reply_type > -1 ? "class=\"hasData" + (nowEditingId == item.id.ToString() ? " current" : "") + "\"" : "") + "> <div class=\"text\"><a href=\"javascript:void(0);\" onclick=\"javascript:editNavPush(this)\">" + item.menu_name + "</a></div>");
                            contents.Append("<div class=\"btns\">");
                            contents.Append("<input type=\"button\" class=\"btn add\" onclick=\"javascript: addSubNavList(this);\" title=\"添加子导航项\">");
                            contents.Append("<input type=\"button\" class=\"btn edit\" onclick=\"javascript: editNavList(this);\" title=\"编辑\">");
                            contents.Append("<input type=\"button\" class=\"btn delete\" onclick=\"javascript: deleteNavList(this);\" title=\"删除\">");
                            contents.Append("</div>");
                            contents.Append("<div class=\"sorts\">");
                            contents.Append("<input type=\"button\" class=\"btn up\" title=\"向上\">");
                            contents.Append("<input type=\"button\" class=\"btn down\" title=\"向下\">");
                            contents.Append(" </div></dt>");

                            #region 二级菜单

                            List<t_wx_diy_menus> childList = KDWechat.BLL.Chats.wx_diy_menus.GetListByWxIdAndParentId(wx_id, item.id);
                            if (childList != null)
                            {
                                foreach (t_wx_diy_menus child in childList)
                                {
                                    contents.Append("<dd id=\"" + child.id + "\" " + (child.reply_type > -1 ? "class=\"hasData" + (nowEditingId == child.id.ToString() ? " current" : "") + "\"" : "") + ">");
                                    contents.Append("<div class=\"text\"><a href=\"javascript:void(0);\" onclick=\"javascript:editNavPush(this)\">" + child.menu_name + "</a></div>");
                                    contents.Append("<div class=\"btns\"><input type=\"button\" class=\"btn edit\" onclick=\"javascript: editNavList(this);\" title=\"编辑\">");
                                    contents.Append("<input type=\"button\" class=\"btn delete\" onclick=\"javascript: deleteNavList(this);\" title=\"删除\">");
                                    contents.Append("</div>  <div class=\"sorts\">");
                                    contents.Append(" <input type=\"button\" class=\"btn up\" title=\"向上\">");
                                    contents.Append("<input type=\"button\" class=\"btn down\" title=\"向下\">");
                                    contents.Append(" </div></dd>");
                                }
                            }
                            #endregion

                            contents.Append(" </dl>");
                            #endregion
                        }
                    }
                    reslut = contents.ToString();
                    break;
                case "edit":
                    if (!string.IsNullOrEmpty(name))
                    {
                        #region 编辑菜单名称

                        DAL.t_wx_diy_menus modeldiy = BLL.Chats.wx_diy_menus.GetModel(id);
                        if (modeldiy != null)
                        {
                            //判断字计数
                            if (parent_id == 0 && GetByteLen(name, 16) == false)
                            {
                                reslut = "{\"status\":0,\"msg\":\"一级菜单名称不多于8个汉字或16个字母\"}";

                            }
                            else if (parent_id > 0 && GetByteLen(name, 16) == false)
                            {
                                reslut = "{\"status\":0,\"msg\":\"二级菜单名称不多于8个汉字或16个字母\"}";
                            }
                            else
                            {

                                bool is_exists = false;
                                modeldiy.menu_name = name;
                                if (old_name != name)
                                {
                                    if (BLL.Chats.wx_diy_menus.Exists(modeldiy))
                                    {
                                        is_exists = true;
                                        reslut = "{\"status\":0,\"msg\":\"菜单名称已存在\"}";
                                    }
                                }
                                if (!is_exists)
                                {
                                    modeldiy = BLL.Chats.wx_diy_menus.Update(modeldiy);
                                    if (modeldiy != null)
                                    {
                                        basePage.AddLog("修改自定义菜单的" + (parent_id > 0 ? "二级" : "一级") + "菜单名称，原名称：" + old_name + "，新名称：" + name + "", LogType.修改);
                                        reslut = "{\"status\":1,\"msg\":\"保存成功\"}";
                                    }
                                    else
                                    {
                                        reslut = "{\"status\":0,\"msg\":\"保存失败\"}";
                                    }
                                }


                            }
                        }
                        else
                        {
                            reslut = "{\"status\":-1,\"msg\":\"数据已丢失\"}";
                        }
                        #endregion
                    }
                    else
                    {
                        reslut = "{\"status\":0,\"msg\":\"请输入菜单名称\"}";
                    }
                    break;
                case "add":

                    if (!string.IsNullOrEmpty(name))
                    {
                        #region 添加菜单名称
                        int count = KDWechat.BLL.Chats.wx_diy_menus.GetCountByWxIdAndParentId(wx_id, parent_id);
                        if (parent_id == 0 && count >= 3)
                        {
                            reslut = "{\"status\":0,\"msg\":\"主菜单不能超过3条\"}";
                        }
                        else if (parent_id > 0 && count >= 5)
                        {
                            reslut = "{\"status\":0,\"msg\":\"子菜单不能超过5条\"}";
                        }
                        else
                        {
                            DAL.t_wx_diy_menus model = new t_wx_diy_menus();
                            //判断字计数
                            if (parent_id == 0 && GetByteLen(name, 16) == false)
                            {
                                reslut = "{\"status\":0,\"msg\":\"一级菜单名称不多于8个汉字或16个字母\"}";

                            }
                            else if (parent_id > 0 && GetByteLen(name, 16) == false)
                            {
                                reslut = "{\"status\":0,\"msg\":\"二级菜单名称不多于8个汉字或16个字母\"}";
                            }
                            else
                            {
                                model.menu_name = name;
                                model.contents = "建设中";
                                model.create_time = DateTime.Now;
                                model.menu_key = HzToPy.GetChineseSpell(name) + Utils.Number(6, true); //菜单key(首字母大写+六位随机数),不可修改
                                model.menu_type = "click";
                                model.menu_url = "";
                                model.parent_id = parent_id;
                                model.reply_type = (int)msg_type.文本;
                                model.sort_id = 99;
                                model.soucre_id = 0;
                                model.u_id = u_id;
                                model.wx_id = wx_id;
                                model.wx_og_id = basePage.wx_og_id;

                                if (!BLL.Chats.wx_diy_menus.Exists(model))
                                {
                                    model = BLL.Chats.wx_diy_menus.Add(model);
                                    if (model != null)
                                    {
                                        if (parent_id > 0)
                                        {
                                            BLL.Chats.wx_diy_menus.UpdateStatus(parent_id);
                                        }

                                        basePage.AddLog("添加" + (parent_id > 0 ? "二级" : "一级") + "自定义菜单，名称为：" + name + "", LogType.添加);
                                        reslut = "{\"status\":1,\"msg\":\"保存成功\"}";
                                    }
                                    else
                                    {
                                        reslut = "{\"status\":0,\"msg\":\"保存失败\"}";
                                    }
                                }
                                else
                                {
                                    reslut = "{\"status\":0,\"msg\":\"菜单名称已存在\"}";
                                }



                            }
                        }

                        #endregion
                    }
                    else
                    {
                        reslut = "{\"status\":0,\"msg\":\"请输入菜单名称\"}";
                    }



                    break;

                case "del":
                    #region 删除菜单
                    if (id > 0)
                    {
                        DAL.t_wx_diy_menus m = BLL.Chats.wx_diy_menus.GetModel(id);
                        if (m != null)
                        {
                            if (KDWechat.BLL.Chats.wx_diy_menus.Delete(id))
                            {
                                basePage.AddLog("删除" + (m.parent_id > 0 ? "二级" : "一级") + "自定义菜单，名称为：" + m.menu_name + "", LogType.删除);
                                reslut = "{\"status\":1,\"msg\":\"删除成功\"}";
                            }
                            else
                            {
                                reslut = "{\"status\":0,\"msg\":\"删除失败\"}";
                            }
                        }
                        else
                        {
                            reslut = "{\"status\":0,\"msg\":\"数据丢失\"}";
                        }

                    }
                    else
                    {
                        reslut = "{\"status\":0,\"msg\":\"数据丢失\"}";
                    }
                    #endregion
                    break;
                case "show":
                    #region 获取菜单动作
                    if (id > 0)
                    {
                        DAL.t_wx_diy_menus model_shhow = BLL.Chats.wx_diy_menus.GetModel(id);
                        if (model_shhow != null)
                        {
                            int reply_type = Common.Utils.ObjToInt(model_shhow.reply_type, -1);

                            switch (reply_type)
                            {

                                case (int)msg_type.文本:
                                    if (model_shhow.contents != null)
                                    {
                                        reslut += "{\"status\":1,\"channel_id\":0,\"contents\":\"" + model_shhow.contents.Replace("\n", "\\n").Replace("\n\r", "\\r\\n").Replace("\r", "\\r") + "\"}";

                                    }
                                    else
                                    {
                                        reslut += "{\"status\":1,\"channel_id\":0,\"contents\":\"\"}";
                                    }
                                    break;
                                case (int)msg_type.图片:

                                case (int)msg_type.语音:
                                case (int)msg_type.视频:
                                    int _channel_id = 1;
                                    if (model_shhow.reply_type == (int)msg_type.语音)
                                        _channel_id = 2;
                                    else if (model_shhow.reply_type == (int)msg_type.视频)
                                        _channel_id = 3;
                                    t_wx_media_materials m = KDWechat.BLL.Chats.wx_media_materials.GetMediaMaterialByID((int)model_shhow.soucre_id);
                                    if (m != null)
                                    {

                                        reslut = "{\"status\":1,\"reply_type\":" + reply_type + ",\"channel_id\":" + _channel_id + ",\"material_id\":" + model_shhow.soucre_id + ",\"_path_link\":\"" + m.file_url + "\",\"_title\":\"" + m.title + "\",\"_create_time\":\"" + m.create_time.ToString("yyyy-MM-dd") + "\",\"_suumary\":\"" + (Common.Utils.DropHTML(m.remark, 140).Replace("\n", "").Trim()) + "\",\"is_close\":0,\"_multi_list\":\"\",\"video_img\":\"" + (_channel_id == 3 ? m.hq_music_url : "") + "\",\"video_type\":0}";
                                    }
                                    break;

                                case (int)msg_type.单图文:
                                    t_wx_news_materials m2 = KDWechat.BLL.Chats.wx_news_materials.GetModel((int)model_shhow.soucre_id);
                                    if (m2 != null)
                                    {

                                        reslut = "{\"status\":1,\"reply_type\":" + reply_type + ",\"channel_id\":4,\"material_id\":" + model_shhow.soucre_id + ",\"_path_link\":\"" + m2.cover_img + "\",\"_title\":\"" + m2.title + "\",\"_create_time\":\"" + m2.create_time.ToString("yyyy-MM-dd") + "\",\"_suumary\":\"" + m2.summary.Replace("\n", "").Trim() + "\",\"is_close\":0,\"_multi_list\":\"\",\"video_img\":\"\",\"video_type\":0}";
                                    }
                                    break;

                                case (int)msg_type.多图文:
                                    t_wx_news_materials multi = KDWechat.BLL.Chats.wx_news_materials.GetModel((int)model_shhow.soucre_id);
                                    if (multi != null)
                                    {
                                        //取出子级图文
                                        //string child_str = string.Empty;
                                        //List<t_wx_news_materials> list_news = KDWechat.BLL.Chats.wx_news_materials.GetChildList(multi.id);
                                        //if (list_news != null)
                                        //{
                                        //    foreach (t_wx_news_materials item in list_news)
                                        //    {
                                        //        child_str += "<div class='infoField'><div class='img'> <span><img src='" + item.cover_img + "' > </span> <div class='tip'>缩略图</div></div><div class='title'><h1>" + item.title + "</h1></div> </div>";
                                        //    }

                                        //}
                                        reslut = "{\"status\":1,\"reply_type\":" + reply_type + ",\"channel_id\":5,\"material_id\":" + model_shhow.soucre_id + ",\"_path_link\":\"" + multi.cover_img + "\",\"_title\":\"" + multi.title + "\",\"_create_time\":\"" + multi.create_time.ToString("yyyy-MM-dd") + "\",\"_suumary\":\"" + multi.summary.Replace("\n", "").Trim() + "\",\"is_close\":0,\"_multi_list\":\"" + multi.multi_html.Replace("\n", "").Replace("\"", "'").Trim() + "\",\"video_img\":\"\",\"video_type\":0}";


                                    }
                                    break;
                                case (int)msg_type.外链:

                                    reslut += "{\"status\":1,\"channel_id\":6,\"contents\":\"" + model_shhow.menu_url + "\"}";
                                    break;
                                case (int)msg_type.授权:

                                    reslut += "{\"status\":1,\"channel_id\":7,\"contents\":\"" + model_shhow.menu_url + "\"}";
                                    break;
                                case (int)msg_type.模块:
                                    DataTable dt = BLL.Chats.module_wechat.GetListByQuery("select id,module_id,wx_id,app_name,app_img_url,app_remark,(select title from t_modules where id=module_id) as module_name from t_module_wechat where id=" + (int)model_shhow.soucre_id);
                                    if (dt != null)
                                    {



                                        reslut = "{\"status\":1,\"reply_type\":" + reply_type + ",\"channel_id\":8,\"material_id\":" + model_shhow.soucre_id + ",\"_path_link\":\"" + dt.Rows[0]["app_img_url"] + "\",\"_title\":\"" + "【" + dt.Rows[0]["module_name"] + "】" + dt.Rows[0]["app_name"] + "\",\"_create_time\":\"\",\"_suumary\":\"" + (Common.Utils.DropHTML(dt.Rows[0]["app_remark"].ToString(), 140).Replace("\n", "").Trim()) + "\",\"is_close\":0,\"_multi_list\":\"\",\"video_img\":\"\",\"video_type\":0}";
                                    }
                                    break;
                                case (int)msg_type.多客服:
                                    reslut += "{\"status\":1,\"channel_id\":10,\"contents\":\"\"}";
                                    break;

                                default:
                                    reslut = "{\"status\":1,\"msg\":\"暂无数据\"}";
                                    break;
                            }
                        }
                        else
                        {
                            reslut = "{\"status\":0,\"msg\":\"数据丢失\"}";
                        }

                    }
                    else
                    {
                        reslut = "{\"status\":0,\"msg\":\"数据丢失\"}";
                    }
                    #endregion
                    break;

                case "event":
                    int channel_id = RequestHelper.GetQueryInt("channel_id", -1);
                    int _source_id = RequestHelper.GetQueryInt("source_id", 0);
                    string _contents = RequestHelper.GetFormString("contents");
                    if (id > 0)
                    {
                        DAL.t_wx_diy_menus model_shhow = BLL.Chats.wx_diy_menus.GetModel(id);
                        if (model_shhow != null)
                        {
                            switch (channel_id)
                            {
                                case 0:
                                    model_shhow.reply_type = (int)msg_type.文本;
                                    model_shhow.menu_type = "click";  //菜单类型，click view(超链接：view，推送事件：click)
                                    model_shhow.contents = Common.Utils.DropHTMLOnly(_contents);
                                    model_shhow.menu_url = "";
                                    break;
                                case 1:
                                    model_shhow.reply_type = (int)msg_type.图片;
                                    model_shhow.menu_type = "click";
                                    model_shhow.contents = "";

                                    break;
                                case 2:
                                    model_shhow.reply_type = (int)msg_type.语音;
                                    model_shhow.menu_type = "click";
                                    model_shhow.contents = "";

                                    break;
                                case 3:
                                    model_shhow.menu_type = "click";
                                    model_shhow.reply_type = (int)msg_type.视频;
                                    model_shhow.contents = "";
                                    break;
                                case 4:
                                    model_shhow.reply_type = (int)msg_type.单图文;
                                    model_shhow.menu_type = "click";
                                    model_shhow.contents = "";

                                    break;
                                case 5:
                                    model_shhow.reply_type = (int)msg_type.多图文;
                                    model_shhow.menu_type = "click";
                                    model_shhow.contents = "";
                                    break;
                                case 6:
                                    model_shhow.reply_type = (int)msg_type.外链;
                                    model_shhow.menu_type = "view";
                                    model_shhow.menu_url = _contents;
                                    model_shhow.contents = "";
                                    break;
                                case 7:
                                    model_shhow.reply_type = (int)msg_type.授权;
                                    model_shhow.menu_type = "view";
                                    model_shhow.menu_url = _contents;
                                    model_shhow.contents = "";
                                    break;
                                case 8:
                                    model_shhow.reply_type = (int)msg_type.模块;
                                    model_shhow.menu_type = "click";
                                    model_shhow.contents = "";
                                    break;
                                case 10:
                                    model_shhow.reply_type = (int)msg_type.多客服;
                                    model_shhow.menu_type = "click";
                                    model_shhow.contents = "";
                                    break;
                                default:                     //一级菜单
                                    model_shhow.reply_type = -1;
                                    model_shhow.menu_type = "";
                                    model_shhow.contents = "";

                                    break;
                            }

                            model_shhow.soucre_id = _source_id;

                            model_shhow = BLL.Chats.wx_diy_menus.Update(model_shhow);
                            if (model_shhow != null)
                            {
                                basePage.AddLog("修改" + (parent_id > 0 ? "二级" : "一级") + "自定义菜单的动作，菜单名称：" + model_shhow.menu_name + "，动作类型：" + ((KDWechat.Common.msg_type)model_shhow.reply_type).ToString() + "", LogType.修改);
                                reslut = "{\"status\":1,\"msg\":\"保存成功\"}";
                            }
                            else
                            {
                                reslut = "{\"status\":0,\"msg\":\"保存失败\"}";
                            }

                        }
                        else
                        {
                            reslut = "{\"status\":0,\"msg\":\"数据丢失\"}";
                        }
                    }
                    else
                    {
                        reslut = "{\"status\":0,\"msg\":\"数据丢失\"}";
                    }
                    break;
            }

            context.Response.Write(reslut);
        }

        private void check_exists_name(HttpContext context)
        {
            string db_table = DESEncrypt.Decrypt(RequestHelper.GetFormString("tb"));
            string db_name = DESEncrypt.Decrypt(RequestHelper.GetFormString("prefix"));
            string old_name = RequestHelper.GetFormString("old_name");
            string new_name = RequestHelper.GetFormString("new_name");
            int parent_id = RequestHelper.GetQueryInt("parent_id", 0);
            int old_parent_id = RequestHelper.GetQueryInt("old_par", 0);
            int channel_id = RequestHelper.GetQueryInt("channel_id", 0);
            int cate_id = RequestHelper.GetQueryInt("cate_id", 0);
            int vid = RequestHelper.GetQueryInt("vid", 0);
            if (db_table != "" && db_name != "")
            {
                int wx_id = new KDWechat.Web.UI.BasePage().wx_id;
                int result = BLL.Chats.wx_wechats.CheckUserExists(old_name, new_name, db_table, parent_id, channel_id, cate_id, wx_id, db_name.Trim().ToLower(), vid);
                context.Response.Write(result);
            }
            else
            {
                context.Response.Write(0);
            }
        }

        private void check_group(HttpContext context)
        {
            int chaneel_id = RequestHelper.GetQueryInt("channel_id", 0);
            string old_name = RequestHelper.GetQueryString("old_name");
            string new_name = RequestHelper.GetQueryString("new_name");
            if (chaneel_id > 0)
            {
                bool isc = true;
                if (old_name.Trim().Length > 0)
                {
                    if (old_name.Trim() != new_name.Trim())
                    {
                        isc = KDWechat.BLL.Users.wx_group_tags.CheckTagOrGroup(new_name, new KDWechat.Web.UI.BasePage().wx_id, chaneel_id);
                    }

                }
                else
                {
                    isc = KDWechat.BLL.Users.wx_group_tags.CheckTagOrGroup(new_name, new KDWechat.Web.UI.BasePage().wx_id, chaneel_id);
                }

                context.Response.Write((isc == true ? 0 : 1));
            }
            else
            {
                context.Response.Write(0);
            }
        }

        private string GetJsonText(JsonData jsonObj, string key)
        {
            string str = "";
            try
            {
                str = jsonObj[key] == null ? "" : Common.Utils.DropHTML(jsonObj[key].ToString());

            }
            catch (Exception)
            {
            }
            if (str == null)
            {
                str = "";
            }
            return str;
        }

        /// <summary>
        /// 更新粉丝标签和分组
        /// </summary>
        /// <param name="context"></param>
        private void fans_tag_group(HttpContext context)
        {
            int fans_id = RequestHelper.GetQueryInt("fans_id", 0);
            int chaneel_id = RequestHelper.GetQueryInt("chaneel_id", 0);
            string id_list = context.Request.QueryString["id_list"];
            string name = RequestHelper.GetQueryString("name");
            string title = RequestHelper.GetQueryString("title");
            if (fans_id > 0 && chaneel_id > 0)
            {
                if (chaneel_id == 1)  //分组
                {
                    bool isc = KDWechat.BLL.Users.wx_fans.UpdateFansGroup(fans_id, Utils.StrToInt(id_list, 0));
                    if (isc)
                    {
                        new KDWechat.Web.UI.BasePage().AddLog("修改粉丝用户“" + name + "”的分组信息为：" + title, LogType.修改);
                        context.Response.Write("分组更新成功");
                    }
                    else
                    {
                        context.Response.Write("分组更新失败");
                    }
                }
                else if (chaneel_id == 2)  //标签
                {

                    List<string> ids = new List<string>();
                    string names = "";
                    string[] list_json = id_list.Replace("},", "}&").Split(new char[] { '&' });
                    for (int i = 0; i < list_json.Length; i++)
                    {
                        if (list_json[i].Trim().Length > 0)
                        {
                            JsonData jsonData2 = JsonMapper.ToObject(list_json[i]);
                            if (jsonData2 != null)
                            {
                                ids.Add(GetJsonText(jsonData2, "id"));
                                names += GetJsonText(jsonData2, "name");
                            }
                        }


                    }
                    //if (ids.Count>0)
                    //{
                    bool isc = KDWechat.BLL.Users.wx_fans_tags.ChangeTags(new string[1] { fans_id.ToString() }, ids.ToArray());
                    if (isc)
                    {
                        new KDWechat.Web.UI.BasePage().AddLog("修改粉丝用户“" + name + "”的标签信息为：" + names, LogType.修改);
                        context.Response.Write("标签更新成功");
                    }
                    else
                    {
                        context.Response.Write("标签更新失败");
                    }
                    //}
                    //else
                    //{
                    //    context.Response.Write("数据解析失败");
                    //}

                }
            }
            else
            {
                context.Response.Write("信息丢失");
            }
        }

        #region 自定义菜单排序处理方法==============================
        private void menu_sort(HttpContext context)
        {

            int cid = Utils.ObjToInt(RequestHelper.GetString("cid"), 0);
            if (cid == 0)
            {
                return;
            }
            int px = Utils.ObjToInt(RequestHelper.GetString("px"), 0);
            BLL.Chats.wx_diy_menus.UpdateSort(cid, px);
            new KDWechat.Web.UI.BasePage().AddLog("修改自定义菜单排序", LogType.修改);
            //if (bll.UpdateField(cid, "sort_id=" + px)>0)
            //{
            //    context.Response.Write("{\"msg\":1, \"msgbox\":\"更新成功！\"}");
            //}
            //else
            //{
            //    context.Response.Write("{\"msg\":0, \"msgbox\":\"更新失败！\"}");
            //}
            //return;

        }
        #endregion

 

        #region 验证导航菜单ID是否重复==========================
        private void navigation_validate(HttpContext context)
        {
            string navname = RequestHelper.GetString("name");
            string old_name = RequestHelper.GetString("old_name");
            if (string.IsNullOrEmpty(navname))
            {
                context.Response.Write("{ \"info\":\"该导航菜单ID不可为空\", \"status\":\"n\" }");
                return;
            }
            if (navname.ToLower() == old_name.ToLower())
            {
                context.Response.Write("{ \"info\":\"该导航菜单ID可使用\", \"status\":\"y\" }");
                return;
            }
            //检查保留的名称开头
            if (navname.ToLower().StartsWith("channel_"))
            {
                context.Response.Write("{ \"info\":\"该导航菜单ID系统保留，请更换\", \"status\":\"n\" }");
                return;
            }

            if (!BLL.Users.sys_navigation.CheckNavigationName(navname))
            {
                context.Response.Write("{ \"info\":\"该导航菜单ID已被占用，请更换\", \"status\":\"n\" }");
                return;
            }
            context.Response.Write("{ \"info\":\"该导航菜单ID可使用\", \"status\":\"y\" }");
            return;
        }
        #endregion

        #region 创建子帐号==========================
        private void CreateUsers(HttpContext context)
        {
            string name = RequestHelper.GetFormString("name");
            string pwd = RequestHelper.GetString("pwd");
            string tel = RequestHelper.GetString("tel");
            string mob = RequestHelper.GetString("mob");
            string email = RequestHelper.GetString("email");
            string strNav = RequestHelper.GetFormString("strNav");
            strNav = strNav.Remove(strNav.Length - 1, 1);
            string real_name = RequestHelper.GetFormString("real_name");
            string area = RequestHelper.GetString("area");
            string par_id = RequestHelper.GetString("par_id");

            string salt = Utils.CreateSalt();
            string password = Utils.CreatePassword(pwd, salt);
            t_sys_users user = new t_sys_users()
            {
                area = Common.Utils.StrToInt(area, 0),
                create_time = DateTime.Now,
                create_ip = Utils.GetUserIp(),
                dept_name = "",// 
                email = email,
                flag = (int)Common.UserFlag.子账号,
                login_ip = Utils.GetUserIp(),
                login_time = DateTime.Now,
                mobile = mob,
                real_name = real_name,
                tel = tel,
                type_id = 0,//int.Parse(ddlBusssinessType.SelectedValue),
                user_name = name,// txtAccountName.Text,
                salt = salt,
                user_pwd = password,
                status = 1,//status,
                parent_id = Common.Utils.StrToInt(par_id, 0)
            };

            user = BLL.Users.sys_users.InsertUser(user);
            if (user.id == 0)
            {

                context.Response.Write("{ \"info\":\"子账号添加失败！\", \"status\":\"y\" }");
                return;
            }
            else
            {
                //添加权限
                DAL.t_sys_users_power power = new t_sys_users_power();
                string[] p_list = strNav.Split(new string[] { "~!@#" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string child in p_list)
                {
                    string[] c_list = child.Split('|');
                    power.u_id = user.id;
                    power.action_type = c_list[1];
                    power.wx_id = Common.Utils.StrToInt(c_list[0], 0);
                    power.nav_name = "";
                    BLL.Users.sys_users_power.InsertPower(power);
                }
                new KDWechat.Web.UI.BasePage().AddLog("创建子帐号,账号ID：" + user.id + ";账号名称：" + name + "", LogType.添加);
                context.Response.Write("{ \"info\":\"子账号添加成功！\", \"status\":\"y\" }");
                return;
                //AddLog("添加了用户：" + user.user_name, LogType.添加);

            }

        }
        #endregion

        #region 修改子帐号==========================
        private void UpdateUsers(HttpContext context)
        {
            int uid = RequestHelper.GetInt("uid", 0);
            string name = RequestHelper.GetFormString("name");
            string pwd = RequestHelper.GetString("pwd");
            string tel = RequestHelper.GetString("tel");
            string mob = RequestHelper.GetString("mob");
            string email = RequestHelper.GetString("email");
            string strNav = RequestHelper.GetFormString("strNav");
            strNav = strNav.Remove(strNav.Length - 1, 1);
            string real_name = RequestHelper.GetFormString("real_name");
            string area = RequestHelper.GetString("area");
            string par_id = RequestHelper.GetString("par_id");

            string salt = Utils.CreateSalt();
            string password = Utils.CreatePassword(pwd, salt);
            t_sys_users user = new t_sys_users()
            {
                id = uid,
                area = Common.Utils.StrToInt(area, 0),
                create_time = DateTime.Now,
                create_ip = Utils.GetUserIp(),
                dept_name = "",// 
                email = email,
                flag = (int)Common.UserFlag.子账号,
                login_ip = Utils.GetUserIp(),
                login_time = DateTime.Now,
                mobile = mob,
                real_name = real_name,
                tel = tel,
                type_id = 0,//int.Parse(ddlBusssinessType.SelectedValue),
                user_name = name,// txtAccountName.Text,
                salt = salt,
                user_pwd = password,
                status = 1,//status,
                parent_id = Common.Utils.StrToInt(par_id, 0)
            };

            bool res = BLL.Users.sys_users.UpdateUsers(user);
            if (!res)
            {

                context.Response.Write("{ \"info\":\"子账号修改失败\", \"status\":\"n\" }");
                return;
            }
            else
            {
                //添加权限
                DAL.t_sys_users_power power = new t_sys_users_power();
                BLL.Users.sys_users_power.DeletePowerByUID(user.id);
                string[] p_list = strNav.Split(new string[] { "~!@#" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string child in p_list)
                {
                    string[] c_list = child.Split('|');
                    power.u_id = user.id;
                    power.action_type = (c_list[1].Trim() == "" ? "" : ",") + c_list[1];
                    power.wx_id = Common.Utils.StrToInt(c_list[0], 0);
                    power.nav_name = "";
                    BLL.Users.sys_users_power.InsertPower(power);
                }

                new KDWechat.Web.UI.BasePage().AddLog("修改子帐号,账号ID：" + user.id + ";账号名称：" + name + "", LogType.修改);

                context.Response.Write("{ \"info\":\"子账号修改成功\", \"status\":\"y\" }");
                return;
                //AddLog("添加了用户：" + user.user_name, LogType.添加);

            }
        }
        #endregion

        #region 验证子帐号用户名==========================
        private void validateAccount(HttpContext context)
        {

            string old_name = RequestHelper.GetFormString("old_name");
            string new_name = RequestHelper.GetFormString("new_name");

            bool isc = true;
            if (old_name.Trim().Length > 0)
            {
                if (old_name.Trim() != new_name.Trim())
                {
                    isc = BLL.Users.sys_users.CheckUserName(new_name);
                }

            }
            else
            {
                isc = BLL.Users.sys_users.CheckUserName(new_name);
            }

            context.Response.Write((isc == true ? 0 : 1));

        }

        #endregion

      
    

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        protected bool GetByteLen(string str, int len)
        {
            byte[] bytes = Encoding.Default.GetBytes(str);
            int b_len = bytes.Length;
            if (b_len > len)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}