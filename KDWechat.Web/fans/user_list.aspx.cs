using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.BLL.Users;
using KDWechat.Common;
using System.Linq.Expressions;
using Newtonsoft.Json;
using LinqKit;

namespace KDWechat.Web.fans
{
    public partial class user_list : Web.UI.BasePage
    {
        #region 筛选条件
        protected int gpID{get{return RequestHelper.GetQueryInt("gp",-1);}}
        protected int tagID{get{return RequestHelper.GetQueryInt("tag",-1);}}
        protected int msgContain{get{return RequestHelper.GetQueryInt("mc",-1);}}
        protected int memberType { get { return RequestHelper.GetQueryInt("mt", -1); } }
        protected string keyword { get { return RequestHelper.GetQueryString("key",false); } }
        //protected string keyword { get { return string.IsNullOrEmpty(RequestHelper.GetFormString("txt_date_show", false)) ? RequestHelper.GetQueryString("keyword", false) : RequestHelper.GetFormString("txt_date_show", false); } }
        protected string nn { get { return RequestHelper.GetQueryString("nn",false); } }
        protected int sex { get { return RequestHelper.GetQueryInt("sex", -1); } }
        protected int state { get { return RequestHelper.GetQueryInt("state",-1); } }
        protected int publicTag { get { return RequestHelper.GetQueryInt("ptag", -1); } }

        protected int fansFrom { get { return RequestHelper.GetQueryInt("ff", -1); } }

        #endregion

