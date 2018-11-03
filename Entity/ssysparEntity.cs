using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class ssysparEntity
    {
        public string keyname { get; set; }
        public string keyvalue { get; set; }
        public string keydes { get; set; }
        public DateTime datachange_lasttime { get; set; }

        public uinfoEntity uinfo { get; set; }
    }
}
