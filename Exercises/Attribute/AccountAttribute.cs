using Exercises.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Exercises.Attribute
{
    public class AccountAttribute:ActionFilterAttribute
    {
        public bool IsChecked { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            foreach (var item in HttpContext.Current.Request.Cookies) {
                var i = item;
            }
            CookieManage.Cookie = System.Web.HttpContext.Current.Request.Cookies["MYWORD"];
            if (IsChecked) {
                if (CookieManage.HasUserinfo())
                {
                    return;
                }
                else {
                    filterContext.Result = new RedirectResult("/User/Login");
                }
               
            }
        }
    }
}