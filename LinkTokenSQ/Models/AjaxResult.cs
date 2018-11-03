using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkTokenSQ.Models
{
    /// <summary>
    /// 异步请求，返回信息
    /// </summary>
    public class AjaxResult
    {
        /// <summary>
        /// 返回编码
        /// </summary>
        public long RetCode { get; set; }
        /// <summary>
        /// Html信息
        /// </summary>
        public string Html { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// 结果信息
        /// </summary>
        public string ResultInfo { get; set; }

        /// <summary>
        /// 结果对象
        /// </summary>
        public Object ResultObj { get; set; }

    }
}