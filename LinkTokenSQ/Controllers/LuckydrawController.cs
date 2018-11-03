 
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
    /// <summary>
    ///  
    /// </summary>
    public class LuckydrawController : BaseController
    {
        public ActionResult Index()
        {
            var u = GetUser();
            if (!u.wkccheckpass)
            {
                this.Request.RequestContext.HttpContext.Response.Redirect("/verify", true);
            }
            return View(u);
        }
     

        public ActionResult jzdetail()
        {
            var u = GetUser();
            if (!u.wkccheckpass)
            {
                this.Request.RequestContext.HttpContext.Response.Redirect("/verify", true);
            }
            jzinfoV v = new jzinfoV();
            v.uinfo = u;
            v.jzdetails = jz_userdetailDal.GetListAll();
            v.jzze = v.jzdetails.Sum(s => s.fmoney);
            return View(v);
        }

        public ActionResult Startuserjz(int uid,double fmoney)
        {
            PostResponse _Respone = new PostResponse() { IsSuccess = false };
            if (LinkTokenSQ.MvcApplication.Syspar["OpeanLuckdraw"].keyvalue == "0")
            {
                _Respone.Message = LinkTokenSQ.MvcApplication.Syspar["OpeanLuckdraw"].keydes;
                return Json(_Respone);
            }
            var uinfo = this.GetUser();
            try
            {
                if (fmoney <= 0)
                {
                    _Respone.Message = "捐赠数额要>0 ";
                    return Json(_Respone);
                }
                if (uid == uinfo.uid)
                {
                    string info = "";
                    var acc=uaccountDal.GetByKeyId(uid, ref info);
                    if (acc.accountmony < fmoney)
                    {
                        _Respone.Message = "用户积分余额不足,谢谢支持.";
                        return Json(_Respone);
                    }
                    acc.accountmony -= fmoney;
                    if (uaccountDal.Update(acc))
                    {
                        jz_userdetailEntity jzo = new jz_userdetailEntity();
                        jzo.createtime = DateTime.Now;
                        jzo.fmoney = fmoney;
                        jzo.uid = uid;
                        jzo.remark = "用户捐赠";
                        jz_userdetailDal.Insert(jzo);
                        uchangedetailEntity ch = new uchangedetailEntity();
                        ch.remark = "用户捐赠";
                        ch.ftype = 30;
                        ch.uid = uid;
                        ch.fmoney = fmoney;
                        ch.datachange_lasttime = DateTime.Now;
                        uchangedetailDal.Inert(ch);
                        _Respone.IsSuccess = true;

                    }
                    else
                    {
                        _Respone.Message = "捐赠失败,请重试看看,谢谢支持.";
                    }
                }
            }
            catch (Exception ex)
            {
                _Respone.Message = ex.Message;
            }
            return Json(_Respone);
        }
    }
 
}