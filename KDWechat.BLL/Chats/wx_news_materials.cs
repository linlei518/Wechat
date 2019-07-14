using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KDWechat.DAL;
using EntityFramework.Extensions;
using KDWechat.Common;
using System.Data.SqlClient;
using System.Data;
using KDWechat.DBUtility;

namespace KDWechat.BLL.Chats
{
    /// <summary>
    /// 图片素材
    /// </summary>
    public class wx_news_materials
    {


        /// <summary>
        /// 提取1条图文素材
        /// </summary>
        /// <param name="id">素材ID</param>
        /// <returns>提取到的图文素材</returns>
        public static t_wx_news_materials GetModel(int id)
        {
            t_wx_news_materials material = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {

                material = db.t_wx_news_materials.Where(x => x.id == id).FirstOrDefault();
                //if (material!=null)
                //{
                //    if (string.IsNullOrEmpty(material.link_url))
                //    {
                //        material.link_url = "";
                //    }
                //}
                //material = (from x in db.t_wx_news_materials where x.id == id select x).FirstOrDefault();
            }
            return material;
        }

        /// <summary>
        /// 检测是否存在
        /// </summary>
        /// <param name="title"></param>
        /// <param name="?"></param>
        /// <param name="wx_id"></param>
        /// <returns></returns>
        public static bool Exists(string title, int channel_id, int wx_id)
        {
            bool isc = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isc = (from x in db.t_wx_news_materials where x.channel_id == channel_id && x.title == title && x.wx_id == wx_id select x.id).Count() > 0;
            }
            return isc;
        }


