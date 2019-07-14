using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KDWechat.DAL;
using EntityFramework.Extensions;
using KDWechat.Common;
using System.Linq.Expressions;

namespace KDWechat.BLL.Users
{
    public class wx_fans_tags
    {
        public static t_wx_fans_tags GetFansTagsByID(int id)
        {
            t_wx_fans_tags tag = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                tag = (from x in db.t_wx_fans_tags where x.id == id select x).FirstOrDefault();
            }
            return tag;
        }

        //批量添加标签
        public static bool AddTags(string[] uidList, string[] tagList)
        {

            bool isComplete = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                foreach (string _uid in uidList)
                {
                    int uid = Utils.StrToInt(_uid, 0);
                    if (uid > 0)
                    {
                        var fan = wx_fans.GetFansByID(uid);
                        foreach (string _tagID in tagList)
                        {
                            int tagID = Utils.StrToInt(_tagID, 0);
                            var relation = (from x in db.t_wx_fans_tags where x.tag_id == tagID && x.fans_id == uid select x).FirstOrDefault();
                            if (null == relation&&tagID>0)
                            {
                                var tagS = new t_wx_fans_tags()
                                {
                                    fans_guid = fan.guid,
                                    fans_id = uid,
                                    tag_id = tagID,
                                    wx_id = fan.wx_id,
                                    wx_og_id = fan.wx_og_id
                                };
                                db.t_wx_fans_tags.Add(tagS);
                            }
                        }
                    }

                }
                isComplete = db.SaveChanges() > 0;
            }
            return isComplete;
        }

        //批量删除标签
        public static bool DeleteTags(string[] uidList, string[] tagList)
        {
            bool isComplete = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                foreach (string _uid in uidList)
                {
                    int uid = Utils.StrToInt(_uid, 0);
                    if (uid > 0)
                    {
                        var fan = wx_fans.GetFansByID(uid);
                        foreach (string _tagID in tagList)
                        {
                            int tagID = Utils.StrToInt(_tagID, 0);
                            var relation = (from x in db.t_wx_fans_tags where x.tag_id == tagID && x.fans_id == uid select x).FirstOrDefault();
                            if (null != relation)
                                db.t_wx_fans_tags.Remove(relation);
                        }
                    }
                }
                isComplete = db.SaveChanges() > 0;
            }
            return isComplete;
        }


        //批量修改标签
        public static bool ChangeTags(int[] uids, int[] tags)
        {
            bool isComplete = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                foreach (int uid in uids)
                {
                    db.t_wx_fans_tags.Where(x => x.fans_id == uid).Delete();
                    var fan = wx_fans.GetFansByID(uid);
                    foreach (int tagID in tags)
                    {
                       
                        var tagS = new t_wx_fans_tags()
                        {
                            fans_guid = fan.guid,
                            fans_id = uid,
                            tag_id = tagID,
                            wx_id = fan.wx_id,
                            wx_og_id = fan.wx_og_id
                        };
                        db.t_wx_fans_tags.Add(tagS);
                    }
                }
                isComplete = db.SaveChanges() > 0;
            }
            return isComplete;
        }

        public static int[] GetArray(int wechat_id) 
        {
            int[] array = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                int[] containArray = db.t_wx_fans_tags.Where(x => x.wx_id == wechat_id).GroupBy(x=>x.fans_id).Select(x=>x.Key).ToArray();
                if (containArray != null && containArray.Length > 0)
                {
                    array = db.t_wx_fans.Where(x => !containArray.Contains(x.id)&&x.wx_id==wechat_id).Select(x => x.id).ToArray();
                }
                else
                    array = db.t_wx_fans.Where(x => x.wx_id == wechat_id).Select(x => x.id).ToArray();
            }
            return array;
        }

        
        public static bool ChangeTags(List<fans_tag_list> data)
        {
            bool isComplete = false;
            bool deleteOk=false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                foreach (fans_tag_list tag in data)
                {
                    deleteOk = db.t_wx_fans_tags.Where(x => x.fans_id == tag.id).Delete()>0;
                    var fan = wx_fans.GetFansByID(tag.id);
                    foreach (int tagID in tag.data)
                    {

                        var tagS = new t_wx_fans_tags()
                        {
                            fans_guid = fan.guid,
                            fans_id = tag.id,
                            tag_id = tagID,
                            wx_id = fan.wx_id,
                            wx_og_id = fan.wx_og_id
                        };
                        db.t_wx_fans_tags.Add(tagS);
                    }
                }
                isComplete = db.SaveChanges() > 0 || deleteOk;
            }
            return isComplete;
        }

        public static bool ChangeTags(string[] uidList, string[] tagList)
        {
            
            bool isComplete = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                foreach (string _uid in uidList)
                {
                    int uid = Utils.StrToInt(_uid, 0);
                    if (uid>0)
                    {
                        db.t_wx_fans_tags.Where(x => x.fans_id == uid).Delete();
                        isComplete = true;
                        var fan = wx_fans.GetFansByID(uid);
                        foreach (string _tagID in tagList)
                        {
                            int tagID = Utils.StrToInt(_tagID, 0);
                            if (tagID>0)
                            {
                                var tagS = new t_wx_fans_tags()
                                {
                                    fans_guid = fan.guid,
                                    fans_id = uid,
                                    tag_id = tagID,
                                    wx_id = fan.wx_id,
                                    wx_og_id = fan.wx_og_id
                                };
                                db.t_wx_fans_tags.Add(tagS);
                            }
                          
                        }
                    }
                    
                }
                db.SaveChanges();
            }
            return isComplete;
        }


        public static int[] GetFansIDListByGroupID(int wxid, int tagID)
        {
            int[] tag = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                tag = (from x in db.t_wx_fans_tags where x.wx_id == wxid && x.tag_id == tagID select x.fans_id).ToArray();
            }
            return tag;
        }

        public static List<int> GetFansIDListByGroupID_List(int wxid, int tagID)
        {
            List<int> tag = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                tag = (from x in db.t_wx_fans_tags where x.wx_id == wxid && x.tag_id == tagID select x.fans_id).ToList();
            }
            return tag;
        }

        public static t_wx_fans_tags CreateTag(t_wx_fans_tags tag)
        {
            return EFHelper.AddUser<t_wx_fans_tags>(tag);
        }

        public static bool DeleteTagByID(int id)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isFinish = db.t_wx_fans_tags.Where(x => x.id == id).Delete() > 0;
            }
            return isFinish;
        }

        public static bool UpdateTag(t_wx_fans_tags tag)
        {
            return EFHelper.UpdateUser<t_wx_fans_tags>(tag);
        }
        /// <summary>
        /// 根据粉丝guid获取粉丝的所有标签
        /// </summary>
        /// <param name="fans_guid"></param>
        /// <returns></returns>
        public static List<string> GetTagListByFansid(string fans_guid)
        {
            List<string> list = null;
            using (creater_wxEntities db=new creater_wxEntities())
            {
                List<int> tags = (from x in db.t_wx_fans_tags where x.fans_guid == fans_guid select x.tag_id).ToList();
                list = (from x in db.t_wx_group_tags where tags.Contains(x.id) select x.title).ToList();
            }
            return list;
        } 
    }


    public class fans_tag_list
    {
        public int id { get; set; }
        public List<int> data { get; set; }
    }
}

