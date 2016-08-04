using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Exercises.Base
{
    public class BaseController: Controller
    {
        public Entities db = new Entities();
 
    }
}