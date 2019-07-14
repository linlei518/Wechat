using KDWechat.Common;
using KDWechat.DAL;
using KDWechat.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using EntityFramework.Extensions;
using LinqKit;
using KDWechat.DBUtility;

namespace KDWechat.BLL.Chats
{
    public class module_wx_switch
    {

        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static t_module_wx_switch GetModel(int id)
        {
            t_module_wx_switch material = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                material = db.t_module_wx_switch.Where(x => x.id == id).FirstOrDefault();
            }
            return material;
        }

        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static t_module_wx_switch GetModel(Expression<Func<t_module_wx_switch,bool>> where )
        {
            t_module_wx_switch material = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                 
               material = db.t_module_wx_switch.Where(where.Expand()).FirstOrDefault();
                
            }
            return material;
        }

        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static t_module_wx_switch GetModel(int wx_id, int u_id, int module_id)
        {
            t_module_wx_switch material = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {

                var modlue_ids = (from x in db.t_module_wx_user_role where x.wx_id == wx_id && x.user_id == u_id && x.module_id == module_id select x.module_id).FirstOrDefault();
                material = db.t_module_wx_switch.Where(x => x.wx_id == wx_id && x.module_id == modlue_ids && x.status == (int)Status.正常).FirstOrDefault();

            }
            return material;
        }
       
        /// <summary>
        /// 提取module_wx_switch
        /// </summary>
        /// <param name="wxID"></param>
        /// <param name="moduleID"></param>
        /// <returns></returns>
        public static t_module_wx_switch GetModelByWxIDAndModuleID(int wxID, int moduleID)
        {
            t_module_wx_switch material = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                material = db.t_module_wx_switch.Where(x => x.wx_id == wxID && x.module_id == moduleID).FirstOrDefault();
            }
            return material;
        }

        /// <summary>
        /// 删除一条消息
        /// </summary>
        /// <param name="id">消息ID</param>
        /// <returns>是否删除成功</returns>
        public static t_module_wx_switch Delete(int id)
        {
            t_module_wx_switch isFinish = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isFinish = db.t_module_wx_switch.Where(x => x.id == id).FirstOrDefault();

                if (isFinish != null)
                {
                    isFinish.status = (int)Status.禁用;
                    db.SaveChanges();
                }
            }
            return isFinish;
        }
        /// <summary>
        /// 移除一个模块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool RemoveByMid(int mid)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                db.t_module_wx_switch.Where(x => x.module_id == mid).Update(x => new t_module_wx_switch { status = (int)Status.禁用 });
                isFinish = db.SaveChanges() > 0;
            }
            return isFinish;
        }
        /// <summary>
        /// 移除一个模块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static t_module_wx_switch RemoveByMid(int mid, int wx_id)
        {
            t_module_wx_switch isFinish = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isFinish = db.t_module_wx_switch.Where(x => x.module_id == mid && x.wx_id == wx_id).FirstOrDefault();
                if(isFinish!=null)
                    isFinish.status = (int)Status.禁用;
                db.SaveChanges();
            }
            return isFinish;
        }
        public static bool ChangeStatusToOk(int mid, int wx_id=0)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                if (wx_id == 0)
                {
                    var s = db.t_module_wx_switch.Where(x => x.module_id == mid).Update(x => new t_module_wx_switch { status=(int)Status.正常});
                }
                else
                {
                    var s= db.t_module_wx_switch.Where(x => x.module_id == mid && x.wx_id == wx_id).FirstOrDefault();
                    if(s!=null)
                        s.status = (int)Status.正常;
                }
                isFinish = db.SaveChanges()>0;
            }
            return isFinish;

        }

        /// <summary>
        /// 移除一个模块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static t_module_wx_switch Remove(int id)
        {
            t_module_wx_switch isFinish = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isFinish = db.t_module_wx_switch.Where(x => x.id == id).FirstOrDefault();
                isFinish.status = (int)Status.禁用;
                db.SaveChanges();
            }
            return isFinish;
        }

        /// <summary>
        /// 添加一条消息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_module_wx_switch Add(t_module_wx_switch model)
        {

            model = EFHelper.AddWeChat<t_module_wx_switch>(model);
            return model;//返回添加后的消息
        }

        /// <summary>
        /// 修改一条消息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_module_wx_switch Update(t_module_wx_switch model)
        {
            if (model != null)
            {
                EFHelper.UpdateWeChat<t_module_wx_switch>(model);
            }
            return model;
        }

        //提取列表
        public static List<t_module_wx_switch> GetList(Expression<Func<t_module_wx_switch, bool>> where, int pagesize, int pageindex, out int totalCount)
        {
            List<t_module_wx_switch> list = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                var query = db.t_module_wx_switch.Where(where.Expand());
                totalCount = query.Count();
                list = query.OrderByDescending(x=>x.id).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            }
            return list;

        }

        public static DataTable GetListByQuery(string query)
        {
            DataSet ds = KDWechat.DBUtility.DbHelperSQL.Query(query);
            DataTable dt = ds.Tables[0] as DataTable;
            return dt;
        }

        /// <summary>
        /// 添加禁用模块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static t_module_wx_switch RemoveOrAddModule(int id)
        {
            t_module_wx_switch module = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                module = (from x in db.t_module_wx_switch where x.id == id select x).FirstOrDefault();
                if (module != null)
                {
                    var md_wx_list = (from x in db.t_module_wechat where x.wx_id == module.wx_id && x.module_id == module.module_id select x);
                    if (module.status == (int)Common.Status.禁用)
                    {
                        module.status = (int)Common.Status.正常;
                        foreach (var md_wx in md_wx_list)
                        {
                            md_wx.status = (int)Status.正常;
                        }
                    }
                    else
                    {
                        module.status = (int)Common.Status.禁用;
                        foreach (var md_wx in md_wx_list)
                        {
                            md_wx.status = (int)Status.禁用;
                        }
                    }
                    db.SaveChanges();
                }
            }
            return module;
        }
    }
}
