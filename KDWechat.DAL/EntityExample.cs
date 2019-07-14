using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityFramework.Extensions;

namespace KDWechat.DAL
{
    class EntityExample
    {
        //删除对应ID的wechat
        public static bool DeleteWeChatByID(int id)
        {
            bool isComplete = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                //使用Where函数首先查出对应ID的对象，但并不实例化，紧接着Delete,就完成了删除操作
                isComplete = db.t_wx_wechats.Where(x => x.id == id).Delete() > 0;
                //等价于
                isComplete = (from x in db.t_wx_wechats where x.id == id select x).Delete() > 0;
            }
            return isComplete;
        }


        //更新整个实体（一般先要进行查询）
        public static t_wx_wechats UpdateWeChat(t_wx_wechats wechat)
        {
            //调用EFhelper进行更新
            return EFHelper.UpdateWeChat<t_wx_wechats>(wechat);
        }


        //更新部分字段
        public static bool UpdateWeChat(int id, string country)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                //首先使用where获取需要更新的Iqueryable对象（并未执行SQL），之后执行UPDATE操作，在此之前需要引入entityframework.extension命名空间。
                //Update方法只能使用lambda表达式来实现，其中x是临时变量，在后面新建一个需要更新的对象实体，对想要更新的字段进行赋值
                isFinish = db.t_wx_wechats.Where(x => x.id == id).Update(x => new t_wx_wechats { country = country }) > 0;
            }
            return isFinish;
        }

        //添加wechat
        public static t_wx_wechats InsertWeChat(t_wx_wechats wechat)
        {
            //调用EFhelper进行添加
            return EFHelper.AddWeChat<t_wx_wechats>(wechat);
        }

        //select 相关方法请见DbAttach




    }
}
