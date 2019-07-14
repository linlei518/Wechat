using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;
using KDWechat.Common.Config;
using KDWechat.DAL;
namespace KDWechat.Web.wxpage.projects
{
    public partial class projectList : KDWechat.Web.UI.TemplateHelp
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Common.Utils.WriteCookie("house_recommend", "");
            //返回模板路劲、模版列表字段以及模版id
            string path = "/templates/projects/item2/projectList.html";
            switch (type)
            {
                case "item1":
                    path = "/templates/projects/item2/projectList.html";
                    break;
                case "item2":
                    path = "/templates/projects/item2/projectList_2.html";
                    break;
                case "item3":
                    path = "/templates/projects/item3/projectlist.html";
                    break;
            }
            if (path != "")
            {
                string _html = ReadFile(Server.MapPath(path), System.Text.Encoding.UTF8);


                KDWechat.DAL.t_project_groups model = null;
                if (g_id > 0)
                {
                    model = KDWechat.BLL.Chats.project_groups.GetModel(g_id);
                }
                switch (type)
                {
                    case "item1":
                    case "item2":
                        #region 模版1和模版2公共部分
                        if (gp.Trim() != "")
                        {
                            _html = _html.Replace("$is_show$", "style='display:none'");
                        }
                        else
                        {
                            _html = _html.Replace("$is_show$", "");
                        }

                        if (tname != "")
                        {
                            _html = _html.Replace("$curren_name$", tname);
                        }
                        else
                        {
                            _html = _html.Replace("$curren_name$", "请选择类型");
                        }



                        if (model != null)
                        {

                            if (model.show_open == 1)
                            {
                                _html = _html.Replace("$show_class$", "");
                                _html = _html.Replace("$show_title$", model.show_title);
                                _html = _html.Replace("$show_remark$", model.remark.Replace("\r\n", "<br>"));
                            }
                            else
                            {
                                _html = _html.Replace("$show_class$", "style='display:none'");
                                _html = _html.Replace("$show_title$", "");
                                _html = _html.Replace("$show_remark$", "");
                            }
                        }
                        else
                        {
                            _html = _html.Replace("$show_class$", "style='display:none'");
                            _html = _html.Replace("$show_title$", "");
                            _html = _html.Replace("$show_remark$", "");
                        }
                        #endregion
                        break;

                    case "item3":
                        #region 模版3高级筛选部分
                        List<int> filter = new List<int>();
                        if (model != null)
                        {
                            if (string.IsNullOrEmpty(model.filter_type))
                            {
                                if (tp > 0)
                                {
                                    filter.Add(tp); //取出组合条件的
                                }
                            }
                            else
                            {
                                //取出组合选择的
                                string[] temp_list = model.filter_type.TrimEnd(',').TrimStart(',').Split(new char[] { ',' });
                                for (int i = 0; i < temp_list.Length; i++)
                                {
                                    filter.Add(Common.Utils.StrToInt(temp_list[i], 0));
                                }
                            }


                        }
                        else
                        {

                            if (tp > 0)
                            {
                                filter.Add(tp); //取出组合条件的
                            }

                        }
                        if (filter.Count > 0)
                        {
                            System.Text.StringBuilder filterStr = new System.Text.StringBuilder();
                            List<DAL.t_sys_tags> list = BLL.Users.sys_tags.GetList<t_sys_tags>(x => x.channel_id > 1 && x.channel_id < 8);
                            foreach (int typeid in filter)
                            {
                                if (typeid == 47)
                                {
                                    #region 住宅高级筛选
                                    filterStr.Append("<div class=\"dsearchField\"><div class=\"btname\">住宅高级筛选</div><ul>");

                                    #region 地区
                                    filterStr.Append("<li>");
                                    filterStr.Append("<select name=\"ddl_region2\" id=\"ddl_region2\" class=\"select_01\"><option value=\"\">所在地区</option>");
                                    foreach (var item in list.Where(x => x.channel_id == 2).OrderBy(x => x.sort_id).ThenByDescending(x => x.id).ToList())
                                    {
                                        filterStr.Append("<option value=\"" + item.id + "\">" + item.title + "</option>");
                                    }
                                    filterStr.Append("</select></li>");
                                    #endregion

                                    #region 城市
                                    filterStr.Append("<li>");
                                    filterStr.Append("<select name=\"ddl_city2\" id=\"ddl_city2\" class=\"select_01\"><option value=\"\">所在城市</option>");
                                    foreach (var item in list.Where(x => x.channel_id == 3).OrderBy(x => x.sort_id).ThenByDescending(x => x.id).ToList())
                                    {
                                        filterStr.Append("<option value=\"" + item.id + "\">" + item.title + "</option>");
                                    }
                                    filterStr.Append("</select></li>");
                                    #endregion


                                    #region 状态
                                    filterStr.Append("<li>");
                                    filterStr.Append("<select name=\"ddl_status2\" id=\"ddl_status2\" class=\"select_01\"><option value=\"\">销售状态</option>");
                                    foreach (var item in list.Where(x => x.channel_id == 5 && x.parent_id == typeid).OrderBy(x => x.sort_id).ThenByDescending(x => x.id).ToList())
                                    {
                                        filterStr.Append("<option value=\"" + item.id + "\">" + item.title + "</option>");
                                    }
                                    filterStr.Append("</select></li>");
                                    #endregion

                                    #region 系列
                                    filterStr.Append("<li>");
                                    filterStr.Append("<select name=\"ddl_series\" id=\"ddl_series\" class=\"select_01\"><option value=\"\">系列</option>");
                                    foreach (var item in list.Where(x => x.channel_id == 8).OrderBy(x => x.sort_id).ThenByDescending(x => x.id).ToList())
                                    {
                                        filterStr.Append("<option value=\"" + item.id + "\">" + item.title + "</option>");
                                    }
                                    filterStr.Append("</select></li>");
                                    #endregion

                                    #region 房型
                                    filterStr.Append("<li>");
                                    filterStr.Append("<select name=\"ddl_room_type\" id=\"ddl_room_type\" class=\"select_01\"><option value=\"\">房型</option>");
                                    foreach (var item in list.Where(x => x.channel_id == 6).OrderBy(x => x.sort_id).ThenByDescending(x => x.id).ToList())
                                    {
                                        filterStr.Append("<option value=\"" + item.id + "\">" + item.title + "</option>");
                                    }
                                    filterStr.Append("</select></li>");
                                    #endregion


                                    #region 价格
                                    filterStr.Append("<li>");
                                    filterStr.Append("<select name=\"ddl_price\" id=\"ddl_price\" class=\"select_01\"><option value=\"\">价格</option>");
                                    foreach (var item in list.Where(x => x.channel_id == 7).OrderBy(x => x.sort_id).ThenByDescending(x => x.id).ToList())
                                    {
                                        filterStr.Append("<option value=\"" + item.id + "\">" + item.title + "</option>");
                                    }
                                    filterStr.Append("</select></li>");
                                    #endregion

                                    filterStr.Append("</ul>");
                                    filterStr.Append("<input  type=\"text\" class=\"text1\" placeholder=\"搜索楼盘\" value=\"" + key + "\" id=\"txtkey2\">");
                                    filterStr.Append("<input   type=\"button\" class=\"ssbtn\" value=\"搜索\"  onclick=\"search2();\"></div>");

                                    #endregion
                                    _html = _html.Replace("{hf1}", "1");
                                }
                                else if (typeid == 46)
                                {
                                    #region 办公高级筛选

                                    filterStr.Append("<div class=\"dsearchField\"><div class=\"btname\">办公高级筛选</div><ul class=\"ulstyle\">");

                                    #region 地区
                                    filterStr.Append("<li>");
                                    filterStr.Append("<select name=\"ddl_region3\" id=\"ddl_region3\" class=\"select_01\"><option value=\"\">所在地区</option>");
                                    foreach (var item in list.Where(x => x.channel_id == 2).OrderBy(x => x.sort_id).ThenByDescending(x => x.id).ToList())
                                    {
                                        filterStr.Append("<option value=\"" + item.id + "\">" + item.title + "</option>");
                                    }
                                    filterStr.Append("</select></li>");
                                    #endregion

                                    #region 城市
                                    filterStr.Append("<li>");
                                    filterStr.Append("<select name=\"ddl_city3\" id=\"ddl_city3\" class=\"select_01\"><option value=\"\">所在城市</option>");
                                    foreach (var item in list.Where(x => x.channel_id == 3).OrderBy(x => x.sort_id).ThenByDescending(x => x.id).ToList())
                                    {
                                        filterStr.Append("<option value=\"" + item.id + "\">" + item.title + "</option>");
                                    }
                                    filterStr.Append("</select></li>");
                                    #endregion


                                    #region 状态
                                    filterStr.Append("<li>");
                                    filterStr.Append("<select name=\"ddl_status3\" id=\"ddl_status3\" class=\"select_01\"><option value=\"\">租赁状态</option>");
                                    foreach (var item in list.Where(x => x.channel_id == 5 && x.parent_id == typeid).OrderBy(x => x.sort_id).ThenByDescending(x => x.id).ToList())
                                    {
                                        filterStr.Append("<option value=\"" + item.id + "\">" + item.title + "</option>");
                                    }
                                    filterStr.Append("</select></li>");
                                    #endregion


                                    filterStr.Append("</ul>");
                                    filterStr.Append("<input  type=\"text\" class=\"text1\" placeholder=\"搜索楼盘\"  value=\"" + key + "\" id=\"txtkey3\">");
                                    filterStr.Append("<input   type=\"button\" class=\"ssbtn\" value=\"搜索\"  onclick=\"search3();\"></div>");

                                    #endregion
                                    _html = _html.Replace("{hf2}", "1");
                                }
                            }
                            _html = _html.Replace("_filterlist_", filterStr.ToString());
                        }
                        else
                        {
                            _html = _html.Replace("{hf2}", "0");
                            _html = _html.Replace("{hf1}", "0");
                            _html = _html.Replace("_filterlist_", "");
                        }

                        if (tp == 45 || tp == 44)
                        {
                            _html = _html.Replace("_filter_", "<a href=\"http://a.app.qq.com/o/simple.jsp?pkgname=com.themobilelife.capitastar.china\">凯德购物星</a>");
                            _html = _html.Replace("_filterclass_", "gjsearch gjsearch_02");
                        }
                        else
                        {
                            _html = _html.Replace("_filter_", "<a href=\"javascript:showSearch();\">高级搜索</a>");
                            _html = _html.Replace("_filterclass_", "gjsearch");
                        }
                        #endregion
                        break;
                }




                //解析模板
                Response.Write(StartParsing(_html));

            }
            else
            {
                Response.Write("找不到模板");
            }

        }
    }
}