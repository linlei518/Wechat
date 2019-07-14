using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.BLL.Users;
using Newtonsoft.Json;
using KDWechat.Common;
using KDWechat.DAL;
using System.Data;

namespace KDWechat.Web.fans
{
    public partial class user_filter_list : Web.UI.BasePage
    {
        protected string selectString = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckUserAuthority("wechat_fans_user_manager");
            if (Request.HttpMethod == "GET")
            {
                if (!string.IsNullOrEmpty(Request.QueryString["remove"]))
                    Removed();
                if (!IsPostBack)
                    InitData();
            }
        }

        private void Removed()
        {
            int removeID = RequestHelper.GetQueryInt("remove", -1);
            if (removeID != -1)
            {
                List<Fans_Filter> filterList = CacheHelper.Get<List<Fans_Filter>>("filter_json_" + u_id);
                if (filterList != null)
                {
                    var itemToRemove = filterList.Where(x => x.id == removeID).FirstOrDefault();
                    if (itemToRemove != null)
                        filterList.Remove(itemToRemove);
                    wx_fans.GetAdvanceQuery(filterList, wx_id, u_id);
                }
            }
            //throw new NotImplementedException();
        }



        private void InitData()
        {
            var query = CacheHelper.Get<string>("adwanced_query_"+u_id);
            if (string.IsNullOrEmpty(query))
            {
                Repeater1.DataSource = GetPageList(DbDataBaseEnum.KD_USERS, "select group_id,id,headimgurl,nick_name,unionid,last_interact_time,reply_state,open_id from t_wx_fans where wx_id=" + wx_id, pageSize, page, "*", "id desc", ref totalCount);
            }
            else
            {
                List<Fans_Filter> filterList = CacheHelper.Get<List<Fans_Filter>>("filter_json_" + u_id);
                foreach (var filter in filterList)
                {
                    selectString += "<a href=\"user_filter_list.aspx?m_id=18&remove="+filter.id+"\" class=\"btn cancelBubble\" title=\"点击取消\">" + filter.name + "</a>'";
                }
                Repeater1.DataSource = GetPageList(DbDataBaseEnum.KD_USERS, query, pageSize, page, "*", "id desc", ref totalCount);
            }
            Repeater1.DataBind();
            string pageUrl = string.Format("user_filter_list.aspx?page=__id__&m_id={0}", m_id);
            div_page.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);

        }

        protected void btnSubbmit_Click(object sender, EventArgs e)
        {
            var tup = wx_fans.GetAdvanceQuery(hfJson.Value, wx_id, u_id);
            var filterList = tup.Item2;
            var query = tup.Item1;
            foreach (var filter in filterList)
            {
                selectString += "<a href=\"user_filter_list.aspx?m_id=18&remove=" + filter.id + "\" class=\"btn cancelBubble\" title=\"点击取消\">" + filter.name + "</a>'";
            }
            Repeater1.DataSource = GetPageList(DbDataBaseEnum.KD_USERS, query, pageSize, 1, "*", "id desc", ref totalCount);
            Repeater1.DataBind();
            string pageUrl = string.Format("user_filter_list.aspx?page=__id__&m_id={0}", m_id);
            div_page.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }


        protected void Repeater1_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            switch (e.Item.ItemType)
            {
                case ListItemType.Item:
                case ListItemType.AlternatingItem:
                    Repeater rep2 = (Repeater)e.Item.FindControl("tag_repeater");
                    Label gpLabel = (Label)e.Item.FindControl("GroupLabel");
                    DataRowView rowv = (DataRowView)e.Item.DataItem;//找到分类Repeater关联的数据项 
                    gpLabel.Text = BLL.Users.wx_group_tags.GetGroupName(Utils.StrToInt(rowv["group_id"].ToString(),0));//groupList.Where(x => x.id == rowv.group_id).First().title;
                    int fanID = Utils.StrToInt(rowv["id"].ToString(),0);
                    var list = wx_tags_relation.GetRelationViewByFanID(fanID);
                    rep2.DataSource = list;
                    rep2.DataBind();
                    break;
            }
        }


    }
}