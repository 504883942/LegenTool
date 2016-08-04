using Exercises.Account;
using Exercises.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Exercises.Controllers
{
    public class GameController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetCode(Guid uid)
        {
            JsonResult ret = new JsonResult();
            ret.Data = db.Code_t.Where(p => p.Code_ApplicationUid == uid).ToList();
            return ret;
        }
        public JsonResult PostCode(Guid uid, string code, string name)
        {
            JsonResult ret = new JsonResult();
            try
            {
                Code_t c = new Code_t();
                c.Code_Content = code;
                c.Code_Name = name;
                c.Code_UpdateTime = DateTime.Now;
                c.Code_CreateTime = c.Code_UpdateTime;
                c.Code_Author = CookieManage.GetUserinfo();
                c.Code_ApplicationUid = uid;

                db.Code_t.Add(c);
                db.SaveChanges();
                ret.Data = true;
            }
            catch
            {
                ret.Data = false;
            }
            return ret;
        }
    }
}