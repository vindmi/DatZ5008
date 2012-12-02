using System.Web.Mvc;
using GooglePlus.Data.Contract;
using GooglePlus.Data.Managers;
using GooglePlus.Data.Model;
using GooglePlus.DataImporter;
using GooglePlus.Web.Classes;
using System;
using Spring.Context.Support;
using System.Linq;
using System.Web.Configuration;
using System.Collections.Generic;
using System.Net.Sockets;

namespace GooglePlus.Web.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        public IMembershipAdapter Membership { get; set; }
        public IGoogleDataAdapter DataAdapter { get; set; }
        public RedisDataManager RedisManager { get; set; }

        [HttpGet]
        public ActionResult Main(int? id)
        {
            var currentUserId = Membership.GetUserId(User.Identity.Name);

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
            var currentUserId = Membership.GetUserId(User.Identity.Name);
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

            return RedirectToAction("Main");
        }

        [HttpGet]
        public ActionResult GetUserForm(string googleId, int userId)
        {
            var importer = (UserImportDataProcessor)SpringContext.Resolve("IUserImportDataProcessor");

            importer.ImportData(new[] { googleId });

            var user = DataAdapter.GetUserByGoogleId(googleId);

            return PartialView("_UserForm", user ?? DataAdapter.GetUserById(userId));      
        }

        [ChildActionOnly]
        public ActionResult GetUserFeedList(int userId)
        {
            ViewBag.FeedMaxCount = WebConfigurationManager.AppSettings["MaxUserFeedsInList"].ToString();
            int max = int.Parse(ViewBag.FeedMaxCount);

            RedisManager = (RedisDataManager)SpringContext.Resolve("IRedisDataManager");

            var feeds = RedisManager
                .GetFeeds(userId)
                .OrderByDescending(f => f.CreatedDate)
                .Take(max);

            return PartialView("_UserFeedList", feeds);   
        }

        [HttpGet]
        public ActionResult Subscribe(int ToUserId)
        {
            var currentUserId = Membership.GetUserId(User.Identity.Name);
            RedisManager = (RedisDataManager)SpringContext.Resolve("IRedisDataManager");

            try
            {
                RedisManager.AddSubscription(ToUserId, currentUserId);
            }
            catch (SocketException)
            {
                //log exception
            }
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult Unsubscribe(int ToUserId)
        {
            var currentUserId = Membership.GetUserId(User.Identity.Name);
            RedisManager = (RedisDataManager)SpringContext.Resolve("IRedisDataManager");

            try
            {
                RedisManager.DeleteSubscription(ToUserId, currentUserId);
            }
            catch (SocketException)
            {
                //log exception
            }
            return RedirectToAction("List");
        }

        private List<int> GetUserSubscriptions(int userId)
        {
            try
            {
                RedisManager = (RedisDataManager)SpringContext.Resolve("IRedisDataManager");

                return RedisManager.GetSubscriptions(userId);
            }
            catch (SocketException)
            {
                //log exception
                return new List<int>();
            }
        }
    }
}
