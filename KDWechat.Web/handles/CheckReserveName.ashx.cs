using KDWechat.Common;
using KDWechat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
namespace KDWechat.Web.handles
{
    /// <summary>
    /// CheckReserveName 的摘要说明
    /// </summary>
    public class CheckReserveName : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string action = RequestHelper.GetFormString("action");
            //输入的预约名称
            string ReserveName = RequestHelper.GetFormString("ReserveName");
            string wx_og_id = RequestHelper.GetFormString("wx_og_id");
            //当前修改时数据库对应的预约名称
            string strName = RequestHelper.GetFormString("strName");

            string mobile = RequestHelper.GetFormString("mobile");//手机号
            string buidname = RequestHelper.GetFormString("buidname");//项目id
            #region //检测是否存在预约名称
            if (action == "reserveName")
            {
                
                if (ReserveName != strName)
                {

                    if (!ISReserveName(ReserveName, wx_og_id))
                    {
                        //预约名称不存在
                        context.Response.Write("success");
                    }
                    else
                    {
                        //预约名称已存在
                        context.Response.Write("fail");
                    }
                }
                if (ReserveName == strName)
                {
                    //预约名称不存在
                    context.Response.Write("success");
                }
            }
            #endregion

            #region//检测当前预约下的手机号是否存在 
            if (action == "reservephone")
            {
                int buidid=0;
                KDWechat.DAL.t_projects projects = KDWechat.BLL.Chats.projects.GetModel(buidname);
                if (projects != null)
                {
                    int manageid = Utils.ObjToInt(projects.id, 0);
                    buidid = manageid;//项目id
                }
                if (!IsMobile(mobile, buidid))
                {
                    //手机号不存在
                    context.Response.Write("success");
                }
                else
                {
                    //手机号已存在
                    context.Response.Write("fail");
                }
            }
            #endregion

        }
        //检查预约名称是否存在
        public bool ISReserveName(string name,string wx_og_id)
        {
            KDWechat.DAL.t_md_reserve_manage mod = KDWechat.BLL.Module.md_reserve_manage.GetModel(name,wx_og_id);
            if (mod != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //检查手机号是否存在
        public bool IsMobile(string mobile, int buidid)
        {
            KDWechat.DAL.t_md_reserve_house mod = KDWechat.BLL.Module.md_reserve_house.GetModel(mobile, buidid);
            if (mod != null)
            {
                return true;
            }
            else 
            {
                return false;
            }
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