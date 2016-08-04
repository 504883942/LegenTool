using Exercises.Attribute;
using Exercises.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Utils;

namespace Exercises.Controllers
{
 
        // GET: User
        public class UserController : BaseController
        {
            [AccountAttribute(IsChecked = false)]
            // GET: User
            public ActionResult Login(string username, string password, bool remember = false)
            {

         


                if (Request.HttpMethod == "GET")
                {
 
                    return View();
                }
                if (username == null || password == null)
                {
                    return View();
                }

            string encrypted_pwd = CryptoUtils.MD5(password);

            var user = db.User_t.Where(p => p.Account == username).FirstOrDefault();
                if ((user != null && string.Compare(username, user.Account, true) == 0 && string.Compare(encrypted_pwd, user.Password, true) == 0))
                {

                    Response.Cookies["MYWORD"].Value = CryptoUtils.DesEncrypt(user.Account, CryptoUtils.KEY);
                    if (remember)
                    {
                        Response.Cookies["MYWORD"].Expires = DateTime.Now.AddDays(7);
                    }
                    return Redirect("/home/index");
                }
 
                return View();
            }
            [AccountAttribute(IsChecked = false)]
            public ActionResult Register(string username, string password)
            {
                if (Request.HttpMethod == "GET")
                {
                    return View();
                }
                if (username == null || password == null)
                {
                    return View();
                }
                if (db.User_t.Where(p => p.Account == username).Count() != 0)
                {
                    return View();
                }

            db.User_t.Add(new User_t()
            {
                Account = username,
                Password = CryptoUtils.MD5(password),
                Name = username,
                Uid = Guid.NewGuid()
                });
                db.SaveChanges();
                Response.Cookies["MYWORD"].Value = CryptoUtils.DesEncrypt(username, CryptoUtils.KEY);

                return Redirect("/home/index");
            }

            public ActionResult Logout()
            {
                if (Request.Cookies["MYWORD"] != null)
                {
                    HttpCookie cookie = new HttpCookie("MYWORD");
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie);
                }
                return Redirect("/user/login");
            }
        }
    
}