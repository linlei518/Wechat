using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using KDWechat.Web.UI;
using KDWechat.Common;
using LitJson;
using System.Web.SessionState;
using System.Web.UI.HtmlControls;
using System.Drawing;
namespace KDWechat.Web.handles
{
    /// <summary>
    /// upload_ajax 的摘要说明
    /// </summary>
    public class upload_ajax : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            //取得处事类型
            string action = RequestHelper.GetQueryString("action");

            switch (action)
            {
                case "PathFile": //自定义的路劲的上传
                case "EditorFile": //编辑器文件
                    EditorFile(context);
                    break;
                case "EditorFile360": //360全景编辑器文件
                    EditorFile360(context);
                    break;
                case "ManagerFile": //管理文件
                    ManagerFile(context);
                    break;
                case "MultipleFile"://微相册专用
                    MultipleFile(context);
                    break;
                default: //普通上传
                    UpLoadFile(context);
                    break;
            }

        }

        #region 微相册专用===================================
        private void MultipleFile(HttpContext context)
        {
            BasePage bp = new BasePage();
            string folder =bp.folder;// RequestHelper.GetQueryString("folder"); //上传的文件夹
            string upload_type = RequestHelper.GetQueryString("upload_type");//上传类型（1、图片 2、音频 3 、视频  4、其他）
            string _upfilepath = context.Request.QueryString["UpFilePath"]; //取得上传的对象名称
            HttpPostedFile _upfile = context.Request.Files[_upfilepath];
            bool _iswater = false; //默认不打水印
            bool _isthumbnail = false; //默认不生成缩略图
            int wx_id = bp.wx_id;// RequestHelper.GetQueryInt("wx_id", bp.wx_id);
            int u_id = bp.u_id;// RequestHelper.GetQueryInt("u_id", bp.u_id);
            if (context.Request.QueryString["IsWater"] == "1")
                _iswater = true;
            if (context.Request.QueryString["IsThumbnail"] == "1")
                _isthumbnail = true;

            if (_upfile == null)
            {
                context.Response.Write("{\"msg\": 0, \"msgbox\": \"请选择要上传文件！\"}");
                return;
            }
            UpLoad upFiles = new UpLoad();
            //string msg = upFiles.fileSaveAs(_upfile, _isthumbnail, _iswater, false, false, folder, upload_type);
            string msg = upFiles.fileSaveAs(_upfile, folder, _isthumbnail, _iswater, upload_type, wx_id, u_id, 0, 0);
            //返回成功信息
            context.Response.Write(msg);
            context.Response.End();
        }
        #endregion

        #region 上传文件处理===================================

        private void UpLoadFile(HttpContext context)
        {
            BasePage bp = new BasePage();
            string folder =bp.folder;// RequestHelper.GetQueryString("folder"); //上传的文件夹
            string _delfile = RequestHelper.GetString("DelFilePath");
            HttpPostedFile _upfile = context.Request.Files["Filedata"];
            string upload_type = RequestHelper.GetQueryString("upload_type");//上传类型（1、图片 2、音频 3 、视频  4、其他）
            bool _iswater = false; //默认不打水印
            bool _isthumbnail = false; //默认不生成缩略图
            int write = RequestHelper.GetQueryInt("write", 1);
            
            int wx_id =bp.wx_id;// RequestHelper.GetQueryInt("wx_id", bp.wx_id);
            int u_id =bp.u_id;// RequestHelper.GetQueryInt("u_id", bp.u_id);
            int is_public = RequestHelper.GetQueryInt("is_public", 0);

            if (RequestHelper.GetQueryString("IsWater") == "1")
                _iswater = true;
            if (RequestHelper.GetQueryString("IsThumbnail") == "1")
                _isthumbnail = true;
            if (_upfile == null)
            {
                context.Response.Write("{\"status\": 0, \"msg\": \"请选择要上传文件\"}");
                return;
            }
            UpLoad upFiles = new UpLoad();
            string msg = upFiles.fileSaveAs(_upfile, folder, _isthumbnail, _iswater, upload_type, wx_id, u_id, write, is_public);
            //删除已存在的旧文件
            if (!string.IsNullOrEmpty(_delfile))
            {
                Utils.DeleteUpFile(_delfile);
            }
            //返回成功信息
            context.Response.Write(msg);
            context.Response.End();
        }
        #endregion

        #region 编辑器上传处理===================================
        private void EditorFile(HttpContext context)
        {
            BasePage bp = new BasePage();
            string folder =bp.folder;// RequestHelper.GetQueryString("folder"); //上传的文件夹
            string upload_type = RequestHelper.GetQueryString("upload_type");//上传类型（1、图片 2、音频 3 、视频  4、其他）
            bool _iswater = false; //默认不打水印
            if (RequestHelper.GetQueryString("IsWater") == "1")
                _iswater = true;
            HttpPostedFile imgFile = context.Request.Files["imgFile"];
            int write = RequestHelper.GetQueryInt("write", 1);
           
            int wx_id =bp.wx_id;// RequestHelper.GetQueryInt("wx_id", bp.wx_id);
            int u_id =bp.u_id;// RequestHelper.GetQueryInt("u_id", bp.u_id);
            int is_public = RequestHelper.GetQueryInt("is_public", 0);
            string old_file = RequestHelper.GetQueryString("old_file");
            if (imgFile == null)
            {
                showError(context, "请选择要上传文件");
                return;
            }
            UpLoad upFiles = new UpLoad();
            string remsg = upFiles.fileSaveAs(imgFile, folder, false, _iswater, upload_type, wx_id, u_id, write, is_public, old_file);
            JsonData jd = JsonMapper.ToObject(remsg);
            string status = jd["status"].ToString();
            string msg = jd["msg"].ToString();
            if (status == "0")
            {
                showError(context, msg);
                return;
            }
            string filePath = jd["path"].ToString(); //取得上传后的路径
            Hashtable hash = new Hashtable();
            hash["error"] = 0;
            hash["url"] = filePath;
            context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
            context.Response.Write(JsonMapper.ToJson(hash));
            context.Response.End();
        }
        //显示错误
        private void showError(HttpContext context, string message)
        {
            Hashtable hash = new Hashtable();
            hash["error"] = 1;
            hash["message"] = message;
            context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
            context.Response.Write(JsonMapper.ToJson(hash));
            context.Response.End();
        }
        #endregion

        #region 360全景图调用===================================

        #region 上传文件处理===================================

        private void UpLoadFile360(HttpContext context)
        {
            BasePage bp = new BasePage();
            string folder =bp.folder;// RequestHelper.GetQueryString("folder"); //上传的文件夹
            string _delfile = RequestHelper.GetString("DelFilePath");
            HttpPostedFile _upfile = context.Request.Files["Filedata"];
            string upload_type = RequestHelper.GetQueryString("upload_type");//上传类型（1、图片 2、音频 3 、视频  4、其他）
            string IsFor360 = RequestHelper.GetQueryString("IsFor360");//（1、来自360上传的文件 需要生产缩略图）
            string posation = context.Request["posation"].ToString();//上传图片的顺序
            string imgid = RequestHelper.GetQueryString("imgid");//来自360的图片 名称不要随机数生成 以id为准
            bool _iswater = false; //默认不打水印
            bool _isthumbnail = true; //默认不生成缩略图
            int write = RequestHelper.GetQueryInt("write", 1);
           
            int wx_id = bp.wx_id;// RequestHelper.GetQueryInt("wx_id", bp.wx_id);
            int u_id =bp.u_id;// RequestHelper.GetQueryInt("u_id", bp.u_id);

            if (RequestHelper.GetQueryString("IsWater") == "1")
                _iswater = true;
            if (RequestHelper.GetQueryString("IsThumbnail") == "1")
                _isthumbnail = true;
            if (_upfile == null)
            {
                context.Response.Write("{\"status\": 0, \"msg\": \"请选择要上传文件\"}");
                return;
            }
            UpLoad upFiles = new UpLoad();
            string msg = "";
            if (IsFor360 == "1")
            {
                msg = upFiles.For360fileSaveAs(_upfile, posation, imgid, folder, true, _iswater, upload_type, wx_id, u_id, write);
            }
            else
            {
                msg = upFiles.fileSaveAs(_upfile, folder, true, _iswater, upload_type, wx_id, u_id, write);
            }
            //删除已存在的旧文件
            if (!string.IsNullOrEmpty(_delfile))
            {
                Utils.DeleteUpFile(_delfile);
            }
            //返回成功信息
            context.Response.Write(msg);
            context.Response.End();
        }
        #endregion

        #region 编辑器上传处理===================================
        private void EditorFile360(HttpContext context)
        {
            BasePage bp = new BasePage();
            string folder =bp.folder;// RequestHelper.GetQueryString("folder"); //上传的文件夹
            string upload_type = RequestHelper.GetQueryString("upload_type");//上传类型（1、图片 2、音频 3 、视频  4、其他）
            string IsFor360 = RequestHelper.GetQueryString("IsFor360");//（1、来自360上传的文件 需要生产缩略图）
            string posation = context.Request["posation"].ToString();//上传图片的顺序
            string imgid = RequestHelper.GetQueryString("imgid");//来自360的图片 名称不要随机数生成 以id为准
            bool _iswater = false; //默认不打水印
            if (RequestHelper.GetQueryString("IsWater") == "1")
                _iswater = true;
            HttpPostedFile imgFile = context.Request.Files["imgFile"];
            int write = RequestHelper.GetQueryInt("write", 1);

            int wx_id = bp.wx_id;//RequestHelper.GetQueryInt("wx_id", bp.wx_id);
            int u_id =bp.u_id;// RequestHelper.GetQueryInt("u_id", bp.u_id);

            if (imgFile == null)
            {
                showError(context, "请选择要上传文件");
                return;
            }
            UpLoad upFiles = new UpLoad();

            string remsg = "";
            if (IsFor360 == "1")
            {
                remsg = upFiles.For360fileSaveAs(imgFile, posation, imgid, folder, true, _iswater, upload_type, wx_id, u_id, write);
            }
            else
            {
                remsg = upFiles.fileSaveAs(imgFile, folder, false, _iswater, upload_type, wx_id, u_id, write);
            }
            JsonData jd = JsonMapper.ToObject(remsg);
            string status = jd["status"].ToString();
            string msg = jd["msg"].ToString();
            if (status == "0")
            {
                showError(context, msg);
                return;
            }
            string filePath = jd["path"].ToString(); //取得上传后的路径

            Hashtable hash = new Hashtable();
            hash["error"] = 0;
            hash["url"] = filePath;
            context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
            context.Response.Write(JsonMapper.ToJson(hash));
            context.Response.End();
        }

        #endregion
        #endregion


        #region 浏览文件处理=====================================
        private void ManagerFile(HttpContext context)
        {
            BasePage bp = new BasePage();
            string folder =bp.folder;// RequestHelper.GetQueryString("folder"); //上传的文件夹
            Common.Config.siteconfig siteConfig = new BLL.Config.siteconfig().loadConfig();
            string upload_type = RequestHelper.GetQueryString("upload_type");//上传类型（1、图片 2、音频 3 、视频  4、其他）
            //String aspxUrl = context.Request.Path.Substring(0, context.Request.Path.LastIndexOf("/") + 1);

            //根目录路径，相对路径
            String rootPath = siteConfig.webpath + siteConfig.filepath + "/" + folder + "/"; //站点目录+上传目录
            //根目录URL，可以指定绝对路径，比如 http://www.yoursite.com/attached/
            String rootUrl = siteConfig.webpath + siteConfig.filepath + "/" + folder + "/";
            string path_zidingyi = GetFolderByType(upload_type);
            rootPath += path_zidingyi;
            rootUrl += path_zidingyi;


            //图片扩展名
            String fileTypes = "gif,jpg,jpeg,png,bmp";

            String currentPath = "";
            String currentUrl = "";
            String currentDirPath = "";
            String moveupDirPath = "";

            String dirPath = Utils.GetMapPath(rootPath);
            String dirName = RequestHelper.GetQueryString("dir");

            //根据path参数，设置各路径和URL
            String path = RequestHelper.GetQueryString("path");
            path = String.IsNullOrEmpty(path) ? "" : path;
            if (path == "")
            {
                currentPath = dirPath;
                currentUrl = rootUrl;
                currentDirPath = "";
                moveupDirPath = "";
            }
            else
            {
                currentPath = dirPath + path;
                currentUrl = rootUrl + path;
                currentDirPath = path;
                moveupDirPath = Regex.Replace(currentDirPath, @"(.*?)[^\/]+\/$", "$1");
            }

            //排序形式，name or size or type
            String order = RequestHelper.GetQueryString("order");
            order = String.IsNullOrEmpty(order) ? "" : order.ToLower();

            //不允许使用..移动到上一级目录
            if (Regex.IsMatch(path, @"\.\."))
            {
                context.Response.Write("Access is not allowed.");
                context.Response.End();
            }
            //最后一个字符不是/
            if (path != "" && !path.EndsWith("/"))
            {
                context.Response.Write("Parameter is not valid.");
                context.Response.End();
            }
            //目录不存在或不是目录
            if (!Directory.Exists(currentPath))
            {
                Hashtable result1 = new Hashtable();
                result1["moveup_dir_path"] = moveupDirPath;
                result1["current_dir_path"] = currentDirPath;
                result1["current_url"] = currentUrl;
                result1["total_count"] = 0;
                List<Hashtable> dirFileList1 = new List<Hashtable>();
                result1["file_list"] = dirFileList1;

                context.Response.AddHeader("Content-Type", "application/json; charset=UTF-8");
                context.Response.Write(JsonMapper.ToJson(result1));
                context.Response.End();
                //context.Response.Write("Directory does not exist.");
                //context.Response.End();
            }

            //遍历目录取得文件信息
            string[] dirList = Directory.GetDirectories(currentPath);
            string[] fileList = Directory.GetFiles(currentPath);

            switch (order)
            {
                case "size":
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new SizeSorter());
                    break;
                case "type":
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new TypeSorter());
                    break;
                case "name":
                default:
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new NameSorter());
                    break;
            }

            Hashtable result = new Hashtable();
            result["moveup_dir_path"] = moveupDirPath;
            result["current_dir_path"] = currentDirPath;
            result["current_url"] = currentUrl;
            result["total_count"] = dirList.Length + fileList.Length;
            List<Hashtable> dirFileList = new List<Hashtable>();
            result["file_list"] = dirFileList;
            for (int i = 0; i < dirList.Length; i++)
            {
                DirectoryInfo dir = new DirectoryInfo(dirList[i]);
                Hashtable hash = new Hashtable();
                hash["is_dir"] = true;
                hash["has_file"] = (dir.GetFileSystemInfos().Length > 0);
                hash["filesize"] = 0;
                hash["is_photo"] = false;
                hash["filetype"] = "";
                hash["filename"] = dir.Name;
                hash["datetime"] = dir.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                dirFileList.Add(hash);
            }
            for (int i = 0; i < fileList.Length; i++)
            {
                FileInfo file = new FileInfo(fileList[i]);
                Hashtable hash = new Hashtable();
                hash["is_dir"] = false;
                hash["has_file"] = false;
                hash["filesize"] = file.Length;
                hash["is_photo"] = (Array.IndexOf(fileTypes.Split(','), file.Extension.Substring(1).ToLower()) >= 0);
                hash["filetype"] = file.Extension.Substring(1);
                hash["filename"] = file.Name;
                hash["datetime"] = file.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                dirFileList.Add(hash);
            }
            context.Response.AddHeader("Content-Type", "application/json; charset=UTF-8");
            context.Response.Write(JsonMapper.ToJson(result));
            context.Response.End();
        }

        #region Helper
        public class NameSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                if (y == null)
                {
                    return 1;
                }
                FileInfo xInfo = new FileInfo(x.ToString());
                FileInfo yInfo = new FileInfo(y.ToString());

                return xInfo.FullName.CompareTo(yInfo.FullName);
            }
        }

        public class SizeSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                if (y == null)
                {
                    return 1;
                }
                FileInfo xInfo = new FileInfo(x.ToString());
                FileInfo yInfo = new FileInfo(y.ToString());

                return xInfo.Length.CompareTo(yInfo.Length);
            }
        }

        public class TypeSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                if (y == null)
                {
                    return 1;
                }
                FileInfo xInfo = new FileInfo(x.ToString());
                FileInfo yInfo = new FileInfo(y.ToString());

                return xInfo.Extension.CompareTo(yInfo.Extension);
            }
        }
        #endregion
        #endregion




        private string GetFolderByType(string type)
        {
            string str = "0";
            int _upload_type = Common.Utils.StrToInt(type, 0);
            //判断上传的类型
            switch (_upload_type)
            {
                case (int)media_type.素材图片库:
                    str = "material/images/";
                    break;
                case (int)media_type._360全景图片:
                    str = "360view/images/";
                    break;
                case (int)media_type.公众号头像:
                    str = "wechathead/images/";
                    break;
                case (int)media_type.图文模板图片库:
                    str = "newstemplate/images/";
                    break;
                case (int)media_type.项目模块:
                    str = "projects/images/";
                    break;
                case (int)media_type.销售头像图片:
                    str = "sales/images/";
                    break;
                case (int)media_type.站内信图片:

                    str = "msg/images/";
                    break;

                case (int)media_type.消息订阅图片:

                    str = "news_subscription/images/";
                    break;
                case (int)media_type.帮助中心图片:
                    str = "help/images/";
                    break;
                case (int)media_type.刮刮卡图片:
                    str = "card/images/";
                    break;
                case (int)media_type.微邀请图片:
                    str = "invite/images/";
                    break;
                case (int)media_type.微邀请音频:
                    str = "invite/voices/";
                    break;
                case (int)media_type.素材语音:
                    str = "material/voices/";
                    break;
                case (int)media_type.素材视频:
                    str = "material/videos/";
                    break;
                case (int)media_type.手机站广告位图片:
                    str = "mobile_adert/images/";
                    break;
                default:

                    str = "file/";
                    break;

            }
            return str;
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