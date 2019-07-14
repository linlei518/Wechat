using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.BLL.Users;
using KDWechat.Common;
using System.Linq.Expressions;
using KDWechat.DAL;
using Newtonsoft.Json;

namespace KDWechat.Web.fans
{
    public partial class all_user_list : Web.UI.BasePage
    {
        #region 筛选条件
        protected int wxID { get { return RequestHelper.GetQueryInt("wc", -1); } }
        protected int msgContain { get { return RequestHelper.GetQueryInt("mc", -1); } }
        protected int memberType { get { return RequestHelper.GetQueryInt("mt", -1); } }
        protected string keyword { get { return string.IsNullOrEmpty(RequestHelper.GetFormString("txt_date_show", false)) ? RequestHelper.GetQueryString("keyword", false) : RequestHelper.GetFormString("txt_date_show", false); } }
        protected int sex { get { return RequestHelper.GetQueryInt("sex", -1); } }
        protected string nn { get { return RequestHelper.GetQueryString("nn", true); } }
        #endregion

        protected string selectedString = "";//选中的筛选条件拼接
        protected int pagesize = 10;//页面容量
        protected string tagJson = "";//标签的JSON
        protected string groupJson = "";//分组的JSON



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckUserAuthority("wechat_fans_user_list_hq");
                CheckParam();
                InitData();
            }

        }
        protected string GetWxName(string wxID)
        {
            var wechat =BLL.Chats.wx_wechats.GetWeChatByID(Utils.StrToInt(wxID,0));
            string wxName="";
            if(null!=wechat)
            {
                wxName = wechat.wx_pb_name;
            }
            return wxName;
        }

        private void InitData()
        {
            var groupList = wx_group_tags.GetListByChannelId((int)GroupTagType.分组, wx_id, -1);
            groupJson += "{id:'0',name:'默认分组'},";
            foreach (var x in groupList)
            {
                if (x.status != (int)Status.禁用)
                    groupJson += string.Format("{{id:'{0}',name:'{1}'}},", x.id, x.title);
            }
            if (tagJson.Length > 1)
                groupJson = groupJson.Substring(0, tagJson.Length - 1);



            var tagList = wx_group_tags.GetListByChannelId((int)GroupTagType.标签, wx_id, -1);
            foreach (var x in tagList)
            {
                if (x.status != (int)Status.禁用)
                    tagJson += string.Format("{{id:'{0}',name:'{1}'}},", x.id, x.title);
            }
            if (tagJson.Length > 1)
                tagJson = tagJson.Substring(0, tagJson.Length - 1);

            var weChatList = BLL.Chats.wx_wechats.GetList();
            repWeChat.DataSource = weChatList;
            repWeChat.DataBind();


            Expression<Func<t_wx_fans, bool>> where = (x => x.status == (int)Status.正常 );
            if (msgContain != -1)
            {
                switch ((MsgContainType)msgContain)
                {
                    case MsgContainType.姓名:
                        where = where.And(x => x.real_name != null && x.real_name != "");
                        break;
                    case MsgContainType.手机:
                        where = where.And(x => x.mobile != null && x.mobile != "");
                        break;
                    case MsgContainType.身份证:
                        where = where.And(x => x.id_card != null && x.id_card != "");
                        break;
                }
            }
            if (wxID != -1)
            {
                where = where.And(x => x.wx_id == wxID);
            }
            if (sex != -1)
            {
                where = where.And(x => x.sex == sex);
            }
            if (memberType != -1)
            {
                if (memberType == (int)MemberType.注册用户)
                    where = where.And(x => x.unionid != null && x.unionid != "");
                else
                    where = where.And(x => x.unionid == null || x.unionid == "");
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                var startTime = Utils.StrToDateTime(txtbegin_date.Text.ToString(), DateTime.Now);
                var eneTime = Utils.StrToDateTime(txtend_date.Text.ToString(), DateTime.Now);
                eneTime = eneTime.AddDays(1);
                where = where.And(x => x.subscribe_time > startTime && x.subscribe_time < eneTime);
            }
            if (!string.IsNullOrEmpty(nn))
            {
                where = where.And(x => x.nick_name.Contains(nn));
            }
            int totalCount;
            var list = wx_fans.GetFansListByWxID(where, pagesize, page, out totalCount);
            string pageUrl = string.Format("all_user_list.aspx?page=__id__&m_id=53&key={0}&wc={1}&mc={2}&mt={3}&sex={4}&nn={5}", keyword, wxID, msgContain, memberType,sex,nn);
            div_page.InnerHtml = Utils.OutPageList(pagesize, page, totalCount, pageUrl, 8);
            Repeater1.DataSource = list;
            Repeater1.DataBind();
        }

        private void CheckParam()
        {
            string chooseFormat = "{0}";
            bool showChoose = false;
            string itemString = "";
            //if (msgContain != -1)
            //{
            //    itemString += "<a href=\"all_user_list.aspx?nn="+nn+"&sex="+sex+"&mt=" + memberType + "&key=" + keyword + "&m_id=53&wc=" + wxID + "&mc=-1\" class=\"btn cancelBubble\" title=\"点击取消\">信息包含：" + ((MsgContainType)msgContain).ToString() + "</a>";
            //    dlMsgContain.Attributes.Add("class", "selected");
            //    showChoose = true;
            //}
            if (wxID != -1)
            {
                itemString += "<a href=\"all_user_list.aspx?nn="+nn+"&sex="+sex+"&mt=" + memberType + "&key=" + keyword + "&m_id=53&mc=" + msgContain + "&wc=-1\" class=\"btn cancelBubble\" title=\"点击取消\">微信号：" + GetWxName(wxID.ToString()) + "</a>";
                dlWechat.Attributes.Add("class", "selected");
                showChoose = true;
            }
            if (sex != -1)
            {
                itemString += "<a href=\"all_user_list.aspx?nn=" + nn + "&sex=-1&mt=" + memberType + "&key=" + keyword + "&m_id=53&mc=" + msgContain + "&wc=" + wxID + "\" class=\"btn cancelBubble\" title=\"点击取消\">性别：" + ((WeChatSex)sex).ToString() + "</a>";
                dlSex.Attributes.Add("class", "selected");
                showChoose = true;
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                itemString += "<a href=\"all_user_list.aspx?nn=" + nn + "&sex=" + sex + "&mt=" + memberType + "&key=-1&m_id=53&mc=" + msgContain + "&wc=" + wxID + "\" class=\"btn cancelBubble\" title=\"点击取消\">关注时间：" + keyword + "</a>";
                dlHuDong.Attributes.Add("class", "selected");
                showChoose = true;
            }
            if (!string.IsNullOrEmpty(nn))
            {
                itemString += "<a href=\"all_user_list.aspx?nn=&sex=" + sex + "&mt=" + memberType + "&key="+keyword+"&m_id=53&mc=" + msgContain + "&wc=" + wxID + "\" class=\"btn cancelBubble\" title=\"点击取消\">昵称：" + nn + "</a>";
                dlFansName.Attributes.Add("class", "selected");
                showChoose = true;
            }
            //if (memberType != -1)
            //{
            //    itemString += "<a href=\"all_user_list.aspx?nn="+nn+"&sex=" + sex + "&mt=-1&key=" + keyword + "&m_id=53&mc=" + msgContain + "&wc=" + wxID + "\" class=\"btn cancelBubble\" title=\"点击取消\">会员属性：" + ((MemberType)memberType).ToString() + "</a>";
            //    dlMemberType.Attributes.Add("class", "selected");
            //    showChoose = true;
            //}
            if (showChoose)
                chooseFormat = "<dl class=\"selectedList\"><dt>已选择：</dt><dd class=\"btns\"> <a href=\"all_user_list.aspx?m_id="+m_id+"\" class=\"btn filterCancel\">全部撤销</a></dd><dd>{0}</dd></dl>";
            selectedString = string.Format(chooseFormat, itemString);
        }

        protected void Repeater1_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            switch (e.Item.ItemType)
            {
                case ListItemType.Item:
                case ListItemType.AlternatingItem:
                    Repeater rep2 = (Repeater)e.Item.FindControl("tag_repeater");
                    var rowv = (BLL.Entity.user_list_model)e.Item.DataItem;//找到分类Repeater关联的数据项 
                    int fanID = rowv.id;
                    rep2.DataSource = wx_tags_relation.GetRelationViewByFanID(fanID);
                    rep2.DataBind();
                    break;
            }
        }

        protected string getGroupName(string id)
        {
            int groupId = Utils.StrToInt(id, 0);
            var group = wx_group_tags.GetModel(groupId);
            if (group != null)
                return group.title;
            return null;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            CheckParam();
            InitData();
        }

    }
}