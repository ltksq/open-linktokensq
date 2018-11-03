using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class uinfoEntity
    {
        /// <summary>
        /// uid PK
        /// </summary>
        public int uid { get; set; }
        public int puid { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string nikename { get; set; }

        /// <summary>
        /// 用户登录账号(手机号)
        /// </summary>
        public string userid { get; set; }

        /// <summary>
        /// 最后手机验证码
        /// </summary>
        public string lastcheckcode { get; set; }

        /// <summary>
        /// 验证码最后时间
        /// </summary>
        public DateTime lastchecktime { get; set; }

        public string userpwd { get; set; }

        public string changepwd { get; set; }

        public string wkcaddress { get; set; }


        public DateTime datachange_lasttime { get; set; }

        public short usertype { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime regdatetime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string countrycode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool wkccheckpass { get; set; }

        public double wkccheckmoney { get; set; }

        /// <summary>
        /// 是否是会员
        /// </summary>
        public short isvip { get; set; }

        /// <summary>
        /// 会员日期
        /// </summary>
        public DateTime vipdate { get; set; }
    }
}
