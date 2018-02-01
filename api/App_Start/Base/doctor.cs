using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using api.Models;

namespace api.Base 
{
    public class doctor : ApiController
    {

       public static string s;

        System.Web.HttpContext httpCon = System.Web.HttpContext.Current;
        public  doctor()
        {
            string a_pass = httpCon.Request.Headers["Authorization"];
            bool b_pass = Validation.userAuth(a_pass);
            if (b_pass)
            {
                authorize = true;
            }
            else
            {
                authorize = false;
            }

        }

        public static bool authorize {get;set;}
    }
}