using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common.Security
{
    public class SecurityHelp
    {
        private static string keystr="js#!@#sa";
        private static string ivstr = "1Ds123";
        
        
        /// <summary>  
        /// DES解密方法  
        /// </summary>  
        /// <param name="encryptedValue">待解密的字符串</param>  
        /// <param name="key">密钥</param>  
        /// <param name="iv">向量</param>  
        /// <returns>解密后的字符串</returns>  
        public static string DESDecrypt(string encryptedValue, string key, string iv)
        {
            using (DESCryptoServiceProvider sa =
                new DESCryptoServiceProvider { Key = Encoding.UTF8.GetBytes(key), IV = Encoding.UTF8.GetBytes(iv) })
            {
                using (ICryptoTransform ct = sa.CreateDecryptor())
                {
                    byte[] byt = Convert.FromBase64String(encryptedValue);

                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, ct, CryptoStreamMode.Write))
                        {
                            cs.Write(byt, 0, byt.Length);
                            cs.FlushFinalBlock();
                        }
                        return Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
        }

        /// <summary>
        /// DES解密方法  
        /// </summary>
        /// <param name="encryptedValue">待解密的字符串</param>
        /// <returns></returns>
        public static string DESDecrypt(string encryptedValue)
        {
            return DESDecrypt(encryptedValue, keystr, ivstr);
        }

        /// <summary>  
        /// DES加密方法  
        /// </summary>  
        /// <param name="encryptedValue">要加密的字符串</param>  
        /// <param name="key">密钥</param>  
        /// <param name="iv">向量</param>  
        /// <returns>加密后的字符串</returns>  
        public static string DESEncrypt(string originalValue, string key, string iv)
        {
            using (DESCryptoServiceProvider sa
                = new DESCryptoServiceProvider { Key = Encoding.UTF8.GetBytes(key), IV = Encoding.UTF8.GetBytes(iv) })
            {
                using (ICryptoTransform ct = sa.CreateEncryptor())
                {
                    byte[] by = Encoding.UTF8.GetBytes(originalValue);
                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, ct,
                                                         CryptoStreamMode.Write))
                        {
                            cs.Write(by, 0, by.Length);
                            cs.FlushFinalBlock();
                        }
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
        }

        /// <summary>
        /// DES加密方法 
        /// </summary>
        /// <param name="originalValue">要加密的字符串</param>
        /// <returns></returns>
        public static string DESEncrypt(string originalValue)
        {
            return DESEncrypt(originalValue, keystr, ivstr);
        }

        /// <summary>  
        /// MD5加密  
        /// </summary>  
        /// <param name="s">需要加密的字符串</param>  
        /// <returns></returns>  
        public static string EncryptMD5(string originalValue)  
        {  
            var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();  
            var result = "";
            if (!string.IsNullOrWhiteSpace(originalValue))  
            {  
                result = BitConverter.ToString(md5.ComputeHash(UnicodeEncoding.UTF8.GetBytes(originalValue))).Replace("-","").ToUpper();  
            }  
            return result;  
        }

        public static string GenerateHash(string txt)
        {
            var computedContent = string.Format("blob {0}\0{1}", txt.Length, txt);
            var hashBytes = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(computedContent));
            var sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }
            return sb.ToString();
        }

        static Random _rd = new Random();
        /// <summary>
        /// 生成Hash
        /// </summary>
        /// <returns></returns>
        public static string CreateHash()
        {
            int hi = _rd.Next(0,10000);
            string txt = ((TimeSpan)(DateTime.Now - DateTime.Parse("2018-1-1"))).TotalMilliseconds.ToString() + "_" + hi;
            return GenerateHash(txt);
        }
   
    }
}
