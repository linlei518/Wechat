using KDWechat.DAL;
using KDWechat.Common;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.CommonAPIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityFramework.Extensions;
using KDWechat.DBUtility;


namespace KDWechat.BLL.Chats
{
    public class wx_media_materials
    {
        #region 外部方法
        /// <summary>
        /// 获取多媒体素材列表
        /// </summary>
        /// <param name="pagesize">每页数量</param>
        /// <param name="pageindex">页码</param>
        /// <returns>素材列表</returns>
        public static List<t_wx_media_materials> GetMediaListBySizeAndIndex(int pagesize, int pageindex)
        {
            List<t_wx_media_materials> materialsList = null;

            using (creater_wxEntities db = new creater_wxEntities())
            {
                materialsList = (from x in db.t_wx_media_materials orderby x.id descending select x).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            }

            return materialsList;
        }

        /// <summary>
        /// 检测是否存在
        /// </summary>
        /// <param name="title"></param>
        /// <param name="?"></param>
        /// <param name="wx_id"></param>
        /// <returns></returns>
        public static bool Exists(string title,int channel_id,int wx_id)
        {
            bool isc=false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isc = (from x in db.t_wx_media_materials where x.channel_id == channel_id && x.title == title && x.wx_id == wx_id select x.id).Count() > 0;
            }
            return isc;
        }

