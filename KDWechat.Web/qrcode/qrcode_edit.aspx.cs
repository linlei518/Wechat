using KDWechat.BLL.Chats;
using KDWechat.Common;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.CommonAPIs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.qrcode
{
    public partial class qrcode_edit : Web.UI.BasePage
    {
        protected int id { get { return RequestHelper.GetQueryInt("id", 0); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitData();
            }
        }

        private void InitData()
        {
            var qrcode = wx_qrcode.GetModel<int>(x => x.id==id, x => x.id, true);
            if (qrcode != null)
            {
                CheckUserAuthority("qrcode_list", RoleActionType.Edit);
                txtTitle.Text = qrcode.q_name;
            }
            else
            {
                CheckUserAuthority("qrcode_list", RoleActionType.Add);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var maxSourceId = 1;
            var qrcode = wx_qrcode.GetModel<int>(x => x.wx_id == wx_id, x => x.id, true);
            var wechat = wx_wechats.GetWeChatByID(wx_id);
            if (wechat != null)
            {
                if (id == 0)
                {
                    if (qrcode != null)
                        maxSourceId = qrcode.souce_id + 1;
                    var wmtOgID = System.Configuration.ConfigurationManager.AppSettings["WMTOgID"] ?? "gh_ca90998bc552";

                    if (wx_og_id == wmtOgID)
                    {
                        var url = ConfigurationManager.AppSettings["WMTApiUrl"].ToString() + "?mt=getMaxQrID";
                        WebClient wc = new WebClient();
                        maxSourceId = Utils.StrToInt(wc.DownloadString(url), 500);
                        wc.Dispose();
                    }
                    var accessToken = BLL.Chats.wx_wechats.GetAccessToken(wechat.id);//  AccessTokenContainer.TryGetToken(wechat.app_id, wechat.app_secret);
                    if (!accessToken.Contains("Error:"))
                    {
                        var ticket_result = QrCode.Create(accessToken, 0, maxSourceId);
                        var urlFormat = string.Format("https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={0}", ticket_result.ticket);
                        qrcode = wx_qrcode.GetModel<int>(x => x.q_name == txtTitle.Text && x.wx_id == wx_id, x => x.id, true);
                        if (qrcode == null)
                        {
                            qrcode = new DAL.t_wx_qrcode()
                            {
                                create_time = DateTime.Now,
                                q_name = txtTitle.Text,
                                souce_id = maxSourceId,
                                ticket = ticket_result.ticket,
                                wx_id = wx_id,
                                wx_og_id = wx_og_id,
                                q_type=(int)QrCodeType.拓客用
                            };
                            if (wx_qrcode.AddModel(qrcode))
                                JsHelper.RegisterScriptBlock(Page, "backParentPage('success','二维码添加成功')");
                            else
                                JsHelper.RegisterScriptBlock(Page, "showTip.show('二维码添加失败', true);");
                        }
                        else
                            JsHelper.RegisterScriptBlock(Page, "showTip.show('已存在同名二维码', true);");
                    }
                    else
                    {
                        JsHelper.RegisterScriptBlock(Page, "showTip.show('"+accessToken.Replace("Error:","")+"', true);");
                    }
                }
                else
                {
                    var searchQrcode = wx_qrcode.GetModel<int>(x => x.q_name == txtTitle.Text && x.wx_id == wx_id, x => x.id, true);
                    qrcode = wx_qrcode.GetModel<int>(x=>x.id==id, x => x.id, true);
                    if ((searchQrcode == null||searchQrcode.id==qrcode.id)&&qrcode!=null)
                    {
                        qrcode.q_name = txtTitle.Text;
                        wx_qrcode.UpdateModel(qrcode);
                        JsHelper.RegisterScriptBlock(Page, "backParentPage('success','二维码修改成功')");
                    }
                    else
                    {
                        JsHelper.RegisterScriptBlock(Page, "showTip.show('已存在同名二维码', true);");
                    }

                }
            }
            else
            {
                JsHelper.AlertAndRedirect("登录超时","../kdlogin/login.aspx");
            }

        }
    }
}