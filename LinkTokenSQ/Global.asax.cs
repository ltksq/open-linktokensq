
using DAL;
using Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace LinkTokenSQ
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static DateTime loadpartime = DateTime.Parse("2018-1-1");
        public static Dictionary<string, ssysparEntity> Syspar = new Dictionary<string, ssysparEntity>();


        protected void Application_Start()
        {
          
          Task task = new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        LoadSyspar();
                    }
                    catch { }
                    System.Threading.Thread.Sleep(10000);
                }
            });


            Common.HeistMon.HMon.GetInstance().StartMon();
            Common.Cache.User.GetInstance().LoadDataToCache();
            Common.Cache.SecurityCode.GetInstance().LoadDataToCache();

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
 

            task.Start();
 
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            string requestkey = GetRequesterIP();
            if (Common.HeistMon.HMon.GetInstance().KillList.ContainsKey(requestkey))
            {
                if (this.Request.RawUrl.IndexOf("/firewall") < 0)
                {
                    Context.Response.Write("FireWall!");
                    Context.Response.End();
                    return;
                }
            }

            if (Syspar.ContainsKey("SysUpdate"))
            {
                if (Syspar["SysUpdate"].keyvalue == "1")
                {
                    Context.Response.Write(Syspar["SysUpdate"].keydes);
                    Context.Response.End();
                    return;
                }
            }
            

            
            if (!Common.HeistMon.HMon.GetInstance().HDict.ContainsKey(requestkey))
            {
                Common.HeistMon.HMon.GetInstance().HDict.TryAdd(requestkey, 1);
            }
            else
            {
                int c = 0;
                if (Common.HeistMon.HMon.GetInstance().HDict.TryGetValue(requestkey, out c))
                {
                    int cc = c + 1;
                    Common.HeistMon.HMon.GetInstance().HDict.TryUpdate(requestkey, cc, c);
                }
            }

           
        }


        /// <summary>
        /// 获取真实IP
        /// </summary>
        /// <returns></returns>
        private string GetRequesterIP()
        {
            string Result = String.Empty;
            Result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (null == Result || Result == String.Empty)
            {
                Result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (null == Result || Result == String.Empty)
            {
                Result = HttpContext.Current.Request.UserHostAddress;
            }
            if (null == Result || Result == String.Empty)
            {
                return "0.0.0.0";
            }

            return Result;
        }

        protected void LoadSyspar()
        {
             
            string info = "";
            DateTime dt = DateTime.Now;
            var pars = ssysparDal.GetListByWhere(string.Format("datachange_lasttime>='{0}'",loadpartime.ToString()), ref info);
            if (string.IsNullOrEmpty(info))
            {
                loadpartime = dt;
                if (pars != null)
                {
                    pars.ForEach(f => {
                        if (!Syspar.ContainsKey(f.keyname))
                        {
                            Syspar.Add(f.keyname, f);
                        }
                        else
                            Syspar[f.keyname] = f;
                    });
                }
            }
        }
 
 
 
    }

   
}