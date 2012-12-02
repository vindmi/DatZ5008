using System.Security.Principal;
namespace GooglePlus.Web.Classes
{
    public interface IMembershipAdapter
    {
        bool Login(string userName, string password, bool persistCookie = false);
        void Logout();
        int GetUserId(string userName);
        int GetUserId(IPrincipal user);
        void CreateUserAndAccount(string userName, string password, object additionalProperties);
    }
}
