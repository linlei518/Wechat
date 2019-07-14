using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KDWechat.DAL;
using EntityFramework.Extensions;
using KDWechat.Common;
using Companycn.Core.DbHelper;
using System.Data.SqlClient;
using System.Data;


namespace KDWechat.BLL.Module
{
    public class md_reserve_house
    {

        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static t_md_reserve_house GetModel(int id)
        {
            t_md_reserve_house model = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                model = db.t_md_reserve_house.Where(x => x.id == id).FirstOrDefault();
            }
            return model;
        }
        /// <summary>
        /// 提取1条数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static t_md_reserve_house GetModel(string mobile, int buidid)
        {
            t_md_reserve_house model = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                model = db.t_md_reserve_house.Where(x => x.phone == mobile && x.buidid == buidid).FirstOrDefault();
            }
            return model;
        }
        /// <summary>
        /// 根据所属菜单ID提取1条数据
        /// </summary>
        /// <param name="id">M_id</param>
        /// <returns></returns>
        public static t_md_reserve_house GetModelByM_id(int m_id)
        {
            t_md_reserve_house model = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                model = db.t_md_reserve_house.Where(x => x.id == m_id).FirstOrDefault();
            }
            return model;
        }

        /// <summary>
        /// 删除一条信息
        /// </summary>
        /// <param name="id">信息ID</param>
        /// <returns>是否删除成功</returns>
        public static bool Delete(int id)
        {
            bool isFinish = false;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                isFinish = db.t_md_reserve_house.Where(x => x.id == id).Delete() > 0;
            }
            return isFinish;
        }
       

        /// <summary>
        /// 添加一条信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_md_reserve_house Add(t_md_reserve_house model)
        {

            model = EFHelper.AddModule<t_md_reserve_house>(model);
            return model;//返回添加后的信息
        }

        //简单的增删改查可在后台
        //model = EFHelper.AddModel<kd_moduleEntities, t_md_reserve_house>(model);

        //int i;
        //List<t_md_reserve_house> list = EFHelper.GetList<kd_moduleEntities, t_md_reserve_house, int>(x => x.id == 1, x => x.id, int.MaxValue, 1, out i, true);

        /// <summary>
        /// 修改一条信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_md_reserve_house Update(t_md_reserve_house model)
        {
            return EFHelper.UpdateModule<t_md_reserve_house>(model);
        }
        
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_reserve_house> GetList(int p_id,int wx_id,string wx_og_id, int pagesize, int pageindex, out int count)
        {
            List<t_md_reserve_house> list = null;
            List<t_md_reserve_house> listForCount = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                
                    list = (from x in db.t_md_reserve_house where x.p_id==p_id && x.wx_id==wx_id && x.wx_og_id==wx_og_id  orderby x.id select x).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                    listForCount = (from x in db.t_md_reserve_house where x.p_id == p_id && x.wx_id == wx_id && x.wx_og_id == wx_og_id orderby x.id select x).ToList();
               
                
                
                    count = listForCount.Count();
                
            }
            return list;
        }


        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_reserve_house> GetListForDDL(int buidid,int reserve_id, int wx_id, string wx_og_id, int pagesize, int pageindex, out int count)
        {
            List<t_md_reserve_house> list = null;
            List<t_md_reserve_house> listForCount = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {

                list = (from x in db.t_md_reserve_house where x.buidid == buidid && x.p_id==reserve_id && x.wx_id == wx_id && x.wx_og_id == wx_og_id orderby x.id select x).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                listForCount = (from x in db.t_md_reserve_house where x.buidid == buidid && x.p_id == reserve_id && x.wx_id == wx_id && x.wx_og_id == wx_og_id orderby x.id select x).ToList();



                count = listForCount.Count();

            }
            return list;
        }
        /// <summary>
        /// 根据预约id和微信id，og_id获取列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="wx_id"></param>
        /// <param name="wx_og_id"></param>
        /// <returns></returns>
        public static DataTable GetListForExcel(int p_id, int wx_id, string wx_og_id)
        {

            return DbHelperSQLModule.Query("select b.reservename as RName,c.title as BName," +
                                "a.Name as Name,a.phone as Phone,a.SeeTime as SeeTime,a.SeeSum as SeeSum,a.status as Status from "+
                                " t_md_reserve_house as a " +
                                " inner join t_md_reserve_manage as b on a.p_id=b.id "+
                                " inner join kd_wechats.dbo.t_projects as c on a.buidid=c.id "+
                                " where a.p_id="+p_id+" and a.wx_id="+wx_id+" and a.wx_og_id='"+wx_og_id+"'").Tables[0];
        }
        /// <summary>
        /// 根据预约id，项目id和微信id，og_id获取列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="wx_id"></param>
        /// <param name="wx_og_id"></param>
        /// <returns></returns>
        public static DataTable GetListForExcel(int buidid, int p_id, int wx_id, string wx_og_id)
        {
            return DbHelperSQLModule.Query("select b.reservename as RName,c.title as BName," +
                                "a.Name as Name,a.phone as Phone,a.SeeTime as SeeTime,a.SeeSum as SeeSum,a.status as Status from " +
                                " t_md_reserve_house as a  " +
                                " inner join t_md_reserve_manage as b on a.p_id=b.id " +
                                " inner join kd_wechats.dbo.t_projects as c on a.buidid=c.id " +
                                " where a.buidid="+buidid+" and a.p_id=" + p_id + " and a.wx_id=" + wx_id + " and  a.wx_og_id='" + wx_og_id + "'").Tables[0];
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_reserve_house> GetList(int p_id, int wx_id, string wx_og_id)
        {
            List<t_md_reserve_house> list = null;
          
            using (kd_moduleEntities db = new kd_moduleEntities())
            {

                list = (from x in db.t_md_reserve_house where x.p_id == p_id && x.wx_id == wx_id && x.wx_og_id == wx_og_id orderby x.id select x).ToList();
             
            }
            return list;
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_reserve_house> GetList(int buidid,int p_id, int wx_id, string wx_og_id)
        {
            List<t_md_reserve_house> list = null;

            using (kd_moduleEntities db = new kd_moduleEntities())
            {

                list = (from x in db.t_md_reserve_house where x.buidid==buidid && x.p_id == p_id && x.wx_id == wx_id && x.wx_og_id == wx_og_id orderby x.id select x).ToList();

            }
            return list;
        }
        /// <summary>
        /// 获取信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<t_md_reserve_house> GetListView()
        {
            List<t_md_reserve_house> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {
                var query = (from x in db.t_md_reserve_house select x);
                list = query.ToList();

            }
            return list;
        }
        
      
    
        public static List<t_md_reserve_house> GetMaxID()
        {
            List<t_md_reserve_house> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {

                var query = (from x in db.t_md_reserve_house select x);
                list = query.ToList();
            }
            return list;
        }


        public static List<t_md_reserve_house> GetLIstForReserve_id(int reserve_id)
        {
            List<t_md_reserve_house> list = null;
            using (kd_moduleEntities db = new kd_moduleEntities())
            {

                var query = (from x in db.t_md_reserve_house where  x.p_id==reserve_id select x);
                list = query.ToList();
            }
            return list;
        }
    }


    public class t_md_reserve_house_export
    {
        public string reserveName { get; set; }
        public string projectName { get; set; }
        public string Name { get; set; }
        public string phone { get; set; }
        public Nullable<System.DateTime> SeeTime { get; set; }
        public Nullable<int> SeeSum { get; set; }
        public string status { get; set; }
       
    }

}
