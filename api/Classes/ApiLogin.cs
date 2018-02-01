using api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.Classes
{
    public class ApiLogin
    {
        public static bool Login(string username, string password)
        {
            using (SV_db1Entities auth = new SV_db1Entities())
            {
                return auth.app_authentication.Any(user => username.Equals(username, StringComparison.OrdinalIgnoreCase)
                    && user.password == password
                );
            }
        }
    }
}