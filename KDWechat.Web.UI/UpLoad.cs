using System;
using System.Collections;
using System.Web;
using System.IO;
using System.Drawing;
using System.Net;
using System.Configuration;
using KDWechat.Common;
using KDWechat.Common.Config;
using System.Web.SessionState;
using System.Text.RegularExpressions;
//using Shell32;

namespace KDWechat.Web.UI
{
    public class UpLoad
    {
        protected internal siteconfig siteConfig;
        public UpLoad()
        {
            siteConfig = new BLL.Config.siteconfig().loadConfig();
        }

        /// <summary>
        /// 裁剪图片并保存
        /// </summary>
        public bool cropSaveAs(string fileName, string newFileName, int maxWidth, int maxHeight, int cropWidth, int cropHeight, int X, int Y)
        {
            string fileExt = Utils.GetFileExt(fileName); //文件扩展名，不含“.”
            if (!Common.Utils.IsImage(fileExt))
            {
                return false;
            }
            string newFileDir = Utils.GetMapPath(newFileName.Substring(0, newFileName.LastIndexOf(@"/") + 1));
            //检查是否有该路径，没有则创建
            if (!Directory.Exists(newFileDir))
            {
                Directory.CreateDirectory(newFileDir);
            }
            try
            {
                string fileFullPath = Utils.GetMapPath(fileName);
                string toFileFullPath = Utils.GetMapPath(newFileName);
                return Thumbnail.MakeThumbnailImage(fileFullPath, toFileFullPath, 180, 180, cropWidth, cropHeight, X, Y);
            }
            catch
            {
                return false;
            }
        }

        public string GetFolderByType(string type)
        {
            string str = "0";
            int _upload_type = Common.Utils.StrToInt(type, 0);
            //判断上传的类型
            switch (_upload_type)
            {
                case (int)media_type.素材图片库:
                    str = "material";
                    break;
                case (int)media_type._360全景图片:
                    str = "360view";
                    break;
                case (int)media_type.公众号头像:
                    str = "wechathead";
                    break;
                case (int)media_type.图文模板图片库:
                    str = "newstemplate";
                    break;
                case (int)media_type.项目模块:
                    str = "projects";
                    break;
                case (int)media_type.销售头像图片:
                    str = "sales";
                    break;
                case (int)media_type.站内信图片:

                    str = "msg";
                    break;
                case (int)media_type.消息订阅图片:

                    str = "news_subscription";
                    break;
                case (int)media_type.帮助中心图片:
                    str = "help";
                    break;
                case (int)media_type.刮刮卡图片:
                    str = "card";
                    break;
                case (int)media_type.微邀请图片:
                    str = "invite";
                    break;
                case (int)media_type.微邀请音频:
                    str = "invite";
                    break;
                case (int)media_type.素材语音:
                    str = "material";
                    break;
                case (int)media_type.素材视频:
                    str = "material";
                    break;
                case (int)media_type.微相册图片:
                    str = "ablum";
                    break;
                case (int)media_type.手机站广告位图片:
                    str = "mobile_adert";
                    break;
            }
            return str;
        }


