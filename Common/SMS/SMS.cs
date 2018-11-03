using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Common.SMS
{
    public class SMS
    {
        public static Random random = new Random();
        public static bool SMS_sender(string mobile,string sum_content)
        {
            StringBuilder sms = new StringBuilder();
            sms.AppendFormat("name={0}", "账号");
            sms.AppendFormat("&pwd={0}", "");//登陆平台，管理中心--基本资料--接口密码（28位密文）；复制使用即可。
            sms.AppendFormat("&content=【链克社区】您的验证码:{0} ，此验证码10分钟有效，请勿告之他人。", sum_content);
            sms.AppendFormat("&mobile={0}", mobile);
            sms.AppendFormat("&sign={0}", System.Configuration.ConfigurationSettings.AppSettings["SMSsign"]); //公司的简称或产品的简称都可以
            sms.Append("&type=pt");
            string resp = PushToWeb(System.Configuration.ConfigurationSettings.AppSettings["SMSServiceUrl"], sms.ToString(), Encoding.UTF8);
            string[] msg = resp.Split(',');
            if (msg[0] == "0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static string PushToWeb(string weburl, string data, Encoding encode)
        {
            byte[] byteArray = encode.GetBytes(data);

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri(weburl));
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = byteArray.Length;
            Stream newStream = webRequest.GetRequestStream();
            newStream.Write(byteArray, 0, byteArray.Length);
            newStream.Close();

            //接收返回信息：
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            StreamReader aspx = new StreamReader(response.GetResponseStream(), encode);
            return aspx.ReadToEnd();
        }

    }
}
