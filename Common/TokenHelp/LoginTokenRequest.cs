using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.TokenHelp
{
    /// <summary>
    /// 登录token 30秒失效
    /// </summary>
    public class LoginTokenRequest
    {
        /// <summary>
        /// uid
        /// </summary>
        public int uid { get; set; }
        /// <summary>
        /// passowrd
        /// </summary>
        public string pwd { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime timespan { get; set; }

        /// <summary>
        /// md5密文，防串改
        /// </summary>
        public string datamd5 { get; set; }
    }
}
