using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GooglePlus.ApiClient.Contract
{
    public interface IJsonServiceClient
    {
        T GetData<T>(string uri);
    }
}
