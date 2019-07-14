using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;

namespace KDWechat.Web.wxpage
{
    public partial class audio_detail : System.Web.UI.Page
    {
         
        protected string openId { get { return RequestHelper.GetQueryString("openId"); } }
        protected int id { get { return RequestHelper.GetQueryInt("id", 0); } }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (id > 0)
            {
                BindData(id);
            }

        }
        /// <summary>
        /// 绑定详细页数据
        /// </summary>
        private void BindData(int id)
        {

            DAL.t_wx_media_materials model = new DAL.t_wx_media_materials();
            model = BLL.Chats.wx_media_materials.GetMediaMaterialByID(id);
            if (model != null)
            {
                lblTitle.Text = model.title;
                lblAudio.Text ="<source src=\""+model.file_url+"\" type=\"audio/mp3\">";
                Title = model.title;

                KDWechat.DAL.t_wx_wechats wechat = KDWechat.BLL.Chats.wx_wechats.GetWeChatByID(model.wx_id);
                if (wechat != null)
                {
                    top1.wx_head_pic = wechat.header_pic;
                    top1.wx_name = wechat.wx_pb_name;

                    //添加浏览记录
                    KDWechat.BLL.Logs.wx_fans_hisview.CreateFansHisview(new DAL.t_wx_fans_hisview()
                    {
                        channel_name = "音频详细",
                        open_id = openId,
                        page_name = "音频详细",
                        page_url = Request.Url.ToString(),
                        view_time = DateTime.Now,
                        wx_id = wechat.id,
                        wx_og_id = wechat.wx_og_id,
                        channel_id = (int)TemplateType.微信,
                        news_id = id,
                        type_id = (int)HistoryViewType.浏览数
                    });
                }

            }


        }
    }
}