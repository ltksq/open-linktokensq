using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class uaccountEntity
    {
        /// <summary>
        /// pk
        /// </summary>
        public int uid { get; set; }
        /// <summary>
        /// 账户余额
        /// </summary>
        public double accountmony { get; set; }
        /// <summary>
        /// 冻结金额
        /// </summary>
        public double stopmoney { get; set; }
        public DateTime datachange_lasttime { get; set; }
    }
}
