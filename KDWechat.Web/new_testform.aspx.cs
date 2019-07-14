using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace KDWechat.Web
{
    public partial class new_testform : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var data = "<?xml version=\"1.0\" encoding=\"utf-8\"?><xml><ToUserName><![CDATA[gh_24b28a35578b]]></ToUserName><FromUserName><![CDATA[oIyYMj-x8y5lrg-kAiI9LLJNVJjI]]></FromUserName><CreateTime>1419238303</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[oo]]></Content><MsgId>6095582096817499341</MsgId></xml>";
                var dataBytes = Encoding.UTF8.GetBytes(data);
                System.IO.MemoryStream inputS = new System.IO.MemoryStream(dataBytes);

                byte[] byts = new byte[inputS.Length];
                inputS.Read(byts, 0, byts.Length);

                
                var ms = new MemoryStream(byts);
                //string req = System.Text.Encoding.UTF8.GetString(byts);
                //req = System.Web.HttpUtility.UrlDecode(req);
                XElement xml = XElement.Load(ms);


                var msgType = xml.Element("MsgType").Value;

                if (msgType == "text")
                {
                    var signature = GetSignature("abc", "abc", "984784_z");
                    string url = "http://api.osd.weimob.com/api?t=c66d6ae7f7cdde96cc91bc79c1ebe16b==G&signature=" + signature + "&timestamp=abc&nonce=abc";
                    ms.Position = 0;
                    var responses = BaiDuMapAPI.HttpUtility.RequestUtility.HttpPost(url, ms);
                    WriteContent(responses);
                }
            }
        }






        private void WriteContent(string str)
        {
            Response.Output.Write(str);
        }

        public string GetSignature(string timestamp, string nonce, string token = null)
        {
            var arr = new[] { token, timestamp, nonce }.OrderBy(z => z).ToArray();
            var arrString = string.Join("", arr);
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var sha1Arr = sha1.ComputeHash(Encoding.UTF8.GetBytes(arrString));
            StringBuilder enText = new StringBuilder();
            foreach (var b in sha1Arr)
            {
                enText.AppendFormat("{0:x2}", b);
            }
            return enText.ToString();
        }









    }
}