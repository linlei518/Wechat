using KDWechat.BLL.Entity;
using KDWechat.BLL.Entity.JsonResult;
using KDWechat.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;

namespace KDWechat.Web.api
{
    /// <summary>
    /// Member 的摘要说明
    /// </summary>
    public class Member : IHttpHandler
    {

        HttpRequest Request;
        HttpResponse Response;
        string streamContent;


        public void ProcessRequest(HttpContext context)
        {
            context.Response.Clear();
            context.Response.ClearContent();
            context.Response.ClearHeaders();
            //禁止缓存
            context.Response.Expires = -1;//相对过期时间
            context.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);//绝对过期时间
            context.Response.CacheControl = "no-cache";
            context.Response.AddHeader("pragma", "no-cache");
            context.Response.AddHeader("cache-control", "private");
            context.Response.ContentType = "application/json";

            Request = context.Request;
            Response = context.Response;
            var method = Request.QueryString["mt"];



            if (string.IsNullOrWhiteSpace(method))
                WriteError("参数错误！");

            if (Request.InputStream.Length > 0)
            {
                byte[] byts = new byte[Request.InputStream.Length];//根据流长度新建byte数组
                Request.InputStream.Read(byts, 0, byts.Length);//把流读入byte数组
                streamContent = Encoding.UTF8.GetString(byts);
            }
            //else
                //WriteError("请不要提交空数据！");

