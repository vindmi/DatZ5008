using System.Web.Mvc;
using GooglePlus.Data.Contract;
using GooglePlus.Web.Classes;
using GooglePlus.Data.Model;
using System;

namespace GooglePlus.Web.Controllers
{
    [Authorize]
    public class ActivitiesController : Controller
    {
        public IMembershipAdapter Membership { get; set; }
        public IGoogleDataAdapter DataAdapter { get; set; }

        public ActionResult Posts(int? id)
        {
            var currentUserId = Membership.GetUserId(User.Identity.Name);

            if (!id.HasValue)
            {
                id = currentUserId;
            }

            var posts = DataAdapter.GetPosts(id.Value);

            if (id.Value == currentUserId)
            {
                return View(posts);
            }

            ViewBag.ProfileId = id.Value;

            return View("OtherPosts", posts);
        }

        public ActionResult Photos(int? id)
        {
            var currentUserId = Membership.GetUserId(User.Identity.Name);

            if (!id.HasValue)
            {
                id = currentUserId;
            }

            var photos = DataAdapter.GetPhotos(id.Value);

            if (id.Value == currentUserId)
            {
                return View(photos);
            }

            ViewBag.ProfileId = id.Value;

            return View("OtherPhotos", photos);
        }

        public ActionResult Shares(int? id)
        {
            var currentUserId = Membership.GetUserId(User.Identity.Name);

            if (!id.HasValue)
            {
                id = currentUserId;
            }

            var shares = DataAdapter.GetShares(id.Value);

            if (id.Value == currentUserId)
            {
                return View(shares);
            }

            ViewBag.ProfileId = id.Value;

            return View("OtherShares", shares);
        }

        public ActionResult CreateShare()
        {
            return View("CreateShare");
        }

        [HttpPost]
        public ActionResult CreateShare(Share share)
        {
            var currentUserId = SaveActivity(share);

            return RedirectToAction("Shares", currentUserId);
        }

        public ActionResult CreatePhoto()
        {
            return View("CreatePhoto");
        }

        [HttpPost]
        public ActionResult CreatePhoto(Photo photo)
        {
            var currentUserId = SaveActivity(photo);

            return RedirectToAction("Photos", currentUserId);
        }

        public ActionResult CreatePost()
        {
            return View("CreatePost");
        }

        [HttpPost]
        public ActionResult CreatePost(Post post)
        {
            var currentUserId = SaveActivity(post);

            return RedirectToAction("Posts", currentUserId);
        }

        private int SaveActivity(Activity activity)
        {
            var currentUserId = Membership.GetUserId(User.Identity.Name);

            activity.Author = DataAdapter.GetUserById(currentUserId);
            activity.Created = DateTime.Now;
            DataAdapter.SaveActivity(activity);

            return currentUserId;
        }
    }
}
