using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.BLL.Chats;
using KDWechat.Common;
using KDWechat.DAL;

namespace KDWechat.Web.setting
{
    public partial class template_manager_select_wechat : Web.UI.BasePage
    {
        protected int template_id { get { return RequestHelper.GetQueryInt("template_id", -1); } }
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckUserAuthority("template");
            if (!IsPostBack)
            {
                InitCheckBox();
                InitData();
            }
        }

        private void InitCheckBox()
        {
            var wechatList = wx_wechats.GetList();

            //wechatList.Add(new t_wx_wechats() { id = 0, wx_pb_name = "全部" });
            //wechatList.Reverse();

            chbAllowWechats.DataSource = wechatList;

            chbAllowWechats.DataTextField = "wx_pb_name";
            chbAllowWechats.DataValueField = "id";

            chbAllowWechats.DataBind();
        }

        private void InitData()
        {
            if (template_id > 0)
            {
                DAL.t_wx_templates model = BLL.Chats.wx_templates.GetModel(template_id);
                if (model!=null)
                {
                    hfname.Value = model.title;
                    List<string> list = BLL.Chats.wx_templates.GetWxIdListBytemplateId(template_id);
                    if (list == null)
                    {
                        list = new List<string>();
                    }

                    foreach (ListItem x in chbAllowWechats.Items)
                    {
                        if (list.Contains(x.Value))
                        {
                            x.Selected = true;
                        }
                    }
                }
                else
                {
                    JsHelper.RegisterScriptBlock(this, " backParentPage('fail', '模板不存在');");
                }

                
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            
            List<DAL.t_wx_templates_wechats> list = new List<DAL.t_wx_templates_wechats>();
            List<int> remove = new List<int>();
            string name = "";
            foreach (ListItem item in chbAllowWechats.Items)
            {
                if (item.Selected)
                {
                    list.Add(new t_wx_templates_wechats()
                    {
                        wx_id = Common.Utils.StrToInt(item.Value, 0),
                        channel_id = 1,
                        is_default = 0,
                        template_id = template_id,
                        wx_og_id = BLL.Chats.wx_wechats.GetWeChatByID(Common.Utils.StrToInt(item.Value, 0)).wx_og_id

                    });
                    name +=item.Text+ ",";
                }
                else
                {
                    remove.Add(Common.Utils.StrToInt(item.Value, 0));
                }

            }
            int num = BLL.Chats.wx_templates.AddTemplatesWechats(list,template_id);
            if (num > 0)
            {
              
                AddLog("为图文模板【" + hfname.Value + "】分配了以下公众号：" + name.TrimEnd(','), LogType.修改);
                JsHelper.RegisterScriptBlock(this, " backParentPage('success', '权限分配成功');");
            }
            else
                JsHelper.RegisterScriptBlock(this, " backParentPage('fail', '权限分配失败');");
        }
    }
}