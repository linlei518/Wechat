using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;

namespace KDWechat.Web.material
{
    public partial class select_appchildlist : Web.UI.BasePage
    {
        public int id { get { return RequestHelper.GetQueryInt("id"); } }

        public bool is_360 = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                display();
            }
        }

        /// <summary>
        /// 从内存中取得所有下级类别列表（自身迭代）
        /// </summary>
        private void GetChilds(DataTable oldData, DataTable newData, int parent_id, string parent_name, int level)
        {
            string and = "app_table='t_projects'";
            if (level == 0)
            {
                and = "app_table='t_projects'";
            }
            else if (level == 1)
            {
                and = "app_table='t_md_360viewtype'";
            }
            else if (level == 2)
            {
                and = "app_table='t_md_360view'";
            }
            DataRow[] dr = oldData.Select("app_parent_id=" + parent_id + " and " + and);
            level++;
            for (int i = 0; i < dr.Length; i++)
            {
                //添加一行数据
                DataRow row = newData.NewRow();
                row["app_id"] = dr[i]["app_id"].ToString();
                row["app_parent_id"] = dr[i]["app_parent_id"].ToString();
                row["app_table"] = dr[i]["app_table"].ToString();
                row["app_name"] = dr[i]["app_name"].ToString();
                row["app_img_url"] = dr[i]["app_img_url"].ToString();
                row["app_link_url"] = dr[i]["app_link_url"].ToString();
                row["app_all_name"] = parent_name == "" ? dr[i]["app_name"].ToString() : parent_name + "-" + dr[i]["app_name"].ToString();
                row["level"] = level.ToString();
                row["create_time"] = dr[i]["create_time"].ToString();
                newData.Rows.Add(row);
                
                //调用自身迭代
                this.GetChilds(oldData, newData, int.Parse(dr[i]["app_id"].ToString()), row["app_all_name"].ToString(), level);
            }
        }

        private void display()
        {
            if (id > 0)
            {
                DAL.t_modules model = BLL.Chats.modules.GetModel(id);
                if (model != null)
                {
                    lblTypeName.Text = model.title;
                    if (id == 2)
                    {
                       

                        #region 360全景
                        is_360 = true;
                        DataTable oldData = BLL.Chats.module_wechat.GetAllListForView360(wx_id);
                        if (oldData != null)
                        {
                            //复制结构
                            DataTable newData = oldData.Clone();
                            foreach (DataRow r1 in oldData.Rows)
                            {
                                DataRow new_row = newData.NewRow();
                                new_row["app_id"] = r1["app_id"].ToString();
                                new_row["app_parent_id"] = "0";
                                new_row["app_table"] = "t_projects";
                                new_row["app_name"] = r1["app_name"].ToString();
                                new_row["app_img_url"] = "";
                                new_row["app_link_url"] = "";
                                new_row["app_all_name"] = r1["app_name"];
                                new_row["level"] = "1";
                                new_row["create_time"] = r1["create_time"].ToString();
                                newData.Rows.Add(new_row);
                                //查找房型
                                DataTable dt2 = BLL.Chats.module_wechat.Get360List(wx_id, Common.Utils.ObjToInt(r1["app_id"], 0), 2);
                                if (dt2 != null)
                                {
                                    foreach (DataRow r2 in dt2.Rows)
                                    {
                                        //添加房型到表中
                                        DataRow new_row2 = newData.NewRow();
                                        new_row2["app_id"] = r2["app_id"].ToString();
                                        new_row2["app_parent_id"] = r2["app_parent_id"].ToString();
                                        new_row2["app_table"] = r2["app_table"].ToString();
                                        new_row2["app_name"] = r2["app_name"].ToString();
                                        new_row2["app_img_url"] = r2["app_img_url"].ToString();
                                        new_row2["app_link_url"] = r2["app_link_url"].ToString();
                                        new_row2["app_all_name"] = r1["app_name"] + "-" + r2["app_name"].ToString();
                                        new_row2["level"] = "2";
                                        new_row2["create_time"] = r2["create_time"].ToString();
                                        newData.Rows.Add(new_row2);
                                        //查找全景
                                        DataTable dt3 = BLL.Chats.module_wechat.Get360List(wx_id, Common.Utils.ObjToInt(r2["app_id"], 0), 3);
                                        if (dt3 != null)
                                        {
                                            foreach (DataRow r3 in dt3.Rows)
                                            {
                                                //添加房型到表中
                                                DataRow new_row3 = newData.NewRow();
                                                new_row3["app_id"] = r3["app_id"].ToString();
                                                new_row3["app_parent_id"] = r3["app_parent_id"].ToString();
                                                new_row3["app_table"] = r3["app_table"].ToString();
                                                new_row3["app_name"] = r3["app_name"].ToString();
                                                new_row3["app_img_url"] = r3["app_img_url"].ToString();
                                                new_row3["app_link_url"] = r3["app_link_url"].ToString();
                                                new_row3["app_all_name"] = r1["app_name"] + "-" + r2["app_name"].ToString() + "-" + r3["app_name"].ToString();
                                                new_row3["level"] = "3";
                                                new_row3["create_time"] = r3["create_time"].ToString();
                                                newData.Rows.Add(new_row3);

                                            }
                                        }

                                    }
                                }


                            }

                            repList.DataSource = newData;
                            repList.DataBind();
                        }


                        #endregion

                     

                    }
                    else
                    {
                        StringBuilder Query = new StringBuilder();
                        Query.Append(string.Format("select *, APP_NAME as app_all_name,1 as 'level' from t_module_wechat where wx_id={0} and channel_id=1 and  module_id={1} and status=1", wx_id, id));
                        repList.DataSource = GetPageList(DbDataBaseEnum.KD_WECHATS, Query.ToString(), pageSize, page, "*", "id desc", ref totalCount);
                        repList.DataBind();
                        string pageUrl = string.Format("select_appchildlist.aspx?id={0}&page=__id__", id);
                        div_page.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
                    }



                }
            }
        }

        public string GetClassName(object level)
        {
            string str = "";
            if (id == 2)
            {
                str = "class=\"l" + level + "\"";
            }
            return str;
        }

        public string GetRoomType(object app_table)
        {
            string str = "";
            switch (app_table.ToString())
            {
                case "t_projects":
                    str = "楼盘介绍";
                    break;
                case "t_md_360viewtype":
                    str = "户型介绍";
                    break;
                case "t_md_360view":
                    str = "360°全景内页";
                    break;
                case "t_md_360viewList":
                    str = "户型平面图";
                    break;
            }
            return str;
        }
    }
}