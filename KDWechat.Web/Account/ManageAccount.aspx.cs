using System.CodeDom;
using KDWechat.BLL.Users;
using KDWechat.Common;
using KDWechat.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.DAL;

namespace KDWechat.Web.Account
{
    public partial class ManageAccount : BasePage
    {
        protected int userFlag { get { return RequestHelper.GetQueryInt("uf", -1); } }
        protected int bussinessType { get { return RequestHelper.GetQueryInt("bt", -1); } }
        protected int areaType { get { return RequestHelper.GetQueryInt("at", -1); } }
        protected int status { get { return RequestHelper.GetQueryInt("st", -1); } }
        protected int pagesize { get { return 10; } }
        protected string selectedString="";



        protected void Page_Load(object sender, EventArgs e)
        {
            CheckUserAuthority("sys_account");            
            if (!IsPostBack)
            {
                SetRefferUrl();
                CheckParam();
                InitData();
            }
        }

        private void CheckParam()
        {
            string chooseFormat = "{0}";
            bool showChoose = false;               
            string itemString = "";
            if (userFlag != -1)
            {
                itemString += "<a href=\"ManageAccount.aspx?uf=-1&m_id="+m_id+"&bt="+bussinessType.ToString()+"&st="+status.ToString()+"&at="+areaType.ToString()+"\" class=\"btn cancelBubble\" title=\"点击取消\">账号类型：" + ((UserFlag)userFlag).ToString() + "</a>";
                dlUserFlag.Attributes.Add("class", "selected");
                showChoose = true;
            }
            if (bussinessType != -1)
            {
                itemString += "<a href=\"ManageAccount.aspx?uf=" + userFlag.ToString() + "&bt=-1&m_id=" + m_id + "&st=" + status.ToString() + "&at=" + areaType.ToString() + "\" class=\"btn cancelBubble\" title=\"点击取消\">业务类型：" + ((BussinessType)bussinessType).ToString() + "</a>";
                dlBussinessType.Attributes.Add("class", "selected");
                showChoose = true;
            }
            if (status != -1)
            {
                itemString += "<a href=\"ManageAccount.aspx?uf=" + userFlag.ToString() + "&m_id=" + m_id + "&bt=" + bussinessType.ToString() + "&st=-1&at=" + areaType.ToString() + "\" class=\"btn cancelBubble\" title=\"点击取消\">状态：" + ((Status)status).ToString() + "</a>";
                dlStatus.Attributes.Add("class", "selected");
                showChoose = true;
            }
            if (areaType != -1)
            {
                itemString += "<a href=\"ManageAccount.aspx?uf=" + userFlag.ToString() + "&m_id=" + m_id + "&bt=" + bussinessType.ToString() + "&st=" + status.ToString() + "&at=-1\" class=\"btn cancelBubble\" title=\"点击取消\">地区：" + ((AreaType)areaType).ToString() + "</a>";
                dlAreaType.Attributes.Add("class", "selected");
                showChoose = true;
            }
            if (showChoose)
                chooseFormat = "<dl class=\"selectedList\"><dt>已选择：</dt><dd class=\"btns\"> <a href=\"manageAccount.aspx?m_id="+m_id+"\" class=\"btn filterCancel\">全部撤销</a></dd><dd>{0}</dd></dl>";
            selectedString = string.Format(chooseFormat, itemString);
        }

        private void SetRefferUrl()
        {
            try
            {
                hfReturlUrl.Value = Request.UrlReferrer.ToString();
            }
            catch (Exception)
            {
                hfReturlUrl.Value = "ManageAccount.aspx?m_id=51";
            }
        }

        //通过uid获取管理微信号的数量
        protected string GetWxCountByUid(string uid)
        {
            int childid = Utils.StrToInt(uid, 0);
            if (childid != 0)
            {
                int cou = sys_users.GetWxCountByUid(childid);
                return cou==0?null:cou.ToString();
            }
            return null;
        }


        private void InitData()
        {

            Expression<Func<DAL.sysUser_WeChat_View, bool>> where = (x => x.parent_id==0 && x.flag!=4); 
            //提供一个永远为true的表达式，为方便后面拼接

            if (userFlag != -1)
                where = where.And(x => x.flag == userFlag);
                //这里进行条件的拼接
            if (bussinessType != -1)
                where = where.And(x => x.type_id == bussinessType);
            if (areaType != -1)
                where = where.And(x => x.area == areaType);
            if (status != -1)
                where = where.And(x => x.status == status);

            int totalCount;
            var lis2 = BLL.Users.sys_users.GetUserWeChatCountList(pagesize, page, where,out totalCount);
            //将select条件作为参数进行传递

            string pageUrl = string.Format("ManageAccount.aspx?m_id={4}&st={0}&at={1}&bt={2}&uf={3}&page=__id__", status, areaType, bussinessType, userFlag,m_id);

            div_page.InnerHtml = Utils.OutPageList(pagesize, page, totalCount, pageUrl, 8);

            DataRepeater.DataSource = lis2;
            DataRepeater.DataBind();
        }

        protected void DataRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Disable")
            {

                int id = KDWechat.Common.Utils.StrToInt(e.CommandArgument.ToString(), 0);
                Status status = Status.禁用;
                if (((Button)e.CommandSource).Text == "启用")
                {
                    status = Status.正常;
                }
                BLL.Users.sys_users.SetUserStatus(id, status);
                AddLog(string.Format("{0}公众账号:{1}", status == Status.正常 ? "启用" : "停用", id.ToString()),LogType.修改);
                Response.Redirect(Request.Url.ToString());
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < DataRepeater.Items.Count; i++)
            {
                int id = Utils.StrToInt(((HiddenField)DataRepeater.Items[i].FindControl("hidId")).Value,0);
                CheckBox cb = (CheckBox)DataRepeater.Items[i].FindControl("chkId");
                if (cb.Checked)
                {
                    BLL.Users.sys_users.SetUserStatus(id, Status.禁用);
                }
            }
            Response.Redirect(Request.Url.ToString());
        }

        protected string GetLastOpreation(object uid)
        {
            string outPut = string.Empty;
            int u_id = Utils.StrToInt(uid.ToString(),0);
            if(u_id!=0)
            {
                var log = BLL.Logs.wx_logs.GetModel<int>(x=>x.u_id==u_id,x=>x.id,true);
                if (log != null)
                {
                    outPut ="<a href='javascript:bombbox.openBox(\"../module/show_description.aspx?id="+log.id+"\");'>"+log.create_time+"</a>";
                }
            }
            return outPut;
        }
    }
}