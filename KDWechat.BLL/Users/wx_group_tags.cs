using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KDWechat.DAL;
using EntityFramework.Extensions;
using KDWechat.Common;
using KDWechat.BLL.Chats;
using System.Data;
using KDWechat.DBUtility;

namespace KDWechat.BLL.Users
{
    /// <summary>
    /// 微信公众号中的关注用户分组和标签共用表
    /// </summary>
    public class wx_group_tags
    {
        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static t_wx_group_tags Add(t_wx_group_tags model)
        {
            if (model.is_public==1)
            {
                return EFHelper.AddUser<t_wx_group_tags>(model);
            }
            else
            {
                return EFHelper.AddUser<t_wx_group_tags>(model);
            }
            
        }
        /// <summary>
        /// 获取一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static t_wx_group_tags GetModel(int id)
        {
            t_wx_group_tags model = null;
            using (creater_wxEntities db=new creater_wxEntities())
            {
                model = db.t_wx_group_tags.Where(x => x.id == id).FirstOrDefault();
            }
            return model;
        }

        /// <summary>
        /// 更新一个对象
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool Update(int id,t_wx_group_tags model)
        {

            bool result = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                result = db.t_wx_group_tags.Where(x => x.id == id).Update(x => new t_wx_group_tags() { title = model.title,contents=model.contents,parent_id=model.parent_id }) > 0;
            }
            return result;
        }

        /// <summary>
        /// 根据分组id获取分组名称
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static string GetGroupName(int groupId)
        {
            string name = "默认分组";
            if (groupId > 0)
            {
                using (KDWechat.DAL.creater_wxEntities db = new DAL.creater_wxEntities())
                {
                    name = (from x in db.t_wx_group_tags where x.id == groupId select x.title).FirstOrDefault();
                    if (name == null)
                    {
                        name = "";
                    }
                }
            }


            return name;
        }
        /// <summary>
        /// 根据channel_id 获取某个公众号的数据
        /// </summary>
        /// <param name="channel_id">区分ID（1-关注用户分组，2-关注用户标签，3-素材分组）</param>
        /// <param name="wx_id">所属微信ID</param>
        /// <param name="wx_og_id">所属微信生成的GUID</param>
        /// <param name="is_public">是否公共素材（1-是，0-否</param>
        /// <returns></returns>
        public static List<KDWechat.DAL.t_wx_group_tags> GetListByChannelId(int channel_id, int wx_id, string wx_og_id, int is_public,int status=1)
        {
            List<KDWechat.DAL.t_wx_group_tags> list = null;
            using (KDWechat.DAL.creater_wxEntities db = new DAL.creater_wxEntities())
            {
                if (status==-1)
                {
                    list = db.t_wx_group_tags.Where(x => x.channel_id == channel_id && x.wx_id == wx_id && x.wx_og_id == wx_og_id && x.is_public == is_public ).OrderBy(x => x.id).ToList();
                }
                else
                {
                    list = db.t_wx_group_tags.Where(x => x.channel_id == channel_id && x.wx_id == wx_id && x.wx_og_id == wx_og_id && x.is_public == is_public && x.status == status).OrderBy(x => x.id).ToList();
                }
               
            }
            return list;
        }

