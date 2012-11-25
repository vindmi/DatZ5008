using System.Web.Mvc;
using GooglePlus.Data.Contract;
using GooglePlus.Web.Classes;

namespace GooglePlus.Web.Controllers
{
    [Authorize]
    public class ActivitiesController : Controller
    {
        public IMembershipAdapter Membership { get; set; }
        public IGoogleDataAdapter DataAdapter { get; set; }

        public ActionResult Posts()
        {
            var posts = DataAdapter.GetPosts(Membership.GetUserId(User.Identity.Name));

            return View(posts);
        }

        public ActionResult Photos()
        {
            var photos = DataAdapter.GetPhotos(Membership.GetUserId(User.Identity.Name));

            return View(photos);
        }

        public ActionResult Shares()
        {
            var shares = DataAdapter.GetShares(Membership.GetUserId(User.Identity.Name));

            return View(shares);
        }
    }
}
