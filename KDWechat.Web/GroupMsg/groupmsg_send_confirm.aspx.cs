using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;
using KDWechat.DAL;
using EFHelper = Companycn.Core.EntityFramework.EFHelper;

namespace KDWechat.Web.GroupMsg
{
    public partial class groupmsg_send_confirm : Web.UI.BasePage
    {
        private int _id = RequestHelper.GetQueryInt("id",0);
        protected int id { get { return _id; } }
        t_wx_group_msg_key authOk;

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckUserAuthority("wechat_massage");
            if (!IsPostBack)
            {
                CheckAuth();
            }
        }

        private bool CheckAuth()
        {
            if (id != 0)
            {
                authOk = Companycn.Core.EntityFramework.EFHelper.GetModel<creater_wxEntities, t_wx_group_msg_key>(x => x.group_msg_id == id);
                if (authOk != null)
                {
                    return true;
                }
            }
            JsHelper.RegisterScriptBlock(Page, "backParentPage('fail','参数错误，请刷新后重试')");
            return false;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            authOk = Companycn.Core.EntityFramework.EFHelper.GetModel<creater_wxEntities, t_wx_group_msg_key>(x => x.group_msg_id == id);
            if (authOk != null)
            {
                var groupMsg = EFHelper.GetModel<creater_wxEntities, t_wx_group_msgs>(x => x.id == authOk.group_msg_id);
                if (groupMsg != null && groupMsg.is_send == (int)is_sendMode.否)
                {
                    if (txtConfirmCode.Text.Trim() == authOk.accessKey)
                    {
                        if (groupMsg.is_timer == (int)is_timerMode.否)//普通群发
                        {
                            groupMsg.is_send = (int)is_sendMode.是;
                            EFHelper.UpdateModel<creater_wxEntities, t_wx_group_msgs>(groupMsg);
                            var sentMsgResult =BLL.Chats.wx_group_msgs.SendGroupMsg(groupMsg);
                            if (sentMsgResult.Item1)
                            {
                                JsHelper.RegisterScriptBlock(Page, "backParentPage('success','群发成功');");
                                groupMsg.send_time = DateTime.Now;
                                groupMsg.is_check = (int)GroupMsgCheckMode.已审核;
                                EFHelper.UpdateModel<creater_wxEntities, t_wx_group_msgs>(groupMsg);
                            }
                            else if (groupMsg.msg_type == (int)msg_type.单图文 || groupMsg.msg_type == (int)msg_type.多图文)
                            {
                                groupMsg.is_send = (int)is_sendMode.否;
                                EFHelper.UpdateModel<creater_wxEntities, t_wx_group_msgs>(groupMsg);
                                JsHelper.RegisterScriptBlock(Page, string.Format("backParentPage('fail','{0}');",sentMsgResult.Item2));
                            }
                            else
                            {
                                JsHelper.RegisterScriptBlock(Page,string.Format("backParentPage('fail','{0}');",sentMsgResult.Item2));
                            }
                        }
                        else//是定时群发
                        {
                            if(groupMsg.send_time>DateTime.Now)
                            {
                                groupMsg.is_check = (int)GroupMsgCheckMode.已审核;
                                EFHelper.UpdateModel<creater_wxEntities, t_wx_group_msgs>(groupMsg);
                                JsHelper.RegisterScriptBlock(Page, "backParentPage('success','群发请求已审核，该群发将在规定时间内进行');");
                            }
                            else
                                JsHelper.RegisterScriptBlock(Page, "backParentPage('fail','预订群发时间已过，请修改后再试');");
                        }
                    }
                    else
                        JsHelper.RegisterScriptBlock(Page, "showTip.show('验证码错误', true);");
                }
                else
                    JsHelper.RegisterScriptBlock(Page, "backParentPage('fail','请不要重复操作，刷新后重试');");
            }
            else
                JsHelper.RegisterScriptBlock(Page, "backParentPage('fail','参数错误，请刷新后重试');");


        }
    }
}