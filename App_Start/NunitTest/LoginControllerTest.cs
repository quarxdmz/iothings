using api.Controllers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;

namespace api.NunitTest
{
    [TestFixture]
    public class LoginControllerTest
    {
        HttpControllerContext controllerContext;
        LoginController lc;
        [SetUp]
        public void SetUp()
        {
            controllerContext = new HttpControllerContext();

            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://localhost", null), new HttpResponse(null));

            controllerContext.Request = new HttpRequestMessage();

            controllerContext.Request.Headers.Add("Basic", "password");

            lc = new LoginController();

            lc.ControllerContext = controllerContext;
        }

        [TestCase("orlysbondoc.healthsplash@gmail.com", "password", true)]
        [TestCase("orlysbondoc.healthsplash@gmail.com", "wrong", false)]
        public void Login(string email, string password, bool success)
        {
            
            var data = new param_login()
            {
                username = email,
                password = password
            };

            dynamic res = lc.postlogin(data);

            Console.WriteLine(res.Content.message);

            Assert.AreEqual(res.Content.success, success);
        }

        [TestCase("orlysbondoc.healthsplash@gmail.com", "1234", false)]
        [TestCase("orlysbondoc.healthsplash@gmail.com", "KHO7", false)]
        public void VerifyUserAccount(string email, string verification_code, bool success)
        {

            dynamic res = lc.getverifyemail(email, verification_code);

            Console.WriteLine(res.Content.message);

            Assert.AreEqual(res.Content.success, success);

        }

        [TestCase("orlysbondoc.healthsplash@gmail.com", true)]
        [TestCase("noemail@gmail.com", false)]
        public void RequestChangePassword(string email, bool success)
        {
            dynamic res = lc.postchangepassword(email);

            Console.WriteLine(res.Content.message);

            Assert.AreEqual(res.Content.success, success);

        }

        //[TestCase("11234", "facebook", "o", "b", true), Ignore("Have to debug something!")]
        public void SocialAppLogin(string social_id, string social_type, string first_name, string last_name, bool success)
        {
            var data = new param_loginsocial()
            {
                social_id = social_id,
                social_type = social_type,
                first_name = first_name,
                last_name = last_name
            };

            dynamic res = lc.Postsocial(data);

            Console.WriteLine(res.Content.message);

            Assert.AreEqual(res.Content.success, success);
        }






    }
}