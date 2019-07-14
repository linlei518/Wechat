using KDWechat.BLL.Chats;
using KDWechat.Common;
using Companycn.Core.DbHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.Account
{
    public partial class region_wxlist : Web.UI.BasePage
    {
        /// <summary>
        /// 关键字
        /// </summary>
        protected string key
        {
            get { return RequestHelper.GetQueryString("key"); }
        }



        /// <summary>
        /// 起始时间
        /// </summary>
        public string beginDate
        {
            get { return RequestHelper.GetQueryString("beginDate"); }
        }


        /// <summary>
        ///结束时间
        /// </summary>
        public string endDate
        {
            get { return RequestHelper.GetQueryString("endDate"); }
        }


        protected string strWxlist;

        protected void Page_Load(object sender, EventArgs e)
        {
          

            if (!IsPostBack)
            {
                if (u_type==2 || u_type==3)
                {
                    Utils.WriteCookie(KDKeys.COOKIE_WECHATS_ID, "0");
                    Utils.WriteCookie(KDKeys.COOKIE_WECHATS_WX_OG_ID, "");
                    Utils.WriteCookie(KDKeys.COOKIE_WECHATS_NAME, "");
                    Utils.WriteCookie(KDKeys.COOKIE_WECHATS_HEADIMG, "");
                    Utils.WriteCookie(KDKeys.COOKIE_WECHATS_TYPE, "0");
                }
                
                SetRefferUrl();
                InitData();
            }
        }

        private void SetRefferUrl()
        {
            hfReturlUrl.Value = "region_wxlist.aspx?m_id=" + m_id;
        }


        private void InitData()
        {
           
            StringBuilder query = new StringBuilder();
            StringBuilder query2 = new StringBuilder();
            query.Append("select qy_manager_nick,id,uid,wx_pb_name,type_id,create_time,status,(select user_name from t_sys_users where id=uid) as u_name from t_wx_wechats");
            query2.Append("select id,wx_pb_name  from t_wx_wechats ");
            switch (u_type)
            {
                case 1:
                    query.Append(" where ( uid in( select id from  t_sys_users where flag=2 union select id from  t_sys_users where flag=3  ) or uid="+u_id+")");
                    query2.Append("where ( uid in( select id from  t_sys_users where flag=2 union select id from  t_sys_users where flag=3  ) or uid=" + u_id + ")");
                    break;

                case 2:
                    query.Append(" where uid="+u_id);
                    query2.Append(" where uid=" + u_id);
                    break;

                case 3:
                    query.Append(" where id in(select distinct wx_id from  t_sys_users_power where u_id="+u_id+")");
                    query2.Append(" where id in(select distinct wx_id from  t_sys_users_power where u_id=" + u_id + ")");
                    break;

                case 4:
                    query.Append(" where 1=1");
                    break;
            }
            query2.Append(" order by id desc");
            if (!string.IsNullOrEmpty(key))
            {
                query.Append(" and  wx_pb_name like '%" + key + "%'");
            }


            if (beginDate.Length > 0 && endDate.Length > 0)
            {
                txt_date_show.Value = beginDate + " — " + endDate;
                query.Append(" and convert(varchar(10),create_time,120)>= '" + beginDate + "' and convert(varchar(10),create_time,120)<='" + endDate + "'");
            }
            else if (beginDate.Length > 0)
            {
                txt_date_show.Value = beginDate;
                query.Append(" and create_time between '" + beginDate + "'  and getdate()");
            }
            else if (endDate.Length > 0)
            {
                txt_date_show.Value = endDate;
                query.Append(" and convert(varchar(10),create_time) <='" + endDate + "' ");
            }

            Repeater1.DataSource = GetPageList(DbDataBaseEnum.KD_WECHATS,query.ToString(),pageSize,page,"*","id desc",ref totalCount);
            Repeater1.DataBind();

            txtKey.Value = key;
            txtbegin_date.Text = beginDate;
            txtend_date.Text = endDate;

            string url = "region_wxlist.aspx?page=__id__&key=" +key + "&beginDate=" + beginDate + "&endDate=" +endDate + "&m_id=" + m_id;
            div_page.InnerHtml = Utils.OutPageList(pageSize, page, totalCount, url, 8);


            DataTable dt = KDWechat.DBUtility.DbHelperSQL.Query(query2.ToString()).Tables[0];
            BindWxlist(dt);
             


        }
        /// <summary>
        /// 绑定对比公众号
        /// </summary>
        protected void BindWxlist(DataTable list)
        {
            string str = "[";
            StringBuilder strJson = new StringBuilder();

           

            foreach (DataRow r in list.Rows)
            {
                strJson.Append("{ id: '" + r["id"] + "', name: '" + r["wx_pb_name"] + "' },");
            }

            if (strJson.Length > 1)
            {
                str += strJson.ToString().TrimEnd(',');

            }
            str += "]";
            strWxlist = str;

        }
        /// <summary>
        /// 删除一条数据（执行事务,删除该微信号的所有信息，慎重使用）
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string url = "region_wxlist.aspx?page="+page+"&key=" + txtKey.Value.Trim() + "&beginDate=" + txtbegin_date.Text.Trim() + "&endDate=" + txtend_date.Text.Trim() + "&m_id=" + m_id;
            if (e.CommandName == "del")
            {
                string[] estr = e.CommandArgument.ToString().Split(',');
                int id = int.Parse(estr[0]);
                string name = estr[1];

                if (BLL.Chats.wx_wechats.DeleteWeChatsAll(id))
                {

                    AddLog(string.Format("删除了公众号:{0}", name), LogType.删除);
                    JsHelper.AlertAndRedirect("删除成功", url);
                }
                else
                    JsHelper.Alert(Page,"删除失败","true");
            }
            else if (e.CommandName == "manage")
            {
                int id = int.Parse(e.CommandArgument.ToString());
                var wechat = wx_wechats.GetWeChatByID(id);
                Utils.WriteCookie(KDKeys.COOKIE_WECHATS_ID, wechat.id.ToString());
                Utils.WriteCookie(KDKeys.COOKIE_WECHATS_WX_OG_ID, wechat.wx_og_id);
                Utils.WriteCookie(KDKeys.COOKIE_WECHATS_NAME, wechat.wx_pb_name);
                Utils.WriteCookie(KDKeys.COOKIE_WECHATS_HEADIMG, wechat.header_pic);
                Utils.WriteCookie(KDKeys.COOKIE_WECHATS_TYPE, wechat.type_id.ToString());
                Response.Redirect("~/Index.aspx?m_id=33");
            }
            else if (e.CommandName == "edit")
            {
                Response.Redirect("NewWXAccount.aspx?id=" + e.CommandArgument.ToString() + "&m_id="+m_id);
            }
            else if (e.CommandName == "status")
            {
                string title = (e.CommandSource as Button).Text;
                int id = int.Parse(e.CommandArgument.ToString());
                DAL.t_wx_wechats model = BLL.Chats.wx_wechats.GetWeChatByID(id);
                if (model != null)
                {
                    model.status = title == "开启" ? 1 : 0;
                    BLL.Chats.wx_wechats.UpdateWeChat(model);

                    AddLog(title + "微信公众号：" + (e.Item.FindControl("lblTitle") as Literal).Text, LogType.修改);
                    JsHelper.AlertAndRedirect("已" + title, url);
                }

            }
        }

        public string GetManageLink(object status, object id)
        {
            string str = "";
            if (status.ToString()=="1")
            {
                if (u_type==1 || u_type==4)
                {
                    str = "<a class=\"btn btn6\" target=\"_blank\" href=\"/select_wechat.aspx?id="+id+"\">管理</a>";
                }
                else
                {
                    str = "<a class=\"btn btn6\"   href=\"/select_wechat.aspx?id=" + id + "\">管理</a>";
                }
            }
            return str;
        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (u_type == 3 && (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem))
            {
                var item = e.Item;
                var control = item.FindControl("aEdit");
                var control2 = item.FindControl("btnDelete");
                control.Visible = false;
                control2.Visible = false;
            }

        }

        protected string GetLastOpreation(object wechatID)
        {
            string outPut = string.Empty;
            int wxid = Utils.StrToInt(wechatID.ToString(), 0);
            if (wxid != 0)
            {
                var log = BLL.Logs.wx_logs.GetModel<int>(x => x.wx_id == wxid, x => x.id, true);
                if (log != null)
                {
                    outPut = "<a href='javascript:bombbox.openBox(\"../module/show_description.aspx?id=" + log.id + "\");'>" + log.create_time + "</a>";
                }
            }
            return outPut;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string url = "region_wxlist.aspx?page=" + page + "&key=" + txtKey.Value.Trim() + "&beginDate=" + txtbegin_date.Text.Trim() + "&endDate=" + txtend_date.Text.Trim() + "&m_id=" + m_id;
            Response.Redirect(url);
        }
    }
}