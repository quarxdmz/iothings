using api.Controllers;
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
    public class NotificationControllerTest
    {
        HttpControllerContext controllerContext;
        notificationController controller;
        [SetUp]
        public void SetUp()
        {
            controllerContext = new HttpControllerContext();

            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://localhost", null), new HttpResponse(null));

            controllerContext.Request = new HttpRequestMessage();

            controllerContext.Request.Headers.Add("Basic", "password");

            controller = new notificationController();

            controller.ControllerContext = controllerContext;
        }

        [TestCase("465", true)]
        [TestCase("1", false)]
        public void GetUserNotification(string user_id, bool success)
        {
            dynamic res = controller.getNotification(user_id);

            Console.WriteLine(res.Content.message);

            Assert.AreEqual(res.Content.success, success);

        }

        //[Test, Ignore("have to change the code")]
        public void AddNewNotification()
        {
            // have to change the code 
        }

        //[Test, Ignore("have to change the code")]
        public void UpdateNotifcation()
        {
            // have to change the code 
        }

    }
}