using KDWechat.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.logs
{
    public partial class list : KDWechat.Web.UI.BasePage
    {
        protected string strWxids = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (u_type == 1 )  //总部
            {
                CheckUserAuthority("head_permissions");
            }
            

            if (!IsPostBack)
            {
                BindWechat();
                BindList();
            }
        }
        /// <summary>
        /// 关键字
        /// </summary>
        protected string key
        {
            get { return RequestHelper.GetQueryString("key"); }
        }
        /// <summary>
        /// 公众号id
        /// </summary>
        protected int wxid
        {
            get { return RequestHelper.GetQueryInt("wxid", -1); }
        }
        /// <summary>
        /// 操作类型id
        /// </summary>
        protected int type
        {
            get { return RequestHelper.GetQueryInt("type", -1); }
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
        /// <summary>
        /// 绑定公众号
        /// </summary>
        protected void BindWechat() 
        {
            if (u_type == (int)UserFlag.总部账号 || u_type == (int)UserFlag.超级管理员)  
            {
                ddlWechat.DataSource = BLL.Chats.wx_wechats.GetList();
            }
            else
            {
                //List<DAL.t_wx_wechats> lists = BLL.Chats.wx_wechats.GetListByUid(u_id);
                //if(lists.Count>0)
                //{
                //    foreach(DAL.t_wx_wechats wx in lists)
                //    {
                //        strWxids += wx.id.ToString() + ",";
                //    }
                //}

                ddlWechat.DataSource = BLL.Chats.wx_wechats.GetListByUid(u_id);
            }
            ddlWechat.DataTextField = "wx_pb_name";
            ddlWechat.DataValueField = "id";
            ddlWechat.DataBind();
            ddlWechat.Items.Insert(0,new ListItem("选择公众号","-1"));

            ddlWechat.SelectedValue = wxid.ToString();
            ddlType.SelectedValue = type.ToString();
            txtKey.Value = key;

            txtbegin_date.Text = beginDate;
            txtend_date.Text = endDate;

            if (beginDate.Trim() != "" && endDate.Trim() != "")
            {
                txt_date_show.Value = beginDate + " — " + endDate;
            }
            else if (beginDate.Trim() != "")
            {
                txt_date_show.Value = beginDate;
            }
            else if (endDate.Trim() != "")
            {
                txt_date_show.Value = endDate;
            }
        }
        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindList()
        {
            StringBuilder Query = new StringBuilder();

            Query.Append("select ID,u_id,wx_id,login_name,contents,ip,type,create_time from t_wx_logs where 1=1");

            //if(!string.IsNullOrEmpty(strWxids))
            //{
            //    strWxids = strWxids.Remove(strWxids.Length-1,1);
            //    Query.Append(" and wx_id in ("+strWxids+")");
            //}
            if (wxid > -1)
            {
                Query.Append(" and wx_id=" + wxid);
            }
            else { 
                //判断是否是地区帐号 韦章飞修改
                if (u_type==(int)UserFlag.地区账号)
                {
                    Query.Append(" and u_id in(select id from t_sys_users where parent_id=" + u_id + " or id=" + u_id + ")");
                }
                else if (u_type==(int)UserFlag.子账号)
                {
                     Query.Append("  wx_id in(select wx_id from t_sys_users_power where u_id="+u_id+")");
                }
            }
            if (type > -1)
            {
                Query.Append(" and type=" + type);
            }
            if (beginDate.Length > 0 && endDate.Length > 0)
            {
                Query.Append(" and CONVERT(varchar(10),create_time,120) Between  CONVERT(varchar(10),'" + beginDate + "',120) and CONVERT(varchar(10),'" + endDate + "',120)");
            }
            else if (beginDate.Length > 0)
            {
                Query.Append(" and CONVERT(varchar(10),create_time,120) Between CONVERT(varchar(10),'" + beginDate + "',120) and convert(varchar(10),getdate(),120)");
            }
            else if (endDate.Length > 0)
            {
                Query.Append(" and CONVERT(varchar(10),create_time,120) <= CONVERT(varchar(10),'" + endDate + "',120) ");
            }
            if (!string.IsNullOrEmpty(key))
            {
                Query.Append(" and contents like '%" + key + "%'");
            }
           
            repList.DataSource = GetPageList(DbDataBaseEnum.KD_LOGS, Query.ToString(), pageSize, page, "*", "id desc", ref totalCount);
            repList.DataBind();
            string pageUrl = string.Format("list.aspx?key={0}&beginDate={1}&endDate={2}&wxid={3}&type={4}&m_id={5}&page=__id__", HttpUtility.UrlEncode(Common.Utils.Filter(txtKey.Value.Trim())), Common.Utils.Filter(txtbegin_date.Text.Trim()), Common.Utils.Filter(txtend_date.Text.Trim()), ddlWechat.SelectedValue, ddlType.SelectedValue, m_id);
            div_page.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string pageUrl = string.Format("list.aspx?key={0}&beginDate={1}&endDate={2}&wxid={3}&type={4}&m_id={5}", HttpUtility.UrlEncode(Common.Utils.Filter(txtKey.Value.Trim())), Common.Utils.Filter(txtbegin_date.Text.Trim()), Common.Utils.Filter(txtend_date.Text.Trim()), ddlWechat.SelectedValue, ddlType.SelectedValue, m_id);

            Response.Redirect(pageUrl);
       }
        /// <summary>
        /// 按公众号筛选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlWechat_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pageUrl = string.Format("list.aspx?key={0}&beginDate={1}&endDate={2}&wxid={3}&type={4}&m_id={5}", HttpUtility.UrlEncode(Common.Utils.Filter(txtKey.Value.Trim())), Common.Utils.Filter(txtbegin_date.Text.Trim()), Common.Utils.Filter(txtend_date.Text.Trim()), ddlWechat.SelectedValue, ddlType.SelectedValue, m_id);
            
            Response.Redirect(pageUrl);
        }
        /// <summary>
        /// 按操作类型筛选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pageUrl = string.Format("list.aspx?key={0}&beginDate={1}&endDate={2}&wxid={3}&type={4}&m_id={5}", HttpUtility.UrlEncode(Common.Utils.Filter(txtKey.Value.Trim())), Common.Utils.Filter(txtbegin_date.Text.Trim()), Common.Utils.Filter(txtend_date.Text.Trim()), ddlWechat.SelectedValue, ddlType.SelectedValue, m_id);

            Response.Redirect(pageUrl);
        }
        //获取操作类型
        protected string GetType(object _type)
        {
            int type = Common.Utils.ObjToInt(_type,0);
            switch (type) 
            {
                case (int)LogType.添加:
                    return "添加";
                case (int)LogType.修改:
                    return "修改";
                case (int)LogType.删除:
                    return "删除";
                default:
                    return "";
            }
        }
        /// <summary>
        /// 根据Id获取公众号名称
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        protected string GetWxnameById(object _id) 
        {
            int Id = Utils.ObjToInt(_id,0);
            DAL.t_wx_wechats model = BLL.Chats.wx_wechats.GetWeChatByID(Id);
            if (model != null)
            {
                return model.wx_pb_name;
            }
            else 
            {
                return "";
            }
        }
    }
}