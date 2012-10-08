using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GooglePlus.ApiClient.Contract
{
    public interface IRestServiceClient
    {
        T GetData<T>(string uri);
    }
}
