using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GooglePlusData;
using GooglePlusData.Model;
using GooglePlusData.Managers;

namespace MainApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var usr = new User();
            usr.FirstName = "a";
            usr.LastName = "b";
            usr.Username = "ab";

            new UserManager().Save(usr);
        }
    }
}
