using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.BLL.Chats;
using KDWechat.Common;
using KDWechat.DAL;

namespace KDWechat.Web.material
{
    public partial class select_appList : Web.UI.BasePage
    {
        protected int pagesize = 10;
        protected int tag { get { return RequestHelper.GetQueryInt("tag", 0); } }

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
            Expression<Func<t_module_wx_switch, bool>> where = x => x.wx_id == wx_id && x.status == (int)Status.正常 ;
            var list = KDWechat.BLL.Chats.module_wx_switch.GetList(where, int.MaxValue, 1, out totalCount);
            var openArray = (from x in list select x.module_id).ToArray();



            Expression<Func<t_modules, bool>> where2 = x => x.status == (int)Status.正常 &&  x.is_push==1;
            if (tag > 0)
                where2 = where2.And(x => x.type == tag);
            where2=where2.And( x=> openArray.Contains(x.id));
            var list2 = modules.GetList(where2, pagesize, page, out totalCount);
            
          



            ////设置分页
            string pageUrl = "select_appList.aspx?page=__id__";
            div_page.InnerHtml = Utils.OutPageList(pagesize, page, totalCount, pageUrl, 8);

            //绑定repeater
            repAllSysModule.DataSource = list2;
            repAllSysModule.DataBind();


        }

 
    }
}