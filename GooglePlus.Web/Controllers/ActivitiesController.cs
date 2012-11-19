using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GooglePlus.Data.Model;

namespace GooglePlus.Web.Controllers
{
    [Authorize]
    public class ActivitiesController : Controller
    {
        //
        // GET: /Activities/

        public ActionResult Posts()
        {
            return View(new List<Post>());
        }

        public ActionResult Photos()
        {
            return View(new List<Photo>());
        }

        public ActionResult Shares()
        {
            return View(new List<Share>());
        }
    }
}
