using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GooglePlus.Main.Contract;

namespace GooglePlus.Main
{
    public class UserIdStore : IUserIdStore
    {
        public string UserIds { get; set; }
    }
}
