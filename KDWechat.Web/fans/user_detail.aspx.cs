using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.BLL.Users;
using KDWechat.Common;
using KDWechat.DAL;
using Senparc.Weixin.MP.CommonAPIs;

namespace KDWechat.Web.fans
{
    public partial class user_detail : KDWechat.Web.UI.BasePage
    {
        protected double lng = 0;
        protected double lat = 0;

        protected string _disable = "";

        protected string is_wuye = "0";
        /// <summary>
        /// 选择的城市
        /// </summary>
        protected string selectCity = string.Empty;
        /// <summary>
        /// 分组数据
        /// </summary>
        protected string groupList = string.Empty;
        /// <summary>
        /// 标签数据
        /// </summary>
        protected string tagList = string.Empty;
        /// <summary>
        /// 粉丝用户的id
        /// </summary>
        protected int id { get { return Common.RequestHelper.GetQueryInt("id", 0); } }

        protected string openId { get { return Common.RequestHelper.GetQueryString("openId"); } }

        protected string showMsg = string.Empty;

        /// <summary>
        /// 用户所属的公众号id
        /// </summary>
        protected int _wx_id { get { return Common.RequestHelper.GetQueryInt("wx_id", 0) == 0 ? wx_id : Common.RequestHelper.GetQueryInt("wx_id", 0); } }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                string role = "wechat_fans_user_list_hq";
                string page = "all_user_list.aspx?m_id=" + m_id;
                if (u_type == 2 || u_type == 3)
                {
                    role = "wechat_fans_user_list";
                    page = "user_list.aspx?m_id=" + m_id;
                }
                CheckUserAuthority(role);
               
                hfReturnUrl.Value = page;
                KDWechat.DAL.t_wx_fans model = null;
                if (id > 0)
                {
                    model = KDWechat.BLL.Users.wx_fans.GetFansByID(id);
                }
                else if (!string.IsNullOrEmpty(openId))
                {
                    model = KDWechat.BLL.Users.wx_fans.GetFansByID(openId);
                }

