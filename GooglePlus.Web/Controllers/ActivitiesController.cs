using System.Web.Mvc;
using Common.Logging;
using GooglePlus.Data.Contract;
using GooglePlus.Web.Classes;
using GooglePlus.Data.Model;
using System;
using GooglePlus.DataImporter;

namespace GooglePlus.Web.Controllers
{
    [Authorize]
    public class ActivitiesController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ActivitiesController));

        public IMembershipAdapter Membership { get; set; }
        public IGoogleDataAdapter DataAdapter { get; set; }

        public ActionResult Posts(int? id)
        {
            var currentUserId = Membership.GetUserId(User);

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
            var currentUserId = Membership.GetUserId(User);

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
            var currentUserId = Membership.GetUserId(User);

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

            log.Info(String.Format("User '{0}' added some share", User.Identity.Name));

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

            log.Info(String.Format("User '{0}' added some photo", User.Identity.Name));

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

            log.Info(String.Format("User '{0}' added some post", User.Identity.Name));

            return RedirectToAction("Posts", currentUserId);
        }

        private int SaveActivity(Activity activity)
        {
            var currentUserId = Membership.GetUserId(User);

            activity.Author = DataAdapter.GetUserById(currentUserId);
            activity.Created = DateTime.Now;
            DataAdapter.SaveActivity(activity);

            //save to Redis
            var importer = (UserImportDataProcessor)SpringContext.Resolve("IUserImportDataProcessor");
            importer.AddRedisFeed(activity);

            log.Debug("Activity saved to Redis feed");

            return currentUserId;
        }

    }
}
