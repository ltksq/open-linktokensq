using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LinkTokenSQ.Controllers
{
    public class VerifyController : BaseController
    {
        //
        // GET: /Verify/

        public ActionResult Index()
        {
            var u = GetUser();
            if (u.wkccheckpass)
            {
                this.Request.RequestContext.HttpContext.Response.Redirect("/", true);
            }
            return View(u);
        }

        [HttpPost]
        public ActionResult CheckVerify()
        {
            PostResponse _Respone = new PostResponse() { IsSuccess = false };
            try
            {
                var u = GetUser();
                if (u.wkccheckpass)
                {
                    _Respone.IsSuccess = true;
                    _Respone.Message = "/";
                }
            }
            catch(Exception ex)
            {
                _Respone.Message = ex.Message;
                _Respone.IsSuccess = false;
            }

            return Json(_Respone);
        }


    }
}
