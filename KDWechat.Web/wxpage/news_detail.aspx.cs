using KDWechat.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.wxpage
{
    public partial class news_detail : System.Web.UI.Page
    {
        protected string openId { get { return RequestHelper.GetQueryString("openId"); } }
        protected int id { get { return RequestHelper.GetQueryInt("id", 0); } }
        protected int lbs { get { return RequestHelper.GetQueryInt("lbs", 0); } }
        protected string ShareContent;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (lbs > 0)
            {
                BindLbs(lbs);
            }
            else
            {
                if (id > 0)
                {
                    BindData(id);
                }
            }

        }

        //绑定LBS数据
        private void BindLbs(int lbs)
        {
            //DAL.t_wx_news_materials model = new DAL.t_wx_news_materials();
            var model = BLL.Chats.wx_lbs.GetLBSByID(lbs);
            if (model != null)
            {
                litTime.Text = Common.Utils.ObjectToDateTime(model.CreateTime).ToString("yyyy年MM月dd日", DateTimeFormatInfo.InvariantInfo);
                litTitle.Text = model.Title;
                litContent.Text = model.Contents;
                Title = model.Title;

                ShareContent = Utils.DropHTML(model.Contents, 40);

                KDWechat.DAL.t_wx_wechats wechat = KDWechat.BLL.Chats.wx_wechats.GetWeChatByID(model.wx_id);
                if (wechat != null)
                {
                    top1.wx_head_pic = wechat.header_pic;
                    top1.wx_name = wechat.wx_pb_name;

                    //添加浏览记录
                    KDWechat.BLL.Logs.wx_fans_hisview.CreateFansHisview(new DAL.t_wx_fans_hisview()
                    {
                        channel_name = "图文详细",
                        open_id = openId,
                        page_name = "图文详细",
                        page_url = Request.Url.ToString(),
                        view_time = DateTime.Now,
                        wx_id = wechat.id,
                        wx_og_id = wechat.wx_og_id
                    });
                }

            }
        }
        /// <summary>
        /// 绑定详细页数据
        /// </summary>
        private void BindData(int id)
        {

            DAL.t_wx_news_materials model = new DAL.t_wx_news_materials();
            model = BLL.Chats.wx_news_materials.GetModel(id);
            if (model != null)
            {
                litTime.Text = Common.Utils.ObjectToDateTime(model.create_time).ToString("yyyy年MM月dd日", DateTimeFormatInfo.InvariantInfo);
                litTitle.Text = model.title;
                litContent.Text = model.contents;
                Title = model.title;

                ShareContent = Utils.DropHTML(model.contents, 40);

                KDWechat.DAL.t_wx_wechats wechat = KDWechat.BLL.Chats.wx_wechats.GetWeChatByID(model.wx_id);
                if (wechat != null)
                {
                    top1.wx_head_pic = wechat.header_pic;
                    top1.wx_name = wechat.wx_pb_name;

                    //添加浏览记录
                    KDWechat.BLL.Logs.wx_fans_hisview.CreateFansHisview(new DAL.t_wx_fans_hisview()
                    {
                        channel_name="图文详细",
                        open_id=openId,
                        page_name = "图文详细",
                        page_url = Request.Url.ToString(),
                        view_time=DateTime.Now,
                        wx_id=wechat.id,
                        wx_og_id=wechat.wx_og_id
                    });
                }

            }


        }
    }
}