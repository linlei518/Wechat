using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.BLL.Chats;
using KDWechat.Common;

namespace KDWechat.Web.Account
{
    public partial class NewWeiXinAccount : KDWechat.Web.UI.BasePage
    {
        protected string qy_nick_name = "";
        protected int id
        {
            get { return RequestHelper.GetQueryInt("id", 0); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (u_type == 2 || u_type == 3)
            {
                if (id == 0)
                    CheckUserAuthority("region_wechat", RoleActionType.Add);
                else
                    CheckUserAuthority("region_wechat", RoleActionType.Edit);
            }


            if (!IsPostBack)
            {
                try
                {
                    hfReturlUrl.Value = Request.UrlReferrer.ToString();
                }
                catch (Exception)
                {
                    hfReturlUrl.Value = "region_wxlist.aspx?m_id=59";
                }
                bindCity();
                if (id > 0)
                {
                    InitData();
                }
                else
                {
                    //生成微信号对应的唯一guid
                    string guid = Guid.NewGuid().ToString().Replace("-", "");
                    //根据guid生成token
                    string token = DESEncrypt.Encrypt(guid, guid.Substring(1, 5)).Substring(0, 10);
                    txtApiUrl.Text = siteConfig.weburl + siteConfig.webmanagepath + "KDWeChat.aspx?t=" + guid;  //ceiling update
                    txtToken.Text = token;
                    txtApiUrl.Enabled = txtToken.Enabled = false;

                }


            }
        }

        private void bindCity()
        {
            ddlCity.DataSource = Companycn.Core.EntityFramework.EFHelper.GetList<DAL.creater_wxEntities, DAL.t_category, int?>(x => x.channel_id == (int)CategoryChannel.项目城市地区 && x.parent_id==0, x => x.sort_id, int.MaxValue, 1);
            ddlCity.DataBind();
        }

        private void InitData()
        {
            var wechat = wx_wechats.GetWeChatByID(id);

            if (null != wechat)
            {
                txtPbName.Text = wechat.wx_pb_name;
                txtOGID.Text = wechat.wx_og_id;
                txtWxID.Text = wechat.wx_name;
                txtFile.Value = wechat.header_pic;
                img_show.Src = wechat.header_pic;
                txtImg.Value = wechat.qrcode_img;
                img_erweima.Src = wechat.qrcode_img;

                ddlType.SelectedIndex = wechat.type_id;
                txtAppID.Text = wechat.app_id;
                txtAppSecret.Text = wechat.app_secret;
                txtApiUrl.Text = wechat.api_url;
                txtToken.Text = wechat.token;
                txtOGID.Enabled = txtApiUrl.Enabled = txtToken.Enabled = false;
                dlApi.Attributes.Clear();
                dlToken.Attributes.Clear();
                qy_user_id.Value = wechat.qy_manager_name;
                qy_user_name.Value = wechat.qy_manager_nick;
                //绑定管理员名称
                qy_nick_name = wechat.qy_manager_nick ?? "";
                ddlCity.SelectedValue = wechat.city;
            }


        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (id == 0)
                AddWeChat();
            else
            {
                var wechat = wx_wechats.GetWeChatByID(id);
                if (null != wechat)
                    UpdateWeChat(wechat);
                else
                    AddWeChat();
            }
        }

        void UpdateWeChat(DAL.t_wx_wechats wechat)
        {
            wechat.wx_pb_name = Utils.DropHTML(txtPbName.Text.TrimEnd('.'));
            wechat.wx_og_id = Utils.DropHTML(txtOGID.Text.TrimEnd('.'));
            wechat.wx_name = Utils.DropHTML(txtWxID.Text.TrimEnd('.'));
            wechat.header_pic = Utils.DropHTML(txtFile.Value.TrimEnd('.'));
            wechat.type_id = ddlType.SelectedIndex;//(int)WeChatServiceType.认证后服务号;
            wechat.app_id = Utils.DropHTML(txtAppID.Text.TrimEnd('.'));
            wechat.app_secret = Utils.DropHTML(txtAppSecret.Text.TrimEnd('.'));
            wechat.qy_manager_name = qy_user_id.Value;
            wechat.qy_manager_nick = qy_user_name.Value;
            wechat.city = ddlCity.SelectedValue;
            wechat.qrcode_img = txtImg.Value;
            wx_wechats.UpdateWeChat(wechat);
            AddLog(string.Format("修改了公众号：{0}", wechat.wx_name), LogType.修改);
            JsHelper.AlertAndRedirect("修改成功", hfReturlUrl.Value);


        }

        void AddWeChat()
        {
            var wechat = wx_wechats.CreateWeChat(
                u_id,
                Utils.DropHTML(txtPbName.Text.TrimEnd('.')),
                Utils.DropHTML(txtOGID.Text.Replace(" ", "").Replace(".", "").Replace("+", "")),
                Utils.DropHTML(txtWxID.Text.TrimEnd('.')),
                Utils.DropHTML(txtFile.Value.TrimEnd('.')),
                (WeChatServiceType)ddlType.SelectedIndex,//.认证后服务号,
                Utils.DropHTML(txtAppID.Text.TrimEnd('.')),
                Utils.DropHTML(txtAppSecret.Text.TrimEnd('.')),
                Utils.DropHTML(txtToken.Text.Trim().TrimEnd('.')),
                Utils.DropHTML(txtApiUrl.Text.Trim().TrimEnd('.')),
                qy_user_id.Value,
                qy_user_name.Value,
                ddlCity.SelectedValue,
                txtImg.Value
            );
          
            if (null != wechat)
            {
                AddLog(string.Format("添加了公众号：{0}", wechat.wx_name), LogType.添加);
                RegisterStartupScript("key1", "<script>bombbox.openBox('api_token_detail.aspx?id=" + wechat.id + "', function () { setTimeout(function () { bombbox.iframeItem.get(0).contentWindow.start(); },1000)});</script>");
            }
            else
            {
                JsHelper.Alert("公众号已存在，添加失败");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(hfReturlUrl.Value);
        }
    }
}