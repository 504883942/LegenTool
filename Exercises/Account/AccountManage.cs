using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utils;

namespace Exercises.Account
{
    public static class CookieManage
    {
        public static HttpCookie Cookie;
        public static bool HasUserinfo() {
            return Cookie != null;
        }
        public static string GetUserinfo() {
            return CryptoUtils.DesDecrypt(Cookie.Value, CryptoUtils.KEY);
        }
 
    }
}