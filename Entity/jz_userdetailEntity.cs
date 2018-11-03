using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 捐赠信息
    /// </summary>
    public class jz_userdetailEntity
    {
        public int zjuid { get; set; }
        public int uid { get; set; }
        public string nikename { get; set; }

        public double fmoney { get; set; }

        public string remark { get; set; }

        public DateTime createtime { get; set; }
    }
}
