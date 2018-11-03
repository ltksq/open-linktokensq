using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkTokenSQ.Models
{
    public class VoteIndexModel
    {
        public int uid { get; set; }

        public double addvoteprice { get; set; }

        public double joinvoteprice { get; set; }
        public bool myvote { get; set; }
        public int votecount { get; set; }
        /// <summary>
        /// 投票主题列表
        /// </summary>
        public List<c_voteEntity> VoteList = new List<c_voteEntity>();

        public List<c_voteitemEntity> VoteItemList = new List<c_voteitemEntity>();

        /// <summary>
        /// 参与用户
        /// </summary>
        public List<c_voteuserEntity> VoteJoinUserList = new List<c_voteuserEntity>();

        public int GetJoinUserCount(long svid)
        {
            if (VoteJoinUserList != null)
                return VoteJoinUserList.FindAll(f => f.svoteid == svid).Count;
            return 0;
        }

        public double GetJoinUserTicket(long svid)
        {
            if (VoteJoinUserList != null)
                return Math.Round(VoteJoinUserList.FindAll(f => f.svoteid == svid).Sum(s => s.wkcamount),8);
            return 0;
        }

        public string GetX()
        {
            string data = "";
            VoteItemList.ForEach(f=>
                {
                    data += "'"+f.title.Replace("'"," ")+"',";
            });
            return data.TrimEnd(',');
        }

        public string GetY()
        {
            string data = "";
            VoteItemList.ForEach(f =>
            {
                data += (long)Math.Round(this.GetJoinUserTicket(f.svoteid), 0) + ",";
            });
            return data.TrimEnd(',');
        }
    }
}