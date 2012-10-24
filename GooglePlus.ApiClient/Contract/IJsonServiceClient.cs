
namespace GooglePlus.ApiClient.Contract
{
    public interface IRestServiceClient
    {
        T GetData<T>(string uri);
    }
}