                if (model != null)
                {
                    ShowPage(model);
                }
                else
                {
                    Response.Redirect(hfReturnUrl.Value);
                }
            }
        }

        private void ShowPage(DAL.t_wx_fans model)
        {
            if (u_type == 2 || u_type == 3)
            {
                if (_wx_id == 0)
                {
                    JsHelper.AlertAndRedirect("请先选择一个公众号", "/Account/region_wxlist.aspx?m_id=59", "fail");
                    return;
                }
                else if (_wx_id != model.wx_id)
                {
                    JsHelper.AlertAndRedirect("该用户不是您的粉丝，您无权查看", hfReturnUrl.Value, "fail");
                    return;
                }
            }


  

            #region 加载关注的公众号
            string wechat_string = "";// Common.CacheHelper.Get("wechat_string") as string;
            //if (wechat_string==null)
            //{
            wechat_string = KDWechat.BLL.Chats.wx_wechats.GetNameList(model.wx_id, model.unionid);
            //    Common.CacheHelper.Insert("wechat_string", wechat_string, 2);
            //}
            lblwechats.Text = wechat_string;

            #endregion

            #region 微信资料
            lblcounty.Text = model.wx_country;
            lblcity.Text = model.wx_province + "-" + model.wx_city;
            switch (model.wx_sex)
            {
                case 1:
                    lblSex.Text = "男";
                    break;
                case 2:
                    lblSex.Text = "女";
                    break;
                default:
                    lblSex.Text = "未知";
                    break;
            }

            if (model.language.ToLower().Contains("cn"))
            {
                lbllanuage.Text = "中文";
            }
            else if (model.language.ToLower().Contains("en"))
            {
                lbllanuage.Text = "英文";
            }
            else
            {
                lbllanuage.Text = "未知";
            }
            #endregion

            if (model.source_id == 0)
            {
                lblsource.Text = "通过搜索关注";
            }
            else
            {
                #region 之前读取项目表，现在不用了，直接读取qr_code表
                //foreach (DataRow m in dt_projects.Rows)
                //{
                //    if (Common.Utils.StrToInt(m["id"].ToString(), 0) == model.source_id)
                //    {
                //        lblsource.Text = m["title"].ToString();
                //        break;
                //    }
                //} 
                #endregion
                DAL.t_wx_qrcode qr = BLL.Chats.wx_qrcode.GetModel<int>(x => x.souce_id == model.source_id && x.wx_id==model.wx_id, x =>x.id);
                if (qr!=null)
                {
                    lblsource.Text = qr.q_name;
                }
            }

   

            showMsg = "user_msg_list.aspx?openId=" + model.open_id + "&m_id=" + m_id;

            string member_name = KDWechat.BLL.Users.wx_fans.GetMemberName(model.unionid);
            if (!string.IsNullOrEmpty(member_name))
            {
                lblMemberName.Text += " 注册会员 ：" + member_name;
            }





            hfopenid.Value = model.open_id;
            if (!string.IsNullOrEmpty(model.headimgurl))
            {
                img_head.Src = model.headimgurl;
            }
            else
            {
                img_head.Src = "../images/logo_01.png";
            }

            lblNickName.Text = model.nick_name;
              

            #region 加载已选的分组和标签
            if (model.group_id > 0)
            {
                lblGroupName.Text = KDWechat.BLL.Users.wx_group_tags.GetGroupName(model.group_id);
            }
            else
            {
                lblGroupName.Text = "默认分组";
            }

            List<string> list_tags = KDWechat.BLL.Users.wx_fans_tags.GetTagListByFansid(model.guid);
            if (list_tags != null)
            {
                foreach (string tag in list_tags)
                {
                    lblFansTags.Text += "<span class=\"tag\">" + tag + "</span>";
                }
            }
            if (list_tags.Count == 0)
            {
                btnModifyTag.Value = "添加标签";
            }
            #endregion

         

            #region 加载所有标签和分组
            List<KDWechat.DAL.t_wx_group_tags> list_group = KDWechat.BLL.Users.wx_group_tags.GetListByChannelId((int)channel_idType.关注用户分组, _wx_id);
            groupList += "[{id:'0',name:'默认分组'}";
            if (list_group != null)
            {
                if (list_group.Count > 0)
                {
                    groupList += ",";
                    int i = 1;
                    foreach (var item in list_group)
                    {
                        groupList += "{id:'" + item.id + "',name:'" + item.title + "'}";
                        if (i < list_group.Count)
                        {
                            groupList += ",";
                        }
                        i++;
                    }

                }

            }
            groupList += "]";

            List<KDWechat.DAL.t_wx_group_tags> list_tag = KDWechat.BLL.Users.wx_group_tags.GetListByChannelId((int)channel_idType.关注用户标签, _wx_id);
            if (list_tag != null)
            {
                if (list_tag.Count > 0)
                {
                    tagList += "[";
                    int i = 1;
                    foreach (var item in list_tag)
                    {
                        tagList += "{id:'" + item.id + "',name:'" + item.title + "'}";
                        if (i < list_tag.Count)
                        {
                            tagList += ",";
                        }
                        i++;
                    }
                    tagList += "]";
                }

            }
            #endregion

           

            #region 计算粉丝的聊天状态
            //DateTime lastTime = DateTime.Now.AddDays(-7); -----Damos
            var fcs = KDWechat.BLL.Users.wx_fans.GetFansChatStatus(model.last_interact_time, model.reply_state);//.GetFansChatStatus(model.open_id, ref lastTime);
            lblReplyStatus.Text = fcs.ToString();
            if (fcs != FansChatsTypeNew.暂无)
            {
                lblLastMsgTime.Text = "最后互动 ：" + model.last_interact_time.ToString();//lastTime.ToString();
            }
            #endregion

            if (u_type == 2 || u_type == 3)
            {
                if (_wx_id == 0)
                {
                    JsHelper.AlertAndRedirect("请先选择一个公众号", "/Account/region_wxlist.aspx?m_id=59", "fail");
                    return;
                }
                else if (_wx_id != model.wx_id)
                {
                    JsHelper.AlertAndRedirect("该用户不是您的粉丝，您无权查看", hfReturnUrl.Value,"fail");
                    return;
                }
            }
            else
            {
                if (wx_id==0 || m_id==86)
                {
                    divShowMsg.Visible = false;
                    btnCancel.Visible = false;
                    btnModifyGroup.Visible = false;
                    btnModifyTag.Visible = false;
                    btnsynchro.Visible = false;
                  
                }
               
            }

        }

     
      

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(hfReturnUrl.Value);
        }

        protected void btnLock_Click(object sender, EventArgs e)
        {

        }

        //同步微信基本信息
        protected void btnsynchro_Click(object sender, EventArgs e)
        {
            t_wx_wechats wechat = KDWechat.BLL.Chats.wx_wechats.GetWeChatByID(wx_id);
            if (wechat != null)
            {
                string accesstoken = BLL.Chats.wx_wechats.GetAccessToken(wechat.id, wechat);//  AccessTokenContainer.TryGetToken(wechat.app_id, wechat.app_secret);
                if (!accesstoken.Contains("Error:"))
                {
                    try
                    {
                        var user = Senparc.Weixin.MP.AdvancedAPIs.User.Info(accesstoken, hfopenid.Value);
                        if (user != null)
                        {
                            KDWechat.DAL.t_wx_fans model = KDWechat.BLL.Users.wx_fans.GetFansByID(id);
                            if (model != null)
                            {
                                model.wx_country = user.country;
                                model.wx_city = user.city;
                                model.wx_sex = user.sex;
                                model.wx_province = user.province;
                                model.nick_name = user.nickname;
                                model.language = user.language;
                                model.headimgurl = user.headimgurl;
                                KDWechat.BLL.Users.wx_fans.UpdateFans(model);
                                //JsHelper.Alert("同步成功");
                                AddLog("同步粉丝数据，粉丝昵称：" + lblNickName.Text, LogType.修改);
                                // JsHelper.RegisterScriptBlock(this.Page, "showTip.show('同步成功', false);");
                                JsHelper.AlertAndRedirect("同步成功", "user_detail.aspx?m_id=" + m_id + "&id=" + id + (Common.RequestHelper.GetQueryInt("wx_id", 0) > 0 ? "&wx_id=" + wx_id + "" : ""));
                                //string temp = "";
                                //if (!string.IsNullOrEmpty(model.province))
                                //    temp += "\"" + model.province + "\"";
                                //else
                                //    temp += "\"选择省份\"";
                                //if (!string.IsNullOrEmpty(model.city))
                                //    temp += ",\"" + model.city + "\"";
                                //else
                                //    temp += ",\"选择城市\"";
                                //if (!string.IsNullOrEmpty(model.area))
                                //    temp += ",\"" + model.area + "\"";
                                //else
                                //    temp += ",\"选择地区\"";

                                //selectCity = "new PCAS(\"Province\", \"City\", \"Area\", " + temp + ")";
                            }
                            else
                            {
                                JsHelper.AlertAndRedirect("数据已不存在", hfReturnUrl.Value);
                            }
                        }
                        else
                        {
                            JsHelper.AlertAndRedirect("微信服务器繁忙，请稍后再试。", "user_detail.aspx?m_id=" + m_id + "&id=" + id + (Common.RequestHelper.GetQueryInt("wx_id", 0) > 0 ? "&wx_id=" + wx_id + "" : ""), "fail");
                        }
                    }
                    catch (Senparc.Weixin.Exceptions.ErrorJsonResultException ex)
                    {
                       

                        JsHelper.AlertAndRedirect("微信服务器繁忙，请稍后再试。", "user_detail.aspx?m_id=" + m_id + "&id=" + id + (Common.RequestHelper.GetQueryInt("wx_id", 0) > 0 ? "&wx_id=" + wx_id + "" : ""), "fail");
                    }
                }
                else
                {
                    JsHelper.AlertAndRedirect(accesstoken.Replace("Error:", ""), "user_detail.aspx?m_id=" + m_id + "&id=" + id + (Common.RequestHelper.GetQueryInt("wx_id", 0) > 0 ? "&wx_id=" + wx_id + "" : ""), "fail");
                }
            }
            else
            {
                JsHelper.AlertAndRedirect("暂无此微信号", "user_detail.aspx?m_id=" + m_id + "&id=" + id + (Common.RequestHelper.GetQueryInt("wx_id", 0) > 0 ? "&wx_id=" + wx_id + "" : ""), "fail");
            }


        }
    }
}