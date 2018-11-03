using Common.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Common.TokenHelp
{
    public class TokenClass
    {
        /// <summary>
        /// 验证token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool CheckTokenLoin(string token,out string msg,out LoginTokenRequest lr)
        {
            bool r = true;
            msg = "";
            lr = null;
            if (string.IsNullOrEmpty(token))
            {
                msg = "token is empty.";
                r = false;
                return r;
            }
            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                LoginTokenRequest login = js.Deserialize<LoginTokenRequest>(SecurityHelp.DESDecrypt(token));
                lr = login;
                string md5 = SecurityHelp.EncryptMD5(string.Format("{0}{1}{2}", login.uid, login.pwd, login.timespan.ToString()));
                if (!md5.Equals(login.datamd5))
                {
                    msg = "data is invalidity.";
                    r = false;
                    return r;
                }

                if (((TimeSpan)(DateTime.Now - login.timespan)).TotalSeconds > 30)
                {
                    msg = "invalidation of token.";
                    r = false;
                    return r;
                }

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                r = false;
            }
            return r;
        }
    }
}
