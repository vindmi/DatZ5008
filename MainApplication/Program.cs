using log4net;
using GooglePlus.Data.Managers;
using GooglePlus.Data.Model;

namespace GooglePlus.Main
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
