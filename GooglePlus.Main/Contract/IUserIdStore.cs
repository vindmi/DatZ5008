using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GooglePlus.Main.Contract
{
    public interface IUserIdStore
    {
        string UserIds { get; }
    }
}