        /// <summary>
        /// 提取1条多媒体素材
        /// </summary>
        /// <param name="id">素材ID</param>
        /// <returns>提取到的多媒体素材</returns>
        public static t_wx_media_materials GetMediaMaterialByID(int id)
        {
            t_wx_media_materials material = null;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                material = db.t_wx_media_materials.Where(x => x.id == id ).FirstOrDefault();
            }
            return material;
        }


        /// <summary>
        /// 获取素材id的引用数量（数组长度为4 ，分别为：关注回复引用数量、无匹配回复引用数量、关键字回复引用数量、自定义菜单引用数量）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<int> GetUseCount(int id)
        {
            List<int> list = new List<int>();
            
            //关注回复引用数量
            list.Add(Common.Utils.ObjToInt(KDWechat.DBUtility.DbHelperSQL.GetSingle(string.Format("select COUNT(1) from t_wx_basic_reply where source_id={0} and reply_type in(3,4,5) and channel_id=1", id)), 0));
            //无匹配回复引用数量
            list.Add(Common.Utils.ObjToInt(KDWechat.DBUtility.DbHelperSQL.GetSingle(string.Format("select COUNT(1) from t_wx_basic_reply where source_id={0} and reply_type in(3,4,5) and channel_id=2", id)), 0));
            //关键字回复引用数量
            list.Add(Common.Utils.ObjToInt(KDWechat.DBUtility.DbHelperSQL.GetSingle(string.Format("select COUNT(1) from t_wx_rule_reply where source_id={0} and reply_type in(3,4,5) ", id)), 0));
            //自定义菜单引用数量
            list.Add(Common.Utils.ObjToInt(KDWechat.DBUtility.DbHelperSQL.GetSingle(string.Format("select COUNT(1) from t_wx_diy_menus where soucre_id={0} and reply_type in(3,4,5) ", id)), 0));
            return list;
        }

        /// <summary>
        /// 删除一条多媒体素材
        /// </summary>
        /// <param name="id">多媒体素材ID</param>
        /// <returns>是否删除成功</returns>
        public static bool Delete(int id)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isFinish = db.t_wx_media_materials.Where(x => x.id == id).Delete() > 0;
            }
            return isFinish;
        }

        /// <summary>
        /// 添加一条多媒体素材
        /// </summary>
        /// <param name="wx_id">公众号id</param>
        /// <param name="u_id">信息创建人ID</param>
        /// <param name="title">标题</param>
        /// <param name="remark">简介</param>
        /// <param name="group_id">分组ID</param>
        /// <param name="file_url">文件路径</param>
        /// <param name="file_ext">文件扩展名称</param>
        /// <param name="file_size">文件大小</param>
        /// <param name="m_type">区分ID（1-图片，2-语音，3-视频，4-音乐）</param>
        /// <param name="v_type">视频区分类型Id(1-本地视频，2-微视）</param>
        /// <param name="media_id">微信上传多媒体返回的ID，群发时上传到微信服务器</param>
        /// <param name="hq_music_url">高质量音乐链接，WIFI环境优先使用该链接播放音乐</param>
        /// <param name="upload_time">上传到微信服务器时间</param>
        /// <param name="status">状态（1-可用，0-禁用）</param>
        /// <param name="is_public">是否公共素材（0-否，1-是）</param>
        /// <returns></returns>
        public static t_wx_media_materials Add(int wx_id, int u_id, string title, string remark, int group_id, string file_url, string file_ext, string file_size, media_type m_type, int v_type, string media_id, string hq_music_url, DateTime upload_time, int status = 1, int is_public = 0)
        {

            t_wx_media_materials model = new t_wx_media_materials()
            {
                    remark = remark,
                    title = title,
                    is_public = is_public,
                    create_time = DateTime.Now,
                    group_id = group_id,
                    u_id = u_id,
                    wx_id = wx_id,
                    status = status,
                    file_url = file_url,
                    file_size = file_size,
                    file_ext = file_ext,
                    channel_id = (int)m_type,
                    media_id = media_id,
                    video_type = v_type,
                    hq_music_url = hq_music_url
                };
            if (upload_time != null)
            {
                model.upload_time = upload_time;
            }
            if (m_type != media_type.素材视频)
            {
                model.video_type = -1;
            }
            if (is_public == 0)
            {
                var wechat = wx_wechats.GetWeChatByID(wx_id);//获取微信号信息
                if (wechat != null)
                {
                    model.wx_og_id = wechat.wx_og_id;
                    model = EFHelper.AddWeChat<t_wx_media_materials>(model);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                model.wx_og_id = "";
                model = EFHelper.AddWeChat<t_wx_media_materials>(model);
            }

            return model;
        }

        /// <summary>
        /// 修改一条多媒体素材
        /// </summary>
        /// <param name="id">素材id</param>
        /// <param name="title">标题</param>
        /// <param name="remark">简介</param>
        /// <param name="group_id">分组ID</param>
        /// <param name="file_url">文件路径</param>
        /// <param name="file_ext">文件扩展名称</param>
        /// <param name="file_size">文件大小</param>
        /// <param name="v_type">视频区分类型Id(1-本地视频，2-微视）</param>
        /// <param name="media_id">微信上传多媒体返回的ID，群发时上传到微信服务器</param>
        /// <param name="hq_music_url">高质量音乐链接，WIFI环境优先使用该链接播放音乐</param>
        /// <param name="status">状态（1-可用，0-禁用）</param>
        /// <param name="is_public">是否公共素材（0-否，1-是）</param>
        /// <param name="is_newfile">是否上传了新文件</param>
        /// <returns></returns>
        public static t_wx_media_materials Update(int id, string title, string remark, int group_id, string file_url, string file_ext, string file_size, int v_type, string media_id, string hq_music_url, int status, int is_public, bool is_newfile = false)
        {
            var material = GetMediaMaterialByID(id);//获取素材
            if (material != null)
            {

                //参数赋值
                material.title = title  ;
                material.remark = remark  ;
                material.is_public = is_public;
                material.status = status;
                material.group_id = group_id;
                //material.wx_og_id = model.wx_og_id ?? material.wx_og_id;
                //material.wx_id = model.wx_id;
                material.file_url = file_url;
                material.file_ext = file_ext; ;
                material.file_size = file_size; ;
                material.video_type = material.channel_id == (int)media_type.素材视频 ? v_type : -1;
                material.hq_music_url = hq_music_url;
                material.media_id = is_newfile == true ? media_id : material.media_id;

                EFHelper.UpdateWeChat<t_wx_media_materials>(material);
            }
            return material;
        }


        #endregion

        #region 内部方法


        #region 其他通用方法
        static UploadMediaFileType GetUploadMediaTypeBymedia_type(media_type msgType)
        {
            UploadMediaFileType mediaType = UploadMediaFileType.image;
            switch (msgType)
            {
                case media_type.素材视频:
                    mediaType = UploadMediaFileType.video;
                    break;
                case media_type.素材图片库:
                    mediaType = UploadMediaFileType.image;
                    break;
                case media_type.图文模板图片库:
                    mediaType = UploadMediaFileType.image;
                    break;
                case media_type.素材语音:
                    mediaType = UploadMediaFileType.voice;
                    break;
                default:
                    mediaType = UploadMediaFileType.image;
                    break;
            }
            return mediaType;
        }
        #endregion

        #endregion


        public static bool UpdateStatus(int id, int status)
        {
            bool isFinish = false;
            using (creater_wxEntities db = new creater_wxEntities())
            {
                isFinish = db.t_wx_media_materials.Where(x => x.id == id).Update(x => new t_wx_media_materials() { status = status }) > 0;
            }
            return isFinish;
        }
    }
}
