using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class c_voteEntity
    {
        public int voteid { get; set; }

        public int uid { get; set; }

        public string votetitle { get; set; }

        public short votetype { get; set; }

        /// <summary>
        /// 0暂存,1提交审核,2通过审核,3审核不通过,4关闭,5下线不显示
        /// </summary>
        public short votestatus { get; set; }
        public string statusname
        {
            get
            {
                if (votestatus == 0)
                {
                    return "暂存";
                }
                else if (votestatus == 1)
                {
                    return "待审核";
                }
                else if (votestatus == 2)
                {
                    return "投票中";
                }
                else if (votestatus == 3)
                {
                    return "审核拒绝";
                }
                else if (votestatus == 4)
                {
                    return "结束";
                }
                else if (votestatus == 5)
                {
                    return "下线";
                }
                return "";
            }
        }

        public DateTime createtime { get; set; }

        public DateTime lasttime { get; set; }

        public double paywkcamount { get; set; }

        public string remark { get; set; }

        public DateTime enddate { get; set; }

        /// <summary>
        /// 参与人数
        /// </summary>
        public int joinusercount { get; set; }

        /// <summary>
        /// 参与总票数
        /// </summary>
        public double ticketcount { get; set; }

        /// <summary>
        /// 参与总票数（四舍五入取支整）
        /// </summary>
        public long ticketnumber
        {
            get
            {
                return (long)Math.Round(ticketcount, 0);
            }
        }

        /// <summary>
        /// 耗费wkc数量
        /// </summary>
        public double useamount { get; set; }
    }
}
