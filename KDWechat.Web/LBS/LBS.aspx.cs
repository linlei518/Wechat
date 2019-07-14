using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiDuMapAPI;
using Newtonsoft.Json;
using KDWechat.Web.UI;
using KDWechat.DAL;
using KDWechat.BLL;
using KDWechat.Common;
using KDWechat.BLL.Chats;

namespace KDWechat.Web
{
    public partial class LBS : BasePage 
    {
        protected string load_script = ""; //页面加载时的JS

        protected int id
        {
            get { return RequestHelper.GetQueryInt("id", 0); }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckUserAuthority("wechat_lbsset"); //检测权限
                radStatusFalse.Checked = true;
                if (id != 0)
                    InitData();  //初始化数据
            }
        }

        private void InitData()
        {
            var poi = BLL.Chats.wx_lbs.GetLBSByID(id);
            if (null != poi)
            {
                AddressText.Text = poi.Address;
                txtContent.Value = poi.Contents;
                txtFile.Text = poi.ImgUrl;
                TitleText.Text = poi.Title;
                TitleText.Text = poi.tags;
                txtWUrl.Text = poi.w_url;
                img_show.Src = poi.ImgUrl;
                load_script += string.Format("new PCAS(\"Province\",\"City\",\"Area\",\"{0}\",\"{1}\",\"{2}\");loca2();", poi.Province, poi.City, poi.County);
                if (poi.is_top == 1)
                {
                    radStatusOk.Checked = true;
                    radStatusFalse.Checked = false;
                }
                //上面的语句是用来提取省市地址并定位用的，这里有待改进，可直接读取经纬度来进行操作
            }
        }



        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (id == 0)//id为0时插入，否则更新
                AddLbs();
            else
                UpdateLbs(id);
        }

        private void UpdateLbs(int id)
        {
            string lat = Request.Form["lat"].ToString();
            string lng = Request.Form["lng"].ToString();
            string ImgUrl = Request.Form["txtFile"].ToString();
            t_wx_lbs location = BLL.Chats.wx_lbs.GetLBSByID(id);
            var wechat = wx_wechats.GetWeChatByID(wx_id);
            string alertText = "";
            if (null != wechat)
            {
                //下面的语句调用了百度接口，进行了云存储的数据更新
                string jsonResult = BaiDuLBS.UpdatePoint(location.baidu_id.ToString(), AddressText.Text, txtContent.Value, TitleText.Text, lat, lng, TitleText.Text, ImgUrl, wechat.wx_og_id, txtWUrl.Text, id.ToString(), radStatusOk.Checked ? "1" : "0");
                var result = JsonConvert.DeserializeObject<StatusResultBase>(jsonResult);//newtownsoft的json逆解析
                if (result.status == 0)
                {
                    location.Address = AddressText.Text;
                    location.City = Request["City"].ToString();
                    location.Contents = Utils.Filter(txtContent.Value.Trim());
                    location.County = Request["Area"].ToString();
                    location.CreateTime = DateTime.Now;
                    location.ImgUrl = ImgUrl;
                    location.Province = Request["Province"].ToString();
                    location.Title = TitleText.Text;
                    location.u_id = u_id;
                    location.wx_id = wx_id;
                    location.wx_og_id = wx_og_id;
                    location.tags = TitleText.Text;
                    location.lat = decimal.Parse(lat);
                    location.lng = decimal.Parse(lng);
                    location.poi_id = (int)result.id;
                    location.w_url = txtWUrl.Text;
                    if (radStatusOk.Checked)
                        location.is_top = 1;
                    KDWechat.BLL.Chats.wx_lbs.UpdateLbs(location);
                    AddLog(string.Format("更新了位置信息：“{1}”", id.ToString(),location.Title), LogType.修改);
                }
                alertText = result.status == 0 ? "LBS位置修改成功" : "LBS位置修改失败";
            }
            JsHelper.AlertAndRedirect(alertText,"lbslist.aspx?m_id=69");
        }

        private void AddLbs()
        {
            string addressText = Request.Form["Province"].ToString() + Request.Form["City"].ToString() + Request.Form["Area"].ToString() + AddressText.Text;
            string lat = Request.Form["lat"].ToString();
            string lng = Request.Form["lng"].ToString();
            string ImgUrl = Request.Form["txtFile"].ToString();
            var wechat = wx_wechats.GetWeChatByID(wx_id);
            string ogid = "";
            if (wechat != null)
                ogid = wechat.wx_og_id;
            //下面的语句调用了百度云存储
            string jsonResult = BaiDuLBS.CreatePoint(addressText, txtContent.Value, TitleText.Text, lat, lng, TitleText.Text, ImgUrl, ogid,txtWUrl.Text,radStatusOk.Checked?"1":"0");
            var result = JsonConvert.DeserializeObject<StatusResultBase>(jsonResult);
            if (result!=null&&result.status == 0)//判断云存储是否成功
            {
                t_wx_lbs location = new t_wx_lbs()
                {
                    Address = AddressText.Text,
                    City = Request["City"].ToString(),
                    Contents = Utils.Filter(txtContent.Value.Trim()),
                    County = Request["Area"].ToString(),
                    CreateTime = DateTime.Now,
                    ImgUrl = ImgUrl,
                    Province = Request["Province"].ToString(),
                    Title = TitleText.Text,
                    u_id = u_id,
                    wx_id = wx_id,
                    wx_og_id = ogid,
                    tags = TitleText.Text,
                    lat = decimal.Parse(lat),
                    lng = decimal.Parse(lng),
                    poi_id = (int)result.id,
                    baidu_id = result.id ?? 0,
                    w_url = txtWUrl.Text
                };
                if (radStatusOk.Checked)
                    location.is_top = 1;
                location = KDWechat.BLL.Chats.wx_lbs.InsertLbs(location);
                BaiDuLBS.UpdatePoint(location.baidu_id.ToString(), AddressText.Text, txtContent.Value, TitleText.Text, lat, lng, TitleText.Text, ImgUrl, wechat.wx_og_id, txtWUrl.Text, location.ID.ToString(), radStatusOk.Checked ? "1" : "0");
                AddLog("添加LBS位置信息:"+location.Title,LogType.添加);
            }
            string alertText = result.status == 0 ? "LBS位置添加成功" : "LBS位置添加失败";
            JsHelper.AlertAndRedirect(alertText, "lbslist.aspx?m_id=69");

        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("lbslist.aspx?m_id=69");//取消
        }
    }
}