        protected string selectedString = "";//选中的筛选条件拼接
        protected int pagesize = 10;//页面容量
        protected string tagJson = "";//标签的JSON
        protected string groupJson = "";//分组的JSON
        protected List<KDWechat.DAL.t_wx_group_tags> tagList = null;
        protected List<KDWechat.DAL.t_wx_group_tags> groupList = null;
        protected List<KDWechat.DAL.t_wx_group_tags> pubTagList = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckUserAuthority("wechat_fans_user_manager");
            if (!IsPostBack)
            {
                if (Request.HttpMethod == "POST")
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["a"]))//异步更新标签
                        AjaxChangeTag();
                    else if (!string.IsNullOrEmpty(Request.QueryString["b"]))//异步更新分组
                        AjaxChangeGroup();
                    else if (!string.IsNullOrEmpty(RequestHelper.GetQueryString("sss")))//同步用户信息
                    {
                        var tuple = wx_fans.UpdateAllFans(wx_id);
                        Response.Write("1|"+tuple.Item1+"|"+tuple.Item2);
                        Response.End();
                    }
                    else if (!string.IsNullOrEmpty(RequestHelper.GetQueryString("as")))//高级筛选
                        AdvanceFilter();
                }
                else
                {
                    //if (!string.IsNullOrEmpty(Request.QueryString["ad"]))
                    //    ExportExcel();
                }
                CheckParam();
                InitData();
            }

        }

        //高级筛选
        private void AdvanceFilter()
        {
            //if (CheckUserAuthorityBool("wechat_fans_user_manager", RoleActionType.Export))
            //{
            var tup = wx_fans.GetAdvanceQuery(Request.Form["data"], wx_id, u_id);
            Response.Write("0|user_filter_list.aspx?m_id=18");
            //}
            //else
            //{
            //    Response.Write("0|您没有高级筛选及导出列表的权限，请联系管理员");
            //}
            Response.End();
        }

        #region 导出
        private void ExportExcel()
        {
            
            Expression<Func<KDWechat.DAL.t_wx_fans, bool>> where = (x => x.status == (int)Status.正常 && x.wx_id == wx_id);
            if (gpID != -1)
            {
                where = where.And(x => x.group_id == gpID);
            }
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
            if (sex != -1)
            {
                where = where.And(x => x.sex == sex);
            }
            if (tagID != -1)
            {
                int[] tagsList = wx_fans_tags.GetFansIDListByGroupID(wx_id, tagID);
                where = where.And(x => tagsList.Contains(x.id));
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
                var timeList = keyword.Split('—');
                if (timeList.Length == 1)
                {
                    var startTime = Utils.StrToDateTime(timeList[0], DateTime.Now);
                    where = where.And(x => x.subscribe_time > startTime);
                }
                else if (timeList.Length == 2)
                {
                    var startTime = Utils.StrToDateTime(timeList[0], DateTime.Now);
                    var eneTime = Utils.StrToDateTime(timeList[1], DateTime.Now);
                    eneTime = eneTime.AddDays(1);
                    where = where.And(x => x.subscribe_time > startTime && x.subscribe_time < eneTime);
                }
            }
            if (!string.IsNullOrEmpty(nn))
            {
                string temp = nn.ToString();
                where = where.And(x => x.nick_name.Contains(temp));
            }
            if (state != -1)
            {
                DateTime start = DateTime.Now.AddDays(-2);
                switch ((FansChatsTypeNew)state)
                {
                    case FansChatsTypeNew.暂无:
                        where = where.And(x => x.reply_state == (int)FansReplyState.暂无);
                        break;
                    case FansChatsTypeNew.未回复:
                        where = where.And(x => x.reply_state == (int)FansReplyState.未回复 && x.last_interact_time > start);
                        break;
                    case FansChatsTypeNew.已回复:
                        where = where.And(x => x.reply_state == (int)FansReplyState.已回复 && x.last_interact_time > start);
                        break;
                    case FansChatsTypeNew.已过期:
                        where = where.And(x => x.reply_state == (int)FansReplyState.已回复 && x.last_interact_time < start);
                        break;
                    case FansChatsTypeNew.未回复已过期:
                        where = where.And(x => x.reply_state == (int)FansReplyState.未回复 && x.last_interact_time < start);
                        break;
                }
            }
            if (fansFrom != -1)
            {
                where = where.And(x => x.source_id == fansFrom);
            }
            var list = wx_fans.GetAllFansList(where);
            var table = EnumableHelper.ToDataTable<t_wx_fans_export>(list);
            GemBoxExcelLiteHelper.SaveExcel(Server.MapPath("~/upload/" + wx_og_id + ".xls"), this, true, true, new List<string>() { "openid", "昵称", "国家", "省份", "城市", "语言", "性别 0-未知，1-男，2-女" }, table);//.DataTable1Excel(table);
        }
        #endregion
        #region ajax回发接收
        private void AjaxChangeGroup()
        {
            string[] uids =  Request["uids"].Replace("user","").Split(',');
            int groupID = Utils.StrToInt(Request["groupID"], -1);
            if (groupID != -1)
            {
                if (wx_fans.UpdateFansGroupList(uids, groupID))
                {
                    Response.Write("1|操作成功");
                    AddLog(string.Format("批量操作了用户分组"), LogType.修改);
                }
                else
                    Response.Write("0|操作失败");
            }
            else
                Response.Write("0|参数获取失败");
            Response.End();

        }

        private void AjaxChangeTag()
        {
            string data = Request["data"].Replace("user", "");
            var list = JsonConvert.DeserializeObject<List<fans_tag_list>>(data);
            if (list.Count > 0)
            {
                if (wx_fans_tags.ChangeTags(list))
                {
                    Response.Write("1|操作成功");
                    AddLog(string.Format("批量操作了用户标签"), LogType.修改);
                }
                else
                {
                    Response.Write("0|操作失败");
                }
            }
            else
                Response.Write("0|操作失败");
            Response.End();
        }
        #endregion

        private void InitData()
        {

            Expression<Func<KDWechat.DAL.t_wx_fans, bool>> where = (x => x.status == (int)Status.正常 && x.wx_id==wx_id);
            

            if (gpID != -1)
            {
                where = where.And(x => x.group_id == gpID);
            }
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
            if (sex != -1)
            {
                where = where.And(x => x.sex == sex);
            }
            if (memberType != -1)
            {
                if(memberType==(int)MemberType.注册用户)
                    where = where.And(x => x.unionid != null && x.unionid != "");
                else
                    where = where.And(x => x.unionid == null || x.unionid == "");
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                var timeList = keyword.Split('—');
                if (timeList.Length == 1)
                {
                    var startTime = Utils.StrToDateTime(timeList[0], DateTime.Now);
                    where = where.And(x => x.subscribe_time > startTime);
                }
                else if(timeList.Length==2)
                {
                    var startTime = Utils.StrToDateTime(timeList[0], DateTime.Now);
                    var eneTime = Utils.StrToDateTime(timeList[1], DateTime.Now);
                    eneTime = eneTime.AddDays(1);
                    where = where.And(x => x.subscribe_time > startTime && x.subscribe_time < eneTime);
                }
            }
            if (state != -1)
            {
                DateTime start = DateTime.Now.AddDays(-2);
                switch ((FansChatsTypeNew)state)
                {
                    case FansChatsTypeNew.暂无:
                        where = where.And(x => x.reply_state == (int)FansReplyState.暂无);
                        break;
                    case FansChatsTypeNew.未回复:
                        where = where.And(x => x.reply_state == (int)FansReplyState.未回复 && x.last_interact_time > start);
                        break;
                    case FansChatsTypeNew.已回复:
                        where = where.And(x => x.reply_state == (int)FansReplyState.已回复 && x.last_interact_time > start);
                        break;
                    case FansChatsTypeNew.已过期:
                        where = where.And(x => x.reply_state == (int)FansReplyState.已回复 && x.last_interact_time < start);
                        break;
                    case FansChatsTypeNew.未回复已过期:
                        where = where.And(x => x.reply_state == (int)FansReplyState.未回复 && x.last_interact_time < start);
                        break;
                }
            }
            if (tagID != -1)
            {
                var tagsList = wx_fans_tags.GetFansIDListByGroupID_List(wx_id, tagID);
                //if (publicTag != -1)
                //{
                //    var pubTagList = wx_fans_tags.GetFansIDListByGroupID_List(wx_id, publicTag);
                //    tagsList.AddRange(pubTagList);
                //}
                where = where.And(x => tagsList.Contains(x.id));
            }
            if (publicTag != -1)
            {
                var tagsList = wx_fans_tags.GetFansIDListByGroupID_List(wx_id, publicTag);
                //if (tagID != -1)
                //{
                //    var pubTagList = wx_fans_tags.GetFansIDListByGroupID_List(wx_id, tagID);
                //    tagsList.AddRange(pubTagList);
                //}
                where = where.And(x => tagsList.Contains(x.id));
            }
            if (!string.IsNullOrEmpty(nn))
            {
                if (nn.Contains("|"))
                {
                    Expression<Func<KDWechat.DAL.t_wx_fans, bool>> nickWhere = x => false;
                    var nick_name_array = nn.Split('|');
                    foreach (var nick in nick_name_array)
                    {
                        nickWhere = nickWhere.Or(x => x.nick_name.Contains(nick));
                    }
                    where = where.And(nickWhere.Expand());
                }
                else
                    where = where.And(x => x.nick_name.Contains(nn.ToString()));
                
            }
            if (fansFrom != -1)
            {
                where = where.And(x => x.source_id == fansFrom);
            }

            var list = wx_fans.GetFansListByWxID(where, pagesize, page,out totalCount);
            string pageUrl = string.Format("user_list.aspx?page=__id__&m_id=18&key={0}&tag={1}&gp={2}&mc={3}&mt={4}&nn={5}&sex={6}&ff={7}", keyword, tagID, gpID, msgContain,memberType,nn,sex,fansFrom);
            div_page.InnerHtml = Utils.OutPageList(pagesize, page, totalCount, pageUrl, 8);
            Repeater1.DataSource = list;
            Repeater1.DataBind();
        }

        private void CheckParam()
        {
            groupList = wx_group_tags.GetListByChannelId((int)GroupTagType.分组, wx_id, -1);
            groupList.Add(new KDWechat.DAL.t_wx_group_tags() { id = 0, status = 1, title = "默认分组" });
            repGroup.DataSource = groupList;
            repGroup.DataBind();
            foreach (var x in groupList)
            {
                if (x.status != (int)Status.禁用)
                    groupJson += string.Format("{{id:'{0}',name:'{1}'}},", x.id, x.title);
            }
            if (tagJson.Length > 1)
                groupJson = groupJson.Substring(0, tagJson.Length - 1);



            tagList = wx_group_tags.GetListByChannelId((int)GroupTagType.标签, wx_id, -1);
            repTag.DataSource = tagList;
            repTag.DataBind();

            pubTagList = wx_group_tags.GetListByChannelId((int)GroupTagType.标签, 0, -1);
            repPublicTag.DataSource = pubTagList;
            repPublicTag.DataBind();

            var qrCodeList = Companycn.Core.EntityFramework.EFHelper.GetList<DAL.creater_wxEntities, DAL.t_wx_qrcode, int>(x => x.wx_id == wx_id, x => x.id, int.MaxValue, 1, true);
            repFansFrom.DataSource = qrCodeList;
            repFansFrom.DataBind();

            foreach (var x in pubTagList)
            {
                if (x.status != (int)Status.禁用)
                    tagJson += string.Format("{{id:'{0}',name:'{1}',global:true}},", x.id, x.title);
            }
            foreach (var x in tagList)
            {
                if (x.status != (int)Status.禁用)
                    tagJson += string.Format("{{id:'{0}',name:'{1}'}},", x.id, x.title);
            }

            if (tagJson.Length > 1)
                tagJson = tagJson.Substring(0, tagJson.Length - 1);


            string chooseFormat = "{0}";
            bool showChoose = false;
            string itemString = "";
            if (tagID != -1)
            {
                var tag = tagList.Where(x=>x.id==tagID).FirstOrDefault();
                if (tag != null)
                {
                    itemString += "<a href=\"user_list.aspx?ff=" + fansFrom + "&ptag=" + publicTag + "&state=" + state + "&sex=" + sex + "&nn=" + nn + "&mt=" + memberType + "&key=" + keyword + "&m_id=18&tag=-1&gp=" + gpID.ToString() + "&mc=" + msgContain.ToString() + "\" class=\"btn cancelBubble\" title=\"点击取消\">标签：" + tag.title + "</a>";
                    dlTag.Attributes.Add("class", "selected");
                    showChoose = true;
                }
            }
            if (gpID != -1)
            {
                var group  = groupList.Where(x => x.id == gpID).FirstOrDefault();
                if (group != null)
                {
                    itemString += "<a href=\"user_list.aspx?ff=" + fansFrom + "&ptag=" + publicTag + "&state=" + state + "&sex=" + sex + "&nn=" + nn + "&mt=" + memberType + "&key=" + keyword + "&m_id=18&tag=" + tagID + "&gp=-1&mc=" + msgContain.ToString() + "\" class=\"btn cancelBubble\" title=\"点击取消\">分组：" + group.title + "</a>";
                    dlGroup.Attributes.Add("class", "selected");
                    showChoose = true;
                }
            }
            if (msgContain != -1)
            {
                itemString += "<a href=\"user_list.aspx?ff=" + fansFrom + "&ptag=" + publicTag + "&state=" + state + "&sex=" + sex + "&nn=" + nn + "&mt=" + memberType + "&key=" + keyword + "&m_id=18&tag=" + tagID + "&gp=" + gpID.ToString() + "&mc=-1\" class=\"btn cancelBubble\" title=\"点击取消\">信息包含：" + ((MsgContainType)msgContain).ToString() + "</a>";
                dlMsgContain.Attributes.Add("class", "selected");
                showChoose = true;
            }
            if (sex != -1)
            {
                itemString += "<a href=\"user_list.aspx?ff=" + fansFrom + "&ptag=" + publicTag + "&state=" + state + "&sex=-1&nn=" + nn + "&mt=" + memberType + "&key=" + keyword + "&m_id=18&tag=" + tagID + "&gp=" + gpID.ToString() + "&mc=" + msgContain.ToString() + "\" class=\"btn cancelBubble\" title=\"点击取消\">性别：" + ((WeChatSex)sex).ToString() + "</a>";
                dlSex.Attributes.Add("class", "selected");
                showChoose = true;
            }
            if (keyword != "")
            {
                itemString += "<a href=\"user_list.aspx?ff=" + fansFrom + "&ptag=" + publicTag + "&state=" + state + "&sex=" + sex + "&nn=" + nn + "&mt=" + memberType + "&key=&m_id=18&tag=" + tagID + "&gp=" + gpID.ToString() + "&mc=" + msgContain.ToString() + "\" class=\"btn cancelBubble\" title=\"点击取消\">关注时间：" + keyword + "</a>";
                dlHuDong.Attributes.Add("class", "selected");
                txt_date_show.Value = keyword;
                showChoose = true;
            }
            if (memberType != -1)
            {
                itemString += "<a href=\"user_list.aspx?ff=" + fansFrom + "&ptag=" + publicTag + "&state=" + state + "&sex=" + sex + "&nn=" + nn + "&mt=-1&key=" + keyword + "&m_id=18&tag=" + tagID + "&gp=" + gpID.ToString() + "&mc=" + msgContain.ToString() + "\" class=\"btn cancelBubble\" title=\"点击取消\">会员属性：" + ((MemberType)memberType).ToString() + "</a>";
                dlMemberType.Attributes.Add("class", "selected");
                showChoose = true;
            }
            if (!string.IsNullOrEmpty(nn))
            {
                itemString += "<a href=\"user_list.aspx?ff=" + fansFrom + "&ptag=" + publicTag + "&state=" + state + "&sex=" + sex + "&nn=&mt=" + memberType + "&key=" + keyword + "&m_id=18&tag=" + tagID + "&gp=" + gpID.ToString() + "&mc=" + msgContain.ToString() + "\" class=\"btn cancelBubble\" title=\"点击取消\">会员名：" + nn + "</a>";
                dlFansName.Attributes.Add("class", "selected");
                showChoose = true;
            }
            if (state != -1)
            {
                itemString += "<a href=\"user_list.aspx?ff=" + fansFrom + "&ptag=" + publicTag + "&state=-1&sex=" + sex + "&nn=" + nn + "&mt=" + memberType + "&key=" + keyword + "&m_id=18&tag=" + tagID + "&gp=" + gpID.ToString() + "&mc=" + msgContain.ToString() + "\" class=\"btn cancelBubble\" title=\"点击取消\">回复状态：" + ((FansChatsTypeNew)state).ToString() + "</a>";
                dlState.Attributes.Add("class", "selected");
                showChoose = true;
            }
            if (publicTag != -1)
            {
                var pubTag = pubTagList.Where(x => x.id == publicTag).FirstOrDefault();
                if (pubTag != null)
                {
                    itemString += "<a href=\"user_list.aspx?ff="+fansFrom+"&ptag=-1&state=" + state + "&sex=" + sex + "&nn=" + nn + "&mt=" + memberType + "&key=" + keyword + "&m_id=18&tag=" + tagID + "&gp=" + gpID.ToString() + "&mc=" + msgContain.ToString() + "\" class=\"btn cancelBubble\" title=\"点击取消\">公用标签：" + pubTag.title + "</a>";
                    dlPTag.Attributes.Add("class", "selected");
                    showChoose = true;
                }
            }
            if (fansFrom != -1)
            {
                var qrcode = qrCodeList.Where(x => x.id == fansFrom).FirstOrDefault();

                if (qrcode != null||fansFrom==0)
                {
                    itemString += "<a href=\"user_list.aspx?ff=-1&ptag=" + publicTag + "&state=" + state + "&sex=" + sex + "&nn=" + nn + "&mt=" + memberType + "&key=" + keyword + "&m_id=18&tag=" + tagID + "&gp=" + gpID.ToString() + "&mc=" + msgContain.ToString() + "\" class=\"btn cancelBubble\" title=\"点击取消\">来源：" + (fansFrom==0?"搜索关注": qrcode.q_name) + "</a>";
                    dlFansFrom.Attributes.Add("class", "selected");
                    showChoose = true;
                }
            }
            if (showChoose)
                chooseFormat = "<dl class=\"selectedList\"><dt>已选择：</dt><dd class=\"btns\"> <a href=\"user_list.aspx?m_id="+m_id+"\" class=\"btn filterCancel\">全部撤销</a></dd><dd>{0}</dd></dl>";
            selectedString = string.Format(chooseFormat, itemString);
        }

        protected void Repeater1_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            switch (e.Item.ItemType)
            {
                case ListItemType.Item:
                case ListItemType.AlternatingItem:
                    Repeater rep2 = (Repeater)e.Item.FindControl("tag_repeater");
                    Label gpLabel = (Label)e.Item.FindControl("GroupLabel");
                    var rowv = (KDWechat.BLL.Entity.user_list_model)e.Item.DataItem;//找到分类Repeater关联的数据项 
                    gpLabel.Text = groupList.Where(x => x.id == rowv.group_id).First().title;
                    int fanID = rowv.id;
                    var list = wx_tags_relation.GetRelationViewByFanID(fanID);
                    rep2.DataSource = list;
                    rep2.DataBind();
                    if(list.Count==0)
                    {
                        System.Web.UI.HtmlControls.HtmlInputButton input = (System.Web.UI.HtmlControls.HtmlInputButton)e.Item.FindControl("input_add_Tag");
                        input.Value = "添加标签";
                    }
                    break;
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            CheckParam();
            InitData();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            CheckParam();
            InitData();
        }


    }




}