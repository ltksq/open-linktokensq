using System;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace LinkTokenSQ.Controllers
{
    public class firewallController: BaseController
    {
        public ActionResult Index()
        {
            List<string> killlist = new List<string>();
            var u = GetUser();
            if (u != null && u.userid == "")
            {
                foreach (var item in Common.HeistMon.HMon.GetInstance().KillList)
                {
                    killlist.Add(item.Key);
                }
            }
            else
            {
                this.HttpContext.Response.Write("FireWall!");
                this.HttpContext.Response.End();
                return null;
            }
            return View(killlist);
        }

        [HttpPost]
        public ActionResult DeleteKill(string key)
        {
            PostResponse _res = new PostResponse();
            _res.IsSuccess = false;
            var u = GetUser();
            if (u != null && u.userid == "" && !string.IsNullOrEmpty(key))
            {
                int count = 0;
                _res.IsSuccess = Common.HeistMon.HMon.GetInstance().KillList.TryRemove(key, out count);
            }

            return Json(_res);
        }
    }
}