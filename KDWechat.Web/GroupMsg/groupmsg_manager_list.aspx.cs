using KDWechat.BLL.Chats;
using KDWechat.Common;
using KDWechat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.GroupMsg
{
    public partial class groupmsg_manager_list : Web.UI.BasePage
    {
        protected int pagesize = 10;
        protected Common.Config.wechatconfig wechatConfig = new BLL.Config.wechat_config().loadConfig();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckUserAuthority("groupmsg_manager_list");
                SetRefferUrl();
                InitData();
            }
        }

        private void SetRefferUrl()
        {
            try
            {
                hfReturlUrl.Value = Request.UrlReferrer.ToString();
            }
            catch (Exception)
            {
                hfReturlUrl.Value = "groupmsg_manager_list.aspx?m_id=" + m_id;
            }
        }


        private void InitData()
        {
            if (wx_id > 0)
            {
                var lis2 = Companycn.Core.EntityFramework.EFHelper.GetList<creater_wxEntities, t_qy_manager, int>(x => x.wx_id == wx_id, x => x.id, pagesize, page ,out totalCount, true);
                string pageUrl = "groupmsg_manager_list.aspx?m_id="+m_id+"&page=__id__";
                div_page.InnerHtml = Utils.OutPageList(pagesize, page, totalCount, pageUrl, 8);
                DataRepeater.DataSource = lis2;
                DataRepeater.DataBind();
            }


        }

        protected void DataRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                string uName = e.CommandArgument.ToString();
                if (!string.IsNullOrEmpty(uName))
                {
                    var influnceCount = Companycn.Core.EntityFramework.EFHelper.DeleteModel<creater_wxEntities, t_qy_manager>(x => x.user_name == uName&&x.wx_id==wx_id);
                    if (influnceCount != 0)
                    {
                        AddLog(string.Format("删除了群发管理员:{0}",uName), LogType.删除);
                        JsHelper.AlertAndRedirect("删除成功", Request.Url.ToString());
                    }

                }
            }
            else if (e.CommandName == "send")
            {
                try
                {
                    var accessToken = Senparc.Weixin.QY.CommonAPIs.CommonApi.GetToken(wechatConfig.qy_app_id, wechatConfig.qy_app_secret);
                    var url = "https://qyapi.weixin.qq.com/cgi-bin/invite/send?access_token=" + accessToken.access_token;
                    var data = "{\"userid\":\"" + e.CommandArgument.ToString() + "\",\"invite_tips\":\"微信管理员邀请您请加入群发管理组！\"}";
                    var dataBytes = Encoding.UTF8.GetBytes(data);
                    var ms = new System.IO.MemoryStream(dataBytes);
                    var response = BaiDuMapAPI.HttpUtility.RequestUtility.HttpPost(url, ms);
                    AddLog(string.Format("对群发管理员:{0}发送了关注邀请", e.CommandArgument.ToString()), LogType.修改);
                    JsHelper.AlertAndRedirect("邀请成功！", Request.Url.ToString());

                }
                catch
                {
                    JsHelper.RegisterScriptBlock(Page, "showTip.show('微信企业号接口请求失败', true);");
                    return;
                }

            }
        }


    }
}