using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace KDWechat.BLL.Pay
{
    public class Allowance
    {

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }
        /// <summary>
        /// 发送红包给单个用户
        /// </summary>
        /// <param name="userHostAddress">Request.UserHostAddress</param>
        /// <param name="appid">公众号Appid</param>
        /// <param name="mchid">商户Mchid</param>
        /// <param name="payKey">支付的key</param>
        /// <param name="certPath">证书地址</param>
        /// <param name="providerName">提供者名称</param>
        /// <param name="senderName">发送者名称</param>
        /// <param name="openID">接受者的openid</param>
        /// <param name="count">发送的金额（元）</param>
        /// <param name="greeting">祝福语</param>
        /// <param name="actName">活动名称</param>
        /// <param name="remark">备注</param>
        /// <param name="logo_imgurl">logo</param>
        /// <param name="share_content">分享文案</param>
        /// <param name="share_url">分享地址</param>
        /// <param name="share_imgurl">分享的logo</param>
        /// <returns></returns>
        public string SendRedPack(string userHostAddress, string appid, string mchid, string payKey, string certPath, string providerName, string senderName, string openID, double count, string greeting, string actName, string remark, string logo_imgurl = null, string share_content = null, string share_url = null, string share_imgurl = null)
        {
            string mchbillno = DateTime.Now.ToString("HHmmss");
            var totalCount = (count * 100).ToString();
            TenPayUtil.BuildRandomStr(28);

            string nonceStr = TenPayUtil.GetNoncestr();
            RequestHandler packageReqHandler = new RequestHandler(null);

            //设置package订单参数
            packageReqHandler.SetParameter("nonce_str", nonceStr); //随机字符串
            packageReqHandler.SetParameter("wxappid", appid); //公众账号ID
            packageReqHandler.SetParameter("mch_id", mchid); //商户号
            packageReqHandler.SetParameter("mch_billno", mchbillno); //填入商家订单号
            packageReqHandler.SetParameter("nick_name", providerName); //提供方名称
            packageReqHandler.SetParameter("send_name", senderName); //红包发送者名称
            packageReqHandler.SetParameter("re_openid", openID); //接受收红包的用户的openId
            packageReqHandler.SetParameter("total_amount", totalCount); //付款金额，单位分
            packageReqHandler.SetParameter("min_value", totalCount); //最小红包金额，单位分
            packageReqHandler.SetParameter("max_value", totalCount); //最大红包金额，单位分
            packageReqHandler.SetParameter("total_num", "1"); //红包发放总人数
            packageReqHandler.SetParameter("wishing", greeting); //红包祝福语
            packageReqHandler.SetParameter("client_ip", userHostAddress); //调用接口的机器Ip地址
            packageReqHandler.SetParameter("act_name", actName); //活动名称
            packageReqHandler.SetParameter("remark", remark); //备注信息
            if (!string.IsNullOrEmpty(logo_imgurl))
                packageReqHandler.SetParameter("logo_imgurl", logo_imgurl); //logo
            if (!string.IsNullOrEmpty(share_content))
                packageReqHandler.SetParameter("share_content", share_content); //分享文案
            if (!string.IsNullOrEmpty(share_url))
                packageReqHandler.SetParameter("share_url", share_url); //分享地址
            if (!string.IsNullOrEmpty(share_imgurl))
                packageReqHandler.SetParameter("share_imgurl", share_imgurl); //分享的logo

            string sign = packageReqHandler.CreateMd5Sign("key", payKey);
            packageReqHandler.SetParameter("sign", sign); //签名
            //退款需要post的数据
            string data = packageReqHandler.ParseXML();

            //退款接口地址
            string url = "https://api.mch.weixin.qq.com/mmpaymkttransfers/sendredpack";
            //本地或者服务器的证书位置（证书在微信支付申请成功发来的通知邮件中）
            string cert = certPath;
            //私钥（在安装证书时设置）
            string password = mchid;
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            //调用证书
            X509Certificate2 cer = new X509Certificate2(cert, password, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);

            #region 发起post请求
            HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create(url);
            webrequest.ClientCertificates.Add(cer);
            webrequest.Method = "post";

            byte[] postdatabyte = Encoding.UTF8.GetBytes(data);
            webrequest.ContentLength = postdatabyte.Length;
            Stream stream;
            stream = webrequest.GetRequestStream();
            stream.Write(postdatabyte, 0, postdatabyte.Length);
            stream.Close();

            HttpWebResponse httpWebResponse = (HttpWebResponse)webrequest.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            string responseContent = streamReader.ReadToEnd();
            return responseContent;
            #endregion

        }
    }
}