        /// <summary>
        /// 文件上传方法
        /// </summary>
        /// <param name="postedFile">文件流</param>
        ///  <param name="folder">存放的文件夹</param>
        /// <param name="isThumbnail">是否生成缩略图</param>
        /// <param name="isWater">是否打水印</param>
        /// <param name="upload_type">上传类型</param>
        /// <param name="wx_id">微信号id</param>
        /// <param name="u_id">当前用户id</param>
        /// <param name="write">是否写入数据库</param>
        /// <param name="is_public">是否公共</param>
        /// <returns>上传后文件信息</returns>
        public string fileSaveAs(HttpPostedFile postedFile, string folder, bool isThumbnail, bool isWater, string upload_type, int wx_id, int u_id, int write = 1, int is_public = 0, string old_file = "")
        {
            try
            {
                string fileExt = Utils.GetFileExt(postedFile.FileName); //文件扩展名，不含“.”
                int fileSize = postedFile.ContentLength; //获得文件大小，以字节为单位
                string fileName = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf(@"\") + 1); //取得原文件名
                string newFileName = Utils.GetRamCode() + "." + fileExt; //随机生成新的文件名
                string newThumbnailFileName = "thumb_" + newFileName; //随机生成缩略图文件名
                string upLoadPath = GetUpLoadPath(folder, fileExt, GetFolderByType(upload_type)); //上传目录相对路径
                string fullUpLoadPath = Utils.GetMapPath(upLoadPath); //上传目录的物理路径
                string newFilePath = upLoadPath + newFileName; //上传后的路径
                string newThumbnailPath = upLoadPath + newThumbnailFileName; //上传后的缩略图路径
                string fileCode = postedFile.InputStream.ReadByte().ToString() + postedFile.InputStream.ReadByte().ToString();//文件真实类型代码  

                string errorMsg = "";
                int _upload_type = Common.Utils.StrToInt(upload_type, 0);
                bool is_add = false;
                //判断上传的类型
                switch (_upload_type)
                {
                    case (int)media_type.素材图片库:
                    case (int)media_type._360全景图片:
                    case (int)media_type.公众号头像:
                    case (int)media_type.图文模板图片库:
                    case (int)media_type.项目模块:
                    case (int)media_type.销售头像图片:
                    case (int)media_type.消息订阅图片:
                    case (int)media_type.刮刮卡图片:
                    case (int)media_type.帮助中心图片:
                    case (int)media_type.微邀请图片:
                    case (int)media_type.微相册图片:
                    case (int)media_type.手机站广告位图片:
                        is_add = true;
                        errorMsg = "{\"status\": 0, \"msg\": \"请上传bmp, png, jpeg, jpg, gif类型的图片\"}";
                        if (Utils.IsImage(fileExt) == false)
                        {
                            return errorMsg;
                        }
                        break;
                    case (int)media_type.站内信图片:
                   
                        errorMsg = "{\"status\": 0, \"msg\": \"请上传bmp, png, jpeg, jpg, gif类型的图片\"}";
                        if (Utils.IsImage(fileExt) == false)
                        {
                            return errorMsg;
                        }
                        break;
                    case (int)media_type.素材语音:
                        errorMsg = "{\"status\": 0, \"msg\": \"请上传mp3, amr类型的语音文件\"}";
                        if (Utils.IsVoice(fileExt) == false)
                        {

                            return errorMsg;
                        }
                        break;
                    case (int)media_type.微邀请音频:
                        errorMsg = "{\"status\": 0, \"msg\": \"请上传mp3类型的语音文件\"}";
                        if (Utils.IsVoice(fileExt) == false)
                        {
                            return errorMsg;
                        }
                        break;
                    case (int)media_type.素材视频:
                        errorMsg = "{\"status\": 0, \"msg\": \"请上传 rm, rmvb, wmv, avi, mpg, mpeg, mp4类型的视频文件\"}";
                        if (Utils.IsVideo(fileExt) == false)
                        {

                            return errorMsg;
                        }
                        break;
                    default:
                        //检查文件扩展名是否合法
                        if (!CheckFileExt(fileExt))
                        {
                            return "{\"status\": 0, \"msg\": \"不允许上传" + fileExt + "类型的文件\"}";
                        }
                        break;
                }

                //检测文件是否是真实的文件
                if (!CheckFileIsTrue(fileCode, fileExt))
                {
                    return errorMsg;
                }



                //检查文件大小是否合法
                if (!CheckFileSize(fileExt, fileSize))
                {
                    return "{\"status\": 0, \"msg\": \"文件超过限制的大小啦\"}";
                }



                //检查上传的物理路径是否存在，不存在则创建
                if (!Directory.Exists(fullUpLoadPath))
                {
                    Directory.CreateDirectory(fullUpLoadPath);
                }

                //保存文件
                postedFile.SaveAs(fullUpLoadPath + newFileName);
                string time_length = "";
                if (Utils.IsVoice(fileExt))
                {
                    #region 处理音频长度，暂不用了

                    //FileStream fs = new FileStream(Utils.GetMapPath("/upload/log.txt"), FileMode.Append);
                    //StreamWriter sw = new StreamWriter(fs);


                    // string strFileName = fullUpLoadPath + newFileName;
                    //// sw.WriteLine("文件路劲：" + strFileName);
                    // string dirName = System.IO.Path.GetDirectoryName(strFileName);
                    // string SongName = System.IO.Path.GetFileName(strFileName);//获得歌曲名称
                    // FileInfo fInfo = new FileInfo(strFileName);
                    // ShellClass sh = new ShellClass();
                    // //sw.WriteLine("ShellClass对象：" + sh);
                    // Folder dir = sh.NameSpace(dirName);
                    //// sw.WriteLine("Folder对象：" + dir);
                    // FolderItem item = dir.ParseName(SongName);
                    //// sw.WriteLine("FolderItem对象：" + item);
                    // //sw.WriteLine("time_length对象：" + dir.GetDetailsOf(item, -1));
                    // //sw.Close();
                    // //fs.Close();
                    //  time_length = Regex.Match(dir.GetDetailsOf(item, -1), "\\d:\\d{2}:\\d{2}").Value;//获取歌曲时间
                    // if (!string.IsNullOrEmpty(time_length))
                    // {
                    //     string[] list = time_length.Split(new char[] { ':' });
                    //     if (list.Length==3)
                    //     {
                    //         int m = Utils.StrToInt(list[1],0);
                    //         int s = Utils.StrToInt(list[2], 0);
                    //         if (m>0 && s>0)
                    //         {
                    //             File.Delete(fullUpLoadPath + newFileName);
                    //             return "{\"status\": 0, \"msg\": \"音频文件的播放长度不能超过60s\"}";
                    //         }
                    //         else if (m>1)
                    //         {
                    //             File.Delete(fullUpLoadPath + newFileName);
                    //             return "{\"status\": 0, \"msg\": \"音频文件的播放长度不能超过60s\"}";
                    //         }
                    //     }
                    // } 
                    #endregion


                }

                if (old_file.Trim().Length > 0)
                {
                    try
                    {
                        if (File.Exists(HttpContext.Current.Server.MapPath(old_file)))
                        {
                            File.Delete(HttpContext.Current.Server.MapPath(old_file));
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                //如果是图片，检查图片是否超出最大尺寸，是则裁剪
                if (Utils.IsImage(fileExt) && (this.siteConfig.imgmaxheight > 0 || this.siteConfig.imgmaxwidth > 0))
                {
                    Thumbnail.MakeThumbnailImage(fullUpLoadPath + newFileName, fullUpLoadPath + newFileName,
                        this.siteConfig.imgmaxwidth, this.siteConfig.imgmaxheight);
                }
                //如果是图片，检查是否需要生成缩略图，是则生成
                if (Utils.IsImage(fileExt) && isThumbnail && this.siteConfig.thumbnailwidth > 0 && this.siteConfig.thumbnailheight > 0)
                {
                    Thumbnail.MakeThumbnailImage(fullUpLoadPath + newFileName, fullUpLoadPath + newThumbnailFileName,
                        this.siteConfig.thumbnailwidth, this.siteConfig.thumbnailheight, "Cut");
                }
                //如果是图片，检查是否需要打水印
                if (IsWaterMark(fileExt) && isWater)
                {
                    switch (this.siteConfig.watermarktype)
                    {
                        case 1:
                            WaterMark.AddImageSignText(newFilePath, newFilePath,
                                this.siteConfig.watermarktext, this.siteConfig.watermarkposition,
                                this.siteConfig.watermarkimgquality, this.siteConfig.watermarkfont, this.siteConfig.watermarkfontsize);
                            break;
                        case 2:
                            WaterMark.AddImageSignPic(newFilePath, newFilePath,
                                this.siteConfig.watermarkpic, this.siteConfig.watermarkposition,
                                this.siteConfig.watermarkimgquality, this.siteConfig.watermarktransparency);
                            break;
                    }
                }


                if (write == 1 && is_add == true)
                {
                    try
                    {

                        KDWechat.BLL.Chats.wx_media_materials.Add((is_public == 1 ? 0 : wx_id), u_id, fileName, "", 0, newFilePath, fileExt, fileSize.ToString(), (media_type)Enum.Parse(typeof(media_type), upload_type), -1, "", "", DateTime.Now.AddDays(-7), 1, is_public);


                    }
                    catch (Exception)
                    {
                    }
                }
                //处理完毕，返回JOSN格式的文件信息
                return "{\"status\": 1, \"msg\": \"上传文件成功\", \"name\": \""
                    + fileName + "\", \"path\": \"" + newFilePath + "\", \"thumb\": \""
                    + newThumbnailPath + "\", \"size\": " + fileSize + ", \"ext\": \"" + fileExt + "\"}";
            }
            catch
            {
                return "{\"status\": 0, \"msg\": \"上传过程中发生意外错误\"}";
            }
        }

        #region 私有方法

        /// <summary>
        /// 返回上传目录相对路径
        /// </summary>
        /// <param name="fileName">上传文件名</param>
        public string GetUpLoadPath(string wx_guid, string fileExt, string upload_type)
        {
            string path = siteConfig.webpath + siteConfig.filepath + "/" + wx_guid + "/"; //站点目录+上传目录
            switch (fileExt.ToLower())
            {
                case "gif":
                case "png":
                case "jpg":
                case "jpeg":
                case "bmp":
                    path += upload_type + "/images/";
                    break;
                case "mp4":
                case "rm":
                case "avi":
                case "rmvb":
                case "wmv":
                case "mpg":
                case "mpeg":
                    path += upload_type + "/videos/";
                    break;
                case "mp3":
                case "amr":
                case "wma":
                case "wav":
                    path += upload_type + "/voices/";
                    break;
                default:
                    path += upload_type + "/files/";
                    break;
            }
            switch (this.siteConfig.filesave)
            {
                case 1: //按年月日每天一个文件夹
                    path += DateTime.Now.ToString("yyyyMMdd");
                    break;
                case 2: //按年月一个文件夹
                    path += DateTime.Now.ToString("yyyyMM");
                    break;
                default: //按年月/日存入不同的文件夹
                    path += DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.ToString("dd");
                    break;
            }
            return path + "/";
        }

        /// <summary>
        /// 是否需要打水印
        /// </summary>
        /// <param name="_fileExt">文件扩展名，不含“.”</param>
        private bool IsWaterMark(string _fileExt)
        {
            //判断是否开启水印
            if (this.siteConfig.watermarktype > 0)
            {
                //判断是否可以打水印的图片类型
                ArrayList al = new ArrayList();
                al.Add("bmp");
                al.Add("jpeg");
                al.Add("jpg");
                al.Add("png");
                if (al.Contains(_fileExt.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }



        /// <summary>
        /// 检查是否为合法的上传文件
        /// </summary>
        private bool CheckFileExt(string _fileExt)
        {
            //检查危险文件
            string[] excExt = { "asp", "aspx", "php", "jsp", "htm", "html" };
            for (int i = 0; i < excExt.Length; i++)
            {
                if (excExt[i].ToLower() == _fileExt.ToLower())
                {
                    return false;
                }
            }
            //检查合法文件
            string[] allowExt = this.siteConfig.fileextension.Split(',');
            for (int i = 0; i < allowExt.Length; i++)
            {
                if (allowExt[i].ToLower() == _fileExt.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 检查文件大小是否合法
        /// </summary>
        /// <param name="_fileExt">文件扩展名，不含“.”</param>
        /// <param name="_fileSize">文件大小(B)</param>
        private bool CheckFileSize(string _fileExt, int _fileSize)
        {
            //判断是否为图片文件
            if (Utils.IsImage(_fileExt))
            {
                if (_fileSize > 1024 * 1024)
                {
                    return false;
                }
            }
            else if (Utils.IsVoice(_fileExt))
            {
                if (_fileSize > 2048 * 1024)
                {
                    return false;
                }
            }
            else if (Utils.IsVideo(_fileExt))
            {
                if (_fileSize > 20480 * 1024)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 检测文件的真实性
        /// </summary>
        /// <param name="file_code">实际上传的文件代码</param>
        /// <param name="file_ext">文件扩展名，不含“.”</param>
        /// <returns></returns>
        private bool CheckFileIsTrue(string file_code, string file_ext)
        {
            /*文件扩展名说明 
            *7173        gif  
            *255216      jpg 
            *13780       png 
            *6677        bmp 
            *239187      txt,aspx,asp,sql 
            *208207      xls.doc.ppt 
            *6063        xml 
            *6033        htm,html 
            *4742        js 
            *8075        xlsx,zip,pptx,mmap,zip 
            *8297        rar    
            *01          accdb,mdb 
            *7790        exe,dll            
            *5666        psd  
            *255254      rdp  
            *10056       bt种子  
            *64101       bat  
            
            mp3         7368
            flv         7076
            avi         8273
            wav         8273
             
            amr         3533
             
            wmv         4838
            wma         4838
             
            rm          4682
            rmvb        4682
             
            mp4         00
            mpg         00
            mpeg        00
            */
            bool isc = false;
            if (file_code != "-1-1") //-1-1表示是空文件，针对office等文件
            {
                string file_code_true = "";
                switch (file_ext.ToLower())
                {
                    case "gif":
                        file_code_true = "7173";
                        break;
                    case "jpg":
                    case "jpeg":
                        file_code_true = "255216";
                        break;
                    case "png":
                        file_code_true = "13780";
                        break;
                    case "bmp":
                        file_code_true = "6677";
                        break;
                    case "mp4":
                    case "mpeg":
                    case "mpg":
                        file_code_true = "00";
                        break;
                    case "wmv":
                    case "wma":
                        file_code_true = "4838";
                        break;
                    case "rm":
                    case "rmvb":
                        file_code_true = "4682";
                        break;
                    case "mp3":
                        file_code_true = "7368";
                        break;
                    case "flv":
                        file_code_true = "7076";
                        break;
                    case "amr":
                        file_code_true = "3533";
                        break;
                    case "avi":
                    case "wav":
                        file_code_true = "8273";
                        break;
                    case "xls":
                    case "doc":
                    case "ppt":
                        file_code_true = "208207";
                        break;
                    case "xlsx":
                    case "zip":
                    case "docx":
                    case "pptx":
                        file_code_true = "8075";
                        break;
                    case "rar":
                        file_code_true = "8297";
                        break;
                }
                if (file_code == file_code_true)
                {
                    isc = true;
                }
            }
            else
            {
                isc = true;
            }


            return isc;
        }

        #endregion

        /// <summary>
        /// 上传自定义文件(目前不用)
        /// </summary>
        /// <param name="_upfile">上传的对象名称</param>
        /// <param name="_path">自定义上传的文件夹</param>
        /// <param name="is_cover">是否覆盖已存在的文件</param>
        /// <param name="is_rename">是否重命名文件</param>
        /// <param name="upload_type">上传类型（1、图片 2、音频 3 、视频  4、其他）</param>
        /// <returns></returns>
        private string fileSaveAs(HttpPostedFile postedFile, string folder, string is_cover, string is_rename, string upload_type, bool isWater, int wx_id, int u_id, int write = 0, int is_public = 0)
        {
            try
            {
                bool isThumbnail = false;
                string fileExt = Utils.GetFileExt(postedFile.FileName); //文件扩展名，不含“.”
                int fileSize = postedFile.ContentLength; //获得文件大小，以字节为单位
                string orgFileName = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf(@"\") + 1); //取得原文件名

                string newFileName = Utils.GetRamCode() + "." + fileExt; //随机生成新的文件名
                if (is_rename == "0")
                {
                    newFileName = orgFileName;
                }
                string newThumbnailFileName = "thumb_" + newFileName; //随机生成缩略图文件名
                string upLoadPath = GetUpLoadPath(folder, fileExt, GetFolderByType(upload_type)); //上传目录相对路径
                string fullUpLoadPath = Utils.GetMapPath(upLoadPath); //上传目录的物理路径
                string newFilePath = upLoadPath + newFileName; //上传后的路径
                string newThumbnailPath = upLoadPath + newThumbnailFileName; //上传后的缩略图路径

                string fileCode = postedFile.InputStream.ReadByte().ToString() + postedFile.InputStream.ReadByte().ToString();//文件真实类型代码  

                string errorMsg = "";
                int _upload_type = Common.Utils.StrToInt(upload_type, 0);
                //判断上传的类型
                switch (_upload_type)
                {
                    case (int)media_type.素材图片库:
                    case (int)media_type._360全景图片:
                    case (int)media_type.公众号头像:
                    case (int)media_type.图文模板图片库:
                    case (int)media_type.项目模块:
                    case (int)media_type.销售头像图片:
                    case (int)media_type.站内信图片:
                        errorMsg = "{\"status\": 0, \"msg\": \"请上传bmp, png, jpeg, jpg, gif类型的图片\"}";
                        if (Utils.IsImage(fileExt) == false)
                        {

                            return errorMsg;
                        }
                        break;
                    case (int)media_type.素材语音:
                        errorMsg = "{\"status\": 0, \"msg\": \"请上传mp3, amr类型的语音文件\"}";
                        if (Utils.IsVoice(fileExt) == false)
                        {

                            return errorMsg;
                        }
                        break;
                    case (int)media_type.素材视频:
                        errorMsg = "{\"status\": 0, \"msg\": \"请上传 rm, rmvb, wmv, avi, mpg, mpeg, mp4类型的视频文件\"}";
                        if (Utils.IsVideo(fileExt) == false)
                        {

                            return errorMsg;
                        }
                        break;
                    default:
                        //检查文件扩展名是否合法
                        if (!CheckFileExt(fileExt))
                        {
                            return "{\"status\": 0, \"msg\": \"不允许上传" + fileExt + "类型的文件\"}";
                        }
                        break;
                }

                //检测文件是否是真实的文件
                if (!CheckFileIsTrue(fileCode, fileExt))
                {
                    return errorMsg;
                }




                //检查文件大小是否合法
                if (!CheckFileSize(fileExt, fileSize))
                {
                    return "{\"status\": 0, \"msg\": \"文件超过限制的大小啦\"}";
                }
                //检查上传的物理路径是否存在，不存在则创建
                if (!Directory.Exists(fullUpLoadPath))
                {
                    Directory.CreateDirectory(fullUpLoadPath);
                }
                if (is_rename == "0" && is_cover == "0")
                {
                    //判断是否存在相同的文件
                    if (File.Exists(fullUpLoadPath + newFileName))
                    {
                        return "{\"status\": 0, \"msg\": \"文件名已存在，请更换文件名\"}";
                    }

                }

                //保存文件
                postedFile.SaveAs(fullUpLoadPath + newFileName);
                string time_length = "";
                if (Utils.IsVoice(fileExt))
                {
                    //string strFileName = fullUpLoadPath + newFileName;
                    //string dirName = System.IO.Path.GetDirectoryName(strFileName);
                    //string SongName = System.IO.Path.GetFileName(strFileName);//获得歌曲名称
                    //FileInfo fInfo = new FileInfo(strFileName);

                    //ShellClass sh = new ShellClass();
                    //Folder dir = sh.NameSpace(dirName);
                    //FolderItem item = dir.ParseName(SongName);

                    //time_length = Regex.Match(dir.GetDetailsOf(item, -1), "\\d:\\d{2}:\\d{2}").Value;//获取歌曲时间
                    //if (!string.IsNullOrEmpty(time_length))
                    //{
                    //    string[] list = time_length.Split(new char[] { ':' });
                    //    if (list.Length == 3)
                    //    {
                    //        int m = Utils.StrToInt(list[1], 0);
                    //        int s = Utils.StrToInt(list[2], 0);
                    //        if (m > 0 && s > 0)
                    //        {
                    //            File.Delete(fullUpLoadPath + newFileName);
                    //            return "{\"status\": 0, \"msg\": \"音频文件的播放长度不能超过60s\"}";
                    //        }
                    //        else if (m > 1)
                    //        {
                    //            File.Delete(fullUpLoadPath + newFileName);
                    //            return "{\"status\": 0, \"msg\": \"音频文件的播放长度不能超过60s\"}";
                    //        }
                    //    }
                    //}


                }

                //如果是图片，检查图片是否超出最大尺寸，是则裁剪
                if (Utils.IsImage(fileExt) && (this.siteConfig.imgmaxheight > 0 || this.siteConfig.imgmaxwidth > 0))
                {
                    Thumbnail.MakeThumbnailImage(fullUpLoadPath + newFileName, fullUpLoadPath + newFileName,
                        this.siteConfig.imgmaxwidth, this.siteConfig.imgmaxheight);
                }
                //如果是图片，检查是否需要生成缩略图，是则生成
                if (Utils.IsImage(fileExt) && isThumbnail && this.siteConfig.thumbnailwidth > 0 && this.siteConfig.thumbnailheight > 0)
                {
                    Thumbnail.MakeThumbnailImage(fullUpLoadPath + newFileName, fullUpLoadPath + newThumbnailFileName,
                        this.siteConfig.thumbnailwidth, this.siteConfig.thumbnailheight, "Cut");
                }
                //如果是图片，检查是否需要打水印
                if (IsWaterMark(fileExt) && isWater)
                {
                    switch (this.siteConfig.watermarktype)
                    {
                        case 1:
                            WaterMark.AddImageSignText(newFilePath, newFilePath,
                                this.siteConfig.watermarktext, this.siteConfig.watermarkposition,
                                this.siteConfig.watermarkimgquality, this.siteConfig.watermarkfont, this.siteConfig.watermarkfontsize);
                            break;
                        case 2:
                            WaterMark.AddImageSignPic(newFilePath, newFilePath,
                                this.siteConfig.watermarkpic, this.siteConfig.watermarkposition,
                                this.siteConfig.watermarkimgquality, this.siteConfig.watermarktransparency);
                            break;
                    }
                }
                //if (write == 1 && upload_type == "1")
                //{
                //    try
                //    {
                //        //写入图片素材
                //        if (wx_id > 0 || is_public == 1)
                //        {
                //            KDWechat.BLL.Chats.wx_media_materials.Add(wx_id, u_id, orgFileName, "", 0, newFilePath, fileExt, fileSize.ToString(), media_type.素材图片库, -1, "", "", DateTime.Now.AddDays(-7), 1, (wx_id == 0 ? 1 : 0));
                //        }

                //    }
                //    catch (Exception)
                //    {
                //    }
                //}

                //处理完毕，返回JOSN格式的文件信息
                return "{\"status\": 1, \"msg\": \"上传文件成功\", \"name\": \""
                    + orgFileName + "\", \"path\": \"" + newFilePath + "\", \"thumb\": \""
                    + newThumbnailPath + "\", \"size\": " + fileSize + ", \"ext\": \"" + fileExt + "\"}";
            }
            catch
            {
                return "{\"status\": 0, \"msg\": \"上传过程中发生意外错误\"}";
            }
        }

        #region
        /// <summary>
        /// 文件上传方法 360全景
        /// </summary>
        /// <param name="postedFile">文件流</param>
        ///  <param name="folder">存放的文件夹</param>
        /// <param name="isThumbnail">是否生成缩略图</param>
        /// <param name="isWater">是否打水印</param>
        /// <returns>上传后文件信息</returns>
        public string For360fileSaveAs(HttpPostedFile postedFile, string postion, string imgid, string folder, bool isThumbnail, bool isWater, string upload_type, int wx_id, int u_id, int write = 1, int is_public = 0)
        {
            try
            {
                string fileExt = Utils.GetFileExt(postedFile.FileName); //文件扩展名，不含“.”
                int fileSize = postedFile.ContentLength; //获得文件大小，以字节为单位
                string fileName = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf(@"\") + 1); //取得原文件名
                string newFileName = imgid + "pano_" + postion + "." + fileExt; //随机生成新的文件名
                string newThumbnailFileName = newFileName.Replace("pano", "mobile");  //随机生成缩略图文件名
                string upLoadPath = GetUpLoadPath(folder, fileExt, GetFolderByType(upload_type)); //上传目录相对路径
                string fullUpLoadPath = Utils.GetMapPath(upLoadPath); //上传目录的物理路径
                string newFilePath = upLoadPath + newFileName; //上传后的路径
                string newThumbnailPath = upLoadPath + newThumbnailFileName; //上传后的缩略图路径
                string fileCode = postedFile.InputStream.ReadByte().ToString() + postedFile.InputStream.ReadByte().ToString();//文件真实类型代码  

                //判断上传的类型
                switch (upload_type)
                {
                    case "1":
                        if (Utils.IsImage(fileExt) == false)
                        {
                            return "{\"status\": 0, \"msg\": \"请上传bmp, png, jpeg, jpg, gif类型的图片\"}";
                        }
                        break;
                    default:
                        //检查文件扩展名是否合法
                        if (!CheckFileExt(fileExt))
                        {
                            return "{\"status\": 0, \"msg\": \"不允许上传" + fileExt + "类型的文件\"}";
                        }
                        break;
                }

                //检测文件是否是真实的文件
                if (!CheckFileIsTrue(fileCode, fileExt))
                {
                    return "{\"status\": 0, \"msg\": \"您上传的文件内容已损坏\"}";
                }





                //检查上传的物理路径是否存在，不存在则创建
                if (!Directory.Exists(fullUpLoadPath))
                {
                    Directory.CreateDirectory(fullUpLoadPath);
                }

                //保存文件
                postedFile.SaveAs(fullUpLoadPath + newFileName);


                //如果是图片，检查图片是否超出最大尺寸，是则裁剪
                if (Utils.IsImage(fileExt) && (this.siteConfig.imgmaxheight > 0 || this.siteConfig.imgmaxwidth > 0))
                {
                    Thumbnail.MakeThumbnailImage(fullUpLoadPath + newFileName, fullUpLoadPath + newFileName,
                        1024, 1024);
                }
                //如果是图片，检查是否需要生成缩略图，是则生成

                if (Utils.IsImage(fileExt) && isThumbnail && this.siteConfig.thumbnailwidth > 0 && this.siteConfig.thumbnailheight > 0)
                {
                    Thumbnail.MakeThumbnailImage(fullUpLoadPath + newFileName, fullUpLoadPath + newThumbnailFileName,
                        1024, 1024, "Cut");
                }
                //如果是图片，检查是否需要打水印
                if (IsWaterMark(fileExt) && isWater)
                {
                    switch (this.siteConfig.watermarktype)
                    {
                        case 1:
                            WaterMark.AddImageSignText(newFilePath, newFilePath,
                                this.siteConfig.watermarktext, this.siteConfig.watermarkposition,
                                this.siteConfig.watermarkimgquality, this.siteConfig.watermarkfont, this.siteConfig.watermarkfontsize);
                            break;
                        case 2:
                            WaterMark.AddImageSignPic(newFilePath, newFilePath,
                                this.siteConfig.watermarkpic, this.siteConfig.watermarkposition,
                                this.siteConfig.watermarkimgquality, this.siteConfig.watermarktransparency);
                            break;
                    }
                }


                if (write == 1)
                {
                    try
                    {
                        //写入图片素材
                        if (wx_id > 0)
                        {
                            KDWechat.BLL.Chats.wx_media_materials.Add(wx_id, u_id, fileName, "", 0, newFilePath, fileExt, fileSize.ToString(), media_type._360全景图片, -1, "", "", DateTime.Now.AddDays(-7), 1, (wx_id == 0 ? 1 : 0));
                        }

                    }
                    catch (Exception)
                    {
                    }
                }
                //处理完毕，返回JOSN格式的文件信息
                return "{\"status\": 1, \"msg\": \"上传文件成功\", \"name\": \""
                    + fileName + "\", \"path\": \"" + newFilePath + "\", \"thumb\": \""
                    + newThumbnailPath + "\", \"size\": " + fileSize + ", \"ext\": \"" + fileExt + "\"}";
            }
            catch
            {
                return "{\"status\": 0, \"msg\": \"上传过程中发生意外错误\"}";
            }
        }


        /// <summary>
        /// 上传自定义文件 360全景
        /// </summary>
        /// <param name="_upfile">上传的对象名称</param>
        /// <param name="_path">自定义上传的文件夹</param>
        /// <param name="is_cover">是否覆盖已存在的文件</param>
        /// <param name="is_rename">是否重命名文件</param>
        /// <param name="upload_type">上传类型（1、图片 2、音频 3 、视频  4、其他）</param>
        /// <returns></returns>
        public string For360fileSaveAs(HttpPostedFile postedFile, string postion, string imgid, string folder, string is_cover, string is_rename, string upload_type, bool isWater, int wx_id, int u_id, int write = 0, int is_public = 0)
        {
            try
            {
                bool isThumbnail = true;
                string fileExt = Utils.GetFileExt(postedFile.FileName); //文件扩展名，不含“.”
                int fileSize = postedFile.ContentLength; //获得文件大小，以字节为单位
                string orgFileName = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf(@"\") + 1); //取得原文件名

                string newFileName = imgid + "pano_" + postion + "." + fileExt; //随机生成新的文件名
                string newThumbnailFileName = Utils.GetRamCode() + "mobile_" + postion + "." + fileExt;  //随机生成缩略图文件名
                if (is_rename == "0")
                {
                    newFileName = orgFileName;
                    newThumbnailFileName = orgFileName;
                }

                string upLoadPath = GetUpLoadPath(folder, fileExt, GetFolderByType(upload_type)); //上传目录相对路径
                string fullUpLoadPath = Utils.GetMapPath(upLoadPath); //上传目录的物理路径
                string newFilePath = fullUpLoadPath + newFileName; //上传后的路径
                string newThumbnailPath = fullUpLoadPath + newThumbnailFileName; //上传后的缩略图路径

                string fileCode = postedFile.InputStream.ReadByte().ToString() + postedFile.InputStream.ReadByte().ToString();//文件真实类型代码  


                //判断上传的类型
                switch (upload_type)
                {
                    case "5":
                        if (Utils.IsImage(fileExt) == false)
                        {
                            return "{\"status\": 0, \"msg\": \"请上传bmp, png, jpeg, jpg, gif类型的图片\"}";
                        }
                        break;

                    default:
                        //检查文件扩展名是否合法
                        if (!CheckFileExt(fileExt))
                        {
                            return "{\"status\": 0, \"msg\": \"不允许上传" + fileExt + "类型的文件\"}";
                        }
                        break;
                }

                //检测文件是否是真实的文件
                if (!CheckFileIsTrue(fileCode, fileExt))
                {
                    return "{\"status\": 0, \"msg\": \"您上传的文件内容已损坏\"}";
                }

                //检查上传的物理路径是否存在，不存在则创建
                if (!Directory.Exists(fullUpLoadPath))
                {
                    Directory.CreateDirectory(fullUpLoadPath);
                }
                if (is_rename == "0" && is_cover == "0")
                {
                    //判断是否存在相同的文件
                    if (File.Exists(fullUpLoadPath + newFileName))
                    {
                        return "{\"status\": 0, \"msg\": \"文件名已存在，请更换文件名\"}";
                    }

                }

                //保存文件
                postedFile.SaveAs(fullUpLoadPath + newFileName);


                //如果是图片，检查图片是否超出最大尺寸，是则裁剪
                if (Utils.IsImage(fileExt) && (this.siteConfig.imgmaxheight > 0 || this.siteConfig.imgmaxwidth > 0))
                {
                    Thumbnail.MakeThumbnailImage(fullUpLoadPath + newFileName, fullUpLoadPath + newFileName,
                        1024, 1024);
                }
                //如果是图片，检查是否需要生成缩略图，是则生成
                if (Utils.IsImage(fileExt) && isThumbnail && this.siteConfig.thumbnailwidth > 0 && this.siteConfig.thumbnailheight > 0)
                {
                    Thumbnail.MakeThumbnailImage(fullUpLoadPath + newFileName, fullUpLoadPath + newThumbnailFileName,
                        1024, 1024, "Cut");
                }
                //如果是图片，检查是否需要打水印
                if (IsWaterMark(fileExt) && isWater)
                {
                    switch (this.siteConfig.watermarktype)
                    {
                        case 1:
                            WaterMark.AddImageSignText(newFilePath, newFilePath,
                                this.siteConfig.watermarktext, this.siteConfig.watermarkposition,
                                this.siteConfig.watermarkimgquality, this.siteConfig.watermarkfont, this.siteConfig.watermarkfontsize);
                            break;
                        case 2:
                            WaterMark.AddImageSignPic(newFilePath, newFilePath,
                                this.siteConfig.watermarkpic, this.siteConfig.watermarkposition,
                                this.siteConfig.watermarkimgquality, this.siteConfig.watermarktransparency);
                            break;
                    }
                }
                if (write == 1)
                {
                    try
                    {
                        //写入图片素材
                        if (wx_id > 0)
                        {
                            KDWechat.BLL.Chats.wx_media_materials.Add(wx_id, u_id, orgFileName, "", 0, newFilePath, fileExt, fileSize.ToString(), media_type._360全景图片, -1, "", "", DateTime.Now.AddDays(-7), 1, (wx_id == 0 ? 1 : 0));
                        }

                    }
                    catch (Exception)
                    {
                    }
                }

                //处理完毕，返回JOSN格式的文件信息
                return "{\"status\": 1, \"msg\": \"上传文件成功\", \"name\": \""
                    + orgFileName + "\", \"path\": \"" + newFilePath + "\", \"thumb\": \""
                    + newThumbnailPath + "\", \"size\": " + fileSize + ", \"ext\": \"" + fileExt + "\"}";
            }
            catch
            {
                return "{\"status\": 0, \"msg\": \"上传过程中发生意外错误\"}";
            }
        }

        #endregion

        

        /// <summary>
        /// 是否为图片文件
        /// </summary>
        /// <param name="_fileExt">文件扩展名，不含“.”</param>
        private bool IsImage(string _fileExt)
        {
            ArrayList al = new ArrayList();
            al.Add("bmp");
            al.Add("jpeg");
            al.Add("jpg");
            al.Add("gif");
            al.Add("png");
            if (al.Contains(_fileExt.ToLower()))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 微相册专用
        /// </summary>
        /// <param name="postedFile">文件流</param>
        /// <param name="isThumbnail">是否生成缩略图</param>
        /// <param name="isWater">是否打水印</param>
        /// <param name="isReOriginal">是否返回文件原名称</param>
        /// <returns>服务器文件路径</returns>
        private string fileSaveAs(HttpPostedFile postedFile, bool isThumbnail, bool isWater, bool _isImage, bool _isReOriginal, string folder, string upload_type)
        {
            try
            {
                string fileExt = Utils.GetFileExt(postedFile.FileName); //文件扩展名，不含“.”
                string originalFileName = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf(@"\") + 1); //取得文件原名
                string fileName = Utils.GetRamCode() + "." + fileExt; //随机文件名
                string dirPath = GetUpLoadPath(folder, "bmp", GetFolderByType(upload_type)); //上传目录相对路径

                //检查文件扩展名是否合法
                if (!CheckFileExt(fileExt))
                {
                    return "{\"msg\": 0, \"msgbox\": \"不允许上传" + fileExt + "类型的文件！\"}";
                }
                //检查是否必须上传图片
                if (_isImage && !IsImage(fileExt))
                {
                    return "{\"msg\": 0, \"msgbox\": \"对不起，仅允许上传图片文件！\"}";
                }
                //检查文件大小是否合法
                if (!CheckFileSize(fileExt, postedFile.ContentLength))
                {
                    return "{\"msg\": 0, \"msgbox\": \"文件超过限制的大小啦！\"}";
                }
                //获得要保存的文件路径
                string serverFileName = dirPath + fileName;
                string serverThumbnailFileName = dirPath + "small_" + fileName;
                string returnFileName = serverFileName;
                //物理完整路径                    
                string toFileFullPath = Utils.GetMapPath(dirPath);
                //检查有该路径是否就创建
                if (!Directory.Exists(toFileFullPath))
                {
                    Directory.CreateDirectory(toFileFullPath);
                }
                //保存文件
                postedFile.SaveAs(toFileFullPath + fileName);
                //如果是图片，检查图片尺寸是否超出限制
                //if (IsImage(fileExt) && (this.siteConfig.attachimgmaxheight > 0 || this.siteConfig.attachimgmaxwidth > 0))
                //{
                //    Thumbnail.MakeThumbnailImage(toFileFullPath + fileName, toFileFullPath + fileName, this.siteConfig.attachimgmaxwidth, this.siteConfig.attachimgmaxheight);
                //}
                //是否生成缩略图
                if (IsImage(fileExt) && isThumbnail && this.siteConfig.thumbnailwidth > 0 && this.siteConfig.thumbnailheight > 0)
                {
                    Thumbnail.MakeThumbnailImage(toFileFullPath + fileName, toFileFullPath + "small_" + fileName, this.siteConfig.thumbnailwidth, this.siteConfig.thumbnailheight, "Cut");
                    returnFileName += "," + serverThumbnailFileName; //返回缩略图，以逗号分隔开
                }
                //是否打图片水印
                if (IsWaterMark(fileExt) && isWater)
                {
                    switch (this.siteConfig.watermarktype)
                    {
                        case 1:
                            WaterMark.AddImageSignText(serverFileName, serverFileName,
                                this.siteConfig.watermarktext, this.siteConfig.watermarkposition,
                                this.siteConfig.watermarkimgquality, this.siteConfig.watermarkfont, this.siteConfig.watermarkfontsize);
                            break;
                        case 2:
                            WaterMark.AddImageSignPic(serverFileName, serverFileName,
                                this.siteConfig.watermarkpic, this.siteConfig.watermarkposition,
                                this.siteConfig.watermarkimgquality, this.siteConfig.watermarktransparency);
                            break;
                    }
                }
                //如果需要返回原文件名
                if (_isReOriginal)
                {
                    return "{\"msg\": 1, \"msgbox\": \"" + serverFileName + "\", \"mstitle\": \"" + originalFileName + "\"}";
                }
                return "{\"msg\": 1, \"msgbox\": \"" + returnFileName + "\"}";
            }
            catch
            {
                return "{\"msg\": 0, \"msgbox\": \"上传过程中发生意外错误！\"}";
            }
        }

    }
}
