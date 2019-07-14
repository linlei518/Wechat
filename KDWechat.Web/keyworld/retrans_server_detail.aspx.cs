using KDWechat.Common;
using KDWechat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.keyworld
{
    public partial class retrans_server_detail : Web.UI.BasePage
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

                if (id > 0)
                {
                    var model = Companycn.Core.EntityFramework.EFHelper.GetModel<creater_wxEntities, t_retrans_server>(x => x.id == id);
                    if (model != null)
                    {
                        txtTitle.Text = model.title;
                        hftitle.Value = model.title;
                        txtToken.Text = model.token;//(model.time==null?"":((DateTime)model.time).ToShortDateString());
                        txtUrl.Text = model.url;
                        txtDescription.Text = model.description;
                        txtRetransTimes.Text = (model.image_retrans_times ?? 0).ToString();
                        ckbIsImageServer.Checked = (model.is_image_server ?? 0) == 1;
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

            if (txtUrl.Text.Contains(wchatConfig.domain))
            {
                JsHelper.RegisterScriptBlock(Page, "showTip.show('不支持转发至本平台。', true);");
                return;
            }
            string timestamp = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");
            string nonce = "1254852";
            string echostr = "success";
            var signature = Senparc.Weixin.MP.CheckSignature.GetSignature(timestamp, nonce, txtToken.Text);

            string url = txtUrl.Text + (txtUrl.Text.Contains("?") ? "&" : "?") + "signature=" + signature + "&timestamp=" + timestamp + "&nonce=" + nonce + "&echostr=" + echostr;//根据转发表中的链接地址附加参数
            WebClient wc = new WebClient();
            try
            {
                var resu = wc.DownloadString(url);
                if (resu != echostr)
                {
                    wc.Dispose();
                    JsHelper.RegisterScriptBlock(Page, "showTip.show('第三方服务器验证未通过', true);");
                    return;
                }
            }
            catch
            {
                wc.Dispose();
                JsHelper.RegisterScriptBlock(Page, "showTip.show('第三方服务器验证未通过,请填写正确的URL地址', true);");
                return;
            }
            wc.Dispose();
            string msgToAppend = "";//如果已存在image转发服务器后的添加提示语

            if (id > 0)
            {
                var model = Companycn.Core.EntityFramework.EFHelper.GetModel<creater_wxEntities, t_retrans_server>(x => x.id == id);

                bool is_exists = true;
                if (hftitle.Value.Trim().Length > 0)
                {
                    if (hftitle.Value.Trim() != txtTitle.Text.Trim())
                    {
                        is_exists = Companycn.Core.EntityFramework.EFHelper.GetModel<creater_wxEntities, t_retrans_server>(x => x.title == txtTitle.Text) == null;
                    }

                }
                if (is_exists)
                {
                    model.title = txtTitle.Text.Trim();
                    model.description = txtDescription.Text;
                    model.token = txtToken.Text;
                    model.url = txtUrl.Text;
                    model.image_retrans_times = Utils.StrToInt(txtRetransTimes.Text, 0);
                    if (ckbIsImageServer.Checked)
                    {
                        var isImage = Companycn.Core.EntityFramework.EFHelper.GetModel<creater_wxEntities, t_retrans_server>(x => x.is_image_server == 1 && x.wx_id == wx_id);
                        if (isImage == null || isImage.id == model.id)
                        {
                            model.is_image_server = 1;
                        }
                        else
                            msgToAppend = "，但存在其他图片转发服务器，默认图片服务器设置失败";
                    }
                    else
                        model.is_image_server = 0;

                    bool isc = Companycn.Core.EntityFramework.EFHelper.UpdateModel<creater_wxEntities, t_retrans_server>(model);
                    if (isc)
                    {
                        AddLog("修改第三方服务：" + model.title, LogType.修改);
                        JsHelper.RegisterScriptBlock(Page, "backParentPage('success','第三方服务修改成功"+msgToAppend+"')");
                    }
                    else
                    {
                        JsHelper.RegisterScriptBlock(Page, "showTip.show('第三方服务修改失败，该第三方服务已不存在', true);");
                    }
                }
                else
                {
                    JsHelper.RegisterScriptBlock(Page, "showTip.show('第三方服务修改失败，该第三方服务已存在', true);");
                }
            }
            else
            {
                



                bool isc = Companycn.Core.EntityFramework.EFHelper.GetModel<creater_wxEntities, t_retrans_server>(x => x.title == txtTitle.Text) == null;
                var model = new t_retrans_server()
                {
                    create_time = DateTime.Now,
                    description = txtDescription.Text,
                    title = txtTitle.Text,
                    token = txtToken.Text,//Utils.StrToDateTime(txtTime.Text, DateTime.Now),
                    wx_id = wx_id,
                    wx_og_id = wx_og_id,
                    status= 1,
                    url=txtUrl.Text,
                    image_retrans_times = Utils.StrToInt(txtRetransTimes.Text, 0)
                };
                if (ckbIsImageServer.Checked)
                {
                    var isImage = Companycn.Core.EntityFramework.EFHelper.GetModel<creater_wxEntities, t_retrans_server>(x => x.is_image_server == 1 && x.wx_id == wx_id);
                    if (isImage == null)
                    {
                        model.is_image_server = 1;
                    }
                    else
                        msgToAppend = "，但存在其他图片转发服务器，默认图片服务器设置失败";
                }
                else
                    model.is_image_server = 0;
                if (isc)
                {
                    bool res = Companycn.Core.EntityFramework.EFHelper.AddModelBool<creater_wxEntities, t_retrans_server>(model);
                    if (res)
                    {
                        AddLog("添加第三方服务：" + model.title, LogType.添加);
                        JsHelper.RegisterScriptBlock(this, "backParentPage('success','第三方服务添加成功"+msgToAppend+"')");
                    }
                    else
                    {
                        JsHelper.RegisterScriptBlock(Page, "showTip.show('第三方服务添加失败', true);");
                    }
                }
                else
                {
                    JsHelper.RegisterScriptBlock(Page, "showTip.show('第三方服务名称已存在', true);");
                }
            }

        }
    }
}