        /// <summary>
        /// 获取子级的图文
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public static List<t_wx_news_materials> GetChildList(int parent_id)
        {
            List<t_wx_news_materials> list = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                list = db.t_wx_news_materials.Where(x => x.par_id == parent_id).ToList();
                //list=(from x in db.t_wx_news_materials where x.par_id==parent_id select x).ToList();
            }
            return list;
        }

        /// <summary>
        /// 获取子级的图文
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public static string GetChildList(int top, int parent_id)
        {
            string childList = "";
            using (creater_wxEntities db = new creater_wxEntities())
            {

                var list = (from x in db.t_wx_news_materials where x.par_id == parent_id orderby x.id ascending select x).Take(top).ToList();
                foreach (var news in list)
                {
                    childList += " <div class=\"info\">";
                    childList += "<div class=\"img\"> <span><img src=\"" + news.cover_img + "\" alt=\"\">  </span></div>";
                    childList += "<div class=\"title\"> <h1>" + news.title + "</h1> </div> </div>";
                }

            }
            return childList;
        }

        /// <summary>
        /// 删除一条图文素材
        /// </summary>
        /// <param name="id">素材ID</param>
        /// <param name="is_multi">是否是多图文</param>
        /// <returns>是否删除成功</returns>
        public static bool Delete(int id, bool is_multi = false)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isFinish = db.t_wx_news_materials.Where(x => x.id == id).Delete() > 0;
                if (is_multi)
                {
                    db.t_wx_news_materials.Where(x => x.par_id == id).Delete();
                }
            }
            return isFinish;
        }

        /// <summary>
        /// 添加一条素材
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_wx_news_materials Add(t_wx_news_materials model)
        {
            model.summary = (model.summary??"").Replace("\r", "").Replace("\n","");
            model = EFHelper.AddWeChat<t_wx_news_materials>(model,false);
            return model;

        }

        /// <summary>
        /// 修改一条素材
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_wx_news_materials Update(t_wx_news_materials model)
        {
            model.summary = (model.summary??"").Replace("\r", "").Replace("\n","");
            model = EFHelper.UpdateWeChat<t_wx_news_materials>(model, false);
            return model;
        }

        /// <summary>
        /// 添加多图文
        /// </summary>
        /// <param name="model">父级图文</param>
        /// <param name="listChild">子级图文</param>
        /// <returns></returns>
        public static int AddMulti(t_wx_news_materials model, List<t_wx_news_materials> listChild)
        {
            int result_id = 0;
            bool isc = false;
            if (model.is_public == 1)
            {
                isc = true;
            }
            else
            {
                var wechat = wx_wechats.GetWeChatByID(model.wx_id);//获取微信号信息
                if (wechat != null)
                {
                    isc = true;
                }
            }

            if (isc)
            {
                #region 父级图文
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into t_wx_news_materials(");
                strSql.Append("wx_id,wx_og_id,u_id,title,author,cover_img,summary,contents,link_url,source_url,group_id,template_id,par_id,is_public,channel_id,status,create_time,push_type,app_id,app_name,app_link,app_type_name,app_type_img,app_table_name,multi_html,content_html,img_list)");
                strSql.Append(" values (");
                strSql.Append("@wx_id,@wx_og_id,@u_id,@title,@author,@cover_img,@summary,@contents,@link_url,@source_url,@group_id,@template_id,@par_id,@is_public,@channel_id,@status,@create_time,@push_type,@app_id,@app_name,@app_link,@app_type_name,@app_type_img,@app_table_name,@multi_html,@content_html,'" + model.img_list + "')");
                strSql.Append(";set @ReturnValue= @@IDENTITY");
                SqlParameter[] parameters = {
					new SqlParameter("@wx_id", SqlDbType.Int,4),
					new SqlParameter("@wx_og_id", SqlDbType.NVarChar,50),
					new SqlParameter("@u_id", SqlDbType.Int,4),
					new SqlParameter("@title", SqlDbType.NVarChar,50),
					new SqlParameter("@author", SqlDbType.NVarChar,50),
					new SqlParameter("@cover_img", SqlDbType.NVarChar,500),
					new SqlParameter("@summary", SqlDbType.NVarChar,500),
					new SqlParameter("@contents", SqlDbType.Text),
					new SqlParameter("@link_url", SqlDbType.NVarChar,500),
					new SqlParameter("@source_url", SqlDbType.NVarChar,500),
					new SqlParameter("@group_id", SqlDbType.Int,4),
					new SqlParameter("@template_id", SqlDbType.Int,4),
					new SqlParameter("@par_id", SqlDbType.Int,4),
					new SqlParameter("@is_public", SqlDbType.Int,4),
					new SqlParameter("@channel_id", SqlDbType.Int,4),
					new SqlParameter("@status", SqlDbType.Int,4),
					new SqlParameter("@create_time", SqlDbType.DateTime),
                           new SqlParameter("@push_type", SqlDbType.NVarChar,20),                   
                                  new SqlParameter("@app_id", SqlDbType.Int,4),                   
                               new SqlParameter("@app_name", SqlDbType.NVarChar,50),    
                                new SqlParameter("@app_link", SqlDbType.NVarChar,150),    
                                 new SqlParameter("@app_type_name", SqlDbType.NVarChar,20),    
                                  new SqlParameter("@app_type_img", SqlDbType.NVarChar,150),    
                                   new SqlParameter("@app_table_name", SqlDbType.NVarChar,20),    
                                    new SqlParameter("@multi_html", SqlDbType.NVarChar,1000),    
                                     new SqlParameter("@content_html", SqlDbType.NVarChar,1000),
                    new SqlParameter("@ReturnValue",SqlDbType.Int)};
                parameters[0].Value = (model.is_public == 1 ? 0 : model.wx_id);
                parameters[1].Value = (model.is_public == 1 ? "" : model.wx_og_id);
                parameters[2].Value = model.u_id;
                parameters[3].Value = model.title;
                parameters[4].Value = model.author;
                parameters[5].Value = model.cover_img;
                parameters[6].Value = (model.summary??"").Replace("\r", "").Replace("\n", "");
                parameters[7].Value = model.contents;
                parameters[8].Value = model.link_url;
                parameters[9].Value = model.source_url;
                parameters[10].Value = model.group_id;
                parameters[11].Value = model.template_id;
                parameters[12].Value = model.par_id;
                parameters[13].Value = model.is_public;
                parameters[14].Value = model.channel_id;
                parameters[15].Value = model.status;
                parameters[16].Value = model.create_time;

                parameters[17].Value = model.push_type;
                parameters[18].Value = model.app_id;
                parameters[19].Value = model.app_name;
                parameters[20].Value = model.app_link;
                parameters[21].Value = model.app_type_name;
                parameters[22].Value = model.app_type_img;
                parameters[23].Value = model.app_table_name;
                parameters[24].Value = model.multi_html.Replace("\r", "").Replace("\n", "");
                parameters[25].Value = model.content_html;

                parameters[26].Direction = ParameterDirection.Output;
                List<CommandInfo> sqllist = new List<CommandInfo>();
                CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
                sqllist.Add(cmd);

                #endregion
                if (listChild != null)
                {
                    StringBuilder strSql_Child;
                    int i = 0;
                    foreach (t_wx_news_materials m in listChild)
                    {
                        i++;
                        
                        #region 循环加入子级图文
                        strSql_Child = new StringBuilder();
                        strSql_Child.Append("insert into t_wx_news_materials(");
                        strSql_Child.Append("wx_id,wx_og_id,u_id,title,author,cover_img,summary,contents,link_url,source_url,group_id,template_id,par_id,is_public,channel_id,status,create_time,push_type,app_id,app_name,app_link,app_type_name,app_type_img,app_table_name,multi_html,content_html)");
                        strSql_Child.Append(" values (");
                        strSql_Child.Append("@wx_id,@wx_og_id,@u_id,@title,@author,@cover_img,@summary,@contents,@link_url,@source_url,@group_id,@template_id,@par_id,@is_public,@channel_id,@status,@create_time,@push_type,@app_id,@app_name,@app_link,@app_type_name,@app_type_img,@app_table_name,@multi_html,@content_html)");
                        SqlParameter[] parameters_child = {
					            new SqlParameter("@wx_id", SqlDbType.Int,4),
					            new SqlParameter("@wx_og_id", SqlDbType.NVarChar,50),
					            new SqlParameter("@u_id", SqlDbType.Int,4),
					            new SqlParameter("@title", SqlDbType.NVarChar,50),
					            new SqlParameter("@author", SqlDbType.NVarChar,50),
					            new SqlParameter("@cover_img", SqlDbType.NVarChar,500),
					            new SqlParameter("@summary", SqlDbType.NVarChar,500),
					            new SqlParameter("@contents", SqlDbType.Text),
					            new SqlParameter("@link_url", SqlDbType.NVarChar,500),
					            new SqlParameter("@source_url", SqlDbType.NVarChar,500),
					            new SqlParameter("@group_id", SqlDbType.Int,4),
					            new SqlParameter("@template_id", SqlDbType.Int,4),
					            new SqlParameter("@par_id", SqlDbType.Int,4),
					            new SqlParameter("@is_public", SqlDbType.Int,4),
					            new SqlParameter("@channel_id", SqlDbType.Int,4),
					            new SqlParameter("@status", SqlDbType.Int,4),
					            new SqlParameter("@create_time", SqlDbType.DateTime),
                                                           new SqlParameter("@push_type", SqlDbType.NVarChar,20),                   
                                  new SqlParameter("@app_id", SqlDbType.Int,4),                   
                               new SqlParameter("@app_name", SqlDbType.NVarChar,50),    
                                new SqlParameter("@app_link", SqlDbType.NVarChar,150),    
                                 new SqlParameter("@app_type_name", SqlDbType.NVarChar,20),    
                                  new SqlParameter("@app_type_img", SqlDbType.NVarChar,150),    
                                   new SqlParameter("@app_table_name", SqlDbType.NVarChar,20),    
                                    new SqlParameter("@multi_html", SqlDbType.NVarChar,1000),    
                                     new SqlParameter("@content_html", SqlDbType.NVarChar,1000)
                                                          };
                        parameters_child[0].Value = m.wx_id;
                        parameters_child[1].Value = m.wx_og_id;
                        parameters_child[2].Value = m.u_id;
                        parameters_child[3].Value = m.title;
                        parameters_child[4].Value = m.author;
                        parameters_child[5].Value = m.cover_img;
                        parameters_child[6].Value = (m.summary??"").Replace("\r", "").Replace("\n", "");
                        parameters_child[7].Value = m.contents;
                        parameters_child[8].Value = m.link_url;
                        parameters_child[9].Value = m.source_url;
                        parameters_child[10].Value = m.group_id;
                        parameters_child[11].Value = m.template_id;
                        parameters_child[12].Direction = ParameterDirection.InputOutput;
                        parameters_child[13].Value = m.is_public;
                        parameters_child[14].Value = m.channel_id;
                        parameters_child[15].Value = m.status;
                        parameters_child[16].Value = m.create_time;

                        parameters_child[17].Value = m.push_type;
                        parameters_child[18].Value = m.app_id;
                        parameters_child[19].Value = m.app_name;
                        parameters_child[20].Value = m.app_link;
                        parameters_child[21].Value = m.app_type_name;
                        parameters_child[22].Value = m.app_type_img;
                        parameters_child[23].Value = m.app_table_name;
                        parameters_child[24].Value = m.multi_html;
                        parameters_child[25].Value = m.content_html;
                        cmd = new CommandInfo(strSql_Child.ToString(), parameters_child);
                        sqllist.Add(cmd);
                        #endregion
                        //if (i > 9)
                        //{
                        //    break;
                        //}
                    }
                }

                KDWechat.DBUtility.DbHelperSQL.ExecuteSqlTranWithIndentity(sqllist,false);
                result_id = (int)parameters[26].Value;
                //model.content_html = model.content_html.Replace("$id$", result_id.ToString());
                //UpdateHTML(result_id, model.content_html);
            }
            return result_id;
        }

        public static void UpdateHTML(int id, string html)
        {
            string sql = "update t_wx_news_materials set content_html='" + html + "' where id=" + id;
            KDWechat.DBUtility.DbHelperSQL.ExecuteSql(sql);
        }

        /// <summary>
        /// 添加多图文
        /// </summary>
        /// <param name="model">父级图文</param>
        /// <param name="listChild">子级图文</param>
        /// <returns></returns>
        public static bool UpdateMulti(t_wx_news_materials model, List<t_wx_news_materials> listChild)
        {
            bool result = true;


            using (SqlConnection conn = new SqlConnection(KDWechat.DBUtility.DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        #region 父级图文
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update t_wx_news_materials set ");
                        strSql.Append("wx_id=@wx_id,");
                        strSql.Append("wx_og_id=@wx_og_id,");
                        strSql.Append("u_id=@u_id,");
                        strSql.Append("title=@title,");
                        strSql.Append("author=@author,");
                        strSql.Append("cover_img=@cover_img,");
                        strSql.Append("summary=@summary,");
                        strSql.Append("contents=@contents,");
                        strSql.Append("link_url=@link_url,");
                        strSql.Append("source_url=@source_url,");
                        strSql.Append("group_id=@group_id,");
                        strSql.Append("template_id=@template_id,");
                        strSql.Append("par_id=@par_id,");
                        strSql.Append("is_public=@is_public,");
                        strSql.Append("channel_id=@channel_id,");
                        strSql.Append("status=@status,push_type=@push_type,app_id=@app_id,app_name=@app_name,app_link=@app_link,app_type_name=@app_type_name,app_type_img=@app_type_img,app_table_name=@app_table_name,multi_html=@multi_html,content_html=@content_html,img_list='" + model.img_list + "' ");
                        strSql.Append(" where id=@id");
                        SqlParameter[] parameters = {
					            new SqlParameter("@wx_id", SqlDbType.Int,4),
					            new SqlParameter("@wx_og_id", SqlDbType.NVarChar,50),
					            new SqlParameter("@u_id", SqlDbType.Int,4),
					            new SqlParameter("@title", SqlDbType.NVarChar,50),
					            new SqlParameter("@author", SqlDbType.NVarChar,50),
					            new SqlParameter("@cover_img", SqlDbType.NVarChar,500),
					            new SqlParameter("@summary", SqlDbType.NVarChar,500),
					            new SqlParameter("@contents", SqlDbType.Text),
					            new SqlParameter("@link_url", SqlDbType.NVarChar,500),
					            new SqlParameter("@source_url", SqlDbType.NVarChar,500),
					            new SqlParameter("@group_id", SqlDbType.Int,4),
					            new SqlParameter("@template_id", SqlDbType.Int,4),
					            new SqlParameter("@par_id", SqlDbType.Int,4),
					            new SqlParameter("@is_public", SqlDbType.Int,4),
					            new SqlParameter("@channel_id", SqlDbType.Int,4),
					            new SqlParameter("@status", SqlDbType.Int,4),
					            new SqlParameter("@id", SqlDbType.Int,4),
                                new SqlParameter("@push_type", SqlDbType.NVarChar,20),                   
                                  new SqlParameter("@app_id", SqlDbType.Int,4),                   
                               new SqlParameter("@app_name", SqlDbType.NVarChar,50),    
                                new SqlParameter("@app_link", SqlDbType.NVarChar,150),    
                                 new SqlParameter("@app_type_name", SqlDbType.NVarChar,20),    
                                  new SqlParameter("@app_type_img", SqlDbType.NVarChar,150),    
                                   new SqlParameter("@app_table_name", SqlDbType.NVarChar,20),    
                                    new SqlParameter("@multi_html", SqlDbType.NVarChar,1000),    
                                     new SqlParameter("@content_html", SqlDbType.NVarChar,1000)    
                          };
                        parameters[0].Value = model.wx_id;
                        parameters[1].Value = model.wx_og_id;
                        parameters[2].Value = model.u_id;
                        parameters[3].Value = model.title;
                        parameters[4].Value = model.author;
                        parameters[5].Value = model.cover_img;
                        parameters[6].Value = (model.summary??"").Replace("\r", "").Replace("\n", "");
                        parameters[7].Value = model.contents;
                        parameters[8].Value = model.link_url;
                        parameters[9].Value = model.source_url;
                        parameters[10].Value = model.group_id;
                        parameters[11].Value = model.template_id;
                        parameters[12].Value = model.par_id;
                        parameters[13].Value = model.is_public;
                        parameters[14].Value = model.channel_id;
                        parameters[15].Value = model.status;
                        parameters[16].Value = model.id;

                        parameters[17].Value = model.push_type;
                        parameters[18].Value = model.app_id;
                        parameters[19].Value = model.app_name;
                        parameters[20].Value = model.app_link;
                        parameters[21].Value = model.app_type_name;
                        parameters[22].Value = model.app_type_img;
                        parameters[23].Value = model.app_table_name;
                        parameters[24].Value = model.multi_html.Replace("\r", "").Replace("\n", "");
                        parameters[25].Value = model.content_html;
                        KDWechat.DBUtility.DbHelperSQL.ExecuteSql(conn, trans, strSql.ToString(), parameters,false);

                        #endregion

                        //删除已删除的子级图文
                        DeleteChilded(conn, trans, listChild, model.id);

                        //删除全部子级图文
                        // DeleteChilded(conn, trans,  model.id);


                        #region 子级图文
                        if (listChild != null)
                        {
                            int i = 0;
                            foreach (t_wx_news_materials m in listChild)
                            {  i++;
                               
                                StringBuilder strSql_Child;
                                if (m.id > 0 && m.id!=model.id)
                                {
                                    #region 已存在，更新

                                    strSql_Child = new StringBuilder();
                                    strSql_Child.Append("update t_wx_news_materials set ");
                                    strSql_Child.Append("wx_id=@wx_id,");
                                    strSql_Child.Append("wx_og_id=@wx_og_id,");
                                    strSql_Child.Append("u_id=@u_id,");
                                    strSql_Child.Append("title=@title,");
                                    strSql_Child.Append("author=@author,");
                                    strSql_Child.Append("cover_img=@cover_img,");
                                    strSql_Child.Append("summary=@summary,");
                                    strSql_Child.Append("contents=@contents,");
                                    strSql_Child.Append("link_url=@link_url,");
                                    strSql_Child.Append("source_url=@source_url,");
                                    strSql_Child.Append("group_id=@group_id,");
                                    strSql_Child.Append("template_id=@template_id,");
                                    strSql_Child.Append("par_id=@par_id,");
                                    strSql_Child.Append("is_public=@is_public,");
                                    strSql_Child.Append("channel_id=@channel_id,");
                                    strSql_Child.Append("status=@status,push_type=@push_type,app_id=@app_id,app_name=@app_name,app_link=@app_link,app_type_name=@app_type_name,app_type_img=@app_type_img,app_table_name=@app_table_name,multi_html=@multi_html,content_html=@content_html ");
                                    strSql_Child.Append(" where id=@id");
                                    SqlParameter[] parameters_child = {
					                    new SqlParameter("@wx_id", SqlDbType.Int,4),
					                    new SqlParameter("@wx_og_id", SqlDbType.NVarChar,50),
					                    new SqlParameter("@u_id", SqlDbType.Int,4),
					                    new SqlParameter("@title", SqlDbType.NVarChar,50),
					                    new SqlParameter("@author", SqlDbType.NVarChar,50),
					                    new SqlParameter("@cover_img", SqlDbType.NVarChar,500),
					                    new SqlParameter("@summary", SqlDbType.NVarChar,500),
					                    new SqlParameter("@contents", SqlDbType.Text),
					                    new SqlParameter("@link_url", SqlDbType.NVarChar,500),
					                    new SqlParameter("@source_url", SqlDbType.NVarChar,500),
					                    new SqlParameter("@group_id", SqlDbType.Int,4),
					                    new SqlParameter("@template_id", SqlDbType.Int,4),
					                    new SqlParameter("@par_id", SqlDbType.Int,4),
					                    new SqlParameter("@is_public", SqlDbType.Int,4),
					                    new SqlParameter("@channel_id", SqlDbType.Int,4),
					                    new SqlParameter("@status", SqlDbType.Int,4),
					                    new SqlParameter("@id", SqlDbType.Int,4),
                                     new SqlParameter("@push_type", SqlDbType.NVarChar,20),                   
                                  new SqlParameter("@app_id", SqlDbType.Int,4),                   
                               new SqlParameter("@app_name", SqlDbType.NVarChar,50),    
                                new SqlParameter("@app_link", SqlDbType.NVarChar,150),    
                                 new SqlParameter("@app_type_name", SqlDbType.NVarChar,20),    
                                  new SqlParameter("@app_type_img", SqlDbType.NVarChar,150),    
                                   new SqlParameter("@app_table_name", SqlDbType.NVarChar,20),    
                                    new SqlParameter("@multi_html", SqlDbType.NVarChar,1000),    
                                     new SqlParameter("@content_html", SqlDbType.NVarChar,1000)
                                                                      };
                                    parameters_child[0].Value = m.wx_id;
                                    parameters_child[1].Value = m.wx_og_id;
                                    parameters_child[2].Value = m.u_id;
                                    parameters_child[3].Value = m.title;
                                    parameters_child[4].Value = m.author;
                                    parameters_child[5].Value = m.cover_img;
                                    parameters_child[6].Value = (m.summary ?? "").Replace("\r", "").Replace("\n", "");
                                    parameters_child[7].Value = m.contents;
                                    parameters_child[8].Value = m.link_url;
                                    parameters_child[9].Value = m.source_url;
                                    parameters_child[10].Value = m.group_id;
                                    parameters_child[11].Value = m.template_id;
                                    parameters_child[12].Value = m.par_id;
                                    parameters_child[13].Value = m.is_public;
                                    parameters_child[14].Value = m.channel_id;
                                    parameters_child[15].Value = m.status;
                                    parameters_child[16].Value = m.id;


                                    parameters_child[17].Value = m.push_type;
                                    parameters_child[18].Value = m.app_id;
                                    parameters_child[19].Value = m.app_name;
                                    parameters_child[20].Value = m.app_link;
                                    parameters_child[21].Value = m.app_type_name;
                                    parameters_child[22].Value = m.app_type_img;
                                    parameters_child[23].Value = m.app_table_name;
                                    parameters_child[24].Value = m.multi_html;
                                    parameters_child[25].Value = m.content_html;
                                    KDWechat.DBUtility.DbHelperSQL.ExecuteSql(conn, trans, strSql_Child.ToString(), parameters_child,false);
                                    #endregion
                                }
                                else
                                {
                                    #region 不存在，新建
                                    strSql_Child = new StringBuilder();
                                    strSql_Child.Append("insert into t_wx_news_materials(");
                                    strSql_Child.Append("wx_id,wx_og_id,u_id,title,author,cover_img,summary,contents,link_url,source_url,group_id,template_id,par_id,is_public,channel_id,status,create_time,push_type,app_id,app_name,app_link,app_type_name,app_type_img,app_table_name,multi_html,content_html)");
                                    strSql_Child.Append(" values (");
                                    strSql_Child.Append("@wx_id,@wx_og_id,@u_id,@title,@author,@cover_img,@summary,@contents,@link_url,@source_url,@group_id,@template_id,@par_id,@is_public,@channel_id,@status,@create_time,@push_type,@app_id,@app_name,@app_link,@app_type_name,@app_type_img,@app_table_name,@multi_html,@content_html)");
                                    SqlParameter[] parameters_child = {
					                        new SqlParameter("@wx_id", SqlDbType.Int,4),
					                        new SqlParameter("@wx_og_id", SqlDbType.NVarChar,50),
					                        new SqlParameter("@u_id", SqlDbType.Int,4),
					                        new SqlParameter("@title", SqlDbType.NVarChar,50),
					                        new SqlParameter("@author", SqlDbType.NVarChar,50),
					                        new SqlParameter("@cover_img", SqlDbType.NVarChar,500),
					                        new SqlParameter("@summary", SqlDbType.NVarChar,500),
					                        new SqlParameter("@contents", SqlDbType.Text),
					                        new SqlParameter("@link_url", SqlDbType.NVarChar,500),
					                        new SqlParameter("@source_url", SqlDbType.NVarChar,500),
					                        new SqlParameter("@group_id", SqlDbType.Int,4),
					                        new SqlParameter("@template_id", SqlDbType.Int,4),
					                        new SqlParameter("@par_id", SqlDbType.Int,4),
					                        new SqlParameter("@is_public", SqlDbType.Int,4),
					                        new SqlParameter("@channel_id", SqlDbType.Int,4),
					                        new SqlParameter("@status", SqlDbType.Int,4),
					                        new SqlParameter("@create_time", SqlDbType.DateTime),
                                            new SqlParameter("@push_type", SqlDbType.NVarChar,20),                   
                                  new SqlParameter("@app_id", SqlDbType.Int,4),                   
                               new SqlParameter("@app_name", SqlDbType.NVarChar,50),    
                                new SqlParameter("@app_link", SqlDbType.NVarChar,150),    
                                 new SqlParameter("@app_type_name", SqlDbType.NVarChar,20),    
                                  new SqlParameter("@app_type_img", SqlDbType.NVarChar,150),    
                                   new SqlParameter("@app_table_name", SqlDbType.NVarChar,20),    
                                    new SqlParameter("@multi_html", SqlDbType.NVarChar,1000),    
                                     new SqlParameter("@content_html", SqlDbType.NVarChar,1000)
                                                                      };
                                    parameters_child[0].Value = m.wx_id;
                                    parameters_child[1].Value = m.wx_og_id;
                                    parameters_child[2].Value = m.u_id;
                                    parameters_child[3].Value = m.title;
                                    parameters_child[4].Value = m.author;
                                    parameters_child[5].Value = m.cover_img;
                                    parameters_child[6].Value = (m.summary??"").Replace("\r", "").Replace("\n", "");
                                    parameters_child[7].Value = m.contents;
                                    parameters_child[8].Value = m.link_url;
                                    parameters_child[9].Value = m.source_url;
                                    parameters_child[10].Value = m.group_id;
                                    parameters_child[11].Value = m.template_id;
                                    parameters_child[12].Value = model.id;
                                    parameters_child[13].Value = m.is_public;
                                    parameters_child[14].Value = m.channel_id;
                                    parameters_child[15].Value = m.status;
                                    parameters_child[16].Value = m.create_time;

                                    parameters_child[17].Value = m.push_type;
                                    parameters_child[18].Value = m.app_id;
                                    parameters_child[19].Value = m.app_name;
                                    parameters_child[20].Value = m.app_link;
                                    parameters_child[21].Value = m.app_type_name;
                                    parameters_child[22].Value = m.app_type_img;
                                    parameters_child[23].Value = m.app_table_name;
                                    parameters_child[24].Value = m.multi_html;
                                    parameters_child[25].Value = m.content_html;
                                    KDWechat.DBUtility.DbHelperSQL.ExecuteSql(conn, trans, strSql_Child.ToString(), parameters_child,false);
                                    #endregion
                                }
                               //if (i > 9)
                               // {
                               //     break;
                               // }
                            }
                        }
                        #endregion

                        result = true;
                        trans.Commit();
                    }
                    catch (Exception)
                    {
                        result = false;
                        try
                        {
                            trans.Rollback();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }

            return result;
        }
        public static bool UpdateStatus(int id, int status)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isFinish = db.t_wx_news_materials.Where(x => x.id == id).Update(x => new t_wx_news_materials() { status = status }) > 0;
            }
            return isFinish;
        }

        public static bool CheckContains(string content_url)
        {
            bool contain = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                contain = db.t_wx_news_materials.Where(x => x.link_url == content_url).FirstOrDefault() != null;
            }
            return contain;
        }

        /// <summary>
        /// 获取素材id的引用数量（数组长度为4 ，分别为：关注回复引用数量、无匹配回复引用数量、关键字回复引用数量、自定义菜单引用数量）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<int> GetUseCount(int id)
        {
            List<int> list = new List<int>();

            //关注回复引用数量
            list.Add(Common.Utils.ObjToInt(KDWechat.DBUtility.DbHelperSQL.GetSingle(string.Format("select COUNT(1) from t_wx_basic_reply where source_id={0} and reply_type in(2,6) and channel_id=1", id)), 0));
            //无匹配回复引用数量
            list.Add(Common.Utils.ObjToInt(KDWechat.DBUtility.DbHelperSQL.GetSingle(string.Format("select COUNT(1) from t_wx_basic_reply where source_id={0} and reply_type in(2,6) and channel_id=2", id)), 0));
            //关键字回复引用数量
            list.Add(Common.Utils.ObjToInt(KDWechat.DBUtility.DbHelperSQL.GetSingle(string.Format("select COUNT(1) from t_wx_rule_reply where source_id={0} and reply_type in(2,6)", id)), 0));
            //自定义菜单引用数量
            list.Add(Common.Utils.ObjToInt(KDWechat.DBUtility.DbHelperSQL.GetSingle(string.Format("select COUNT(1) from t_wx_diy_menus where soucre_id={0} and reply_type in(2,6)", id)), 0));
            return list;
        }

        #region 私有方法
        /// <summary>
        /// 删除已删除的子级图文
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="listChild"></param>
        /// <param name="p"></param>
        private static void DeleteChilded(SqlConnection conn, SqlTransaction trans, List<t_wx_news_materials> listChild, int parent_id)
        {
            StringBuilder idList = new StringBuilder();
            if (listChild != null)
            {
                foreach (t_wx_news_materials modelt in listChild)
                {
                    if (modelt.id > 0)
                    {
                        idList.Append(modelt.id + ",");
                    }
                }
            }
            string id_list = Utils.DelLastChar(idList.ToString(), ",");
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,cover_img from t_wx_news_materials where par_id=" + parent_id);
            if (!string.IsNullOrEmpty(id_list))
            {
                strSql.Append(" and id not in(" + id_list + ")");
            }
            DataSet ds = KDWechat.DBUtility.DbHelperSQL.Query(conn, trans, strSql.ToString());
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int rows = KDWechat.DBUtility.DbHelperSQL.ExecuteSql(conn, trans, "delete from t_wx_news_materials where id=" + dr["id"].ToString()); //删除数据库
                if (rows > 0)
                {
                    //Utils.DeleteFile(dr["cover_img"].ToString()); //删除原图
                }
            }
        }

        /// <summary>
        /// 删除所有的子级图文
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="listChild"></param>
        /// <param name="p"></param>
        private static void DeleteChilded(SqlConnection conn, SqlTransaction trans, int parent_id)
        {
            if (parent_id > 0)
            {
                int rows = KDWechat.DBUtility.DbHelperSQL.ExecuteSql(conn, trans, "delete from t_wx_news_materials where  par_id=" + parent_id); //删除数据库
            }



        }
        #endregion



        public static List<t_wx_news_materials> GetList()
        {
            List<t_wx_news_materials> list = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                list = db.t_wx_news_materials.ToList();
            }
            return list;
        }
        public static List<t_wx_news_materials> GetList(int id,int p_id)
        {
            List<t_wx_news_materials> list = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                list = db.t_wx_news_materials.Where(x=>x.channel_id==id && x.par_id==p_id).ToList();
            }
            return list;
        }
      
    }
}
