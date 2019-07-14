using Companycn.Core.DbHelper;
using KDWechat.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.fans
{
    public partial class filter_export : Web.UI.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
                CheckUserAuthority("wechat_fans_user_manager",RoleActionType.Export);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var ziduan = "";
            List<string> nameList = new  List<string>();
            foreach (ListItem x in chbAllowWechats.Items)
            {
                if (x.Selected)
                {
                    if (x.Value == "sex")
                        ziduan += "case sex when 1 then '男' when 2 then '女' else '未知' end,";
                    else if (x.Value == "wx_sex")
                        ziduan += "case wx_sex when 1 then '男' when 2 then '女' else '未知' end,";
                    else if (x.Value == "is_kd_owner")
                        ziduan += "case is_kd_owner when 1 then '是' else '否' end,";
                    else if (x.Value == "is_kd_worker")
                        ziduan += "case is_kd_worker when 1 then '是' else '否' end,";
                    else if (x.Value == "have_child")
                        ziduan += "case have_child when 1 then '有' else '无' end,";
                    else if (x.Value == "status")
                        ziduan += "case status when 1 then '关注中' else '已取消关注' end,";
                    else
                        ziduan += Utils.Filter(x.Value) + ",";
                    nameList.Add(x.Text);
                }
            }
            if(ziduan.Length>0)
                ziduan=ziduan.TrimEnd(',');
            var query = CacheHelper.Get<string>("adwanced_query_" + u_id);
            System.Data.DataTable table;
            if (string.IsNullOrEmpty(query))
                table = KDWechat.DBUtility.DbHelperSQL.Query("select " + ziduan + " from t_wx_fans where wx_id =" + wx_id).Tables[0];
            else
            {
                table = KDWechat.DBUtility.DbHelperSQL.Query(query.Replace("group_id,id,headimgurl,nick_name,unionid,last_interact_time,reply_state,open_id", ziduan)).Tables[0];
            }

            GemBoxExcelLiteHelper.SaveExcel(Server.MapPath("~/upload/" + wx_og_id + ".xls"), this, true, true, nameList , table);//.DataTable1Excel(table);
        }

        string GetSex(object sexObj)
        {
            switch (sexObj.ToString())
            {
                case "1":
                    return "男";
                case "2":
                    return "女";
                case "0":
                default:
                    return "未知";
            }
        }
    }
}