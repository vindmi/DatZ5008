using System.Web.Mvc;
using GooglePlus.Data.Contract;
using GooglePlus.Web.Classes;

namespace GooglePlus.Web.Controllers
{
    [Authorize]
    public class ActivitiesController : Controller
    {
        private IMembershipAdapter membership;
        private IGoogleDataAdapter dataAdapter;
        //
        // GET: /Activities/
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            membership = SpringContext.Resolve<IMembershipAdapter>();
            dataAdapter = SpringContext.Resolve<IGoogleDataAdapter>();
        }

        public ActionResult Posts()
        {
            var posts = dataAdapter.GetPosts(membership.GetUserId(User.Identity.Name));

            return View(posts);
        }

        public ActionResult Photos()
        {
            var photos = dataAdapter.GetPhotos(membership.GetUserId(User.Identity.Name));

            return View(photos);
        }

        public ActionResult Shares()
        {
            var shares = dataAdapter.GetShares(membership.GetUserId(User.Identity.Name));

            return View(shares);
        }
    }
}
