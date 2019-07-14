using KDWechat.BLL.Chats;
using KDWechat.Common;
using KDWechat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.module
{
    public partial class module_edit : Web.UI.BasePage
    {
        protected int id { get { return RequestHelper.GetQueryInt("id", -1); } }
        protected int model_id { get { return RequestHelper.GetQueryInt("module_id",-1); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckUserAuthority("wechat_application");//配置完权限后添加
                if (id != -1)
                    InitData();
                else
                {
                    if (model_id == -1)
                        JsHelper.AlertAndRedirect("请不要随意乱入！", "module_list.aspx?m_id="+m_id.ToString());
                    radStatusOk.Checked = true;
                }
            }
        }

        private void InitData()
        {
            t_module_wechat model = module_wechat.GetModel(id);

            txtCallName.Text = model.call_name;
            txtCheckUrl.Text = model.check_url;
            txtDescription.Text = model.description;
            txtManageUrl.Text = model.manage_url;
            txtModuleName.Text = model.title;
            txtImgUrl.Text = model.img_url;
            txtEndContent.Text = model.end_description;
            txtEndUrl.Text = model.end_url;
            img_show.Src = model.img_url;
            txt_date_show.Value = (model.start_time ?? DateTime.Now).ToShortDateString() + "——" + (model.end_time ?? DateTime.Now).ToShortDateString();

            if (model.status == (int)Status.禁用)
                radStatusFalse.Checked = true;
            else
                radStatusOk.Checked = true;
        }

        protected void SubmitButtom_Click(object sender, EventArgs e)
        {
            if (id == -1)
            {
                if (wx_id != 0)
                {
                    var wechat = wx_wechats.GetWeChatByID(wx_id);
                    if (null != wechat)
                    {
                        t_module_wechat module = new t_module_wechat()
                        {
                            call_name = txtCallName.Text,
                            check_url = txtCheckUrl.Text,
                            description = txtDescription.Text,
                            manage_url = txtManageUrl.Text,
                            title = txtModuleName.Text,
                            img_url = txtImgUrl.Text,
                            u_id = u_id,
                            create_time = DateTime.Now,
                            wx_id = wechat.id,
                            wx_og_id = wechat.wx_og_id,
                            status = radStatusOk.Checked ? (int)Status.正常 : (int)Status.禁用,
                            module_id = model_id,
                            start_time = Utils.StrToDateTime(txtbegin_date.Text,DateTime.Now), //DateTime.Parse(txtbegin_date.Text),
                            end_time = Utils.StrToDateTime( txtend_date.Text, DateTime.Now),
                            end_url = txtEndUrl.Text,
                            end_description = txtEndContent.Text
                        };
                        if (module_wechat.Add(module).ID != 0)
                            JsHelper.AlertAndRedirect("模块添加成功！", "module_list.aspx?m_id=" + Request["m_id"]);
                        else
                            JsHelper.Alert("模块添加失败，请重试！");
                    }
                }
            }
            else
            {
                t_module_wechat module = module_wechat.GetModel(id);
                module.call_name = txtCallName.Text;
                module.check_url = txtCheckUrl.Text;
                module.description = txtDescription.Text;
                module.manage_url = txtManageUrl.Text;
                module.title = txtModuleName.Text;
                module.img_url = txtImgUrl.Text;
                module.status = radStatusOk.Checked ? (int)Status.正常 : (int)Status.禁用;
                module.start_time = Utils.StrToDateTime(txtbegin_date.Text, DateTime.Now);
                module.end_time = Utils.StrToDateTime(txtend_date.Text, DateTime.Now);
                module.end_url = txtEndUrl.Text;
                module.end_description = txtEndContent.Text;
                module_wechat.Update(module);
                JsHelper.AlertAndRedirect("模块修改成功！", "module_list.aspx?m_id=" + Request["m_id"]);
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("module_list.aspx?m_id=" + Request["m_id"]);
        }
    }
}