using WebMatrix.WebData;

namespace GooglePlus.Web.Classes
{
    internal class WebSecurityAdapter : IMembershipAdapter
    {
        public bool Login(string userName, string password, bool persistCookie = false)
        {
            return WebSecurity.Login(userName, password, persistCookie);
        }

        public void Logout()
        {
            WebSecurity.Logout();
        }

        public int GetUserId(string userName)
        {
            return WebSecurity.GetUserId(userName);
        }

        public void CreateUserAndAccount(string userName, string password)
        {
            WebSecurity.CreateUserAndAccount(userName, password);
        }
    }
}