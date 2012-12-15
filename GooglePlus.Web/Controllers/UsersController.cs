using System.Web.Mvc;
using Common.Logging;
using GooglePlus.Data.Contract;
using GooglePlus.Data.Managers;
using GooglePlus.Data.Model;
using GooglePlus.DataImporter;
using GooglePlus.Web.Classes;
using System;
using System.Linq;
using System.Web.Configuration;
using System.Collections.Generic;

namespace GooglePlus.Web.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UsersController));

        public IMembershipAdapter Membership { get; set; }
        public IGoogleDataAdapter DataAdapter { get; set; }
        public RedisDataManager RedisManager { get; set; }

        [HttpGet]
        public ActionResult Main(int? id)
        {
            var currentUserId = Membership.GetUserId(User);

            if (!id.HasValue)
            {
                id = currentUserId;
            }

            var user = DataAdapter.GetUserById(id.Value);

            if (id.Value == currentUserId)
            {
                return View(user);
            }
            
            return View("OtherMain", user);
        }

        [HttpGet]
        public ActionResult List()
        {
            var currentUserId = Membership.GetUserId(User);
            var users = from u in DataAdapter.GetUsers()
                    where u.Id != currentUserId
                    orderby u.FirstName ascending
                    select u;

            ViewBag.subscribedToUsers = GetUserSubscriptions(currentUserId);

            return View(users);
        }

        [HttpGet]
        public ActionResult Subscriptions(int? id)
        {
            var currentUserId = Membership.GetUserId(User.Identity.Name);
            if (!id.HasValue)
            {
                id = currentUserId;
            }
            
            var userSubscriptions = GetUserSubscriptions(id.Value);
            var users = from u in DataAdapter.GetUsers()
                        where userSubscriptions.Contains(u.Id)
                        orderby u.FirstName ascending
                        select u;

            ViewBag.ProfileId = id.Value;
            return View(users);
        }

        [HttpPost]
        public ActionResult Update(User user)
        {          
            DataAdapter.SaveUser(user);

            log.Info(String.Format("User '{0}' updated profile", user.FullName));

            return RedirectToAction("Main");
        }

        [HttpGet]
        public ActionResult ImportUserData(string googleId, int userId)
        {
            var importer = (UserImportDataProcessor)SpringContext.Resolve("IUserImportDataProcessor");

            var currentUser = DataAdapter.GetUserById(userId);

            importer.ImportData(googleId, currentUser);

            log.Info(String.Format("User '{0}' imported data from Google+", User.Identity.Name));

            return PartialView("_UserForm", currentUser);      
        }

        [ChildActionOnly]
        public ActionResult GetUserFeedList(int userId)
        {
            ViewBag.FeedMaxCount = WebConfigurationManager.AppSettings["MaxUserFeedsInList"];
            int max = int.Parse(ViewBag.FeedMaxCount);

            IEnumerable<Feed> feeds;

            try
            {
                feeds = RedisManager
                    .GetFeeds(userId)
                    .OrderByDescending(f => f.CreatedDate)
                    .Take(max);
            }
            catch (Exception)
            {
                feeds = Enumerable.Empty<Feed>();
            }

            return PartialView("_UserFeedList", feeds);   
        }

        [HttpGet]
        public ActionResult Subscribe(int toUserId)
        {
            var currentUserId = Membership.GetUserId(User.Identity.Name);

            try
            {
                RedisManager.AddSubscription(toUserId, currentUserId);

                log.Info(String.Format("User '{0}' subscribed to user's '{1}' feed", User.Identity.Name, toUserId));
            }
            catch (Exception)
            {
                log.Error("Subscription to user's feeds failed");
            }
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult Unsubscribe(int toUserId)
        {
            var currentUserId = Membership.GetUserId(User.Identity.Name);

            try
            {
                RedisManager.DeleteSubscription(toUserId, currentUserId);

                log.Info(String.Format("User '{0}' unsubscribed from user's '{1}' feed", User.Identity.Name, toUserId));
            }
            catch (Exception)
            {
                log.Error("Unsubscription from user's feeds failed");
            }
            return RedirectToAction("List");
        }

       
        [ChildActionOnly]
        public ActionResult OtherUserActivitiesList(int userId)
        {
            var maxCount = WebConfigurationManager.AppSettings["MaxActivityCount"];
            int max = int.Parse(maxCount);

            var subs = GetUserSubscriptions(userId);

            var activities = new List<Activity>();
            foreach(int id in subs)
            {
                var actions = DataAdapter.GetUserActivities(id);
                activities.AddRange(actions);
            }
            var result = activities
                .OrderByDescending(ac => ac.Created)
                .Take(max);

            return PartialView("_OtherUserActivitiesList", result);  
        }

        private List<int> GetUserSubscriptions(int userId)
        {
            try
            {
                return RedisManager.GetSubscriptions(userId);
            }
            catch (Exception)
            {
                log.Error("GetUserSubscriptions failed");
            }

            return Enumerable.Empty<int>().ToList();
        }
    }
}
