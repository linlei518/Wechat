using KDWechat.Common;
using KDWechat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace KDWechat.Web.handles
{
    /// <summary>
    /// ChackViewName 的摘要说明
    /// </summary>
    public class ChackViewName : IHttpHandler, IRequiresSessionState
    {
        Web.UI.BasePage bages = new UI.BasePage();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
           


            #region 检测名称是否存在及添加修改删除

            string strName = RequestHelper.GetFormString("strname");
            string lng = RequestHelper.GetFormString("lng");
            string lat = RequestHelper.GetFormString("lat");
            // int sortid = RequestHelper.GetQueryInt("sort");
            string bimg = RequestHelper.GetFormString("bimg");
            string timg = RequestHelper.GetFormString("timg");
            string contents = RequestHelper.GetFormString("contents").Replace("BR", "<br>").Replace("undefined", "");
            string summary = RequestHelper.GetFormString("summary").Replace("BR", "<br>").Replace("undefined", "");
            string wx_og_id = RequestHelper.GetFormString("wx_og_id");
            int id = Utils.ObjToInt(RequestHelper.GetFormInt("id"), 0);
            string isName = RequestHelper.GetFormString("IsName");

            if (strName != "")
            {
                if (id > 0)
                {
                    if (strName != isName)
                    {
                        var list = KDWechat.BLL.Module.md_view360_buid.GetListForName(strName, wx_og_id);
                        if (list.Count > 0)
                        {
                            context.Response.Write("1");
                        }

                        else
                        {
                            if (!DoEdit(id, strName, bimg, timg, contents, summary, lng, lat, wx_og_id))
                            {
                                context.Response.Write("3");
                            }
                            else
                            {
                                context.Response.Write("2");
                            }
                        }
                    }
                    if (strName == isName)
                    {
                        if (id > 0)
                        {
                            if (!DoEdit(id, strName, bimg, timg, contents, summary, lng, lat, wx_og_id))
                            {
                                context.Response.Write("3");
                            }
                            else
                            {
                                context.Response.Write("2");
                            }

                        }
                    }
                }
                if (id == 0)
                {
                    var list = KDWechat.BLL.Module.md_view360_buid.GetListForName(strName, wx_og_id);
                    if (list.Count > 0)
                    {
                        context.Response.Write("1");
                    }
                    else
                    {
                        if (!DoAdd(strName, bimg, timg, contents, summary, lng, lat, wx_og_id))
                        {
                            context.Response.Write("5");
                        }
                        else
                        {
                            context.Response.Write("4");
                        }
                    }
                }
            }
            #endregion

            // action 标示为数据来源
            string wx_og_ids = bages.wx_og_id;
            string action = RequestHelper.GetFormString("action");
            string actions = RequestHelper.GetFormString("action");
            string strNames = RequestHelper.GetFormString("strNames");
            string strNameType = RequestHelper.GetFormString("strNameType");
            int type_id = RequestHelper.GetFormInt("type_id");
            int p_id = RequestHelper.GetFormInt("p_id");
            int t_id = RequestHelper.GetFormInt("t_id");
            string strleft = RequestHelper.GetFormString("strleft");
            string strflot = RequestHelper.GetFormString("strflot");
            string strright = RequestHelper.GetFormString("strright");
            string strback = RequestHelper.GetFormString("strback");
            string strup = RequestHelper.GetFormString("strup");
            string strdown = RequestHelper.GetFormString("strdown");
            string strshow = RequestHelper.GetFormString("strshow");
            string strhimg = RequestHelper.GetFormString("strhimg");
            string strSum = RequestHelper.GetFormString("strSum").Replace("BR", "<br>").Replace("undefined", ""); ;
            string strSummer = RequestHelper.GetFormString("strSummer").Replace("BR", "<br>").Replace("undefined", "");
            string strHimg = RequestHelper.GetFormString("strHimg");
            #region acton=1  户型平面图
            if (action == "1")
            {
                var list = KDWechat.BLL.Module.md_view360list.GetListForName(p_id, strNames, wx_og_ids);
                if (type_id > 0)
                {
                    if (strNames != isName)
                    {
                       
                        if (list.Count > 0)
                        {
                            context.Response.Write("1");
                        }

                        else
                        {
                            if (!DoListEdit(type_id, strNames, strhimg, p_id))
                            {
                                context.Response.Write("3");
                            }
                            else
                            {
                                context.Response.Write("2");
                            }
                        }
                    }
                    if (strNames == isName)
                    {
                        if (type_id > 0)
                        {
                            if (!DoListEdit(type_id, strNames, strhimg, p_id))
                            {
                                context.Response.Write("3");
                            }
                            else
                            {
                                context.Response.Write("2");
                            }

                        }
                    }
                }
                if (type_id == 0)
                {
                    if (list.Count > 0)
                    {
                        context.Response.Write("1");
                    }
                    else
                    {
                        if (!DoListAdd(strNames, strhimg, p_id))
                        {
                            context.Response.Write("5");
                        }
                        else
                        {
                            context.Response.Write("4");
                        }
                    }
                }
            }
           
            #endregion
            #region acton=2 //户型
            int viewtsort_id = RequestHelper.GetFormInt("sort_id");
            int p_ids = RequestHelper.GetFormInt("p_id");
            int viewtype_id = RequestHelper.GetFormInt("viewtype_id");
            string showimgs = RequestHelper.GetFormString("showimgs");
            if (actions == "2")
            {
                var list = KDWechat.BLL.Module.md_view360type.GetListForName(p_ids, strNameType, wx_og_ids);
                if (viewtype_id > 0)
                {
                    if (strNameType != isName)
                    {

                        if (list.Count > 0)
                        {
                            context.Response.Write("1");
                        }

                        else
                        {
                            if (!DoTypeEdit(viewtype_id, strNameType, strSum, strSummer, strHimg, viewtsort_id, p_ids, showimgs))
                            {
                                context.Response.Write("3");
                            }
                            else
                            {
                                context.Response.Write("2");
                            }
                        }
                    }
                    if (strNameType == isName)
                    {
                        if (viewtype_id > 0)
                        {
                            if (!DoTypeEdit(viewtype_id, strNameType, strSum, strSummer, strHimg, viewtsort_id, p_ids, showimgs))
                            {
                                context.Response.Write("3");
                            }
                            else
                            {
                                context.Response.Write("2");
                            }

                        }
                    }
                }
                if (viewtype_id == 0)
                {
                    if (list.Count > 0)
                    {
                        context.Response.Write("1");
                    }
                    else
                    {
                        if (!DoTypeAdd(strNameType, strSum, strSummer, strHimg, viewtsort_id, p_ids, showimgs))
                        {
                            context.Response.Write("5");
                        }
                        else
                        {
                            context.Response.Write("4");
                        }
                    }
                }
            }

            #endregion
            #region acton=3  全景图
            if (action == "3")
            {
                var list = KDWechat.BLL.Module.md_view.GetListForName(t_id, strNames, wx_og_ids);
                if (type_id > 0)
                {
                    if (strNames != isName)
                    {

                        if (list.Count > 0)
                        {
                            context.Response.Write("1");
                        }

                        else
                        {
                            if (!DoViewEdit(type_id, strNames, strleft, strflot, strright, strback, strup, strdown, strshow, t_id))
                            {
                                context.Response.Write("3");
                            }
                            else
                            {
                                context.Response.Write("2");
                            }
                        }
                    }
                    if (strNames == isName)
                    {
                        if (type_id > 0)
                        {
                            if (!DoViewEdit(type_id, strNames, strleft, strflot, strright, strback, strup, strdown, strshow, t_id))
                            {
                                context.Response.Write("3");
                            }
                            else
                            {
                                context.Response.Write("2");
                            }

                        }
                    }
                }
                if (type_id == 0)
                {
                    if (list.Count > 0)
                    {
                        context.Response.Write("1");
                    }
                    else
                    {
                        if (!DoViewAdd(strNames, strleft, strflot, strright, strback, strup, strdown, strshow, t_id))
                        {
                            context.Response.Write("5");
                        }
                        else
                        {
                            context.Response.Write("4");
                        }
                    }
                }
            }

            #endregion
            #region//户型排序
            int type_sort_id = RequestHelper.GetQueryInt("type_sort_id");
            int type_ids = RequestHelper.GetQueryInt("type_ids");
            string actionsort = RequestHelper.GetQueryString("action");
            if (actionsort == "TypeSort")
            {
                if (type_ids != 0 && type_sort_id != 0)
                {
                    if (!DoTypeSort_id(type_ids, type_sort_id))
                    {
                        context.Response.Write("typeSortF");
                    }
                    else
                    {
                        context.Response.Write("typeSortS");
                    }
                }
            }
            #endregion
            #region//检测消息推送分类是否存在
            int wx_id = RequestHelper.GetFormInt("wx_id");
            int newtypeid = RequestHelper.GetFormInt("newtypeid");
            string newstypeName = RequestHelper.GetFormString("newstypeName");
            string news_name = RequestHelper.GetFormString("news_name");
            if (action == "newstype")
            {
                if (newstypeName != news_name)
                {
                    var list = KDWechat.BLL.Module.md_news_type.GetModel(news_name, wx_id, wx_og_id);
                    if (list != null)
                    {
                        context.Response.Write("1");
                    }
                    else
                    {
                        context.Response.Write("2");
                    }
                }
                else
                {
                    context.Response.Write("2");
                }
            
            }
            #endregion

            //#region//检测消息是否存在

            //string newsinfoName = RequestHelper.GetFormString("newsinfoName");
            //string newsinfo_name = RequestHelper.GetFormString("newsinfo_name");
            //if (action == "newsinfo")
            //{
            //    if (newsinfoName != newsinfo_name)
            //    {
            //        var list = KDWechat.BLL.Module.md_news_info.GetModel(newsinfo_name, wx_id, wx_og_id);
            //        if (list != null)
            //        {
            //            context.Response.Write("1");
            //        }
            //        else
            //        {
            //            context.Response.Write("2");
            //        }
            //    }
            //    else
            //    {
            //        context.Response.Write("2");
            //    }
            //}
            //#endregion
            #region //删除消息中的图文
            int newsid = RequestHelper.GetFormInt("newsid");
            int info_id = RequestHelper.GetFormInt("info_id");
            int news_sort = RequestHelper.GetFormInt("news_sort");

            if (action == "deletenews")
            {
                if (info_id > 0 && newsid>0)
                {
                    KDWechat.DAL.t_md_news_info mod = KDWechat.BLL.Module.md_news_info.GetModel(info_id);
                    if (mod != null)
                    {
                       
                        if (KDWechat.BLL.Module.md_news_info.Delete(info_id))
                        {
                            //成功
                            context.Response.Write("1");
                            return;
                        }
                        else
                        {
                            //失败
                            context.Response.Write("2");
                            return;
                        }
                    }
                    else
                    {
                        //失败
                        context.Response.Write("2");
                        return;
                    }
                }
                
            }

            #endregion
            #region//消息订阅添加图文
            int new_type_id = RequestHelper.GetFormInt("type_id");
            int news_id = RequestHelper.GetFormInt("new_id");
            if (action == "save_new_id")
            {
                KDWechat.DAL.t_md_news_info mod = new KDWechat.DAL.t_md_news_info();
                mod.news_ids = news_id;
                mod.type_id = new_type_id;
                mod.wx_id = wx_id;
                mod.wx_og_id = wx_og_id;
                mod.sort_id = 0;
                mod.create_time = DateTime.Now;
                mod.Istop = 0;
                int addid = KDWechat.BLL.Module.md_news_info.Add(mod).id;
                if (addid > 0)
                {
                    //成功
                    context.Response.Write("1");
                    return;
                }
                else
                {
                    //失败
                    context.Response.Write("2");
                    return;
                }

            }
            #endregion

            #region //置顶
            if (action == "topnewsli")
            {
                if (info_id != 0)
                {
                    var list = KDWechat.BLL.Module.md_news_info.GetListForTop(1, new_type_id, wx_id, wx_og_id);
                    if (list.Count > 0)
                    {
                        //已存在置顶
                        context.Response.Write("3");
                        return;
                       
                    }
                    else
                    {
                        KDWechat.DAL.t_md_news_info mod = KDWechat.BLL.Module.md_news_info.GetModel(info_id);
                        mod.news_ids = news_id;
                        mod.wx_id = wx_id;
                        mod.wx_og_id = wx_og_id;
                        mod.sort_id = 0;
                        mod.Istop = 1;
                        int addid = KDWechat.BLL.Module.md_news_info.Update(mod).id;
                        if (addid > 0)
                        {
                            //成功
                            context.Response.Write("1");
                            return;
                        }
                        else
                        {
                            //失败
                            context.Response.Write("2");
                            return;
                        }
                    }
                }
            }
            #endregion
            #region //取消置顶
            if (action == "untopnewsli")
            {
                if (info_id != 0)
                {
                    KDWechat.DAL.t_md_news_info mod = KDWechat.BLL.Module.md_news_info.GetModel(info_id);
                    mod.news_ids = news_id;
                    mod.wx_id = wx_id;
                    mod.wx_og_id = wx_og_id;
                    mod.sort_id = 0;
                    mod.Istop = 0;
                    int addid = KDWechat.BLL.Module.md_news_info.Update(mod).id;
                    if (addid > 0)
                    {
                        //成功
                        context.Response.Write("1");
                        return;
                    }
                    else
                    {
                        //失败
                        context.Response.Write("2");
                        return;
                    }
                }
            }
            #endregion
        }
        #region 楼盘添加修改方法
        protected bool DoEdit(int id, string strName, string bimg, string timg, string contents, string summary, string lng, string lat, string wx_og_id)
        {
            string log_title = "修改了";
            Web.UI.BasePage bases = new UI.BasePage();
            KDWechat.DAL.t_md_360buid model = KDWechat.BLL.Module.md_view360_buid.GetModel(id);
            model.id = id;
            model.name = strName;
            model.buidimg = bimg;
            model.tranimg = timg;
            model.introdution = contents;
            model.summary = summary;
            model.Lng = Utils.StrToInt(lng, 0).ToString();
            model.Lat = Utils.StrToInt(lat, 0).ToString();
            model.wx_og_id = wx_og_id;
            model.sort_id = 0;
            log_title += "名称为:" + strName + " 的楼盘";
            int typeid = KDWechat.BLL.Module.md_view360_buid.Update(model).id;
            if (typeid > 0)
            {
                KDWechat.DAL.t_module_wechat module = KDWechat.BLL.Chats.module_wechat.GetModelForViewBuid(bases.wx_id,typeid, 0, "t_md_360buid");
                if (module != null)
                {

                    module.wx_id = bases.wx_id; ;
                    module.wx_og_id = wx_og_id;
                    module.channel_id = 1;
                    module.module_id = 2;
                    module.app_id = typeid;
                    module.app_parent_id = 0;
                    module.app_table = "t_md_360buid";
                    module.app_name = strName;
                    module.app_img_url = "/upload/admin/images/201409/201409151108213218.jpg";
                    module.app_link_url = bases.siteConfig.weburl + "/view360/view_info_buid.aspx?p_id=" + typeid + "&wx_og_id=" + wx_og_id + "";
                    module.status = 1;
                    module.u_id = bases.u_id;

                    KDWechat.BLL.Chats.module_wechat.Update(module);
                }
                else
                {
                    KDWechat.DAL.t_module_wechat modnew = new KDWechat.DAL.t_module_wechat();
                    modnew.wx_id = bases.wx_id; ;
                    modnew.wx_og_id = wx_og_id;
                    modnew.channel_id = 1;
                    modnew.module_id = 2;
                    modnew.app_id = typeid;
                    modnew.app_parent_id = 0;
                    modnew.app_table = "t_md_360buid";
                    modnew.app_name = strName;
                    modnew.app_img_url = "/upload/admin/images/201409/201409151108213218.jpg";
                    modnew.app_link_url = bases.siteConfig.weburl + "/view360/view_info_buid.aspx?p_id=" + typeid + "&wx_og_id=" + wx_og_id + "";
                    modnew.status = 1;
                    modnew.u_id = bases.u_id;
                    modnew.create_time = DateTime.Now;
                    KDWechat.BLL.Chats.module_wechat.Add(modnew);
                }
                bases.AddLog(log_title, LogType.修改);
                return true;
            }
            else
            {
                return false;
            }
        }
        protected bool DoAdd(string strName, string bimg, string timg, string contents, string summary, string lng, string lat, string wx_og_id)
        {
            Web.UI.BasePage bases = new UI.BasePage();
            string log_title = "新建楼盘";
            KDWechat.DAL.t_md_360buid model = new KDWechat.DAL.t_md_360buid();
            model.name = strName;
            model.buidimg = bimg;
            model.tranimg = timg;
            model.introdution = contents;
            model.summary = summary;
            model.Lng = Utils.StrToInt(lng, 0).ToString();
            model.Lat = Utils.StrToInt(lat, 0).ToString();
            model.wx_og_id = wx_og_id;
            model.sort_id = 0;
            log_title += "名称为：" + strName;

            int ids = KDWechat.BLL.Module.md_view360_buid.Add(model).id;
            if (ids > 0)
            {
                KDWechat.DAL.t_module_wechat module = new KDWechat.DAL.t_module_wechat();
             
                module.wx_id = bases.wx_id; ;
                module.wx_og_id = wx_og_id;
                module.channel_id = 1;
                module.module_id = 2;
                module.app_id = ids;
                module.app_parent_id = 0;
                module.app_table = "t_md_360buid";
                module.app_name = strName;
                module.app_img_url = "/upload/admin/images/201409/201409151108213218.jpg";
                module.app_link_url = bases.siteConfig.weburl + "/view360/view_info_buid.aspx?p_id=" + ids + "&wx_og_id=" + wx_og_id + "";
                module.status = 1;
                module.u_id = bases.u_id;
                module.create_time = DateTime.Now;
                KDWechat.BLL.Chats.module_wechat.Add(module);
                bases.AddLog(log_title, LogType.添加);
                return true;

            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 户型平面图添加修改方法

        /// <summary>
        /// 增加操作
        /// </summary>
        /// <returns></returns>
        private bool DoListAdd(string strName,string himg,int p_id)
        {
            string log_title = "添加户型平面图，";
            KDWechat.DAL.t_md_360viewList model = new KDWechat.DAL.t_md_360viewList();
            model.name = Utils.ObjectToStr(strName);
            model.wx_og_id = Utils.ObjectToStr(bages.wx_og_id);
            model.himg = Utils.ObjectToStr(himg);
            model.pid =Utils.ObjToInt(p_id,0);
            log_title += "名称为：" + strName;

            int id = KDWechat.BLL.Module.md_view360list.Add(model).id;
            if (id > 0)
            {
                KDWechat.DAL.t_module_wechat module = new KDWechat.DAL.t_module_wechat();
                module.wx_id = bages.wx_id; ;
                module.wx_og_id = bages.wx_og_id;
                module.channel_id = 1;
                module.module_id = 2;
                module.app_id = id;
                module.app_parent_id = p_id;
                module.app_table = "t_md_360viewList";
                module.app_name = strName;
                module.app_img_url = "/upload/admin/images/201409/201409151108213218.jpg";
                module.app_link_url = bages.siteConfig.weburl + "/view360/view_info_house.aspx?listid=" + id + "&p_id=" + p_id + "&wx_og_id=" + bages.wx_og_id;
                module.status = 1;
                module.u_id = bages.u_id;
                module.create_time = DateTime.Now;
                KDWechat.BLL.Chats.module_wechat.Add(module);
                bages.AddLog(log_title, LogType.添加);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 修改操作
        /// </summary>
        /// <returns></returns>
        private bool DoListEdit(int id,string strName,string himg,int p_id)
        {
            string log_title = "修改了";
            KDWechat.DAL.t_md_360viewList model = new KDWechat.DAL.t_md_360viewList();
            model.id =Utils.ObjToInt(id,0);
            model.name = Utils.ObjectToStr(strName);
            model.wx_og_id = Utils.ObjectToStr(bages.wx_og_id);
            model.himg = Utils.ObjectToStr(himg);
            model.pid =Utils.ObjToInt(p_id,0);
            log_title += "名称为:" + strName + " 的户型平面图";
            int typeid = KDWechat.BLL.Module.md_view360list.Update(model).id;
            if (typeid > 0)
            {
                KDWechat.DAL.t_module_wechat module = KDWechat.BLL.Chats.module_wechat.GetModelForViewBuid(bages.wx_id,typeid, p_id, "t_md_360view");
                if (module != null)
                {

                    module.wx_id = bages.wx_id; ;
                    module.wx_og_id = bages.wx_og_id;
                    module.channel_id = 1;
                    module.module_id = 2;
                    module.app_id = id;
                    module.app_parent_id = p_id;
                    module.app_table = "t_md_360viewList";
                    module.app_name = strName;
                    module.app_img_url = "/upload/admin/images/201409/201409151108213218.jpg";
                    module.app_link_url = bages.siteConfig.weburl + "/view360/view_info_house.aspx?listid=" + id + "&p_id=" + p_id + "&wx_og_id=" + bages.wx_og_id;
                    module.status = 1;
                    module.u_id = bages.u_id;

                    KDWechat.BLL.Chats.module_wechat.Update(module);
                }
                else
                {
                    KDWechat.DAL.t_module_wechat modnew = new KDWechat.DAL.t_module_wechat();
                    modnew.wx_id = bages.wx_id; ;
                    modnew.wx_og_id = bages.wx_og_id;
                    modnew.channel_id = 1;
                    modnew.module_id = 2;
                    modnew.app_id = id;
                    modnew.app_parent_id = p_id;
                    modnew.app_table = "t_md_360viewList";
                    modnew.app_name = strName;
                    modnew.app_img_url = "/upload/admin/images/201409/201409151108213218.jpg";
                    modnew.app_link_url = bages.siteConfig.weburl + "/view360/view_info_house.aspx?listid=" + id + "&p_id=" + p_id + "&wx_og_id=" + bages.wx_og_id;
                    modnew.status = 1;
                    modnew.u_id = bages.u_id;
                    modnew.create_time = DateTime.Now;
                    KDWechat.BLL.Chats.module_wechat.Add(modnew);
                }
                bages.AddLog(log_title, LogType.修改);
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
        #region 户型添加修改方法
        /// <summary>
        /// 增加操作
        /// </summary>
        /// <returns></returns>
        private bool DoTypeAdd(string strName,string strSum,string strSummer,string strHimg,int sort_id,int p_id,string show_imgs)
        {

            string log_title = "添加户型，";
            KDWechat.DAL.t_md_360viewtype model = new KDWechat.DAL.t_md_360viewtype();
            model.name =Utils.ObjectToStr(strName);
            model.summarize = strSum;
            model.summary = strSummer;
            model.showImgs = show_imgs;
            model.himg = Utils.ObjectToStr(strHimg);
            model.wx_og_id = Utils.ObjectToStr(bages.wx_og_id);
            model.pid =Utils.ObjToInt(p_id,0);
            model.sort_id =Utils.ObjToInt(sort_id,0);
            log_title += "名称为：" + strName;
            int id = KDWechat.BLL.Module.md_view360type.Add(model).id;
            if (id > 0)
            {

                KDWechat.DAL.t_module_wechat promod = KDWechat.BLL.Chats.module_wechat.GetModelForViewBuid(bages.wx_id,p_id, 0, "t_projects");
                if (promod == null)
                {
                    KDWechat.DAL.t_projects projects=KDWechat.BLL.Chats.projects.GetModel(p_id);
                    if (projects != null)
                    {
                        KDWechat.DAL.t_module_wechat modnew = new KDWechat.DAL.t_module_wechat();
                        modnew.wx_id = bages.wx_id; ;
                        modnew.wx_og_id = bages.wx_og_id;
                        modnew.channel_id = 1;
                        modnew.module_id = 2;
                        modnew.app_id = p_id;
                        modnew.app_parent_id = 0;
                        modnew.app_table = "t_projects";
                        modnew.app_name = projects.title;
                        modnew.app_img_url = "/upload/admin/images/201409/201409151108213218.jpg";
                        modnew.app_link_url = bages.siteConfig.weburl + "/view360/view_info_buid.aspx?t_id=" + id + "&p_id=" + p_id + "&wx_og_id=" + bages.wx_og_id;
                        modnew.status = 1;
                        modnew.u_id = bages.u_id;
                        modnew.create_time = DateTime.Now;
                        KDWechat.BLL.Chats.module_wechat.Add(modnew);
                    }

                }
                KDWechat.DAL.t_module_wechat module = new KDWechat.DAL.t_module_wechat();
                module.wx_id = bages.wx_id; ;
                module.wx_og_id = bages.wx_og_id;
                module.channel_id = 1;
                module.module_id = 2;
                module.app_id = id;
                module.app_parent_id = p_id;
                module.app_table = "t_md_360viewtype";
                module.app_name = strName;
                module.app_img_url = "/upload/admin/images/201409/201409151108213218.jpg";
                module.app_link_url = bages.siteConfig.weburl + "/view360/view_info_type.aspx?t_id=" + id + "&p_id=" + p_id + "&wx_og_id=" + bages.wx_og_id;
                module.status = 1;
                module.u_id = bages.u_id;
                module.create_time = DateTime.Now;
                KDWechat.BLL.Chats.module_wechat.Add(module);
                bages.AddLog(log_title, LogType.添加);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 修改操作
        /// </summary>
        /// <returns></returns>
        private bool DoTypeEdit(int id, string strName, string strSum, string strSummer, string strHimg,int sort_id, int p_id,string show_imgs)
        {
            string log_title = "修改了";
            KDWechat.DAL.t_md_360viewtype model = new KDWechat.DAL.t_md_360viewtype();
            model.id = id;
            model.name = Utils.ObjectToStr(strName);
            model.summarize = strSum;
            model.summary = strSummer;
            model.showImgs = show_imgs;
            model.himg = Utils.ObjectToStr(strHimg);
            model.wx_og_id = Utils.ObjectToStr(bages.wx_og_id);
            model.pid =Utils.ObjToInt(p_id,0);
            model.sort_id =Utils.ObjToInt(sort_id,0);
            log_title += "名称为：" + strName + "的户型";
            int typeid = KDWechat.BLL.Module.md_view360type.Update(model).id;
            if (typeid > 0)
            {
                KDWechat.DAL.t_module_wechat promod = KDWechat.BLL.Chats.module_wechat.GetModelForViewBuid(bages.wx_id, p_id, 0, "t_projects");
                if (promod == null)
                {  
                    KDWechat.DAL.t_projects projects=KDWechat.BLL.Chats.projects.GetModel(p_id);
                    if (projects != null)
                    {
                        KDWechat.DAL.t_module_wechat modnew = new KDWechat.DAL.t_module_wechat();
                        modnew.wx_id = bages.wx_id; ;
                        modnew.wx_og_id = bages.wx_og_id;
                        modnew.channel_id = 1;
                        modnew.module_id = 2;
                        modnew.app_id = p_id;
                        modnew.app_parent_id = 0;
                        modnew.app_table = "t_projects";
                        modnew.app_name = projects.title;
                        modnew.app_img_url = "/upload/admin/images/201409/201409151108213218.jpg";
                        modnew.app_link_url = bages.siteConfig.weburl + "/view360/view_info_buid.aspx?t_id=" + typeid + "&p_id=" + p_id + "&wx_og_id=" + bages.wx_og_id;
                        modnew.status = 1;
                        modnew.u_id = bages.u_id;
                        modnew.create_time = DateTime.Now;
                        KDWechat.BLL.Chats.module_wechat.Add(modnew);
                    }

                }

                KDWechat.DAL.t_module_wechat module = KDWechat.BLL.Chats.module_wechat.GetModelForViewBuid(bages.wx_id, typeid, p_id, "t_md_360viewtype");
                if (module != null)
                {
                    module.wx_id = bages.wx_id; ;
                    module.wx_og_id = bages.wx_og_id;
                    module.channel_id = 1;
                    module.module_id = 2;
                    module.app_id = typeid;
                    module.app_parent_id = p_id;
                    module.app_table = "t_md_360viewtype";
                    module.app_name = strName;
                    module.app_img_url = "/upload/admin/images/201409/201409151108213218.jpg";
                    module.app_link_url = bages.siteConfig.weburl + "/view360/view_info_type.aspx?t_id=" + typeid + "&p_id=" + p_id + "&wx_og_id=" + bages.wx_og_id;
                    module.status = 1;
                    module.u_id = bages.u_id;

                    KDWechat.BLL.Chats.module_wechat.Update(module);
                }
                else
                {
                    KDWechat.DAL.t_module_wechat modnew = new KDWechat.DAL.t_module_wechat();
                    modnew.wx_id = bages.wx_id; ;
                    modnew.wx_og_id = bages.wx_og_id;
                    modnew.channel_id = 1;
                    modnew.module_id = 2;
                    modnew.app_id = typeid;
                    modnew.app_parent_id = p_id;
                    modnew.app_table = "t_md_360viewtype";
                    modnew.app_name = strName;
                    modnew.app_img_url = "/upload/admin/images/201409/201409151108213218.jpg";
                    modnew.app_link_url = bages.siteConfig.weburl + "/view360/view_info_type.aspx?t_id=" + typeid + "&p_id=" + p_id + "&wx_og_id=" + bages.wx_og_id;
                    modnew.status = 1;
                    modnew.u_id = bages.u_id;
                    modnew.create_time = DateTime.Now;
                    KDWechat.BLL.Chats.module_wechat.Add(modnew);
                }
                bages.AddLog(log_title, LogType.修改);
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 修改操作
        /// </summary>
        /// <returns></returns>
        private bool DoTypeSort_id(int id, int sort_id)
        {
            string log_title = "修改了";
            KDWechat.DAL.t_md_360viewtype model = KDWechat.BLL.Module.md_view360type.GetModel(id);
            model.id =Utils.ObjToInt(id,0);
            model.sort_id =Utils.ObjToInt(sort_id,0);
            log_title += "名称为：" + model.name + "的户型的排序";
            int typeid = KDWechat.BLL.Module.md_view360type.Update(model).id;
            if (typeid > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
        #region 全景图添加修改方法

        /// <summary>
        /// 增加操作
        /// </summary>
        /// <returns></returns>
        private bool DoViewAdd(string strName,string left,string flat,string right,string back,string up,string down,string showimg, int t_id)
        {
            string log_title = "添加全景图，";
            KDWechat.DAL.t_md_360view model = new KDWechat.DAL.t_md_360view();
            model.title = Utils.ObjectToStr(strName);
            model.wx_og_id = Utils.ObjectToStr(bages.wx_og_id);
            model.imgleft = Utils.ObjectToStr(left.Replace("pano", "mobile"));
            model.imgpre = Utils.ObjectToStr(flat.Replace("pano", "mobile"));
            model.imgright = Utils.ObjectToStr(right.Replace("pano", "mobile"));
            model.imgnext = Utils.ObjectToStr(back.Replace("pano", "mobile"));
            model.imgtop = Utils.ObjectToStr(up.Replace("pano", "mobile"));
            model.imgbottom = Utils.ObjectToStr(down.Replace("pano", "mobile"));
            model.showimg = Utils.ObjectToStr(showimg);
            model.p_id =Utils.ObjToInt(t_id,0);
            log_title += "名称为：" + strName;

            int id = KDWechat.BLL.Module.md_view.Add(model).id;
            if (id > 0)
            {
                KDWechat.DAL.t_module_wechat module = new KDWechat.DAL.t_module_wechat();
                module.wx_id = bages.wx_id; ;
                module.wx_og_id = bages.wx_og_id;
                module.channel_id = 1;
                module.module_id = 2;
                module.app_id = id;
                module.app_parent_id = t_id;
                module.app_table = "t_md_360view";
                module.app_name = strName;
                module.app_img_url = "/upload/admin/images/201409/201409151108213218.jpg";
                module.app_link_url = bages.siteConfig.weburl + "/view360/test.aspx?id=" + id + "&p_id=" + t_id + "&wx_og_id=" + bages.wx_og_id;
                module.status = 1;
                module.u_id = bages.u_id;
                module.create_time = DateTime.Now;
                KDWechat.BLL.Chats.module_wechat.Add(module);
                bages.AddLog(log_title, LogType.添加);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 修改操作
        /// </summary>
        /// <returns></returns>
        private bool DoViewEdit(int id, string strName, string left, string flat, string right, string back, string up, string down, string showimg, int t_id)
        {
            string log_title = "修改了";
            KDWechat.DAL.t_md_360view model = new KDWechat.DAL.t_md_360view();
            model.id =Utils.ObjToInt(id,0);
            model.title = Utils.ObjectToStr(strName);
            model.wx_og_id =Utils.ObjectToStr(bages.wx_og_id);
            model.imgleft = Utils.ObjectToStr(left.Replace("pano", "mobile"));
            model.imgpre = Utils.ObjectToStr(flat.Replace("pano", "mobile"));
            model.imgright = Utils.ObjectToStr(right.Replace("pano", "mobile"));
            model.imgnext = Utils.ObjectToStr(back.Replace("pano", "mobile"));
            model.imgtop = Utils.ObjectToStr(up.Replace("pano", "mobile"));
            model.imgbottom = Utils.ObjectToStr(down.Replace("pano", "mobile"));
            model.showimg =Utils.ObjectToStr(showimg);
            model.p_id =Utils.ObjToInt(t_id,0);
            log_title += "名称为:" + strName + " 的全景图";
            int typeid = KDWechat.BLL.Module.md_view.Update(model).id;
            if (typeid > 0)
            {
                KDWechat.DAL.t_module_wechat module = KDWechat.BLL.Chats.module_wechat.GetModelForViewBuid(bages.wx_id, typeid, t_id, "t_md_360view");
                if (module != null)
                {

                    module.wx_id = bages.wx_id; ;
                    module.wx_og_id = bages.wx_og_id;
                    module.channel_id = 1;
                    module.module_id = 2;
                    module.app_id = id;
                    module.app_parent_id = t_id;
                    module.app_table = "t_md_360view";
                    module.app_name = strName;
                    module.app_img_url = "/upload/admin/images/201409/201409151108213218.jpg";
                    module.app_link_url = bages.siteConfig.weburl + "/view360/test.aspx?id=" + id + "&p_id=" + t_id + "&wx_og_id=" + bages.wx_og_id;
                    module.status = 1;
                    module.u_id = bages.u_id;

                    KDWechat.BLL.Chats.module_wechat.Update(module);
                }
                else
                {
                    KDWechat.DAL.t_module_wechat modnew = new KDWechat.DAL.t_module_wechat();
                    modnew.wx_id = bages.wx_id; ;
                    modnew.wx_og_id = bages.wx_og_id;
                    modnew.channel_id = 1;
                    modnew.module_id = 2;
                    modnew.app_id = id;
                    modnew.app_parent_id = t_id;
                    modnew.app_table = "t_md_360view";
                    modnew.app_name = strName;
                    modnew.app_img_url = "/upload/admin/images/201409/201409151108213218.jpg";
                    modnew.app_link_url = bages.siteConfig.weburl + "/view360/test.aspx?id=" + id + "&p_id=" + t_id + "&wx_og_id=" + bages.wx_og_id;
                    modnew.status = 1;
                    modnew.u_id = bages.u_id;
                    modnew.create_time = DateTime.Now;
                    KDWechat.BLL.Chats.module_wechat.Add(modnew);
                }
                bages.AddLog(log_title, LogType.修改);
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region //检测消息推送分类是否存在
        public bool CheckNewsName(int id, int wx_id, string wx_og_id, string name)
        {

            return true;

        }
        #endregion
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        private static string[] Remove(string[] array, int index)
        {
            int length = array.Length;
            string[] result = new string[length - 1];
            Array.Copy(array, result, index);
            Array.Copy(array, index + 1, result, index, length - index - 1);
            return result;
        }
    }
}