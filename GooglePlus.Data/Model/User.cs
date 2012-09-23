﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GooglePlus.Data.Model
{
    public class User
    {
        public long Id { get; set; }

        public string GoogleId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Gender Gender { get; set; }
    }
}