using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KDWechat.Common;
using KDWechat.DAL;
using Senparc.Weixin.MP.CommonAPIs;

namespace KDWechat.Web.fans
{
    public partial class down_file : KDWechat.Web.UI.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckWXid();
                string media_id = KDWechat.Common.RequestHelper.GetQueryString("media_id");
                if (media_id.Trim().Length > 0)
                {
                    DAL.t_wx_fans_chats model = Companycn.Core.EntityFramework.EFHelper.GetModel<creater_wxEntities, t_wx_fans_chats>(x => x.media_id == media_id);
                    bool isc = false;
                    if (model!=null)
                    {
                        if (model.create_time.AddHours(70)>DateTime.Now)
                        {
                            isc = true;
                        }
                    }
                    if (isc)
                    {
                        t_wx_wechats wx_wechat = BLL.Chats.wx_wechats.GetWeChatByID(wx_id);
                        if (wx_wechat != null)
                        {
                            string accessToken = BLL.Chats.wx_wechats.GetAccessToken(wx_wechat.id);// AccessTokenContainer.TryGetToken(wx_wechat.app_id, wx_wechat.app_secret);

                         
                            if (model.msg_type==(int)msg_type.语音)
                            {
                                string file_path = Server.MapPath("/upload/mp3/" + media_id + ".mp3");
                                if (!File.Exists(file_path))
                                {
                                    System.IO.MemoryStream sr = new MemoryStream();
                                    Senparc.Weixin.MP.AdvancedAPIs.Media.Get(accessToken, media_id, sr);
                                    if (sr.Length > 0)
                                    {
                                        if (!System.IO.Directory.Exists(Server.MapPath("/upload/mp3/")))
                                        {
                                            System.IO.Directory.CreateDirectory(Server.MapPath("/upload/mp3/"));
                                        }

                                        FileStream fs = new FileStream(Server.MapPath("/upload/mp3/" + media_id + ".mp3"), FileMode.Create);
                                        fs.Write(sr.GetBuffer(), 0, (int)sr.Length);
                                        sr.Dispose();
                                        fs.Dispose();
                                        sr.Close();
                                        fs.Close();
                                        donwFile(media_id, file_path);

                                    }
                                    else
                                    {
                                        Response.Write("媒体文件已超过下载时限，无法下载！");
                                        Response.End();
                                    }
                                }
                                else
                                {
                                    donwFile(media_id, file_path);
                                }
                              
                            }
                            else
                            {
                                Response.Redirect("http://file.api.weixin.qq.com/cgi-bin/media/get?access_token=" + accessToken + "&media_id=" + media_id + "");
                            }
                            
                        }

                    }
                    else
                    {
                        Response.Write("媒体文件已超过下载时限，无法下载！");
                        Response.End();
                    }
                  
                }
            }
        }

        private void donwFile(string media_id, string file_path)
        {
            string _path = file_path;
            System.IO.FileInfo file = new System.IO.FileInfo(_path);
            Response.Clear();
            Response.Charset = "UTF8";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            //下载文件默认文件名
            Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(media_id + ".mp3", System.Text.Encoding.UTF8));
            //添加头信息，指定文件大小，让浏览器能显示下载进度
            Response.AddHeader("Content-Length", file.Length.ToString());
            Response.ContentType = "application/octet-stream";
            //把文件发送该客户段
            Response.WriteFile(file.FullName);
            Response.Flush();
            Response.Clear();
        }
    }
}