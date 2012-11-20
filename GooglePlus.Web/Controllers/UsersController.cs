using System.Collections.Generic;
using System.Web.Mvc;
using GooglePlus.Data.Contract;
using GooglePlus.Data.Model;
using GooglePlus.Web.Classes;

namespace GooglePlus.Web.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        //
        // GET: /Users/
        [HttpGet]
        public ActionResult Index()
        {
            var userProvider = SpringContext.Resolve<IGoogleDataAdapter>();
            var membership = SpringContext.Resolve<IMembershipAdapter>();

            var user = userProvider.GetUserById(membership.GetUserId(User.Identity.Name));

            return View(user);
        }

        [HttpGet]
        public ActionResult List()
        {
            return View(new List<User>());
        }

        [HttpPost]
        public ActionResult Update(User user)
        {
            return View("Index");
        }
    }
}
