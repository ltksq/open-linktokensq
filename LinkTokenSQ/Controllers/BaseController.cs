using Common.Security;
using DAL;
using Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LinkTokenSQ.Controllers
{
    public class BaseController : NoLoginController
    {
        protected string GetUserid(HttpSessionStateBase session,HttpRequestBase cookierequest,HttpResponseBase cookieresponse )
        {
            if (session["loginsessionuser"] != null)
            {
                if (cookierequest.Cookies["loginsessionuser"] != null)
                {
                    HttpCookie ucookie = cookierequest.Cookies["loginsessionuser"];
                    ucookie.Expires= DateTime.Now.AddDays(2);
                    ucookie.Value= session["loginsessionuser"].ToString();
                    cookieresponse.Cookies.Add(ucookie);

                    cookierequest.Cookies["loginsessionuser"].Expires = DateTime.Now.AddDays(2);
                    cookierequest.Cookies["loginsessionuser"].Value = session["loginsessionuser"].ToString();
                    return SecurityHelp.DESDecrypt(session["loginsessionuser"].ToString());
                }
                else
                {
                    HttpCookie ucookie = new HttpCookie("loginsessionuser");
                    ucookie.Value = session["loginsessionuser"].ToString();
                    ucookie.Expires = DateTime.Now.AddDays(2);
                    cookieresponse.Cookies.Add(ucookie);
                    return SecurityHelp.DESDecrypt(session["loginsessionuser"].ToString());
                }
            }
            else
            {
                if (cookierequest.Cookies["loginsessionuser"] != null && !string.IsNullOrEmpty(cookierequest.Cookies["loginsessionuser"].Value))
                {
                    cookierequest.Cookies["loginsessionuser"].Expires = DateTime.Now.AddDays(2);
                    session["loginsessionuser"] = cookierequest.Cookies["loginsessionuser"].Value;
                    return SecurityHelp.DESDecrypt(session["loginsessionuser"].ToString());
                }
                else
                    return string.Empty;
            }
        }
        protected string GetUserid()
        {
            return this.GetUserid(this.Request.RequestContext.HttpContext.Session, this.Request.RequestContext.HttpContext.Request, this.Request.RequestContext.HttpContext.Response);
        }
        protected uinfoEntity GetUser()
        {
            uinfoEntity u = new uinfoEntity();
            string uid = this.GetUserid(this.Request.RequestContext.HttpContext.Session, this.Request.RequestContext.HttpContext.Request, this.Request.RequestContext.HttpContext.Response);
            string o_uid = uid;
            uid = uid.Split('-').Length > 1 ? uid.Split('-')[0] + uid.Split('-')[1] : uid;
            if (!string.IsNullOrEmpty(uid))
            {
                if (Common.Cache.User.GetInstance().MapingData.ContainsKey(uid))
                    u = Common.Cache.User.GetInstance().MapingData[uid];
                else
                {
                    string msg = "";
                    var ul = uinfoDal.GetListByWhere(string.Format("countrycode='{0}' and userid='{1}'", o_uid.Split('-')[0], o_uid.Split('-')[1]), ref msg);
                    if (ul != null && ul.Count > 0)
                        u = ul[0];
                }
            }
            return u;
        }
        protected void LoginOut()
        {
            this.Request.RequestContext.HttpContext.Session["loginsessionuser"] = null;
            if (this.Request.RequestContext.HttpContext.Request.Cookies["loginsessionuser"] != null)
            {
                HttpCookie ucookie = this.Request.RequestContext.HttpContext.Request.Cookies["loginsessionuser"];
                ucookie.Values.Clear();
                
                this.Request.RequestContext.HttpContext.Response.Cookies.Add(ucookie);
            }
        }
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            string uurl = requestContext.HttpContext.Request.Url.AbsolutePath.ToLower();
            if (!uurl.ToLower().EndsWith("userhelp") 
                && !uurl.ToLower().EndsWith("usernote") 
                && !uurl.ToLower().EndsWith("vote/index")
                && !uurl.ToLower().EndsWith("vote/ajaxgetvotelist")
                && uurl.ToLower().IndexOf("vote/viewvote")<0
                && !uurl.ToLower().EndsWith("vote/joinvoteitem")) {
                if (string.IsNullOrEmpty(GetUserid(requestContext.HttpContext.Session, requestContext.HttpContext.Request, requestContext.HttpContext.Response)))
                {
                    //未登录,进入登录页面
                    requestContext.HttpContext.Response.Redirect("/login" + (string.IsNullOrEmpty(requestContext.HttpContext.Request["puid"]) ? "" : "?puid=" + requestContext.HttpContext.Request["puid"]), true);
                    return;
                }
            }
            base.Initialize(requestContext);

       
        }

        #region 视图帮助相关
        /// <summary>
        /// 将视图转换成String输出
        /// </summary>
        /// <param name="viewName">视图名</param>
        /// <param name="model">Model数据</param>
        /// <returns>String输出</returns>
        protected string RenderViewToString(string viewName, object model)
        {
            Controller controller = this;
            controller.ViewData.Model = model;
            IView view = ViewEngines.Engines.FindView(controller.ControllerContext, viewName, null).View;
            using (StringWriter sw = new StringWriter())
            {
                ViewContext viewContext = new ViewContext(controller.ControllerContext, view, controller.ViewData, controller.TempData, sw);
                viewContext.View.Render(viewContext, sw);
                return sw.ToString();
            }
        }
        /// <summary>
        /// 将视图转换成String输出
        /// </summary>
        /// <param name="viewName">视图名</param>
        /// <returns>String输出</returns>
        protected string RenderViewToString(string viewName)
        {
            return this.RenderViewToString(viewName, null);
        }
        #endregion

    }
}
