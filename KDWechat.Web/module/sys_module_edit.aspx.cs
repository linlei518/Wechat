using KDWechat.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.DAL;
using KDWechat.BLL.Chats;

namespace KDWechat.Web.module
{
    public partial class sys_module_edit : Web.UI.BasePage
    {
        protected int id { get { return RequestHelper.GetQueryInt("id", -1); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckUserAuthority("sys_module");
                InitWeChatList();
                if (id != -1)
                    InitData();
                else
                    radStatusOk.Checked = true;
            }
        }

        private void InitWeChatList()
        {
            var wechatList = wx_wechats.GetList();

            chbAllowWechats.DataSource = wechatList;
            chbAllowWechats.DataTextField = "wx_pb_name";
            chbAllowWechats.DataValueField = "id";

            chbAllowWechats.DataBind();
        }

        private void InitData()
        {
            t_modules model = modules.GetModel(id);

            string[] wechatIds = model.allow_wechats.Split(',');

            foreach (ListItem x in chbAllowWechats.Items)
            {
                if (wechatIds.Contains(x.Value))
                {
                    x.Selected = true;
                }
            }

            txtCallName.Text = model.call_name;
            txtCheckUrl.Text = model.check_url;
            txtDescription.Text = model.description;
            txtManageUrl.Text = model.manage_url;
            txtModuleName.Text = model.title;
            txtImgUrl.Text = model.img_url;
            img_show.Src = model.img_url;

            ddlModuleType.SelectedValue = model.type.ToString();

            if (model.status == (int)Status.禁用)
                radStatusFalse.Checked = true;
            else
                radStatusOk.Checked = true;
        }

        protected void SubmitButtom_Click(object sender, EventArgs e)
        {
            string wechatIDs = "0,";
            foreach (ListItem item in chbAllowWechats.Items)
            {
                if (item.Selected)
                    wechatIDs += item.Value + ",";
            }
            wechatIDs = wechatIDs.TrimEnd(',');
            
            
            if (id == -1)
            {
                t_modules module = new t_modules()
                {
                    call_name = txtCallName.Text,
                    check_url = txtCheckUrl.Text,
                    description = txtDescription.Text,
                    manage_url = txtManageUrl.Text,
                    title = txtModuleName.Text,
                    type = Utils.StrToInt(ddlModuleType.SelectedValue, 3),
                    img_url = txtImgUrl.Text,
                    allow_wechats = wechatIDs,
                    u_id = u_id,
                    create_time = DateTime.Now,
                    status = radStatusOk.Checked ? (int)Status.正常 : (int)Status.禁用,
                    is_sys = 1
                };
                if (modules.Add(module).id != 0)
                    JsHelper.AlertAndRedirect("模块添加成功", "sys_module_list.aspx?m_id=" + Request["m_id"]);
                else
                    JsHelper.Alert("模块添加失败，请重试");
            }
            else
            {
                t_modules module = modules.GetModel(id);
                module.call_name = txtCallName.Text;
                module.check_url = txtCheckUrl.Text;
                module.description = txtDescription.Text;
                module.manage_url = txtManageUrl.Text;
                module.title = txtModuleName.Text;
                module.type = Utils.StrToInt(ddlModuleType.SelectedValue, 3);
                module.img_url = txtImgUrl.Text;
                module.allow_wechats = wechatIDs;
                module.u_id = u_id;
                module.status = radStatusOk.Checked ? (int)Status.正常 : (int)Status.禁用;
                modules.Update(module);
                JsHelper.AlertAndRedirect("模块修改成功", "sys_module_list.aspx?m_id=" + Request["m_id"]);
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("sys_module_list.aspx?m_id="+Request["m_id"]);
        }
    }
}