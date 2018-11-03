using DAL;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Cache
{
    public class User
    {
        private static readonly User instance = new User();
        public DateTime runDate = DateTime.Parse("2018-1-1");
        public Dictionary<string, uinfoEntity> MapingData = new Dictionary<string, uinfoEntity>();
        private bool firstrunning = true;
        public bool complete = false;
        private User()
        {
             
        }

        public static User GetInstance()
        {
            return instance;
        }

        /// <summary>
        /// 装载用户数据进缓存
        /// </summary>
        public void LoadDataToCache()
        {
            if (!firstrunning) return;
            firstrunning = false;
            Task task = new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        DateTime dt = DateTime.Now.AddSeconds(-0.5);
                        string msg = "";
                        var list = uinfoDal.GetListByWhere(string.Format("datachange_lasttime>='{0}'", runDate.ToString()), ref msg);
                        if (string.IsNullOrEmpty(msg))
                        {
                            runDate = dt;
                            if (list != null)
                                list.ForEach(f =>
                                    {
                                        f.userpwd = string.Empty;
                                        f.changepwd = string.Empty;
                                        if (!User.GetInstance().MapingData.ContainsKey(f.countrycode+f.userid))
                                        {
                                            User.GetInstance().MapingData.Add(f.countrycode + f.userid, f);
                                        }
                                        else
                                        {
                                            User.GetInstance().MapingData[f.countrycode + f.userid] = f;
                                        }
                                    });
                        }
                    }
                    catch
                    {

                    }
                    complete = true;
                    System.Threading.Thread.Sleep(3000);
                }
            });

            task.Start();
        }

        
    }
}
