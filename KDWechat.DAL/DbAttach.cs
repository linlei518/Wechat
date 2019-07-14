using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Linq;
using System.Linq;
using System.Text;
using EntityFramework.Extensions;


namespace KDWechat.DAL
{
    class DbAttach
    {
        //entityframework 提取列表并分页示例
        public static List<t_wx_wechats> GetChatListByPage(int pagesize, int pageIndex)
        {
            List<t_wx_wechats> list = null;
            //所有entity，都需要最后进行dispose,如果不想手动释放，可以采用using
            using (creater_wxEntities db = new creater_wxEntities())
            {
                //一般来说 linq 的形式是 from x(临时变量,可随意起名) in db.xxx(表名) where x.xxx(属性名，字段名)=xxx &&  || x.xxx>=xxx select x
                list = (from x in db.t_wx_wechats where x.city == "上海" && x.country == "中国" select x).Skip((pageIndex - 1) * pagesize).Take(pagesize).ToList();
                //skip方法决定跳过多少条，take方法决定取多少条，最后如果是列表，一定要ToList()
    

                //如果不习惯linq写法，下面的lambda表达式等同于上面的LINQ
                list = db.t_wx_wechats.Where(x => x.city == "上海" && x.country == "中国").Skip((pageIndex - 1) * pagesize).Take(pagesize).ToList();


                //如果未采用using,释放链接操作应该如此来写
                db.Dispose();
            }

            
            return list;
        }


        //entityframework 提取单条 示例
        public static t_wx_wechats GetwxChatsByID(int id)
        {
            t_wx_wechats chat = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                chat = (from x in db.t_wx_wechats where x.id == id select x).FirstOrDefault();
                //如果不确定是否存在本条信息，可使用如上的firstordefault方法，如果确定存在此条数据，可直接使用first()
            }
            return chat;
        }

        //entityframework 修改单条最易懂示例
        public static bool UpdateWxChatUsual(t_wx_wechats chat)
        {
            //是否已完成修改的标记字段
            bool isFinish = false;

            using (creater_wxEntities db = new creater_wxEntities())
            {
                var chatToUpd = (from x in db.t_wx_wechats where x.id == chat.id select x).FirstOrDefault();
                if (chatToUpd != null)
                {
                    chatToUpd.account_name = chat.account_name;
                    chatToUpd.account_pwd = chat.account_pwd;
                    chatToUpd.api_url = chat.api_url;
                    chatToUpd.app_id = chat.app_id;
                    chatToUpd.app_secret = chat.app_secret;
                    chatToUpd.city = chat.city;
                    chatToUpd.country = chat.country;
                    //chatToUpd.create_time = DateTime.Now;
                    chatToUpd.fans = chat.fans;
                    chatToUpd.header_pic = chatToUpd.header_pic;
                    chatToUpd.modify_time = DateTime.Now;
                    chatToUpd.province = chat.province;
                    chatToUpd.template_id = chat.template_id;
                    chatToUpd.token = chat.token;
                    //字段实在太多。。懒得写了，小数量的数据可以这么写，大数量数据更新的正确方法请见下面

                    

                    //最后别忘记savingchanges,否则数据无法保存
                    isFinish = db.SaveChanges()>0;
                }
            }
            return isFinish;
        }


        //entityframework 修改单条进阶示例
        public static bool UpdateWxChats(t_wx_wechats chat)
        {
            //是否已完成修改的标记字段
            bool isFinish = false;

            using (creater_wxEntities db = new creater_wxEntities())
            {
                //此处先进行attach目的是将实体附加到上下文，既建立新建的对象与数据库上下文之间的联系
                db.t_wx_wechats.Attach(chat);

                //此处首先进入chat,将它的状态标为已修改。
                db.Entry(chat).State = EntityState.Modified;

                //update,insert,delete操作之后一定要进行savingChanges(),此方法会执行前面所有修改的语句集合，并返回受影响行数
                isFinish = db.SaveChanges() > 0;
            }
            return isFinish;
        }

        //entityframework 删除单条 示例
        public static bool DeleteWxChatsByID(int id)
        {
            //是否已完成修改的标记字段
            bool isFinish = false;

            using (creater_wxEntities db = new creater_wxEntities())
            {
                //删除操作一般不采用attach方式，直接查找，remove即可
                var chatToDelete = (from x in db.t_wx_wechats where x.id == id select x).FirstOrDefault();
                if (null != chatToDelete)
                {
                    //remove后，被remove的实体被标记为deleted，删除多条可以手动标为Deleted
                    db.t_wx_wechats.Remove(chatToDelete);
                    isFinish = db.SaveChanges() > 0;
                }
            }
            return isFinish;
        }

        //entityframework 删除多条 示例
        public static bool DeleteWxChatsByIDs(int[] ids)
        {
            //是否已完成修改的标记字段
            bool isFinish = false;

            using (creater_wxEntities db = new creater_wxEntities())
            {
                //多条可以采用contains，会转换为sql的in操作
                var chatListToDelete = (from x in db.t_wx_wechats where ids.Contains(x.id) select x).ToList();

                foreach (var x in chatListToDelete)
                {
                    //标记状态为deleted
                    db.Entry(x).State = EntityState.Deleted;
                }

                isFinish = db.SaveChanges() > 0;
            }
            return isFinish;
        }

        //entityframework 删除多条 进阶 示例
        public static bool DeleteWxChatByIDs(int[] ids)
        {
            //是否已完成修改的标记字段
            bool isFinish = false;

            using (creater_wxEntities db = new creater_wxEntities())
            {
                //EntityFramework.Extended中的方法，此方法不需要savingchange,但要引用entityframework.extension命名空间
                isFinish = db.t_wx_wechats.Where(x => ids.Contains(x.id)).Delete() > 0;
            }
            return isFinish;
        }


        //entityframework 添加 示例
        public static bool AddWxChats(t_wx_wechats chat)
        {           
            bool isFinish = false;

            using (creater_wxEntities db = new creater_wxEntities())
            {
                //添加时直接对对象进行ADD操作即可
                db.t_wx_wechats.Add(chat);
                isFinish = db.SaveChanges() > 0;
            }
            return isFinish;
        }

        
    }
}
