using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LinkTokenSQ.Controllers
{
    public class AccountController : NoLoginController
    {
        //
        // GET: /Account/

        public ActionResult SecurityCode123()
        {
            if (!Common.HeistMon.HMon.GetInstance().AddMon("SecurityCode123_" + base.GetRequesterIP(), 20))
            {
                this.HttpContext.Response.Write("FireWall!");
                this.HttpContext.Response.End();
                return null;
            }
            string code = Common.Cache.SecurityCode.CreateRandomCode(5); //验证码的字符为4个
            TempData["SecurityCode"] = code; //验证码存放在TempData中
            return File(Common.Cache.SecurityCode.CreateValidateGraphic(code), "image/Jpeg");
        }

        public ActionResult SecurityCacheCode()
        {
            if(!Common.HeistMon.HMon.GetInstance().AddMon("SecurityCacheCode_" + base.GetRequesterIP(), 20))
            {
                this.HttpContext.Response.Write("FireWall!");
                this.HttpContext.Response.End();
                return null;
            }
            Common.Cache.SCode sc=new Common.Cache.SCode();
            if (Common.Cache.SecurityCode.GetInstance().CodeList.TryDequeue(out sc))
            {
                string code = sc.code; //验证码的字符为4个
                TempData["SecurityCode"] = code; //验证码存放在TempData中
                return File(sc.value, "image/Jpeg");
            }
            else
            {
                TempData["SecurityCode"] = "";
            }
            return View();             
        }
    }
}
