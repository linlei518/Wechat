using System;
using System.Drawing.Imaging;
using System.IO;
using KDWechat.Common;
using KDWechat.Web.UI;
using QuickMark;

namespace KDWechat.Web.product
{
    public partial class qr_code_show : BasePage
    {
        public string code = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (id > 0)
            {
                //生成二维码

               code= CreateQrCode("http://61.129.33.240:88/product_info?id=" + id, "qr_code_"+id);

            }
            else
            {
                JsHelper.AlertAndRedirect("参数错误！","");
            }
        }



        /// <summary>
        /// 生成图文二维码
        /// </summary>
        /// <param name="link_url">二维码页面地址,都是外链</param>
        /// <param name="img_name">生成的二维码图片的名称</param>
        /// <param name="is_cover">是否覆盖之前</param>
        /// <returns></returns>
        public string CreateQrCode(string link_url, string img_name, bool is_cover = false)
        {
            string img_url = "/qr_codes/" + img_name + ".png";
            if (is_cover)
            {
                try { File.Delete(Server.MapPath(img_url)); }
                catch (Exception) { }
            }
            if (!File.Exists(Server.MapPath(img_url)))
            {
                QuickMark.CreateTwoCode ctc = new QuickMark.CreateTwoCode();
                string link = link_url;
                using (var bitmap = ctc.CreateCode(link, CreateTwoCode.CodeType.Byte, CreateTwoCode.Correct.M, 0, 15))
                {
                    if (bitmap != null)
                    {
                        //检查上传的物理路径是否存在，不存在则创建
                        if (!Directory.Exists(Server.MapPath("/qr_codes/")))
                            Directory.CreateDirectory(Server.MapPath("/qr_codes/"));
                        bitmap.Save(Server.MapPath(img_url), ImageFormat.Png);
                    }
                }


            }
            return img_url;
        }

    }
}