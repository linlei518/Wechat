using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.DAL;
using KDWechat.Common;
using System.Linq.Expressions;

namespace KDWechat.Web.Statistics
{
    public partial class Binding_List : Web.UI.BasePage
    {
        protected List<BLL.Chats.TimeStatistics<int>> lastTimeList = null;
        protected List<BLL.Chats.CountStatistics<int>> suscribtList = null;
        protected List<BLL.Chats.CountStatistics<int>> unSuscribtList = null;
        protected int isBind { get { return RequestHelper.GetQueryInt("isB",0); } }
        protected string IDs = RequestHelper.GetQueryString("Ids");

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckUserAuthority("binding_list");
            if (!IsPostBack)
            {
                InitData();
            }
        }

        private void InitData()
        {
            lastTimeList = new List<BLL.Chats.TimeStatistics<int>>();
            var fansTimeList = BLL.Chats.wechat_binding.GetFansTimeStatistics();
            var viewTimeList = BLL.Chats.wechat_binding.GetViewTimeStatistics();
            var chatTimeList = BLL.Chats.wechat_binding.GetChatsTimeStatistics();
            var locationTimeList = BLL.Chats.wechat_binding.GetLocationTimeStatistics();
            //var suscribtList = Companycn.Core.EntityFramework.EFHelper.GetGroupBy<DAL.creater_wxEntities, DAL.t_wx_fans>(x => , x => x.wx_id);
            suscribtList = BLL.Chats.wechat_binding.GetWeeklyFansCount(true);
            unSuscribtList = BLL.Chats.wechat_binding.GetWeeklyFansCount(false);

            //lastTimeList.AddRange(fansTimeList);
            lastTimeList.AddRange(viewTimeList);
            lastTimeList.AddRange(chatTimeList);
            lastTimeList.AddRange(locationTimeList);
            CacheHelper.Insert("s_fansTimeList", fansTimeList);
            CacheHelper.Insert("s_viewTimeList", viewTimeList);
            CacheHelper.Insert("s_chatTimeList", chatTimeList);
            CacheHelper.Insert("s_locationTimeList", locationTimeList);


            Expression<Func<t_wx_wechats, bool>> wx_list_where = x => x.status == (int)Status.正常;
            if (!string.IsNullOrWhiteSpace(IDs))
            {
                var idArray = IDs.Split(',');
                if (idArray.Length != 0)
                {
                    var wx_ids = new int[idArray.Length];
                    for (int i = 0; i < wx_ids.Length; i++)
                    {
                        wx_ids[i] = Utils.StrToInt(idArray[i], 0);
                    }
                    wx_list_where = wx_list_where.And(x => wx_ids.Contains(x.id));
                }
            }

            var wecahtList = Companycn.Core.EntityFramework.EFHelper.GetList<creater_wxEntities, t_wx_wechats, int>(wx_list_where, x => x.id, int.MaxValue, 1, out totalCount, true);//去除分页，取所有--Damos

            Repeater1.DataSource = wecahtList;
            Repeater1.DataBind();

        }
        protected string GetSCount(object objId)
        {
            var strToReturn = "0";
            var id = Utils.ObjToInt(objId, 0);
            if (id != 0)
            {
                var last_time = suscribtList.Where(x => x.key == id).FirstOrDefault();
                if (last_time != null)
                    strToReturn = last_time.count.ToString();
            }
            return strToReturn;
        }

        protected string GetUSCount(object objId)
        {
            var strToReturn = "0";
            var id = Utils.ObjToInt(objId, 0);
            if (id != 0)
            {
                var last_time = unSuscribtList.Where(x => x.key == id).FirstOrDefault();
                if (last_time != null)
                    strToReturn = last_time.count.ToString();
            }
            return strToReturn;

        }
        protected string GetDisplay(object objId)
        {
            var state = GetStatus(objId);
            if (isBind == 1)
            {
                if (state == "否")
                    return "display:none";
            }
            else if(isBind==2)
            {
                if (state == "是")
                    return "display:none";
            }
            return "";
            
        }



        protected string GetLastTime(object objId)
        {
            var strToReturn ="无";
            var id = Utils.ObjToInt(objId, 0);
            if (id != 0)
            {
                var last_time = lastTimeList.Where(x => x.key == id).OrderByDescending(x=>x.last_time).FirstOrDefault();
                if (last_time != null)
                    strToReturn = last_time.last_time.ToString();
            }
            return strToReturn;
        }

        protected string GetStatus(object objId)
        {
            var strToReturn = "否";
            var id = Utils.ObjToInt(objId, 0);
            if (id != 0)
            {
                var last_time = lastTimeList.Where(x => x.key == id).OrderByDescending(x => x.last_time).FirstOrDefault();
                if (last_time != null)
                {
                    var span = DateTime.Now - last_time.last_time;
                    if (span < TimeSpan.FromDays(10) && span > TimeSpan.FromDays(0))
                        strToReturn = "是";
                }
            }
            return strToReturn;
        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (GetDisplay(((t_wx_wechats)e.Item.DataItem).id) != "")
                {
                    e.Item.Visible = false;
                    //e.Item.Dispose();
                }
            }
        }
    }
}