using KDWechat.BLL.Chats;
using KDWechat.Common;
using KDWechat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.GroupMsg
{
    public partial class group_messsage_list : Web.UI.BasePage
    {
        protected int pagesize = 10;
        protected Common.Config.wechatconfig wechatConfig = new BLL.Config.wechat_config().loadConfig();
        protected string historyLink = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckUserAuthority("wechat_massage");
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
                hfReturlUrl.Value = "group_message_list.aspx?m_id=10";
            }
        }


        private void InitData()
        {

            int totalCount;
            List<t_wx_group_msgs> lis2 = null;
            if (wx_id > 0)
            {
                lis2 = BLL.Chats.wx_group_msgs.GetGroupMsgListByWxID(wx_id, page,pagesize, out totalCount);
                string pageUrl = "group_messsage_list.aspx?m_id=10&page=__id__";
                div_page.InnerHtml = Utils.OutPageList(pagesize, page, totalCount, pageUrl, 8);
                var wechatConfig = new BLL.Config.wechat_config().loadConfig();
                historyLink = wechatConfig.domain + "/groupmsg/history_group_msg.aspx?fd="+DESEncrypt.Encrypt(wx_id.ToString());
            }

            DataRepeater.DataSource = lis2;
            DataRepeater.DataBind();
        }

        protected void DataRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int id = KDWechat.Common.Utils.StrToInt(e.CommandArgument.ToString(), 0);
            if (id != 0)
            {
                if (e.CommandName == "del")
                {
                    t_wx_group_msgs msg = null;

                    msg = wx_group_msgs.DeleteMsg(id);
                    if (msg != null)
                    {
                        AddLog(string.Format("删除了群发信息:{0}", msg.title), LogType.修改);
                        JsHelper.AlertAndRedirect("删除成功", Request.Url.ToString());
                        //Response.Redirect(Request.Url.ToString());
                    }
                }
                else if (e.CommandName == "sendCheck")
                {
                    var managerList = Companycn.Core.EntityFramework.EFHelper.GetList<creater_wxEntities, t_qy_manager, int>(x => x.wx_id == wx_id, x => x.id, int.MaxValue, 1);
                    string uList = "";
                    if (null != managerList)
                        managerList.ForEach(x => uList += x.user_name + "|");
                    if (uList.Length > 0)
                    {
                        uList = uList.TrimEnd('|');
                        try
                        {
                            var code = Companycn.Core.EntityFramework.EFHelper.GetModel<creater_wxEntities, t_wx_group_msg_key>(x => x.group_msg_id == id);
                            if (code != null)
                            {
                                var msg = Companycn.Core.EntityFramework.EFHelper.GetModel<creater_wxEntities, t_wx_group_msgs>(x => x.id == code.group_msg_id);
                                if (msg != null)
                                {
                                    var accessToken = Senparc.Weixin.QY.CommonAPIs.CommonApi.GetToken(wechatConfig.qy_app_id, wechatConfig.qy_app_secret);
                                    Senparc.Weixin.QY.AdvancedAPIs.Mass.SendText(accessToken.access_token, uList, "", "", wechatConfig.qy_agent_id, string.Format("有一条群发消息需要您的验证，消息名称为‘{0}’，验证码为‘{1}’", msg.title, code.accessKey));
                                    JsHelper.RegisterScriptBlock(Page, "showTip.show('发送成功', false);");
                                }
                            }
                        }
                        catch
                        {
                            JsHelper.RegisterScriptBlock(Page, "showTip.show('发送失败，请重试', true);");
                        }
                    }
                }
            }
        }
    }
}