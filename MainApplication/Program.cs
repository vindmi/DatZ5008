using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GooglePlusData;
using GooglePlusData.Model;
using GooglePlusData.Managers;
using log4net;

namespace MainApplication
{
    class Program
    {
        private static ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            Log.Debug("Main START");

            var usr = new User();
            usr.FirstName = "a";
            usr.LastName = "b";

            new UserManager().Save(usr);

            Log.Debug("Main END");
        }
    }
}
