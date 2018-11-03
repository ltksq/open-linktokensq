 
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
    public class ModifyUInfoController: NoLoginController
    {
        public ActionResult Index()
        {
            uinfoEntity uinfo = new uinfoEntity();
            uinfo.uid = 0;
            uinfo.userid = "";
            string userid = TempData["reguserid"] != null ? TempData["reguserid"].ToString() : "";
             
            if (string.IsNullOrEmpty(userid))
            {
                return View(uinfo);
            }
            string msg = "";
            var ulist=uinfoDal.GetListByWhere(string.Format("userid='{0}'", userid), ref msg);
           
            if (ulist != null && ulist.Count > 0) uinfo = ulist[0];

            return View(uinfo);
        }

        [HttpPost]
        public ActionResult SetTempDataUserid(string userid)
        {
            PostResponse _Respone = new PostResponse() { IsSuccess = false };
            try
            {
                if (!string.IsNullOrEmpty(userid.Trim()))
                {
                    TempData["reguserid"] = userid;
                    _Respone.IsSuccess = true;
                    _Respone.Message = "/ModifyUInfo";
                }
            }
            catch { }
            return Json(_Respone);
        }

        [HttpPost]
        public ActionResult UInfoModify(string tel, string area, string scode, string pwd1, string pwd2,
            string wkcaddress, string nickname)
        {
             
            if (!Common.HeistMon.HMon.GetInstance().AddMon("UInfoModify_" + base.GetRequesterIP(), 10))
            {
                this.HttpContext.Response.Write("FireWall!");
                this.HttpContext.Response.End();
                return null;
            }
            PostResponse _Respone = new PostResponse() { IsSuccess = false };
            tel = tel.Trim();
            area = area.Trim();
            wkcaddress = wkcaddress.Trim();
            if (!Common.Cache.User.GetInstance().MapingData.ContainsKey(area + tel))
            {
                _Respone.IsSuccess = false;
                _Respone.Message = "手机号无注册,请先注册.";

                return Json(_Respone);
            }

            string info = "";
            string mobileno = area + tel;
            var dlist = mmsgnocDal.GetListByWhere(string.Format("mobileno='{0}'", mobileno), ref info);
            if (dlist == null || dlist.Count == 0)
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
                dlist[0].stopdatetime=DateTime.Now.AddHours(-1);
                mmsgnocDal.Update(dlist[0]);
            }
            info = "";
            var ulist = uinfoDal.GetListByWhere(string.Format("userid='{0}' and countrycode='{1}'", tel, area), ref info);
            if (!(ulist != null && ulist.Count > 0))
            {
                _Respone.IsSuccess = false;
                _Respone.Message = "手机号无注册,请先注册.";

                return Json(_Respone);
            }
            else
            {
                var uinfo = ulist[0];
                bool changewkcaddress = false;
                if (wkcaddress.Trim().Length > 0 && uinfo.wkcaddress.Trim() != wkcaddress.Trim())
                {
                    changewkcaddress = true;
                    var ulist1 = uinfoDal.GetListByWhere(string.Format("wkcaddress='{0}' ", wkcaddress), ref info);
                    if (ulist1 != null && ulist1.Count > 0)
                    {
                        _Respone.IsSuccess = false;
                        _Respone.Message = "钱包已被其他用户绑定!";

                        return Json(_Respone);
                    }
                }

                uinfoEntity ue = uinfo;
                ue.regdatetime = DateTime.Now;
                ue.nikename = nickname;
         
                ue.datachange_lasttime = DateTime.Now;

                if (changewkcaddress)
                {
                    ue.wkcaddress = wkcaddress.Trim();
                    ue.wkccheckmoney = double.Parse("0.0" + Common.SMS.SMS.random.Next(1001, 2000).ToString());
                    ue.wkccheckpass = false;
                }
                if (pwd2.Trim().Length > 0)
                    ue.changepwd = SecurityHelp.EncryptMD5(pwd2);

                if (pwd1.Trim().Length > 0)
                    ue.userpwd = SecurityHelp.EncryptMD5(pwd1);
                
                _Respone.IsSuccess = uinfoDal.Update(ue);
                if (_Respone.IsSuccess)
                {
                    _Respone.Message = "/login";
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