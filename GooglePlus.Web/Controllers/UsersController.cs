using System.Web.Mvc;
using GooglePlus.Data.Contract;
using GooglePlus.Data.Model;
using GooglePlus.DataImporter;
using GooglePlus.Web.Classes;
using System;
using Spring.Context.Support;
using System.Linq;

namespace GooglePlus.Web.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        public IMembershipAdapter Membership { get; set; }
        public IGoogleDataAdapter DataAdapter { get; set; }

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
    }
}
