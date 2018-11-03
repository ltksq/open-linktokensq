using Common.Security;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LinkTokenSQ.Controllers
{
    public class LoginController : NoLoginController
    {
        public ActionResult Index()
        {
            return View();
        }

         [HttpPost]
        public ActionResult CheckLogin(string ccode,string userid,string pwd,string scode)
        {
            PostResponse _Respone = new PostResponse() { IsSuccess = false };

            try
            {
                if(!Common.Cache.User.GetInstance().MapingData.ContainsKey(ccode+userid))
                {
                    _Respone.IsSuccess = false;
                    _Respone.Message = "未注册的账号,请先进行注册.";

                    return Json(_Respone);
                }
                if(TempData["SecurityCode"]!=null
                    && !string.IsNullOrEmpty(scode) 
                    && TempData["SecurityCode"].Equals(scode))
                {
                    string info = "";
                    _Respone.IsSuccess = DAL.uinfoDal.CheckUidAndPwd(userid, SecurityHelp.EncryptMD5(pwd), ref info);
                    _Respone.Message = info;
                    if (!_Respone.IsSuccess)
                    {
                        _Respone.Message = "账号或密码有误.";
                    }
                    else
                    {
                        this.Request.RequestContext.HttpContext.Session["loginsessionuser"] = SecurityHelp.DESEncrypt(ccode + "-" + userid);
                        var u = Common.Cache.User.GetInstance().MapingData[(ccode + userid)];
                        try
                        {
                            var uu = DAL.uinfoDal.GetListByWhere("userid='" + userid + "'", ref info)[0];
                            uu.lastchecktime = DateTime.Now;
                            DAL.uinfoDal.Update(uu);
                        }
                        catch { }
                        
                        _Respone.Message = "/vote";
                    }
                }
                else
                {
                    _Respone.IsSuccess = false;
                    _Respone.Message = "验证码有误.";
                }
            }
             catch(Exception ex)
            {
                _Respone.IsSuccess = false;
                _Respone.Message = ex.Message;
            }

            return Json(_Respone);
        }
    }
}
