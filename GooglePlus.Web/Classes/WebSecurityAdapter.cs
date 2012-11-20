using WebMatrix.WebData;

namespace GooglePlus.Web.Classes
{
    internal class WebSecurityAdapter : IMembershipAdapter
    {
        public int GetUserId(string userName)
        {
            return WebSecurity.GetUserId(userName);
        }
    }
}