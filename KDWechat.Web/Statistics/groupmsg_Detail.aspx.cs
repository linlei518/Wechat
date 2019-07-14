using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;
using Newtonsoft.Json;
using EFHelper = Companycn.Core.EntityFramework.EFHelper;
using KDWechat.DAL;

namespace KDWechat.Web.Statistics
{
    public partial class groupmsg_Detail : Web.UI.BasePage
    {
        /// <summary>
        /// 群发ID
        /// </summary>
        protected int groupMsgID { get { return RequestHelper.GetQueryInt("msgID", 0); } }
        protected List<Tuple<string, string>> Tuplelist;
        protected new int wx_id = RequestHelper.GetQueryInt("wx_id", 0);
        protected int read_people_total = 0;
        protected int read_count_total = 0;
        protected int old_read_people_total = 0;
        protected int old_read_count_total = 0;
        protected int share_people_total = 0;
        protected int share_count_total = 0;
        protected int collect_people_total = 0;
        protected int collect_count_total = 0;
        protected int source
        {
            get
            {
                return RequestHelper.GetQueryInt("source", 0);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (source == 0)
                {
                    this.div_detail.Visible = true;
                    this.div_total.Visible = false;
                }
                else
                {
                    this.div_detail.Visible = false;
                    this.div_total.Visible = true;
                }            
                InitData();
            }
        }

        protected void InitData()
        {
            var msg = EFHelper.GetModel<creater_wxEntities, t_wx_group_msgs>(x => x.id == groupMsgID);
            if (msg != null)
            {
                //var relationArray = msg.msgID_to_SouceID.Split('|').ToList();
                //Tuplelist = new List<Tuple<string, string>>();
                //relationArray.ForEach(x =>
                //{
                //    var key_valArray = x.Split(':');
                //    Tuplelist.Add(new Tuple<string, string>(key_valArray[0], key_valArray[1]));
                //});

                var accessToken = BLL.Chats.wx_wechats.GetAccessToken(wx_id);

                var url = "https://api.weixin.qq.com/datacube/getarticletotal?access_token=" + accessToken;
                var date = msg.send_time ?? DateTime.Now;
                string data = string.Format("{{\"begin_date\":\"{0}\",\"end_date\":\"{0}\"}}", date.Date.ToString("yyyy-MM-dd"));
                var databytes = Encoding.UTF8.GetBytes(data);
                MemoryStream ms = new MemoryStream(databytes);
                var responses = RequestUtility.HttpPostWithEncoding(url, ms, Encoding.UTF8);
                var list = JsonConvert.DeserializeObject<BLL.Entity.WeiXin_Group_Article_Total_List>(responses);
                if (list != null && list.list != null && list.list.Count != 0)
                {


                    Repeater1.DataSource = list.list;
                    Repeater1.ItemDataBound += Repeater1_ItemDataBound;
                    Repeater1.DataBind();
                }
                else
                {

                }
            }
        }

