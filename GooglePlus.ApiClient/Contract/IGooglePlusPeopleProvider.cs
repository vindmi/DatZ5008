using GooglePlus.ApiClient.Classes;

namespace GooglePlus.ApiClient.Contract
{
    public interface IGooglePlusPeopleProvider
    {
        GooglePlusUser GetProfile(string userId);
    }
}
