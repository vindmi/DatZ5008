using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GooglePlus.ApiClient.Classes;

namespace GooglePlus.ApiClient.Contract
{
    public interface IGooglePlusPeopleProvider
    {
        GooglePlusUser GetProfile(string userId);
    }
}
