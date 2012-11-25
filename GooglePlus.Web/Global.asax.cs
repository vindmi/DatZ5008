﻿using System.Web.Mvc;
using System.Web.Routing;
using GooglePlus.Web.Classes;
using WebMatrix.WebData;
using System.Web.Optimization;
using GooglePlus.Data;

namespace GooglePlus.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            DatabaseInitializer.EnsureDatabase();

            WebSecurity.InitializeDatabaseConnection(
                "GooglePlus", "Users", "id", "username", autoCreateTables: true);

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ControllerBuilder.Current.SetControllerFactory(typeof(SpringControllerFactory));
        }
    }
}