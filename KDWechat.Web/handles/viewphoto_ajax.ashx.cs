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
    /// viewphoto_ajax 的摘要说明
    /// </summary>
    public class viewphoto_ajax : IHttpHandler, IRequiresSessionState
    {
        Web.UI.BasePage bages = new UI.BasePage();
        public void ProcessRequest(HttpContext context)
        {
            int houseid = RequestHelper.GetQueryInt("houseid");
            int viewid = RequestHelper.GetQueryInt("viewid");
            int typeid = RequestHelper.GetQueryInt("typeid");
            int housevalues = RequestHelper.GetQueryInt("housevalues");
            int viewvalues = RequestHelper.GetQueryInt("viewvalues");
            int houseSortId = RequestHelper.GetQueryInt("houseSortId");
            int viewSortId = RequestHelper.GetQueryInt("viewSortId");
            if (houseid > 0)
            {
                //删除房型图
                DeleteHouse(houseid);
                context.Response.Write("1");
                
            }
           
            if (viewid > 0)
            {
                //删除全景图
                DeleteView(viewid);
                context.Response.Write("2");  
            }
            if (typeid > 0)
            {
                var list = KDWechat.BLL.Module.md_view.GetListForPro(typeid);
                if (list.Count > 0)
                {
                    context.Response.Write("x");
                }
                //删除房型
                else
                {
                    DeleteType(typeid);
                    context.Response.Write("3");
                }
            }
            if (houseSortId > 0 && housevalues > 0)
            {
                //更新房型图列表
                if (!DoEditForHouse(houseSortId, housevalues))
                {
                    context.Response.Write("4"); 
                }
                else
                {
                    context.Response.Write("5"); 
                }
            }
            if (viewSortId > 0 && viewvalues > 0)
            {
                //更新全景图列表
               if(! DoEditForView(viewSortId, viewvalues))
               {
                   context.Response.Write("6"); 
               }
               else
               {
                   context.Response.Write("7"); 
               }
            }
            if (houseid == 0 && viewid == 0 && typeid == 0 && viewSortId == 0 && viewvalues == 0 && houseSortId == 0 && housevalues == 0)
            {
                context.Response.Write("8");  
            }
            context.Response.ContentType = "text/plain";
          
        }
        private bool DoEditForView(int id, int value)
        {
            KDWechat.DAL.t_md_360view mod = KDWechat.BLL.Module.md_view.GetModel(id);
            
                mod.sort_id = value;
               int vid= BLL.Module.md_view.Update(mod).id;
               if (vid > 0)
                {
                   
                    return true;
                }
                else
                {
                    return false;
                }
            
        }
        private bool DoEditForHouse(int id, int value)
        {
            KDWechat.DAL.t_md_360viewList mod = KDWechat.BLL.Module.md_view360list.GetModel(id);
           
                mod.sort_id = value;
               int hid= BLL.Module.md_view360list.Update(mod).id;
               if (hid > 0)
                {

                    return true;
                }
                else
                {
                    return false;
                }
        }
        private void DeleteHouse(int id)
        {
            KDWechat.DAL.t_md_360viewList mod = KDWechat.BLL.Module.md_view360list.GetModel(id);
            if (mod!=null)
            {
                int app_parent_id = Utils.ObjToInt(mod.pid, 0);
                KDWechat.BLL.Module.md_view360list.Delete(id);
                KDWechat.DAL.t_module_wechat module = KDWechat.BLL.Chats.module_wechat.GetModelForViewBuid(bages.wx_id, id, app_parent_id, "t_md_360viewList");
                if (module != null)
                {
                    KDWechat.BLL.Chats.module_wechat.DeleteForBuid(id, app_parent_id, "t_md_360viewList");
                }
                //JsHelper.AlertAndRedirect("全景图删除成功！", "viewlist.aspx?m_id=" + id + "&p_id=" + p_id);
                
            }
        }
        private void DeleteView(int id)
        {
            KDWechat.DAL.t_md_360view mod = KDWechat.BLL.Module.md_view.GetModel(id);
            if (mod != null)
            {
                int app_parent_id = Utils.ObjToInt(mod.p_id, 0);
                KDWechat.BLL.Module.md_view.Delete(id);
                KDWechat.DAL.t_module_wechat module = KDWechat.BLL.Chats.module_wechat.GetModelForViewBuid(bages.wx_id, id, app_parent_id, "t_md_360view");
                if (module != null)
                {
                    KDWechat.BLL.Chats.module_wechat.DeleteForBuid(id, app_parent_id, "t_md_360view");
                }
                //JsHelper.AlertAndRedirect("全景图删除成功！", "viewlist.aspx?m_id=" + id + "&p_id=" + p_id);

            }
        }
        private void DeleteType(int id)
        {
            KDWechat.DAL.t_md_360viewtype mod = KDWechat.BLL.Module.md_view360type.GetModel(id);
            if (mod != null)
            {
                int app_parent_id =Utils.ObjToInt(mod.pid,0);
                KDWechat.BLL.Module.md_view360type.Delete(id);
                KDWechat.DAL.t_module_wechat module = KDWechat.BLL.Chats.module_wechat.GetModelForViewBuid(bages.wx_id, id, app_parent_id, "t_md_360viewtype");
                if (module != null)
                {
                    KDWechat.BLL.Chats.module_wechat.DeleteForBuid(id, app_parent_id, "t_md_360viewtype");
                }
                //JsHelper.AlertAndRedirect("全景图删除成功！", "viewlist.aspx?m_id=" + id + "&p_id=" + p_id);

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