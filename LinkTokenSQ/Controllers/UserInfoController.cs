
using DAL;
using Entity;
using Entity.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LinkTokenSQ.Controllers
{
    public class UserInfoController : BaseController
    {
        //
        // GET: /userinfo/

        public ActionResult Index()
        {
            if (!Common.HeistMon.HMon.GetInstance().AddMon("userinfoindex_" + base.GetUserid(), 20))
            {
                this.HttpContext.Response.Write("FireWall!");
                this.HttpContext.Response.End();
                return null;
            }
            var u = GetUser();
            if (!u.wkccheckpass)
            {
                this.Request.RequestContext.HttpContext.Response.Redirect("/verify", true);
            }

            return View(u);
        }
 
         
  
        public ActionResult userinfo()
        {
            if (!Common.HeistMon.HMon.GetInstance().AddMon("userinfo_" + base.GetUserid(), 10))
            {
                this.HttpContext.Response.Write("FireWall!");
                this.HttpContext.Response.End();
                return null;
            }
            userinfoV uv = new userinfoV();
            uv.Uinfo = this.GetUser();
            string msg = "";
            uv.Acc = uaccountDal.GetByKeyId(uv.Uinfo.uid, ref msg);
            
            if (uv.Uinfo.vipdate >= DateTime.Now.Date)
            {
                
            }
            return View(uv);
        }
 
        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        public ActionResult loginout()
        {
            base.LoginOut();
            this.Request.RequestContext.HttpContext.Response.Redirect("/login", true);

            return View();
        }

      
        /// <summary>
        /// 账户明细
        /// </summary>
        /// <returns></returns>
        public ActionResult accdetail(int currpage = 1)
        {
            if (!Common.HeistMon.HMon.GetInstance().AddMon("accdetail_" + base.GetUserid(), 20))
            {
                this.HttpContext.Response.Write("FireWall!");
                this.HttpContext.Response.End();
                return null;
            }

            accdetailV v = new accdetailV();
            var uinfo = this.GetUser();
            if (uinfo != null && uinfo.uid > 0)
            {
                if (this.HttpContext.Request.QueryString["currpage"] != null) currpage = int.Parse(this.HttpContext.Request.QueryString["currpage"].ToString());
                string ftype = "";
                if (this.HttpContext.Request.QueryString["ftype"] != null) ftype = this.HttpContext.Request.QueryString["ftype"].ToString();
                string wherestr = "ftype>=0 and uid=" + uinfo.uid;
                if (!string.IsNullOrEmpty(ftype))
                    wherestr += " and ftype=" + ftype;
                v.Count = uchangedetailDal.DataCount(wherestr);
                v.Items = uchangedetailDal.PageData(wherestr, currpage);
                v.CurrPage = currpage;
            }
            return View(v);
        }
       
        /// <summary>
        /// help
        /// </summary>
        /// <returns></returns>
        public ActionResult userhelp()
        {
            if (!Common.HeistMon.HMon.GetInstance().AddMon("userhelp_" + base.GetUserid(), 10))
            {
                this.HttpContext.Response.Write("FireWall!");
                this.HttpContext.Response.End();
                return null;
            }

            if (LinkTokenSQ.MvcApplication.Syspar.ContainsKey("userhelp"))
            {
                return View(LinkTokenSQ.MvcApplication.Syspar["userhelp"]);
            }
            return View(new ssysparEntity());
        }
        /// <summary>
        /// 公告
        /// </summary>
        /// <returns></returns>
        public ActionResult usernote()
        {
            if (!Common.HeistMon.HMon.GetInstance().AddMon("usernote_" + base.GetUserid(), 10))
            {
                this.HttpContext.Response.Write("FireWall!");
                this.HttpContext.Response.End();
                return null;
            }
            if (LinkTokenSQ.MvcApplication.Syspar.ContainsKey("usernote"))
            {
                LinkTokenSQ.MvcApplication.Syspar["usernote"].uinfo = base.GetUser();
                return View(LinkTokenSQ.MvcApplication.Syspar["usernote"]);
            }
            return View(new ssysparEntity());
        }

     
    }
 
}
