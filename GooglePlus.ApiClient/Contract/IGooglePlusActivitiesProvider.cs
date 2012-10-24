using GooglePlus.ApiClient.Classes;

namespace GooglePlus.ApiClient.Contract
{
    public interface IGooglePlusActivitiesProvider
    {
        GooglePlusActivitiesList GetActivities(string userId);
    }
}
