﻿using System.Web.Mvc;
using GooglePlus.Data.Contract;
using GooglePlus.Data.Model;
using GooglePlus.Web.Classes;
using System;
using Spring.Context.Support;

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
            return View(DataAdapter.GetUsers());
        }

        [HttpPost]
        public ActionResult Update(User user)
        {          
            DataAdapter.SaveUser(user);

            return RedirectToAction("Main");
        }

        [HttpPost]
        public ActionResult Import(User user)
        {
            throw new Exception("not implemented");

            return RedirectToAction("Main");
        }
    }
}
