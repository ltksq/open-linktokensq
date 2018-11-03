using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class uimportmoneyEntity
    {
        public long inmid { get; set; }
        /// <summary>
        /// 充钱包地址
        /// </summary>
        public string wkcaddress { get; set; }
        /// <summary>
        /// 交易hash
        /// </summary>
        public string thash { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime indatetime { get; set; }
        /// <summary>
        /// 对应用户
        /// </summary>
        public int uid { get; set; }

        public double fmoney { get; set; }
    }
}
