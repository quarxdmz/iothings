using api.Controllers;
using api.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;

namespace api.NunitTest
{
    [TestFixture]
    public class UserControllerTest
    {
        HttpControllerContext controllerContext;
        UserController controller;
        [SetUp]
        public void SetUp()
        {
            controllerContext = new HttpControllerContext();

            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://localhost", null), new HttpResponse(null));

            controllerContext.Request = new HttpRequestMessage();

            controllerContext.Request.Headers.Add("Basic", "password");

            controller = new UserController();

            controller.ControllerContext = controllerContext;
        }

        [TestCase("orlysbondoc@gmail.com", false)]
        [TestCase("orlysbondoc.healthsplash@gmail.com", false)]
        [TestCase("orlysbondoc.costarica@gmail.com", false)]
        public void RegisterNewUser(string email, bool success)
        {
   
            var d = new param_user()
            {
                username = email,
                first_name = "orly",
                last_name = "bondoc",
                password = "password",
                device_token = "12456"
            };

            dynamic res = controller.Postuser(d);

            Console.WriteLine(res.Content.message);

            Assert.AreEqual(res.Content.success, success);
        }

        [TestCase(358, true)]
        public void GetUserByID(long user_id, bool success)
        {
            dynamic res = controller.Get(user_id);

            Console.WriteLine(res.Result.Content.success);

            Assert.AreEqual(res.Result.Content.success, success);
        }

        [TestCase(358, "Montong", false)]
        [TestCase(0, "Montonz", false)]
        public void UpdateUser(long user_id, string first_name, bool success)
        {
            var data = new u_user()
            {
                user_id = user_id,
                first_name = first_name
            };

            dynamic res = controller.wrupdateUser(data);

            Console.WriteLine(res.Content.message);

            Assert.AreEqual(res.Content.success, success);
        }
        
        [TestCase("orlysbondoc@gmail.com", "12345", true)]
        public void ChangePassword(string email, string password, bool success)
        {
            dynamic res = controller.getchangepassword(email, password);

            Console.WriteLine(res.Content.message);

            Assert.AreEqual(res.Content.success, success);
        }

    }
}