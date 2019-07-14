using KDWechat.Common;
using KDWechat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.Account
{
    public partial class send_qy_message : Web.UI.BasePage
    {
        protected Common.Config.wechatconfig wechatConfig = new BLL.Config.wechat_config().loadConfig();

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckUserAuthority("head_letter");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var managerList = Companycn.Core.EntityFramework.EFHelper.GetList<creater_wxEntities, t_wx_wechats, int>(x => true, x => x.id, int.MaxValue, 1);
            string uList = "";
            if (null != managerList)
            {
                managerList.ForEach(x =>{
                    if(!string.IsNullOrEmpty(x.qy_manager_name))
                        uList += x.qy_manager_name + "|";
                });
            }
            try
            {
                var accessToken = Senparc.Weixin.QY.CommonAPIs.CommonApi.GetToken(wechatConfig.qy_app_id, wechatConfig.qy_app_secret);
                Senparc.Weixin.QY.AdvancedAPIs.Mass.SendText(accessToken.access_token, uList, "", "", wechatConfig.qy_agent_id, txtContents.Value.Replace("<br />",""));
                JsHelper.AlertAndParentUrl("发送成功！", Request.Url.ToString());
            }
            catch
            {
                JsHelper.AlertAndParentUrl("企业号接口请求失败！", Request.Url.ToString());
            }
        }
    }
}