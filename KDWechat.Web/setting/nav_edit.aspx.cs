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
    public partial class nav_edit : KDWechat.Web.UI.BasePage
    {
        protected int id
        {
            get { return RequestHelper.GetQueryInt("id", 0); }
        }

        protected int channel_id
        {
            get { return RequestHelper.GetQueryInt("channel_id", 0); }
        }

        protected string action 
        {
            get { return RequestHelper.GetQueryString("action"); }
        }

        protected string oldnavname;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (u_type<4)
            {
                Response.Redirect("/error.aspx?m_id=88");
            }

            if (!Page.IsPostBack)
            {
                // ChkAdminLevel("app_navigation_list", DTEnums.ActionEnum.View.ToString()); //检查权限
                TreeBind("System"); //绑定导航菜单
                ActionTypeBind(); //绑定操作权限类型
                if (action == "Edit" && this.id > 0) //修改
                {
                    ShowInfo(this.id);
                }
                else 
                {
                    if (this.channel_id > 0 && this.id > 0)
                    {
                        ddlParentId.SelectedValue = this.id.ToString();
                    }
                }
            }
        }
        //改变菜单类型
        protected void ddlMenuType_SelectedChanged(object sender, EventArgs e) 
        {
            TreeBind("System");
        }
        #region 绑定导航菜单=============================
        private void TreeBind(string nav_type)
        {
            if(this.channel_id>0 && this.id>0)
            {
                ddlMenuType.SelectedValue = this.channel_id.ToString();
            }
            int ch_id = Common.Utils.StrToInt(ddlMenuType.SelectedValue,(int)UserFlag.子账号);

            BLL.Users.sys_navigation bll = new BLL.Users.sys_navigation();
            DataTable dt = bll.GetList(0,ch_id);
            this.ddlParentId.Items.Clear();
            this.ddlParentId.Items.Add(new ListItem("无父级导航", "0"));
            foreach (DataRow dr in dt.Rows)
            {
                string Id = dr["id"].ToString();
                int ClassLayer = int.Parse(dr["class_layer"].ToString());
                string Title = dr["title"].ToString().Trim();

                if (ClassLayer == 1)
                {
                    this.ddlParentId.Items.Add(new ListItem(Title, Id));
                }
                else
                {
                    Title = "├ " + Title;
                    Title = Utils.StringOfChar(ClassLayer - 1, "　") + Title;
                    this.ddlParentId.Items.Add(new ListItem(Title, Id));
                }
            }
        }
        #endregion

        #region 绑定操作权限类型=========================
        private void ActionTypeBind()
        {
            cblActionType.Items.Clear();
            foreach (KeyValuePair<string, string> kvp in Utils.ActionType())
            {
                cblActionType.Items.Add(new ListItem(kvp.Value + "(" + kvp.Key + ")", kvp.Key));
            }
        }
        #endregion

        #region 赋值操作=================================
        private void ShowInfo(int _id)
        {
           
            DAL.t_sys_navigation model = BLL.Users.sys_navigation.GetNavigationByID(_id);
            ddlParentId.SelectedValue = model.parent_id.ToString();
            txtSortId.Text = model.sort_id.ToString();
            txtName.Text = model.name;
            oldnavname = model.name;
            //txtName.Attributes.Add("ajaxurl", "do_ajax.ashx?action=navigation_validate&old_name=" + Utils.UrlEncode(model.name));
            //txtName.Focus(); //设置焦点，防止JS无法提交
            if (model.is_sys == 1)
            {
                ddlParentId.Enabled = false;
                txtName.ReadOnly = true;
            }
            txtSubTitle.Text=model.sub_title;
            txtTitle.Text = model.title;
            txtLinkUrl.Text = model.link_url;
            txtRemark.Text = model.remark;
            rbtnIssystem.SelectedIndex = rbtnIssystem.Items.IndexOf(rbtnIssystem.Items.FindByValue(model.is_sys.ToString()));
            rbtnIshide.SelectedIndex = rbtnIshide.Items.IndexOf(rbtnIshide.Items.FindByValue(model.is_lock.ToString()));
            //赋值操作权限类型
            string[] actionTypeArr = model.action_type.Split(',');
            for (int i = 0; i < cblActionType.Items.Count; i++)
            {
                for (int n = 0; n < actionTypeArr.Length; n++)
                {
                    if (actionTypeArr[n].ToLower() == cblActionType.Items[i].Value.ToLower())
                    {
                        cblActionType.Items[i].Selected = true;
                    }
                }
            }
            this.ddlMenuType.Enabled = false;

            ddlTargetType.SelectedValue=model.target_type;
            ddlType.SelectedValue=model.type_id.ToString();

        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {
            string log_title = "增加系统菜单:";
            try
            {
                DAL.t_sys_navigation model = new DAL.t_sys_navigation();

                model.nav_type = "System";
                model.name = txtName.Text.Trim();
                model.sub_title=txtSubTitle.Text;
                log_title += model.name;
                model.title = txtTitle.Text.Trim();
                model.link_url = txtLinkUrl.Text.Trim();
                model.sort_id = int.Parse(txtSortId.Text.Trim());
                model.channel_id = Common.Utils.StrToInt(ddlMenuType.SelectedValue,(int)UserFlag.子账号);

                model.type_id = Common.Utils.StrToInt(ddlType.SelectedValue, -1);
                model.target_type=ddlTargetType.SelectedValue;
                if (rbtnIssystem.SelectedValue == "1")
                {
                    model.is_sys = 1;
                }
                else
                {
                    model.is_sys = 0;
                }

                if (rbtnIshide.SelectedValue == "1")
                {
                    model.is_lock = 1;
                }
                else 
                {
                    model.is_lock = 0;
                }
                model.remark = txtRemark.Text.Trim();
                model.parent_id = int.Parse(ddlParentId.SelectedValue);

                //添加操作权限类型
                string action_type_str = string.Empty;
                for (int i = 0; i < cblActionType.Items.Count; i++)
                {
                    if (cblActionType.Items[i].Selected && Utils.ActionType().ContainsKey(cblActionType.Items[i].Value))
                    {
                        action_type_str += cblActionType.Items[i].Value + ",";
                    }
                }
                model.action_type = Utils.DelLastComma(action_type_str);

                if (BLL.Users.sys_navigation.InsertNavigation(model)!=null)
                {
                    AddLog(log_title,LogType.添加);
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(int _id)
        {
            string log_title = "修改系统菜单:";
            try
            {
                DAL.t_sys_navigation model = BLL.Users.sys_navigation.GetNavigationByID(_id);
              
                if (model.name != txtName.Text.Trim())
                {
                    log_title += "标题：" + model.name + "修改为：" + txtName.Text.Trim();
                }
                else 
                {
                    log_title += model.name;
                }
                model.name = txtName.Text.Trim();
                model.title = txtTitle.Text.Trim();
                model.sub_title = txtSubTitle.Text;
                model.link_url = txtLinkUrl.Text.Trim();
                model.sort_id = int.Parse(txtSortId.Text.Trim());
                model.type_id = Common.Utils.StrToInt(ddlType.SelectedValue, -1);
                model.target_type = ddlTargetType.SelectedValue;
                if (rbtnIssystem.SelectedValue == "1")
                {
                    model.is_sys = 1;
                }
                else
                {
                    model.is_sys = 0;
                }

                if (rbtnIshide.SelectedValue == "1")
                {
                    model.is_lock = 1;
                }
                else
                {
                    model.is_lock = 0;
                }
                
                model.remark = txtRemark.Text.Trim();
                if (model.is_sys == 0)
                {
                    int parentId = int.Parse(ddlParentId.SelectedValue);
                    //如果选择的父ID不是自己,则更改
                    if (parentId != model.id)
                    {
                        model.parent_id = parentId;
                    }
                }

                //添加操作权限类型
                string action_type_str = string.Empty;
                for (int i = 0; i < cblActionType.Items.Count; i++)
                {
                    if (cblActionType.Items[i].Selected && Utils.ActionType().ContainsKey(cblActionType.Items[i].Value))
                    {
                        action_type_str += cblActionType.Items[i].Value + ",";
                    }
                }
                model.action_type = Utils.DelLastComma(action_type_str);

                if (BLL.Users.sys_navigation.UpdateNavigation(model))
                {
                    AddLog(log_title,LogType.修改);
                   // AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "修改导航信息:" + model.title); //记录日志
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }
        #endregion


        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            if (action == "Edit") //修改
            {
                //ChkAdminLevel("app_navigation_list", DTEnums.ActionEnum.Edit.ToString()); //检查权限
                if (!DoEdit(this.id))
                {
                    JsHelper.AlertAndRedirect("保存过程中发生错误", "nav_list.aspx?m_id=" + m_id);
                    
                    return;
                }
                CacheHelper.RemoveAll();
                JsHelper.AlertAndRedirect("导航菜单修改成功", "nav_list.aspx?m_id=" + m_id);
               
            }
            else //添加
            {
               // ChkAdminLevel("app_navigation_list", DTEnums.ActionEnum.Add.ToString()); //检查权限
                if (!DoAdd())
                {
                    JsHelper.AlertAndRedirect("保存过程中发生错误", "nav_list.aspx?m_id=" + m_id);
                    
                    return;
                }
                CacheHelper.RemoveAll();
                JsHelper.AlertAndRedirect("导航菜单添加成功", "nav_list.aspx?m_id=" + m_id);
              
            }
        }
    }
}