using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class uchangedetailEntity
    {
        public long detid { get; set; }
        public int uid { get; set; }
        public double fmoney { get; set; }
        public int ftype { get; set; }
        public DateTime datachange_lasttime { get; set; }
        public string remark { get; set; }

        public string ftypename
        {
            get
            {
                if (ftype == 8)
                    return "充值";
                else if (ftype == 30)
                    return "支出";

                return "";
            }
        }
    }

}
