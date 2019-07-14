using KDWechat.Common;
using KDWechat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDWechat.BLL.Users
{
    public class member
    {
        /// <summary>
        /// 添加会员
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static t_member AddMember(t_member member)
        {
            using (var transaction = new System.Transactions.TransactionScope())
            {
                using (kd_usersEntities db = new kd_usersEntities())
                {
                    var searMember = db.t_member.Where(x => x.phone == member.phone).FirstOrDefault();
                    if (searMember != null)
                        member = searMember;
                    else
                    {
                        db.t_member.Add(member);
                        db.SaveChanges();
                    }
                }
                transaction.Complete();
            }
            return member;
        }

        /// <summary>
        /// 会员登陆
        /// </summary>
        /// <param name="username">会员名</param>
        /// <param name="password">会员密码</param>
        /// <returns>登陆成功则返回对应会员，否则返回空</returns>
        public static t_member UserLogin(string tel, string password,string loginIp="")
        {
            t_member user = null;
            using (kd_usersEntities db = new kd_usersEntities())
            {
                user = (from x in db.t_member where x.phone == tel select x).FirstOrDefault();
                if (null == user || Utils.CreatePassword(password, user.salt) != user.pwd)
                    user = null;
                else
                {
                    if(loginIp!="")
                        user.login_ip = loginIp;
                    user.login_time = DateTime.Now;
                    db.SaveChanges();
                }
            }
            return user;
        }
    }
}