        BLL.Entity.WeiXin_Group_Article_Total modelList = null;

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var articalTotal = (BLL.Entity.WeiXin_Group_Article_Total)e.Item.DataItem;
            var rep2 = (Repeater)e.Item.FindControl("Repeater2");
            if (articalTotal != null)
            {
                var titleLabel = (Label)e.Item.FindControl("labTitle");

                titleLabel.Text = articalTotal.title;
                //articalTotal.details.ForEach()

                for (int i = articalTotal.details.Count - 1; i > 0; i--)
                {
                    articalTotal.details[i].add_to_fav_count = articalTotal.details[i].add_to_fav_count - articalTotal.details[i - 1].add_to_fav_count;
                    articalTotal.details[i].add_to_fav_user = articalTotal.details[i].add_to_fav_user - articalTotal.details[i - 1].add_to_fav_user;
                    articalTotal.details[i].int_page_read_count = articalTotal.details[i].int_page_read_count - articalTotal.details[i - 1].int_page_read_count;
                    articalTotal.details[i].int_page_read_user = articalTotal.details[i].int_page_read_user - articalTotal.details[i - 1].int_page_read_user;
                    articalTotal.details[i].ori_page_read_count = articalTotal.details[i].ori_page_read_count - articalTotal.details[i - 1].ori_page_read_count;
                    articalTotal.details[i].ori_page_read_user = articalTotal.details[i].ori_page_read_user - articalTotal.details[i - 1].ori_page_read_user;
                    articalTotal.details[i].share_count = articalTotal.details[i].share_count - articalTotal.details[i - 1].share_count;
                    articalTotal.details[i].share_user = articalTotal.details[i].share_user - articalTotal.details[i - 1].share_user;
                }
                for (int i = 0; i < articalTotal.details.Count; i++)
                {
                    read_people_total += articalTotal.details[i].int_page_read_user;
                    read_count_total += articalTotal.details[i].int_page_read_count;
                    old_read_people_total += articalTotal.details[i].ori_page_read_user;
                    old_read_count_total += articalTotal.details[i].ori_page_read_count;
                    share_people_total += articalTotal.details[i].share_user;
                    share_count_total += articalTotal.details[i].share_count;
                    collect_people_total += articalTotal.details[i].add_to_fav_user;
                    collect_count_total += articalTotal.details[i].add_to_fav_count;
                }

            }
            rep2.DataSource = articalTotal.details;
            modelList = articalTotal;
            rep2.ItemDataBound += Repeater2_ItemDataBound;
            rep2.DataBind();

        }

        protected void Repeater2_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var articalTotal = modelList;
            int read_people = 0;
            int read_count = 0;
            int old_read_people = 0;
            int old_read_count = 0;
            int share_people = 0;
            int share_count = 0;
            int collect_people = 0;
            int collect_count = 0;
            for (int i = 0; i < articalTotal.details.Count; i++)
            {
                read_people += articalTotal.details[i].int_page_read_user;
                read_count += articalTotal.details[i].int_page_read_count;
                old_read_people += articalTotal.details[i].ori_page_read_user;
                old_read_count += articalTotal.details[i].ori_page_read_count;
                share_people += articalTotal.details[i].share_user;
                share_count += articalTotal.details[i].share_count;
                collect_people += articalTotal.details[i].add_to_fav_user;
                collect_count += articalTotal.details[i].add_to_fav_count;
            }
            if (e.Item.ItemType == ListItemType.Footer)
            {
                if (e.Item.FindControl("read_people") != null)
                {
                    Label lab = (Label)e.Item.FindControl("read_people");
                    lab.Text = read_people.ToString();
                }
                if (e.Item.FindControl("read_count") != null)
                {
                    Label lab = (Label)e.Item.FindControl("read_count");
                    lab.Text = read_count.ToString();
                }
                if (e.Item.FindControl("old_read_people") != null)
                {
                    Label lab = (Label)e.Item.FindControl("old_read_people");
                    lab.Text = old_read_people.ToString();
                }
                if (e.Item.FindControl("old_read_count") != null)
                {
                    Label lab = (Label)e.Item.FindControl("old_read_count");
                    lab.Text = old_read_count.ToString();
                }
                if (e.Item.FindControl("share_people") != null)
                {
                    Label lab = (Label)e.Item.FindControl("share_people");
                    lab.Text = share_people.ToString();
                }
                if (e.Item.FindControl("share_count") != null)
                {
                    Label lab = (Label)e.Item.FindControl("share_count");
                    lab.Text = share_count.ToString();
                }
                if (e.Item.FindControl("collect_people") != null)
                {
                    Label lab = (Label)e.Item.FindControl("collect_people");
                    lab.Text = collect_people.ToString();
                }
                if (e.Item.FindControl("collect_count") != null)
                {
                    Label lab = (Label)e.Item.FindControl("collect_count");
                    lab.Text = collect_count.ToString();
                }
            }
        }
    }
}