using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LinkTokenSQ.Controllers
{
    public class HomeController : NoLoginController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            this.Redirect("/");
            return View();
        }
   
    }
}