            switch (method)
            {
                case "creatM":
                    CreateMember();
                    break;
                case "alterM":
                    UpdateMember();
                    break;
                case "loginM":
                    LoginMember();
                    break;
                case "forgetP":
                    ForgetPassword();
                    break;
                case "changeP":
                    ChangePassword();
                    break;
                default:
                    WriteError("方法未找到");
                    break;
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        private void ChangePassword()
        {
            var oldPwd = Request.QueryString["uOldPwd"];
            if (!string.IsNullOrWhiteSpace(oldPwd))
                ChangePasswordByOldPwd();

            var uTel = Request.QueryString["uTel"];
            var uPwd = Request.QueryString["uPwd"];
            var ticket = Request.QueryString["uKey"];
            
            if (!string.IsNullOrWhiteSpace(uTel) && !string.IsNullOrWhiteSpace(uPwd) && !string.IsNullOrWhiteSpace(ticket))
            {
                var key = Companycn.Core.EntityFramework.EFHelper.GetModel<kd_usersEntities, member_forget_password_View>(x => x.phone == uTel);
                if (key != null)
                {
                    if (key.change_key != ticket)
                        WriteError("验证码错误");
                    var newSalt = Common.Utils.CreateSalt();
                    var pwd = Common.Utils.CreatePassword(uPwd, newSalt);                    
                    if (Companycn.Core.EntityFramework.EFHelper.UpdateModel<kd_usersEntities, t_member>(x => x.phone == uTel, x => new t_member { salt = newSalt, pwd = pwd }) > 0)
                    {
                        Companycn.Core.EntityFramework.EFHelper.DeleteModel<kd_usersEntities, t_member_forget_password>(x => x.member_id == key.member_id);
                        WriteSuccess("密码修改成功");
                    }
                    else
                        WriteError("数据库错误");
                }
                else
                    WriteError("验证码失效");

            }
            else
                WriteError("参数错误");
        }

        private void ChangePasswordByOldPwd()
        {
            var uTel = Request.QueryString["uTel"];
            var uPwd = Request.QueryString["uPwd"];
            var oldPwd = Request.QueryString["uOldPwd"];
            if (!string.IsNullOrWhiteSpace(uTel) && !string.IsNullOrWhiteSpace(uPwd) && !string.IsNullOrWhiteSpace(oldPwd))
            {
                var member = BLL.Users.member.UserLogin(uTel, oldPwd);
                if (member != null)
                {
                    var nePwd = Common.Utils.CreatePassword(uPwd, member.salt);
                    member.pwd = nePwd;
                    if (Companycn.Core.EntityFramework.EFHelper.UpdateModel<kd_usersEntities, t_member>(member))
                    {
                        WriteSuccess("密码修改成功");
                    }
                    else
                        WriteError("数据库更新失败");
                    //WriteJsonResult<t_member>("登录成功", member);
                }
                else
                    WriteError("用户名密码错误");
            }
            else
                WriteError("参数错误");

        }


        /// <summary>
        /// 忘记密码
        /// </summary>
        private void ForgetPassword()
        {
            var uTel = Request.QueryString["uTel"];
            if (!string.IsNullOrWhiteSpace(uTel))
            {
                var key = Companycn.Core.EntityFramework.EFHelper.GetModel<kd_usersEntities, member_forget_password_View>(x => x.phone == uTel);
                if (key != null)
                {
                    if (DateTime.Now - key.creat_time <= TimeSpan.FromSeconds(60))
                    {
                        WriteError("操作过于频繁");
                    }
                    WriteJsonResult<string>("验证码重新发送成功", key.change_key);

                    //重发key
                }
                else
                {
                    var ticket = Common.Utils.Number(6).ToString();
                    var member = Companycn.Core.EntityFramework.EFHelper.GetModel<kd_usersEntities, t_member>(x => x.phone == uTel);
                    if(member!=null)
                    {
                        Companycn.Core.EntityFramework.EFHelper.AddModel<kd_usersEntities, t_member_forget_password>(new t_member_forget_password { is_send = 1, change_key = ticket, creat_time = DateTime.Now, g_method = 1, member_id = member.id });
                        //发key
                        WriteJsonResult<string>("验证码发送成功",ticket);
                    }
                    else
                        WriteError("不存在此用户");
                }
            }
            else
                WriteError("参数错误");
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        private void LoginMember()
        {
            var uTel = Request.QueryString["uTel"];
            var uPwd = Request.QueryString["uPwd"];
            var uIP = Request.QueryString["uIP"];
            if (!string.IsNullOrWhiteSpace(uTel) && !string.IsNullOrWhiteSpace(uPwd) && !string.IsNullOrWhiteSpace(uIP))
            {
                var member = BLL.Users.member.UserLogin(uTel, uPwd, uIP);
                if (member != null)
                    WriteJsonResult<t_member>("登录成功", member);
                else
                    WriteError("用户名密码错误");
            }
            else
                WriteError("参数错误");
        }
            
        /// <summary>
        /// 修改会员
        /// </summary>
        private void UpdateMember()
        {
            MemberForApi member = null;
            try
            {
                member = JsonConvert.DeserializeObject<MemberForApi>(streamContent);
            }
            catch
            {
                WriteError("数据格式错误！");
            }
            //t_member member =BLL.Users.member.UserLogin(newMember.phone, newMember.pwd);
            if (member != null)
            {
                var oldMember = Companycn.Core.EntityFramework.EFHelper.GetModel<kd_usersEntities, t_member>(x => x.phone == member.phone);
                if (oldMember == null)
                    WriteError("用户不存在");
                if (CheckString(member.e_mail))
                    oldMember.e_mail = member.e_mail;
                if (CheckString(member.nick_name))
                    oldMember.nick_name = member.nick_name;

                var memberProperty = Companycn.Core.EntityFramework.EFHelper.GetModel<kd_usersEntities, t_member_property>(x => x.member_id == oldMember.id);
                bool hasAdd = false;
                if (memberProperty == null)
                {
                    memberProperty = new t_member_property(){member_id=oldMember.id};
                    hasAdd = true;
                }
                if (CheckValueType(member.sex))
                    memberProperty.sex= member.sex;
                if (CheckString(member.nation))
                    memberProperty.nation = member.nation;
                if (CheckValueType(member.birth))
                    memberProperty.birth = member.birth;
                if (CheckValueType(member.ID_Type))
                    memberProperty.ID_Type = member.ID_Type;
                if(CheckString(member.ID_No))
                    memberProperty.ID_No = member.ID_No;

                if (hasAdd)
                    Companycn.Core.EntityFramework.EFHelper.AddModelBool<kd_usersEntities, t_member_property>(memberProperty);
                else
                    Companycn.Core.EntityFramework.EFHelper.UpdateModel<kd_usersEntities, t_member_property>(memberProperty);

                if (Companycn.Core.EntityFramework.EFHelper.UpdateModel<kd_usersEntities, t_member>(oldMember))
                    WriteJsonResult<MemberForApi>("会员修改成功！", member);
                else
                    WriteError("数据库服务错误！");
            }
            else
            {
                WriteError("用户名，密码错误！");
            }
        }

        /// <summary>
        /// 创建会员
        /// </summary>
        private void CreateMember()
        {
            t_member member = null;
            try
            {
                member = JsonConvert.DeserializeObject<DAL.t_member>(streamContent);
            }
            catch
            {
                WriteError("数据格式错误！");
            }
            var salt = Common.Utils.CreateSalt();
            var pwd = Common.Utils.CreatePassword(member.pwd, salt);
            member.salt = salt;
            member.pwd = pwd;
            member.login_time = DateTime.Now;
            member.add_time = DateTime.Now;
            var newMember = BLL.Users.member.AddMember(member);
            if (newMember.pwd != member.pwd)
                WriteError("用户手机号重复");
            WriteJsonResult<t_member>("会员创建成功！", member);
        }


        //输出一个值类型
        void WriteObj(ValueType obj)
        {
            Response.Write(obj);
            Response.End();
        }


        //返回成功实例（性能一般）
        void WriteJsonResult(object entity, string msg)
        {
            Response.Write(JsonConvert.SerializeObject(new { data = entity, msg = msg, result = 1 }));
            Response.End();
        }

        //返回成功实例（泛型，性能较好）
        void WriteJsonResult<T>(string msg, T entity)
        {
            Response.Write(JsonConvert.SerializeObject(new JsonDataResult<T> { data = entity, msg = msg, result = 1 }));
            Response.End();
        }

        //返回成功信息
        void WriteSuccess(string successMsg)
        {
            Response.Write(JsonConvert.SerializeObject(new JsonErrorResult { msg = successMsg, result = 1 }));
            Response.End();
        }

        //返回错误
        void WriteError(string errorMsg)
        {
            Response.Write(JsonConvert.SerializeObject(new JsonErrorResult { msg = errorMsg, result = 0 }));
            Response.End();
        }

        bool CheckValueType(ValueType valueToCheck)
        {
            
            if (valueToCheck is int || valueToCheck is float || valueToCheck is Double || valueToCheck is decimal)
                return valueToCheck.ToString() != "0";
            else if (valueToCheck is int? || valueToCheck is float? || valueToCheck is Double? || valueToCheck is decimal?)
                return (valueToCheck ?? 0).ToString() != "0";
            else
                return valueToCheck != null;
        }

        //检测是否为空
        bool CheckString(string strToCheck)
        {
            return !string.IsNullOrWhiteSpace(strToCheck);
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}