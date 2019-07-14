using KDWechat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KDWechat.Common;
using System.Security.Cryptography;
using System.Linq.Expressions;
using EntityFramework.Extensions;
using System.Data;
using KDWechat.DBUtility;

namespace KDWechat.BLL.Users
{
    public class sys_letter
    {
   
        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static t_sys_letter GetModel(int id)
        {
            t_sys_letter model = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                model = db.t_sys_letter.Where(x => x.id == id).FirstOrDefault();
            }
            return model;
        }
        /// <summary>
        ///创建者删除一条信息
        /// </summary>
        /// <param name="id">信息ID</param>
        /// <returns>是否删除成功</returns>
        public static bool Delete(int id)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isFinish = db.t_sys_letter.Where(x => x.id == id).Delete() > 0;
                if (isFinish)
                {
                    db.t_sys_letter_receiver.Where(x => x.l_id == id).Delete();
                }
            }
            return isFinish;
        }
       
        /// <summary>
        ///接收者删除一条信息
        /// </summary>
        /// <param name="id">信息ID</param>
        public static int DeleteByRec(int id)
        {
            int res = 0;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                t_sys_letter_receiver model = db.t_sys_letter_receiver.Where(x => x.id == id).FirstOrDefault();
                if (model != null)
                {
                    model.status = (int)LetterType.删除;
                    t_sys_letter_receiver rec = db.Update<t_sys_letter_receiver>(model);
                    if (rec != null)
                    {
                        res = rec.id;
                    }
                }
            }
            return res;
        }
        /// <summary>
        ///设置已读
        /// </summary>
        /// <param name="id">信息ID</param>
        public static int SetRead(int id,int uid)
        {
            int res = 0;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                t_sys_letter_receiver model = db.t_sys_letter_receiver.Where(x => x.id == id && x.u_id==uid).FirstOrDefault();
                if (model != null)
                {
                    model.status = (int)LetterType.已读;
                    t_sys_letter_receiver rec = db.Update<t_sys_letter_receiver>(model);
                    if (rec != null)
                    {
                        res = rec.id;
                    }
                }
            }
            return res;
        }
        /// <summary>
        /// 发送站内信
        /// </summary>
        /// <param name="model"></param>
        /// <param name="RecList"></param>
        /// <returns></returns>
        public static int PostLetter(t_sys_letter model,int[] RecList) 
        {
            int res = 0;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                int L_id = db.Add<t_sys_letter>(model).id;
                
                if(L_id>0)
                {
                    for (int i = 0; i < RecList.Length; i++)
                    {
                        t_sys_letter_receiver recModel = new t_sys_letter_receiver()
                        {
                            l_id = L_id,
                            u_id = RecList[i],
                            status = 0
                        };
                        if (db.Add<t_sys_letter_receiver>(recModel) != null) 
                        {
                            res++;
                        }

                    }
                }
            }
            return res;
        }
        /// <summary>
        /// 根据作者获取站内信
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public static List<t_sys_letter> GetListByUserId(int uid, int pagesize, int pageindex, out int count)
        {
            List<t_sys_letter> letList = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                var query = (from x in db.t_sys_letter where x.u_id == uid orderby x.id descending select x);
                count = query.Count();
                letList = query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            }
            return letList;
        }
        /// <summary>
        /// 根据接收者获取站内信
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public static List<t_sys_letter> GetListByRecId(int uid, int pagesize, int pageindex, int status, out int count)
        {
            List<t_sys_letter> letList = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                int?[] lidList;
                if (status>-1)
                {
                     lidList = db.t_sys_letter_receiver.Where(x => x.u_id == uid && x.status==status).Select(x => x.l_id).ToArray();
                }
                else
                {
                    lidList = db.t_sys_letter_receiver.Where(x => x.u_id == uid && x.status!=(int)LetterType.删除).Select(x => x.l_id).ToArray();
                }
                var query = db.t_sys_letter.Where(x => lidList.Contains(x.id));
                count = query.Count();
                letList = query.OrderByDescending(x => x.id).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            }
            return letList;
        }
        /// <summary>
        /// 获取用户未读站内信数量
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static int GetUnread(int uid)
        {
            int count = 0;
            
            using (creater_wxEntities db = new creater_wxEntities())
            {
                var query = db.t_sys_letter_receiver.Where(x => x.u_id == uid && x.status == (int)LetterType.未读);
               if(query!=null)
               {
                   count = query.Count();
               }
            }
            return count;
        }

       /// <summary>
       /// 获取符合条件的用户Id数组（用户表）
       /// </summary>
       /// <param name="query"></param>
       /// <returns></returns>
        public int[] GetUserIdList(string query)
        {

            DataSet ds = KDWechat.DBUtility.DbHelperSQL.Query(query);
            DataTable tabIds = ds.Tables[0] as DataTable;
            if (tabIds != null)
            {
                int count = tabIds.Rows.Count;
                if (count > 0)
                {
                    int[] UidList = new int[count];
                    for (int i = 0; i < count; i++)
                    {
                        UidList[i] = Common.Utils.StrToInt(tabIds.Rows[i]["id"].ToString(), 0);
                    }
                    return UidList;
                }

            }
           
            int[] UidList1 = new int[1];
            UidList1[0] = 1;
            return UidList1;
         
           
        }
        /// <summary>
        /// 获取符合条件的用户Id数组（站内信接收表）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int[] GetUserIdListFromRec(string query)
        {

            DataSet ds = KDWechat.DBUtility.DbHelperSQL.Query(query);
            DataTable tabIds = ds.Tables[0] as DataTable;
            if (tabIds != null)
            {
                int count = tabIds.Rows.Count;
                if (count > 0)
                {
                    int[] UidList = new int[count];
                    for (int i = 0; i < count; i++)
                    {
                        UidList[i] = Common.Utils.StrToInt(tabIds.Rows[i]["u_id"].ToString(), 0);
                    }
                    return UidList;
                }

            }

            int[] UidList1 = new int[1];
            UidList1[0] = 1;
            return UidList1;


        }
    }
}
