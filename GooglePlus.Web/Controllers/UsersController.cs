using System.Net.Sockets;
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

            return View(users);
        }

        [HttpPost]
        public ActionResult Update(User user)
        {          
            DataAdapter.SaveUser(user);

            return RedirectToAction("Main");
        }

        [HttpGet]
        public ActionResult ImportUserData(string googleId, int userId)
        {
            var importer = (UserImportDataProcessor)SpringContext.Resolve("IUserImportDataProcessor");

            var currentUser = DataAdapter.GetUserById(userId);

            importer.ImportData(googleId, currentUser);

            return PartialView("_UserForm", currentUser);      
        }

        [ChildActionOnly]
        public ActionResult GetUserFeedList(int userId)
        {
            ViewBag.FeedMaxCount = WebConfigurationManager.AppSettings["MaxUserFeedsInList"];
            int max = int.Parse(ViewBag.FeedMaxCount);

            RedisManager = (RedisDataManager)SpringContext.Resolve("IRedisDataManager");

            IEnumerable<Feed> feeds;

            try
            {
                feeds = RedisManager
                    .GetFeeds(userId)
                    .OrderByDescending(f => f.CreatedDate)
                    .Take(max);
            }
            catch (SocketException)
            {
                feeds = Enumerable.Empty<Feed>();
            }

            return PartialView("_UserFeedList", feeds);   
        }
    }
}
