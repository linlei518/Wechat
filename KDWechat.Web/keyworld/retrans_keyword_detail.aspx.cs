using KDWechat.Common;
using KDWechat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.keyworld
{
    public partial class retrans_keyword_detail : Web.UI.BasePage
    {
        protected int id
        {
            get { return RequestHelper.GetQueryInt("id", 0); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //权限相关，发布开启
            //CheckUserAuthority("draw_winner_all");
            if (!IsPostBack)
            {

                var serverList = Companycn.Core.EntityFramework.EFHelper.GetList<creater_wxEntities, t_retrans_server, int>(x => x.status == 1 && x.wx_id == wx_id, x => x.id, int.MaxValue, 1, true);
                ddlServer.DataSource = serverList;
                ddlServer.DataTextField = "title";
                ddlServer.DataValueField = "id";
                ddlServer.DataBind();

                if (id > 0)
                {
                    var model = Companycn.Core.EntityFramework.EFHelper.GetModel<creater_wxEntities, t_retrans_keyword>(x => x.id == id);
                    if (model != null)
                    {
                        txtTitle.Text = model.keyword;
                        hftitle.Value = model.keyword;
                        txtRetransTimes.Text = (model.retrans_times ?? 0).ToString();
                        ddlServer.SelectedValue = model.retrans_id.ToString();
                    }
                    else
                    {
                        Response.Redirect("retrans_server_detail.aspx?m_id=" + m_id);
                    }
                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            CheckWXid();

            if (id > 0)
            {
                var model = Companycn.Core.EntityFramework.EFHelper.GetModel<creater_wxEntities, t_retrans_keyword>(x => x.id == id);

                bool is_exists = true;
                if (hftitle.Value.Trim().Length > 0)
                {
                    if (hftitle.Value.Trim() != txtTitle.Text.Trim())
                    {
                        is_exists = Companycn.Core.EntityFramework.EFHelper.GetModel<creater_wxEntities, t_retrans_keyword>(x => x.keyword == txtTitle.Text) == null;
                    }

                }
                if (is_exists)
                {
                    model.keyword = txtTitle.Text.Trim();
                    model.retrans_id = Utils.StrToInt(ddlServer.SelectedValue, 0);
                    model.retrans_times = Utils.StrToInt(txtRetransTimes.Text, 0);
                    bool isc = Companycn.Core.EntityFramework.EFHelper.UpdateModel<creater_wxEntities, t_retrans_keyword>(model);
                    if (isc)
                    {
                        AddLog("修改第三方关键词：" + model.keyword, LogType.修改);
                        JsHelper.RegisterScriptBlock(Page, "backParentPage('success','第三方关键词修改成功')");
                    }
                    else
                    {
                        JsHelper.RegisterScriptBlock(Page, "showTip.show('第三方关键词修改失败，该第三方关键词已不存在', true);");
                    }
                }
                else
                {
                    JsHelper.RegisterScriptBlock(Page, "showTip.show('第三方关键词修改失败，该第三方关键词已存在', true);");
                }
            }
            else
            {


                bool isc = Companycn.Core.EntityFramework.EFHelper.GetModel<creater_wxEntities, t_retrans_keyword>(x => x.keyword == txtTitle.Text) == null;
                var retrans_id = Utils.StrToInt(ddlServer.SelectedValue, -1);
                if (retrans_id != -1)
                {
                    var model = new t_retrans_keyword()
                    {
                        create_time = DateTime.Now,
                        keyword = txtTitle.Text,
                        retrans_id = retrans_id,
                        wx_id = wx_id,
                        wx_og_id = wx_og_id,
                        status = 1,
                        retrans_times = Utils.StrToInt(txtRetransTimes.Text, 0)
                    };
                    if (isc)
                    {
                        bool res = Companycn.Core.EntityFramework.EFHelper.AddModelBool<creater_wxEntities, t_retrans_keyword>(model);
                        if (res)
                        {
                            AddLog("添加第三方关键词：" + model.keyword, LogType.添加);
                            JsHelper.RegisterScriptBlock(this, "backParentPage('success','第三方关键词添加成功')");
                        }
                        else
                        {
                            JsHelper.RegisterScriptBlock(Page, "showTip.show('第三方关键词添加失败', true);");
                        }
                    }
                    else
                    {
                        JsHelper.RegisterScriptBlock(Page, "showTip.show('第三方关键词名称已存在', true);");
                    }
                }
                else
                {
                    JsHelper.RegisterScriptBlock(Page, "showTip.show('请选择正确的第三方服务', true);");
                }
            }

        }
    }
}