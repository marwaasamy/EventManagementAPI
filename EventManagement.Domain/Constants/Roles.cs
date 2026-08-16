using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace EventManagement.Domain.Constants
{
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string User = "User";

        public const string Organizer = "Organizer";

        public static readonly string[] All =
       {
            Admin, User, Organizer
        };


    }
}
