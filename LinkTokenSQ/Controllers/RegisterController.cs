 
using Common.Security;
using DAL;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace LinkTokenSQ.Controllers
{
    public class RegisterController : NoLoginController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetTelCode(string tel, string area, string scode,int flag=0)
        {
            PostResponse _Respone = new PostResponse() { IsSuccess = false };

            try
            {
                if (MvcApplication.Syspar.ContainsKey("AllowReg"))
                {
                    if (MvcApplication.Syspar["AllowReg"].keyvalue == "0")
                    {
                        _Respone.IsSuccess = false;
                        _Respone.Message = !string.IsNullOrEmpty(MvcApplication.Syspar["AllowReg"].keydes) ? MvcApplication.Syspar["AllowReg"].keydes : "暂停注册,请关注官方通知.";
                        return Json(_Respone);
                    }
                }
                
                if (!Common.HeistMon.HMon.GetInstance().AddMon("GetTelCode_" + base.GetRequesterIP(), 20))
                {
                    _Respone.IsSuccess = true;
                    return Json(_Respone);
 
                }
                if (flag == 0)
                {
                    if (Common.Cache.User.GetInstance().MapingData.ContainsKey(area + tel))
                    {
                        _Respone.IsSuccess = false;
                        _Respone.Message = "账号已注册,请进入登录页面登录.";

                        return Json(_Respone);
                    }
                }
                
                if (TempData["SecurityCode"] != null
                   && !string.IsNullOrEmpty(scode)
                   && TempData["SecurityCode"].Equals(scode))
                {
                    string mobileno = area + tel;
                    int code = Common.SMS.SMS.random.Next(100000, 999999);
                    if (Common.SMS.SMS.SMS_sender(area == "86" ? tel : area + tel, code.ToString()))
                    {
                        Common.HeistMon.HMon.GetInstance().AddSendMSGIP("GetTelCode_" + base.GetRequesterIP());
                        string info = "";
                        var dlist = mmsgnocDal.GetListByWhere(string.Format("mobileno='{0}'", mobileno), ref info);
                        if (dlist != null && dlist.Count > 0)
                        {
                            mmsgnocEntity noc = dlist[0];
                            noc.mobileno = mobileno;
                            noc.msgcode = code.ToString();
                            noc.stopdatetime = DateTime.Now.AddMinutes(10);
                            mmsgnocDal.Update(noc);
                        }
                        else
                        {
                            mmsgnocEntity noc = new mmsgnocEntity();
                            noc.msgid = -1;
                            noc.mobileno = mobileno;
                            noc.msgcode = code.ToString();
                            noc.stopdatetime = DateTime.Now.AddMinutes(10);
                            mmsgnocDal.Insert(noc);
                        }
                    }

                    _Respone.IsSuccess = true;
                }
                else
                {
                    _Respone.IsSuccess = false;
                    _Respone.Message = "验证码有误.";
                }

            }
            catch { }
            return Json(_Respone);
        }

        [HttpPost]
        public ActionResult URegister(string tel, string area, string scode, string pwd1, string pwd2,
            string wkcaddress, string nickname,string puid)
        {
            PostResponse _Respone = new PostResponse() { IsSuccess = false };

            if (MvcApplication.Syspar.ContainsKey("AllowReg"))
            {
                if (MvcApplication.Syspar["AllowReg"].keyvalue == "0")
                {
                    _Respone.IsSuccess = false;
                    _Respone.Message = !string.IsNullOrEmpty(MvcApplication.Syspar["AllowReg"].keydes) ? MvcApplication.Syspar["AllowReg"].keydes : "暂停注册,请关注官方通知.";
                    return Json(_Respone);
                }
            }
            
            tel = tel.Trim();
            area = area.Trim();
            wkcaddress = wkcaddress.Trim();
            if (Common.Cache.User.GetInstance().MapingData.ContainsKey(area + tel))
            {
                _Respone.IsSuccess = false;
                _Respone.Message = "账号已注册,请进入登录页面登录.";

                return Json(_Respone);
            }
            if (wkcaddress.Trim().Length == 0)
            {
                _Respone.IsSuccess = false;
                _Respone.Message = "钱包地址未填.";

                return Json(_Respone);
            }
                string info = "";
            string mobileno = area + tel;
            var dlist = mmsgnocDal.GetListByWhere(string.Format("mobileno='{0}'", mobileno), ref info);
            if(dlist==null || dlist.Count == 0)
            {
                _Respone.IsSuccess = false;
                _Respone.Message = "请先获取短信验证码.";

                return Json(_Respone);
            }
            else
            {
                if (dlist[0].stopdatetime <= DateTime.Now)
                {
                    _Respone.IsSuccess = false;
                    _Respone.Message = "短信验证码已过期，请重新获取短信验证码。";

                    return Json(_Respone);
                }
                if (dlist[0].msgcode != scode)
                {
                    _Respone.IsSuccess = false;
                    _Respone.Message = "短信验证码不正确，请重新输入。";

                    return Json(_Respone);
                }
                dlist[0].stopdatetime = DateTime.Now.AddHours(-1);
                mmsgnocDal.Update(dlist[0]);
            }
            info = "";
            var ulist = uinfoDal.GetListByWhere(string.Format("userid='{0}' and countrycode='{1}'", tel, area), ref info);
            if (ulist != null && ulist.Count > 0)
            {
                _Respone.IsSuccess = false;
                _Respone.Message = "账号已注册,请进入登录页面登录.";

                return Json(_Respone);
            }
            else
            {
               
                    var ulist1 = uinfoDal.GetListByWhere(string.Format("wkcaddress='{0}' ", wkcaddress), ref info);
                    if (ulist1 != null && ulist1.Count > 0)
                    {
                        _Respone.IsSuccess = false;
                        _Respone.Message = "钱包已绑定!";

                        return Json(_Respone);
                    }
                

                uinfoEntity ue = new uinfoEntity();
                ue.regdatetime = DateTime.Now;
                ue.nikename = nickname;
                ue.userid = tel;
                ue.countrycode = area;
                ue.datachange_lasttime = DateTime.Now;
                ue.lastchecktime = DateTime.Now;
                ue.usertype = 0;
                ue.wkcaddress = wkcaddress;
                ue.wkccheckmoney = double.Parse("0.0"+Common.SMS.SMS.random.Next(1001, 2000).ToString());
                ue.wkccheckpass = false;
                ue.changepwd = pwd2.Length > 0 ? SecurityHelp.EncryptMD5(pwd2) : "";
                ue.userpwd = SecurityHelp.EncryptMD5(pwd1);
                ue.puid = int.Parse(string.IsNullOrEmpty(puid) ? "0" : puid);
                string fuid = string.Empty;
                _Respone.IsSuccess = uinfoDal.Insert(ue,ref fuid);
                if (_Respone.IsSuccess)
                {
                    this.Request.RequestContext.HttpContext.Session["loginsessionuser"] = SecurityHelp.DESEncrypt(area + "-" + tel);
                   // _Respone.Message = "/verify";
                    _Respone.Message = "/vote";
                    var ul = uinfoDal.GetListByWhere(string.Format("countrycode='{0}' and userid='{1}'", area, tel), ref info);

                    
                     
                }
                else
                {
                    _Respone.Message = "注册失败,请稍后尝试.";
                }
            }

            return Json(_Respone);
        }
    }
}