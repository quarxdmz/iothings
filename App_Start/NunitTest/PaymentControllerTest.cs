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
    public class PaymentControllerTest
    {
        HttpControllerContext controllerContext;
        PaymentController controller;
        [SetUp]
        public void SetUp()
        {
            controllerContext = new HttpControllerContext();

            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://localhost", null), new HttpResponse(null));

            controllerContext.Request = new HttpRequestMessage();

            controllerContext.Request.Headers.Add("Basic", "password");

            controller = new PaymentController();

            controller.ControllerContext = controllerContext;
        }

        [TestCase(1, false)]
        public void GetUserPaymentHistory(long user_id, bool success)
        {
            dynamic res = controller.get(user_id);

            Console.WriteLine(res.Content.message);

            Assert.AreEqual(res.Content.success, success);
        }
    }
}