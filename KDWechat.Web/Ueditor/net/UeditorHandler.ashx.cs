using KDWechat.Common;
using KDWechat.Web.handles;
using KDWechat.Web.UI;
using LitJson;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;


namespace KDWechat.Web.Ueditor.net
{
    /// <summary>
    /// UeditorHandler 的摘要说明
    /// </summary>
    public class UeditorHandler : IHttpHandler, IRequiresSessionState
    {
        
        //public HttpServerUtility Server { get; private set; }
        public void ProcessRequest(HttpContext context)
        {
            Handler action = null;
            switch (context.Request["action"])
            {
                case "config":
                    action = new ConfigHandler(context);
                    break;
                case "uploadimage":
                    EditorFile(context);
                    //action = new UploadHandler(context, new UploadConfig()
                    //{
                    //    AllowExtensions = Config.GetStringList("imageAllowFiles"),
                    //    PathFormat = Config.GetString("imagePathFormat"),
                    //    SizeLimit = Config.GetInt("imageMaxSize"),
                    //    UploadFieldName = Config.GetString("imageFieldName")
                    //});
                    break;
                case "uploadscrawl":
                    action = new UploadHandler(context, new UploadConfig()
                    {
                        AllowExtensions = new string[] { ".png" },
                        PathFormat = Config.GetString("scrawlPathFormat"),
                        SizeLimit = Config.GetInt("scrawlMaxSize"),
                        UploadFieldName = Config.GetString("scrawlFieldName"),
                        Base64 = true,
                        Base64Filename = "scrawl.png"
                    });
                    break;
                case "uploadvideo":
                    action = new UploadHandler(context, new UploadConfig()
                    {
                        AllowExtensions = Config.GetStringList("videoAllowFiles"),
                        PathFormat = Config.GetString("videoPathFormat"),
                        SizeLimit = Config.GetInt("videoMaxSize"),
                        UploadFieldName = Config.GetString("videoFieldName")
                    });
                    break;
                case "uploadfile":
                    action = new UploadHandler(context, new UploadConfig()
                    {
                        AllowExtensions = Config.GetStringList("fileAllowFiles"),
                        PathFormat = Config.GetString("filePathFormat"),
                        SizeLimit = Config.GetInt("fileMaxSize"),
                        UploadFieldName = Config.GetString("fileFieldName")
                    });
                    break;
                case "listimage":
                    //action = new ListFileManager(context, Config.GetString("imageManagerListPath"), Config.GetStringList("imageManagerAllowFiles"));
                    GetListimage(context);
                    break;
                case "listfile":
                    action = new ListFileManager(context, Config.GetString("fileManagerListPath"), Config.GetStringList("fileManagerAllowFiles"));
                    break;
                case "catchimage":
                    action = new CrawlerHandler(context);
                    break;
                default:
                    action = new NotSupportedHandler(context);
                    break;
            }
            action.Process();
        }

        private void GetListimage(HttpContext context) 
        {
            BasePage bp = new BasePage();
            string folder =bp.folder;// RequestHelper.GetQueryString("folder"); //上传的文件夹
            string upload_type = RequestHelper.GetQueryString("upload_type");//上传类型（1、图片 2、音频 3 、视频  4、其他）
            bool _iswater = false; //默认不打水印
            if (RequestHelper.GetQueryString("IsWater") == "1")
                _iswater = true;
            
            int write = RequestHelper.GetQueryInt("write", 1);
          
            int wx_id =bp.wx_id;// RequestHelper.GetQueryInt("wx_id", bp.wx_id);
            int u_id = bp.u_id;// RequestHelper.GetQueryInt("u_id", bp.u_id);
            int is_public = RequestHelper.GetQueryInt("is_public", 0);
            string old_file = RequestHelper.GetQueryString("old_file");
            //string fileExt = Utils.GetFileExt(postedFile.FileName)
            UpLoad upFiles = new UpLoad();
            string urlpath = upFiles.GetUpLoadPath(folder, "bmp", upFiles.GetFolderByType(upload_type));
            String[] SearchExtensions;
            SearchExtensions = Config.GetStringList("imageManagerAllowFiles").Select(x => x.ToLower()).ToArray();
            try
            {
                GetListimageEntity Result = new GetListimageEntity();
                Result.start = String.IsNullOrEmpty(context.Request["start"]) ? 0 : Convert.ToInt32(context.Request["start"]);
                Result.size = String.IsNullOrEmpty(context.Request["size"]) ? Config.GetInt("imageManagerListSize") : Convert.ToInt32(context.Request["size"]);
                var buildingList = new List<String>();

                var localPath = System.Web.HttpContext.Current.Server.MapPath(urlpath);
                buildingList.AddRange(Directory.GetFiles(localPath, "*", SearchOption.AllDirectories)
                    .Where(x => SearchExtensions.Contains(Path.GetExtension(x).ToLower()))
                    .Select(x => urlpath + x.Substring(localPath.Length).Replace("\\", "/")));
                Result.total = buildingList.Count;
                Result.list = buildingList.OrderBy(x => x).Skip(Result.start).Take(Result.size).ToArray().Select(x => new { url = x });
                               
                Result.state = "SUCCESS";
                string json = JsonConvert.SerializeObject(Result);
                context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
                context.Response.Write(json);
                context.Response.End();
            }
            catch (FormatException)
            {
                return;
            }
                    
        }

        private void EditorFile(HttpContext context)
        {
            BasePage bp = new BasePage();
            string folder =bp.folder;// RequestHelper.GetQueryString("folder"); //上传的文件夹
            string upload_type = RequestHelper.GetQueryString("upload_type");//上传类型（1、图片 2、音频 3 、视频  4、其他）
            bool _iswater = false; //默认不打水印
            if (RequestHelper.GetQueryString("IsWater") == "1")
                _iswater = true;
            /*upfile为config.json配置文件中imageFieldName配置值*/
            HttpPostedFile imgFile = context.Request.Files["upfile"];
            int write = RequestHelper.GetQueryInt("write", 1);
           
            int wx_id =bp.wx_id;// RequestHelper.GetQueryInt("wx_id", bp.wx_id);
            int u_id = bp.u_id;//RequestHelper.GetQueryInt("u_id", bp.u_id);
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
            #region 封装ueditor编辑器所需返回值
                        
            EuUploadResult Result = new EuUploadResult();
            Result.state = GetStateMessage(status);
            Result.url = filePath;
            Result.original = jd["name"].ToString();
            Result.title = jd["name"].ToString();
            string json = JsonConvert.SerializeObject(Result);

            #endregion
            context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
            context.Response.Write(json);
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
       

        private string GetStateMessage(string state)
        {
            switch (state)
            {
                case "1":
                    return "SUCCESS";              
            }
            return "未知错误";
        }

        //public EuUploadResult Result { get; private set; }
        public class EuUploadResult
        {
            public string state { get; set; }
            public string url { get; set; }
            public string title { get; set; }

            public string original { get; set; }

            public string error { get; set; }
        }

        public class GetListimageEntity
        {
            public string state;
            public int size;
            public int total;
            public int start;
            public String PathToList;
            public object list;
            
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