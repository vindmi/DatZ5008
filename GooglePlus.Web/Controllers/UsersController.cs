using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GooglePlus.Data;
using GooglePlus.Data.Model;
using WebMatrix.WebData;

namespace GooglePlus.Web.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        //
        // GET: /Users/

        public ActionResult Index()
        {
            return View(new User { GoogleId = "fake google id" });
        }

        public ActionResult List()
        {
            return View(new List<User>());
        }
    }
}
