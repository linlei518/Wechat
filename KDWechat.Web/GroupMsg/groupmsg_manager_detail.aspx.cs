using KDWechat.Common;
using KDWechat.DAL;
using Senparc.Weixin.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.GroupMsg
{
    public partial class groupmsg_manager_detail : Web.UI.BasePage
    {
        protected Common.Config.wechatconfig wechatConfig = new BLL.Config.wechat_config().loadConfig();

        protected int id
        {
            get { return RequestHelper.GetQueryInt("id", 0); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //权限相关，发布开启
            CheckUserAuthority("groupmsg_manager_list");
            if (!IsPostBack)
            {
                try
                {
                    var accessToken = Senparc.Weixin.QY.CommonAPIs.CommonApi.GetToken(wechatConfig.qy_app_id, wechatConfig.qy_app_secret);
                    var departmentID = Utils.StrToInt(wechatConfig.qy_manage_group, 0);
                    var list = Senparc.Weixin.QY.AdvancedAPIs.Member.GetDepartmentMemberInfo(accessToken.access_token, departmentID, 1, 1);
                    Repeater1.DataSource = list.userlist;
                    Repeater1.DataBind();
                }
                catch
                {
                    JsHelper.RegisterScriptBlock(Page, "showTip.show('企业号接口调用失败，请检查配置后再试。', true);");
                }
            }
        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "sel")
            {
                var userID = e.CommandArgument.ToString();
                var manager = Companycn.Core.EntityFramework.EFHelper.GetModel<creater_wxEntities, t_qy_manager>(x => x.user_name == userID&&x.wx_id==wx_id);
                if (manager == null)
                {
                    try
                    {
                        var accessToken = Senparc.Weixin.QY.CommonAPIs.CommonApi.GetToken(wechatConfig.qy_app_id, wechatConfig.qy_app_secret);
                        var user = Senparc.Weixin.QY.AdvancedAPIs.Member.GetMember(accessToken.access_token, userID);
                        if (user != null)
                        {
                            Companycn.Core.EntityFramework.EFHelper.AddModel<creater_wxEntities, t_qy_manager>(new t_qy_manager() { create_time = DateTime.Now, wx_id = wx_id, wx_og_id = wx_og_id, email = user.email, status = (int)Status.正常, mobile = user.mobile, nick_name = user.name, tel = user.mobile, user_name = userID, wechat_no = user.weixinid });
                            JsHelper.RegisterScriptBlock(Page, "backParentPage('success','群发管理员设置成功')");
                        }
                        else
                        {
                            JsHelper.RegisterScriptBlock(Page, "showTip.show('用户选择失败，请刷新后重试', true);");
                        }
                    }
                    catch
                    {
                        JsHelper.RegisterScriptBlock(Page, "showTip.show('企业号接口调用失败，请检查配置后再试。', true);");
                    }
                }
                else
                {
                    JsHelper.RegisterScriptBlock(Page, "showTip.show('当前管理员已存在', true);");
                }
            }
        }



    }
}