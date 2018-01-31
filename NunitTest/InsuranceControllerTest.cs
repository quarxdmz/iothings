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
    public class InsuranceControllerTest
    {
        HttpControllerContext controllerContext;
        InsuranceController controller;
        [SetUp]
        public void SetUp()
        {
            controllerContext = new HttpControllerContext();

            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://localhost", null), new HttpResponse(null));

            controllerContext.Request = new HttpRequestMessage();

            controllerContext.Request.Headers.Add("Basic", "password");

            controller = new InsuranceController();

            controller.ControllerContext = controllerContext;
        }

        [Test]
        public void Getcurrent()
        {
            dynamic res = controller.Getinsurance();

            Console.WriteLine(res.Content.message);

            Assert.AreEqual(res.Content.success, true);

        }
    }
}