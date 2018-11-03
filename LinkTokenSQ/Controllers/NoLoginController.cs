using Common.Security;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LinkTokenSQ.Controllers
{
    public class NoLoginController : Controller
    {
        /// <summary>
        /// 获取真实IP
        /// </summary>
        /// <returns></returns>
        protected string GetRequesterIP()
        {
            string Result = String.Empty;
            Result = this.HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (null == Result || Result == String.Empty)
            {
                Result = this.HttpContext.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (null == Result || Result == String.Empty)
            {
                Result = this.HttpContext.Request.UserHostAddress;
            }
            if (null == Result || Result == String.Empty)
            {
                return "0.0.0.0";
            }

            return Result;
        }

        public static string GetToolDisplay(string url)
        {
            if (url=="/" || url.ToLower().IndexOf("/modifyuinfo") >= 0
                || url.ToLower().IndexOf("/login") >= 0
                || url.ToLower().IndexOf("/register") >= 0)
            {
                return "style='display:none'";
            }
            return "";
        }
        public static string GetToolCSS(string url,string curl)
        {
            if (curl == "/userinfo" && url==curl)
            {
                return "class='weui-tabbar__item weui-bar__item_on'";
            }
            else if (url.ToLower().IndexOf(curl) >= 0 && curl != "/userinfo")
            {
                return "class='weui-tabbar__item weui-bar__item_on'";
            }
            if(curl== "/userinfo/user")
            {
                if (url.ToLower().IndexOf("/accdetail") >= 0)
                {
                    return "class='weui-tabbar__item weui-bar__item_on'";
                }
            }
            return "class='weui-tabbar__item'";
        }
        
    }
}
