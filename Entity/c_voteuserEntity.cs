using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class c_voteuserEntity
    {
        public long usvoteid { get; set; }

        public int voteid { get; set; }

        public long svoteid { get; set; }

        public int uid { get; set; }

        public string wkcaddress { get; set; }

        public double wkcamount { get; set; }

        public DateTime createtime { get; set; }

        public DateTime lasttime { get; set; }

        public int onecloudnum { get; set; }

        public double paywkcamount { get; set; }

        public int getamountok { get; set; }

        public string nikename { get; set; }
    }
}
