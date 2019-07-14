using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.DAL;
using System.Data;
using KDWechat.BLL.Users;
using KDWechat.Common;

namespace KDWechat.Web.fans
{
    public partial class fans_filter : Web.UI.BasePage
    {
        protected string scriptToRun = "";
        protected string filterString = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckUserAuthority("wechat_fans_user_manager");
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["s"]))
                    BindFilter();
                InitData();
            }
        }

        private void BindFilter()
        {
            List<Fans_Filter> filterList = CacheHelper.Get<List<Fans_Filter>>("filter_json_" + u_id);
            if (filterList != null)
            {
                string idA = "", nameA = "", shA = "", valuseA = "";
                int counter=0;
                foreach (var filter in filterList)
                {
                    //idA += "'"+ filter.id + "',";
                    //nameA += "'" + filter.name + "',";
                    //shA += "'" + filter.sh + "',";
                    //valuseA += "'" + filter.values + "',";
                    idA +=  filter.id + ",";
                    nameA += filter.name + ",";
                    shA += filter.sh + ",";
                    valuseA += filter.values + ",";
                    filterString+="<a href=\"javascript:void(0)\" onclick=\"RemoveInput(this)\" class=\"btn cancelBubble\"  wid=\"" + counter + "\"  title=\"点击取消\">"+filter.name+"</a>" ;
                    counter++;
                }
                if (idA.Length > 0)
                {
                    //idA = "[" + idA.TrimEnd(',') + "]";
                    //nameA = "[" + nameA.TrimEnd(',') + "]";
                    //shA = "[" + shA.TrimEnd(',') + "]";
                    //valuseA = "[" + valuseA.TrimEnd(',') + "]";
                    idA = "'" + idA.TrimEnd(',') + "'";
                    nameA = "'" + nameA.TrimEnd(',') + "'";
                    shA = "'" + shA.TrimEnd(',') + "'";
                    valuseA = "'" + valuseA.TrimEnd(',') + "'";

                }
                scriptToRun = "InitSSa(" + idA + "," + shA + "," + valuseA + "," + nameA + ");";
            }
        }

        private void InitData()
        {
            var list = BLL.Users.wx_group_tags.GetListByChannelId(1, wx_id);
            foreach(var x in list)
            {
                litGroup.Text += string.Format("<a href=\"javascript:void(0);\" class=\"btn filterSelect\" sh=\"Group\" pro=\"{0}\" >{1}</a>",x.id,x.title);
            }
            list = BLL.Users.wx_group_tags.GetListByChannelId(2, wx_id);
            foreach (var x in list)
            {
                litTag.Text += string.Format("<a href=\"javascript:void(0);\" class=\"btn filterSelect\" sh=\"Tag\" pro=\"{0}\" >{1}</a>", x.id, x.title);
            }
            //var fromList = BLL.Chats.projects.GetListByWxid(wx_id);//.wx_qrcode.GetList(x => x.wx_id == wx_id);
            //foreach (DataRow x in fromList.Rows)
            //{
            //    litFrom.Text += string.Format("<a href=\"javascript:void(0);\" class=\"btn filterSelect\" sh=\"From\" pro=\"{0}\" >{1}</a>", x["id"], x["title"]);
            //}


            //var countryList = Common.CacheHelper.Get("t_wechat_country_array_" + wx_id) as string[];
            //if (countryList == null)
            //{
            //    countryList = EFHelper.GetGroupByArray<creater_wxEntities, t_wx_fans>(x => x.wx_id == wx_id, x => x.wx_country);
            //    Common.CacheHelper.Insert("t_wechat_country_array_" + wx_id, countryList, 360);
            //}
            //foreach (var x in countryList)
            //{
            //    litWeChatCountry.Text += string.Format("<a href=\"javascript:void(0);\" class=\"btn filterSelect\" sh=\"WeChatCountry\" pro=\"{0}\" >{0}</a>", x);
            //}

            //countryList = Common.CacheHelper.Get("t_country_array_" + wx_id) as string[];
            //if (countryList == null)
            //{
            //    countryList = EFHelper.GetGroupByArray<creater_wxEntities, t_wx_fans>(x => x.wx_id == wx_id, x => x.country);
            //    Common.CacheHelper.Insert("t_country_array_" + wx_id, countryList, 360);
            //}
            //foreach (var x in countryList)
            //{
            //    litCountry.Text += string.Format("<a href=\"javascript:void(0);\" class=\"btn filterSelect\" sh=\"Country\" pro=\"{0}\" >{0}</a>", x);
            //}


            List<t_sys_tags> list_aihao = Common.CacheHelper.Get("t_sys_tags_1") as List<t_sys_tags>;
            if (list_aihao == null)
            {
                list_aihao = KDWechat.BLL.Users.sys_tags.GetList(1);
                Common.CacheHelper.Insert("t_sys_tags_1", list_aihao, 360);
            }
            foreach (var x in list_aihao)
            {
                litHobby.Text += string.Format("<a href=\"javascript:void(0);\" class=\"btn filterSelect\" sh=\"Hobby\" pro=\"{0}\" >{1}</a>", x.id,x.title);
            }

        }
    }
}