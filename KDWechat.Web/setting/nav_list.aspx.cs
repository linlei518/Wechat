using KDWechat.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.setting
{
    public partial class nav_list : KDWechat.Web.UI.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (u_type < 4)
            {
                Response.Redirect("/error.aspx?m_id=88");
            }

            if (!Page.IsPostBack)
            {
               // ChkAdminLevel("app_navigation_list", DTEnums.ActionEnum.View.ToString()); //检查权限
                RptBind();
            }
        }

        public string GetMenuType(object obj)
        { 
            string str="";
            if (obj.ToString()=="1")
            {
                str="地区菜单";
            }
            else if (obj.ToString()=="0")
            {
                str = "总部菜单";
            }
            else if (obj.ToString() == "2")
            {
                str = "地区和总部公用菜单";
            }
            return str;
        }

        //数据绑定
        private void RptBind()
        {
            BLL.Users.sys_navigation bll = new BLL.Users.sys_navigation();
            DataTable dt = bll.GetList(0,0);
            this.repList.DataSource = dt;
            this.repList.DataBind();
        }

        //美化列表
        protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                Literal LitFirst = (Literal)e.Item.FindControl("LitFirst");
                HiddenField hidLayer = (HiddenField)e.Item.FindControl("hidLayer");
                string LitStyle = "<span style=\"display:inline-block;width:{0}px;\"></span>{1}{2}";
                string LitImg1 = "<span class=\"folder-open\"></span>";
                string LitImg2 = "<span class=\"folder-line\"></span>";

                int classLayer = KDWechat.Common.Utils.StrToInt(hidLayer.Value,0);
                if (classLayer == 0)
                {
                    LitFirst.Text = LitImg1;
                }
                else
                {
                    LitFirst.Text = string.Format(LitStyle, (classLayer - 1) * 24, LitImg2, LitImg1);
                }
            }
        }

        //保存排序
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //ChkAdminLevel("app_navigation_list", DTEnums.ActionEnum.Edit.ToString()); //检查权限
            string log_title = "保存系统菜单排序";
            for (int i = 0; i < repList.Items.Count; i++)
            {
                int id = Convert.ToInt32(((HiddenField)repList.Items[i].FindControl("hidId")).Value);
                int sortId;
                if (!int.TryParse(((TextBox)repList.Items[i].FindControl("txtSortId")).Text.Trim(), out sortId))
                {
                    sortId = 99;
                }
                BLL.Users.sys_navigation.UpdateSort(id, sortId);
                //bll.UpdateField(id, "sort_id=" + sortId.ToString());
            }
            AddLog(log_title,LogType.修改);
            JsHelper.AlertAndRedirect("保存排序成功", "nav_list.aspx?m_id="+m_id);
           
        }
        //删除导航
        protected void repList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string[] estr = e.CommandArgument.ToString().Split(new char[]{','});
            int id = int.Parse(estr[0]);
            string title = estr[1];

            string log_title = "删除系统菜单:"+title;
            if (e.CommandName == "del")
            {
                
                if (BLL.Users.sys_navigation.DeleteNavigationByID(id))      //删除该导航
                {
                    BLL.Users.sys_navigation.DeleteNavigationByParentID(id);//删除子导航
                    AddLog(log_title,LogType.删除);
                    JsHelper.AlertAndRedirect("系统菜单删除成功", "nav_list.aspx?m_id=" + m_id);
                    
                }
               
            }
            
        }
    }
}