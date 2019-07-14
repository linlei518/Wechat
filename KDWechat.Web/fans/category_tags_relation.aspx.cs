using KDWechat.BLL.Chats;
using KDWechat.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.DAL;

namespace KDWechat.Web.fans
{
    public partial class category_tags_relation : Web.UI.BasePage
    {
        protected int category_id { get { return RequestHelper.GetQueryInt("id", -1); } }
        List<t_wx_group_tags> tagsList;
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckUserAuthority("sys_module");
            if (!IsPostBack)
            {
                InitCheckBox();
                InitData();
            }
        }

        private void InitCheckBox()
        {
            tagsList = Companycn.Core.EntityFramework.EFHelper.GetList<creater_wxEntities, t_wx_group_tags, int>(x => true, x => x.id, int.MaxValue, 1);//wx_wechats.GetList();
            chbAllowWechats.DataSource = tagsList;
            chbAllowWechats.DataTextField = "title";
            chbAllowWechats.DataValueField = "id";
            chbAllowWechats.DataBind();
        }

        private void InitData()
        {
            if (category_id > 0)
            {
                var checkList = tagsList.Where(x => x.parent_id == category_id).Select(x=>x.id).ToArray();
                //string[] wechatIds = model.allow_wechats.Split(',');

                if (checkList != null && checkList.Length > 0)
                {

                    foreach (ListItem x in chbAllowWechats.Items)
                    {
                        if (checkList.Contains(Utils.StrToInt(x.Value,-100)))
                        {
                            x.Selected = true;
                        }
                    }
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (category_id > 0)
            {
                var tagsIDList = new List<int>();
                var removeTagIDList = new List<int>();
                foreach (ListItem item in chbAllowWechats.Items)
                {
                    if (item.Selected)
                    {
                        var tagID = Utils.StrToInt(item.Value, 0);
                        if (!tagsIDList.Contains(tagID))
                            tagsIDList.Add(tagID);
                    }
                    else
                    {
                        var tagID = Utils.StrToInt(item.Value, 0);
                        if (!removeTagIDList.Contains(tagID))
                            removeTagIDList.Add(tagID);
                    }
                }
                if (tagsIDList.Count > 0)
                    Companycn.Core.EntityFramework.EFHelper.UpdateModel<creater_wxEntities, t_wx_group_tags>(x => tagsIDList.Contains(x.id), x => new t_wx_group_tags { parent_id = category_id });
                if(removeTagIDList.Count>0)
                    Companycn.Core.EntityFramework.EFHelper.UpdateModel<creater_wxEntities, t_wx_group_tags>(x => removeTagIDList.Contains(x.id), x => new t_wx_group_tags { parent_id = 0 });
                JsHelper.RegisterScriptBlock(this, " backParentPage('success', '标签分组选择成功');");

            }
            
        }
    }
}