        public static List<KDWechat.DAL.t_wx_group_tags> GetListByChannelId(int channel_id, int wx_id,int status=1)
        {
            List<KDWechat.DAL.t_wx_group_tags> list = null;
            using (KDWechat.DAL.creater_wxEntities db = new DAL.creater_wxEntities())
            {
                if (status == -1)
                {
                    list = db.t_wx_group_tags.Where(x => x.channel_id == channel_id && x.wx_id == wx_id).ToList();
                }
                else
                {
                    list = db.t_wx_group_tags.Where(x => x.channel_id == channel_id && x.wx_id == wx_id && x.status == status).ToList();
                }
            }
            return list;
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        public static bool UpdateStatus(int id, int status)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isFinish = db.t_wx_group_tags.Where(x => x.id == id).Update(x => new t_wx_group_tags() { status = status }) > 0;
            }
            return isFinish;
        }
        /// <summary>
        /// 删除一条分组或标签
        /// </summary>
        /// <param name="id">分组id</param>
        /// <param name="channel_id">区分ID（1-关注用户分组，2-关注用户标签，3-素材分组）</param>
        /// <param name="errorMsg">返回的错误消息</param>
        /// <returns></returns>
        public static int Delete(int id, channel_idType channel_id, ref string errorMsg)
        {
            int result = 0;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                switch (channel_id)
                {
                    case channel_idType.关注用户分组:
                        result = GetGroupUseCountByFans(id);
                        if (result > 0)
                            errorMsg = "该分组不能删除，请先移除该分组下的关注用户";
                        break;
                    case channel_idType.关注用户标签:
                        result = GetGroupUseCountByFansTag(id);
                        if (result > 0)
                            errorMsg = "该分组不能删除，请先移除该标签下的关注用户";
                        break;
                    case channel_idType.素材分组:
                        result = GetGroupUseCountByMaterial(id);
                        if (result > 0)
                            errorMsg = "该分组不能删除，请先移除该分组下的素材";
                        break;
                    default:
                        break;
                }
                if (result == 0)
                {
                    result = db.t_wx_group_tags.Where(x => x.id == id).Delete();
                }

            }
            return result;
        }

        public static bool CheckTagOrGroup(string title,int wx_id,int channenIDType)
        {
            bool isOk = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isOk = db.t_wx_group_tags.Where(x => x.title == title &&(x.wx_id == wx_id||x.wx_id==0)&&x.channel_id==channenIDType).FirstOrDefault() == null;
            }
            return isOk;
        }

        #region 私有方法

        /// <summary>
        /// 根据分组id获取有多少关注用户使用此标签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static int GetGroupUseCountByFansTag(int id)
        {
            string sql = string.Format("select COUNT(1) from t_wx_fans_tags where tag_id={0}", id);
            return Convert.ToInt32(KDWechat.DBUtility.DbHelperSQL.GetSingle(sql));
        }
        /// <summary>
        /// 根据分组id获取有多少关注用户使用此分组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static int GetGroupUseCountByFans(int id)
        {
            string sql = string.Format("select COUNT(1) from t_wx_fans where group_id={0}", id);
            return Convert.ToInt32(KDWechat.DBUtility.DbHelperSQL.GetSingle(sql));
        }

        /// <summary>
        /// 根据分组id获取有多少素材使用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static int GetGroupUseCountByMaterial(int id)
        {
            string sql = string.Format("select  (select COUNT(1) from t_wx_media_materials where group_id={0})+ (select COUNT(1) from t_wx_txt_materials where group_id={0})+ (select COUNT(1) from t_wx_news_materials where group_id={0})", id);
            return Convert.ToInt32(KDWechat.DBUtility.DbHelperSQL.GetSingle(sql));
        }
        #endregion

        /// <summary>
        /// 给用户添加标签
        /// </summary>
        /// <param name="wx_id">微信id</param>
        /// <param name="wx_og_id">微信原始id</param>
        /// <param name="openId">用户的openid</param>
        /// <param name="tag_list">标签名称（支持多个）</param>
        /// <returns></returns>
        public static bool AddTag(string openId,List<string> tag_list)
        {
            
            int count = 0;
            if (tag_list!=null && tag_list.Count>0)
            {
                //取出用户
         
                DataTable dt_user = KDWechat.DBUtility.DbHelperSQL.Query("select top 1 id,guid,wx_id,wx_og_id from t_wx_fans where open_id='" + openId + "'").Tables[0];
                if (dt_user!=null)
                {
                    int fans_id = Common.Utils.StrToInt(dt_user.Rows[0]["id"].ToString(), 0);
                    string fans_guid = dt_user.Rows[0]["guid"].ToString();
                    int wx_id = Common.Utils.StrToInt(dt_user.Rows[0]["wx_id"].ToString(),0);
                    string wx_og_id = dt_user.Rows[0]["wx_og_id"].ToString();

                    if (fans_id>0 && fans_guid!="")
                    {
                        //循环标签
                        foreach (string tag in tag_list)
                        {
                            //取出标签id
                            int tag_id = Common.Utils.ObjToInt(KDWechat.DBUtility.DbHelperSQL.GetSingle("select id from t_wx_group_tags where title='" + tag + "' and wx_id=" + wx_id + " and channel_id=2"), 0);
                            if (tag_id==0)  
                            {
                                //没有,那就插吧
                                tag_id = Common.Utils.ObjToInt(KDWechat.DBUtility.DbHelperSQL.GetSingle("insert into t_wx_group_tags(title,status,wx_og_id,wx_id,channel_id,is_public,contents,create_time) values('" + tag + "',1,'" + wx_og_id + "'," + wx_id + ",2,0,'',getdate());select @@identity; "), 0);
                            }
                            if (tag_id>0)
                            {
                                //判断是否已经有此标签了
                                int exists_id = Common.Utils.ObjToInt(KDWechat.DBUtility.DbHelperSQL.GetSingle("select id from t_wx_fans_tags where fans_id=" + fans_id + " and wx_id=" + wx_id + " and tag_id=" + tag_id + ""), 0);
                                if (exists_id==0)
                                {
                                    //没有此标签，插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插插 
                                    count += KDWechat.DBUtility.DbHelperSQL.ExecuteSql("insert into t_wx_fans_tags(fans_guid,fans_id,wx_og_id,wx_id,tag_id) values('" + fans_guid + "'," + fans_id + ",'" + wx_og_id + "'," + wx_id + "," + tag_id + ")");
                                }
                            }
                        }
                    }
                }
            }
            return count>0;
        }

    }
}
