using BaiDuMapAPI.HttpUtility;
using KDWechat.BLL.Entity.JsonResult;
using KDWechat.Common;
using KDWechat.Common.Config;
using KDWechat.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KDWechat.Web.fans
{
    public partial class member_regist : System.Web.UI.Page
    {
        protected int wx_id = 0;
        protected wechatconfig wechatconfig = new BLL.Config.wechat_config().loadConfig();
        protected string wx_id_without_des = RequestHelper.GetQueryString("wx_id");
        protected string openID = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(wx_id_without_des))
                wx_id = Utils.StrToInt(DESEncrypt.Decrypt(wx_id_without_des), -1);
            openID = Utils.GetCookie("openID" + wx_id);
            if (wx_id > 0)
            {
                if (string.IsNullOrWhiteSpace(openID))
                {
                    Response.Redirect("member_bind.aspx?wx_id=" + wx_id_without_des);
                }
                else
                {
                    var relation = Companycn.Core.EntityFramework.EFHelper.GetModel<wechatEntities, t_member_fans_relation>(x => x.openid == openID);
                    if (relation != null)
                    {
                        Response.Write("<script>alert('您绑定过会员，请勿重复操作！');location.href='member_bind_result.aspx?msg=您已绑定过会员，请勿重复操作';</script>");
                        Response.End();
                    }
                }
            }
            else
            {
                Response.Write("<script>alert('请勿非法进入！');</script>");
                Response.End();
            }
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string location = "member_bind_result.aspx";
            var ip = Utils.GetUserIp();
            var apiUrl = System.Configuration.ConfigurationManager.AppSettings["KDMemberUrl"];
            var url = apiUrl + "&Mt=creatM";
            t_member member = new t_member();
            member.nick_name = "";
            member.phone = Utils.DropHTML(txtPhone.Value);
            member.pwd = Utils.DropHTML(txtPassword.Value);
            member.add_time = DateTime.Now;
            member.login_time = DateTime.Now;
            member.login_ip = Utils.GetUserIp();
            member.status = 1;
            member.m_from = 23;
            var data = JsonConvert.SerializeObject(member);
            byte[] dataArray = Encoding.UTF8.GetBytes(data);
            var ms = new MemoryStream(dataArray);
            var returnMember = BaiDuMapAPI.HttpUtility.RequestUtility.HttpPost(url, ms);

            var result = JsonApi.GetJsonResult<JsonDataResult<t_member>>(returnMember);
            if (result is JsonDataResult<t_member>)
            {
                var newMember = (JsonDataResult<t_member>)result;


                var relation = Companycn.Core.EntityFramework.EFHelper.GetModel<wechatEntities, t_member_fans_relation>(x => x.openid == openID);
                if (relation != null)
                {
                    Response.Write("<script>alert('您绑定过会员，请勿重复操作！');location.href='" + location + "?msg=您已绑定过会员，请勿重复操作';</script>");
                    Response.End();
                }
                else
                {
                    var fans = Companycn.Core.EntityFramework.EFHelper.GetModel<wechatEntities, t_wx_fans>(x => x.open_id == openID);
                    if (fans != null)
                    {
                        t_member_fans_relation mods = new t_member_fans_relation
                        {
                            fans_id = fans.id,
                            member_id = newMember.data.id,
                            openid = openID,
                            wx_id = fans.wx_id,
                            wx_og_id = fans.wx_og_id
                        };
                        Companycn.Core.EntityFramework.EFHelper.AddModel<wechatEntities, t_member_fans_relation>(mods);
                        Response.Write("<script>alert('会员注册并绑定成功！');location.href='" + location + "?msg=绑定成功';</script>");
                    }
                    else
                    {
                        Response.Write("<script>alert('尚未关注本微信号，请重新关注！');location.href='" + location + "?msg=请重新关注';</script>");
                        Response.End();
                    }
                }
            }
            else
                Response.Write("<script>alert('"+result.msg+"');</script>");
        }




    }
}