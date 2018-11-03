using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.HeistMon
{
    /// <summary>
    /// 监控拦劫
    /// </summary>
    public class HMon
    {
        private static readonly HMon instance = new HMon();

        public ConcurrentDictionary<string, int> HDict = new ConcurrentDictionary<string, int>();
        public ConcurrentDictionary<string, int> KillList = new ConcurrentDictionary<string, int>();
        public ConcurrentDictionary<string, int> KillList1 = new ConcurrentDictionary<string, int>();
        public ConcurrentDictionary<string, DateTime> GetCodeIPs = new ConcurrentDictionary<string, DateTime>();
        private HMon()
        {

        }

        public static HMon GetInstance()
        {
            return instance;
        }

        /// <summary>
        /// 10秒中监控一次访问请求，访问量过大的进入黑名单
        /// </summary>
        private void ClearHDictData()
        {

            Task task = new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        foreach (var item in HDict)
                        {
                            string key = item.Key;
                            int killnumm = 200;
                            if (key.StartsWith("f_")) {
                                int.TryParse(key.Split('_')[1], out killnumm);
                                if (killnumm == 0) killnumm = 10;
                            }
                            
                            if (item.Value > killnumm)
                            {
                                if (!KillList.ContainsKey(key))
                                {
                                    KillList.TryAdd(key, 0);
                                }
                            }
                            int count = 0;
                            HDict.TryRemove(key, out count);
                        }
                         
                    }
                    catch
                    {

                    }

                    System.Threading.Thread.Sleep(1000*5);
                }
            });

            task.Start();
        }

        /// <summary>
        /// 60分钟解除黑名单
        /// </summary>
        private void ClearKillData()
        {

            Task task = new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        foreach (var item in KillList)
                        {
                            string key = item.Key;
                            int count = 0;
                            KillList.TryRemove(key, out count);
                        }

                    }
                    catch
                    {

                    }

                    System.Threading.Thread.Sleep(1000 * 60 * 30);
                }
            });

            task.Start();
        }

        public void StartMon()
        {
            ClearHDictData();
            ClearKillData();
        }

        /// <summary>
        /// 加入监控和请求封杀
        /// </summary>
        /// <param name="requestkey"></param>
        /// <param name="maxrequest"></param>
        public bool AddMon(string requestkey, int maxrequest = 10)
        {
            requestkey = "f_" + maxrequest + "_" + requestkey;
            if (!HDict.ContainsKey(requestkey))
            {
                HDict.TryAdd(requestkey, 1);
            }
            else
            {
                HDict[requestkey] += 1;
            }
            //if (requestkey.IndexOf("_GetTelCode_") >= 0)
            //{

            //    if (!KillList1.ContainsKey(requestkey))
            //    {
            //        KillList1.TryAdd(requestkey, 1);
            //    }
            //    else
            //    {
            //        KillList1[requestkey] += 1;

            //        if (KillList1[requestkey] >= 10)
            //        {
            //            KillList1[requestkey] = 0;
            //            int count = 0;
            //            KillList1.TryRemove(requestkey, out count);
            //            if (!KillList.ContainsKey(requestkey))
            //            {
            //                KillList.TryAdd(requestkey, 0);
            //            }
            //        }
            //    }
            //}

            if (Common.HeistMon.HMon.GetInstance().KillList.ContainsKey(requestkey))
            {
                return false;
            }
            //if (Common.HeistMon.HMon.GetInstance().GetCodeIPs.ContainsKey(requestkey))
            //{
            //    if (((TimeSpan)(DateTime.Now - GetCodeIPs[requestkey])).TotalMinutes < 1)
            //    {
            //        return false;
            //    }

            //}
            return true;
        }

        public void AddSendMSGIP(string requestkey)
        {
            requestkey = "f_20_" + requestkey;
            if (!GetCodeIPs.ContainsKey(requestkey))
            {
                GetCodeIPs.TryAdd(requestkey, DateTime.Now);
            }
            else
            {
                GetCodeIPs[requestkey] = DateTime.Now;
            }
        }
    }
}
